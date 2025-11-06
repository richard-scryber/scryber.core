================================
Body, Pages, breaks and sizes
================================

All the visual content in a document sits in pages. Scryber supports the use of both a single body with content within it.

.. figure:: ../images/samples_pageSizes.png
    :alt: Changing page sizes in a document.
    :width: 600px

The use of the `page-break-before` or `page-break-after` is supported on any content to force a new page when set to 'always' on any component tag

The body has an optional header and footer that will be used on every page if set.

Scryber also supports the use of the @page rule to be able to change the size and orientation of each of the pages either as a whole, or within a section or tag.

.. code:: html

    <body>
        <header>Header on every page</header>
        <div>On the first page</div>
        <div class='next-page' >On the second page in landscape</div>
    </body>

.. code:: css

    @page{ size: A4 portrait }

    @page landscape { size: A4 landscape }

    .next-page {
        page: landscape;
        page-break-before: always;
    }

In code a document can have ``Page``s, ``Section``s and ``PageGroup``s added to it that allow inner content to be split over different parts of the document.
A styled component can also have it's ``Style.Page.BreakBefore`` or ``Style.Page.BreakBefore`` set to `true` and flow onto a new page (if allowed).

.. code:: csharp

    using(var doc = new Document())
    {
        var sect = new Section();

        var div1 = new Div();
        div1.Contents.Add(new TextLiteral("On the first page"));
        sect.Contents.Add(div1);
        doc.Pages.Add(sect);

        sect = new Section();
        sect.PaperSize = PaperSizes.A4;
        sect.PaperOrientation = PaperOrientation.Landscape;

        var div2 = new Div();
        div2.Contents.Add(new TextLiteral("On the second page"));
        sect.Contents.Add(div2);
        doc.Pages.Add(sect);
    }

Generation methods
-------------------

All methods and files in these samples use the standard testing set up as outlined in :doc:`../overview/samples_reference`


The body and its content
--------------------------


A body section has a structure of optional elements

* header - Optional, but always sited at the top of a page
* Sited between the Header and Footer is any content to be included within the page.
* footer - Optional, but always sited at the bottom of a page

If a page has a header or footer the available space for the content will be reduced.
Headers and footers can contain any content in the same way as any other block.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style>

        body {
            background-color: #DDD;
        }

        header, footer{
                padding: 10pt;
                background-color: #333;
                color: #EEE;
                border-bottom: 1px solid black;
                border-top: 1px solid black;
        }
        
        h1{
            padding: 20pt;
        }

        </style>
    </head>
    <body>
        <header>
            <h4>This is the header</h4>
        </header>
        <h1>This is the content</h1>
        <footer>
            <h4>This is the footer</h4>
        </footer>

    </body>

    </html>

.. code:: csharp

    //Scryber.UnitSamples/PagesSamples.cs

    public void SimpleNavigationLinks()
    {
        var path = GetTemplatePath("Pages", "PagesSimple.html");

        using (var doc = Document.ParseDocument(path))
        {
            using (var stream = GetOutputStream("Pages", "PagesSimple.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

        }
    }



.. figure:: ../images/samples_pagesSimple.png
    :target: ../_images/samples_pagesSimple.png
    :alt: Simple Pages.
    :width: 600px
    :class: with-shadow

`Full size version <../_images/samples_pagesSimple.png>`_


.. note:: Any styles set on the body will be applied to the header and footer as well. e.g. padding or margins. But they can have their own (overriding) styles as well.

Single body structure
---------------------

In the example above the ``html`` tag references the ``Scryber.Html.Components.HTMLDocument`` class that inherits from the
``Scryber.Components.Document`` class.

See :doc:`../overview/scryber_parsing` for more information on how instances are created from elements.

The ``HTMLDocumemt`` has 2 properties on it for the `head` (``HTMLHead``) and `body` (``HTMLBody``) that are matched to the content 
of the template.

The ``HTMLBody`` inherits from the ``Scryber.Components.Section`` which in itself inherits from 
the ``Scryber.Components.Page`` class.and supports multiple pages, and then the ``Scryber.Components.PageBase``
that all page components should inherit from.

The ``HTMLHead`` is a specific html component that wraps the title and `Contents` for links, styles etc.


.. figure:: ../images/diagrams_DocumentClasses.png
    :target: ../_images/diagrams_DocumentClasses.png
    :alt: Page class hierarchy.
    :width: 600px
    :class: with-shadow

`Full size version <../_images/diagrams_DocumentClasses.png>`_



Flowing Pages
---------------

If the size of the content is more than can fit on a page it will overflow onto another page. Repeating any header or footer.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style>

            header, footer {
                padding: 10pt;
                background-color: #333;
                color: #EEE;
                border-bottom: 1px solid black;
                border-top: 1px solid black;
            }

            body h1, body div {
                margin: 20pt;
            }

            body div.content {
                font-size: 12pt;
                padding: 4pt;
                border: solid 1px silver;
                column-count: 2;
            }
        </style>
    </head>
    <body>
        <header>
            <h4>This is the header</h4>
        </header>
        <h1>This is the content</h1>
        <!-- main content in the document
            bound from the parameter 'content' -->
        <div class='content' style="white-space: pre-wrap">{{content}}</div>
        <footer>
            <h4>This is the footer</h4>
        </footer>

    </body>

    </html>

Loading a long text file and binding to the `content` parameter, we use the ``white-space: pre-wrap`` style
so the carriage returns are preserved, but the text will flow in the columns and over multiple pages.

.. code:: csharp

    //Scryber.UnitSamples/PagesSamples.cs

    public void PagesFlowing()
    {
        var path = GetTemplatePath("Pages", "PagesFlowing.html");

        var txtPath = GetTemplatePath("Pages", "LongTextFile.txt");
        doc.Params["content"] = System.IO.File.ReadAllText(txtPath);

        using (var doc = Document.ParseDocument(path))
        {
            using (var stream = GetOutputStream("Pages", "PagesFlowing.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

        }
    }

Here we can see that the content flows naturally onto the next pages, including the padding and borders.
And the header and footer are shown on the following pages.

.. figure:: ../images/samples_pagesFlowing.png
    :target: ../_images/samples_pagesFlowing.png
    :alt: Pages flowing across multiple layouts.
    :width: 600px

`Full size version <../_images/samples_pagesFlowing.png>`_

Page breaks
------------

Using the `page-break-before: always` and `page-break-after: always` css properties, we can force content onto 
a new page in the flow.

In this example we have set up a ``h1`` to force the break after so the rest of the content will be on a new page.

.. code:: css

    body h1.title {
        page-break-after : always;
    }

The breaking can be at any depth, and borders; padding; margins; etc. should be preserved.

.. code:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style>

            header, footer {
                padding: 10pt 20pt 10pt 20pt;
                background-color: #333;
                color: #EEE;
                border-bottom: 1px solid black;
                border-top: 1px solid black;
            }

            header{
                text-align: right;
            }

            body h1, body div {
                margin: 20pt;
            }

            body div.content {
                font-size: 12pt;
                padding: 4pt;
                border: solid 1px silver;
                column-count: 2;
            }

            /* title page with background image
                and page-break-after */
            body h1.title{
                background-image: url(../../images/landscape.jpg);
                background-size: cover;
                font: 30pt serif;
                color: white;
                height: 300pt;
                margin: 0;
                vertical-align:middle;
                text-align:center;
                page-break-after: always;
            }

        </style>
    </head>
    <body>
        <header>
            <h4>This is the header</h4>
        </header>

        <!-- title content that forces a
        page break after -->

        <h1 class="title">
            This is the title
        </h1>

        <h1>This is the content</h1>
        <div class='content' style="white-space: pre-wrap">{{content}}</div>
        <footer>
            <h4>This is the footer</h4>
        </footer>

    </body>

    </html>


.. code:: csharp

    public void PagesBreaks()
    {
        var path = GetTemplatePath("Pages", "PagesBreaks.html");

        using (var doc = Document.ParseDocument(path))
        {
            var txtPath = GetTemplatePath("Pages", "LongTextFile.txt");
            doc.Params["content"] = System.IO.File.ReadAllText(txtPath);

            using (var stream = GetOutputStream("Pages", "PagesBreaks.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

        }
    }


.. figure:: ../images/samples_pageBreaks.png
    :target: ../_images/samples_pageBreaks.png
    :alt: Breaking on various pages.
    :width: 600px

`Full size version <../_images/samples_pageBreaks.png>`_


Page sizes
----------

The default page size for a layout in scryber is A4 portrait. 
Scryber supports the use of the ``@page`` directive to alter the size of the layout page in the document.


.. code:: css

    @page {
        size: A4 landscape;
    }

This will change **all** the pages to use landscape layout.

To define specific page sizes the `@page` directive can be followed by a label and then that label applied to the style of
the component that is currently forcing a new page.

.. code:: css

    @page main-body {
        size: A4 portrait;
    }

    .main {
        page: main-body;
        page-break-before: always;
    }

.. note:: As the layout page will be created when a page-break property css is met, the `page` property should be set at that level. This means that a component that has the page-break-after property, should also stipulate which page size to use.

Scryber supports the use of the following page sizes.

* ISO 216 Standard Paper sizes
    * `A0 to A9 <https://papersizes.io/a/>`_
    * `B0 to B9 <https://papersizes.io/b/>`_
    * `C0 to C9 <https://papersizes.io/c/>`_
* Imperial Paper Sizes
    * Quarto, Foolscap, Executive, GovermentLetter, Letter, Legal, Tabloid, Post, Crown, LargePost, Demy, Medium, Royal, Elephant, DoubleDemy, QuadDemy, Statement,
  
But custom values can be used for a specific width or height on the `size` property.

.. code:: css

    @page {
        size: 200mm 200mm;
    }


Putting this together with the example above, the title page uses the default A4 landscape size, and following pages use the portrait size.

.. code:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style>

            header, footer {
                padding: 10pt 20pt 10pt 20pt;
                background-color: #333;
                color: #EEE;
                border-bottom: 1px solid black;
                border-top: 1px solid black;
            }

            header{
                text-align: right;
            }

            body h1, body div {
                margin: 20pt;
            }

            body div.content {
                font-size: 12pt;
                padding: 4pt;
                border: solid 1px silver;
                column-count: 2;
            }

            body h1.title{
                background-image: url(../../images/landscape.jpg);
                background-size: cover;
                font: 30pt serif;
                color: white;
                height: 300pt;
                margin: 0;
                vertical-align:middle;
                text-align:center;
            }

            /* The main will force a new page
                of style main-content
            */
            body h1.main{
                page-break-before: always;
                page: main-content;
            }

            /* default */
            @page {
                size: A4 landscape;
            }

            /* main content specific */
            @page main-content {
                size: A4 portrait;
            }

        </style>
    </head>
    <body>
        <header>
            <h4>This is the header</h4>
        </header>

        <h1 class="title">
            This is the title
        </h1>

        <!-- this now forces a break before and
            specifies the page orientation of portrait -->
        <h1 class="main">This is the content</h1>

        <div class='content' style="white-space: pre-wrap">{{content}}</div>
        <footer>
            <h4>This is the footer</h4>
        </footer>

    </body>

    </html>

.. code:: csharp

    public void PagesSizes()
    {
        var path = GetTemplatePath("Pages", "PageSizes.html");

        using (var doc = Document.ParseDocument(path))
        {
            var txtPath = GetTemplatePath("Pages", "LongTextFile.txt");
            doc.Params["content"] = System.IO.File.ReadAllText(txtPath);

            using (var stream = GetOutputStream("Pages", "PageSizes.pdf"))
            {
                doc.SaveAsPDF(stream);
            }
        }
    }
    

.. figure:: ../images/samples_pageSizes.png
    :target: ../_images/samples_pageSizes.png
    :alt: Changing page sizes in a document.
    :width: 600px

`Full size version <../_images/samples_pageSizes.png>`_




Creating pages in code.
-----------------------

As with everything else in scryber, it is simple and easy to create pages in code from the document and pagebase classes.

It is also possible to insert pages, sections and page groups to an existing parsed template. As the ``body`` inherits from ``Scyber.Components.Section`` this will
be parsed as a single section.

For headers and footers, these are supported through the ``IPDFTemplate`` interface. 
See :doc:`page_headers_reference` for more on this topic.

.. code:: csharp


    public void PagesCoded()
    {
        using(var doc = new Document())
        {
            //Define the title style that matches onto the '.title' style class.
            var titleStyle = new StyleDefn(".title");
            
            titleStyle.Background.ImageSource = "../../../Images/Landscape.jpg";
            titleStyle.Background.PatternRepeat = PatternRepeat.Fill;
            titleStyle.Position.VAlign = VerticalAlignment.Middle;
            titleStyle.Position.HAlign = HorizontalAlignment.Center;
            titleStyle.Size.Height = 300;
            titleStyle.Font.FontSize = 30;
            titleStyle.Fill.Color = PDFColors.White;
            titleStyle.Font.FontFamily = new PDFFontSelector("serif");


            //Define the body style that matches onto the '.body' style class
            var bodyStyle = new StyleDefn(".body");
            bodyStyle.Font.FontSize = 12;
            bodyStyle.Padding.All = 10;
            bodyStyle.Border.Color = (PDFColor)"#AAA";
            bodyStyle.Columns.ColumnCount = 2;

            var textStyle = new StyleDefn(".preserve");
            textStyle.Text.PreserveWhitespace = true;

            //Add the styles to the document
            doc.Styles.Add(bodyStyle);
            doc.Styles.Add(titleStyle);
            doc.Styles.Add(textStyle);

            //Create a page with a size
            var pg = new Page()
            {
                PaperSize = PaperSize.A4,
                PaperOrientation = PaperOrientation.Landscape
            };

            //add it to the document Pages collection
            doc.Pages.Add(pg);

            //Create new instances of the header and footer classes that implement
            //The IPDFTemplate interface and set to the header and footer.
            pg.Header = new CodedHeader();
            pg.Footer = new CodedFooter();

            //Create the title div and add it to the first page
            var div = new Div();
            div.StyleClass = "title";
            pg.Contents.Add(div);

            //With some text in it.
            var txt = new TextLiteral("This is the title page");
            div.Contents.Add(txt);

            //Now add a section to the document
            var sect = new Section()
            {
                PaperOrientation = PaperOrientation.Portrait
            };
            doc.Pages.Add(sect);

            //Set the header and footer (to the same as the page)
            sect.Header = pg.Header;
            sect.Footer = pg.Footer;

            //Add a header
            var contentTitle = new Head1() { Text = "This is the loaded content", Margins = new PDFThickness(20) };
            sect.Contents.Add(contentTitle);

            //And add the body content to the section.
            var body = new Div();
            //Add the body class, and preserve so extra returns are retained
            //Will still wrap text.
            body.StyleClass = "body preserve";
            sect.Contents.Add(body);

            //Read some long plain text from a file into a text literal
            var path = GetTemplatePath("Pages", "LongTextFile.txt");
            var content = new TextLiteral();
            content.Text = System.IO.File.ReadAllText(path);

            //We set the style to preserve, so that the white space in the content is retained
            content.StyleClass = "preserve";
            
            //Add it to the body.
            body.Contents.Add(content);

            //And process in the same way
            using (var stream = GetOutputStream("Pages", "PagesCoded.pdf"))
            {
                doc.SaveAsPDF(stream);
            }
        }


    }

    /// <summary>
    /// IPDFTemplate for the header
    /// </summary>
    private class CodedHeader : IPDFTemplate
    {
        public IEnumerable<IPDFComponent> Instantiate(int index, IPDFComponent owner)
        {
            return new IPDFComponent[]
            {
                new Head4(){
                    Text = "This is the coded header",
                    Padding = new PDFThickness(10, 20, 10, 20),
                    Margins = PDFThickness.Empty(),
                    BackgroundColor = PDFColors.Silver,
                    HorizontalAlignment = HorizontalAlignment.Right
                }
            };
        }
    }

    /// <summary>
    /// IPDFTemplate for the footer
    /// </summary>
    private class CodedFooter : IPDFTemplate
    {
        public IEnumerable<IPDFComponent> Instantiate(int index, IPDFComponent owner)
        {
            var div = new Div() {
                BackgroundColor = PDFColors.Silver,
                FillColor = PDFColors.White,
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new PDFThickness(10)
            };
            div.Contents.Add(new PageNumberLabel() { DisplayFormat = "{0} of {1}" });

            return new IPDFComponent[] { div };
        }
    }


.. figure:: ../images/samples_pageCoded.png
    :target: ../_images/samples_pageCoded.png
    :alt: Creating pages in code.
    :width: 600px

`Full size version <../_images/samples_pageCoded.png>`_

Coded page breaks
------------------

The components in code support the page break before and page break after style.

.. code:: csharp

    content.Style.Page.BreakBefore = true;


To add an explicit page break in a ``Section`` the ``PageBreak`` component can be added to the content.

.. code:: csharp

    var pbreak = new PageBreak();
    body.Contents.Add(pbreak);

    //this can also be disabled with the Visible property

    pbreak.Visible = false;

