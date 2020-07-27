using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber
{
    public class PDFPageNumberOptions
    {
        public bool HasPageNumbering { get; set; }

        public PageNumberStyle? NumberStyle { get; set; }

        public string Format { get; set; }

        public int? StartIndex { get; set; }

        public int? GroupCountHint { get; set; }

        public int? TotalCountHint { get; set; }

        public string NumberGroup { get; set; }
    }
}
