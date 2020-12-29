using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles
{
    
    [PDFParsableComponent("Css")]
    public class StylesCssDocument : StyleBase
    {
        

        [PDFAttribute("source")]
        public string Source { get; set; }

        public StylesCssDocument() : base((PDFObjectType)"Scss")
        {

        }
    }
}
