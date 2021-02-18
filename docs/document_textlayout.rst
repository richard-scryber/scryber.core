================================
Textual Layout - td
================================

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


.. image:: images/documentTextHAlign.png


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


.. image:: images/documentTextVAlign.png


Differences to HTML rendering
------------------------------

As mentioned the vertical and horizontal alignment are declared at the container level and apply to all content within.

This is in contrast to HTML that will genrally align on the element level and flow down. 
It is usually easy to replicate behaviour and visual style on both.


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

    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >

    <Styles>
        
        <!-- Add a style to images -->
        <styles:Style applied-type="doc:Div" applied-class="std-font" >
            <styles:Background color="#AAA"/>
            <styles:Padding all="4pt"/>
            <styles:Margins bottom="10pt" />
        </styles:Style>

        <!-- Alter the default bold component -->
        <styles:Style applied-type="doc:B">
            <styles:Font size="20pt" italic="true"/>
            <!-- Adding character and word spacing too -->
            <styles:Text char-spacing="5pt" word-spacing="10pt"/>
        </styles:Style>

        <styles:Style applied-class="narrow" >
            <styles:Text char-spacing="-0.5pt"/>
        </styles:Style>

        <styles:Style applied-class="wide" >
            <styles:Text char-spacing="1.5pt" leading="15pt"/>
        </styles:Style>

        <styles:Style applied-class="wide-word" >
            <styles:Text char-spacing="0" word-spacing="8pt" />
        </styles:Style>
    </Styles>
    <Pages>
        
        <!-- Setting the font on the page, rather than at each level. -->
        <doc:Page styles:padding="10" styles:font-family="Segoe UI" >
        <Content>
            <doc:Div styles:column-count="3" styles:font-size="10pt">
                <doc:Div styles:class="std-font narrow" >
                    Segoe UI in 10pt font size with the default
                    leading used on each line of the paragraph. But the character spacing is reduced by 0.5 points.
                </doc:Div>
                <doc:ColumnBreak/>
                <doc:Div styles:class="std-font wide">
                    Segoe UI in 10pt font size with the leading increased to 15pt
                    on each line of the paragraph. The character spacing is also
                    set to an extra 1.5 points.
                </doc:Div>
                <doc:ColumnBreak/>
                <doc:Div styles:class="std-font wide-word" >
                    Segoe UI in 10pt font size with the leading and character space normal, but the word
                    spacing increased by 5 points. It should continue to flow nicely onto multiple lines.
                </doc:Div>
            </doc:Div>

            <doc:Div styles:class="std-font wide" styles:text-leading="35pt" >
                Even using various 
                <doc:Span styles:font-size="30" styles:font-family="Comic Sans MS">font sizes and families</doc:Span>
                will maintain the character and 
                word spacing that <doc:B>has been applied.</doc:B>
            </doc:Div>
        </Content>
        </doc:Page>
    </Pages>
    
    </doc:Document>


.. image:: images/drawingfontsSpacing.png