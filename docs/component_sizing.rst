==============================
Sizing your content - td
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

Scryber handles the sizing of images based on the natural size of the image. If no explicit size or positioning is provided, then it will be rendered
at the native size for the image.

If the available space within the container is not sufficuent to hold the image at it's natural size, then the image render size will be reduced
proportionally to fit the space available.

If either a width **or** height is assigned, then this will be used to proportionally resize the image to that height or width.

If both a width **and** height are assigned, then they will both be used to fit the image to that space. No matter what the originals' proportions are.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                    xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd" >
    <Pages>
        <pdf:Page styles:margins="20pt" styles:font-size="12pt" >
            <Content>
                
                <pdf:Span >An image will natually size to it's dimensions without space restriction.</pdf:Span>
                <pdf:Image src="../../Content/Images/landscape.jpg" />

                <pdf:Div styles:column-count="4" styles:margins="10 0 0 0" >
                    <pdf:B>First Column</pdf:B><pdf:Br/>
                    An image will fit to it's container if no explicit size is set.
                    <pdf:Image src="../../Content/Images/landscape.jpg" />
                    <pdf:ColumnBreak/>
                    <pdf:B>Second Column</pdf:B><pdf:Br/>
                    If a width is set, then the sizing will be proportional.
                    <pdf:Image src="../../Content/Images/landscape.jpg" styles:width="100pt" />
                    <pdf:ColumnBreak/>
                    <pdf:B>Third Column</pdf:B><pdf:Br/>
                    If a height is set, then the sizing will be proportional.
                    <pdf:Image src="../../Content/Images/landscape.jpg" styles:height="50pt" />
                    
                    <pdf:ColumnBreak/>
                    <pdf:B>Third Column</pdf:B><pdf:Br/>
                    If a width and height are set these will be used explicitly.
                    <pdf:Image src="../../Content/Images/landscape.jpg" styles:width="100pt" styles:height="50pt" />
                </pdf:Div>

                <!-- Photo by Bailey Zindel on Unsplash -->
            </Content>
        </pdf:Page>
    </Pages>

    </pdf:Document>

.. image:: images/documentsizingimages.png

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

