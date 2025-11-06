---
layout: default
title: HTML Attributes
parent: Template reference
parent_url: /reference/
has_children: true
has_toc: false
nav_order: 2
---

# Supported HTML Attribute Reference
{: .no_toc }

---

<details open markdown="block">
  <summary>
    Table of contents
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---

## Overview

An attribute supports the element in which it is enclosed by adding further information in a formal and discreet manner. The library supports the use of the many standard element attributes on various <a href='../htmltags/' >Html Elements</a>. Where possible the library tries to match expected behaviour to the final output based on existing meaning.

The library also extended the behaviour of the element with a number of custom elements

```html
    {% raw %}<body id='bId' class='main-content other-class' >

      <!--  custom binding repeater on a list -->
        
      <ol class='model-list'>
        <template id='listing' data-bind='{{model.items}}' data-bind-max='200' >
          <li id='{{"item" + index()}}' class='model-item' >{{.name}}</li>
        </template>
      </ol>

      <!-- more content -->

    </body>{% endraw %}
```

More information on actual document creation can be found in <a href='/index.html'>Getting Started</a>. And all <a href='/reference/htmltags/' >elements</a> have a list of the specific attributes they individually support.

---

### Unsupported attributes

When re-using existing content, there are a lot of attributes that can be on an html file that are not supported, or relevant to the library. By default these attributes will be skipped over and ignored. However if running in <code>Strict</code> <a href='/learning/templates/conformancemode.html'>conformance mode</a> the library will raise an error each time it encounters an unknown attribute or attribute value.

---

### Case sensitivity

By default **All Visual Elements** attributes are *case sensitive* and are all lower case.

---

### Binding values to attributes

The library is strongly typed and expects specific types to be set on a value of an attribute. These can be explicity set within a template content, or created dynamically at generation time. Some attributes however are explicitly static only, or explicitly binding only - as the required type is not convertable. These are marked in tables below, with one of **Any** or **Binding Only** or **Static Only**.

---

## Attribute Reference

The library supports the following attributes.

---

### Global Attributes

The following attributes are supported on all visual elements - the elements that are within the body element, including the body element itself.

| Attribute  | Use | Bindable  | Description |
|---|---|---|--|
| <a href='attributes/attr_id.html' >id</a>   | *All Visual Elements* | Any | Defines an identifier for the element it is contained in, that can be used to refer to elsewhere in the template.   |
| <a href='attributes/attr_title.html' >title</a>   | *All Visual Elements* | Any | By default adds an entry into the document outline structure with the attribute text value to support navigation to the element. **NOTE**: See the data extension attributes below as some elements override this default behaviour.   |
| <a href='attributes/attr_style.html' >style</a>   | *All Visual Elements* | Any | Allows a full definition of the visual appearance of the element. Styles and classes are discussed in their own sections as part of <a href='/styling_content.html'>Styling Content</a> and a full reference section on <a href='/reference/cssproperties/'>CSS properties</a>    |
| <a href='attributes/attr_class.html' >class</a>   | *All Visual Elements* | Any | Specifies a set of style class names as <a href='/reference/cssselectors/'>CSS selectors</a> to apply to the element.   |
| <a href='attributes/attr_hidden.html' >hidden</a>   | *All Visual Elements* | Any | Indicates if this content should be displayed or not. As an xhtml template the value of the attribute should also be 'hidden' e.g. hidden='hidden'.  |
| <a href='attributes/attr_name.html' >name</a>   | *All Visual Elements* | Any | Defines an explicit name for the element it is contained in, that can be used to refer to elsewhere in the template.   |
| <a href='attributes/attr_data_content.html' >data-content</a>   | *All Visual Elements*, except <code>if</code> and <code>template</code> | *Binding Only* | Allows the dynamic binding of more visual content into the template at generation time from the documents data. More infomation on data binding can be found in the <a href='/learning/' >Learning section</a>   |
| <a href='attributes/attr_data_content.html' >data-content-action</a>   | *All Visual Elements* | Any | Defines an identifier for the element it is contained in, that can be used to refer to elsewhere in the template.   |
| <a href='attributes/attr_data_content.html' >data-content-type</a>   | *All Visual Elements* | Any | Defines an identifier for the element it is contained in, that can be used to refer to elsewhere in the template.   |
| <a href='attributes/attr_data_style_identifier.html' >data-style-identifier</a>   | *All Visual Elements* | *Static Only* | Defines an identifier for the element it is contained in, that can be used to refer to elsewhere in the template, to significantly improve performance of *many* repeating items in multiple structures.   |

---

### Global Event Attributes


The following event attributes are supported on all visual elements. For more information on document controllers and event handling see the <a href='/learning/binding/codebehind.html'>code behind</a> learning article.

| Attribute  | Use | Bindable  | Description |
|---|---|---|---|
| <a href='events/init.html' >on-init</a>   | *All Visual Elements* | Static Only | An event that is raised to a declared method on the defined controller when the element is initialized.   |
| <a href='events/loaded.html' >on-loaded</a>   | *All Visual Elements* | Static Only | An event that is raised to a declared method on the defined controller when the element is loaded.   |
| <a href='events/binding.html' >on-databinding</a>   | *All Visual Elements* | Static Only | An event that is raised to a declared method on the defined controller before the element is data bound.   |
| <a href='events/bound.html' >on-databound</a>   | *All Visual Elements* | Static Only | An event that is raised to a declared method on the defined controller after the element is databound.   |
| <a href='events/prelayout.html' >on-prelayout</a>   | *All Visual Elements* | Static Only | An event that is raised to a declared method on the defined controller before the element is laid out.   |
| <a href='events/postlayout.html' >on-postlayout</a>   | *All Visual Elements* | Static Only | An event that is raised to a declared method on the defined controller after the element is laid out.   |
| <a href='events/prerender.html' >on-prerender</a>   | *All Visual Elements* | Static Only | An event that is raised to a declared method on the defined controller before the element is rendered.   |
| <a href='event/postrender.html' >on-postrender</a>   | *All Visual Elements* | Static Only | An event that is raised to a declared method on the defined controller after the element is rendered.   |

---

### Supported Standard Attributes


The library supports the use of the following standard attributes that match existing attributes on html elements.

| Attribute  | Use | Bindable  | Description |
|---|---|---|---|
| <a href='attributes/attr_align_valign.html' >align</a>   | <code>img</code> | Any | Defines the alignment on a line for an image when it is laid out.   |
| <a href='attributes/attr_alt.html' >alt</a>   | <code>img</code>, <code>object</code> | Any | An alternative description for the element. *Not currently used, but defined*  |
| <a href='attributes/attr_charset.html' >charset</a>   | <code>&lt;meta&gt;</code> | Any | The character set for the meta data information. *Not currently used, but defined*  |
| <a href='attributes/attr_cite.html' >cite</a>   | <code>&lt;ins&gt;</code>, <code>&lt;del&gt;</code> | Any | The citation information for the inserter or deleter of the section. *Not currently used, but defined*  |
| <a href='attributes/attr_colspan_rowspan.html' >colspan</a>   | <code>&lt;td&gt;</code> | Any |  Defines the number of columns across, a cell occupies including the current column.  |
| <a href='attributes/attr_content.html' >content</a>   | <code>&lt;meta&gt;</code> | Any | Set the actual content value of a named meta-data element so that it can be used in document processing.  |
| <a href='attributes/attr_data_object.html' >data</a>   | <code>&lt;object&gt;</code> | Any | Sets the source file path to a specific location (using any document base path) so the attachment can be loaded and included.  |
| <a href='attributes/attr_datetime.html' >datetime</a>   | <code>&lt;ins&gt;</code>, <code>&lt;del&gt;</code>, <code>&lt;time&gt;</code> | Any | In the case of ins and del, specifies the timestamp for the modification. For a time element, specifies the date and/or time that should be displayed by the element.  |
| <a href='attributes/attr_for.html' >for</a>   | <code>&lt;label&gt;</code>, <code>&lt;output&gt;</code>, <code>&lt;page&gt;</code> | Any | Identifies the id of the referenced element this element is referring to. For a page element, this with then be the page number of that referenced element.  |
| <a href='attributes/attr_width_height.html' >height</a>   | <code>&lt;img&gt;</code> | Any | A legacy support attribute for the image element to explicitly set the pixel height for rendering. Use the CSS properties instead  |
| <a href='attributes/attr_high_low.html' >high</a>   | <code>&lt;meter&gt;</code> | Any | Defines the recommended high value for a graphical meter bar.  |
| <a href='attributes/attr_href.html' >href</a>   | <code>&lt;a&gt;</code>, <code>&lt;link&gt;</code> | Any | Sets the source file path to a specific location (using any document base path) so an image or external resource can be loaded and included.  |
| <a href='attributes/attr_http_equiv.html' >http-equiv</a>   | <code>&lt;meta&gt;</code> | Any | Defines a pragme directive for the template. *Not currently used, but defined*  |
| <a href='attributes/attr_lang.html' >lang</a>   | <code>&lt;html&gt;</code> | Any | Specifies the default output culture (e.g. en-US or fr-FR) for the resultant document. This impacts features such as number conversion and date conversion to rendered strings.  |
| <a href='attributes/attr_high_low.html' >low</a>   | <code>&lt;meter&gt;</code> | Any | Defines the recommended low value for a graphical meter bar.  |
| <a href='attributes/attr_min_max.html' >max</a>   | <code>&lt;meter&gt;</code>, <code>&lt;progress&gt;</code>  | Any | Defines the maximum value for a graphical meter or progress bar - based on this value the offset of the bar will be calculated.  |
| <a href='attributes/attr_media.html' >media</a>   | <code>&lt;source&gt;</code>, <code>&lt;style&gt;</code>  | Any | Specifies the mime type of the picture source or a media query the style is appropriate to be used for.  |
| <a href='attributes/attr_min_max.html' >min</a>   | <code>&lt;meter&gt;</code>  | Any | Defines the minimum value for a graphical meter - based on this value the offset of the bar will be calculated.  |
| <a href='attributes/attr_open.html' >open</a>   | <code>&lt;details&gt;</code> | Any | Used to define if a details section will show all content, or just the summary.  |
| <a href='attributes/attr_optimum.html' >min</a>   | <code>&lt;meter&gt;</code>  | Any | Defines the optimum value for a graphical meter.  |
| <a href='attributes/attr_property.html' >property</a>   | <code>&lt;page&gt;</code>  | Any | Specifies the type of page number that should be looked up and used, e.g. 'total', or 'section' page number or 'sectiontotal' number.  |
| <a href='attributes/attr_rel.html' >rel</a>   | <code>&lt;link&gt;</code>  | Any | Specifies the relationship of the linked source to the current source. **NOTE** anything other than 'stylesheet' will currently be ignored.  |
| <a href='attributes/attr_colspan_rowspan.html' >rowspan</a>   | <code>&lt;td&gt;</code>  | Any | Defines the number of rows down, a cell occupies including the current row.  |
| <a href='attributes/attr_scope.html' >scope</a>   | <code>&lt;th&gt;</code>  | Any | Defines whether a header cell is a header for a column, row, or group of columns or rows. Has no effect on output.  |
| <a href='attributes/attr_src.html' >src</a>   | <code>&lt;embed&gt;</code>, <code>&lt;frame&gt;</code>,<code>&lt;source&gt;</code>, <code>&lt;img&gt;</code>  | Any | Defines the external location of a resource (taking into account the document base path) that the element will use.  |
| <a href='attributes/attr_srcset.html' >srcset</a>   | <code>&lt;source&gt;</code>  | Any | Defines the external location of a range of resource (taking into account the document base path) that the element *can* use.  |
| <a href='attributes/attr_target.html' >target</a>   | <code>&lt;a&gt;</code>  | Any | Sets the location within the consuming application where the linked content should be shown. *Support is based on the reader applications implementation*  |
| <a href='attributes/attr_type.html' >type</a>   | <code>&lt;frame&gt;</code>, <code>&lt;source&gt;</code>, <code>&lt;style&gt;</code> <code>&lt;object&gt;</code>  | Any | Identifies the content mime type of a resource at an external location (taking into account the document base path).  |
| <a href='attributes/attr_val.html' >value</a>   | <code>&lt;progress&gt;</code>  | Any | Defines the actual value for a graphical progress bar - based on this value the offset of the bar will be calculated using max  |
| <a href='attributes/attr_width_height.html' >width</a>   | <code>&lt;img&gt;</code> | Any | A legacy support attribute for the image element to explicitly set the pixel width for rendering. Use the CSS properties instead  |

---

### Extension Attributes


The library uses the <code>data-*</code> attributes to extend the use of existing elements to preserve validity of a html template and provide support for the library features.

| Attribute  | Use | Bindable  | Description |
|---|---|---|---|
| <a href='attributes/attr_data_bind.html' >data-bind</a>   | <code>&lt;template&gt;</code> | *Binding Only* | Allows the dynamic binding of more visual content, **multiple times**, from within the template into the documents' layout, based on the data received from the data-bind value. More infomation on data binding can be found in the <a href='/learning/' >Learning section</a>  |
| <a href='attributes/attr_data_bind_advanced.html' >data-bind-max</a>   | <code>&lt;template&gt;</code> | Any | Limits the dynamic binding of visual content, to a **maximum** number of items. More infomation on data binding can be found in the <a href='/learning/' >Learning section</a>  |
| <a href='attributes/attr_data_bind_advanced.html' >data-bind-start</a>   | <code>&lt;template&gt;</code> | Any | Sets the start of the dynamic binding of more visual content, from within the template into the documents' layout, with zero being the first itteration. More infomation on data binding can be found in the <a href='/learning/' >Learning section</a>  |
| <a href='attributes/attr_data_bind_advanced.html' >data-bind-step</a>   | <code>&lt;template&gt;</code> | Any | Sets the loop over count of the dynamic binding data before any entry is added to the document's layout. More infomation on data binding can be found in the <a href='/learning/' >Learning section</a>  |
| <a href='attributes/attr_data_file.html' >data-file</a>   | <code>&lt;object&gt;</code> | *Binding Only* | Sets the attachment file dynamically to a PDFEmbeddedFileData instance, that will be attached in the document.  |
| <a href='attributes/attr_data_file.html' >data-file-data</a>   | <code>&lt;object&gt;</code> | Bindable Only | Sets the attachment file dynamically to a byte[] of a files binary contents, that will be attached in the document.  |
| <a href='attributes/attr_data_fit_to.html' >data-fit-to</a>   | <code>&lt;a&gt;</code> | Any | Specifies the view location that should occur when the link is clicked - FullPage, PageWidth, PageHeight or BoundingBox (of the referenced element to link to).  |
| <a href='attributes/attr_data_img.html' >data-img</a>   | <code>&lt;img&gt;</code> | Any | Sets the image file dynamically to a ImageData instance, that will be attached in the document.  |
| <a href='attributes/attr_data_img.html' >data-img-data</a>   | <code>&lt;img&gt;</code> | *Binding Only* | Sets the image file dynamically to a binary array (byte[]), that will be used, along with the mime-type, to load and use an image within the document.  |
| <a href='attributes/attr_data_img.html' >data-img-type</a>   | <code>&lt;img&gt;</code> | Any | Sets the data image mime-type, that is used, along with the image binary data, to load and use an image within the document.  |
| <a href='attributes/attr_data_li.html' >data-li-align</a>   | <code>&lt;li&gt;</code>, <code>&lt;menu&gt;</code>, <code>&lt;ol&gt;</code>, <code>&lt;ul&gt;</code> | Any | Specifies the alignment of the content of the list items identifier (number, bullet etc.). This can also be specified via custom style and css.  |
| <a href='attributes/attr_data_li.html' >data-li-concat</a>   | <code>&lt;menu&gt;</code>, <code>&lt;ol&gt;</code>, <code>&lt;ul&gt;</code> | Any | Specifies whether to concatentate the ist items identifier (number, bullet etc.) with any parent or related group list, forming a heirarchy. This can also be specified via custom style and css. |
| <a href='attributes/attr_data_li.html' >data-li-group</a>   | <code>&lt;menu&gt;</code>, <code>&lt;ol&gt;</code>, <code>&lt;ul&gt;</code> | Any | Specifies the name of the group that the ist items identifier (number, bullet etc.) continues, from any other part in the document. This can also be specified via custom style and css. |
| <a href='attributes/attr_data_li.html' >data-li-inset/a>   | <code>&lt;li&gt;</code>, <code>&lt;menu&gt;</code>, <code>&lt;ol&gt;</code>, <code>&lt;ul&gt;</code> | Any | Specifies the width of the list-item identifier part, allowing space for more content. This can also be specified via custom style and css. |
| <a href='attributes/attr_data_li.html' >data-li-postfix</a>   | <code>&lt;menu&gt;</code>, <code>&lt;ol&gt;</code>, <code>&lt;ul&gt;</code> | Any | Specifies a character string that will be added at the end of the list-item identifier part. This can also be specified via custom style and css. |
| <a href='attributes/attr_data_li.html' >data-li-prefix</a>   | <code>&lt;menu&gt;</code>, <code>&lt;ol&gt;</code>, <code>&lt;ul&gt;</code> | Any | Specifies a character string that will be added at the start of a list-item identifier part. This can also be specified via custom style and css. |
| <a href='attributes/attr_data_min_scale.html' >data-min-scale</a>   | <code>&lt;img&gt;</code> | Any | Explicitly modifies the minimum scaling allowed on an image at layout, that has no explicit sizing set.  |
| <a href='attributes/attr_data_outline_title.html' >data-outline-title</a>   | <code>&lt;abbr&gt;</code>, <code>&lt;cite&gt;</code>, <code>&lt;defn&gt;</code> | Any | This is supported on the elements that alter the function of title within the template, and reproduces the behavour of <code>title</code> of the other elements.  |
| <a href='attributes/attr_data_page.html' >data-page-count</a>   | <code>&lt;frame&gt;</code> | Any | Specifies the maximum number of pages from the frames source document, will be output into the final document.  |
| <a href='attributes/attr_data_page.html' >data-page-start</a>   | <code>&lt;frame&gt;</code> | Any | Specifies the starting page index from the frames source document, where output into the final document will begin. Where 1 is the first page in the document.  |
| <a href='attributes/attr_data_passthrough.html' >data-passthrough</a>   | <code>&lt;iframe&gt;</code> | Any | Flag, false by default, that can allow data from the parent document to flow through into a child template - allowing dynamic content within the child.  |
| <a href='attributes/attr_data_test.html' >data-test</a>   | <code>&lt;if&gt;</code> | *Binding Only* | A binding expression that should return a non-null or non-false value and will then show the content within the inner elements of the if elements |
| <a href='attributes/attr_data_id.html' >data-id</a>   | <code>&lt;var&gt;</code> | *Static Only* | A name of an existing or new document variable. THis variable will be set to the result of the <code>data-value</code> expression (each time the var is bound). This can then be used elsewhere in the document to show or calculate further values. |
| <a href='attributes/attr_data_value.html' >data-value</a>   | <code>&lt;num&gt;</code>, <code>&lt;var&gt;</code>, <code>&lt;data&gt;</code> | *Binding Only* | A binding expression that will be used by parent element. Either outputting it as a value in a specific format of the document, or storing it in a document variable for use later on. |



