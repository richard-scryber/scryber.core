======================================
Drawing paths and shapes
======================================

Scryber includes a full drawing capability.

* Lines
* Recangles
* Elipses
* Polygons
* Bezier Curves
* Groups

The drawing components can be either just inline in a document content, or placed directly in a canvas for ease of layout, and compnentization.

Drawing Lines
=============

By default a line will simply extend as a horizontal line block across the available space. Single point width with a black stroke color.
This can be changed using either explicit, or applied style information, with color; width; padding etc.
A width will restrict (or expand) the size horizontally.
If a height is added alone, then it will become a vertical line.
Apply both and it's diagonal, and can also be positioned inline as part of the flowing content.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Styles>

        <styles:Style applied-type="pdf:Div" >
            <styles:Padding all="10pt"/>
            <styles:Margins bottom="10pt" />
            <styles:Background color="#AAA"/>
        </styles:Style>
    
        <!-- Values set on the styles class-->
        <styles:Style applied-class="red" >
            <styles:Padding top="5pt" bottom="5pt" />
            <styles:Stroke color="red" width="3pt"/>
        </styles:Style>
        
    </Styles>
    <Pages>
        
        <pdf:Page styles:margins="20pt" >
        <Content>

            <pdf:Div >
                This is some content
                <pdf:Line />
                After the line.
            </pdf:Div>

            <pdf:Div >
                This is some content
                <pdf:Line styles:class="red" />
                After the line.
            </pdf:Div>

            <pdf:Div >
                This is some content
                <pdf:Line styles:class="red" styles:width="40pt" />
                After the line.
            </pdf:Div>
            
            <pdf:Div >
                This is some content
                <pdf:Line styles:class="red" styles:height="40pt" />
                After the line.
            </pdf:Div>

            <pdf:Div >
                This is some content
                <pdf:Line styles:class="red" styles:position-mode="Inline" styles:height="40pt" styles:width="40pt" />
                After the line.
            </pdf:Div>
        
        </Content>
        </pdf:Page>
    </Pages>
    
    </pdf:Document>

.. image:: images/drawingPathsLines.png


Drawing other shapes
=====================

Rectangles, elipses and polygons are all standard shapes supported by scryber. 
Generally, as closed shapes you will want to provide a width and a height to them, 
so they do **not** fill all the available space, which they will do if not set (both 
horizontally and vertically.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
        <Styles>

            <styles:Style applied-type="pdf:Div" >
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

            <styles:Style applied-class="inline" >
                <styles:Position mode="Inline"/>
                <styles:Padding all="5pt"/>
            </styles:Style>
            
        </Styles>
        <Pages>
            
            <pdf:Page styles:margins="20pt" >
            <Content>
                <pdf:Div >
                    This is some content<pdf:Br/>
                    
                    <pdf:Rect styles:class="red small inline" />
                    <pdf:Ellipse styles:class="red small inline" ></pdf:Ellipse>
                    <pdf:Poly styles:class="red small inline" styles:vertex-count="3" />
                    <pdf:Poly styles:class="red small inline" styles:vertex-count="5" styles:vertex-step="2" />
                    <pdf:Poly styles:class="red small inline" styles:vertex-count="10" styles:vertex-step="3" />

                    <pdf:Br/>After the line.
                </pdf:Div>

            </Content>
            </pdf:Page>
        </Pages>
    
    </pdf:Document>


.. image:: images/drawingPathsShapes.png


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
=====================

Shapes obey the same rules as other block level components when it comes to positioning (see :doc:`component_positioning`)

The location (x and y) of a shape will automatically change the position mode to relative.
Applying a position mode of absolute will take the shape completely out of the flow of the document.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Styles>

        <styles:Style applied-type="pdf:Div" >
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

        <pdf:Page styles:margins="20pt" >
        <Content>
            <pdf:Div styles:bg-color="#AAA" >
                This is some content<pdf:Br/>

                <!-- relatively positioned shapes -->
                <pdf:Rect styles:class="red small relative" />
                <pdf:Ellipse styles:class="red small relative" 
                            styles:x="220pt" styles:fill-opacity="0.5" ></pdf:Ellipse>
                
                <!-- absolutely positioned shapes -->
                <pdf:Poly styles:class="small absolute" 
                            styles:vertex-count="5" styles:vertex-step="2" />
                <pdf:Poly styles:class="small absolute" styles:x="440pt"
                            styles:vertex-count="10" styles:vertex-step="3" />

                <pdf:Br/>After the line.
            </pdf:Div>

        </Content>
        </pdf:Page>
    </Pages>

    </pdf:Document>


.. image:: images/drawingPathsPositioned.png

Drawing paths
=============

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

    <pdf:Document xmlns:pdf="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd" >
    <Styles>

        <styles:Style applied-type="pdf:Div" >
            <styles:Padding all="10pt"/>
            <styles:Margins bottom="10pt" />
            <styles:Background color="#AAA"/>
        </styles:Style>

    </Styles>
    <Pages>

        <pdf:Page styles:margins="20pt" >
        <Content>
            <pdf:Div styles:bg-color="#AAA" >
                This is some content<pdf:Br/>

                <!-- relatively positioned shapes -->
                <pdf:Canvas styles:width="450" styles:height="400" styles:border-color="black" styles:bg-color="white">

                    <pdf:Path d="M 100 350 l 150 -300" styles:stroke-color="red" styles:stroke-width="3" ></pdf:Path>
                    <pdf:Path d="M 250 50 l 150 300" styles:stroke-color="red" styles:stroke-width="3" ></pdf:Path>

                    <pdf:Path d="M 175 200 l 150 0" styles:stroke-color="green" styles:stroke-width="3" ></pdf:Path>

                    <pdf:Path d="M 100 350 q 150 -300 300 0" styles:stroke-color="black" styles:fill-style="None" styles:stroke-width="3" ></pdf:Path>

                    <pdf:Ellipse styles:x="97" styles:y="347" styles:width="6" styles:height="6" styles:fill-color="black" />
                    <pdf:Ellipse styles:x="247" styles:y="47" styles:width="6" styles:height="6" styles:fill-color="black"  />
                    <pdf:Ellipse styles:x="397" styles:y="347" styles:width="6" styles:height="6" styles:fill-color="black"  />

                    <pdf:Label text="A" styles:x="75" styles:y="350" styles:font-bold="true" styles:font-size="30" />
                    <pdf:Label text="B" styles:x="240" styles:y="15" styles:font-bold="true" styles:font-size="30" />
                    <pdf:Label text="C" styles:x="400" styles:y="350" styles:font-bold="true" styles:font-size="30" />
                </pdf:Canvas>
                After the canvas.
            </pdf:Div>

        </Content>
        </pdf:Page>
    </Pages>

    </pdf:Document>


.. image:: images/drawingPathsBezier.png

Fills and Repeats
=================

All closed shapes support the use of Solid or Repeating image fills.
See :doc:`drawing_images` for more.
