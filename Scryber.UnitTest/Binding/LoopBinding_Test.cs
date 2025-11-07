using System;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Data;

namespace Scryber.Core.UnitTests.Binding
{



    [TestClass()]
    public class LoopBinding_Test
    {

        public TestContext TextContext
        {
            get;
            set;
        }


        public LoopBinding_Test()
        {
        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void LoopBinding_IndividualItems()
        {

            var controllerType = "Scryber.Core.UnitTests.Mocks.MockControllerClass, Scryber.UnitTests";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Looping Document</title>
    </head>
    <body style='padding: 10pt' >
        <ol id='list'>
            <template data-bind='{{model.items}}' >
            <li>{{.name}}</li>
            </template>
        </ol>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                
                var data = new
                {
                    items = new [] {
                        new { name = "First Item" },
                        new { name = "Second Item" },
                        new { name = "Third Item" }
                    }
                };
                doc.Params["model"] = data;
                
                using (var stream = DocStreams.GetOutputStream("LoopBinding_individualItems.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var comp = doc.FindAComponentById("list");
                Assert.IsNotNull(comp);
                var list = comp as Scryber.Components.ListOrdered;
                Assert.IsNotNull(list);
                Assert.AreEqual(data.items.Length, list.Items.Count);
                for (var i = 0; i < data.items.Length; i++)
                {
                    var item = list.Items[i];
                    var d = data.items[i];
                    
                    Assert.AreEqual(1, item.Contents.Count);
                    var literal = item.Contents[0] as TextLiteral;
                    Assert.IsNotNull(literal);
                    Assert.AreEqual(d.name, literal.Text);
                }
            }
            
        }
        
        
        [TestMethod()]
        [TestCategory("Binding")]
        public void LoopBinding_NestedItems()
        {

            var controllerType = "Scryber.Core.UnitTests.Mocks.MockControllerClass, Scryber.UnitTests";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Looping Document</title>
    </head>
    <body style='padding: 10pt' >
        <ol id='list'>
            {{#each model.items }}
            <li>{{.name}}
                <ul id='inner' >
                {{#each .inner }}<li>{{.name}}</li>{{/each}}
                </ul>
            </li>
            {{/each }}
        </ol>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                
                var data = new
                {
                    items = new [] {
                        new { 
                            name = "First Item", inner = new []
                            {
                                new { name = "First Inner 1"},
                                new { name = "First Inner 2"},
                                new { name = "First Inner 3"}
                            } 
                        },
                        new { name = "Second Item",  inner = new []
                            {
                                new { name = "Second Inner 1"},
                                new { name = "Second Inner 2"},
                                new { name = "Second Inner 3"}
                            }  
                        },
                        new { name = "Third Item", inner = new []
                            {
                            new { name = "Third Inner 1"},
                            new { name = "Third Inner 2"},
                            new { name = "Third Inner 3"}
                            } 
                        }
                    }
                };
                doc.Params["model"] = data;
                
                using (var stream = DocStreams.GetOutputStream("LoopBinding_nestedItems.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var comp = doc.FindAComponentById("list");
                Assert.IsNotNull(comp);
                var list = comp as Scryber.Components.ListOrdered;
                Assert.IsNotNull(list);
                Assert.AreEqual(data.items.Length, list.Items.Count);
                for (var i = 0; i < data.items.Length; i++)
                {
                    var item = list.Items[i];
                    var d = data.items[i];
                    
                    Assert.AreEqual(4, item.Contents.Count); //2 literals (one is empty)
                    var literal = item.Contents[0] as TextLiteral;
                    Assert.IsNotNull(literal);
                    Assert.AreEqual(d.name, literal.Text);

                    var inner = item.Contents[2] as ListUnordered;
                    Assert.IsNotNull(inner);
                    
                    Assert.AreEqual(3, inner.Items.Count);
                }
            }
            
        }
        
        
         [TestMethod()]
        [TestCategory("Binding")]
        public void LoopBinding_NestedChoiceItems()
        {

            var controllerType = "Scryber.Core.UnitTests.Mocks.MockControllerClass, Scryber.UnitTests";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Looping Document</title>
    </head>
    <body style='padding: 10pt' >
        <ol id='list'>
            {{#each model.items }}
            <li>{{.name}}
                {{#if .innerVisible }}
                <ul id='inner' >
                {{#each .inner }}<li>{{.name}}</li>{{/each}}
                </ul>
                {{/if}}
            </li>
            {{/each }}
        </ol>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                
                var data = new
                {
                    items = new [] {
                        new { 
                            name = "First Item",
                            innerVisible = true,
                            inner = new []
                            {
                                new { name = "First Inner 1"},
                                new { name = "First Inner 2"},
                                new { name = "First Inner 3"}
                            } 
                        },
                        new { name = "Second Item",
                            innerVisible = false,
                            inner = new []
                            {
                                new { name = "Second Inner 1"},
                                new { name = "Second Inner 2"},
                                new { name = "Second Inner 3"}
                            }  
                        },
                        new { name = "Third Item",
                            innerVisible = true,
                            inner = new []
                            {
                            new { name = "Third Inner 1"},
                            new { name = "Third Inner 2"},
                            new { name = "Third Inner 3"}
                            } 
                        }
                    }
                };
                doc.Params["model"] = data;
                
                using (var stream = DocStreams.GetOutputStream("LoopBinding_NestedChoiceItems.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var comp = doc.FindAComponentById("list");
                Assert.IsNotNull(comp);
                var list = comp as Scryber.Components.ListOrdered;
                Assert.IsNotNull(list);
                Assert.AreEqual(data.items.Length, list.Items.Count);
                for (var i = 0; i < data.items.Length; i++)
                {
                    var item = list.Items[i];
                    var d = data.items[i];

                    if (d.innerVisible)
                    {
                        Assert.AreEqual(5, item.Contents.Count);
                        var inner = item.Contents[2] as TemplateInstance; //Contains the inner list
                        Assert.IsNotNull(inner);
                        Assert.AreEqual(3, inner.Content.Count); // whitespace, list, whitespace
                        var innerlist = inner.Content[1] as ListUnordered;
                        Assert.IsNotNull(innerlist);
                        Assert.AreEqual(3, innerlist.Items.Count);
                    }
                    else
                    {
                        Assert.AreEqual(4, item.Contents.Count); //2 literals name and whitespace, Choice and whitespace.
                    }

                    var literal = item.Contents[0] as TextLiteral;
                    Assert.IsNotNull(literal);
                    Assert.AreEqual(d.name, literal.Text);
                }
            }
            
        }
        
        
        
         [TestMethod()]
        [TestCategory("Binding")]
        public void LoopBinding_NestedChoiceElseItems()
        {
            

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Looping Document</title>
    </head>
    <body style='padding: 10pt' >
        <ol id='list'>
            {{#each model.items }}
            <li>{{.name}}
                {{#if .innerType == 'ordered' }}
                <ol id='inner' >
                {{#each .inner }}<li>{{.name}}</li>{{/each}}
                </ol>
                {{else if .innerType == 'unordered' }}
                <ul id='inner' >
                {{#each .inner }}<li>{{.name}}</li>{{/each}}
                </ul>
                {{else}}
                {{#each .inner }}<div>{{.name}}</div>{{/each}}
                {{/if}}
            </li>
            {{/each }}
        </ol>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                
                var data = new
                {
                    items = new [] {
                        new { 
                            name = "First Item",
                            innerType = "",
                            inner = new []
                            {
                                new { name = "First Inner 1"},
                                new { name = "First Inner 2"},
                                new { name = "First Inner 3"}
                            } 
                        },
                        new { name = "Second Item",
                            innerType = "ordered",
                            inner = new []
                            {
                                new { name = "Second Inner 1"},
                                new { name = "Second Inner 2"},
                                new { name = "Second Inner 3"}
                            }  
                        },
                        new { name = "Third Item",
                            innerType = "unordered",
                            inner = new []
                            {
                            new { name = "Third Inner 1"},
                            new { name = "Third Inner 2"},
                            new { name = "Third Inner 3"}
                            } 
                        }
                    }
                };
                doc.Params["model"] = data;
                
                using (var stream = DocStreams.GetOutputStream("LoopBinding_NestedChoiceElseItems.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var comp = doc.FindAComponentById("list");
                Assert.IsNotNull(comp);
                var list = comp as Scryber.Components.ListOrdered;
                Assert.IsNotNull(list);
                Assert.AreEqual(data.items.Length, list.Items.Count);
                for (var i = 0; i < data.items.Length; i++)
                {
                    var item = list.Items[i];
                    var d = data.items[i];

                    Assert.AreEqual(5, item.Contents.Count);
                    
                    var literal = item.Contents[0] as TextLiteral;
                    Assert.IsNotNull(literal);
                    Assert.AreEqual(d.name, literal.Text);
                    
                    var inner = item.Contents[2] as TemplateInstance; //Contains the inner items
                    Assert.IsNotNull(inner);
                    
                    
                    if (d.innerType == "unordered")
                    {
                        Assert.AreEqual(3, inner.Content.Count); // whitespace, list, whitespace
                        var innerlist = inner.Content[1] as ListUnordered;
                        Assert.IsNotNull(innerlist);
                        Assert.AreEqual(3, innerlist.Items.Count);
                    }
                    else if (d.innerType == "ordered")
                    {
                        Assert.AreEqual(3, inner.Content.Count); // whitespace, list, whitespace
                        var innerlist = inner.Content[1] as ListOrdered;
                        Assert.IsNotNull(innerlist);
                        Assert.AreEqual(3, innerlist.Items.Count);
                    }
                    else
                    {
                        Assert.AreEqual(6, inner.Content.Count); // whitespace, 3 templates, the choice, whitespace
                    }

                    
                }
            }
            
        }



    }
}
