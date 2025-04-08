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
using Scryber.PDF.Resources;
using Scryber.PDF.Native;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF;
using Scryber.PDF.Graphics;

namespace Scryber.Components
{
    public abstract class ImageBase : VisualComponent, IPDFImageComponent, IPDFRenderComponent
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


        protected ImageBase(ObjectType type)
            : base(type)
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();

            this.MinimumScaleReduction = config.ImagingOptions.MinimumScaleReduction;
            this.AllowMissingImages = config.ImagingOptions.AllowMissingImages;
        }

        public override string MapPath(string source, out bool isfile)
        {
            return base.MapPath(source, out isfile);
        }



        /// <summary>
        /// Implementation method for the IPDFImageComponent to return the image data in a PDFImageXObject instance.
        /// </summary>
        /// <returns></returns>
        public virtual PDFImageXObject GetImageObject(ContextBase context, Style fullstyle)
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
        protected abstract PDFImageXObject InitImageXObject(ContextBase context, Style fullstyle);

        /// <summary>
        /// Overrides the base implementation to specify that images are by default blocks.
        /// </summary>
        /// <returns></returns>
        protected override Style GetBaseStyle()
        {
            Style s = base.GetBaseStyle();
            s.Position.DisplayMode = Scryber.Drawing.DisplayMode.Block;
            s.Position.PositionMode = PositionMode.Static;
            return s;
        }

        /// <summary>
        /// Overrides the base implementation to registed the image data as a resource on the current container
        /// </summary>
        /// <param name="context"></param>
        /// <param name="set"></param>
        /// <param name="fullstyle"></param>
        protected override void DoRegisterArtefacts(PDFLayoutContext context, PDF.PDFArtefactRegistrationSet set, Style fullstyle)
        {
            if (this.Visible && fullstyle.Position.DisplayMode != DisplayMode.Invisible)
            {
                IResourceContainer resources = this.GetResourceContainer();
                if (null == resources)
                    throw RecordAndRaise.NullReference(Errors.ResourceContainerOfComponnetNotFound, "Image", this.ID);
                PDFImageXObject xobj = this.GetImageObject(context, fullstyle);

                if (null != xobj)
                    resources.Register(xobj);

                base.DoRegisterArtefacts(context, set, fullstyle);
            }
        }


        public virtual PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (!this.Visible)
                return null;

            PDFGraphics graphics = context.Graphics;
            Style full = null;

            ComponentArrangement arrange = this.GetFirstArrangement();
            if (null != arrange)
                full = arrange.FullStyle;

            if (null == full)
                full = context.FullStyle;

            if (full.Position.DisplayMode == DisplayMode.Invisible)
                return null;

            PDFImageXObject img = this.GetImageObject(context, full);
            if (img != null)
            {
                img.RegisterUse(context.Graphics.Container.Resources, this);
                
                Point pos = img.GetRequiredOffsetForRender(context.Offset, context);
                
                Size imgsize = img.GetRequiredSizeForRender(context.Space, context);

                

                graphics.SaveGraphicsState();

                StyleValue<double> op;
                if(full.TryGetValue(StyleKeys.FillOpacityKey, out op))
                {
                    if (op.Value(full) < 1.0)
                    {
                        graphics.SetFillOpacity(op.Value(full));
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

        public Size GetRequiredSizeForLayout(Size available, LayoutContext context, Style appliedstyle)
        {
            PDFPositionOptions pos = appliedstyle.CreatePostionOptions(context.PositionDepth > 0);
            PDFTextRenderOptions opts = appliedstyle.CreateTextOptions();
            PDFImageXObject xobj = this.GetImageObject(context, appliedstyle);
            Size naturalSize = xobj.GetImageSize();

            Unit h;
            Unit w;
            if(pos.Height.HasValue && pos.Width.HasValue)
            {
                w = pos.Width.Value;
                h = pos.Height.Value;
            }
            else if(pos.Width.HasValue)
            {
                w = pos.Width.Value;
                h = naturalSize.Height * (w.PointsValue / naturalSize.Width.PointsValue);

                if (pos.MaximumHeight.HasValue && h > pos.MaximumHeight.Value)
                    h = pos.MaximumHeight.Value;
                if (pos.MinimumHeight.HasValue && h < pos.MinimumHeight.Value)
                    h = pos.MinimumHeight.Value;
            }
            else if(pos.Height.HasValue)
            {
                h = pos.Height.Value;
                w = naturalSize.Width * (h.PointsValue / naturalSize.Height.PointsValue);

                if (pos.FillWidth)
                    w = available.Width;

                if (pos.MaximumWidth.HasValue && w > pos.MaximumWidth.Value)
                    w = pos.MaximumWidth.Value;
                if (pos.MinimumWidth.HasValue && w < pos.MinimumWidth.Value)
                    w = pos.MinimumWidth.Value;
            }
            else if(pos.FillWidth)
            {
                w = available.Width;
                h = naturalSize.Height * (w.PointsValue / naturalSize.Width.PointsValue);

                if (pos.MaximumWidth.HasValue && w > pos.MaximumWidth.Value)
                    w = pos.MaximumWidth.Value;
                if (pos.MinimumWidth.HasValue && w < pos.MinimumWidth.Value)
                    w = pos.MinimumWidth.Value;

                if (pos.MaximumHeight.HasValue && h > pos.MaximumHeight.Value)
                    h = pos.MaximumHeight.Value;
                if (pos.MinimumHeight.HasValue && h < pos.MinimumHeight.Value)
                    h = pos.MinimumHeight.Value;
            }
            else if(pos.MaximumWidth.HasValue)
            {
                if(naturalSize.Width > pos.MaximumWidth.Value)
                {
                    w = pos.MaximumWidth.Value;
                    h = naturalSize.Height * (w.PointsValue / naturalSize.Width.PointsValue);
                }
                else if(pos.MinimumWidth.HasValue && naturalSize.Width < pos.MinimumWidth.Value)
                {
                    w = pos.MinimumWidth.Value;
                    h = naturalSize.Height * (w.PointsValue / naturalSize.Width.PointsValue);

                    if (pos.MinimumHeight.HasValue && h < pos.MinimumHeight.Value)
                        h = pos.MinimumHeight.Value;
                }
                else if(pos.MinimumHeight.HasValue && naturalSize.Height < pos.MinimumHeight.Value)
                {
                    h = pos.MinimumHeight.Value;
                    w = naturalSize.Width * (h.PointsValue / naturalSize.Height.PointsValue);
                }
                else //width is smaller than maximum size
                {
                    w = naturalSize.Width;
                    h = naturalSize.Height;
                }

                if(pos.MaximumHeight.HasValue && h > pos.MaximumHeight.Value)
                {
                    h = pos.MaximumHeight.Value;
                    w = naturalSize.Width * (h.PointsValue / naturalSize.Height.PointsValue);
                }
            }
            else if(pos.MaximumHeight.HasValue)
            {
                if(naturalSize.Height > pos.MaximumHeight.Value)
                {
                    h = pos.MaximumHeight.Value;
                    w = naturalSize.Width * (h.PointsValue / naturalSize.Height.PointsValue);
                }
                else if (pos.MinimumWidth.HasValue && naturalSize.Width < pos.MinimumWidth.Value)
                {
                    w = pos.MinimumWidth.Value;
                    h = naturalSize.Height * (w.PointsValue / naturalSize.Width.PointsValue);

                    if (pos.MinimumHeight.HasValue && h < pos.MinimumHeight.Value)
                        h = pos.MinimumHeight.Value;
                }
                else if (pos.MinimumHeight.HasValue && naturalSize.Height < pos.MinimumHeight.Value)
                {
                    h = pos.MinimumHeight.Value;
                    w = naturalSize.Width * (h.PointsValue / naturalSize.Height.PointsValue);
                }
                else //width is smaller than maximum size
                {
                    w = naturalSize.Width;
                    h = naturalSize.Height;
                }
            }
            else if (pos.MinimumWidth.HasValue)
            {
                w = Unit.Min(naturalSize.Width, available.Width); //either smaller available or natural width
                w = Unit.Max(pos.MinimumWidth.Value, w); //if min is less than w use min

                h = naturalSize.Height * (w.PointsValue / naturalSize.Width.PointsValue);


                if (pos.MinimumHeight.HasValue && h < pos.MinimumHeight.Value)
                {
                    h = pos.MinimumHeight.Value;

                    //we have a new height so try to fit that width
                    var newW = naturalSize.Width * (h.PointsValue / naturalSize.Height.PointsValue);
                    if (newW < pos.MinimumWidth.Value)
                        newW = pos.MinimumWidth.Value;
                    if (newW > available.Width)
                        newW = available.Width;

                    w = newW;

                }
            }
            else if (pos.MinimumHeight.HasValue)
            {
                h = Unit.Min(naturalSize.Height, available.Height); //either smaller available or natural
                h = Unit.Max(pos.MinimumHeight.Value, h);

                w = naturalSize.Width * (h.PointsValue / naturalSize.Height.PointsValue);

                if(w > available.Width) //we cant fit and we dont have an explicit width
                {
                    w = available.Width;

                    h = naturalSize.Height * (w.PointsValue / naturalSize.Width.PointsValue);
                    if (h < pos.MinimumHeight.Value)
                        h = pos.MinimumHeight.Value;
                }

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
                        context.TraceLog.Add(TraceLevel.Warning, "Images", "The image '" + this.ID + "' was reduced beyond the minimum scale reduction of " + this.MinimumScaleReduction + ". Reverting to the natural size");
                        h = naturalSize.Height;
                        w = naturalSize.Width;
                    }
                }
            }
            
            return new Size(w, h);
        }

        public void SetRenderSizes(Rect content, Rect border, Rect total, Style style)
        {
            //Do Nothing
        }

    }
}
