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
using System.Drawing;
using Scryber.Drawing;

namespace Scryber
{
    public static class Papers
    {

        public static class ISO
        {
            public static PaperSize A0 { get { return PaperSize.A0; } }
            public static PaperSize A1 { get { return PaperSize.A1; } }
            public static PaperSize A2 { get { return PaperSize.A2; } }
            public static PaperSize A3 { get { return PaperSize.A3; } }
            public static PaperSize A4 { get { return PaperSize.A4; } }
            public static PaperSize A5 { get { return PaperSize.A5; } }
            public static PaperSize A6 { get { return PaperSize.A6; } }
            public static PaperSize A7 { get { return PaperSize.A7; } }
            public static PaperSize A8 { get { return PaperSize.A8; } }
            public static PaperSize A9 { get { return PaperSize.A9; } }
            public static PaperSize B0 { get { return PaperSize.B0; } }
            public static PaperSize B1 { get { return PaperSize.B1; } }
            public static PaperSize B2 { get { return PaperSize.B2; } }
            public static PaperSize B3 { get { return PaperSize.B3; } }
            public static PaperSize B4 { get { return PaperSize.B4; } }
            public static PaperSize B5 { get { return PaperSize.B5; } }
            public static PaperSize B6 { get { return PaperSize.B6; } }
            public static PaperSize B7 { get { return PaperSize.B7; } }
            public static PaperSize B8 { get { return PaperSize.B8; } }
            public static PaperSize B9 { get { return PaperSize.B9; } }
            public static PaperSize C0 { get { return PaperSize.C0; } }
            public static PaperSize C1 { get { return PaperSize.C1; } }
            public static PaperSize C2 { get { return PaperSize.C2; } }
            public static PaperSize C3 { get { return PaperSize.C3; } }
            public static PaperSize C4 { get { return PaperSize.C4; } }
            public static PaperSize C5 { get { return PaperSize.C5; } }
            public static PaperSize C6 { get { return PaperSize.C6; } }
            public static PaperSize C7 { get { return PaperSize.C7; } }
            public static PaperSize C8 { get { return PaperSize.C8; } }
            public static PaperSize C9 { get { return PaperSize.C9; } }


        }

        public static class US
        {
            public static PaperSize Quarto { get { return PaperSize.Quarto; } }
            public static PaperSize Foolscap { get { return PaperSize.Foolscap; } }
            public static PaperSize Executive { get { return PaperSize.Executive; } }
            public static PaperSize GovermentLetter { get { return PaperSize.GovermentLetter; } }
            public static PaperSize Letter { get { return PaperSize.Letter; } }
            public static PaperSize Legal { get { return PaperSize.Legal; } }
            public static PaperSize Tabloid { get { return PaperSize.Tabloid; } }
            public static PaperSize Post { get { return PaperSize.Post; } }
            public static PaperSize Crown { get { return PaperSize.Crown; } }
            public static PaperSize LargePost { get { return PaperSize.LargePost; } }
            public static PaperSize Demy { get { return PaperSize.Demy; } }
            public static PaperSize Medium { get { return PaperSize.Medium; } }
            public static PaperSize Royal { get { return PaperSize.Royal; } }
            public static PaperSize Elephant { get { return PaperSize.Elephant; } }
            public static PaperSize DoubleDemy { get { return PaperSize.DoubleDemy; } }
            public static PaperSize QuadDemy { get { return PaperSize.QuadDemy; } }
            public static PaperSize Statement { get { return PaperSize.Statement; } }

        }

        public static PaperSize GetPaperFromSize(SizeF sizeinmm)
        {
            return PaperSize.Custom;
        }

        public static SizeF GetSizeInMM(PaperSize paper)
        {
            SizeF p = SizeF.Empty;
            switch (paper)
            {
                case (PaperSize.A0):
                    p = new SizeF(841F, 1189F);
                    break;
                case (PaperSize.A1):
                    p = new SizeF(594F, 841F);
                    break;
                case (PaperSize.A2):
                    p = new SizeF(420F, 594F);
                    break;
                case (PaperSize.A3):
                    p = new SizeF(297F, 420.6F);
                    break;
                case (PaperSize.A4):
                    p = new SizeF(210.3F, 297F);
                    break;
                case (PaperSize.A5):
                    p = new SizeF(148F, 210.3F);
                    break;
                case (PaperSize.A6):
                    p = new SizeF(105F, 148F);
                    break;
                case (PaperSize.A7):
                    p = new SizeF(74.0F, 105.0F);
                    break;
                case (PaperSize.A8):
                    p = new SizeF(52.0F, 74.0F);
                    break;
                case (PaperSize.A9):
                    p = new SizeF(37.0F, 52.0F);
                    break;
                case (PaperSize.B0):
                    p = new SizeF(1000.0F, 1414.0F);
                    break;
                case (PaperSize.B1):
                    p = new SizeF(707.0F, 1000.0F);
                    break;
                case (PaperSize.B2):
                    p = new SizeF(500.0F, 707.0F);
                    break;
                case (PaperSize.B3):
                    p = new SizeF(353.0F, 500.0F);
                    break;
                case (PaperSize.B4):
                    p = new SizeF(250.0F, 353.0F);
                    break;
                case (PaperSize.B5):
                    p = new SizeF(176.0F, 250.0F);
                    break;
                case (PaperSize.B6):
                    p = new SizeF(125.0F, 176.0F);
                    break;
                case (PaperSize.B7):
                    p = new SizeF(88.0F, 125.0F);
                    break;
                case (PaperSize.B8):
                    p = new SizeF(62.0F, 88.0F);
                    break;
                case (PaperSize.B9):
                    p = new SizeF(44.0F, 62.0F);
                    break;
                case (PaperSize.C0):
                    p = new SizeF(917.0F, 1297.0F);
                    break;
                case (PaperSize.C1):
                    p = new SizeF(648.0F, 917.0F);
                    break;
                case (PaperSize.C2):
                    p = new SizeF(458.0F, 648.0F);
                    break;
                case (PaperSize.C3):
                    p = new SizeF(324.0F, 458.0F);
                    break;
                case (PaperSize.C4):
                    p = new SizeF(229.0F, 324.0F);
                    break;
                case (PaperSize.C5):
                    p = new SizeF(162.0F, 229.0F);
                    break;
                case (PaperSize.C6):
                    p = new SizeF(114.0F, 162.0F);
                    break;
                case (PaperSize.C7):
                    p = new SizeF(81.0F, 114.0F);
                    break;
                case (PaperSize.C8):
                    p = new SizeF(57.0F, 81.0F);
                    break;
                case (PaperSize.C9):
                    p = new SizeF(40.0F, 57.0F);
                    break;
                case (PaperSize.Quarto):
                    p = new SizeF(203.0F, 254.0F);
                    break;
                case (PaperSize.Foolscap):
                    p = new SizeF(203.0F, 330.0F);
                    break;
                case (PaperSize.Executive):
                    p = new SizeF(184.0F, 267.0F);
                    break;
                case (PaperSize.GovermentLetter):
                    p = new SizeF(203.0F, 267.0F);
                    break;
                case (PaperSize.Letter):
                    p = new SizeF(216.0F, 279.0F);
                    break;
                case (PaperSize.Legal):
                    p = new SizeF(216.0F, 356.0F);
                    break;
                case (PaperSize.Tabloid):
                    p = new SizeF(297.0F, 432.0F);
                    break;
                case (PaperSize.Post):
                    p = new SizeF(394.0F, 489.0F);
                    break;
                case (PaperSize.Crown):
                    p = new SizeF(381.0F, 508.0F);
                    break;
                case (PaperSize.LargePost):
                    p = new SizeF(419.0F, 533.0F);
                    break;
                case (PaperSize.Demy):
                    p = new SizeF(445.0F, 572.0F);
                    break;
                case (PaperSize.Medium):
                    p = new SizeF(457.0F, 584.0F);
                    break;
                case (PaperSize.Royal):
                    p = new SizeF(508.0F, 635.0F);
                    break;
                case (PaperSize.Elephant):
                    p = new SizeF(584.0F, 711.0F);
                    break;
                case (PaperSize.DoubleDemy):
                    p = new SizeF(597.0F, 889.0F);
                    break;
                case (PaperSize.QuadDemy):
                    p = new SizeF(88.0F, 1143.0F);
                    break;
                case (PaperSize.Statement):
                    p = new SizeF(140.0F, 216.0F);
                    break;
                case (PaperSize.Custom):
                default:
                    break;
            }

            return p;

        }

        public static PDFSize GetSizeInDeviceIndependentUnits(PaperSize paperSize)
        {
            SizeF mm = Papers.GetSizeInMM(paperSize);
            PDFSize full = new PDFSize(
                new PDFUnit(mm.Width, PageUnits.Millimeters),
                new PDFUnit(mm.Height, PageUnits.Millimeters));
            
            return full;
        }
    }
}
