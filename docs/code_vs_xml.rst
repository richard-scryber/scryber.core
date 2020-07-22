==========================
To code or not to code...
==========================

Scryber does not rely on xml, but it makes life easier and is more visual and structured.
But it does hand in hand you code. When ever you parse a PDFDocument or component you are simply
creating the same as you could in code.

XML Template
------------

.. code-block:: html

    <?xml version="1.0" encoding="UTF-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
              xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
              xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
        <Pages>
            <pdf:Page>
                <Content>
                    <pdf:Label>Hello World, from scryber.</pdf:Label>
                </Content>
            </pdf:Page>
        </Pages>
    </pdf:Document>


The same in code
-----------------

.. code-block:: csharp

            var doc = new PDFDocument();
            var page = new PDFPage();
            doc.Pages.Add(page);

            var label = new PDFLabel();
            label.Text = "Hello World, from scryber";
            page.Contents.Add(label);

            return doc.ProcessDocument();

Loading from a stream
----------------------

It is always possible to mix the declarative with the code.
For example we can load the XML as a string or stream and inject the data as needed.

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
            string content = @"<?xml version='1.0' encoding='utf-8' ?>
                        <pdf:Document xmlns:pdf = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' >
                        <Params>
                            <pdf:Object-Param id='Model' ></pdf:Object-Param>
                        </Params>
                        <Pages>
                            <pdf:Section>
                                <Content>
                                    <data:ForEach id='Foreach2' value='{@:Model.Entries}' >
                                        <Template>
                                            <pdf:Label text='{@:.Name}' /><pdf:Br/>
                                        </Template>
                                    </data:ForEach>
                                </Content>
                                <Footer>
                                    <pdf:Div styles:padding='5pt' styles:h-align='Center' >
                                        <pdf:PlaceHolder contents='{@:Model.Footer}' />
                                    </pdf:Div>
                            </pdf:Section>
                        </Pages>
                    </pdf:Document>";

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
                Footer = "<pdf:PageNumber />"
            };
            return data;
        }

.. note:: When loading from a stream, there is no relative reference to a local file. If you need to reference other files do so relative to the working directory, or pass in your own IPDFReferenceResolver


Why use one over the other
--------------------------

We always think that the declarative is better for what you need, but sometimes building in code works.
Using the :doc:`document_controllers` allows you to hook content back into a document template.

In this documentation, we will concentrate on the use of the declarative XML with code where appropriate, but remember that 
everything that is declared can be coded too.


