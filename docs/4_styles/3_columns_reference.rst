===================================
Column layout - PD
===================================

Scryber supports flowing content layout. No matter the font, content type or structure. (see :doc:`document_pages`)
It also supports the same with columns, at the page and container level.


Specifying columns on Pages
----------------------------

All block level components (see :doc:`component_positioning`) support the use of columns.

There is by default on a block a single column, but by specifying a style:column-count (either on the block, or in the style definition) then 
the layout will split the block into that number of regions within it.

Content within the column will flow down as far as it is able (either the bottom or the page, or the maximum height of the container)
and then move to the top of the next column.

Below we specify the section to have 3 columns, they will be of equal size and the content within it will flow onto each
column and then ulimately the next page.

.. code-block:: html

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <!-- set the coumn count on the body -->
    <body style="column-count:3;">
        <header>
            <h4 style='margin: 5pt; border-bottom-width: 1pt; border-bottom-color:aqua'>This is the header</h4>
        </header>

        <h1 style='margin:5pt;border-width:1pt; border-color:green'>This is the content</h1>
        
        <div style='margin:5pt; font-size:14pt; border-width: 1pt; border-color: navy'>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis orci mollis, finibus eros a,
            tincidunt magna. Mauris efficitur nisl lorem, vitae semper nulla convallis id. Nam dignissim rutrum
            mollis. Fusce imperdiet fringilla augue non venenatis. Mauris dictum velit augue, ut iaculis risus
            pulvinar vitae. Aliquam id pretium sem. Pellentesque vel tellus risus. Etiam dolor neque, auctor id
            convallis hendrerit, tincidunt at sem. Integer finibus congue turpis eu feugiat. Nullam non ultrices enim.<br />
            <!-- Truncated for brevity
            .
            . -->
            Maecenas vitae vehicula mauris. Aenean egestas et neque sit amet pulvinar.
            Phasellus ultrices congue semper. Praesent ultrices orci ipsum. Maecenas suscipit tellus elit,
            non ullamcorper nulla blandit sed. Nulla eget gravida turpis, et vestibulum nunc. Nulla mollis
            dui eu ipsum dapibus, vel efficitur lectus aliquam. Nullam efficitur, dui a maximus ullamcorper,
            quam nisi imperdiet sapien, ac venenatis diam lectus a metus. Fusce in lorem viverra, suscipit
            dui et, laoreet metus. Quisque maximus libero sed libero semper porttitor. Ut tincidunt venenatis
            ligula at viverra. Phasellus bibendum egestas nibh ac consequat. Phasellus quis ante eu leo tempor
            maximus efficitur quis velit. Phasellus et ante eget ex feugiat finibus ullamcorper ut nisl. Sed mi
            nunc, blandit ut sem vitae, bibendum hendrerit ipsum.<br />
        </div>
        <h1 style="margin:5pt; border-width:1pt; border-color: green">After the content</h1>
        
        <footer>
            <h4 style="margin: 5pt; border-width: 1pt; border-color: purple">This is the footer</h4>
        </footer>
    </body>

    </html>

.. image:: images/documentcolumns1.png


Here the page is set to 3 columns across the layout page. The headers are independent of the column setting, but the inner content 
flows down the first column, and then moves to the next when no more can be fitted, and so on until it reaches the end of the layout page. 

As we can overflow, a new layout page is added, and the content latout continues until the end.

.. note:: The borders and any background will be based around the container and show on each column.

Columns on containers
----------------------

As we can see above, the headings were also part of the column layout on the page. 

Scryber supports the use of columns on containers too. So our layout can be improved if we remove the columns from the page,
and set them on the `div` itself.

This will allow the headers to be full width, with the content flowing within the columns of the container.


.. code-block:: html

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <!-- remove the coumn count on the body -->
    <body>
        <header>
            <h4 style='margin: 5pt; border-bottom-width: 1pt; border-bottom-color:aqua'>This is the header</h4>
        </header>
        <h1 style='margin:5pt;border-width:1pt; border-color:green'>This is the content</h1>
        
        <!-- Set the div to have 3 columns
            rather than the page -->

        <div style='column-count:3; margin:5pt; font-size:14pt; border-width: 1pt; border-color: navy'>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis orci mollis, finibus eros a,
            tincidunt magna. Mauris efficitur nisl lorem, vitae semper nulla convallis id. Nam dignissim rutrum
            mollis. Fusce imperdiet fringilla augue non venenatis. Mauris dictum velit augue, ut iaculis risus
            pulvinar vitae. Aliquam id pretium sem. Pellentesque vel tellus risus. Etiam dolor neque, auctor id
            convallis hendrerit, tincidunt at sem. Integer finibus congue turpis eu feugiat. Nullam non ultrices enim.<br />
            <!-- Truncated for brevity
            .
            . -->
            Maecenas vitae vehicula mauris. Aenean egestas et neque sit amet pulvinar.
            Phasellus ultrices congue semper. Praesent ultrices orci ipsum. Maecenas suscipit tellus elit,
            non ullamcorper nulla blandit sed. Nulla eget gravida turpis, et vestibulum nunc. Nulla mollis
            dui eu ipsum dapibus, vel efficitur lectus aliquam. Nullam efficitur, dui a maximus ullamcorper,
            quam nisi imperdiet sapien, ac venenatis diam lectus a metus. Fusce in lorem viverra, suscipit
            dui et, laoreet metus. Quisque maximus libero sed libero semper porttitor. Ut tincidunt venenatis
            ligula at viverra. Phasellus bibendum egestas nibh ac consequat. Phasellus quis ante eu leo tempor
            maximus efficitur quis velit. Phasellus et ante eget ex feugiat finibus ullamcorper ut nisl. Sed mi
            nunc, blandit ut sem vitae, bibendum hendrerit ipsum.<br />
        </div>
        <!-- after the columns we go full width again -->
        <h1 style="margin:5pt; border-width:1pt; border-color: green">After the content</h1>
        
        <footer>
            <h4 style="margin: 5pt; border-width: 1pt; border-color: purple">This is the footer</h4>
        </footer>
    </body>


.. image:: images/documentcolumns2.png

Column breaks and keeping content together
------------------------------------------

Just as with pages, columns support both breaking before and after along with the flowing layout.

Using the 'break-before: always' style a new column (or page if we are already on the last columns) will be moved to
for the layout of content. Using 'break-after: always' will force the following content onto a new column or page.

If there is a block of content that should stay together, the the 'break-inside: avoid' style will attempt to keep all the inner
content (blocks, images, text etc), on a single page.

.. code-block:: html

    <html xmlns='http://www.w3.org/1999/xhtml'>

    <body>
        <header>
            <h4 style='margin: 5pt; border-width: 1pt; border-color:aqua'>This is the header</h4>
        </header>

        <h1 style='margin:5pt;border-width:1pt; border-color:green'>This is the content</h1>

        <!-- 3 columns with inner block content -->
    
        <div style='column-count:3; margin:5pt; font-size:14pt; border-width: 1pt; border-color: navy'>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis orci mollis, finibus eros a,
            <!-- Truncated for brevity -->
            convallis hendrerit, tincidunt at sem. Integer finibus congue turpis eu feugiat. Nullam non ultrices enim.
            <br />
            <br />
            <!-- This will always be on a new column -->
            <div style="break-before: always;">
                Sed commodo metus id erat accumsan, quis feugiat augue vestibulum. Praesent porta sit amet erat a hendrerit.
                <!-- Truncated for brevity -->
                Quisque tincidunt nec quam sollicitudin consequat.
            </div>
            <div>
                Donec mattis eros non nibh mattis, in volutpat mi pretium. Donec mattis iaculis neque a accumsan. 
                <!-- Truncated for brevity -->
                vitae sem bibendum laoreet.
            </div>

            <div style="break-inside: avoid" >
                Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nam viverra laoreet 
                <!-- Truncated for brevity -->
                Nulla viverra sagittis leo.
            </div>
            
            <!-- this content will be kept together and then a column break will be put after it. -->
            <div style="break-inside: avoid; break-after: always" >
                Maecenas vitae vehicula mauris. Aenean egestas et neque sit amet pulvinar.
                <!-- Truncated for brevity -->
                nunc, blandit ut sem vitae, bibendum hendrerit ipsum.
            </div>
            <br />
            <br/>
            <div style="break-inside: avoid;" >
            Phasellus congue commodo elit, ac tincidunt ex placerat vitae. Lorem ipsum dolor sit amet, consectetur adipiscing 
            <!-- Truncated for brevity -->
            Ut sagittis sed mi nec tempus. Pellentesque et consectetur lorem, eget sagittis nibh. Cras vehicula ligula est. 
            Nulla viverra sagittis leo.
        </div>
        </div>
        <h1 style="margin:5pt; border-width:1pt; border-color: green">After the content</h1>

        <footer>
            <h4 style="margin: 5pt; border-width: 1pt; border-color: purple">This is the footer</h4>
        </footer>
    </body>

    </html>

.. image:: images/documentcolumns6.png

Column-width and column-gap
----------------------------

Rather than specifying the number of columns, scryber also supports the standard html column-width option.

This makes the width value the predominant driver, and will layout the maximum number of columns that are at least this width within the available space, so 
that it is full width.

e.g. if you have a container that is 300pts wide and a column-width of 80pt, then there will be 3 columns 
of about 92pts wide (assuming the alley / gap is the default 10pt). Increasing the column-width to 120pt, and the number of columns will
reduce to 2 of around 145pts.

If our page size or orientation changes then the number of columns fitted changes.

Column-gaps are the margins, or alleys, between each column. The default is 10pt, but it can be specified as a single unit value, e.g. 20pt or 5mm
(see :doc:`drawing_units` for more on scryber measurements).


.. code-block:: html

 <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
          "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css" >

            @page {
            size: A4 Landscape;
            }

        </style>
    </head>
    <body>
        <header>
            <h4 style='margin: 5pt; border-bottom-width: 1pt; border-bottom-color:aqua'>This is the header</h4>
        </header>
        <h1 style='margin:5pt;border-width:1pt; border-color:green'>This is the content</h1>
        <!-- Set a section to not break on the first page -->
        <div style='column-width: 150pt; column-gap:60pt; margin:5pt; font-size: 11pt; border-width: 1pt; border-color: navy'>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis orci mollis, finibus eros a,
            tincidunt magna. Mauris efficitur nisl lorem, vitae semper nulla convallis id. Nam dignissim rutrum
            ......
            <div style="break-before:always;" >
                In ac diam sapien. Morbi viverra ante non lectus venenatis posuere. Curabitur porttitor viverra augue sit amet
                convallis. Duis hendrerit suscipit vestibulum. Fusce fringilla convallis eros, in vehicula nibh tempor sed.
                ........
            </div>
            <div style="break-inside:avoid">
                Duis et tincidunt nisi. Etiam sed augue a turpis semper cursus. Proin facilisis feugiat risus, in malesuada
                lectus posuere eget. Nullam ultricies velit purus, vel lobortis felis commodo nec. Nam bibendum eleifend blandit.
                Vestibulum et turpis a metus euismod euismod nec sed nulla. Aliquam iaculis, magna in posuere finibus, turpis
                .......
            </div>
            <br/>
            In ac diam sapien. Morbi viverra ante non lectus venenatis posuere. Curabitur porttitor viverra augue sit amet
            convallis. Duis hendrerit suscipit vestibulum. Fusce fringilla convallis eros, in vehicula nibh tempor sed.
            Fusce gravida, orci eget venenatis hendrerit, augue erat euismod magna, nec interdum eros dolor sed ipsum.
            .....
            maximus efficitur quis velit. Phasellus et ante eget ex feugiat finibus ullamcorper ut nisl. Sed mi
            nunc, blandit ut sem vitae, bibendum hendrerit ipsum.
        </div>
        <h1 style="margin:5pt; border-width:1pt; border-color: green">After the content</h1>
        
        <footer>
            <h4 style="margin: 5pt; border-width: 1pt; border-color: purple">This is the footer</h4>
        </footer>
    </body>

    </html>

Here we can see that we have changed the paper orientation to landscape, set the column width to 150pt with gap of 60pt.
The layout engine adjusts all content automatically within the column widths.

The second div will always be on a new column, and the 3rd div moves to a new column as it cannot fit.
And the rest of the layout continues on the 3rd column until it reaches the end, and will flow onto another page.

.. image:: images/documentcolumns3.png


.. note:: As can be seen in the above image, scryber does not balance columns across the page (matching height). We may look to support this, but the min-height, max-height and breaks can be used to maintain the structure.

Images and Shapes in columns
-----------------------------

As with :doc:`component_sizing`, images and shapes that do not have an explicit size, take their natural width up to the size of the container.

This also applies to columns. If an image is too wide for the column it will be proportionally resized to fit within the column.
Any content can be placed in a column.

Nested containers and columns
------------------------------

Scryber fully supports nested columns whether that be at the page or multiple container level.
Again mixed content can be used within the columns, and the content will flow as normal.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css" >

            @page {
            size: A4 Landscape;
            }

        </style>
    </head>
    <body>
        <header>
            <h4 style='margin: 5pt; border-bottom-width: 1pt; border-bottom-color:aqua'>This is the header</h4>
        </header>
        <h1 style='margin:5pt;border-width:1pt; border-color:green'>This is the content</h1>
        <!-- Set a section to not break on the first page -->
        <div style="column-count: 3; column-gap:20pt; font-size:12px; padding:10pt;">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis orci mollis, finibus eros a,
            tincidunt magna. Mauris efficitur nisl lorem, vitae semper nulla convallis id. Nam dignissim rutrum
            .....
            <div style="column-count:2; border:solid 1px red; padding:10pt; margin:5pt 0pt;">
                <img src="./images/group.png" style="break-after:always;" />
                In ac diam sapien. Morbi viverra ante non lectus venenatis posuere. Curabitur porttitor viverra augue sit amet
                convallis. Duis hendrerit suscipit vestibulum. Fusce fringilla convallis eros, in vehicula nibh tempor sed.
                Fusce gravida, orci eget venenatis hendrerit, augue erat euismod magna, nec interdum eros dolor sed ipsum.
                
            </div>
            Duis et tincidunt nisi. Etiam sed augue a turpis semper cursus. Proin facilisis feugiat risus, in malesuada
            lectus posuere eget. Nullam ultricies velit purus, vel lobortis felis commodo nec. Nam bibendum eleifend blandit.
            ......
            elit maximus. Suspendisse non ultricies mi. Integer efficitur sapien lectus, non laoreet tellus dictum vel.
            Maecenas vitae vehicula mauris. Aenean egestas et neque sit amet pulvinar.<br />
            <img src="./images/group.png" style="margin:5pt;" />
            Maecenas vitae vehicula mauris. Aenean egestas et neque sit amet pulvinar.
            .......
            nunc, blandit ut sem vitae, bibendum hendrerit ipsum.
        </div>
        <h1 style="margin:5pt; border-width:1pt; border-color: green">After the content</h1>
        
        <footer>
            <h4 style="margin: 5pt; border-width: 1pt; border-color: purple">This is the footer</h4>
        </footer>
    </body>

    </html>


.. image:: images/documentcolumns5.png


