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
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;

namespace Scryber.Drawing.Imaging
{
    
    [Obsolete("Use the image factories", true)]
    internal static class ImageFormatParser
    {
        //const PixelFormat pixelFormat32bppCMYK = (PixelFormat)0x200F;
        //const PixelFormat JPEGPixelFormat32bppCMYK = (PixelFormat)(15 | (32 << 8)); //8207

        //internal static ImageParser GetParser(System.Drawing.Image image)
        //{
        //    if (image.RawFormat != null && image.RawFormat.Guid == ImageFormat.Jpeg.Guid)
        //    {
        //        return new ImageParser(GetJpegImageData);
        //    }
        //    else
        //    {
        //        ImageFlags flags = (ImageFlags)image.Flags;
        //        if ((flags & ImageFlags.ColorSpaceCmyk) == ImageFlags.ColorSpaceCmyk)
        //            return new ImageParser(GetCmykImageData);

        //        PixelFormat format = image.PixelFormat;

        //        switch (format)
        //        {
        //            case pixelFormat32bppCMYK:
        //                return new ImageParser(Get32bppCMYKImageData);

        //            //1 bit per pixel bitmap
        //            case PixelFormat.Format1bppIndexed:
        //                return new ImageParser(Get1bppImageData);

        //            //4 bits per pixel indexed
        //            case PixelFormat.Format4bppIndexed:
        //                return new ImageParser(Get4bppIndexedData);

        //            //8 Bits per pixel indexed bitmap
        //            case PixelFormat.Format8bppIndexed:
        //                return new ImageParser(Get8bppIndexedData);

        //            //24 bit RGB image
        //            case PixelFormat.Format24bppRgb:
        //                return new ImageParser(Get24bppRGBImageData);

        //            //32 bit ARGB image
        //            case PixelFormat.Canonical:
        //            case PixelFormat.Format32bppArgb:
        //                return new ImageParser(Get32bppARGBImageData);

        //            //32bit RGB where the last byte is ignored
        //            case PixelFormat.Format32bppRgb:
        //                return new ImageParser(Get32bppRGBImageData);

        //            //Not Supported (currently)
        //            case System.Drawing.Imaging.PixelFormat.Format16bppArgb1555:
        //            case System.Drawing.Imaging.PixelFormat.Format16bppGrayScale:
        //            case System.Drawing.Imaging.PixelFormat.Format16bppRgb555:
        //            case System.Drawing.Imaging.PixelFormat.Format16bppRgb565:
        //            case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
        //            case System.Drawing.Imaging.PixelFormat.Format48bppRgb:
        //            case System.Drawing.Imaging.PixelFormat.Format64bppArgb:
        //            case System.Drawing.Imaging.PixelFormat.Format64bppPArgb:
        //                throw new NotSupportedException("The pixel format '" + format + "' is not supported");
                        


        //            //Not valid format
        //            case System.Drawing.Imaging.PixelFormat.Alpha:
        //            case System.Drawing.Imaging.PixelFormat.PAlpha:
        //            case System.Drawing.Imaging.PixelFormat.Extended:
        //            case System.Drawing.Imaging.PixelFormat.Gdi:
        //            case System.Drawing.Imaging.PixelFormat.Indexed:
        //            case System.Drawing.Imaging.PixelFormat.Max:
        //            case System.Drawing.Imaging.PixelFormat.DontCare:
        //            default:
        //                throw new NotSupportedException("The pixel format '" + format + "' is not valid.");
        //        }
        //    }
            
        //}


        ////
        //// Image Parser Implementations
        ////

        //#region private static PDFImageData GetJpegImageData(string uri, System.Drawing.Image img)
        ///// <summary>
        ///// Gets an instance of the PDFImageData representing the specified JPEG image data
        ///// </summary>
        ///// <param name="uri"></param>
        ///// <param name="img"></param>
        ///// <returns></returns>
        //private static ImageData GetJpegImageData(string uri, System.Drawing.Image img)
        //{
        //    PDFJpegImageData imgdata = new PDFJpegImageData(uri, img);
        //    return imgdata;
        //}

        //#endregion


        //#region private static PDFImageData Get1bppImageData(string uri, System.Drawing.Image image)

        ///// <summary>
        ///// Returns a new 1 bit per pixel (Black and White) image data instance
        ///// </summary>
        ///// <param name="uri"></param>
        ///// <param name="image"></param>
        ///// <returns></returns>
        //private static ImageData Get1bppImageData(string uri, System.Drawing.Image image)
        //{
        //    Bitmap bmp = (Bitmap)image;
        //    Rectangle bounds = new Rectangle(0, 0, image.Width, image.Height);

        //    byte[] all;
        //    int width;
        //    int height;
        //    int datawidth;

        //    unsafe
        //    {
        //        BitmapData data = bmp.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format1bppIndexed);

        //        try
        //        {
        //            //Image data is aligned to 4 bytes on the Bitmap
        //            //PDF Image data is aligned on the byte.
        //            //So we need a byte[] that is modulo 8 in the image data, but read from a modulo 32.

        //            int modulo = bmp.Width % 32;

        //            if (modulo == 0)
        //                width = bmp.Width / 8;
        //            else
        //                width = (bmp.Width + (32 - modulo)) / 8;

        //            height = bmp.Height;

        //            //Calculate the data width as byte aligned
        //            modulo = bmp.Width % 8;
        //            if (modulo == 0)
        //                datawidth = bmp.Width / 8;
        //            else
        //                datawidth = (bmp.Width + (8 - modulo)) / 8;

        //            int stride = data.Stride;//total number of bytes in 1 row
        //            int step = 1;//number of bytes per pixel

        //            int size = datawidth * height;

        //            int offset = stride - (width * step);//remainder in a row as padding

        //            all = new byte[size];

        //            fixed (byte* dest = all)
        //            {
        //                byte* destptr = dest;
        //                byte* imgptr = (byte*)data.Scan0;

        //                for (int y = 0; y < height; y++)
        //                {
        //                    for (int x = 0; x < width; x++)
        //                    {
        //                        //skip the extra bytes in a doubleword padded array
        //                        if (x < datawidth)
        //                        {
        //                            *destptr = *imgptr;
        //                            destptr++;
        //                        }
        //                        imgptr++;
        //                    }
        //                    imgptr += offset;
        //                }

        //            }
        //        }
        //        finally
        //        {
        //            bmp.UnlockBits(data);
        //        }
        //    }

        //    PDF1BppImageData imgdata = new PDF1BppImageData(uri, image.Width, image.Height);
        //    imgdata.BytesPerLine = datawidth;
        //    imgdata.BitsPerColor = 1;
        //    imgdata.ColorsPerSample = 1;
        //    imgdata.ColorSpace = ColorSpace.G;
        //    imgdata.Data = all;
        //    imgdata.HorizontalResolution = (int)Math.Round(image.HorizontalResolution, 0);
        //    imgdata.VerticalResolution = (int)Math.Round(image.VerticalResolution, 0);
        //    imgdata.Filters = GetImageDataFilters();
        //    return imgdata;
        //}

        //#endregion

        //#region private static PDFImageData Get4bppIndexedData(string uri, System.Drawing.Image image)

        ///// <summary>
        ///// Gets a new PDFImageData instance from a 4 bit per pixel indexed image
        ///// </summary>
        ///// <param name="uri"></param>
        ///// <param name="image"></param>
        ///// <returns></returns>
        //private static ImageData Get4bppIndexedData(string uri, System.Drawing.Image image)
        //{
        //    Bitmap bmp = (Bitmap)image;
        //    Rectangle bounds = new Rectangle(0, 0, image.Width, image.Height);
        //    byte[] all;
        //    int width;
        //    int height;
        //    int datawidth;
        //    Color[] index;

        //    unsafe
        //    {
        //        BitmapData data = bmp.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format4bppIndexed);
        //        try
        //        {
        //            int stride = data.Stride;
        //            width = image.Width / 2;
        //            height = image.Height;
        //            datawidth = image.Width;//store the values as bytes not half bytes


        //            int size = datawidth * height;

        //            int offset = stride - width;//remainder in a row as padding

        //            all = new byte[datawidth * height];

        //            fixed (byte* dest = all)
        //            {
        //                byte* destptr = dest;
        //                byte* imgptr = (byte*)data.Scan0;

        //                for (int y = 0; y < height; y++)
        //                {
        //                    for (int x = 0; x < width; x++)
        //                    {
        //                        byte one = *imgptr;
        //                        byte two = *imgptr;

        //                        one = (byte)((int)one >> 4);//4 high bits
        //                        two = (byte)(two & 15); //low 4 bits
        //                        *destptr = one;
        //                        destptr++;

        //                        *destptr = two;
        //                        destptr++;

        //                        imgptr++;
        //                    }
        //                    imgptr += offset;
        //                }
        //            }
        //            index = GetPDFPalette(bmp.Palette);
        //        }
        //        finally
        //        {
        //            bmp.UnlockBits(data);
        //        }
        //    }

        //    PDFIndexedImageData imgdata = new PDFIndexedImageData(uri, bmp.Width, bmp.Height);
        //    imgdata.BitsPerColor = 8;
        //    imgdata.ColorsPerSample = 1;
        //    imgdata.Data = all;
        //    imgdata.ColorSpace = ColorSpace.Custom;
        //    imgdata.Pallette = index;
        //    imgdata.HorizontalResolution = (int)Math.Round(image.HorizontalResolution, 0);
        //    imgdata.VerticalResolution = (int)Math.Round(image.VerticalResolution, 0);
        //    imgdata.Filters = GetImageDataFilters();
        //    return imgdata;
        //}

        //#endregion

        //#region private static PDFImageData Get8bppIndexedData(string uri, System.Drawing.Image image)
        ///// <summary>
        ///// Gets a PDFImageData instance representing the 8 bits per pixel image
        ///// </summary>
        ///// <param name="uri"></param>
        ///// <param name="image"></param>
        ///// <returns></returns>
        //private static ImageData Get8bppIndexedData(string uri, System.Drawing.Image image)
        //{
        //    Bitmap bmp = (Bitmap)image;
        //    Rectangle bounds = new Rectangle(0, 0, image.Width, image.Height);
        //    byte[] all;
        //    int width;
        //    int height;
        //    int datawidth;
        //    Color[] index;

        //    unsafe
        //    {
        //        BitmapData data = bmp.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
        //        try
        //        {
        //            int stride = data.Stride;
        //            width = image.Width;
        //            height = image.Height;
        //            datawidth = image.Width;//store the values as bytes not half bytes


        //            int size = datawidth * height;

        //            int offset = stride - width;//remainder in a row as padding

        //            all = new byte[datawidth * height];

        //            fixed (byte* dest = all)
        //            {
        //                byte* destptr = dest;
        //                byte* imgptr = (byte*)data.Scan0;

        //                for (int y = 0; y < height; y++)
        //                {
        //                    for (int x = 0; x < width; x++)
        //                    {
        //                        byte one = *imgptr;
        //                        *destptr = one;
        //                        destptr++;
        //                        imgptr++;
        //                    }
        //                    imgptr += offset;
        //                }
        //            }
        //            index = GetPDFPalette(bmp.Palette);
        //        }
        //        finally
        //        {
        //            bmp.UnlockBits(data);
        //        }
        //    }

        //    PDFIndexedImageData imgdata = new PDFIndexedImageData(uri, bmp.Width, bmp.Height);
        //    imgdata.BitsPerColor = 8;
        //    imgdata.ColorsPerSample = 1;
        //    imgdata.Data = all;
        //    imgdata.ColorSpace = ColorSpace.Custom;
        //    imgdata.Pallette = index;
        //    imgdata.HorizontalResolution = (int)Math.Round(image.HorizontalResolution, 0);
        //    imgdata.VerticalResolution = (int)Math.Round(image.VerticalResolution, 0);
        //    imgdata.Filters = GetImageDataFilters();
        //    return imgdata;
        //}

        //#endregion

        //#region  private static PDFImageData Get24bppRGBImageData(string uri, System.Drawing.Image image)

        ///// <summary>
        ///// Gets a PDFImageData instance that represents the 24 bit per pixel RGB Image
        ///// </summary>
        ///// <param name="uri"></param>
        ///// <param name="image"></param>
        ///// <returns></returns>
        //private static ImageData Get24bppRGBImageData(string uri, System.Drawing.Image image)
        //{
        //    Bitmap bmp = (Bitmap)image;
        //    Rectangle bounds = new Rectangle(0, 0, image.Width, image.Height);

        //    byte[] all;

        //    unsafe
        //    {
        //        BitmapData data = bmp.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

        //        try
        //        {
        //            int width = bmp.Width;
        //            int height = bmp.Height;

        //            int stride = data.Stride;//total number of bytes in 1 row
        //            int step = 3;//number of bytes per pixel
        //            int offset = stride - (width * step);//remainder in a row as padding
        //            all = new byte[width * height * step];

        //            fixed (byte* dest = all)
        //            {
        //                byte* destptr = dest;
        //                byte* imgptr = (byte*)data.Scan0;

        //                for (int y = 0; y < data.Height; y++)
        //                {
        //                    for (int x = 0; x < data.Width; x++)
        //                    {
        //                        //order for RGB is actually Blue, Green, Red

        //                        byte b = *imgptr;//blue
        //                        imgptr++;
        //                        byte g = *imgptr;//green
        //                        imgptr++;
        //                        byte r = *imgptr;//red
        //                        imgptr++;
        //                        *destptr = r;
        //                        destptr++;
        //                        *destptr = g;
        //                        destptr++;
        //                        *destptr = b;
        //                        destptr++;
        //                    }
        //                    imgptr += offset;
        //                }
        //            }

        //        }
        //        finally
        //        {
        //            bmp.UnlockBits(data);
        //        }
        //    }

        //    PDF24BppRGBImageData imgdata = new PDF24BppRGBImageData(uri, image.Width, image.Height);
        //    imgdata.BitsPerColor = 8;
        //    imgdata.ColorsPerSample = 3;
        //    imgdata.ColorSpace = ColorSpace.RGB;
        //    imgdata.Data = all;
        //    imgdata.HorizontalResolution = (int)Math.Round(image.HorizontalResolution, 0);
        //    imgdata.VerticalResolution = (int)Math.Round(image.VerticalResolution, 0);
        //    imgdata.Filters = GetImageDataFilters();
        //    return imgdata;
        //}

        //#endregion

        //#region private static PDFImageData Get32bppRGBImageData(string uri, System.Drawing.Image image)

        ///// <summary>
        ///// Gets a PDFImageData instance representing the 32 bits per pixel RGB image
        ///// </summary>
        ///// <param name="uri"></param>
        ///// <param name="image"></param>
        ///// <returns></returns>
        //private static ImageData Get32bppRGBImageData(string uri, System.Drawing.Image image)
        //{
        //    Bitmap bmp = (Bitmap)image;
        //    Rectangle bounds = new Rectangle(0, 0, image.Width, image.Height);

        //    byte[] all;

        //    unsafe
        //    {
        //        BitmapData data = bmp.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
        //        try
        //        {
        //            int width = bmp.Width;
        //            int height = bmp.Height;
        //            int stride = data.Stride;//total number of bytes in 1 row
        //            int step = 4;//number of bytes per pixel
        //            int offset = stride - (width * step);//remainder in a row as padding

        //            //create the arrays to hold the data
        //            all = new byte[width * height * 3];

        //            fixed (byte* dest = all)
        //            {
        //                byte* destptr = dest;

        //                byte* imgptr = (byte*)data.Scan0;

        //                for (int y = 0; y < data.Height; y++)
        //                {
        //                    for (int x = 0; x < data.Width; x++)
        //                    {
        //                        //order for ARGB is actually Blue, Green, Red, Alpha
        //                        byte b = *imgptr;//blue
        //                        imgptr++;
        //                        byte g = *imgptr;//green
        //                        imgptr++;
        //                        byte r = *imgptr;//red
        //                        imgptr++;
        //                        byte a = *imgptr;//alpha - ignore
        //                        imgptr++;

        //                        *destptr = r;
        //                        destptr++;
        //                        *destptr = g;
        //                        destptr++;
        //                        *destptr = b;
        //                        destptr++;
        //                    }
        //                    imgptr += offset;
        //                }
        //            }
        //        }
        //        finally
        //        {
        //            bmp.UnlockBits(data);
        //        }
        //    }

        //    PDF24BppRGBImageData imgdata = new PDF24BppRGBImageData(uri, image.Width, image.Height);
        //    imgdata.BitsPerColor = 8;
        //    imgdata.ColorsPerSample = 3;
        //    imgdata.Data = all;
        //    imgdata.HorizontalResolution = (int)Math.Round(image.HorizontalResolution, 0);
        //    imgdata.VerticalResolution = (int)Math.Round(image.VerticalResolution, 0);
        //    imgdata.Filters = GetImageDataFilters();
        //    return imgdata;
        //}

        //#endregion

        //#region private static PDFImageData Get32bppARGBImageData(string uri, System.Drawing.Image image)

        ///// <summary>
        ///// Gets a new PDFImageData instance representing the 32 bits per pixel ARGB image
        ///// </summary>
        ///// <param name="uri"></param>
        ///// <param name="image"></param>
        ///// <returns></returns>
        //private static ImageData Get32bppARGBImageData(string uri, System.Drawing.Image image)
        //{
        //    Bitmap bmp = (Bitmap)image;
        //    Rectangle bounds = new Rectangle(0, 0, image.Width, image.Height);

        //    byte[] all;
        //    byte[] alpha;
        //    unsafe
        //    {
        //        BitmapData data = bmp.LockBits(bounds, ImageLockMode.ReadOnly, bmp.PixelFormat);
        //        try
        //        {
        //            int width = bmp.Width;
        //            int height = bmp.Height;
        //            int stride = data.Stride;//total number of bytes in 1 row
        //            int step = 4;//number of bytes per pixel
        //            int offset = stride - (width * step);//remainder in a row as padding

        //            //create the arrays to hold the data
        //            all = new byte[width * height * 3];
        //            alpha = new byte[width * height];
        //            fixed (byte* dest = all, destalpha = alpha)
        //            {
        //                byte* destptr = dest;
        //                byte* alphaptr = destalpha;

        //                byte* imgptr = (byte*)data.Scan0;

        //                for (int y = 0; y < data.Height; y++)
        //                {
        //                    for (int x = 0; x < data.Width; x++)
        //                    {
        //                        //order for ARGB is actually Blue, Green, Red, Alpha
        //                        byte b = *imgptr;//blue
        //                        imgptr++;
        //                        byte g = *imgptr;//green
        //                        imgptr++;
        //                        byte r = *imgptr;//red
        //                        imgptr++;
        //                        byte a = *imgptr;//alpha
        //                        imgptr++;

        //                        *destptr = r;
        //                        destptr++;
        //                        *destptr = g;
        //                        destptr++;
        //                        *destptr = b;
        //                        destptr++;

        //                        *alphaptr = a;
        //                        alphaptr++;
        //                    }
        //                    imgptr += offset;
        //                }
        //            }
        //        }
        //        finally
        //        {
        //            bmp.UnlockBits(data);
        //        }
        //    }

        //    PDF32BppARGBImageData imgdata = new PDF32BppARGBImageData(uri, image.Width, image.Height);
        //    imgdata.BitsPerColor = 8;
        //    imgdata.ColorsPerSample = 3;
        //    imgdata.Data = all;
        //    imgdata.HorizontalResolution = (int)Math.Round(image.HorizontalResolution, 0);
        //    imgdata.VerticalResolution = (int)Math.Round(image.VerticalResolution, 0);
        //    imgdata.AlphaData = alpha;
        //    imgdata.Filters = GetImageDataFilters();
        //    return imgdata;
        //}

        //#endregion

        //#region private static PDFImageData Get32bppCMYKImageData(string uri, System.Drawing.Image image)

        ///// <summary>
        ///// Returns a new image data instance representing
        ///// a 32 bits per pixel CMYK image (8 bits per colour).
        ///// </summary>
        ///// <param name="uri"></param>
        ///// <param name="image"></param>
        ///// <returns></returns>
        //private static ImageData Get32bppCMYKImageData(string uri, System.Drawing.Image image)
        //{
        //    Bitmap bmp = (Bitmap)image;
        //    Rectangle bounds = new Rectangle(0, 0, image.Width, image.Height);

        //    byte[] all;

        //    unsafe
        //    {
        //        BitmapData data = bmp.LockBits(bounds, ImageLockMode.ReadOnly, bmp.PixelFormat);
        //        try
        //        {
        //            int width = bmp.Width;
        //            int height = bmp.Height;
        //            int stride = data.Stride;//total number of bytes in 1 row
        //            int step = 4;//number of bytes per pixel
        //            int offset = stride - (width * step);//remainder in a row as padding

        //            //create the arrays to hold the data
        //            all = new byte[width * height * 4];
        //            //alpha = new byte[width * height];
        //            fixed (byte* dest = all)
        //            {
        //                byte* destptr = dest;
                        

        //                byte* imgptr = (byte*)data.Scan0;

        //                for (int y = 0; y < data.Height; y++)
        //                {
        //                    for (int x = 0; x < data.Width; x++)
        //                    {
        //                        //order for ARGB is actually Blue, Green, Red, Alpha
        //                        byte c = *imgptr;//blue
        //                        imgptr++;
        //                        byte m = *imgptr;//green
        //                        imgptr++;
        //                        byte yel = *imgptr;//red
        //                        imgptr++;
        //                        byte k = *imgptr;//alpha
        //                        imgptr++;

        //                        *destptr = c;
        //                        destptr++;
        //                        *destptr = m;
        //                        destptr++;
        //                        *destptr = yel;
        //                        destptr++;
        //                        *destptr = k;
        //                        destptr++;
        //                    }
        //                    imgptr += offset;
        //                }
        //            }
        //        }
        //        finally
        //        {
        //            bmp.UnlockBits(data);
        //        }
        //    }

        //    PDF32BppCMYKImageData imgdata = new PDF32BppCMYKImageData(uri, image.Width, image.Height);
        //    imgdata.BitsPerColor = 8;
        //    imgdata.ColorsPerSample = 4;
        //    imgdata.Data = all;
        //    imgdata.HorizontalResolution = (int)Math.Round(image.HorizontalResolution, 0);
        //    imgdata.VerticalResolution = (int)Math.Round(image.VerticalResolution, 0);
        //    imgdata.Filters = GetImageDataFilters();
        //    return imgdata;
        //}

        //private static ImageData GetCmykImageData(string uri, System.Drawing.Image image)
        //{
        //    return Get32bppCMYKImageData(uri, image);
        //}

        //#endregion

        ////
        //// support methods
        ////

        
        //#region internal static byte[] GetRawBytesFromImage(System.Drawing.Image bmp)

        ///// <summary>
        ///// Returns a byte[] of the raw image data (as saved) from the bitmap image.
        ///// </summary>
        ///// <param name="bmp"></param>
        ///// <returns></returns>
        //internal static byte[] GetRawBytesFromImage(System.Drawing.Image bmp)
        //{
        //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        //    {
        //        bmp.Save(ms, bmp.RawFormat);
        //        return ms.ToArray();
        //    }
        //}

        //#endregion

        //#region internal static int GetDepth(System.Drawing.Imaging.PixelFormat pixelFormat)

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pixelFormat"></param>
        ///// <returns></returns>
        //internal static int GetImageBitDepth(System.Drawing.Imaging.PixelFormat pixelFormat)
        //{
            
        //    int i;
        //    switch (pixelFormat)
        //    {
        //        case System.Drawing.Imaging.PixelFormat.Format16bppArgb1555:
        //        case System.Drawing.Imaging.PixelFormat.Format16bppGrayScale:
        //        case System.Drawing.Imaging.PixelFormat.Format16bppRgb555:
        //        case System.Drawing.Imaging.PixelFormat.Format16bppRgb565:
        //            i = 16;
        //            break;
        //        case System.Drawing.Imaging.PixelFormat.Format1bppIndexed:
        //            i = 1;
        //            break;
        //        case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
        //            i = 24;
        //            break;
        //        case System.Drawing.Imaging.PixelFormat.Canonical:
        //        case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
        //        case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
        //        case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
        //            i = 32;
        //            break;
        //        case System.Drawing.Imaging.PixelFormat.Format48bppRgb:
        //            i = 48;
        //            break;
        //        case System.Drawing.Imaging.PixelFormat.Format4bppIndexed:
        //            i = 4;
        //            break;
        //        case System.Drawing.Imaging.PixelFormat.Format64bppArgb:
        //        case System.Drawing.Imaging.PixelFormat.Format64bppPArgb:
        //            i = 64;
        //            break;
        //        case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
        //            i = 8;
        //            break;
        //        case JPEGPixelFormat32bppCMYK:
        //            i = 32;
        //            break;
        //        case System.Drawing.Imaging.PixelFormat.Alpha:
        //        case System.Drawing.Imaging.PixelFormat.DontCare:
        //        case System.Drawing.Imaging.PixelFormat.Extended:
        //        case System.Drawing.Imaging.PixelFormat.Gdi:
        //        case System.Drawing.Imaging.PixelFormat.Indexed:
        //        case System.Drawing.Imaging.PixelFormat.Max:
        //        case System.Drawing.Imaging.PixelFormat.PAlpha:
        //        default:
        //            i = -1;
        //            break;
        //    }
        //    return i;
        //}


        //#endregion

        //#region internal static ColorSpace GetColorSpace(System.Drawing.Imaging.PixelFormat pixelFormat)

        //internal static ColorSpace GetColorSpace(PixelFormat pixelFormat)
        //{
        //    if (pixelFormat == PixelFormat.Alpha
        //        || pixelFormat == PixelFormat.Format16bppGrayScale)
        //        return ColorSpace.G;
        //    else if (pixelFormat == pixelFormat32bppCMYK || pixelFormat == JPEGPixelFormat32bppCMYK)
        //        return ColorSpace.CMYK;
        //    else
        //        return ColorSpace.RGB;
        //}

        //#endregion

        //internal static int GetColorChannels(ColorSpace cs)
        //{
        //    int channels;
        //    switch (cs)
        //    {
        //        case ColorSpace.G:
        //            channels = 1;
        //            break;
        //        case ColorSpace.RGB:
        //        case ColorSpace.HSL:
        //        case ColorSpace.LAB:
        //            channels = 3;
        //            break;
        //        case ColorSpace.CMYK:
        //            channels = 4;
        //            break;
        //        case ColorSpace.Custom:
        //            channels = 1;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException("cs");
        //    }
        //    return channels;
        //}

        //#region private static PDFColor[] GetPDFPalette(ColorPalette palette) + 1 overload

        //private static Color[] GetPDFPalette(ColorPalette palette)
        //{
        //    ColorSpace cspace;
        //    if ((palette.Flags & 2) > 0)
        //        cspace = ColorSpace.G;

        //    else
        //        cspace = ColorSpace.RGB;

        //    return GetPDFPalette(cspace, palette.Entries);
        //}

        //private static Color[] GetPDFPalette(ColorSpace cspace, System.Drawing.Color[] colors)
        //{
        //    if (null == colors)
        //        throw new ArgumentNullException("colors");
        //    Color[] all = new Color[colors.Length];

        //    if (cspace == ColorSpace.G)
        //    {
        //        for (int i = 0; i < colors.Length; i++)
        //        {
        //            var c = colors[i];
        //            all[i] = new Color(c.R, c.G, c.B).ToGray();
        //        }
        //    }
        //    else if (cspace == ColorSpace.CMYK)
        //    {
        //        for (int i = 0; i < colors.Length; i++)
        //        {
        //            var c = colors[i];
        //            all[i] = new Color(c.R, c.G, c.B).ToCMYK();
        //        }
        //    }
        //    else if (cspace == ColorSpace.RGB)
        //    {
        //        for (int i = 0; i < colors.Length; i++)
        //        {
        //            var c = colors[i];
        //            all[i] = new Color(c.R, c.G, c.B);
        //        }
        //    }
        //    else
        //        throw new ArgumentOutOfRangeException(nameof(cspace));
            
        //    return all;
        //}

        //#endregion

        //#region private static IStreamFilter[] GetImageDataFilters()

        ///// <summary>
        ///// Returns an array of stream filters with the FlateDecode filter in there
        ///// </summary>
        ///// <returns></returns>
        //private static IStreamFilter[] GetImageDataFilters()
        //{
        //    return new IStreamFilter[] { PDFStreamFilters.FlateDecode };
        //}

        //#endregion
    }
}
