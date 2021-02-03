================
File structure
================

Scryber expects all content to be in valid XHTML structrure.
Tags must be properly closed, and ampersands (&) must either be escaped or valid html character notations (&amp; &quot; etc.)


Example
--------

.. code-block:: html

    <?scryber append-log='true' log-level='Messages' parser-log='true' ?>
    <!DOCTYPE HTML >
    <html xmlns='http://www.w3.org/1999/xhtml' >
        <head>
            <title>Hello World</title>
        </head>
        <body>
            <div style='padding:10px'>Hello World from scryber.</div>
        </body>
    </html>

At the top of the file scryber has it's own **optional** processing instuctions to specify log levels and output.
This is so you can quickly and easily check what is actually going on under the hood.

The DOCTYPE is not required, and if present will be ignored in preference to the xmlns (next).

The html tag has the xmlns attribute - this tells scryber to expect an XHTML formatted document,
rather than any other document description. It is required, although you can use prefixes and any other supported namespaces (see :doc:`drawing_paths`)

The rest of the document follows the standard html structure, which is discussed in detail below.


Scryber processing instruction
--------------------------------

The following are the supported options on the processing instruction.

* 'append-log' - Controls the tracing log output for a single document
    * false - This is the default and the document will be rendered as normal.
    * true - If set to true, then once the document has been generated, a trace log of output will be appended to the resultant file, containing all the recorded entries.
* 'log-level' - This is an enumeration of the granularity of the logging performed on the pdf file. Values supported (from least to most) are
    * Off - no entries be recorded.
    * Errors - only errors will be recorded (depending on the parser mode switch)
    * Warnings - warnings will occur if some of the contents cannot be loaded, or the parsing fails for a non-error condition.
    * Messages - This will output key stage messages for the generation of the file.
    * Verbose - A quantity of messages will be output for each of the compoents, and is a useful level to understand what is going wrong (if anything) with your document.
    * Diagnostic - **Be carefull**, this will generate a large log file and can slow the creation of a PDF file significnatly. But it's very informative.
* 'parser-log' - Controls the logging from the xml parser.
    * true - then both the reading of the content, to create the document, as well as the output of the content to PDF will be recorded.
    * false - then only messages from the content creation and output will be recorded.
* 'parser-culture' - specifies the global culture settings when parsing a file for interpreting dates and number formats in the content. e.g.
    * en-gb - This specifies the english, britsh culture. It can be useful for reading number formats or dates from files e.g. 
    * es-es - This will read spanish nuber formats where . 'dot' is a thousand separator and , 'comma' is the decimal separator.
* 'parser-mode' - Defines how errors will be recorded if unknown or invalid attributes values are encountered. 
    * Strict - Will raise exceptions to the top of the stack and must be handled in your code. (Good for dev)
    * Lax - Default. If this is set  then the parser is more complianant, where errors will be logged, but not cause the output to fail. (Good for Prod).

.. note:: If you set the log level to Diagnostic for the Hello World example in our Getting started examples, the appended log file is around 30 pages in length. If it's a long template - diagnostic is going to hurt, but for a quick check or a feature - it is awesome without needing debugging.

.. image:: images/HelloWorldTracing.png


Namespaces
----------

Scryber is dynamic and extensible. The xml namespaces refer directly to namespaces (and assemblies) in the library.
There are 2 primary namespaces in use with xhtml documents.


* http://www.w3.org/1999/xhtml
    * This is the main visual and structural components in an html file or document.
    * It refers to the assembly namespace `Scryber.Html.Components, Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe`
    * see `<https://github.com/richard-scryber/scryber.core/tree/master/Scryber.Components/Html/Components>`_ for the classes in this namespace.
    * see :doc:`document_components` for a description of each of these.
* http://www.w3.org/2000/svg
    * These are the svg graphics components. e.g. svg, path, line, rect 
    * It refers directly to the assembly namespace `Scryber.Svg.Components, Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe`
    * see `<https://github.com/richard-scryber/scryber.core/tree/master/Scryber.Components/Html/Components>`_ for the classes in this namespace.
    * see :doc:`drawing_paths` for a description of each of these.


For more information on how these are mapped, and also adding your own namespaces see :doc:`namespaces_and_assemblies` along with :doc:`scryber_configuration`

Html header
-----------

The following tags are supports as direct mappings to the PDF document information.

.. code-block:: html

    <head>
        <title>My Document</title>
        <meta name='author' content='Richard Hewitson' />
        <meta name='description' content='This is the subject' />
        <meta name='keywords' content='Scryber; Document Info; Properties' />
        <meta name='generator' content='Scryber Documentation' />
    </head>


.. image:: images/documentproperties.png

The header also supports the <link> and <style> elements discussed below, although these are happily supported elsewhere too.
It is only the meta and title elements that need to be in the html head.


Html link element
------------------

If a <link> is included in the html file (in the head preferably). 
Then it must have the 'rel' attribute of stylesheet and a 'href' to a valid css file.

.. note:: If the rel attribute is not set, then it is assumed to be a stylesheet, and loaded. But may not be able to be parsed.

The href can either be relative to the current file, or a full absolute url to a file.
