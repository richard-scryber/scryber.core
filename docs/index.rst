=============
Scryber Core
=============

Scryber.Core is **the** engine to create dynamic documents quickly and easily with consistant styles and easy flowing layout.
It's open source; flexible; styles based; data driven and with a low learning curve. 

A document generation tool written entirely in C# for dotnet core.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>
    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
        <Params>
            <pdf:String-Param id="Title" value="Document Title" />
        </Params>
        
        <Data>
            <data:JsonDataSource id="XmlSource" source-path="http://localhost:5000/Home/Json" ></data:JsonDataSource>
        </Data>
        
        <Styles>
            <styles:Style applied-type="pdf:H1" applied-class="title" >
                <styles:Background color="#336666"/>
                <styles:Fill color="#FFFFFF"/>
                <styles:Font family="Gill Sans" size="24pt" italic="true"/>
            </styles:Style>
        </Styles>
        
        <Pages>

            <pdf:Page styles:margins="20pt">
                <Content>
                    <data:With datasource-id="JsonSource"  >

                    <pdf:H1 styles:class="title" text="{@:Title}" > </pdf:H1>
                    
                    <pdf:Ul>
                        <data:ForEach value="{@:.Entries/Entry}" >
                        <Template>
                            <pdf:Li>
                            <pdf:Text value="{@:.Name}" />
                            </pdf:Li>
                        </Template>
                        </data:ForEach>
                    </pdf:Ul>
                    </data:With>
                    
                </Content>
            </pdf:Page>
        </Pages>

    </pdf:Document>

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

Binding to your data
--------------------

With a simple binding notation it's easy to add references to your data structures and pass information
and complex data to your document, or get the document to look up and bind the data for you.

Learn More
----------

Take a look at the quick start guides on `Getting started with MVC <mvc_controller_full>`_ or
`Getting started with GUI applications <gui_controller_full>`_ to learn more.


.. toctree::
    :caption: Getting Started
    :maxdepth: 1

    mvc_controller_full
    gui_controller_full

.. toctree::
    :caption: Layout Content.
    :maxdepth: 1

    code_vs_xml
    document_structure
    document_components
    document_styles
    referencing_files
    component_positioning
    component_sizing
    document_columns
    drawing_units
    drawing_colors
    drawing_images
    drawing_fonts
    drawing_paths
    document_outline
    
.. toctree::
    :caption: Binding to data
    :maxdepth: 1

    document_model
    document_parameters
    document_databinding
    document_controllers

.. toctree::
    :caption: Extending Scryber
    :maxdepth: 1

    document_lifecycle
    dynamic_loading
    namespaces_and_assemblies
    scryber_configuration
    extending_scryber

.. toctree::
    :caption: Reference
    :maxdepth: 1

    libgdiplus
    version_history
    reference/index


