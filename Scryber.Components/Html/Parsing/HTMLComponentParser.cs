using System;
using System.IO;
using System.Text;
using System.Xml;
using Scryber.Generation;
using HtmlAgilityPack;

namespace Scryber.Html.Parsing
{

    public class HTMLParserFactory : IParserFactory
    {
        public MimeType[] SupportedTypes { get; }

        public HTMLParserFactory()
        {
            this.SupportedTypes = new MimeType[] { MimeType.Html };
        }

        public IComponentParser CreateParser(MimeType forType, ParserSettings settings)
        {
            if (!forType.IsValid || !forType.Equals(MimeType.Html))
                throw new ArgumentOutOfRangeException("The html fragment parser factroy can only create parsers for the '" + MimeType.Html.ToString() + "' type of content ");

            return new HTMLContentParser(settings);
        }
    }

    /// <summary>
    /// Implements the parsing of non-formal html content
    /// </summary>
	public class HTMLContentParser : IComponentParser
    {
        public object RootComponent { get; set; }

        public ParserSettings Settings {
            get;
            private set;
        }

        public Scryber.Generation.XMLParser XmlParser { get; set; }

        public HTMLContentParser(ParserSettings settings)
        {
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.XmlParser = new Scryber.Generation.XMLParser(settings);
        }

        public IComponent Parse(string source, Stream stream, ParseSourceType type)
        {
            using (var sr = new StreamReader(stream))
                return Parse(source, sr, type);
        }


        /// <summary>
        /// Matches the escaped HTML entities (&nbsp; is converted to &amp;nbsp;).
        /// Regex is thread-safe so can be kept static.
        /// </summary>
        private static System.Text.RegularExpressions.Regex restoreHtmlEntities =
            new System.Text.RegularExpressions.Regex("&amp;([^ ]{1,8});");

        /// <summary>
        /// As teh HTML agility pack escapes the html entities in XML (e.g.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <remarks>The HtmlAgilityPack will convert &nbsp; to &amp;nbsp; if output as well formed XML,
        /// and we want to convert them back to &nbsp; so they are recognised by the XmlReader as entities
        /// </remarks>
        public string ReplaceEscapedEntities(string content)
        {
            var restored = restoreHtmlEntities.Replace(content, (input) => {
                //first group is full string
                //second should be the characters between the &amp; and trailing ;
                var val = "&" + input.Groups[1].Value + ";";
                return val;
            });

            return restored;
        }


        public IComponent Parse(string source, TextReader reader, ParseSourceType type)
        {
            

            //var content = reader.ReadToEnd();
            var hag = new HtmlAgilityPack.HtmlDocument();
            hag.Load(reader);
            var root = hag.DocumentNode;
            hag.OptionOutputAsXml = true;


            if (type == ParseSourceType.Template)
            {
                //This is a fragment
                if (root.ChildNodes.Count > 1)
                {
                    int count = root.ChildNodes.Count + 1;
                    var wrapper = HtmlNode.CreateNode("<fragment xmlns='http://www.w3.org/1999/xhtml' ></fragment>");
                    root.ChildNodes.Insert(0, wrapper);

                    //Take all of the children and move them to inside the wrapper
                    for (var i = 1; i < count; i++)
                    {
                        var toSwap = root.ChildNodes[1]; //This is now the first node - so number 1
                        wrapper.MoveChild(toSwap);
                    }

                }
            }
            else
            {
                var html = root.SelectSingleNode("//html");

            }

            StringBuilder sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                hag.Save(sw);
            }
            var content = sb.ToString();
            content = ReplaceEscapedEntities(content);

            using (var sr = new StringReader(content))
            {
                using (var xr = new XmlHtmlEntityReader(sr))
                {
                    return Parse(source, xr, type);
                }
            }
        }

        public IComponent Parse(string source, XmlReader reader, ParseSourceType type)
        {
            return this.XmlParser.Parse(source, reader, type);
        }
    }
}

