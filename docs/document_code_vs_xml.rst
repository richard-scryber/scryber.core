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
    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
              xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
              xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
        <Pages>
            <doc:Page>
                <Content>
                    <doc:Label>Hello World, from scryber.</doc:Label>
                </Content>
            </doc:Page>
        </Pages>
    </doc:Document>


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

See :doc:`dynamic_loading` for an example.



Why use one over the other
--------------------------

We always think that the declarative is better for what you need, but sometimes building in code works.
Using the :doc:`document_controllers` allows you to hook content back into a document template.

In this documentation, we will concentrate on the use of the declarative XML with code where appropriate, but remember that 
everything that is declared can be coded too.


