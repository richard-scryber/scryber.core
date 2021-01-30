=============
Scryber 5.0
=============

Scryber is **the** engine to create dynamic documents quickly and easily with consistant styles and easy flowing layout.
It's open source; flexible; styles based; data driven and with a low learning curve. 

A document generation tool written entirely in C# for dotnet 5 using XHTML, CSS and even SVG.

Documentation for previous 1.0.x pdfx versions for `Read the docs here <https://scrybercore.readthedocs.io/en/v1.0.0.20-beta/>`_

-----------------
Hello World +
-----------------

.. code-block:: html

    <!DOCTYPE HTML >
    <!-- The xmlns is needed, and it should all be valid xhtml -->
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

            <!-- document headers are supported -->
            <header>
                <p class="header">Scryber document creation</p>
            </header>

            <!-- support for many HTML5 tags and inline style support -->
            <main style="padding:10pt">

                <!-- binding styles and values on content -->
                <h2 style="{@:model.titlestyle}">{@:model.title}</h2>

                <div>We hope you like it.</div>

                <!-- Loop with nested item collection binding to the objects -->
                <ol>
                    <template data-bind='{@:model.items}'>
                        <!-- just a list, but can be anything, and can be nested -->
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

    //add the namespaces
    //using Scryber.Components;
    //using Scryber.Components.Mvc;

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

..image:: https://raw.githubusercontent.com/richard-scryber/scryber.core/svgParsing/docs/images/helloworld.png


Easy, and intuitive structure
-----------------------------

Whether you are using xhtml templates or directly in code, scryber
is quick and easy to build complex documents from your designs and data.


Intelligent flowing layout engine
---------------------------------

In scryber, content can either be laid out explicitly, or jut flowing with the the page.
Change the page size, or insert content and everything will adjust around it.

Cascading Styles 
----------------

With a styles based structure, it's easy to apply designs to templates. Use class names, id's or component types,
or nested selectors.

Low code, zero code development
-------------------------------

Scryber is based around xml templates - just like XHTML. It can be transformed, it can be added to,
and it can be dynamic built. By design we minimise errors, reduce effort and allow reuse.

Minimal learning curve
-------------------------------

Scryber uses native html content and layout neatly and easily within pages.
It also supports the use of inline and class styles.
This makes it simple to define your templates.


Binding to your data
--------------------

With a simple binding notation it's easy to add references to your data structures and pass information
and complex data to your document from SQL, JSON, Entity Model and more.
Or get the document to look up and bind the data for you.



