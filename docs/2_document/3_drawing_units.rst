========================================
Using units and measures
========================================

Within scryber all drawing and positioning is based from the top left of the page. Scryber allows the definition of a dimension 
based on a number of positioning and sizing structures. All based around the **Unit** of measure.

In all the examples so far we have used pt (points) as the unit of measure, but scryber also supports the use of millimeters (mm), inches (in)
and also pixels (px) as postfix units. A pixel is translated to 1/96 th of an inch for printing.

.. note:: Scryber does not support the use of relative dimensions: em, rem, vh etc. The only exception is the use of 100% on widths.

* PDFUnit
    * This is the base single dimension value.
    * Its default scale is the prinding standard points unit (1/72nd of an inch).
    * Values can also be specified in millimeters (mm) and inches (in) as well as explicitly in points.
    * Units are used in many places in xml templates
    * e.g. 72, 72pt, 1in, 25.4mm would all represent a 1.0 inch dimension.
    * Units can directly be cast and converted from integer and double values, or constructed in code.

* PDFSize
    * This is a width and height dimension with 2 PDFUnits.
    * Units can be mixed and matched within a size, but are generally only used internally for calculation
    * e.g. `72pt 1in` is a 1.0 inch wide and high

* PDFPoint
    * This represents a location on a page or container with an x and y component.
    * Again units can be mixed and matched within a point, but are generally only used internally for calculation
    * e.g. `72pt 1in` is 1 inch in from the left of the container and 1 inch down.

* PDFThickness
    * A thickness represent 4 dimensions around a square.
    * It follows the same order as html starting at the top and moving in a clockwise direction to the right, bottom and left.
    * It can be defined with 1, 2 or 4 values as a string, where 1 dimension refers to all values, 2 is the vertical and then horizontal and all 4 are explicit.
    * Thicknesses are used by the margins, padding, clipping attributes on components.
    * e.g. `25.4mm`, `1in 72pt` or `72 72 72 72` are all equivalent to 1 inch thickness all around.

* PDFRect
    * A rectagle is represented by 4 dimensions forming the x, y, width and height of a rectangle.
    * They can be mixed and matched in units, but are generally only used internally for calculation.
    * This should not be confused with the PDFRectangle used in drawing (see :doc:`drawing_paths`)


Units use in templates
-----------------------

When using units in xml templates its easy just to provide the values.
For example the following will add an absolutely positioned Div on a page with some thickness padding textual content

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
            "http://www.w3.org/TR/html4/strict.dtd">

    <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <style type="text/css">
            body{ 
                background-color: aqua;
            }

            .positioned {
                position: absolute;
                top: 30mm;
                left: 40mm;
                width: 100mm;
                padding:20pt 0.25in 4mm 20pt;
                background-color: #AAAAFF;
            }

        </style>
    </head>
    <body>
        <div class="positioned">
            20pt padding all around at 10pt, 20pt with a width of 100mm.
        </div>
    </body>
    </html>


.. image:: images/drawingunits1.png


Units in code
--------------

The same could have be achieved in code using the Unit and Thickness constructors.

All the dimensions have a range of constructors, casting and parsing options as needed.


.. code-block:: csharp

    //using Scryber.Drawing

    PDFUnit unit1 = 20; //implicit cast to 20pts
    var unit2 = (PDFUnit)72; //explicit cast to 72 points (1 inch)
    var unit3 = new PDFUnit(1, PageUnits.Inches); //explicit unit scale

    var pt1 = new PDFPoint(20,72); //defaults to points
    var pt2 = new PDFPoint(unit1, unit2); //explicit unit dimensions

    var thick1 = new PDFThickness(unit3); //Applies to all with a PDFUnit value
    var thick2 = new PDFThickness(10,20,10,20); //Applies explicit values to each dimensions

    var rect = PDFRect.Empty; //Set to Zeroed values.
    rect.Inflate(thick2); //Then inflate the rectangle by the thickness.

    var rect2 = PDFRect.Parse("12pr 10pt 100pt 2in"); //And all support parsing too.


One Hundred Percent
---------------------

The special value of 100% for width applies true to the underlying FullWidth style value.

By default div's have a FullWidth of true, so they will be 100% wide, but tables, lists etc do not. 
By specifying a width of 100% on these, they will use all the available space.

See :doc:`component_positioning` for more information.


Overiding relative units
-------------------------

Finally: If there is an existing template or file being used, then overriding any relative styles can be done using the @media print rule - so
it will only be used by scryber (or when the document is printed anyway).