==================================
Textual wrapping and spacing - PD
==================================






Line leading
------------

TODO

Word wrapping
--------------

By default, scryber will wrap text around the available space and flow evenly across the page, no matter the content in the source.

If this is not the desired behaviour, then the css attributes for white-space are supported.

 * nowrap - will ignore white space, AND not wrap the content when the outer edge is reached.
 * pre - will take all white space into account and render content as seen.

 The layout also supports the use of overflow-x and overflow-y to clip the visibility to the bounds of the container.
 (Scroll is not supported).

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
        <div class="std-font" style="white-space: nowrap">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit.Fusce pulvinar elit leo, sit amet egestas neque porttitor nec.
            Nunc pellentesque turpis ac pellentesque scelerisque.

            Etiam at nibh mattis, pulvinar velit eget, consequat ligula.
        </div>

        <div class="std-font" style="white-space: nowrap">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit.Fusce pulvinar elit leo, sit amet egestas neque porttitor nec.<br />
            Nunc pellentesque turpis ac pellentesque scelerisque.<br />

            Etiam at nibh mattis, pulvinar velit eget, consequat ligula.<br />
        </div>

        <div class="std-font" style="white-space:pre; overflow-x:hidden;">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit.Fusce pulvinar elit leo, sit amet egestas neque porttitor nec.
        Nunc pellentesque turpis ac pellentesque scelerisque.

        Etiam at nibh mattis, pulvinar velit eget, consequat ligula.
        </div>

    </body>
    </html>

.. image:: images/documentTextPre.png


Character and Word Spacing
--------------------------

With scryber the character and word spacing is supported at the style definition level (not on the component attributes). 
They are less frequently used, but can help in adjusting fonts that are too narrow at a particular size, or for graphical effect.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>Document Character spacing</title>
        <meta name="author" content="Scryber Team" />
        <style type="text/css">

            .std-font {
                font-size: 14pt;
                background-color: #AAA;
                padding: 4pt;
                margin-bottom: 10pt;
                font-family: 'Segoe UI', sans-serif;
            }

            .narrow{ letter-spacing:-0.5pt;}

            .wide{ letter-spacing:1.5pt; line-height:15pt; }

            .wide-word{ letter-spacing: 0; word-spacing: 10pt; }

        </style>
    </head>
    <body style="padding: 20pt">
        <div style="column-count:3;font-size:10pt">
            <div class="std-font narrow" style="break-after:always">
                Segoe UI in 10pt font size with the default
                leading used on each line of the paragraph. But the character spacing is reduced by 0.5 points.
            </div>
            <div class="std-font wide" style="break-after:always">
                Segoe UI in 10pt font size with the leading increased to 15pt
                on each line of the paragraph. The character spacing is also
                set to an extra 1.5 points.
            </div>
            <div class="std-font wide-word">
                Segoe UI in 10pt font size with the leading and character space normal, but the word
                spacing increased by 5 points. It should continue to flow nicely onto multiple lines.
            </div>
        </div>

        <div class="std-font wide" style="line-height:30pt;" >
            Even using various
            <span style="font-size:30pt; font-family:Optima, serif;">font sizes and families</span>
            will maintain the character and
            word spacing that <b>has been applied.</b>
        </div>

    </body>
    </html>


.. image:: images/drawingfontsSpacing.png

.. note:: There is a known issue with the baseline adjust on multiple font sizes that has crept in, and will hopefilly be resolved in the next release.


Wrapping and spacing in code
----------------------------

Next Steps
----------
