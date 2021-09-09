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
        <!-- our heading is also now valid css -->
        <div style='color: var(--theme_color); padding: var(--theme_space); text-align: var(--theme_align)'>
            Hello {{model.user.FirstName}}.
        </div>
        <div style='padding: var(--theme_space);'>
            <!-- order list class styles are assigned -->
            <table class="orderlist" >
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
                <div id='payNow' hidden='{{if(model.order.PaymentTerms &gt; 0, "", "hidden")}}'>
                    <p>Your items will be shipped immediately, please ensure you pay our invoice within <b>{{model.order.PaymentTerms}} days</b></p>
                </div>
            </div>
        </div>
    </body>
    </html>

Now we can set up our theme and apply styles to the order list and #payNow box.



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



.. code:: csharp

    var doc = Document.ParseDocument("MyFile.html");

    doc.Params["model"] = new {
                user = new
                {
                    lastname = "Smith",
                    firstname = "Richard",
                    salutation = "Mr"
                },
                order = new
                {
                    items = new[] {
                        new { itemNo = "O 12", name = "Widget", qty = 2, price = 12.5 },
                        new { itemNo = "O 17", name = "Sprogget", qty = 4, price = 1.5 },
                        new { itemNo = "I 13", name = "M10 bolts with a counter clockwise thread on the inner content and a star nut top, tamper proof and locking ring included.", qty = 8, price = 1.0 },
                    },
                    currencyFormat = "£##0.00",
                    taxRate = 0.2,
                    total = 39.0
                }
    };

    doc.Params["theme"] = new {
                                color = "#FAFAFA",
                                space = "10pt",
                                align = "center"
                          };

    doc.SaveAsPDF("OutputPath.pdf");

.. figure:: ../images/doc_expression_ordertemplate.png
    :target: ../_images/doc_expression_ordertemplate.png
    :alt: Repeating binding on items for documents
    :width: 600px
    :class: with-shadow

`Full size version <../_images/doc_sexpression_ordertemplate.png>`_

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

The template data-binding works on any collection of objects, or an individual object as a *with* expression.


It would also be possible to use the model.order.items[index()].*property* in replacement of any of the context cases.

It is also possible to nest templates, for example looping over drders and items in the orders.