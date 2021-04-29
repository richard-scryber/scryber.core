using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Scryber.Drawing
{


    [PDFParsableValue()]
    public abstract class PDFGradientDescriptor
    {

        #region public GradientType GradientType { get;}

        /// <summary>
        /// Gets the type of gradient this descriptor is for (linear or radial)
        /// </summary>
        public GradientType GradientType { get; protected set; }

        #endregion

        #region public PDFGradientColor[] Colors

        /// <summary>
        /// Gets or sets the array of GradientColours in this gradient
        /// </summary>
        public PDFGradientColor[] Colors
        {
            get;
            set;
        }

        #endregion

        #region public bool Repeating { get; set; }

        /// <summary>
        /// Gets or sets the flag to indicate if this gradient is repeating
        /// </summary>
        public bool Repeating { get; set; }

        #endregion

        //
        // .ctor
        //

        public PDFGradientDescriptor(GradientType type)
        {
            this.GradientType = type;
        }

        //
        // instance method(s)
        //

        #region public override PDFGradientFunction GetGradientFunction()

        /// <summary>
        /// Buids and returns a gradient function that will render the stops and colors for this linear gradient
        /// </summary>
        /// <returns></returns>
        public virtual PDFGradientFunction GetGradientFunction()
        {
            if (this.Repeating)
            {
                List<PDFGradientFunctionBoundary> bounds = new List<PDFGradientFunctionBoundary>();

                double total = 0.0;
                while (total < 1.0)
                {
                    double curr = 0.0;
                    for (int i = 1; i < this.Colors.Length; i++)
                    {
                        var col = this.Colors[i];
                        curr = col.Distance.HasValue ? total + (col.Distance.Value / 100) : total;
                        bounds.Add(new PDFGradientFunctionBoundary(curr));
                    }
                    total = curr;
                }
                List<PDFGradientFunction2> functions = new List<PDFGradientFunction2>();

                var col0Index = 0;
                var col1Index = 1;
                for (int i = 0; i < bounds.Count; i++)
                {
                    var color0 = this.Colors[col0Index].Color;
                    var color1 = this.Colors[col1Index].Color;

                    var func = new PDFGradientFunction2(color0, color1);
                    functions.Add(func);

                    col0Index++;
                    col1Index++;

                    if (col1Index >= this.Colors.Length)
                    {
                        col0Index = 0;
                        col1Index = 1;
                    }
                }

                bounds.RemoveAt(bounds.Count - 1);

                while (bounds[bounds.Count - 1].Bounds > 1)
                {
                    bounds.RemoveAt(bounds.Count - 1);
                    functions.RemoveAt(functions.Count - 1);
                }

                return new PDFGradientFunction3(functions.ToArray(), bounds.ToArray());
            }
            else if (this.Colors.Length == 2)
            {
                return new PDFGradientFunction2(this.Colors[0].Color, this.Colors[1].Color);
            }
            else
            {
                List<PDFGradientFunction2> functions = new List<PDFGradientFunction2>();
                List<PDFGradientFunctionBoundary> bounds = new List<PDFGradientFunctionBoundary>();
                double boundsValue = 1.0 / (this.Colors.Length - 1);

                for (int i = 1; i < this.Colors.Length; i++)
                {
                    functions.Add(new PDFGradientFunction2(this.Colors[i - 1].Color, this.Colors[i].Color));

                    if (i < this.Colors.Length - 1)
                    {
                        bounds.Add(new PDFGradientFunctionBoundary(boundsValue * i));
                    }
                }

                return new PDFGradientFunction3(functions.ToArray(), bounds.ToArray());
            }
        }

        #endregion


        //
        // static parse methods
        //

        #region public static PDFGradientDescriptor Parse(string value)

        /// <summary>
        /// Parses a string to an appropriate gradient descriptor, throwing an invalid operation exception if not possible.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PDFGradientDescriptor Parse(string value)
        {
            PDFGradientDescriptor desc;
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");
            else if (TryParse(value, out desc))
                return desc;
            else
                throw new InvalidOperationException("The gradient descriptor '" + value + "' could not be parsed");
        }

        #endregion

        #region public static bool TryParse(string value, out PDFGradientDescriptor descriptor)

        /// <summary>
        /// Attempts to parse a css style gradient string into a specific gradient descriptor
        /// </summary>
        /// <param name="value">The string to parse e.g. radial-gradient(red, green)</param>
        /// <param name="descriptor">Set to the parsed descriptor value</param>
        /// <returns>True if the gradient was parsed correctly</returns>
        public static bool TryParse(string value, out PDFGradientDescriptor descriptor)
        {
            descriptor = null;

            if (null == value)
                return false;

            value = value.Trim();


            if (string.IsNullOrEmpty(value))
                return false;

            else if (value.StartsWith("linear-gradient"))
            {
                value = value.Substring("linear-gradient".Length).Trim();
                PDFGradientLinearDescriptor linear;

                if (value.StartsWith("(") && value.EndsWith(")") && PDFGradientLinearDescriptor.TryParseLinear(value.Substring(1, value.Length - 2), out linear))
                {
                    linear.Repeating = false;
                    descriptor = linear;
                    return true;
                }
            }
            else if(value.StartsWith("repeating-linear-gradient"))
            {
                value = value.Substring("repeating-linear-gradient".Length).Trim();
                PDFGradientLinearDescriptor linear;

                if (value.StartsWith("(") && value.EndsWith(")") && PDFGradientLinearDescriptor.TryParseLinear(value.Substring(1, value.Length - 2), out linear))
                {
                    linear.Repeating = true;
                    descriptor = linear;
                    return true;
                }
            }
            else if(value.StartsWith("radial-gradient"))
            {
                value = value.Substring("radial-gradient".Length).Trim();
                PDFGradientRadialDescriptor radial;

                if (value.StartsWith("(") && value.EndsWith(")") && PDFGradientRadialDescriptor.TryParseRadial(value.Substring(1, value.Length - 2), out radial))
                {
                    radial.Repeating = false;
                    descriptor = radial;
                    return true;
                }
            }
            else if (value.StartsWith("repeating-radial-gradient"))
            {
                value = value.Substring("repeating-radial-gradient".Length).Trim();
                PDFGradientRadialDescriptor radial;

                if (value.StartsWith("(") && value.EndsWith(")") && PDFGradientRadialDescriptor.TryParseRadial(value.Substring(1, value.Length - 2), out radial))
                {
                    radial.Repeating = true;
                    descriptor = radial;
                    return true;
                }
            }

            return false;
        }

        #endregion

    }










}
