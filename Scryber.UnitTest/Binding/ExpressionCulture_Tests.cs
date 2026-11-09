using System;
using System.IO;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Binding
{
    /// <summary>
    /// Numbers in strings (e.g. json or field values) are always read in the invariant culture, the same as numbers
    /// in the expression itself, whatever the culture of the machine or browser. Numbers and dates written out as text
    /// still use the current culture.
    /// </summary>
    [TestClass]
    public class ExpressionCulture_Tests
    {
        //'.' is the group separator in nl-BE, so "1.5" would be read as 15
        private const string Dutch = "nl-BE";

        private static string Render(string body, object model, string culture)
        {
            var current = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);

                var src = "<html xmlns='http://www.w3.org/1999/xhtml'><body>" + body + "</body></html>";
                using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
                {
                    doc.Params["model"] = model;
                    var ms = new MemoryStream();
                    doc.SaveAsPDF(ms);
                    return Generation.PanelContent_Tests.TextOf(ms.ToArray());
                }
            }
            finally
            {
                CultureInfo.CurrentCulture = current;
            }
        }

        private static readonly object Model = new { v = "1.5", w = "2.25" };

        [TestMethod]
        public void ExpressionCulture_01_StringNumber_Arithmetic()
        {
            Assert.AreEqual("6 ", Render("<p>{{round(model.v * 4)}}</p>", Model, Dutch), "multiply");
            Assert.AreEqual("3 ", Render("<p>{{model.v / 0.5}}</p>", Model, Dutch), "divide");
            //a string first is joined as text, so the number goes first
            Assert.AreEqual("4 ", Render("<p>{{round(2.5 + model.v)}}</p>", Model, Dutch), "add");
            Assert.AreEqual("1 ", Render("<p>{{round(model.v - 0.5)}}</p>", Model, Dutch), "subtract");
            Assert.AreEqual("9 ", Render("<p>{{round(model.v ^ 2 * 4)}}</p>", Model, Dutch), "power");
        }

        [TestMethod]
        public void ExpressionCulture_02_StringNumber_Functions()
        {
            Assert.AreEqual("2 ", Render("<p>{{round(model.v)}}</p>", Model, Dutch), "round");
            Assert.AreEqual("3 ", Render("<p>{{sqrt(model.w) * 2}}</p>", Model, Dutch), "sqrt");
            Assert.AreEqual("3 ", Render("<p>{{floor(model.v) * 3}}</p>", Model, Dutch), "floor");
            Assert.AreEqual("3 ", Render("<p>{{double(model.v) * 2}}</p>", Model, Dutch), "double");
            Assert.AreEqual("3 ", Render("<p>{{round(decimal(model.v) * 2)}}</p>", Model, Dutch), "decimal");
        }

        [TestMethod]
        public void ExpressionCulture_03_StringNumber_Comparison()
        {
            var model = new { v = "1.5", n = 9.0 };
            Assert.AreEqual("yes ", Render("<p>{{if(model.v == 1.5, 'yes', 'no')}}</p>", model, Dutch), "equals");
            Assert.AreEqual("yes ", Render("<p>{{if(model.n &gt; model.v, 'yes', 'no')}}</p>", model, Dutch), "greater than");
        }

        [TestMethod]
        public void ExpressionCulture_04_NumbersAsText_UseTheCurrentCulture()
        {
            Assert.AreEqual("1,50 ", Render("<p>{{string(1.5, 'N2')}}</p>", Model, Dutch));
            Assert.AreEqual("1.50 ", Render("<p>{{string(1.5, 'N2')}}</p>", Model, "en-GB"));
        }
    }
}
