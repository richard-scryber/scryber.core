================================
Dynamic loading of content
================================

With scryber it is perfectly possible and fully supported to load content from a string,
a compound string, an XMLReader, a sql blob, or a http response stream.

Ultimately the source of the template is up to the developer.

.. code-block:: csharp

        public IActionResult DocumentDynamic(string title = "New Document")
        {
            //Load the content and model in an MVC Controller method
            using (var pdfx = GetDocument(title))
            {
                var model = GetData(title);

                //And output the content as together
                return this.PDF(pdfx, model);
            }
        }

        protected PDFDocument GetDocument(string title)
        {

            //This content could be from anywhere, by any means.

            string content = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' >
                        <Params>
                            <doc:Object-Param id='Model' ></doc:Object-Param>
                        </Params>
                        <Pages>
                            <doc:Section>
                                <Content>
                                    <data:ForEach id='Foreach2' value='{@:Model.Entries}' >
                                        <Template>
                                            <doc:Label text='{@:.Name}' /><doc:Br/>
                                        </Template>
                                    </data:ForEach>
                                </Content>
                                <Footer>
                                    <doc:Div styles:padding='5pt' styles:h-align='Center' >
                                        <doc:PlaceHolder contents='{@:Model.Footer}' />
                                    </doc:Div>
                            </doc:Section>
                        </Pages>
                    </doc:Document>";

            //With a string reader, but could be any stream, text reader, xml reader or other source.
            using (var reader = new System.IO.StringReader(content))
            {
                return PDFDocument.ParseDocument(reader, ParseSourceType.DynamicContent);
            }
        }

        protected object GetData(string title)
        {
            var data = new
            {
                Title = title,
                Entries = new[]
                    {
                        new { Name = "First", Id = "FirstID"},
                        new { Name = "Second", Id = "SecondID"}
                    },
                Footer = "<doc:PageNumber />"
            };
            return data;
        }

.. note:: When loading from a stream, there is no relative reference to a local file. If you need to reference other files do so relative to the working directory, or pass in your own IPDFReferenceResolver


