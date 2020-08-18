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

.. image:: images/drawingFontsSelect.png

Rather than the file name of the ttf or ttc file.


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

.. image:: images/drawingfontssystem.png


.. note:: .woff files are not currently supported, but these can be easily converted to their ttf components online. They may be supported in future.

Font Sizes
==========

Font styles
===========


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


