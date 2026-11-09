using System;
using System.Collections.Generic;
using Scryber.PDF;


namespace Scryber.PDF.Graphics
{
    /// <summary>
    /// Base abstract class for all Gradient functions.
    /// </summary>
    public abstract class PDFGradientFunction
    {
        
        public double DomainStart { get; set; }

        public double DomainEnd { get; set; }
        
        public PDFGradientFunction()
        {
            DomainStart = 0;
            DomainEnd = 1;
        }

        public abstract void WriteFunctionDictionary(ContextBase context, PDFWriter writer);
    }

}
