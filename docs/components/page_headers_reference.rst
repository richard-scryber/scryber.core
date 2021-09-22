=======================================
Page Headers and Footers.
=======================================

All the visual content in a document sits in pages. Scryber supports the use of both a single body with content within it, 
and also explicit flowing pages in a section.

The use of the page-break-before and after is supported on any content to force a new page is set to 'always' on a section, 
but can, along with page-break-after, be set and supported on any component tag

The body has an optional header and footer that will be used on every page if set.

Scryber also supports the use of the @page rule to be able to change the size and orientation of each of the pages either as a whole, or within a section or tag.

The body and its content
--------------------------


A single page has a structure of optional elements

* header - Optional, but always sited at the top of a page
* Sited between the Header and Footer is any content to be included within the page.
* footer - Optional, but always sited at the bottom of a page

If a page has a header or footer the available space for the content will be reduced.

.. code-block:: html

    <!-- -->
    <?xml version="1.0" encoding="utf-8" ?>
    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style>

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

.. image:: images/documentpages1.png

.. note:: Any styles set on the body will be applied to the header and footer as well. e.g. padding or margins.

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
            font-size:12pt;
            padding: 4pt;
            border: solid 1px silver;
        }

        </style>
    </head>
    <body>
        <header>
            <h4>This is the header</h4>
        </header>
        <h1>This is the content</h1>
        <div class='content'>
        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas scelerisque porttitor urna. 
        Duis pellentesque sem tempus magna faucibus, quis lobortis magna aliquam. Nullam eu risus 
        facilisis sapien fermentum condimentum. Pellentesque ut placerat diam, sed suscipit nibh. 
        Integer dictum dolor vel finibus imperdiet. Orci varius natoque penatibus et magnis dis 
        parturient montes, nascetur ridiculus mus. Integer congue turpis at varius porttitor. 
        <!-- Truncated for brevity -->
        nec faucibus ipsum bibendum sed. Nunc tristique risus eu quam porttitor blandit.
        In erat mauris, imperdiet a venenatis eu, tempus a nunc.
        <br/>
        Nullam et erat vel nisl suscipit volutpat id vitae massa. Nunc volutpat feugiat iaculis. 
        Mauris sit amet eleifend augue. Nulla imperdiet eu mauris nec consequat. Donec a urna blandit, 
        porttitor libero vel, rutrum diam. Fusce scelerisque diam eu rutrum vestibulum. 
        Vivamus a quam in nisi euismod laoreet. Morbi mauris augue, lobortis id volutpat in, 
        venenatis ut ex. Donec euismod risus eros, dapibus tincidunt dolor varius id. 
        </div>
        <footer>
            <h4>This is the footer</h4>
        </footer>

    </body>

    </html>



Here we can see that the content flows naturally onto the next page, including the padding and borders.
And the header and footer are shown on the second page.

.. image:: images/documentpages3.png

Page breaks
-------------

When using a <section> it will, by default, force a break in the pages using the before the component, so that it flows
nicely onto a new page and begins the new content from there. (the default style is page-break-before:always)

This behaviour can can be stopped by applying the css attribute for 'page-break-before:avoid' value,
and a page break can also be applied to any element using the style 'page-break-before:always' (or 'page-break-after:always').

Margins, padding, boarder and depth should be preserved during the page break, and the engine 
will try and layout the content appropriately for breaks inside nested elements.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css">

            header, footer {
            padding: 10pt;
            background-color: #333;
            color: #EEE;
            border-bottom: 1px solid black;
            border-top: 1px solid black;
            }

            body .content {
            margin: 20pt;
            font-size:12pt;
            padding: 4pt;
            border: solid 1px silver;
            }

        </style>
    </head>
    <body>
        <header>
            <h4>This is the header</h4>
        </header>
        <h1>This is the content</h1>

        <!-- section that does not force a new page (so that it stays on the first page -->
        <section class='content' style="page-break-before:avoid">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas scelerisque porttitor urna.
            <!-- Truncated for brevity -->
            Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos.
            Praesent mollis tempor enim.<br />

        </section>

        <!-- This will be default appear on a new page -->
        <section class='content'>
            Nullam et erat vel nisl suscipit volutpat id vitae massa. Nunc volutpat feugiat iaculis.
            Mauris sit amet eleifend augue. Nulla imperdiet eu mauris nec consequat. Donec a urna blandit,
            <!-- Truncated for brevity -->
            sagittis dignissim volutpat. Integer efficitur euismod lectus at varius. Vestibulum euismod massa mauris.
            Mauris laoreet urna est, et tristique velit lobortis eu.
        </section>

        <!-- Any tag can force a new page within the document flow, and it does not have to be at the
            root level. Borders and spacing will be preserved as much as possible -->
        <div class="content">
            The inner content will be on a new page.
            <div class='content' style="page-break-before:always;">
                Phasellus luctus dapibus nisi, et pulvinar neque ultrices vitae. Pellentesque quis purus felis.
                <!-- Truncated for brevity -->
                venenatis ut ex. Donec euismod risus eros, dapibus tincidunt dolor varius id.
            </div>
            After the content.
        </div>
        <footer>
            <h4>This is the footer</h4>
        </footer>

    </body>

    </html>

.. image:: images/SectionsOverflow.png

Page size and orientation
-------------------------

When outputting a page the default paper size is ISO A4 Portrait (210mm x 29.7mm), however Scryber supports setting the paper size 
either on the section or via styles to the standard ISO or Imperial page sizes, in landscape or portrait.

* ISO 216 Standard Paper sizes
    * `A0 to A9 <https://papersizes.io/a/>`_
    * `B0 to B9 <https://papersizes.io/b/>`_
    * `C0 to C9 <https://papersizes.io/c/>`_
* Imperial Paper Sizes
    * Quarto, Foolscap, Executive, GovermentLetter, Letter, Legal, Tabloid, Post, Crown, LargePost, Demy, Medium, Royal, Elephant, DoubleDemy, QuadDemy, Statement,


The body or a section can only be 1 size of paper, but different sections (or page breaks) can be different pages and can have different sizes.

An @page { ... } rule will apply to all pages in the document.

To specify an explicit named page size use the name after the @page rule, and then 
identify the rule with the page css declaration either on the tag style or in css. 
The same priories will be applied if multiple page values are matched.

To revert back to the default size use a value of auto or initial.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css">

            header, footer {
            padding: 10pt;
            background-color: #333;
            color: #EEE;
            border-bottom: 1px solid black;
            border-top: 1px solid black;
            }

            body .content {
            margin: 20pt;
            font-size:12pt;
            padding: 4pt;
            border: solid 1px silver;
            }

            .small-page{
            page: initial;
            }

            .big-page{
            page: landscape;
            }

            /* This will be the default initial size */
            @page {
            size: A4 landscape;
            }

            /* any new pages with the page:landscape will
            use this size */

            @page landscape {
            size: A3 landscape;
            }

        </style>
    </head>
    <body>
        <header>
            <h4>This is the header</h4>
        </header>
        <h1>This is the content</h1>

        <!-- section that does not force a new page (so that it stays on the first page -->
        <section class='content' style="page-break-before:avoid">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas scelerisque porttitor urna.
            Duis pellentesque sem tempus magna faucibus, quis lobortis magna aliquam. Nullam eu risus

            <!-- Truncated for brevity -->

            Praesent mollis tempor enim.
        </section>

        <!-- This will be on the A3 landscape page -->
        <section class='content big-page'>
            Nullam et erat vel nisl suscipit volutpat id vitae massa. Nunc volutpat feugiat iaculis.
            Mauris sit amet eleifend augue. Nulla imperdiet eu mauris nec consequat. Donec a urna blandit,

            <!-- Truncated for brevity -->

            Mauris laoreet urna est, et tristique velit lobortis eu.
        </section>

        <!-- The inner div of small-page will revert the size back to the default (initial) size  -->
        <div class="content">
        
            The inner content will be on a new page.

            <div class='content small-page' style="page-break-before:always;">
                Phasellus luctus dapibus nisi, et pulvinar neque ultrices vitae. Pellentesque quis purus felis.
                Aliquam feugiat efficitur sem quis placerat. Quisque viverra magna vitae elit eleifend, a porttitor
                enim vulputate. Quisque elit metus, aliquam eget purus at, blandit gravida diam.

                <!-- Truncated for brevity -->

                venenatis ut ex. Donec euismod risus eros, dapibus tincidunt dolor varius id.
            </div>
            After the content.
        </div>
        <footer>
            <h4>This is the footer</h4>
        </footer>

    </body>

    </html>


.. image:: images/SectionsPageSizes.png


Stopping overflow
-------------------

If overflowing onto a new page is not required or wanted then the 
page-break-inside='avoid' will block any overflow or new pages.

A section can be a single page, and never overflow.

