================================
Numbers, Dates and Times
================================

Within the content of pages the use of numbers, dates and times can often be frought with cultural and positioning issues.
Scryber supports the formatting of these types of content with specific elements

.. code:: html

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



