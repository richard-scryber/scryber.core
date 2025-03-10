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
using System.Threading;

namespace Scryber.Drawing
{
    /// <summary>
    /// Defines the type of Color - Gray, RGB etc.
    /// </summary>
    public enum ColorSpace : byte
    {
        None = 0,
        G,
        RGB,
        HSL,
        LAB,
        CMYK,
        Custom
    }

    public enum ImageFormat
    {
        Png,
        Jpeg,
        Tiff,
        Gif,
        Bitmap
    }

    /// <summary>
    /// Available page units
    /// </summary>
    public enum PageUnits
    {
        //Reserved 0 for empty units
        Points = 0,
        Millimeters = 1,
        Inches = 2,
        Pixel = 3,
        //relative break point
        Percent = 4,
        EMHeight = 5,
        EXHeight = 6,
        ZeroWidth = 7,
        RootEMHeight = 8,
        ViewPortWidth = 9,
        ViewPortHeight = 10,
        ViewPortMin = 11,
        ViewPortMax = 12,
    }

    

    

    /// <summary>
    /// The style of fill
    /// </summary>
    public enum FillType
    {
        None,
        Solid,
        Pattern,
        Image
    }


    [Flags()]
    public enum Sides
    {
        Top = 1,
        Left = 2,
        Bottom = 4,
        Right = 8
    }

    public enum LineType
    {
        None,
        Solid,
        Dash,
        Pattern
    }

    public enum Quadrants
    {
        TopLeft = 1,
        TopRight = 2,
        BottomLeft = 4,
        BottomRight = 8
    }

    public enum LineCaps
    {
        Butt = 0,
        Round = 1,
        Projecting = 2,
        Square = 2
    }


    public enum LineJoin
    {
        Mitre = 0,
        Round = 1,
        Bevel = 2
    }

    public enum GraphicFillMode
    {
        EvenOdd,
        Winding
    }

    public enum StandardFont
    {
        Helvetica,
        Times,
        Courier,
        Symbol,
        Zapf_Dingbats
    }


    public enum FontStyle
    {
        Regular = 0,
        Italic = 2,
        Oblique = 3
    }

    public enum ListNumberingGroupStyle
    {
        None,
        Decimals,
        UppercaseRoman,
        LowercaseRoman,
        UppercaseLetters,
        LowercaseLetters,
        Bullet,
        Labels
        //[Obsolete("The labels and images are not currently supported", false)]
        //Image
    }

    public static class FontWeights
    {
        /// <summary>
        /// Thin weight = 100
        /// </summary>
        public static readonly int Thin = 100;
        /// <summary>
        /// ExtraLight = 200
        /// </summary>
        public static readonly int ExtraLight = 200;
        /// <summary>
        /// Light = 300
        /// </summary>
        public static readonly int Light = 300;
        /// <summary>
        /// Regular (aka Normal) = 400
        /// </summary>
        public static readonly int Regular = 400;
        /// <summary>
        /// Medium = 500
        /// </summary>
        public static readonly int Medium = 500;
        /// <summary>
        /// SemiBold = 600
        /// </summary>
        public static readonly int SemiBold = 600;
        /// <summary>
        /// Bold = 700
        /// </summary>
        public static readonly int Bold = 700;
        /// <summary>
        /// ExtraBold = 800
        /// </summary>
        public static readonly int ExtraBold = 800;
        /// <summary>
        /// Black = 900
        /// </summary>
        public static readonly int Black = 900;
    }

    public enum FontType
    {
        Type1,
        Type0,
        Type3,
        TrueType
    }

    public enum FontStretch
    {
        UltraCondensed,
        ExtraCondensed,
        Condensed,
        SemiCondensed,
        Normal,
        SemiExpanded,
        Expanded,
        ExtraExpanded,
        UltraExpanded
    }

    [Flags()]
    public enum FontFlags
    {
        FixedPitch = 1,
        Serif = 2,
        Symbolic = 4,
        Script = 8,
        NonSymbolic = 32,
        Italic = 64,
        AllCap = 65536,
        SmallCap = 131072,
        Bold = 262144
    }

    public enum FontSourceType
    {
        Base64,
        Local,
        Url
    }

    public enum FontSourceFormat
    {
        Default,
        EmbeddedOpenType,
        WOFF2,
        WOFF,
        TrueType, OpenType, TTC, //Currently the only supported versions.
        SVG
    }

    public enum PositionMode
    {
        /// <summary>
        /// Default positioning within the flow of the content
        /// </summary>
        Static = 0,

        /// <summary>
        /// A postion that is set explicitly on the page, no matter where its parent is
        /// </summary>
        Fixed = 1,

        /// <summary>
        /// A postion that is set relative to this Components (first positioned) parent position.
        /// </summary>
        Absolute = 2,

        /// <summary>
        /// A block that still maintains the space used, but is shifted by the position
        /// </summary>
        Relative = 3

    }

    public enum DisplayMode
    {
        /// <summary>
        /// No Expicit postion - if it fits next to the last component on the current line, then it will be appended,
        /// otherwise a new line will be created. Following elements will be appended to the same line
        /// </summary>
        Inline = 0,
        
        /// <summary>
        /// A block that sits on the current line of either a fixed size, or taking the space up that it's internal contents need.
        /// </summary>
        InlineBlock = 1,
        
        /// <summary>
        /// A block element breaks any current line is rendered on it's own, with any positioning
        /// </summary>
        Block = 2,
        
        /// <summary>
        /// Specific dispaly mode that will vertically align content within a cell.
        /// </summary>
        TableCell = 4,
        
        /// <summary>
        /// If invisible, then it takes up no room, and does not impact the layout
        /// </summary>
        Invisible = 10
    }

    public enum FloatMode
    {
        /// <summary>
        /// Default value of not floating
        /// </summary>
        None = 0,

        /// <summary>
        /// Will float content to the left side of the container
        /// </summary>
        Left = 1,

        /// <summary>
        /// Will float content to the right side of the container
        /// </summary>
        Right = 2,

    }

    /// <summary>
    /// Defines the location of the rendered character in an SVG text block, based on the x value.
    /// </summary>
    public enum TextAnchor
    {
        /// <summary>
        /// An x value defines the starting point of the rendered characters.
        /// </summary>
        Start,

        /// <summary>
        /// An x value defines the middle point of the rendered characters.
        /// </summary>
        Middle,

        /// <summary>
        /// An x value defines the end point of the rendered characters.
        /// </summary>
        End
    }

    /// <summary>
    /// Defines the baseline adjustment of the rendered characters in an SVG Textblock.
    /// Such that the vertical offset of the characters can ge set for a specific y position.
    /// </summary>
    public enum DominantBaseline
    {
        /// <summary>
        /// Allows the rendering engine to align the baseline for a set of characters as it wishes. Defaults to TextTop
        /// </summary>
        Auto,

        /// <summary>
        /// Aligns the text position
        /// as per the baseline of a font with descenders below the the y position.
        /// </summary>
        Text_Top,
        
        /// <summary>
        /// Aligns the text position
        /// as per the baseline of a font with descenders below the the y position.
        /// </summary>
        Text_Bottom,

        /// <summary>
        /// Aligns the text to the middle of the lowercase x character height, based on the y position and the font size.
        /// </summary>
        Middle,

        /// <summary>
        /// Aligns the text to the middle of the uppercase M character height, based on the y position and the font size.
        /// </summary>
        Central,

        /// <summary>
        /// Aligns the text to the top of the uppercase M character height, based on the y position and the font size.
        /// </summary>
        Hanging,

        /// <summary>
        /// Aligns the text to the top of the lowercase x character height, based on the y position and the font size.
        /// </summary>
        Mathematical,

        /// <summary>
        /// Aligns the text to the bottom of the lowercase y descender, based on the y position and the font size
        /// </summary>
        Ideographic,

        /// <summary>
        /// As per text top
        /// </summary>
        Alphabetic,

        /// <summary>
        /// Aligns the text to its font bounding box with characters above the y position based on the font size.
        /// </summary>
        Text_After_Edge,

        /// <summary>
        /// Aligns the text to its font bounding box with characters below the y position 
        /// </summary>
        Text_Before_Edge

    }

    public enum OverflowAction
    {
        /// <summary>
        /// overflow will be pushed onto a new page where possible
        /// </summary>
        NewPage,

        /// <summary>
        /// overflow will not be rendered and layout will stop once the container is full
        /// </summary>
        Truncate,

        /// <summary>
        /// overflow will just be continued but the parent container will be graphically clipped
        /// so the extra is not shown.
        /// </summary>
        Clip,

        /// <summary>
        /// No new page will be generated, but child content will be rendered beyond any boundaries
        /// </summary>
        None
    }

    public enum OverflowSplit
    {
        //Component,
        Any,
        Never
    }

    public enum Visibility
    {
        /// <summary>
        /// Defalt value - the item is fully visible
        /// </summary>
        Visible,
        /// <summary>
        /// The item is not visible and does not take up any space
        /// </summary>
        None,
        /// <summary>
        /// The item is not visible, but still takes up any required space.
        /// </summary>
        Hidden
    }

    public enum Corner
    {
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft
    }

    public enum BadgeType
    {
        WhiteOnBlack,
        BlackOnWhite,
        Environment
    }

    public enum PathDataType
    {
        Move,
        Line,
        Rect,
        SubPath,
        Bezier,
        Arc,
        Quadratic,
        Close
    }

    /// <summary>
    /// Defines the sweeping direction of an arc path
    /// </summary>
    public enum PathArcSweep
    {
        Negative = 0,
        Positive = 1
    }

    /// <summary>
    /// Defines the segment of ellipse that is drawn when in an arc.
    /// </summary>
    public enum PathArcSize
    {
        Small = 0,
        Large = 1
    }

    /// <summary>
    /// Defines the transformation origin options
    /// </summary>
    public enum TransformationOrigin
    {
        BottomLeft,
        TopLeft,
        TopRight,
        BottomRight,
        CenterMiddle,
        Origin
    }

    

    /// <summary>
    /// Defines a type of supported gradient
    /// </summary>
    public enum GradientType
    {
        Linear,
        Radial
    }

    /// <summary>
    /// Pre-defined enumeration of the linear gradient angles
    /// </summary>
    public enum GradientAngle
    {
        Top = 270,
        Left = 180,
        Bottom = 90,
        Right = 0,
        Top_Left = 225,
        Top_Right = 315,
        Bottom_Left = 135,
        Bottom_Right = 45
    }

    /// <summary>
    /// Defines the order in which matrix transformations are applied.
    /// </summary>
    public enum MatrixOrder
    {
        Append,
        Prepend
    }

    /// <summary>
    /// Predefined enumeration of the supported radial gradient shapes
    /// </summary>
    public enum RadialShape
    {
        [Obsolete("Ellipse gradients are not currently supported", true)]
        Ellipse,
        Circle
    }

    /// <summary>
    /// Predefined enumeration of the supported sides of a radial gradient
    /// </summary>
    public enum RadialSize
    {
        None,
        ClosestSide,
        FarthestSide,
        ClosestCorner,
        FarthestCorner
    }

    public enum GradientSpreadMode
    {
        Pad,
        Reflect,
        Repeat
    }

    public enum GradientUnitType
    {
        ObjectBoundingBox,
        UserSpaceOnUse
    }

    public enum AdornmentOrder
    {
        Before,
        After
    }

    [Flags]
    public enum AdornmentPlacements
    {
        None = 0,
        Start = 1,
        Middle = 2,
        End = 4,
        All = Start + Middle + End
    }


    /// <summary>
    /// The supported types of style content values
    /// </summary>
    public enum ContentDescriptorType
    {
        Text,
        Image,
        Gradient,
        Counter,
        Counters,
        Attribute,
        Quote
    }
    
    public enum AspectRatioAlign : byte
    {
        None,
        xMinYMin,
        xMidYMin,
        xMaxYMin,
        xMinYMid,
        xMidYMid,
        xMaxYMid,
        xMinYMax,
        xMidYMax,
        xMaxYMax
    }

    public enum AspectRatioMeet : byte
    {
        None,
        Meet,
        Slice
    }

}
