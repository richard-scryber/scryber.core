================================
Scryber Trace log details - PD
================================

Internally scryber has lots of logging that can be used to understand what is going on underneath, without having to debug the libraries
This is achieved with the scryber processing instruction that needs to be at the top of the file.

Example without logging
-----------------------

If we have the below example for our template.

.. code-block:: html

    <!DOCTYPE HTML >
    <html xmlns='http://www.w3.org/1999/xhtml' >
        <head>
            <title>Hello World</title>
            <link href='../css/printstyles.css' rel='stylesheet' />
        </head>
        <body>
            <div style='padding:10px; text-align:center'>
                <img src='../images/sitelogo.png'>
                <p>Hello World from scryber.</p>
            </div>
        </body>
    </html>

Then when we generate, we will get the following output

.. image:: images/HelloWorldNoLogging.png

It is there, but there is no image, and it's not looking like we expected.

However if we add our scryber processing intruction to append the log for messages then we see so much more going on.

Example with logging
---------------------

.. code-block:: html

    <?scryber append-log='true' log-level='Messages' parser-log='true' ?>
    <!DOCTYPE HTML >
    <html xmlns='http://www.w3.org/1999/xhtml' >
        <head>
            <title>Hello World</title>
            <link href='../css/printstyles.css' rel='stylesheet' />
        </head>
        <body>
            <div style='padding:10px; text-align:center'>
                <img src='../images/sitelogo.png'>
                <p>Hello World from scryber.</p>
            </div>
        </body>
    </html>

Then when we generate, we will get the following output

.. image:: images/HelloWorldWithLogging.png

Here we can see that there are another 3 pages added to the document - and quite a few errors.
Specifically the stylesheet could not be found, and nor could the image.


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
    * Diagnostic - This will generate a large log file and can slow the creation of a PDF file significnatly. But it's very informative.
* 'parser-log' - Controls the logging from the xml parser.
    * true - then both the reading of the content, to create the document, as well as the output of the content to PDF will be recorded.
    * false - then only messages from the content creation and output will be recorded.
* 'parser-culture' - specifies the global culture settings when parsing a file for interpreting dates and number formats in the content. e.g.
    * en-gb - This specifies the english, britsh culture. It can be useful for reading number formats or dates from files e.g. 
    * es-es - This will read spanish nuber formats where . 'dot' is a thousand separator and , 'comma' is the decimal separator.
* 'parser-mode' - Defines how errors will be recorded if unknown or invalid attributes values are encountered. 
    * Strict - Will raise exceptions to the top of the stack and must be handled in your code. (Good for dev)
    * Lax - Default. If this is set then the parser is more complianant, where errors will be logged, but not cause the output to fail. (Good for Prod).

.. note:: If you set the log level to Diagnostic for the Hello World example, the appended log file is around 10 pages in length. If it's a long document - diagnostic is going to hurt.


Tracing Details
----------------

There is some really good information available in the tracing output not just in the logging, but also on the metrics and overview.

.. image:: images/HelloWorldTraceOutput.png

The top section will give information on the versions, file sizes and generation time (for the document without the logging).

The middle section will give information on timings for each type fo activity. 
If the trace level is Verbose (or Diagnostic) then the performance metrics will detail specific areas, for eaxmple below we can see that the loading of the google font(s) was causing our 
template to increase generation time by 110 milliseconds to load the font css. Luckily the font files themselves are cached and did not need to be reloaded each time. 
But we could save that time by using a local css.

.. image:: images/ReadMetTraceVerbose.png

