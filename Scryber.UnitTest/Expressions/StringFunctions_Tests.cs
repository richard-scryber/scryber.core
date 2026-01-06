using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Binding;

namespace Scryber.Core.UnitTests.Expressions
{
    /// <summary>
    /// Tests for string expression functions
    /// Phase 1: concat(), substring(), toUpper(), toLower(), trim(), length(), contains(), startsWith(), endsWith(), replace()
    /// Phase 3: padLeft(), padRight(), split(), join(), indexOf(), format(), regex functions, trim variants
    /// </summary>
    [TestClass()]
    public class StringFunctions_Tests
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

        #endregion

        #region concat() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Concat_TwoStrings_CombinesCorrectly()
        {
            var result = EvaluateExpression<string>("concat('Hello', ' World')");
            Assert.AreEqual("Hello World", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Concat_MultipleStrings_CombinesAll()
        {
            var result = EvaluateExpression<string>("concat('A', 'B', 'C', 'D')");
            Assert.AreEqual("ABCD", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Concat_WithVariables_CombinesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "firstName", "John" },
                { "lastName", "Doe" }
            };
            var result = EvaluateExpression<string>("concat(firstName, ' ', lastName)", vars);
            Assert.AreEqual("John Doe", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Concat_WithNumbers_ConvertsAndCombines()
        {
            var result = EvaluateExpression<string>("concat('Value: ', 42)");
            Assert.AreEqual("Value: 42", result);
        }

        #endregion

        #region substring() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Substring_FromStart_ReturnsCorrectPart()
        {
            var result = EvaluateExpression<string>("substring('Hello World', 0, 5)");
            Assert.AreEqual("Hello", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Substring_FromMiddle_ReturnsCorrectPart()
        {
            var result = EvaluateExpression<string>("substring('Hello World', 6, 5)");
            Assert.AreEqual("World", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Substring_ToEnd_ReturnsRemainingString()
        {
            var result = EvaluateExpression<string>("substring('Testing', 4, 3)");
            Assert.AreEqual("ing", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Substring_WithVariable_ExtractsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "text", "Programming" } };
            var result = EvaluateExpression<string>("substring(text, 0, 7)", vars);
            Assert.AreEqual("Program", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        [ExpectedException(typeof(Expressive.Exceptions.ExpressiveException))]
        public void Substring_StartIndexOutOfRange_ThrowsException()
        {
            EvaluateExpression<string>("substring('test', 10, 2)");
        }

        #endregion

        #region toUpper() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void ToUpper_LowercaseString_ConvertsToUppercase()
        {
            var result = EvaluateExpression<string>("toUpper('hello world')");
            Assert.AreEqual("HELLO WORLD", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void ToUpper_MixedCaseString_ConvertsAllToUppercase()
        {
            var result = EvaluateExpression<string>("toUpper('HeLLo WoRLd')");
            Assert.AreEqual("HELLO WORLD", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void ToUpper_AlreadyUppercase_RemainsUnchanged()
        {
            var result = EvaluateExpression<string>("toUpper('HELLO')");
            Assert.AreEqual("HELLO", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void ToUpper_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "text", "lowercase" } };
            var result = EvaluateExpression<string>("toUpper(text)", vars);
            Assert.AreEqual("LOWERCASE", result);
        }

        #endregion

        #region toLower() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void ToLower_UppercaseString_ConvertsToLowercase()
        {
            var result = EvaluateExpression<string>("toLower('HELLO WORLD')");
            Assert.AreEqual("hello world", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void ToLower_MixedCaseString_ConvertsAllToLowercase()
        {
            var result = EvaluateExpression<string>("toLower('HeLLo WoRLd')");
            Assert.AreEqual("hello world", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void ToLower_AlreadyLowercase_RemainsUnchanged()
        {
            var result = EvaluateExpression<string>("toLower('hello')");
            Assert.AreEqual("hello", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void ToLower_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "text", "UPPERCASE" } };
            var result = EvaluateExpression<string>("toLower(text)", vars);
            Assert.AreEqual("uppercase", result);
        }

        #endregion

        #region trim() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Trim_LeadingSpaces_RemovesSpaces()
        {
            var result = EvaluateExpression<string>("trim('   Hello')");
            Assert.AreEqual("Hello", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Trim_TrailingSpaces_RemovesSpaces()
        {
            var result = EvaluateExpression<string>("trim('Hello   ')");
            Assert.AreEqual("Hello", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Trim_BothEndsSpaces_RemovesBothSides()
        {
            var result = EvaluateExpression<string>("trim('  Hello World  ')");
            Assert.AreEqual("Hello World", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Trim_NoSpaces_RemainsUnchanged()
        {
            var result = EvaluateExpression<string>("trim('Hello')");
            Assert.AreEqual("Hello", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Trim_WithVariable_TrimsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "text", "  spaced  " } };
            var result = EvaluateExpression<string>("trim(text)", vars);
            Assert.AreEqual("spaced", result);
        }

        #endregion

        #region length() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Length_SimpleString_ReturnsCorrectLength()
        {
            var result = EvaluateExpression<int>("length('Hello')");
            Assert.AreEqual(5, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Length_EmptyString_ReturnsZero()
        {
            var result = EvaluateExpression<int>("length('')");
            Assert.AreEqual(0, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Length_StringWithSpaces_CountsSpaces()
        {
            var result = EvaluateExpression<int>("length('Hello World')");
            Assert.AreEqual(11, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Length_WithVariable_ReturnsCorrectLength()
        {
            var vars = new Dictionary<string, object> { { "text", "Testing" } };
            var result = EvaluateExpression<int>("length(text)", vars);
            Assert.AreEqual(7, result);
        }

        #endregion

        #region contains() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Contains_SubstringPresent_ReturnsTrue()
        {
            var result = EvaluateExpression<bool>("contains('Hello World', 'World')");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Contains_SubstringNotPresent_ReturnsFalse()
        {
            var result = EvaluateExpression<bool>("contains('Hello World', 'test')");
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Contains_CaseSensitive_ReturnsFalseForDifferentCase()
        {
            var result = EvaluateExpression<bool>("contains('Hello World', 'world')");
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Contains_EmptySubstring_ReturnsTrue()
        {
            var result = EvaluateExpression<bool>("contains('Hello', '')");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Contains_WithVariable_ChecksCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "text", "Programming" },
                { "search", "gram" }
            };
            var result = EvaluateExpression<bool>("contains(text, search)", vars);
            Assert.AreEqual(true, result);
        }

        #endregion

        #region startsWith() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void StartsWith_MatchingPrefix_ReturnsTrue()
        {
            var result = EvaluateExpression<bool>("startsWith('Hello World', 'Hello')");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void StartsWith_NonMatchingPrefix_ReturnsFalse()
        {
            var result = EvaluateExpression<bool>("startsWith('Hello World', 'World')");
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void StartsWith_CaseSensitive_ReturnsFalseForDifferentCase()
        {
            var result = EvaluateExpression<bool>("startsWith('Hello World', 'hello')");
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void StartsWith_EmptyString_ReturnsTrue()
        {
            var result = EvaluateExpression<bool>("startsWith('Hello', '')");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void StartsWith_WithVariable_ChecksCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "text", "Testing123" },
                { "prefix", "Test" }
            };
            var result = EvaluateExpression<bool>("startsWith(text, prefix)", vars);
            Assert.AreEqual(true, result);
        }

        #endregion

        #region endsWith() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void EndsWith_MatchingSuffix_ReturnsTrue()
        {
            var result = EvaluateExpression<bool>("endsWith('Hello World', 'World')");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void EndsWith_NonMatchingSuffix_ReturnsFalse()
        {
            var result = EvaluateExpression<bool>("endsWith('Hello World', 'Hello')");
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void EndsWith_CaseSensitive_ReturnsFalseForDifferentCase()
        {
            var result = EvaluateExpression<bool>("endsWith('Hello World', 'world')");
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void EndsWith_EmptyString_ReturnsTrue()
        {
            var result = EvaluateExpression<bool>("endsWith('Hello', '')");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void EndsWith_WithVariable_ChecksCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "filename", "document.pdf" },
                { "extension", ".pdf" }
            };
            var result = EvaluateExpression<bool>("endsWith(filename, extension)", vars);
            Assert.AreEqual(true, result);
        }

        #endregion

        #region replace() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Replace_SingleOccurrence_ReplacesCorrectly()
        {
            var result = EvaluateExpression<string>("replace('Hello World', 'World', 'Universe')");
            Assert.AreEqual("Hello Universe", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Replace_MultipleOccurrences_ReplacesAll()
        {
            var result = EvaluateExpression<string>("replace('test test test', 'test', 'demo')");
            Assert.AreEqual("demo demo demo", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Replace_NoMatch_ReturnsOriginal()
        {
            var result = EvaluateExpression<string>("replace('Hello World', 'test', 'demo')");
            Assert.AreEqual("Hello World", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Replace_WithEmptyString_RemovesOccurrences()
        {
            var result = EvaluateExpression<string>("replace('Hello World', ' World', '')");
            Assert.AreEqual("Hello", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Replace_WithVariable_ReplacesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "text", "Hello {name}" },
                { "placeholder", "{name}" },
                { "value", "John" }
            };
            var result = EvaluateExpression<string>("replace(text, placeholder, value)", vars);
            Assert.AreEqual("Hello John", result);
        }

        #endregion

        #region padLeft() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void PadLeft_WithDefaultChar_PadsCorrectly()
        {
            var result = EvaluateExpression<string>("padLeft('test', 10)");
            Assert.AreEqual("      test", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void PadLeft_WithCustomChar_PadsCorrectly()
        {
            var result = EvaluateExpression<string>("padLeft('42', 5, '0')");
            Assert.AreEqual("00042", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void PadLeft_LongerThanTarget_RemainsUnchanged()
        {
            var result = EvaluateExpression<string>("padLeft('verylongtext', 5)");
            Assert.AreEqual("verylongtext", result);
        }

        #endregion

        #region padRight() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void PadRight_WithDefaultChar_PadsCorrectly()
        {
            var result = EvaluateExpression<string>("padRight('test', 10)");
            Assert.AreEqual("test      ", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void PadRight_WithCustomChar_PadsCorrectly()
        {
            var result = EvaluateExpression<string>("padRight('42', 5, '-')");
            Assert.AreEqual("42---", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void PadRight_LongerThanTarget_RemainsUnchanged()
        {
            var result = EvaluateExpression<string>("padRight('verylongtext', 5)");
            Assert.AreEqual("verylongtext", result);
        }

        #endregion

        #region split() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Split_WithComma_SplitsIntoArray()
        {
            var vars = new Dictionary<string, object> { { "text", "apple,banana,cherry" } };
            var result = EvaluateExpression<object>("split(text, ',')", vars);
            Assert.IsNotNull(result);
            // Note: Result is an array/collection
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Split_WithSpace_SplitsWords()
        {
            var result = EvaluateExpression<object>("split('Hello World Test', ' ')");
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Split_NoDelimiter_ReturnsSingleItem()
        {
            var result = EvaluateExpression<object>("split('NoDelimiterHere', ',')");
            Assert.IsNotNull(result);
        }

        #endregion

        #region join() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Join_ArrayWithComma_CombinesWithDelimiter()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[] { "apple", "banana", "cherry" } }
            };
            var result = EvaluateExpression<string>("join(', ', items)", vars);
            Assert.AreEqual("apple, banana, cherry", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Join_ArrayWithSpace_CombinesWords()
        {
            var vars = new Dictionary<string, object>
            {
                { "words", new[] { "Hello", "World", "Test" } }
            };
            var result = EvaluateExpression<string>("join(' ', words)", vars);
            Assert.AreEqual("Hello World Test", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Join_EmptyArray_ReturnsEmptyString()
        {
            var vars = new Dictionary<string, object>
            {
                { "empty", new string[] { } }
            };
            var result = EvaluateExpression<string>("join(',', empty)", vars);
            Assert.AreEqual("", result);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Join_StringsWithComma_CombinesWithDelimiter()
        {
            var vars = new Dictionary<string, object>
            {
            };
            var result = EvaluateExpression<string>("join(', ', 'apple', 'banana', 'cherry')", vars);
            Assert.AreEqual("apple, banana, cherry", result);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Join_StringsAndArrayWithSemiColon_CombinesWithDelimiter()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[] { "apple", "banana", "cherry" } }
            };
            var result = EvaluateExpression<string>("join('; ', 'mango', items, 'grapes')", vars);
            Assert.AreEqual("mango; apple; banana; cherry; grapes", result);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Join_StringsAndEmptyArrayWithComma_CombinesWithDelimiter()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new string[] {}  }
            };
            var result = EvaluateExpression<string>("join(', ', 'apple', 'banana', items, 'cherry')", vars);
            Assert.AreEqual("apple, banana, cherry", result);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Join_StringsAndNullWithComma_CombinesWithDelimiter()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", null  }
            };
            var result = EvaluateExpression<string>("join(', ', 'apple', 'banana', items, 'cherry')", vars);
            Assert.AreEqual("apple, banana, cherry", result);
        }

        #endregion

        #region indexOf() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void IndexOf_SubstringFound_ReturnsPosition()
        {
            var result = EvaluateExpression<int>("indexOf('Hello World', 'World')");
            Assert.AreEqual(6, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void IndexOf_SubstringNotFound_ReturnsMinusOne()
        {
            var result = EvaluateExpression<int>("indexOf('Hello World', 'test')");
            Assert.AreEqual(-1, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void IndexOf_AtStart_ReturnsZero()
        {
            var result = EvaluateExpression<int>("indexOf('Hello World', 'Hello')");
            Assert.AreEqual(0, result);
        }

        #endregion

        #region format() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Format_Currency_FormatsCorrectly()
        {
            var result = EvaluateExpression<string>("format(1234.56, 'C2')");
            Assert.IsTrue(result.Contains("1,234.56") || result.Contains("1234.56"));
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Format_Number_FormatsWithDecimals()
        {
            var result = EvaluateExpression<string>("format(1234.5678, 'N2')");
            Assert.IsTrue(result.Contains("1,234.57") || result.Contains("1234.57"));
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Format_Date_FormatsCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "date", new DateTime(2024, 3, 15) }
            };
            var result = EvaluateExpression<string>("format(date, 'yyyy-MM-dd')", vars);
            Assert.AreEqual("2024-03-15", result);
        }

        #endregion

        #region trimStart() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void TrimStart_LeadingSpaces_RemovesSpaces()
        {
            var result = EvaluateExpression<string>("trimStart('   Hello World   ')");
            Assert.AreEqual("Hello World   ", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void TrimStart_NoLeadingSpaces_RemainsUnchanged()
        {
            var result = EvaluateExpression<string>("trimStart('Hello World')");
            Assert.AreEqual("Hello World", result);
        }

        #endregion

        #region trimEnd() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void TrimEnd_TrailingSpaces_RemovesSpaces()
        {
            var result = EvaluateExpression<string>("trimEnd('   Hello World   ')");
            Assert.AreEqual("   Hello World", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void TrimEnd_NoTrailingSpaces_RemainsUnchanged()
        {
            var result = EvaluateExpression<string>("trimEnd('Hello World')");
            Assert.AreEqual("Hello World", result);
        }

        #endregion

        #region regexIsMatch() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void RegexIsMatch_PatternMatches_ReturnsTrue()
        {
            var result = EvaluateExpression<bool>("isMatch('test123', '[0-9]+')");
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void RegexIsMatch_PatternDoesNotMatch_ReturnsFalse()
        {
            var result = EvaluateExpression<bool>("isMatch('testonly', '[0-9]+')");
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void RegexIsMatch_EmailPattern_ValidatesCorrectly()
        {
            var pattern = "'^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$'";
            var result = EvaluateExpression<bool>($"isMatch('test@example.com', {pattern})");
            Assert.AreEqual(true, result);
        }

        #endregion

        #region regexMatches() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void RegexMatches_FindsMatches_ReturnsCollection()
        {
            var result = EvaluateExpression<object>("matches('test123abc456', '[0-9]+')");
            Assert.IsNotNull(result);

            // Result should contain matches: "123" and "456"
            var items = result as string[];
            Assert.IsNotNull(items);
            Assert.AreEqual(2, items.Length);
            Assert.AreEqual("123", items[0]);
            Assert.AreEqual("456", items[1]);
           
        }

        #endregion

        #region regexSwap() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void RegexSwap_ReplacesPattern_ReturnsModifiedString()
        {
            var result = EvaluateExpression<string>("swap('test123abc456', '[0-9]+', '#')");
            Assert.AreEqual("test#abc#", result);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void RegexSwap_ReplacesPattern2_ReturnsModifiedString()
        {
            var result = EvaluateExpression<string>("swap('test123abc456', '[0-9]', '#')");
            Assert.AreEqual("test###abc###", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void RegexSwap_NoMatch_ReturnsOriginal()
        {
            var result = EvaluateExpression<string>("swap('testonly', '[0-9]+', '#')");
            Assert.AreEqual("testonly", result);
        }

        #endregion

        #region eval() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Eval_SimpleExpression_EvaluatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "expr", "10 + 20" }
            };
            var result = EvaluateExpression<double>("eval(expr)", vars);
            Assert.AreEqual(30.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("String")]
        public void Eval_MathExpression_CalculatesCorrectly()
        {
            var result = EvaluateExpression<double>("eval('5 * 6')");
            Assert.AreEqual(30.0, result, 0.0001);
        }

        #endregion
    }
}
