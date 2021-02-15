==============================
Sizing your content
==============================

Scryber has an intelligent layout engine. By default eveything will be laid out as per the flowing layout of the document Pages and columns.
Each component, be it block level or inline will have a position next to its siblings and move and following content along in the document.
If the content comes to the end of the page and cannot be fitted, then if allowed, it will be moved to the next page.

However it is very easy to size and position (see :doc:`component_positioning`) the content. All measusements are using the scryber unit and thickness
(see :doc:`drawing_units` for more on the use of measurements and dimensions).

Width and Height
------------------

All block components support an explicit width and / or height value. If it's width is set, then any full-width style will be ignored.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css" >

            .bordered {
                border: solid 1px #777;
                background-color: #EEE;
                padding:4pt;
            }

        </style>
    </head>
    <body>
        <div class="bordered">
            The content of this div is all as a block (by default)
        </div>

        <div class="bordered" style="width:300pt">
            The content of this div is set to 300pt <u>wide</u>, so the content will flow within this width,
            and grow the height as needed.
        </div>

        <div class="bordered" style="height:150pt">
            The content of this div is set to 150pt <u>high</u>, so the content will flow within this
            as full width, but the height will still be 150pt.
        </div>

        <div class="bordered" style="width:300pt; height:150pt">
            The content of this div is set to 300pt <u>wide</u> and 150pt <u>high</u>, so the content will flow within this
            as full width, but the height will still be 150pt.
        </div>
    </body>

    </html>

.. image:: images/documentsizing.png

Images with width and height
-----------------------------

Scryber handles the sizing of images based on the natural size of the image. If no explicit size or positioning is provided, then it will be rendered
at the native size for the image.

If the available space within the container is not sufficuent to hold the image at it's natural size, then the image render size will be reduced
proportionally to fit the space available.

If either a width **or** height is assigned, then this will be used to proportionally resize the image to that height or width.

If both a width **and** height are assigned, then they will both be used to fit the image to that space. No matter what the originals' proportions are.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css" >

            body{ padding: 20pt;}

            .bordered { font-size:14pt; }

            .columns{ column-count: 4; }

            .columns div.bordered{ break-after:always; }

        </style>
    </head>
    <body>
        <div class="bordered" style="margin:30pt;">
            An image will natually size to it's dimensions without space restriction.
            <img src="./images/landscape.jpg" />
        </div>
        <div class="columns" style="column-count: 4">
            <div class="bordered">
                <b>First Column</b><br />
                An image will fit to it's container if no explicit size is set.
                <img src="./images/landscape.jpg" />
            </div>
            <div class="bordered">
                <b>Second Column</b><br />
                If a width is set, then the sizing will be proportional.
                <img src="./images/landscape.jpg" style="width:100pt;" />
            </div>

            <div class="bordered" >
                <b>Third Column</b><br />
                If a height is set, then the sizing will be proportional.
                <img src="./images/landscape.jpg" style="height:50pt;" />
            </div>

            <div class="bordered" >
                <b>Fourth Column</b><br />
                If a width and height are set these will be used explicitly.
                <img src="./images/landscape.jpg" style="width:100pt; height:50pt;" />
            </div>
        </div>

        <!-- Photo by Bailey Zindel on Unsplash -->
    </body>

    </html>

.. image:: images/documentsizingimages.png


Margins and Padding
--------------------

All block level elements support padding and margins.
Unlike html, scryber does not count the width of the border as part of the box dimensions (on purpose).

Dimensions can be set either directly on the component, or on a style applied to the components (see: :doc:`document_styles`).

The `Margin` and `Padding` style have the 4 individual properties that can also be set.

* Top
* Right
* Bottom
* and Left

If an individual side property is set, then this will override any value set on all.

The margins or padding attributes on tags can alsp be set with 1, 2 or 4 values. If only one is provided it will be applied to each.
If 4 are provided, they will be applied to each individual value in the `top`, `right`, `bottom`, `left` (as per html padding). If 
2 are provided the first will be applied to the top and bottom, the second to the left and right.

.. note:: If any margins or padding attribute is set on the component, it will override ALL values set in any style.

If not set then the values will be zero.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css">

            body {
                margin: 20pt;
                font-size:12pt;
            }

            .bordered {
                border-style: solid;
                border-width: 1pt;
                border-color: #777;
                background-color: #EEE;
            }

            .red {
                border-color: #F00;
            }

            .spaced {
                margin: 20pt;
                margin-left: 10pt;
                margin-right: 10pt;
                padding: 5pt;
            }
        </style>
    </head>
    <body class="bordered">

        <b>First Example</b>
        <div class="bordered red">
            The content of this div has a red border with no padding or margins.
        </div>

        <b>Second Example</b>
        <div class="bordered red spaced">
            The content of this div has a red border with both margins and padding set from the style.
        </div>

        <b>Third Example</b>
        <div class="bordered red spaced" style='padding:20pt;'>
            The content of this div has a red border with margins set from the style and padding overridden explicitly on the component.
        </div>

        <b>Borders are supported on images and other blocks too, and will respect the width and or height properties.</b>
        <img src="./Images/landscape.jpg" class="bordered spaced" style="width:100pt" />
        <h1 class="bordered spaced">Heading with spacing.</h1>

    </body>
    </html>

.. image:: images/documentsizingmargins.png


Minimum and Maximum size
-------------------------

Along with the use of width and height, scryber also supports the use of minimum height/width and maximum height/width.

As you might expect, the minimum will ensure that a container is at least as big as the specified value, and that the maximum will 
ensure the content, never grows beyond that specified value.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css">

            body {
                margin: 20pt;
                font-size:12pt;
            }

            .bordered {
                border-style: solid;
                border-width: 1pt;
                border-color: #777;
                background-color: #EEE;
            }

            .red {
                border-color: #F00;
            }

            .spaced {
                margin: 20pt;
                margin-left: 10pt;
                margin-right: 10pt;
                padding: 5pt;
            }

            .sized{
                max-height:60pt;
                max-width:350pt;
            }
        </style>
    </head>
    <body class="bordered">
        <br />
        <b>Minimum Size, not reached</b>
        <div class="bordered red" style="min-height:60pt; min-width:350pt">
            This div has a red border with min size.
        </div>
        <br />
        <b>Minimum Size, width reached</b>
        <div class="bordered red" style="min-height:60pt; min-width:350pt">
            This div has a red border with min size, but the content will push this out beyond the minimum width.
        </div>
        <br />
        <b>Minimum Size, width reached</b>
        <div class="bordered red" style="min-height:60pt; min-width: 350pt">
            This div has a red border with min size, but the content will push this out beyond the minimum width to the
            space in the container, and then flow as normal.
        </div>
        <br />
        <b>Maximum Size, not reached</b>
        <div class="bordered red sized">
            This div has a red border with max size.
        </div>
        <br />
        <b>Maximum Size, width reached</b>
        <div class="bordered red sized">
            This div has a red border with max size, and the content will flow as the max-width is reached with the text.
        </div>

    </body>
    </html>


.. image:: images/documentsizingminmax.png
