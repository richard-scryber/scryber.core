======================================
Links in  and out of documents
======================================

Within a document, it's easy to add a link to another component, another page, 
another document, or remote web link.


The Link Component
==================

A `pdf:Link` is an invisible component, although styles will be applied to content within it.
There are 4 propoerties that determine what happens when the content within the link is clicked.

* action - This defines the primary type of link action to perform, each section below describes each type of action.
* file - If set, then the link is an action for a different document or Url. Effectively like the href of an anchor tag in html.
* destination - If set then this is the location within the current or other file to show. Like the #name on a Url
* destination-fit - If set, it defines the fit type for the link (full page, width, height, or bounds).


Page Navigation Link
=====================

The simplest link type is navigational. The possible actions are as follows:

* FirstPage
* PrevPage
* NextPage
* LastPage

These are self-evident in their purpose, and no other attributes need defining.
It does not matter what page they are put on, they will perform the action if possible.


.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
    <Params>
        <pdf:String-Param id="search" value="https://www.google.com" />
    </Params>

    <Styles>

        <styles:Style applied-class="nav" >
        <styles:Background color="navy"/>
        <styles:Font size="10pt" bold="true"/>
        <styles:Position h-align="Center"/>
        <styles:Fill color="#CCC"/>
        <styles:Padding all="10pt"/>
        <styles:Margins bottom="10pt"/>
        <styles:Columns count="5"/>
        </styles:Style>
        
    </Styles>
    <Pages>

        <pdf:PageGroup>
        <Header>
            <!-- Shared header across all pages with 5 columns -->
            <pdf:Div styles:class="nav" >
                <pdf:Link action="FirstPage" >&lt;&lt; First Page</pdf:Link>
                <pdf:ColumnBreak/>
                <pdf:Link action="PrevPage">&lt; Previous Page</pdf:Link>
                <pdf:ColumnBreak/>
                <pdf:PageNumber />
                <pdf:ColumnBreak/>
                <pdf:Link action="NextPage" > Next Page &gt;</pdf:Link>
                <pdf:ColumnBreak/>
                <pdf:Link action="LastPage">Last Page &gt;&gt;</pdf:Link>
            </pdf:Div>
        </Header>
        <Pages>
            <!-- Title page and 3 following pages -->
            <pdf:Page >
            <Content>Title Page</Content>
            </pdf:Page>

            <pdf:Section>
            <Content>
                Content 1
                <pdf:PageBreak/>
                Content 2
                <pdf:PageBreak/>
                Content 3
            </Content>
            </pdf:Section>
        </Pages>
        </pdf:PageGroup>

    
    </Pages>
    
    </pdf:Document>

.. image:: images/documentLinksNavigation.png

.. note:: Some of the browser pdf readers do not support the naviagional links. Readers do.


Linking to other Components
===========================

