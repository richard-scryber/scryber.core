using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

namespace Scryber.Drawing
{
    public class GradientLinearDescriptor : GradientDescriptor
    {

        public double Angle { get; set; }

        public GradientLinearDescriptor() : base(GradientType.Linear)
        { }

        


        //
        // static parse linear methods
        //

        #region public static bool TryParseLinear(string value, out PDFGradientLinearDescriptor linear)

        private static Regex _splitter = new Regex(",(?![^\\(]*\\))");

        /// <summary>
        /// Parses a linear gradient from a string without decorations e.g. "to top right, red, green
        /// </summary>
        /// <param name="value"></param>
        /// <param name="linear"></param>
        /// <returns></returns>
        public static bool TryParseLinear(string value, out GradientLinearDescriptor linear)
        {
            linear = null;
            string[] all = _splitter.Split(value);
            if (all.Length == 0)
                return false;

            int colorStopIndex = 0;
            double angle;

            if (all[0].StartsWith("to "))
            {
                var ga = all[0].Substring(3).Trim().Replace(" ", "_");
                GradientAngle parsed;
                if (Enum.TryParse(ga, true, out parsed))
                    angle = (double)parsed;
                else
                    return false;
                colorStopIndex = 1;
            }
            else if (char.IsNumber(all[0], 0))
            {
                var deg = all[0];

                if (deg.EndsWith("deg"))
                    deg = deg.Substring(0, deg.Length - 3);

                if (!double.TryParse(deg, out angle))
                    return false;

                colorStopIndex = 1;
            }
            else
                angle = (double)GradientAngle.Bottom;

            GradientColor[] colors = new GradientColor[all.Length - colorStopIndex];

            for (int i = 0; i < colors.Length; i++)
            {
                GradientColor parsed;
                if (GradientColor.TryParse(all[i + colorStopIndex], out parsed))
                    colors[i] = parsed;
                else
                    return false;
            }

            linear = new GradientLinearDescriptor() { Angle = angle, Colors = new List<GradientColor>(colors) };
            return true;
        }

        #endregion

    }
}
