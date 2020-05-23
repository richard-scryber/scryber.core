using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber
{
    internal class PDFTraceLogStyles : PDFStylesDocument
    {

        public PDFTraceLogStyles()
        {
        }

        protected override void OnInit(PDFInitContext context)
        {
            base.OnInit(context);
            this.InitTraceLogStyles();
        }

        private void InitTraceLogStyles()
        {
            PDFStyleDefn traceSect = new PDFStyleDefn() { AppliedType = typeof(PDFSection) };
            traceSect.Margins.All = 10;
            traceSect.PageStyle.PaperSize = PaperSize.A4;
            traceSect.Font.FontFamily = "Helvetica";
            this.Styles.Add(traceSect);

            PDFStyleDefn h1 = new PDFStyleDefn() { AppliedType = typeof(PDFHead1) };
            h1.Background.Color = new PDFColor(0.6);
            h1.Size.FullWidth = true;
            h1.Fill.Color = PDFColors.White;
            h1.Padding.All = 5;
            this.Styles.Add(h1);

            PDFStyleDefn h3 = new PDFStyleDefn() { AppliedType = typeof(PDFHead3) };
            h3.Background.Color = new PDFColor(0.9);
            h3.Size.FullWidth = true;
            h3.Padding.All = 5;
            this.Styles.Add(h3);

            PDFStyleDefn tbl = new PDFStyleDefn() { AppliedType = typeof(PDFTableGrid)};
            tbl.Size.FullWidth = true;
            tbl.Margins.All = 10;
            tbl.Font.FontSize = 10;
            this.Styles.Add(tbl);

            

            //general cell

            PDFStyleDefn tcell = new PDFStyleDefn()
            {
                AppliedType = typeof(PDFTableCell)
            };
            tcell.Border.Width = 1;
            tcell.Border.Color = PDFColors.Gray;
            tcell.Padding.All = 2;
            this.Styles.Add(tcell);

            PDFStyleDefn tcellhead = new PDFStyleDefn()
            {
                AppliedType = typeof(PDFTableHeaderCell)
            };
            tcellhead.Font.FontBold = true;
            this.Styles.Add(tcellhead);

            PDFStyleDefn tcellnum = new PDFStyleDefn()
            {
                AppliedType = typeof(PDFTableCell),
                AppliedClass = "number"
            };
            tcellnum.Position.HAlign = HorizontalAlignment.Right;
            this.Styles.Add(tcellnum);


            //log level styles

            PDFStyleDefn logGrid = new PDFStyleDefn()
            {
                AppliedClass = "log-grid"
            };
            logGrid.Font.FontSize = 9;
            this.Styles.Add(logGrid);

            PDFStyleDefn logMsg = new PDFStyleDefn()
            {
                AppliedClass = "Message"
            };
            logMsg.Fill.Color = PDFColors.Green;
            this.Styles.Add(logMsg);

            PDFStyleDefn logErr = new PDFStyleDefn()
            {
                AppliedClass = "Error"
            };
            logErr.Fill.Color = PDFColors.White;
            logErr.Background.Color = PDFColors.Red;
            this.Styles.Add(logErr);

            PDFStyleDefn logWarn = new PDFStyleDefn()
            {
                AppliedClass = "Warning"
            };
            logWarn.Fill.Color = PDFColors.Red;
            this.Styles.Add(logWarn);

            PDFStyleDefn logVerb = new PDFStyleDefn()
            {
                AppliedClass = "Verbose"
            };
            logVerb.Fill.Color = PDFColors.Black;
            this.Styles.Add(logVerb);

            PDFStyleDefn logDebug = new PDFStyleDefn()
            {
                AppliedClass = "Debug"
            };
            logDebug.Fill.Color = new PDFColor(0.4, 0.4, 0.4);
            logDebug.Font.FontItalic = true;
            this.Styles.Add(logDebug);

        }
    }
}
