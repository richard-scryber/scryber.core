=============================================
1.4. Scryber packages and the libraries - TD
=============================================

It is not nescessary to know the structure of the scryber code, or how it processes a document into a PDF.
But it helps in understanding what is going on under the hood, and also understanding the logs.

1.4.1. NuGet Packages
----------------------

There are 3 NuGet packages for scryber.

The `Scryber.Core.OpenType <https://www.nuget.org/packages/Scryber.Core.OpenType/>`_ package contains a single library for parsing ttf (open type) font files.

The `Scryber.Core <https://www.nuget.org/packages/Scryber.Core/>`_ package contains the main libraries for PDF generation.

The `Scryber.Core.Mvc <https://www.nuget.org/packages/Scryber.Core.Mvc/>`_ package contains the MVC extensions that allow for easy generation of you PDF from a web request.


1.4.2. Source code
------------------

The `Scryber.Core Git repository <https://github.com/richard-scryber/scryber.core>`_ contains the open source code, you are at liberty to use in your own projects for if wanted.

It also contains the samples for this documentation in the `Scryber.UnitSamples` project of the source.


1.4.3. Scryber.Core libraries
-----------------------------

Within the core package are 6 main libraries with the top level library Scryber.Components referencing others, and the Scryber.Common containing most of the interfaces and base structures.

.. figure:: ../images/dll_references.png
    :target: ../_images/dll_references.png
    :alt: Package and library references
    :class: with-shadow

`Full size version <../_images/dll_references.png>`_


1.4.4 Useful Namespaces
--------------------------------

When building documents, knowing the namespaces for Components and content is useful. 
As an overview these are the main namespaces in each of the libraries, anong with the purpose
of the classes in that namespace.

**The Scryber.Common library**

The base library for all other libraries

1. Scryber - Contains the core interfaces, attributes, events and handler delegate definitions.
2. Scryber.Logging - Contains specific implementations of the Scryber logging infrastructure
3. Scryber.Options - Classes for the configuration of scryber.
4. Scryber.Native - Classes for the reading and parsing of an existing PDF file.
5. Scryber.Caching - Base classes and interfaces for data caching.
6. Scryber.Utility - Helper classes for paths, data load; numbering and types; frameworks and versions.

**The Scryber.Drawing library**

Contains all the drawing structures and classes for units, points, rects, thicknesses and colors, along with text, imaging and resources.

1. Scryber - The interfaces specific to drawing, and the PDFxxxOptions that are built from styles.
2. Scryber.Drawing - THe main graphics classes with units, points, rects, thicknesses, colors, gradients, dashes, fonts, brushes and pens.
3. Scryber.Drawing.Imaging - The classes that understand different pixel image formats.
4. Scryber.Options - Classes for configuration of the drawing library.
5. Scryber.Resources - Classes that are shared resources in a document.
6. Scryber.Text - Classes associated with reading textual content and encodings.
   
**The Scryber.Expressive library**

Contains all the expression parsing and evaluation classes for the {{handlebars}} syntax.
This is based on the source from Shaun Lawrence in the `https://github.com/bijington/expressive`_ repository

1. Scryber.Expressive - Root namespace for the parsers, top expression class.
2. Scryber.Expressive.Tokenisation - Conversion of expression strings to tokens.
3. Scryber.Expressive.Funtions - All available function classes.
4. Scryber.Expressive.Operators - All available operator classes.
5. Scryber.Expressive.Expressions - All expression classes that operators build.
6. Scryber.Expressive.Helpers - Type conversion and number calculation methods.

**The Scryber.Generation library**

Contains all functionality to read from streams and files and parse into instances and objects.

1. Scryber - Interfaces, enumerations and delegates that are commonly used for generation.
2. Scryber.Generation - The main classes for parsing content and building instances based on reflection of attributes.
3. Scryber.Binding - The factories and classes for creating the expressions for binding in a document.

**The Scryber.Styles library**

Contains all functionality for parsing, building, merging and referencing styles.

1. Scryber - Interfaces, enumerations and delegates that are commonly used for styles.
2. Scryber.Styles - The main classes for style properties, the keys they are stored with and values assigned to them.
3. Scryber.Styles.Parsing - The classes for converting css to styles and their definitions.
4. Scryber.Styles.Selectors - The classes for css Selectors and matching to components.

**The Scryber.Components library**


