using System;
using System.Collections.Generic;
using Scryber.PDF.Graphics;

namespace Scryber.Drawing
{


    [PDFParsableValue()]
    public abstract class GradientDescriptor
    {

        #region public GradientType GradientType { get;}

        /// <summary>
        /// Gets the type of gradient this descriptor is for (linear or radial)
        /// </summary>
        public GradientType GradientType { get; protected set; }

        #endregion

        #region public PDFGradientColor[] Colors

        private List<GradientColor> _colors;
        /// <summary>
        /// Gets or sets the array of GradientColours in this gradient
        /// </summary>
        public List<GradientColor> Colors
        {
            get
            {
                if (null == _colors)
                    _colors = new List<GradientColor>(2);
                return _colors;
            }
            set
            {
                this._colors = value;
            }
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

        public GradientDescriptor(GradientType type)
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
        public virtual PDFGradientFunction GetGradientFunction(Point offset, Size size)
        {

            //if our first value is offset from the start put one in of
            //the same colour at 0 offset

            if (this.Colors.Count > 1 && this.Colors[0].Distance.HasValue && this.Colors[0].Distance.Value > 0.0)
                this.Colors.Insert(0, new GradientColor(this.Colors[0].Color, 0.0, this.Colors[0].Opacity));

            //if our last colour does not have a distance then put it at 100.
            if (this.Colors.Count > 1 && this.Colors[this.Colors.Count - 1].Distance.HasValue == false)
            {
                if (this.GradientType == GradientType.Radial && this.Repeating)
                    this.Colors.Add(new GradientColor(this.Colors[this.Colors.Count - 1].Color, 100, null));
                else
                    this.Colors[this.Colors.Count - 1].Distance = 100;
            }

            if (this.Repeating)
            {
                List<PDFGradientFunctionBoundary> bounds = GetRepeatingBoundaries(offset, size);
                List<PDFGradientFunction2> functions = GetRepeatingFunctionsForBounds(bounds);

                
                bounds.RemoveAt(bounds.Count - 1);

                while (bounds[bounds.Count - 1].Bounds > 1)
                {
                    bounds.RemoveAt(bounds.Count - 1);
                    functions.RemoveAt(functions.Count - 1);
                }

                return new PDFGradientFunction3(functions.ToArray(), bounds.ToArray());
            }
            else if (this.Colors.Count == 2)
            {
                var c0 = this.Colors[0];
                var c1 = this.Colors[1];
                var domainStart = c0.Distance.HasValue ? (c0.Distance.Value / 100.0) : 0;
                var domainEnd = c1.Distance.HasValue ? 1.0/(c1.Distance.Value / 100) : 1;
                return new PDFGradientFunction2(c0.Color, c1.Color, domainStart, domainEnd, 1.0);
            }
            else
            {
                List<PDFGradientFunction> functions = new List<PDFGradientFunction>();
                List<PDFGradientFunctionBoundary> bounds = new List<PDFGradientFunctionBoundary>();
                double boundsValue = 1.0 / (this.Colors.Count - 1);

                for (int i = 1; i < this.Colors.Count; i++)
                {
                    
                    var color0 = this.Colors[i - 1];
                    var color1 = this.Colors[i];
                    
                    //we have a single gradient from the previous color to this color.
                    //color 0 is specified at the start of the domain, so has a color, but no bounds.
                    //last color is at the end of the domain, so no bounds either
                    
                    functions.Add(new PDFGradientFunction2(color0.Color, color1.Color));
                    
                    if (i < this.Colors.Count - 1)
                    {
                        var distance = color1.Distance ?? boundsValue * i;
                        bounds.Add(new PDFGradientFunctionBoundary(distance));
                    }
                }

                return new PDFGradientFunction3(functions.ToArray(), bounds.ToArray());
            }
        }

        protected virtual List<PDFGradientFunction2> GetRepeatingFunctionsForBounds(List<PDFGradientFunctionBoundary> bounds)
        {
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

                if (col1Index >= this.Colors.Count)
                {
                    col0Index = 0;
                    col1Index = 1;
                }
            }

            return functions;
        }

        /// <summary>
        /// Gets a list of all the function boundaries for a gradient.
        /// </summary>
        /// <param name="offset">The offset of the shape in the page</param>
        /// <param name="size">The size of the shape in the page</param>
        /// <returns>A list of function boundaries</returns>
        protected virtual List<PDFGradientFunctionBoundary> GetRepeatingBoundaries(Point offset, Size size)
        {
            List<PDFGradientFunctionBoundary> bounds = new List<PDFGradientFunctionBoundary>();

            double total = 0.0;
            
            this.PreFillColorDistances();

            
            while (total < 1.0)
            {
                double curr = 0.0;
                for (int i = 1; i < this.Colors.Count; i++)
                {
                    var col = this.Colors[i];
                    if (col.Distance.HasValue && col.Distance.Value < 1.0)
                        curr = total + col.Distance.Value;
                    else if (total == 0.0 && curr == 0.0)
                        curr = total + (1.0 / this.Colors.Count);
                    else
                        curr = total; //Hard break
                    
                    bounds.Add(new PDFGradientFunctionBoundary(curr));
                }
                total = curr;
            }

            return bounds;
        }

        #endregion

        protected virtual void PreFillColorDistances()
        {
            int lastValue = -1;
            List<GradientColor> spacers = new List<GradientColor>();

            //Loop through each of the colors and either add it to the spacers if there is no difference
            //or 
            for(var i = 0; i < this.Colors.Count; i++)
            {
                var col = this.Colors[i];
                if (col.Distance.HasValue)
                {
                    if (spacers.Count > 0)
                    {
                        ApplySplitDistanceToSpacers(lastValue, spacers, col);
                    }
                    lastValue = i;
                }
                else
                    spacers.Add(col);
            }

            if(spacers.Count > 0)
            {
                ApplySplitDistanceToSpacers(lastValue, spacers, this.Colors[this.Colors.Count -1]);
            }
        }

        /// <summary>
        /// Applies a distance value equally to all the entries in the spacers list from the first color with distance at last index to the col provided
        /// </summary>
        /// <param name="lastValue">the index of the previous last color with a vlaue (or -1 for none)</param>
        /// <param name="spacers">All the spacer colors to add values to</param>
        /// <param name="col">The ultimate color to reach - it must have a distance value</param>
        private void ApplySplitDistanceToSpacers(int lastValue, List<GradientColor> spacers, GradientColor col)
        {
            double min = 0.0;
            if (lastValue >= 0) //first time
                min = this.Colors[lastValue].Distance.Value;

            double max = col.Distance.Value;
            double split = (max - min) / spacers.Count;

            for (var j = 0; j < spacers.Count; j++)
            {
                spacers[j].Distance = min + split;
                min += split;
            }
            spacers.Clear();
        }

        //
        // static parse methods
        //

        #region public static PDFGradientDescriptor Parse(string value)

        /// <summary>
        /// Parses a string to an appropriate gradient descriptor, throwing an invalid operation exception if not possible.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static GradientDescriptor Parse(string value)
        {
            GradientDescriptor desc;
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
        public static bool TryParse(string value, out GradientDescriptor descriptor)
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
                GradientLinearDescriptor linear;

                if (value.StartsWith("(") && value.EndsWith(")") && GradientLinearDescriptor.TryParseLinear(value.Substring(1, value.Length - 2), out linear))
                {
                    linear.Repeating = false;
                    descriptor = linear;
                    return true;
                }
            }
            else if(value.StartsWith("repeating-linear-gradient"))
            {
                value = value.Substring("repeating-linear-gradient".Length).Trim();
                GradientLinearDescriptor linear;

                if (value.StartsWith("(") && value.EndsWith(")") && GradientLinearDescriptor.TryParseLinear(value.Substring(1, value.Length - 2), out linear))
                {
                    linear.Repeating = true;
                    descriptor = linear;
                    return true;
                }
            }
            else if(value.StartsWith("radial-gradient"))
            {
                value = value.Substring("radial-gradient".Length).Trim();
                GradientRadialDescriptor radial;

                if (value.StartsWith("(") && value.EndsWith(")") && GradientRadialDescriptor.TryParseRadial(value.Substring(1, value.Length - 2), out radial))
                {
                    radial.Repeating = false;
                    descriptor = radial;
                    return true;
                }
            }
            else if (value.StartsWith("repeating-radial-gradient"))
            {
                value = value.Substring("repeating-radial-gradient".Length).Trim();
                GradientRadialDescriptor radial;

                if (value.StartsWith("(") && value.EndsWith(")") && GradientRadialDescriptor.TryParseRadial(value.Substring(1, value.Length - 2), out radial))
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
