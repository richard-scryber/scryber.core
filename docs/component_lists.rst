=======================================
Unordered, Ordered and Definition Lists
=======================================

Scryber supports the use of lists both ordered and unordered and allows nesting, overflow, and definition lists.
It also supports the use of binding and repeating.

Unordered and ordered lists
===========================

Scryber uses the pdf:List as the generic component with a default bullet style, but same 
tags as Html for the Ordered Lists (pdf:Ol) and Unordered Lists (pdf:Ul) as short hand for these variants.

The contents of a list item (pdf:Li) can be any form of content (inline or otherwise).
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
================

Definition lists are slightly different as they use the pdf:Dl and pdf:Di components, with the item-label style value rather than a bullet or number.
They also have a default inset of 100pt, rather than 30pt to fit the label content. 

This can be changed using the number inset, and number alignment.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
    <Pages>

        <pdf:Page styles:margins="20pt" styles:font-size="12pt" >
            <Content>
                
                <pdf:Dl styles:margins="0 0 20 0">
                    <pdf:Di styles:item-label="First" >First Item</pdf:Di>
                    <pdf:Di styles:item-label="Second" >Second Item</pdf:Di>
                    <pdf:Di styles:item-label="Third" >Third Item</pdf:Di>
                    <pdf:Di styles:item-label="Fourth" >Fourth Item</pdf:Di>
                    <pdf:Di styles:item-label="Fifth" >Fifth Item</pdf:Di>
                </pdf:Dl>


                <pdf:Dl styles:number-inset="150pt" styles:number-alignment="Left">
                    <pdf:Di styles:item-label="Long First" >First Item</pdf:Di>
                    <pdf:Di styles:item-label="Long Second" >Second Item</pdf:Di>
                    <pdf:Di styles:item-label="Long Third" >Third Item</pdf:Di>
                    <pdf:Di styles:item-label="Long Fourth" >Fourth Item</pdf:Di>
                    <pdf:Di styles:item-label="Very Long Fifth that will force a new line" >
                        Fifth Item
                        <pdf:Span styles:fill-color="red">
                        With inner content,
                        <pdf:Image src="../../Content/Images/Toroid24.png" styles:width="18pt" styles:position-mode="Inline" />
                        that flows across the page and onto a new line.
                        </pdf:Span>
                    </pdf:Di>
                </pdf:Dl>
                
            </Content>
        </pdf:Page>

    
    </Pages>
    
    </pdf:Document>


.. image:: images/documentListDefinitions.png



Overflowing list items
======================

As with table rows (see :doc:`component_tables`) the list items are not designed to be split across columns or pages.
They will attempt to keep together and bring any numbers, bullets or defitions with them.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
    <Pages>

        <pdf:Page styles:margins="20pt" styles:font-size="12pt" >
            <Content>

                <pdf:Div styles:column-count="3" styles:height="150" styles:border-color="blue">
                <pdf:Ul>
                    <pdf:Li >1st Item</pdf:Li>
                    <pdf:Li >2nd Item</pdf:Li>
                    <pdf:Li >3rd Item</pdf:Li>
                    <pdf:Li >4th Item</pdf:Li>
                    <pdf:Li >5th Item</pdf:Li>
                    <pdf:Li >6th Item</pdf:Li>
                    <pdf:Li >7th Item</pdf:Li>
                    <pdf:Li >8th Item</pdf:Li>
                    <pdf:Li >9th Item</pdf:Li>
                    <pdf:Li >10th Item</pdf:Li>
                    <pdf:Li >11th Item</pdf:Li>
                    <pdf:Li >12th Item</pdf:Li>
                    <pdf:Li >13th Item</pdf:Li>
                    <pdf:Li >14th Item</pdf:Li>
                </pdf:Ul>
                
                <pdf:Br/>
                
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
                
                </pdf:Div>

                
            </Content>
        </pdf:Page>

    
    </Pages>
    
    </pdf:Document>


.. image:: images/documentListOverflow.png


List styles and grouping
========================


Horizontal lists
================


Nesting Lists
=============

Binding List items
==================