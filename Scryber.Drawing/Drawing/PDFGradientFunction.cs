using System;
using System.Collections.Generic;

namespace Scryber.Drawing
{
    public abstract class PDFGradientFunction
    {
        public PDFGradientFunction()
        {
        }

        public abstract void WriteFunctionDictionary(PDFContextBase context, PDFWriter writer);
    }

    /// <summary>
    /// An axial function (linear from one colour to the next)
    /// </summary>
    public class PDFGradientLinearFunction2 : PDFGradientFunction
    {
        public double DomainStart { get; set; }

        public double DomainEnd { get; set; }

        public PDFColor ColorZero { get; set; }

        public PDFColor ColorOne { get; set; }

        public double Exponent { get; set; }


        public PDFGradientLinearFunction2(PDFColor color0, PDFColor color1)
            :this(color0, color1, 0, 1, 1)
        {
        }

        public PDFGradientLinearFunction2(PDFColor color0, PDFColor color1, double domainStart, double domainEnd, double exponent)
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

            writer.BeginDictionaryEntry("C0");
            writer.WriteArrayRealEntries(this.ColorZero.Red.Value, this.ColorZero.Green.Value, this.ColorZero.Blue.Value);
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("C1");
            writer.WriteArrayRealEntries(this.ColorOne.Red.Value, this.ColorOne.Green.Value, this.ColorOne.Blue.Value);
            writer.EndDictionaryEntry();

            writer.WriteDictionaryRealEntry("N", this.Exponent);
            writer.EndDictionary(); //function
        }
    }

    public class PDFGradientFunctionBoundary
    {
        public double Bounds { get; set; }

        public PDFGradientFunctionBoundary(double value)
        {
            this.Bounds = value;
        }
    }

    public class PDFGradientFunction3 : PDFGradientFunction
    {
        public PDFGradientFunctionBoundary[] Boundaries { get; set; }

        public PDFGradientFunction[] Functions { get; set; }

        public double DomainStart { get; set; }

        public double DomainEnd { get; set; }

        public PDFGradientFunction3(PDFGradientFunction[] functions, PDFGradientFunctionBoundary[] boundaries)
        {
            if (boundaries.Length != functions.Length - 1)
                throw new ArgumentOutOfRangeException("The array of boundaries must be one less than the array of functions");

            this.Boundaries = boundaries;
            this.Functions = functions;
        }

        public override void WriteFunctionDictionary(PDFContextBase context, PDFWriter writer)
        {
            writer.BeginDictionary();
            writer.WriteDictionaryNumberEntry("FunctionType", 3);

            writer.BeginDictionaryEntry("Domain");
            writer.WriteArrayRealEntries(0.0, 1.0);
            writer.EndDictionaryEntry();


            //The bounds is the function extents of the functions
            writer.BeginDictionaryEntry("Bounds");
            writer.BeginArray();
            foreach (var boundary in this.Boundaries)
            {
                writer.BeginArrayEntry();
                writer.WriteRealS(boundary.Bounds);
                writer.EndArrayEntry();
            }
            writer.EndArray();
            writer.EndDictionaryEntry();

            //Write the array of function 2 (Axial aka Linear between 2 colours)
            List<double> encodes = new List<double>();
            writer.BeginDictionaryEntry("Functions");
            writer.BeginArray();
            foreach (var func in this.Functions)
            {
                writer.BeginArrayEntry();
                func.WriteFunctionDictionary(context, writer);
                writer.EndArrayEntry();
                //May need to change these values
                encodes.Add(0);
                encodes.Add(1);
            }
    
            writer.EndArray();
            writer.EndDictionaryEntry();

            //Write the encodes for each of the functions 0 1 in a single array
            writer.BeginDictionaryEntry("Encode");
            writer.WriteArrayRealEntries(encodes.ToArray());

            writer.EndDictionaryEntry();
            writer.EndDictionary();
        }
    }
}
