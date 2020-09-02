=====================================
Labels, Dates,  Numbers and Text
=====================================

Along with text containers, Scryber also supports a range of inline 
components for specific textual values.

Label Component
===============

The doc:Label is a texual component who's default value is an empty string. 
It can be styled as any span and supports data binding.


.. code-block:: xml

    <doc:Label text="{@:Label}" />


Text Component
===============

The doc:Text component is also available, and is **not** stylable. 
It will simply add text to a run of characters.

However, it too is fully bindable.

.. code-block:: xml

    <doc:Text value="{@:Label}" />

Number Component
================

The doc:Number component is available and it can be styled 
as any span. It supports data binding, but also the standard .Net number format
options for converting to a string.

.. code-block:: xml

    <doc:Number value="{@:Number}" styles:number-format="C" />


Date Component
================

The doc:Date component is available and it can be styled 
as any span. It supports data binding, but also the standard .Net date format
options for converting to a string.

The default value is the current date and time, if not set.

.. code-block:: xml

    <doc:Date value="{@:Number}" styles:date-format="D" />


These also support all the inline style attributes associated with components (and block positioning
excetera if their position-mode is changed.

.. code-block:: xml

    <?xml version="1.0" encoding="utf-8" ?>

    <doc:Document xmlns:doc="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd"
                xmlns:styles="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd"
                xmlns:data="http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd">
    <Params>
        <doc:Double-Param id="Currency" value="1020.50" />
        <doc:Double-Param id="Long" value="10000000000"/>
        <doc:Date-Param id="Eons" value="1664-03-06" />
        <doc:String-Param id="label" value="text value" />
        <doc:String-Param id="DateFormat" value="ddd dd MMMM yyyy" />
    </Params>
    
    <Styles>

        <styles:Style applied-type="doc:H5" >
            <styles:Text decoration="Underline" />
        </styles:Style>

        <styles:Style applied-class="field" >
            <styles:Font italic="true" />
            <styles:Fill color="red"/>
        </styles:Style>

        <styles:Style applied-class="date" >
            <styles:Text date-format="{@:dateFormat}"/>
        </styles:Style>
    </Styles>
    <Pages>

        <doc:Page styles:margins="20pt" styles:font-size="12pt">

        <Content>
            <doc:H5 text="Inline Text Components" styles:margins="20" />
            <doc:Para>
                The inline components support assignment of simple values (or even default values).<doc:Br/>
                For example this is a number <doc:Number value="10" />, this is a <doc:Text value="Label"  />, and this is the current date and time <doc:Date />.
            </doc:Para>
            <doc:Para>
                However the real power of these components is, that they are bindable and can be explicitly styled.<doc:Br/>
                For example this is a number from a parameter <doc:Number value="{@:Currency}" styles:number-format="C" /> in currency format,
                this is a number with a specific format <doc:Number value="{@:Long}" styles:number-format="#,###,000.000" />,
                this is a <doc:Label text="{@:Label}" styles:class="field"  />, and this is a date and time '<doc:Date value="{@:Eons}" styles:class="date" />' with
                the style applied.
            </doc:Para>

            <doc:H5 text="And they can be used for data sources in tables, loops etc." styles:margins="20" />
            <doc:Table>
                <doc:Header-Row>
                    <doc:Header-Cell>Label</doc:Header-Cell>
                    <doc:Header-Cell>Number</doc:Header-Cell>
                    <doc:Header-Cell>Date</doc:Header-Cell>
                </doc:Header-Row>
                <doc:Row>
                    <doc:Cell>
                        <doc:Label text="{@:Label}" />
                    </doc:Cell>
                    <doc:Cell>
                        <doc:Number value="{@:Currency}" styles:number-format="C" />
                    </doc:Cell>
                    <doc:Cell>
                        <doc:Date value="{@:Eons}" styles:date-format="D" />
                    </doc:Cell>
                </doc:Row>
            </doc:Table>
        </Content>
        </doc:Page>
    
    </Pages>
    
    </doc:Document>

.. image:: images/documentTextInline.png


Culture and Formats
===================

The use of numbers and dates in a template, can be affected by the culture of the OS.
Whether that is numbers directly entered, or bound to the data.

This can be critical for the creation of dates and numbers.

Scryber supports the `parsing-culture` on the scryber processing instruction that alters the reading an loading of dates etc.
See :doc:`document_structure` for more information.

