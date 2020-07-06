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

    <?scryber append-log='true' log-level='Messages' parser-log='true' ?>

The scryber processing instruction is an optional entry at the top of the xml file or content to define explicit options for the following content.
As a processign instruction, the schema xsds do not support validation of the processing instruction, but the following are the supported options.

* append-log - If set to true, then once the document has been generated, a trace log of output will be appended to the resultant file, containing all the recorded entries.
* log-level - This is an enumeration of the granularity of the logging performed on the pdf file. Values supported (from least to most) are
    * Off - no entries be recorded.
    * Errors - only errors will be recorded (depending on the parser mode switch)
    * Warnings - warnings will occur if some of the contents cannot be loaded, or the parsing fails for a non-error condition.
    * Messages - This will output key stage messages for the generation of the file.
    * Verbose - A quantity of messages will be output for each of the compoents, and is a useful level to understand what is going wrong (if anything) with your document.
    * Diagnostic - Be carefull, this will generate a large log file and can slow the creation of a PDF file significnatly. But it's very informative.
* parser-log - If set to true, then both the reading of the content, to create the document, as well as the output of the content to PDF will be recorded.
* parser-culture - This specifies the culture of the document. It can be useful for reading number formats or dates from files e.g. es-es will read spanish nuber formats where . 'dot' is a thousand separator.
* parser-mode - Strict of Lax. If this is set to strict then exceptions will bubble up from the parsing and rendering of files. Lax is more complianant, where errors will be logged, but not cause the output to fail.
* controller - This is the full type name of a controller for the docucment, that can interact with and handle events on the document.







