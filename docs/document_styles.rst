=======================
Styles in your template
=======================

In scryber styles are used through out to build the document. Every component has a base style and some styles (such as fill colour and font) flow down
to their inner contents.

Styles on components
====================

Styles are supported on each component within the template. They are based on the styles namespace 
xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd".

.. code-block:: xml

    <pdf:Div styles:margins="20pt" styles:padding="4pt" styles:bg-color="#FF0000" 
                styles:fill-color="#FFFFFF" styles:font-family="Arial" styles:font-size="20pt" >
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

                    <pdf:Div styles:class="mystyle">
                        <pdf:Label>Hello World, from scryber.</pdf:Label>
                    </pdf:Div>
                    
                </Content>
            </pdf:Page>
            
        </Pages>
    </pdf:Document>

By using styles, it's the same as html and css. It cleans the code and makes it easier to standardise and change later on.
This can either be within the document itself, or in a `referenced stylesheet <referencing_files>`_



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


Applying Styles
===============

Styles can be applied to an element based upon a combination of 3 attributes of the Style.

@applied-id
@applied-class
@applied-type

e.g.

.. code-block:: xml

    <Styles>

        <!-- This style will be applied at the document level specifying
             the base level font, size and color for text. Because These
             cascade down, then it will be inherited by components in the document. -->

        <styles:Style applied-type="pdf:Document" >
            <style:Font family="Gill Sans" size="14pt" />
            <style:Fill color="#333" />
        </styles:Style>

        <!-- This style will be applied to all top level headings 
             specifying the font size and some spacing -->

        <styles:Style applied-type="pdf:H1" >
            <styles:Font bold="true" size="30pt" />
            <styles:Margins top="20pt" />
            <styles:Padding all="5pt" />
        </styles:Style>

        <!-- This style will be applied to all top level headings with a class of 'warning'
             and give a background colour of red on white text.  -->

        <styles:Style applied-class="warning">
            <styles:Background color="#FF0000"/>
            <styles:Fill color="#FFFFFF" />
        </styles:Style>

        <!-- This style will be applied to all components with a class of 'border'
             and give a background colour of red with white text -->

        <styles:Style applied-class="border">
            <styles:Border color="#7777" width="1pt" style="Solid"/>
            <styles:Fill color="#444" />
        </styles:Style>

        <!-- This style will be applied to all H1 Headings with a class of 'border'
             and give a border colour of red with white text -->

        <styles:Style applied-type="pdf:H1" applied-class="border">
            <styles:Border color="#550000" />
            <styles:Fill color="#550000" />
        </styles:Style>

        <!-- This style will only be applied to a component with ID 'FirstHead'
             and give a font size of 48pt -->

        <styles:Style applied-id="FirstHead">
            <styles:Font size="48pt"/>
        </styles:Style>

    </Styles>


.. note:: Currently scryber does not support the concept of nested or path styles as css e.g. div.class -> h1.class. It may be supported in the future.

The Applied Type
=================

A style definition with an applied-type attribute is used on all components of that type.
It also supports inheritance. The format is based on the qualified component name.

A sample set is give in the schema intellisense if used, but as long as the type name is known the it can be used.
Even invisible components such as data:ForEach can have styles applied.


Applying Multiple Styles
========================

Every component supports the style:class attribute. And the value of this can be one or more class names.

.. code-block:: xml

    <pdf:H1 id="FirstHead" styles:class="warning border" styles:font-italic="true" >This is the Warning heading</pdf:H1>



This will apply the H1 style, the 2 classes for the warning and border and increase the size based on the ID of first head.
And then the inline italic style will be applied.

.. image:: images/headingstyle.png


Late binding of styles
======================

Even once you have parsed or built a document, the styles can still be modified or added to.
Either on a component, or at a document level, as they are evaluated, allowing runtime alteration of the output.


.. code-block:: csharp

    //change the style sheet based on a flag check
    var sheet = checkflag ? "Sheet1.psfx" : "Sheet2.psfx"

    var doc = PDFDocument.ParseDocument("MyPath.pdfx");

    //Load the stylesheet as a referenced component
    var styles = PDFComponent.Parse(sheet) as Styles.PDFStylesDocument;

    //and add it to the document styles.
    doc.Styles.Add(styles);

Data binding Styles
===================

The process of data-binding (see: :doc:`document_lifecycle`, and :doc:`document_databinding`) can apply values to styles (and the referenced styles).

e.g.

.. code-block:: xml

    <Params>
        <pdf:Color-Param id='theme-bg' value='#FFFFFF'/>
        <pdf:Color-Param id='theme-bg2 value='#AAAAAA'/>
        <pdf:Color-Param id='theme-title-font' value='Helvetica'>
    </Params>
    <Styles>

        <!-- This style will be applied at the document level specifying
             the base level font, size and color for text. Because These
             cascade down, then it will be inherited by components in the document. -->

        <styles:Style applied-class="title" >
            <style:Font family="{@:theme-title-font}" size="14pt" />
            <style:Background color="{@:theme-by2}" />
        </styles:Style>

    </Styles>

Here the font family and background for any component with the class title assigned, will pick up the default theme values.
Were the code can override these values and provide new colours and fonts for output.

.. code-block:: csharp

    var doc = PDFDocument.ParseDocument(path);
    doc.Params["theme-bg"] = new Scryber.Drawing.PDFColor(0.0, 0.0, 0.0);
    doc.Params["theme-bg2"] = new Scryber.Drawing.PDFColor(0.3, 0.3, 0.3);
    doc.Params["theme-title-fill"] = new Scryber.Drawing.PDFColor(1, 1, 1);
    doc.Params["theme-title-font"] = "Gill Sans";

    return this.PDF(doc);

As per object databinding, you can even provide a specific class for binding.

Order and Precedence
====================

Scryber has a very basic precedence order - based on the order in the document.

1. The style from the parent is collected.
2. Any styles in the document are evaluated in the order they appear.
3. If a stylesheet reference is encountered, then the styles within it will be evaluated before moving on to the following siblings
4. Finally the styles directly applied will be evaluated, giving the final result.

This will then be flattened and used in the layout and rendering of the component.