using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    /// <summary>
    /// counter-increment and counter-reset on table rows (and cells), as well as the other components.
    /// </summary>
    [TestClass]
    public class CSSTableCounter_Tests
    {
        private static string Render(string css, string body)
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>@page{size:300pt 200pt;margin:10pt}" + css + "</style></head><body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                return Generation.PanelContent_Tests.TextOf(ms.ToArray());
            }
        }

        private const string RowCss = "table{counter-reset:r} tr.row{counter-increment:r} td.n::before{content:'R' counter(r)}";

        [TestMethod]
        public void RowIncrement_CountsEachRow()
        {
            var text = Render(RowCss, "<table><tbody><tr class='row'><td class='n'></td><td>A</td></tr><tr class='row'><td class='n'></td><td>B</td></tr><tr class='row'><td class='n'></td><td>C</td></tr></tbody></table>");
            Assert.IsTrue(text.Contains("R1") && text.Contains("R2") && text.Contains("R3"), text);
            Assert.IsFalse(text.Contains("R0"), text);
            Assert.IsFalse(text.Contains("R4"), text);
        }

        [TestMethod]
        public void RowIncrement_OnlyTheRowsWithTheCounter()
        {
            //the status row between has no counter, as in a contents list
            var text = Render(RowCss, "<table><tbody><tr class='row'><td class='n'></td><td>A</td></tr><tr><td colspan='2'>status</td></tr><tr class='row'><td class='n'></td><td>B</td></tr><tr><td colspan='2'>status</td></tr></tbody></table>");
            Assert.IsTrue(text.Contains("R1") && text.Contains("R2"), text);
            Assert.IsFalse(text.Contains("R3"), text);
        }

        [TestMethod]
        public void RowIncrement_ByAmount_AndSeparateTables()
        {
            var text = Render("table{counter-reset:r 10} tr{counter-increment:r 5} td.n::before{content:counter(r) '|'}",
                "<table><tbody><tr><td class='n'></td></tr><tr><td class='n'></td></tr></tbody></table><table><tbody><tr><td class='n'></td></tr></tbody></table>");
            Assert.IsTrue(text.Contains("15|") && text.Contains("20|"), text);
            //the second table is reset again
            Assert.IsFalse(text.Contains("25|"), text);
        }

        [TestMethod]
        public void RowIncrement_ContinuesOverPages_AndRepeatedHeadersAreNotCounted()
        {
            var rows = new System.Text.StringBuilder();
            for (int i = 0; i < 40; i++)
                rows.Append("<tr class='row'><td class='n'></td><td>Row " + i + "</td></tr>");

            var text = Render(RowCss, "<table><thead><tr><th colspan='2'>Head</th></tr></thead><tbody>" + rows + "</tbody></table>");
            Assert.IsTrue(text.Contains("R40"), "The last row is 40: " + text);
            Assert.IsFalse(text.Contains("R41"), text);
            Assert.AreEqual(1, System.Text.RegularExpressions.Regex.Matches(text, "R40").Count, "Counted once");
        }

        [TestMethod]
        public void CellIncrement_CountsEachCell()
        {
            var text = Render("table{counter-reset:c} td{counter-increment:c} td::before{content:'C' counter(c) ' '}",
                "<table><tbody><tr><td>A</td><td>B</td></tr><tr><td>C</td></tr></tbody></table>");
            Assert.IsTrue(text.Contains("C1") && text.Contains("C2") && text.Contains("C3"), text);
            Assert.IsFalse(text.Contains("C0"), text);
        }

        [TestMethod]
        public void RowIncrement_WithBlocksOutsideTheTable_StillCount()
        {
            var text = Render("main{counter-reset:s} section{counter-increment:s} " + RowCss + " section::before{content:'S' counter(s)}",
                "<main><section>one</section><table><tbody><tr class='row'><td class='n'></td><td>A</td></tr></tbody></table><section>two</section></main>");
            Assert.IsTrue(text.Contains("S1") && text.Contains("S2") && text.Contains("R1"), text);
        }
    }
}
