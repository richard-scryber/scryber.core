================================
Page numbering - td
================================

A documents pages starts by default at layout page 1 and increments by 1 for each subsequent layout page to the end of the document.
We can show the current page number by adding the :doc:`reference/pdf_pagenumber` on the page.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Styles>
        <styles:Style applied-class="pg-num1" >
            <styles:Position h-align="Center"/>
            <styles:Padding all="20pt"/>
            <styles:Font size="100pt"/>
        </styles:Style>

    </Styles>
    
    <Pages>
        <pdf:Page styles:class="pg-num1">
        <Content>
            <!-- Page 1 -->
            <pdf:PageNumber />
        </Content>
        </pdf:Page>

        <pdf:Section styles:class="pg-num1">
        <Content>
            <!-- Page 2 -->
            <pdf:PageNumber />
            <pdf:PageBreak/>
            <!-- Page 3 -->
            <pdf:PageNumber />
            <pdf:PageBreak />
            <!-- Page 4 -->
            <pdf:PageNumber />
        </Content>
        </pdf:Section>

        <pdf:Section styles:class="pg-num1">
        <Content>
            <!-- Page 5 -->
            <pdf:PageNumber />
            <pdf:PageBreak />
            <!-- Page 6 -->
            <pdf:PageNumber />
        </Content>
        </pdf:Section>
    </Pages>
    
    </pdf:Document>

.. image:: images/documentpagenumbers1.png


Page Number format
===================

If we wanted our sections or pages to use a different numbering format, then we can specify it in the styles
or on the element itself.

Scryber supports