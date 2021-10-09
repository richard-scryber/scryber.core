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

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// Concrete implementation of a tiling pattern that just 
    /// </summary>
    public class PDFImageTilingPattern : PDFTilingPattern, IResourceContainer
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

        #region public PDFResourceList Resources {get; private set;}

        /// <summary>
        /// Gets the list of resources used by this tiling pattern
        /// </summary>
        public PDFResourceList Resources
        {
            get;
            private set;
        }

        #endregion

        #region public PDFSize ImageSize {get;set;}

        /// <summary>
        /// Gets or sets the size to render the image
        /// </summary>
        public PDFSize ImageSize
        {
            get;
            set;
        }

        #endregion

        #region public IPDFDocument Document

        public IDocument Document
        {
            get { return this.OwningComponent.Document; }
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

            this.Resources = new PDFResourceList(this);
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
        protected override PDFObjectRef DoRenderToPDF(PDFContextBase context, PDFWriter writer)
        {
            IStreamFilter[] filters = writer.DefaultStreamFilters;
            PDFObjectRef pattern = writer.BeginObject();
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Pattern");
            writer.WriteDictionaryNumberEntry("PatternType", (int)this.PatternType);
            writer.WriteDictionaryNumberEntry("PaintType", (int)this.PaintType);
            writer.WriteDictionaryNumberEntry("TilingType", (int)this.TilingType);
            writer.BeginDictionaryEntry("BBox");

            PDFPoint offset = new PDFPoint(this.Start.X, this.Start.Y-this.ImageSize.Height);// this.Start;
            PDFSize size = this.ImageSize;

            PDFSize graphicsSize = new PDFSize(size.Width + offset.X, size.Height + offset.Y);

            writer.WriteArrayRealEntries(true, offset.X.PointsValue,
                                               offset.Y.PointsValue,
                                               offset.X.PointsValue + size.Width.PointsValue,
                                               offset.Y.PointsValue + size.Height.PointsValue);

            writer.EndDictionaryEntry();

            writer.WriteDictionaryRealEntry("XStep", this.Step.Width.PointsValue);
            writer.WriteDictionaryRealEntry("YStep", this.Step.Height.PointsValue);

            PDFObjectRef all = this.Resources.WriteResourceList(context, writer);
            writer.WriteDictionaryObjectRefEntry("Resources", all);
            
            writer.BeginStream(pattern);
            
            using (PDFGraphics g = PDFGraphics.Create(writer, false, this, DrawingOrigin.TopLeft, 
                graphicsSize, context))
            {
                offset = new PDFPoint(offset.X, 0.0);
                g.PaintImageRef(this.Image, size, offset);
            }
            long len = writer.EndStream();

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

        #region IPDFResourceContainer Members



        public string MapPath(string source)
        {
            return this.Container.MapPath(source);
        }

        string IResourceContainer.Register(ISharedResource rsrc)
        {
            if (rsrc is PDFResource pdfrsrc)
                return this.Register(pdfrsrc).Value;
            else
                throw new InvalidCastException("PDFImageTilingPatterns can only use PDFResources");
        }

        PDFName Register(PDFResource rsrc)
        {
            if (null == rsrc.Name || string.IsNullOrEmpty(rsrc.Name.Value))
            {
                string name = this.Document.GetIncrementID(rsrc.Type);
                rsrc.Name = (PDFName)name;
            }
            if (this.Container is IComponent)
                rsrc.RegisterUse(this.Resources, (IComponent)this.Container);
            else
            {
                rsrc.RegisterUse(this.Resources, this.Document);
            }
            //    throw new InvalidCastException(string.Format(Errors.CouldNotCastObjectToType, "IPDFComponent", this.Container.GetType()));

            return rsrc.Name;
        }

        
        #endregion
    }
}
