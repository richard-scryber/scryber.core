using System;
using System.Collections.Generic;
using Scryber.PDF;
using Scryber.Drawing;

namespace Scryber.PDF.Graphics
{

    /// <summary>
    /// Defines the bounds of a function within a domain. (0.1, 0.3, etc) 
    /// </summary>
    public class PDFGradientFunctionBoundary
    {
        /// <summary>
        /// Gets the defined bounds.
        /// This will be from the previous boundary (or 0.0) to this boundary
        /// </summary>
        public double Bounds { get; set; }


        public PDFGradientFunctionBoundary(double value)
        {
            this.Bounds = value;
        }
    }

    /// <summary>
    /// Gets the function encoding within the array of a Function3
    /// </summary>
    public class PDFGradientFunctionEncode
    {
        public double Start { get; set; }

        public double End { get; set; }
    }

    /// <summary>
    /// A combined set of gradient functions, from one boundary to the next
    /// </summary>
    public class PDFGradientFunction3 : PDFGradientFunction
    {
        public PDFGradientFunctionBoundary[] Boundaries { get; set; }

        public PDFGradientFunction[] Functions { get; set; }

        public PDFGradientFunctionEncode[] Encodes { get; set; }

        public double DomainStart { get; set; }

        public double DomainEnd { get; set; }


        /// <summary>
        /// Creates a new GradientFunction 3 with the specified functions and boundaries (which there should be 1 less than functions)
        /// </summary>
        /// <param name="functions"></param>
        /// <param name="boundaries"></param>
        /// <param name="encodes"></param>
        public PDFGradientFunction3(PDFGradientFunction[] functions, PDFGradientFunctionBoundary[] boundaries)
            : this(functions, boundaries, CreateDefaultEncodes(functions))
        { }
        /// <summary>
        /// Creates a new GradientFunction 3 with the specified functions and boundaries (which there should be 1 less than functions)
        /// </summary>
        /// <param name="functions"></param>
        /// <param name="boundaries"></param>
        /// <param name="encodes"></param>
        public PDFGradientFunction3(PDFGradientFunction[] functions, PDFGradientFunctionBoundary[] boundaries, PDFGradientFunctionEncode[] encodes)
        {
            if (null == boundaries || boundaries.Length == 0)
                throw new ArgumentNullException("No boundaries were provided");

            if(null == encodes || encodes.Length == 0)
                throw new ArgumentNullException("No encodes were provided");

            if(null == functions || functions.Length == 0)
                throw new ArgumentNullException("No functions were provided");


            if (boundaries.Length != functions.Length - 1)
                throw new ArgumentOutOfRangeException("The array of boundaries must be one less than the array of functions");

            if(encodes.Length != functions.Length)
                throw new ArgumentOutOfRangeException("The array of encodes must be the same length as the array of functions");

            this.Boundaries = boundaries;
            this.Functions = functions;
            this.Encodes = encodes;
            this.DomainStart = 0;
            this.DomainEnd = 1;
        }

        public override void WriteFunctionDictionary(PDFContextBase context, PDFWriter writer)
        {
            writer.BeginDictionary();
            writer.WriteDictionaryNumberEntry("FunctionType", 3);

            writer.BeginDictionaryEntry("Domain");
            writer.WriteArrayRealEntries(this.DomainStart, this.DomainEnd);
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

        private static PDFGradientFunctionEncode[] CreateDefaultEncodes(PDFGradientFunction[] forfunctions)
        {
            List<PDFGradientFunctionEncode> all = new List<PDFGradientFunctionEncode>();
            foreach (var f in forfunctions)
            {
                all.Add(new PDFGradientFunctionEncode() { Start = 0, End = 1 });
            }
            return all.ToArray();
        }
    }
}
