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
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Components
{
    [PDFParsableComponent("Poly")]
    public class Polygon : PolygonBase
    {
        private PDFPointArray _pts;


        [PDFAttribute("vertex-count", Scryber.Styles.Style.PDFStylesNamespace)]
        public int VertexCount
        {
            get { return this.Style.Shape.VertexCount; }
            set { this.Style.Shape.VertexCount = value; }
        }


        [PDFAttribute("vertex-step", Scryber.Styles.Style.PDFStylesNamespace)]
        public int VertexStep
        {
            get { return this.Style.Shape.VertexStep; }
            set { this.Style.Shape.VertexStep = value; }
        }

        [PDFAttribute("points")]
        public PDFPointArray Points
        {
            get
            {
                if (null == _pts)
                    _pts = new PDFPointArray();
                return _pts;
            }
            set
            {
                this._pts = value;
            }
        }

        /// <summary>
        /// Returns true if there are points explicitly set on the poly
        /// </summary>
        public bool HasPoints
        {
            get { return null != _pts && _pts.Count > 0; }
        }


        public Polygon()
            : this(PDFObjectTypes.ShapePolygon)
        {
        }

        protected Polygon(PDFObjectType type)
            : base(type)
        {
        }



        protected override Drawing.PDFPoint[] GetPoints(Drawing.PDFRect bounds, Style style)
        {
            if (this.HasPoints)
            {
                return this.Points.ToArray();
            }
            else
                return base.GetPoints(bounds, style);
        }

        protected override void BuildPath(PDFGraphicsPath path, PDFPoint[] points, Style style, bool end)
        {
            int vertexstep = style.GetValue(StyleKeys.ShapeVertexStepKey, 1);
            bool closed = style.GetValue(StyleKeys.ShapeClosedKey, true);

            if (this.HasPoints == false && vertexstep > 1)
                this.BuildPolygramPath(path, points, vertexstep, closed, end);
            else
                base.BuildPath(path, points, style, end);
        }

        protected virtual void BuildPolygramPath(PDFGraphicsPath path, PDFPoint[] points, int step, bool closed, bool end)
        {
            if (points.Length < 5)
                throw new PDFException(Errors.CannotCreatePolygramWithLessThan5Sides, null);

            if (step >= points.Length)
                throw new PDFException(Errors.StepCountCannotBeGreaterThanVertexCount);

            bool[] visited = new bool[points.Length];
            int loopcount = 0;
            int firstFree = 0;

            //checks the integer array of see if everyone has been covered
            Func<bool> visitedAll = delegate()
            {
                for (int i = 0; i < visited.Length; i++)
                {
                    if (visited[i] == false)
                    {
                        firstFree = i;
                        return false;
                    }
                }
                return true;
            };

            while (!visitedAll() && loopcount < 10)
            {
                if (path.HasCurrentPath == false)
                    path.BeginPath();

                int index = firstFree; //firstFree is set from the visitedAll method
                PDFPoint first = points[index];
                PDFPoint current = first;

                path.MoveTo(current);
                do
                {
                    index += step;
                    if (index >= points.Length)
                        index -= points.Length;

                    current = points[index];

                    visited[index] = true; //mark the point as visited.

                    if (current == first)
                    {
                        if (closed)
                            path.ClosePath(end);
                        else if (end)
                            path.EndPath();
                    }
                    else
                        path.LineTo(current);


                } while (current != first);

                path.EndPath();

                loopcount++;
            }
        }


    }
}
