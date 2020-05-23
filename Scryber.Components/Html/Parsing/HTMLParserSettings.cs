using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Html.Parsing
{

    /// <summary>
    /// Settings for the HTML Parser to control how it parses the textual content
    /// </summary>
    public class HTMLParserSettings
    {
        /// <summary>
        /// The default read only known set of tags to skip over (any inner content will be ignored). Changes made to this collection will be applied to all default HTMLParserSettings
        /// </summary>
        public static readonly List<string> DefaultSkipOverTags = new List<string>(new string[] {
                                                                            "script", "style", "embed", "object", "head", "noframes", 
                                                                            "noscript", "select", "input", "var", "video", "xml", "svg", "caption" });

        /// <summary>
        /// The default set of known HTML Entities that are escpared in a file, but should be replaced by single characters in the parsed contents. Changes made to this collection will be applied to all default HTMLParserSettings
        /// </summary>
        public static readonly Dictionary<string, char> DefaultEscapedHTMLEntities; //Initialized in the static constructor

        //default skip falgs

        private const bool DefaultSkipUnknown = true;
        private const bool DefaultSkipProcessingInstructions = true;
        private const bool DefaultSkipStyles = false;
        private const bool DefaultSkipCssClasses = false;
        private const bool DefaultSkipComments = true;
        private const bool DefaultSkipDocType = true;
        private const bool DefaultSkipCData = true;


        /// <summary>
        /// Gets or sets the flag to indicate if the parser should report or ignore html Doc Type declarations
        /// </summary>
        public bool SkipDocType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flag to indicate if the parser should report or ignore html processing instructions
        /// </summary>
        public bool SkipProcessingInstructions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flag to indicate if the parser should report or ignore html style attributes and their inner values
        /// </summary>
        public bool SkipStyles
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flag to indicate if the parser should report or ignore html comments
        /// </summary>
        public bool SkipComments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flag to indicate if the parser should report or ignore html CDADA sections
        /// </summary>
        public bool SkipCData
        {
            get;
            set;
        }



        /// <summary>
        /// Gets or sets the flag to indicate if the parser should report or ignore CSS class names on HTML elements
        /// </summary>
        public bool SkipCssClasses
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the TraceLog for the parser
        /// </summary>
        public PDFTraceLog TraceLog
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the level at which any messages sould be written
        /// </summary>
        public TraceLevel LogLevel
        {
            get;
            set;
        }


        #region public bool SkipUnknownTags {get; set;}

        /// <summary>
        /// If true then any tags that are not matched (do not return a component will simply be skipped - moving onto inner content.
        /// </summary>
        public bool SkipUnknownTags
        {
            get;
            set;
        }

        #endregion

        #region public IList<string> SkipOverTags {get;set;}

        /// <summary>
        /// The list of known tags that do not represent any usable content 
        /// and should be skipped completely (including any inner content).
        /// </summary>
        public IList<string> SkipOverTags
        {
            get;
            private set;
        }

        #endregion

        #region public IDictionary<string, char> HTMLEntityMappings {get; private set;}

        /// <summary>
        /// Gets the dictionary of HTML Entities and their corresponding unicode characters
        /// </summary>
        public IDictionary<string, char> HTMLEntityMappings
        {
            get;
            private set;
        }

        #endregion

        //
        // .ctor
        //

        #region public HTMLParserSettings()

        public HTMLParserSettings() : this(null)
        { }

        #endregion

        #region public HTMLParserSettings(PDFTraceLog log)

        /// <summary>
        /// Creates a new default instance of the parser settings
        /// </summary>
        public HTMLParserSettings(PDFTraceLog log)
            : this(DefaultSkipUnknown, DefaultSkipCssClasses, DefaultSkipStyles, DefaultSkipProcessingInstructions,DefaultSkipComments,DefaultSkipDocType, DefaultSkipCData, TraceLevel.Verbose, log, new ReadOnlyList<string>(DefaultSkipOverTags), new ReadOnlyDictionary<string,char>(DefaultEscapedHTMLEntities))
        {
        }

        #endregion

        #region public HTMLParserSettings(bool skipUnknown, string[] skipOverTags, Dictionary<string,char> escapeEntites)

        /// <summary>
        /// Creates a new parser settings instance with cusomizable setting values.
        /// </summary>
        /// <param name="skipUnknown">Flag if unknown tags should be ignored</param>
        /// <param name="skipOverTags">Any known but unusable tags that should be skipped completely (including any of their inner content)</param>
        /// <param name="escapeEntites">All the escaped entities mapping HTML entities (e.g. &amp;) to the character equivalent (e.g. &)</param>
        private HTMLParserSettings(bool skipUnknown, bool skipCssClasses, bool skipStyles, bool skipProcessingInstructions, bool skipComments, bool skipDocType, bool skipCData, TraceLevel loglevel, PDFTraceLog log, IList<string> skipOverTags, IDictionary<string,char> escapeEntites)
        {
            if (null == escapeEntites)
                throw new ArgumentNullException("escapeEntities");
            if (null == skipOverTags)
                throw new ArgumentNullException("skipOverTags");

            this.SkipUnknownTags = skipUnknown;
            this.SkipOverTags = skipOverTags;
            this.SkipComments = skipComments;
            this.SkipDocType = skipDocType;
            this.SkipCData = skipCData;
            this.SkipCssClasses = skipCssClasses;
            this.SkipStyles = skipStyles;
            this.SkipProcessingInstructions = skipProcessingInstructions;
            this.HTMLEntityMappings = escapeEntites;
            this.LogLevel = loglevel;
            this.TraceLog = log;
        }

        #endregion

        //
        // ..ctor
        //

        #region static HTMLParserSettings() + InitKnownHTMLEntities()

        /// <summary>
        /// Static ctor to initialize the DefaultHTMLEscapedCharacters
        /// </summary>
        static HTMLParserSettings()
        {
            DefaultEscapedHTMLEntities = InitKnownHTMLEntities();
        }

        static Dictionary<string,char> InitKnownHTMLEntities() 
        {
            Dictionary<string, char> known = new Dictionary<string, char>();
            known.Add("&nbsp;", ' ');
            known.Add("&lt;", '<');
            known.Add("&gt;", '>');
            known.Add("&amp;", '&');
            known.Add("&quot;", '"');
            known.Add("&euro;", '€');
            known.Add("&iexcl;", '¡');
            known.Add("&cent;", '¢');
            known.Add("&pound;", '£');
            known.Add("&curren;", '¤');
            known.Add("&yen;", '¥');
            known.Add("&brvbar;", '¦');
            known.Add("&sect;", '§');
            known.Add("&uml;", '¨');
            known.Add("&copy;", '©');
            known.Add("&ordf;", 'ª');
            known.Add("&not;", '¬');
            known.Add("&reg;", '®');
            known.Add("&macr;", '¯');
            known.Add("&deg;", '°');
            known.Add("&plusmn;", '±');
            known.Add("&sup2;", '²');
            known.Add("&sup3;", '³');
            known.Add("&acute;", '´');
            known.Add("&micro;", 'µ');
            known.Add("&para;", '¶');
            known.Add("&middot;", '·');
            known.Add("&cedil;", '¸');
            known.Add("&sup1;", '¹');
            known.Add("&ordm;", 'º');
            known.Add("&frac14;", '¼');
            known.Add("&frac12;", '½');
            known.Add("&frac34;", '¾');
            known.Add("&iquest;", '¿');
            known.Add("&Agrave;", 'À');
            known.Add("&Aacute;", 'Á');
            known.Add("&Acirc;", 'Â');
            known.Add("&Atilde;", 'Ã');
            known.Add("&Auml;", 'Ä');
            known.Add("&Aring;", 'Å');
            known.Add("&AElig;", 'Æ');
            known.Add("&Ccedil;", 'Ç');
            known.Add("&Egrave;", 'È');
            known.Add("&Eacute;", 'É');
            known.Add("&Ecirc;", 'Ê');
            known.Add("&Euml;", 'Ë');
            known.Add("&Igrave;", 'Ì');
            known.Add("&Iacute;", 'Í');
            known.Add("&Icirc;", 'Î');
            known.Add("&Iuml;", 'Ï');
            known.Add("&ETH;", 'Ð');
            known.Add("&Ntilde;", 'Ñ');
            known.Add("&Ograve;", 'Ò');
            known.Add("&Oacute;", 'Ó');
            known.Add("&Ocirc;", 'Ô');
            known.Add("&Otilde;", 'Õ');
            known.Add("&Ouml;", 'Ö');
            known.Add("&times;", '×');
            known.Add("&Oslash;", 'Ø');
            known.Add("&Ugrave;", 'Ù');
            known.Add("&Uacute;", 'Ú');
            known.Add("&Ucirc;", 'Û');
            known.Add("&Uuml;", 'Ü');
            known.Add("&Yacute;", 'Ý');
            known.Add("&THORN;", 'Þ');
            known.Add("&szlig;", 'ß');
            known.Add("&agrave;", 'à');
            known.Add("&aacute;", 'á');
            known.Add("&acirc;", 'â');
            known.Add("&atilde;", 'ã');
            known.Add("&auml;", 'ä');
            known.Add("&aring;", 'å');
            known.Add("&aelig", 'æ');
            known.Add("&ccedil;", 'ç');
            known.Add("&egrave;", 'è');
            known.Add("&eacute;", 'é');
            known.Add("&ecirc;", 'ê');
            known.Add("&euml;", 'ë');
            known.Add("&igrave;", 'ì');
            known.Add("&iacute;", 'í');
            known.Add("&icirc;", 'î');
            known.Add("&iuml;", 'ï');
            known.Add("&eth;", 'ð');
            known.Add("&ntilde;", 'ñ');
            known.Add("&ograve;", 'ò');
            known.Add("&oacute;", 'ó');
            known.Add("&ocirc;", 'ô');
            known.Add("&otilde;", 'õ');
            known.Add("&ouml;", 'ö');
            known.Add("&divide;", '÷');
            known.Add("&oslash;", 'ø');
            known.Add("&ugrave;", 'ù');
            known.Add("&uacute;", 'ú');
            known.Add("&ucirc;", 'û');
            known.Add("&uuml;", 'ü');
            known.Add("&yacute;", 'ý');
            known.Add("&thorn;", 'þ');
            known.Add("&raquo;", '»');
            known.Add("&aelig;", 'æ');
            return known;
        }

        #endregion
    }


}
