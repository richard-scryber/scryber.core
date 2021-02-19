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
using Scryber.Resources;
using Scryber.Native;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Components
{
    public abstract class ImageBase : VisualComponent, IPDFImageComponent, IPDFOptimizeComponent
    {

        private PDFImageXObject _xobj = null;

        /// <summary>
        /// Gets or sets the image data associated with this instance.
        /// </summary>
        public PDFImageXObject XObject
        {
            get { return _xobj; }
            protected set { _xobj = value; }
        }

        private bool _compress = false;
        private bool _hasExplicitCompress = false;

        [PDFAttribute("compress")]
        public virtual bool Compress
        {
            get { return _compress; }
            set
            {
                _compress = value;
                _hasExplicitCompress = true;
            }
        }

        

        [PDFAttribute("min-scale")]
        public virtual double MinimumScaleReduction
        {
            get;
            set;
        }

        [PDFAttribute("allow-missing-images")]
        public virtual bool AllowMissingImages
        {
            get;
            set;
        }


        protected ImageBase(PDFObjectType type)
            : base(type)
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();

            this.MinimumScaleReduction = config.ImagingOptions.MinimumScaleReduction;
            this.AllowMissingImages = config.ImagingOptions.AllowMissingImages;
        }

        protected override void DoInit(PDFInitContext context)
        {
            if (!this._hasExplicitCompress)
                this._compress = this.Document != null && this.Document.RenderOptions != null ? this.Document.RenderOptions.ImageCompression == ImageCompressionType.WebOptimize : false;
            base.DoInit(context);
        }
        /// <summary>
        /// Implementation method for the IPDFImageComponent to return the image data in a PDFImageXObject instance.
        /// </summary>
        /// <returns></returns>
        public virtual PDFImageXObject GetImageObject(PDFContextBase context, Style fullstyle)
        {
            if (null == this.XObject)
            {
                bool log = context.ShouldLogDebug;
                if (log)
                    context.TraceLog.Begin("Image", "Initializing and loading image for component " + this.UniqueID);

                try
                {
                    this.XObject = this.InitImageXObject(context, fullstyle);
                }
                catch (PDFMissingImageException ex)
                {
                    if (this.AllowMissingImages && this.Document.RenderOptions.AllowMissingImages)
                    {
                        context.TraceLog.Add(TraceLevel.Error, "Image", "Missing Image replaced: " + ex.Message);
                        this.Visible = false;
                    }
                    else
                        throw;
                }
                catch (PDFException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    if (context.Conformance == ParserConformanceMode.Lax)
                        context.TraceLog.Add(TraceLevel.Error, "Load of image data failed. " + ex.Message, ex);
                    else
                        throw new PDFException(string.Format(Errors.CouldNotInitializeTheImageForComponent, this.UniqueID, ex.Message), ex);
                }


                if (log)
                {
                    if (null == this.XObject)
                        context.TraceLog.End("Image", "No image loaded for component " + this.UniqueID);
                    else
                        context.TraceLog.End("Image", "Initialized and loaded image '" + this.XObject.ToString() + "' for component " + this.UniqueID);
                }
            }
            return this.XObject;
        }

        /// <summary>
        /// Releases any stored resources required for generating and rendering the image.
        /// </summary>
        public virtual void ResetImageObject()
        {
            this._xobj = null;
        }


        /// <summary>
        /// Inheritors must implement this method to actually load the required image data.
        /// </summary>
        /// <returns></returns>
        protected abstract Resources.PDFImageXObject InitImageXObject(PDFContextBase context, Style fullstyle);

        /// <summary>
        /// Overrides the base implementation to specify that images are by default blocks.
        /// </summary>
        /// <returns></returns>
        protected override Style GetBaseStyle()
        {
            Style s = base.GetBaseStyle();
            s.Position.PositionMode = Scryber.Drawing.PositionMode.Block;
            
            return s;
        }

        /// <summary>
        /// Overrides the base implementation to registed the image data as a resource on the current container
        /// </summary>
        /// <param name="context"></param>
        /// <param name="set"></param>
        /// <param name="fullstyle"></param>
        protected override void DoRegisterArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet set, Style fullstyle)
        {
            IPDFResourceContainer resources = this.GetResourceContainer();
            if (null == resources)
                throw RecordAndRaise.NullReference(Errors.ResourceContainerOfComponnetNotFound, "Image", this.ID);
            PDFImageXObject xobj = this.GetImageObject(context, fullstyle);

            if (null != xobj)
                resources.Register(xobj);

            base.DoRegisterArtefacts(context, set, fullstyle);
        }


        public virtual PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            PDFGraphics graphics = context.Graphics;
            Style full = null;

            PDFComponentArrangement arrange = this.GetFirstArrangement();
            if (null != arrange)
                full = arrange.FullStyle;

            if (null == full)
                full = context.FullStyle;

            PDFImageXObject img = this.GetImageObject(context, full);
            if (img != null)
            {
                PDFPoint pos = context.Offset;


                PDFSize imgsize = context.Space;

                //the pictures are drawn from their bottom left corner, so take off the height.
                //if (context.DrawingOrigin == DrawingOrigin.TopLeft)
                //    pos.Y = pos.Y + imgsize.Height;

                graphics.SaveGraphicsState();

                StyleValue<double> op;
                if(full.TryGetValue(StyleKeys.FillOpacityKey, out op))
                {
                    if (op.Value < 1.0)
                    {
                        graphics.SetFillOpacity(op.Value);
                    }
                }
                PDFObjectRef imgref = img.EnsureRendered(context, writer);
                graphics.PaintImageRef(img, imgsize, pos);

                graphics.RestoreGraphicsState();
                return imgref;
            }
            else
                return null;
        }

        public PDFSize GetRequiredSizeForLayout(PDFSize available, PDFLayoutContext context, Style appliedstyle)
        {
            PDFPositionOptions pos = appliedstyle.CreatePostionOptions();
            PDFTextRenderOptions opts = appliedstyle.CreateTextOptions();
            PDFImageXObject xobj = this.GetImageObject(context, appliedstyle);
            PDFSize naturalSize = xobj.GetImageSize();

            PDFUnit h;
            PDFUnit w;
            if(pos.Height.HasValue && pos.Width.HasValue)
            {
                w = pos.Width.Value;
                h = pos.Height.Value;
            }
            else if(pos.Width.HasValue)
            {
                w = pos.Width.Value;
                h = naturalSize.Height * (pos.Width.Value.PointsValue / naturalSize.Width.PointsValue);
            }
            else if(pos.Height.HasValue)
            {
               
                h = pos.Height.Value;
                w = naturalSize.Width * (pos.Height.Value.PointsValue / naturalSize.Height.PointsValue);
            }
            else if(pos.PositionMode == PositionMode.Inline)
            {
                //We dont have an explicit size
                //So set it to the line height.
                if (opts.Font != null && opts.Font.FontMetrics != null)
                    h = opts.Font.FontMetrics.Ascent;
                else
                    h = opts.Font.Size * 0.75;

                w = naturalSize.Width * (h.PointsValue / naturalSize.Height.PointsValue);
            }
            else //We are in a block on our own line
            {
                h = naturalSize.Height;
                w = naturalSize.Width;

                if (w > available.Width)
                {
                    w = available.Width;
                    h = naturalSize.Height * (available.Width.PointsValue / naturalSize.Width.PointsValue);
                    if(h < naturalSize.Height * this.MinimumScaleReduction)
                    {
                        //exceeded our maximum - so reverting back.
                        h = naturalSize.Height;
                        w = naturalSize.Width;
                    }
                }

                if(h > available.Height)
                {
                    h = available.Height;
                    w = naturalSize.Width * (available.Height.PointsValue / naturalSize.Height.PointsValue);

                    if(w < naturalSize.Width * this.MinimumScaleReduction)
                    {
                        h = naturalSize.Height;
                        w = naturalSize.Width;
                    }
                }
            }
            

            return new PDFSize(w, h);
        }

        public void SetRenderSizes(PDFRect content, PDFRect border, PDFRect total, Style style)
        {
            //Do Nothing
        }

    }
}
