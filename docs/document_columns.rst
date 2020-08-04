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

    <pdf:Section >
        <Header>
            <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="aqua" >This is the header</pdf:H4>
        </Header>
        <Content>
            <pdf:H1 styles:margins="5pt" styles:border-width="1pt" styles:border-color="green" >This is the content</pdf:H1>
            <pdf:Div styles:column-count="3" styles:margins="5pt" styles:font-size="14pt" styles:border-width="1pt" styles:border-color="navy">


.. image:: images/documentcolumns2.png


Column Widths
==============


Balanced Columns
================


Nested columns
==============



Breaking columns
=================
