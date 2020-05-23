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
    public class PDFBlock : PDFDiv
    {

        protected override PDFStyle GetBaseStyle()
        {
            PDFStyle style = base.GetBaseStyle();
            style.Padding.All = new Scryber.Drawing.PDFUnit(20, Drawing.PageUnits.Points);
            return style;
        }
    }

    [PDFParsableComponent("SplitBlock")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_div")]
    public class PDFSplitBlock : PDFBlock
    {

        protected override PDFStyle GetBaseStyle()
        {
            PDFStyle style = base.GetBaseStyle();
            style.Columns.ColumnCount = 2;
            return style;
        }
    }

    [PDFParsableComponent("Split12Block")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_div")]
    public class PDFSplit12Block : PDFSplitBlock
    {

        protected override PDFStyle GetBaseStyle()
        {
            PDFStyle style = base.GetBaseStyle();
            style.Columns.ColumnWidths = new Drawing.PDFColumnWidths(new double[] { 0.33 });
            return style;
        }
    }

    [PDFParsableComponent("Split21Block")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_div")]
    public class PDFSplit21Block : PDFSplitBlock
    {

        protected override PDFStyle GetBaseStyle()
        {
            PDFStyle style = base.GetBaseStyle();
            style.Columns.ColumnWidths = new Drawing.PDFColumnWidths(new double[] { 0.66 });
            return style;
        }
    }

    [PDFParsableComponent("Split3Block")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_div")]
    public class PDFSplit3Block : PDFSplitBlock
    {

        protected override PDFStyle GetBaseStyle()
        {
            PDFStyle style = base.GetBaseStyle();
            style.Columns.ColumnCount = 3;
            style.Columns.ColumnWidths = new Drawing.PDFColumnWidths(new double[] { 0.33, 0.33 });
            return style;
        }
    }



}
