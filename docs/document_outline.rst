================================
Document outline (or bookmarks)
================================

Structing a document also allows for the outline. This is effectively a table of contents
available in PDF readers that support quick navigation of the whole document, to go to the 
section or content needed.

All container components have an Outline element, and a shorthand outline-title attribute, that will build into the hierarchy
of a document as it is output.


Simple Outline
===============

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
    <!-- Document outline -->
    <Outline title="Top Level Document" />

    <Pages>

        <!-- inner outline item-->
        <doc:PageGroup outline-title="Page Group outline" >
        <Pages>

            <doc:Page outline-title="First Page">
            <Content>
                
                <doc:H3 >This is the first page</doc:H3>
            </Content>
            </doc:Page>

            <doc:Section outline-title="Second Section">
                <Content>
                    <doc:H3>This is the second page</doc:H3>
                    <doc:PageBreak/>
                    <!-- outline title on a component -->
                    <doc:H3 outline-title="Inner Heading">This is the third page</doc:H3>
                    <doc:Br/>
                    <doc:Br/>
                    <doc:Br/>
                    <doc:Br/>
                    <doc:H3 outline-title="Inner Heading 2">This is still the third page</doc:H3>
                </Content>
            </doc:Section>
            
        </Pages>
        
        </doc:PageGroup>
        
        <doc:Page outline-title="Out of the group">
            <Content>
                <doc:H3 >This is after the group</doc:H3>
            </Content>
        </doc:Page>

    
    </Pages>
    
    </doc:Document>


When output the reader application or browser can show the content of the outline.
Selecting any of the bookmark items should navigate directly to the page the content is on.

.. image:: images/documentoutline1.png


.. image:: images/documentoutline2.png


Show or Hide the outline
========================

The viewer options on the document control how a the reader application should show the document outline.

See :doc:`document_structure` for more information on the `Viewer` element.


Styling the outline
===================

The Outline element, or Outline style on an element, control how an outline or bookmark appears.
It supports the following options.

* styles:open - true or false. If true the outline will show any children by default, but can be closed.

The following were supported with Acrobat Reader, but not with other viewers. They may be supported again in future.

* styles:bold - true or false. If true then the font used to show the bookmark will be em-boldened. Not fully supported in all readers.
* styles:italic - true or false. If true then the font used to show the bookmark will be italicised. Not fully supported in all readers.
* styles:color - #RRGGBB. A colour component that the title text should be displayed in. Again, not fully supported in all readers.


