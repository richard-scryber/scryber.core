﻿/*  Copyright 2012 PerceiveIT Limited
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
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;

namespace Scryber.Drawing.Imaging
{
    internal class PDFJpegImageData : PDFBinaryImageData
    {

        public override bool IsPrecompressedData
        {
            get { return true; }
        }

        internal PDFJpegImageData(string source, byte[] data, int width, int height)
            : base(source, width, height)
        {
            this.ColorSpace = ColorSpace.RGB; // ImageFormatParser.GetColorSpace(jpeg.PixelFormat);
            this.ColorsPerSample = 3;// ImageFormatParser.GetColorChannels(this.ColorSpace);
            this.BitsPerColor = 8; // ImageFormatParser.GetImageBitDepth(jpeg.PixelFormat) / this.ColorsPerSample;
            this.Data = data;// ImageFormatParser.GetRawBytesFromImage(jpeg);
            this.Filters = new IStreamFilter[] { new PDFJpegStreamFilter() };
            //TODO: Fix the resolution
            this.HorizontalResolution = 96;
            this.VerticalResolution =  96;

        }

        protected override void RenderImageInformation(ContextBase context, PDFWriter writer)
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
