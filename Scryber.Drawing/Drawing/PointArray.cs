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
    public class PointArray : IEnumerable<Point>
    {
        private List<Point> _items;

        public Point this[int index]
        {
            get { return this._items[index]; }
            set { this._items[index] = value; }
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public PointArray()
        {
            _items = new List<Point>();
        }

        public PointArray(params Unit[] xys)
            : this()
        {
            if (xys.Length % 2 != 0)
                throw new ArgumentOutOfRangeException("xys");
            for (int i = 0; i < xys.Length; i+= 2)
            {
                Unit x = xys[i];
                Unit y = xys[i + 1];
                this.Add(new Point(x, y));
            }
        }

        public PointArray(params Point[] items)
            : this((IEnumerable<Point>)items)
        {
        }

        public PointArray(IEnumerable<Point> items)
            : this()
        {
            this.AddRange(items);
        }


        public void Add(Point point)
        {
            this._items.Add(point);
        }


        public void Add(Unit x, Unit y)
        {
            this.Add(new Point(x, y));
        }

        public void AddRange(IEnumerable<Point> items)
        {
            if (null != items)
            {
                foreach (Point pt in items)
                {
                    this.Add(pt);
                }
            }
        }

        public bool Remove(Point pt)
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

        public Point[] ToArray()
        {
            return this._items.ToArray();
        }

        #region IEnumerable

        public IEnumerator<Point> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        #endregion

        private const char Separator = ' ';

        public static PointArray Parse(string value)
        {
            if (string.IsNullOrEmpty(value) == false)
            {
                PointArray all = new PointArray();
                //TODO: Init with capacity
                try
                {
                    string[] pts = value.Split(Separator);
                    foreach (string one in pts)
                    {
                        Point parsed = Point.Parse(one);
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
                return new PointArray();

        }
    }

}
