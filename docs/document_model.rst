======================================
Passing data to your document template
======================================

A document template  is just that, a template.
In your code you can add any source of information to be included.

* As an value or model for the parameters
* Bound to an XML datasource (xml, sql or object)
* As a Controller with an event method
* Just in code

And they can be used in your document with many of the data controls

* The document itself
* A Data Grid
* A Data List
* `With` a single entry
* `ForEach` loop

And the content supports as default both object and xpath binding. The notation for an binding on an attribute is 
based on the { and } with a method (@ or xpath for the built in binders), a colon ':', and then finally the selector.

e.g. `attribute='{@:paramName}'` for objects or `attribute='{xpath:selector}'` for xml



The Document parameters
=======================

Every Document can have parameters associated with it.
These should be declared at the top of the Document, in the Params element, for clarity to other developers
(even if the default value is empty).

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Params>
        <!-- Declare a complex object parameter -->
        <pdf:Object-Param id="Model" />
    </Params>
    
    <Pages>
        <!-- Use the models 'DocTitle' property for the outline. -->
        <pdf:Page outline-title="{@:Model.DocTitle}" styles:margins="20pt">
        <Content>
            <!-- And use it as the text on the heading -->
            <pdf:H1 styles:class="title" text="{@:Model.DocTitle}" > </pdf:H1>
            
            <pdf:Ul>
            <!-- now we loop through the 'Entries' property -->
            <data:ForEach value="{@:Model.Entries}" >
                <Template>
                <pdf:Li>
                    <!-- and create a list item for each entry (. prefix) with the name property. -->
                    <pdf:Text value="{@:.Name}" />
                </pdf:Li>
                </Template>
            </data:ForEach>
            </pdf:Ul>
            
        </Content>
        </pdf:Page>
    </Pages>
    
    </pdf:Document>

And the value can be set or changed at runtime

.. code-block:: csharp

        var model = new { 
            DocTitle = "Testing Document Parameters",
            Entries = new[] {
                    new { Name = "First", Id = "FirstID"},
                    new { Name = "Second", Id = "SecondID" }
                }
        };

        var pdfDoc = PDFDocument.ParseDocument(path);
        pdfDoc.Params["Model"] = model;

        pdfDoc.ProcessDocument(output);

Or passed as the Model in the MVC methods

.. code-block:: csharp

     public IActionResult DocumentParameters()
    {
        var path = _env.ContentRootPath;
        path = System.IO.Path.Combine(path,"Views", "PDF", "DocumentParameters.pdfx");
        
        // This could be any object dynamically built or strongly typed.
        var model = new
        {
            DocTitle = "Testing Document Parameters",
            Entries = new[] {
                    new { Name = "First", Id = "FirstID"},
                    new { Name = "Second", Id = "SecondID" }
                }
        };

        //This method always stores the passed model as the `Model` parameter
        return this.PDF(path, model);
    }


And this will be used in the output.

.. image:: images/documentparameterssimple.png

See :doc:`document_parameters` for full details. 


The Datasources
===============

Putting the document more in control of the data it uses, is supported from the available DataSources and Commands that sit in the `Data` element of the document.

This element should contain all the datasources required by the document.
They can be an XML file, or XML Http request, a SQL database call, an object call, or a json request

e.g. This document has an xml content reference from a remote source (in this case a local host controller method). 
That returns the following content..

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <DataSources title="Testing Xml Datasources">
        <Entries>
            <Entry Name="First Xml" Id="FirstID" />
            <Entry Name="Second Xml" Id="SecondID" />
        </Entries>
    </DataSources>

And with that we can bind the source into the document

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
        <Data>
            <!-- This is a data source declared witin the document, that pulls the xml from the feed -->
            <data:XMLDataSource id="XmlSource" source-path="http://localhost:5000/Home/Xml" ></data:XMLDataSource>
        </Data>
        <Pages>
            
            <pdf:Page styles:margins="20pt">
            <Content>
                <!-- Use the `data:With` component to specify a source and path within the xml as a starting point. -->
                <data:With datasource-id="XmlSource" select="//DataSources" >

                <!-- And use it as the text on the heading -->
                <pdf:H1 styles:class="title" text="{xpath:@title}" > </pdf:H1>
                
                <pdf:Ul>
                    <!-- now we loop through the 'Entries' property -->
                    <data:ForEach value="{xpath:Entries/Entry}" >
                    <Template>
                        <pdf:Li>
                        <!-- and create a list item for each entry (. prefix) with the name property. -->
                        <pdf:Text value="{xpath:@Name}" />
                        </pdf:Li>
                    </Template>
                    </data:ForEach>
                </pdf:Ul>
                </data:With>
                
            </Content>
            </pdf:Page>
        </Pages>

    </pdf:Document>

With the result of the output showing the content.

.. image:: images/documentxmlbindingsimple.png


We could have specified the source on the `data:ForEach`, and alternatively we could have used a Json DataSource to return an object binding.
See :doc:`document_databinding` for more details.


The Document Controller
=======================

The document file or referenced files can also have Controllers associated with them to handle events and properties.
This gives complete control back to your code during the lifecycle of the document.

.. code-block:: xml

    <Document controller></Document>

.. code-block:: csharp

    class Controller{}


