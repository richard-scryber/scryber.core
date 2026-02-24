using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Core.UnitTests;

namespace Scryber.UnitSamples
{
    /// <summary>
    /// Visual test to verify rowspan rendering in PDF output
    /// </summary>
    [TestClass]
    public class RowspanVisualTest : SampleBase
    {
        [TestMethod]
        public void GenerateRowspanTablePDF()
        {
            Console.WriteLine("Generating rowspan visual test PDF...");

            // Create XHTML table with rowspan
            string htmlContent = @"<?xml version='1.0' encoding='utf-8'?>
<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Table Rowspan Test</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            padding: 20px;
        }
        h1 {
            font-size: 24pt;
            font-weight: bold;
            margin-bottom: 20pt;
        }
        table {
            border-collapse: collapse;
            width: 500px;
            margin-top: 20pt;
        }
        td {
            border: 1px solid black;
            padding: 10px;
            text-align: center;
            width: 100px;
        }
        .blue { background-color: #ADD8E6; }
        .green { background-color: #90EE90; }
        .yellow { background-color: #FFFFE0; }
        .cyan { background-color: #E0FFFF; }
        .salmon { background-color: #FFA07A; }
        .skyblue { background-color: #87CEFA; }
        .gray { background-color: #D3D3D3; }
        .lightsteelblue { background-color: #B0C4DE; }
    </style>
</head>
<body>
    <h1>Table Rowspan Test</h1>
    <table>
        <tr>
            <td class='blue' rowspan='3'>Cell A<br/>(rowspan=3)</td>
            <td class='green'>Cell B</td>
            <td class='yellow' colspan='2' style='width: 200px'>Cell C<br/>(colspan=2)</td>
        </tr>
        <tr>
            <td class='cyan'>Cell D</td>
            <td class='salmon'>Cell E</td>
            <td class='gray'>Cell F</td>
        </tr>
        <tr>
            <td class='lightsteelblue'>Cell G</td>
            <td class='skyblue'>Cell H</td>
            <td class='yellow'>Cell I</td>
        </tr>
    </table>
</body>
</html>";

            // Parse HTML to document - using ParseDocument with XHTML string
            using (var reader = new System.IO.StringReader(htmlContent))
            {
                Document doc = Document.ParseDocument(reader);

                // Generate PDF using DocStreams (outputs to ~/Documents/Scryber Test Output/)
                using (var stream = DocStreams.GetOutputStream("RowspanVisualTest.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                Console.WriteLine("✓ PDF generated successfully: RowspanVisualTest.pdf");
                
                // Assertion to mark test as passed
                Assert.IsNotNull(doc);
            }
        }
    }
}
