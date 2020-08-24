==========================
Tables, Row and Cells - td
==========================

Scryber supports the use of tables with rows, cells and allows nesting, overflow, headings, footers and 
column-spans.

It also supports the use of binding and repeating.


Simple Tables
=============

A simple table with no style or formatting will be output with a single point gray border and 4pt padding on each cell.
The cells support a column-span attribute to allow multiple column content.

Each column will take up as much room as needed (or possible).
Applying the full-width addtibute will make the table use all available space, obeying any fixed column widths.

Rows and cells also support individual styles.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
    
    <Styles>

        <styles:Style applied-type="pdf:Cell" applied-class="strong" >
            <styles:Font bold="true"/>
        </styles:Style>
    </Styles>
    <Pages>

        <pdf:Page styles:margins="20pt" styles:font-size="14pt">
            <Content>

                <pdf:Table styles:margins="0 0 10 0">
                <pdf:Row>
                    <pdf:Cell>Cell 1.1</pdf:Cell>
                    <pdf:Cell>Wide Cell 1.2</pdf:Cell>
                    <pdf:Cell>Cell 1.3</pdf:Cell>
                </pdf:Row>
                <pdf:Row>
                    <pdf:Cell>Cell 2.1</pdf:Cell>
                    <pdf:Cell styles:column-span="2">2 Column Cell 2.2</pdf:Cell>
                </pdf:Row>
                <pdf:Row>
                    <pdf:Cell>Cell 3.1</pdf:Cell>
                    <pdf:Cell>Cell 3.2</pdf:Cell>
                    <pdf:Cell styles:width="200pt">Cell 3.3</pdf:Cell>
                </pdf:Row>
                </pdf:Table>

                <pdf:Table styles:margins="0 0 10 0" styles:full-width="true">
                <pdf:Row styles:bg-color="#CCC">
                    <pdf:Cell styles:class="strong">Cell 1.1</pdf:Cell>
                    <pdf:Cell>Wide Cell 1.2</pdf:Cell>
                    <pdf:Cell styles:class="strong">Cell 1.3</pdf:Cell>
                </pdf:Row>
                <pdf:Row>
                    <pdf:Cell>Cell 2.1</pdf:Cell>
                    <pdf:Cell styles:column-span="2">2 Column Cell 2.2</pdf:Cell>
                </pdf:Row>
                <pdf:Row>
                    <pdf:Cell>Cell 3.1</pdf:Cell>
                    <pdf:Cell>Cell 3.2</pdf:Cell>
                    <pdf:Cell styles:width="200pt">Cell 3.3</pdf:Cell>
                </pdf:Row>
                <pdf:Row>
                    <pdf:Cell styles:class="strong" styles:bg-color="#CCC">Cell 4.1</pdf:Cell>
                </pdf:Row>
                </pdf:Table>

            </Content>
        </pdf:Page>

    </Pages>
    
    </pdf:Document>


.. image:: images/documentTables1.png

Headers, Footers and overflow
=============================


Mixed content and nesting
=========================


Binding to Data
===============

