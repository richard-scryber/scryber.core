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
using System.Drawing;
using Scryber.PDF.Native;
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Size = Scryber.Drawing.Size;

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// Concrete implementation of a tiling pattern that just 
    /// </summary>
    public class PDFImageTilingPattern : PDFTilingPattern
    {

        

        #region public PDFImageXObject Image {get;set;}

        private PDFImageXObject _img;
        /// <summary>
        /// Gets or sets the Image to be tiled
        /// </summary>
        public PDFImageXObject Image
        {
            get { return _img; }
            set
            {
                _img = value;
                if (_img != null)
                    this.Register(_img);
            }
        }

        #endregion
        

        #region public PDFSize ImageSize {get;set;}

        /// <summary>
        /// Gets or sets the size to render the image
        /// </summary>
        public Drawing.Size ImageSize
        {
            get;
            set;
        }

        #endregion

        


        //
        // ctor(s)
        //

        #region public PDFImageTilingPattern(IPDFComponent container, string key, PDFImageXObject image)

        /// <summary>
        /// Creates a new PDFImageTilingPattern that will render an image as a tile pattern
        /// </summary>
        /// <param name="container"></param>
        /// <param name="key"></param>
        public PDFImageTilingPattern(IComponent container, string key, PDFImageXObject image)
            : base(container, key)
        {
            if (null == image)
                throw new ArgumentNullException("image");
            
            this.Image = image;
        }

        #endregion

        //
        // methods
        //

        #region protected override PDFObjectRef DoRenderToPDF(PDFContextBase context, PDFWriter writer)

        /// <summary>
        /// Renders the tiling image
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override PDFObjectRef DoRenderToPDF(ContextBase context, PDFWriter writer)
        {
            IStreamFilter[] filters = writer.DefaultStreamFilters;
            PDFObjectRef pattern = writer.BeginObject();
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Pattern");
            writer.WriteDictionaryNumberEntry("PatternType", (int)this.PatternType);
            writer.WriteDictionaryNumberEntry("PaintType", (int)this.PaintType);
            writer.WriteDictionaryNumberEntry("TilingType", (int)this.TilingType);
            writer.BeginDictionaryEntry("BBox");

            Drawing.Point offset = new Drawing.Point(this.Start.X, this.Start.Y-this.ImageSize.Height);// this.Start;
            Drawing.Size size = this.ImageSize;

            Drawing.Size graphicsSize = new Drawing.Size(size.Width + offset.X, size.Height + offset.Y);

            writer.WriteArrayRealEntries(true, offset.X.PointsValue,
                                               offset.Y.PointsValue,
                                               offset.X.PointsValue + size.Width.PointsValue,
                                               offset.Y.PointsValue + size.Height.PointsValue);

            writer.EndDictionaryEntry();

            writer.WriteDictionaryRealEntry("XStep", this.Step.Width.PointsValue);
            writer.WriteDictionaryRealEntry("YStep", this.Step.Height.PointsValue);
            
            writer.BeginStream(pattern);
            
            using (PDFGraphics g = PDFGraphics.Create(writer, false, this, DrawingOrigin.TopLeft, 
                graphicsSize, context))
            {
                g.SaveGraphicsState();
                
                if(this.Opacity.HasValue)
                    g.SetFillOpacity(this.Opacity.Value);
                
                var matrix = this.GetTilingImageMatrix(this.Image.ImageData, g, offset, size);
                g.SetTransformationMatrix(matrix, true, true);
                g.PaintXObject(this.Image);
                g.RestoreGraphicsState();
            }
            long len = writer.EndStream();
            
            PDFObjectRef all = this.Resources.WriteResourceList(context, writer);
            writer.WriteDictionaryObjectRefEntry("Resources", all);

            if (null != filters && filters.Length > 0)
            {
                writer.BeginDictionaryEntry("Length");
                writer.WriteNumberS(len);
                writer.EndDictionaryEntry();
                writer.BeginDictionaryEntry("Filter");
                writer.BeginArray();
                foreach (IStreamFilter filter in filters)
                {
                    writer.BeginArrayEntry();
                    writer.WriteName(filter.FilterName);
                    writer.EndArrayEntry();
                }
                writer.EndArray();
                writer.EndDictionaryEntry();
            }
            else
            {
                writer.BeginDictionaryEntry("Length");
                writer.WriteNumberS(len);
                writer.EndDictionaryEntry();
            }

            writer.EndDictionary();
            writer.EndObject();

            return pattern;

        }

        #endregion

        private PDFTransformationMatrix GetTilingImageMatrix(ImageData img, PDFGraphics graphicsContainer, Scryber.Drawing.Point offset, Size tilesize)
        {
            
            Matrix2D matrix = Matrix2D.Identity;
            double x, y, w, h, sh, sv;
            if (img.ImageType == ImageType.Vector)
            {
                //vectors have a bounding box co-ordinate space
                //so we need to scale appropriately for the required image tile size.
                w = 1;
                h = 1;
                x = offset.X.PointsValue;
                y = offset.Y.PointsValue;
                var actSize = img.GetSize();
                sh = (tilesize.Width.PointsValue / actSize.Width.PointsValue); //100pt wide vector into 50pt tile = 0.5
                sv = (tilesize.Height.PointsValue / actSize.Height.PointsValue); //100pt high vector into 50pt tile = 0.5
            }
            else
            {
                //raster images have a scale for the actual pixel size to go from 1 to the required size.
                w = tilesize.Width.PointsValue;
                h = tilesize.Height.PointsValue;

                x = offset.X.PointsValue;

                // if (graphicsContainer.Origin == DrawingOrigin.TopLeft)
                //     //convert the top down to bottom of the page up to the image
                //     y = graphicsContainer.ContainerSize.Height.PointsValue - offset.Y.PointsValue -
                //         tilesize.Height.PointsValue;
                // else
                y = offset.Y.PointsValue;

                //Scale is done by the bounding box of the image being drawn
                sh = 1;
                sv = 1;
            }

            matrix.Elements[0] = w;
            matrix.Elements[3] = h;
            matrix.Elements[4] = x;
            matrix.Elements[5] = y;
            matrix = new Matrix2D(w, 0, 0, h, x, y);
            matrix.Scale(sh, sv);

            return new PDFTransformationMatrix(matrix);

        }
        
    }
}
