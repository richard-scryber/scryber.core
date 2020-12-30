using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Generation
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
            Document doc = new Document();
            Page pg = new Page();
            doc.Pages.Add(pg);
            TableGrid tb = new TableGrid();
            pg.Contents.Add(tb);

            Label txt;
            TableRow tbhead;
            TableCell tbCellhead;

            
            tbhead = new TableRow();
            tbhead.ID = "row";
            for (int iii = 0; iii < 9; iii++)
            {
                tbCellhead = new TableCell();
                tbCellhead.ID = "cell_" + iii.ToString();
                tbhead.Cells.Add(tbCellhead);

                txt = new Label();
                txt.ID = "lbl_" + iii.ToString();
                txt.Text = "jijiji";
                tbCellhead.Contents.Add(txt);
                
            }
            tbhead.Repeat = TableRowRepeat.None;
            tb.Rows.Add(tbhead);

            using (var ms = DocStreams.GetOutputStream("TableHeadTest.pdf"))
            {
                doc.SaveAsPDF(ms);
            }
        }

    }
}
