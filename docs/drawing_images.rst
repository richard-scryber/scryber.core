======================================
Images in documents
======================================

Scryber supports most standard image types, including the following.

* GIF - 1 & 8 bit, 
* TIFF - 1, 8, 24 and 32 bit alpha, Indexed, RGB and CMYK.
* PNG  - 8, 24 and 32 bit alpha, RGB.
* JPEG - 24 bit RGB.

Adding an image to the output is as simple as putting an `<img src='[path]' />` in the template.
If the image has an alpha channel this will automatically be applied to the image.

Image Source Paths.
---------------------

The source for an image path can be referenced relative to the current file being parsed.

e.g. ../../Content/images/Toroid32.tiff

Or it can be the full url or full file path to the image.

e.g. http://localhost:5000/images/Toroid32.tiff


.. note:: If the image component is declared on a file referenced from the document, 
          then the image path should still be relative to the file where the image component is declared. Not the top level document. 

Explicit image data
-------------------

Data can be set on the image explicitly when loaded, using the Scryber.Drawing.PDFImageData class.
This class has a range of static methods to load the images from existing bitmaps, local files or streams.

And the image data can then be assigned to an image component directly.

.. code-block:: csharp

    using (var stream = GetMyImageData())
    {
        var img = doc.FindAComponentById("myImage") as Image;
        if (null != img)
            img.Data = PDFImageData.LoadImageFromStream("IdentifyingKeyForImage", mybitmapstream);
    }

    //Other overloads

    //PDFImageData.LoadImageFromBitmap();
    //PDFImageData.LoadImageFromLocalFile();
    //PDFImageData.LoadImageFromURI();

Binding Image data
-------------------

Binding to parameter data is also supported in the img tag using the data-img attribute.

.. code-block:: html

    <img alt='Data bound image' data-img='{@:model.imagedata}' >

And this can be loaded and set in the usual way.

.. code-block:: csharp

    using (var stream = GetMyImageData())
    {
        var model = new {
            imageData = PDFImageData.LoadImageFromStream("IdentifyingKeyForImage", mybitmapstream)
        };
        doc.Params["model"] = model;
    }


Image Sizing
--------------

Without an explicit size the images will be rendered at the natural size, if it fits in the container. 

If it does not fit in the container it will be reduced in size proportionally to fit. 
Setting a height or width will also constrain the image proportionally. If both are set then the image will use these and it
may no longer be proportional.

See :doc:`component_sizing` and :doc:`document_columns` for more about sizing images with widths and heights.


Pulling it together
--------------------

Once the path or data are set, it's just a case of rendering the document in the normal way.


.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css">

            img.toroid{
                border: solid 2pt #666;
                background-color:#AAA;
                padding: 4pt;
                margin: 10pt 0;
                break-after:always;
            }

            div.wrap {
                column-count:3;
                font-size:12pt;
                text-align:center;
            }

        </style>
    </head>
    <body style="padding:20pt;">
        <div class="wrap">
            <span>PNG 24bit Image with no transparency</span>
            <img class="toroid" src="./images/Toroid24.png" alt="24 bit PNG file" />

            <span>PNG 32bit Image with transpart background</span>
            <img class="toroid" src="./images/Toroid32.png" alt="32 bit PNG file" />
        </div>

        <div class="wrap">
            <span>TIFF 24bit Image with no transparency from url</span>
            <img class="toroid"
                src="https://raw.githubusercontent.com/richard-scryber/scryber.core/master/docs/images/Toroid24.tiff"
                alt="24 bit Tiff file" />

            <span>TIFF 32bit Image with transparent background</span>
            <img id="tiff32" class="toroid" alt="32 bit TIFF file" />
        </div>

        <div class="wrap">
            <span>JPEG 24bit Image with source path from model</span>
            <img class="toroid" src="{@:model.jpgSrc}" alt="24 bit JPEG file" />

            <span>JPEG 24bit Image with image data from model</span>
            <img class="toroid" data-img="{@:model.jpgData}" alt="32 bit JPEG file" />
        </div>

    </body>
    </html>


.. code-block:: csharp

        var path = System.Environment.CurrentDirectory;
        var docPath = System.IO.Path.Combine(path, "../../../Content/HTML/documentation.html");

        using (var doc = Document.ParseDocument(docPath))
        {
            //pass paramters as needed, supporting simple values, arrays or complex classes.
            var img = doc.FindAComponentById("tiff32") as Image;

            if(null != img)
                img.Source = System.IO.Path.Combine(path, "../../../Content/HTML/Images/Toroid32.tiff");

            var jpgSrc = System.IO.Path.Combine(path, "../../../Content/HTML/Images/Toroid24.jpg");
            var jpgData = PDFImageData.LoadImageFromLocalFile(jpgSrc);

            var model = new
            {
                jpgSrc = jpgSrc,
                jpgData = jpgData
            };

            doc.Params["model"] = model;

            using (var stream = DocStreams.GetOutputStream("documentation.pdf"))
            {
                doc.SaveAsPDF(stream); 
            }
        }


.. image:: images/documentimagesformat.png

.. note:: Pre v5.0.5 of the package, the imgate data attribute was img-data. This will continue to work, but is considered obselete and the preferred attribute is data-img.

Not found Images
-----------------

If a path is set on an image but not resolved then scryber will simply treat the flow as normal. 

    
.. code-block:: html

    <div class="wrap">
        <span>Replacing with a non-existant image.</span>
        <img class="toroid" src="DoesnotExist.png" alt="24 bit PNG file" />

        <span>PNG 32bit Image with transpart background</span>
        <img class="toroid" src="./images/Toroid32.png" alt="32 bit PNG file" />
    </div>

.. image:: images/documentimagesnotfound.png


There will however, if we switch them on, be an error in the **always useful** logs

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <?scryber append-log='true' ?>

.. image:: images/documentimagesnotfound_log.png


Enforcing not found exceptions
------------------------------


This is the safest behaviour, but if this is not the desired behaviour, then there are a couple of options that
change the behaviour to allow missing images.

* On the image itself, if you know it may not be found, simply set the attribute data-allow-missing-images to false.
* On the document as a whole with the AllowMissingImages property.
* Change the behaviour of scryber as a whole to disallow missing images using the configuration options. See :doc:`scryber_configuration`

.. code-block:: html

    <img class="toroid" data-allow-missing-images="false" src="DoesnotExist.png" alt="24 bit PNG file" />

.. code-block:: csharp

    doc.RenderOptions.AllowMissingImages = false;

If either are set to false an exception will be raised (i.e. it is not possible to disallow images at the document level, but allow on some images).
