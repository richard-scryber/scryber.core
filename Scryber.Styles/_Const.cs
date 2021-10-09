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
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber
{
    internal static class Const
    {

        public const PaperSize DefaultPaperSize = PaperSize.A4;
        public const PaperOrientation DefaultPaperOrientation = PaperOrientation.Portrait;
        public const LineCaps DefaultLineCaps = LineCaps.Butt;
        public const LineJoin DefaultLineJoin = LineJoin.Bevel;
        public const HorizontalAlignment DefaultHorizontalAlign = HorizontalAlignment.Left;
        public const VerticalAlignment DefaultVerticalAlign = VerticalAlignment.Top;
        public const string DefaultFontFamily = "Helvetica";
        public const float DefaultFontSize = 12.0F;
        public static readonly Color DefaultFillColor = PDFColors.Black;

        public static readonly PDFUnit DefaultListNumberInset = 30;
        public static readonly PDFUnit DefaultDefinitionListInset = 100;
        public const HorizontalAlignment DefaultListNumberAlignment = HorizontalAlignment.Right;

    }

    public static class StyleDefaults
    {
        public static readonly HorizontalAlignment HAlign = Const.DefaultHorizontalAlign;
        public static readonly VerticalAlignment VAlign = Const.DefaultVerticalAlign;

    }
}
