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
using System.Text;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.Components
{
    /// <summary>
    /// All the heading classes (H1,H2..) inherit from this base class which is 
    /// itself a text base class
    /// </summary>
    [PDFParsableComponent("Heading")]
    public abstract class HeadingBase : Panel
    {
        public const string HeadingLabelGroupPrefix = "__scryber_head";
        public const string Heading1LabelGroupName = "__scryber_head1";
        public const string Heading2LabelGroupName = "__scryber_head2";
        public const string Heading3LabelGroupName = "__scryber_head3";
        public const string Heading4LabelGroupName = "__scryber_head4";
        public const string Heading5LabelGroupName = "__scryber_head5";
        public const string Heading6LabelGroupName = "__scryber_head6";
        
        private Label _textlbl = null;
        private string _actualText = string.Empty;
        private string _numbertext;
        private int _headdepth;

        [PDFAttribute("text")]
        [PDFDesignable("Text", Ignore = true)]
        public virtual string Text
        {
            get
            {
                return this._actualText;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    if (null != this._textlbl)
                        this.Contents.Remove(this._textlbl);
                    this._textlbl = null;
                    this._actualText = string.Empty;
                }
                else
                {
                    if (null == this._textlbl)
                    {
                        this._textlbl = new Label();
                        if (this.Contents.Count == 0)
                            this.Contents.Add(this._textlbl);
                        else
                            this.Contents.Insert(0, this._textlbl);
                    }
                    this._textlbl.Text = value;
                    this._actualText = value;
                }
                
            }
        }

        

        public int HeadingDepth
        {
            get { return _headdepth; }
        }

        [PDFElement()]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        

        public HeadingBase(PDFObjectType type, int depth)
            : base(type)
        {
            _headdepth = depth;
        }

        public void SetHeadingNumber(string number)
        {
            if (string.IsNullOrEmpty(number) == false && !number.EndsWith(" "))
                number = number + " ";

            if (null != this._textlbl)
            {
                this._textlbl.Text = string.Concat(number, this._actualText);
            }

            if (!string.IsNullOrEmpty(this.OutlineTitle))
            {
                if (!string.IsNullOrEmpty(this._numbertext) && this.OutlineTitle.StartsWith(_numbertext))
                    this.OutlineTitle = this._actualText;
                this.OutlineTitle = string.Concat(number, this.OutlineTitle);
            }

            _numbertext = number;
        }

        protected override IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, PDFStyle style)
        {
            return new Layout.LayoutEngineHeading(this, parent);
        }

        protected static PDFStyle GetBaseStyles(PDFUnit fontsize, bool bold, bool italic, string groupname)
        {
            PDFStyle fs = new PDFStyle();
            fs.Font.FontSize = fontsize;
            fs.Font.FontBold = bold;
            fs.Font.FontItalic = italic;
            fs.Position.PositionMode = PositionMode.Block;
            fs.Size.FullWidth = true;
            fs.Overflow.Split = OverflowSplit.Never;
            fs.List.NumberingGroup = groupname;
            return fs;
        }

        
    }

    [PDFParsableComponent("H1")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_heading")]
    public class Head1 : HeadingBase
    {

        public Head1()
            : this(PDFObjectTypes.H1)
        {
        }

        protected Head1(PDFObjectType type)
            : base(type, 1)
        {
        }

        protected override PDFStyle GetBaseStyle()
        {
            return HeadingBase.GetBaseStyles(36,true, false, Heading1LabelGroupName);
        }
    }

    [PDFParsableComponent("H2")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_heading")]
    public class Head2 : HeadingBase
    {

        public Head2()
            : this(PDFObjectTypes.H2)
        { }

        protected Head2(PDFObjectType type)
            : base(type, 2)
        {
        }

        protected override PDFStyle GetBaseStyle()
        {
            return HeadingBase.GetBaseStyles(30, true, true, Heading2LabelGroupName);
        }
    }

    [PDFParsableComponent("H3")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_heading")]
    public class Head3 : HeadingBase
    {

        public Head3()
            : this(PDFObjectTypes.H3)
        { }

        protected Head3(PDFObjectType type)
            : base(type, 3)
        {
        }

        protected override PDFStyle GetBaseStyle()
        {
            return HeadingBase.GetBaseStyles(24, true, false, Heading3LabelGroupName);
        }
    }

    [PDFParsableComponent("H4")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_heading")]
    public class Head4 : HeadingBase
    {

        public Head4()
            : this(PDFObjectTypes.H4)
        { }

        protected Head4(PDFObjectType type)
            : base(type, 4)
        {
        }

        protected override PDFStyle GetBaseStyle()
        {
            return HeadingBase.GetBaseStyles(20, true, true, Heading4LabelGroupName);
        }
    }

    [PDFParsableComponent("H5")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_heading")]
    public class Head5 : HeadingBase
    {

        public Head5()
            : this(PDFObjectTypes.H5)
        { }

        protected Head5(PDFObjectType type)
            : base(type, 5)
        {
        }

        protected override PDFStyle GetBaseStyle()
        {
            return HeadingBase.GetBaseStyles(17, true, false, Heading5LabelGroupName);
        }
    }

    [PDFParsableComponent("H6")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_heading")]
    public class Head6 : HeadingBase
    {

        public Head6()
            : this(PDFObjectTypes.H6)
        { }

        protected Head6(PDFObjectType type)
            : base(type, 6)
        {
        }

        protected override PDFStyle GetBaseStyle()
        {
            return HeadingBase.GetBaseStyles(15, true, true, Heading6LabelGroupName);
        }
    }
}
