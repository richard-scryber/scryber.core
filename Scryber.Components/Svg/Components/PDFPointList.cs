using System;
using Scryber.Drawing;
using System.Collections.Generic;
using System.Collections;

namespace Scryber.Svg.Components
{
    [PDFParsableValue]
    public class PDFPointList : ICollection<PDFPoint>
    {
        private List<PDFPoint> _points;

        public PDFPointList()
        {
            _points = new List<PDFPoint>();
        }

        public int Count
        {
            get { return this._points.Count; }
        }

        public bool IsReadOnly { get { return false; } }



        public PDFPoint this[int index]
        {
            get { return this._points[index]; }
        }

        public void Add(PDFPoint item)
        {
            this._points.Add(item);
        }

        public void Clear()
        {
            this._points.Clear();
        }

        public bool Contains(PDFPoint item)
        {
            return this._points.Contains(item);
        }

        public void CopyTo(PDFPoint[] array, int arrayIndex)
        {
            this._points.CopyTo(array, arrayIndex);
        }

        public IEnumerator<PDFPoint> GetEnumerator()
        {
            return this._points.GetEnumerator();
        }

        public bool Remove(PDFPoint item)
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
                PDFUnit one, two;

                if (PDFUnit.TryParse(vals[i - 1], out one) && PDFUnit.TryParse(vals[i], out two))
                    all.Add(new PDFPoint(one, two));
            }

            return all;
        }
    }
}
