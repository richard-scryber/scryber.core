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
        
        <!-- Add a style to images -->
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

Scryber supports the following font file format

* ttf & otf - A truetype font file or opentype font file.
* ttc & otc - A truetype font collection (multiple styles) or open type collection

Fonts should be referred to by their Postscript Name


Font Sizes
==========

Font styles
===========

Default Font
============



Font Fallback
=============


Leading and spacing
===================


Multi-byte Characters
=====================


Right to Left
=============


Font Folders
============


Explict font Folders
====================


Explicit Fonts
==============


