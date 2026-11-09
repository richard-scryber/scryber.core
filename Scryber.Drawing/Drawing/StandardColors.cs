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

namespace Scryber.Drawing
{
    public class StandardColors
    {

        [System.Xml.Serialization.XmlAttribute("Aqua")]
        public static readonly Color Aqua = Color.Parse("#00FFFF");

        [System.Xml.Serialization.XmlAttribute("Black")]
        public static readonly Color Black = Color.Parse("#000000");

        [System.Xml.Serialization.XmlAttribute("Blue")]
        public static readonly Color Blue = Color.Parse("#0000FF");

        [System.Xml.Serialization.XmlAttribute("Fuchisa")]
        public static readonly Color Fuchsia = Color.Parse("#FF00FF");

        [System.Xml.Serialization.XmlAttribute("Gray")]
        public static readonly Color Gray = Color.Parse("#808080");

        
        [System.Xml.Serialization.XmlAttribute("Green")]
        public static readonly Color Green = Color.Parse("#008000");

        [System.Xml.Serialization.XmlAttribute("Lime")]
        public static readonly Color Lime = Color.Parse("#00FF00");

        [System.Xml.Serialization.XmlAttribute("Maroon")]
        public static readonly Color Maroon = Color.Parse("#800000");

        [System.Xml.Serialization.XmlAttribute("Navy")]
        public static readonly Color Navy = Color.Parse("#000080");

        [System.Xml.Serialization.XmlAttribute("Olive")]
        public static readonly Color Olive = Color.Parse("#808000");

        [System.Xml.Serialization.XmlAttribute("Purple")]
        public static readonly Color Purple = Color.Parse("#800080");

        [System.Xml.Serialization.XmlAttribute("Red")]
        public static readonly Color Red = Color.Parse("#FF0000");

        [System.Xml.Serialization.XmlAttribute("Silver")]
        public static readonly Color Silver = Color.Parse("#C0C0C0");

        [System.Xml.Serialization.XmlAttribute("Teal")]
        public static readonly Color Teal = Color.Parse("#008080");

        [System.Xml.Serialization.XmlAttribute("White")]
        public static readonly Color White = Color.Parse("#FFFFFF");

        [System.Xml.Serialization.XmlAttribute("Yellow")]
        public static readonly Color Yellow = Color.Parse("#FFFF00");

        // Light color variants
        [System.Xml.Serialization.XmlAttribute("LightBlue")]
        public static readonly Color LightBlue = Color.Parse("#ADD8E6");

        [System.Xml.Serialization.XmlAttribute("LightGreen")]
        public static readonly Color LightGreen = Color.Parse("#90EE90");

        [System.Xml.Serialization.XmlAttribute("LightYellow")]
        public static readonly Color LightYellow = Color.Parse("#FFFFE0");

        [System.Xml.Serialization.XmlAttribute("LightCoral")]
        public static readonly Color LightCoral = Color.Parse("#F08080");

        [System.Xml.Serialization.XmlAttribute("LightCyan")]
        public static readonly Color LightCyan = Color.Parse("#E0FFFF");

        [System.Xml.Serialization.XmlAttribute("LightGray")]
        public static readonly Color LightGray = Color.Parse("#D3D3D3");

        [System.Xml.Serialization.XmlAttribute("LightSalmon")]
        public static readonly Color LightSalmon = Color.Parse("#FFA07A");

        [System.Xml.Serialization.XmlAttribute("LightSkyBlue")]
        public static readonly Color LightSkyBlue = Color.Parse("#87CEFA");

        [System.Xml.Serialization.XmlAttribute("LightSteelBlue")]
        public static readonly Color LightSteelBlue = Color.Parse("#B0C4DE");

        [System.Xml.Serialization.XmlAttribute("Transparent")]
        public static readonly Color Transparent = new Color();

        private static Dictionary<string, Color> _named;

        static StandardColors()
        {
            _named = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);
            LoadColorsFromFields(_named);
        }

        private static void LoadColorsFromFields(Dictionary<string, Color> hash)
        {
            System.Reflection.FieldInfo[] fields = typeof(StandardColors).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (System.Reflection.FieldInfo fi in fields)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(System.Xml.Serialization.XmlAttributeAttribute), false);
                if (null != attrs && attrs.Length > 0)
                {
                    System.Xml.Serialization.XmlAttributeAttribute attr = (System.Xml.Serialization.XmlAttributeAttribute)attrs[0];
                    string name = attr.AttributeName;
                    if (string.IsNullOrEmpty(name))
                        name = fi.Name;
                    object color = fi.GetValue(null);
                    if (null != color && color is Color)
                    {
                        hash.Add(name, (Color)color);
                    }
                }
            }
        }

        public static bool IsDefinedName(string name)
        {
            Color c;
            return TryGetColorFromName(name, out c);
        }


        public static bool TryGetColorFromName(string name, out Color color)
        {
            color = Color.Transparent;
            bool known = false;

            if (string.IsNullOrEmpty(name))
                return false;

            string lower = name.ToLower();
            switch (lower)
            {
                case ("aqua"):
                    color = StandardColors.Aqua;
                    known = true;
                    break;
                case ("black"):
                    color = StandardColors.Black;
                    known = true;
                    break;
                case ("blue"):
                    color = StandardColors.Blue;
                    known = true;
                    break;
                case ("fuchsia"):
                    color = StandardColors.Fuchsia;
                    known = true;
                    break;
                case ("gray"):
                    color = StandardColors.Gray;
                    known = true;
                    break;
                case ("green"):
                    color = StandardColors.Green;
                    known = true;
                    break;
                case ("lime"):
                    color = StandardColors.Lime;
                    known = true;
                    break;
                case ("maroon"):
                    color = StandardColors.Maroon;
                    known = true;
                    break;
                case ("navy"):
                    color = StandardColors.Navy;
                    known = true;
                    break;
                case ("olive"):
                    color = StandardColors.Olive;
                    known = true;
                    break;
                case ("purple"):
                    color = StandardColors.Purple;
                    known = true;
                    break;
                case ("red"):
                    color = StandardColors.Red;
                    known = true;
                    break;
                case ("silver"):
                    color = StandardColors.Silver;
                    known = true;
                    break;
                case ("teal"):
                    color = StandardColors.Teal;
                    known = true;
                    break;
                case ("white"):
                    color = StandardColors.White;
                    known = true;
                    break;
                case ("yellow"):
                    color = StandardColors.Yellow;
                    known = true;
                    break;
                case ("lightblue"):
                    color = StandardColors.LightBlue;
                    known = true;
                    break;
                case ("lightgreen"):
                    color = StandardColors.LightGreen;
                    known = true;
                    break;
                case ("lightyellow"):
                    color = StandardColors.LightYellow;
                    known = true;
                    break;
                case ("lightcoral"):
                    color = StandardColors.LightCoral;
                    known = true;
                    break;
                case ("lightcyan"):
                    color = StandardColors.LightCyan;
                    known = true;
                    break;
                case ("lightgray"):
                    color = StandardColors.LightGray;
                    known = true;
                    break;
                case ("lightsalmon"):
                    color = StandardColors.LightSalmon;
                    known = true;
                    break;
                case ("lightskyblue"):
                    color = StandardColors.LightSkyBlue;
                    known = true;
                    break;
                case ("lightsteelblue"):
                    color = StandardColors.LightSteelBlue;
                    known = true;
                    break;
                case ("transparent"):
                    color = StandardColors.Transparent;
                    known = true;
                    break;
                default:
                    break;
            }
            return known;
        }

        public static Color FromName(string name)
        {
            Color c;
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");


            if (!TryGetColorFromName(name, out c))
                throw new ArgumentOutOfRangeException("name");
            
            return c;
        }
    }

    [Obsolete("Please use the Scryber.Drawing.StandardColors class, rather than the Scryber.Drawing.PDFColors", true)]
    public class PDFColors : StandardColors
    {
        
    }
}
