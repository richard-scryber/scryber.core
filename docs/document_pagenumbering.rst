================================
Pages Numbers
================================

Putting numbers in pages is often a requirement, but honestly we have never liked the CSS approach.

At scryber we have taken a slightly more declarative approach with the 'page' tag. Browsers do not understand this tag, and will ignore it.
The scryber engine will understand and output the current page number

Current Page Numbers
---------------------
.. code-block:: html

    <footer>
        <p class='print-only foot'> The Current Page is <page /></p>
    </footer>

We can add our explicit styles to this, so the content of the footer will not be shown by browsers
But will display nicely on the document.

.. code-block:: css

    .print-only{ display:none;}

    @media print{

        .print-only{ display: block; }

        .foot{
            border-top: solid 1pt gray;
            padding: 10pt;
            text-align:center;
            font: 10pt sans-serif;
            margin: 5pt;
        }
    }

And our page footer will display as expected.

.. image:: images/PageNumbers1.png

The page tag also supports the property attribute for displying the 'total' number of pages, and also the current 'section' or 'sectiontotal' page count.

.. code-block:: html

    <footer>
        <p class='print-only foot'> Page <page /> of <page propery='total' /></p>
    </footer>

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



