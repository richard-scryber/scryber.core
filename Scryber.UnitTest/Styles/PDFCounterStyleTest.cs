using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Drawing;
using Scryber.Text;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFTextStyleTest and is intended
    ///to contain all PDFTextStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFCounterStyleTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        const string Category = "Style Values";
        const int Undefined = ComponentCounterSet.UndefinedValue;

        /// <summary>
        ///A test for ComponentCounterSet constructor
        ///</summary>
        [TestMethod()]
        [TestCategory(Category)]
        public void CounterSet_ConstructorTest()
        {
            ComponentCounterSet set = new ComponentCounterSet(null);
            Assert.IsNull(set.Owner);
            Assert.IsNull(set.Root);
            Assert.AreEqual(0, set.Count);

            Assert.AreEqual(Undefined, set.Value("Empty"));
        }



        /// <summary>
        ///A test for ComponentCounterSet Value function
        ///</summary>
        [TestMethod()]
        [TestCategory(Category)]
        public void CounterSet_ValueTest()
        {
            ComponentCounterSet set = new ComponentCounterSet(null);
            Assert.IsNull(set.Owner);
            Assert.IsNull(set.Root);
            Assert.AreEqual(0, set.Count);

            set.Reset("Counter1");
            set.Reset("Counter2", 2);

            Assert.AreEqual(0, set.Value("Counter1"));
            Assert.AreEqual(2, set.Value("Counter2"));
        }

        /// <summary>
        ///A test for ComponentCounterSet Value function
        ///</summary>
        [TestMethod()]
        [TestCategory(Category)]
        public void CounterSet_OwnerTest()
        {
            var div = new Components.Div();
            Assert.IsFalse(div.HasCounters);

            ComponentCounterSet set = new ComponentCounterSet(div);
            Assert.AreEqual(div, set.Owner);
            Assert.IsNull(set.Root);
            Assert.AreEqual(0, set.Count);


            div = new Components.Div();
            set = div.Counters;

            Assert.IsNotNull(set);
            Assert.AreEqual(div, set.Owner);
            Assert.IsFalse(div.HasCounters);

            div.Counters.Reset("Counter1");
            Assert.IsTrue(div.HasCounters);
            Assert.AreEqual(1, div.Counters.Count);
            Assert.AreEqual(1, set.Count);

        }


        /// <summary>
        ///A test for ComponentCounterSet Increment function
        ///</summary>
        [TestMethod()]
        [TestCategory(Category)]
        public void CounterSet_IncrementTest()
        {
            ComponentCounterSet set = new ComponentCounterSet(null);
            Assert.IsNull(set.Root);
            Assert.AreEqual(0, set.Count);

            set.Reset("Counter1");
            set.Reset("Counter2", 2);

            Assert.IsNotNull(set.Root);
            Assert.AreEqual(2, set.Count);

            Assert.AreEqual(1, set.Increment("Counter1"));
            Assert.AreEqual(4, set.Increment("Counter2", 2));

            Assert.AreEqual(1, set.Value("Counter1"));
            Assert.AreEqual(4, set.Value("Counter2"));

            Assert.AreEqual(3, set.Increment("Counter1", 2));
            Assert.AreEqual(5, set.Increment("Counter2"));

            Assert.AreEqual(3, set.Value("Counter1"));
            Assert.AreEqual(5, set.Value("Counter2"));
        }

        /// <summary>
        ///A test for ComponentCounterSet Reset function
        ///</summary>
        [TestMethod()]
        [TestCategory(Category)]
        public void CounterSet_ResetTest()
        {
            ComponentCounterSet set = new ComponentCounterSet(null);
            Assert.IsNull(set.Root);
            Assert.AreEqual(0, set.Count);

            set.Reset("Counter1");
            set.Reset("Counter2", 2);

            Assert.AreEqual(1, set.Increment("Counter1"));
            Assert.AreEqual(4, set.Increment("Counter2", 2));

            Assert.AreEqual(1, set.Value("Counter1"));
            Assert.AreEqual(4, set.Value("Counter2"));

            set.Reset("Counter2");

            Assert.AreEqual(1, set.Value("Counter1"));
            Assert.AreEqual(0, set.Value("Counter2"));

            set.Reset("Counter1", 10);

            Assert.AreEqual(10, set.Value("Counter1"));
            Assert.AreEqual(0, set.Value("Counter2"));


        }

        /// <summary>
        ///A test for ComponentStyleValue constructor and properties
        ///</summary>
        [TestMethod()]
        [TestCategory(Category)]
        public void CounterStyleValues_Test()
        {
            CounterStyleValue counter = new CounterStyleValue("Counter1");
            Assert.AreEqual("Counter1", counter.Name);
            Assert.AreEqual(0, counter.Value);
            Assert.IsNull(counter.Next);

            CounterStyleValue next = new CounterStyleValue("Counter2");
            counter.Next = next;
            Assert.AreEqual(next, counter.Next);
            Assert.AreNotEqual(next, counter);

            counter.Value = 1;
            Assert.AreEqual(1, counter.Value);
            Assert.AreNotEqual(counter.Value, counter.Next.Value);
            Assert.IsNull(counter.Next.Next);

        }


        /// <summary>
        ///A test for ComponentStyleValue Parsing function
        ///</summary>
        [TestMethod()]
        [TestCategory(Category)]
        public void CounterStyleValue_Parse_Test()
        {
            var one = "Counter1";

            CounterStyleValue counter = CounterStyleValue.Parse(one);
            Assert.AreEqual("Counter1", counter.Name);
            Assert.AreEqual(0, counter.Value);
            Assert.IsNull(counter.Next);

            var two = @" Counter1
Counter2   ";

            counter = CounterStyleValue.Parse(two);
            Assert.AreEqual("Counter1", counter.Name);
            Assert.AreEqual(0, counter.Value);
            Assert.IsNotNull(counter.Next);
            Assert.AreEqual("Counter2", counter.Next.Name);
            Assert.AreEqual(0, counter.Next.Value);
            Assert.IsNull(counter.Next.Next);

            var twoWithValue = @" Counter1 3
Counter2 -1  ";

            counter = CounterStyleValue.Parse(twoWithValue);
            Assert.AreEqual("Counter1", counter.Name);
            Assert.AreEqual(3, counter.Value);
            Assert.IsNotNull(counter.Next);
            Assert.AreEqual("Counter2", counter.Next.Name);
            Assert.AreEqual(-1, counter.Next.Value);
            Assert.IsNull(counter.Next.Next);

        }


        /// <summary>
        ///A test for ComponentStyleValue storage in a style definition
        ///</summary>
        [TestMethod()]
        [TestCategory(Category)]
        public void CounterStyle_Test()
        {
            var style = new StyleDefn();

            var one = "Counter1";

            CounterStyleValue counter = CounterStyleValue.Parse(one);
            style.SetValue(StyleKeys.CounterIncrementKey, counter);

            var two = @" Counter1
Counter2 3  ";

            counter = CounterStyleValue.Parse(two);
            style.SetValue(StyleKeys.CounterResetKey, counter);

            Assert.IsNotNull(style.GetValue(StyleKeys.CounterIncrementKey, null));
            Assert.AreEqual("Counter1", style.GetValue(StyleKeys.CounterIncrementKey, null).Name);
            Assert.AreEqual(0, style.GetValue(StyleKeys.CounterIncrementKey, null).Value);
            Assert.IsNull(style.GetValue(StyleKeys.CounterIncrementKey, null).Next);


            Assert.IsNotNull(style.GetValue(StyleKeys.CounterResetKey, null));
            Assert.AreEqual("Counter1", style.GetValue(StyleKeys.CounterResetKey, null).Name);
            Assert.AreEqual(0, style.GetValue(StyleKeys.CounterResetKey, null).Value);
            Assert.IsNotNull(style.GetValue(StyleKeys.CounterResetKey, null).Next);
            Assert.AreEqual("Counter2", style.GetValue(StyleKeys.CounterResetKey, null).Next.Name);
            Assert.AreEqual(3, style.GetValue(StyleKeys.CounterResetKey, null).Next.Value);
            Assert.IsNull(style.GetValue(StyleKeys.CounterResetKey, null).Next.Next);

        }


    }
}
