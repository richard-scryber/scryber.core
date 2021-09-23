=======================
Styles in your template
=======================

In scryber styles are used through out to build the document. Every component has a base style and styles (such as fill colour and font) that flow down
to their inner contents.

Styles on elements
-----------------------

.. code-block:: xml

    <div style='margin:20pt;padding:4pt; background-color:#FF0000; color:#FFFFFF; font-family: Arial, sans-serif; font-size:20pt' >
        <span>Hello World, from scryber.</span>
    </div>

Or if you are dynamically generating some content in the code

.. code-block:: csharp

    private static Component StyledComponent()
    {
        var div = new Div()
        {
            BackgroundColor = new Scryber.Drawing.PDFColor(Drawing.ColorSpace.RGB, 255, 0, 0),
            Margins = new Drawing.PDFThickness(20),
            Padding = new Drawing.PDFThickness(4),
            FontFamily = "Arial",
            FontSize = 20,
            FillColor = Scryber.Drawing.PDFColors.White
        };

        div.Contents.Add(new Label()
        {
            Text = "Hello World from scryber"
        });

        return div;

    }

Style Classes
---------------

Along with appling styles directly to the components, Scryber supports the use of styles declaratively and applied to the content dynamically.

.. code-block:: xml

    <html xmlns='http://www.w3.org/1999/xhtml'>
        <head>
            <style>
                div {
                    font: Arial 20pt;
                }
                .mystyle {
                    background-color:#FF0000;
                    color:white;
                    padding:20pt;
                    margin:20pt;
                }
            </style>
        </head>
        <body>
            <div class="mystyle">
                <span>Hello World, from scryber.</span>
            </div>
     
        </body>
    </html>

By using styles, it cleans the code and makes it easier to standardise and change later on.
This can either be within the document itself, or in a separate link files (see: :doc:`referencing_files`)


Block Styles
-------------

Components such as div's, paragraphs, headings, tables, lists and list items are by default blocks. This means they will begin on a new line.
Components such as spans, labels, dates and numbers are inline components. This means they will continue with the flow of content in the current line.

There are certain style attributes that will only be used on block level components. These are:

* Background Styles
* Border Styles
* Margins
* Padding
* Vertical and Horizontal alignment.

Scryber does not (currently) support inline-blocks with their associated styles, but it is in the backlog.

Applying Styles
----------------

Just as in css and html, styles can be applied to an element based upon (multiple) combination(s) of 3 attributes of the Style.

id
class
type

e.g.

.. code-block:: css

    <style>

    /* This style will be applied at the document level specifying
    the base level font, size and color for text. Because These
    cascade down, then it will be inherited by components in the document. */

   html {
       font-family: "Gill Sans", sans-serif;
       font-size: 14pt;
       color: #333;
   }

   /* This style will be applied to the body tag for the first (set of) pages. */

   body {
       margin: 10px;
   }

   /* This style will be applied to all top level headings
    specifying the font size and some spacing */

   h1 {
       font-weight: bold;
       font-size: 30pt;
       margin-top: 20pt;
       padding: 5pt;
   }

   /* This style will be applied to all top level headings with a class of 'warning'
    and give a background colour of red on white text.  */

   .warning {
       background-color: #FF0000;
       color: #FFFFFF;
   }

   /* This style will be applied to all components with a class of 'border'
    and give a background colour of red with white text */

   .border {
       border-color: #777;
       border-width: 1pt;
       border-style: Solid;
       color: #444;
   }

   /* This style will be applied to all H1 Headings with a class of 'border'
    and give a border colour of red with white text. It has a higher precedence than either h1 or .border */

   h1.border {
       border-color: #550000;
       color: white;
   }

   /* This style will only be applied to a component with ID 'FirstHead'
    and give a font size of 48pt */

   #FirstHead {
       font-size: 36pt;
       font-weight: 400;
   }

    </style>


.. note:: Currently scryber does not support the concept of pseudo-classes such as :hover or :first as css e.g. div.class:first. Nor does it support !important. It may be supported in the future.

The same styles can also be applied in the code of the document styles


Applying Multiple Styles
-------------------------

Every component supports the 'class' attribute. And the value of this can be one or more class names.

.. code-block:: html

    <h1 id="FirstHead" class="warning border" style="font-italic:true" >Hello World, from scryber</h1>


This will apply the h1 style, the 2 classes for the warning and border, and the h1.border applied in that precedence order and increase the size based on the ID of FirstHead.
And then the inline italic style will be applied.

.. image:: images/helloworldpage_styled.png


Late adding of styles
-----------------------

Even once you have parsed or built a document, the styles can still be modified or added to.
Either on a component, or at a document level, as they are evaluated, allowing runtime alteration of the output.

.. code-block:: csharp

    //change the style sheet based on a flag check
    var sheet = checkflag ? "Sheet1.css" : "Sheet2.css"

    using(var doc = PDFDocument.ParseDocument("MyPath.html") as HTMLDocument)
    {
        //Load the stylesheet as a referenced component
        var link = new HtmlLink(){ Href = sheet };

        //and add it to the document styles.
        doc.Head.Contents.Add(link);

        //or explicitly define a style on the document
        var defn = new StyleDefn("h1.border");
        defn.Background.Color = (PDFColor)"#FFA";
        defn.Border.Width = 2;
        defn.Border.Color = PDFColors.Red;
        defn.Border.LineStyle = LineType.Solid;

        doc.Styles.Add(defn);
    }

Data binding Styles
--------------------

The process of data-binding (see: :doc:`document_lifecycle`, and :doc:`document_databinding`) can 
apply values to styles and classes on tags.

e.g.

.. code-block:: html

    <style>

    html {
        font-family: "Gill Sans", sans-serif;
        font-size: 14pt;
        color: #333;
    }

    body {
        margin: 10px;
    }

    /* this style will be applied as the bound class in the model */

    .border {
        border-color: #777;
        border-width: 1pt;
        border-style: Solid;
        color: #444;
    }

    </style>
    <body>
        <!-- apply a theme.headclass and explicit styles -->
        <div class='{@:model.theme.headclass}' style="{@:model.theme.bg}" >

            <!-- dynamic styles for the title and number -->
            <span style="{@:model.theme.title}" >This is the title</span><br/>
            <span style="{@:model.theme.number}" >1</span>
        </div>

    </body>

Here the theme div and spans will pick up the default theme values.
Were the code can provide new style colours and fonts for output.

.. code-block:: csharp

    var doc = PDFDocument.ParseDocument(path);
    doc.Params["model"] = new {
       theme = new {
           headclass="border",
           bg = "background-color:#FFA;padding:20pt;border:solid 1px red;",
           title = "font-family:\"Times New Roman\", Times, serif;",
           number = "font-style: italic"
        }
    };

    return this.PDF(doc);

    
.. image:: images/helloworldpage_stylebound.png

Order and Precedence
---------------------

Scryber tries to apply a priority, just as html to styles as they are loaded.
This is based on order, depth and explicit.

div.class has a higher priority than .class 

Explicit will be highest priority

<div style='color:white' > 

And it will always fall back to the default (e.g. blue underline for anchor links).

.. note:: Scryber does not support !important overrides, nor does it support the use of :first-child, :hover or other pseudo classes.


Scryber has the same precedence order as html - based on the order in the document.

1. The inherited style from the parent is collected.
2. Any styles in the document are evaluated in the order they appear.
    1. What is the precedence of the matcher. Tag < Class < ID.
    2. What is the complexity of the match. Tag+Class < Tag+ID < Tag+Class+ID
    3. And parent selectors are evaluated to precedence Child < Parent(s) + Child 
3. If a stylesheet reference is encountered, then the styles within it will be evaluated before moving on to the following styles
4. Finally the styles directly applied will be evaluated, giving the full style result.

This will then be flattened as a complete style and used in the layout and rendering of the component.


Supported CSS 
---------------

The following CSS standard tags are supported...

* border
    * border-width
    * border-style
    * border-color
    * border-top
        * border-top-width
        * border-top-color
        * border-top-style
    * border-left
        * border-left-width
        * border-left-color
        * border-left-style
    * border-right
        * border-right-width
        * border-right-color
        * border-right-style
    * border-bottom
        * border-bottom-width
        * border-bottom-color
        * border-bottom-style
* color
* background
    * background-image
    * background-color
    * background-repeat
    * background-size
    * background-position
* font
    * font-style
    * font-weight - Translated to regular and bold (for the moment)
    * font-size
    * font-family
    * line-height

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
* fill-opacity

* column-count
* column-gap
* column-span (for table cells)

* page-break-inside
* page-break-after
* page-break-before
    

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

* display
    * inline
    * block
    * none

* overflow
    * visible, auto
    * hidden

* position
    * relative
    * absolute
    * static

* text-decoration
* letter-spacing
* word-spacing

* white-space
* list-style-type (and list-style which is treated as equivalent)
    * bullet, disc
    * decimal
    * lower-roman
    * lower-alpha
    * upper-roman
    * upper-alpha
    * none

* stroke
    * stroke-opacity
    * stroke-width

* size
    * A4, A3, Letter, etc.
    * portrait or landscape

* page
    * explicit name (of an @page style)

at-rules supported
-------------------

The following at-rules are supported

* @media - including or excluding css based on print.
* @font-face - using explicit font files and names.
* @page - specifying page sizes for sections and breaks.


