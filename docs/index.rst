=============
Scryber 5.0
=============

**Helping to change the way we can use documents is at the heart of everything we do.**

Scryber is **the** engine to create dynamic documents quickly and easily with consistant styles and easy flowing layout.
It's open source; flexible; styles based; data driven and with a low learning curve. 

A document generation tool written entirely in C# for dotnet 5 using XHTML, CSS and even SVG.

Documentation for previous 1.0.x pdfx versions for `Read the docs here <https://scrybercore.readthedocs.io/en/v1.0.0.20-beta/>`_


Hello World MVC
-----------------

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

Check out :doc:`mvc_controller_full` for a full MVC example with styles and binding, or :doc:`gui_controller_full` for a full gui application example (with styles and binding)


Features
--------


Easy, and intuitive structure
=============================

Whether you are using xhtml templates or directly in code, scryber
is quick and easy to build complex documents from your designs and data using standard xhtml.

See :doc:`document_structure` and `html_tags`


Intelligent flowing layout engine
=================================

In scryber, content can either be laid out explicitly, or jut flowing with the the page.
Change the page size, or insert content and everything will adjust around it.

See `component_positioning` and `document_pages`

Cascading Styles 
================

With a styles based structure, it's easy to apply designs to templates. Use class names, id's or component types,
or nested selectors.

See `document_styles` and `document_structure`

Drawing and Typographic support
===============================

Scryber supports inclusion of Images, Fonts (inc. Google fonts) and SVG components for drawing graphics and icons.

See `drawing_images`, `drawing_fonts` and `drawing_paths`

Binding to your data
====================

With a simple binding notation it's easy to add references to your data structures and pass information
and complex data to your document from SQL, JSON, Entity Model and more.
Or get the document to look up and bind the data for you.

See `binding_model` and `binding_templates`

Extensible Framework
=====================

Scryber was designed from the ground up to be extensible. If it doesn't do what you need, then we think you can make it do it.
From the parser namespaces to the object graph to the writer - it can be built and extended.

See: `extending_scryber` and `extending_configuration`


.. toctree::
    :maxdepth: 2
    :hidden:
    :caption: Getting started

    index
    mvc_controller_full
    gui_controller_full



.. toctree::
    :caption: In depth capabilities
    :maxdepth: 2
    :hidden:

    html_markdown
    mvc_views
    html_tags
    document_code_vs_xml
    mvc_views
    extending_configuration
    extending_scryber
    namespaces_and_assemblies
    version_history


.. toctree::
    :cation: Standard features
    :maxdepth: 2
    :hidden:

    document_structure
    document_styles
    document_components
    referencing_files
    binding_content
    binding_model
    component_linking
    page_numbers
    drawing_fonts
    document_textlayout
    drawing_colors
    drawing_units
    drawing_paths

