==========================
To code or not to code...
==========================

Scryber does not rely on xml / xhtml, but it makes life easier and is more visual and structured.

When ever you parse a Document or component you are simply creating the same as you could in code.


XHTML Template
------------

.. code-block:: html

    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
          "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml' >

    <head>
        <title>HTML Document</title>
        <style>
            .grey{ background-color: grey; }
        </style>
    </head>

    <body class="grey" title="Page 1">
        <p title="Inner">Hello World, from scryber.</p>
    </body>
    </html>


The same in code
-----------------

.. code-block:: csharp

        var doc = new Document();
        doc.Info.Title = "Coded Document";

        var style = new StyleDefn(".grey");
        style.Background.Color = (PDFColor)"grey";
        doc.Styles.Add(style);

        var pg = new Page();
        pg.StyleClass = "grey";
        pg.OutlineTitle = "Page 1";

        var para = new Paragraph();
        para.Contents.Add(new TextLiteral("Hello World From scryber"));

        pg.Contents.Add(para);
        doc.Pages.Add(pg);

        return doc.ProcessDocument();


The same in XLinQ
------------------

.. code-block:: csharp

            XNamespace html = "http://www.w3.org/1999/xhtml";
            XElement root = new XElement(html + "html",
                new XElement(html + "head",
                    new XElement(html + "title", "XElememt Document"),
                    new XElement(html + "style", ". grey { background-color: gray;}")
                ),
                new XElement(html + "body",
                    new XAttribute("class", "grey"),
                    new XAttribute("title", "Page 1"),
                        new XElement(html + "p",
                        new XAttribute("title", "Inner"),
                        new XText("Hello World from Scryber"))
                    )
                );

            string basePath = string.Empty;
            doc = Document.ParseDocument(root.CreateReader(), basePath, ParseSourceType.DynamicContent);

.. note:: The use of the base path allows relative images and styles to be used from a reader in dynamic content.


Loading partial content
------------------------

Scryber supports the loading of partial content (not a whole document) through the use of the instance method ParseTemplate(), or the Document static Parse method.

There are a number of overloads for the parse template method that use streams, text readers and xml readers. But as a simple loading mechanism it can be useful to include
some custom stored content.

.. code-block:: csharp


    //This content can be loaded from any source.

    var content = "<p xmlns='http://www.w3.org/1999/xhtml' >" +
                    "This <b>Is my content</b>" +
                    "</p>";

    using (var reader = new StringReader(content))
    {
        var comp = doc.ParseTemplate(doc, reader) as Component;
        (doc.Pages[0] as Page).Contents.Add(comp);
    }

The use of the first component argument in ParseTemplate is to provide the source path for any relative references.
It can also be called with the owner and a base path.


Resolving paths to custom content
-----------------------------------

The static Parse method has 12 overloads and counting, from a simple stream and base path, to the explicit PDFGeneratorSettings.
It also supports the use of the PDFReferenceResolver, that can implement custom methods to resolve references to content (e.g. database images, or authenticated document sources).

.. code-block:: csharp

        //custom reference resolver implementation

        private IPDFComponent ResolveReference(string filename, string xpath, PDFGeneratorSettings settings)
        {
            Stream content = GetMyContentForPath(filename);
            return Document.Parse(filename, content, ParseSourceType.DynamicContent, settings.Resolver);
        }


And this method can be used when parsing inner content or documents with references to other content.

.. code-block:: csharp

        using (var reader = new StringReader(content))
        {
            Document.Parse("", reader, ParseSourceType.DynamicContent, new PDFReferenceResolver(this.ResolveReference));
        }



Why use one over the other
--------------------------

We always think that the declarative is better for what you need, but sometimes building in code works.
See the :doc:`document_code_classes` for a break down of the class heierarchy.

In this documentation, we will concentrate on the use of the declarative html with code where appropriate, but remember that 
everything that is declared can be coded too.


