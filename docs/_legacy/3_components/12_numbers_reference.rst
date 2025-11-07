================================
Numbers, Dates and Times - PD
================================

Within the content of pages the use of numbers, dates and times can often be frought with cultural and positioning issues.
Scryber supports the formatting of these types of content with specific elements

.. code:: html

    <!-- xmlns='http://www.w3.org/1999/xhtml' -->

    <!-- output as british pounds -->
    <num value='100' data-format='£#0.00' />

    <!-- output current date time as a long date -->
    <time data-format='dd MMMM yyyy' />

    <!-- output specific date time as a month -->
    <time datetime='2021-09-21 12:00:00' data-format='MMM' />

    <!-- the values can also be calculated at run time -->
    <num value='{{model.total * 0.2}}' data-format='£#0.00'>

They can also be created and used in code

.. code:: csharp

    //using Scryber.Components;

    var num = new Number() { NumberFormat = "£#0.00", Value = 100 };

    var dt = new Date() { DateFormat = "D", Value = DateTime.Now };

    //look up an existing component
    if(parsedDoc.TryFindAComponentById("MyDateTime", out Date found))
        found.Value = DateTime.Now.AddDays(30);


The general use for the ``num`` and ``time`` tag is when binding data to a parameters, as other content is treated simply as text.

Generation methods
-------------------

All methods and files in these samples use the standard testing set up as outlined in :doc:`../overview/samples_reference`

See the :doc:`tables_reference` for more details on what is supported in tables.

Binding Simple numbers
----------------------




Binding Dates and Times
-----------------------


Calculating values
------------------


Building in code
-----------------

