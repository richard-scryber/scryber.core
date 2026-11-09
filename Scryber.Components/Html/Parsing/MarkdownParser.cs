using System;
using System.IO;
using System.Xml;
using Scryber.Components;
using Scryber.Generation;


namespace Scryber.Html.Parsing
{
    
    

    public class MarkdownParserFactory : IParserFactory
    {
        private readonly object _threadlock = new object();
        
        private Markdown _implementation = null;
        private HTMLParserFactory _parser = null;
        
        /// <summary>
        /// Gets the supported mime-types for this parser (Html)
        /// </summary>
        public MimeType[] SupportedTypes { get; }

        /// <summary>
        /// Creates a new instance of the HTMLParserFactory
        /// </summary>
        public MarkdownParserFactory(HTMLParserFactory htmlParser)
        {
            this.SupportedTypes = [MimeType.XMarkdown, MimeType.Markdown];
            this._parser = htmlParser ?? throw new ArgumentNullException(nameof(htmlParser));
        }

        internal Markdown GetMarkdownImplementation()
        {
            lock (_threadlock)
            {
                if(null == _implementation)
                    _implementation = new Markdown();
                return _implementation;
            }
        }

        public IComponentParser CreateParser(MimeType forType, ParserSettings settings)
        {
            return new MarkdownParser(settings, this,  _parser.CreateParser(MimeType.Html, settings));
        }
    }
    
    
    
    public class MarkdownParser : IComponentParser
    {
        private object _rootComponent;

        public object RootComponent
        {
            get => _rootComponent;
            set => _rootComponent = value;
        }
        
        public ParserSettings Settings { get; private set; }
        
        public MarkdownParserFactory Factory { get; set; }
        
        public IComponentParser ContentParser { get; private set; }

        public MarkdownParser(ParserSettings settings, MarkdownParserFactory factory, IComponentParser htmlParser)
        {
            this.Settings = settings;
            this.Factory = factory;
            this.ContentParser = htmlParser;
        }

        public IComponent Parse(string source, Stream stream, ParseSourceType type)
        {
            using (var reader = new StreamReader(stream))
            {
                return Parse(source, reader, type);
            }
        }

        public IComponent Parse(string source, TextReader reader, ParseSourceType type)
        {
            var implementation = this.Factory.GetMarkdownImplementation();
            var content = reader.ReadToEnd();
            var html = implementation.Transform(content);

            using (var strReader = new StringReader(html))
            {
                var parsed = this.ContentParser.Parse(source, strReader, type);
                return parsed;
            }
            
        }

        public IComponent Parse(string source, XmlReader reader, ParseSourceType type)
        {
            throw new NotImplementedException("The markdown parser does not support reading from XML");
        }
    }
}
