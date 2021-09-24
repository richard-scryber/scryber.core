==============================================
Splitting into multiple files
==============================================

For large documents or projects, it's often easier to split your templates into multiple files.
These can be separate stylesheets, pages, components and the top level document.

As a converions the files should have the following extensions.

* Documents - [MyTemplate].html
* Stylesheets - [MyStyles].css
* Pages and content - [MyInnerContent].html

It just makes life easier.


4 file example
---------------

As an example we can split a single document into 4 files.
Here we will take the top level document and reference a stylesheet, a page header component and a cover page.


DocumentRefs.html
-----------------

At the top level is the Document - `DocumentRefs.html`

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <?scryber append-log='true' log-level='verbose' ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>My multi file document</title>
        <!-- Stylesheet links -->
        <link rel="stylesheet" media="screen" href="./css/includeScreen.css" />
        <link rel="stylesheet" media="print" href="./css/includePrint.css" />
    </head>
    <body style="margin:20pt; font-size:20pt">
        <header>
            <embed src="./fragments/pageheader.html" />
        </header>    

        <section style="page-break-before:avoid; page-break-after:always;">
            <embed src="./fragments/coverpage.html">
        </section>

        <div>
            <h1 class="title" >This is the second page</h1>
        </div>

    </body>
    </html>

This contains a reference to `includeScreen.css` and `includePrint.css` in the `css` folder.
As the first link is specified as for screen only it will not be loaded, and only the includePrint.css will be loaded.


An embedded reference to a `PageHeader.html` in the `Fragments` folder for a standard document header,
and a reference to a `CoverPage.html` in the `Fragments` folder for the cover page content.

The path references are relative to the current document (but could be absolute urls)

includePrint.css
-----------------

This is the content of the `includePrint.css`

.. code-block:: css

    .title{
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        font-weight:bold;
        font-size:60pt;
        margin: 20pt;
        padding:10pt;
        text-align:center;
    }

    .page-head {
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        font-weight: normal;
        font-size: 14pt;
        margin: 20pt 10pt 10pt 10pt;
        padding: 10pt;
        border-bottom:solid 1pt black;
    }

This file declares 2 style classes that can be applied to any element with class names `title` and `page-head`
For more info about styles see :doc:`document_styles`

CoverPage.html
---------------

This is the content of the `CoverPage.html`, which will be directly included in the content of the document, so should not start with the HTML of body tag, 
but go directly to the actual content used.

As this is intended to be the first page, and always a page, the page-break-before and page-break-after have been switched.

The namespace is important on includes, just as with top-level documents, the namespace is **critical** 


.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <div xmlns='http://www.w3.org/1999/xhtml' >
        <h1 class="title">Heading Page</h1>
    </div>

.. note:: These are just samples and can be as complex as you like, but to be good xml it should still only have a single root.

PageHeader.html
----------------

The component is referenced from link in the `DocumentRefs.pdfx`.
This file is just used as the content for the header of the pages.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <div  xmlns='http://www.w3.org/1999/xhtml' class="page-head" style="column-count:2">
        <span class="head-text" style="break-after:always;" >Referenced File Example</span>
        <time date-format="dd MMM yyyy" />
    </div>


The content could be anything, but for this time we are using it as a standard header.
It has 2 columns with a title on one side and then a date label on the other.


Bringing it all together
-------------------------

These are all the files, and we just need to generate them.
All being well, then when we bring it together we will get a 2 page document with consistent headers and content.

.. image:: images/referencefilesoutput.png

The styles are used across all content even referenced files, and the layout flows just as you would expect.

Circular references
-------------------

Scryber will not allow circular references. i.e. files that reference either themselves, or other files that reference back to the original
as it could create an infinie parsing loop. 

Whilst a file can be embedded from multiple places in multiple documents, each time it will be loaded as new content.
Once loaded changes to one instance will not affect any other instances loaded from that file.


iFrame support
----------------

Along with the embed option, scryber supports the use of iFrames with a src.

.. code-block:: html

    <iframe src='Fragments/PageHeader.html' />

The frame is not isolated, or independent of the main document, and styles will be transferred down into the content of the frame.
This gives the wrong usage impression - but is supported as a tag element.

