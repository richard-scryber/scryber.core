======================================
Drawing with SVG
======================================

Scryber includes the drawing capability with a subset of SVG capabilities.

* Lines
* Recangles
* Elipses
* Polygons
* Bezier Curves
* Groups
* Text
* Use and definitions
* ViewPorts

The drawing components should all be within a namespace qualified svg element, or prefixed svg at the document root.


Drawing SVG content
--------------------

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML >

    <html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <style type="text/css">

            svg{
                font-family:sans-serif;
                font-size:12pt;
            }

            .colored {
                fill: blue;
            }

        </style>
    </head>
    <body style="padding:20pt;">
        <p>The svg content is below</p>
        <!-- Adding an svg rect and some text to the page -->
        <svg xmlns="http://www.w3.org/2000/svg" >
            <rect class="colored" width="100pt" height="100pt" >
            </rect>
            <text x="20" y="50" fill="white" >I'm SVG</text>
        </svg>
        <p>And after the svg content</p>
    </body>
    </html>


.. image:: images/drawingPathSVG.png


As can be seen, the svg content is as a block and renders within the flow of the content.
The rect(angle) picks up the styles from the css and the font flows down from the svg container.

With scryber it is possible to use svg elements directly in the document by delcaring a prefixed namespace at the top. And the example below will render the same.
(See :doc:`namespaces_and_assemblies` for more information on how namespaces are used)


.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML >

    <!-- Declare the naespace here -->
    <html xmlns='http://www.w3.org/1999/xhtml'
        xmlns:svg="http://www.w3.org/2000/svg" >
    <head>
        <style type="text/css">

            .canvas {
                font-family: sans-serif;
                font-size: 12pt;
            }
            .colored {
                fill: blue;
            }

        </style>
    </head>
    <body style="padding:20pt;">
        <p>The svg content is below</p>
        <div class="canvas">
            <!-- And prefix our elements here (in a div) -->
            <svg:rect class="colored" width="100pt" height="100pt" >
            </svg:rect>
            <svg:text x="20" y="50" fill="#EEF" >I'm SVG</svg:text>
        </div>
        <p>And after the svg content</p>
    </body>
    </html>

.. note:: depending on the purpose, this might be advantageous. But not make any html parsers happy unless wrapped in an svg:svg element.

All examples below will follow the standard <svg xmlns='' > convention.


Supported shapes
-----------------

Scryber supports the standard shapes for rectangles, elipses, circles and lines. 
Generally, as closed shapes they will have a black fill and no stroke.

A group group (g) can contain multiple shapes and paths, and alter the style of inner content,
e.g. applying a constitent stroke.

Without a width or height the svg element in scryber with size to the inner content, but it is good practice to specify values.

Scryber also supports the use of styles on the svg element itself.

.. code-block:: html

    <?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE HTML >

    <html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
    </head>
    <body style="padding:20pt;">
        <p>The svg content is below</p>

        <svg xmlns="http://www.w3.org/2000/svg" style="border:solid 1px black" >
            <rect x="0pt" y="0pt" width="100pt" height="80pt" fill="lime" ></rect>
            <g id="eye" stroke="black" stroke-width="2pt" >
                <ellipse cx="50pt" cy="40pt" rx="40pt" ry="20pt" fill="white"></ellipse>
                <circle cx="50pt" cy="40pt" r="20pt" fill="#66F"></circle>
                <circle cx="50pt" cy="40pt" r="10pt" fill="black"></circle>
                <line x1="10" x2="90" y1="40" y2="40" />
                <line x1="50" x2="50" y1="20" y2="60" />
            </g>
        </svg>

        <p>And after the svg content</p>
    </body>
    </html>


.. image:: images/drawingPathsSVGShapes.png


Polygon vertices
-----------------

A polygon is rendered using the points calculated on the vertices distributed evenly around an elipse that would fit within the space available.

There are 2 options that control the shape points that are rendered, 

* the `vertex-count` that dictates the number of points on the shape
* The `vertex-step` that dictates the offset to the next point moved to for drawing.

The default for the step is 1, which will draw a regular polygon. Increasing the step will create more of a star like shape.

Line options
-------------

The stroke style also supports the ending and join options for Butt, Round and Projecting, that will alter the way lines and vertices are rendered.
The stroke style mitre limit (0 - 1) defines the angle at which the Projecting or Round will convert to a Butt ending. So the shape does not extend too far.


Specifying a location
-----------------------

Shapes obey the same rules as other block level components when it comes to positioning (see :doc:`component_positioning`)

The location (x and y) of a shape will automatically change the position mode to relative.
Applying a position mode of absolute will take the shape completely out of the flow of the document.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Styles>

        <styles:Style applied-type="doc:Div" >
            <styles:Padding all="10pt"/>
            <styles:Margins bottom="10pt" />
            <styles:Background color="#AAA"/>
        </styles:Style>

        <!-- Values set on the styles class-->
        <styles:Style applied-class="red" >
            <styles:Padding top="5pt" bottom="5pt" />
            <styles:Stroke color="red" width="3pt"/>
        </styles:Style>

        <styles:Style applied-class="small" >
            <styles:Size width="40pt" height="40pt"/>
            <styles:Fill color="lime"/>
        </styles:Style>

        <!-- A relative position-->
        <styles:Style applied-class="relative" >
            <styles:Position mode="Relative" x="200pt" y="80pt"/>
        </styles:Style>

        <!-- An absolute position -->
        <styles:Style applied-class="absolute" >
            <styles:Position mode="Absolute" x="400pt" y="160pt"/>
        </styles:Style>

    </Styles>
    <Pages>

        <doc:Page styles:margins="20pt" >
        <Content>
            <doc:Div styles:bg-color="#AAA" >
                This is some content<doc:Br/>

                <!-- relatively positioned shapes -->
                <doc:Rect styles:class="red small relative" />
                <doc:Ellipse styles:class="red small relative" 
                            styles:x="220pt" styles:fill-opacity="0.5" ></doc:Ellipse>
                
                <!-- absolutely positioned shapes -->
                <doc:Poly styles:class="small absolute" 
                            styles:vertex-count="5" styles:vertex-step="2" />
                <doc:Poly styles:class="small absolute" styles:x="440pt"
                            styles:vertex-count="10" styles:vertex-step="3" />

                <doc:Br/>After the line.
            </doc:Div>

        </Content>
        </doc:Page>
    </Pages>

    </doc:Document>


.. image:: images/drawingPathsPositioned.png

Drawing paths
--------------

Scryber supports the use of bezier paths for the creation of the complex curves and shapes.
The format of the drawing data (d) is exacly the same as the **svg** drawing operations.

* M = moveto
* L = lineto
* H = horizontal lineto
* V = vertical lineto
* C = curveto
* S = smooth curveto
* Q = quadratic Bézier curve
* T = smooth quadratic Bézier curveto
* A = elliptical Arc
* Z = closepath

See below for using the path data.

Canvases
========

A canvas is a panel that has an overflow set to clip and will place all components automatically in relatively positioned mode.
As such without an x or y location they will all appear at the top left.

It makes it ideal as a drawing surface.

The example below was mapped directly from the W3 Schools Svg paths 2 sample.
`https://www.w3schools.com/graphics/tryit.asp?filename=trysvg_path2`_

There are many other examples of the use of svg paths, that can be mapped directly to paths.

.. note:: Yes we are! We support html import and are actively looking at brining in svg.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Styles>

        <styles:Style applied-type="doc:Div" >
            <styles:Padding all="10pt"/>
            <styles:Margins bottom="10pt" />
            <styles:Background color="#AAA"/>
        </styles:Style>

    </Styles>
    <Pages>

        <doc:Page styles:margins="20pt" >
        <Content>
            <doc:Div styles:bg-color="#AAA" >
                This is some content<doc:Br/>

                <!-- relatively positioned shapes -->
                <doc:Canvas styles:width="450" styles:height="400" styles:border-color="black" styles:bg-color="white">

                    <doc:Path d="M 100 350 l 150 -300" styles:stroke-color="red" styles:stroke-width="3" ></doc:Path>
                    <doc:Path d="M 250 50 l 150 300" styles:stroke-color="red" styles:stroke-width="3" ></doc:Path>

                    <doc:Path d="M 175 200 l 150 0" styles:stroke-color="green" styles:stroke-width="3" ></doc:Path>

                    <doc:Path d="M 100 350 q 150 -300 300 0" styles:stroke-color="black" styles:fill-style="None" styles:stroke-width="3" ></doc:Path>

                    <doc:Ellipse styles:x="97" styles:y="347" styles:width="6" styles:height="6" styles:fill-color="black" />
                    <doc:Ellipse styles:x="247" styles:y="47" styles:width="6" styles:height="6" styles:fill-color="black"  />
                    <doc:Ellipse styles:x="397" styles:y="347" styles:width="6" styles:height="6" styles:fill-color="black"  />

                    <doc:Label text="A" styles:x="75" styles:y="350" styles:font-bold="true" styles:font-size="30" />
                    <doc:Label text="B" styles:x="240" styles:y="15" styles:font-bold="true" styles:font-size="30" />
                    <doc:Label text="C" styles:x="400" styles:y="350" styles:font-bold="true" styles:font-size="30" />
                </doc:Canvas>
                After the canvas.
            </doc:Div>

        </Content>
        </doc:Page>
    </Pages>

    </doc:Document>


.. image:: images/drawingPathsBezier.png

Fills and Repeats
------------------

All closed shapes support the use of Solid or Repeating image fills.
See :doc:`drawing_images` for more.
