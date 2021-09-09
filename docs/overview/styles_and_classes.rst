===============================
Styles, Classes and selectors
================================

Scryber supports full cascading styles on all visual components.
It also supports declaration of classes and styles within either the html head or as referenced stylesheets.

So from our previous example we can reference an external stylesheet *(empty at the moment)*, relative to the document, and also declare some styles inside the document head.

.. code:: html

    <!DOCTYPE HTML>
    <html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>{{concat('Hello ', model.user.FirstName)}}</title>
        <link rel="stylesheet" href="./css/orderStyles.css" />
        <style type="text/css" >
            :root{
                --theme_color:#232323;
                --theme_space:10pt;
                --theme_align: center;
            }

            table.orderlist{
                width:100%;
                font-size: 12pt;
            }

            table.orderlist thead{
                background-color: #333;
                color: white;
            }

            #terms{
                margin-top: 20pt;
                font-size: 12pt;
            }

            #payNow {
                border: 1px solid red;
                padding: 5px;
                background-color: #FFAAAA;
                color: #FF0000;
                font-weight: bold;
            }

        </style>
    </head>
    <body>
        <!-- our heading has explicit styles -->
        <div style='color: var(--theme_color, #000); padding: var(--theme_space); text-align: var(--theme_align)'>
            Hello {{model.user.FirstName}}.
        </div>
        <div style='padding: var(--theme_space);'>
            <!-- order list class styles are assigned -->
            <table id="orders" class="orderlist" >
                <thead>
                    <tr>
                        <td>#</td>
                        <td>Item</td>
                        <td>Description</td>
                        <td>Unit Price</td>
                        <td>Qty.</td>
                        <td>Total</td>
                    </tr>
                </thead>
                <tbody>
                    <!-- Binding on each of the items in the model.order -->
                    <template data-bind='{{model.order.Items}}'>
                        <tr>
                            <!-- The indexing of the loop + 1 -->
                            <td>{{index() + 1}}</td>
                            <td>{{.ItemNo}}</td>
                            <td>{{.ItemName}}</td>
                            <td>
                                <!-- we use a number tag to specify the data-format referring to the top model -->
                                <num value='{{.ItemPrice}}' data-format='{{model.order.CurrencyFormat}}' />
                            </td>
                            <td>{{.Quantity}}</td>
                            <td>
                                <num value='{{.ItemPrice * .Quantity}}' data-format='{{model.order.CurrencyFormat}}' />
                            </td>
                        </tr>
                    </template>
                </tbody>
            </table>
            <div id='terms'>
                <div id='paidAlready' hidden='{{if(model.order.PaymentTerms &lt; 0, "", "hidden")}}'>
                    <p>Thank you for pre-paying for these items. They will be shipped immediately</p>
                </div>
                <div id='payNow' hidden='{{if(model.order.PaymentTerms == 0, "", "hidden")}}'>
                    <p>Please pay for your items now, and  we can process your order once received.</p>
                </div>
                <div id='paySoon' hidden='{{if(model.order.PaymentTerms &gt; 0, "", "hidden")}}'>
                    <p>Your items will be shipped immediately, please ensure you pay our invoice within <b>{{model.order.PaymentTerms}} days</b></p>
                </div>
            </div>
        </div>
    </body>
    </html>

Now we can set up our theme and apply styles to the order list table and #payNow box.
In the same was as css, the font size of the #terms div is cascaded to the #payNow div, and the #payNow styles are applied over the top, overriding where appropriate. 


.. figure:: ../images/doc_initial_styles.png
    :target: ../_images/doc_initial_styles.png
    :alt: Initial Styles
    :class: with-shadow

`Full size version <../_images/doc_initial_styles.png>`_

The heading div uses the css variables with the ``var()`` syntax, and scryber also supports using a fallback value in this function.

.. code:: css

    color: var(--theme_color, #000);


Allowed style selectors.
-------------------------

Scyber does not support the full set of selectors or the !important modifier (at the moment). 
We only support the use of 

* Chained selectors on tags, classes and id. 
* The > direct descendant selector.
* The :root selector
* The @font-face, @media and @page rules.

Other unsupported selectors and rules will be ignored.


.. code:: css

    /* these are supported */

    .classname { }
    #id { }
    tag { }

    tag.classname { }

    tag.classname .inner { }
    tag.classname tag.inner {  }

    tag.classname > .direct.descendant { }

    @media print {

        tag.mediaoverrides {

        }
    }

    /* these and other pseudo classes will not be supported 

    td:first {}
    td::last {}

    */

    /* Or these other rules

    @import {}

    @supports () {}


    */

Supported css properties
------------------------

For a complete list of all the supported style properties see :doc:`styles/document_styles`, but as an overview scyber currently supports.

* Fills - Colors, images, positions, repeats and gradients.
* Strokes - Widths, dashes, colors and joins.
* Backgrounds - Colors, images, positions, repeats and gradients.
* Borders - Width, dashes, colors and individual sides.
* Text - Fonts, alignment, spacing, wrapping
* Size - Explicit width, height, minimum and maximum widths and heights.
* Positions - Block, Inline, Relative to parents, Absolute to the current page, 100% width.
* Spacing - Margins, padding including individual sides
* Lists - style, groups, number formats and labels.
* Page - sizes, orientations, numbers and formats.
* Columns - count, widths, gutter/alleys.

.. note:: All dimensions in scryber are based on actual sizes, rather than relative sizes. We are hoping to implement relative sizes, but for the moment units should be in Points (pt), Millimeters (mm) and Inches (in).


Setting styles in code
-----------------------

Remember that all content parsed is converted to an object graph? This applies to styles as well.

All visual components (generally anything on a page) have a range of properties for setting styles, as well as a ``Style`` property. So we could apply some values to the style directly from our generation method.

We can even define our own styles in the document to override


.. code:: csharp

    //using Scryber.Components
    //using Scryber.Drawing
    //using Scryber.Styles

    var doc = Document.ParseDocument("MyFile.html");

    var service = new OrderMockService();
    var user = new User() { Salutation = "Mr", FirstName = "Richard", LastName = "Smith" };
    var order = service.GetOrder(1);
    order.PaymentTerms = 30;

    doc.Params["model"] = new {
                user =  user,
                order = order
    };

    var grid = doc.FindAComponentById("orders") as TableGrid;
    var pay = doc.FindAComponentById("payNow") as Div;
    
   
    //Properties directly on the visual component.
    grid.BackgroundColor = "#EEE";

    //Using the style property
    grid.Style.Margins.Right = 20;
    grid.Style.Margins.Left = 20;

    //Using style keys
    pay.Style.SetValue(StyleKeys.BorderStyleKey, LineType.Dash);
    pay.Style.SetValue(StyleKeys.BorderDashKey, PDFDashes.LongDash);

    //A new style to the document
    StyleDefn style = new StyleDefn("#terms div#payNow");
    style.Border.Width = 2;
    doc.Styles.Add(style);

    doc.SaveAsPDF("OutputPath.pdf");


.. figure:: ../images/doc_coded_styles.png
    :target: ../_images/doc_coded_styles.png
    :alt: Styles in code.
    :class: with-shadow

`Full size version <../_images/doc_coded_styles.png>`_

.. note:: We had to set the Border Style to dash, as well as providing a dash value, as our css styles had defined the border as solid. 

All the style properties are strongly typed, even the ``Style.SetValue`` as the style keys are strongly typed. However most of the values used have an explicit or implicit conversion from numbers or strings, or a simple constructor.
The main classes (and structs) used in styles are

* PDFUnit - a basic dimension with units. Implicit conversion from a number, along with parsing and constructors. See :doc:`drawing_units`
* PDFColor - a standard color in either RGB, CMYK or Gray scale. Implicit conversion from a string, along with parsing and constructors. See :doc:`drawing_colors`
* PDFThickness - 4 PDFUnits in a top, right, bottom and left order. Parsing and constructors. See :doc:`drawing_units`
* PDFFontSelector - A chained list of names of fonts, e.g "Arial" sans-serif. Explicit conversion along with parsing and constructor. See :doc:`drawing_fonts`
* Various Enumerations - Used for setting style types such as line caps, background styles, etc.


Base components styles
----------------------

Each component has a standard base style applied. For example the Div has a position mode of block. The paragraph also has a position mode of block, but also a top margin of 4 points. The table cell has a standard gray 1 point border.
By defining these there is a consistant appearance, but these can be easily overriden using css styles in your document or referenced css stylesheet.

.. code:: css

    td { border: none; }


Using calc() and binding dynamic values.
-----------------------------------------

Along with support for ``var()`` for looking up css variables, scryber supports ``calc()``.
This enables styles to be completely dynamic as well as the data.

The functions can either be on the css classes or wthin the style attribute itself.

In our linked orderStyles.css file we can set up some standard widths.

.. code:: css

    :root {
        --std-width: 30pt;
    }

    .td_w1 {
        width: var(--std-width);
    }

    .td_w2 {
        width: calc(var(--std-width) * 2.0);
    }

    .td_w3 {
        width: calc(var(--std-width) * 3.0);
    }

And in our code we can create a style parameter.

.. code:: csharp


    //using Scryber.Components
    //using Scryber.Drawing

    var doc = Document.ParseDocument("MyFile.html");

    var service = new OrderMockService();
    var user = new User() { Salutation = "Mr", FirstName = "Richard", LastName = "Smith" };
    var order = service.GetOrder(1);
    order.PaymentTerms = 30;

    doc.Params["model"] = new {
                user =  user,
                order = order
    };

    //new style document parameter
    doc.Params["style"] = new
    {
        rowColor = (PDFColor)"#EEE",
        altColor = (PDFColor)"#DDD",
        dateFormat ="dd MMMM yyyy",
        currencyFormat = "£##0.00"
    };

    doc.SaveAsPDF("OutputPath.pdf");

And finally we can update our template to use the new styles and add a bit more juice to the template.

.. code:: html

    <!DOCTYPE HTML>
    <html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>{{concat('Orders for ', model.user.FirstName)}}</title>
        <link rel="stylesheet" href="./css/orderStyles.css" />
        <style type="text/css">
            :root {
                --theme_color: #FF0000;
                --theme_space: 10pt;
                --theme_align: center;
                --theme_fsize: 12pt;
            }

            table.orderlist {
                width: 100%;
                font-size: var(--theme_fsize);
            }

            table.orderlist thead {
                background-color: #333;
                color: white;
            }


            #terms {
                margin-top: 20pt;
                font-size: var(--theme_fsize);
            }

            #payNow {
                border: 1px solid red;
                padding: 5px;
                background-color: #FFAAAA;
                color: #FF0000;
                font-weight: bold;
            }

        </style>
    </head>
    <body>
        <!-- setting the background color to the style -->
        <div style='background-color: calc(style.altColor); padding: var(--theme_space); text-align: var(--theme_align)'>
            {{count(model.order.Items)}} items for {{join(' ', model.user.Salutation, model.user.FirstName, model.user.LastName)}}
        </div>
        <!-- a preamble paragraph with concatenated values and a date -->
        <div style="padding: var(--theme_space);">
            <p style="font-size: var(--theme_fsize);" >
                Dear {{concat(model.user.Salutation, ' ', model.user.LastName)}},<br/>
                Thank you for your order on <time data-format="{{var(style.dateFormat, 'dd MMM yyyy')}}" /> for the following items:
            </p>
        </div>
        <div style='padding: var(--theme_space);'>
            <!-- classes on the header cells define the width of the table cells
                relative to the variable in the css stylesheet. -->
            <table id="orders" class="orderlist">
                <thead>
                    <tr>
                        <td class="td_w1">#</td>
                        <td class="td_w2">Item</td>
                        <td >Description</td>
                        <td class="td_w3">Unit Price</td>
                        <td class="td_w1">Qty</td>
                        <td class="td_w3">Total</td>
                    </tr>
                </thead>
                <tbody>
                    <!-- Changing the row color for alternates -->
                    <template data-bind='{{model.order.Items}}'>
                        <tr style="background-color: calc(if(index() % 2 == 1, style.altColor, style.rowColor));">
                            <!-- The indexing of the loop + 1 -->
                            <td>{{index() + 1}}</td>
                            <td>{{.ItemNo}}</td>
                            <td>{{.ItemName}}</td>
                            <td>
                                <!-- Data format is now coming from the style parameter -->
                                <num value='{{.ItemPrice}}' data-format='{{style.currencyFormat}}' />
                            </td>
                            <td>{{.Quantity}}</td>
                            <td>
                                <num value='{{.ItemPrice * .Quantity}}' data-format='{{style.currencyFormat}}' />
                            </td>
                        </tr>
                    </template>
                </tbody>
                <!-- Added some footer rows for calculations with fallback values -->
                <tfoot>
                    <tr>
                        <td class="noborder" colspan="3"></td>
                        <td colspan="2">Total (ex VAT)</td>
                        <td colspan="1">
                            <num value='{{model.order.Total}}' data-format='{{style.currencyFormat}}' />
                        </td>
                    </tr>
                    <tr>
                        <td class="noborder" colspan="3"></td>
                        <td colspan="2">VAT</td>
                        <td colspan="1">
                            <num value='{{model.order.Total * var(model.order.TaxRate,0.2)}}' data-format='{{style.currencyFormat}}' />
                        </td>
                    </tr>
                    <tr>
                        <td class="noborder" colspan="3"></td>
                        <td colspan="2" style="background-color: calc(style.altColor);">Grand Total </td>
                        <td colspan="1" style="background-color: calc(style.altColor);">
                            <num value='{{model.order.Total + (model.order.Total * var(model.order.TaxRate, 0.2))}}' data-format='{{style.currencyFormat}}' />
                        </td>
                    </tr>
                </tfoot>
            </table>
            <div id='terms'>
                <div id='paidAlready' hidden='{{if(model.order.PaymentTerms &lt; 0, "", "hidden")}}'>
                    <p>Thank you for pre-paying for these items. They will be shipped immediately</p>
                </div>
                <div id='payNow' hidden='{{if(model.order.PaymentTerms == 0, "", "hidden")}}'>
                    <p>Please pay for your items now, and we can process your order once received.</p>
                </div>
                <div id='paySoon' hidden='{{if(model.order.PaymentTerms &gt; 0, "", "hidden")}}'>
                    <p>Your items will be shipped immediately, please ensure you pay our invoice within <b>{{model.order.PaymentTerms}} days</b></p>
                </div>
            </div>
        </div>
    </body>
    </html>




New for version 5.1 is the support for variables, and the ``var()`` and ``calc()`` functions inside styles.

.. code:: html

    <!DOCTYPE HTML>
    <html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>{{concat('Orders for ', model.user.firstname)}}</title>
        <!-- external stylesheet reference -->
        <link rel="stylesheet" href="./css/orderStyles.css" />
        <!-- in document styles -->
        <style type="text/css" >

            :root{
                --theme_color:#232323;
                --theme_space:10pt;
                --theme_align: center;
                --td_w1: 30pt;
            }

            table.orderlist{
                width:100%;
            }

            #payNow {
                border: 1px solid red;
                padding: 5px;
                background-color: #FFAAAA;
                color: #FF0000;
                font-weight: bold;
            }

            .content{
                background-color: var(--theme_color);
                font-size: var(--theme_text_size);
            }

            .content h2{
                padding: var(--theme_space);
                margin-bottom: calc(--theme_space * 2);
            }

            .orderlist tr{
                background-color: #EEE;
            }

            .orderlist tr.alt{
                background-color: #DDD;
            }

            .orderlist td.w1{
                width: var(--td_w1);
            }

            .orderlist td.w2{
                width: calc(--td_w1 * 2);
            }

            .orderlist td.w3{
                width: calc(--td_w1 * 3);
            }

        </style>
    </head>
    <body>
        <div class='content' >
                <h2>
                    {{count(model.order.items)}} orders for {{join(' ', model.user.salutation, model.user.firstname, model.user.lastname)}}
                </h2>
            </div>
            <div style='padding: var(--theme_space); font-size: 12pt'>
                <table class='orderlist' >
                    <thead>
                        <tr style='background-color: #666; color: #FFF'>
                            <td class='w1'>#</td>
                            <td class='w2'>Item</td>
                            <td>Description</td>
                            <td class='w3'>Unit Price</td>
                            <td class='w2'>Qty.</td>
                            <td class='w3'>Total</td>
                        </tr>
                    </thead>
                    <tbody>
                        <template data-bind='{{model.order.items}}'>
                            <tr style='background-color: {{if(index() % 2 == 1, "#DDD","#EEE")}}'>
                                <td>{{index() + 1}}</td>
                                <td>{{.itemNo}}</td>
                                <td>{{.name}}</td>
                                <td><num value='{{.price}}' data-format='{{model.order.currencyFormat}}' /></td>
                                <td>{{.qty}}</td>
                                <td>
                                    <num value='{{.price * .qty}}' data-format='{{model.order.currencyFormat}}' />
                                </td>
                            </tr>
                        </template>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan='3' style="border:none;"></td>
                            <td colspan='2'>Total (ex.Tax)</td>
                            <td>
                                <num value='{{model.order.total}}' data-format='{{model.order.currencyFormat}}' />
                            </td>
                        </tr>
                        <tr>
                            <td colspan='3' style="border:none;"></td>
                            <td colspan='2'>Tax</td>
                            <td>
                                <num value='{{model.order.total * model.order.taxRate}}' data-format='{{model.order.currencyFormat}}' />
                            </td>
                        </tr>
                        <tr>
                            <td colspan='3' style="border:none;"></td>
                            <td colspan='2' style='background-color: #666; color: #FFF'>Grand Total</td>
                            <td style='background-color: #666; color: #FFF'>
                                <num value='{{(model.order.total * model.order.taxRate) + model.order.total}}' data-format='{{model.order.currencyFormat}}' />
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </div>
    </body>
    </html>


And our output should now look something similar to this.

.. figure:: ../images/doc_styled_orders.png
    :target: ../_images/doc_final_orders.png
    :alt: Styled order items.
    :class: with-shadow

`Full size version <../_images/doc_styled_orders.png>`_


There is a lot going on here, but...

* The heading is counting the number of order items and joining some strings together
* The table head is setting the widths of the columns that the content flows into.
* The table body has a ``template`` and is looping over the ``model.order.items`` collection, and creating a row for each of the items.
* The ``index()`` function is returning the *zero-based* index in the collection.
* The ``if(calc, true, false)`` function is setting the style for alternate rows.
* Inside the template row we are referring to the current item with the dot prefix.
* Inside the template row we can still reference the global document parameters without the dot prefix.
* The I 13 item has a long desciption that is flowing across multiple line in the cell.
* The ``footer`` rows are performing some calculations based on the summary information, and outputting the total values.
* The ``num @data-format`` is changing the output text to a currency value within the model.


It is also possible to nest templates, for example looping over orders and items in the orders.