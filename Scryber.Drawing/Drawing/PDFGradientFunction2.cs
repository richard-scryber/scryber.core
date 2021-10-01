using System;
using System.Collections.Generic;
using Scryber.PDF;

namespace Scryber.Drawing
{
    
    /// <summary>
    /// An axial function (linear from one colour to the next)
    /// </summary>
    public class PDFGradientFunction2 : PDFGradientFunction
    {
        public double DomainStart { get; set; }

        public double DomainEnd { get; set; }

        public PDFColor ColorZero { get; set; }

        public PDFColor ColorOne { get; set; }

        public double Exponent { get; set; }


        public PDFGradientFunction2(PDFColor color0, PDFColor color1)
            :this(color0, color1, 0, 1, 1)
        {
        }

        public PDFGradientFunction2(PDFColor color0, PDFColor color1, double domainStart, double domainEnd, double exponent)
        {
            this.ColorZero = color0;
            this.ColorOne = color1;
            this.DomainStart = domainStart;
            this.DomainEnd = domainEnd;
            this.Exponent = exponent;
        }

        public override void WriteFunctionDictionary(PDFContextBase context, PDFWriter writer)
        {
            writer.BeginDictionary();
            writer.WriteDictionaryNumberEntry("FunctionType", 2);

            writer.BeginDictionaryEntry("Domain");
            writer.WriteArrayRealEntries(this.DomainStart, this.DomainEnd);
            writer.EndDictionaryEntry();
            var c0 = this.ColorZero.ToRGB();
            writer.BeginDictionaryEntry("C0");
            writer.WriteArrayRealEntries(c0.Red, c0.Green, c0.Blue);
            writer.EndDictionaryEntry();
            var c1 = this.ColorOne.ToRGB();
            writer.BeginDictionaryEntry("C1");
            writer.WriteArrayRealEntries(c1.Red, c1.Green, c1.Blue);
            writer.EndDictionaryEntry();

            writer.WriteDictionaryRealEntry("N", this.Exponent);
            writer.EndDictionary(); //function
        }
    }

    
}
