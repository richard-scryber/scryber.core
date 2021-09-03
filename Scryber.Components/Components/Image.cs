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
using System.IO.IsolatedStorage;
using System.Text;
using Scryber.Drawing;
using Scryber.Native;
using Scryber.Resources;
using Scryber.Styles;

namespace Scryber.Components
{
    [PDFParsableComponent("Image")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_image")]
    public class Image : ImageBase
    {
        private string _src;

        [PDFAttribute("src")]
        [PDFJSConvertor("scryber.studio.design.convertors.imgSource_attr")]
        [PDFDesignable("Source",Category = "General",Type ="ImageSource")]
        public string Source
        {
            get { return _src; }
            set 
            {
                if (value != _src)
                {
                    this.ResetImageObject();
                    _src = value;

                }
                
            }
        }

        private PDFImageData _data;

        [PDFAttribute("img-data")]
        [PDFElement("Data")]
        [PDFDesignable("Image Data", Category = "General", Priority = 3, Type = "Binary")]
        public virtual PDFImageData Data
        {
            get { return _data; }
            set
            {
                _data = value;
                this.ResetImageObject();
                _src = null == _data ? string.Empty : _data.SourcePath;
            }
        }

        
        
        public Image()
            : this(PDFObjectTypes.Image)
        {
        }

        protected Image(PDFObjectType type)
            : base(type)
        {
        }


        protected override Resources.PDFImageXObject InitImageXObject(PDFContextBase context, Style style)
        {
            Document doc = this.Document;
            if (null == doc)
                throw RecordAndRaise.NullReference(Errors.ParentDocumentCannotBeNull);

            PDFImageXObject xobj = null;

            if (null != this.Data)
            {
                xobj = this.Document.GetImageResource(this.Data.SourcePath, this, false);
                if (null == xobj)
                {
                    string name = this.Document.GetIncrementID(PDFObjectTypes.ImageXObject);
                    xobj = PDFImageXObject.Load(this.Data, name);
                    this.Document.SharedResources.Add(xobj);
                }
            }
            else if (String.IsNullOrEmpty(this.Source) == false)
            {
                string fullpath = this.Source;

                try
                {
                    xobj = this.Document.GetImageResource(fullpath, this, true);
                }
                catch (Exception ex)
                {
                    throw new PDFMissingImageException(string.Format(Errors.CouldNotLoadImageFromPath, this.Source), ex);
                }

                if(null == xobj)
                {

                    PDFImageXObject data = null;
                    try
                    {
                        data = this.Document.LoadImageData(this, fullpath);
                    }
                    catch(Exception ex)
                    {
                        throw new PDFMissingImageException(string.Format(Errors.CouldNotLoadImageFromPath, this.Source), ex);
                    }

                    if (null != data)
                    {
                        this.Document.SharedResources.Add(data);
                        xobj = data;
                    }
                    else
                        throw new PDFMissingImageException(string.Format(Errors.CouldNotLoadImageFromPath, this.Source));
                }
            }

                
            return xobj;
        }


    }
}
