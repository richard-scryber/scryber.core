using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.PDF.Native;

namespace Scryber.PDF.Parsing
{
    internal class Const
    {
        public const string PDFModificationsLogCategory = "Modifications";

        public static readonly PDFName PagesName = (PDFName)"Pages";
        public static readonly PDFName PageName = (PDFName)"Page";
        public static readonly PDFName TypeName = (PDFName)"Type";
        public static readonly PDFName KidsName = (PDFName)"Kids";
        public static readonly PDFName ResourcesName = (PDFName)"Resources";
        public static readonly PDFName MediaBoxName = (PDFName)"MediaBox";
        public static readonly PDFName CropBoxName = (PDFName)"CropBox";
        public static readonly PDFName RotateName = (PDFName)"Rotate";
    }
}
