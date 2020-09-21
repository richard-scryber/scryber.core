using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber
{
    internal class PDFTraceLogStyles : StylesDocument
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
            StyleDefn traceSect = new StyleDefn() { AppliedType = typeof(Section) };
            traceSect.Margins.All = 10;
            traceSect.PageStyle.PaperSize = PaperSize.A4;
            traceSect.Font.FontFamily = "Helvetica";
            this.Styles.Add(traceSect);

            StyleDefn h1 = new StyleDefn() { AppliedType = typeof(Head1) };
            h1.Background.Color = new PDFColor(0.6);
            h1.Size.FullWidth = true;
            h1.Fill.Color = PDFColors.White;
            h1.Padding.All = 5;
            this.Styles.Add(h1);

            StyleDefn h3 = new StyleDefn() { AppliedType = typeof(Head3) };
            h3.Background.Color = new PDFColor(0.9);
            h3.Size.FullWidth = true;
            h3.Padding.All = 5;
            this.Styles.Add(h3);

            StyleDefn tbl = new StyleDefn() { AppliedType = typeof(TableGrid)};
            tbl.Size.FullWidth = true;
            tbl.Margins.All = 10;
            tbl.Font.FontSize = 10;
            this.Styles.Add(tbl);

            

            //general cell

            StyleDefn tcell = new StyleDefn()
            {
                AppliedType = typeof(TableCell)
            };
            tcell.Border.Width = 1;
            tcell.Border.Color = PDFColors.Gray;
            tcell.Padding.All = 2;
            this.Styles.Add(tcell);

            StyleDefn tcellhead = new StyleDefn()
            {
                AppliedType = typeof(TableHeaderCell)
            };
            tcellhead.Font.FontBold = true;
            this.Styles.Add(tcellhead);

            StyleDefn tcellnum = new StyleDefn()
            {
                AppliedType = typeof(TableCell),
                AppliedClass = "number"
            };
            tcellnum.Position.HAlign = HorizontalAlignment.Right;
            this.Styles.Add(tcellnum);


            //log level styles

            StyleDefn logGrid = new StyleDefn()
            {
                AppliedClass = "log-grid"
            };
            logGrid.Font.FontSize = 9;
            this.Styles.Add(logGrid);

            StyleDefn logMsg = new StyleDefn()
            {
                AppliedClass = "Message"
            };
            logMsg.Fill.Color = PDFColors.Green;
            this.Styles.Add(logMsg);

            StyleDefn logErr = new StyleDefn()
            {
                AppliedClass = "Error"
            };
            logErr.Fill.Color = PDFColors.White;
            logErr.Background.Color = PDFColors.Red;
            this.Styles.Add(logErr);

            StyleDefn logWarn = new StyleDefn()
            {
                AppliedClass = "Warning"
            };
            logWarn.Fill.Color = PDFColors.Red;
            this.Styles.Add(logWarn);

            StyleDefn logVerb = new StyleDefn()
            {
                AppliedClass = "Verbose"
            };
            logVerb.Fill.Color = PDFColors.Black;
            this.Styles.Add(logVerb);

            StyleDefn logDebug = new StyleDefn()
            {
                AppliedClass = "Debug"
            };
            logDebug.Fill.Color = new PDFColor(0.4, 0.4, 0.4);
            logDebug.Font.FontItalic = true;
            this.Styles.Add(logDebug);

        }
    }
}
