========================================
1.4. Scryber packages and the libraries
========================================

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


1.4.4 Assemblies and Namespaces
--------------------------------

**The Scryber.Common library**

The base library for all other libraries

1. Scryber - Contains the core interfaces, attributes, events and handler delegate definitions.
2. Scryber.Logging - Contains specific implementations of the Scryber logging infrastructure
3. Scryber.Options - Classes for the configuration of scryber.
4. Scryber.Native - Classes for the reading and parsing of an existing PDF file.
5. Scryber.Caching - Base classes and interfaces for data caching.
6. Scryber.Utility - Helper classes for paths, data load; numbering and types; frameworks and versions.

**The Scryber.Drawing library**

Contains all the drawing structures and classes for units, points, rects, thicknesses and colors.



