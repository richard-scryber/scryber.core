/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Native;
using System.CodeDom;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Scryber.Drawing
{
    /// <summary>
    /// Defines a colour in one of the known PDF colourspaces - G, RGB, LAB, HSB.
    /// </summary>
    /// <remarks>Note the LAB and HSB are not currently supported</remarks>
    [PDFParsableValue()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFColor : PDFObject, IEquatable<PDFColor>, IPDFSimpleExpressionValue
    {
        private ColorSpace _cs;

        /// <summary>
        /// Gets or sets the current colorspace.
        /// </summary>
        public ColorSpace ColorSpace
        {
            get { return _cs; }
        }

        private System.Drawing.Color _c;
        /// <summary>
        /// Gets the internal color of the item
        /// </summary>
        public System.Drawing.Color Color
        {
            get { return _c; }
        }

        public PDFReal Red
        {
            get { return new PDFReal(GetPDFColorComponent(Color.R)); }
        }

        public PDFReal Green
        {
            get { return new PDFReal(GetPDFColorComponent(Color.G)); }
        }

        public PDFReal Blue
        {
            get { return new PDFReal(GetPDFColorComponent(Color.B)); }
        }

        public PDFReal Gray
        {
            get
            {
                PDFReal val = this.Red + this.Green + this.Blue;
                val = new PDFReal(val.Value / 3.0);
                return val;
            }
        }

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <param name="three"></param>
        public PDFColor(ColorSpace cs, double one, double two, double three)
            : this(cs, System.Drawing.Color.FromArgb(
                                            GetSystemColorComponent(one),
                                            GetSystemColorComponent(two),
                                            GetSystemColorComponent(three)))
        {
            if (cs != ColorSpace.RGB && cs != ColorSpace.RGB)
                throw new ArgumentOutOfRangeException(String.Format(Errors.ColorValueIsNotCurrentlySupported, this.ColorSpace.ToString()), "cs");
        }


        public PDFColor(double gray)
            : this(ColorSpace.G, System.Drawing.Color.FromArgb(
                                            GetSystemColorComponent(gray),
                                            GetSystemColorComponent(gray),
                                            GetSystemColorComponent(gray)))
        {

        }

        /// <summary>
        /// Creates a new instance of the PDF Color with an RGB Color Space and Transparent colors
        /// </summary>
        public PDFColor(double red, double green, double blue)
            : this(ColorSpace.RGB, 
                    System.Drawing.Color.FromArgb(
                            GetSystemColorComponent(red), 
                            GetSystemColorComponent(green), 
                            GetSystemColorComponent(blue)))
        {
        }

        /// <summary>
        /// Creates a new instance of the PDF Color with the specified Color Space and Color
        /// </summary>
        /// <param name="cs">The color space to use</param>
        /// <param name="c">The specific color</param>
        public PDFColor(ColorSpace cs, System.Drawing.Color c)
            : this(cs,c,PDFObjectTypes.Color)
        {
            this._cs = cs;
            this._c = c;
        }

        protected PDFColor(ColorSpace cs, System.Drawing.Color c, PDFObjectType type)
            : base(type)
        {
            if (cs == ColorSpace.LAB || cs == ColorSpace.Custom)
                throw new ArgumentOutOfRangeException(String.Format(Errors.ColorValueIsNotCurrentlySupported, this.ColorSpace.ToString()), "cs");
            this._cs = cs;
            this._c = c;
        }

        #endregion

        public bool Equals(PDFColor other)
        {
            if (null == other)
                return false;
            else if (this.ColorSpace != other.ColorSpace)
                return false;
            else if (this.Color.A != other.Color.A)
                return false;
            else if (this.Color.R != other.Color.R)
                return false;
            else if (this.Color.G != other.Color.G)
                return false;
            else if (this.Color.B != other.Color.B)
                return false;
            else
                return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is PDFColor)
                return this.Equals((PDFColor)obj);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return (int)this._cs ^ this._c.GetHashCode();
        }

        #region public override string ToString()

        public override string ToString()
        {
            System.Text.StringBuilder sb = new StringBuilder(20);
            sb.Append(this.ColorSpace.ToString());
            sb.Append(" (");
            switch (this.ColorSpace)
            {
                case ColorSpace.G:
                    sb.Append(this.Gray.Value.ToString());
                    break;
                case ColorSpace.HSL:
                    sb.Append(this.Color.GetHue());
                    sb.Append(",");
                    sb.Append(this.Color.GetSaturation());
                    sb.Append(",");
                    sb.Append(this.Color.GetBrightness());
                    break;
                case ColorSpace.RGB:
                    sb.Append(this.Color.R);
                    sb.Append(",");
                    sb.Append(this.Color.G);
                    sb.Append(",");
                    sb.Append(this.Color.B);
                    break;
                case ColorSpace.LAB:
                case ColorSpace.Custom:
                default:
                    throw new ArgumentOutOfRangeException("this.ColorSpace",String.Format(Errors.ColorValueIsNotCurrentlySupported,this.ColorSpace.ToString()));
                    
            }
            sb.Append(")");
            return sb.ToString();
        }

        #endregion

        #region Parse(string) + bool TryParse(string, out color)

        public static PDFColor Parse(string value)
        {
            PDFColor color;
            if(!TryParse(value,out color))
                throw new FormatException("The color string '" + (String.IsNullOrEmpty(value) ? "" : value) + "' was in the incorrect format");
            return color;
        }

        /// <summary>
        /// Creates a new PDFColor from the provided string  rgb(Red,Green,Blue) or g(Gray) or #GG or #RGB or #RRGGBB
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>A new instance of the PDF Color</returns>
        public static bool TryParse(string value, out PDFColor color)
        {
            ColorSpace cs = ColorSpace.RGB;
            color = null;
            if (string.IsNullOrEmpty(value))
                return false;

            if (value.IndexOf("(") > 0)
            {
                string s = value.Trim().Substring(0, value.IndexOf("(")).ToUpper();
                value = value.Substring(s.Length + 1);
                int close = value.IndexOf(")");
                if (close < 0 || close >= value.Length)
                    return false;

                value = value.Substring(0, close);//remove closing bracket

                if (Enum.IsDefined(typeof(ColorSpace), s) == false)
                    return false;
                else
                    cs = (ColorSpace)Enum.Parse(typeof(ColorSpace), s.Trim());

                string[] vals = value.Split(',');
                int[] rgbs = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    if (i < vals.Length)
                    {
                        int parsed;
                        if(!int.TryParse(vals[i], out parsed) || parsed > 255 || parsed < 0)
                            return false;
                        
                        rgbs[i] = parsed;
                    }
                    else
                    {
                        rgbs[i] = 0;
                    }
                }

                if (cs == ColorSpace.G)
                {
                    color = new PDFColor(cs, System.Drawing.Color.FromArgb(rgbs[0], rgbs[0], rgbs[0]));
                    return true;
                }
                else if (cs == ColorSpace.RGB)
                {
                    color = new PDFColor(cs, System.Drawing.Color.FromArgb(rgbs[0], rgbs[1], rgbs[2]));
                    return true;
                }
                else if (cs == ColorSpace.HSL)
                {
                    color = new PDFColor(cs, rgbs[0], rgbs[1], rgbs[2]);
                    return true;
                }
                else
                    return false;

            }
            else if (value.StartsWith("#"))
            {
                cs = ColorSpace.RGB;


                string r;
                string g;
                string b;
                string a = "FF";
                if (value.Length == 2)
                {
                    cs = ColorSpace.G;
                    r = value.Substring(1, 1);
                    r = r + r;
                    g = r;
                    b = r;
                }
                else if (value.Length == 3)
                {
                    cs = ColorSpace.G;
                    r = value.Substring(1, 2);
                    g = r;
                    b = r;
                }
                else if (value.Length == 4)
                {
                    r = value.Substring(1, 1);
                    r = r + r;
                    g = value.Substring(2, 1);
                    g = g + g;
                    b = value.Substring(3, 1);
                    b = b + b;
                }
                else if (value.Length == 7)
                {
                    r = value.Substring(1, 2);
                    g = value.Substring(3, 2);
                    b = value.Substring(5, 2);
                }
                else
                    return false;
                
                byte ab, rb, gb, bb;

                if (!byte.TryParse(a, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out ab))
                    return false;
                if (!byte.TryParse(r, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out rb))
                    return false;
                if (!byte.TryParse(g, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out gb))
                    return false;
                if (!byte.TryParse(b, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out bb))
                    return false;

                color = new PDFColor(cs, System.Drawing.Color.FromArgb((int)ab, System.Drawing.Color.FromArgb((int)rb, (int)gb, (int)bb)));
                return true;

            }
            else if (value.Equals("inherit", StringComparison.CurrentCultureIgnoreCase))
                return false;
            else
            {
                if (PDFColors.TryGetColorFromName(value, out color))
                    return true;
                else
                    return false;
            }
        }

        #endregion


        
        private static float GetPDFColorComponent(byte p)
        {
            return GetPDFColorComponent(Convert.ToSingle(p));
        }

        private static float GetPDFColorComponent(float val)
        {
            return (1.0F / 255.0F) * val;
        }

        private static int GetSystemColorComponent(double val)
        {
            if (val > 1.0 || val < 0.0)
                throw new ArgumentOutOfRangeException("val", String.Format(Errors.ColorComponentMustBeBetweenZeroAndOne, val));
            val = 255.0 * val;
            return (int)val;
        }

        public static implicit operator PDFColor(System.Drawing.Color rgbcolor)
        {
            return new PDFColor(ColorSpace.RGB, rgbcolor);
        }


        public static PDFColor Transparent
        {
            get { return PDFColors.Transparent; }
        }

        public bool IsEmpty
        {
            get { return this.Color.IsEmpty; }
        }

        #region IPDFSimpleCodeDomValue Members

        public System.Linq.Expressions.Expression GetConstructorExpression()
        {
            Type drawingColor = typeof(System.Drawing.Color);
            Type pdfColor = typeof(PDFColor);
            ConstructorInfo ctor = pdfColor.GetConstructor(new Type[] { typeof(ColorSpace), typeof(System.Drawing.Color) });

            Expression colorSpace = Expression.Constant(this._cs);
            Expression colorCtor;

            if(this._c.IsNamedColor)
            {
                //System.Drawing.Color c = System.Drawing.Color.FromName(_c.ToKnownColor());
                string name = _c.Name;
                Expression Sname = Expression.Constant(name);
                MethodInfo fromName = drawingColor.GetMethod("FromName");
                if (null == fromName)
                    throw new MissingMethodException("No Method FromName exists on the System.Drawing.Color class");
                colorCtor = Expression.Call(null, fromName, Sname);
            }
            else
            {
                //System.Drawing.Color argb = System.Drawing.Color.FromArgb((int)this.Red, (int)this.Green, (int)this.Blue);
                int red = this._c.R;
                int green = this._c.G;
                int blue = this._c.B;
                MethodInfo fromArgb = drawingColor.GetMethod("FromArgb", new Type[] { typeof(int), typeof(int), typeof(int) });
                if (null == fromArgb)
                    throw new MissingMethodException("No Method FromArgb(int,int,int) could be found on the System.Drawing.Color class");
                colorCtor = Expression.Call(null, fromArgb, Expression.Constant(red), Expression.Constant(green), Expression.Constant(blue));
            }

            //return new PDFColor(cs, color);
            Expression create = Expression.New(ctor, colorSpace, colorCtor);
            return create;


        }

        //public CodeExpression GetConstructorExpression()
        //{
        //    //new PDFColor(ColorSpace,one,two,three)
        //    CodePrimitiveExpression one = new CodePrimitiveExpression(this.Red.Value);
        //    CodePrimitiveExpression two = new CodePrimitiveExpression(this.Green.Value);
        //    CodePrimitiveExpression three = new CodePrimitiveExpression(this.Blue.Value);
        //    CodePropertyReferenceExpression cs = new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(ColorSpace)), this.ColorSpace.ToString());
        //    return new CodeObjectCreateExpression(typeof(PDFColor), cs, one, two, three);
        //}

        #endregion
    }
}
