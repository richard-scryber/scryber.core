==============================
Sizing your content
==============================

Scryber has an intelligent layout engine. By default eveything will be laid out as per the flowing layout of the document Pages and columns.
Each component, be it block level or inline will have a position next to its siblings and move and following content along in the document.
If the content comes to the end of the page and cannot be fitted, then if allowed, it will be moved to the next page.

However it is very easy to size and position (see :doc:`component_positioning`) the content.

Width and Height
================

All block components support an explicit width and / or height value. If it's width is set, then any full-width style will be ignored.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                    xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd" >

    <Styles>
        <styles:Style applied-class="bordered" >
            <styles:Border color="#777" width="1pt" style="Solid"/>
            <styles:Background color="#EEE"/>
            <styles:Padding all="4pt"/>
        </styles:Style>
    </Styles>
    <Pages>
    
        <pdf:Page styles:margins="20pt" styles:font-size="18pt">
            <Content>
                <pdf:Div styles:class="bordered" >
                The content of this div is all as a block (by default)
                </pdf:Div>

                <pdf:Div styles:class="bordered" styles:width="300pt" >
                The content of this div is set to 300pt <pdf:U>wide</pdf:U>, so the content will flow within this width,
                and grow the height as needed.
                </pdf:Div>

                <pdf:Div styles:class="bordered" styles:height="150pt" >
                The content of this div is set to 150pt <pdf:U>high</pdf:U>, so the content will flow within this
                as full width, but the height will still be 150pt.
                </pdf:Div>

                <pdf:Div styles:class="bordered" styles:width="300pt" styles:height="150pt" >
                The content of this div is set to 300pt <pdf:U>wide</pdf:U> and 150pt <pdf:U>high</pdf:U>, so the content will flow within this
                as full width, but the height will still be 150pt.
                </pdf:Div>    
            
            </Content>
        </pdf:Page>
    </Pages>

    </pdf:Document>

.. image:: images/documentsizing.png

Images with width and height
==============================

Margins and Padding
====================

Clipping
========

Minimum size
============

Maximum size
============


Sizing Tables
=============

