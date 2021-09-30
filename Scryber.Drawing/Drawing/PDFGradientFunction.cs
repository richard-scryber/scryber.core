using System;
using System.Collections.Generic;
using Scryber.PDF;


namespace Scryber.Drawing
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
