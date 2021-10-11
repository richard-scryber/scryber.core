using System;
using Scryber.Drawing;
using System.Collections.Generic;
using System.Collections;

namespace Scryber.Svg.Components
{
    [PDFParsableValue]
    public class PDFPointList : ICollection<Point>
    {
        private List<Point> _points;

        public PDFPointList()
        {
            _points = new List<Point>();
        }

        public int Count
        {
            get { return this._points.Count; }
        }

        public bool IsReadOnly { get { return false; } }



        public Point this[int index]
        {
            get { return this._points[index]; }
        }

        public void Add(Point item)
        {
            this._points.Add(item);
        }

        public void Clear()
        {
            this._points.Clear();
        }

        public bool Contains(Point item)
        {
            return this._points.Contains(item);
        }

        public void CopyTo(Point[] array, int arrayIndex)
        {
            this._points.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return this._points.GetEnumerator();
        }

        public bool Remove(Point item)
        {
            return this._points.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private static char[] Separators = new char[] { ',', ' ' };

        public static PDFPointList Parse(string value)
        {
            PDFPointList all = new PDFPointList();

            if (string.IsNullOrEmpty(value))
                return all;

            var vals = value.Split(Separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            for (int i = 1; i < vals.Length; i+= 2)
            {
                Unit one, two;

                if (Unit.TryParse(vals[i - 1], out one) && Unit.TryParse(vals[i], out two))
                    all.Add(new Point(one, two));
            }

            return all;
        }
    }
}
