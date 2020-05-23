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
using Scryber.Native;
using System.ComponentModel;

namespace Scryber.Styles
{
    [PDFParsableComponent("Background")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PDFJSConvertor("scryber.studio.design.convertors.styleItem", JSParams = "\"background\"")]
    public class PDFBackgroundStyle : PDFStyleItemBase
    {
        

        //
        // constructors
        //

        #region .ctor() + .ctor(type, inherited)

        public PDFBackgroundStyle()
            : base(PDFStyleKeys.BgItemKey)
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
        public PDFColor Color
        {
            get 
            {
                PDFColor col;
                if (this.TryGetValue(PDFStyleKeys.BgColorKey, out col))
                    return col;
                else
                    return PDFColor.Transparent;
            }
            set { this.SetValue(PDFStyleKeys.BgColorKey, value); }
        }


        public void RemoveColor()
        {
            this.RemoveValue(PDFStyleKeys.BgColorKey);
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
                if (this.TryGetValue(PDFStyleKeys.BgImgSrcKey, out val))
                    return val;
                else
                    return String.Empty;
            }
            set { this.SetValue(PDFStyleKeys.BgImgSrcKey, value); }
        }

        public void RemoveImageSource()
        {
            this.RemoveValue(PDFStyleKeys.BgImgSrcKey);
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
                if (this.TryGetValue(PDFStyleKeys.BgRepeatKey,out rep))
                    return (PatternRepeat)rep;
                else
                    return PatternRepeat.RepeatBoth;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BgRepeatKey, value);
            }
        }

        public void RemovePatternRepeat()
        {
            this.RemoveValue(PDFStyleKeys.BgRepeatKey);
        }

        #endregion

        #region public PDFUnit PatternXPosition {get;set;} + RemovePatternXPosition()

        [PDFAttribute("x-pos")]
        public PDFUnit PatternXPosition
        {
            get
            {
                PDFUnit xpos;
                if (this.TryGetValue(PDFStyleKeys.BgXPosKey,out xpos))
                    return xpos;
                else
                    return 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BgXPosKey, value);
            }
        }

        public void RemovePatternXPosition()
        {
            this.RemoveValue(PDFStyleKeys.BgXPosKey);
        }

        #endregion

        #region public PDFUnit PatternYPosition {get;set;} + RemovePatternYPosition()

        [PDFAttribute("y-pos")]
        public PDFUnit PatternYPosition
        {
            get
            {
                PDFUnit ypos;
                if (this.TryGetValue(PDFStyleKeys.BgYPosKey, out ypos))
                    return ypos;
                else
                    return 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BgYPosKey, value);
            }
        }

        public void RemovePatternYPosition()
        {
            this.RemoveValue(PDFStyleKeys.BgYPosKey);
        }

        #endregion

        #region public PDFUnit PatternXStep {get;set;} + RemovePatternXStep()

        [PDFAttribute("x-step")]
        public PDFUnit PatternXStep
        {
            get
            {
                PDFUnit xstep;
                if (this.TryGetValue(PDFStyleKeys.BgXStepKey,out xstep))
                    return xstep;
                else
                    return 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BgXStepKey, value);
            }
        }

        public void RemovePatternXStep()
        {
            this.RemoveValue(PDFStyleKeys.BgXStepKey);
        }

        #endregion

        #region public PDFUnit PatternYStep {get;set;} + RemovePatternYStep()

        [PDFAttribute("y-step")]
        public PDFUnit PatternYStep
        {
            get
            {
                PDFUnit ystep;
                if (this.TryGetValue(PDFStyleKeys.BgYStepKey,out ystep))
                    return ystep;
                else
                    return 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BgYStepKey, value);
            }
        }

        public void RemovePatternYStep()
        {
            this.RemoveValue(PDFStyleKeys.BgYStepKey);
        }

        #endregion

        #region public PDFUnit PatternXSize {get;set;} + RemovePatternXSize()

        [PDFAttribute("x-size")]
        public PDFUnit PatternXSize
        {
            get
            {
                PDFUnit xsize;
                if (this.TryGetValue(PDFStyleKeys.BgXSizeKey,out xsize))
                    return xsize;
                else
                    return 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BgXSizeKey,value);
            }
        }

        public void RemovePatternXSize()
        {
            this.RemoveValue(PDFStyleKeys.BgXSizeKey);
        }

        #endregion

        #region public PDFUnit PatternYSize {get;set;} + RemovePatternYSize()

        [PDFAttribute("y-size")]
        public PDFUnit PatternYSize
        {
            get
            {
                PDFUnit ysize;
                if (this.TryGetValue(PDFStyleKeys.BgYSizeKey, out ysize))
                    return ysize;
                else
                    return 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BgYSizeKey, value);
            }
        }

        public void RemovePatternYSize()
        {
            this.RemoveValue(PDFStyleKeys.BgYSizeKey);
        }

        #endregion

        #region public FillStyle Style {get; set} + RemoveFillStyle()

        [PDFAttribute("style")]
        public FillStyle FillStyle
        {
            get
            {
                FillStyle found;
                if (this.TryGetValue(PDFStyleKeys.BgStyleKey,out found))
                    return (FillStyle)found;
                else
                    return FillStyle.None;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BgStyleKey, value);
            }
        }

        public void RemoveFillStyle()
        {
            this.RemoveValue(PDFStyleKeys.BgStyleKey);
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
                if (this.TryGetValue(PDFStyleKeys.BgOpacityKey,out found))
                    return found;
                else
                    return 1.0; //Default of 100% opacity
            }
            set
            {
                this.SetValue(PDFStyleKeys.BgOpacityKey,value);
            }
        }

        public void RemoveOpacity()
        {
            this.RemoveValue(PDFStyleKeys.BgOpacityKey);
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
