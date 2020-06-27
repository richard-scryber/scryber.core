=============
Styles in your template
=============

In scryber styles are used through out to build the document. Every component has a base style and some styles (such as fill colour and font) flow down
to their inner contents.

Styles on components
====================

Styles are supported on each component within the template. They are based on the styles namespace 
xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd".

.. code-block:: xml
        <pdf:Div *styles:margins="20pt" styles:padding="4pt" styles:bg-color="#FF0000" 
                 styles:fill-color="#FFFFFF" styles:font-family="Arial" styles:font-size="20pt"*>
          <pdf:Label>Hello World, from scryber.</pdf:Label>
        </pdf:Div>

Or in the code

.. code-block:: csharp
        private static PDFComponent StyledComponent()
        {
            var div = new PDFDiv()
            {
                BackgroundColor = new Scryber.Drawing.PDFColor(Drawing.ColorSpace.RGB, 255, 0, 0),
                Margins = new Drawing.PDFThickness(20),
                Padding = new Drawing.PDFThickness(4),
                FontFamily = "Arial",
                FontSize = 20,
                FillColor = Scryber.Drawing.PDFColors.White
            };

            div.Contents.Add(new PDFLabel()
            {
                Text = "Hello World from scryber"
            });

            return div;

        }


Block Styles
============

Components such as div's, headings, tables, lists and list items are by default blocks. This means they will begin on a new line 


