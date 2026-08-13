using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Styles;
using Scryber.Styles.Selectors;

namespace Scryber.Core.UnitTests.Html.CSSParsers
{
    /// <summary>
    /// Tests for the CSS structural pseudo-class family: the An+B micro-syntax parser,
    /// the StructuralPseudoClass matching formula, and StyleMatcher's selector parsing.
    /// </summary>
    [TestClass()]
    public class CSSNthChild_Tests
    {
        // -----------------------------------------------------------------------
        // Section 1: NthExpressionParser
        // -----------------------------------------------------------------------

        #region NthExpressionParser

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_Odd()
        {
            Assert.IsTrue(NthExpressionParser.TryParse("odd", out int a, out int b));
            Assert.AreEqual(2, a);
            Assert.AreEqual(1, b);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_Even()
        {
            Assert.IsTrue(NthExpressionParser.TryParse("even", out int a, out int b));
            Assert.AreEqual(2, a);
            Assert.AreEqual(0, b);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_OddEven_CaseInsensitive()
        {
            Assert.IsTrue(NthExpressionParser.TryParse("ODD", out int a, out int b));
            Assert.AreEqual(2, a);
            Assert.AreEqual(1, b);

            Assert.IsTrue(NthExpressionParser.TryParse("Even", out a, out b));
            Assert.AreEqual(2, a);
            Assert.AreEqual(0, b);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_ZeroInteger()
        {
            Assert.IsTrue(NthExpressionParser.TryParse("0", out int a, out int b));
            Assert.AreEqual(0, a);
            Assert.AreEqual(0, b);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_PlainInteger()
        {
            Assert.IsTrue(NthExpressionParser.TryParse("5", out int a, out int b));
            Assert.AreEqual(0, a);
            Assert.AreEqual(5, b);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_CoefficientOnly()
        {
            Assert.IsTrue(NthExpressionParser.TryParse("2n", out int a, out int b));
            Assert.AreEqual(2, a);
            Assert.AreEqual(0, b);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_CoefficientPlusOffset()
        {
            Assert.IsTrue(NthExpressionParser.TryParse("2n+1", out int a, out int b));
            Assert.AreEqual(2, a);
            Assert.AreEqual(1, b);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_CoefficientMinusOffset()
        {
            Assert.IsTrue(NthExpressionParser.TryParse("2n-1", out int a, out int b));
            Assert.AreEqual(2, a);
            Assert.AreEqual(-1, b);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_NegativeCoefficientPlusOffset()
        {
            Assert.IsTrue(NthExpressionParser.TryParse("-n+3", out int a, out int b));
            Assert.AreEqual(-1, a);
            Assert.AreEqual(3, b);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_BareN()
        {
            Assert.IsTrue(NthExpressionParser.TryParse("n", out int a, out int b));
            Assert.AreEqual(1, a);
            Assert.AreEqual(0, b);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_NegativeBareN()
        {
            Assert.IsTrue(NthExpressionParser.TryParse("-n", out int a, out int b));
            Assert.AreEqual(-1, a);
            Assert.AreEqual(0, b);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_WhitespaceVariants()
        {
            Assert.IsTrue(NthExpressionParser.TryParse("2n + 1", out int a, out int b));
            Assert.AreEqual(2, a);
            Assert.AreEqual(1, b);

            Assert.IsTrue(NthExpressionParser.TryParse("  2n  -  1  ", out a, out b));
            Assert.AreEqual(2, a);
            Assert.AreEqual(-1, b);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void NthExpression_Invalid_ReturnsFalse()
        {
            Assert.IsFalse(NthExpressionParser.TryParse("abc", out _, out _));
            Assert.IsFalse(NthExpressionParser.TryParse("n+", out _, out _));
            Assert.IsFalse(NthExpressionParser.TryParse("", out _, out _));
            Assert.IsFalse(NthExpressionParser.TryParse(null, out _, out _));
        }

        #endregion

        // -----------------------------------------------------------------------
        // Section 2: StructuralPseudoClass matching formula
        // -----------------------------------------------------------------------

        #region StructuralPseudoClass

        private class StubSiblingProvider : ISiblingIndexProvider
        {
            public int SiblingIndex { get; set; }
            public int SiblingCount { get; set; }
            public int SiblingOfTypeIndex { get; set; }
            public int SiblingOfTypeCount { get; set; }
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Structural_NthChild_OddMatchesOddPositions()
        {
            var pseudo = new StructuralPseudoClass(StructuralPseudoClassKind.NthChild, 2, 1);

            for (int i = 1; i <= 6; i++)
            {
                var provider = new StubSiblingProvider { SiblingIndex = i, SiblingCount = 6 };
                bool expected = (i % 2) == 1;
                Assert.AreEqual(expected, pseudo.IsMatch(provider), $"Index {i} odd-match mismatch");
            }
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Structural_NthChild_EvenMatchesEvenPositions()
        {
            var pseudo = new StructuralPseudoClass(StructuralPseudoClassKind.NthChild, 2, 0);

            for (int i = 1; i <= 6; i++)
            {
                var provider = new StubSiblingProvider { SiblingIndex = i, SiblingCount = 6 };
                bool expected = (i % 2) == 0;
                Assert.AreEqual(expected, pseudo.IsMatch(provider), $"Index {i} even-match mismatch");
            }
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Structural_NthChild_ExactSingleIndex()
        {
            //:nth-child(3) => a=0, b=3, matches only index 3
            var pseudo = new StructuralPseudoClass(StructuralPseudoClassKind.NthChild, 0, 3);

            Assert.IsFalse(pseudo.IsMatch(new StubSiblingProvider { SiblingIndex = 2, SiblingCount = 5 }));
            Assert.IsTrue(pseudo.IsMatch(new StubSiblingProvider { SiblingIndex = 3, SiblingCount = 5 }));
            Assert.IsFalse(pseudo.IsMatch(new StubSiblingProvider { SiblingIndex = 4, SiblingCount = 5 }));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Structural_FirstChild()
        {
            var pseudo = new StructuralPseudoClass(StructuralPseudoClassKind.FirstChild);

            Assert.IsTrue(pseudo.IsMatch(new StubSiblingProvider { SiblingIndex = 1, SiblingCount = 4 }));
            Assert.IsFalse(pseudo.IsMatch(new StubSiblingProvider { SiblingIndex = 2, SiblingCount = 4 }));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Structural_LastChild()
        {
            var pseudo = new StructuralPseudoClass(StructuralPseudoClassKind.LastChild);

            Assert.IsTrue(pseudo.IsMatch(new StubSiblingProvider { SiblingIndex = 4, SiblingCount = 4 }));
            Assert.IsFalse(pseudo.IsMatch(new StubSiblingProvider { SiblingIndex = 3, SiblingCount = 4 }));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Structural_OnlyChild()
        {
            var pseudo = new StructuralPseudoClass(StructuralPseudoClassKind.OnlyChild);

            Assert.IsTrue(pseudo.IsMatch(new StubSiblingProvider { SiblingIndex = 1, SiblingCount = 1 }));
            Assert.IsFalse(pseudo.IsMatch(new StubSiblingProvider { SiblingIndex = 1, SiblingCount = 2 }));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Structural_NthLastChild()
        {
            //:nth-last-child(1) is equivalent to :last-child
            var pseudo = new StructuralPseudoClass(StructuralPseudoClassKind.NthLastChild, 0, 1);

            Assert.IsTrue(pseudo.IsMatch(new StubSiblingProvider { SiblingIndex = 5, SiblingCount = 5 }));
            Assert.IsFalse(pseudo.IsMatch(new StubSiblingProvider { SiblingIndex = 4, SiblingCount = 5 }));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Structural_OfTypeFamily_UsesOfTypeCounts()
        {
            var firstOfType = new StructuralPseudoClass(StructuralPseudoClassKind.FirstOfType);
            var lastOfType = new StructuralPseudoClass(StructuralPseudoClassKind.LastOfType);
            var nthOfType = new StructuralPseudoClass(StructuralPseudoClassKind.NthOfType, 2, 1);

            //Overall position (SiblingIndex/Count) deliberately differs from of-type position,
            //to confirm the of-type family reads the of-type properties, not the plain ones.
            var provider = new StubSiblingProvider
            {
                SiblingIndex = 4,
                SiblingCount = 6,
                SiblingOfTypeIndex = 1,
                SiblingOfTypeCount = 3
            };

            Assert.IsTrue(firstOfType.IsMatch(provider));
            Assert.IsFalse(lastOfType.IsMatch(provider));
            Assert.IsTrue(nthOfType.IsMatch(provider)); //odd of-type index (1)
        }

        #endregion

        // -----------------------------------------------------------------------
        // Section 3: StyleMatcher selector parsing
        // -----------------------------------------------------------------------

        #region StyleMatcher parsing

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Parse_NthChild_ProducesStructuralSelector()
        {
            var matcher = StyleMatcher.Parse("li:nth-child(2n+1)");
            Assert.IsNotNull(matcher);
            Assert.IsNotNull(matcher.Selector.AppliedStructural);
            Assert.AreEqual(StructuralPseudoClassKind.NthChild, matcher.Selector.AppliedStructural.Kind);
            Assert.AreEqual(2, matcher.Selector.AppliedStructural.A);
            Assert.AreEqual(1, matcher.Selector.AppliedStructural.B);
            Assert.AreEqual("li", matcher.Selector.AppliedElement);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Parse_NthChild_Odd()
        {
            //Note: a pseudo-class alone with no preceding type/class/id (e.g. bare ":nth-child(odd)")
            //is not matched - this is a pre-existing limitation shared with :hover/:before/:after,
            //not something introduced or fixed by this change.
            var matcher = StyleMatcher.Parse("li:nth-child(odd)");
            Assert.IsNotNull(matcher.Selector.AppliedStructural);
            Assert.AreEqual(2, matcher.Selector.AppliedStructural.A);
            Assert.AreEqual(1, matcher.Selector.AppliedStructural.B);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Parse_FirstChild()
        {
            var matcher = StyleMatcher.Parse("p:first-child");
            Assert.IsNotNull(matcher.Selector.AppliedStructural);
            Assert.AreEqual(StructuralPseudoClassKind.FirstChild, matcher.Selector.AppliedStructural.Kind);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Parse_LastChild()
        {
            var matcher = StyleMatcher.Parse("p:last-child");
            Assert.AreEqual(StructuralPseudoClassKind.LastChild, matcher.Selector.AppliedStructural.Kind);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Parse_OnlyChild()
        {
            var matcher = StyleMatcher.Parse("p:only-child");
            Assert.AreEqual(StructuralPseudoClassKind.OnlyChild, matcher.Selector.AppliedStructural.Kind);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Parse_NthOfTypeFamily()
        {
            Assert.AreEqual(StructuralPseudoClassKind.FirstOfType, StyleMatcher.Parse("p:first-of-type").Selector.AppliedStructural.Kind);
            Assert.AreEqual(StructuralPseudoClassKind.LastOfType, StyleMatcher.Parse("p:last-of-type").Selector.AppliedStructural.Kind);
            Assert.AreEqual(StructuralPseudoClassKind.OnlyOfType, StyleMatcher.Parse("p:only-of-type").Selector.AppliedStructural.Kind);

            var nth = StyleMatcher.Parse("p:nth-of-type(3n)");
            Assert.AreEqual(StructuralPseudoClassKind.NthOfType, nth.Selector.AppliedStructural.Kind);
            Assert.AreEqual(3, nth.Selector.AppliedStructural.A);
            Assert.AreEqual(0, nth.Selector.AppliedStructural.B);

            var nthLast = StyleMatcher.Parse("p:nth-last-of-type(2)");
            Assert.AreEqual(StructuralPseudoClassKind.NthLastOfType, nthLast.Selector.AppliedStructural.Kind);
            Assert.AreEqual(0, nthLast.Selector.AppliedStructural.A);
            Assert.AreEqual(2, nthLast.Selector.AppliedStructural.B);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Parse_CaseInsensitive()
        {
            var matcher = StyleMatcher.Parse("li:NTH-CHILD(2n+1)");
            Assert.IsNotNull(matcher.Selector.AppliedStructural);
            Assert.AreEqual(StructuralPseudoClassKind.NthChild, matcher.Selector.AppliedStructural.Kind);
            Assert.AreEqual(2, matcher.Selector.AppliedStructural.A);
            Assert.AreEqual(1, matcher.Selector.AppliedStructural.B);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Parse_UnknownPseudoClass_Unaffected_NoRegression()
        {
            //A genuinely unrecognised pseudo-class is absorbed into the element-name buffer
            //today (pre-existing behaviour, unrelated to this change) - AppliedState never
            //reaches Unknown because the char-scan loop never breaks out for it.
            var matcher = StyleMatcher.Parse("a:visited");
            Assert.IsNull(matcher.Selector.AppliedStructural);
            Assert.AreEqual(ComponentState.Normal, matcher.Selector.AppliedState);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Parse_MalformedNthArgument_FallsBackToUnknown()
        {
            //Recognised as a structural pseudo-class by prefix, but the argument doesn't parse -
            //should degrade safely to Unknown (matches nothing) rather than throwing or matching everything.
            var matcher = StyleMatcher.Parse("li:nth-child(garbage)");
            Assert.IsNull(matcher.Selector.AppliedStructural);
            Assert.AreEqual(ComponentState.Unknown, matcher.Selector.AppliedState);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Parse_ExistingHoverStillWorks_NoRegression()
        {
            var matcher = StyleMatcher.Parse("a:hover");
            Assert.IsNull(matcher.Selector.AppliedStructural);
            Assert.AreEqual(ComponentState.Over, matcher.Selector.AppliedState);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Parse_ClassAndPseudoClass_SameCompound()
        {
            //A class and a structural pseudo-class on the same compound selector.
            var matcher = StyleMatcher.Parse(".class:first-of-type");
            var selector = matcher.Selector;

            Assert.IsNotNull(selector.AppliedClass);
            Assert.AreEqual("class", selector.AppliedClass.ClassName);
            Assert.IsNotNull(selector.AppliedStructural);
            Assert.AreEqual(StructuralPseudoClassKind.FirstOfType, selector.AppliedStructural.Kind);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Parse_ClassPseudoClass_AsDescendantAncestor()
        {
            //The structural pseudo-class sits on the ANCESTOR half of a descendant combinator,
            //not on the target/leaf selector - confirms the ancestor chain carries it correctly.
            var matcher = StyleMatcher.Parse(".class:first-of-type .sub-class");
            var target = matcher.Selector;

            Assert.IsNotNull(target.AppliedClass);
            Assert.AreEqual("sub-class", target.AppliedClass.ClassName);
            Assert.IsNull(target.AppliedStructural, "The structural pseudo-class belongs to the ancestor, not the target");

            Assert.IsTrue(target.HasAncestor);
            var ancestor = target.Ancestor;
            Assert.IsNotNull(ancestor.AppliedClass);
            Assert.AreEqual("class", ancestor.AppliedClass.ClassName);
            Assert.IsNotNull(ancestor.AppliedStructural);
            Assert.AreEqual(StructuralPseudoClassKind.FirstOfType, ancestor.AppliedStructural.Kind);
        }

        #endregion

        // -----------------------------------------------------------------------
        // Section 4: Specificity
        // -----------------------------------------------------------------------

        #region Specificity

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Priority_StructuralPseudoClass_WeighsSameAsClass()
        {
            var withStructural = StyleMatcher.Parse("li:first-child").Selector;
            var withClass = StyleMatcher.Parse("li.some-class").Selector;
            var plain = StyleMatcher.Parse("li").Selector;

            Assert.AreEqual(withClass.Priority, withStructural.Priority,
                "A structural pseudo-class should weigh the same as a class in specificity");
            Assert.IsTrue(withStructural.Priority > plain.Priority,
                "A structural pseudo-class should increase specificity over a plain element selector");
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-NthChild")]
        public void Priority_HoverPseudoClass_NowWeighsSameAsClass()
        {
            //Confirms the specificity fix also applies to the pre-existing state pseudo-classes.
            var withHover = StyleMatcher.Parse("a:hover").Selector;
            var withClass = StyleMatcher.Parse("a.some-class").Selector;

            Assert.AreEqual(withClass.Priority, withHover.Priority);
        }

        #endregion
    }
}
