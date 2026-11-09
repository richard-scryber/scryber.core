using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Binding;

namespace Scryber.Core.UnitTests.Expressions
{
    /// <summary>
    /// Tests for CSS-related expression functions: calc(), var()
    /// These functions generate CSS syntax for use in style attributes
    /// </summary>
    [TestClass()]
    public class CssFunctions_Tests
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

        #region calc() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Calc_SimpleAddition_GeneratesCalcExpression()
        {
            var result = EvaluateExpression<string>("calc('100% - 20pt')");
            Assert.AreEqual("100% - 20pt", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Calc_WithMultiplication_GeneratesCorrectSyntax()
        {
            var result = EvaluateExpression<string>("calc('50% * 2')");
            Assert.AreEqual("50% * 2", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Calc_WithDivision_GeneratesCorrectSyntax()
        {
            var result = EvaluateExpression<string>("calc('100vw / 3')");
            Assert.AreEqual("100vw / 3", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Calc_ComplexExpression_PreservesFormatting()
        {
            var result = EvaluateExpression<string>("calc('(100% - 40pt) / 2 + 10pt')");
            Assert.AreEqual("(100% - 40pt) / 2 + 10pt", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Calc_WithVariable_SubstitutesValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "margin", 20 }
            };
            var result = EvaluateExpression<string>("concat('100% - ', margin, 'pt')", vars);
            Assert.AreEqual("100% - 20pt", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Calc_WidthCalculation_GeneratesValidCss()
        {
            var vars = new Dictionary<string, object>
            {
                { "columns", 3 },
                { "gap", 10 }
            };
            // Generate calc expression for column width
            var result = EvaluateExpression<string>(
                "calc(concat('(100% - ', (columns - 1) * gap, 'pt) / ', columns))",
                vars
            );
            Assert.AreEqual("(100% - 20pt) / 3", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Calc_MixedUnits_PreservesAllUnits()
        {
            var result = EvaluateExpression<string>("calc('100vw - 2em - 10px')");
            Assert.AreEqual("100vw - 2em - 10px", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Calc_WithParentheses_MaintainsGrouping()
        {
            var result = EvaluateExpression<string>("calc('(100% - 20pt) * 0.5')");
            Assert.AreEqual("(100% - 20pt) * 0.5", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Calc_DynamicMargin_BuildsExpression()
        {
            var vars = new Dictionary<string, object>
            {
                { "baseMargin", 10 },
                { "multiplier", 2 }
            };
            var result = EvaluateExpression<string>(
                "calc(concat('100% - ', baseMargin * multiplier, 'pt'))",
                vars
            );
            Assert.AreEqual("100% - 20pt", result);
        }

        #endregion

        #region var() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Var_SimpleCssVariable_GeneratesVarSyntax()
        {
            var vars = new Dictionary<string, object>
            {
                { "--primary-color", "#336633" }
            };
            
            var result = EvaluateExpression<string>("var(--primary-color)", vars);
            Assert.AreEqual("#336633", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Var_WithFallbackValue_NotNeeded()
        {
            var vars = new Dictionary<string, object>
            {
                { "--primary-color", "#336633" }
            };
            var result = EvaluateExpression<string>("var(--primary-color, '#336699')", vars);
            Assert.AreEqual("#336633", result);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Var_WithFallbackValue_IncludesFallback()
        {
            var vars = new Dictionary<string, object>
            {
                { "--secondary-color", "#336633" }
            };
            var result = EvaluateExpression<string>("var(--primary-color, '#336699')", vars);
            Assert.AreEqual("#336699", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Var_NumericVariable_GeneratesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "--base-spacing", "20pt" }
            };
            
            var result = EvaluateExpression<string>("var(--base-spacing)", vars);
            Assert.AreEqual("20pt", result);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Var_NumericVariableNotDeclared_IsNull()
        {
            var vars = new Dictionary<string, object>
            {
                { "--big-spacing", "20pt" }
            };
            
            var result = EvaluateExpression<string>("var(--base-spacing)", vars);
            Assert.IsNull(result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Var_WithUnitFallback_PreservesUnits()
        {
            var vars = new Dictionary<string, object>
            {
                { "--spacing", "20pt" }
            };
            
            var result = EvaluateExpression<string>("var(--spacing, '10pt')",  vars);
            Assert.AreEqual("20pt", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Var_NestedInCalc_CombinesBothFunctions()
        {
            var vars = new Dictionary<string, object>
            {
                { "--base-width", "1in" }
            };
            
            var result = EvaluateExpression<string>("calc(var(--base-width) - 20pt)", vars);
            Assert.AreEqual("52", result); //Converted to points during calculation  = 72pt - 20pt as a double
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Var_WithDynamicName_BuildsVariableName()
        {
            var vars = new Dictionary<string, object>
            {
                { "theme", "dark" },
                { "--dark-background", "#030303" }
            };
            var result = EvaluateExpression<string>(
                "var(eval(concat('--', theme, '-background')))",
                vars
            );
            Assert.AreEqual("#030303", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Var_ColorWithFallback_GeneratesValidCss()
        {
            var result = EvaluateExpression<string>("var(--text-color, rgb(0, 255, 0))");
            Assert.AreEqual("rgb(0,255,0)", result); //no spaces, also means we parsed and created the color.
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Var_ColorWithFallbackAndSecondVar_GeneratesValidCss()
        {
            var vars = new Dictionary<string, object>
            {
                { "--green", "128" }
            };
            var result = EvaluateExpression<string>("var(--text-color, rgb(0,var(--green), 0))", vars);
            Assert.AreEqual("rgb(0,128,0)", result); //no spaces, also means we parsed and created the color.
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("CSS")]
        public void Var_FontFamilyVariable_HandlesSpaces()
        {
            var result = EvaluateExpression<string>("var(--font-family, 'Helvetica, Arial, sans-serif')");
            Assert.AreEqual("Helvetica, Arial, sans-serif", result);
        }
        
        #endregion
    }
}
