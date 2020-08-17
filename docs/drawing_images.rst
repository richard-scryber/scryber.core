======================================
Images in documents - td
======================================

Scryber supports most standard image types, including the following.

* GIF - 1 & 8 bit, 
* TIFF - 1, 8, 24 and 32 bit alpha, Indexed, RGB and CMYK.
* PNG  - 8, 24 and 32 bit alpha, RGB.
* JPEG - 24 bit RGB.

Adding an image to the output is as simple as putting an `<pdf:Image src='[path]' />` in the template.
If the image has an alpha channel this will automatically be applied to the image.

Image Source Paths.
=============================

The source for an image path can be referenced relative to the current file being parsed.

e.g. ../../Content/images/filename.ext

Or it can be the full url or full file path to the image.

e.g. http://localhost:5000/images/Toroid32.tiff


.. note:: If the image component is declared on a file referenced from the document, then the image path should still be relative to the file where the image component is declared. Not the top level document. 

Explicit image data
===================

Data can be set on the image explicitly when loaded, using the Scryber.Drawing.PDFImageData class.
This class has a range of static methods to load the images from existing bitmaps, local files or streams.

And the image data can then be assigned to an image component directly, or via an object parameter

Image Sizing
============

Without an explicit size the images will be rendered at the natural size, if it fits in the container. 

If it does not fit in the container it will be reduced in size to fit. 
See :doc:`component_sizing` for more about sizing images with widths and heights.


Rendering Images
=================

Once the path or data are set, it's just a case of rendering the document in the normal way.


.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Params>
        <!-- Parameters for the path and binary data -->
        <pdf:String-Param value="../../Content/Images/Toroid24.jpg" id="toroidPath" />
        <pdf:Object-Param id="toroidBin"/>
    </Params>
    <Styles>
        
        <!-- Add a style to images
             with a background and border -->
        <styles:Style applied-type="pdf:Image" >
        <styles:Border color="#666" style="Solid" width="2pt" />
        <styles:Background color="#AAA"/>
        <styles:Padding all="4pt"/>
        <styles:Margins bottom="4pt" top="4pt"/>
        </styles:Style>

        <!-- Div wrapper style -->
        <styles:Style applied-class="img-wrap">
        <styles:Columns count="3" alley-width="5pt"/>
        <styles:Font size="12pt"/>
        <styles:Position h-align="Center"/>
        </styles:Style>
    </Styles>
    <Pages>
        
        <pdf:Page styles:padding="40 10 20 10" >
        <Content>
        
        <pdf:Div styles:class="img-wrap" >
            
            <pdf:Span> PNG 24bit Image with no transparency</pdf:Span>
            <pdf:Image src="../../Content/Images/Toroid24.png" />
            <pdf:ColumnBreak/>

            <pdf:Span> PNG 32bit Image with alpha channel</pdf:Span>
            <pdf:Image src="../../Content/Images/Toroid32.png" />

        </pdf:Div>

        <pdf:Div styles:class="img-wrap">

            <pdf:Span> TIFF 24bit Image no transparency from url</pdf:Span>
            <pdf:Image src="http://localhost:5000/images/Toroid24.tiff" />
            <pdf:ColumnBreak/>
            
            <pdf:Span> TIFF 32bit Image with alpha from assigned path</pdf:Span>
            <pdf:Image id="tiff32" />

        </pdf:Div>

        <pdf:Div styles:class="img-wrap" >

            <pdf:Span> JPEG Image with the source set from a parameter</pdf:Span>
            <pdf:Image src="{@:toroidPath}" />
            <pdf:ColumnBreak/>

            <pdf:Span> JPEG Image with data set from object parameter</pdf:Span>
            <pdf:Image img-data="{@:toroidBin}" />
            
        </pdf:Div>
        
        </Content>
        </pdf:Page>
    </Pages>
    
    </pdf:Document>


.. code-block:: csharp

        public IActionResult ImageDocument()
        {
            // get the execution root, and path to the document.
            var root = _env.ContentRootPath;
            var path = System.IO.Path.Combine(root, "Views", "PDF", "DrawingImages.pdfx");
            path = System.IO.Path.GetFullPath(path);

            using(var doc = PDFDocument.ParseDocument(path))
            {
                var images = System.IO.Path.Combine(root, "Content", "Images");
                
                //Set the source path directly on the image
                (doc.FindAComponentById("tiff32") as PDFImage).Source = System.IO.Path.Combine(images, "Toroid32.tiff");

                //Set the source parameter to be used by an image
                doc.Params["toroidPath"] = System.IO.Path.Combine(images, "Toroid24.jpg");

                //Set the Image Data on a parameter from another bitmap or file
                var bmp = LoadImageBitmap();
                var data = PDFImageData.LoadImageFromBitmap("DynamicJpeg", bmp, false);
                
                doc.Params["toroidBin"] = data;

                return this.PDF(doc);
            }
        }

        private System.Drawing.Bitmap LoadImageBitmap()
        {
            //Example method that just returns an image from a file

            var path = _env.ContentRootPath;
            path = System.IO.Path.Combine(path, "Content", "Images", "Toroid24.jpg");

            return System.Drawing.Bitmap.FromFile(path) as System.Drawing.Bitmap;
        }


.. image:: images/documentimagesformat.png


Not found Images
================

If a path is set on an image but not resolved then by default scryber will raise an exception.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Styles>
        
        <!-- Add a style to images -->
        <styles:Style applied-type="pdf:Image" >
            <styles:Border color="#666" style="Solid" width="2pt" />
            <styles:Background color="#AAA"/>
            <styles:Padding all="4pt"/>
            <styles:Margins bottom="4pt" top="4pt"/>
        </styles:Style>

        <!-- Div wrapper style -->
        <styles:Style applied-class="img-wrap">
            <styles:Columns count="3" alley-width="5pt"/>
            <styles:Font size="12pt"/>
            <styles:Position h-align="Center"/>
        </styles:Style>
    </Styles>
    <Pages>
        
        <pdf:Page styles:padding="40 10 20 10" >
        <Content>
        
        <pdf:Div styles:class="img-wrap" >
            <!-- This will raise an exception -->
            <pdf:Span> Image that does not exist</pdf:Span>
            <pdf:Image src="../../Content/Images/NotFound.png" />
            

            <pdf:Span> PNG 32bit Image with alpha channel</pdf:Span>
            <pdf:Image src="../../Content/Images/Toroid32.png" />

        </pdf:Div>


        </Content>
        </pdf:Page>
    </Pages>
    
    </pdf:Document>

.. image:: images/documentimagesnotfound.png


This is the safest behaviour, but if this is not the desired behaviour, then there are a couople of options that
change the behaviour to allow missing images.

* On the image itself, if you know it may not be found, simply set the attribute allow-missing-images to true.
* Change the behaviour of scryber as a whole to allow missing images using the configuration options. See :doc:`scryber_configuration`

If the configuration is set to allow missing images, the attribute will override and cause an exception if set to false.
The attribute also supports binding, but is not styles based.

Images as backgrounds and fills
===============================

Images are also supported on the backgrounds of block level components (see :doc:`component_positioning`),
and of fills for shapes, text, etc.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Styles>
        <!-- Div style -->
        <styles:Style applied-class="img-wrap">
            <styles:Columns count="3" alley-width="5pt"/>
            <styles:Font size="12pt"/>
            <styles:Position h-align="Center"/>
            <!-- Set a backgrouns image for the div relative to the document -->
            <styles:Background img-src="../../Content/Images/Landscape.jpg"/>
        </styles:Style>
    </Styles>
    <Pages>
        
        <pdf:Page styles:padding="40 10 20 10" >
        <Content>
            <!-- Minimum height, so we can see the pattern -->
            <pdf:Div styles:class="img-wrap" styles:min-height="260pt" >
                <pdf:Span> Background image with default settings on the div</pdf:Span>
            </pdf:Div>
        
        </Content>
        </pdf:Page>
    </Pages>
    
    </pdf:Document>

.. image:: images/drawingImagesBackgrounds.png

The background has been drawn with the image repeating from the top left corner at its natural size, 
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


.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Styles>
        <!-- Div style -->
        <styles:Style applied-class="img-wrap">
        <styles:Font size="20pt" bold="true"/>
        <styles:Position h-align="Center"/>
        <styles:Border color="fuchsia"/>
        <styles:Background img-src="../../Content/Images/Landscape.jpg" x-size="60pt" />
        </styles:Style>

        <styles:Style applied-class="bg-pos">
        <styles:Margins top="5pt"/>
        <styles:Background x-pos="30pt" y-pos="-15pt" x-step="70pt" y-step="60pt" />
        <styles:Size min-height="100pt"/>
        </styles:Style>

        <styles:Style applied-type="pdf:Span">
        <styles:Fill color="fuchsia"/>
        </styles:Style>
    </Styles>
    <Pages> 
        
        <pdf:Page styles:padding="40 10 20 10" >
        <Content>

        <pdf:Div styles:class="img-wrap" styles:min-height="160pt" >
            <pdf:Span> Background image x-size only</pdf:Span>
        </pdf:Div>
        
        <pdf:Div styles:class="img-wrap bg-pos" styles:bg-repeat="RepeatX" >
            <pdf:Span> Background image with X repeat only</pdf:Span>
        </pdf:Div>

        <pdf:Div styles:class="img-wrap bg-pos" styles:bg-repeat="RepeatY" >
            <pdf:Span> Background image with Y repeat only</pdf:Span>
        </pdf:Div>

        <pdf:Div styles:class="img-wrap bg-pos" styles:bg-repeat="Fill" >
            <pdf:Span> Background image with Fill, overriding other settings</pdf:Span>
        </pdf:Div>
            
        </Content>
        </pdf:Page>
    </Pages>
    
    </pdf:Document>

.. image:: images/drawingImagesbgsize.png

Dynamic Images
==============