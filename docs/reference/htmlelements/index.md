---
layout: default
title: HTML Elements
parent: Template reference
parent_url: /reference/
has_children: true
has_toc: false
nav_order: 1
---


# Supported HTML Element Reference
{: .no_toc }

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---


## Overview

The library supports the use of (x)html elements, also referred to a tags to structure content within a template. This can be extended by embedding external files, or dymamically binding elements.

The root level of a document should always be the <code>&lt;html&gt;</code> element, (preferably) using the xmlns attribute namespace to define the content as xhtml. Followed by the <code>&lt;head&gt;</code> element for document meta data (to describe the the document) and then a <code>&lt;body&gt;</code> to contain the content of the document. 

```html
   <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
      <!-- document information -->
    </head>
    <body>
   
      <!-- add further content -->

    </body>
   </html>
```

More information on actual document creation can be found in <a href='/index.html'>Getting Started</a>
{: .no_toc }
---


### Using Comments

Enclosing any content starting with a <code>&lt;!-</code> and ending with <code>--&gt;</code> will mark a comment within the document.

This content will not be processed, and be ignored. It can either be used to exclude content whilst creating a template, or adding context to the structure of a document.

---

### Case sensitivity

By default **all** elements are *case sensitive* and are all lower case.

---

## Supported Elements

The library supports the following elelemts (tags) within a template.

---

### Document Root Element

The root of a document is always the <code>&lt;html&gt;</code> element. Any known DTD and or processing instructions (<code>&lt;?&nbsp;&nbsp;?&gt;</code>) along with whitespace and comments are supported before the outermost html element

| Element  | Tag  | Description |
|---|---|---|
| <a href='elements/html_html_element.html' >Html Root</a>   | <code>&lt;html&gt;</code> | Marks the start of a complete document template, and encapsulates all references, metadata and content for that template.   |

---

### Sectioning Root Elements


Within the document there should be a <code>&lt;head&gt;</code> for the metadata and either a <code>&lt;body&gt;</code> or <code>&lt;frameset&gt;</code> for the actual content.

| Element  | Tag  | Description |
|---|---|---|
| <a href='elements/html_head_element.html' >Head Content</a>   | <code>&lt;head&gt;</code> | Will mark the beginning of the metadata section for the document.   |
| <a href='elements/html_body_element.html' >Body Content</a>   | <code>&lt;body&gt;</code> | Will mark the beginning of the visible content within the document.   |
| <a href='elements/html_frameset_frame_element.html' >Frameset Content</a>   | <code>&lt;frameset&gt;</code> | Will mark the beginning of a set of <code>&lt;frame&gt;</code> elements that a source document, or template and a set of pages from that document to include. Replaces the <code>&lt;body&gt;</code> element.   |
| <a href='elements/html_frameset_frame_element.html' >Frame</a>   | <code>&lt;frame&gt;</code> | An element within a <code>&lt;frameset&gt;</code> that begins a new section of content from within an existing document, or from a referenced template.  |

---

### Document Metadata Elements


The library supports the use of the following elements within the meta-data <code>&lt;head&gt;</code> of the document.

| Element  | Tag  | Description |
|---|---|---|
| <a href='elements/html_head_element.html' >Document Title</a>   | <code>&lt;title&gt;</code> | A purely textual value that will set the display title for the output document.   |
| <a href='elements/html_head_element.html' >Document Base Path</a>   | <code>&lt;base&gt;</code> | A folder or uri reference to to a path where any relative files specified in the content of the document (images etc.) can be located.  |
| <a href='elements/html_meta_element.html' >Meta data</a>   | <code>&lt;meta&gt;</code> | A generalized informational tag that can define information about the final document production, its owners or security settings for use.  |
| <a href='elements/html_link_element.html' >Linked files</a>   | <code>&lt;link&gt;</code> | References an external file that contains resources (specifically styles) that the document should use when generating the output.  |
| <a href='elements/html_style_element.html' >Style content</a>   | <code>&lt;style&gt;</code> | Marks the document specific visual styles for the content that the document should use. Has a higher priority than any linked stylesheets.  |



---

### Content Sectioning Elements


The library supports the use of the following sectioning elements used to divide up the main content of the template into significant blocks.

| Element  | Tag  | Description |
|---|---|---|
| <a href='elements/html_header_footer_element.html' >Page Header</a>   | <code>&lt;header&gt;</code> | Begins a new block for content that will be shown at the top of the first page of the document, and all subsequent pages within a <code>&lt;body&gt;</code> element **<u>unless</u>** a continuation header is defined.   |
| <a href='elements/html_header_footer_element.html' >Page Footer</a>   | <code>&lt;footer&gt;</code> | Begins a new content block for elements that will be shown at the bottom of the first page of the document, and all subsequent pages within a <code>&lt;body&gt;</code> element **<u>unless</u>** a continuation header is defined.   |
| <a href='elements/html_continuation-header_element.html' >Continuation Header *</a>   | <code>&lt;continuation&#8209;header&gt;</code> | Begins a new content block for elements that will be shown at the top of every page of the document, within a <code>&lt;body&gt;</code> element **<u>except</u>** the first page.   |
| <a href='elements/html_continuation-footer_element.html' >Continuation Footer *</a>   | <code>&lt;continuation&#8209;footer&gt;</code> | Begins a new content block for elements that will be shown at the bottom of every page of the document, within a <code>&lt;body&gt;</code> element **<u>except</u>** the first page.   |
| <a href='elements/html_section_element.html' >Section</a>   | <code>&lt;section&gt;</code> | Denotes a block of content within the template that is in discreet. **NOTE:** By default each section in a template will start on a new page in the output document. |
| <a href='elements/html_main_element.html' >Main</a>   | <code>&lt;main&gt;</code> | Marks the content within the template that contains the majority of the document content.  |
| <a href='elements/html_nav_element.html' >Nav</a>   | <code>&lt;nav&gt;</code> | Marks the content within the template that performs navigation functions.  |


---


### Dynamic Content Elements

The following elements support the inclusion of further content, or outputing content dynamically based on current data during processing.


| Element  | Tag  | Description |
|---|---|---|
| <a href='elements/html_a_element.html' >Anchor Link</a>   | <code>&lt;a&gt;</code> |  Marks any of the inner content within the anchor link, as a navigation element to another point in the document output, or an external link to another resource. |
| <a href='elements/html_iframe_embed_element.html' >Embedded Content</a>   | <code>&lt;embed&gt;</code> | Allows external or dynamic content to be included within the output document as if it is part of the original content.  |
| <a href='elements/html_if_element.html' >If *</a>   | <code>&lt;if&gt;</code> | Denotes any optional block of content that will be output *only* when the <code>data-test</code> value is true.  |
| <a href='elements/html_iframe_embed_element.html' >i-Frame</a>   | <code>&lt;iframe&gt;</code> | Denotes an external source of content to be included within the output document, but unlike embedding, the inner content does not use any of the outer document visual style.  |
| <a href='elements/html_object_element.html' >Object</a>   | <code>&lt;object&gt;</code> | Denotes an external source to be attached within the output document, which can then be linked to via the anchor. |
| <a href='elements/html_template_element.html' >Template Content</a>   | <code>&lt;template&gt;</code> | An invisible container that will repeatably re-generate its contents within the document based on any bound data. |

---

### Structural Elements

The following elements provide a general way to divide content in the template, to be output in the document.


| Element  | Tag  | Description |
|---|---|---|
| <a href='elements/html_address_element.html' >Address</a>   | <code>&lt;address&gt;</code> | Denotes a single block of content that is a physical address.   |
| <a href='elements/html_article_element.html' >Article</a>   | <code>&lt;article&gt;</code> | Denotes a continuous block of content that is on a specific subject.  |
| <a href='elements/html_aside_element.html' >Aside</a>   | <code>&lt;aside&gt;</code> | Denotes a block of content that is not part of the current main content but relevant to place at the location.  |
| <a href='elements/html_blockquote_element.html' >Block Quote</a>   | <code>&lt;blockquote&gt;</code> | Denotes a quote within the context of the temlate that is separate from the primary content |
| <a href='elements/html_details_summary_element.html' >Details</a>   | <code>&lt;details&gt;</code> | Denotes a block of content that has a <code>summary</code> (below) and then further information available to provide greater clarity.|
| <a href='elements/html_details_summary_element.html' >Details Summary</a>   | <code>&lt;summary&gt;</code> | Denotes the shorter information of a details block before the main information. |
| <a href='elements/html_div_element.html' >Div</a>   | <code>&lt;div&gt;</code> | Denotes a discrete block of content, without specific meaining. |
| <a href='elements/html_fieldset_legend_element.html' >Fieldset</a>   | <code>&lt;fieldset&gt;</code> | Denotes a grouping of similar content with a <code>legend</code> (below) available to describe the content.  |
| <a href='elements/html_fieldset_legend_element.html' >Fieldset Legend </a>   | <code>&lt;legend&gt;</code> | A descriptive element to the outer <code>fieldset</code>. |
| <a href='elements/html_headings_element.html' >Headings 1-6</a>   | <code>&lt;h1&gt; - &lt;h6&gt;</code> | Denotes a heading within the content. Levels vary in importance from level 1 down to 6.  |
| <a href='elements/html_hr_element.html' >Horizontal Rule</a>   | <code>&lt;hr&gt;</code> | Denotes a horizontal line, by default across the width of the containing element. |
| <a href='elements/html_p_element.html' >Paragraph</a>   | <code>&lt;p&gt;</code> | Denotes an individual paragraph of content, usually textual. |
| <a href='elements/html_pre_element.html' >Pre-formatted</a>   | <code>&lt;pre&gt;</code> | Denotes a block of textual content that is already laid-out into explicit lines and spacing, that should be maintained.  |
| <a href='elements/html_span_element.html' >Span</a>   | <code>&lt;span&gt;</code> | Denotes a discreet inline container for phrasing content. |

---

### List Content Elements

The following elements allow content to be output within a list style, to show a grouping of similar items along with a marker, number or term to identify each one.

| Element  | Tag  | Description |
|---|---|---|
| <a href='elements/html_dl_dt_dd_elements.html' >Definition List</a>   | <code>&lt;dl&gt;</code> | A container block of multiple terms <code>dt</code> and associated defintion values <code>dd</dd>.  |
| <a href='elements/html_dl_dt_dd_elements.html' >Definition List Term</a>   | <code>&lt;dt&gt;</code> | Denotes the term to be defined within the list.  |
| <a href='elements/html_dl_dt_dd_elements.html' >Definition List Item</a>   | <code>&lt;dd&gt;</code> | Denotes the definition value of the term within the list. |
| <a href='elements/html_li_element.html' >List Item</a>   | <code>&lt;li&gt;</code> | Denotes an individual item within an ordered, unordered or menu list.  |
| <a href='elements/html_ol_element.html' >List Ordered</a>   | <code>&lt;ol&gt;</code> | Denotes a list of items whose order **is not** specific. |
| <a href='elements/html_ul_element.html' >List Unordered</a>   | <code>&lt;ul&gt;</code> | Denotes a list of items whose order **is**  |
| <a href='elements/html_menu_element.html' >Menu List</a>   | <code>&lt;li&gt;</code> | Denotes a list of items that represent a choice.  |


---

### Table Content Elements

The following elements allow content to be output within a tabular (grid) structure.

| Element  | Tag  | Description |
|---|---|---|
| <a href='elements/html_table_element.html' >Table</a>   | <code>&lt;table&gt;</code> | A container block whose contents will be output in a tablular (grid) format. |
| <a href='elements/html_table_element.html' >Table Body</a>   | <code>&lt;tbody&gt;</code> | An optional table row container that denotes the start of the main part of the parent table. |
| <a href='elements/html_table_element.html' >Table Header</a>   | <code>&lt;thead&gt;</code> | An optional table row container that denotes the start of the top descriptive part of the parent table. **NOTE**: By default a table header content will repead across columns and pages. |
| <a href='elements/html_table_element.html' >Table Footer</a>   | <code>&lt;tfoot&gt;</code> | An optional table row container that denotes the end content of the parent table. |
| <a href='elements/html_table_element.html' >Table Row</a>   | <code>&lt;tr&gt;</code> | A grouping of individual cells that will be output next to each other as a single row.  |
| <a href='elements/html_td_element.html' >Table Cell</a>   | <code>&lt;td&gt;</code> | A container for any content to be output in the document within the rectangular boundaries defined within the table. |
| <a href='elements/html_td_element.html' >Table Header Cell</a>   | <code>&lt;th&gt;</code> | A container that denotes a descriptive content of the table, to be output in the document within the rectangular boundaries defined within the table. |

---


### Image and Graphical Elements

The following elements support showing raster and vector graphical content within a document.


| Function  | Example  | Description |
|---|---|---|
| <a href='elements/html_img_element.html' >Image Content</a>   | <code>&lt;img&gt;</code>| An external (or bound) graphical image, referernced within the template, to be output within final document. |
| <a href='elements/html_figure_element.html' >Figure</a>   | <code>&lt;figure&gt;</code> | Denotes self contained content, usually a graphic or illustration, with an optional caption to describe the content. |
| <a href='elements/html_figure_element.html' >Figure Caption</a>   | <code>&lt;figcaption&gt;</code> | Denotes the description of the main content within the parent figure. |
| <a href='elements/html_picture_element.html' >Picture Content</a>   | <code>&lt;picture&gt;</code> | A group of <code>source</code> elements that define various graphical images that can be used on the pictures inner <code>img</code> element based on media and output type. |
| <a href='elements/html_picture_element.html' >Picture Source</a>   | <code>&lt;source&gt;</code> | A single reference to an external or bound graphic that will be used instead of the primary <code>img</code> source when it is a more appropriate rendition. |
| <a href='elements/html_meter_element.html' >Meters</a>   | <code>&lt;meter&gt;</code> | Denotes a scalar value within a pre-defined range. |
| <a href='elements/html_progress_element.html' >Progress</a>   | <code>&lt;progress&gt;</code> | Denotes how far within an individual process, based on scalar value within a known range. |
| <a href='/reference/svgelements/' >SVG Drawing</a>   | <code>&lt;svg&gt;</code> | The library supports a full set of SVG elements and attributes. These are covered in their own section <a href='/reference/svgelements/' >here</a>. |

---

### Dynamic Text Elements

The following elements support generating dynamic content within the final document based on provided or calculated values.

| Function  | Example  | Description |
|---|---|---|
| <a href='elements/html_output_slot_num_elements.html' >Number *</a>   | <code>&lt;num&gt;</code> | A textual element that can output a numeric value in a specific display format. |
| <a href='elements/html_pagenumber_element.html' >Page Number *</a>   | <code>&lt;page&gt;</code> | Outputs the current document or sections page number, or alternatively the page number of another referenced element within the final document. |
| <a href='elements/html_time_element.html' >Time Span</a>   | <code>&lt;time&gt;</code> | A textual element that can output a date time value in a specific format. |
| <a href='elements/html_var_element.html' >Variable Store and Display</a>   | <code>&lt;var&gt;</code> | A simple text element, that also allows data within the template to be calculated, stored and modified during the processing, and then used by other elements later on. |


### Semantic Text Elements

The following elements are based on and have a similar function to the standard HTML defined tags. Some will also alter the style of the inner content based on the intended meaning of the element.


| Function  | Example  | Description |
|---|---|---|
| <a href='elements/html_abbr_element.html' >Abbreviation</a>   | <code>&lt;abbr&gt;</code> | Marks the inner content as an abbreviation of a longer word or phrase. |
| <a href='elements/html_big_element.html' >Big</a>   | <code>&lt;big&gt;</code> | Marks that the inner content of the <code>big</code> element should use a larger font size, by default 120%. |
| <a href='elements/html_strong_em_element.html' >Bold</a>   | <code>&lt;b&gt;</code> | Marks that the inner content of the <code>b</code> element should use a heavier font weight. |
| <a href='elements/html_cite_defn_q_elements.html' >Citation</a>   | <code>&lt;cite&gt;</code> | Denotes the inner content of the <code>cite</code> is a reference to an different source which has been used in the document. |
| <a href='elements/html_code_kbd_samp_elements.html' >Code</a>   | <code>&lt;code&gt;</code> | Marks the inner content as a short input into or out of a computer programme. |
| <a href='elements/html_cite_defn_q_elements.html' >Definition</a>   | <code>&lt;defn&gt;</code> | Marks the inner content as to be defined within the template and output document. |
| <a href='elements/html_del_ins_u_s_element.html' >Mark Deleted</a>   | <code>&lt;del&gt;</code> | Marks a range of content that has been deleted from an original document. |
| <a href='elements/html_strong_em_element.html' >Emphasised</a>   | <code>&lt;em&gt;</code> | Marks a range of content that has stress emphasis. |
| <a href='elements/html_del_ins_u_s_element.html' >Mark Inserted</a>   | <code>&lt;ins&gt;</code> | Marks a range of content that has been added into the document. |
| <a href='elements/html_strong_em_element.html' >Italic</a>   | <code>&lt;i&gt;</code> | Marks that the inner content of the <code>i</code> element should use an oblique font style. |
| <a href='elements/html_code_kbd_samp_elements.html' >Keyboard</a>   | <code>&lt;kbd&gt;</code> | Marks the inner content of the <code>kbd</code> element as user text input. |
| <a href='elements/html_label_element.html' >Label</a>   | <code>&lt;label&gt;</code> |  Denotes a description for another associated template content elements |
| <a href='elements/html_br_element.html' >Line Break</a>   | <code>&lt;br&gt;</code> | Denotes a break in the flow of content in the output document, Any following content will begin on a new line.  |
| <a href='elements/html_mark_sub_sup_elements.html' >Marked span</a>   | <code>&lt;mark&gt;</code> | Marks the inner content of the <code>mark</code> element as highlighted for reference.  |
| <a href='elements/html_output_slot_num_elements.html' >Output</a>   | <code>&lt;output&gt;</code> | Marks the inner content of the <code>output</element> as  a value that has been calculated by a separate process. |
| <a href='elements/html_cite_defn_q_elements.html' >Quoted Span</a>   | <code>&lt;q&gt;</code> | Marks the inner content of the <code>q</element> as short inline quotation. |
| <a href='elements/html_code_kbd_samp_elements.html' >Sample Span</a>   | <code>&lt;samp&gt;</code> | Marks the inner content of the <code>output</element> as a sample or quoted output by a separate process. |
| <a href='elements/html_mark_sub_sup_elements.html' >Small</a>   | <code>&lt;small&gt;</code> | Marks that the inner content of the <code>small</code> element should use a smaller font size, by default 70%  |
| <a href='elements/html_del_ins_u_s_element.html' >Strikethrough</a>   | <code>&lt;strike&gt;</code> | Marks that the inner content of the <code>strike</code> element should have a line rendered through the middle of it.  |
| <a href='elements/html_strong_em_element.html' >Strong style</a>   | <code>&lt;strong&gt;</code> | Marks a reng of content that has a strong emphasis.  |

| <a href='elements/html_mark_sub_sup_elements.html' >Subscript</a>   | <code>&lt;sub&gt;</code> | Marks that the inner content of the <code>sub</code> element should be subscript - use a smaller font and have an ascender on the baseline of the parent line. |
| <a href='elements/html_mark_sub_sup_elements.html' >Superscript</a>   | <code>&lt;sup&gt;</code> | Marks that the inner content of the <code>sub</code> element should be superscript - use a smaller font and have a descender on the mid-point of the parent line. |
| <a href='elements/html_del_ins_u_s_element.html' >Underlined</a>   | <code>&lt;u&gt;</code> | Marks that the inner content of the <code>u</code> element should have a line rendered underneath it. |

---


* These elements are unique to the library, and are expected to be ignored by other applications / browsers that display the template. 

<!--- 

## Forms Content Elements <span class='label label-yellow'>alpha</span>

The library supports the use of the following logical functions.

| Operator  | Example  | Description |
|---|---|---|
| <a href='tags/input.html' >Input Control </a>   | <code>&lt;fieldset&gt;</code> |   |
| <a href='tags/select.html' >Select Control </a>   | <code>&lt;select&gt;</code> |   |

--->

