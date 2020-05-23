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
    [PDFParsableComponent("Fill")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PDFJSConvertor("scryber.studio.design.convertors.styleItem", JSParams = "\"Fill\"")]
    public class PDFFillStyle : PDFStyleItemBase
    {
        
        //
        // constructors
        //

        #region .ctor() + .ctor(type, inherited)

        public PDFFillStyle()
            : base(PDFStyleKeys.FillItemKey)
        {
        }

        #endregion

        //
        // style properties
        //

        #region public PDFColor Color {get;set;} + RemoveColor()

        [PDFAttribute("color")]
        [PDFJSConvertor("scryber.studio.design.convertors.color_css", JSParams = "\"color\"")]
        public PDFColor Color
        {
            get 
            {
                PDFColor col;
                if (this.TryGetValue(PDFStyleKeys.FillColorKey, out col))
                    return col;
                else
                    return Const.DefaultFillColor;
            }
            set { this.SetValue(PDFStyleKeys.FillColorKey, value); }
        }


        public void RemoveColor()
        {
            this.RemoveValue(PDFStyleKeys.FillColorKey);
        }

        #endregion

        #region public string ImageSource {get;set;} + RemoveImageSource()

        [PDFAttribute("img-src")]
        public string ImageSource
        {
            get
            {
                string val;
                if (this.TryGetValue(PDFStyleKeys.FillImgSrcKey,out val))
                    return val;
                else
                    return String.Empty;
            }
            set { this.SetValue(PDFStyleKeys.FillImgSrcKey, value); }
        }

        public void RemoveImageSource()
        {
            this.RemoveValue(PDFStyleKeys.FillImgSrcKey);
        }

        #endregion

        #region public PatternRepeat PatternRepeat {get;set;} + RemovePatternRepeat()

        [PDFAttribute("repeat")]
        public PatternRepeat PatternRepeat
        {
            get
            {
                PatternRepeat rep;
                if (this.TryGetValue(PDFStyleKeys.FillRepeatKey,out rep))
                    return (PatternRepeat)rep;
                else
                    return PatternRepeat.RepeatBoth;
            }
            set
            {
                this.SetValue(PDFStyleKeys.FillRepeatKey, value);
            }
        }

        public void RemovePatternRepeat()
        {
            this.RemoveValue(PDFStyleKeys.FillRepeatKey);
        }

        #endregion

        #region public PDFUnit PatternXPosition {get;set;} + RemovePatternXPosition()

        [PDFAttribute("x-pos")]
        public PDFUnit PatternXPosition
        {
            get
            {
                PDFUnit xpos;
                if (this.TryGetValue(PDFStyleKeys.FillXPosKey,out xpos))
                    return xpos;
                else
                    return 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.FillXPosKey, value);
            }
        }

        public void RemovePatternXPosition()
        {
            this.RemoveValue(PDFStyleKeys.FillXPosKey);
        }

        #endregion

        #region public PDFUnit PatternYPosition {get;set;} + RemovePatternYPosition()

        [PDFAttribute("y-pos")]
        public PDFUnit PatternYPosition
        {
            get
            {
                PDFUnit ypos;
                if (this.TryGetValue(PDFStyleKeys.FillYPosKey, out ypos))
                    return ypos;
                else
                    return 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.FillYPosKey, value);
            }
        }

        public void RemovePatternYPosition()
        {
            this.RemoveValue(PDFStyleKeys.FillYPosKey);
        }

        #endregion

        #region public PDFUnit PatternXStep {get;set;} + RemovePatternXStep()

        [PDFAttribute("x-step")]
        public PDFUnit PatternXStep
        {
            get
            {
                PDFUnit xstep;
                if (this.TryGetValue(PDFStyleKeys.FillXStepKey,out xstep))
                    return xstep;
                else
                    return 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.FillXStepKey, value);
            }
        }

        public void RemovePatternXStep()
        {
            this.RemoveValue(PDFStyleKeys.FillXStepKey);
        }

        #endregion

        #region public PDFUnit PatternYStep {get;set;} + RemovePatternYStep()

        [PDFAttribute("y-step")]
        public PDFUnit PatternYStep
        {
            get
            {
                PDFUnit ystep;
                if (this.TryGetValue(PDFStyleKeys.FillYStepKey,out ystep))
                    return ystep;
                else
                    return 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.FillYStepKey, value);
            }
        }

        public void RemovePatternYStep()
        {
            this.RemoveValue(PDFStyleKeys.FillYStepKey);
        }

        #endregion

        #region public PDFUnit PatternXSize {get;set;} + RemovePatternXSize()

        [PDFAttribute("x-size")]
        public PDFUnit PatternXSize
        {
            get
            {
                PDFUnit xsize;
                if (this.TryGetValue(PDFStyleKeys.FillXSizeKey,out xsize))
                    return xsize;
                else
                    return 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.FillXSizeKey,value);
            }
        }

        public void RemovePatternXSize()
        {
            this.RemoveValue(PDFStyleKeys.FillXSizeKey);
        }

        #endregion

        #region public PDFUnit PatternYSize {get;set;} + RemovePatternYSize()

        [PDFAttribute("y-size")]
        public PDFUnit PatternYSize
        {
            get
            {
                PDFUnit ysize;
                if (this.TryGetValue(PDFStyleKeys.FillYSizeKey, out ysize))
                    return ysize;
                else
                    return 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.FillYSizeKey, value);
            }
        }

        public void RemovePatternYSize()
        {
            this.RemoveValue(PDFStyleKeys.FillYSizeKey);
        }

        #endregion

        #region public FillStyle Style {get; set} + RemoveFillStyle()

        [PDFAttribute("style")]
        public FillStyle FillStyle
        {
            get
            {
                FillStyle found;
                if (this.TryGetValue(PDFStyleKeys.FillStyleKey,out found))
                    return (FillStyle)found;
                else
                    return FillStyle.None;
            }
            set
            {
                this.SetValue(PDFStyleKeys.FillStyleKey, value);
            }
        }

        public void RemoveFillStyle()
        {
            this.RemoveValue(PDFStyleKeys.FillStyleKey);
        }

        #endregion

        #region public PDFReal Opacity {get; set} + RemoveOpacity()

        /// <summary>
        /// Gets or sets the opacity of the fill
        /// </summary>
        [PDFAttribute("opacity")]
        public double Opacity
        {
            get
            {
                double found;
                if (this.TryGetValue(PDFStyleKeys.FillOpacityKey,out found))
                    return found;
                else
                    return 1.0; //Default of 100% opacity
            }
            set
            {
                this.SetValue(PDFStyleKeys.FillOpacityKey,value);
            }
        }

        public void RemoveOpacity()
        {
            this.RemoveValue(PDFStyleKeys.FillOpacityKey);
        }

        #endregion

        //
        // methods
        //

        #region public virtual PDFBrush CreateBrush()

        public virtual PDFBrush CreateBrush()
        {
            return this.AssertOwner().DoCreateFillBrush();
        }

        #endregion

        #region protected static PDFBrush Create(PDFFillStyle Fill)

        /// <summary>
        /// The size of the x step if it is not repeated in the x direction
        /// </summary>
        public static readonly PDFUnit NoXRepeatStepSize = int.MaxValue;

        /// <summary>
        /// The size of the y step if it is not repeated in the y direction
        /// </summary>
        public static readonly PDFUnit NoYRepeatStepSize = int.MaxValue;

        [Obsolete("Just retained for reference",true)]
        protected static PDFBrush Create(PDFFillStyle fill)
        {
            if (fill == null)
                throw new ArgumentNullException("fill");

            FillStyle fillstyle;
            bool fillstyledefined;
            string imgsrc;
            double opacity;
            PDFColor color;

            if (fill.TryGetValue(PDFStyleKeys.FillStyleKey, out fillstyle))
            {
                if (fillstyle == FillStyle.None)
                    return null;
                fillstyledefined = true;
            }
            else
                fillstyledefined = false;

            //If we have an image source and we are set to use an image fill style (or it has not been specified)
            if (fill.TryGetValue(PDFStyleKeys.FillImgSrcKey, out imgsrc) && !string.IsNullOrEmpty(imgsrc) && (fillstyle == FillStyle.Image || fillstyledefined == false))
            {
                PDFImageBrush img = new PDFImageBrush(imgsrc);
                PatternRepeat repeat = fill.PatternRepeat;

                if (repeat == PatternRepeat.RepeatX || repeat == PatternRepeat.RepeatBoth)
                    img.XStep = fill.PatternXStep;
                else
                    img.XStep = NoXRepeatStepSize;

                if (repeat == PatternRepeat.RepeatY || repeat == PatternRepeat.RepeatBoth)
                    img.YStep = fill.PatternYStep;
                else
                    img.YStep = NoYRepeatStepSize;

                img.XPostion = fill.PatternXPosition;
                img.YPostion = fill.PatternYPosition;

                PDFUnit x, y;

                if (fill.TryGetValue(PDFStyleKeys.FillXSizeKey, out x))
                    img.XSize = x;

                if (fill.TryGetValue(PDFStyleKeys.FillYSizeKey, out y))
                    img.YSize = y;

                if (fill.TryGetValue(PDFStyleKeys.FillOpacityKey, out opacity))
                    img.Opacity = opacity;

                return img;
            }

            //if we have a colour and a solid style (or no style)
            else if (fill.TryGetValue(PDFStyleKeys.FillColorKey, out color) && (fill.FillStyle == FillStyle.Solid || fillstyledefined == false))
            {
                PDFSolidBrush solid = new PDFSolidBrush(fill.Color);

                //if we have an opacity set it.
                if (fill.TryGetValue(PDFStyleKeys.FillOpacityKey, out opacity))
                    solid.Opacity = opacity;

                return solid;
            }
            else
                return null;
        }

        #endregion
    }
}
