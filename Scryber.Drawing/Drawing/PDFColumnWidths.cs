using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Drawing
{
    [PDFParsableValue()]
    public struct PDFColumnWidths
    {
        public const double UndefinedWidth = 0.0;
        private static readonly double[] _emptyWidths = new double[] { };
        private static readonly char[] _splitChars = new char[] { ' ' };
        private double[] _widths;

        /// <summary>
        /// Gets the values associated with this set of column widths
        /// </summary>
        public double[] Widths
        {
            get { return _widths; }
        }

        public bool IsEmpty
        {
            get
            {
                if (null == _widths || _widths.Length == 0)
                    return true;
                else
                    return false;
            }
        }

        public PDFColumnWidths(double[] widths)
        {
            this._widths = widths;
        }

        public override string ToString()
        {
            if (null == this._widths || this._widths.Length == 0)
                return string.Empty;
            else
                return string.Join(" ", _widths);
        }

        public static PDFColumnWidths Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return PDFColumnWidths.Empty;

            string[] all = value.Split(_splitChars, StringSplitOptions.RemoveEmptyEntries);
            double[] parsed = new double[all.Length];
            double sum = 0;
            for(var i = 0; i < all.Length; i++)
            {
                double one;
                if (all[i] == "*")
                    one = UndefinedWidth;
                else
                    one = double.Parse(all[i]);

                sum += one;
                if (sum > 1.0)
                    throw new ArgumentOutOfRangeException("value", "The widths of the columns is a percentage number that must not exceed 1.");

                parsed[i] = one;
            }
            
            return new PDFColumnWidths(parsed);
        }

        public static PDFColumnWidths Empty
        {
            get { return new PDFColumnWidths(_emptyWidths); }
        }


    }
}
