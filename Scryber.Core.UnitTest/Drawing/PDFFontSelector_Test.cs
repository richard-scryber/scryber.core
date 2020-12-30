using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.Core.UnitTests.Drawing
{
    [TestClass]
   public  class PDFFontSelector_Test
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


        [TestMethod()]
        public void ParsingSimple()
        {
            var str = "Gill Sans, Helvetica, Arial";
            var sel = PDFFontSelector.Parse(str);

            var curr = sel;
            Assert.IsNotNull(curr);
            Assert.AreEqual("Gill Sans", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNotNull(curr);
            Assert.AreEqual("Helvetica", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNotNull(curr);
            Assert.AreEqual("Arial", curr.FamilyName);
            curr = curr.Next;

            
            Assert.IsNull(curr);
        }

        [TestMethod()]
        public void ParsingQuoted()
        {
            var str = "'Gill Sans MT', Helvetica, Arial, sans-serif";
            var sel = PDFFontSelector.Parse(str);

            var curr = sel;
            Assert.IsNotNull(curr);
            Assert.AreEqual("Gill Sans MT", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNotNull(curr);
            Assert.AreEqual("Helvetica", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNotNull(curr);
            Assert.AreEqual("Arial", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNotNull(curr);
            Assert.AreEqual("sans-serif", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNull(curr);
        }

        [TestMethod()]
        public void ParsingDoubleQuotedAndEmpty()
        {
            var str = "\"Gill Sans MT\", Helvetica,, sans-serif ";
            var sel = PDFFontSelector.Parse(str);

            var curr = sel;
            Assert.IsNotNull(curr);
            Assert.AreEqual("Gill Sans MT", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNotNull(curr);
            Assert.AreEqual("Helvetica", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNotNull(curr);
            Assert.AreEqual("sans-serif", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNull(curr);
        }

        [TestMethod]
        public void ToStringOverrides()
        {
            var str = "'Gill Sans MT', Gill Sans, Helvetica, Arial, sans-serif";
            var sel = PDFFontSelector.Parse(str);

            var result = sel.ToString();
            Assert.AreEqual("'Gill Sans MT', 'Gill Sans', Helvetica, Arial, sans-serif", result);

            str = "\"Gill Sans MT\", Helvetica,, sans-serif ";
            sel = PDFFontSelector.Parse(str);

            result = sel.ToString();
            Assert.AreEqual("'Gill Sans MT', Helvetica, sans-serif", result);
        }

        public void GetSystemFont()
        {
            var str = "'Gill Sans MT', Gill Sans, Helvetica, Arial, sans-serif";
            var sel = PDFFontSelector.Parse(str);

            var curr = sel;
            Assert.IsNotNull(curr);
            Assert.AreEqual("Gill Sans MT", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNotNull(curr);
            Assert.AreEqual("Helvetica", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNotNull(curr);
            Assert.AreEqual("Arial", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNotNull(curr);
            Assert.AreEqual("sans-serif", curr.FamilyName);
            curr = curr.Next;

            Assert.IsNull(curr);
        }

       

    }
}
