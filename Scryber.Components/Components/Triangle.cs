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
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.Styles;
namespace Scryber.Components
{
    /// <summary>
    /// Defines a regular (equilateral) triangle
    /// </summary>
    [PDFParsableComponent("Triangle")]
    public class Triangle : PolygonBase
    {

        private PDFPointArray _points;

        /// <summary>
        /// Gets or sets the points in the triangle.
        /// If set - must be an array of 3 points.
        /// </summary>
        [PDFAttribute("points")]
        public PDFPointArray Points
        {
            get { return _points; }
            set { _points = value; }
        }

        public Triangle()
            : this(PDFObjectTypes.ShapeTriangle)
        {
        }


        protected Triangle(PDFObjectType type)
            : base(type)
        {
            this.Style.SetValue(PDFStyleKeys.ShapeVertexCountKey, 3);
        }


        protected override PDFPoint[] GetPoints(PDFRect bounds, Styles.PDFStyle style)
        {
            if (null != Points)
            {
                if (this.Points.Count < 3)
                    throw new IndexOutOfRangeException(Errors.PathMustHave3PointsForTriangle);

                return Points.ToArray();
            }
            else
                return base.GetPoints(bounds, style);
        }


    }
}
