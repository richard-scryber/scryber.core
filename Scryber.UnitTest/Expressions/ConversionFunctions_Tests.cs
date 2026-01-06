using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Binding;

namespace Scryber.Core.UnitTests.Expressions
{
    /// <summary>
    /// Tests for conversion expression functions: bool(), integer(), long(), double(), decimal(), string(), date(), typeof()
    /// </summary>
    [TestClass()]
    public class ConversionFunctions_Tests
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Helper Methods

        private BindingCalcExpressionFactory CreateFactory()
        {
            return new BindingCalcExpressionFactory();
        }

        private T EvaluateExpression<T>(string expression, Dictionary<string, object> vars = null)
        {
            var factory = CreateFactory();
            var expr = factory.CreateExpression(expression);
            return expr.Evaluate<T>(vars ?? new Dictionary<string, object>());
        }

        private object EvaluateExpression(string expression, Dictionary<string, object> vars = null)
        {
            var factory = CreateFactory();
            var expr = factory.CreateExpression(expression);
            return expr.Evaluate(vars ?? new Dictionary<string, object>());
        }

        #endregion

        #region bool() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Bool_TrueString_ReturnsTrue()
        {
            var result = EvaluateExpression<bool>("bool('true')");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Bool_FalseString_ReturnsFalse()
        {
            var result = EvaluateExpression<bool>("bool('false')");
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Bool_NumericOne_ReturnsTrue()
        {
            var result = EvaluateExpression<bool>("bool(1)");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Bool_NumericZero_ReturnsFalse()
        {
            var result = EvaluateExpression<bool>("bool(0)");
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Bool_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", "True" } };
            var result = EvaluateExpression<bool>("bool(value)", vars);
            Assert.AreEqual(true, result);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Boolean_TrueString_ReturnsTrue()
        {
            var result = EvaluateExpression<bool>("boolean('true')");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Boolean_FalseString_ReturnsFalse()
        {
            var result = EvaluateExpression<bool>("boolean('false')");
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Boolean_NumericOne_ReturnsTrue()
        {
            var result = EvaluateExpression<bool>("boolean(1)");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Boolean_NumericZero_ReturnsFalse()
        {
            var result = EvaluateExpression<bool>("boolean(0)");
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Boolean_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", "True" } };
            var result = EvaluateExpression<bool>("boolean(value)", vars);
            Assert.AreEqual(true, result);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Boolean_Null_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", "True" } };
            var result = EvaluateExpression<bool>("boolean(null)", vars);
            Assert.AreEqual(false, result);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Boolean_NotNull_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", "True" } };
            var result = EvaluateExpression<bool>("boolean('a value')", vars);
            Assert.AreEqual(true, result);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Boolean_NotNullVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", "'a string'" } };
            var result = EvaluateExpression<bool>("boolean(value)", vars);
            Assert.AreEqual(true, result);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Boolean_NullVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", null } };
            var result = EvaluateExpression<bool>("boolean(value)", vars);
            Assert.AreEqual(false, result);
        }

        #endregion

        #region integer() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Integer_ValidString_ReturnsInteger()
        {
            var result = EvaluateExpression<int>("integer('42')");
            Assert.AreEqual(42, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Integer_NegativeString_ReturnsNegativeInteger()
        {
            var result = EvaluateExpression<int>("integer('-100')");
            Assert.AreEqual(-100, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Integer_Double_RoundsToInteger()
        {
            var result = EvaluateExpression<int>("integer(42.7)");
            Assert.AreEqual(43, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Integer_HexString_ConvertsCorrectly()
        {
            var result = EvaluateExpression<int>("integer('0xFF')");
            Assert.AreEqual(255, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Integer_BinaryString_ConvertsCorrectly()
        {
            var result = EvaluateExpression<int>("integer('0b1010')");
            Assert.AreEqual(10, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Integer_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", "123" } };
            var result = EvaluateExpression<int>("integer(value)", vars);
            Assert.AreEqual(123, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        [ExpectedException(typeof(Expressive.Exceptions.ExpressiveException))]
        public void Integer_InvalidString_ThrowsException()
        {
            EvaluateExpression<int>("integer('not-a-number')");
        }

        #endregion

        #region long() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Long_ValidString_ReturnsLong()
        {
            var result = EvaluateExpression<long>("long('9223372036854775807')");
            Assert.AreEqual(9223372036854775807L, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Long_NegativeString_ReturnsNegativeLong()
        {
            var result = EvaluateExpression<long>("long('-9223372036854775808')");
            Assert.AreEqual(-9223372036854775808L, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Long_Integer_ConvertsToLong()
        {
            var result = EvaluateExpression<long>("long(42)");
            Assert.AreEqual(42L, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Long_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", "1000000" } };
            var result = EvaluateExpression<long>("long(value)", vars);
            Assert.AreEqual(1000000L, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        [ExpectedException(typeof(Expressive.Exceptions.ExpressiveException))]
        public void Long_InvalidString_ThrowsException()
        {
            EvaluateExpression<long>("long('invalid')");
        }

        #endregion

        #region double() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Double_ValidString_ReturnsDouble()
        {
            var result = EvaluateExpression<double>("double('42.5')");
            Assert.AreEqual(42.5, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Double_NegativeString_ReturnsNegativeDouble()
        {
            var result = EvaluateExpression<double>("double('-123.456')");
            Assert.AreEqual(-123.456, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Double_Integer_ConvertsToDouble()
        {
            var result = EvaluateExpression<double>("double(42)");
            Assert.AreEqual(42.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Double_ScientificNotation_ConvertsCorrectly()
        {
            var result = EvaluateExpression<double>("double('1.23e2')");
            Assert.AreEqual(123.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Double_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", "3.14159" } };
            var result = EvaluateExpression<double>("double(value)", vars);
            Assert.AreEqual(3.14159, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        [ExpectedException(typeof(Expressive.Exceptions.ExpressiveException))]
        public void Double_InvalidString_ThrowsException()
        {
            EvaluateExpression<double>("double('not-a-number')");
        }

        #endregion

        #region decimal() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Decimal_ValidString_ReturnsDecimal()
        {
            var result = EvaluateExpression<decimal>("decimal('123.45')");
            Assert.AreEqual(123.45M, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Decimal_NegativeString_ReturnsNegativeDecimal()
        {
            var result = EvaluateExpression<decimal>("decimal('-99.99')");
            Assert.AreEqual(-99.99M, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Decimal_Integer_ConvertsToDecimal()
        {
            var result = EvaluateExpression<decimal>("decimal(100)");
            Assert.AreEqual(100M, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Decimal_HighPrecision_MaintainsPrecision10()
        {
            var result = EvaluateExpression<decimal>("decimal('123.456789012345')");
            var expected = Math.Round(123.456789012345M, 10);
            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Decimal_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "price", "19.99" } };
            var result = EvaluateExpression<decimal>("decimal(price)", vars);
            Assert.AreEqual(19.99M, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        [ExpectedException(typeof(Expressive.Exceptions.ExpressiveException))]
        public void Decimal_InvalidString_ThrowsException()
        {
            EvaluateExpression<decimal>("decimal('invalid')");
        }

        #endregion

        #region string() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void String_Integer_ConvertsToString()
        {
            var result = EvaluateExpression<string>("string(42)");
            Assert.AreEqual("42", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void String_Double_ConvertsToString()
        {
            var result = EvaluateExpression<string>("string(3.14)");
            Assert.IsTrue(result.StartsWith("3.14"));
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void String_Boolean_ConvertsToString()
        {
            var result = EvaluateExpression<string>("string(true)");
            Assert.AreEqual("True", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void String_Date_ConvertsToString()
        {
            var vars = new Dictionary<string, object> { { "date", new DateTime(2024, 1, 15) } };
            var result = EvaluateExpression<string>("string(date)", vars);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("2024"));
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void String_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", 100 } };
            var result = EvaluateExpression<string>("string(value)", vars);
            Assert.AreEqual("100", result);
        }

        #endregion

        #region date() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_ValidDateString_ReturnsDate()
        {
            var result = EvaluateExpression<DateTime>("date('2024-01-15')");
            Assert.AreEqual(new DateTime(2024, 1, 15), result.Date);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_DateTimeString_ReturnsDateTime()
        {
            var result = EvaluateExpression<DateTime>("date('2024-01-15 10:30:00')");
            Assert.AreEqual(new DateTime(2024, 1, 15, 10, 30, 0), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_IsoFormat_ParsesCorrectly()
        {
            var result = EvaluateExpression<DateTime>("date('2024-01-15T10:30:00')");
            Assert.AreEqual(new DateTime(2024, 1, 15, 10, 30, 0), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "dateStr", "2024-12-25" } };
            var result = EvaluateExpression<DateTime>("date(dateStr)", vars);
            Assert.AreEqual(new DateTime(2024, 12, 25), result.Date);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        [ExpectedException(typeof(Expressive.Exceptions.ExpressiveException))]
        public void Date_InvalidString_ThrowsException()
        {
            EvaluateExpression<DateTime>("date('not-a-date')");
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_WithFormat_US_ParsesCorrectly()
        {
            // US date format: MM/dd/yyyy
            var result = EvaluateExpression<DateTime>("date('01/15/2024', 'MM/dd/yyyy')");
            Assert.AreEqual(new DateTime(2024, 1, 15), result.Date);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_WithFormat_UK_ParsesCorrectly()
        {
            // UK date format: dd/MM/yyyy
            var result = EvaluateExpression<DateTime>("date('15/01/2024', 'dd/MM/yyyy')");
            Assert.AreEqual(new DateTime(2024, 1, 15), result.Date);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_WithFormat_FullDateTime_ParsesCorrectly()
        {
            // Full date and time with format
            var result = EvaluateExpression<DateTime>("date('2024-01-15 14:30:45', 'yyyy-MM-dd HH:mm:ss')");
            Assert.AreEqual(new DateTime(2024, 1, 15, 14, 30, 45), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_WithFormat_MonthName_ParsesCorrectly()
        {
            // Date with month name
            var result = EvaluateExpression<DateTime>("date('January 15, 2024', 'MMMM dd, yyyy')");
            Assert.AreEqual(new DateTime(2024, 1, 15), result.Date);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_WithFormat_ShortMonthName_ParsesCorrectly()
        {
            // Date with abbreviated month name
            var result = EvaluateExpression<DateTime>("date('Jan 15, 2024', 'MMM dd, yyyy')");
            Assert.AreEqual(new DateTime(2024, 1, 15), result.Date);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_WithFormat_12HourClock_ParsesCorrectly()
        {
            // 12-hour clock format with AM/PM
            var result = EvaluateExpression<DateTime>("date('01/15/2024 02:30:00 PM', 'MM/dd/yyyy hh:mm:ss tt')");
            Assert.AreEqual(new DateTime(2024, 1, 15, 14, 30, 0), result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_WithFormat_Variable_ParsesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "dateStr", "15-01-2024" },
                { "format", "dd-MM-yyyy" }
            };
            var result = EvaluateExpression<DateTime>("date(dateStr, format)", vars);
            Assert.AreEqual(new DateTime(2024, 1, 15), result.Date);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_FromMilliseconds_Integer_ConvertsFromUnixEpoch()
        {
            // Unix timestamp in milliseconds for 2024-01-15 00:00:00 UTC
            // (Approximately 1705276800000 milliseconds since 1970-01-01)
            long milliseconds = 1705276800000L;
            var vars = new Dictionary<string, object> { { "ms", (int)(milliseconds / 1000000) } };

            // Convert milliseconds to DateTime
            var result = EvaluateExpression<DateTime>("date(1705276800000)", vars);

            // Verify it's a valid DateTime (exact value depends on timezone handling)
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Year == 2024);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_FromMilliseconds_Long_ConvertsFromUnixEpoch()
        {
            // Unix timestamp in milliseconds
            var vars = new Dictionary<string, object>
            {
                { "timestamp", 1705276800000L } // 2024-01-15 00:00:00 UTC
            };

            var result = EvaluateExpression<DateTime>("date(timestamp)", vars);

            // Verify it's a valid DateTime
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Year == 2024);
            Assert.IsTrue(result.Month == 1);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_FromMilliseconds_Zero_ReturnsUnixEpoch()
        {
            // 0 milliseconds = Unix epoch (1970-01-01 00:00:00)
            var result = EvaluateExpression<DateTime>("date(0)");

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.AreEqual(epoch.Date, result.ToUniversalTime().Date);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_FromMilliseconds_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "unixTime", 1609459200000L } // 2021-01-01 00:00:00 UTC
            };

            var result = EvaluateExpression<DateTime>("date(unixTime)", vars);

            Assert.IsNotNull(result);
            Assert.AreEqual(2021, result.Year);
            Assert.AreEqual(1, result.Month);
            Assert.AreEqual(1, result.Day);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_FromMilliseconds_RecentDate_ConvertsCorrectly()
        {
            // Recent timestamp: December 25, 2023 12:00:00 UTC
            var vars = new Dictionary<string, object>
            {
                { "christmas2023", 1703505600000L }
            };

            var result = EvaluateExpression<DateTime>("date(christmas2023)", vars);

            Assert.IsNotNull(result);
            Assert.AreEqual(2023, result.Year);
            Assert.AreEqual(12, result.Month);
            Assert.AreEqual(25, result.Day);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        [ExpectedException(typeof(Expressive.Exceptions.ExpressiveException))]
        public void Date_WithInvalidFormat_ThrowsException()
        {
            // Invalid date string for the specified format
            EvaluateExpression<DateTime>("date('2024-01-15', 'MM/dd/yyyy')");
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void Date_WithFormat_YearMonthDay_NoSeparators_ParsesCorrectly()
        {
            // Compact format without separators
            var result = EvaluateExpression<DateTime>("date('20240115', 'yyyyMMdd')");
            Assert.AreEqual(new DateTime(2024, 1, 15), result.Date);
        }

        #endregion

        #region typeof() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void TypeOf_Integer_ReturnsIntegerType()
        {
            var result = EvaluateExpression<string>("typeof(42)");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("Int") || result.Contains("32"));
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void TypeOf_String_ReturnsStringType()
        {
            var result = EvaluateExpression<string>("typeof('test')");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("String"));
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void TypeOf_Boolean_ReturnsBooleanType()
        {
            var result = EvaluateExpression<string>("typeof(true)");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("Bool"));
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void TypeOf_Double_ReturnsDoubleType()
        {
            var result = EvaluateExpression<string>("typeof(3.14)");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("Double") || result.Contains("Decimal"));
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Conversion")]
        public void TypeOf_WithVariable_ReturnsCorrectType()
        {
            var vars = new Dictionary<string, object> { { "value", new DateTime(2024, 1, 1) } };
            var result = EvaluateExpression<string>("typeof(value)", vars);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("DateTime"));
        }

        #endregion
    }
}
