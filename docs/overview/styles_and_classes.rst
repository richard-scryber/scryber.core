===============================
Styles, Classes and selectors
================================




.. code:: html

    <!DOCTYPE HTML>
    <html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <title>{{concat('Orders for ', model.user.firstname)}}</title>
    </head>
    <body>
        <div style='background-color: {{theme.color}};' >
            <div style='padding:{{theme.space}}'>
                <h2>{{count(model.order.items)}} orders for {{join(' ', model.user.salutation, model.user.firstname, model.user.lastname)}}</h2>
            </div>
            <div style='padding: {{theme.space}}; font-size: 12pt'>
                <table style='width:100%'>
                    <thead>
                        <tr style='background-color: #666; color: #FFF'>
                            <td style='width:30px'>#</td>
                            <td style='width:60px'>Item</td>
                            <td>Description</td>
                            <td style='width:100px'>Unit Price</td>
                            <td style='width:60px'>Qty.</td>
                            <td style='width:90px'>Total</td>
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