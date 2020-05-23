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

    [Flags()]
    public enum TextDecoration
    {
        None = 0,
        Underline = 1,
        Overline = 2,
        StrikeThrough = 4,
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
