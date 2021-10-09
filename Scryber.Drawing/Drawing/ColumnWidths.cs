using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Logging;

namespace Scryber.Drawing
{
    [PDFParsableValue()]
    public struct ColumnWidths
    {
        public const double UndefinedWidth = 0.0;
        private static readonly double[] _emptyWidths = new double[] { };
        private static readonly char[] _splitChars = new char[] { ' ' };

        private double[] _widths;
        private PDFUnit _explicit;

        /// <summary>
        /// Gets the values associated with this set of column widths
        /// </summary>
        public double[] Widths
        {
            get { return _widths; }
        }

        public PDFUnit Explicit
        {
            get { return _explicit; }
        }

        public bool IsEmpty
        {
            get
            {
                if (null == _widths || _widths.Length == 0)
                    return this._explicit.IsEmpty;
                else
                    return false;
            }
        }

        public bool HasExplicitWidth
        {
            get
            {
                return !this._explicit.IsEmpty;
            }
        }

        public ColumnWidths(PDFUnit explicitWidth)
        {
            this._widths = null;
            this._explicit = explicitWidth;
        }

        public ColumnWidths(double[] widths)
        {
            this._widths = widths;
            this._explicit = PDFUnit.Empty;
        }

        public PDFUnit[] GetPercentColumnWidths(PDFUnit available, PDFUnit alley, int colCount)
        {
            
            if (null == this.Widths || this.Widths.Length == 0)
                return GetEqualColumnWidths(available, alley, colCount);

            //Validate the widths are not greater than 1;
            double total = 0.0;
            for (var i = 0; i < this.Widths.Length; i++)
                total += this.Widths[i];
            if (total > 1)
                throw new ArgumentOutOfRangeException("The total percentage column widths are greater than 100%");


            PDFUnit[] all = new PDFUnit[colCount];
            total = available.PointsValue;
            double totalAlley = alley.PointsValue * (colCount - 1);
            double totalAvail = total - totalAlley;

            double remainderAvail = totalAvail;
            int remainerCount = 0;
            

            double[] widths = this.Widths;
            PDFUnit[] calc = new PDFUnit[colCount];
            
            for (int i = 0; i < colCount; i++)
            {
                if(i >= widths.Length || widths[i] <= UndefinedWidth)
                {
                    //don't have a value so set to zero and increment the remainder counter
                    calc[i] = PDFUnit.Zero;
                    remainerCount++;
                }
                else 
                {
                    //do have a value so set the width and take away from the remainder total
                    calc[i] = totalAvail * widths[i];
                    remainderAvail -= calc[i].PointsValue;
                }
                
            }

            if(remainerCount > 0)
            {
                double each = remainderAvail / remainerCount;

                for(var i = 0; i < colCount; i++)
                {
                    if (calc[i] == PDFUnit.Zero)
                        calc[i] = new PDFUnit(each, PageUnits.Points);
                }
            }

            return calc;
        }

        public PDFUnit[] GetExplicitColumnWidths(PDFUnit available, PDFUnit alley, out int count)
        {
            if (!this.HasExplicitWidth)
                throw new InvalidOperationException("The column widths does not have an explicit value");

            //If the available width is 650 and the alley is 10 and the explict width is 150
            //Then the max avaialbe will be
            // 150+10 for a column (knowing we have an extra) = 160;
            // Math.Floor(650/160) = 4;
            // The the available = 650 - (alley * (4 -1)) = 620;
            // Actual width = 620 / 4 = 155;

            PDFUnit expl = this.Explicit + alley;
            int maxCount = (int)Math.Floor(available.PointsValue / expl.PointsValue);
            if(maxCount < 2)
            {
                count = 1;
                return new PDFUnit[] { available };
            }

            double totalAlley = alley.PointsValue * (maxCount - 1);
            double maxWidth = available.PointsValue - totalAlley;
            double actual = maxWidth / maxCount;

            PDFUnit[] all = new PDFUnit[(int)maxCount];
            for (var i = 0; i < maxCount; i++)
                all[i] = actual;

            count = maxCount;
            return all;
        }

        public override string ToString()
        {
            if (this.HasExplicitWidth)
                return "[" + this.Explicit.ToString() + "]";

            if (null == this._widths || this._widths.Length == 0)
                return string.Empty;
            else
                return "[" + string.Join(" ", _widths) + "]";
        }

        public static PDFUnit[] GetEqualColumnWidths(PDFUnit available, PDFUnit alley, int colCount)
        {
            double total = available.PointsValue;
            total -= (alley.PointsValue * (colCount - 1));
            double each = total / colCount;

            PDFUnit[] all = new PDFUnit[colCount];
            for (var i = 0; i < colCount; i++)
                all[i] = new PDFUnit(each, PageUnits.Points);

            return all;
        }

        public static ColumnWidths Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return ColumnWidths.Empty;

            string[] all = value.Split(_splitChars, StringSplitOptions.RemoveEmptyEntries);
            if(all.Length == 1)
            {
                //check to see if it is an explicit width for all columns (e.g. 200pt)
                var expl = all[0];
                if(char.IsLetter(expl, expl.Length - 1))
                {
                    PDFUnit val;
                    if (PDFUnit.TryParse(expl, out val))
                        return new ColumnWidths(val);
                    else
                        throw new ArgumentException("The value '" + value + "' could not be converted to column widths. Either use an explicit width (e.g. 200pt) or a set of percentage widths (e.g. 30% 40% 30%, or 0.3 0.4 0.3) ", "value");
                }
            }

            double[] parsed = new double[all.Length];
            double sum = 0;
            for(var i = 0; i < all.Length; i++)
            {
                double one;
                if (all[i] == "*")
                    one = UndefinedWidth;
                else if(all[i].EndsWith("%"))
                {
                    one = double.Parse(all[i].Substring(0, all[i].Length - 1));
                    one = one / 100.0;
                }
                else
                    one = double.Parse(all[i]);

                sum += one;
                if (sum > 1.0)
                    throw new ArgumentOutOfRangeException("value", "The widths of the columns is a percentage number that must not exceed 1.");

                parsed[i] = one;
            }
            
            return new ColumnWidths(parsed);
        }

        public static ColumnWidths Empty
        {
            get { return new ColumnWidths(_emptyWidths); }
        }


    }
}
