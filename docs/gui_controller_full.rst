================================
Console or GUI - Getting Started
================================

A Complete example for creating a hello world PDF file in a console application or GUI front end.
For us, we have just created a new dotnet core console application in Visual Studio.

How it works
-------------

We hope scryber works just as you would expect. The engine is based around the controllers you have, using XHTML template views with css, graphics 
and images you are used to, along with your model data you have, to create PDF documents quickly, easily and flexibly.

.. image:: images/ScryberMVCGraphic.png

Nuget Packages
---------------

Make sure you install the Nuget Packages from the Nuget Package Manager

`<https://www.nuget.org/packages/Scryber.Core>`_

This will add the latest version of the Scryber.Core nuget package.

Add a document template
------------------------

In our applications we like to add our templates to a PDF folder. You can break it down however works for you, 
but for now a create a new XHML file called HelloWorld.html in your folder.

And paste the following content into the file

.. code-block:: html

    <!DOCTYPE HTML >
    <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
        <head>
            <title>Hello World</title>
        </head>
        <body>
            <div style='padding:10px'>Hello World from scryber.</div>
        </body>
    </html>


Pdfx file properties
---------------------

In the file properties for the HelloWorld.html file:
Set the Build Action to None (if it is not already)
And the Copy to output to Always.

Your solution should look something like this.

.. image:: images/initialhelloworldgui.png



Program code
--------------

In your program.cs add the namespace to the top of your class.

.. code-block:: csharp

    using Scryber.Components;



Replace your main program method.
----------------------------------

Next change the 'Main' method to your class to load the template and generate the pdf file

.. code-block:: csharp

        static void Main(string[] args)
        {
            System.Console.WriteLine("Beginning PDF Creation");

            //Get the working and temp directory
            string workingDirectory = System.Environment.CurrentDirectory;
            string tempDirectory = System.IO.Path.GetTempPath();

            //The path to the input template - could be a stream, text reader, xml reader, resource etc
            var path = System.IO.Path.Combine(workingDirectory, "PDFs\\HelloWorld.html");

            //The path to the output file - could be a stream
            var output = System.IO.Path.Combine(tempDirectory, "HelloWorld.pdf");

            
            //Load the template and output to the directory
            var doc = PDFDocument.ParseDocument(path);
            doc.ProcessDocument(output, System.IO.FileMode.OpenOrCreate);

            //Notify completion
            System.Console.WriteLine("PDF File generated at " + output);
            System.Console.ReadKey();

        }


.. image:: images/programcs.png

The parser will read the document from the pdfx XML content, and then create a new PDF document in the tempDirectory for the output.


Testing your code
------------------

Running your application, you should see the console output the path to the pdf. 
And opening this will show you the file. you could have saved it to a share, opened in Acrobat reader, or sent via email as a stream attachment.


.. image:: images/helloworldconsole.png




Further reading
===============

You can read more about the what you can do with scryber here:

* :doc:`document_model`
* :doc:`document_structure`
* :doc:`component_types`
* :doc:`document_styles`
* :doc:`referencing_files`