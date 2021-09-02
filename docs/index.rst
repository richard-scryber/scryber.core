=============
Scryber 5.1
=============


**Simple, data driven, good looking, documents from templates**

Scryber is **the** engine to create dynamic PDF documents quickly and easily from XHTML templates with consistant styles, your own data, and an easy flowing layout.
It's open source; flexible; styles based; data driven and with a low learning curve. 

Written entirely in C# for dotnet 5 using HTML, CSS and SVG.

Documentation for the 5.0.x versions is here `5.0.6 Read the docs here <https://scrybercore.readthedocs.io/en/v5.0.6-package-release/>`_
Documentation for previous 1.0.x pdfx versions for `1.0.0 Read the docs here <https://scrybercore.readthedocs.io/en/v1.0.0.20-beta/>`_


Hello World MVC
-----------------

Download the nuget package

`<https://www.nuget.org/packages/Scryber.Core.Mvc>`_

Start with a template. **The xmlns namespace declaration is important.**

.. code-block:: html

    <!DOCTYPE HTML >
    <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
        <head>
            <title>{{model}}</title>
        </head>
        <body>
            <div style='padding:10px'>{{hello}}.</div>
        </body>
    </html>

And then generate your template in a view.

.. code-block:: csharp

    //add the namespaces
    using Scryber.Components;
    using Scryber.Components.Mvc;
    using Microsoft.AspNetCore.Mvc;

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
                doc.Params["hello"] = "Hello World";
                return this.PDF(doc); //convenience extension method to return the result.
            }
        }

    }




.. image:: images/HelloWorldIndex.png

Hello World Plus
-----------------

Check out :doc:`overview/mvc_controller_full` for a full MVC example with styles and binding, or :doc:`overview/gui_controller_full` for a full gui application example (with styles and binding)


=========
Features
=========


Easy, and intuitive structure
------------------------------

Whether you are using xhtml templates or directly in code, scryber
is quick and easy to build complex documents from your designs and data using standard xhtml.

:doc:`document_structure`

Intelligent flowing layout engine
----------------------------------

In scryber, content can either be laid out explicitly, or jut flowing with the the page.
Change the page size, or insert content and everything will adjust around it.

:doc:`document_pages`

Cascading Styles 
-----------------

With a styles based structure, it's easy to apply designs to templates. Use class names, id's or component types,
or nested selectors.

:doc:`document_styles`

Drawing and Typographic support
--------------------------------

Scryber supports inclusion of Images, Fonts (inc. Google fonts) and SVG components for drawing graphics and icons.

:doc:`drawing_fonts`, :doc:`drawing_images` and :doc:`drawing_paths`

Standard HTML tags
------------------

Use divs, spans, tables, lists, headers, footers, links, images and many other standard tags to support your document building
or page conversion.

:doc:`document_components`, :doc:`component_linking` or :doc:`drawing_images`

Binding to your data
---------------------

With a simple handlebars binding notation it's easy to add references to your data structures and pass information
and complex data to your document from your model and more.

**Now supporting full expressions support including css var and calc support**

:doc:`binding_model`

Extensible Framework
-----------------------

Scryber was designed from the ground up to be extensible. If it doesn't do what you need, then we think you can make it do it.
With iFrame includes, a namespace based parser engine, and configuration options for images, fonts, binding it's down to your imagination

Secure and Encrypted
-----------------------

Scryber fully supports the PDF restrictions and both 40 bit and 128 bit encryption of documents using owner and user passwords.

:doc:`document_security`


.. toctree::
    :maxdepth: 1
    :hidden:
    :caption: Getting Started

    overview/mvc_controller_full
    overview/gui_controller_full
    overview/packages_and_libs


.. toctree::
    :caption: Standard features
    :maxdepth: 1
    :hidden:

    document_structure
    document_styles
    document_components
    document_pages
    document_pagenumbering
    document_columns
    component_sizing
    component_positioning
    document_references
    binding_model
    component_linking
    drawing_fonts
    document_textlayout
    drawing_colors
    drawing_units
    drawing_images
    drawing_image_backgrounds
    drawing_paths
    document_outline
    document_security


.. toctree::
    :caption: Extended capabilities
    :maxdepth: 1
    :hidden:

    document_code_vs_xml
    document_code_classes
    extending_logging
    namespaces_and_assemblies
    document_controllers
    extending_scryber
    version_history