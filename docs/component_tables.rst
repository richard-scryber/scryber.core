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

                <!-- Basic unstyled Table -->

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

                <!-- Table with full width and styles -->

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

Tables support both header and footer rows (single or multiple) along with header and footer cells.
The header cells by default will repeat across columns and or pages, but can be set not to repeat.
(Alternatively, rows can simply be set to repeat, and will do so after they are initially been laid out).

Rows support the block styles, except margins, padding and positioning.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
    
    <Styles>

        <styles:Style applied-type="pdf:Cell" >
        </styles:Style>

        <styles:Style applied-type="pdf:Cell" applied-class="strong" >
        <styles:Font bold="true"/>
        </styles:Style>

        <styles:Style applied-class="table-title" >
        <styles:Table row-repeat="None"/>
        </styles:Style>
    </Styles>
    <Pages>

            <pdf:Page styles:margins="20pt" styles:font-size="12pt">
            <Content>

                <pdf:Div styles:column-count="2" styles:max-height="200pt" styles:border-color="aqua" styles:padding="2pt" >


                <pdf:Table styles:margins="0 0 10 0" styles:full-width="true">
                    
                    <!-- Header that will not repeat based on style-->
                    <pdf:Header-Row styles:class="table-title" >
                    <pdf:Header-Cell styles:column-span="3" >A flowing table</pdf:Header-Cell>
                    </pdf:Header-Row>
                    
                    <!-- Header that will repeat -->
                    <pdf:Header-Row>
                    <pdf:Header-Cell>Header 1</pdf:Header-Cell>
                    <pdf:Header-Cell>Header 2</pdf:Header-Cell>
                    <pdf:Header-Cell>Header 3</pdf:Header-Cell>
                    </pdf:Header-Row>
                    
                    <pdf:Row>
                    <pdf:Cell>Cell 1.1</pdf:Cell>
                    <pdf:Cell>Wide Cell 1.2</pdf:Cell>
                    <pdf:Cell>Cell 1.3</pdf:Cell>
                    </pdf:Row>
                    <pdf:Row>
                    <pdf:Cell>Cell 2.1</pdf:Cell>
                    <pdf:Cell styles:column-span="2">2 Column Cell 2.2</pdf:Cell>
                    </pdf:Row>
                    
                    <!-- Standard row, that will repeat after
                    it has been initially laid out -->
                    <pdf:Row styles:repeat="RepeatAtTop" styles:bg-color="#EEE">
                    <pdf:Cell>Repeat 3.1</pdf:Cell>
                    <pdf:Cell>Repeat 3.2</pdf:Cell>
                    <pdf:Cell styles:width="60pt">Cell 3.3</pdf:Cell>
                    </pdf:Row>
                    
                    <pdf:Row><pdf:Cell>Cell 4.1</pdf:Cell><pdf:Cell>Wide Cell 4.2</pdf:Cell><pdf:Cell>Cell 4.3</pdf:Cell></pdf:Row>
                    <pdf:Row><pdf:Cell>Cell 5.1</pdf:Cell><pdf:Cell>Wide Cell 5.2</pdf:Cell><pdf:Cell>Cell 5.3</pdf:Cell></pdf:Row>
                    <pdf:Row><pdf:Cell>Cell 6.1</pdf:Cell><pdf:Cell>Wide Cell 6.2</pdf:Cell><pdf:Cell>Cell 6.3</pdf:Cell></pdf:Row>
                    <pdf:Row><pdf:Cell>Cell 7.1</pdf:Cell><pdf:Cell>Cell 7.2</pdf:Cell><pdf:Cell>Cell 7.3</pdf:Cell></pdf:Row>
                    <pdf:Row><pdf:Cell>Cell 8.1</pdf:Cell><pdf:Cell>Cell 8.2</pdf:Cell><pdf:Cell>Cell 8.3</pdf:Cell></pdf:Row>
                    <pdf:Row><pdf:Cell>Cell 9.1</pdf:Cell><pdf:Cell>Cell 9.2</pdf:Cell><pdf:Cell>Cell 9.3</pdf:Cell></pdf:Row>
                    <pdf:Row><pdf:Cell>Cell 10.1</pdf:Cell><pdf:Cell>Cell 10.2</pdf:Cell><pdf:Cell>Cell 10.3</pdf:Cell></pdf:Row>
                    
                    <pdf:Footer-Row styles:bg-color="#CCC" >
                    <pdf:Footer-Cell>Footer 1</pdf:Footer-Cell>
                    <pdf:Footer-Cell>Footer 2</pdf:Footer-Cell>
                    <pdf:Footer-Cell>Footer 3</pdf:Footer-Cell>
                    </pdf:Footer-Row>
                </pdf:Table>
                
                </pdf:Div>

            </Content>
            </pdf:Page>

    </Pages>
    
    </pdf:Document>


.. image:: images/documentTablesFlow.png



Mixed content and nesting
=========================


Binding to Data
===============

