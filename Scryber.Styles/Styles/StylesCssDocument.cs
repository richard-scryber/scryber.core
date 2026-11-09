using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles
{
    
    [PDFParsableComponent("Css")]
    public class StylesCssDocument : StyleBase
    {
        
        /// <summary>
        /// THe local or remote style sheet source. This can be a local file path, a URL, or a string containing the CSS content itself. The StylesCssDocument will use this source to load and apply the CSS styles to the document. If the source is a URL or a file path, the StylesCssDocument will attempt to fetch and parse the CSS from that location. If the source is a string containing CSS content, it will parse that content directly.
        /// </summary>
        [PDFAttribute("source")]
        public string Source { get; set; }


        /// <summary>
        /// Creates a new instance of the StylesCssDocument class with the provided source string. The source string should contain the CSS content that you want to apply to the document. This constructor allows you to initialize the StylesCssDocument with specific CSS rules directly from a string input.
        /// </summary>
        /// <param name="source"></param>
        public StylesCssDocument(string source)
            : this()
        {
            this.Source = source;
        }

        public StylesCssDocument() : base((ObjectType)"Scss")
        {

        }
    }
}
