==============================
Positioning your content - td
==============================

Scryber has an intelligent layout engine. By default eveything will be laid out as per the flowing layout of the document Pages and columns.
Each component, be it block level or inline will have a position next to its siblings and move and following content along in the document.
If the content comes to the end of the page and cannot be fitted, then if allowed, it will be moved to the next page.

Inline Positioning
==================

Inline components such as text and spans will continue on the current line, and if they do not fit all the contnet, then they will 
flow onto the next line (or column or page). If the content moves, so the inline content will move with the container.

Carriager returns within the content of the xml file are ignored by default, 
as per html (see :doc:`reference/pdf_pre` if you don't want them to be.).

Examples of inline components are spans, labels, text literals, page numbers,

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                    xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd" >

    <Pages>
    
        <pdf:Page styles:margins="20pt" styles:font-size="20pt">
            <Content>
                This is the content of the page, 
                <pdf:Span styles:fill-color="maroon" >and this will continue on the current line until it reaches the end
                and then flow onto the next line.</pdf:Span> 
                This with then flow after the line.<pdf:Br/>
                A line break forces a new line in the content but flow in the page (#<pdf:PageNumber />) will continue. 
                <pdf:Span styles:fill-color="maroon" styles:font-size="30pt" >It also supports the use of multiple font sizes</pdf:Span> in multiple lines, 
                adjusting the line height as needed.
            </Content>
        </pdf:Page>
    </Pages>

    </pdf:Document>

Generating this document will create the following output 
(see :doc:`mvc_controller_full` or :doc:`gui_controller_full` to understand how to do this).

.. image:: images/documentpositioninginline.png

For more information on laying out textual content see :doc:`documenttextlayout`


Block Positioning
=================

A block starts on a new line in the content of the page. Children will be laid out within the block (unless absolutely positioned), and
content after the block will also begin a new line.

Examples of blocks are Div's, Paragraphs, Tables, BlockQuotes, Headings, Images, and Shapes.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                    xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd" >

        <Pages>
        
            <pdf:Page styles:margins="20pt" styles:font-size="20pt">
                <Content>
                    This is the content of the page, 
                    
                    <pdf:Div styles:fill-color="maroon" >This will always be on a new on the line, and it's content will then continue inline until it reaches the end
                    and then flow onto the next line.</pdf:Div> 
                    
                    After a block, this with then continue with the previous flow on the next line.<pdf:Br/>
                    A line break forces a new line in the content but flow in the page (#<pdf:PageNumber />) will continue. 
                    
                    <pdf:Div styles:fill-color="#666600" >
                    Blocks also supports the use of inline and block content within them
                    <pdf:Span styles:fill-color="#006666"  styles:font-size="30pt">in multiple lines, adjusting the line height as needed.</pdf:Span>
                    <pdf:Div >As a separate block within the container</pdf:Div>
                    </pdf:Div>
                    
                </Content>
            </pdf:Page>
        </Pages>

    </pdf:Document>

.. image:: images/documentpositioningblocks.png

Blocks also support the use of backgrounds, borders, margins and padding.
They also support :doc:`document_columns`

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                  xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd" >

    <Pages>
    
        <pdf:Page styles:margins="20pt" styles:font-size="20pt">
        <Content>
            This is the content of the page, 
            
            <pdf:Div styles:fill-color="maroon" styles:margins="20pt 10pt 10pt 10pt" >This will always 
                be on a new on the line, and it's content will then continue inline 
                until it reaches the end and then flow onto the next line.
            </pdf:Div> 
            
            After a block, this with then continue with the previous flow on the next line.<pdf:Br/>
            A line break forces a new line in the content but flow in the page (#<pdf:PageNumber />) will continue. 
            
            <pdf:Div styles:fill-color="#666600" styles:bg-color="#BBBB00" styles:padding="10pt"
                     styles:margins="10pt" styles:column-count="2">
                Blocks also supports the use of inline and block content within them

                <pdf:Span styles:fill-color="#006666"  styles:font-size="30pt">in multiple lines, 
                    adjusting the line height as needed.</pdf:Span>

                <!-- breaking onto a new column-->
                <pdf:ColumnBreak />

                <pdf:Div styles:fill-color="black" styles:bg-color="white" >As a separate block within the container</pdf:Div>
                And coming after the child block.
            </pdf:Div>
            
        </Content>
        </pdf:Page>
    </Pages>

    </pdf:Document>

.. image:: images/documentpositioningblocks2.png


Changing the position-mode
==========================

It is posible to change the default position mode for many components on the page. A span can be a block and a div can be a span.
Images and shapes (see :doc:`document_images` and :doc:`drawing_paths`) also support the use of the the position mode.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                  xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd" >

    <Pages>
    
        <pdf:Page styles:margins="20pt" styles:font-size="20pt">
            <Content>
                <pdf:Div styles:border-color="black" styles:border-width="1pt" >
                    The content of this div is all as a block (by default)
                    
                    <pdf:Div styles:fill-color="maroon" >This div is positioned as a block.</pdf:Div>

                    <!-- Images are by detault displayed as blocks -->
                    <pdf:Image styles:width="60pt" src="../../Content/Images/group.png" />
                    
                    After the content.
                </pdf:Div>

                <pdf:Div styles:border-color="black" styles:border-width="1pt" >
                    The content of this div is set explicity to inline.
                    
                    <pdf:Div styles:position-mode="Inline" styles:fill-color="maroon">This div is positioned as a block.</pdf:Div>

                    <!-- Image is also set to inline and will increase the line height automatically -->
                    <pdf:Image styles:position-mode="Inline" styles:width="60pt"  src="../../Content/Images/group.png" />

                    After the content.
                </pdf:Div>
                
            
            </Content>
        </pdf:Page>
    </Pages>

    </pdf:Document>


.. image:: images/documentpositioningblocks3.png


The full-width attribute
========================

The attribute full-width makes any block component automatically fill the available width of the region. Even if the inner content does not need it.
It's effectivly set as 100% width.

If it's set to false, the block will be as wide as needed (without going beyond the boundaries of it's own containing region).
This applies to the page, or a column containing the block.

By default Div's and Paragraphs are set to full width. BlockQuotes, Tables and Lists are not.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                    xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd" >

    <Styles>
        <!-- Using a style to repeat the border is easier -->
        <styles:Style applied-class="bordered" >
            <styles:Border color="black" style="Solid" width="1pt"/>
            <styles:Padding all="5pt"/>
        </styles:Style>
    </Styles>
    <Pages>
    
        <pdf:Page styles:margins="20pt" styles:font-size="20pt">
        <Content>
            <pdf:Div styles:class="bordered" >
                This div is full width<pdf:Br/>
                And will extend beyond the content.<pdf:Br/>
                To the width of its container.
            </pdf:Div>
            <pdf:Br/>
            <pdf:Div styles:class="bordered" styles:full-width="false" >
                This div is NOT full width<pdf:Br/>
                And will only size to the content.<pdf:Br/>
            </pdf:Div>
            <pdf:Br/>
            <pdf:Div styles:class="bordered" styles:full-width="false" >
                This div is NOT full width,
                but will  size to the content available in the container,
                and then flow to the next line.
            </pdf:Div>
            <pdf:Br/>
            <!-- Tables are not by default full width-->
            <pdf:Table>
                <pdf:Row>
                    <pdf:Cell styles:class="bordered">First</pdf:Cell>
                    <pdf:Cell styles:class="bordered">Second</pdf:Cell>
                    <pdf:Cell styles:class="bordered">Third</pdf:Cell>
                </pdf:Row>
                <pdf:Row>
                    <pdf:Cell styles:class="bordered">Fourth</pdf:Cell>
                    <pdf:Cell styles:class="bordered">Fifth</pdf:Cell>
                    <pdf:Cell styles:class="bordered">Sixth</pdf:Cell>
                </pdf:Row>
            </pdf:Table>
            <pdf:Br/>
            <!-- Tables are not by default full width-->
            <pdf:Table styles:full-width="true">
                <pdf:Row>
                    <pdf:Cell styles:class="bordered">First</pdf:Cell>
                    <pdf:Cell styles:class="bordered">Second</pdf:Cell>
                    <pdf:Cell styles:class="bordered">Third</pdf:Cell>
                </pdf:Row>
                <pdf:Row>
                    <pdf:Cell styles:class="bordered">Fourth</pdf:Cell>
                    <pdf:Cell styles:class="bordered">Fifth</pdf:Cell>
                    <pdf:Cell styles:class="bordered">Sixth</pdf:Cell>
                </pdf:Row>
            </pdf:Table>
        </Content>
        </pdf:Page>
    </Pages>

    </pdf:Document>

.. image:: images/documentpositioningfullwidth.png

Flowing around components
=========================

At the moment scryber does not support flowing content around other components.
It is something we are looking at supporting. If you want to help, please get in touch.

Relative Positioning
====================

This declares the position of the component relative to the block parent.
By default the position will be 0,0 (top, left), but using the x and y attributes it can be altered.

The component will no longer be in the flow of any inline content, nor alter the layout of the following components.

The parent block will however grow to accomodate the content including it's relative positioning.

[Example TBD]


Absolute Positioning
====================

The declares the position of the component relative to the current output page.
By default the position will again be 0,0 (top, left), but using the x and y attributes it can be altered.

The component will no longer be in the flow of any content, nor alter the layout of following components.

The parent block will NOT grow to accomodate the content, it is outside of the document flow completely.

If the absolutely positioned component is too big to fit on the page it will be clipped and not cause any overflow.

[Example TBD]


Numeric Positioning
===================

All content positioning is from the top left corner of the page or parent. 
This is a natural positioning mechanism for most cultures and developers.

Units of position can either be specified in 

* points (1/72 of an inch) e.g `36pt`, 
* inches e.g. `0.5in` or 
* millimeters e.g. `12.7mm`


If no units are specified then the default is points. See :doc:`drawing_units` for more information.

By specifying an x (left) value, and / or a y (top) value the component will be moved relative to its container, or the page.

[Example TBD]


Rendering Order
===============

All relative or absolutely positioned content will be rendered to the output in the order it appears in the document.
If a block is relatively positioned, it will overlay any content that preceded it, but anything coming after will be over the top.

[Example TBD]



Positioned components
======================

There are 2 components that take advantage of the positioning within Scryber.

1. :doc:`reference/pdf_canvas` positions all direct child components in the canvas as relative, whether they have been decared as such or not.
2. :doc:`reference/pdf_layergroup` has a collection of child Layers. These will be relatively positioned to the group.