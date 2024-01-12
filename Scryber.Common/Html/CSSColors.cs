﻿using System;
using System.Collections.Generic;

namespace Scryber.Html
{
    /// <summary>
    /// A complete list of all the css colors
    /// </summary>
    public static class CSSColors
    {
        private static IDictionary<string, string> _name2color;

        public static IDictionary<string, string> Names2Colors
        {
            get { return _name2color; }
        }

        static CSSColors()
        {

            _name2color = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _name2color.Add("Black", "#000000");
            _name2color.Add("Navy", "#000080");
            _name2color.Add("DarkBlue", "#00008B");
            _name2color.Add("MediumBlue", "#0000CD");
            _name2color.Add("Blue", "#0000FF");
            _name2color.Add("DarkGreen", "#006400");
            _name2color.Add("Green", "#008000");
            _name2color.Add("Teal", "#008080");
            _name2color.Add("DarkCyan", "#008B8B");
            _name2color.Add("DeepSkyBlue", "#00BFFF");
            _name2color.Add("DarkTurquoise", "#00CED1");
            _name2color.Add("MediumSpringGreen", "#00FA9A");
            _name2color.Add("Lime", "#00FF00");
            _name2color.Add("SpringGreen", "#00FF7F");
            _name2color.Add("Aqua", "#00FFFF");
            _name2color.Add("Cyan", "#00FFFF");
            _name2color.Add("MidnightBlue", "#191970");
            _name2color.Add("DodgerBlue", "#1E90FF");
            _name2color.Add("LightSeaGreen", "#20B2AA");
            _name2color.Add("ForestGreen", "#228B22");
            _name2color.Add("SeaGreen", "#2E8B57");
            _name2color.Add("DarkSlateGray", "#2F4F4F");
            _name2color.Add("LimeGreen", "#32CD32");
            _name2color.Add("MediumSeaGreen", "#3CB371");
            _name2color.Add("Turquoise", "#40E0D0");
            _name2color.Add("RoyalBlue", "#4169E1");
            _name2color.Add("SteelBlue", "#4682B4");
            _name2color.Add("DarkSlateBlue", "#483D8B");
            _name2color.Add("MediumTurquoise", "#48D1CC");
            _name2color.Add("Indigo ", "#4B0082");
            _name2color.Add("DarkOliveGreen", "#556B2F");
            _name2color.Add("CadetBlue", "#5F9EA0");
            _name2color.Add("CornflowerBlue", "#6495ED");
            _name2color.Add("RebeccaPurple", "#663399");
            _name2color.Add("MediumAquaMarine", "#66CDAA");
            _name2color.Add("DimGray", "#696969");
            _name2color.Add("DimGrey", "#696969");
            _name2color.Add("SlateBlue", "#6A5ACD");
            _name2color.Add("OliveDrab", "#6B8E23");
            _name2color.Add("SlateGray", "#708090");
            _name2color.Add("SlateGrey", "#708090");
            _name2color.Add("LightSlateGray", "#778899");
            _name2color.Add("LightSlateGrey", "#778899");
            _name2color.Add("MediumSlateBlue", "#7B68EE");
            _name2color.Add("LawnGreen", "#7CFC00");
            _name2color.Add("Chartreuse", "#7FFF00");
            _name2color.Add("Aquamarine", "#7FFFD4");
            _name2color.Add("Maroon", "#800000");
            _name2color.Add("Purple", "#800080");
            _name2color.Add("Olive", "#808000");
            _name2color.Add("Gray", "#808080");
            _name2color.Add("Grey", "#808080");
            _name2color.Add("SkyBlue", "#87CEEB");
            _name2color.Add("LightSkyBlue", "#87CEFA");
            _name2color.Add("BlueViolet", "#8A2BE2");
            _name2color.Add("DarkRed", "#8B0000");
            _name2color.Add("DarkMagenta", "#8B008B");
            _name2color.Add("SaddleBrown", "#8B4513");
            _name2color.Add("DarkSeaGreen", "#8FBC8F");
            _name2color.Add("LightGreen", "#90EE90");
            _name2color.Add("MediumPurple", "#9370DB");
            _name2color.Add("DarkViolet", "#9400D3");
            _name2color.Add("PaleGreen", "#98FB98");
            _name2color.Add("DarkOrchid", "#9932CC");
            _name2color.Add("YellowGreen", "#9ACD32");
            _name2color.Add("Sienna", "#A0522D");
            _name2color.Add("Brown", "#A52A2A");
            _name2color.Add("DarkGray", "#A9A9A9");
            _name2color.Add("DarkGrey", "#A9A9A9");
            _name2color.Add("LightBlue", "#ADD8E6");
            _name2color.Add("GreenYellow", "#ADFF2F");
            _name2color.Add("PaleTurquoise", "#AFEEEE");
            _name2color.Add("LightSteelBlue", "#B0C4DE");
            _name2color.Add("PowderBlue", "#B0E0E6");
            _name2color.Add("FireBrick", "#B22222");
            _name2color.Add("DarkGoldenRod", "#B8860B");
            _name2color.Add("MediumOrchid", "#BA55D3");
            _name2color.Add("RosyBrown", "#BC8F8F");
            _name2color.Add("DarkKhaki", "#BDB76B");
            _name2color.Add("Silver", "#C0C0C0");
            _name2color.Add("MediumVioletRed", "#C71585");
            _name2color.Add("IndianRed ", "#CD5C5C");
            _name2color.Add("Peru", "#CD853F");
            _name2color.Add("Chocolate", "#D2691E");
            _name2color.Add("Tan", "#D2B48C");
            _name2color.Add("LightGray", "#D3D3D3");
            _name2color.Add("LightGrey", "#D3D3D3");
            _name2color.Add("Thistle", "#D8BFD8");
            _name2color.Add("Orchid", "#DA70D6");
            _name2color.Add("GoldenRod", "#DAA520");
            _name2color.Add("PaleVioletRed", "#DB7093");
            _name2color.Add("Crimson", "#DC143C");
            _name2color.Add("Gainsboro", "#DCDCDC");
            _name2color.Add("Plum", "#DDA0DD");
            _name2color.Add("BurlyWood", "#DEB887");
            _name2color.Add("LightCyan", "#E0FFFF");
            _name2color.Add("Lavender", "#E6E6FA");
            _name2color.Add("DarkSalmon", "#E9967A");
            _name2color.Add("Violet", "#EE82EE");
            _name2color.Add("PaleGoldenRod", "#EEE8AA");
            _name2color.Add("LightCoral", "#F08080");
            _name2color.Add("Khaki", "#F0E68C");
            _name2color.Add("AliceBlue", "#F0F8FF");
            _name2color.Add("HoneyDew", "#F0FFF0");
            _name2color.Add("Azure", "#F0FFFF");
            _name2color.Add("SandyBrown", "#F4A460");
            _name2color.Add("Wheat", "#F5DEB3");
            _name2color.Add("Beige", "#F5F5DC");
            _name2color.Add("WhiteSmoke", "#F5F5F5");
            _name2color.Add("MintCream", "#F5FFFA");
            _name2color.Add("GhostWhite", "#F8F8FF");
            _name2color.Add("Salmon", "#FA8072");
            _name2color.Add("AntiqueWhite", "#FAEBD7");
            _name2color.Add("Linen", "#FAF0E6");
            _name2color.Add("LightGoldenRodYellow", "#FAFAD2");
            _name2color.Add("OldLace", "#FDF5E6");
            _name2color.Add("Red", "#FF0000");
            _name2color.Add("Fuchsia", "#FF00FF");
            _name2color.Add("Magenta", "#FF00FF");
            _name2color.Add("DeepPink", "#FF1493");
            _name2color.Add("OrangeRed", "#FF4500");
            _name2color.Add("Tomato", "#FF6347");
            _name2color.Add("HotPink", "#FF69B4");
            _name2color.Add("Coral", "#FF7F50");
            _name2color.Add("DarkOrange", "#FF8C00");
            _name2color.Add("LightSalmon", "#FFA07A");
            _name2color.Add("Orange", "#FFA500");
            _name2color.Add("LightPink", "#FFB6C1");
            _name2color.Add("Pink", "#FFC0CB");
            _name2color.Add("Gold", "#FFD700");
            _name2color.Add("PeachPuff", "#FFDAB9");
            _name2color.Add("NavajoWhite", "#FFDEAD");
            _name2color.Add("Moccasin", "#FFE4B5");
            _name2color.Add("Bisque", "#FFE4C4");
            _name2color.Add("MistyRose", "#FFE4E1");
            _name2color.Add("BlanchedAlmond", "#FFEBCD");
            _name2color.Add("PapayaWhip", "#FFEFD5");
            _name2color.Add("LavenderBlush", "#FFF0F5");
            _name2color.Add("SeaShell", "#FFF5EE");
            _name2color.Add("Cornsilk", "#FFF8DC");
            _name2color.Add("LemonChiffon", "#FFFACD");
            _name2color.Add("FloralWhite", "#FFFAF0");
            _name2color.Add("Snow", "#FFFAFA");
            _name2color.Add("Yellow", "#FFFF00");
            _name2color.Add("LightYellow", "#FFFFE0");
            _name2color.Add("Ivory", "#FFFFF0");
            _name2color.Add("White", "#FFFFFF");
            _name2color.Add("mark", "#FFFF00");

            //Convert to readonly
            _name2color = new ReadOnlyDictionary<string, string>(_name2color);
        }
    }
}

