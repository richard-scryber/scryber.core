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
    * Ordered lists with numbering styles, see :doc:`reference/pdf_orderedlist`
    * Unodered lists with a bullet styles, see :doc:`reference/pdf_unorderedlist`
    * Definition lists with a label and content, see :doc:`reference/pdf_definitionlist`
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


Textual components
==================

* Text
* Number
* Date
* Label
* PageNumber
* PageOf



Graphical components
====================

* Images
    * A static or dynamic image


Data visual components
======================

* ForEach
* DataGrid
* DataList
* With
* WithFieldSet
* Choose
* If

Html components
===============

* Html
* Html Fragment.


Navigational components
=======================

* Link
* Outline
* Column break
* Page break

