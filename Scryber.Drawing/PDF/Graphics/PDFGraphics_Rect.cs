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
using Scryber.Drawing;

namespace Scryber.PDF.Graphics
{
    public partial class PDFGraphics
    {

        #region DrawRectangle(PDFRectangle) + 3 overloads

        public void DrawRectangle(PDFPen pen, Rect rect)
        {
            this.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawRectangle(PDFPen pen, Rect rect, Sides sides)
        {
            this.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height, sides);
        }

        
        public void DrawRectangle(PDFPen pen, Point location, Size size)
        {
            this.DrawRectangle(pen, location.X, location.Y, size.Width, size.Height);
        }

        public void DrawRectangle(PDFPen pen, Point location, Size size, Sides sides)
        {
            this.DrawRectangle(pen, location.X, location.Y, size.Width, size.Height, sides);
        }

        #endregion

        

        #region DrawRectangle(X,Y,Width,Height)

        public void DrawRectangle(PDFPen pen, Unit x, Unit y, Unit width, Unit height)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            this.SaveGraphicsState();
            Rect rect = new Rect(x, y, width, height);
            pen.SetUpGraphics(this, rect);
            this.RenderRectangle(x, y, width, height);
            this.RenderStrokePathOp();
            pen.ReleaseGraphics(this, rect);
            this.RestoreGraphicsState();
        }

        public void DrawRectangle(PDFPen pen, Unit x, Unit y, Unit width, Unit height, Sides sides)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            Rect rect = new Rect(x, y, width, height);

            this.SaveGraphicsState();
            if (pen.SetUpGraphics(this, rect))
            {

                //check to see if we are outputting all sides
                if (sides == (Sides.Top | Sides.Right | Sides.Left | Sides.Bottom))
                    this.RenderRectangle(x, y, width, height);
                else
                {
                    bool recalc = true; //flag to identifiy if the last op moved the cursor to the next correct position

                    if ((sides & Sides.Top) > 0)
                    {
                        this.RenderLine(x, y, x + width, y);
                        recalc = false;
                    }
                    else
                        recalc = true;

                    if ((sides & Sides.Right) > 0)
                    {
                        if (recalc == false)
                        {
                            this.RenderContinuationLine(x + width, y + height);
                        }
                        else
                        {
                            this.RenderLine(x + width, y, x + width, y + height);
                        }
                        recalc = false;
                    }
                    else
                        recalc = true;

                    if ((sides & Sides.Bottom) > 0)
                    {
                        if (recalc == false)
                            this.RenderContinuationLine(x, y + height);
                        else
                            this.RenderLine(x + width, y + height, x, y + height);
                        recalc = false;
                    }
                    else
                        recalc = true;

                    if ((sides & Sides.Left) > 0)
                    {
                        if (recalc == false)
                            this.RenderContinuationLine(x, y);
                        else
                            this.RenderLine(x, y + height, x, y);
                    }
                }
                this.RenderStrokePathOp();
                pen.ReleaseGraphics(this, rect);
            }
            this.RestoreGraphicsState();
        }

        #endregion

        #region FillRectangle(PDFBrush brush, PDFRectangle) + 2 overloads

        public void FillRectangle(PDFBrush brush, Rect rect)
        {
            this.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void FillRectangle(PDFBrush brush, Point location, Size size)
        {
            this.FillRectangle(brush, location.X, location.Y, size.Width, size.Height);
        }

        public void FillRectangle(PDFBrush brush, Unit x, Unit y, Unit width, Unit height)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            if (null != brush.UnderBrush)
                FillRectangle(brush.UnderBrush, x, y, width, height);

            this.SaveGraphicsState();
            Rect bounds = new Rect(x, y, width, height);
            brush.SetUpGraphics(this, bounds);
            
            this.RenderRectangle(x, y, width, height);
            this.RenderFillPathOp();
            
            brush.ReleaseGraphics(this, bounds);

            this.RestoreGraphicsState();
        }

        #endregion

        #region DrawRoundRectangle()

        public void DrawRoundRectangle(PDFPen pen, Rect rect, Unit cornerRadius)
        {
            this.DrawRoundRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height, cornerRadius);
        }

        public void DrawRoundRectangle(PDFPen pen, Point pos, Size size, Unit cornerRadius)
        {
            this.DrawRoundRectangle(pen, pos.X, pos.Y, size.Width, size.Height, cornerRadius);
        }

        public void DrawRoundRectangle(PDFPen pen, Unit x, Unit y, Unit width, Unit height, Unit cornerRadius)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            this.SaveGraphicsState();
            Rect bounds = new Rect(x, y, width, height);

            pen.SetUpGraphics(this, bounds);
            this.DoOutputRoundRectangle(x, y, width, height, cornerRadius);
            this.RenderStrokePathOp();
            pen.ReleaseGraphics(this, bounds);

            this.RestoreGraphicsState();
        }

        public void DrawRoundRectangle(PDFPen pen, Rect rect, Sides sides, Unit cornerRadius)
        {
            this.DrawRoundRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height, sides, cornerRadius);
        }

        public void DrawRoundRectangle(PDFPen pen, Point pos, Size size, Sides sides, Unit cornerRadius)
        {
            this.DrawRoundRectangle(pen, pos.X, pos.Y, size.Width, size.Height, sides, cornerRadius);
        }

        public void DrawRoundRectangle(PDFPen pen, Unit x, Unit y, Unit width, Unit height, Sides sides, Unit cornerRadius)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            Rect bounds = new Rect(x, y, width, height);

            this.SaveGraphicsState();
            pen.SetUpGraphics(this, bounds);
            this.DoOutputRoundRectangleWithSidesPath(x, y, width, height, cornerRadius, sides);
            this.RenderStrokePathOp();
            pen.ReleaseGraphics(this, bounds);
            this.RestoreGraphicsState();
        }

        #endregion

        #region FillRoundRectangle()

        public void FillRoundRectangle(PDFBrush brush, Unit x, Unit y, Unit width, Unit height, Unit cornerRadius)
        {
            Rect rect = new Rect(x, y, width, height);
            this.FillRoundRectangle(brush, rect, cornerRadius);
        }

        public void FillRoundRectangle(PDFBrush brush, Point pos, Size size, Unit cornerRadius)
        {
            Rect r = new Rect(pos, size);
            this.FillRoundRectangle(brush, r, cornerRadius);
        }

        public void FillRoundRectangle(PDFBrush brush, Rect rect, Unit cornerRadius)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            if (null != brush.UnderBrush)
                this.FillRoundRectangle(brush.UnderBrush, rect, cornerRadius);

            this.SaveGraphicsState();
            //PDFRect bounds = new PDFRect(x, y, width, height);
            
            brush.SetUpGraphics(this, rect);
            this.DoOutputRoundRectangle(rect.X, rect.Y, rect.Width, rect.Height, cornerRadius);
            this.RenderFillPathOp();
            brush.ReleaseGraphics(this, rect);

            this.RestoreGraphicsState();
        }

        public void FillRoundRectangle(PDFBrush brush, Rect rect, Sides sides, Unit cornerRadius)
        {
            this.FillRoundRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height, sides, cornerRadius);
        }

        public void FillRoundRectangle(PDFBrush brush, Point pos, Size size, Sides sides, Unit cornerRadius)
        {
            this.FillRoundRectangle(brush, pos.X, pos.Y, size.Width, size.Height, sides, cornerRadius);
        }

        public void FillRoundRectangle(PDFBrush brush, Unit x, Unit y, Unit width, Unit height, Sides sides, Unit cornerRadius)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            if (null != brush.UnderBrush)
                this.FillRoundRectangle(brush.UnderBrush, x, y, width, height, sides, cornerRadius);

            Rect bounds = new Rect(x, y, width, height);
            this.SaveGraphicsState();
            brush.SetUpGraphics(this, bounds);
            this.DoOutputRoundRectangleWithSidesFill(x, y, width, height, cornerRadius, sides);
            this.RenderFillPathOp();
            brush.ReleaseGraphics(this, bounds);
            this.RestoreGraphicsState();
        }

        #endregion

        private void DoOutputRoundRectangle(Unit x, Unit y, Unit width, Unit height, Unit cornerRadius)
        {
            Unit handleoffset = (Unit)(cornerRadius.PointsValue * CircularityFactor);
            this.RenderMoveTo(x, y + height - cornerRadius);
            //left vertical
            this.RenderLineTo(x, y + cornerRadius);
            //topleft arc
            this.RenderBezierCurveTo(x + cornerRadius, y, x, y + cornerRadius - handleoffset, x + cornerRadius - handleoffset, y);
            //top horizontal
            this.RenderLineTo(x + width - cornerRadius, y);
            //topright arc
            this.RenderBezierCurveTo(x + width, y + cornerRadius, x + width - cornerRadius + handleoffset, y, x + width, y + cornerRadius - handleoffset);
            //right vertical
            this.RenderLineTo(x + width, y + height - cornerRadius);
            //bottomright arc
            this.RenderBezierCurveTo(x + width - cornerRadius, y + height, x + width, y + height - cornerRadius + handleoffset, x + width - cornerRadius + handleoffset, y + height);
            //bottom line
            this.RenderLineTo(x + cornerRadius, y + height);
            //bottom left arc
            this.RenderBezierCurveTo(x, y + height - cornerRadius, x + cornerRadius - handleoffset, y + height, x, y + height - cornerRadius + handleoffset);
        }

        private void DoOutputRoundRectangleWithSidesFill(Unit x, Unit y, Unit width, Unit height, Unit cornerRadius, Sides sides)
        {
            Unit handleoffset = (Unit)(cornerRadius.PointsValue * CircularityFactor);
            
            //position cursor at left bottom
            if ((sides & Sides.Left) > 0 && (sides & Sides.Bottom) > 0)
            {
                //if we have a bottom edge then we are going to have an arc
                this.RenderMoveTo(x, y + height - cornerRadius);
            }
            else
                this.RenderMoveTo(x, y + height);

            //draw left vertical and optional arc
            if ((sides & Sides.Left) > 0 && (sides & Sides.Top) > 0)
            {
                //as we have a left and a top then we have an arc
                this.RenderLineTo(x, y + cornerRadius);
                //top left arc
                this.RenderBezierCurveTo(x + cornerRadius, y, x, y + cornerRadius - handleoffset, x + cornerRadius - handleoffset, y);
            }
            else   
                this.RenderLineTo(x, y);

            //draw top horizontal and optional arc
            if ((sides & Sides.Top) > 0 && (sides & Sides.Right) > 0)
            {
                //as we have a top and a right then we have an arc
                this.RenderLineTo(x + width - cornerRadius, y);
                //top right arc
                this.RenderBezierCurveTo(x + width, y + cornerRadius, x + width - cornerRadius + handleoffset, y, x + width, y + cornerRadius - handleoffset);
            }
            else
                this.RenderLineTo(x + width, y);


            //draw right vertical and optional arc
            if ((sides & Sides.Right) > 0 && (sides & Sides.Bottom) > 0)
            {
                //right vertical
                this.RenderLineTo(x + width, y + height - cornerRadius);
                //bottomright arc
                this.RenderBezierCurveTo(x + width - cornerRadius, y + height, x + width, y + height - cornerRadius + handleoffset, x + width - cornerRadius + handleoffset, y + height);
            }
            else
                this.RenderLineTo(x + width, y + height);

            //draw bottom horizontal and optional arc
            if ((sides & Sides.Bottom) > 0 && (sides & Sides.Left) > 0)
            {
                //bottom line
                this.RenderLineTo(x + cornerRadius, y + height);
                //bottom left arc
                this.RenderBezierCurveTo(x, y + height - cornerRadius, x + cornerRadius - handleoffset, y + height, x, y + height - cornerRadius + handleoffset);
            }
            else
                this.RenderLineTo(x, y + height);
            
        }

        private void DoOutputRoundRectangleWithSidesPath(Unit x, Unit y, Unit width, Unit height, Unit cornerRadius, Sides sides)
        {
            Unit handleoffset = (Unit)(cornerRadius.PointsValue * CircularityFactor);
            
            bool requiresmove = false;

            if ((sides & Sides.Left) > 0)
            {
                if ((sides & Sides.Bottom) == 0)
                    this.RenderMoveTo(x, y + height);
                else
                    this.RenderMoveTo(x, y + height - cornerRadius);

                //left vertical
                if ((sides & Sides.Top) == 0) //no top line so extend full height
                    this.RenderLineTo(x,y);
                else
                    this.RenderLineTo(x, y + cornerRadius);
            }
            else
                requiresmove = true;

            if ((sides & Sides.Top) > 0)
            {
                if (requiresmove)
                {
                    if ((sides & Sides.Left) == 0)//no left side
                        RenderMoveTo(x, y);
                    else
                        RenderMoveTo(x + cornerRadius, y);
                }
                else
                {
                    //topleft arc
                    this.RenderBezierCurveTo(x + cornerRadius, y, x, y + cornerRadius - handleoffset, x + cornerRadius - handleoffset, y);
                }

                //top horizontal
                if ((sides & Sides.Right) == 0)
                    this.RenderLineTo(x + width, y);//no right line so extend full width
                else
                    this.RenderLineTo(x + width - cornerRadius, y);
                requiresmove = false;
            }
            else
                requiresmove = true;

            if ((sides & Sides.Right) > 0)
            {
                if (requiresmove)
                {
                    if ((sides & Sides.Top) == 0)//no top side 
                        RenderMoveTo(x + width, y);//go to the top right corner
                    else
                        RenderMoveTo(x + width, y + cornerRadius);
                }
                else
                {
                    //topright arc
                    this.RenderBezierCurveTo(x + width, y + cornerRadius, x + width - cornerRadius + handleoffset, y, x + width, y + cornerRadius - handleoffset);
                }
                
                //right vertical
                if ((sides & Sides.Bottom) == 0)
                    this.RenderLineTo(x + width, y + height);//no bottom line so extend to full height
                else
                    this.RenderLineTo(x + width, y + height - cornerRadius);
                requiresmove = false;
            }
            else
                requiresmove = true;

            if ((sides & Sides.Bottom) > 0)
            {
                if (requiresmove)
                {
                    if ((sides & Sides.Right) == 0)//no right side
                        RenderMoveTo(x + width, y + height);//go to the bottom left corner
                    else
                        RenderMoveTo(x + width, y + height - cornerRadius);
                }
                else
                {
                    //bottomright arc
                    this.RenderBezierCurveTo(x + width - cornerRadius, y + height, x + width, y + height - cornerRadius + handleoffset, x + width - cornerRadius + handleoffset, y + height);
                }
                //bottom line
                if ((sides & Sides.Left) == 0)
                    this.RenderLineTo(x, y + height);
                else
                    this.RenderLineTo(x + cornerRadius, y + height);

                //if we have the left and bottom sides we need to connect them
                if ((sides & Sides.Left) > 0)
                {
                    //bottom left arc
                    this.RenderBezierCurveTo(x, y + height - cornerRadius, x + cornerRadius - handleoffset, y + height, x, y + height - cornerRadius + handleoffset);

                }
            }
            else
                requiresmove = true;

            
        }
        
    }
}
