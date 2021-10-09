using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.PDF.Native
{
    internal class PDFReader17 : PDFReader14
    {

        public PDFReader17(System.IO.Stream stream, bool ownsStream, TraceLog log) 
            : base(stream, ownsStream, log)
        {
        }

        protected override void InitData(TraceLog log)
        {
            this.Searcher.Position = this.Searcher.Length;
            PDFFileRange eofPos = AssertFoundRange(Searcher.MatchBackwardString(EndOfFileMarker), EndOfFileMarker);
            PDFFileRange startxrefPos = AssertFoundRange(Searcher.MatchBackwardString(StartXRefMarker), StartXRefMarker);

            //Look for a object number as the xref object as per v1.5 onwards.

            this.Searcher.Position = startxrefPos.EndOffset;
            string offset = this.Searcher.GetInnerText(startxrefPos, eofPos).Trim();

            long pos;
            if (long.TryParse(offset, out pos))
            {
                this.Searcher.Position = pos;
                PDFFileRange num = this.Searcher.MatchForwardString(" ");
                PDFFileRange vers = this.Searcher.MatchForwardString(" ");

            }


            base.InitData(log);
        }
    }
}
