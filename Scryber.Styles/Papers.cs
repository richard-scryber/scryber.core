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

        public static PaperSize GetPaperFromSize(PDFSize sizeinmm)
        {
            return PaperSize.Custom;
        }

        public static PDFSize GetSizeInMM(PaperSize paper)
        {
            PDFSize p = PDFSize.Empty;
            switch (paper)
            {
                case (PaperSize.A0):
                    p = new PDFSize(841.0, 1189.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.A1):
                    p = new PDFSize(594.0, 841.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.A2):
                    p = new PDFSize(420.0, 594.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.A3):
                    p = new PDFSize(297.0, 420.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.A4):
                    p = new PDFSize(210.0, 297.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.A5):
                    p = new PDFSize(148.0, 210.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.A6):
                    p = new PDFSize(105.0, 148, PageUnits.Millimeters);
                    break;
                case (PaperSize.A7):
                    p = new PDFSize(74.0, 105.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.A8):
                    p = new PDFSize(52.0, 74.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.A9):
                    p = new PDFSize(37.0, 52.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.B0):
                    p = new PDFSize(1000.0, 1414.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.B1):
                    p = new PDFSize(707.0, 1000.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.B2):
                    p = new PDFSize(500.0, 707.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.B3):
                    p = new PDFSize(353.0, 500.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.B4):
                    p = new PDFSize(250.0, 353.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.B5):
                    p = new PDFSize(176.0, 250.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.B6):
                    p = new PDFSize(125.0, 176.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.B7):
                    p = new PDFSize(88.0, 125.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.B8):
                    p = new PDFSize(62.0, 88.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.B9):
                    p = new PDFSize(44.0, 62.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.C0):
                    p = new PDFSize(917.0, 1297.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.C1):
                    p = new PDFSize(648.0, 917.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.C2):
                    p = new PDFSize(458.0, 648.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.C3):
                    p = new PDFSize(324.0, 458.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.C4):
                    p = new PDFSize(229.0, 324.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.C5):
                    p = new PDFSize(162.0, 229.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.C6):
                    p = new PDFSize(114.0, 162.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.C7):
                    p = new PDFSize(81.0, 114.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.C8):
                    p = new PDFSize(57.0, 81.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.C9):
                    p = new PDFSize(40.0, 57.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Quarto):
                    p = new PDFSize(203.0, 254.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Foolscap):
                    p = new PDFSize(203.0, 330.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Executive):
                    p = new PDFSize(184.0, 267.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.GovermentLetter):
                    p = new PDFSize(203.0, 267.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Letter):
                    p = new PDFSize(216.0, 279.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Legal):
                    p = new PDFSize(216.0, 356.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Tabloid):
                    p = new PDFSize(297.0, 432.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Post):
                    p = new PDFSize(394.0, 489.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Crown):
                    p = new PDFSize(381.0, 508.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.LargePost):
                    p = new PDFSize(419.0, 533.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Demy):
                    p = new PDFSize(445.0, 572.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Medium):
                    p = new PDFSize(457.0, 584.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Royal):
                    p = new PDFSize(508.0, 635.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Elephant):
                    p = new PDFSize(584.0, 711.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.DoubleDemy):
                    p = new PDFSize(597.0, 889.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.QuadDemy):
                    p = new PDFSize(88.0, 1143.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Statement):
                    p = new PDFSize(140.0, 216.0, PageUnits.Millimeters);
                    break;
                case (PaperSize.Custom):
                default:
                    break;
            }

            return p;

        }

        public static PDFSize GetSizeInDeviceIndependentUnits(PaperSize paperSize)
        {
            PDFSize mm = Papers.GetSizeInMM(paperSize);
            return mm.ToPoints();
        }
    }
}
