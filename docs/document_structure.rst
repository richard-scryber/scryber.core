================================
A Scryber XML Document structure
================================

At the root, the document has a number of capabilities that change the output content.


* Processing Instruction
* View Options
* Render Options
* Styles
* Params
* Pages

Processing Instructions
=======================

.. code-block:: XML

    <?xml version="1.0" encoding="utf-8" ?>
    <?scryber append-log='true' log-level='Messages' parser-log='true' ?>
    <pdf:Document ....


The scryber processing instruction is an optional entry at the top of the xml file or content to define explicit options for the following content.
As a processign instruction, the schema xsds do not support validation of the processing instruction, but the following are the supported options.

* 'append-log' - If set to true, then once the document has been generated, a trace log of output will be appended to the resultant file, containing all the recorded entries.
* 'log-level' - This is an enumeration of the granularity of the logging performed on the pdf file. Values supported (from least to most) are
    * Off - no entries be recorded.
    * Errors - only errors will be recorded (depending on the parser mode switch)
    * Warnings - warnings will occur if some of the contents cannot be loaded, or the parsing fails for a non-error condition.
    * Messages - This will output key stage messages for the generation of the file.
    * Verbose - A quantity of messages will be output for each of the compoents, and is a useful level to understand what is going wrong (if anything) with your document.
    * Diagnostic - Be carefull, this will generate a large log file and can slow the creation of a PDF file significnatly. But it's very informative.
* 'parser-log' - If set to true, then both the reading of the content, to create the document, as well as the output of the content to PDF will be recorded.
* 'parser-culture' - This specifies the culture of the document. It can be useful for reading number formats or dates from files e.g. es-es will read spanish nuber formats where . 'dot' is a thousand separator.
* 'parser-mode' - Strict of Lax. If this is set to strict then exceptions will bubble up from the parsing and rendering of files. Lax is more complianant, where errors will be logged, but not cause the output to fail.
* 'controller' - This is the full type name of a controller for the docucment, that can interact with and handle events on the document.


Viewer Options
==============

The viewer options within the Document level element alter how readers (should) show the document and it's contents.
Not all readers support these (especially browsers), but it can help.

For example the following viewer options will:

.. code-block:: XML

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
              xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
              xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd"
              auto-bind="true" >
    <Viewer hide-toolbar="true" page-display="Thumbnails" page-layout="TwoPageLeft" fit-window="false" />

Will open in Acrobat Reader as:

.. image:: images/viewOptions.png

Whereas without the View options the default is:

.. image:: images/viewOptionsNone.png


The following options are declared and supported in the Viewer element

* `hide-toolbar` - will show or hide the toolbar (currently a side bar) in reader.
* `page-display` - Indicates the type of side navigation shown for the document. Supported values are:
    * `None` - Side display is hidden (contracted).
    * `Thumbnails` - The page thumbnails are shown.
    * `Outlines` - The document outline, a hierarcial structure of the content, is shown. (see :doc:`document_outline`)
    * `Attachments` - The document attachments panel is shown.
    * `FullScreen` - This attempts to open the document in full screen presentation mode. A warning to the end user is often shown beforehand.
* `page-layout` - Indicates how pages will be displayed in the view. Supported values are:
    * `SinglePage` - It will open with a page per view sizing in the reader window.
    * `TwoPageLeft` - The document will open with a side by side view of 2 pages, where the first page is on the left.
    * `TwoPageRight` - The document will open with a single first page (the right page) and then 2 page per view following that. Very similar to reading a book.
    * `OneColumn` - The document will open with a full width continuous display, to support scrolling through the complete document.
    * `TwoColumnLeft` - 2 pages, side by side with a full width continuous display.
    * `TwoColumnRight` - 2 pages, side by side, continuous scrolling, with the first page on it's own as per a book.
* `fit-window` - If true the window will resize to fit the width of the first page.
* `center-window` - If true, the UI reader window will center in the main screen.
* `hide-menubar` - If true, then the window menu bar should be hidden.







