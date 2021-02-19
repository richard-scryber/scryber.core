======================================
Images as backgrounds and fills
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
that will change the defaults of how the image repeats. Only the background repeat is available on the
component itself, the other 

* The Repeat - 'repeat' or 'styles:bg-repeat' on the component.
    * None - The background will only be shown once.
    * RepeatX - The background will only repeat in the X (horizontal) direction.
    * RepeatY - The background will only repeat in the Y (vertical) direction.
    * Both - The default value, where the image repeats both X and Y directions.
    * Fill - The image will only be shown once, but fill the available container size **(also overrides any of the following size options)**.
* The size of the image of the rendered image.
    * x-size - Determines the vertical height of the rendered background image in units.
    * y-size - Determines the vertical height of the rendered background image in units.
* The starting position of the pattern.
    * x-pos - Determines the horizontal offset of the rendered background image in units.
    * y-pos - Determines the vertical  offset of the rendered background image in units.
* The pattern repeat step.
    * x-step - Sets the horizontal offset between repeating patterns, which can be more or less than the size of the rendered image.
    * y-step - Sets the vertical offset between repeating patterns, which can be more or less than the size of the rendered image.


.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Styles>
        <!-- Div style -->
        <styles:Style applied-class="img-wrap">
        <styles:Font size="20pt" bold="true"/>
        <styles:Position h-align="Center"/>
        <styles:Border color="fuchsia"/>
        <!-- x-size (or y-size) alone will keep the natural proportions of the image -->
        <styles:Background img-src="../../Content/Images/Landscape.jpg" x-size="60pt" />
        </styles:Style>

        <styles:Style applied-class="bg-pos">
        <styles:Margins top="5pt"/>
        <!-- aplying a specific stating position and step -->
        <styles:Background x-pos="30pt" y-pos="-15pt" x-step="70pt" y-step="60pt" />
        <styles:Size min-height="100pt"/>
        </styles:Style>

        <styles:Style applied-type="doc:Span">
        <styles:Fill color="fuchsia"/>
        </styles:Style>
    </Styles>
    <Pages> 
        
        <doc:Page styles:padding="40 10 20 10" >
        <Content>

        <doc:Div styles:class="img-wrap" styles:min-height="160pt" >
            <doc:Span> Background image x-size only</doc:Span>
        </doc:Div>
        
        <doc:Div styles:class="img-wrap bg-pos" styles:bg-repeat="RepeatX" >
            <doc:Span> Background image with X repeat only</doc:Span>
        </doc:Div>

        <doc:Div styles:class="img-wrap bg-pos" styles:bg-repeat="RepeatY" >
            <doc:Span> Background image with Y repeat only</doc:Span>
        </doc:Div>

        <doc:Div styles:class="img-wrap bg-pos" styles:bg-repeat="Fill" >
            <doc:Span> Background image with Fill, overriding other settings</doc:Span>
        </doc:Div>
            
        </Content>
        </doc:Page>
    </Pages>
    
    </doc:Document>

.. image:: images/documentimagesbgsize.png

Images as fills
===============

An image can also be used as the fill for text or shapes. It has the same properties and options as 
the background. But will be trimmed around the shape of the component it is filling.

The background and fill are also independent, so can be used together for multiple patterns 
as in the rectangle in the example below.


.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Styles>
        <!-- Div style -->
        <styles:Style applied-class="img-wrap">
            <styles:Font size="40pt" bold="true"/>
            <styles:Position h-align="Center"/>
            <styles:Border color="fuchsia"/>
            <!-- x-size (or y-size) alone will keep the natural proportions of the image -->
            <styles:Fill img-src="../../Content/Images/Landscape.jpg" x-size="60pt" />
        </styles:Style>

        <styles:Style applied-class="fill-pos">
            <styles:Margins top="5pt"/>
            <!-- aplying a specific stating position and step -->
            <styles:Fill x-pos="30pt" y-pos="-15pt" x-step="70pt" y-step="60pt" />
            <styles:Size min-height="100pt"/>
        </styles:Style>

        
    </Styles>
    <Pages>

        <doc:Page styles:padding="40 10 20 10" >
        <Content>

            <doc:Div styles:class="img-wrap" styles:min-height="100pt" >
                <doc:Span> Filled image x-size only</doc:Span>
            </doc:Div>

            <doc:Div styles:class="img-wrap fill-pos" styles:fill-repeat="RepeatX" >
                <doc:Span> Filled image with X repeat only</doc:Span>
            </doc:Div>

            <doc:Div styles:class="img-wrap fill-pos" styles:fill-repeat="RepeatY" >
                <doc:Span> Filled image with Y repeat only</doc:Span>
            </doc:Div>

            <doc:Div styles:class="img-wrap fill-pos" styles:fill-repeat="Fill" >
            <!-- Fill repeat doesn't work at the moment. We are loking at it.-->
                <doc:Span> Filled image with Fill, overriding other settings</doc:Span>
            </doc:Div>

            <!-- A shape with a fill and background image -->
            <doc:Rect styles:position-mode="Absolute" styles:class="img-wrap"
                        styles:bg-image="../../Content/Images/group.png" styles:padding="20"
                        styles:x="360" styles:y="300" styles:width="120" styles:height="120pt" ></doc:Rect>
        </Content>
        </doc:Page>
    </Pages>

    </doc:Document>

.. image:: images/documentimagesfills.png


.. note:: The Fill repeat option on the shape or text fill does not currently work. Use the sizing options (for the moment) to replicate the Fill repeat pattern.



Dynamic Images
==============

Sometimes it's not possible to reference an image file, or practical to reference image data in parameters.
There could be a standard source of image data, that you want to use, not directly supported by scryber.

In this case, the best option is to use dynamic image factories. 

With an image factory in the configuration options, any class supporting the IPDFImageDataFactory interface can return a 
dynamic image to the scryber layout engine.

.. code-block:: c#

    using System;
    using Scryber.Drawing;
    using System.Drawing;

    namespace Scryber.Mocks
    {
        //Must implement the IPDFImageDataFactory interface

        public class MockImageFactory : IPDFImageDataFactory
        {
                
            public bool ShouldCache { get { return false; } }

            public PDFImageData LoadImageData(IPDFDocument document, IPDFComponent owner, string path)
            {
                
                try
                {
                    var uri = new Uri(path);
                    var param = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
                    var name = System.IO.Path.GetFileNameWithoutExtension(param);

                    // Standard System.Drawing routines to draw a bitmap with the name on.
                    // Could load an image from remote source, use doc parameters, whatever is needed

                    Bitmap bmp = new Bitmap(300, 100);
                    using (Graphics graphics = Graphics.FromImage(bmp))
                    {
                        graphics.FillRectangle(new SolidBrush(Color.LightBlue), new Rectangle(0, 0, 300, 100));
                        graphics.DrawString(name, new Font("Times", 12), new SolidBrush(Color.Blue), PointF.Empty);
                        graphics.Flush();
                    }
                    
                    PDFImageData data = PDFImageData.LoadImageFromBitmap(path, bmp, false);
                    return data;
                }
                catch(Exception ex)
                {
                    throw new ArgumentException("The image creation failed", ex);
                }
            }
        }
    }


For the app settings specify the Factory with a regular expression match on the path 
(in this case '[anything].dynamic', and then specify the type and assembly where the class is defined.

See :doc:`scryber_configuration` for more details on changing the configuration options.

.. code-block:: json

    {
        "Scryber": {
            "Imaging": {
            "AllowMissingImages": "True",
            "ImageCacheDuration": 60,
            "Factories": [
                {
                "Match": ".*\\.dynamic",
                "FactoryType": "Scryber.Mocks.MockImageFactory",
                "FactoryAssembly": "Scryber.UnitTests"
                }
            ]
            }
        }
    }

And then in your template simply specify the image matching the pattern, to invoke the Image Data Factory.

.. code-block:: xml

    <?xml version='1.0' encoding='utf-8' ?>
    <doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                xmlns:styles='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                xmlns:data='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' >
    <Pages>

        <doc:Page styles:margins='20pt'>
        <Content>
            <doc:Span>This is before the image</doc:Span>

            <!-- A dynamic image that will be generated on the fly -->
            <doc:Image id='LoadedImage' src='This+is+an+image.dynamic' />

            <doc:Span>This is after the image</doc:Span>

        </Content>
        </doc:Page>
    </Pages>

    </doc:Document>


.. image:: images/documentimagesdynamic.png


.. note:: Only one instance of the image factory will be created, and it MUST have a parameterless constructor.
