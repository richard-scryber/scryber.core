using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles.Parsing;

namespace Scryber.Html.Parsing
{
    public class HTMLParser : IHtmlContentParser, IEnumerable<HTMLParserResult>
    {
        private const string LogCategory = "HTML Parser";

        #region ivars

        private string _text;
        private IParserComponentFactory _instanceFact;
        private IParserStyleFactory _styleFact;
        private HTMLParserSettings _settings;
        private bool _shouldlog;

        #endregion

        //
        // properties
        //

        #region public IParserComponentFactory ComponentFactory {get;}

        /// <summary>
        /// Gets the component factory associated with this parser.
        /// </summary>
        public IParserComponentFactory ComponentFactory
        {
            get
            {
                return _instanceFact;
            }
        }

        #endregion

        #region public IParserStyleFactory StyleFactory {get;}

        /// <summary>
        /// Gets the style factory for this parser
        /// </summary>
        public IParserStyleFactory StyleFactory
        {
            get
            {
                return _styleFact;
            }
        }

        #endregion

        #region public string Source {get;}

        /// <summary>
        /// Gets the text this parser will be parsing. 
        /// </summary>
        public string Source
        {
            get
            {
                return _text;
            }
        }

        #endregion

        #region public HTMLParserSettings Settings {get;}

        /// <summary>
        /// Returns the settings for this parsers
        /// </summary>
        public HTMLParserSettings Settings
        {
            get { return _settings; }
        }

        #endregion

        public PDFTraceLog Log
        {
            get { return this._settings.TraceLog; }
        }
        //
        // .ctor
        //

        #region public HTMLParser(string text)

        /// <summary>
        /// Creates a new instance of the HTMLParser that will convert the specified text into a series of Components in a ParserResult
        /// </summary>
        /// <param name="text"></param>
        public HTMLParser(string text)
            : this(text, null, null, null)
        {
        }

        #endregion

        #region public HTMLParser(string text, HTMLParserSettings settings)

        /// <summary>
        /// Creates a new instance of the HTMLParser that will convert the specified text into a series of Components in a ParserResult,
        /// using the provided settings
        /// </summary>
        /// <param name="text"></param>
        public HTMLParser(string text, HTMLParserSettings settings)
            : this(text, null, null, settings)
        {
        }

        #endregion

        #region public HTMLParser(string text, IParserComponentFactory factory, HTMLParserSettings settings)

        /// <summary>
        /// Creates a new instance of an HTMLParser that will convert the specified text into a series of Components using the component factory.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="componentFactory">The component factory (can be null)</param>
        /// <param name="styleFactory">The style value factory (can be null)</param>
        /// <param name="settings">The parser settings (can be null)</param>
        public HTMLParser(string text, IParserComponentFactory componentFactory, IParserStyleFactory styleFactory, HTMLParserSettings settings)
        {
            this.Init(text, componentFactory, styleFactory, settings);
        }

        #endregion

        //
        // implementation methods
        //

        #region protected virtual void Init(string text, IParserComponentFactory factory)

        /// <summary>
        /// Initializes this instance with the text and factory. Inheritors can override
        /// </summary>
        /// <param name="text"></param>
        /// <param name="factory"></param>
        protected virtual void Init(string text, IParserComponentFactory factory, IParserStyleFactory styles, HTMLParserSettings settings)
        {
            if (null == factory)
                factory = new HTMLParserComponentFactory();

            if (null == styles)
                styles = new CSSStyleItemAllParser();

            if (null == settings)
                settings = new HTMLParserSettings();

            _text = text;
            _instanceFact = factory;
            _settings = settings;
            _styleFact = styles;

            _shouldlog = null != _settings.TraceLog && _settings.TraceLog.ShouldLog(_settings.LogLevel);
        }

        #endregion

        //
        // IEnumerable Interface of the IContentParser interface
        //

        #region public virtual IEnumerator<ParserResult> GetEnumerator() + IEnumerable.GetEnumerator()

        /// <summary>
        /// Returns an enumerator for this Parser that will enumerate over each of the componentes parsed from the text in this Parser.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<HTMLParserResult> GetEnumerator()
        {
            return new HTMLParserEnumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        //
        // Logging
        //

        #region public bool IsLogging {get;}

        /// <summary>
        /// Returns true if this parser is logging messages to the trace.
        /// </summary>
        public bool IsLogging
        {
            get { return _shouldlog; }
        }

        #endregion

    }

}
