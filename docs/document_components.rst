=================================
Standard document components
=================================

The scryber library comes with all the standard components used in document creation, similar to HTML

Document level components
--------------------------

* head
    * The document description meta tags
    * Also the area to put links and styles
    * Supports the security 'restrictions' meta tags.

* body
    * The main pages within the document. 
    * Has an optional page header and page footer, as well as content.

* section
    * A section can appear in the body and will by default be placed on a new page (or set of pages).
    * It supports the @page css at rule for altering the page size.


Structural components
--------------------------------

* div
    * a block level component that will fill the width of the available parent.
* span 
    * an inline compnent that can have any content including text.
* table
    * A grid of rows and cells (that can be spanned across columns).
* ol, ul, dl
    * Ordered lists with numbering styles.
    * Unodered lists with a bullet styles.
    * Definition lists with a label and content.
* p(aragraph)
    * A textual block (and other content), that has a more defined style than a div.
* h1 to h6
    * A textual block (and other content), that is given a pre-defined style based on it's level of 1 to 6
* blockquote
    * A panel with specific margins, and a left border by default.
* pre(formatted)
    * A container for pre-formatted text, that will not flow over new lines, or remove line breaks (by detault).
* iframe
    * A reference to an external file or stream that will be injected into the page(s) at runtime.
* main
    * A block component within the body.
    * It has an optional header and a footer.
* page
    * This is a non-standard component, that will be output as the 

Textual components
-------------------

Text is generally supported throughout the body of the file as you would expect.

* b, strong
    * An inline compenent where inner text font will be bold by default.

* i, em
    * An inline compenent where inner text font will be italic by default.

* u, ins
    * An inline compenent where inner text font will be underlined by default.

* strike, del
    * An inline compenent where inner text font will be strikethrough by default.

* page
    * A textual compenent that displays the current page number, page number of a referenced component.
    * Supports the use of page section counting and total document page count.
    * Supports the 'for' attribute to get the page number of another component.

* time
    * A textual component that supports the display of a date or time.
    * Can use a date-format or textual value.


Graphical components
----------------------

* hr
    * A static horizontal rule on the page.

* img
    * An image loaded from a source, and inserted into the output document.
    * Supports the use of full, relative or dynamic url references.
    * Supports png, jpeg and tiff file formats.
    * Supports alpha channels where available in the source.

* svg
    * Standard drawing svg components that can be used for drawing/designs.
    * Supports the viewport and sizing options.
    * Inner content support for paths, rects, ellipses, polygons and polylines.

See :doc:`drawing_images` for images and :doc:`drawing_paths` for the line, rect and path componenets.


Data components
----------------

For a general use of the data components see :doc:`document_model` and  :doc:`document_databinding`.

* template
    * Loops through one or more values in a source.
    * The data-bind attribute is used to specify the content that will be used as a source.
    * Will execute multiple times for a content within the template and the number of items binding to.
    * If it is null, then noting will be output.
    





