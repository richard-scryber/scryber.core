=============
Scryber 5.0
=============

**Helping to change the way we can use documents is at the heart of everything we do.**

Scryber is **the** engine to create dynamic documents quickly and easily with consistant styles and easy flowing layout.
It's open source; flexible; styles based; data driven and with a low learning curve. 

A document generation tool written entirely in C# for dotnet 5 using XHTML, CSS and even SVG.

Documentation for previous 1.0.x pdfx versions for `Read the docs here <https://scrybercore.readthedocs.io/en/v1.0.0.20-beta/>`_

How it works
--------------

We hope scryber works just as you would expect. The scryber engine is based around the controllers you have, using XHTML template views with css, graphics 
and images you are used to, along with your model data you have, to create PDF documents quickly, easily and flexibly, 
**just as you would with web pages**.

.. image:: images/ScryberMVCGraphic.png

Hello World
-------------

Download the nuget package

`<https://www.nuget.org/packages/Scryber.Core.Mvc>`_

Start with a template. **The namespace declaration is important.**

.. code-block:: html

    <!DOCTYPE HTML >
    <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
        <head>
            <title>Hello World</title>
        </head>
        <body>
            <div style='padding:10px'>Hello World.</div>
        </body>
    </html>

And then generate your template in a view.

.. code-block:: csharp

    //add the namespaces
    //using Scryber.Components;
    //using Scryber.Components.Mvc;
    //using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        
        private readonly IWebHostEnvironment _env;
        
        public HomeController(IWebHostEnvironment environment)
        {
            _env = environment;
        }

        [HttpGet]
        public IActionResult HelloWorld()
        {
            // get the path to where you have saved your template 
            var path = _env.ContentRootPath;
            path = System.IO.Path.Combine(path, "Views", "PDF", "HelloWorld.html");

            //parsing the document creates a complete object graph from the content
            using(var doc = Document.ParseDocument(path))
            {
                return this.PDF(doc); //convenience extension method to return the result.
            }
        }

    }


Hello World Plus
-----------------

Adding content, styles and a model to the template to make it a bit more interesting.

.. code-block:: html

    <!DOCTYPE HTML >
    <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
        <head>
            <!-- support for standard document attributes -->
            <title>Hello World</title>
            <meta charset='utf-8' name='author' content='Richard Hewitson' />
            
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

        </head>
        <body>

            <!-- document headers are supported and repeat -->
            <header>
                <p class="header">Scryber document creation</p>
            </header>

            <!-- support for many HTML5 tags and inline style support -->
            <main style="padding:10pt">

                <!-- binding styles and values on content using the {@: ... } syntax -->
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


            <!-- footers that will repeat across pages, using custom paramters -->
            <footer>
                <table class="foot" style="width:100%">
                    <tr>
                        <td>{@:author}</td>

                        <!-- output the current page number using the special page tag -->
                        <td><page /></td>

                        <td>Hello World Sample</td>
                    </tr>
                </table>

            </footer>
        </body>
    </html>


Generating the template in an MVC view
----------------------------------------

.. code-block:: csharp

    []
    public IActionResult HelloWorld()
    {
        var path = _env.ContentRootPath;
        path = System.IO.Path.Combine(path, "Views", "PDF", "HelloWorld.html");

        //parsing the document creates a complete object graph from the content
        using(var doc = Document.ParseDocument(path))
        {
            //your model can be anything
            var model = GetHelloWorldData();

            //make any changes to the document you want, or add paramters (just like a view bag).
            doc.Info.Title = "Hello World Sample";
            doc.Params["author"] = "Scryber Engine";

            //And simply return it as a response with your model data automatically bound
            return this.PDF(doc, model); // , inline:false, outputFileName:"HelloWorld.pdf");
        }
    }

    private dynamic GetHelloWorldData()
    {
        //get your model data however you wish
        //it's just a sample object for this one.

        var model = new
            {
                titlestyle = "color:#ff6347", //binding style data
                title = "Hello from scryber", //binding simple content
                items = new[]                 //or even binding complex object data
                {
                    new { name = "First item" },
                    new { name = "Second item" },
                    new { name = "Third item" },
                }
            };

        return model;
    }


And the output
---------------

.. image:: https://raw.githubusercontent.com/richard-scryber/scryber.core/svgParsing/docs/images/helloworld.png


Easy, and intuitive structure
-----------------------------

Whether you are using xhtml templates or directly in code, scryber
is quick and easy to build complex documents from your designs and data using standard xhtml.

See `html_tags` and `document_structure`


Intelligent flowing layout engine
---------------------------------

In scryber, content can either be laid out explicitly, or jut flowing with the the page.
Change the page size, or insert content and everything will adjust around it.

See `component_positioning` and `document_pages`

Cascading Styles 
----------------

With a styles based structure, it's easy to apply designs to templates. Use class names, id's or component types,
or nested selectors.

See `document_styles` and `document_structure`

Drawing and Typographic support
-------------------------------

Scryber supports inclusion of Images, Fonts (inc. Google fonts) and SVG components for drawing graphics and icons.

See `drawing_images`, `drawing_fonts` and `drawing_paths`

Binding to your data
--------------------

With a simple binding notation it's easy to add references to your data structures and pass information
and complex data to your document from SQL, JSON, Entity Model and more.
Or get the document to look up and bind the data for you.

See `binding_model` and `binding_templates`

Extensible Framework
-------------------------------

Scryber was designed from the ground up to be extensible. If it doesn't do what you need, then we think you can make it do it.
From the parser namespaces to the object graph to the writer - it can be built and extended.

See: `extending_scryber` and `extending_configuration`



