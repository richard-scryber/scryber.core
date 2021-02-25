==========================
Class Hierarchy
==========================

When working with the code or adding objects to the code once parsed it is good to understand the hierarchy.

Scryber has a top level of Components. These create the basic level of document structure.


Base Component Classes
-----------------------

The base classes form the foundation of the functionality for each of the main concrete classes. It's much easier to create your own funtionality using one of these classes.

* Component - Implements the IPDFComponent interface along with IPDFBindable and 
* ContainerComponent - Holds child instances. Implements the 'InnerContent' collection as a protected property and the IPDFContainer.Contents implementation. Also passes the lifecycle methods to children.
* VisualComponent - Extends a container to be styled with the IPDFStyledComponent interface and a lot of properties for individual styles.
    * Panel - Base class for the standard container componnents (Div, Span, etc) that implements the IPDFViewPortComponent interface to do the laying out of content.
    * PageBase - Base class for all page types.
    * ImageBase - Base class for all the image types.
    * TextBase - Base class for the textual components and implements the IPDFTextComponent interface.
    * ShapeComponent - Base class for drawing components, implementing the IPDFGraphicPathComponent interface.

All components inheriting from VisualComponent have a virtual method for GetBaseStyle() which returns the default style for that component before anything is applied.

The PDFObjectType used in constructors is simply a 4 character struct that can identify the type of component, and can also be used for generating ID's. It can be directly cast from a string value or const string.


.. note:: Any custom classes should include a parameterless constructor if they sould be parsed as part of some other xml/xhtml content.


Collection classes
--------------------

Scryber maintains a bi-directional structure to the document content graph. When a component is added as a child to a container, that child's Parent value is updated to the container.
As such, each child knows it's parent, page and document. 

In order to maintian this the ContainerComponent creates the ComponentList class with itself as an owner.

Scryber then provides the abstract ComponnetWrappingList and ComponentWrappingList<T> classes for stronger typing on the contents of the collection and implementing the ICollection<T> interface.

e.g. The TableGrid has a Rows property of TableRowCollection which inherits from ComponentWrappingList<TableRow> and wraps the ContainerComponent.InnerContent.

.. code-block:: csharp

    public class TableGrid : ContainerComponent
    {
        //Strongly typed collection of Rows that will have their parent set automatically.
        public TableRowList Rows
        {
            get
            {
                if (this._rows == null)
                    this._rows = new TableRowList(this.InnerContent);
                return this._rows;
            }
        }

    }

    //The strongly typed collection for TableRows.
    public class TableRowList : ComponentWrappingList<TableRow>
    {
        public TableRowList(ComponentList content)
            : base(content)
        {
        }
    }


Concrete Component Classes
--------------------------

All the components are in the Scryber.Components namespace and inherit from VisualComponent

* PageBase
    * Page - a single page by default style, with a Header and Footer.
        * Section - allows multiple layout pages by default, with a continuation header and footer.
    * PageGroup - a set of PageBases instances but also a Header, Footer and continuation header and footer that are passed down.
* Panel
    * Div - basic concrete panel implementation with contents and a full width.
    * BlockQuote - basic block quote implementation with a custom style of 10pt margins.
    * Canvas - a container where all content will be relatively positioned and the content clipped to the canvas bounds.
    * List - contains inner list items.
        * ListOrdered - a list that has a default decimal numbering.
        * ListUnordered - a list that has a default bullet addornment.
        * ListDefinition - a list that has terms and content.
    * ListItem - the individual items in a list.
    * UserComponent - allows the dynamic loading of content from a remote source.
    * Paragraph - a block of inner content, with a 4pt margin at the top as a default style.
    * Preformatted - a block of inner content, with a default style for rendering code.
* TextBase
    * Date - renders the current of defined date in a specific format.
    * Number - renders a numeric value in any specific format.
    * PageNumberLabel - renders the current page (along with totals) in any specific format.
    * PageOfLabel - renders the page number of another component.
* TextLiteral - A non-visual component for text strings, including assigment within the constructor. 
* TableGrid - A layout of content in a tabular way.
* TableRow - A single row of cells within a grid.
* TableCell - the final content of the cells in a table grid.
* ShapeComponent
    * HorizontalRule - basic flat line.
    * Line - Line that supports a position and size.
    * Path - Complex path definition with M(oves), L(ines to) etc.
    * PolygonBase
        * Polygon - Multi-sided shape with style.
        * Rectangle - A 4 sided shape with style.
        * Triangle - Just the 3 sides.
        * Ellipse - A box bounded circle or ellipse with style.
* PageBreak - Forces the flow onto the next page if possible.
* ColumnBreak - Forces the flow onto the next column or page if possible.
* LineBreak - Forces the flow onto a new line.



Html Classes
------------------

When parsing content from HTML the document component graph will be constructed from subclasses of the main components in the Scryber.Html.Components namespace.

.. code-block:: csharp

    namespace Scryber.Html.Components {

        public class HTMLDiv : Scryber.Components.Div
        {

        }
    }

Layout content
---------------

In the creation of a PDF document, the components above are used to create the actual layout items. 
These are much more basic, but know how to generate the pdf content streams and data used by PDF readers.

If a document has a Page, and then a Section with 2 page breaks - the layout will be 4 pages long with all the text and runs in the respective pages.

If needed any component can implement or override the IPDFViewPortComponent interface and return a new LayoutEngine for that component.
The LayoutEngineBase and LayoutEnginePanel are good starting points to layout your own custom content.


* PDFLayoutDocument - Top level holding font references, image resource references and the list of layout pages.
* PDFLayoutPage - A single page of a content block, with an optional header content block and or footer content block, and any absolutely positioned regions.
* PDFLayoutBlock - A grouping of one or more column regions along with any relatively positioned regions, that will render the style.
* PDFLayoutRegion - A single continuous set of lines and/or other blocks.
* PDFLayoutLine - A single line of content runs.
* PDFLayoutRun - A single lightweight atomic graphical content operation.
    * PDFTextRun - Textual operation
        * PDFTextRunBegin - Start of the text, includes setting the font etc.
        * PDFTextRunCharacter - Text Drawing operation
        * PDFTextRunNewLine - Simple line break operation
        * PDFTextRunProxy - Placeholder for text to come from the owning component.
        * PDFTextRunEnd - Completion of text.
        * PDFTextRunSpacer - Offset of a line run to allow for other content.
    * PDFLayoutXObject - Renders PDF content as a separate stream, return the reference to that stream.
* PDFLayoutComponentRun - allows the owning component to render it's own content explicitly (e.g. Paths).



Content Styles
----------------


The style classes are based around a dictionary of inherited and direct style item keys with storongly typed style value keys.
All of the standay ones are defined in the Scryber.Styles.StyleKeys static class.

If a style value is inherited, the it will be copied to any descendent element (e.g. FontFamily) and any direct value will only be used on the component it is defined on (e.g. BackgroundColor)

Implementor can create their own style items and keys as needed using the static constructor methods with distinct object types (use mixed case to ensure they are unique).

.. code-block:: csharp

    
    const bool INHERITED = true;
    var tocStyle = StyleKey.CreateStyleItemKey((PDFObjectType)"Ctoc", INHERITED);
    var tocLeader = StyleKey.CreateStyleValue<LineStyle>((PDFObjectType)"Ctld", tocStyle);

This can then be used on any style definition or styled component to get or set a value, it can be bound to a value, and as it is inherited, will flow down with the content (merged).

.. code-block:: csharp

    var styleDefn = new StyleDefn();
    styleDefn.SetValue(tocLeader, LineStyle.Dotted);

    LineStyle default = LineStyle.None;
    var defined = styleDefn.GetValue(tocLeader, default);

    if(styleDefn.TryGetValue(tocLeader, out defined)
    {
        //Do something with the defined style.
    }


The style class hierarchy is as follows.

* StyleBase - root abstract class that holds the actual values.
    * Style : StyleBase - the main class used on components themselves directly.
        * StyleDefn : Style - has a class matcher property that will ensure that this style is only applied to Components that match.
        * StyleFull : Style - a readonly, locked set of style values with known values - position, font, padding etc.
    * StyleGroup : StyleBase - a collection of style base items, that can be treated as one item in an outer collection.

The document has a Styles property which is a StyleCollection, so any of the above can be added to the the document.
Each VisualComponent has a Style property where these values can be directly applied.

The flow for creating a full style for a component is linear.

1. The GetBaseStyle returns a new instance the standard style for a component.
1. If the component inherits from a super class VisualComponent then it should call the base.GetBaseStyle() and apply any styles to that before returning.
1. The GetAppliedStyle is then called with the base style.
1. This traverses up the component hierarchy, finally reaching the document.
1. The document calls MergeInto on its style collection with the base style.
1. Each style within the collection is MergedInto the style.
1. If that style is a StyleDefn it is checked to make sure it is matched, before being merged.
1. If that style is a StyleGroup, the it calls MergeInto on its own collection of styles.
1. If it should be merged, then each style value is assessed to see if it exists and compares the priority.
1. If the style that should be merged is a higher priority then the value is replaced.
1. We then come back to the original component and any direct styles are applied to to orginal base.
1. Once this is done it is pushed onto the StyleStack, where the hierarchy of styles from parent components are.
1. And finally a full style is built based on inherited and direct values.
1. That full style is retained and used through the rest of the layout and rendering.

Despite the number of steps, the build of styles is usually not an issue, compared to extracting font files, image binary data or encrypting streams.
However for some documents with a large number or containers e.g. a very long table with many rows it can become the limiting factor as well as memory intensive.

If this is the case then there are usually a lot of containers that have the very same style.
By setting the component.DataStyleIdentifier property, or in data-style-identifier in the template.
All components for the same identifier will use the same full style.

.. code-block:: html

    <templalate data-bind='{@:Model.Items}' >
        <tr data-style-identifier='boundrow'>
            <td class='desc-cell' data-style-identifier='boundcellDesc' >{@:.Description}</td>
            <td class='val-cell' data-style-identifier='boundcellValue' >{@:.Value}</td>
        </tr>
    </template>



Why and when to implement
--------------------------

A lot of the time, it is easier to use compound components to build all the main characteristics of the content needed.
However sometimes there is a need to use explict functionality or capabilities that are not currently available.

At scryber we also use this framework extensively to provide new top level features with safe knowledge the lower engine layers can deal with the grunt work.

See :doc:`extending_logging` and :doc:`extending_scryber` along with :doc:`namespaces_and_assemblies` for more on this.


