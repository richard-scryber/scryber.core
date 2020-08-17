using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.UnitTests.Generation
{
    [TestClass()]
    public class TableHead_Test
    {

        public TestContext TextContext
        {
            get;
            set;
        }

        [TestMethod()]
        [TestCategory("Components")]
        public void GenerateTable()
        {
            PDFDocument doc = new PDFDocument();
            PDFPage pg = new PDFPage();
            doc.Pages.Add(pg);
            PDFTableGrid tb = new PDFTableGrid();
            pg.Contents.Add(tb);

            PDFLabel txt;
            PDFTableRow tbhead;
            PDFTableCell tbCellhead;

            
            tbhead = new PDFTableRow();
            tbhead.ID = "row";
            for (int iii = 0; iii < 9; iii++)
            {
                tbCellhead = new PDFTableCell();
                tbCellhead.ID = "cell_" + iii.ToString();
                tbhead.Cells.Add(tbCellhead);

                txt = new PDFLabel();
                txt.ID = "lbl_" + iii.ToString();
                txt.Text = "jijiji";
                tbCellhead.Contents.Add(txt);
                
            }
            tbhead.Repeat = TableRowRepeat.None;
            tb.Rows.Add(tbhead);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.ProcessDocument(ms);
            }
        }

    }
}
