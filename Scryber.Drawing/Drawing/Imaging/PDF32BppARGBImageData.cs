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
using Scryber;
using Scryber.PDF.Native;
using Scryber.PDF;

namespace Scryber.Drawing.Imaging
{
    internal class PDF32BppARGBImageData : PDFBinaryImageData
    {

        private byte[] _alpha;

        internal byte[] AlphaData
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        private byte[] _filteredAlpha = null;


        public PDF32BppARGBImageData(string uri, int width, int height)
            : base(uri, width, height)
        {
        }

        protected override void RenderImageInformation(ContextBase context, PDFWriter writer)
        {
            base.RenderImageInformation(context, writer);
            if (this.AlphaData != null)
            {
                PDFObjectRef alpha = this.RenderAlpaImageData(context, writer);
                writer.WriteDictionaryObjectRefEntry("SMask", alpha);
            }
        }

        private PDFObjectRef RenderAlpaImageData(ContextBase context, PDFWriter writer)
        {
            context.TraceLog.Add(TraceLevel.Debug, "IMAGE", "Rendering image alpha mask");
            PDFObjectRef mask = writer.BeginObject();
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "XObject");
            writer.WriteDictionaryNameEntry("Subtype", "Image");
            writer.WriteDictionaryNumberEntry("Width", this.PixelWidth);
            writer.WriteDictionaryNumberEntry("Height", this.PixelHeight);
            //writer.WriteDictionaryNumberEntry("Length", this.AlphaData.LongLength);
            writer.WriteDictionaryNameEntry("ColorSpace", "DeviceGray");
            writer.WriteDictionaryNumberEntry("BitsPerComponent", 8);
            this.WriteFilterNames(context, writer);

            //writer.EndDictionary();
            writer.BeginStream(mask);

            byte[] data;
            if (this.HasFilter && this.ShouldApplyFilters(context))
            {
                data = this.GetAlphaFilteredData(this.Filters, context);
                if (null == data)
                {
                    data = this.ApplyFiltersToData(this.AlphaData, context);
                    this.SetAlphaFilteredData(this.Filters, data, context);
                }
            }
            else
                data = this.AlphaData;

            writer.WriteRaw(data, 0, data.Length);

            writer.EndStream();

            //inserted 15/01/15 - Write the filtered length, not the actua image data length to the dictionary.
            writer.WriteDictionaryNumberEntry("Length", data.Length);
            writer.EndDictionary();
            //end of insert

            writer.EndObject();

            return mask;
        }

        //
        // caching filtered data
        //

        public override void ResetFilterCache()
        {
            base.ResetFilterCache();
            this._filteredAlpha = null;
        }

        public byte[] GetAlphaFilteredData(IStreamFilter[] filters, ContextBase context)
        {
            return _filteredAlpha;
        }

        public void SetAlphaFilteredData(IStreamFilter[] filters, byte[] data, ContextBase context)
        {
            if (null == data)
                throw new ArgumentNullException("data");

            this._filteredAlpha = data;
        }
    }
}
