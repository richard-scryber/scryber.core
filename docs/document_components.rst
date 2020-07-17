================================
Standard document components
================================

The scryber library comes with all the standard components used in document creation, similar to HTML

Document level visual components
================================

* Page
    * A single page, where content that will extend beyond the boundaries is truncated
    * Has an optional page header and page footer, as well as content.
    * see :doc:`reference/pdf_page`
* Section
    * A set of pages of the same size and orientation, where content that flow onto the next page.
    * Has an optional continuation page header and footer, along with the page header, page footer and content.
    * see :doc:`reference/pdf_section`
* Page-Group
    * A group of pages that can have shared size, header and footer content, and style.
    * Individual pages can override as needed.
    * see :doc:`reference/pdf_pagegroup`
* Page-Ref
    * A reference to one of the above components in a separate file.
    * Specified via a required source attribute.
    * see :doc:`referencing_files` 

Standard structural components
==============================

* Div
    * a block level component that will fill the width of the available parent.
    * see :doc:`reference/pdf_div`
* Span 
    * an inline compnent that can have any content including text.
    * see :doc:`reference/pdf_span`
* Table
    * A grid of rows and cells (that can be spanned across columns.
    * see :doc:`reference/pdf_tablegrid`
* Lists
    * Ordered lists with numbering styles.
    * Unodered lists with a bullet styles.
    * Definition lists with a label and content.
    * see :doc:`reference/pdf_list`
* Paragraph
    * A textual (and other content), that has a more defined style than a div.
    * see :doc:`reference/pdf_para`
* Heading (1 to 6)
    * A textual (and other content), that is given a pre-defined style based on it's level of 1 to 6
    * H1, H2, H3, H4, H5, H6
    * see :doc:`reference/pdf_headings`
* Layer-Group
    * A wrapper for a set of Layers.
    * Each layer will be relatively positioned (default to 0,0) ontop ove each other.
    * Layers can be shown and hidden as needed.
    * see :doc:`reference/pdf_layergroup`
* Canvas
    * A drawing panel that will by default relatively position all child components
    * see :doc:`reference/pdf_canvas`
* Block Quote
    * A panel with specific margins.
    * see :doc:`reference/pdf_blockquote`
* Preformatted
    * A container for pre-formatted text, that will not flow over new lines, or remove line breaks (by detault).
    * see :doc:`reference/pdf_pre`
* Component-Ref
    * A reference to an external file or stream that will be injected into the page at runtime.
    * see :doc:`referencing_files` 
* Column break
    * Stops the flow of content within the current region, and moves any following content onto the next available column.
    * Can be positioned at any depth within a multicolumn layout.
    * see :doc:`document_layout` and :doc:`reference/pdf_columnbreak`
* Page break
    * Stops the flow of content within the current page, and moves any following content onto the next available page.
    * Can be positioned at any depth within a layout.
    * see :doc:`document_layout` and :doc:`reference/pdf_pagebreak`


Textual components
==================

* Text
    * A text literal compenent where the text can be set to the @value attribute.
    * Supports full data binding.
    * see :doc:`reference/pdf_text`
* Number
    * A litteral component that supports numeric values (@value attribute as well as number formatting (@styles:number-format)
    * Can display numbers in any of the standard floating point, currency and integral types.
    * see :doc:`reference/pdf_number`
* Date
    * A litteral component that supports date time values (@value attribute as well as date formatting (@styles:date-format)
    * Can display dates in any of the standard localized formats.
    * see :doc:`reference/pdf_date`
* Label
    * A text literal component where the text can be set to the @text attribute.
    * Supports full data binding.
    * The only difference is a more formal distinction of purpose than text.
    * see :doc:`reference/pdf_label`
* PageNumber
    * A textual component that displays the current output page number where the component is placed.
    * Supports the use of page section counting and total document page count.
    * see :doc:`reference/pdf_pagenumber`
* PageOf
    * A textual compenent that displays the page number of a referenced component.
    * Supports the use of page section counting and total document page count.
    * see :doc:`reference/pdf_pageof`
* Link
    * A hyper link to a location within the current document, or another document, or a web resource.
    * Content within can be styled appropriately.
    * Document references can be based on ID or name.
    * Page links can be First, Previous, Next, Last or numbered.
    * see :doc:`document_linking` and :doc:`reference/pdf_link`


Graphical components
====================

* Images
    * A static or dynamic image loaded from a source, and inserting into the output document.
    * Supports the use of full, relative or dynamic url references.
    * Supports png, jpeg and tiff file formats.
    * Supports alpha channels where available in the source.
    * see :doc:`document_images` and :doc:`reference/pdf_image`
* Horizontal Rule
    * A single line within the flow of the document.
    * Can be styled as a independant component.
    * see :doc:`reference/pdf_hr`
* Line, Rect, Polygon, Ellipse, Path
    * Standard drawing components that can be used either within the flow of the content or for drawing/designs.
    * see :doc:`document_paths` 
    * and for individual components :doc:`reference/pdf_line`, :doc:`reference/pdf_rect`, :doc:`reference/pdf_ellipse`, :doc:`reference/pdf_polygon`, :doc:`reference/pdf_path`


Data visual components
======================

For a general use of the data components see :doc:`document_databinding`.
And for an overview of the data sources available see :doc:`document_datasources`

* ForEach
    * Loops through each value in a data source, with an optional step, offset and count.
    * Outputs the content within the tempate, that can be any inner content.
    * see :doc:`reference/data_foreach`
* DataGrid
    * Loops through each value in a data source.
    * Outputs the content as a table of results, with various column types.
    * Allows for auto population from a schema in a data source.
    * Also supports alternating styles, fotters and headers.
    * see :doc:`reference/data_datagrid`
* DataList
    * Loops through each value in a data source, with an optional step, offset and count.
    * Outputs the content as panels, lists, or spans.
    * Allows for auto population from a schema in a data source.
    * Also supports output order, flow direction, and alternating styles.
    * see :doc:`reference/data_datalist`
* With
    * Takes a data value or source and applies it to the current context so it can be used in binding statements.
    * Can have any content, and they are full components, rather than templates.
    * Supports both xml and object values.
    * see :doc:`reference/data_with`
* WithFieldSet
    * Takes a data value or source and applies it to the current context so it can be used in binding statements.
    * Supports the use of fields within the block to automatically create the content.
    * Allows for auto population from a schema in a data source.
    * Supports both xml and object values.
    * see :doc:`reference/data_withfieldset`
* Choose
    * Optionally displays a set of content based on a decision (test).
    * Allows multiple :doc:`reference/data_ChooseWhen` to be defined within the component.
    * The first true decision will be output, and all others not rendered in the document.
    * Allows the use of one :doc:`reference/data_ChooseOtherwise` component as a catch all.
    * see :doc:`reference/data_choose`
* If
    * Optionally displays a set of content based on a decision (test).
    * If the decision is false, then no inner content will be rendered.
    * see :doc:`reference/data_if`

Html components
===============

* Html Page
    * A full section that supports the inclusion for html (or markdown) content output within a document as it's own page(s).
    * Supports the use of inline style conversion (with limitations) to scryber styles.
    * Content can either be loaded dynamically by the component, assigned from a data source, or explicitly set from code.
    * see :doc:`using_html` for more information on Html in scryber.
    * see :doc:`reference/pdf_html`
* Html Fragment.
    * A block of html that can sit within a document.
    * Supports the use of inline style conversion (with limitations) to scryber styles.
    * Content can either be loaded dynamically by the component, assigned from a data source, or explicitly set from code.
    * see :doc:`using_html` for more information on Html in scryber.
    * see :doc:`reference/pdf_htmlfragment`


