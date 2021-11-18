using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.PDF
{
    internal class PDFTraceLogStyles : StylesDocument
    {

        public PDFTraceLogStyles()
        {
        }

        protected override void OnInit(InitContext context)
        {
            base.OnInit(context);
            this.InitTraceLogStyles();
        }

        private void InitTraceLogStyles()
        {
            StyleDefn traceSect = new StyleDefn("section");
            traceSect.Margins.All = 10;
            traceSect.PageStyle.PaperSize = PaperSize.A4;
            traceSect.Font.FontFamily = (FontSelector)"Helvetica";
            this.Styles.Add(traceSect);

            StyleDefn h1 = new StyleDefn("h1");
            h1.Background.Color = new Color(153);
            h1.Size.FullWidth = true;
            h1.Fill.Color = StandardColors.White;
            h1.Padding.All = 5;
            this.Styles.Add(h1);

            StyleDefn h3 = new StyleDefn("h3");
            h3.Background.Color = new Color(229);
            h3.Size.FullWidth = true;
            h3.Padding.All = 5;
            this.Styles.Add(h3);

            StyleDefn tbl = new StyleDefn("table");
            tbl.Size.FullWidth = true;
            tbl.Margins.All = 10;
            tbl.Font.FontSize = 10;
            this.Styles.Add(tbl);

            

            //general cell

            StyleDefn tcell = new StyleDefn("td");
            tcell.Border.Width = 1;
            tcell.Border.Color = StandardColors.Gray;
            tcell.Padding.All = 2;
            this.Styles.Add(tcell);

            StyleDefn tcellhead = new StyleDefn("td.head");
            tcellhead.Font.FontBold = true;
            this.Styles.Add(tcellhead);

            StyleDefn tcellnum = new StyleDefn("td.number");
            tcellnum.Position.HAlign = HorizontalAlignment.Right;
            this.Styles.Add(tcellnum);


            //log level styles

            StyleDefn logGrid = new StyleDefn(".log-grid");
            logGrid.Font.FontSize = 9;
            this.Styles.Add(logGrid);

            StyleDefn logMsg = new StyleDefn(".Message");
            logMsg.Fill.Color = StandardColors.Green;
            this.Styles.Add(logMsg);

            StyleDefn logErr = new StyleDefn(".Error");
            logErr.Fill.Color = StandardColors.White;
            logErr.Background.Color = StandardColors.Red;
            this.Styles.Add(logErr);

            StyleDefn logWarn = new StyleDefn(".Warning");
            logWarn.Fill.Color = StandardColors.Red;
            this.Styles.Add(logWarn);

            StyleDefn logVerb = new StyleDefn(".Verbose");
            logVerb.Fill.Color = StandardColors.Black;
            this.Styles.Add(logVerb);

            StyleDefn logDebug = new StyleDefn(".Debug");
            logDebug.Fill.Color = new Color(102);
            logDebug.Font.FontItalic = true;
            this.Styles.Add(logDebug);

        }
    }
}
