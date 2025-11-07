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
using System.Text;
using Scryber.Styles;
using Scryber.Drawing;
namespace Scryber.Components
{
    [PDFParsableComponent("Line")]
    public class Line : ShapeComponent
    {

        public Line()
            : this(ObjectTypes.ShapeLine)
        {
        }

        protected Line(ObjectType type) : base(type)
        { }


        protected override Style GetBaseStyle()
        {
            Style inherited = base.GetBaseStyle();
            inherited.Stroke.Width = new Unit(1, PageUnits.Points);
            inherited.Stroke.Color = StandardColors.Black;
            inherited.Position.DisplayMode = Scryber.Drawing.DisplayMode.Block;
            inherited.Fill.Style = FillType.None;

            return inherited;
        }


        protected override GraphicsPath CreatePath(Size available, Style fullstyle)
        {
            var pos = fullstyle.CreatePostionOptions(false);

            Point start = Point.Empty;
            Point end = new Point(available.Width, start.Y);

            if (pos.X.HasValue)
                start.X = pos.X.Value;

            if (pos.Y.HasValue)
            {
                start.Y = pos.Y.Value;
                end.Y = pos.Y.Value;
            }

            if (pos.Width.HasValue)
            {
                end.X = pos.Width.Value + start.X;

                if (pos.Height.HasValue)
                    end.Y = pos.Height.Value - start.Y;
                else // no hight so this is a horizontal line
                    end.Y = start.Y;
            }
            else if (pos.FillWidth)
            {
                end.X = available.Width - start.X;

                if (pos.Height.HasValue)
                    end.Y = pos.Height.Value - start.Y;
                else // no hight so this is a horizontal line
                    end.Y = start.Y;

            }
            //no width so if we have a height this is a vertical line
            else if (pos.Height.HasValue)
            {
                end.Y = pos.Height.Value + start.Y;
                end.X = start.X;
            }
            else //default is a horizontal line
            {
                end.X = available.Width - start.X;
                end.Y = start.Y;
            }
            

            GraphicsPath path = new GraphicsPath();
            path.MoveTo(start);
            path.LineTo(end);

            return path;
        }
        
    }
}
