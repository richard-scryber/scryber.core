using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Components
{
    [PDFParsableComponent("Block")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_div")]
    public class Block : Div
    {

        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            style.Padding.All = new Scryber.Drawing.Unit(20, Drawing.PageUnits.Points);
            return style;
        }
    }

    [PDFParsableComponent("SplitBlock")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_div")]
    public class SplitBlock : Block
    {

        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            style.Columns.ColumnCount = 2;
            return style;
        }
    }

    [PDFParsableComponent("Split12Block")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_div")]
    public class Split12Block : SplitBlock
    {

        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            style.Columns.ColumnWidths = new Drawing.ColumnWidths(new double[] { 0.33 });
            return style;
        }
    }

    [PDFParsableComponent("Split21Block")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_div")]
    public class Split21Block : SplitBlock
    {

        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            style.Columns.ColumnWidths = new Drawing.ColumnWidths(new double[] { 0.66 });
            return style;
        }
    }

    [PDFParsableComponent("Split3Block")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_div")]
    public class Split3Block : SplitBlock
    {

        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            style.Columns.ColumnCount = 3;
            style.Columns.ColumnWidths = new Drawing.ColumnWidths(new double[] { 0.33, 0.33 });
            return style;
        }
    }



}
