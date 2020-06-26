=============
To code or not to code...
=============

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


Why use one over the other
--------------------------


* Render Options
* Styles
* Params
* Pages




