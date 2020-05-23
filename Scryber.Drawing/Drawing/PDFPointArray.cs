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
using System.Linq;
using System.Text;

namespace Scryber.Drawing
{
    /// <summary>
    /// A mutable list of points that can enumerated over, and can be parsed from a string in the format 'x1 y1, x2 y2, x3...'
    /// </summary>
    [PDFParsableValue()]
    public class PDFPointArray : IEnumerable<PDFPoint>
    {
        private List<PDFPoint> _items;

        public PDFPoint this[int index]
        {
            get { return this._items[index]; }
            set { this._items[index] = value; }
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public PDFPointArray()
        {
            _items = new List<PDFPoint>();
        }

        public PDFPointArray(params PDFUnit[] xys)
            : this()
        {
            if (xys.Length % 2 != 0)
                throw new ArgumentOutOfRangeException("xys");
            for (int i = 0; i < xys.Length; i+= 2)
            {
                PDFUnit x = xys[i];
                PDFUnit y = xys[i + 1];
                this.Add(new PDFPoint(x, y));
            }
        }

        public PDFPointArray(params PDFPoint[] items)
            : this((IEnumerable<PDFPoint>)items)
        {
        }

        public PDFPointArray(IEnumerable<PDFPoint> items)
            : this()
        {
            this.AddRange(items);
        }


        public void Add(PDFPoint point)
        {
            this._items.Add(point);
        }


        public void Add(PDFUnit x, PDFUnit y)
        {
            this.Add(new PDFPoint(x, y));
        }

        public void AddRange(IEnumerable<PDFPoint> items)
        {
            if (null != items)
            {
                foreach (PDFPoint pt in items)
                {
                    this.Add(pt);
                }
            }
        }

        public bool Remove(PDFPoint pt)
        {
            return this._items.Remove(pt);
        }

        public void RemoveAt(int index)
        {
            this._items.RemoveAt(index);
        }

        public void Clear()
        {
            this._items.Clear();
        }

        public PDFPoint[] ToArray()
        {
            return this._items.ToArray();
        }

        #region IEnumerable

        public IEnumerator<PDFPoint> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        #endregion

        private const char Separator = ' ';

        public static PDFPointArray Parse(string value)
        {
            if (string.IsNullOrEmpty(value) == false)
            {
                PDFPointArray all = new PDFPointArray();
                //TODO: Init with capacity
                try
                {
                    string[] pts = value.Split(Separator);
                    foreach (string one in pts)
                    {
                        PDFPoint parsed = PDFPoint.Parse(one);
                        all.Add(parsed);
                    }
                }
                catch (Exception ex)
                {
                    string msg = string.Format(Errors.CouldNotParseValue_3, value, "PDFPointArray", "x1,y1 x2,y2 x3,y3...");
                    throw new PDFException(msg, ex);
                }

                return all;
            }
            else
                return new PDFPointArray();

        }
    }

}
