======================================
Version History
======================================

The following change log is for developers upgrading from previous versions,
or looking for new features

Version 1.1 Core Change log
===========================

This is a breaking change for existing implementations, but represents a significant step foreward.


* XML content should now use the doc: prefix for the components namepsace
* The Scryber.Components namespace classes no longer have the PDF prefix i.e. PDFDocument is now Document.
* The output of a pdf method has changed SaveAsPDF
* Updated the schemas to match the new document structure

Other changes include the use of the match='[css selector]' on styles with priorities based on depth,
and the support for xhtml as a root element in a document parsing.


Version 1.0 Core Change log
===========================

This is the first release of the library for DotNet Core
It includes the switch to a Document/Data element
Improved layout capabilities
The support for TTC (true type collection fonts)
Various other enhancements


