==============================
Supported Html Tags and Styles
==============================

Scryber supports the majority of tags and styles within html.
The following tags will be recognised and style applied appropriately.

* body
* div
* span
* table
* tr
* td
* th
* ul
* ol
* li
* h1 to h6
* blockquote
* b
* i
* u
* em
* strong
* p
* code
* pre
* img
* br
* dl
* dt
* fieldset
* legend
* font
* a
* hr

All the factories are referenced in the public DefaultTags dictionary within the Scryber.Html.Parsing.HtmlParserComponentFactory class. This can be added to as needed. 
Scryber actually defines a component for each of the tags that are parsed (See :doc:`namespaces_and_assemblies` for more)
Defining another component is simple based on the IParserComponentFactoryInterface
See the extending scryber section for more information

Unsupported tags
================

The HtmlFragment parser will ignore tags that are not known and parse the inner content 
of the tag as if it was not there.

Supported styles
================

Within the style attribute of each tag, the following css parts are supported.

* border
* border-color
* border-style
* border-width
* color
* background
* background-color
* background-image
* background-repeat
* font (single font family only)
* font-family (again single font family only)
* font-weight (bold only)
* font-size
* line-height (leading)
* margin
* margin-left
* margin-right
* margin-top
* margin-bottom
* padding
* padding-left
* padding-right
* padding-top
* padding-bottom
* opacity
* column-count
* column-gap
* column-span
* left
* top
* width
* height
* min-width
* min-height
* max-width
* max-height
* text-align
* vertical-align
* text-decoration
* letter-spacing
* word-spacing
* white-space
* display
* overflow
* list-style-type (most values)
* list-style
* page-break-inside

All sizes in pixels are converted to points at 72 ppi. i.e. A direct conversion of 1 pixel = 1 point.

Applying styles and classes
=======================

Along with the style attribute, scryber supports the class attribute. 
This will be applied to the relevant component so it can use the scryber style classes.

Taking the content in the previous dynamic example (:doc:`html_dynamic`), classes and styles can be applied and used.

.. code-block:: html

    @model Scryber.Core.Samples.Web.Models.DataContentList

    <p class="strong" style="font-size:20px">This page is sample content from the view controller with title @ViewBag.Name.</p>
    <table>
        @for(var i = 0; i < Model.Count; i++)
        {
            <tr class="@((i % 2) == 0 ? "row even" : "row odd")">
                <td>@Model[i].ID</td>
                <td>@Model[i].Name</td>
                <td style="font-weight:bold; color:#323232; background-color:#00a8a1; text-align:right; width:150px">&#163;@Model[i].Price.ToString("#,##0.00")</td>
            </tr>
        }
    </table>

Updated styles in a template
==============================

These styles and classes can then be used in the document template (or a referenced stylesheet) to update the design

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
    <Params>
        <doc:Xml-Param id="title" >
        <h1 class="title">Styled Html Title</h1>
        </doc:Xml-Param>
    </Params>

    <Styles>
        
        <!-- Style for the heading title-->
        <styles:Style applied-class="title" >
            <styles:Background color="#323232"/>
            <styles:Font family="Franklin Gothic Medium" bold="false"/>
            <styles:Fill color="#00a8a1"/>
            <styles:Padding all="20pt"/>
        </styles:Style>

        <!-- Styles for the even and odd rows -->
        <styles:Style applied-class="even" >
            <styles:Background color="#AAA"/>
        </styles:Style>
        
        <styles:Style applied-class="odd" >
            <styles:Background color="#CCC"/>
            <styles:Fill color="#323232"/>
        </styles:Style>

        <!-- Style for the paragraph  -->
        <styles:Style applied-class="strong" >
            <styles:Padding left="5pt"/>
            <styles:Margins bottom="10pt"/>
            <styles:Font italic="true"/>
            <styles:Border color="#00a8a1" width="2pt" sides="Left"/>
        </styles:Style>
    </Styles>

    <Pages>
        
        <doc:Section styles:font-size="12pt">
        <Content>
            
            <!-- Fragment bound to the xml 'title' parameter -->
            <doc:HtmlFragment source="" contents="{@:title}" />
            
            <!-- Fragment that comes from an MVC Controller method with a name parameter
                split over 2 columns with the explicit height -->
            <doc:Div styles:column-count="2" styles:padding="20pt" styles:height="200pt">
                <doc:HtmlFragment source="http://localhost:5000/Home/html?name=my%20styled%20content" />
            </doc:Div>
            
        </Content>
        </doc:Section>

    </Pages>
    
    </doc:Document>

Rendered styled content
=======================
With the resulting output matching the required layout

.. image:: images/documentHtmlStyled.png


Html Components
===============

When html is parsed, the tags are converted to fully qualified scryber components in the `Scryber.Html.Components` namespace
of the Scryber.Components library.

As such it is possible to register the html prefix and associate with these components, the same as the `doc:` prefix.
Global styles for td, tr, field, label etc can then be made.

e.g.

.. code-block:: 
    
    <!-- xmlns:html="http://www.w3.org/1999/xhtml" -->

    <styles:Style applied-type="html:td" applied-class="strong" >
        ..
    </styles:Style>

See :doc:`namespaces_and_assemblies` for more information.

