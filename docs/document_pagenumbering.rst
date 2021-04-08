================================
Pages Numbers
================================

Putting numbers in pages is often a requirement, but honestly we have never liked the CSS approach.

At scryber we have taken a slightly more declarative approach with the 'page' tag. Browsers do not understand this tag, and will ignore it.
The scryber engine will understand and output the current page number

Current Page Numbers
---------------------

The <page/> tag can be placed anywhere within the text of a document, and will render in the current style. Although it does also support the 
inline style options from as with any other span.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>My Document</title>
        <style>

            p, h1 {
                padding: 10pt;
            }

            .print-only{
                display:none;
            }

            @media print{

                .print-only{ display: block; }

                .foot{
                    border-top: solid 1pt gray;
                    text-align:center;
                    font: 10pt sans-serif;
                    margin: 5pt;
                }
            }

        </style>
    </head>
    <body>
        <header>
            <p>This is the header</p>
        </header>

        <!-- Page number within the content -->
        <h1>This is the content on page <page /></h1>

        <footer>
            <!-- a page number using the current font style in a footer -->
            <p class='print-only foot'> The Current Page is <page /></p>
        </footer>

    </body>

    </html>

And our page footer will display as expected.

.. image:: images/PageNumbers1.png

The page tag also supports the property attribute for displying the 'total' number of pages, and it also the current 'section' or 'sectiontotal' page count.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>My Document</title>
        <style>

            p, h1 {
            padding: 10pt;
            }

            .print-only{
            display:none;
            }

            @media print{

                .print-only{ display: block; }

                .foot{
                border-top: solid 1pt gray;
                text-align:center;
                font: 10pt sans-serif;
                margin: 5pt;
                }

                .break{
                page-break-before:always;
                }
            }

        </style>
    </head>
    <body>
        <header>
            <p>This is the header</p>
        </header>
        <h1 id='First'>This is the content on page <page /> of <page property='total' /></h1>
        <h1 id='Second' class='break'>This is the content on page <page /> of <page property='total' /></h1>
        <h1 id='Third' class='break'>This is the content on page <page /> of <page property='total' /></h1>
        <h1 id='Fourth' class='break'>This is the content on page <page /> of <page property='total' /></h1>
        <footer>
            <p class='print-only foot'> The Current Page is <page /> of <page property='total' /></p>
        </footer>

    </body>

    </html>



.. image:: images/PageNumbers1of2.png



The page for
-------------


Conversly to the current page number, it is also possible to get the page number of another element.
By using the 'for' attribute.

The example below is a table of contents with links to sections based on their 
ID and a line leading to the page numbers on the right cell.

.. code-block:: html

    <table class="toc" style="margin:20pt; width:100%;">
        <thead>
            <tr>
                <td colspan="2">Table of contents</td>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td><a href="#section1">Section 1</a><hr class="tab-spacer" /></td>
                <td class="pg-num"><page for="#section1" /></td>
            </tr>
            <tr>
                <td><a href="#section2">Section 2</a><hr class="tab-spacer" /></td>
                <td class="pg-num"><page for="#section2" /></td>
            </tr>
            <tr>
                <td><a href="#section3">Section 1</a><hr class="tab-spacer" /></td>
                <td class="pg-num"><page for="#section3" /></td>
            </tr>
        </tbody>
    </table>

There is a bit of css fun going on to achieve this..

.. code-block:: css


        table.toc{
            font-size:12pt;
            margin-left:30pt;
        }

        table.toc thead{
            font-weight:bold;
            text-decoration:underline;
        }

        table.toc a{
            text-decoration:none;
        }

        table.toc hr{
            display:inline;
            margin-top:12pt;
            stroke: gray;
            stroke-dasharray: 2;
        }

        table.toc td{
            border:none;
        }

        table.toc td.pg-num {
            width:30pt;
        }

But the output is quite pleasing. And you could use databinding to achieve this.

.. image:: images/PageTableOfContents.png



