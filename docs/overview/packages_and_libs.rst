==================================
Scryber packages and the libraries
==================================

It is not nescessary to know the structure of the scryber code, or how it processes a document into a PDF.
But it helps in understanding what is going on under the hood, and also understanding the logs.

NuGet Packages
--------------

There are 3 NuGet packages for scryber.

The Scryber.Core.OpenType package contains a single library for parting ttf (open type) font files.

The Scryber.Core package contains the main libraries for PDF generation.

The Scryber.Core.Mvc package contains the MVC extensions that allow for easy generation of you PDF from a web request.

Source code
------------

The Scryber.Core Git repository contains the open source code, you are at liberty to use in your own projects for if wanted.


Scryber.Core libraries
------------------------

Within the core are 6 main libraries with the top level library Scryber.Components referencing others, and the Scryber.Common referencing none.

.. image:: ../images/dll_references.png


Document Processing lifecycle
------------------------------

When creating a PDF document from a template in your code there is a clear linear process that is followed to generate the final output.

* Parsing the template creates the document object model.
* Initialize and load on each component to ensure the correct state.
* Databind to any data models (which can create further components)
* Layout converts the high level components to lower level entities
* Render allows the layout entities to render themselves to a PDFWriter

Each of the stages raises events that can be captured to perform any custom processing required

.. image:: ../images/doc_lifecycle.padding



