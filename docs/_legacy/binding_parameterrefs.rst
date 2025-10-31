==============================================
Passing parameters to references - td
==============================================


Using `document styles <document_styles>`_ and `document parameters <document_model>`_ it is possible to modify the content of the document when it is bound.

To start with we can alter the styles that we have loaded from the style sheet.

.. code-block:: xml

     <Styles>
        <!-- Original Style sheet reference -->
        <styles:Styles-Ref source="./Styles/Stylesheet.psfx"/>

        <!-- Modification to the styles -->

        <styles:Style applied-class="title" >
            <styles:Font bold="true" size="40"/>
            <styles:Position h-align="Right"/>
        </styles:Style>


        <styles:Style applied-class="page-head" >
            <styles:Border color="red" width="2pt"/>
            <styles:Font size="10pt"/>
        </styles:Style>
        
  </Styles>

These will be applied to the pages and components whenever they are referenced. 
Retaining the original properties where they are unchanged.

.. image:: images/referencefilesoutput2.png


And then we can add parameters to our `DocumentRefs.pdfx` that we can use in our components and sub pages.

.. code-block:: xml

    <Params>
        <doc:String-Param id="doc-title" value="Referenced Files" />
    </Params>

And reference that in our component `StdHeader.pcfx` with the standard binding notation on the text attribute **`{@:doc-title}`**

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <doc:Div xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
            xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
            styles:class="page-head" styles:column-count="2" >

        <doc:Label styles:class="head-text" text="{@:doc-title}" />
        <doc:ColumnBreak/>
        <doc:Date styles:class="head-text" styles:date-format="dd MMM yyyy" />
    </doc:Div>

If we render this now, then the header should always use the `doc-title` value.
If it is not provided, then it will simply be blank.

.. image:: images/referencefilesoutput3.png

Finally we can put parameters explicitly in the template. These will only apply within the template and nowhere else.
So we can provide a new value for the `doc-title` for our referenced page and that will be used on the header component,
but it will revert back to the default value for our second actual page.

.. code-block:: xml

     <doc:Page-Ref source="Pages/HeaderPage.ppfx">
      <Params>
        <doc:String-Param id="doc-title" value="Different Section" />
      </Params>
    </doc:Page-Ref>

Rendering this will change the title for the header in the referenced component.

.. image:: images/referencefilesoutput4.png

.. note:: You are not limited to strings in parameters, you can provide colours, data, xml and actual scryber components into the parameters.


Our full code for the `DocumentRefs.pdfx` file is

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd"
                auto-bind="true" >
    <Styles>
        <styles:Styles-Ref source="./Styles/Stylesheet.psfx"/>

        <styles:Style applied-class="title" >
        <styles:Font bold="true" size="40"/>
        <styles:Position h-align="Right"/>
        </styles:Style>


        <styles:Style applied-class="page-head" >
        <styles:Border color="red" width="2pt"/>
        <styles:Font size="10pt"/>
        </styles:Style>
        
    </Styles>

    <Params>
        <doc:String-Param id="doc-title" value="Referenced Files" />
    </Params>
    
    <Pages>
        <doc:Page-Ref source="Pages/HeaderPage.ppfx">
        <Params>
            <doc:String-Param id="doc-title" value="Different Section" />
        </Params>
        </doc:Page-Ref>

        <doc:Page>
        <Header>
            <doc:Component-Ref source="Components/StdHeader.pcfx"></doc:Component-Ref>
        </Header>
        <Content>
            <doc:H1 styles:class="title" >This is the second Page </doc:H1>
        </Content>
        </doc:Page>
    </Pages>
    </doc:Document>

