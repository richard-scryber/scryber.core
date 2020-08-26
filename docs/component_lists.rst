=======================================
Unordered, Ordered and Definition Lists
=======================================

Scryber supports the use of lists both ordered and unordered and allows nesting, overflow, and definition lists.
It also supports the use of binding and repeating.

Simple unordered and ordered lists
-------------------------------------

Scryber uses the pdf:List as the generic component with a default bullet style, but same 
tags as Html for the Ordered Lists (pdf:Ol) and Unordered Lists (pdf:Ul) as short hand for these variants.

The contents of a list item (pdf:Li) can be any form of content (inline or otherwise), but as with table rows, the content does not split across columns or pages.
The entire list item will move.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
    <Pages>

            <pdf:Page styles:margins="20pt" styles:font-size="12pt" styles:column-count="3">
            <Content>
                
                <pdf:Ul>
                    <pdf:Li >First Item</pdf:Li>
                    <pdf:Li >Second Item</pdf:Li>
                    <pdf:Li >Third Item</pdf:Li>
                    <pdf:Li >Fourth Item</pdf:Li>
                    <pdf:Li >Fifth Item</pdf:Li>
                </pdf:Ul>

                <pdf:ColumnBreak/>

                <pdf:Ol>
                    <pdf:Li >First Item</pdf:Li>
                    <pdf:Li >Second Item</pdf:Li>
                    <pdf:Li >Third Item</pdf:Li>
                    <pdf:Li >Fourth Item</pdf:Li>
                    <pdf:Li >
                        Complex Item
                        <pdf:Span styles:fill-color="red">
                        With inner content,
                        <pdf:Image src="../../Content/Images/Toroid24.png" styles:width="18pt" styles:position-mode="Inline" />
                        that flows across the column.
                        </pdf:Span>
                    </pdf:Li>
                </pdf:Ol>

                <pdf:ColumnBreak/>

                <pdf:List styles:number-style="LowercaseRoman">
                    <pdf:Li>First item roman.</pdf:Li>
                    <pdf:Li>Second item roman.</pdf:Li>
                    <pdf:Li>Third item roman.</pdf:Li>
                    <pdf:Li>Fourth item roman.</pdf:Li>
                    <pdf:Li>Fifth item roman.</pdf:Li>
                </pdf:List>
                
            </Content>
            </pdf:Page>

    
    </Pages>
    
    </pdf:Document>

.. image:: images/documentLists1.png




Definition Lists
----------------

Overlowing list items
---------------------

Nesting Lists
-------------

List styles and grouping
-----------------------------

Binding List items
------------------