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
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.Components
{
    /// <summary>
    /// Abstract base class for regular and irregular polygons.
    /// </summary>
    public abstract class PDFPolygonBase : PDFShapeComponent
    {


        /// <summary>
        /// Gets the fill style associated with this polygon
        /// </summary>
        [PDFAttribute("fill-style", Scryber.Styles.PDFStyle.PDFStylesNamespace)]
        public FillStyle FillStyle
        {
            get { return this.Style.Fill.FillStyle; }
            set { this.Style.Fill.FillStyle = value; }
        }

        /// <summary>
        /// Gets or sets the flag for if this polygon is closed or open
        /// </summary>
        [PDFAttribute("closed", Scryber.Styles.PDFStyle.PDFStylesNamespace)]
        public bool Closed
        {
            get { return this.Style.Shape.Closed; }
            set { this.Style.Shape.Closed = value; }
        }


        /// <summary>
        /// Gets or sets the rotation angle of the polygon
        /// </summary>
        [PDFAttribute("rotate", Scryber.Styles.PDFStyle.PDFStylesNamespace)]
        public double Rotation
        {
            get { return this.Style.Shape.Rotation; }
            set { this.Style.Shape.Rotation = value; }
        }


        //Default angle is negative 90 - this rotates the first point to the top centre, rather than right middle.
        private const double _DefaultRotationAngle = -90;

        private const int _DefaultSVertexCount = 4;

        /// <summary>
        /// Can be used by inheritiong classes to alter the standard rotation angle
        /// </summary>
        protected double DefaultRotation
        {
            get;
            set;
        }

        protected int DefaultShapeVetexCount
        {
            get;
            set;
        }

        public PDFPolygonBase(PDFObjectType type)
            : base(type)
        {
            this.DefaultRotation = _DefaultRotationAngle;
            this.DefaultShapeVetexCount = _DefaultSVertexCount;
        }


        /// <summary>
        /// Implements the base classes abstract method to build a path.
        /// </summary>
        /// <param name="available"></param>
        /// <param name="fullstyle"></param>
        /// <returns></returns>
        protected override PDFGraphicsPath CreatePath(PDFSize available, PDFStyle fullstyle)
        {
            PDFRect rect = this.GetPrescribedBounds(available, fullstyle);

            PDFPoint[] points = this.GetPoints(rect, fullstyle);

            PDFGraphicsPath path = new PDFGraphicsPath();
            this.BuildPath(path, points, fullstyle, true);

            return path;
        }

        /// <summary>
        /// Gets all the points in this polygon based on the required bounds. 
        /// Inheritors can override this method.
        /// </summary>
        /// <param name="bounds">The bounds this polygo should fit into.</param>
        /// <param name="style">The style of the polygon</param>
        /// <returns>An array of points</returns>
        protected virtual Drawing.PDFPoint[] GetPoints(Drawing.PDFRect bounds, PDFStyle style)
        {

            int vCount = style.GetValue(PDFStyleKeys.ShapeVertexCountKey, this.DefaultShapeVetexCount);
            double rotation = style.GetValue(PDFStyleKeys.ShapeRotationKey, 0.0);

            if (vCount > 2)
            {
                PDFUnit radiusX = (bounds.Width / 2.0);
                PDFUnit radiusY = (bounds.Height / 2.0);
                PDFPoint centre = new PDFPoint(radiusX, radiusY);

                return this.GetRegularPolygonPoints(centre, radiusX, radiusY, vCount, rotation);
            }
            else if (vCount != 0) //between 1 and 2 sides is invalid.
                throw new PDFException(Errors.CannotCreatePolygonWithLessThan3Sides, null);
            else
                return new PDFPoint[] { };
        }



        //
        // helper methods
        //


        #region protected virtual void BuildPath(PDFGraphicsPath path, PDFPoint[] points, PDFStyle style, bool end)

        /// <summary>
        /// Adds a series of lines to the path based on the points. Moving to the first in the array, adding lines after, and optionally closing and ending the path.
        /// </summary>
        /// <param name="path">The path to build the lines in</param>
        /// <param name="points">The points to add lines between</param>
        /// <param name="close">Closes the path (adds an extra line back to the starting point</param>
        /// <param name="end">If true then ends the path so no more points can be added to it</param>
        protected virtual void BuildPath(PDFGraphicsPath path, PDFPoint[] points, PDFStyle style, bool end)
        {
            if (path.HasCurrentPath == false)
                path.BeginPath();

            for (int i = 0; i < points.Length; i++)
            {
                if (i == 0)
                    path.MoveTo(points[i]);
                else
                    path.LineTo(points[i]);
            }

            bool closed = style.GetValue(PDFStyleKeys.ShapeClosedKey, true);
            if (closed)
                path.ClosePath(end);
            else if (end)
                path.EndPath();
        }

        #endregion

        #region protected virtual PDFRect GetPrescribedBounds(PDFSize avail, PDFStyle fullstyle)

        /// <summary>
        /// Based on the available size and full style, returns a bounding rectagle the shape should fit into.
        /// </summary>
        /// <param name="avail"></param>
        /// <param name="fullstyle"></param>
        /// <returns></returns>
        protected virtual PDFRect GetPrescribedBounds(PDFSize avail, PDFStyle fullstyle)
        {
            PDFUnit x = fullstyle.GetValue(PDFStyleKeys.PositionXKey, 0);
            PDFUnit y = fullstyle.GetValue(PDFStyleKeys.PositionYKey, 0);
            PDFUnit w = fullstyle.GetValue(PDFStyleKeys.SizeWidthKey, avail.Width);
            PDFUnit h = fullstyle.GetValue(PDFStyleKeys.SizeHeightKey, avail.Height);

            return new PDFRect(x, y, w, h);
        }

        #endregion

        #region protected PDFPoint[] GetRegularPolygonPoints(PDFPoint centre, PDFUnit radius, int numsides)

        /// <summary>
        /// Creates an array of points for a regular polygon based on the centre, radius and the number of sides.
        /// </summary>
        /// <param name="centre"></param>
        /// <param name="radius"></param>
        /// <param name="numsides"></param>
        /// <returns></returns>
        protected PDFPoint[] GetRegularPolygonPoints(PDFPoint centre, PDFUnit radiusX, PDFUnit radiusY, int numsides, double rotationDegrees)
        {
            PDFPoint[] all = new PDFPoint[numsides];

            rotationDegrees = rotationDegrees + DefaultRotation;
            double rotation = rotationDegrees * InRadians;

            for (int i = 0; i < numsides; i++)
            {
                double factor = (double)i / (double)numsides;
                double angle = rotation + (2 * Math.PI * factor);
                PDFUnit x = centre.X + (radiusX * Math.Cos(angle));
                PDFUnit y = centre.Y + (radiusY * Math.Sin(angle));
                all[i] = new PDFPoint(x, y);
            }

            return all;
        }

        #endregion

        /// <summary>
        /// Rotates the specified point around the origin (0,0) by the specified number of radians
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="radians"></param>
        /// <returns></returns>
        protected static PDFPoint Rotate(PDFPoint pt, double radians)
        {
            if (radians != 0.0)
            {
                PDFPoint pt2 = new PDFPoint();
                //x*cos(theta) - y*sin(theta)
                pt2.X = pt.X * Math.Cos(radians) - pt.Y * Math.Sin(radians);
                pt2.Y = pt.X * Math.Sin(radians) + pt.Y * Math.Cos(radians);
                pt = pt2;
            }
            return pt;
        }

        /// <summary>
        /// Moves the point by x and y dimensions
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected static PDFPoint Translate(PDFPoint pt, PDFUnit x, PDFUnit y)
        {
            pt.X += x;
            pt.Y += y;

            return pt;
        }

        protected const double InRadians = Math.PI / 180.0;

        protected static double ToRadians(double degrees)
        {
            return degrees * InRadians;
        }
    }
}
