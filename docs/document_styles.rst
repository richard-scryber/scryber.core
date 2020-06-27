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

Style Classes
=============

Along with appling styles directly to the components, Scryber supports the use of styles declaratively and applied to the content dynamically.
This can either be within the document itself, or in a `referenced stylesheet <referenced_styles>`_

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
        <Styles>

            <styles:Style *applied-class="mystyle"* >
                <styles:Margins all="20pt"/>
                <styles:Padding all="20pt"/>
                <styles:Font family="Arial" size="20pt"/>
                <styles:Background color="#FF0000"/>
                <styles:Fill color="white"/>
            </styles:Style>
            
        </Styles>
        <Pages>
            <pdf:Page>
                <Content>

                    <pdf:Div *styles:class="mystyle"*>
                        <pdf:Label>Hello World, from scryber.</pdf:Label>
                    </pdf:Div>
                    
                </Content>
            </pdf:Page>
            
        </Pages>
    </pdf:Document>

By using styles, it's the same as html and css. It cleans the code and makes it easier to standardise and change later on.


Block Styles
============

Components such as div's, paragraphs, headings, tables, lists and list items are by default blocks. This means they will begin on a new line.
Components such as spans, labels, dates and numbers are inline components. This means they will continue with the flow of content in the current line.

There are certain style attributes that will only be used on block level components. These are:

* Background Styles
* Border Styles
* Margins
* Padding
* Vertical and Horizontal alignment.





