======================================
Document fonts and font styles - td
======================================

Fonts are by detault automatically loaded at startup, and can be referenced by name.

Built in fonts
==============

There are a number (14 to be precise) of built in fonts with PDF readers that can be used in documents. These are as follows

* Helvetica - Regular, Bold, Italic and Bold Italic.
* Times - Regular, Bold, Italic and Bold Italic.
* Courier - Regular, Bold, Italic and Bold Italic.
* Zapf Dingbats - Regular
* Symbol - Regular

It is safe to assume that these fonts exist and can be used.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >

    <Styles>
        
        <!-- Add a style to font divs -->
        <styles:Style applied-type="pdf:Div" applied-class="std-font" >
            <styles:Font size="20pt" />
            <styles:Background color="#AAA"/>
            <styles:Padding all="4pt"/>
            <styles:Margins bottom="10pt" />
        </styles:Style>

    </Styles>
    <Pages>
        
        <pdf:Page styles:padding="10" >
        <Content>
            <pdf:Div styles:class="std-font" styles:font-family="Helvetica" >
                <pdf:Span>Helvetica is the default font.</pdf:Span>
            </pdf:Div>
            <pdf:Div styles:class="std-font" styles:font-family="Times" >
                <pdf:Span>Times is a standard font.</pdf:Span>
            </pdf:Div>
            <pdf:Div styles:class="std-font" styles:font-family="Courier" >
                <pdf:Span>Courier is a standard font.</pdf:Span>
            </pdf:Div>
            <pdf:Div styles:class="std-font" styles:font-family="Zapf Dingbats" >
                <pdf:Span>Dingbats is a standard font.</pdf:Span>
            </pdf:Div>
            <pdf:Div styles:class="std-font" styles:font-family="Symbol" >
                <pdf:Span>Symbol is a standard font.</pdf:Span>
            </pdf:Div>
        </Content>
        </pdf:Page>
    </Pages>
    
    </pdf:Document>

.. image:: images/drawingfontsStandard.png



Using different fonts
=====================

Along with the standard fonts, scryber supports the systems fonts (the fonts in the Environment.SpecialFolder.Fonts).
It does not support postscript font files but does support.

* ttf & otf - A truetype font file or opentype font file.
* ttc & otc - A truetype font collection (multiple styles) or open type collection


Fonts should be referred to by their Unicode Name, usually displayed through the font browser of the underlying operating system.
Rather than the file name of the ttf or ttc file.

.. image:: images/drawingFontsSelect.png

The following uses 4 different ttf fonts installed on the machine generating the document.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >

    <Styles>
        
        <!-- Add a style to font divs -->
        <styles:Style applied-type="pdf:Div" applied-class="std-font" >
            <styles:Font size="20pt" />
            <styles:Background color="#AAA"/>
            <styles:Padding all="4pt"/>
            <styles:Margins bottom="10pt" />
        </styles:Style>

    </Styles>
    <Pages>
        
        <pdf:Page styles:padding="10" >
        <Content>
        
            <pdf:Div styles:class="std-font" styles:font-family="Arial" >
                <pdf:Span>Arial is a system installed font.</pdf:Span>
            </pdf:Div>
            <pdf:Div styles:class="std-font" styles:font-family="Segoe UI" >
                <pdf:Span>Segoe UI is a system installed font.</pdf:Span>
            </pdf:Div>
            <pdf:Div styles:class="std-font" styles:font-family="Impact" >
                <pdf:Span>Impact is a system installed font.</pdf:Span>
            </pdf:Div>
            <pdf:Div styles:class="std-font" styles:font-family="Comic Sans MS" >
                <pdf:Span>Comic Sans is a system installed font.</pdf:Span>
            </pdf:Div>
            <pdf:Div styles:class="std-font" styles:font-family="Wingdings" >
                <pdf:Span>Wingdings is a system installed font.</pdf:Span>
            </pdf:Div>
        </Content>
        </pdf:Page>
    </Pages>
    
    </pdf:Document>

.. image:: images/drawingfontsSystem.png

As the font is set to inherit, all child text components will use the specified font of the parent. If the
font is changed, then all children will use the new font.

.. note:: .woff files are not currently supported, but these can be easily converted to their ttf components online. They may be supported in future.

Font styles
===========

Along with the font family scryber supports the use of 'Bold', 'Italic' and 'Bold Italic' within the font to change the style.

The use of the <pdf:B></pdf:B> and <pdf:I></pdf:I> components also applies the Bold and Italic flags based on the style. They can be applied 
individually or nested, but they cannot be mixed inconsistently (breaking the rules of XML).

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >

    <Styles>
        
        <!-- Add a style to images -->
        <styles:Style applied-type="pdf:Div" applied-class="std-font" >
        <styles:Font size="20pt" />
        <styles:Background color="#AAA"/>
        <styles:Padding all="4pt"/>
        <styles:Margins bottom="10pt" />
        </styles:Style>

    </Styles>
    <Pages>
        
        <!-- Setting the font on the page, rather than at each level. -->
        <pdf:Page styles:padding="10" styles:font-family="Segoe UI" >
        <Content>
        
            <pdf:Div styles:class="std-font" >
                <pdf:Span>Regular Segoe UI.</pdf:Span>
            </pdf:Div>
            <pdf:Div styles:class="std-font" 
                        styles:font-bold="true" >
                <pdf:Span>Segoe UI has a bold variant.</pdf:Span>
            </pdf:Div>
            <pdf:Div styles:class="std-font"
                        styles:font-italic="true" >
                <pdf:Span>Segoe UI is also available in italic.</pdf:Span>
            </pdf:Div>
            <pdf:Div styles:class="std-font" styles:font-italic="true" >
                <pdf:B>This is Segoe UI within a Bold span, with italic on the div.</pdf:B>
            </pdf:Div>
            <pdf:Div styles:class="std-font" styles:font-family="Segoe UI Light" >
                <pdf:Span>This is the light variant of the font <pdf:I>with Italic inside</pdf:I> the span.</pdf:Span>
            </pdf:Div>
        </Content>
        </pdf:Page>
    </Pages>
    
    </pdf:Document>


.. image:: images/drawingfontsStyles.png

.. warning:: If the bold or italic variants are not available as a font, by default, an exception will be raised.
    e.g. There is no bold variant of 'Segoe UI Light' as you might understand


Font Decoration
===============


Font Sizes
==========


Font Fallback
=============


Leading and spacing
===================


Multi-byte Characters
=====================


Right to Left
=============


Changing the default font
=========================

Font Folders
============


Explict font Folders
====================


Explicit Fonts
==============


