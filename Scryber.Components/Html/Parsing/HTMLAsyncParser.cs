using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Scryber.Generation;

namespace Scryber.Html.Parsing;

public class HTMLAsyncParser : HTMLParser, IComponentParserAsync
{
    
    protected XMLAsyncParser InnerAsyncParser { get; set; }

    public HTMLAsyncParser(ParserSettings settings) : this(settings, new XMLAsyncParser(settings))
    {
    }
    
    public HTMLAsyncParser(ParserSettings settings, XMLAsyncParser xmlParser)
        : base(settings, xmlParser) 
    {
        this.InnerAsyncParser = xmlParser ?? throw new ArgumentNullException(nameof(xmlParser));
    }
    
    
        #region public Task<IPDFComponent> ParseAsync(string source, Stream stream, bool istemplate) + 2 overloads

        /// <summary>
        /// Parses a stream (with the source value set to an appropriate value)
        /// </summary>
        /// <param name="source">An identifier (usually a file path or unique id) that can be recognised further into the execution by any reference resolver</param>
        /// <param name="stream">The stream of data to parse</param>
        /// <param name="istemplate">True if this stream is a template content (rather than a complete standalone file)</param>
        /// <returns>The parsed component</returns>
        public async Task<IComponent> ParseAsync(string source, Stream stream, ParseSourceType type)
        {
            using (var sr = new StreamReader(stream))
            {
                return await ParseAsync(source, sr, type);
            }
        }

        /// <summary>
        /// Parses a text reader (with the source value set to an appropriate value)
        /// </summary>
        /// <param name="source">An identifier (usually a file path or unique id) that can be recognised further into the execution by any reference resolver</param>
        /// <param name="reader">The TextReader of information to parse</param>
        /// <param name="istemplate">True if this stream is a template content (rather than a complete standalone file)</param>
        /// <returns>The parsed component</returns>
        public async Task<IComponent> ParseAsync(string source, TextReader reader, ParseSourceType type)
        {
            var content = ConvertToXHTML(reader, type);
            IComponent parsed;
            
            using (var sr = new StringReader(content))
            {
                using (var xmlReader = CreateXmlReader(sr))
                {
                    parsed = await ParseAsync(source, xmlReader, type);
                }
            }
            
            return parsed;
        }

        /// <summary>
        /// Parses an XML reader (with the source value set to an appropriate value)
        /// </summary>
        /// <param name="source">An identifier (usually a file path or unique id) that can be recognised further into the execution by any reference resolver</param>
        /// <param name="stream">The stream of data to parse</param>
        /// <param name="istemplate">True if this stream is a template content (rather than a complete standalone file)</param>
        /// <returns></returns>
        /// <returns></returns>
        public async Task<IComponent> ParseAsync(string source, XmlReader reader, ParseSourceType type)
        {
            return await this.InnerAsyncParser.ParseAsync(source, reader, type);
        }

        #endregion
}