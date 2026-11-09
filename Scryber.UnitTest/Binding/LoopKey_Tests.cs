using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Binding
{
    /// <summary>
    /// Tests the @key variable for {{#each}} over dictionaries, json objects and collections, and that it is restored after nested loops.
    /// </summary>
    [TestClass]
    public class LoopKey_Tests
    {
        private static string Render(string body, object model, string pi = "")
        {
            var src = pi + "<html xmlns='http://www.w3.org/1999/xhtml'><body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                doc.Params["model"] = model;
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                return Generation.PanelContent_Tests.TextOf(ms.ToArray());
            }
        }

        private static Dictionary<string, int> Letters()
        {
            return new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } };
        }

        [TestMethod]
        public void Key_Dictionary_IteratesEntries()
        {
            var text = Render("{{#each model.letters}}<p>{{@key}}={{this}}</p>{{/each}}", new { letters = Letters() });
            Assert.AreEqual("a=1b=2c=3 ", text);
        }

        [TestMethod]
        public void Key_Dictionary_WithIndexFirstAndLast()
        {
            var text = Render("{{#each model.letters}}<p>{{@index}}:{{@key}}:{{@first}}:{{@last}}</p>{{/each}}", new { letters = Letters() });
            Assert.AreEqual("0:a:True:False1:b:False:False2:c:False:True ", text);
        }

        [TestMethod]
        public void Key_Dictionary_SingleEntry()
        {
            var text = Render("{{#each model.letters}}<p>{{@key}}:{{@first}}:{{@last}}</p>{{/each}}", new { letters = new Dictionary<string, int> { { "only", 1 } } });
            Assert.AreEqual("only:True:True ", text);
        }

        [TestMethod]
        public void Key_Dictionary_Empty_NoOutput()
        {
            var text = Render("{{#each model.letters}}<p>{{@key}}</p>{{/each}}<p>done</p>", new { letters = new Dictionary<string, int>() });
            Assert.AreEqual("done ", text);
        }

        [TestMethod]
        public void Key_Dictionary_IntKeys()
        {
            var text = Render("{{#each model.nums}}<p>{{@key}}={{this}}</p>{{/each}}", new { nums = new Dictionary<int, string> { { 10, "ten" }, { 20, "twenty" } } });
            Assert.AreEqual("10=ten20=twenty ", text);
        }

        [TestMethod]
        public void Key_OutsideLoop_IsEmpty_BeforeAndAfter()
        {
            var text = Render("<p>before=[{{@key}}]</p>{{#each model.letters}}<p>{{@key}}</p>{{/each}}<p>after=[{{@key}}]</p>", new { letters = Letters() });
            Assert.AreEqual("before=[]abcafter=[] ", text);
        }

        [TestMethod]
        public void Key_NestedDictionaries_OuterKeyRestoredAfterInner()
        {
            var model = new
            {
                outer = new Dictionary<string, Dictionary<string, int>>
                {
                    { "x", new Dictionary<string, int> { { "x1", 1 }, { "x2", 2 } } },
                    { "y", new Dictionary<string, int> { { "y1", 3 } } }
                }
            };
            var text = Render("{{#each model.outer}}<p>before=[{{@key}}]</p>{{#each this}}<p>in=[{{@key}}:{{@last}}]</p>{{/each}}<p>after=[{{@key}}:{{@last}}]</p>{{/each}}", model);
            Assert.AreEqual("before=[x]in=[x1:False]in=[x2:True]after=[x:False]before=[y]in=[y1:True]after=[y:True] ", text);
        }

        [TestMethod]
        public void Key_ArrayInsideDictionary_InnerIsIndex_OuterRestored()
        {
            var model = new { outer = new Dictionary<string, string[]> { { "p", new[] { "i", "j" } }, { "q", new[] { "k" } } } };
            var text = Render("{{#each model.outer}}<p>[{{@key}}]</p>{{#each this}}<p>{{@key}}={{this}}</p>{{/each}}<p>after=[{{@key}}]</p>{{/each}}", model);
            Assert.AreEqual("[p]0=i1=jafter=[p][q]0=kafter=[q] ", text);
        }

        [TestMethod]
        public void Key_DictionaryInsideArray_InnerIsKey_OuterIndexRestored()
        {
            var model = new { rows = new[] { new { name = "r1", cells = Letters() }, new { name = "r2", cells = new Dictionary<string, int> { { "z", 9 } } } } };
            var text = Render("{{#each model.rows}}<p>[{{@key}}:{{this.name}}]</p>{{#each this.cells}}<p>{{@key}}</p>{{/each}}<p>after=[{{@key}}:{{@index}}]</p>{{/each}}", model);
            Assert.AreEqual("[0:r1]abcafter=[0:0][1:r2]zafter=[1:1] ", text);
        }

        [TestMethod]
        public void Key_ThreeLevels_EachRestoredInTurn()
        {
            var model = new
            {
                l1 = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>
                {
                    { "A", new Dictionary<string, Dictionary<string, int>> { { "B", new Dictionary<string, int> { { "C", 1 } } } } }
                }
            };
            var text = Render("{{#each model.l1}}<p>1:{{@key}}</p>{{#each this}}<p>2:{{@key}}</p>{{#each this}}<p>3:{{@key}}</p>{{/each}}<p>2after:{{@key}}</p>{{/each}}<p>1after:{{@key}}</p>{{/each}}<p>end:[{{@key}}]</p>", model);
            Assert.AreEqual("1:A2:B3:C2after:B1after:Aend:[] ", text);
        }

        [TestMethod]
        public void Key_Array_IsIndex()
        {
            var text = Render("{{#each model.items}}<p>{{@key}}:{{this}}</p>{{/each}}", new { items = new[] { "x", "y" } });
            Assert.AreEqual("0:x1:y ", text);
        }

        [TestMethod]
        public void Key_InIfExpression()
        {
            var text = Render("{{#each model.letters}}<p>{{if(@key == 'b', 'BEE', @key)}}</p>{{/each}}", new { letters = Letters() });
            Assert.AreEqual("aBEEc ", text);
        }

        [TestMethod]
        public void Key_InIfHelper()
        {
            var text = Render("{{#each model.letters}}{{#if @key == 'c'}}<p>last is c</p>{{/if}}{{/each}}", new { letters = Letters() });
            Assert.AreEqual("last is c ", text);
        }

        [TestMethod]
        public void Key_Parent_InDictionaryEach_IsOuterContext()
        {
            var text = Render("{{#with model}}{{#each letters}}<p>{{../title}}:{{@key}}</p>{{/each}}{{/with}}", new { title = "T", letters = Letters() });
            Assert.AreEqual("T:aT:bT:c ", text);
        }

        [TestMethod]
        public void Key_NewtonsoftJObject()
        {
            var model = new { obj = Newtonsoft.Json.Linq.JObject.Parse("{\"first\":1,\"second\":2}") };
            var text = Render("{{#each model.obj}}<p>{{@key}}={{this}}</p>{{/each}}", model);
            Assert.AreEqual("first=1second=2 ", text);
        }

        [TestMethod]
        public void Key_SystemTextJsonObject()
        {
            using (var jdoc = System.Text.Json.JsonDocument.Parse("{\"alpha\":\"A\",\"beta\":\"B\"}"))
            {
                var model = new { obj = jdoc.RootElement.Clone() };
                var text = Render("{{#each model.obj}}<p>{{@key}}={{this}}</p>{{/each}}", model);
                Assert.AreEqual("alpha=Abeta=B ", text);
            }
        }

        [TestMethod]
        public void Key_Strict_Dictionary()
        {
            var text = Render("{{#each model.letters}}<p>{{@key}}={{this}}</p>{{/each}}", new { letters = Letters() }, "<?scryber parser-mode='Strict' ?>");
            Assert.AreEqual("a=1b=2c=3 ", text);
        }

        [TestMethod]
        public void TemplateElement_WithDictionary_StillBindsOnce()
        {
            //Only the handlebars each iterates entries, the template element binds the object once (as before).
            var text = Render("<template data-bind=\"{{model.single}}\"><p>[{{this.a}}]</p></template>", new { single = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } } });
            Assert.AreEqual("[1] ", text);
        }

        [TestMethod]
        public void Key_ObjectValues_PropertiesAvailable()
        {
            var model = new { people = new Dictionary<string, object> { { "ann", new { age = 30 } }, { "bob", new { age = 40 } } } };
            var text = Render("{{#each model.people}}<p>{{@key}} is {{this.age}}</p>{{/each}}", model);
            Assert.AreEqual("ann is 30bob is 40 ", text);
        }
    }
}
