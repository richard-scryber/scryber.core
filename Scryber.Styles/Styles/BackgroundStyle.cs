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
using Scryber.Drawing;
using Scryber;
using System.ComponentModel;
using Scryber.PDF.Graphics;

namespace Scryber.Styles
{
    [PDFParsableComponent("Background")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PDFJSConvertor("scryber.studio.design.convertors.styleItem", JSParams = "\"background\"")]
    public class BackgroundStyle : StyleItemBase
    {
        

        //
        // constructors
        //

        #region .ctor() + .ctor(type, inherited)

        public BackgroundStyle()
            : base(StyleKeys.BgItemKey)
        {
        }

        #endregion

        //
        // style properties
        //

        #region public PDFColor Color {get;set;} + RemoveColor()

        [PDFAttribute("color")]
        [PDFJSConvertor("scryber.studio.design.convertors.color_css",JSParams = "\"background-color\"")]
        [PDFDesignable("Background Color", Category = "Background", Priority = 1, Type = "Color")]
        public Color Color
        {
            get 
            {
                Color col;
                if (this.TryGetValue(StyleKeys.BgColorKey, out col))
                    return col;
                else
                    return Color.Transparent;
            }
            set { this.SetValue(StyleKeys.BgColorKey, value); }
        }


        public void RemoveColor()
        {
            this.RemoveValue(StyleKeys.BgColorKey);
        }

        #endregion

        #region public string ImageSource {get;set;} + RemoveImageSource()

        [PDFAttribute("img-src")]
        [PDFDesignable("Background Image", Category = "Background", Priority = 2, Type = "ImageSource")]
        [PDFJSConvertor("scryber.studio.design.convertors.imgSource_attr", JSParams = "\"bg-image\"")]
        public string ImageSource
        {
            get
            {
                string val;
                if (this.TryGetValue(StyleKeys.BgImgSrcKey, out val))
                    return val;
                else
                    return String.Empty;
            }
            set { this.SetValue(StyleKeys.BgImgSrcKey, value); }
        }

        public void RemoveImageSource()
        {
            this.RemoveValue(StyleKeys.BgImgSrcKey);
        }

        #endregion

        #region public PatternRepeat PatternRepeat {get;set;} + RemovePatternRepeat()

        [PDFAttribute("repeat")]
        [PDFDesignable("Background Repeat", Category = "Background", Priority = 2, Type = "Select")]
        [PDFJSConvertor("scryber.studio.design.convertors.bgRepeat_attr")]
        public PatternRepeat PatternRepeat
        {
            get
            {
                PatternRepeat rep;
                if (this.TryGetValue(StyleKeys.BgRepeatKey,out rep))
                    return (PatternRepeat)rep;
                else
                    return PatternRepeat.RepeatBoth;
            }
            set
            {
                this.SetValue(StyleKeys.BgRepeatKey, value);
            }
        }

        public void RemovePatternRepeat()
        {
            this.RemoveValue(StyleKeys.BgRepeatKey);
        }

        #endregion

        #region public PDFUnit PatternXPosition {get;set;} + RemovePatternXPosition()

        [PDFAttribute("x-pos")]
        public PDFUnit PatternXPosition
        {
            get
            {
                PDFUnit xpos;
                if (this.TryGetValue(StyleKeys.BgXPosKey,out xpos))
                    return xpos;
                else
                    return 0;
            }
            set
            {
                this.SetValue(StyleKeys.BgXPosKey, value);
            }
        }

        public void RemovePatternXPosition()
        {
            this.RemoveValue(StyleKeys.BgXPosKey);
        }

        #endregion

        #region public PDFUnit PatternYPosition {get;set;} + RemovePatternYPosition()

        [PDFAttribute("y-pos")]
        public PDFUnit PatternYPosition
        {
            get
            {
                PDFUnit ypos;
                if (this.TryGetValue(StyleKeys.BgYPosKey, out ypos))
                    return ypos;
                else
                    return 0;
            }
            set
            {
                this.SetValue(StyleKeys.BgYPosKey, value);
            }
        }

        public void RemovePatternYPosition()
        {
            this.RemoveValue(StyleKeys.BgYPosKey);
        }

        #endregion

        #region public PDFUnit PatternXStep {get;set;} + RemovePatternXStep()

        [PDFAttribute("x-step")]
        public PDFUnit PatternXStep
        {
            get
            {
                PDFUnit xstep;
                if (this.TryGetValue(StyleKeys.BgXStepKey,out xstep))
                    return xstep;
                else
                    return 0;
            }
            set
            {
                this.SetValue(StyleKeys.BgXStepKey, value);
            }
        }

        public void RemovePatternXStep()
        {
            this.RemoveValue(StyleKeys.BgXStepKey);
        }

        #endregion

        #region public PDFUnit PatternYStep {get;set;} + RemovePatternYStep()

        [PDFAttribute("y-step")]
        public PDFUnit PatternYStep
        {
            get
            {
                PDFUnit ystep;
                if (this.TryGetValue(StyleKeys.BgYStepKey,out ystep))
                    return ystep;
                else
                    return 0;
            }
            set
            {
                this.SetValue(StyleKeys.BgYStepKey, value);
            }
        }

        public void RemovePatternYStep()
        {
            this.RemoveValue(StyleKeys.BgYStepKey);
        }

        #endregion

        #region public PDFUnit PatternXSize {get;set;} + RemovePatternXSize()

        [PDFAttribute("x-size")]
        public PDFUnit PatternXSize
        {
            get
            {
                PDFUnit xsize;
                if (this.TryGetValue(StyleKeys.BgXSizeKey,out xsize))
                    return xsize;
                else
                    return 0;
            }
            set
            {
                this.SetValue(StyleKeys.BgXSizeKey,value);
            }
        }

        public void RemovePatternXSize()
        {
            this.RemoveValue(StyleKeys.BgXSizeKey);
        }

        #endregion

        #region public PDFUnit PatternYSize {get;set;} + RemovePatternYSize()

        [PDFAttribute("y-size")]
        public PDFUnit PatternYSize
        {
            get
            {
                PDFUnit ysize;
                if (this.TryGetValue(StyleKeys.BgYSizeKey, out ysize))
                    return ysize;
                else
                    return 0;
            }
            set
            {
                this.SetValue(StyleKeys.BgYSizeKey, value);
            }
        }

        public void RemovePatternYSize()
        {
            this.RemoveValue(StyleKeys.BgYSizeKey);
        }

        #endregion

        #region public FillStyle Style {get; set} + RemoveFillStyle()

        [PDFAttribute("style")]
        public Drawing.FillType FillStyle
        {
            get
            {
                Drawing.FillType found;
                if (this.TryGetValue(StyleKeys.BgStyleKey,out found))
                    return (Drawing.FillType)found;
                else
                    return Drawing.FillType.None;
            }
            set
            {
                this.SetValue(StyleKeys.BgStyleKey, value);
            }
        }

        public void RemoveFillStyle()
        {
            this.RemoveValue(StyleKeys.BgStyleKey);
        }

        #endregion

        #region public PDFReal Opacity {get; set} + RemoveOpacity()

        /// <summary>
        /// Gets or sets the opacity of the fill
        /// </summary>
        [PDFAttribute("opacity")]
        [PDFDesignable("Background Opacity", Ignore = true, Category = "Background", Priority = 3, Type = "Percent")]
        [PDFJSConvertor("scryber.studio.design.convertors.opacity_css", JSParams = "\"bg-opacity\"")]
        public double Opacity
        {
            get
            {
                double found;
                if (this.TryGetValue(StyleKeys.BgOpacityKey,out found))
                    return found;
                else
                    return 1.0; //Default of 100% opacity
            }
            set
            {
                this.SetValue(StyleKeys.BgOpacityKey,value);
            }
        }

        public void RemoveOpacity()
        {
            this.RemoveValue(StyleKeys.BgOpacityKey);
        }

        #endregion

        #region public virtual PDFBrush CreateBrush()

        public virtual PDFBrush CreateBrush()
        {
            return this.AssertOwner().DoCreateBackgroundBrush();
        }


        #endregion
    }
}
