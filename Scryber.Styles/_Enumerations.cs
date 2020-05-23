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

namespace Scryber
{
    public enum ComponentState
    {
        Normal,
        Over,
        Down
    }

    public enum PaperOrientation
    {
        Landscape,
        Portrait
    }

    public enum PaperSize
    {
        A0 = 0,
        A1 = 1,
        A2 = 2,
        A3 = 3,
        A4 = 4,
        A5 = 5,
        A6 = 6,
        A7 = 7,
        A8 = 8,
        A9 = 9,
        B0 = 10,
        B1 = 11,
        B2 = 12,
        B3 = 13,
        B4 = 14,
        B5 = 15,
        B6 = 16,
        B7 = 17,
        B8 = 18,
        B9 = 19,
        C0 = 20,
        C1 = 21,
        C2 = 22,
        C3 = 23,
        C4 = 24,
        C5 = 25,
        C6 = 26,
        C7 = 27,
        C8 = 28,
        C9 = 29,

        Quarto = 50,
        Foolscap = 51,
        Executive = 52,
        GovermentLetter = 53,
        Letter = 54,
        Legal = 55,
        Tabloid = 56,
        Post = 57,
        Crown = 58,
        LargePost = 59,
        Demy = 60,
        Medium = 61,
        Royal = 62,
        Elephant = 63,
        DoubleDemy = 64,
        QuadDemy = 65,
        Statement = 66,

        Custom = 100
    }

    public enum PatternRepeat
    {
        None,
        RepeatX,
        RepeatY,
        RepeatBoth,
        Fill
    }

    public enum PageNumberDisplay
    {
        PageIndex,
        PageCount,
        PageIndexLabel,
        PageCountLabel,
        SectionOffset,
        SectionCount,
        SectionOffsetLabel,
        SectionCountLabel
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

    public enum TableRowRepeat
    {
        None,
        RepeatAtTop
    }

    
    

    public enum NumberingOutputType
    {
        String,
        Component
    }
}
