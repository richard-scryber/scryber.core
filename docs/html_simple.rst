==============================
Adding html content
==============================

Scryber supports the use of html within a page as a fragment, or as a full page.

The Html fragment
=================

The easiest way is to simply put the content within the HtmlFragment element.
As the templates use pure xml the content should be XHTML, and it should also declare a namespace.
Scyber does not force this, but it is simply to be valid Xml.

The content within the tag will flow naturally, as a fully qualified component within the document.


.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
    <Pages>
        
        <doc:Section styles:margins="20pt" styles:font-size="12pt">
        <Content>

            <doc:HtmlFragment>
                <h1>Html Content</h1> 
            </doc:HtmlFragment>

            <doc:Div styles:column-count="2" styles:height="200pt">
                <doc:HtmlFragment >
                    <div xmlns="http://www.w3.org/1999/xhtml" style="color:#FFF; background-color:#323232; padding: 10px">
                        <p>This is a paragraph of text</p>
                        <table style="width:400pt; padding: 10pt 0pt; color:#00a8a1">
                        <tr>
                            <td>Cell1</td>
                            <td>Cell2</td>
                            <td>Cell3</td>
                        </tr>
                        <tr  style="background-color:#00a8a1; font-family: 'Arial'; font-size: 30px; color:#323232; font-weight:bold;">
                            <td style="width:100pt">Cell1</td>
                            <td>Cell2</td>
                            <td>Cell3</td>
                        </tr>
                        <tr>
                            <td colspan="2">Cell1</td>
                            <td>Cell2</td>
                        </tr>
                        <tr>
                            <td>Cell1</td>
                            <td>Cell2</td>
                            <td>Cell3</td>
                        </tr>
                        <!-- 
                            Truncated for brevity
                        -->
                        
                        <tfoot>
                            <tr>
                            <td>Footer1</td>
                            <td>Footer2</td>
                            <td>Footer3</td>
                            </tr>
                        </tfoot>
                        </table>
                    </div>
                </doc:HtmlFragment>
            </doc:Div>
        </Content>
        </doc:Section>

    </Pages>
    
    </doc:Document>


Here we have 2 fragments. The first is a simple heading that inherits all the style information form a doc:H1 heading
The second we have added some more complex mixed content and styles within the body of an Html Fragment.
Again the elements inherit the style of their base scryber components, but also have the explicit styles added.

The fragment is within a 2 column Div of a fixed height, so wraps fluidly across the page.

.. image:: images/documentHtmlSimple.png

.. note:: Within the content and html and body tags will be ignored, and there should be only 1 root element.
