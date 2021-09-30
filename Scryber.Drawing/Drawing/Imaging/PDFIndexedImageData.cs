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
using Scryber.PDF.Native;
using Scryber.PDF;

namespace Scryber.Drawing.Imaging
{
    internal class PDFIndexedImageData : PDFImageData
    {

        private string _bytestreamdata = null;//the palette as a string of bytes
        private PDFColor[] _pallette;

        internal PDFColor[] Pallette
        {
            get { return _pallette; }
            set
            {
                _pallette = value;
                _bytestreamdata = null;
            }
        }

        internal PDFIndexedImageData(string uri, int width, int height)
            : base(uri, width, height)
        { }


        /// <summary>
        /// Overrides the default method to write a 4 element array
        /// </summary>
        /// <param name="writer"></param>
        protected override void  RenderCustomColorSpace(PDFWriter writer)
        {
            writer.BeginDictionaryEntry("ColorSpace");
            
            
            if (null == this.Pallette || this.Pallette.Length == 0)
            {
                throw new NullReferenceException("Palette");
            }
            else
            {
                PDFObjectRef index = writer.BeginObject();
                writer.BeginArray();
                writer.BeginArrayEntry();
                writer.WriteName("Indexed");
                writer.EndArrayEntry();

                writer.BeginArrayEntry();
                ColorSpace cs = this.Pallette[0].ColorSpace;

                if (cs == ColorSpace.RGB)
                    writer.WriteName("DeviceRGB");
                else if (cs == ColorSpace.G)
                    writer.WriteName("DeviceG");
                else
                    throw new ArgumentOutOfRangeException("Palette[0].ColorSpace");
                writer.EndArrayEntry();

                writer.BeginArrayEntry();
                writer.WriteNumber(this.Pallette.Length - 1);//maximum value not number of entries
                writer.EndArrayEntry();
                //check the stored instance
                if (null == _bytestreamdata)
                    _bytestreamdata = GetPaletteString(cs);

                writer.BeginArrayEntry();
                writer.WriteByteString(_bytestreamdata);
                writer.EndArrayEntry();
                writer.EndArray();
                writer.EndObject();
                writer.WriteObjectRef(index);
               
            } 
            writer.EndDictionaryEntry();
        }

        private string GetPaletteString(ColorSpace cs)
        {
            StringBuilder bytes = new StringBuilder();
            if (cs == ColorSpace.G)
            {
                foreach (PDFColor c in this.Pallette)
                {
                    if (bytes.Length > 0)
                        bytes.Append(" ");
                    byte g = (byte)c.Gray;
                    bytes.Append(g.ToString("X2"));
                }
            }
            else
            {
                foreach (PDFColor c in this.Pallette)
                {
                    if (bytes.Length > 0)
                        bytes.Append(" ");
                    byte r = (byte)c.Color.R;
                    byte g = (byte)c.Color.G;
                    byte b = (byte)c.Color.B;
                    bytes.Append(r.ToString("X2"));
                    bytes.Append(g.ToString("X2"));
                    bytes.Append(b.ToString("X2"));
                }
            }

            return bytes.ToString();
        }
    }
}
