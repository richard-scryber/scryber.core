================================
MVC Controller - Getting Started
================================

A Complete example for creating a hello world PDF file from an MVC Controller in C#

Nuget Packages
==============

If you have not done so already, make sure you install the Nuget Packages

`<https://www.nuget.org/packages/Scryber.Core.Mvc>`_

This will add the latest version of the Scryber.Core nuget package, and the Scryber.Core.Mvc extension methods.


Add a document template
=======================

In our applications we like to add our templates to a PDF folder the Views folder. You can break it down however 
works for you, but for a create a new XML file called HelloWorld.pdfx in your folder.

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


Your solution should look something like this.

.. image:: images/initialhelloworld.png



The xmlns is really important for knowing what type of document and schema is being used.

For for more information on the namespaces and mappings see this :doc:`namespaces_and_assemblies` documentation

Controller code
===============

Add a new controller to your project, and a couple of namespaces are important to add to the top of your controller.

.. code-block:: csharp

    using Scryber.Components;
    using Scryber.Components.Mvc;


Add the Web host service
========================

In order to nicely reference files in the solution, we add a reference to the IWebHostEnvironment to the home controller constructor.

.. code-block:: csharp

    private readonly IWebHostEnvironment _env;
            
    public HomeController(IWebHostEnvironment environment)
    {
        _env = environment;
    }


Add a Controller Method
=======================

Next add a new Controller Method to your class for retrieve and generate

.. code-block:: csharp

    [HttpGet]
    public IActionResult HelloWorld()
    {
        var path = _env.ContentRootPath;
        path = System.IO.Path.Combine(path, "Views", "PDF", "HelloWorld.pdfx");

        using(var doc = Document.ParseDocument(path))
            return this.PDF(doc);
    }


The PDF extension method will read the PDF template from the path and generate the file to the response.

.. image:: images/homecontroller.png

Testing your action
===================

To create your pdf simply add a link to your action method in a view.


.. code-block:: html

    <div>
        <h2 class="display-4">Simple sample from the PDF Controller</h2>
        <ul>
            <li><a href='@Url.Action("HelloWorld","Home")' target='_blank'>Hello World PDF</a></li>
        </ul>
    </div>


Running your application, you should see the link and clicking on it will open the pdf in a new tab or window.

.. image:: images/helloworldpage.png

Adding dynamic content
=======================

One of the driving forces behind scryber is the separation of the content, data and style. It
is common practice in sites. With scryber all attributes and content is bindable to the data you want to pass to it,

So we can specify our model data with from any source (here we are just using a dynamic object).
And we can pass it to the parsed document either explicitly, or using the special 'model' overload 
on the PDF extension method. 

.. code-block:: csharp

    private dynamic GetHelloWorldData()
    {
        //get your model data however you wish
        //it's just a sample object for this one.

        var model = new
            {
                titlestyle = "color:#ff6347", //style data
                title = "Hello from scryber", //simple content
                items = new[]                 //or even complex object data
                {
                    new { name = "First item" },
                    new { name = "Second item" },
                    new { name = "Third item" },
                }
            };

        return model;
    }

    [HttpGet]
    public IActionResult HelloWorld()
    {
        var path = _env.ContentRootPath;
        path = System.IO.Path.Combine(path, "Views", "PDF", "HelloWorld.html");

        using(var doc = Document.ParseDocument(path))
        {
            var model = GetHelloWorldData();
            
            //could use doc.Params["model"] = model; for the same effect.
            //It is just more convenient as below.
            return this.PDF(doc, model);
        }
    }


The general syntax for referring paramters in a template is

{@:**parameter[.property]**}

And the html5 tag 'template' is used with the data-bind attribute to loop over one or more items in a collection, and the 
inner objects and properties can be used with the . prefix to reference the current data context.

So we can expand our template body to use our model.

.. code-block:: html

        <body>

            <main style="padding:10pt">

                <!-- binding styles and values on content -->
                <h2 style="{@:model.titlestyle}">{@:model.title}</h2>

                <div>We hope you like it.</div>

                <!-- Loop with nested item collection binding to the objects -->
                <ol>
                    <template data-bind='{@:model.items}'>
                        <!-- binding within the model.items content, and can be nested -->
                        <li>{@:.name}</li> 
                    </template>
                </ol>
            </main>

        </body>



Adding Fonts and Styles
=======================

It's good but simple. With scryber we can use css styles as we would in html.

.. code-block: html

    <!-- support for external style sheets - in this case the Fraunces google font (watch out for the &amp; link in the url) -->
        <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Fraunces:ital,wght@0,400;0,700;1,400;1,700&amp;display=swap" title="Fraunces" />

        <!-- support for css selectors -->
        <style>

            /* Setting the defaults */

            body{
                font-family: 'Fraunces', serif;
                font-size: 14pt;
            }

            /* Complex style with backgrounds, images and color */

            p.header {
                color: #AAA;
                background-color: #333;
                background-image: url('../html/images/ScyberLogo2_alpha_small.png');
                background-repeat: no-repeat;
                background-position: 10pt 10pt;
                background-size: 20pt 20pt;
                margin-top: 0pt;
                padding: 10pt 10pt 10pt 35pt;
            }

            /* print only css with nested selectors */

            @media print {

                .foot td {
                    border: none;
                    text-align: center;
                    font-size: 10pt;
                    margin-bottom: 10pt;
                }
            }

            /* page selectors for sizing and allows page breaks */

            @page {
                size:A4 portrait;
            }

        </style>


You can read more about what css selectors we

* :doc:`document_model`
* :doc:`document_structure`
* :doc:`component_types`
* :doc:`document_styles`
* :doc:`referencing_files`