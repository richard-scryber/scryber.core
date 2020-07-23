================================
Pages and Sections
================================

All the visual content in a document sit's in pages. Scryber supports the use of both a single :doc:`reference/pdf_page` with content within it.
And mutliple flowing pages in a :doc:`reference/pdf_section`.

A Page and its content
======================


A single page has a structure of optional elements

* Header - Optional, but always sited at the top of a page
* Content - Sited between the Header and Footer.
* Footer - Optional, but always sited at the bottom of a page

If a page has a header or footer the available space for the content will be reduced.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Pages>
        <pdf:Page >
            <Header>
                <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="aqua" >This is the header</pdf:H4>
            </Header>
            <Content>
                <pdf:H1 styles:margins="5pt" styles:border-width="1pt" styles:border-color="green" >This is the content</pdf:H1>
            </Content>
            <Footer>
                <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="purple" >This is the footer</pdf:H4>
            </Footer>
        </pdf:Page>
    </Pages>
    
    </pdf:Document>

.. image:: images/documentpages1.png

If the size of the content is more than can fit on a page it will be truncated.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Pages>
        <pdf:Page>
            <Header>
                <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="aqua" >This is the header</pdf:H4>
            </Header>
            <Content>
                <pdf:H1 styles:margins="5pt" styles:border-width="1pt" styles:border-color="green" >This is the content</pdf:H1>
                <pdf:Div styles:margins="5pt" styles:font-size="14pt" styles:border-width="1pt" styles:border-color="navy">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis orci mollis, finibus eros a, 
                tincidunt magna. Mauris efficitur nisl lorem, vitae semper nulla convallis id. Nam dignissim rutrum 
                mollis. Fusce imperdiet fringilla augue non venenatis. Mauris dictum velit augue, ut iaculis risus 
                pulvinar vitae. Aliquam id pretium sem. Pellentesque vel tellus risus. Etiam dolor neque, auctor id 
                convallis hendrerit, tincidunt at sem. Integer finibus congue turpis eu feugiat. Nullam non ultrices enim.<pdf:Br/>
                <pdf:Br/>
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
                nunc, blandit ut sem vitae, bibendum hendrerit ipsum.<pdf:Br/>
            </pdf:Div>
            </Content>
            <Footer>
                <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="purple" >This is the footer</pdf:H4>
            </Footer>
        </pdf:Page>
    </Pages>
    
    </pdf:Document>


.. image:: images/documentpages2.png


Sections and continuation
=========================

A section differs from a page in 2 ways. Firstly the default style has an overflow action of NewPage (rather than Truncate), 
and it also has allows for a definition of a continuation header and footer.

If defined, then the continuation headers and footers will be shown on the following pages, after the first.
If not defined, then the main page headers and footers will be shown.


.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Pages>
        <pdf:Section>
            <Header>
                <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="aqua" >This is the header</pdf:H4>
            </Header>
            <Continuation-Header>
                <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="fuschia" >This is the continuation header</pdf:H4>
            </Continuation-Header>
            <Content>
                <pdf:H1 styles:margins="5pt" styles:border-width="1pt" styles:border-color="green" >This is the content</pdf:H1>
                <pdf:Div styles:margins="5pt" styles:font-size="14pt" styles:border-width="1pt" styles:border-color="navy">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis orci mollis, finibus eros a, 
                tincidunt magna. Mauris efficitur nisl lorem, vitae semper nulla convallis id. Nam dignissim rutrum 
                mollis. Fusce imperdiet fringilla augue non venenatis. Mauris dictum velit augue, ut iaculis risus 
                pulvinar vitae. Aliquam id pretium sem. Pellentesque vel tellus risus. Etiam dolor neque, auctor id 
                convallis hendrerit, tincidunt at sem. Integer finibus congue turpis eu feugiat. Nullam non ultrices enim.<pdf:Br/>
                <pdf:Br/>
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
                nunc, blandit ut sem vitae, bibendum hendrerit ipsum.<pdf:Br/>
            </pdf:Div>
            </Content>
            <Footer>
                <pdf:H4 styles:margins="5pt" styles:border-width="1pt" styles:border-color="purple" >This is the footer</pdf:H4>
            </Footer>
        </pdf:Section>
    </Pages>
    
    </pdf:Document>

Here we can see that the content flows naturally onto the next page, including the padding and borders.
And the continuation header is shown on the second page.

The footer is consistent throughout, so shows on both output pages.

.. image:: images/documentpages3.png
