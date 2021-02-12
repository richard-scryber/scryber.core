================================
Pages and Sections
================================

All the visual content in a document sit's in pages. Scryber supports the use of both a single body with content within it.
And also explicit flowing pages in a section.

The use of the page-break-before is set to 'always' on the section, but can, along with page-break-after, be set and supported on any component tag

The body has an optional header and footer that will be used on every page if set.

scryber also supports the use of the @page rule to be able to change the size and orientation of each of the pages either as a whole, or within a section or tag.

The body and its content
--------------------------


A single page has a structure of optional elements

* header - Optional, but always sited at the top of a page
* Sited between the Header and Footer is any content to be included within the page.
* footer - Optional, but always sited at the bottom of a page

If a page has a header or footer the available space for the content will be reduced.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <html xmlns='http://www.w3.org/1999/xhtml' >
    <body>
        <header>
            <h4 style="margins:5pt; border-width:1pt; border-color:aqua" >This is the header</h4>
        </header>
        <h1 style='margins:5pt; border-width=1pt; border-color:green;" >This is the content</h1>
        <footer>
            <h4 styles="margins:5pt; border-width:1pt; border-color:purple" >This is the footer</h4>
        </footer>

    </body>
    
    </html>

.. image:: images/documentpages1.png

Flowing Pages
---------------
If the size of the content is more than can fit on a page it will overflow onto another page. Repeating any header or footer.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <html xmlns='http://www.w3.org/1999/xhtml' >
        <body>
            <header>
                <h4 style='margin: 5pt; border-width: 1pt; border-color:aqua' >This is the header</h4>
            </header>
            <h1 style='margins: 5pt; border-width: 1pt; border-color: green;' >This is the content</h1>
            <div styles="margins:5pt; font-size: 14pt; border-width: 1pt; border-color: navy">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis orci mollis, finibus eros a, 
                tincidunt magna. Mauris efficitur nisl lorem, vitae semper nulla convallis id. Nam dignissim rutrum 
                mollis. Fusce imperdiet fringilla augue non venenatis. Mauris dictum velit augue, ut iaculis risus 
                pulvinar vitae. Aliquam id pretium sem. Pellentesque vel tellus risus. Etiam dolor neque, auctor id 
                convallis hendrerit, tincidunt at sem. Integer finibus congue turpis eu feugiat. Nullam non ultrices enim.<doc:Br/>
                <doc:Br/>
                <!-- Truncated for brevity 
                .
                . -->
                Phasellus ultrices congue semper. Praesent ultrices orci ipsum. Maecenas suscipit tellus elit,
                non ullamcorper nulla blandit sed. Nulla eget gravida turpis, et vestibulum nunc. Nulla mollis
                dui eu ipsum dapibus, vel efficitur lectus aliquam. Nullam efficitur, dui a maximus ullamcorper,
                quam nisi imperdiet sapien, ac venenatis diam lectus a metus. Fusce in lorem viverra, suscipit
                dui et, laoreet metus. Quisque maximus libero sed libero semper porttitor. Ut tincidunt venenatis
                ligula at viverra. Phasellus bibendum egestas nibh ac consequat. Phasellus quis ante eu leo tempor
                maximus efficitur quis velit. Phasellus et ante eget ex feugiat finibus ullamcorper ut nisl. Sed mi
                nunc, blandit ut sem vitae, bibendum hendrerit ipsum.<doc:Br/>
            </div>
            <footer>
                <h4 styles="margin:5pt; border-width: 1pt; border-color: purple;" >This is the footer</h4>
            </footer>
        </body>
    
    </html>

Here we can see that the content flows naturally onto the next page, including the padding and borders.
And the header and footer are shown on the second page.

.. image:: images/documentpages3.png

Page breaks
-------------

When using a section it will by default force a break in the pages using the before the component. 

This can can be stopped by applying the css attribute for page-break-before='avoid' value,
and a page break can be applied to any element using the page-break-before (or page-break-after) attribute set to 'always'.

Margins, padding and depth should be preserved during the page break.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <html xmlns='http://www.w3.org/1999/xhtml' >
        <body>
            <header>
                <h4 style='margin: 5pt; border-width: 1pt; border-color:aqua' >This is the header</h4>
            </header>
            <h1 style='margins: 5pt; border-width: 1pt; border-color: green;' >This is the content</h1>
            <!-- Set a section to not break on the first page -->
            <section styles="margins:5pt; font-size: 14pt; border-width: 1pt; border-color: navy; page-break-before: avoid">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis orci mollis, finibus eros a, 
                tincidunt magna. Mauris efficitur nisl lorem, vitae semper nulla convallis id. Nam dignissim rutrum 
                mollis. Fusce imperdiet fringilla augue non venenatis. Mauris dictum velit augue, ut iaculis risus 
                pulvinar vitae. Aliquam id pretium sem. Pellentesque vel tellus risus. Etiam dolor neque, auctor id 
                convallis hendrerit, tincidunt at sem. Integer finibus congue turpis eu feugiat. Nullam non ultrices enim.
            </section>
            <!-- By default this will start on a new page -->
            <section styles="margins:5pt; font-size: 14pt; border-width: 1pt; border-color: navy;">
                <!-- Truncated for brevity 
                .
                . -->
                Phasellus ultrices congue semper. Praesent ultrices orci ipsum. Maecenas suscipit tellus elit,
                non ullamcorper nulla blandit sed. Nulla eget gravida turpis, et vestibulum nunc. Nulla mollis
                dui eu ipsum dapibus, vel efficitur lectus aliquam. Nullam efficitur, dui a maximus ullamcorper,
                quam nisi imperdiet sapien, ac venenatis diam lectus a metus. Fusce in lorem viverra, suscipit
                dui et, laoreet metus. Quisque maximus libero sed libero semper porttitor. Ut tincidunt venenatis
                ligula at viverra. Phasellus bibendum egestas nibh ac consequat. Phasellus quis ante eu leo tempor
                maximus efficitur quis velit. Phasellus et ante eget ex feugiat finibus ullamcorper ut nisl. Sed mi
                nunc, blandit ut sem vitae, bibendum hendrerit ipsum.<doc:Br/>
            </div>
            <footer>
                <h4 styles="margin:5pt; border-width: 1pt; border-color: purple;" >This is the footer</h4>
            </footer>
        </body>
    
    </html>

Page size and orientation
-------------------------

When outputting a page the default paper size is ISO A4 Portrait (210mm x 29.7mm), however Scryber supports setting the paper size 
either on the page or via styles to the standard ISO or Imperial page sizes, in landscape or portrait, or even a custom size.

* ISO 216 Standard Paper sizes
    * `A0 to A9 <https://papersizes.io/a/>`_
    * `B0 to B9 <https://papersizes.io/b/>`_
    * `C0 to C9 <https://papersizes.io/c/>`_
* Imperial Paper Sizes
    * Quarto, Foolscap, Executive, GovermentLetter, Letter, Legal, Tabloid, Post, Crown, LargePost, Demy, Medium, Royal, Elephant, DoubleDemy, QuadDemy, Statement,


A section can only be 1 size of paper, but different sections and different pages can have different sizes.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Styles>
        
        <!-- changing the default page size to A3 Landscape -->
        <styles:Style applied-type="doc:Page" >
        <styles:Page size="A3" orientation="Landscape"/>
        </styles:Style>

        <!-- a style for portrait pages-->
        <styles:Style applied-class="long" >
        <styles:Page orientation="Portrait"/>
        </styles:Style>

        <!-- set up the default style for a heading 1-->
        <styles:Style applied-type="doc:H1" >
        <styles:Border color="green" width="2"/>
        <styles:Padding all="5pt"/>
        <styles:Margins all="10pt"/>
        <styles:Font size="60pt"/>
        <styles:Position h-align="Center"/>
        </styles:Style>
    </Styles>
    
    <Pages>
        <doc:Page>
        <Content>
            <doc:H1>This is the content on a default page size</doc:H1>
        </Content>
        </doc:Page>

        <doc:Page styles:class="long">
        <Content>
            <doc:H1>This is the content on a portrait page</doc:H1>
        </Content>
        </doc:Page>

        <doc:Section styles:class="long" styles:paper-size="A4">
        <Content>
            <doc:H1>This is the content on an explict page size</doc:H1>
            <!-- Force a break in the page -->
            <doc:PageBreak/>
            <doc:H1 >That continues to the next page</doc:H1>
        </Content>
        </doc:Section>

        <doc:Section>
        <Content>
            <doc:H1>And back to the default size</doc:H1>
        </Content>
        </doc:Section>
    </Pages>
    
    </doc:Document>


.. image:: images/documentpagesizes.png



By applying a header at the group level, we can be sure that it is repeated across all pages.

.. image:: images/documentpagegroups.png

Page numbering
---------------

In the previous example we saw use of the :doc:`reference/pdf_pagenumber` component to display the current page number on a page.
The actual numbering is held at a document level, but can be altered for each group, section or individual page.

See :doc:`document_pagenumbers` for a complete example.

