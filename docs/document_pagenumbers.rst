================================
Page numbering
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


Page number style
=====================

There is always a global page numbering in all documents, and the default is decimal.

If we wanted our sections or pages to use a different numbering style, then we can specify it in the styles
or on the element itself.

Scryber supports the following numbering styles.

* Decimal - 1, 2, 3, 4, 5, etc.
* UppercaseRoman - I, II, III, IV, V, etc.
* LowercaseRoman - i, ii, iii, iv, v, etc.
* UppercaseLetters - A, B, C, D, E, etc.
* LowercaseLetters - a, b, c, d, e, etc.
* And None.


Page number index
==================

Along with the number styles, scryber also supports a start-index on a document or page.

Setting the start-index (page-number-start-index) will begin a new numbering group at that page, and increment from there within the group.
When the layout leaves the section or page group, the numbering will revert back to the previous values.

Page display format
====================

The display-format enables full control of the text of the page number output.

There are 4 values that can be set in a standard dotnet format string.

* {0} - The current page index within the group
* {1} - The total page count within the group
* {2} - The current global page index within the document.
* {3} - The global page count within the document.

The number styles will be automatically used with any format string.

.. code-block:: xml

    <?xml version='1.0' encoding='utf-8' ?>

    <pdf:Document xmlns:pdf='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                xmlns:styles='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                xmlns:data='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' >
    <Render-Options compression-type='None' string-output='Text' />
    <Styles>
        <styles:Style applied-class='pg-num' >
        <styles:Padding all='20pt'/>
        <styles:Font size='60pt' family='Helvetica'/>
        <styles:Page display-format='Page {0} of {1}'/>
        </styles:Style>

        <styles:Style applied-class='intro' >
        <styles:Page number-style='LowercaseRoman'/>
        </styles:Style>

        <styles:Style applied-class='appendix' >
        <styles:Page display-format='Appendix {0}'  number-style='UppercaseLetters' number-start-index='1' />
        </styles:Style>
    </Styles>
    
    <Pages>

        <pdf:Section styles:class='pg-num intro'>
        <Content>
            <pdf:Div>Introductions with lowercase roman</pdf:Div>
            <!-- Page 1 -->
            <pdf:PageNumber />
            <pdf:PageBreak/>
            <!-- Page 2 -->
            <pdf:PageNumber />
            <pdf:PageBreak />
            <!-- Page 3 -->
            <pdf:PageNumber />
        </Content>
        </pdf:Section>

        <pdf:Section styles:class='pg-num' styles:page-number-start-index='1' >
        <Content>
            <pdf:Div>These are the page numbers shown on each of the pages</pdf:Div>
            <!-- Page 1 -->
            <pdf:PageNumber />
            <pdf:PageBreak/>
            <!-- Page 2 -->
            <pdf:PageNumber />
            <pdf:PageBreak />
            <!-- Page 3 -->
            <pdf:Div>With a different format</pdf:Div>
            <pdf:PageNumber id='ExplicitPageNum' styles:display-format='Page {0} of {1} (Total {2} of {3})' />
        </Content>
        </pdf:Section>

        <pdf:Section styles:class='pg-num appendix'>
        <Content>
            <pdf:Div>The appendix style has upper case letters with a formatted value to show the current appendix letter.</pdf:Div>
            <!-- Page 4 -->
            <pdf:PageNumber />
            <pdf:PageBreak />
            <!-- Page 5 -->
            <pdf:PageNumber />
        </Content>
        </pdf:Section>

    </Pages>
    
    </pdf:Document>


.. image:: images/documentpagenumbers2.png


Page of a different component
==============================

