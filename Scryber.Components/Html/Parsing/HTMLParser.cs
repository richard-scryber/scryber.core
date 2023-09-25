using System;
using System.IO;
using System.Text;
using System.Xml;
using Scryber.Generation;
using HtmlAgilityPack;

namespace Scryber.Html.Parsing
{

    /// <summary>
    /// Implements the IParserFactory for the HTMLParser
    /// </summary>
    public class HTMLParserFactory : IParserFactory
    {
        /// <summary>
        /// Gets the supported mime-types for this parser (Html)
        /// </summary>
        public MimeType[] SupportedTypes { get; }

        /// <summary>
        /// Creates a new instance of the HTMLParserFactory
        /// </summary>
        public HTMLParserFactory()
        {
            this.SupportedTypes = new MimeType[] { MimeType.Html };
        }

        /// <summary>
        /// Public method for creating a new IComponentParser instance from the factory
        /// </summary>
        /// <param name="forType">The mime-type (must match the supported type(s))</param>
        /// <param name="settings">The parser settings to use</param>
        /// <returns>A new IComponentParser for the requested mime-type</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the mine-type is not supported.</exception>
        public IComponentParser CreateParser(MimeType forType, ParserSettings settings)
        {
            if (!forType.IsValid || !forType.Equals(MimeType.Html))
                throw new ArgumentOutOfRangeException("The html fragment parser factroy can only create parsers for the '" + MimeType.Html.ToString() + "' type of content ");

            return new HTMLParser(settings);
        }
    }



    /// <summary>
    /// Implements the parsing of non-formal html content from streams and readers that can
    /// either be fragments of HTML (<see cref="ParseSourceType.Template" />) or complete HTML documents.
    /// The parser can handle unclosed tags and mixed XML content and paths, along with html entities.
    /// </summary>
    /// <remarks>The content to be passed is converted to an XHTML content by HTMLAgilityPack,
    /// with a few tweaks for processing and namespaces.
    /// Then read as usual by the <see cref="XMLParser"/> implementation that can be
    /// passed, or will be created.</remarks>
	public class HTMLParser : IComponentParser
    {

        #region public object RootComponent {get;set;}

        /// <summary>
        /// Gets or sets the root component for this parser (transfers to the inner XMLParser).
        /// </summary>
        public object RootComponent
        {
            get { return this.XmlParser.RootComponent; }
            set { this.XmlParser.RootComponent = value; }
        }

        #endregion

        #region public ParserSettings Settings {get; private set;}

        /// <summary>
        /// Gets the Settings associated with this parser (set from the constructor)
        /// </summary>
        public ParserSettings Settings
        {
            get;
            private set;
        }

        #endregion

        #region public XMLParser XmlParser {get; private set;}

        /// <summary>
        /// Gets the inner XmlParser associated with this html parser (set from the constructor)
        /// </summary>
        public XMLParser XmlParser
        {
            get;
            private set;
        }

        #endregion

        //
        // .ctor
        //

        #region public HTMLParser(ParserSettings settings)

        /// <summary>
        /// Creates a new HTMLContentParser with an inner XMLParser both using the settings provided.
        /// </summary>
        /// <param name="settings"></param>
        public HTMLParser(ParserSettings settings)
            : this(settings , new XMLParser(settings))
        {
        }

        #endregion

        #region public HTMLParser(ParserSettings settings, XMLParser xMLParser)

        /// <summary>
        /// Creates a new HTMLContentParser with the specified XMLParser and the settings provided.
        /// </summary>
        /// <param name="settings">The parser settings to use</param>
        /// <param name="xMLParser">The xml parser that will parse the content once the HTMLParser has </param>
        /// <exception cref="ArgumentNullException"></exception>
        public HTMLParser(ParserSettings settings, XMLParser xMLParser)
        {
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.XmlParser = xMLParser ?? throw new ArgumentNullException(nameof(xMLParser));
        }

        #endregion

        #region public IComponent Parse(string source, Stream stream, ParseSourceType type)

        /// <summary>
        /// Parses a general stream of html content into a new Component instance.
        /// If the <paramref name="type"/> is <see cref="ParseSourceType.Template"/>,
        /// then it is wrapped in a <see cref="Scryber.Html.Components.HTMLFragmentWrapper" />
        /// so that multiple inner components can be parsed, of any known type and returned.
        /// If it is any other <paramref name="type"/>, then it must be a complete html document. 
        /// </summary>
        /// <param name="source">The original source path - can be null or empty</param>
        /// <param name="stream">The stream of content</param>
        /// <param name="type">The <see cref="ParseSourceType"/> that should be Template for any mixed content, otherwise a full document is expected.</param>
        /// <returns>The parsed component</returns>
        public IComponent Parse(string source, Stream stream, ParseSourceType type)
        {
            using (var sr = new StreamReader(stream))
            {
                return Parse(source, sr, type);
            }
        }

        #endregion

        #region public IComponent Parse(string source, TextReader reader, ParseSourceType type)

        /// <summary>
        /// Parses a general text reader of html content into a new Component instance.
        /// If the <paramref name="type"/> is <see cref="ParseSourceType.Template"/>,
        /// then it can be any valid html and is wrapped in a <see cref="Scryber.Html.Components.HTMLFragmentWrapper" />
        /// so that multiple inner components can be parsed, and returned.
        /// If it is any other <paramref name="type"/>, then it must be a complete html document. 
        /// </summary>
        /// <param name="source">The original source path, so relative resources (images etc.) can be found.</param>
        /// <param name="reader">The reader to read the content from, positioned as the start of the content to read</param>
        /// <param name="type">The <see cref="ParseSourceType"/> that should be Template for any mixed content, otherwise a full document is expected.</param>
        /// <returns>The parsed component</returns>
        public IComponent Parse(string source, TextReader reader, ParseSourceType type)
        {
            //Use HTMLAgilityPack to load the html into an effective XML document
            var hag = new HtmlDocument();
            hag.Load(reader);
            var root = hag.DocumentNode;
            hag.OptionOutputAsXml = true;
            hag.OptionOutputOriginalCase = false;
            hag.OptionOutputUpperCase = false;
            hag.OptionWriteEmptyNodes = true;
            hag.OptionPreserveXmlNamespaces = true;

            string content = string.Empty;

            if (type == ParseSourceType.Template)
            {
                //This is a fragment - so wrap it up and then save to a string

                root = this.WrapContentInFragment(root);
                
                StringBuilder sb = new StringBuilder();
                using (var sw = new StringWriter(sb))
                {
                    hag.Save(sw);
                }

                content = sb.ToString();

            }
            else
            {
                //This is a document so get the processing and html content (fix the CDATA title)

                var processing = ExtractProcessingInstructions(root);

                content = ExtractHTMLContent(root);

                content = ReplaceCDATATitle(content);
                content = ReplaceCDATAStyle(content);

                if (!string.IsNullOrEmpty(processing))
                    content = processing + "\r\n" + content;
            }

            //HAG will replace &quot; with &amp;quot; - so we can swap these back, as the XmHtmlEntityReader handles them.

            content = ReplaceEscapedEntities(content);
            content = ReplaceEscapedBindingExpressions(content);

            //Console.WriteLine("Extracted HTML content : " + content);

            IComponent parsed;

            using (var sr = new StringReader(content))
            {
                using (var xr = new XmlHtmlEntityReader(sr))
                {
                    parsed = Parse(source, xr, type);
                }
            }

            return parsed;
        }



        #endregion

        #region public IComponent Parse(string source, XmlReader reader, ParseSourceType type)

        /// <summary>
        /// Parses the content as valid XML, simply by using the inner XmlParser to do it.
        /// </summary>
        /// <param name="source">The original source path, so relative resources (images etc.) can be found.</param>
        /// <param name="reader">The reader to read the content from, positioned as the start of the content to read</param>
        /// <param name="type">The <see cref="ParseSourceType"/> that should be Template for any mixed content, otherwise a full document is expected.</param>
        /// <returns>The component that has been parsed.</returns>
        public IComponent Parse(string source, XmlReader reader, ParseSourceType type)
        {
            return this.XmlParser.Parse(source, reader, type);
        }

        #endregion

        //
        // support methods
        //


        #region protected virtual HtmlNode WrapContentInFragment(HtmlNode root)

        private static readonly string FragmentElement = "<fragment xmlns='http://www.w3.org/1999/xhtml' ></fragment>";

        /// <summary>
        /// Puts all the html content in the root inside a new fragment that can be parsed
        /// </summary>
        /// <param name="root">The top level node of a HtmlAgilityPack document</param>
        /// <returns>The root node with the child nodes now in the wrapper</returns>
        protected virtual HtmlNode WrapContentInFragment(HtmlNode root)
        {
            int count = root.ChildNodes.Count + 1;

            //Create and add the wrapper to the top of the document

            var wrapper = HtmlNode.CreateNode(FragmentElement);
            root.ChildNodes.Insert(0, wrapper);

            //Take all of the children after the wrapper and move them to inside.

            for (var i = 1; i < count; i++)
            {
                var toSwap = root.ChildNodes[1]; //This is now the first node - so number 1
                wrapper.MoveChild(toSwap);
            }

            return root;
        }

        #endregion

        #region public virtual string ReplaceEscapedEntities(string content)

        /// <summary>
        /// Matches the escaped HTML entities (&amp;nbsp; is converted to &amp;amp;nbsp;).
        /// Regex is thread-safe so can be kept static.
        /// </summary>
        private static readonly System.Text.RegularExpressions.Regex restoreHtmlEntities =
            new System.Text.RegularExpressions.Regex("&amp;([^ &]{1,8});");


        /// <summary>
        /// As the HTML agility pack escapes the html entities in XML (e.g. &amp;quot; goes to &amp;amp;quot;)
        /// this method returns them all back to &amp;quot; so they can be used later in the XHtml parsing
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <remarks>The HtmlAgilityPack will convert &amp;nbsp; to &amp;amp;nbsp; if output as well formed XML,
        /// and we want to convert them back to &amp;nbsp; so they are recognised by the XmlReader as entities
        /// </remarks>
        public virtual string ReplaceEscapedEntities(string content)
        {
            var restored = restoreHtmlEntities.Replace(content, (input) => {
                //first group is full string
                //second should be the characters between the &amp; and trailing ;
                var val = "&" + input.Groups[1].Value + ";";
                return val;
            });

            return restored;
        }

        #endregion

        //TODO: Convert to a single regex.

        /// <summary>
        /// Matches the escaped HTML entities (&amp;nbsp; is converted to &amp;amp;nbsp;).
        /// Regex is thread-safe so can be kept static.
        /// </summary>
        private static readonly System.Text.RegularExpressions.Regex captureBindings =
            new System.Text.RegularExpressions.Regex("{{([^><]*)}}");


        private static readonly System.Text.RegularExpressions.Regex captureEscapedContent
            = new System.Text.RegularExpressions.Regex("&(\\w){1,8};");

        public virtual string ReplaceEscapedBindingExpressions(string content)
        {
            var restored = captureBindings.Replace(content, (input) =>
            {
                var val = input.Groups[0].Value;

                var replaced = captureEscapedContent.Replace(val, (matched) => {
                    string result;
                    switch (matched.Value)
                    {
                        case ("&gt;"):
                            result = ">";
                            break;
                        //Don't do < as it will be done later on in the binding expression parser
                        //case ("&lt;"):
                        //    result = "<";
                        //    break;
                        case ("&quot;"):
                            result = "\"";
                            break;
                        case ("&apos;"):
                            result = "'";
                            break;
                        default:
                            result = matched.Value;
                            break;
                    }
                    return result;
                });

                return replaced;
            });

            return restored;
        }

        #region public virtual string ReplaceCDATATitle(string content)

        /// <summary>
        /// Matches the &lt;title&gt;(Anything)&lt;/title&gt;
        /// </summary>
        private static readonly System.Text.RegularExpressions.Regex matchTitle =
            new System.Text.RegularExpressions.Regex("<title>([\\s\\S]*)<\\/title>", System.Text.RegularExpressions.RegexOptions.Multiline);

        /// <summary>
        /// As HTML Agility Pack wraps the title content in CDATA we can strip it out afterwads
        /// </summary>
        /// <param name="content">The html content to find the CDATA title in.</param>
        /// <returns>The content with the //&lt;[CDATA[ ... //]]&gt;// removed</returns>
        public virtual string ReplaceCDATATitle(string content)
        {
            content = matchTitle.Replace(content, (match) => {
                if (match.Groups.Count > 1)
                {
                    //Get the bit, inside
                    var middle = match.Groups[1].Value.Trim();

                    middle = middle.Replace("//<![CDATA[", "");
                    middle = middle.Replace("//]]>//", "");
                    return "<title>" + middle.Trim() + "</title>";
                }
                else
                    return match.Groups[0].Value;
            }, 1);

            return content;
        }

        #endregion

        #region public virtual string ReplaceCDATATitle(string content)

        /// <summary>
        /// Matches the &lt;title&gt;(Anything)&lt;/title&gt;
        /// </summary>
        private static readonly System.Text.RegularExpressions.Regex matchStyle =
            new System.Text.RegularExpressions.Regex("<style([\\s\\S]*)<\\/style>", System.Text.RegularExpressions.RegexOptions.Multiline);

        /// <summary>
        /// As HTML Agility Pack wraps the title content in CDATA we can strip it out afterwads
        /// </summary>
        /// <param name="content">The html content to find the CDATA title in.</param>
        /// <returns>The content with the //&lt;[CDATA[ ... //]]&gt;// removed</returns>
        public virtual string ReplaceCDATAStyle(string content)
        {
            content = matchStyle.Replace(content, (match) => {
                if (match.Groups.Count > 1)
                {
                    //Get the bit, inside
                    var middle = match.Groups[1].Value.Trim();

                    middle = middle.Replace("//<![CDATA[", "");
                    middle = middle.Replace("//]]>//", "");
                    return "<style " + middle.Trim() + "</style>";
                }
                else
                    return match.Groups[0].Value;
            }, 1);

            return content;
        }

        #endregion

        #region protected virtual string ExtractProcessingInstructions(HtmlNode root)

        /// <summary>
        /// Gets the scryber processing instruction from the root node children, before it is converted to a comment in the saving
        /// </summary>
        /// <param name="root">The top level documnet node</param>
        /// <returns>The scryber processing instruction, if it is the very first node</returns>
        protected virtual string ExtractProcessingInstructions(HtmlNode root)
        {
            var processing = string.Empty;
            if(root.ChildNodes.Count > 1)
            {

                foreach (var node in root.ChildNodes)
                {
                    if (node.NodeType == HtmlNodeType.Element)
                        break; //we are starting the actual document - so go no further

                    else if (node.NodeType == HtmlNodeType.Comment && node.InnerHtml.StartsWith("<?scryber"))
                        processing = node.InnerHtml;
                }
            }

            return processing;
        }

        #endregion

        #region protected virtual string ExtractHTMLContent(HtmlNode root)

        /// <summary>
        /// Gets the complete &lt;html&gt; .... &lt;/html&gt; content from the root node, adding the xhtml namespace to the the html node.
        /// </summary>
        /// <param name="root">The root of the document to get the html content from</param>
        /// <returns>The complete &lt;html&gt; .... &lt;/html&gt; content as a string.</returns>
        /// <exception cref="PDFParserException">Thrown if the root node does not contain an &lt;html&gt; node.</exception>
        protected virtual string ExtractHTMLContent(HtmlNode root)
        {
            var html = root.SelectSingleNode("//html");

            if (null == html)
                throw new PDFParserException("The root node of the html file should be 'html' unless the ParseSouerceType is a Template");

            if (html.Attributes.Contains("xmlns"))
                html.Attributes.Remove("xmlns");

            var document = html.OwnerDocument;
            var xmlns = document.CreateAttribute("xmlns", "http://www.w3.org/1999/xhtml");

            html.Attributes.Add(xmlns);

            return html.OuterHtml;
        }

        #endregion
    }
}

