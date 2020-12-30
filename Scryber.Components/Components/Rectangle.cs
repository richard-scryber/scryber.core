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
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Native;
using Scryber.Styles;

namespace Scryber.Components
{
    [PDFParsableComponent("Rect")]
    public class Rectangle : PolygonBase
    {

        public Rectangle()
            : this(PDFObjectTypes.ShapeRectangle)
        {
        }

        protected Rectangle(PDFObjectType type)
            : base(type)
        {
            this.Style.Shape.VertexCount = 4;
            this.DefaultRotation = 0;
        }

        protected override PDFPoint[] GetPoints(PDFRect bounds, Style style)
        {
            double angle = style.GetValue(StyleKeys.ShapeRotationKey, this.DefaultRotation);
            double x = bounds.Width.PointsValue / 2;
            double y = bounds.Height.PointsValue / 2;
            PDFPoint[] all = new PDFPoint[4];

            //build the rectangle around the origin
            all[0] = new PDFPoint(-x, -y);
            all[1] = new PDFPoint(x, -y);
            all[2] = new PDFPoint(x, y);
            all[3] = new PDFPoint(-x, y);

            //rotate by the required amount
            double rotation = ToRadians(angle);
            PDFUnit minx = 0;
            PDFUnit miny = 0;
            if (rotation != 0)
            {
                for (int i = 0; i < all.Length; i++)
                {
                    PDFPoint pt = all[i];
                    pt = Rotate(pt, rotation);
                    if (i == 0)
                    {
                        minx = pt.X;
                        miny = pt.Y;
                    }
                    else
                    {
                        minx = PDFUnit.Min(minx, pt.X);
                        miny = PDFUnit.Min(miny, pt.Y);
                    }
                    all[i] = pt;
                }
            }
            else
            {
                minx = -x;
                miny = -y;
            }

            //then tanslate by the required amount (top left + half width and half height)
            PDFUnit xoffset = 0 - minx;
            PDFUnit yoffset = 0 - miny;
            for (int i = 0; i < all.Length; i++)
            {
                all[i] = Translate(all[i], xoffset, yoffset);
            }

            return all;
        }
    }

}
