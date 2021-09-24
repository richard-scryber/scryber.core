====================================
Namespaces and their Assemblies
====================================

Scryber relies on the xml namespaces (xmlns) to identify the classes it should use when parsing an XML or XHTML file.
This is based on a mapping of xmlns value to a fully qualified assembly name and namespace.

Declared Namespaces
--------------------

The 3 base namespaces that are automatically added are:

* http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd
    * The base level components used in scryber. e.g. Document, TableGrid, List, Span etc.
    * It refers to the Scryber.Components namespace in the Scryber.Components assembly (Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe)
* http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd
    * The data components used in scryber. e.g. ForEach, Choose, If.
    * It refers to the Scryber.Data namespace in the Scryber.Components assembly (Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe)
* http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd
    * The style components and attributes used in scryber. e.g. StyleDefn, StyleGroup, StylesDocument.
    * It refers to the Scryber.Styles namespace in the Scryber.Styles assembly (Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe)

The html and svg namespaces are also automatically added.

* http://www.w3.org/1999/xhtml
    * The html components used in scryber. e.g. div, span, section etc.
    * It refers to the Scryber.Html.Components namespace in the Scryber.Components assembly (Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe)
* http://www.w3.org/2000/svg
    * The svg drawing components used in scryber. e.g. ellipse, circle, rect etc.
    * It refers to the Scryber.Svg.Components namespace in the Scryber.Components assembly (Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe)


.. note:: If a file or stream of content does not have a namespace, then the classes cannot be found and therefore parsed.

Class attributes
-----------------

Within the assembly namespaces, referred to above, are actual classes decorated with attributes for each of their properties and contents.

.. code-block:: csharp

    namespace Scryber.Html.Components
    {
        [PDFParsableComponent("body")]
        public class HTMLBody : Scryber.Components.Section
        {

            [PDFAttribute("class")]
            public override string StyleClass 
            { 
                get => base.StyleClass; 
                set => base.StyleClass = value; 
            }

            [PDFAttribute("style")]
            public override Style Style 
            { 
                get => base.Style; 
                set => base.Style = value; 
            }

            [PDFElement("")]
            [PDFArray(typeof(Component))]
            public override ComponentList Contents
            {
                get { return base.Contents; }
            }

            [PDFElement("header")]
            [PDFTemplate(IsBlock= true)]
            public override IPDFTemplate Header 
            { 
                get => base.Header; 
                set => base.Header = value; 
            }

            [PDFElement("footer")]
            [PDFTemplate(IsBlock = true)]
            public override IPDFTemplate Footer 
            {
                get => base.Footer; 
                set => base.Footer = value;
            }

            [PDFAttribute("hidden")]
            public string Hidden
            {
                get { return (this.Visible) ? string.Empty : "hidden" }
                set { this.Visible = (string.IsNullOrEmpty(value) || value != "hidden") ? true : false; }
            }

            [PDFAttribute("title")]
            public override string OutlineTitle
            {
                get => base.OutlineTitle;
                set => base.OutlineTitle = value;
            }

            public HTMLBody()
                : base()
            {
            }
        }
    }

Here we can see the html body class decorated with the PDFParsableComponent attribute, so the parser know when it gets to a <body> tag in the content stream in namespace Scryber.Html.Components it
create an instance of the HTMLBody class.

The class inherits from the Scryber.Components.Section (an overflowing page), and overrides some of the base functionality to support the standard html attributes. For example
the [PDFAttribute("class")] maps the @class attribute in the content stream to the StyleClass string property in the instance.

The explicitly named [PDFElement("head")] if found will be assigned to the Header property, in this case as a template so it can be used multiple times (See :doc:`binding_model`)

Finally the empty PDFElement attribute with the PDFArray attribute tells the parser it should expect inner child components (that are not nested within another element) of type Component, and they should be added to this collection.

.. note:: Scryber has an explicit parser, rather than implicit. So if classes or properties are not decorated, then they will not be used.

Parsing the content
----------------------

Considerring the below content we can see the namespace mapping to the classes 

.. code-block:: html

    <html xmlns='http://www.w3.org/1999/xhtml'>
        <body style='padding:20pt;' title='Top Level' >
            <head><p>This is the header</p><head>
            <p class='main'>This is the content</p>
        </body>
    </html>


When parsed this will give us an object graph of the below. The content in the header is kept as a string and will be parsed when used each time.

.. image:: images/parsedObjectContents.png


See :doc:`extending_scryber` to understand how to add your own classes and namespaces.
