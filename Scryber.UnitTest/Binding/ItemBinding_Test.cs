using System;
using System.Net.WebSockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Core.UnitTests.Mocks;

namespace Scryber.Core.UnitTests.Binding
{
    /// <summary>
    /// Tests for the item binding expression
    /// </summary>
    [TestClass()]
    public class ItemBinding_Test
    {
        public TestContext TextContext
        {
            get;
            set;
        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindLabelText()
        {
            var expected = "My Document Title";

            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
                                     >
                        <Styles>

                        <styles:Style applied-class='blue-bg'>
                            <styles:Background color='blue' />
                        </styles:Style>
    
                        </Styles>
                        <Params>
                            <doc:String-Param id='title' value='" + expected + @"' />
                        </Params>
                        <Pages>
    
                        <doc:Section>
                            <Content>
                            <doc:Label text='{@:title}'></doc:Label>
                            </Content>
                        </doc:Section>

                        </Pages>
                    </doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                var sect = doc.Pages[0] as Section;
                var label = sect.Contents[0] as Label;


                doc.InitializeAndLoad();
                doc.DataBind();

                Assert.AreEqual(expected, label.Text, "The document title does not match");
            }

            
        }

        

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindAllTypes()
        {
            var expectedString = "My Document Title";
            var expectedInt = 125;
            var expectedGuid = "{1C978E2A-E3E1-43F9-AA07-97B976B57DAA}";
            var expectedDouble = 12.34;
            var expectedBool = true;
            var expectedDate = "2020-07-03 12:24:24";
            var expectedUnit = "34pt";
            var expectedColor = "#FF3300";
            var expectedThickness = "10pt 20pt 20pt 5pt";
            var expectedEnum = "Dash";
            
            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
                                     >
                        <Params>
                            <doc:String-Param id='title' value='" + expectedString + @"' />
                            <doc:Int-Param id='int' value='" + expectedInt + @"' />
                            <doc:Guid-Param id='guid' value='" + expectedGuid + @"' />
                            <doc:Double-Param id='double' value='" + expectedDouble + @"' />
                            <doc:Bool-Param id='bool' value='" + expectedBool + @"' />
                            <doc:Date-Param id='date' value='" + expectedDate + @"' />
                            <doc:Unit-Param id='unit' value='" + expectedUnit+ @"' />
                            <doc:Color-Param id='color' value='" + expectedColor + @"' />
                            <doc:Thickness-Param id='thick' value='" + expectedThickness + @"' />
                            <doc:Enum-Param id='enum' type='Scryber.Drawing.LineType, Scryber.Drawing' value='" + expectedEnum + @"' />
                        </Params>

                        <Pages>
    
                        <doc:Section>
                            <Content>
                                <doc:Label styles:fill-color='{@:color}' styles:x='{@:unit}' styles:padding='{@:thick}'
                                           styles:border-style='{@:enum}' text='{@:title}'></doc:Label>
                                <doc:Date value='{@:date}' />
                                <doc:Number value='{@:int}' styles:font-bold='{@:bool}' />
                                <doc:Number value='{@:double}' />
                            </Content>
                        </doc:Section>

                        </Pages>
                    </doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                

                var sect = doc.Pages[0] as Section;
                var label = sect.Contents[0] as Label;
                var date = sect.Contents[1] as Date;
                var num1 = sect.Contents[2] as Number;
                var num2 = sect.Contents[3] as Number;

                doc.InitializeAndLoad();
                doc.DataBind();

                Assert.AreEqual(expectedString, label.Text, "The label text does not match");
                Assert.AreEqual(Scryber.Drawing.Color.Parse(expectedColor), label.FillColor, "The Label fill colour does not match");
                Assert.AreEqual(Scryber.Drawing.PDFUnit.Parse(expectedUnit), label.X, "The label x offsets do not match");
                Assert.AreEqual(Scryber.Drawing.PDFThickness.Parse(expectedThickness), label.Padding, "The label paddings do not match");
                Assert.AreEqual(Scryber.Drawing.LineType.Dash, label.BorderStyle, "The label border styles do not match");
                Assert.AreEqual(DateTime.Parse(expectedDate), date.Value, "The date time values do not match");
                Assert.AreEqual(expectedInt, (int)num1.Value,"The integers do not match");
                Assert.AreEqual(expectedBool, num1.FontBold, "The bool values do not match");
                Assert.AreEqual(expectedDouble, num2.Value, "The double values do not match");


            }


        }

        /// <summary>
        /// Test for binding style values to parameters
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding")]
        public void BindStyles()
        {
            var expectedString = "My Document Title";
            var expectedBool = true;
            var expectedUnit = "34pt";
            var expectedColor = "#FF3300";
            
            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
                                     >
                        <Styles>

                        <styles:Style applied-class='blue'>
                            <styles:Background color='{@:color}' />
                            <styles:Padding top='{@:unit}' />
                            <styles:Font bold='{@:bool}' />
                        </styles:Style>
    
                        </Styles>
                        <Params>
                            <doc:String-Param id='title' value='" + expectedString + @"' />
                            <doc:Bool-Param id='bool' value='" + expectedBool + @"' />
                            <doc:Unit-Param id='unit' value='" + expectedUnit + @"' />
                            <doc:Color-Param id='color' value='" + expectedColor + @"' />
                        </Params>

                        <Pages>
    
                        <doc:Section>
                            <Content>
                                <doc:Label styles:class='blue' text='{@:title}'></doc:Label>
                                
                            </Content>
                        </doc:Section>

                        </Pages>
                    </doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                

                var sect = doc.Pages[0] as Section;
                var label = sect.Contents[0] as Label;
                var style = doc.Styles[0] as Scryber.Styles.Style;

                doc.InitializeAndLoad();
                doc.DataBind();

                Assert.AreEqual(expectedString, label.Text, "The label text does not match");

                Assert.IsTrue(style.IsValueDefined(Scryber.Styles.StyleKeys.BgColorKey), "The background colour is not set");
                Assert.AreEqual(Scryber.Drawing.Color.Parse(expectedColor), style.Background.Color, "The style color does not match");

                Assert.IsTrue(style.IsValueDefined(Scryber.Styles.StyleKeys.FontWeightKey), "The font bold is not set");
                Assert.AreEqual(expectedBool, style.Font.FontBold, "The font bolds do not match");

                Assert.IsTrue(style.IsValueDefined(Scryber.Styles.StyleKeys.PaddingTopKey), "The padding top is not set");
                Assert.AreEqual(Scryber.Drawing.PDFUnit.Parse(expectedUnit), style.Padding.Top, "The padding top values do not match");

            }


        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindDynamicObject()
        {

            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
                                     >
                        <Params>
                            <doc:Object-Param id='dynamic' ></doc:Object-Param>
                        </Params>

                        <Styles>

                        <styles:Style applied-class='blue'>
                            <styles:Background color='{@:dynamic.Color}' />
                        </styles:Style>
    
                        </Styles>

                        <Pages>
    
                        <doc:Section>
                            <Content>

                                <data:ForEach value='{@:dynamic.List}' >
                                    <Template>
                                        <doc:Label id='{@:.Id}' text='{@:.Name}' ></doc:Label>
                                        <doc:Br/>
                                    </Template>
                                </data:ForEach>

                            </Content>
                        </doc:Section>

                        </Pages>
                    </doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.Params["dynamic"] = new
                {
                    Color = Scryber.Drawing.PDFColors.Aqua,
                    List = new[] {
                        new { Name = "First", Id = "FirstID"},
                        new { Name = "Second", Id = "SecondID" }
                    }
                };


                doc.InitializeAndLoad();
                doc.DataBind();

                //For the ForEach template with an object source.
                var first = doc.FindAComponentById("FirstID") as Label;
                Assert.IsNotNull(first, "Could not find the first label");
                Assert.AreEqual("First", first.Text, "The first label does not have the correct Name value");

                var second = doc.FindAComponentById("SecondID") as Label;
                Assert.IsNotNull(second, "Could not find the second label");
                Assert.AreEqual("Second", second.Text, "The second label does not have the correct Name value");

                var style = doc.Styles[0] as Scryber.Styles.Style;
                Assert.IsTrue(style.IsValueDefined(Scryber.Styles.StyleKeys.BgColorKey), "The background color is not assigned");
                Assert.AreEqual(Scryber.Drawing.PDFColors.Aqua, style.Background.Color, "The background colors do not match");
            }


        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindXmlAndTemplateObject()
        {
            var expectedXml = @"<node value='1' >
                                    <inner value='1' />
                                    <inner value='2' />
                                </node>";

            var expectedTemplate = @"<doc:Div id='{xpath:concat(""xmlInnerDiv"",@value)}' >
                                        <doc:Label id='{xpath:concat(""xmlLabel"",@value)}' text='{xpath:@value}' />
                                     </doc:Div>";

            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
                                     >

                        <Params>
                            <doc:Xml-Param id='xml' >" + expectedXml + @"</doc:Xml-Param>
                            <doc:Template-Param id='template' >" + expectedTemplate + @"</doc:Template-Param>
                        </Params>

                        <Pages>
    
                        <doc:Section>
                            <Content>
                                <data:ForEach id='Foreach2' value='{@:xml}' select='//node/inner' template='{@:template}' ></data:ForEach>
                            </Content>
                        </doc:Section>

                        </Pages>
                    </doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                doc.InitializeAndLoad();
                doc.DataBind();

                //For the ForEach template with an object source.
                var first = doc.FindAComponentById("xmlInnerDiv1") as Div;
                Assert.IsNotNull(first, "Could not find inner div");
                
                var second = doc.FindAComponentById("xmlLabel2") as Label;
                Assert.IsNotNull(second, "Could not find the second label");
                Assert.AreEqual("2", second.Text, "The second label does not have the correct text value");

            }
        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindTemplatePlaceholder()
        {
            var expectedXml = @"<node value='1' >
                                    <inner value='1' />
                                    <inner value='2' />
                                </node>";

            var expectedTemplate = @"<doc:Div id='{xpath:concat(""xmlInnerDiv"",@value)}' >
                                        <doc:Label id='{xpath:concat(""xmlLabel"",@value)}' text='{xpath:@value}' />
                                     </doc:Div>";

            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
                                     >

                        <Params>
                            <doc:Xml-Param id='xml' >" + expectedXml + @"</doc:Xml-Param>
                            <doc:Template-Param id='template' >" + expectedTemplate + @"</doc:Template-Param>
                        </Params>

                        <Pages>
    
                            <doc:Section>
                                <Content>
                                
                                    <data:ForEach id='Foreach2' value='{@:xml}' select='//node/inner' >
                                        <Template>
                                            <doc:PlaceHolder template='{@:template}' />
                                        </Template>
                                    </data:ForEach>

                                </Content>
                            </doc:Section>

                        </Pages>
                    </doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                doc.InitializeAndLoad();
                doc.DataBind();

                //For the ForEach template with an object source.
                var first = doc.FindAComponentById("xmlInnerDiv1") as Div;
                Assert.IsNotNull(first, "Could not find inner div");

                var second = doc.FindAComponentById("xmlLabel2") as Label;
                Assert.IsNotNull(second, "Could not find the second label");

                Assert.AreEqual("2", second.Text, "The second label does not have the correct text value");



            }

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                doc.Params["template"] = @"<doc:H1 id='{xpath:concat(""xmlH"",@value)}' >
                                        <doc:Text id='{xpath:concat(""xmlText"",@value)}' value='{xpath:@value}' />
                                     </doc:H1>";
                doc.InitializeAndLoad();
                doc.DataBind();

                //For the ForEach template with an object source.
                var first = doc.FindAComponentById("xmlH1") as Head1;
                Assert.IsNotNull(first, "Could not find inner heading");

                var second = doc.FindAComponentById("xmlText2") as TextLiteral;
                Assert.IsNotNull(second, "Could not find the second label");

                Assert.AreEqual("2", second.Text, "The second label does not have the correct text value");



            }


        }


        /// <summary>
        /// Check that the correct types are assigned at runtime
        /// </summary>
        [TestMethod()]
        public void BindingTypeSafety()
        {
            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
                                     >
                        <Params>
                            <doc:String-Param id='string' ></doc:String-Param>
                            <doc:Int-Param id='int' ></doc:Int-Param>
                            <doc:Color-Param id='color' ></doc:Color-Param>
                        </Params>


                        <Pages>
                            <doc:Section>
                                <Content>
                                    <doc:Label id='{@:int}' text='{@:string}' styles:bg-color='{@:color}' ></doc:Label>
                                </Content>
                            </doc:Section>
                        </Pages>
                    </doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                var color = new Scryber.Drawing.Color(1, 0, 0);
                var text = "This is the title";
                var date = DateTime.Now;
                var i = 5;

                doc.Params["color"] = color;
                doc.Params["string"] = text;
                doc.Params["int"] = i;

                doc.InitializeAndLoad();
                doc.DataBind();


                //Find the label as the value should be converted to a string.
                var first = doc.FindAComponentById(i.ToString()) as Label;
                Assert.IsNotNull(first, "Could not find the label");

                //Check that the text matches
                Assert.AreEqual(text, first.Text, "The first label does not have the correct text value");

                //Check that the color matches
                Assert.AreEqual(color, first.BackgroundColor, "Background colours do not match");


            }

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                var color = new Scryber.Drawing.Color(1, 0, 0);
                var text = "This is the title";
                var date = DateTime.Now;
                var i = 5;

                bool caught = false;

                try
                {

                    //This should not be allowed 
                    doc.Params["color"] = text;
                    doc.Params["string"] = text;
                    doc.Params["int"] = i;

                    doc.InitializeAndLoad();
                    //doc.DataBind();
                }
                catch(Scryber.PDFDataException)
                {
                    caught = true;
                }

                Assert.IsTrue(caught, "The assignment of an incorrect type onto the parameter did not raise an error");


            }
        }


        /// <summary>
        /// Checks the asssignment of a string onto items
        /// </summary>
        [TestMethod()]
        public void BindingParamToString()
        {
            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
                                     >
                        <Params>
                            <doc:String-Param id='string' ></doc:String-Param>
                            <doc:Int-Param id='int' ></doc:Int-Param>
                            <doc:Color-Param id='color' ></doc:Color-Param>
                        </Params>


                        <Pages>
                            <doc:Section>
                                <Content>
                                    <doc:Label id='{@:int}' text='{@:string}' styles:bg-color='{@:color}' ></doc:Label>
                                </Content>
                            </doc:Section>
                        </Pages>
                    </doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                var color = new Scryber.Drawing.Color(1, 0, 0);
                var text = "This is the title";
                var date = DateTime.Now;
                var i = 5;

                doc.Params["color"] = color.ToString();
                doc.Params["string"] = text;
                doc.Params["int"] = i.ToString();

                doc.InitializeAndLoad();
                doc.DataBind();


                //Find the label as the value should be converted to a string.
                var first = doc.FindAComponentById(i.ToString()) as Label;
                Assert.IsNotNull(first, "Could not find the label");

                //Check that the text matches
                Assert.AreEqual(text, first.Text, "The first label does not have the correct text value");

                //Check that the color matches
                Assert.AreEqual(color, first.BackgroundColor, "Background colours do not match");


            }

            
        }


        /// <summary>
        /// Checks the asssignment of a subclass object onto as strongly typed object
        /// </summary>
        [TestMethod()]
        public void BindingParamToStrongObject()
        {
            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
                                     >
                        <Params>
                            <doc:Object-Param id='obj' type='Scryber.Core.UnitTests.Mocks.MockParameter, Scryber.UnitTests' ></doc:Object-Param>
                        </Params>

                        <Pages>
                            <doc:Section>
                                <Content>
                                    <doc:Label id='MyTitle' styles:font-bold='{@:obj.BoldTitle}' text='{@:obj.Title}' styles:font-size='{@:obj.SizeField}' styles:bg-color='{@:obj.Background}' ></doc:Label>
                                </Content>
                            </doc:Section>
                        </Pages>
                    </doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var param = new Mocks.MockSubParameter();

                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                doc.Params["obj"] = param;

                doc.InitializeAndLoad();
                doc.DataBind();


                //Find the label as the value should be converted to a string.
                var first = doc.FindAComponentById("MyTitle") as Label;
                Assert.IsNotNull(first, "Could not find the label");

                //Check that the text matches
                Assert.AreEqual(param.Title, first.Text, "The first label does not have the correct text value");

                //Check that the color matches
                Assert.AreEqual(param.Background, first.BackgroundColor, "Background colours do not match");

                Assert.AreEqual(param.SizeField, first.FontSize);


            }


            using (var reader = new System.IO.StringReader(src))
            {
                var param = new Mocks.MockOtherParameter();

                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                var caught = false;
                try
                {
                    doc.Params["obj"] = param;
                }
                catch(Scryber.PDFDataException)
                {
                    caught = true;
                }

                Assert.IsTrue(caught, "No exception was raised.");

                
            }


        }



        [TestMethod()]
        [TestCategory("Binding")]
        public void BindDynamicStyles()
        {

            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
                                     >
                        <Params>
                            <doc:Object-Param id='dynamic' ></doc:Object-Param>
                        </Params>

                        <Styles>

                            <styles:Style applied-class='head'>
                                <styles:Background color='{@:dynamic.Theme.TitleBg}' />
                                <styles:Font family='{@:dynamic.Theme.TitleFont}' />
                                <styles:Fill color='{@:dynamic.Theme.TitleColor}' />
                            </styles:Style>

                            <styles:Style applied-class='body'>
                                <styles:Font family='{@:dynamic.Theme.BodyFont}' size='{@:dynamic.Theme.BodySize}' />
                            </styles:Style>
    
                        </Styles>

                        <Pages>
    
                        <doc:Section>
                            <Content>
                                <doc:H1 styles:class='head' text='{@:Title}' ></doc:H1>
                                <data:ForEach value='{@:dynamic.List}' >
                                    <Template>
                                        <doc:Label styles:class='body' id='{@:.Id}' text='{@:.Name}' ></doc:Label>
                                        <doc:Br/>
                                    </Template>
                                </data:ForEach>

                            </Content>
                        </doc:Section>

                        </Pages>
                    </doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                var binding = new
                {
                    Title = "This is the document title",
                    List = new[] {
                    new { Name = "First", Id = "FirstID" },
                    new { Name = "Second", Id = "SecondID" }
                },
                    Theme = new
                    {
                        TitleBg = new Color(1, 0, 0),
                        TitleColor = new Color(1, 1, 1),
                        TitleFont = (FontSelector)"Segoe UI Light",
                        BodyFont = (FontSelector)"Segoe UI",
                        BodySize = (PDFUnit)12
                    }
                };

                doc.Params["dynamic"] = binding;
                doc.InitializeAndLoad();
                doc.DataBind();

                //For the ForEach template with an object source.
                var first = doc.FindAComponentById("FirstID") as Label;
                Assert.IsNotNull(first, "Could not find the first label");
                Assert.AreEqual("First", first.Text, "The first label does not have the correct Name value");

                var second = doc.FindAComponentById("SecondID") as Label;
                Assert.IsNotNull(second, "Could not find the second label");
                Assert.AreEqual("Second", second.Text, "The second label does not have the correct Name value");

                var style = doc.Styles[0] as Scryber.Styles.Style;
                Assert.IsTrue(style.IsValueDefined(Scryber.Styles.StyleKeys.BgColorKey), "The background color is not assigned");
                Assert.AreEqual(binding.Theme.TitleBg, style.Background.Color, "The background colors do not match");
                Assert.AreEqual(binding.Theme.TitleColor, style.Fill.Color, "The foreground colours do not match");

                style = doc.Styles[1] as Scryber.Styles.Style;
                Assert.AreEqual(binding.Theme.BodyFont, style.Font.FontFamily, "Body fonts do not match");
                Assert.AreEqual(binding.Theme.BodySize, style.Font.FontSize, "Body font sizes do not match");

            }


        }

    }
}
