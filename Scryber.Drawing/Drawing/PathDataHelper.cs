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
    internal static class PathDataHelper
    {
        private const double RadiansPerDegree = Math.PI / 180.0;
        private const double DoublePI = Math.PI * 2;

        internal static IEnumerable<PathBezierCurveData> GetBezierCurvesForArc(PDFPoint start, PathArcData arc)
        {
            List<PathBezierCurveData> all = new List<PathBezierCurveData>();

            try
            {
                if (start == arc.EndPoint)
                {
                    return all;
                }

                if (arc.RadiusX == 0.0f && arc.RadiusY == 0.0f)
                {
                    PathBezierCurveData data = new PathBezierCurveData(arc.EndPoint, start, arc.EndPoint, false, false);
                    all.Add(data);
                    return all;
                }

                double sinPhi = Math.Sin(arc.XAxisRotation.PointsValue * PathDataHelper.RadiansPerDegree);
                double cosPhi = Math.Cos(arc.XAxisRotation.PointsValue * PathDataHelper.RadiansPerDegree);

                double x1dash = cosPhi * (start.X.PointsValue - arc.EndPoint.X.PointsValue) / 2.0 + sinPhi * (start.Y.PointsValue - arc.EndPoint.Y.PointsValue) / 2.0;
                double y1dash = -sinPhi * (start.X.PointsValue - arc.EndPoint.X.PointsValue) / 2.0 + cosPhi * (start.Y.PointsValue - arc.EndPoint.Y.PointsValue) / 2.0;

                double root;
                double numerator = arc.RadiusX.PointsValue * arc.RadiusX.PointsValue * arc.RadiusY.PointsValue * arc.RadiusY.PointsValue 
                                                - arc.RadiusX.PointsValue * arc.RadiusX.PointsValue * y1dash * y1dash 
                                                - arc.RadiusY.PointsValue * arc.RadiusY.PointsValue * x1dash * x1dash;

                double rx = arc.RadiusX.PointsValue;
                double ry = arc.RadiusY.PointsValue;

                if (numerator < 0.0)
                {
                    double s = (double)Math.Sqrt(1.0 - numerator / (arc.RadiusX.PointsValue * arc.RadiusX.PointsValue * arc.RadiusY.PointsValue * arc.RadiusY.PointsValue));

                    rx *= s;
                    ry *= s;
                    root = 0.0;
                }
                else
                {
                    root = ((arc.ArcSize == PathArcSize.Large && arc.ArcSweep == PathArcSweep.Positive) || (arc.ArcSize == PathArcSize.Small && arc.ArcSweep == PathArcSweep.Negative) ? -1.0 : 1.0) 
                        * Math.Sqrt(numerator / (arc.RadiusX.PointsValue * arc.RadiusX.PointsValue * y1dash * y1dash + arc.RadiusY.PointsValue * arc.RadiusY.PointsValue * x1dash * x1dash));
                }

                double cxdash = root * rx * y1dash / ry;
                double cydash = -root * ry * x1dash / rx;

                double cx = cosPhi * cxdash - sinPhi * cydash + (start.X.PointsValue + arc.EndPoint.X.PointsValue) / 2.0;
                double cy = sinPhi * cxdash + cosPhi * cydash + (start.Y.PointsValue + arc.EndPoint.Y.PointsValue) / 2.0;
                

                double theta1 = PathDataHelper.CalculateVectorAngle(1.0, 0.0, (x1dash - cxdash) / rx, (y1dash - cydash) / ry);
                double dtheta = PathDataHelper.CalculateVectorAngle((x1dash - cxdash) / rx, (y1dash - cydash) / ry, (-x1dash - cxdash) / rx, (-y1dash - cydash) / ry);

                if (arc.ArcSweep == PathArcSweep.Negative && dtheta > 0)
                {
                    dtheta -= 2.0 * Math.PI;
                }
                else if (arc.ArcSweep == PathArcSweep.Positive && dtheta < 0)
                {
                    dtheta += 2.0 * Math.PI;
                }

                int segments = (int)Math.Ceiling((double)Math.Abs(dtheta / (Math.PI / 2.0)));
                double delta = dtheta / segments;
                double t = 8.0 / 3.0 * Math.Sin(delta / 4.0) * Math.Sin(delta / 4.0) / Math.Sin(delta / 2.0);

                double startX = start.X.PointsValue;
                double startY = start.Y.PointsValue;


                for (int i = 0; i < segments; ++i)
                {
                    double cosTheta1 = Math.Cos(theta1);
                    double sinTheta1 = Math.Sin(theta1);
                    double theta2 = theta1 + delta;
                    double cosTheta2 = Math.Cos(theta2);
                    double sinTheta2 = Math.Sin(theta2);

                    double endpointX = cosPhi * rx * cosTheta2 - sinPhi * ry * sinTheta2 + cx;
                    double endpointY = sinPhi * rx * cosTheta2 + cosPhi * ry * sinTheta2 + cy;

                    double dx1 = t * (-cosPhi * rx * sinTheta1 - sinPhi * ry * cosTheta1);
                    double dy1 = t * (-sinPhi * rx * sinTheta1 + cosPhi * ry * cosTheta1);

                    double dxe = t * (cosPhi * rx * sinTheta2 + sinPhi * ry * cosTheta2);
                    double dye = t * (sinPhi * rx * sinTheta2 - cosPhi * ry * cosTheta2);

                    PDFPoint endPoint = new PDFPoint(endpointX, endpointY);
                    PDFPoint startHandle = new PDFPoint((startX + dx1), (startY + dy1));
                    PDFPoint endHandle = new PDFPoint((endpointX + dxe), (endpointY + dye));

                    PathBezierCurveData bezier = new PathBezierCurveData(endPoint, startHandle, endHandle, true, true);
                    all.Add(bezier);
                    
                    theta1 = theta2;
                    startX = (float)endpointX;
                    startY = (float)endpointY;
                }
            }
            catch (Exception ex)
            {
                throw new PDFException(Errors.CouldNotBuildPathFromArc, ex);
            }

            return all;
        }

        private static double CalculateVectorAngle(double ux, double uy, double vx, double vy)
        {
            double ta = Math.Atan2(uy, ux);
            double tb = Math.Atan2(vy, vx);

            if (tb >= ta)
            {
                return tb - ta;
            }

            return PathDataHelper.DoublePI - (ta - tb);
        }

        internal static IEnumerable<PathBezierCurveData> GetBezierCurvesForQuadratic(PDFPoint start, PathQuadraticCurve quad)
        {
            PathBezierCurveData[] all = new PathBezierCurveData[1];

            try
            {
                double x1 = start.X.PointsValue + (quad.ControlPoint.X.PointsValue - start.X.PointsValue) * 2 / 3;
                double y1 = start.Y.PointsValue + (quad.ControlPoint.Y.PointsValue - start.Y.PointsValue) * 2 / 3;

                PDFPoint startHandle = new PDFPoint(x1, y1);

                double x2 = quad.ControlPoint.X.PointsValue + (quad.EndPoint.X.PointsValue - quad.ControlPoint.X.PointsValue) / 3;
                double y2 = quad.ControlPoint.Y.PointsValue + (quad.EndPoint.Y.PointsValue - quad.ControlPoint.Y.PointsValue) / 3;

                PDFPoint endHandle = new PDFPoint(x2, y2);

                PathBezierCurveData one = new PathBezierCurveData(quad.EndPoint, startHandle, endHandle, true, true);
                all[0] = one;

            }
            catch (Exception ex)
            {
                throw new PDFException(Errors.CouldNotBuildPathFromQuadratic, ex);
            }

            return all;
        }
    }
}
