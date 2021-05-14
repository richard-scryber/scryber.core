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

namespace Scryber.Drawing
{
    /// <summary>
    /// Defines the type of Color - Gray, RGB etc.
    /// </summary>
    public enum ColorSpace
    {
        G,
        RGB,
        HSL,
        LAB,
        CMYK,
        Custom
    }

    /// <summary>
    /// Available page units
    /// </summary>
    public enum PageUnits
    {
        //Reserved 0 for empty units
        Points = 0,
        Millimeters = 1,
        Inches = 2
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

    [Flags()]
    public enum FontStyle
    {
        Regular = 0,
        Bold = 1,
        Italic = 2,
        Superscript = 4,
        Subscript = 8
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
        /// A postion that is set explicitly on the page, no matter where its parent is
        /// </summary>
        Absolute,

        /// <summary>
        /// A postion that is set relative to this Components parent position.
        /// </summary>
        Relative,

        /// <summary>
        /// A block element breaks is rendered on it's own line
        /// </summary>
        Block,

        /// <summary>
        /// No Expicit postion - if it fits next to the last component on the current line, then it will be appended,
        /// otherwise a new line will be created. Following elements will be appended to the same line
        /// </summary>
        Inline,

        /// <summary>
        /// If invisible, then it takes up no room, and does not impact the layout
        /// </summary>
        Invisible

        //TODO:Float - Appears at the current position with content flowing around it
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
        Top = 0,
        Left = 270,
        Bottom = 180,
        Right = 90,
        Top_Left = 315,
        Top_Right = 45,
        Bottom_Left = 225,
        Bottom_Right = 135
    }

    /// <summary>
    /// Predefined enumeration of the supported radial gradient shapes
    /// </summary>
    public enum RadialShape
    {
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

}
