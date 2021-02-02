================================
MVC Controller - Getting Started
================================

A Complete example for creating a styled and databound hello world PDF file from an MVC Controller in C#

How it works
--------------

We hope scryber works just as you would expect. The engine is based around the controllers you have, using XHTML template views with css, graphics 
and images you are used to, along with your model data you have, to create PDF documents quickly, easily and flexibly.

.. image:: images/ScryberMVCGraphic.png

Nuget Packages
==============

If you have not done so already, make sure you install the Nuget Package in your new or existing MVC project.

`<https://www.nuget.org/packages/Scryber.Core.Mvc>`_

This will add the latest version of the Scryber.Core, and also the Scryber.Core.Mvc Controller extension methods.


Add a document template
=======================

In our applications we like to add our templates to a PDF folder the Views folder. You can break it down however 
works for you, but for now, a create a new xhtml file called HelloWorld.html in your folder.

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
inner objects and properties can be used with the '.' prefix to reference the current data context.

So we can expand our document body to use the model schema.

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


.. image:: images/HelloWorldWithData.png

Adding Fonts and Styles
=======================

It's good but rather uninspiring. With scryber we can use css styles, just as we would in html.
Here we are 

* Adding a stylesheet link to the google 'Fraunces' font (watch that &display=swap link - it's not xhtml)
* Adding some document styles for the body
* A complex style for a page header, with a colour and single background image, that will be repeated across any page.
* And a page footer table with full width and associated style on the inner cells, that will again be repeated.

The css style could just have easily come from another referenced stylesheet.

.. code-block:: html

    <!DOCTYPE HTML >
    <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
        <head>
            <title>Hello World</title>

            <!-- support for complex css selectors (or link ot external style sheets )-->
            <link rel="stylesheet"
                href="https://fonts.googleapis.com/css2?family=Fraunces:ital,wght@0,400;0,700;1,400;1,700&amp;display=swap"
                title="Fraunces" />

            <style>
                body {
                    font-family: sans-serif;
                    font-size: 14pt;
                }

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


                .foot td {
                    border: none;
                    text-align: center;
                    font-size: 10pt;
                    margin-bottom: 10pt;
                }
            </style>
        </head>
        <body>
            <header>
                <!-- document headers -->
                <p class="header">Scryber document creation</p>
            </header>
            <!-- support for many HTML5 tags-->
            <main style="padding:10pt">

                <!-- binding style and values on content -->
                <h2 style="{@:model.titlestyle}">{@:model.title}</h2>
                <div>We hope you like it.</div>
                <ol>
                    <!-- Loop through the items in the model -->
                    <template data-bind='{@:model.items}'>
                        <li>{@:.name}</li> <!-- and bind the name value -->
                    </template>
                </ol>
            </main>
            <footer>
                <!-- footers in a table with style -->
                <table class="foot" style="width:100%">
                    <tr>
                        <td>{@:author}</td>
                        <td>Hello World Sample</td>
                    </tr>
                </table>
            </footer>
        </body>
    </html>


The output from this is much more pleasing. Especially that Fruances font :-)

.. image:: images/HelloWorldWithStyle.png

What Next
==========

We have no idea what you can create with scryber. 
It's just there to hopefully help you create amazing documents in an easy and repeatable way.

* :doc:`document_parameters`
* :doc:`document_structure`
* :doc:`document_styles`
* :doc:`referencing_files`
* :doc:`supported_tags`
* :doc:`supported_css`
* :doc:`mvc_views`