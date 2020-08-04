===================================
Flowing column layout
===================================

Scryber supports flowing content layout. No matter the font, content type or structure. (see :doc:`document_pages`)

It also supports the same with columns, at the page and container level.


Specifying columns on Pages
===========================

All block level components (see :doc:`component_positioning`) support the use of columns.

There is by default on a block a single column, but by specifying a style:column-count (either on the block, or in the style definition) then 
the layout will split the block into that number of regions within it.

Content within the column will flow down as far as it is able (either the bottom or the page, or the maximum height of the container)
and then move to the top of the next column.

Below we specify the section to have 3 columns, they will be of equal size and the content within it will flow onto each
column and then ulimately the next page.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
              xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
              xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Pages>
        <pdf:Section styles:column-count="3" >
        <Header>
            <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="aqua" >This is the header</pdf:H4>
        </Header>
        <Content>
            <pdf:H1 styles:margins="5pt" styles:border-width="1pt" styles:border-color="green" >This is the content</pdf:H1>
            <pdf:Div styles:margins="5pt" styles:font-size="14pt" styles:border-width="1pt" styles:border-color="navy">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis orci mollis, finibus eros a, 
            tincidunt magna. Mauris efficitur nisl lorem, vitae semper nulla convallis id. Nam dignissim rutrum 
            mollis. Fusce imperdiet fringilla augue non venenatis. Mauris dictum velit augue, ut iaculis risus 
            pulvinar vitae. Aliquam id pretium sem. Pellentesque vel tellus risus. Etiam dolor neque, auctor id 
            convallis hendrerit, tincidunt at sem. Integer finibus congue turpis eu feugiat. Nullam non ultrices enim.<pdf:Br/>
            <pdf:Br/>
            <!-- Truncated for brevity
            .
            . -->
            Phasellus ultrices congue semper. Praesent ultrices orci ipsum. Maecenas suscipit tellus elit,
            non ullamcorper nulla blandit sed. Nulla eget gravida turpis, et vestibulum nunc. Nulla mollis
            dui eu ipsum dapibus, vel efficitur lectus aliquam. Nullam efficitur, dui a maximus ullamcorper,
            quam nisi imperdiet sapien, ac venenatis diam lectus a metus. Fusce in lorem viverra, suscipit
            dui et, laoreet metus. Quisque maximus libero sed libero semper porttitor. Ut tincidunt venenatis
            ligula at viverra. Phasellus bibendum egestas nibh ac consequat. Phasellus quis ante eu leo tempor
            maximus efficitur quis velit. Phasellus et ante eget ex feugiat finibus ullamcorper ut nisl. Sed mi
            nunc, blandit ut sem vitae, bibendum hendrerit ipsum.<pdf:Br/>
        </pdf:Div>
        <pdf:H1 styles:margins="5pt" styles:border-width="1pt" styles:border-color="green" >After the content</pdf:H1>
        </Content>
        <Footer>
            <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="purple" >This is the footer</pdf:H4>
        </Footer>
        </pdf:Section>
    </Pages>
    
    </pdf:Document>

.. image:: images/documentcolumns1.png


Here the page is set to 3 columns across the layout page. The headers are independent of the column setting, but the inner content 
flows down the first column, and then moves to the next when no more can be fitted, and so on until it reaches the end of the layout page. 

As we can overflow, a new layout page is added, and the content latout continues until the end.

.. note:: The borders and any background will be based around the container and show on each column.

Columns on containers
=====================

As we can see above, the headings were also part of the column layout on the page. 

Scryber supports the use of columns on containers too. So our layout can be improved if we remove the columns from the page,
and set them on the `pdf:Div` itself.

This will allow the headers to be full width, with the content flowing within the columns of the container.


.. code-block:: xml

    <pdf:Section>
        <Header>
            <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="aqua" >This is the header</pdf:H4>
        </Header>
        <Content>
            <pdf:H1 styles:margins="5pt" styles:border-width="1pt" styles:border-color="green" >This is the content</pdf:H1>
            <pdf:Div styles:column-count="3" styles:margins="5pt" styles:font-size="14pt" styles:border-width="1pt" styles:border-color="navy">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis orci mollis, finibus eros a, 
            tincidunt magna. Mauris efficitur nisl lorem, vitae semper nulla convallis id. Nam dignissim rutrum 
            mollis. Fusce imperdiet fringilla augue non venenatis. Mauris dictum velit augue, ut iaculis risus 
            pulvinar vitae. Aliquam id pretium sem. Pellentesque vel tellus risus. Etiam dolor neque, auctor id 
            convallis hendrerit, tincidunt at sem. Integer finibus congue turpis eu feugiat. Nullam non ultrices enim.<pdf:Br/>
            <pdf:Br/>
            <!-- Truncated for brevity
            .
            . -->
            Phasellus ultrices congue semper. Praesent ultrices orci ipsum. Maecenas suscipit tellus elit,
            non ullamcorper nulla blandit sed. Nulla eget gravida turpis, et vestibulum nunc. Nulla mollis
            dui eu ipsum dapibus, vel efficitur lectus aliquam. Nullam efficitur, dui a maximus ullamcorper,
            quam nisi imperdiet sapien, ac venenatis diam lectus a metus. Fusce in lorem viverra, suscipit
            dui et, laoreet metus. Quisque maximus libero sed libero semper porttitor. Ut tincidunt venenatis
            ligula at viverra. Phasellus bibendum egestas nibh ac consequat. Phasellus quis ante eu leo tempor
            maximus efficitur quis velit. Phasellus et ante eget ex feugiat finibus ullamcorper ut nisl. Sed mi
            nunc, blandit ut sem vitae, bibendum hendrerit ipsum.<pdf:Br/>
        </pdf:Div>
        <pdf:H1 styles:margins="5pt" styles:border-width="1pt" styles:border-color="green" >After the content</pdf:H1>
        </Content>
        <Footer>
            <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="purple" >This is the footer</pdf:H4>
        </Footer>
    </pdf:Section>


.. image:: images/documentcolumns2.png


Column and Alley Widths
========================

Along with changing the number of column scryber also supports the use of column and alley widths that can either be set on the style or
component itself.

Alleys are the margins between each column that defaults to 10pt, but can be specified as a single unit value, e.g. 20pt or 5mm
(see :doc:`drawing_units` for more on scryber measurements).

The column-widths attribute takes multiple fraction values (0.0 to 1.0), for one or more columns.
It also supports the use of the `*` character for variable width.
If a column is not specified then it will use the remainder of the space.

If for example we have 4 columns on a container that is 430pt wide with a 10pt alley we could use any of the following for the column widths.

* `0.2 0.3 0.3 0.2`
    * The first and last column will be 1/5th of the available space (80pt)
    * The second and third columns will be 3/10ths of the available space (120pt)
* `0.2 0.2`
    * The first 2 columns would be 1/5th of the available space (80pt)
    * The last 2 colums would be calculated as the remainder divided equally (120pt)
* `0.2 * 0.2`
    * The first and the 3rd column would be 1/5th of the available space (80pt)
    * The second and last columns will be 3/10ths of the available space (120pt)
    * If the column count were to increase to 5 then the variable columns would accomodate and ultimately all be the same width

.. warning:: It is an error to specify column widths that add up to over 1.0 (100%). An exception will be thrown.



.. code-block:: xml

    <pdf:Section styles:paper-orientation="Landscape" >
        <Header>
            <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="aqua" >This is the header</pdf:H4>
        </Header>
        <Content>
            <pdf:H1 styles:margins="5pt" styles:border-width="1pt" styles:border-color="green" >This is the content</pdf:H1>
            <pdf:Div styles:column-count="4" styles:column-widths="0.2 * 0.2" styles:alley-width="20pt" 
                     styles:margins="5pt" styles:font-size="14pt" styles:border-width="1pt" styles:border-color="navy">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis orci mollis, finibus eros a, 
            tincidunt magna. Mauris efficitur nisl lorem, vitae semper nulla convallis id. Nam dignissim rutrum 
            mollis. Fusce imperdiet fringilla augue non venenatis. Mauris dictum velit augue, ut iaculis risus 
            pulvinar vitae. Aliquam id pretium sem. Pellentesque vel tellus risus. Etiam dolor neque, auctor id 
            convallis hendrerit, tincidunt at sem. Integer finibus congue turpis eu feugiat. Nullam non ultrices enim.<pdf:Br/>
            <pdf:Br/>
            <!-- Truncated for brevity
            .
            . -->
            Phasellus ultrices congue semper. Praesent ultrices orci ipsum. Maecenas suscipit tellus elit,
            non ullamcorper nulla blandit sed. Nulla eget gravida turpis, et vestibulum nunc. Nulla mollis
            dui eu ipsum dapibus, vel efficitur lectus aliquam. Nullam efficitur, dui a maximus ullamcorper,
            quam nisi imperdiet sapien, ac venenatis diam lectus a metus. Fusce in lorem viverra, suscipit
            dui et, laoreet metus. Quisque maximus libero sed libero semper porttitor. Ut tincidunt venenatis
            ligula at viverra. Phasellus bibendum egestas nibh ac consequat. Phasellus quis ante eu leo tempor
            maximus efficitur quis velit. Phasellus et ante eget ex feugiat finibus ullamcorper ut nisl. Sed mi
            nunc, blandit ut sem vitae, bibendum hendrerit ipsum.<pdf:Br/>
        </pdf:Div>
        <pdf:H1 styles:margins="5pt" styles:border-width="1pt" styles:border-color="green" >After the content</pdf:H1>
        </Content>
        <Footer>
            <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="purple" >This is the footer</pdf:H4>
        </Footer>
    </pdf:Section>

Here we can see that we have changed the paper orientation to landscape, set the column number to 4 with widths of 0.2 * 0.2,
and set the alley width to 20pt to give more spacing.

The layout engine adjusts all content automatically within the column widths.

.. image:: images/documentcolumns3.png


Images and Shapes in columns
==============================


Nested columns
==============


Breaking columns
=================


Balanced Columns
=================
