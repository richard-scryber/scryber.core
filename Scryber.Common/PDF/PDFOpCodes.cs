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

namespace Scryber.PDF
{
    /// <summary>
    /// A (non-exhaustive) list of operations that can be performed within a Graphical, or Textual context. See intividual items for their parameter format
    /// </summary>
    public enum PDFOpCode
    {
        /// <summary>
        /// cs - sets the Fill color space to the name value : /name cs
        /// </summary>
        ColorSetFillSpace,
        
        /// <summary>
        /// CS - sets the Stroke color space to the name value : /name CS
        /// </summary>
        ColorSetStrokeSpace,
        
        /// <summary>
        /// scn - sets the current fill pattern (must be in /Pattern fill color space) : /Pattern cs .... /name scn
        /// </summary>
        ColorFillPattern,
        
        /// <summary>
        /// SCN - sets the current stroke pattern (must be in /Pattern stroke color space) : /Pattern CS .... /name SCN
        /// </summary>
        ColorStrokePattern,
        
        /// <summary>
        /// g - sets the current fill color space to grayscale and the current grayscale fill to the real value : real g
        /// </summary>
        ColorFillGrayscaleSpace,
        
        /// <summary>
        /// G - sets the current stroke color space to grayscale and the current grayscale stroke to the real value : real G
        /// </summary>
        ColorStrokeGrayscaleSpace,
        
        /// <summary>
        /// rg - sets the current fill color space to RGB and the current color fill to the real values : real real real rg
        /// </summary>
        ColorFillRGBSpace,
        
        /// <summary>
        /// RG - sets the current stroke color space to RGB and the current color stroke to the real values : real real real RG
        /// </summary>
        ColorStrokeRGBSpace,
        
        /// <summary>
        /// k - sets the current fill color space to CMYK and the current color fill to the real values : real real real real k
        /// </summary>
        ColorFillCMYK,

        /// <summary>
        /// ca - sets the current fill opacity to the real values : real ca
        /// </summary>
        ColorFillOpacity,

        
        /// <summary>
        /// K - sets the current stroke color space to CMYK and the current color stroke to the real values :  real real real real K
        /// </summary>
        ColorStrokeCMYK,

        /// <summary>
        /// CA - sets the current stroke opacity to the real values : real CA
        /// </summary>
        ColorStrokeOpacity,

        /// <summary>
        /// q - Save the current graphics state to the graphics state stack so that previous state can be restored later on : Q
        /// </summary>
        SaveState,

        /// <summary>
        /// Q - Restore the graphics state to the previous stored state : Q
        /// </summary>
        RestoreState,

        /// <summary>
        /// Do - Paint an XObject (image or XObjectForm) : /name Do
        /// </summary>
        XobjPaint,

        /// <summary>
        /// BI - begin an image stream : BI ...ID .. EI
        /// </summary>
        XobjBegin,
        
        /// <summary>
        /// ID - End image data : BI ...ID .. EI
        /// </summary>
        XobjImageData,
        
        /// <summary>
        /// EI end an image stream : BI ...ID .. EI
        /// </summary>
        XobjEndImage,

        /// <summary>
        /// BMC for an XObject
        /// </summary>
        MarkedContentBegin,

        /// <summary>
        /// EMC for an XObject
        /// </summary>
        MarkedContentEnd,

        //Text Operators (p375)
        /// <summary>
        /// BT - Begin Text operations : BT ... ET
        /// </summary>
        TxtBegin,

        /// <summary>
        /// ET - End Text operations : BT ... ET
        /// </summary>
        TxtEnd,

        /// <summary>
        /// Tc - Set the Text character spacing : real Tc
        /// </summary>
        TxtCharSpacing,

        /// <summary>
        /// Tw - Set the Text Word spacing : real Tw
        /// </summary>
        TxtWordSpacing,

        /// <summary>
        /// Tz - set the text Horizontal Scaling : real Tz
        /// </summary>
        TxtHScaling,

        /// <summary>
        /// TL - set the text leading : real TL
        /// </summary>
        TxtLeading,

        /// <summary>
        /// Tf - Set the text font : /name real Tf
        /// </summary>
        TxtFont,

        /// <summary>
        /// Tj - Paint a line of text : (string) Tj
        /// </summary>
        TxtPaint,

        /// <summary>
        /// TJ - Paint an array of lines(?) : [string string|real string|real string|real ....] TJ
        /// </summary>
        TxtPaintArray,

        /// <summary>
        /// Tr - Set the text render mode to fill, stroke, both etc. (1-8) : int Tr
        /// </summary>
        TxtRenderMode,

        /// <summary>
        /// Ts - Set the text rise (&lt;0 superscript and &gt;0 subscript) : real Ts
        /// </summary>
        TxtRise,

        /// <summary>
        /// Td - Move to the next X,Y offset in a text run : real real Td
        /// </summary>
        TxtMoveNextOffset,

        /// <summary>
        /// Tm - set the current text transformation matrix : real real real real real real Tm
        /// </summary>
        TxtTransformMatrix,

        /// <summary>
        /// T* - paint the line and move to the start of the next line : string T*
        /// </summary>
        TxtNextLine,

        //graphics operation (p189)

        /// <summary>
        /// cm - set the current graphics transform matrix : real real real real real real cm
        /// </summary>
        GraphTransformMatrix,

        /// <summary>
        /// w - set the current line width (0 = minimum width) : real w
        /// </summary>
        GraphLineWidth,

        /// <summary>
        /// J - set the current line cap style 0 - 2 : int J
        /// </summary>
        GraphLineCap,

        /// <summary>
        /// j - Set the current line join style 0 - 3 : int j
        /// </summary>
        GraphLineJoin,

        /// <summary>
        /// M - set the current mitre limit for joined paths : real M
        /// </summary>
        GraphMiterLimit,

        /// <summary>
        /// d - set the current dash pattern : [int int] int d
        /// </summary>
        GraphDashPattern,

        /// <summary>
        /// ri - set the current graphical rendering intent (?)
        /// </summary>
        GraphRenderingIntent,

        /// <summary>
        /// i - set the current graphic flatness (0-100 where 0=default) - int i
        /// </summary>
        GraphFlatness,

        /// <summary>
        /// gs - set the current graphical state from a dictionary : /name gs
        /// </summary>
        GraphApplyState,

        /// <summary>
        /// m - move the graphics cursor to the specified position : real real m
        /// </summary>
        GraphMove, 

        /// <summary>
        /// l - draw a straight line from the cursor position to new position : real real l
        /// </summary>
        GraphLineTo, 

        /// <summary>
        /// c - draw a cubic curve to point 1 using point 2 and point 3 as control handles : x1 y1 x2 y2 x3 y3 c
        /// </summary>
        GraphCurve2Handle,

        /// <summary>
        /// v - draw a cubic curve to point 1 using point 2 as a control handle on x1 y1 : x1 y1 x2 y2 v
        /// </summary>
        GraphCurve1HandleEnd,

        /// <summary>
        /// y - draw a cubic curve to point 1 using point 2 as a control handle on cursor position : x1 y1 x2 y2 v
        /// </summary>
        GraphCurve1HandleBegin,

        /// <summary>
        /// h - close the current path : h
        /// </summary>
        GraphClose,

        /// <summary>
        /// re - draw a rectangle (x, y, w, h) : real real real real re
        /// </summary>
        GraphRect,

        /// <summary>
        /// W - sets the clipping path to the intersection of the current clipping path and a draw path : (path operations) W
        /// </summary>
        GraphSetClip,

        /// <summary>
        /// n - No Operation does not paint the last defined path.
        /// </summary>
        GraphNoOp,

        /// <summary>
        /// S - stroke the last drawn path : S
        /// </summary>
        GraphStrokePath,

        /// <summary>
        /// s - close the last path and stroke it (equivalent to : h S) : s
        /// </summary>
        GraphCloseAndStroke,

        /// <summary>
        /// f - fill the last drawn path : f
        /// </summary>
        GraphFillPath,

        /// <summary>
        /// f* - fill the last drawn path with Even Odd winding mode : f*
        /// </summary>
        GraphFillPathEvenOdd,

        /// <summary>
        /// B - fill and stroke the current path : B
        /// </summary>
        GraphFillAndStroke,

        /// <summary>
        /// B* - fill and stroke the current path using Even Odd winding rule : B*
        /// </summary>
        GraphFillAndStrokeEvenOdd, 

        /// <summary>
        /// b - close, fill and stroke the current path : b
        /// </summary>
        GraphCloseFillStroke,

        /// <summary>
        /// b* - close, fill and stroke the current path using the Even Odd winding rule : b*
        /// </summary>
        GraphCloseFileStrokeEvenOdd,

        /// <summary>
        /// n - End the path (with out filling, closing or stroking) used for creating clipping paths : n
        /// </summary>
        GraphEndPath

    }
}
