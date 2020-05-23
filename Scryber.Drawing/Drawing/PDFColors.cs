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
    public static class PDFColors
    {

        [System.Xml.Serialization.XmlAttribute("Aqua")]
        public static readonly PDFColor Aqua = PDFColor.Parse("#00FFFF");

        [System.Xml.Serialization.XmlAttribute("Black")]
        public static readonly PDFColor Black = PDFColor.Parse("#000000");

        [System.Xml.Serialization.XmlAttribute("Blue")]
        public static readonly PDFColor Blue = PDFColor.Parse("#0000FF");

        [System.Xml.Serialization.XmlAttribute("Fuchisa")]
        public static readonly PDFColor Fuchsia = PDFColor.Parse("#FF00FF");

        [System.Xml.Serialization.XmlAttribute("Gray")]
        public static readonly PDFColor Gray = PDFColor.Parse("#808080");

        
        [System.Xml.Serialization.XmlAttribute("Green")]
        public static readonly PDFColor Green = PDFColor.Parse("#008000");

        [System.Xml.Serialization.XmlAttribute("Lime")]
        public static readonly PDFColor Lime = PDFColor.Parse("#00FF00");

        [System.Xml.Serialization.XmlAttribute("Maroon")]
        public static readonly PDFColor Maroon = PDFColor.Parse("#800000");

        [System.Xml.Serialization.XmlAttribute("Navy")]
        public static readonly PDFColor Navy = PDFColor.Parse("#000080");

        [System.Xml.Serialization.XmlAttribute("Olive")]
        public static readonly PDFColor Olive = PDFColor.Parse("#808000");

        [System.Xml.Serialization.XmlAttribute("Purple")]
        public static readonly PDFColor Purple = PDFColor.Parse("#800080");

        [System.Xml.Serialization.XmlAttribute("Red")]
        public static readonly PDFColor Red = PDFColor.Parse("#FF0000");

        [System.Xml.Serialization.XmlAttribute("Silver")]
        public static readonly PDFColor Silver = PDFColor.Parse("#C0C0C0");

        [System.Xml.Serialization.XmlAttribute("Teal")]
        public static readonly PDFColor Teal = PDFColor.Parse("#008080");

        [System.Xml.Serialization.XmlAttribute("White")]
        public static readonly PDFColor White = PDFColor.Parse("#FFFFFF");

        [System.Xml.Serialization.XmlAttribute("Yellow")]
        public static readonly PDFColor Yellow = PDFColor.Parse("#FFFF00");

        [System.Xml.Serialization.XmlAttribute("Transparent")]
        public static readonly PDFColor Transparent = new PDFColor(ColorSpace.RGB, System.Drawing.Color.Transparent);

        private static Dictionary<string, PDFColor> _named;

        static PDFColors()
        {
            _named = new Dictionary<string, PDFColor>(StringComparer.OrdinalIgnoreCase);
            LoadColorsFromFields(_named);
        }

        private static void LoadColorsFromFields(Dictionary<string, PDFColor> hash)
        {
            System.Reflection.FieldInfo[] fields = typeof(PDFColors).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
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
                    if (null != color && color is PDFColor)
                    {
                        hash.Add(name, (PDFColor)color);
                    }
                }
            }
        }

        public static bool IsDefinedName(string name)
        {
            PDFColor c;
            return TryGetColorFromName(name, out c);
        }


        public static bool TryGetColorFromName(string name, out PDFColor color)
        {
            color = null;

            if (string.IsNullOrEmpty(name))
                return false;

            string lower = name.ToLower();
            switch (lower)
            {
                case ("aqua"):
                    color = PDFColors.Aqua;
                    break;
                case ("black"):
                    color = PDFColors.Black;
                    break;
                case ("blue"):
                    color = PDFColors.Blue;
                    break;
                case ("fuchsia"):
                    color = PDFColors.Fuchsia;
                    break;
                case ("gray"):
                    color = PDFColors.Gray;
                    break;
                case ("green"):
                    color = PDFColors.Green;
                    break;
                case ("lime"):
                    color = PDFColors.Lime;
                    break;
                case ("maroon"):
                    color = PDFColors.Maroon;
                    break;
                case ("navy"):
                    color = PDFColors.Navy;
                    break;
                case ("olive"):
                    color = PDFColors.Olive;
                    break;
                case ("purple"):
                    color = PDFColors.Purple;
                    break;
                case ("red"):
                    color = PDFColors.Red;
                    break;
                case ("silver"):
                    color = PDFColors.Silver;
                    break;
                case ("teal"):
                    color = PDFColors.Teal;
                    break;
                case ("white"):
                    color = PDFColors.White;
                    break;
                case ("yellow"):
                    color = PDFColors.Yellow;
                    break;
                case ("transparent"):
                    color = PDFColors.Transparent;
                    break;
                default:
                    break;
            }
            return null != color;
        }

        public static PDFColor FromName(string name)
        {
            PDFColor c;
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");


            if (!TryGetColorFromName(name, out c))
                throw new ArgumentOutOfRangeException("name");
            
            return c;
        }
    }
}
