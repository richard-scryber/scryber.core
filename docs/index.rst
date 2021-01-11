=============
Scryber
=============

Scryber is **the** engine to create dynamic documents quickly and easily with consistant styles and easy flowing layout.
It's open source; flexible; styles based; data driven and with a low learning curve. 

A document generation tool written entirely in C# for dotnet 5 using XHTML, XML or even MVC Views.


.. code-block:: html




.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
        <Params>
            <doc:String-Param id="Title" value="Document Title" />
        </Params>
        
        <Data>
            <data:XmlDataSource id="XmlSource" source-path="http://localhost:5000/Home/Xml" ></data:XmlDataSource>
        </Data>
        
        <Styles>
            <styles:Style applied-type="doc:H1" applied-class="title" >
                <styles:Background color="#323232" 
                                img-source="images/logo.png" repeat="None" x-pos="10pt" y-pos="10pt" x-size="35pt" />
                <styles:Fill color="#00a8a1"/>
                <styles:Font family="Gill Sans" size="24pt" italic="true"/>
            </styles:Style>
        </Styles>
        
        <Pages>

            <doc:Page styles:margins="20pt">
                <Content>
                    <data:With datasource-id="XmlSource"  >

                        <doc:H1 styles:class="title" text="{@:Title}" > </doc:H1>
                        
                        <doc:Ul>
                            <data:ForEach value="{xpath:Entries/Entry}" >
                            <Template>
                                <doc:Li>
                                <doc:Text value="{xpath:Name}" />
                                </doc:Li>
                            </Template>
                            </data:ForEach>
                        </doc:Ul>
                    </data:With>
                    
                </Content>
            </doc:Page>
        </Pages>

    </doc:Document>


.. code-block:: csharp


    public IActionResult HelloWorld()
    {
        var path = _env.ContentRootPath;
        path = System.IO.Path.Combine(path, "Views", "PDF", "HelloWorld.pdfx");

        return this.PDF(path, );
    }

Easy, and intuitive structure
-----------------------------

Whether you are using xml templates or directly in code, scryber
is quick and easy to build complex documents from your designs and data.


Intelligent flowing layout engine
---------------------------------

In scryber, content can either be laid out explicitly, or jut flowing with the the page.
Change the page size, or insert content and everything will adjust around it.

Cascading Styles 
----------------

With a styles based structure, it's easy to apply designs to templates. Use class names, id's or component types,
or a combination of all 3 to apply style information to your documents.

Low code, zero code development
-------------------------------

Scryber is based around xml templates - just like XHTML. It can be transformed, it can be added to,
and it can be dynamic built. By design we minimise errors, reduce effort and allow reuse.

Html Content and Remote sources
-------------------------------

Scryber can use native html content and layout neatly and easily within pages.
It also supports the use of inline and class styles.


Binding to your data
--------------------

With a simple binding notation it's easy to add references to your data structures and pass information
and complex data to your document from SQL, JSON, Entity Model and more.
Or get the document to look up and bind the data for you.

Learn More
----------


.. toctree::
    :caption: Getting Started
    :maxdepth: 1

    mvc_controller_full
    gui_controller_full

.. toctree::
    :caption: Documents and Pages.
    :maxdepth: 1

    document_code_vs_xml
    document_structure
    document_lifecycle
    document_components
    document_styles
    document_pages
    document_columns
    document_references
    document_pagenumbers
    document_outline

.. toctree::
    :caption: Layout Content.
    :maxdepth: 1

    drawing_units
    drawing_colors
    drawing_images
    drawing_fonts
    drawing_paths
    component_positioning
    component_sizing
    component_alignment

.. toctree::
    :caption: Component types
    :maxdepth: 1

    component_tables
    component_lists
    component_textblocks
    component_textelements
    component_linking
    component_html
    component_placeholder

.. toctree::
    :caption: Html Content
    :maxdepth: 1

    html_simple
    html_dynamic
    html_tags
    html_markdown

.. toctree::
    :caption: Binding to data
    :maxdepth: 1

    binding_model
    binding_parameters
    binding_parameterrefs
    binding_dataxmlsources
    binding_dataobjectsources
    binding_grids
    binding_lists
    binding_loops
    binding_choices
    binding_gridsandlists
    binding_controllers

.. toctree::
    :caption: Extending Scryber
    :maxdepth: 1

    extending_configuration
    extending_dynamic_loading
    extending_classes
    extending_namespaces_and_assemblies
    extending_scryber

.. toctree::
    :caption: Reference
    :maxdepth: 1

    libgdiplus
    version_history
    reference/index


