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
        public PDFGradientFunction()
        {
        }

        public abstract void WriteFunctionDictionary(PDFContextBase context, PDFWriter writer);
    }

}
