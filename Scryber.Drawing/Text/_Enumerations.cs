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

namespace Scryber.Text
{
    public enum PDFTextOpType
    {
        None,
        LineBreak,
        StyleStart,
        StyleEnd,
        ClassStart,
        ClassEnd,
        TextContent,
        BeginBlock,
        EndBlock,
        Proxy,
        Unknown
    }

    public enum WordWrap
    {
        Auto,
        Word,
        Character,
        NoWrap
    }

    /// <summary>
    /// Where words can be broken between lines (word-break)
    /// </summary>
    public enum WordBreak
    {
        /// <summary>Words are only broken when they are too long for a line</summary>
        Normal,
        /// <summary>Words can be broken at any character to fill the line</summary>
        BreakAll,
        /// <summary>Same as normal for text that is not Chinese, Japanese or Korean</summary>
        KeepAll,
        /// <summary>Deprecated css, the same as normal with overflow-wrap: anywhere</summary>
        BreakWord
    }

    /// <summary>
    /// If a word that is too long for a line can be broken (overflow-wrap)
    /// </summary>
    public enum OverflowWrap
    {
        Normal,
        BreakWord,
        Anywhere
    }

    public enum WordHyphenation
    {
        None,
        Auto
    }

    /// <summary>
    /// The case transformation applied to text when it is laid out
    /// </summary>
    public enum TextTransform
    {
        None,
        Uppercase,
        Lowercase,
        Capitalize
    }

    [Flags()]
    public enum TextDecoration
    {
        None = 0,
        Underline = 1,
        Overline = 2,
        StrikeThrough = 4,
    }

    /// <summary>
    /// The style of line drawn for a text decoration (text-decoration-style)
    /// </summary>
    public enum TextDecorationStyle
    {
        Solid,
        Double,
        Dotted,
        Dashed,
        Wavy
    }

    public enum TextRenderMode
    {
        Fill = 0,
        Stroke = 1,
        FillAndStroke = 2,
        NoOp = 3,
        FillAndAddToClip = 4,
        StrokeAndAddToClip = 5,
        FillStrokeAndAddToClip = 6,
        AddToClip = 7
    }
}
