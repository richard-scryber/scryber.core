======================================
Version History
======================================

The following change log is for developers upgrading from previous versions,
or looking for new features

Version 5.0.5
--------------

**Big Hitters**

* Embed and iFrame support. (See: :doc:`document_references`)
* Support for border-left, border-right, etc (See: :doc:`drawing_colors`)
* Support for encryption and restrictions. (See: :doc:`document_security`)
* Support for base href in template files. (See: :doc:`document_structure`)
* Added em, strong, strike, del, ins elements. (See: :doc:`document_textlayout`)

**Minor enhancements and bug fixes**

* Classes and styles on template tags are supported.
* Html column width and break inside
* CSS and HTML Logging
* Binding speed improvements for longer documents.
* Fixed application of multiple styles with the same word inside
* Allow missing images on the document is now supported.
* Contain fill style for background images.

Version 5.0.4
---------------

**Initial SVG Support (See: :doc:`drawing_paths`) **

 Local font urls along with some bug fixes.

Version 5.0.3
---------------

* Added @font-face, absolute, relative and display css. (See: :doc:`drawing_fonts`)
* Support for @page css directives for the whole document and section page sizes. (See: :doc:`drawing_fonts`)
* Support for <page /> tags with property or for attributes. (See: :doc:`drawing_fonts`)
* Added support for HTML binding with the template tag and data-bind attribute (See: :doc:`binding_model`)

* Fix for anchor links with internal and external href. 
* Fixes for single character css values and other minor updates.

5.0.1-alpha

** Upgrade to support dotnet 5 **

Plus increased support for the HTML parsing with entities and DTD

Version 1.1 Core Change log
----------------------------

This is a breaking change for existing implementations, but represents a significant step foreward.

* XML content should now use the doc: prefix for the components namepsace
* The Scryber.Components namespace classes no longer have the PDF prefix i.e. PDFDocument is now Document.
* The output of a pdf method has changed SaveAsPDF
* Updated the schemas to match the new document structure

Other changes include the use of the match='[css selector]' on styles with priorities based on depth,
and the support for xhtml as a root element in a document parsing.


Version 1.0 Core Change log
----------------------------

**The first release of the library for DotNet Core**

It includes the switch to a Document/Data element
Improved layout capabilities
The support for TTC (true type collection fonts)
Various other enhancements


