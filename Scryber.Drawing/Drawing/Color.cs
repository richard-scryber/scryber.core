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
using Scryber.PDF.Native;
using Scryber.PDF;
using System.CodeDom;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;


namespace Scryber.Drawing
{
    /// <summary>
    /// Defines an immutable colour in one of the known PDF colourspaces - G, RGB, CMYK.
    /// </summary>
    /// <remarks>Note the LAB and HSB are not currently supported</remarks>
    [PDFParsableValue()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct Color : ITypedObject, IEquatable<Color>
    {
        private const string TransparentName = "Transparent";
        internal const float UnassignedFloat = -1;
        internal const int UnassignedByte = -1;

        private ColorSpace _cs;

        private byte _one;

        private byte _two;

        private byte _three;

        private byte _four;


        /// <summary>
        /// Gets or sets the colorspace for this color.
        /// </summary>
        public ColorSpace ColorSpace
        {
            get { return _cs; }
        }


        //RGB properties

        /// <summary>
        /// Gets the red component for an RGB color (in the range 0.0 to 1.0) or -1.0 if not an RGB color.
        /// </summary>
        public float Red
        {
            get { return this._cs == ColorSpace.RGB ? GetPDFColorComponent(_one) : UnassignedFloat; }
        }

        /// <summary>
        /// Gets the green component for an RGB color (in the range 0.0 to 1.0) or -1.0 if not an RGB color.
        /// </summary>
        public float Green
        {
            get { return this._cs == ColorSpace.RGB ? GetPDFColorComponent(_two) : UnassignedFloat; ; }
        }

        /// <summary>
        /// Gets the blue component for an RGB color (in the range 0.0 to 1.0) or -1.0 if not an RGB color.
        /// </summary>
        public float Blue
        {
            get { return this._cs == ColorSpace.RGB ? GetPDFColorComponent(_three) : UnassignedFloat; ; }
        }

        /// <summary>
        /// Gets the red component for an RGB color (in the range 0 to 255) or -1 if not an RGB color.
        /// </summary>
        public int Red255
        {
            get
            {
                return this._cs == ColorSpace.RGB ? _one : UnassignedByte;
            }
        }

        /// <summary>
        /// Gets the green component for an RGB color (in the range 0 to 255) or -1 if not an RGB color.
        /// </summary>
        public int Green255
        {
            get
            {
                return this._cs == ColorSpace.RGB ? _two : UnassignedByte;
            }
        }

        /// <summary>
        /// Gets the blue component for an RGB color (in the range 0 to 255) or -1 if not an RGB color.
        /// </summary>
        public int Blue255
        {
            get
            {
                return this._cs == ColorSpace.RGB ? _three : UnassignedByte;
            }
        }

        //Gray properties

        /// <summary>
        /// Gets the Gray component for an Grayscale color (in the range 0.0 to 1.0) or -1.0 if not an Grayscale color.
        /// </summary>
        public float Gray
        {
            get { return this._cs == ColorSpace.G ? GetPDFColorComponent(_one) : UnassignedFloat; ; }
        }

        /// <summary>
        /// Gets the Gray component for an Grayscale color (in the range 0 to 255) or -1 if not an Grayscale color.
        /// </summary>
        public int Gray255
        {
            get { return this._cs == ColorSpace.G ? _one : UnassignedByte; ; }
        }

        //CMYK Properties

        /// <summary>
        /// Gets the Cyan component for a CMYK color (in the range 0.0 to 1.0) or -1.0 if not a CMYK color.
        /// </summary>
        public float Cyan
        {
            get { return this._cs == ColorSpace.CMYK ? GetPDFColorComponent(_one) : UnassignedFloat; }
        }

        /// <summary>
        /// Gets the Magenta component for a CMYK color (in the range 0.0 to 1.0) or -1.0 if not a CMYK color.
        /// </summary>
        public float Magenta
        {
            get { return this._cs == ColorSpace.CMYK ? GetPDFColorComponent(_two) : UnassignedFloat; }
        }

        /// <summary>
        /// Gets the Yellow component for a CMYK color (in the range 0.0 to 1.0) or -1.0 if not a CMYK color.
        /// </summary>
        public float Yellow
        {
            get { return this._cs == ColorSpace.CMYK ? GetPDFColorComponent(_three) : UnassignedFloat; }
        }

        /// <summary>
        /// Gets the Black component for a CMYK color (in the range 0.0 to 1.0) or -1.0 if not a CMYK color.
        /// </summary>
        public float Black
        {
            get { return this._cs == ColorSpace.CMYK ? GetPDFColorComponent(_four) : UnassignedFloat; }
        }

        /// <summary>
        /// Gets the Cyan component for a CMYK color (in the range 0 to 255) or -1 if not a CMYK color.
        /// </summary>
        public int Cyan255
        {
            get { return this._cs == ColorSpace.CMYK ? _one : UnassignedByte; }
        }

        /// <summary>
        /// Gets the Magenta component for a CMYK color (in the range 0 to 255) or -1 if not a CMYK color.
        /// </summary>
        public int Magenta255
        {
            get { return this._cs == ColorSpace.CMYK ? _two : UnassignedByte; }
        }

        /// <summary>
        /// Gets the Yellow component for a CMYK color (in the range 0 to 255) or -1 if not a CMYK color.
        /// </summary>
        public int Yellow255
        {
            get { return this._cs == ColorSpace.CMYK ? _three : UnassignedByte; }
        }

        /// <summary>
        /// Gets the Black component for a CMYK color (in the range 0 to 255) or -1 if not a CMYK color.
        /// </summary>
        public int Black255
        {
            get { return this._cs == ColorSpace.CMYK ? _four : UnassignedByte; }
        }

        // Object type

        public ObjectType Type
        {
            get { return ObjectTypes.Color; }
        }

        public bool IsTransparent
        {
            get { return this._cs == ColorSpace.None; }
        }

        public bool IsEmpty
        {
            get { return this.IsTransparent; }
        }

        private Color(ColorSpace cs, byte one, byte two, byte three, byte four)
        {
            this._cs = cs;
            if (this._cs == ColorSpace.None)
            {
                this._one = 0;
                this._two = 0;
                this._three = 0;
                this._four = 0;
            }
            else if (this._cs == ColorSpace.G)
            {
                this._one = one;
                this._two = 0;
                this._three = 0;
                this._four = 0;
            }
            else if (cs == ColorSpace.RGB)
            {
                this._one = one;
                this._two = two;
                this._three = three;
                this._four = 0;
            }
            else if (cs == ColorSpace.CMYK)
            {
                this._one = one;
                this._two = two;
                this._three = three;
                this._four = four;
            }
            else
                throw new ArgumentOutOfRangeException(String.Format(Errors.ColorValueIsNotCurrentlySupported, cs.ToString()), "cs");
        }
        

        /// <summary>
        /// Creates a new Gray color
        /// </summary>
        /// <param name="gray"></param>
        public Color(int gray)
            : this(ColorSpace.G, ValidateColorRange(gray, "gray"), 0 ,0 ,0)
        {
        }

        /// <summary>
        /// Creates a new instance of the PDF Color with an RGB Color Space and specified colors
        /// </summary>
        public Color(int red, int green, int blue)
            : this(ColorSpace.RGB,
                  ValidateColorRange(red, "red"),
                  ValidateColorRange(green, "green"),
                  ValidateColorRange(blue, "blue"),
                  0)
        {
        }

        /// <summary>
        /// Creates a new instance of the PDF Color with a CMYK Color Space and specified colors
        /// </summary>
        public Color(int cyan, int magenta, int yellow, int black)
            : this(ColorSpace.CMYK,
                  ValidateColorRange(cyan, "cyan"),
                  ValidateColorRange(magenta, "magenta"),
                  ValidateColorRange(yellow, "yellow"),
                  ValidateColorRange(black, "black"))
        {
        }

        private static byte ValidateColorRange(int value, string errorParam = "value")
        {
            if (value < 0 || value > 255)
                throw new ArgumentOutOfRangeException(errorParam, "The value " + value + "' for " + errorParam + " must be in the range of 0 to 255");
            return Convert.ToByte(value);
        }

        private static float ValidateColorRange(float value, string color)
        {
            if (value < 0.0 || value > 1.0)
                throw new ArgumentOutOfRangeException("The value '" + value.ToString() + "' for '" + color + "' was not in the range 0 to 1");

            return value;
        }

        private static bool IsUnassigned(float value)
        {
            return value == UnassignedFloat;
        }

        public bool Equals(Color other)
        {
            return other._cs == this._cs
                && other._one == this._one
                && other._two == this._two
                && other._three == this._three
                && other._four == this._four;
        }

        public override bool Equals(object obj)
        {
            if (obj is Color)
                return this.Equals((Color)obj);
            else
                return false;
        }

        public override int GetHashCode()
        {
            int val = (_one << 24);
            val += (_two << 16);
            val += (_three << 8);
            val += _four;
            val = val * (int)_cs;
            return val;
        }


        public Color ToGray()
        {
            switch (this._cs)
            {
                case ColorSpace.None:
                    return StandardColors.Transparent;

                case ColorSpace.G:
                    return this;

                case ColorSpace.RGB:
                    float all = (this._one * 0.3F) + (this._two * 0.59F) + (this._three * 0.11F);
                    int val = all < 0 ? 0 : (all > 255 ? 255 : Convert.ToInt32(all));
                    return new Color(val);

                case ColorSpace.CMYK:
                    var rgb = this.ToRGB();
                    return rgb.ToGray();

                default:
                    throw new NotSupportedException("Cannot convert colorspace " + this._cs + " to gray");
            } 
        }

        public Color ToCMYK()
        {
            switch (this._cs)
            {
                case ColorSpace.None:
                    return StandardColors.Transparent;

                case ColorSpace.G:
                    return new Color(ColorSpace.RGB, this._one, this._one, this._one, 0).ToCMYK();

                case ColorSpace.RGB:

                    var k = FitsByte(255 - Math.Max(Math.Max(_one, _two), _three)); //1 - max(R, G, B)
                    var c = FitsByte((255.0 - _one - k) / (255.0 - k)); //(1-R-K) / (1-K)
                    var m = FitsByte((255.0 - _two - k) / (255.0 - k)); //(1-G-K) / (1-K)
                    var y = FitsByte((255.0 - _three - k) / (255.0 - k)); //(1-B-K) / (1-K)

                    return new Color(c, m, y, k);

                case ColorSpace.CMYK:

                    return this;
                default:
                    throw new NotSupportedException("Cannot convert colorspace " + this._cs + " to CMYK");
            }
        }

        public Color ToRGB()
        {
            if (this._cs == ColorSpace.G)
            {
                return new Color(this._one, this._one, this._one);
            }
            else if (this._cs == ColorSpace.RGB)
            {
                return this;
            }
            else if (this._cs == ColorSpace.CMYK)
            {
                int r = FitsByte((255.0 - _one) * (255.0 - _four)); // 1-C * 1-K
                int g = FitsByte((255.0 - _two) * (255.0 - _four)); // 1-M * 1-K
                int b = FitsByte((255.0 - _three) * (255.0 - _four)); // 1-Y * 1-K

                return new Color(r, g, b);
            }
            else
                throw new NotSupportedException("Cannot convert colorspace " + this._cs + " to RGB");
        }

        private int FitsByte(double v)
        {
            return Convert.ToInt32(v < 0.0 ? 0.0 : (v > 255.0 ? 255.0 : v));
        }

        private int FitsByte(int v)
        {
            return v < 0 ? 0 : (v > 255 ? 255 : v);
        }

        

        #region public override string ToString()

        public override string ToString()
        {
            if (this.ColorSpace == ColorSpace.None && this.IsEmpty)
                return TransparentName;
            
            System.Text.StringBuilder sb = new StringBuilder(20); //CMYK(3,3,3,3)
            
            sb.Append(this.ColorSpace.ToString().ToLower());
            sb.Append("(");
            switch (this.ColorSpace)
            {
                case ColorSpace.G:
                    sb.Append(this._one) ;
                    break;
                case ColorSpace.CMYK:
                    sb.Append(this._one);
                    sb.Append(",");
                    sb.Append(this._two);
                    sb.Append(",");
                    sb.Append(this._three);
                    sb.Append(",");
                    sb.Append(this._four);
                    break;
                case ColorSpace.RGB:
                case ColorSpace.HSL:
                case ColorSpace.LAB:
                    sb.Append(this._one);
                    sb.Append(",");
                    sb.Append(this._two);
                    sb.Append(",");
                    sb.Append(this._three);
                    break;
                case ColorSpace.None:
                case ColorSpace.Custom: 
                default:
                    throw new ArgumentOutOfRangeException("this.ColorSpace",String.Format(Errors.ColorValueIsNotCurrentlySupported,this.ColorSpace.ToString()));
                    
            }
            sb.Append(")");
            return sb.ToString();
        }

        #endregion

        #region Parse(string) + bool TryParse(string, out color)

        public static Color Parse(string value)
        {
            Color color;

            if(!TryParse(value,out color))
                throw new FormatException("The color string '" + (String.IsNullOrEmpty(value) ? "" : value) + "' was in the incorrect format");
            return color;
        }

        /// <summary>
        /// Creates a new PDFColor from the provided string  rgb(Red,Green,Blue) or g(Gray) or #GG or #RGB or #RRGGBB
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>A new instance of the PDF Color</returns>
        public static bool TryParse(string value, out Color color)
        {
            ColorSpace cs = ColorSpace.RGB;
            color = Color.Transparent;

            if (string.IsNullOrEmpty(value))
                return false;

            if (value == TransparentName)
            {
                color = StandardColors.Transparent;
                return true;
            } 
            
            if (value.IndexOf("(") > 0)
            {
                string s = value.Trim().Substring(0, value.IndexOf("(")).ToUpper();
                value = value.Substring(s.Length + 1);
                int close = value.IndexOf(")");
                if (close < 0 || close >= value.Length)
                    return false;

                value = value.Substring(0, close);//remove closing bracket
                object parsedCs;
                if (EnumParser.TryParse(typeof(ColorSpace), s, true, out parsedCs) == false)
                    return false;
                else
                    cs = (ColorSpace)parsedCs;

                string[] vals = value.Split(',');
                byte[] rgbs = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    if (i < vals.Length)
                    {
                        byte parsed;
                        if (!byte.TryParse(vals[i], out parsed))
                            return false;

                        rgbs[i] = parsed;
                    }
                    else
                    {
                        rgbs[i] = 0;
                    }
                }
                switch (cs)
                {
                    case ColorSpace.G:
                        if (vals.Length != 1)
                            return false;
                        color = new Color(cs, rgbs[0], 0, 0, 0);
                        return true;

                    case ColorSpace.RGB:
                        if (vals.Length != 3)
                            return false;
                        color = new Color(cs, rgbs[0], rgbs[1], rgbs[2], 0);
                        return true;

                    case ColorSpace.CMYK:
                        if (vals.Length != 4)
                            return false;

                        color = new Color(cs, rgbs[0], rgbs[1], rgbs[2], rgbs[3]);
                        return true;
                    default:
                        return false;
                }
            }
            else if (value.StartsWith("#"))
            {
                cs = ColorSpace.RGB;


                string r;
                string g;
                string b;

                if (value.Length == 2) //#G
                {
                    cs = ColorSpace.G;
                    r = value.Substring(1, 1);
                    r = r + r;
                    g = r;
                    b = r;
                }
                else if (value.Length == 3) //#GG
                {
                    cs = ColorSpace.G;
                    r = value.Substring(1, 2);
                    g = r;
                    b = r;
                }
                else if (value.Length == 4) //#RGB
                {
                    r = value.Substring(1, 1);
                    r = r + r;
                    g = value.Substring(2, 1);
                    g = g + g;
                    b = value.Substring(3, 1);
                    b = b + b;
                }
                else if (value.Length == 7) //#RRGGBB
                {
                    r = value.Substring(1, 2);
                    g = value.Substring(3, 2);
                    b = value.Substring(5, 2);
                }
                else
                    return false;

                byte rb, gb, bb;

                if (cs == ColorSpace.G)
                {
                    if (!byte.TryParse(r, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out rb))
                        return false;

                    color = new Color(cs, rb, 0, 0, 0);
                }
                else if (cs == ColorSpace.RGB)
                {
                    if (!byte.TryParse(r, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out rb))
                        return false;
                    if (!byte.TryParse(g, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out gb))
                        return false;
                    if (!byte.TryParse(b, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out bb))
                        return false;

                    color = new Color(cs, rb, gb, bb, 0);
                }
                return true;

            }
            else if (value.Equals("inherit", StringComparison.CurrentCultureIgnoreCase))
                return false;

            else if (value.Equals("none", StringComparison.CurrentCultureIgnoreCase))
            {
                color = StandardColors.Transparent;
                return true;
            }
            else if (StandardColors.TryGetColorFromName(value, out color))
            {
                return true;
            }
            else if (Html.CSSColors.Names2Colors.TryGetValue(value, out var found))
            {
                return TryParse(found, out color);
            }
            else
                return false;
        }

        #endregion

        #region TryParseRGBA(value, out color, out opacity)
        
        /// <summary>
        /// Creates a new PDFColor from the provided string  rgb(Red,Green,Blue) or g(Gray) or #GG or #RGB or #RRGGBB
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>A new instance of the PDF Color</returns>
        public static bool TryParseRGBA(string value, out Color color, out double? opacity)
        {
            color = StandardColors.Transparent;
            opacity = null;

            if (string.IsNullOrEmpty(value))
                return false;

            if (value.StartsWith("rgba(", StringComparison.InvariantCultureIgnoreCase))
            {
                string s = value.Trim().Substring(0, value.IndexOf("(")).ToUpper();
                value = value.Substring(s.Length + 1);
                int close = value.IndexOf(")");
                if (close < 0 || close >= value.Length)
                    return false;

                value = value.Substring(0, close);//remove closing bracket
                

                string[] vals = value.Split(',');
                byte[] rgbs = new byte[3];
                byte parsed;
                for (int i = 0; i < 3; i++)
                {
                    if (i < vals.Length)
                    {
                        if (!byte.TryParse(vals[i], out parsed))
                            return false;

                        rgbs[i] = (byte)parsed;
                    }
                    else
                    {
                        rgbs[i] = 0;
                    }
                }
                if(vals.Length >= 4)
                {
                    double op;
                    if (double.TryParse(vals[3], NumberStyles.Any, CultureInfo.InvariantCulture, out op) && op >= 0.0 && op <= 1.0)
                    {
                        opacity = op;
                    }
                    else
                        return false;
                }
                color = new Color(ColorSpace.RGB,
                        rgbs[0],
                        rgbs[1],
                        rgbs[2],
                        0);
                return true;

            }
            else 
            {
                opacity = 1.0;
                return false;
            }
        }
        
        #endregion
        

        private const float ByteToFloatFactor = (1.0F / 255.0F);

        public static float GetPDFColorComponent(byte b)
        {
            return ByteToFloatFactor * Convert.ToSingle(b);
        }


        public static explicit operator Color(string color)
        {
            return Color.Parse(color);
        }

        
        public static Color Transparent
        {
            get { return StandardColors.Transparent; }
        }


        public static Color Empty
        {
            get { return StandardColors.Transparent; }
        }


        /// <summary>
        /// Static construction method for an RGB color
        /// </summary>
        /// <param name="r">The red component (between 0 and 255)</param>
        /// <param name="g">The green component (between 0 and 255)</param>
        /// <param name="b">The blue component (between 0 and 255)</param>
        /// <returns>A new valid color</returns>
        /// <exception cref="ArgumentOutOfRangeException">If one or more of the color components are not in the allowed range</exception>
        public static Color FromRGB(int r, int g, int b)
        {
            return new Color(r, g, b);
        }
        
        /// <summary>
        /// Static construction method for a CMYK color
        /// </summary>
        /// <param name="c">The cyan component (between 0 and 255)</param>
        /// <param name="m">The magenta component (between 0 and 255)</param>
        /// <param name="y">The yellow component (between 0 and 255)</param>
        /// <param name="k">The black component (between 0 and 255)</param>
        /// <returns>A new valid color</returns>
        /// <exception cref="ArgumentOutOfRangeException">If one or more of the color components are not in the allowed range</exception>
        public static Color FromCMYK(int c, int m, int y, int k)
        {
            return new Color(c, m, y, k);
        }

    }
}
