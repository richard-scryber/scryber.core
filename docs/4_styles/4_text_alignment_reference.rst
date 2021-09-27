================================
Textual Layout - PD
================================

FIGURE


Overview
--------

Generation methods
-------------------

All methods and files in these samples use the standard testing set up as outlined in :doc:`../overview/samples_reference`

Text Horizontal Alignment
--------------------------

Scryber supports the alignment of text at a block (rather than line level)

    * Left
    * Right
    * Center
    * Justified

The value is inherited so that child components will be aligned in the same way, unless explicitly set differently.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>Document Text H Align</title>
        <meta name="author" content="Scryber Team" />
        <style type="text/css">

            .std-font {
                font-size: 14pt;
                background-color: #AAA;
                padding: 4pt;
                margin-bottom: 10pt;
                font-family: sans-serif;
            }

        </style>
    </head>
    <body style="padding: 20pt">
        <div class="std-font" style="text-align:left;">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc pellentesque turpis ac pellentesque scelerisque.
            Etiam at nibh mattis, pulvinar velit eget, consequat ligula.
            Aenean sit amet nibh urna. Praesent odio magna, pharetra a posuere non, dignissim non lectus.
            Maecenas cursus porttitor sem vitae posuere. Phasellus quis lorem sapien. Aenean dictum pretium rutrum.
        </div>

        <div class="std-font" style="text-align:right">
            Proin id blandit ante, at pellentesque nulla. Fusce viverra, nibh eu sollicitudin euismod.
            Praesent aliquam gravida scelerisque. Pellentesque ac ante eu augue lacinia blandit nec vitae tellus.
            <div>Inner content</div>
            Nullam lacus dolor, mollis et orci vitae, ornare euismod turpis. Mauris tempus at orci id bibendum.
            Integer et aliquet velit. Proin eget ullamcorper libero. Sed bibendum mattis sem. Mauris in purus leo.
        </div>

        <div class="std-font" style="text-align:center">
            Phasellus dignissim risus vel nisi pellentesque dapibus. Vivamus at eros finibus, cursus mi eget, viverra elit.
            Integer non felis eget justo mollis aliquam. Donec sed pharetra odio.
            <div style="text-align:left">Left inner content</div>
            Fusce pulvinar elit leo, sit amet egestas neque porttitor nec.
            Nunc ac varius augue, ac rhoncus orci. Integer sit amet porta erat, vel scelerisque augue.
        </div>

        <div class="std-font" style="text-align:justify">
            Sed quis nibh libero. Vivamus euismod metus vel purus tristique, vitae gravida massa pretium.
            Proin facilisis arcu fringilla diam malesuada dictum.
            Praesent vel viverra nibh. Donec rhoncus nisl fermentum ante auctor consectetur.
            Proin posuere orci sed justo placerat elementum. Praesent cursus ullamcorper leo.
            Etiam ut massa lectus. Nunc dapibus tempus velit id tincidunt. Phasellus cursus finibus commodo.
        </div>
    </body>
    </html>


.. image:: images/documentTextHalign.png


Text Vertical Alignment
------------------------

The vertical alignment in text is also based on the container, and supports the following values.

 * top
 * middle
 * bottom

.. note:: The size of a block is normally shrunk to the size of the content, so vertical alignment has no visible effect. So a height must be set on the container.


.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>Document Text H Align</title>
        <meta name="author" content="Scryber Team" />
        <style type="text/css">

            .std-font {
                font-size: 14pt;
                background-color: #AAA;
                padding: 4pt;
                margin-bottom: 10pt;
                font-family: 'Hiragino Mincho', sans-serif;
            }

        </style>
    </head>
    <body style="padding: 20pt">

        <div class="std-font" style="vertical-align:top; height: 150pt">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc pellentesque turpis ac pellentesque scelerisque.
            Etiam at nibh mattis, pulvinar velit eget, consequat ligula.
            Aenean sit amet nibh urna. Praesent odio magna, pharetra a posuere non, dignissim non lectus.
            Maecenas cursus porttitor sem vitae posuere. Phasellus quis lorem sapien. Aenean dictum pretium rutrum.
        </div>

        <div class="std-font" style="vertical-align:middle; height: 150pt">
            Proin id blandit ante, at pellentesque nulla. Fusce viverra, nibh eu sollicitudin euismod.
            Praesent aliquam gravida scelerisque. Pellentesque ac ante eu augue lacinia blandit nec vitae tellus.
            Nullam lacus dolor, mollis et orci vitae, ornare euismod turpis. Mauris tempus at orci id bibendum.
            Integer et aliquet velit. Proin eget ullamcorper libero. Sed bibendum mattis sem. Mauris in purus leo.
        </div>

        <div class="std-font" style="vertical-align: bottom; height: 150pt">
            Phasellus dignissim risus vel nisi pellentesque dapibus. Vivamus at eros finibus, cursus mi eget, viverra elit.
            Integer non felis eget justo mollis aliquam. Donec sed pharetra odio.
            Fusce pulvinar elit leo, sit amet egestas neque porttitor nec.
            Nunc ac varius augue, ac rhoncus orci. Integer sit amet porta erat, vel scelerisque augue.
        </div>


    </body>
    </html>


.. image:: images/documentTextValign.png


Differences to HTML rendering
------------------------------

As mentioned the vertical and horizontal alignment are declared at the container level and apply to all content within.

This is in contrast to HTML that will genrally align on the element level and flow down. 
It is usually easy to replicate behaviour and visual style on both.


Alignment in code
----------------------------

Next Steps
----------

