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
using Scryber.Drawing;

namespace Scryber.PDF
{
    /// <summary>
    /// Defines the formatting requirements for document output. e.g. PDF/A PDF/X and variants of these.
    /// </summary>
    public abstract class PDFOutputFormatting :PDFObject
    {
        //delegates

        protected delegate bool ColorSpaceIsSupported(ColorSpace space);


        //ivars

        private ColorSpaceIsSupported _supportedColor;

        //
        // properties
        //

        #region public string Name {get;}

        private string _name;
        /// <summary>
        /// Gets the standard name of this output formatting
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        #endregion

        //
        // .ctor
        //

        #region protected PDFOutputFormatting(PDFObjectType type, string name)

        protected PDFOutputFormatting(ObjectType type, string name, ColorSpaceIsSupported supportedColors)
            : base(type)
        {
            this._name = name;
            this._supportedColor = supportedColors;
        }

        #endregion

        //
        // validation methods
        //


        public bool IsSupportedColorSpace(ColorSpace color)
        {
            return _supportedColor(color);
        }

        public virtual bool IsSupportedImageType(ImageData data)
        {
            return true;
        }

        public virtual bool IsSupportedFont(PDFFont font, out bool requireEmbedding)
        {
            requireEmbedding = false;
            return true;
        }

        public virtual bool IsDocumentCatalogSectionRequired(string feature)
        {
            return false;
        }



        //
        // static methods
        // 

        #region public static PDFOutputFormatting Any {get;}

        /// <summary>
        /// Returns a PDFOutputFormatting instance that supports any of the features - no rules.
        /// </summary>
        public static PDFOutputFormatting Any
        {
            get { return new PDFOutputDocumentAnyFormat(Const.DocFormat_AnyFormatName); }
        }

        #endregion

        #region public static PDFOutputFormatting GetFormat(string name)

        /// <summary>
        /// Gets the appropriate PDFOutputFormatting based on the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PDFOutputFormatting GetFormat(string name)
        {
            PDFOutputFormatting format = null;
            if (string.IsNullOrEmpty(name))
                format = new PDFOutputDocumentAnyFormat(Const.DocFormat_AnyFormatName);

            else if (string.Equals(Const.DocFormat_AnyFormatName, name, StringComparison.OrdinalIgnoreCase))
                format = new PDFOutputDocumentAnyFormat(Const.DocFormat_AnyFormatName);

            else if (string.Equals(Const.DocFormat_NoneFormatname, name, StringComparison.OrdinalIgnoreCase))
                format = new PDFOutputDocumentAnyFormat(Const.DocFormat_NoneFormatname);

            else
                throw new PDFException(string.Format(Errors.UnknownDocumentFormat, name));

            return format;
        }

        #endregion

        // color selector methods
        
        protected static bool AnyColorSpaceIsSupported(ColorSpace colorSpace)
        {
            return true;
        }

        protected static bool ColorSpaceCMYKOnly(ColorSpace colorspace)
        {
            if (colorspace == ColorSpace.CMYK)
                return true;
            else
                return false;
        }

        public static bool ColorSpace_RGB_CMYK_G(ColorSpace colorspace)
        {
            switch (colorspace)
            {
                case ColorSpace.G:
                    return true;
                case ColorSpace.RGB:
                    return true;
                case ColorSpace.CMYK:
                    return true;
                default:
                    return false;
            }
        }

        //
        // inner concrete implementation classes
        // 


        internal class PDFOutputDocumentAnyFormat : PDFOutputFormatting
        {
            public PDFOutputDocumentAnyFormat(string name) :
                base((ObjectType)"FmtA", name, new ColorSpaceIsSupported(AnyColorSpaceIsSupported))
            {
            }
        }


    }

    
   
       
}
