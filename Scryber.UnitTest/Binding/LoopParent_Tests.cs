using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Binding
{
    [TestClass]
    public class LoopParent_Tests
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

        private static readonly object Model = new { title = "T", groups = new[] { new { name = "G1", items = new[] { "i1", "i2" } }, new { name = "G2", items = new[] { "i3" } } } };

        [TestMethod]
        public void Parent_InNestedEach_IsOuterItem()
        {
            var text = Render("{{#each model.groups}}{{#each this.items}}<p>[{{../name}}:{{this}}]</p>{{/each}}{{/each}}", Model);
            Assert.AreEqual("[G1:i1][G1:i2][G2:i3] ", text);
        }

        [TestMethod]
        public void Parent_InEachWithinWith_IsWithObject()
        {
            var text = Render("{{#with model}}{{#each groups}}<p>[{{../title}}:{{this.name}}]</p>{{/each}}{{/with}}", Model);
            Assert.AreEqual("[T:G1][T:G2] ", text);
        }

        [TestMethod]
        public void Parent_InTopLevelEach_HasNoOuterContext()
        {
            var text = Render("{{#each model.groups}}<p>[{{../title}}]</p>{{/each}}", Model);
            Assert.AreEqual("[][] ", text);
        }

        [TestMethod]
        public void Parent_InWith_StillWorks()
        {
            var text = Render("{{#with model}}{{#with groups[0]}}<p>W=[{{../title}}]</p>{{/with}}{{/with}}", Model);
            Assert.AreEqual("W=[T] ", text);
        }

        [TestMethod]
        public void FirstAndLast_InEach_Output()
        {
            var text = Render("{{#each model.groups}}<p>{{this.name}}:{{@first}}:{{@last}}:{{@index}}</p>{{/each}}", Model);
            Assert.AreEqual("G1:True:False:0G2:False:True:1 ", text);
        }

        [TestMethod]
        public void FirstAndLast_InIf()
        {
            var text = Render("{{#each model.groups}}{{#if @first}}<p>first</p>{{/if}}{{#if @last}}<p>last</p>{{/if}}<p>{{this.name}}</p>{{/each}}", Model);
            Assert.AreEqual("firstG1lastG2 ", text);
        }

        [TestMethod]
        public void FirstAndLast_SingleItem_IsBoth()
        {
            var text = Render("{{#each model.groups}}<p>{{@first}}:{{@last}}</p>{{/each}}", new { groups = new[] { "only" } });
            Assert.AreEqual("True:True ", text);
        }

        [TestMethod]
        public void FirstAndLast_Nested_RestoredAfterInnerLoop()
        {
            var text = Render("{{#each model.groups}}{{#each this.items}}<p>{{@last}}</p>{{/each}}<p>{{this.name}}:{{@last}}</p>{{/each}}", Model);
            Assert.AreEqual("FalseTrueG1:FalseTrueG2:True ", text);
        }

        [TestMethod]
        public void FirstAndLast_LinqIterator_Strict()
        {
            var linq = new List<string> { "a", "b", "c" }.Select(n => new { name = n });
            var text = Render("{{#each model.items}}<p>{{this.name}}{{if(@last, '.', ',')}}</p>{{/each}}", new { items = linq }, "<?scryber parser-mode='Strict' ?>");
            Assert.AreEqual("a,b,c. ", text);
        }

        [TestMethod]
        public void FirstAndLast_EmptyCollection_NoOutput()
        {
            var text = Render("{{#each model.items}}<p>{{@first}}</p>{{/each}}<p>done</p>", new { items = new string[0] });
            Assert.AreEqual("done ", text);
        }
    }
}
