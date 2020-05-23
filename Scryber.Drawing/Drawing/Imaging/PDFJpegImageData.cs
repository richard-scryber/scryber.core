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

namespace Scryber.Drawing.Imaging
{
    internal class PDFJpegImageData : PDFImageData
    {

        public override bool IsPrecompressedData
        {
            get { return true; }
        }

        internal PDFJpegImageData(string source, System.Drawing.Image jpeg)
            : base(source, jpeg.Width, jpeg.Height)
        {
            this.ColorSpace = ImageFormatParser.GetColorSpace(jpeg.PixelFormat);
            this.ColorsPerSample = ImageFormatParser.GetColorChannels(this.ColorSpace);
            this.BitsPerColor = ImageFormatParser.GetImageBitDepth(jpeg.PixelFormat) / this.ColorsPerSample;
            this.Data = ImageFormatParser.GetRawBytesFromImage(jpeg);
            this.Filters = new IStreamFilter[] { new PDFJpegStreamFilter() };
            this.HorizontalResolution = (int)Math.Round(jpeg.HorizontalResolution, 0);
            this.VerticalResolution = (int)Math.Round(jpeg.VerticalResolution, 0);

        }

        protected override void RenderImageInformation(PDFContextBase context, PDFWriter writer)
        {
            base.RenderImageInformation(context, writer);
            writer.WriteDictionaryNumberEntry("ColorTransform", 1);
        }

        /// <summary>
        /// The Jpeg filter stream does not acutally perform filtering as the data is 
        /// already compressed using the DCTDecode method. But it does act as a filter marker
        /// </summary>
        internal class PDFJpegStreamFilter : IStreamFilter
        {
            public string FilterName
            {
                get
                {
                    return "DCTDecode";
                }
                set
                {
                    throw new NotSupportedException("Name of the filter cannot be changed");
                }
            }

            public void FilterStream(System.IO.Stream read, System.IO.Stream write)
            {
                if (!(read is System.IO.MemoryStream))
                    throw new PDFStreamException(Errors.WriteToOnlySupportedForMemoryStreams);
                //if (!(write is System.IO.MemoryStream))
                //    throw new PDFStreamException(Errors.WriteToOnlySupportedForMemoryStreams);
                byte[] input = (read as System.IO.MemoryStream).ToArray();
                byte[] output = FilterStream(input);

                //TODO: Lazy - Use a buffer if length is greater than MaxInt
                if (output.LongLength > (long)int.MaxValue)
                    throw new PDFStreamException("Stream is too long to write in one blob");

                write.Write(output, 0, output.Length);
            }

            public byte[] FilterStream(byte[] orig)
            {
                return orig;
            }
        }
    }


}
