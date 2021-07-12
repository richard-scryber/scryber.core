using System;
using System.Net.WebSockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Core.UnitTests.Mocks;
using System.Collections.Generic;
using Scryber.Binding;
using Newtonsoft.Json.Serialization;
using Scryber.Expressive;
using System.Runtime.Serialization;
using System.Xml.Schema;

namespace Scryber.Core.UnitTests.Binding
{
    /// <summary>
    /// Tests for the item binding expression
    /// </summary>
    [TestClass()]
    public class ExpressionBinding_Test
    {
        public TestContext TextContext
        {
            get;
            set;
        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindSingleExpression()
        {
            var str = "10 + 2";
            var expression = new Expression(str);
            var value = expression.Evaluate<double>();

            Assert.AreEqual(12.0, value);

            str = "10 / 2";

            expression = new Expression(str);
            value = expression.Evaluate<double>();

            Assert.AreEqual(5.0, value);

            str = "10 + (2 /4)";

            expression = new Expression(str);
            value = expression.Evaluate<double>();

            Assert.AreEqual(10.5, value);
        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindUnqualifiedVariableExpression()
        {
            Dictionary<string, object> vars = new Dictionary<string, object>();
            vars.Add("val1", 2);
            vars.Add("val2", 4);

            var str = "10 + val1";
            var expression = new Expression(str);
            var value = expression.Evaluate<double>(vars);

            Assert.AreEqual(12.0, value);

            str = "10 / val1";

            expression = new Expression(str);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(5.0, value);

            str = "10 + (val1 / val2)";

            expression = new Expression(str);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(10.5, value);
        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindDeepVariableExpression()
        {
            Dictionary<string, object> vars = new Dictionary<string, object>();
            vars.Add("val1", new { num = 2 });
            vars.Add("val2", 4);

            var str = "val1.num";
            var expression = new Expression(str);
            var value = expression.Evaluate<double>(vars);

            Assert.AreEqual(2.0, value);

            str = "10 + val1.num";
            expression = new Expression(str);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(12.0, value);

            str = "10 / val1.num";

            expression = new Expression(str);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(5.0, value);

            str = "10 + (val1.num / val2)";

            expression = new Expression(str);
            
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(10.5, value);
        }

        



        [TestMethod()]
        [TestCategory("Binding")]
        public void BindingCalcExpressions()
        {
            var factory = new BindingCalcExpressionFactory();

            
            Dictionary<string, object> vars = new Dictionary<string, object>();
            vars.Add("val1", new { num = 2, text = "a variable" });
            vars.Add("val2", 4);
            vars.Add("array1", new int[] { 1, 2, 3, 4 });


            var str = "Max(10, val1.num)";
            var expression = factory.CreateExpression(str, null);
            
            var value = expression.Expression.Evaluate<double>(vars);

            Assert.AreEqual(10.0, value);

            str = "Min(10,val1.num,val2)";

            expression = factory.CreateExpression(str, null);

            value = expression.Expression.Evaluate<double>(vars);

            Assert.AreEqual(2.0, value);

            str = "median(10,val2, val1.num)";

            expression = factory.CreateExpression(str, null);
            
            value = expression.Expression.Evaluate<double>(vars);

            Assert.AreEqual(4, value);


            str = "concat('a string and ', val1.text)";

            expression = factory.CreateExpression(str, null);

            var strResult = expression.Expression.Evaluate<string>(vars);

            Assert.AreEqual("a string and a variable", strResult);


        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindWithFunctionExpression()
        {
            Dictionary<string, object> vars = new Dictionary<string, object>();
            vars.Add("val1", new { num = 2, text = "a variable" });
            vars.Add("val2", 4);
            vars.Add("array1", new int[] { 1, 2, 3, 4 });

            var str = "Max(10, val1.num)";
            var expression = new Expression(str, ExpressiveOptions.IgnoreCaseForParsing);
            var value = expression.Evaluate<double>(vars);

            Assert.AreEqual(10.0, value, str + " did not evaluate correctly");

            str = "Min(10,val1.num,val2)";
            expression = new Expression(str);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(2.0, value, str + " did not evaluate correctly");


            str = "median(10,val2, val1.num)";
            expression = new Expression(str, Expressive.ExpressiveOptions.IgnoreCaseForParsing);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(4, value, str + " did not evaluate correctly");


            str = "concat('a string and ', val1.text)";
            expression = new Expressive.Expression(str, Expressive.ExpressiveOptions.IgnoreCaseForParsing);
            var strResult = expression.Evaluate<string>(vars);

            Assert.AreEqual("a string and a variable", strResult, str + " did not evaluate correctly");


            str = "array1[0] + array1[1]";
            expression = new Expressive.Expression(str, Expressive.ExpressiveOptions.IgnoreCaseForParsing);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(3.0, value, str + " did not evaluate correctly");

            str = "array1[0] + array1[val1.num + 1]"; //entry 0 and entry 2+1
            expression = new Expressive.Expression(str, Expressive.ExpressiveOptions.IgnoreCaseForParsing);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(5.0, value, str + " did not evaluate correctly");
        }



        [TestMethod()]
        [TestCategory("Binding")]
        public void BindCalcExpressionSimple()
        {
            var src = @"<!DOCTYPE html>
                        <?scryber parser-mode='strict' ?>
                        <html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Binding {{model.title}}</title>
                            </head>

                            <body id='mainbody' class='strong' >
                                <p id='myPara' style='border: solid 1px blue; padding: 5px;color:{{model.color}}' >This is a paragraph of content</p>
                            </body>

                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.Params["model"] = new { title = "Document title", color = "#FF0033" };

                using (var stream = DocStreams.GetOutputStream("BindCalculationSimple.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
                Assert.AreEqual("Binding Document title", doc.Info.Title, "Title is not correct");

                var p = doc.FindAComponentById("myPara") as IPDFStyledComponent;
                Assert.AreEqual((PDFColor)"#FF0033", p.Style.Fill.Color, "Paragraph color was not correct");
            }


        }

        /// <summary>
        /// Test for binding style values to parameters
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding")]
        public void BindCalcExpressionRepeat()
        {

            var src = @"<!DOCTYPE html>
                        <?scryber parser-mode='strict' append-log='true' ?>
                        <html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>{{model.title}}</title>
                                <style>
                                    .even{ background-color: #FF0000; }
                                </style>
                            </head>

                            <body id='mainbody' class='strong' style='padding:{{model.padding}}; color: {{model.color}}' >
                                <p id='myPara' style='border: solid 1px blue; padding: 5px;' >This is a paragraph of content</p>
                                <table id='myTable' style='width:100%;font-size:14pt' >
                                    <template data-bind='{@:model.items}' >
                                        <tr class='bound-item {{if(.index % 2 == 0, ""even"", ""odd"")}}' >
                                            <td style='width:20pt'>{{1 + .index}}</td>
                                            <td style='width:70pt'>{{.name}}</td>
                                            <td>This is the cell index {{1 + .index}} with name {{.name}}</td>
                                        </tr>
                                    </template>
                                </table>
                            </body>

                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.Params["model"] = new {
                    title = "Document title",
                    color = "#330033",
                    bg = "#AAA",
                    padding = "20pt",
                    items = new []
                    {
                        new {name = "First Item", index = 0},
                        new {name = "Second Item", index = 1},
                        new {name = "Third Item", index = 2},
                    }
                };

                using (var stream = DocStreams.GetOutputStream("BindCalcExpressionRepeat.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                //Assertions

                //Bound title
                Assert.AreEqual("Document title", doc.Info.Title, "Title is not correct");

                //body color
                var body = doc.FindAComponentById("mainbody") as IPDFStyledComponent;
                Assert.AreEqual((PDFColor)"#330033", body.Style.Fill.Color, "Body color was not correct");

                //table binding
                var table = doc.FindAComponentById("myTable") as TableGrid;
                var rowCount = table.Rows.Count;
                Assert.AreEqual(3, rowCount, "Nuumber of rows was wrong");


                for (int i = 0; i < rowCount; i++)
                {
                    //Table row class
                    var row = table.Rows[i];
                    if (i % 2 == 0)
                        Assert.AreEqual("bound-item even", row.StyleClass, "Style class is not correct for the row");
                    else
                        Assert.AreEqual("bound-item odd", row.StyleClass, "Style class is not correct for the row");

                    var cell = row.Cells[0];
                    var content = cell.Contents[0] as TextLiteral;

                    Assert.AreEqual((1 + i).ToString(), content.Text, "Text Literal for 1 +.index does not match");
                }

            }

            

        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindCalcAllFunctions()
        {
            var model = new
            {
                number = 20.7,
                boolean = true,
                array = new[] { 10.0, 11.0, 12.0 },
                str = "#330033",
                bg = "#AAA",
                padding = "20pt",
                items = new[]
                    {
                        new {name = "First Item", index = 0},
                        new {name = "Second Item", index = 1},
                        new {name = "Third Item", index = 2},
                    }
            };

            var functions = new[]
            {
                new {grp = "Binary Operators", name = "Plus (+)", function = "12 + 4", result = "16"},
                new {grp = "Binary Operators", name = "Minus (-)", function = "12 - 4", result = "8"},
                new {grp = "Binary Operators", name = "Multiply (*)", function = "12 * 4", result = "48"},
                new {grp = "Binary Operators", name = "Divide (/)", function = "12 / 4", result = "3"},
                new {grp = "Binary Operators", name = "All (+-*/)", function = "12 + 2 - 6 * 2 / 4", result = "11"},
                new {grp = "Binary Operators", name = "All (+-*/)", function = "((12 + 2) - 6) * (2 / 4)", result = "4"},
                new {grp = "Binary Operators", name = "Modulo (mod) ", function = "11 mod 3", result = "2"},
                new {grp = "Binary Operators", name = "Modulo (%) ", function = "11 % 2", result = "1"},
                new {grp = "Binary Operators", name = "Bitwise And (&amp;) ", function = "11 &amp; 7", result = "3"},
                new {grp = "Binary Operators", name = "Bitwise Or (|) ", function = "11 | 7", result = "15"},
                new {grp = "Binary Operators", name = "Bitwise Left (&lt;&lt;) ", function = "11 &lt;&lt; 2", result = "44"},
                new {grp = "Binary Operators", name = "Bitwise Right (&gt;&gt;) ", function = "11 &gt;&gt; 2", result = "2"},
                new {grp = "Binary Operators", name = "Null Coalesce ", function = "model.number ?? 4", result = "20.7"},
                new {grp = "Binary Operators", name = "Null Coalesce ", function = "model.notset ?? 4", result = "4"},
                new {grp = "Binary Operators", name = "Null Coalesce ", function = "notset ?? 'empty'", result = "empty"},

                new {grp = "Relational Operators", name = "Equals", function = "model.number == 20.7", result = "True"},
                new {grp = "Relational Operators", name = "Equals", function = "model.number == 10.7", result = "False"},
                new {grp = "Relational Operators", name = "Equals", function = "if(model.number == 20.7,'Equal','Not Equal')", result = "Equal"},
                new {grp = "Relational Operators", name = "Equals", function = "if(model.number == 10.7,'Equal','Not Equal')", result = "Not Equal"},
                new {grp = "Relational Operators", name = "Equals Null", function = "if(model.number == null,'Is Null','Not Null')", result = "Not Null"},
                new {grp = "Relational Operators", name = "Equals Null", function = "if(model.notset == null,'Is Null','Not Null')", result = "Is Null"},

                new {grp = "Relational Operators", name = "Not Equal", function = "model.number != 20.7", result = "False"},
                new {grp = "Relational Operators", name = "Not Equal", function = "model.number != 10.7", result = "True"},
                new {grp = "Relational Operators", name = "Not Equal", function = "if(model.number != 20.7,'Equal','Not Equal')", result = "Not Equal"},
                new {grp = "Relational Operators", name = "Not Equal", function = "if(model.number != 10.7,'Equal','Not Equal')", result = "Equal"},
                new {grp = "Relational Operators", name = "Not Equal Null", function = "if(model.number != null,'Not Null','Is Null')", result = "Not Null"},
                new {grp = "Relational Operators", name = "Not Equal Null", function = "if(model.notset != null,'Not Null','Is Null')", result = "Is Null"},

                new {grp = "Relational Operators", name = "Not Equal", function = "model.number &lt;&gt; 20.7", result = "False"},
                new {grp = "Relational Operators", name = "Not Equal", function = "model.number &lt;&gt; 10.7", result = "True"},
                new {grp = "Relational Operators", name = "Not Equal", function = "if(model.number &lt;&gt; 20.7,'Equal','Not Equal')", result = "Not Equal"},
                new {grp = "Relational Operators", name = "Not Equal", function = "if(model.number &lt;&gt; 10.7,'Equal','Not Equal')", result = "Equal"},

                new {grp = "Relational Operators", name = "Greater than", function = "model.number &gt; 21", result = "False"},
                new {grp = "Relational Operators", name = "Greater than", function = "model.number &gt; 10", result = "True"},
                new {grp = "Relational Operators", name = "Greater than", function = "if(model.number &gt; 21,'Greater','Less')", result = "Less"},
                new {grp = "Relational Operators", name = "Greater than", function = "if(model.number &gt; 10,'Greater','Less')", result = "Greater"},

                new {grp = "Relational Operators", name = "Less than", function = "model.number &lt; 21", result = "True"},
                new {grp = "Relational Operators", name = "Less than", function = "model.number &lt; 10", result = "False"},
                new {grp = "Relational Operators", name = "Less than", function = "if(model.number &lt; 21,'Less','Greater')", result = "Less"},
                new {grp = "Relational Operators", name = "Less than", function = "if(model.number &lt; 10,'Less','Greater')", result = "Greater"},

                new {grp = "Relational Operators", name = "Greater than or Equal", function = "model.number &gt;= 21", result = "False"},
                new {grp = "Relational Operators", name = "Greater than or Equal", function = "model.number &gt;= 20.7", result = "True"},
                new {grp = "Relational Operators", name = "Greater than or Equal", function = "model.number &gt;= 10", result = "True"},
                new {grp = "Relational Operators", name = "Greater than or Equal", function = "if(model.number &gt;= 21,'Greater or equal','Less')", result = "Less"},
                new {grp = "Relational Operators", name = "Greater than or Equal", function = "if(model.number &gt;= 20.7,'Greater or equal','Less')", result = "Greater or equal"},
                new {grp = "Relational Operators", name = "Greater than or Equal", function = "if(model.number &gt;= 10,'Greater or equal','Less')", result = "Greater or equal"},

                new {grp = "Relational Operators", name = "Less than or Equal", function = "model.number &lt;= 21", result = "True"},
                new {grp = "Relational Operators", name = "Less than or Equal", function = "model.number &lt;= 20.7", result = "True"},
                new {grp = "Relational Operators", name = "Less than or Equal", function = "model.number &lt;= 10", result = "False"},
                new {grp = "Relational Operators", name = "Less than or Equal", function = "if(model.number &lt;= 21,'Less or equal','Greater')", result = "Less or equal"},
                new {grp = "Relational Operators", name = "Less than or Equal", function = "if(model.number &lt;= 20.7,'Less or equal','Greater')", result = "Less or equal"},
                new {grp = "Relational Operators", name = "Less than or Equal", function = "if(model.number &lt;= 10,'Less or equal','Greater')", result = "Greater"},

                new {grp = "Logical Operators", name = "And (&amp;&amp;)", function = "if(model.number &lt; 21 &amp;&amp; model.number &gt; 20,'Between','Outside')", result = "Between"},
                new {grp = "Logical Operators", name = "And", function = "if(model.number &lt; 20 and model.number &gt; 10,'Between','Outside')", result = "Outside"},

                new {grp = "Logical Operators", name = "Or (||)", function = "if(model.number &lt; 20 || model.number &gt; 21,'Outside','Between')", result = "Between"},
                new {grp = "Logical Operators", name = "Or", function = "if(model.number &gt; 20 or model.number &gt; 30,'Between','Outside')", result = "Between"},

                new {grp = "Logical Operators", name = "Not (!)", function = "if(!(model.number &lt; 21 &amp;&amp; model.number &gt; 20),'Outside','Between')", result = "Between"},
                new {grp = "Logical Operators", name = "Not", function = "if(not(model.number &lt; 20 and model.number &gt; 21),'Between','Outside')", result = "Between"},

                new {grp = "Conversion Functions", name = "Date", function = "date('30 June 2021 11:00:00')", result = "06/30/2021 11:00:00"},
                new {grp = "Conversion Functions", name = "Decimal", function = "decimal(20 + model.number)", result = (20 + model.number).ToString()},
                new {grp = "Conversion Functions", name = "Double", function = "double(20 + model.number)", result = (20 + model.number).ToString()},
                new {grp = "Conversion Functions", name = "Integer", function = "integer(20 + model.number)", result = Convert.ToInt32(20 + model.number).ToString()},
                new {grp = "Conversion Functions", name = "Long", function = "long(20 + model.number)", result = Convert.ToInt64(20 + model.number).ToString()},
                new {grp = "Conversion Functions", name = "string", function = "string(20 + model.number)", result = (20 + model.number).ToString()},

                new {grp = "Date Add Functions", name = "AddDays", function = "adddays(date('30 June 2021 11:00:00'),10)", result = "07/10/2021 11:00:00"},
                new {grp = "Date Add Functions", name = "AddHours", function = "addhours(date('30 June 2021 11:00:00'),10)", result = "06/30/2021 21:00:00"},
                new {grp = "Date Add Functions", name = "AddMilliSeconds", function = "addmilliseconds(date('30 June 2021 11:00:00'),2000)", result = "06/30/2021 11:00:02"},
                new {grp = "Date Add Functions", name = "AddMinutes", function = "addMinutes(date('30 June 2021 11:00:00'),40)", result = "06/30/2021 11:40:00"},
                new {grp = "Date Add Functions", name = "AddMonths", function = "addMonths(date('30 June 2021 11:00:00'),2)", result = "08/30/2021 11:00:00"},
                new {grp = "Date Add Functions", name = "AddSeconds", function = "addSeconds(date('30 June 2021 11:00:00'),100)", result = "06/30/2021 11:01:40"},
                new {grp = "Date Add Functions", name = "AddYears", function = "addYears(date('30 June 2021 11:00:00'),1000)", result = "06/30/3021 11:00:00"},

                new {grp = "Date Of Functions", name = "DayOf", function = "dayof(date('30 June 2021 11:40:10.345'))", result = "30"},
                new {grp = "Date Of Functions", name = "HourOf", function = "HourOf(date('30 June 2021 11:40:10.345'))", result = "11"},
                new {grp = "Date Of Functions", name = "MillisecondOf", function = "MillisecondOf(date('30 June 2021 11:40:10.345'))", result = "345"},
                new {grp = "Date Of Functions", name = "MinuteOf", function = "MinuteOf(date('30 June 2021 11:40:10.345'))", result = "40"},
                new {grp = "Date Of Functions", name = "MonthOf", function = "MonthOf(date('30 June 2021 11:40:10.345'))", result = "6"},
                new {grp = "Date Of Functions", name = "SecondOf", function = "SecondOf(date('30 June 2021 11:40:10.345'))", result = "10"},
                new {grp = "Date Of Functions", name = "YearOf", function = "YearOf(date('30 June 2021 11:40:10.345'))", result = "2021"},

                new {grp = "Date Between Functions", name = "DaysBetween", function = "DaysBetween(date('11 June 2021 11:40:10.345'),#30 June 2021 11:40:10.345#)", result = "19"},
                new {grp = "Date Between Functions", name = "HoursBetween", function = "HoursBetween(date('30 June 2021 01:40:10.345'),#30 June 2021 11:40:10.345#)", result = "10"},
                new {grp = "Date Between Functions", name = "MillisecondsBetween", function = "MillisecondsBetween(date('30 June 2021 11:40:10.100'),#30 June 2021 11:40:10.345#)", result = "245"},
                new {grp = "Date Between Functions", name = "MinutesBetween", function = "MinutesBetween(date('30 June 2021 11:40:10.345'),#30 June 2021 11:13:10.345#)", result = "-27"},
                new {grp = "Date Between Functions", name = "SecondsBetween", function = "SecondsBetween(date('30 June 2021 11:40:10.345'),#30 June 2021 11:41:56.345#)", result = "106"},

                new {grp = "Logical Functions", name = "If", function = "If(model.boolean == true,24,'none')", result = "24"},
                new {grp = "Logical Functions", name = "If", function = "If(!model.boolean,24,'none')", result = "none"},
                new {grp = "Logical Functions", name = "In", function = "In(12.2,10.0,11.1,12.2)", result = "True"},
                new {grp = "Logical Functions", name = "In", function = "In(12,10.0,11.1,12.2)", result = "False"},
                new {grp = "Logical Functions", name = "In", function = "In(12,model.array)", result = "True"},

                new {grp = "Mathematical Functions", name = "Abs", function = "abs(-12) + abs(model.number)", result = "32.7"},
                new {grp = "Mathematical Functions", name = "Truncate", function = "truncate(Pi())", result = "3"},
                new {grp = "Mathematical Functions", name = "Truncate", function = "truncate(9.99999)", result = "9"},
                new {grp = "Mathematical Functions", name = "Round", function = "round(Log10(30),3)", result = Math.Round(Math.Log10(30),3).ToString("#0.000")},
                new {grp = "Mathematical Functions", name = "Sign", function = "sign(30)", result = "1"},
                new {grp = "Mathematical Functions", name = "Sign", function = "sign(-300)", result = "-1"},
                new {grp = "Mathematical Functions", name = "Sign", function = "sign(0)", result = "0"},
                new {grp = "Mathematical Functions", name = "ACos", function = "round(ACos(-0.5),3)", result = "2.094"},
                new {grp = "Mathematical Functions", name = "ASin", function = "round(ASin(-0.5),3)", result = "-0.524"},
                new {grp = "Mathematical Functions", name = "ATan", function = "round(ATan(2.0),3)", result = "1.107"},
                new {grp = "Mathematical Functions", name = "Ceiling", function = "Ceiling(3.2)", result = "4"},
                new {grp = "Mathematical Functions", name = "Floor", function = "Floor(3.2)", result = "3"},
                new {grp = "Mathematical Functions", name = "ACos", function = "round(Cos(2.094),1)", result = "-0.5"},
                new {grp = "Mathematical Functions", name = "ASin", function = "round(Sin(-0.524),1)", result = "-0.5"},
                new {grp = "Mathematical Functions", name = "ATan", function = "round(Tan(1.107),1)", result = "2"},
                new {grp = "Mathematical Functions", name = "E", function = "round(E(),3)", result = "2.718"},
                new {grp = "Mathematical Functions", name = "Pi", function = "round(Pi(),3)", result = "3.142"},
                new {grp = "Mathematical Functions", name = "Exp", function = "round(exp(3),3)", result = Math.Exp(3).ToString("#0.000")},
                new {grp = "Mathematical Functions", name = "Log10", function = "round(Log10(30),3)", result = Math.Log10(30).ToString("#0.000")},
                new {grp = "Mathematical Functions", name = "Log (base 5)", function = "round(Log(30,5),3)", result = Math.Log(30,5).ToString("#0.000")},
                new {grp = "Mathematical Functions", name = "Pow", function = "pow(3,2)", result = Math.Pow(3,2).ToString()},
                new {grp = "Mathematical Functions", name = "Random (between 2 and 30)", function = "random(2,30)", result = (string)null},
                new {grp = "Mathematical Functions", name = "Random (between 2.4d and 2.6d)", function = "random(2.4, 2.6)", result = (string)null},


                new {grp = "Aggregate and Statistical Functions", name = "Count", function = "count(12, 13, 14)", result = "3"},
                new {grp = "Aggregate and Statistical Functions", name = "Count (with array)", function = "count(12, model.array, model.items, 14)", result = "8"},
                new {grp = "Aggregate and Statistical Functions", name = "Sum", function = "sum(model.array,13,14)", result = (13 + 14 + 10 + 11 + 12 ).ToString()},
                new {grp = "Aggregate and Statistical Functions", name = "Max", function = "Max(model.array,13,14)", result = "14"},
                new {grp = "Aggregate and Statistical Functions", name = "Min", function = "Min(model.array,13,14)", result = "10"},
                new {grp = "Aggregate and Statistical Functions", name = "Average", function = "Average(model.array,13,1)", result = "9.4"},
                new {grp = "Aggregate and Statistical Functions", name = "Mean", function = "Mean(model.array,13,1)", result = "9.4"},
                new {grp = "Aggregate and Statistical Functions", name = "Mode", function = "Mode(model.array,13,1)", result = "10"},
                new {grp = "Aggregate and Statistical Functions", name = "Median", function = "Median(model.array,13,1)", result = "11"},


                new {grp = "String Functions", name = "Concat", function = "concat('A string',' and another string')", result = "A string and another string"},
                new {grp = "String Functions", name = "Concat (with types)", function = "concat('numbers:',9, model.array)", result = "numbers:9101112"},
                new {grp = "String Functions", name = "Join", function = "join('-', 9, 12, 13)", result = "9-12-13"},
                new {grp = "String Functions", name = "Join (with types)", function = "join(', ', 9, model.array, 13)", result = "9, 10, 11, 12, 13"},
                new {grp = "String Functions", name = "Contains", function = "contains('a string','str')", result = "True"},
                new {grp = "String Functions", name = "Contains", function = "contains('a string','l')", result = "False"},
                new {grp = "String Functions", name = "StartsWith", function = "startsWith('a string','a ')", result = "True"},
                new {grp = "String Functions", name = "StartsWith", function = "startsWith('a string','s')", result = "False"},
                new {grp = "String Functions", name = "EndsWith", function = "endsWith('a string','ing')", result = "True"},
                new {grp = "String Functions", name = "EndsWith", function = "endsWith('a string','int')", result = "False"},
                new {grp = "String Functions", name = "EndsWith (+ If)", function = "if(endsWith('a string','ing'),'Yes','No')", result = "Yes"},
                new {grp = "String Functions", name = "IndexOf", function = "indexOf('a string','ing')", result = "5"},
                new {grp = "String Functions", name = "IndexOf", function = "indexOf('a string','int')", result = "-1"},
                new {grp = "String Functions", name = "Length", function = "length('a string')", result = "8"},
                new {grp = "String Functions", name = "Length", function = "length(model.items[0].name)", result = "First Item".Length.ToString()},
                new {grp = "String Functions", name = "PadLeft", function = "padleft(model.items[0].name, 20, '+')", result = "++++++++++First Item"},
                new {grp = "String Functions", name = "PadRight", function = "padright(model.items[0].name, 20, '-')", result = "First Item----------"},
                new {grp = "String Functions", name = "PadRight", function = "padleft(padright(model.items[0].name, 15, '-'),20,'+++++')", result = "+++++First Item-----"},
                new {grp = "String Functions", name = "SubString", function = "substring(model.items[0].name, 6)", result = "Item"},
                new {grp = "String Functions", name = "SubString", function = "substring(model.items[0].name, 6, 2)", result = "It"},
                new {grp = "String Functions", name = "Regex", function = "regex('The quick brown fox','\\squick\\s')", result = "True"},
                new {grp = "String Functions", name = "Regex", function = "regex('The quick brown fox','\\sjump\\s')", result = "False"},
            };

            var src = @"<!DOCTYPE html>
                        <?scryber parser-mode='strict' append-log='true' ?>
                        <html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Expression Functions</title>
                                <style>
                                    .even{ background-color: #FF0000; }
                                </style>
                            </head>

                            <body id='mainbody' class='strong' style='padding:20pt' >
                                <h2>Expression functions and operators</h2>
                                <table id='myTable' style='width:100%;font-size:10pt' >";
            
            var group = "";
            foreach (var fn in functions)
            {

                if (fn.grp != group)
                {
                    src += @"<tr><td colspan='3' ><h3>" + fn.grp + "</h3></td></tr>";
                    group = fn.grp;
                }
                var insert = @"  <tr class='bound-item' >
                                    <td style='width:120pt'>" + fn.name + @"</td>
                                    <td>" + fn.function + @"</td>
                                    <td>{{" + fn.function + @"}}</td>
                                 </tr>";
                src += insert;
            }

            src += @"           </table>
                            </body>
                        </html>";


            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.Params["model"] = model;

                using (var stream = DocStreams.GetOutputStream("BindCalcAllFunctions.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                //table binding
                var table = doc.FindAComponentById("myTable") as TableGrid;


                int grpIndex = 0;
                group = "";

                for (int i = 0; i < functions.Length; i++)
                {
                    var fn = functions[i];

                    if (fn.grp != group)
                    {
                        group = fn.grp;
                        grpIndex++;
                    }

                    if (fn.result == null)
                        continue;

                    //Table row class
                    var row = table.Rows[i + grpIndex];
                    var cell = row.Cells[2];
                    var content = cell.Contents[0] as TextLiteral;

                    Assert.AreEqual(fn.result, content.Text, "Text Literal for " + fn.function + " does not match");
                }
            }

            //Performance check for binding
            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.Params["model"] = model;

                using (var stream = DocStreams.GetOutputStream("BindCalcAllFunctions_repeat.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }


        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindXmlAndTemplateObject()
        {
            //var expectedXml = @"<node value='1' >
            //                        <inner value='1' />
            //                        <inner value='2' />
            //                    </node>";

            //var expectedTemplate = @"<doc:Div id='{xpath:concat(""xmlInnerDiv"",@value)}' >
            //                            <doc:Label id='{xpath:concat(""xmlLabel"",@value)}' text='{xpath:@value}' />
            //                         </doc:Div>";

            //var src = @"<?xml version='1.0' encoding='utf-8' ?>
            //            <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
            //                        xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
            //                        xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
            //                         >

            //            <Params>
            //                <doc:Xml-Param id='xml' >" + expectedXml + @"</doc:Xml-Param>
            //                <doc:Template-Param id='template' >" + expectedTemplate + @"</doc:Template-Param>
            //            </Params>

            //            <Pages>
    
            //            <doc:Section>
            //                <Content>
            //                    <data:ForEach id='Foreach2' value='{@:xml}' select='//node/inner' template='{@:template}' ></data:ForEach>
            //                </Content>
            //            </doc:Section>

            //            </Pages>
            //        </doc:Document>";

            //using (var reader = new System.IO.StringReader(src))
            //{
            //    var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

            //    doc.InitializeAndLoad();
            //    doc.DataBind();

            //    //For the ForEach template with an object source.
            //    var first = doc.FindAComponentById("xmlInnerDiv1") as Div;
            //    Assert.IsNotNull(first, "Could not find inner div");
                
            //    var second = doc.FindAComponentById("xmlLabel2") as Label;
            //    Assert.IsNotNull(second, "Could not find the second label");
            //    Assert.AreEqual("2", second.Text, "The second label does not have the correct text value");

            //}
        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindTemplatePlaceholder()
        {
            //var expectedXml = @"<node value='1' >
            //                        <inner value='1' />
            //                        <inner value='2' />
            //                    </node>";

            //var expectedTemplate = @"<doc:Div id='{xpath:concat(""xmlInnerDiv"",@value)}' >
            //                            <doc:Label id='{xpath:concat(""xmlLabel"",@value)}' text='{xpath:@value}' />
            //                         </doc:Div>";

            //var src = @"<?xml version='1.0' encoding='utf-8' ?>
            //            <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
            //                        xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
            //                        xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
            //                         >

            //            <Params>
            //                <doc:Xml-Param id='xml' >" + expectedXml + @"</doc:Xml-Param>
            //                <doc:Template-Param id='template' >" + expectedTemplate + @"</doc:Template-Param>
            //            </Params>

            //            <Pages>
    
            //                <doc:Section>
            //                    <Content>
                                
            //                        <data:ForEach id='Foreach2' value='{@:xml}' select='//node/inner' >
            //                            <Template>
            //                                <doc:PlaceHolder template='{@:template}' />
            //                            </Template>
            //                        </data:ForEach>

            //                    </Content>
            //                </doc:Section>

            //            </Pages>
            //        </doc:Document>";

            //using (var reader = new System.IO.StringReader(src))
            //{
            //    var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

            //    doc.InitializeAndLoad();
            //    doc.DataBind();

            //    //For the ForEach template with an object source.
            //    var first = doc.FindAComponentById("xmlInnerDiv1") as Div;
            //    Assert.IsNotNull(first, "Could not find inner div");

            //    var second = doc.FindAComponentById("xmlLabel2") as Label;
            //    Assert.IsNotNull(second, "Could not find the second label");

            //    Assert.AreEqual("2", second.Text, "The second label does not have the correct text value");



            //}

            //using (var reader = new System.IO.StringReader(src))
            //{
            //    var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

            //    doc.Params["template"] = @"<doc:H1 id='{xpath:concat(""xmlH"",@value)}' >
            //                            <doc:Text id='{xpath:concat(""xmlText"",@value)}' value='{xpath:@value}' />
            //                         </doc:H1>";
            //    doc.InitializeAndLoad();
            //    doc.DataBind();

            //    //For the ForEach template with an object source.
            //    var first = doc.FindAComponentById("xmlH1") as Head1;
            //    Assert.IsNotNull(first, "Could not find inner heading");

            //    var second = doc.FindAComponentById("xmlText2") as TextLiteral;
            //    Assert.IsNotNull(second, "Could not find the second label");

            //    Assert.AreEqual("2", second.Text, "The second label does not have the correct text value");



            //}


        }


        /// <summary>
        /// Check that the correct types are assigned at runtime
        /// </summary>
        [TestMethod()]
        public void BindingTypeSafety()
        {
            //var src = @"<?xml version='1.0' encoding='utf-8' ?>
            //            <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
            //                        xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
            //                        xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
            //                         >
            //            <Params>
            //                <doc:String-Param id='string' ></doc:String-Param>
            //                <doc:Int-Param id='int' ></doc:Int-Param>
            //                <doc:Color-Param id='color' ></doc:Color-Param>
            //            </Params>


            //            <Pages>
            //                <doc:Section>
            //                    <Content>
            //                        <doc:Label id='{@:int}' text='{@:string}' styles:bg-color='{@:color}' ></doc:Label>
            //                    </Content>
            //                </doc:Section>
            //            </Pages>
            //        </doc:Document>";

            //using (var reader = new System.IO.StringReader(src))
            //{
            //    var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            //    var color = new Scryber.Drawing.PDFColor(1, 0, 0);
            //    var text = "This is the title";
            //    var date = DateTime.Now;
            //    var i = 5;

            //    doc.Params["color"] = color;
            //    doc.Params["string"] = text;
            //    doc.Params["int"] = i;

            //    doc.InitializeAndLoad();
            //    doc.DataBind();


            //    //Find the label as the value should be converted to a string.
            //    var first = doc.FindAComponentById(i.ToString()) as Label;
            //    Assert.IsNotNull(first, "Could not find the label");

            //    //Check that the text matches
            //    Assert.AreEqual(text, first.Text, "The first label does not have the correct text value");

            //    //Check that the color matches
            //    Assert.AreEqual(color, first.BackgroundColor, "Background colours do not match");


            //}

            //using (var reader = new System.IO.StringReader(src))
            //{
            //    var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            //    var color = new Scryber.Drawing.PDFColor(1, 0, 0);
            //    var text = "This is the title";
            //    var date = DateTime.Now;
            //    var i = 5;

            //    bool caught = false;

            //    try
            //    {

            //        //This should not be allowed 
            //        doc.Params["color"] = text;
            //        doc.Params["string"] = text;
            //        doc.Params["int"] = i;

            //        doc.InitializeAndLoad();
            //        //doc.DataBind();
            //    }
            //    catch(Scryber.PDFDataException)
            //    {
            //        caught = true;
            //    }

            //    Assert.IsTrue(caught, "The assignment of an incorrect type onto the parameter did not raise an error");


            //}
        }


        /// <summary>
        /// Checks the asssignment of a string onto items
        /// </summary>
        [TestMethod()]
        public void BindingParamToString()
        {
            //var src = @"<?xml version='1.0' encoding='utf-8' ?>
            //            <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
            //                        xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
            //                        xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
            //                         >
            //            <Params>
            //                <doc:String-Param id='string' ></doc:String-Param>
            //                <doc:Int-Param id='int' ></doc:Int-Param>
            //                <doc:Color-Param id='color' ></doc:Color-Param>
            //            </Params>


            //            <Pages>
            //                <doc:Section>
            //                    <Content>
            //                        <doc:Label id='{@:int}' text='{@:string}' styles:bg-color='{@:color}' ></doc:Label>
            //                    </Content>
            //                </doc:Section>
            //            </Pages>
            //        </doc:Document>";

            //using (var reader = new System.IO.StringReader(src))
            //{
            //    var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            //    var color = new Scryber.Drawing.PDFColor(1, 0, 0);
            //    var text = "This is the title";
            //    var date = DateTime.Now;
            //    var i = 5;

            //    doc.Params["color"] = color.ToString();
            //    doc.Params["string"] = text;
            //    doc.Params["int"] = i.ToString();

            //    doc.InitializeAndLoad();
            //    doc.DataBind();


            //    //Find the label as the value should be converted to a string.
            //    var first = doc.FindAComponentById(i.ToString()) as Label;
            //    Assert.IsNotNull(first, "Could not find the label");

            //    //Check that the text matches
            //    Assert.AreEqual(text, first.Text, "The first label does not have the correct text value");

            //    //Check that the color matches
            //    Assert.AreEqual(color, first.BackgroundColor, "Background colours do not match");


            //}

            
        }


        /// <summary>
        /// Checks the asssignment of a subclass object onto as strongly typed object
        /// </summary>
        [TestMethod()]
        public void BindingParamToStrongObject()
        {
            //var src = @"<?xml version='1.0' encoding='utf-8' ?>
            //            <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
            //                        xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
            //                        xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
            //                         >
            //            <Params>
            //                <doc:Object-Param id='obj' type='Scryber.Core.UnitTests.Mocks.MockParameter, Scryber.UnitTests' ></doc:Object-Param>
            //            </Params>

            //            <Pages>
            //                <doc:Section>
            //                    <Content>
            //                        <doc:Label id='MyTitle' styles:font-bold='{@:obj.BoldTitle}' text='{@:obj.Title}' styles:font-size='{@:obj.SizeField}' styles:bg-color='{@:obj.Background}' ></doc:Label>
            //                    </Content>
            //                </doc:Section>
            //            </Pages>
            //        </doc:Document>";

            //using (var reader = new System.IO.StringReader(src))
            //{
            //    var param = new Mocks.MockSubParameter();

            //    var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

            //    doc.Params["obj"] = param;

            //    doc.InitializeAndLoad();
            //    doc.DataBind();


            //    //Find the label as the value should be converted to a string.
            //    var first = doc.FindAComponentById("MyTitle") as Label;
            //    Assert.IsNotNull(first, "Could not find the label");

            //    //Check that the text matches
            //    Assert.AreEqual(param.Title, first.Text, "The first label does not have the correct text value");

            //    //Check that the color matches
            //    Assert.AreEqual(param.Background, first.BackgroundColor, "Background colours do not match");

            //    Assert.AreEqual(param.SizeField, first.FontSize);


            //}


            //using (var reader = new System.IO.StringReader(src))
            //{
            //    var param = new Mocks.MockOtherParameter();

            //    var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            //    var caught = false;
            //    try
            //    {
            //        doc.Params["obj"] = param;
            //    }
            //    catch(Scryber.PDFDataException)
            //    {
            //        caught = true;
            //    }

            //    Assert.IsTrue(caught, "No exception was raised.");

                
            //}


        }



        [TestMethod()]
        [TestCategory("Binding")]
        public void BindDynamicStyles()
        {

        //    var src = @"<?xml version='1.0' encoding='utf-8' ?>
        //                <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
        //                            xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
        //                            xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
        //                             >
        //                <Params>
        //                    <doc:Object-Param id='dynamic' ></doc:Object-Param>
        //                </Params>

        //                <Styles>

        //                    <styles:Style applied-class='head'>
        //                        <styles:Background color='{@:dynamic.Theme.TitleBg}' />
        //                        <styles:Font family='{@:dynamic.Theme.TitleFont}' />
        //                        <styles:Fill color='{@:dynamic.Theme.TitleColor}' />
        //                    </styles:Style>

        //                    <styles:Style applied-class='body'>
        //                        <styles:Font family='{@:dynamic.Theme.BodyFont}' size='{@:dynamic.Theme.BodySize}' />
        //                    </styles:Style>
    
        //                </Styles>

        //                <Pages>
    
        //                <doc:Section>
        //                    <Content>
        //                        <doc:H1 styles:class='head' text='{@:Title}' ></doc:H1>
        //                        <data:ForEach value='{@:dynamic.List}' >
        //                            <Template>
        //                                <doc:Label styles:class='body' id='{@:.Id}' text='{@:.Name}' ></doc:Label>
        //                                <doc:Br/>
        //                            </Template>
        //                        </data:ForEach>

        //                    </Content>
        //                </doc:Section>

        //                </Pages>
        //            </doc:Document>";

        //    using (var reader = new System.IO.StringReader(src))
        //    {
        //        var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

        //        var binding = new
        //        {
        //            Title = "This is the document title",
        //            List = new[] {
        //            new { Name = "First", Id = "FirstID" },
        //            new { Name = "Second", Id = "SecondID" }
        //        },
        //            Theme = new
        //            {
        //                TitleBg = new PDFColor(1, 0, 0),
        //                TitleColor = new PDFColor(1, 1, 1),
        //                TitleFont = (PDFFontSelector)"Segoe UI Light",
        //                BodyFont = (PDFFontSelector)"Segoe UI",
        //                BodySize = (PDFUnit)12
        //            }
        //        };

        //        doc.Params["dynamic"] = binding;
        //        doc.InitializeAndLoad();
        //        doc.DataBind();

        //        //For the ForEach template with an object source.
        //        var first = doc.FindAComponentById("FirstID") as Label;
        //        Assert.IsNotNull(first, "Could not find the first label");
        //        Assert.AreEqual("First", first.Text, "The first label does not have the correct Name value");

        //        var second = doc.FindAComponentById("SecondID") as Label;
        //        Assert.IsNotNull(second, "Could not find the second label");
        //        Assert.AreEqual("Second", second.Text, "The second label does not have the correct Name value");

        //        var style = doc.Styles[0] as Scryber.Styles.Style;
        //        Assert.IsTrue(style.IsValueDefined(Scryber.Styles.StyleKeys.BgColorKey), "The background color is not assigned");
        //        Assert.AreEqual(binding.Theme.TitleBg, style.Background.Color, "The background colors do not match");
        //        Assert.AreEqual(binding.Theme.TitleColor, style.Fill.Color, "The foreground colours do not match");

        //        style = doc.Styles[1] as Scryber.Styles.Style;
        //        Assert.AreEqual(binding.Theme.BodyFont, style.Font.FontFamily, "Body fonts do not match");
        //        Assert.AreEqual(binding.Theme.BodySize, style.Font.FontSize, "Body font sizes do not match");

        //    }


        }

    }
}
