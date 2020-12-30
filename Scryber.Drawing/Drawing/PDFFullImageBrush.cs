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
using Scryber.Native;
using Scryber.Resources;
using System.ComponentModel;

namespace Scryber.Drawing
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFFullImageBrush : PDFBrush
    {

        private PDFReal _op;

        public PDFReal Opacity
        {
            get { return _op; }
            set { _op = value; }
        }

        private string _source;

        public string ImageSource
        {
            get { return _source; }
            set { _source = value; }
        }

        

        private PDFUnit _xpos;
        /// <summary>
        /// Gets or sets the horizontal offset at which the pattern starts.
        /// The default 0 will use the left as the starting point
        /// </summary>
        public PDFUnit XPostion
        {
            get { return _xpos; }
            set { _xpos = value; }
        }

        private PDFUnit _ypos;
        /// <summary>
        /// Gets or sets the vertical offset at which the pattern starts.
        /// The default 0 will use the top as the starting point
        /// </summary>
        public PDFUnit YPostion
        {
            get { return _ypos; }
            set { _ypos = value; }
        }

        private PDFUnit _xstep;
        /// <summary>
        /// Gets or sets the horizontal repeat step that the pattern will move to render each pattern
        /// The default 0 will use the native dimensions of the image as the offset
        /// </summary>
        public PDFUnit XStep
        {
            get { return _xstep; }
            set { _xstep = value; }
        }

        private PDFUnit _ystep;
        /// <summary>
        /// Gets or sets the horizontal repeat step that the pattern will move to render each pattern. 
        /// The default 0 will use the native dimensions of the image as the offset
        /// </summary>
        public PDFUnit YStep
        {
            get { return _ystep; }
            set { _ystep = value; }
        }


        public override FillType FillStyle
        {
            get { return FillType.Image; }
        }

        public PDFFullImageBrush()
            : this(null)
        {
        }

        public PDFFullImageBrush(string source)
        {
            this._source = source;
        }



        public override void SetUpGraphics(PDFGraphics g, PDFRect bounds)
        {
            Scryber.Resources.PDFImageXObject imagex;

            string fullpath = _source; // g.Container.MapPath(_source); - Map Path is done in the document now
            //TODO: Add XStep, YStep etc.
            string resourcekey = GetImagePatternKey(fullpath);


            PDFResource rsrc = g.Container.Document.GetResource(PDFResource.PatternResourceType, resourcekey, false);
            if (null == rsrc)
            {

                //Create the image
                imagex = g.Container.Document.GetResource(Scryber.Resources.PDFResource.XObjectResourceType, fullpath, true) as PDFImageXObject;
                //The container of a pattern is the document as this is the scope
                PDFImageTilingPattern tile = new PDFImageTilingPattern(g.Container.Document, resourcekey, imagex);
                tile.Container = g.Container;
                tile.Image = imagex;
                tile.PaintType = PatternPaintType.ColoredTile;
                tile.TilingType = PatternTilingType.NoDistortion;

                //Calculate the bounds of the pattern

                PDFUnit width;
                PDFUnit height;
                PDFSize imgsize = CalculateAppropriateImageSize(imagex.ImageData, bounds);
                width = imgsize.Width;
                height = imgsize.Height;


                //Patterns are drawn from the bottom of the page so Y is the container height minus the vertical position and offset
                PDFUnit y = 0;// g.ContainerSize.Height - (bounds.Y);// g.ContainerSize.Height - (bounds.Y + height + this.YPostion);
                //X is simply the horizontal position plus offset
                PDFUnit x = 0;// bounds.X + this.XPostion;

                tile.ImageSize = imgsize;

                PDFSize step = new PDFSize();
                if (this.XStep == 0)
                    step.Width = width;
                else
                    step.Width = this.XStep;

                if (this.YStep == 0)
                    step.Height = height;
                else
                    step.Height = this.YStep;
                tile.Step = step;

                PDFPoint start = new PDFPoint(bounds.X + this.XPostion, bounds.Y + this.YPostion);
                
                if (g.Origin == DrawingOrigin.TopLeft)
                {
                    start.Y = g.ContainerSize.Height - start.Y; 
                }
                tile.Start = start;

                PDFName name = g.Container.Register(tile);

                g.SetFillPattern(name);
            }
        }

        private PDFSize CalculateAppropriateImageSize(PDFImageData imgdata, PDFRect bounds)
        {
            //Find the dimension that makes sure the bounds are fully covered
            double scale = (bounds.Width.PointsValue / imgdata.DisplayWidth.PointsValue);
            double resultHeight = imgdata.DisplayHeight.PointsValue * scale;
            if (resultHeight < bounds.Height)
                scale = bounds.Height.PointsValue / imgdata.DisplayHeight.PointsValue;

            PDFUnit imgw = imgdata.DisplayWidth.PointsValue * scale;
            PDFUnit imgh = imgdata.DisplayHeight.PointsValue * scale;

            if(imgw > bounds.Width)
            {
                this.XPostion = -((imgw.PointsValue - bounds.Width.PointsValue) / 2.0);
            }

            if(imgh > bounds.Height)
            {
                this.YPostion = -((imgh.PointsValue - bounds.Height.PointsValue) / 2.0);
            }

            this.XStep = imgw;
            this.YStep = imgh;

            return new PDFSize(imgw, imgh);

            
        }

        private const string IMAGEPATTERNRESOURCEKEY = "Scryber.Resources.ImageTile:{0}/{1}";

        private string GetImagePatternKey(string fullpath)
        {
            int code = this.GetHashCode(); //reference to this brush and image file
            return string.Format(IMAGEPATTERNRESOURCEKEY, fullpath.ToLower(), code);
        }

        public override void ReleaseGraphics(PDFGraphics g, PDFRect bounds)
        {
            g.ClearFillPattern();
        }
    }
}
