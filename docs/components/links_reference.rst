======================================
Links in  and out of documents
======================================

Within a document, it's easy to add a link to another component, another page, 
another document, or remote web link.

Scryber supports the standard ``a`` anchor tag with the ``href`` attribute for linking between items.


Generation methods
-------------------

All methods and files in these samples use the standard testing set up as outlined in :doc:`overview/samples_reference`


The Anchor Component
----------------------

An `a`nchor tag is an invisible component (although styles will be applied to content within it) that allows linking to other components.
The href attribute supports 3 types of content.

* Named Action - These are the standard links to and from pages (see below).
* #id - If set, then the link is an action for a different document or Url. Effectively like the href of an anchor tag in html.
* url - A relative or remote link to a document or webpage.

The content within a link can be anything, including images; text; svg components and more. 
There can also be more than one component within the link.

By default the ``a`` tag is inline with a style applied for any inner text of underlined blue, 
but it does support the use of being positioned as a block and all other styling options.


Page Named Action
------------------

The simplest link type is navigational. The possible actions are (case insensitive) as follows:

* FirstPage
* PrevPage
* NextPage
* LastPage

These are self-evident in their purpose, and no other attributes need defining.
It does not matter what page they are put on, they will perform the action if possible.

.. code-block:: html

    <a href='nextpage' >Next Page Link</a>


For example we can create a navigation header.

.. code-block:: xml

    <!-- /Templates/Links/LinksSimple.html -->
    <!DOCTYPE html>
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <meta charset="utf-8" />
        <title>Simple Links</title>
        <style>
            .break-before{ page-break-before: always; }
        </style>
    </head>
    <body style="padding:20pt">
        <template data-bind="{{pages}}">
            <div class="{{if(index() > 0, 'break-before', 'break-none')}}">
                <h4>Content for the {{pages[index()]}} page with number <page /></h4>
                <a href="FirstPage">First Page</a>,
                <a href="PreviousPage">Previous Page</a>,
                <a href="NextPage">Next Page</a>,
                <a href="LastPage">Last Page</a>
            </div>
        </template>
    </body>
    </html>


.. code:: csharp

    public void SimpleNavigationLinks()
    {
        var path = GetTemplatePath("Links", "LinksSimple.html");

        using (var doc = Document.ParseDocument(path))
        {
            var pages = new string[] { "first", "second", "third", "fourth" };
            doc.Params["pages"] = pages;

            using (var stream = GetOutputStream("Links", "LinksSimple.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

        }
    }

.. figure:: ../images/samples_linkssimple.png
    :target: ../_images/samples_linkssimple.png
    :alt: Simple links to a page.
    :width: 600px
    :class: with-shadow

`Full size version <../_images/samples_linkssimple.png>`_



Linking within documents
------------------------

When navigating around the documment, scryber supports the direct linking to a specific page or component 
using the id being referenced attribute. Prefix with a # to identify it is an element witin the document.

By default anchor links will be underlined and in blue. But can be styled as needed.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
 
    <div id="first" class="break">
        First <br />
        <a href="#second" >Link to the Second page</a>
        <br />
        <a href="#fourth" style="text-decoration:none; color:gray;" >Link to the Fourth page</a>
    </div>
    <div id="second" class="break">Second</div>
    <div id="third" class="break">Third</div>
    <div id="fourth" class="break">
        Fourth <br />
        <a href="#first" >Link to the first page</a>
    </div>
    <div>Fifth</div>

.. image:: images/documentLinksIDs.png




External Links to Urls
-----------------------

Using the href attribute a remote link can be made to any url or local document. If it's not one of the other type it will be assumed to be a link.
Links can also contain images or any other content, and can use the target='_blank' to open in a new tab.

.. code-block:: xml

     <!-- A web link to the google home page -->
    <a href="https://www.google.com" target="_blank" >Google</a><br/>

    <!-- a link to a local pdf that will open in a new readr tab or window -->
    <a alt="Document Link" href="ReadMeSample.pdf" target="_blank" >
        <img src="./images/group.png" style="width:30pt; display:inline" />Document Link</a>
    

.. image:: images/documentLinksUrls.png


.. note:: Some of the browser pdf readers do not support the full navigaional links capabilities (or allow them). Reader applications generally do.