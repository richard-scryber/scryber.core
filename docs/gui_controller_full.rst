==============
GUI Controller - Getting Started
==============

A Complete example for creating a hello world PDF file in a console application or GUI front end.
For us, we have just created a new dotnet core console application in Visual Studio.

Nuget Packages
==============

Make sure you install the Nuget Packages from the Nuget Package Manager

`<https://www.nuget.org/packages/Scryber.Core>`_

This will add the latest version of the Scryber.Core nuget package.

Add the XML Schema files if you want to have help with the intellisense on the XML files

`<https://www.nuget.org/packages/Scryber.Core.Schemas/>`_


Add a document template
=======================

In our applications we like to add our templates to a PDF folder. You can break it down however works for you, but for a create a new XML file called HelloWorld.pdfx in your folder.

And paste the following content into the file

.. code-block:: html

    <?xml version="1.0" encoding="UTF-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
              xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
              xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
        <Pages>
            <pdf:Page>
                <Content>
                    <pdf:Label>Hello World, from scryber.</pdf:Label>
                </Content>
            </pdf:Page>
        </Pages>
    </pdf:Document>


For for more information on the namespaces and mappings see this [About Namespaces](namespaces_and_assemblies) documentation


Pdfx file properties
-----------------

In the file properties for the HelloWorld.pdfx file:
Set the Build Action to None (if it is not already)
And the Copy to output to Always.

Your solution should look something like this.

.. image:: images/initialhelloworldgui.png



Program code
===============

In your program.cs add the namespace to the top of your class.

.. code-block:: csharp

    using Scryber.Components;


Add the 
========================

In order to nicely reference files in your view, add a reference to the IWebHostEnvironment to your home controller constructor.

.. code-block:: csharp

    private readonly IWebHostEnvironment _env;
            
    public HomeController(IWebHostEnvironment environment)
    {
        _env = environment;
    }


Replace your main pro
=======================

Next add a new Controller Method to your class for retrieve and generate

.. code-block:: csharp

        static void Main(string[] args)
        {
            System.Console.WriteLine("Beginning PDF Creation");

            //Get the working and temp directory
            string workingDirectory = System.Environment.CurrentDirectory;
            string tempDirectory = System.IO.Path.GetTempPath();

            //The path to the input template - could be a stream, text reader, xml reader, resource etc
            var path = System.IO.Path.Combine(workingDirectory, "PDFs\\HelloWorld.pdfx");

            //The path to the output file - could be a stream
            var output = System.IO.Path.Combine(tempDirectory, "HelloWorld.pdf");

            
            //Load the template and output to the directory
            var doc = PDFDocument.ParseDocument(path);
            doc.ProcessDocument(output, System.IO.FileMode.OpenOrCreate);

            //Notify completion
            System.Console.WriteLine("PDF File generated at " + output);
            System.Console.ReadKey();

        }


The parser will read the document from the pdfx XML content, and then create a new PDF document in the tempDirectory for the output.

.. image:: images/programcs.png

Testing your code
===================

Running your application, you should see the console output the path to the pdf. 
And opening this will show you the file. you could have saved it to a share, opened in Acrobat reader, or sent via email as a stream attachment.


.. image:: images/helloworldconsle.png


Further reading
===============

You can read more about the 

* `Passing Data to your Document <document_model>`_
* `Document Structure <document_structure>`_
* `Component List <component_types>`_
* `Styling your documents <document_styles>`_
* `Splitting your content <referencing_files>`_