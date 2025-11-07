======================================
Images as backgrounds and fills - PD
======================================

Images are also supported on the backgrounds of block level components (see :doc:`component_positioning`),
and of fills for shapes, text, etc.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css">

            div.bg {
                background-image: url("./images/landscape.jpg");
                min-height: 260px;
                text-align:center;
                color:white;
                font-family: sans-serif;
                font-size:larger;
                font-weight:bold;
                padding-top:10pt;
            }

        </style>
    </head>
    <body style="padding:20pt;">
        <div class="bg" style="">
            <span>Background image with the default settings</span>
        </div>

    </body>
    </html>

.. image:: images/drawingImagesBackgrounds.png

The background has been drawn with the image repeating from the top left corner at its natural size (or default 96ppi), 
clipped to the boundary of the container.

Along with specifying the image background, there are various other options for how the pattern is laid out
that will change the defaults of how the image repeats.

Background Size
-----------------

The background size option can either be a specific size, or 'cover' which will cover the entire container as a single image.

(Scryber does not currently support 'contain' but it's on our roadmap).

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css">

            div.bg {
                background-image: url("./images/landscape.jpg");
                min-height: 260px;
                text-align:center;
                color:#333;
                font-family: sans-serif;
                font-size:larger;
                font-weight:bold;
                padding-top:10pt;
                border:solid 1px #333;
            }

        </style>
    </head>
    <body style="padding:20pt;">

        <div class="bg" style="background-size: 40pt 40pt; color:white;">
            <span>Background image with explicit size</span>
        </div>
        <br/>
        <div class="bg" style="background-size:cover">
            <span>Background image with cover</span>
        </div>

    </body>
    </html>

.. image:: images/drawingImagesBackgroundSize.png

Background Repeat
-------------------

The options for the background repeating are: 

 * repeat - The default value, where the image repeats both X and Y directions.
 * repeat-x - The background will only repeat in the X (horizontal) direction.
 * repeat-y - The background will only repeat in the Y (vertical) direction.
 * none - The background will only be shown once.

These can be applied with a size, but will not affect anything if the size is cover.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css">

            div.bg {
                background-image: url("./images/landscape.jpg");
                min-height: 260px;
                text-align:center;
                font-family: sans-serif;
                font-size:larger;
                font-weight:bold;
                padding-top:10pt;
                border:solid 1px #333;
                /* consistent size across all */
                background-size: 60pt 60pt;
            }

        </style>
    </head>
    <body style="padding:20pt;">


        <div style="column-count:2; margin-bottom: 10pt; color:white;">
            <div class="bg" style="background-repeat:repeat; break-after:always;">
                <span>Background image with the default repeat</span>
            </div>
            <div class="bg" style="background-repeat:repeat-x">
                <span>Background image with repeat horizontal</span>
            </div>
        </div>

        <div style="column-count:2; color:#333;">
            <div class="bg" style="background-repeat:repeat-y; break-after:always;">
                <span>Background image with repeat vertical</span>
            </div>

            <div class="bg" style="background-repeat:no-repeat">
                <span>Background image with no repeating</span>
            </div>

        </div>
    </body>
    </html>


.. image:: images/drawingImagesBackgroundRepeat.png

Background Position
---------------------------

* The starting position of the pattern.
    * x-pos - Determines the horizontal offset of the rendered background image in units.
    * y-pos - Determines the vertical  offset of the rendered background image in units.
* The pattern repeat step.
    * x-step - Sets the horizontal offset between repeating patterns, which can be more or less than the size of the rendered image.
    * y-step - Sets the vertical offset between repeating patterns, which can be more or less than the size of the rendered image.


.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css">

            div.bg {
                background-image: url("./images/landscape.jpg");
                min-height: 260px;
                text-align: center;
                font-family: sans-serif;
                font-size: larger;
                font-weight: bold;
                padding-top: 10pt;
                border: solid 1px #333;
                /* consistent size across all */
                background-size: 60pt 60pt;
            }
        </style>
    </head>
    <body style="padding:20pt;">


        <div style="column-count:2; margin-bottom: 10pt; color:white;">
            <!-- Position value for x and y -->
            <div class="bg" 
                style="background-repeat:repeat; 
                        background-position: 20pt 20pt; 
                        break-after:always;">
                <span>Background image with the default repeat at 20,20</span>
            </div>
            <!-- Single value should be applied to both x and y -->
            <div class="bg" 
                style="background-repeat:repeat-x; 
                        background-position: 20pt">
                <span>Background image with repeat horizontal at 20,20</span>
            </div>
        </div>

        <div style="column-count:2; color:#333;">
            <!-- x and y as individual properties -->
            <div class="bg"
                style="background-repeat: repeat-y;
                        background-position-x: 20pt;
                        background-position-y: 40pt;
                        break-after: always;">
                <span>Background image with repeat vertical at 20,20</span>
            </div>
            <!-- Single repeat with a bakground color -->
            <div class="bg" 
                style="background-repeat: no-repeat;
                        background-position: 150pt 100pt; 
                        background-color: aquamarine">
                <span>Background image with no repeating at 150,100 and background color</span>
            </div>

        </div>
    </body>
    </html>

.. image:: images/drawingImagesBackgroundPosition.png

Images as fills
-------------------

Scryber also supports images as fills. See the SVG documentation for this.

