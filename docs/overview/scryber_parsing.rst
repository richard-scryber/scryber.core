==================================
Parsing from stream to DOM
==================================

When you parse the contents of an XML file, or a stream or a reader, Scryber builds a full object model of the content.
As the parser is based around XML it is important that all content is valid - it does not like unclosed tags or elements.

Content namespaces
-------------------

The namespace for an element must be known to the parser. For most XHTML templates this will be the standard XML Namespace (xmlns)  http://www.w3.org/1999/xhtml
This namespace is mapped directly onto the library assembly and namespace *Scryber.Html.Components, Scryber.Components*

In the library there is a class called *HTMLDocument* that is decorated with the *PDFParsableComponent* attribute with a name of 'html'.
This is how the parser knows that when it sees an XHMTL element called *html* it should create an instance of the *Scryber.Html.Components.HTMLDocument* class.

This class has a couple of properties on it for *Head* and *Body* that are decorated with the *PDFElement* attribute with names *head* and *body* respectively. 
So the parser knows when it reads elements with this name, the values should be set as instances of the classes *HTMLHead* and *HTMLBody*.

And so it goes on into the rest of the xml reading elements and attributes and trying to set the values to components or property values.

.. code::html

    <!DOCTYPE HTML >
    <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
        <head>
            <title>Hello World</title>
        </head>
        <body>
            <div style='padding:10px'>Hello World.</div>
        </body>
    </html>

And this would be parsed into the following DOM

.. figure:: ../images/doc_object_model.png
    :target: ../_images/doc_object_model.png
    :alt: Parsed Document Object Model
    :class: with-shadow

`Full size version <../_images/doc_object_model.png>`_

Parsing Documents
-----------------

The easiest way to parse any xml content is to use the various static methods on the *Scryber.Components.Document* class.

There are 2 variants called ParseDocument and Parse. 

ParseDocument has 6 overloads and the content parsed must have a root object that is (or inherits from) *Scryber.Components.Document*
The simplest is to load directly from a file

.. code:: csharp

    //using Scryber.components

    string filepath = GetPathToFile();
    var doc = Document.Parse(filepath);

This reads the file from the stream and will resolve any references to relative content (images, stylesheets, etc) based on the *filepath*.

If you want to load content dynamically from a stream then you can use the overloads that take a stream.
An enumeration value for ParseSourceType must be provided, and an optional path value, so the parser can know where other references may reside.

.. code:: csharp

    //from a stream with no references
    using(var content = GetMyDocumentContent())
    {
        doc = Document.ParseDocument(content, PaseSourceType.DynamicContent);
    }

If the stream will contain relative path references to other content such as stylesheets or embedded content then a path should be provided.
If no path is provided then content will be looked for relative to the current executing assembly. 

    //from a stream where references are known to be stored
    var path = "C:/MyFiles/BasePath";
    using(var content = GetMyDocumentContent())
    {
        doc = Document.ParseDocument(content, path, PaseSourceType.DynamicContent);
    }

The options for the content can be any of the following.

* A *System.IO.Stream* or one of its sublcasses.
* A *System.IO.TextReader* or one of its subclasses.
* A *System.XML.XmlReader* or one of its subclasses.

Ultimately the content should be valid XML that can be read.

For example, using an XmlReader

.. code:: csharp

    XNamespace ns = "http://www.w3.org/1999/xhtml";

    var html = new XElement(ns + "html",
        new XElement(ns + "head",
            new XElement(ns + "title",
                new XText("Hello World"))
            ),
        new XElement(ns + "body",
            new XElement(ns + "div",
                new XAttribute("style", "padding:10px"),
                new XText("Hello World."))
            )
        );

    using(var reader = html.CreateReader())
    {
        //passing an empty string to the path as we don't have images or other references to load
        var doc = Document.ParseDocument(reader, string.Empty, ParseSourceType.DynamicContent);
    }


Or from a string itself

.. code:: csharp

    var title = "Hello World";
    var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>" + title + @"</title>
                        </head>
                    <body>
                        <div style='padding: 10px' >" + title + @".</div>
                    </body>
                </html>";

    using (var reader = new StringReader(src))
    {
        var doc = Document.ParseDocument(reader, string.Empty, ParseSourceType.DynamicContent);
    }


Parsing and Reference Resolvers
--------------------------------

Sometimes the content that is to be returned is not a complete document, but a fragment and it's source may not be known at build time.

The *Document.Parse* method, and its 12 overloads allows for parsing of any xml content as long as the root component returned implements the IPDFComponent interface.
If there are references to other content, that needs to be resolved at runtime it is also possible to pass a PDFReferenceResolver delegate to the parser so that your
code can load it's own content and return it.

.. code:: csharp

    public delegate IPDFComponent PDFReferenceResolver(string filename, string xpath, PDFGeneratorSettings settings);

This delegate will be called each time a remote reference is found, with the name of the file, and an optional xpath selector. 
It is upto the implementor to perform the parsing.

For example if we wanted to embed some standard content we could provide our own implementation.

.. code:: csharp

    private IPDFComponent CustomResolve(string filepath, string xpath, PDFGeneratorSettings settings)
    {
        if(filepath == "MyTsAndCs")
        {
            using(var tsAndCs = LoadTermsStream())
            {
                //We have our stream so just do the parsing again with the same settings
                return Document.Parse(filepath, tsAndCs, ParseSourceType.DynamicContent, CustomResolve, settings);
            }
        }
        else
        {
            filepath = System.IO.Path.Combine(MyBasePath, filepath);
            return Document.Parse(filepath, CustomResolve, settings);
        }
    }


    private Document LoadDocument()
    {
        var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>" + title + @"</title>
                        </head>
                    <body>
                        <div style='padding: 10px' >" + title + @".</div>
                        <embed id='TsAndCs' src='MyTsAndCs' />
                    </body>
                </html>";

        using (var reader = new StringReader(src))
        {
            //Execute the parsing with the custom resolver
            var doc = Document.Parse(string.Empty, reader, ParseSourceType.DynamicContent, CustomResolve);
        }
    }
    
.. note:: Remember, the content to be parsed MUST be valid XML. So the content returned from the LoadTermsStream() method should be valid xml in its own right, including all XML namespaces.



Extending namespaces
--------------------

The scryber parsing engine is declarative and does not rely on knowing what it is meant to be parsing.
As such it is easy to extend the namespaces it looks at to build object graphs (infact the html and svg classes are built directly on top of the base component classes).

See :doc:`namespaces_and_assemblies` for more information on how to extend the namespaces and used by the parser.