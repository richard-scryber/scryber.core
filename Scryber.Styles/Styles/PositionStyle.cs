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
using System.Threading.Tasks;
using System.ComponentModel;
using Scryber;
using Scryber.Drawing;

namespace Scryber.Styles
{
    [PDFParsableComponent("Position")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PDFJSConvertor("scryber.studio.design.convertors.styleItem", JSParams ="\"Position\"")]
    public class PositionStyle : StyleItemBase
    {

        #region public PositionMode PositionMode {get;set;} + RemovePositionMode()

        [PDFAttribute("mode")]
        [PDFDesignable("Position Mode", Category = "Layout", Priority = 1, Type = "PositionMode")]
        [PDFJSConvertor("scryber.studio.design.convertors.positionMode_css")]
        public PositionMode PositionMode
        {
            get
            {
                PositionMode val;
                if (this.TryGetValue(StyleKeys.PositionModeKey,out val))
                {
                    return val;
                }
                else if (this.IsDefined(StyleKeys.PositionXKey) || this.IsDefined(StyleKeys.PositionYKey))
                    return PositionMode.Relative;
                else
                    return PositionMode.Static;
            }
            set
            {
                this.SetValue(StyleKeys.PositionModeKey,value);
            }
        }

        public DisplayMode DisplayMode
        {
            get
            {
                DisplayMode val;
                if (this.TryGetValue(StyleKeys.PositionDisplayKey, out val))
                    return val;
                else
                {
                    return DisplayMode.Block;
                }
            }
            set
            {
                this.SetValue(StyleKeys.PositionDisplayKey, value);
            }
        }

        public void RemovePositionMode()
        {
            this.RemoveValue(StyleKeys.PositionModeKey);
        }

        public void RemoveDisplayMode()
        {
            this.RemoveValue(StyleKeys.PositionDisplayKey);
        }

        #endregion

        #region public PDFUnit X {get;set;} + RemoveLeft()

        [PDFAttribute("x")]
        [PDFDesignable("X", Ignore = true, Category = "Position", Priority = 1, Type = "PDFUnit")]
        public Unit X
        {
            get
            {
                Unit f;
                if (this.TryGetValue(StyleKeys.PositionXKey,out f))
                    return f;
                else
                    return Unit.Empty;

            }
            set
            {
                this.SetValue(StyleKeys.PositionXKey, value);
            }
        }

        public void RemoveX()
        {
            this.RemoveValue(StyleKeys.PositionXKey);
        }

        #endregion

        #region public PDFUnit Y {get;set;} + RemoveTop()

        [PDFAttribute("y")]
        [PDFDesignable("Y", Ignore = true, Category = "Position", Priority = 1, Type = "PDFUnit")]
        public Unit Y
        {
            get
            {
                Unit f;
                if (this.TryGetValue(StyleKeys.PositionYKey,out f))
                    return f;
                else
                    return Unit.Empty;

            }
            set
            {
                this.SetValue(StyleKeys.PositionYKey, value);
            }
        }

        public void RemoveY()
        {
            this.RemoveValue(StyleKeys.PositionYKey);
        }

        #endregion

        #region public VerticalAlignment VAlign {get;set;} + RemoveVAlign()

        [PDFAttribute("v-align")]
        [PDFJSConvertor("scryber.studio.design.convertors.valign_css")]
        [PDFDesignable("V. Align", Category = "Layout", Priority = 1, Type = "Select")]
        public VerticalAlignment VAlign
        {
            get
            {
                VerticalAlignment va;
                if (this.TryGetValue(StyleKeys.PositionVAlignKey, out va))
                    return va;
                else
                    return Const.DefaultVerticalAlign;
            }
            set
            {
                this.SetValue(StyleKeys.PositionVAlignKey, value);
            }
        }

        public void RemoveVAlign()
        {
            this.RemoveValue(StyleKeys.PositionVAlignKey);
        }

        #endregion

        #region  public HorizontalAlignment HAlign {get;set;} + RemoveHAlign()

        [PDFAttribute("h-align")]
        [PDFJSConvertor("scryber.studio.design.convertors.halign_css")]
        [PDFDesignable("H. Align", Category = "Layout", Priority = 1, Type = "Select")]
        public HorizontalAlignment HAlign
        {
            get
            {
                HorizontalAlignment ha;
                if (this.TryGetValue(StyleKeys.PositionHAlignKey,out ha))
                    return ha;
                else
                    return Const.DefaultHorizontalAlign;
            }
            set
            {
                this.SetValue(StyleKeys.PositionHAlignKey, value);
            }
        }

        public void RemoveHAlign()
        {
            this.RemoveValue(StyleKeys.PositionHAlignKey);
        }

        #endregion

        #region public PDFRect ViewPort {get;set;}

        [PDFAttribute("viewport")]
        public Rect ViewPort
        {
            get
            {
                Rect f;
                if (this.TryGetValue(StyleKeys.PositionViewPort, out f))
                    return f;
                else
                    return Rect.Empty;

            }
            set
            {
                this.SetValue(StyleKeys.PositionViewPort, value);
            }
        }

        public void RemoveViewPort()
        {
            this.RemoveValue(StyleKeys.PositionViewPort);
        }

        #endregion

        #region public FloatMode Float {get;set;}

        [PDFAttribute("float")]
        public FloatMode Float
        {
            get
            {
                FloatMode f;
                if (this.TryGetValue(StyleKeys.PositionFloat, out f))
                    return f;
                else
                    return FloatMode.None;
            }
            set
            {
                this.SetValue(StyleKeys.PositionFloat, value);
            }
        }

        public void RemoveFloat()
        {
            this.RemoveValue(StyleKeys.PositionFloat);
        }

        #endregion

        #region public PDFUnit Right {get;set;} + RemoveRight()

        [PDFAttribute("right")]
        [PDFDesignable("Right", Ignore = true, Category = "Position", Priority = 1, Type = "PDFUnit")]
        public Unit Right
        {
            get
            {
                Unit f;
                if (this.TryGetValue(StyleKeys.PositionRightKey, out f))
                    return f;
                else
                    return Unit.Empty;

            }
            set
            {
                this.SetValue(StyleKeys.PositionRightKey, value);
            }
        }

        public void RemoveRight()
        {
            this.RemoveValue(StyleKeys.PositionRightKey);
        }

        #endregion

        #region public PDFUnit Bottom {get;set;} + RemoveBottom()

        [PDFAttribute("bottom")]
        [PDFDesignable("Bottom", Ignore = true, Category = "Position", Priority = 1, Type = "PDFUnit")]
        public Unit Bottom
        {
            get
            {
                Unit f;
                if (this.TryGetValue(StyleKeys.PositionBottomKey, out f))
                    return f;
                else
                    return Unit.Empty;

            }
            set
            {
                this.SetValue(StyleKeys.PositionBottomKey, value);
            }
        }

        public void RemoveBottom()
        {
            this.RemoveValue(StyleKeys.PositionBottomKey);
        }

        #endregion

        public bool XObject
        {
            get
            {
                bool xobj;
                if (this.TryGetValue(StyleKeys.PositionXObjectKey, out xobj))
                    return xobj;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.PositionXObjectKey, value);
            }
        }

        public void RemoveXObject()
        {
            this.RemoveValue(StyleKeys.PositionXObjectKey);
        }
        
        // obselete legacy properties - moved to Size


        #region public PDFUnit Width {get;set;} + RemoveWidth()

        [PDFAttribute("width")]
        [Obsolete("This has now moved to the PDFSizeStyle",true)]
        [PDFDesignable("Width", Ignore =true,  Category = "Size", Priority = 1, Type = "PDFUnit")]
        public Unit Width
        {
            get
            {
                Unit f;
                if (this.TryGetValue(StyleKeys.SizeWidthKey, out f))
                    return f;
                else
                    return Unit.Empty;

            }
            set
            {
                this.SetValue(StyleKeys.SizeWidthKey, value);
            }
        }

        [Obsolete("This has now moved to the PDFSizeStyle", true)]
        public void RemoveWidth()
        {
            this.RemoveValue(StyleKeys.SizeWidthKey);
        }

        #endregion

        #region public PDFUnit Height {get;set;} + RemoveHeight()

        [PDFAttribute("height")]
        [PDFDesignable("Height", Ignore = true, Category = "Size", Priority = 1, Type = "PDFUnit")]
        [Obsolete("This has now moved to the PDFSizeStyle", true)]
        public Unit Height
        {
            get
            {
                Unit f;
                if (this.TryGetValue(StyleKeys.SizeHeightKey, out f))
                    return f;
                else
                    return Unit.Empty;

            }
            set
            {
                this.SetValue(StyleKeys.SizeHeightKey, value);
            }
        }

        [Obsolete("This has now moved to the PDFSizeStyle", true)]
        public void RemoveHeight()
        {
            this.RemoveValue(StyleKeys.SizeHeightKey);
        }

        #endregion

        #region public bool FullWidth {get;set;}

        /// <summary>
        /// Gets or sets the full width flag. If true then the component will attempt to stretch across the available width
        /// </summary>
        [PDFAttribute("full-width")]
        [Obsolete("This has now moved to the PDFSizeStyle", true)]
        [PDFDesignable("Full Width", Ignore = true, Category = "Size", Priority = 1, Type = "Select")]
        public bool FullWidth
        {
            get
            {
                bool b;
                if (this.TryGetValue(StyleKeys.SizeFullWidthKey, out b))
                    return b;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.SizeFullWidthKey, value);
            }
        }

        [Obsolete("This has now moved to the PDFSizeStyle", true)]
        public void RemoveFillWidth()
        {
            this.RemoveValue(StyleKeys.SizeFullWidthKey);
        }

        #endregion

        public PositionStyle()
            : base(StyleKeys.PositionItemKey)
        {
        }
    }
}
