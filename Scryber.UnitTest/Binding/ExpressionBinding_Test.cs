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
using System.Text;
using Scryber.Expressive.Functions;
using Scryber.Expressive.Operators;
using System.Threading.Tasks;
using System.Xml;
using Scryber.Html;
using Scryber.PDF.Layout;
using System.IO;

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
            var options = ExpressiveOptions.IgnoreCaseForParsing;
            var set = new FunctionSet(options).AddDefaultFunctions();
            var opSet = new OperatorSet(options).AddDefaultOperators();
            
            var context = new Context(options, set, opSet);

            var str = "10 + 2";
            var expression = new Expression(str, context);
            var value = expression.Evaluate<double>();

            Assert.AreEqual(12.0, value);

            str = "10 / 2";

            expression = new Expression(str, context);
            value = expression.Evaluate<double>();

            Assert.AreEqual(5.0, value);

            str = "10 + (2 /4)";

            expression = new Expression(str, context);
            value = expression.Evaluate<double>();

            Assert.AreEqual(10.5, value);
        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindUnqualifiedVariableExpression()
        {
            var options = ExpressiveOptions.IgnoreCaseForParsing;
            var fSet = new FunctionSet(options).AddDefaultFunctions();
            var opSet = new OperatorSet(options).AddDefaultOperators();

            var context = new Context(options, fSet, opSet);


            Dictionary<string, object> vars = new Dictionary<string, object>();
            vars.Add("val1", 2);
            vars.Add("val2", 4);

            var str = "10 + val1";
            var expression = new Expression(str, context);
            var value = expression.Evaluate<double>(vars);

            Assert.AreEqual(12.0, value);

            str = "10 / val1";

            expression = new Expression(str, context);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(5.0, value);

            str = "10 + (val1 / val2)";

            expression = new Expression(str, context);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(10.5, value);
        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindDeepVariableExpression()
        {
            var options = ExpressiveOptions.IgnoreCaseForParsing;
            var fSet = new FunctionSet(options).AddDefaultFunctions();
            var opSet = new OperatorSet(options).AddDefaultOperators();

            var context = new Context(options, fSet, opSet);

            Dictionary<string, object> vars = new Dictionary<string, object>();
            vars.Add("val1", new { num = 2 });
            vars.Add("val2", 4);

            var str = "val1.num";
            var expression = new Expression(str, context);
            var value = expression.Evaluate<double>(vars);

            Assert.AreEqual(2.0, value);

            str = "10 + val1.num";
            expression = new Expression(str, context);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(12.0, value);

            str = "10 / val1.num";

            expression = new Expression(str, context);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(5.0, value);

            str = "10 + (val1.num / val2)";

            expression = new Expression(str, context);
            
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(10.5, value);
        }

        



        [TestMethod()]
        [TestCategory("Binding")]
        public void BindCalcExpressions()
        {
            var factory = new BindingCalcExpressionFactory();

            
            Dictionary<string, object> vars = new Dictionary<string, object>();
            vars.Add("val1", new { num = 2, text = "a variable" });
            vars.Add("val2", 4);
            vars.Add("array1", new int[] { 1, 2, 3, 4 });


            var str = "Max(10, val1.num)";
            var expression = factory.CreateBindingExpression(str, null);
            
            var value = expression.Expression.Evaluate<double>(vars);

            Assert.AreEqual(10.0, value);

            str = "Min(10,val1.num,val2)";

            expression = factory.CreateBindingExpression(str, null);

            value = expression.Expression.Evaluate<double>(vars);

            Assert.AreEqual(2.0, value);

            str = "median(10,val2, val1.num)";

            expression = factory.CreateBindingExpression(str, null);
            
            value = expression.Expression.Evaluate<double>(vars);

            Assert.AreEqual(4, value);


            str = "concat('a string and ', val1.text)";

            expression = factory.CreateBindingExpression(str, null);

            var strResult = expression.Expression.Evaluate<string>(vars);

            Assert.AreEqual("a string and a variable", strResult);


        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindFunctionNoParametersExpression()
        {
            var factory = new BindingCalcExpressionFactory();


            var str = "Pi";
            var expression = factory.CreateBindingExpression(str, null);

            var value = expression.Expression.Evaluate<double>();

            Assert.AreEqual(Math.Round(Math.PI, 10), value);

            str = "round(Pi, 3)";
            expression = factory.CreateBindingExpression(str, null);

            value = expression.Expression.Evaluate<double>();

            Assert.AreEqual(Math.Round(Math.PI, 3), value);
        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindWithFunctionExpression()
        {
            var options = ExpressiveOptions.IgnoreCaseForParsing;
            var fSet = new FunctionSet(options).AddDefaultFunctions();
            var opSet = new OperatorSet(options).AddDefaultOperators();

            var context = new Context(options, fSet, opSet);

            Dictionary<string, object> vars = new Dictionary<string, object>();
            vars.Add("val1", new { num = 2, text = "a variable" });
            vars.Add("val2", 4);
            vars.Add("array1", new int[] { 1, 2, 3, 4 });

            var str = "Max(10, val1.num)";
            var expression = new Expression(str, context);
            var value = expression.Evaluate<double>(vars);

            Assert.AreEqual(10.0, value, str + " did not evaluate correctly");

            str = "Min(10,val1.num,val2)";
            expression = new Expression(str, context);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(2.0, value, str + " did not evaluate correctly");


            str = "median(10,val2, val1.num)";
            expression = new Expression(str, context);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(4, value, str + " did not evaluate correctly");


            str = "concat('a string and ', val1.text)";
            expression = new Expressive.Expression(str, context);
            var strResult = expression.Evaluate<string>(vars);

            Assert.AreEqual("a string and a variable", strResult, str + " did not evaluate correctly");


            str = "array1[0] + array1[1]";
            expression = new Expressive.Expression(str, context);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(3.0, value, str + " did not evaluate correctly");

            str = "array1[0] + array1[val1.num + 1]"; //entry 0 and entry 2+1
            expression = new Expressive.Expression(str, context);
            value = expression.Evaluate<double>(vars);

            Assert.AreEqual(5.0, value, str + " did not evaluate correctly");
        }

        

        private class FunctionTest
        {
            public string grp { get; set; }
            public string name { get; set; }
            public string function { get; set; }
            public string result { get; set; }
        }

        [TestMethod]
        public void AllExpressions_Test()
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


            var functions = new FunctionTest[]
            {

                new FunctionTest() {grp = "Binary Operators", name = "Plus (+)", function = "12 + 4", result = "16"},
                new FunctionTest() {grp = "Binary Operators", name = "Minus (-)", function = "12 - 4", result = "8"},
                new FunctionTest() {grp = "Binary Operators", name = "Multiply (*)", function = "12 * 4", result = "48"},
                new FunctionTest() {grp = "Binary Operators", name = "Divide (/)", function = "12 / 4", result = "3"},
                new FunctionTest() {grp = "Binary Operators", name = "All (+-* /)", function = "12 + 2 - 6 * 2 / 4", result = "11"},
                new FunctionTest() {grp = "Binary Operators", name = "All (+-* /)", function = "((12 + 2) - 6) * (2 / 4)", result = "4"},
                new FunctionTest() {grp = "Binary Operators", name = "Modulo (%) ", function = "11 % 2", result = "1"},

                new FunctionTest() {grp = "Binary Operators", name = "Bitwise And (&) ", function = "11 & 7", result = "3"},
                new FunctionTest() {grp = "Binary Operators", name = "Bitwise Or (|) ", function = "11 | 7", result = "15"},
                new FunctionTest() {grp = "Binary Operators", name = "Bitwise Left (<<) ", function = "11 << 2", result = "44"},
                new FunctionTest() {grp = "Binary Operators", name = "Bitwise Right (>>) ", function = "11 >> 2", result = "2"},

                new FunctionTest() {grp = "Binary Operators", name = "Null Coalesce ", function = "model.number ?? 4", result = "20.7"},
                new FunctionTest() {grp = "Binary Operators", name = "Null Coalesce ", function = "model.notset ?? 4", result = "4"},
                new FunctionTest() {grp = "Binary Operators", name = "Null Coalesce ", function = "notset ?? 'empty'", result = "empty"},

                new FunctionTest() {grp = "Relational Operators", name = "Equals", function = "model.number == 20.7", result = "True"},
                new FunctionTest() {grp = "Relational Operators", name = "Equals", function = "model.number == 10.7", result = "False"},
                new FunctionTest() {grp = "Relational Operators", name = "Equals", function = "if(model.number == 20.7,'Equal','Not Equal')", result = "Equal"},
                new FunctionTest() {grp = "Relational Operators", name = "Equals", function = "if(model.number == 10.7,'Equal','Not Equal')", result = "Not Equal"},
                new FunctionTest() {grp = "Relational Operators", name = "Equals Null", function = "if(model.number == null,'Is Null','Not Null')", result = "Not Null"},
                new FunctionTest() {grp = "Relational Operators", name = "Equals Null", function = "if(model.notset == null,'Is Null','Not Null')", result = "Is Null"},

                new FunctionTest() {grp = "Relational Operators", name = "Not Equal", function = "model.number != 20.7", result = "False"},
                new FunctionTest() {grp = "Relational Operators", name = "Not Equal", function = "model.number != 10.7", result = "True"},
                new FunctionTest() {grp = "Relational Operators", name = "Not Equal", function = "model.boolean != True", result = "False"},
                new FunctionTest() {grp = "Relational Operators", name = "Not Equal", function = "model.boolean != False", result = "True"},
                new FunctionTest() {grp = "Relational Operators", name = "Not Equal", function = "if(model.number != 20.7,'Equal','Not Equal')", result = "Not Equal"},
                new FunctionTest() {grp = "Relational Operators", name = "Not Equal", function = "if(model.number != 10.7,'Equal','Not Equal')", result = "Equal"},
                new FunctionTest() {grp = "Relational Operators", name = "Not Equal Null", function = "if(model.number != null,'Not Null','Is Null')", result = "Not Null"},
                new FunctionTest() {grp = "Relational Operators", name = "Not Equal Null", function = "if(model.notset != null,'Not Null','Is Null')", result = "Is Null"},

                new FunctionTest() {grp = "Relational Operators", name = "Not Equal", function = "model.number <> 20.7", result = "False"},
                new FunctionTest() {grp = "Relational Operators", name = "Not Equal", function = "model.number <> 10.7", result = "True"},
                new FunctionTest() {grp = "Relational Operators", name = "Not Equal", function = "if(model.number <> 20.7,'Equal','Not Equal')", result = "Not Equal"},
                new FunctionTest() {grp = "Relational Operators", name = "Not Equal", function = "if(model.number <> 10.7,'Equal','Not Equal')", result = "Equal"},

                new FunctionTest() {grp = "Relational Operators", name = "Greater than", function = "model.number > 21", result = "False"},
                new FunctionTest() {grp = "Relational Operators", name = "Greater than", function = "model.number > 10", result = "True"},
                new FunctionTest() {grp = "Relational Operators", name = "Greater than", function = "if(model.number > 21,'Greater','Less')", result = "Less"},
                new FunctionTest() {grp = "Relational Operators", name = "Greater than", function = "if(model.number > 10,'Greater','Less')", result = "Greater"},

                new FunctionTest() {grp = "Relational Operators", name = "Less than", function = "model.number < 21", result = "True"},
                new FunctionTest() {grp = "Relational Operators", name = "Less than", function = "model.number < 10", result = "False"},
                new FunctionTest() {grp = "Relational Operators", name = "Less than", function = "if(model.number < 21,'Less','Greater')", result = "Less"},
                new FunctionTest() {grp = "Relational Operators", name = "Less than", function = "if(model.number < 10,'Less','Greater')", result = "Greater"},

                new FunctionTest() {grp = "Relational Operators", name = "Greater than or Equal", function = "model.number >= 21", result = "False"},
                new FunctionTest() {grp = "Relational Operators", name = "Greater than or Equal", function = "model.number >= 20.7", result = "True"},
                new FunctionTest() {grp = "Relational Operators", name = "Greater than or Equal", function = "model.number >= 10", result = "True"},
                new FunctionTest() {grp = "Relational Operators", name = "Greater than or Equal", function = "if(model.number >= 21,'Greater or equal','Less')", result = "Less"},
                new FunctionTest() {grp = "Relational Operators", name = "Greater than or Equal", function = "if(model.number >= 20.7,'Greater or equal','Less')", result = "Greater or equal"},
                new FunctionTest() {grp = "Relational Operators", name = "Greater than or Equal", function = "if(model.number >= 10,'Greater or equal','Less')", result = "Greater or equal"},

                new FunctionTest() {grp = "Relational Operators", name = "Less than or Equal", function = "model.number <= 21", result = "True"},
                new FunctionTest() {grp = "Relational Operators", name = "Less than or Equal", function = "model.number <= 20.7", result = "True"},
                new FunctionTest() {grp = "Relational Operators", name = "Less than or Equal", function = "model.number <= 10", result = "False"},
                new FunctionTest() {grp = "Relational Operators", name = "Less than or Equal", function = "if(model.number <= 21,'Less or equal','Greater')", result = "Less or equal"},
                new FunctionTest() {grp = "Relational Operators", name = "Less than or Equal", function = "if(model.number <= 20.7,'Less or equal','Greater')", result = "Less or equal"},
                new FunctionTest() {grp = "Relational Operators", name = "Less than or Equal", function = "if(model.number <= 10,'Less or equal','Greater')", result = "Greater"},

                new FunctionTest() {grp = "Logical Operators", name = "And (&amp;&amp;)", function = "if(model.number < 21 && model.number > 20,'Between','Outside')", result = "Between"},

                new FunctionTest() {grp = "Logical Operators", name = "Or (||)", function = "if(model.number < 20 || model.number > 21,'Outside','Between')", result = "Between"},

                new FunctionTest() {grp = "Logical Operators", name = "Not (!)", function = "if(!(model.number < 21 && model.number > 20),'Outside','Between')", result = "Between"},

                new FunctionTest() {grp = "Conversion Functions", name = "Date", function = "date('30 June 2021 11:00:00')", result = DateTime.Parse("30 June 2021 11:00:00 AM").ToString()},
                new FunctionTest() {grp = "Conversion Functions", name = "Decimal", function = "decimal(20 + model.number)", result = (20 + model.number).ToString()},
                new FunctionTest() {grp = "Conversion Functions", name = "Double", function = "double(20 + model.number)", result = (20 + model.number).ToString()},
                new FunctionTest() {grp = "Conversion Functions", name = "Integer", function = "integer(20 + model.number)", result = Convert.ToInt32(20 + model.number).ToString()},
                new FunctionTest() {grp = "Conversion Functions", name = "Long", function = "long(20 + model.number)", result = Convert.ToInt64(20 + model.number).ToString()},
                new FunctionTest() {grp = "Conversion Functions", name = "String", function = "string(20 + model.number)", result = (20 + model.number).ToString()},
                new FunctionTest() {grp = "Conversion Functions", name = "Boolean", function = "boolean(0)", result = "False"},
                new FunctionTest() {grp = "Conversion Functions", name = "Boolean", function = "boolean('')", result = "False"},
                new FunctionTest() {grp = "Conversion Functions", name = "Boolean", function = "boolean('val')", result = "True"},
                new FunctionTest() {grp = "Conversion Functions", name = "Boolean", function = "boolean(1)", result = "True"},
                new FunctionTest() {grp = "Conversion Functions", name = "Boolean", function = "boolean(0)", result = "False"},
                new FunctionTest() {grp = "Conversion Functions", name = "Boolean", function = "boolean(0.0)", result = "False"},
                new FunctionTest() {grp = "Conversion Functions", name = "Boolean", function = "boolean(date())", result = "True"},

                new FunctionTest() {grp = "Date Add Functions", name = "AddDays", function = "adddays(date('30 June 2021 11:00:00'),10)", result = DateTime.Parse("10 July 2021 11:00:00").ToString()},
                new FunctionTest() {grp = "Date Add Functions", name = "AddHours", function = "addhours(date('30 June 2021 11:00:00'),10)", result = DateTime.Parse("30 June 2021 21:00:00").ToString()},
                new FunctionTest() {grp = "Date Add Functions", name = "AddMilliSeconds", function = "addmilliseconds(date('30 June 2021 11:00:00'),2000)", result = DateTime.Parse("30 June 2021 11:00:02").ToString()},
                new FunctionTest() {grp = "Date Add Functions", name = "AddMinutes", function = "addMinutes(date('30 June 2021 11:00:00'),40)", result = DateTime.Parse("30 June 2021 11:40:00").ToString()},
                new FunctionTest() {grp = "Date Add Functions", name = "AddMonths", function = "addMonths(date('30 June 2021 11:00:00'),2)", result = DateTime.Parse("30 August 2021 11:00:00").ToString()},
                new FunctionTest() {grp = "Date Add Functions", name = "AddSeconds", function = "addSeconds(date('30 June 2021 11:00:00'),100)", result = DateTime.Parse("30 June 2021 11:01:40").ToString()},
                new FunctionTest() {grp = "Date Add Functions", name = "AddYears", function = "addYears(date('30 June 2021 11:00:00'),1000)", result = DateTime.Parse("30 June 3021 11:00:00").ToString()},

                new FunctionTest() {grp = "Date Of Functions", name = "DayOf", function = "dayof(date('30 June 2021 11:40:10.345'))", result = "30"},
                new FunctionTest() {grp = "Date Of Functions", name = "HourOf", function = "HourOf(date('30 June 2021 11:40:10.345'))", result = "11"},
                new FunctionTest() {grp = "Date Of Functions", name = "MillisecondOf", function = "MillisecondOf(date('30 June 2021 11:40:10.345'))", result = "345"},
                new FunctionTest() {grp = "Date Of Functions", name = "MinuteOf", function = "MinuteOf(date('30 June 2021 11:40:10.345'))", result = "40"},
                new FunctionTest() {grp = "Date Of Functions", name = "MonthOf", function = "MonthOf(date('30 June 2021 11:40:10.345'))", result = "6"},
                new FunctionTest() {grp = "Date Of Functions", name = "SecondOf", function = "SecondOf(date('30 June 2021 11:40:10.345'))", result = "10"},
                new FunctionTest() {grp = "Date Of Functions", name = "YearOf", function = "YearOf(date('30 June 2021 11:40:10.345'))", result = "2021"},

                new FunctionTest() {grp = "Date Between Functions", name = "DaysBetween", function = "DaysBetween(date('11 June 2021 11:40:10.345'),#30 June 2021 11:40:10.345#)", result = "19"},
                new FunctionTest() {grp = "Date Between Functions", name = "HoursBetween", function = "HoursBetween(date('30 June 2021 01:40:10.345'),#30 June 2021 11:40:10.345#)", result = "10"},
                new FunctionTest() {grp = "Date Between Functions", name = "MillisecondsBetween", function = "MillisecondsBetween(date('30 June 2021 11:40:10.100'),#30 June 2021 11:40:10.345#)", result = "245"},
                new FunctionTest() {grp = "Date Between Functions", name = "MinutesBetween", function = "MinutesBetween(date('30 June 2021 11:40:10.345'),#30 June 2021 11:13:10.345#)", result = "-27"},
                new FunctionTest() {grp = "Date Between Functions", name = "SecondsBetween", function = "SecondsBetween(date('30 June 2021 11:40:10.345'),#30 June 2021 11:41:56.345#)", result = "106"},

                new FunctionTest() {grp = "Logical Functions", name = "If", function = "If(model.boolean == true,24,'none')", result = "24"},
                new FunctionTest() {grp = "Logical Functions", name = "If", function = "If(!model.boolean,24,'none')", result = "none"},
                new FunctionTest() {grp = "Logical Functions", name = "In", function = "In(12.2,10.0,11.1,12.2)", result = "True"},
                new FunctionTest() {grp = "Logical Functions", name = "In", function = "In(12,10.0,11.1,12.2)", result = "False"},
                new FunctionTest() {grp = "Logical Functions", name = "In", function = "In(12,model.array)", result = "True"},

                new FunctionTest() {grp = "Mathematical Functions", name = "Abs", function = "abs(-12) + abs(model.number)", result = "32.7"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Truncate", function = "truncate(Pi())", result = "3"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Truncate", function = "truncate(9.99999)", result = "9"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Round", function = "round(10.024)", result = Math.Round(10.024).ToString()},
                new FunctionTest() {grp = "Mathematical Functions", name = "Round", function = "round(Log10(30),3)", result = Math.Round(Math.Log10(30),3).ToString("#0.000")},
                new FunctionTest() {grp = "Mathematical Functions", name = "Sign", function = "sign(30)", result = "1"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Sign", function = "sign(-300)", result = "-1"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Sign", function = "sign(0)", result = "0"},
                new FunctionTest() {grp = "Mathematical Functions", name = "ACos", function = "round(ACos(-0.5),3)", result = "2.094"},
                new FunctionTest() {grp = "Mathematical Functions", name = "ASin", function = "round(ASin(-0.5),3)", result = "-0.524"},
                new FunctionTest() {grp = "Mathematical Functions", name = "ATan", function = "round(ATan(2.0),3)", result = "1.107"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Ceiling", function = "Ceiling(3.2)", result = "4"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Floor", function = "Floor(3.2)", result = "3"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Cos", function = "round(Cos(2.094),1)", result = "-0.5"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Sin", function = "round(Sin(-0.524),1)", result = "-0.5"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Tan", function = "round(Tan(1.107),1)", result = "2"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Deg", function = "round(deg(Pi),4)", result = "180"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Rad", function = "round(rad(180),3)", result = "3.142"},
                new FunctionTest() {grp = "Mathematical Functions", name = "E", function = "round(E(),3)", result = "2.718"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Pi", function = "round(Pi,3)", result = "3.142"},
                new FunctionTest() {grp = "Mathematical Functions", name = "Exp", function = "round(exp(3),3)", result = Math.Exp(3).ToString("#0.000")},
                new FunctionTest() {grp = "Mathematical Functions", name = "Log10", function = "round(Log10(30),3)", result = Math.Log10(30).ToString("#0.000")},
                new FunctionTest() {grp = "Mathematical Functions", name = "Log (base 5)", function = "round(Log(30,5),3)", result = Math.Log(30,5).ToString("#0.000")},
                new FunctionTest() {grp = "Mathematical Functions", name = "Pow", function = "pow(3,2)", result = Math.Pow(3,2).ToString()},
                new FunctionTest() {grp = "Mathematical Functions", name = "Random (between 2 and 30)", function = "random(2,30)", result = (string)null},
                new FunctionTest() {grp = "Mathematical Functions", name = "Random (between 2.4d and 2.6d)", function = "random(2.4, 2.6)", result = (string)null},


                new FunctionTest() {grp = "Aggregate and Statistical Functions", name = "Count", function = "count(12, 13, 14)", result = "3"},
                new FunctionTest() {grp = "Aggregate and Statistical Functions", name = "Count (with array)", function = "count(model.array)", result = "3"},
                new FunctionTest() {grp = "Aggregate and Statistical Functions", name = "Count (with array)", function = "count(12, model.array, model.items, 14)", result = "8"},
                new FunctionTest() {grp = "Aggregate and Statistical Functions", name = "Sum", function = "sum(model.array,13,14)", result = (13 + 14 + 10 + 11 + 12 ).ToString()},
                new FunctionTest() {grp = "Aggregate and Statistical Functions", name = "Max", function = "Max(model.array,13,14)", result = "14"},
                new FunctionTest() {grp = "Aggregate and Statistical Functions", name = "Min", function = "Min(model.array,13,14)", result = "10"},
                new FunctionTest() {grp = "Aggregate and Statistical Functions", name = "Average", function = "Average(model.array,13,1)", result = "9.4"},
                new FunctionTest() {grp = "Aggregate and Statistical Functions", name = "Mean", function = "Mean(model.array,13,1)", result = "9.4"},
                new FunctionTest() {grp = "Aggregate and Statistical Functions", name = "Mode", function = "Mode(model.array,13,1)", result = "10"},
                new FunctionTest() {grp = "Aggregate and Statistical Functions", name = "Median", function = "Median(model.array,13,1)", result = "11"},


                new FunctionTest() {grp = "String Functions", name = "Concat", function = "concat('A string',' and another string')", result = "A string and another string"},
                new FunctionTest() {grp = "String Functions", name = "Concat (with types)", function = "concat('numbers:',9, model.array)", result = "numbers:9101112"},
                new FunctionTest() {grp = "String Functions", name = "Join", function = "join('-', 9, 12, 13)", result = "9-12-13"},
                new FunctionTest() {grp = "String Functions", name = "Join", function = "join('-', model.array)", result = "10-11-12"},
                new FunctionTest() {grp = "String Functions", name = "Join (with types)", function = "join(', ', 9, model.array, 13)", result = "9, 10, 11, 12, 13"},
                new FunctionTest() {grp = "String Functions", name = "Contains", function = "contains('a string','str')", result = "True"},
                new FunctionTest() {grp = "String Functions", name = "Contains", function = "contains('a string','l')", result = "False"},
                new FunctionTest() {grp = "String Functions", name = "StartsWith", function = "startsWith('a string','a ')", result = "True"},
                new FunctionTest() {grp = "String Functions", name = "StartsWith", function = "startsWith('a string','s')", result = "False"},
                new FunctionTest() {grp = "String Functions", name = "EndsWith", function = "endsWith('a string','ing')", result = "True"},
                new FunctionTest() {grp = "String Functions", name = "EndsWith", function = "endsWith('a string','int')", result = "False"},
                new FunctionTest() {grp = "String Functions", name = "EndsWith (+ If)", function = "if(endsWith('a string','ing'),'Yes','No')", result = "Yes"},
                new FunctionTest() {grp = "String Functions", name = "IndexOf", function = "indexOf('a string','ing')", result = "5"},
                new FunctionTest() {grp = "String Functions", name = "IndexOf", function = "indexOf('a string','int')", result = "-1"},
                new FunctionTest() {grp = "String Functions", name = "Length", function = "length('a string')", result = "8"},
                new FunctionTest() {grp = "String Functions", name = "Length", function = "length(model.items[0].name)", result = "First Item".Length.ToString()},
                new FunctionTest() {grp = "String Functions", name = "PadLeft", function = "padleft(model.items[0].name, 20, '+')", result = "++++++++++First Item"},
                new FunctionTest() {grp = "String Functions", name = "PadRight", function = "padright(model.items[0].name, 20, '-')", result = "First Item----------"},
                new FunctionTest() {grp = "String Functions", name = "PatLeft+PadRight", function = "padleft(padright(model.items[0].name, 15, '-'),20,'+++++')", result = "+++++First Item-----"},
                new FunctionTest() {grp = "String Functions", name = "SubString", function = "substring(model.items[0].name, 6)", result = "Item"},
                new FunctionTest() {grp = "String Functions", name = "SubString", function = "substring(model.items[0].name, 6, 2)", result = "It"},
                new FunctionTest() {grp = "String Functions", name = "Regex", function = "regex('The quick brown fox','\\squick\\s')", result = "True"},
                new FunctionTest() {grp = "String Functions", name = "Regex", function = "regex('The quick brown fox','\\sjump\\s')", result = "False"},

            };



            var options = ExpressiveOptions.IgnoreCaseForParsing;
            var fset = new FunctionSet(options).AddDefaultFunctions();
            var opSet = new OperatorSet(options).AddDefaultOperators();

            var context = new Context(options, fset, opSet);

            Dictionary<string, object> vars = new Dictionary<string, object>(context.ParsingStringComparer)
            {
                {"model", model }
            };

            foreach (var item in functions)
            {
                var value = item.function;

                
                Expression expr = new Expression(value, context);
                object result = expr.Evaluate(vars);

                if (item.result != null && result.ToString() != item.result)
                    throw new InvalidOperationException("result of " + item.function + " does not match " + item.result);


            }

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

                var p = doc.FindAComponentById("myPara") as IStyledComponent;
                Assert.AreEqual((Color)"#FF0033", p.Style.Fill.Color, "Paragraph color was not correct");
            }
        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindCalcExpressionCached()
        {
            var src = @"<!DOCTYPE html>
<?scryber parser-mode='strict' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Binding {{model.title}}</title>
    </head>

    <body id='mainbody' class='strong' >
        <p id='myPara' style='border: solid 1px blue; padding: 5px;color:{{model.color}}' >This is a paragraph of content</p>
        <ul id='mylist'>
            <template data-bind='{{model.items}}' >
                <li>{{concat('Item ',.name)}}</li>
            </template>
        </ul>
    </body>

</html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.Params["model"] = new
                {
                    title = "Document 1",
                    color = "#FF0033",
                    items = new[] {
                        new { name = "first"},
                        new { name = "second"}
                    }
                };

                using (var stream = DocStreams.GetOutputStream("BindCalculationCached1.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
                Assert.AreEqual("Binding Document 1", doc.Info.Title, "Title is not correct");

                var p = doc.FindAComponentById("myPara") as IStyledComponent;
                Assert.AreEqual((Color)"#FF0033", p.Style.Fill.Color, "Paragraph color was not correct");

                var list = doc.FindAComponentById("mylist") as ListUnordered;
                Assert.AreEqual(2, list.Items.Count, "List counts were not correct");
            }


            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.Params["model"] = new
                {
                    title = "Document 2",
                    color = "#FF00FF",
                    items = new[] {
                        new { name = "first"},
                        new { name = "second"},
                        new { name = "third"},
                        new { name = "fourth"},
                        new { name = "fifth"}
                    }
                };

                using (var stream = DocStreams.GetOutputStream("BindCalculationCached2.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
                Assert.AreEqual("Binding Document 2", doc.Info.Title, "Title is not correct");

                var p = doc.FindAComponentById("myPara") as IStyledComponent;
                Assert.AreEqual((Color)"#FF00FF", p.Style.Fill.Color, "Paragraph color was not correct");

                var list = doc.FindAComponentById("mylist") as ListUnordered;
                Assert.AreEqual(5, list.Items.Count, "List counts were not correct");
            }


        }

        [TestMethod]
        [TestCategory("Binding")]
        public void BindHtmlWithDoubleAmpersand()
        {
            var withEscapes = @"<html>
  <body>
    <span>A simple string (&amp;&amp;)</span>
  </body>
</html>";

            using var reader = new StringReader(withEscapes);
            var doc = Document.ParseHtmlDocument(reader, ParseSourceType.DynamicContent);
            doc.AppendTraceLog = true;
            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.Params["model"] = new
            {
                number = 20.9
            };

            using (var stream = DocStreams.GetOutputStream("BindHtmlWithDoubleAmpersand.pdf"))
            {
                doc.SaveAsPDF(stream);
            }
        }

        [TestMethod]
        [TestCategory("Binding")]
        public void BindHtmlWithEscapes()
        {
            var withEscapes = @"<html>
  <body>
    <table class='expressions escaped'>
      <tr>
        <td>A simple string (&amp;&amp;)</td>
        <td>A string with an inner {{model.number}} binding</td>
      </tr>
      <tr>
        <td>Wrapped in escapes \{{ and after }}\ are ignored</td>
        <td>Wrapped in span <span>{{</span> is ignored but not {{model.number}}</td>
      </tr>
      <tr>
        <td>\{{model.number + 1}}\ ignored start</td>
        <td>\{{model.number + 1}} binding start</td>
      </tr>
      <tr>
        <td>Ignored end \{{model.number + 1}}\</td>
        <td>Bound end {{model.number + 1}}\</td>
      </tr>
      <tr>
        <td>Bound with space \ {{model.number + 1}} \</td>
        <td>Bound with space  {{model.number + 1}} \</td>
      </tr>
      <tr>
        <td>Ignored middle \{{concat('&lt;', model.number + 1, '&gt;')}}\ but not {{'\'' + string(model.number + 1) + '\''}} after</td>
        <td>Bound middle {{model.number + 1}} but ignored end \{{model.number+2}}\</td>
      </tr>
    </table>
  </body>
</html>";

            using var reader = new StringReader(withEscapes);
            var doc = Document.ParseHtmlDocument(reader, ParseSourceType.DynamicContent);
            doc.AppendTraceLog = true;
            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.Params["model"] = new
            {
                number = 20.9
            };

            using (var stream = DocStreams.GetOutputStream("BindHtmlWithEscapes.pdf"))
            {
                doc.SaveAsPDF(stream);
            }
        }

        [TestMethod]
        [TestCategory("Binding")]
        public void BindHtmlWithQuotesAndEncoding()
        {
            var withQuotes = @"<html>
  <body>
    <table class='expressions relational'>
      <tr>
        <td>if(model.number == 20.9 , ""Equal"", ""Not equal"")</td>
        <td class='result'>{{if(model.number == 20.9 , ""Equal"", ""Not equal"")}}</td>
      </tr>
      <tr>
        <td>Before &gt;(if(model.number == 20.9 , ""'Equal'"", ""Not 'equal'"")&lt; After</td>
        <td class='result'>Before &gt;{{if(model.number == 20.9 , ""'Equal'"", ""Not 'equal'"")}}&lt; After</td>
      </tr>
      <tr>
        <td>Before &gt; (if(model.number == 20.0 , ""'Equal'"", ""Not &apos;equal&apos;"") &lt; After</td>
        <td class='result'>Before &gt; {{if(model.number == 20.0 , ""'Equal'"", ""Not &apos;equal&apos;"")}} &lt; After</td>
      </tr>
      <tr>
        <td>20.9 &lt; 20.9</td>
        <td class='result'>{{20.9 &lt; 20.9}}</td>
      </tr>
      <tr>
        <td>20.9 > 20.9</td>
        <td class='result'>{{20.9 > 20.9}}</td>
      </tr>
    </table>
  </body>
</html>";

            using var reader = new StringReader(withQuotes);
            var doc = Document.ParseHtmlDocument(reader, ParseSourceType.DynamicContent);

            doc.Params["model"] = new {
                number = 20.9
            };

            using (var stream = DocStreams.GetOutputStream("BindHtmlWithQuotes.pdf"))
            {
                doc.SaveAsPDF(stream);
            }
            var pg = doc.Pages[0] as Page;
            Assert.IsNotNull(pg);

            var tbl = pg.Contents[0] as TableGrid;
            Assert.IsNotNull(tbl) ;

            TableCell cell;
            TextLiteral lit;

            //Row 0 cell 0

            cell = tbl.Rows[0].Cells[0];
            lit = cell.Contents[0] as TextLiteral;
            Assert.AreEqual("if(model.number == 20.9 , &quot;Equal&quot;, &quot;Not equal&quot;)", lit.Text);
            Assert.AreEqual(TextFormat.XML, lit.ReaderFormat);

            //Row 0 cell 1
            cell = tbl.Rows[0].Cells[1];
            lit = cell.Contents[0] as TextLiteral;
            Assert.IsNotNull(lit);

            Assert.AreEqual("Equal", lit.Text);
            Assert.AreEqual(TextFormat.XML, lit.ReaderFormat);

            //Row 1 cell 0

            cell = tbl.Rows[1].Cells[0] as TableCell;
            lit = cell.Contents[0] as TextLiteral;

            Assert.AreEqual("Before &gt;(if(model.number == 20.9 , &quot;&apos;Equal&apos;&quot;, &quot;Not &apos;equal&apos;&quot;)&lt; After", lit.Text);
            Assert.AreEqual(TextFormat.XML, lit.ReaderFormat);

            //Row 1 cell 1

            cell = tbl.Rows[1].Cells[1] as TableCell;

            //3 literals together Before > and 'Equal' and < After
            lit = cell.Contents[0] as TextLiteral;
            Assert.AreEqual(3, cell.Contents.Count);

            Assert.AreEqual(@"Before &gt;", lit.Text);
            Assert.AreEqual(TextFormat.XML, lit.ReaderFormat);

            lit = cell.Contents[1] as TextLiteral;

            Assert.AreEqual("'Equal'", lit.Text);
            Assert.AreEqual(TextFormat.XML, lit.ReaderFormat);

            lit = cell.Contents[2] as TextLiteral;

            Assert.AreEqual("&lt; After", lit.Text);
            Assert.AreEqual(TextFormat.XML, lit.ReaderFormat);

            //Row 2 cell 0

            cell = tbl.Rows[2].Cells[0] as TableCell;
            lit = cell.Contents[0] as TextLiteral;

            Assert.AreEqual("Before &gt; (if(model.number == 20.0 , &quot;&apos;Equal&apos;&quot;, &quot;Not &apos;equal&apos;&quot;) &lt; After", lit.Text);
            Assert.AreEqual(TextFormat.XML, lit.ReaderFormat);

            //Row 2 cell 1

            cell = tbl.Rows[2].Cells[1] as TableCell;

            //3 literals together Before > and 'Equal' and < After
            lit = cell.Contents[0] as TextLiteral;
            Assert.AreEqual(3, cell.Contents.Count);

            Assert.AreEqual(@"Before &gt; ", lit.Text);
            Assert.AreEqual(TextFormat.XML, lit.ReaderFormat);

            lit = cell.Contents[1] as TextLiteral;

            Assert.AreEqual("Not 'equal'", lit.Text);
            Assert.AreEqual(TextFormat.XML, lit.ReaderFormat);

            lit = cell.Contents[2] as TextLiteral;

            Assert.AreEqual(" &lt; After", lit.Text);
            Assert.AreEqual(TextFormat.XML, lit.ReaderFormat);

            //Row 3 cell 0

            cell = tbl.Rows[3].Cells[0] as TableCell;
            lit = cell.Contents[0] as TextLiteral;

            Assert.AreEqual("20.9 &lt; 20.9", lit.Text);
            Assert.AreEqual(TextFormat.XML, lit.ReaderFormat);

            //Row 3 cell 1

            cell = tbl.Rows[3].Cells[1] as TableCell;
            lit = cell.Contents[0] as TextLiteral;

            Assert.AreEqual("False", lit.Text);
            Assert.AreEqual(TextFormat.XML, lit.ReaderFormat);

        }


        string taskSource = @"<!DOCTYPE html>
<?scryber parser-mode='strict' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Binding {{model.title}}</title>
    </head>

    <body id='mainbody' class='strong' >
        <p id='myPara' style='border: solid 1px blue; padding: 5px;color:{{model.color}}' >This is a paragraph of content</p>
        <ul id='mylist'>
            <template data-bind='{{model.items}}' >
                <li>{{concat('List ',.name)}}</li>
            </template>
        </ul>
    </body>

</html>";

        public Document GenerateAsync(object model)
        {
            dynamic content = (dynamic)model;
            int index = content.index;

            using (var sr = new System.IO.StringReader(taskSource))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.Params["model"] = model;

                using (var stream = DocStreams.GetOutputStream("BindCalculationAsync" + index + ".pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                return doc;
            }
        }

        protected object GetAsyncModel(int index)
        {
            var items = new dynamic[index];

            for (int i = 0; i < index; i++)
            {
                items[i] = new { name = "Item " + i, index = i };
            }

            var model = new
            {
                index = index,
                title = "Document " + index.ToString(),
                color = "#FF00FF",
                items = items
            };

            return model;
        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindCalcExpressionMultiThreaded()
        {

            int threadcount = 50;

            Task<Document>[] all = new Task<Document>[threadcount];

            for (int i = 0; i < threadcount; i++)
            {
                object model = GetAsyncModel(i);
                var task = new Task<Document>(GenerateAsync, model);
                all[i] = task;
            }

            foreach (var task in all)
            {
                task.Start();
            }

            Task.WaitAll(all);


            for (int i = 0; i < threadcount; i++)
            {
                var doc = all[i].Result;

                Assert.AreEqual("Binding Document " + i.ToString(), doc.Info.Title, "Title is not correct");

                var p = doc.FindAComponentById("myPara") as IStyledComponent;
                Assert.AreEqual((Color)"#FF00FF", p.Style.Fill.Color, "Paragraph color was not correct");

                var list = doc.FindAComponentById("mylist") as ListUnordered;
                Assert.AreEqual(i, list.Items.Count, "List counts were not correct");

                //Check each item
                for(var j = 0; j < i; j++)
                {
                    var item = list.Items[j];
                    Assert.AreEqual(1, item.Contents.Count, "No list item contents");
                    var literal = item.Contents[0] as TextLiteral;
                    Assert.IsNotNull(literal, "Text literal not found on list item " + j.ToString());

                    Assert.AreEqual("List Item " + j.ToString(), literal.Text, "Literal text does not match for item " + j.ToString());
                }
            }
        }



        [TestMethod()]
        [TestCategory("Binding")]
        public void BindingCalcInvalidExpressions()
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


            var functions = new FunctionTest[]
            {
                new FunctionTest() {grp = "Parentheses", name = "Missing end", function = "(12 + 4", result = ""},
                new FunctionTest() {grp = "Parentheses", name = "Missing start", function = "12 - 4)", result = ""},
                new FunctionTest() {grp = "Parentheses", name = "Missing middle", function = "(12 * (4)", result = ""},
                new FunctionTest() {grp = "Parentheses", name = "Open End", function = "12 / 4 (", result = ""},
                new FunctionTest() {grp = "Parentheses", name = "Function", function = "concat(", result = ""},
                new FunctionTest() {grp = "Functions", name = "Unknown Function", function = "unknown()", result = ""},
                new FunctionTest() {grp = "Functions", name = "Unknown Operator ", function = "11 ; 2", result = ""},
            };

            var options = ExpressiveOptions.IgnoreCaseForParsing;
            var fset = new FunctionSet(options).AddDefaultFunctions();
            var opSet = new OperatorSet(options).AddDefaultOperators();

            var context = new Context(options, fset, opSet);

            Dictionary<string, object> vars = new Dictionary<string, object>(context.ParsingStringComparer)
            {
                {"model", model }
            };

            foreach (var item in functions)
            {
                var value = item.function;
                bool caught = false;
                string message = "";
                try
                {
                    Expression expr = new Expression(value, context);
                    object result = expr.Evaluate(vars);
                }
                catch(Exception ex)
                {
                    message = ex.Message;
                    caught = true;
                }

                
                if (!caught)
                    throw new InvalidOperationException("The function " + item.function + " did not raise an exception " + item.result);


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
                                    <template data-bind='{{model.items}}' >
                                        <tr class='bound-item {{if(.index % 2 == 0, ""even"", ""odd"")}}'  >
                                            <td style='width:20pt'>{{1 + index()}}</td>
                                            <td style='width:70pt'>{{.name}}</td>
                                            <td>This is the cell index {{1 + index()}} with name {{.name}}</td>
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
                var body = doc.FindAComponentById("mainbody") as IStyledComponent;
                Assert.AreEqual((Color)"#330033", body.Style.Fill.Color, "Body color was not correct");

                //table binding
                var table = doc.FindAComponentById("myTable") as TableGrid;
                var rowCount = table.Rows.Count;
                Assert.AreEqual(3, rowCount, "Number of rows was wrong");


                for (int i = 0; i < rowCount; i++)
                {
                    //Table row class
                    var row = table.Rows[i];
                    //if (i % 2 == 0)
                    //    Assert.AreEqual("bound-item even", row.StyleClass, "Style class is not correct for the row");
                    //else
                    //    Assert.AreEqual("bound-item odd", row.StyleClass, "Style class is not correct for the row");

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
                        new {name = "First Item", index = 0, group = "none"},
                        new {name = "Second Item", index = 1, group = "one"},
                        new {name = "Last Item", index = 2, group = "one" }
                    }
            };
            var existCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            var functions = new[]
            {
                new {grp = "Binary Operators", name = "Plus (+)", function = "12 + 4", result = "16"},
                new {grp = "Binary Operators", name = "Minus (-)", function = "12 - 4", result = "8"},
                new {grp = "Binary Operators", name = "Multiply (*)", function = "12 * 4", result = "48"},
                new {grp = "Binary Operators", name = "Divide (/)", function = "12 / 4", result = "3"},
                new {grp = "Binary Operators", name = "All (+-*/)", function = "12 + 2 - 6 * 2 / 4", result = "11"},
                new {grp = "Binary Operators", name = "All (+-*/)", function = "((12 + 2) - 6) * (2 / 4)", result = "4"},
                //new {grp = "Binary Operators", name = "Modulo (mod) ", function = "11 mod 3", result = "2"},
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

                new {grp = "Relational Operators", name = "Not Equal", function = "model.boolean != True", result = "False"},
                new {grp = "Relational Operators", name = "Not Equal", function = "model.boolean != False", result = "True"},
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
                //new {grp = "Logical Operators", name = "And", function = "if(model.number &lt; 20 and model.number &gt; 10,'Between','Outside')", result = "Outside"},

                new {grp = "Logical Operators", name = "Or (||)", function = "if(model.number &lt; 20 || model.number &gt; 21,'Outside','Between')", result = "Between"},
                //new {grp = "Logical Operators", name = "Or", function = "if(model.number &gt; 20 or model.number &gt; 30,'Between','Outside')", result = "Between"},

                new {grp = "Logical Operators", name = "Not (!)", function = "if(!(model.number &lt; 21 &amp;&amp; model.number &gt; 20),'Outside','Between')", result = "Between"},
                //new {grp = "Logical Operators", name = "Not", function = "if(not(model.number &lt; 20 and model.number &gt; 21),'Between','Outside')", result = "Between"},

                new {grp = "Conversion Functions", name = "Date", function = "date('30 June 2021 11:00:01')", result = "6/30/2021 11:00:01 AM"},
                new {grp = "Conversion Functions", name = "Decimal", function = "decimal(20 + model.number)", result = (20 + model.number).ToString()},
                new {grp = "Conversion Functions", name = "Double", function = "double(20 + model.number)", result = (20 + model.number).ToString()},
                new {grp = "Conversion Functions", name = "Integer", function = "integer(20 + model.number)", result = Convert.ToInt32(20 + model.number).ToString()},
                new {grp = "Conversion Functions", name = "Long", function = "long(20 + model.number)", result = Convert.ToInt64(20 + model.number).ToString()},
                new {grp = "Conversion Functions", name = "string", function = "string(20 + model.number)", result = (20 + model.number).ToString()},

                new {grp = "Date Add Functions", name = "AddDays", function = "adddays(date('30 June 2021 11:00:00'),10)", result = "7/10/2021 11:00:00 AM"},
                new {grp = "Date Add Functions", name = "AddHours", function = "addhours(date('30 June 2021 11:00:00'),10)", result = "6/30/2021 9:00:00 PM"},
                new {grp = "Date Add Functions", name = "AddMilliSeconds", function = "addmilliseconds(date('30 June 2021 11:00:00'),2000)", result = "6/30/2021 11:00:02 AM"},
                new {grp = "Date Add Functions", name = "AddMinutes", function = "addMinutes(date('30 June 2021 11:00:00'),40)", result = "6/30/2021 11:40:00 AM"},
                new {grp = "Date Add Functions", name = "AddMonths", function = "addMonths(date('30 June 2021 11:00:00'),2)", result = "8/30/2021 11:00:00 AM"},
                new {grp = "Date Add Functions", name = "AddSeconds", function = "addSeconds(date('30 June 2021 11:00:00'),100)", result = "6/30/2021 11:01:40 AM"},
                new {grp = "Date Add Functions", name = "AddYears", function = "addYears(date('30 June 2021 11:00:00'),1000)", result = "6/30/3021 11:00:00 AM"},

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
                new {grp = "Mathematical Functions", name = "E", function = "round(E,3)", result = "2.718"},
                new {grp = "Mathematical Functions", name = "Pi", function = "round(Pi,3)", result = "3.142"},
                new {grp = "Mathematical Functions", name = "Exp", function = "round(exp(3),3)", result = Math.Exp(3).ToString("#0.000")},
                new {grp = "Mathematical Functions", name = "Log10", function = "round(Log10(30),3)", result = Math.Log10(30).ToString("#0.000")},
                new {grp = "Mathematical Functions", name = "Log (base 5)", function = "round(Log(30,5),3)", result = Math.Log(30,5).ToString("#0.000")},
                new {grp = "Mathematical Functions", name = "Pow", function = "pow(3,2)", result = Math.Pow(3,2).ToString()},
                new {grp = "Mathematical Functions", name = "Random (between 2 and 30)", function = "random(2,30)", result = (string)null},
                new {grp = "Mathematical Functions", name = "Random (between 2.4d and 2.6d)", function = "random(2.4, 2.6)", result = (string)null},


                new {grp = "Aggregate and Statistical Functions", name = "Count", function = "count(12, 13, 14)", result = "3"},
                new {grp = "Aggregate and Statistical Functions", name = "Count (with array)", function = "count(12, model.array, model.items, 14)", result = "8"},
                new {grp = "Aggregate and Statistical Functions", name = "Sum", function = "sum(model.array,13,14)", result = (13 + 14 + 10 + 11 + 12 ).ToString()},
                new {grp = "Aggregate and Statistical Functions", name = "SumOf", function = "sumOf(model.items, .index)", result = (0 + 1 + 2).ToString()},
                new {grp = "Aggregate and Statistical Functions", name = "Max", function = "Max(model.array,13,14)", result = "14"},
                new {grp = "Aggregate and Statistical Functions", name = "MaxOf", function = "MaxOf(model.items, .index)", result = "2"},
                new {grp = "Aggregate and Statistical Functions", name = "Min", function = "Min(model.array,13,14)", result = "10"},
                new {grp = "Aggregate and Statistical Functions", name = "MinOf", function = "MinOf(model.items, .index)", result = "0"},
                new {grp = "Aggregate and Statistical Functions", name = "Average", function = "Average(model.array,13,1)", result = "9.4"},
                new {grp = "Aggregate and Statistical Functions", name = "AverageOf", function = "AverageOf(model.items, .index)", result = "1"},
                new {grp = "Aggregate and Statistical Functions", name = "Max", function = "Max(model.array,13,14)", result = "14"},
                new {grp = "Aggregate and Statistical Functions", name = "Mean", function = "Mean(model.array,13,1)", result = "9.4"},
                new {grp = "Aggregate and Statistical Functions", name = "Mode", function = "Mode(model.array,13,1)", result = "10"},
                new {grp = "Aggregate and Statistical Functions", name = "Median", function = "Median(model.array,13,1)", result = "11"},

                //Coalescing function
                new {grp = "Coalescing Functions", name = "Join Each", function = "join(',',each(15,model.array,20), 30)", result = "15,10,11,12,20,30"},
                new {grp = "Coalescing Functions", name = "Max Each", function = "Max(8, each(model.array))", result = "12"},
                new {grp = "Coalescing Functions", name = "Join EachOf", function = "join(',', 8, eachof(model.items, .index))", result = "8,0,1,2"},
                new {grp = "Coalescing Functions", name = "Sum EachOf", function = "sum(1, eachof(model.items, .index))", result = "4"},
                new {grp = "Coalescing Functions", name = "Join EachOf SortBy", function = "join(',',eachOf(sortBy(model.items, .name, 'desc'), .index))", result = "1,2,0"}, //Second, Last, First
                new {grp = "Coalescing Functions", name = "Join EachOf SelectWhere", function = "join(',',eachOf(selectWhere(model.items, .index > 0),.name))", result = "Second Item,Last Item"},


                new {grp = "String Functions", name = "Concat", function = "concat('A string',' and another string')", result = "A string and another string"},
                new {grp = "String Functions", name = "Concat (with escapes)", function = "concat('A string',' and another string', ' and escap\\'es')", result = "A string and another string and escap'es"},
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
                new {grp = "String Functions", name = "PatLeft+PadRight", function = "padleft(padright(model.items[0].name, 15, '-'),20,'+++++')", result = "+++++First Item-----"},
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
                                    .even{ background-color: #EEE; }
                                </style>
                            </head>

                            <body id='mainbody' class='strong' style='padding:20pt' >
                                <h2>Expression functions and operators</h2>
                                <table id='myTable' style='width:100%;font-size:10pt' >";
            
            var group = "";
            int grpindex = 0;
            foreach (var fn in functions)
            {

                if (fn.grp != group)
                {
                    src += @"<tr><td colspan='3' ><h3>" + fn.grp + "</h3></td></tr>";
                    group = fn.grp;
                    grpindex = 0;
                }
                var insert = @"  <tr class='bound-item " + (grpindex % 2 == 0 ? "even" : "odd") + @"' >
                                    <td style='width:120pt'>" + fn.name + @"</td>
                                    <td>" + fn.function + @"</td>
                                    <td>{{" + fn.function + @"}}</td>
                                 </tr>";
                src += insert;
                grpindex++;
            }

            src += @"         </table>
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

            System.Threading.Thread.CurrentThread.CurrentCulture = existCulture;


        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindCalcsWithCulture()
        {
            var origCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var origUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;

            string src = @"<!DOCTYPE html>
                                <?scryber parser-mode='strict' ?>
                                <html xmlns='http://www.w3.org/1999/xhtml' >
                                    <head>
                                        <title>Binding Numbers with culture</title>
                                    </head>

                                    <body id='mainbody' style='padding:20px' >
                                        <h2>{{culture.id}}</h2>
                                        <dl>
                                            <dt id='cultP1' >Number</dt>
                                            <dd id='numP1' ><num value='{{culture.number}}' /></dd>
                                            <dt id='cultP1' >Currency</dt>
                                            <dd id='numP1' ><num data-format='C' value='{{culture.number}}' /></dd>
                                            <dt id='cultP1' >Date</dt>
                                            <dd id='numP1' ><time data-format='D' datetime='{{culture.date}}' /></dd>
                                            <dt id='cultP1' >Time</dt>
                                            <dd id='numP1' ><time data-format='T' datetime='{{culture.date}}' /></dd>
                                        </dl>
                                    </body>
                                </html>";

            var culture = "nl-NL";

            var data = new
            {
                id = culture,
                number = -3456.56,
                date = new DateTime(2022, 12, 13, 22, 45, 59)
            };

            
            var cultureNumber = "-3456,56";
            var cultureCurrency = "€ -3.456,56";
            var cutlureDate = "dinsdag 13 december 2022";
            var cutlureTime = "22:45:59";

            

            CreateAndAssertCultured(src, culture, data, culture, cultureNumber, cultureCurrency, cutlureDate, cutlureTime, "");

            culture = "fr-FR";
            cultureNumber = "-3456,56";
            cultureCurrency = "-3 456,56 €";
            cutlureDate = "mardi 13 décembre 2022";
            cutlureTime = "22:45:59";

            data = new
            {
                id = culture,
                number = -3456.56,
                date = new DateTime(2022, 12, 13, 22, 45, 59)
            };

            CreateAndAssertCultured(src, culture, data, culture, cultureNumber, cultureCurrency, cutlureDate, cutlureTime, "");

            culture = "en-GB";
            cultureNumber = "-3456.56";
            cultureCurrency = "-£3,456.56";
            cutlureDate = "Tuesday, 13 December 2022";
            cutlureTime = "22:45:59";

            data = new
            {
                id = culture,
                number = -3456.56,
                date = new DateTime(2022, 12, 13, 22, 45, 59)
            };

            CreateAndAssertCultured(src, culture, data, culture, cultureNumber, cultureCurrency, cutlureDate, cutlureTime, "");

            
            culture = "en-US";
            cultureNumber = "-3456.56";
            cultureCurrency = "-$3,456.56";
            cutlureDate = "Tuesday, December 13, 2022";
            cutlureTime = "10:45:59 PM";

            data = new
            {
                id = culture,
                number = -3456.56,
                date = new DateTime(2022, 12, 13, 22, 45, 59)
            };

            CreateAndAssertCultured(src, culture, data, culture, cultureNumber, cultureCurrency, cutlureDate, cutlureTime, "");

            System.Threading.Thread.CurrentThread.CurrentCulture = origCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = origUICulture;

        }

        private PDFLayoutDocument _doc = null;

        private void CreateAndAssertCultured(string src, string culture, object data, string cultureId, string cultureNumber, string cultureCurrency, string cultureDate, string cultureTime, string cultureExplicit)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            using (var doc = Document.ParseDocument(new System.IO.StringReader(src), ParseSourceType.DynamicContent))
            {
                doc.Params.Add("culture", data);

                using (var stream = DocStreams.GetOutputStream("Expression_WithCulture_" + culture + ".pdf"))
                {
                    doc.LayoutComplete += CulturedDoc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    Assert.IsNotNull(_doc, "The document layout was not assigned");
                    var body = _doc.AllPages[0];
                    var list = body.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
                    Assert.IsNotNull(list, "The definition list was not found");

                    
                    var numBlock = list.Columns[0].Contents[1] as PDFLayoutBlock;
                    AssertCultureContent(numBlock, cultureNumber, "number", culture);

                    var currencyBlock = list.Columns[0].Contents[3] as PDFLayoutBlock;
                    AssertCultureContent(currencyBlock, cultureCurrency, "currency", culture);

                    var dateBlock = list.Columns[0].Contents[5] as PDFLayoutBlock;
                    AssertCultureContent(dateBlock, cultureDate, "date", culture);

                    var timeBlock = list.Columns[0].Contents[7] as PDFLayoutBlock;
                    AssertCultureContent(timeBlock, cultureTime, "time", culture);
                }

            }
        }

        private void AssertCultureContent(PDFLayoutBlock container, string text, string name, string culture)
        {
            var line = container.Columns[0].Contents[0] as PDFLayoutLine;
            var span = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(span);
            Assert.AreEqual(text, span.Characters, "The characters '" + span.Characters + "' did not match expected '" + text + "' for the " + name + " value in culture " + culture);
        }

        private void CulturedDoc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _doc = args.Context.GetLayout<PDFLayoutDocument>();
        }



        [TestMethod]
        public void ExpressionsWithJElement()
        {

            var src = @"<!DOCTYPE html>
                        <?scryber parser-mode='strict' append-log='true' ?>
                        <html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Expression JElement</title>
                                <style>
                                    .even{ background-color: #EEE; }
                                </style>
                            </head>

                            <body id='mainbody' class='strong' style='padding:20pt' >
                                <h2>Expression binding with JElement</h2>
                                <table id='myTable' style='width:100%;font-size:10pt' >
                                    <tr><td>Single Expression</td><td>value</td><td>{{value}}</td></tr>
                                    <tr><td>Single Expression with function</td><td>concat('This is a ', value)</td><td>{{concat('This is a ', value)}}</td></tr>
                                    <tr><td>Deep Expression</td><td>object.value</td><td>{{object.value}}</td></tr>
                                    <tr><td>Array Expression</td><td>arrray[0]</td><td>{{array[0]}}</td></tr>
                                    <tr><td>Deep Array Expression</td><td>deeparray[1].value</td><td>{{deeparray[1].value}}</td></tr>
                                    <tr><td>Deepest Array Expression</td><td>deeparray[0].object.value</td><td>{{deeparray[0].object.value}}</td></tr>
                                    <tr><td>Expression with calculation</td><td>1 - (integer(deeparray[1].value) + 10)</td><td>{{1 - (integer(deeparray[1].value) + 10)}}</td></tr>
                                    <tr><td>Not found Expression</td><td>(deeparray[1].notset ?? 'nothing')</td><td>{{deeparray[1].notset ?? 'nothing'}}</td></tr>
                                    <tr><td>Function and Expression</td><td>(concat('Number ',deeparray[1].name))</td><td>{{(concat('Number ',deeparray[1].name))}}</td></tr>
                                    <tr><td>Function With Coalesce</td><td>(join(',', eachOf(selectWhere(deeparray, .value > 1),.name)))</td><td>{{join(',', eachOf(selectWhere(deeparray, .value > 1),.name))}}</td></tr>
                                    <tr><td colspan='3'>Bound Array</td></tr>
                                    <template data-bind='{{deeparray}}'>
                                        <tr>
                                            <td>{{.name}}</td><td><num value='{{.value}}'></num></td><td>{{concat(index(),'. ',.object.value)}}</td>
                                        </tr>
                                    </template>";

            src += @" </table>
                  </body>
                </html>";

            var json = @"{
                       'value':'Single',
                       'object': { 'value' : 'DeepValue' },
                       'array': [
                            'first item',
                            'second item'
                        ],
                       'deeparray' : [
                            { 'value': '1', 'name' : 'One', 'object' : { 'value' : 'Deep deep down' }},
                            { 'value': '2', 'name' : 'Two'}
                        ]
                    }".Replace('\'', '"'); //just easier with the @string

            var parsed = System.Text.Json.JsonDocument.Parse(json);

            using (Document doc = Document.ParseDocument(new System.IO.StringReader(src), ParseSourceType.DynamicContent))
            {
                foreach (var prop in parsed.RootElement.EnumerateObject())
                {
                    doc.Params.Add(prop.Name, prop.Value);
                }

                using (var stream = DocStreams.GetOutputStream("Expression_WithJElement.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var table = doc.FindAComponentById("myTable") as TableGrid;
                Assert.IsNotNull(table, "Could not find the table to loop over");


                var literal = table.Rows[0].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("Single", literal.Text, "The JElement single value failed");

                literal = table.Rows[1].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("This is a Single", literal.Text, "The JElement function with value failed");

                literal = table.Rows[2].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("DeepValue", literal.Text, "The JElement deep value failed");

                literal = table.Rows[3].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("first item", literal.Text, "The JElement array value failed");

                literal = table.Rows[4].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("2", literal.Text, "The JElement array value failed");

                literal = table.Rows[5].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("Deep deep down", literal.Text, "The JElement array value failed");

                literal = table.Rows[6].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("-11", literal.Text, "The JElement calculation failed");

                literal = table.Rows[7].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("nothing", literal.Text, "The JElement non-existant property failed");

                literal = table.Rows[8].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("Number Two", literal.Text, "The JElement non-existant property failed");

                literal = table.Rows[9].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("Two", literal.Text, "The coalescing function did not match");

                //row count is 12 - first 10 above + header row and 2 bound array rows
                Assert.IsTrue(table.Rows.Count == 13, "Not all rows in the array were bound");

                var first = table.Rows[11];
                var second = table.Rows[12];

                literal = first.Cells[0].Contents[0] as TextLiteral;
                Assert.AreEqual("One", literal.Text, "First bound rows value was not correct");

                literal = second.Cells[0].Contents[0] as TextLiteral;
                Assert.AreEqual("Two", literal.Text, "Second bound rows value was not correct");

                var num = first.Cells[1].Contents[0] as Number;
                Assert.AreEqual("1", num.Value.ToString(), "First bound rows value was not correct");

                num = second.Cells[1].Contents[0] as Number;
                Assert.AreEqual("2", num.Value.ToString(), "Second bound rows value was not correct");

                literal = first.Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("0. Deep deep down", literal.Text, "The calculated value on the first row was not correct");

                literal = second.Cells[2].Contents[0] as TextLiteral;
                Assert.IsTrue(string.IsNullOrEmpty(literal.Text), "The calculated value on the second row was not null or empty");
            }


        }


        [TestMethod]
        public void ExpressionsWithNewtonsoft()
        {

            var src = @"<!DOCTYPE html>
                        <?scryber parser-mode='strict' append-log='true' ?>
                        <html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Expression Newtonsoft</title>
                                <style>
                                    .even{ background-color: #EEE; }
                                </style>
                            </head>

                            <body id='mainbody' class='strong' style='padding:20pt' >
                                <h2>Expression binding with Newtonsoft</h2>
                                <table id='myTable' style='width:100%;font-size:10pt' >
                                    <tr><td>Single Expression</td><td>value</td><td>{{value}}</td></tr>
                                    <tr><td>Single Expression with function</td><td>concat('This is a ', value)</td><td>{{concat('This is a ', value)}}</td></tr>
                                    <tr><td>Deep Expression</td><td>object.value</td><td>{{object.value}}</td></tr>
                                    <tr><td>Array Expression</td><td>arrray[0]</td><td>{{array[0]}}</td></tr>
                                    <tr><td>Deep Array Expression</td><td>deeparray[1].value</td><td>{{deeparray[1].value}}</td></tr>
                                    <tr><td>Deepest Array Expression</td><td>deeparray[0].object.value</td><td>{{deeparray[0].object.value}}</td></tr>
                                    <tr><td>Expression with calculation</td><td>1 - (integer(deeparray[1].value) + 10)</td><td>{{1 - (integer(deeparray[1].value) + 10)}}</td></tr>
                                    <tr><td>Not found Expression</td><td>(deeparray[1].notset ?? 'nothing')</td><td>{{deeparray[1].notset ?? 'nothing'}}</td></tr>
                                    <tr><td>Function and Expression</td><td>(concat('Number ',deeparray[1].name))</td><td>{{(concat('Number ',deeparray[1].name))}}</td></tr>
                                    <tr><td>Function With Coalesce</td><td>(join(',', eachOf(selectWhere(deeparray, .value > 1),.name)))</td><td>{{join(',', eachOf(selectWhere(deeparray, .value > 1),.name))}}</td></tr>
                                    <tr><td colspan='3'>Bound Array</td></tr>
                                    <template data-bind='{{deeparray}}'>
                                        <tr>
                                            <td>{{.name}}</td><td><num value='{{.value}}'></num></td><td>{{concat(index(),'. ',.object.value)}}</td>
                                        </tr>
                                    </template>";

            src += @" </table>
                  </body>
                </html>";

            var json = @"{
                       'value':'Single',
                       'object': { 'value' : 'DeepValue' },
                       'array': [
                            'first item',
                            'second item'
                        ],
                       'deeparray' : [
                            { 'value': '1', 'name' : 'One', 'object' : { 'value' : 'Deep deep down' }},
                            { 'value': '2', 'name' : 'Two'}
                        ]
                    }".Replace('\'', '"'); //just easier with the @string

            //Parse as a Linq.JObject
            var parsed = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as Newtonsoft.Json.Linq.JObject;

            using (Document doc = Document.ParseDocument(new System.IO.StringReader(src), ParseSourceType.DynamicContent))
            {
                
                foreach (var prop in parsed)
                {
                    doc.Params.Add(prop.Key, prop.Value);
                }

                using (var stream = DocStreams.GetOutputStream("Expression_WithNewtonsoft.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var table = doc.FindAComponentById("myTable") as TableGrid;
                Assert.IsNotNull(table, "Could not find the table to loop over");


                var literal = table.Rows[0].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("Single", literal.Text, "The Newtonsoft.JObject single value failed");

                literal = table.Rows[1].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("This is a Single", literal.Text, "The Newtonsoft.JObject function with value failed");

                literal = table.Rows[2].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("DeepValue", literal.Text, "The Newtonsoft.JObject deep value failed");

                literal = table.Rows[3].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("first item", literal.Text, "The Newtonsoft.JObject array value failed");

                literal = table.Rows[4].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("2", literal.Text, "The Newtonsoft.JObject array value failed");

                literal = table.Rows[5].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("Deep deep down", literal.Text, "The Newtonsoft.JObject array value failed");

                literal = table.Rows[6].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("-11", literal.Text, "The Newtonsoft.JObject calculation failed");

                literal = table.Rows[7].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("nothing", literal.Text, "The Newtonsoft.JObject non-existant property failed");

                literal = table.Rows[8].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("Number Two", literal.Text, "The Newtonsoft.JObject non-existant property failed");

                literal = table.Rows[9].Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("Two", literal.Text, "The coalescing function did not match");

                //row count is 12 - first 10 above + header row and 2 bound array rows
                Assert.IsTrue(table.Rows.Count == 13, "Not all rows in the array were bound");


                var first = table.Rows[11];
                var second = table.Rows[12];

                literal = first.Cells[0].Contents[0] as TextLiteral;
                Assert.AreEqual("One", literal.Text, "First bound rows value was not correct");

                literal = second.Cells[0].Contents[0] as TextLiteral;
                Assert.AreEqual("Two", literal.Text, "Second bound rows value was not correct");

                var num = first.Cells[1].Contents[0] as Number;
                Assert.AreEqual("1", num.Value.ToString(), "First bound rows value was not correct");

                num = second.Cells[1].Contents[0] as Number;
                Assert.AreEqual("2", num.Value.ToString(), "Second bound rows value was not correct");

                literal = first.Cells[2].Contents[0] as TextLiteral;
                Assert.AreEqual("0. Deep deep down", literal.Text, "The calculated value on the first row was not correct");

                literal = second.Cells[2].Contents[0] as TextLiteral;
                Assert.IsTrue(string.IsNullOrEmpty(literal.Text), "The calculated value on the second row was not null or empty");
            }


        }



        [TestMethod]
        public void ExpressionSelectWhereIndexor()
        {

            var src = @"<!DOCTYPE html>
                        <?scryber parser-mode='strict' append-log='true' ?>
                        <html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Expression SelectWhere Indexor</title>
                                <style>
                                    .even{ background-color: #EEE; }
                                </style>
                            </head>

                            <body id='mainbody' class='strong' style='padding:20pt' >
                                <h2>Expression binding with SelectWhere and an index</h2>
                                 <p id='innerContent' style='border: solid 1px green'>{{selectWhere(items.deeparray, .name == 'One')[0].value}}</p>
                            </body>
                        </html>";

            var json = @"{
                'items': {
                       'value':'Single',
                       'items': { 'value' : 'DeepValue' },
                       'array': [
                            'first item',
                            'second item'
                        ],
                       'deeparray' : [
                            { 'value': '1', 'name' : 'One', 'object' : { 'value' : 'Deep deep down' }},
                            { 'value': '2', 'name' : 'Two'}
                        ]
                    }
                }".Replace('\'', '"'); //just easier with the @string

            var parsed = System.Text.Json.JsonDocument.Parse(json);

            using (Document doc = Document.ParseDocument(new System.IO.StringReader(src), ParseSourceType.DynamicContent))
            {
                foreach (var prop in parsed.RootElement.EnumerateObject())
                {
                    doc.Params.Add(prop.Name, prop.Value);
                }

                using (var stream = DocStreams.GetOutputStream("Expression_SelectWhereIndexor.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var p = doc.FindAComponentById("innerContent") as Paragraph;
                Assert.IsNotNull(p, "Could not find the paragraph to inspect");

                Assert.AreEqual(1, p.Contents.Count);
                var lit = p.Contents[0] as TextLiteral;
                Assert.IsNotNull(lit);
                Assert.AreEqual("1", lit.Text);

                

            }


        }

    }


}
