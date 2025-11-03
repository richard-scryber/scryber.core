---
layout: default
title: SVG Elements
parent: Template reference
has_children: true
has_toc: false
nav_order: 6
---

# SVG Element Reference
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

Drawing vector content within a document is supported by the library using the well-known <code>svg</code> elements from the XML based markup. Although the library only supports part of the SVG specification, a significant proportion, of the static SVG capabilities are available, and most referenced svg images and sources should work without modification as long as they do not rely on filters or animations.

The svg elements can be included inline with the template, or as an <a href='/reference/htmltags/tags/embed.html'>embedded</a> set of content. Or also as a referenced <a href='/reference/htmltags/tags/img.html'>image</a> source. However, only inline or embedded content will get the full binding capability, so drawings can show dynamic values, or styles. Data will not be passed down to svg images.

```html
   <html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
      <title>Different SVG inclusion options</title>
    </head>
    <body>
      <!-- An inline SVG drawing --> 
       <svg xmlns='http://www.w3.org/2000/svg' viewport='0 0 30 30' width='100pt' height='100pt'>
          <rect x='10' y='10' width='10' height='10' />
        </svg>

        <!-- An embedded SVG drawing --> 
        <embed source='./mydrawing.svg' />

        <!-- An referenced SVG drawing as a static image --> 
        <img src='./mydrawing.svg' />

      <!-- add further content -->

    </body>
   </html>
```

---

### Commenting Content

Enclosing any content starting with a <code>&lt;!-</code> and ending with <code>--&gt;</code> will mark a comment within the document.

This content will not be processed, and be ignored. It can either be used to exclude content whilst creating a template, or adding context to the structure of a document.

---


### Case sensitivity

By default **all** elements are *case sensitive* and are all lower case, or camelCase.

---


### Filters.

The filters defined in SVG are used to modify colors and blends of objects based on their raster data, as a vector description of document content, however these are **not** supported at the moment, as it is not possible to know the raster colour image of a drawing until the final rendition by the reading application.

---

### Animations

SVG supports a range of animation elements and attributes, however these are **not** supported in the library at this time, as the document output format is generically a static format.

---

### Unsupported Element Handling

If the library encounters an element it does not understand then, by default, it will skip over the element and all its inner content, moving down to the next sibling (or the end of the document).

If the <a href='/learning/templates/conformancemode.html'>conformance mode</a> is set to <code>strict</code>, then an error will be raised so that any offending content can be tracked if there is an issue.

---

## Supported Elements

The following elements are supported in the library

---

### SVG Root Element

The root of a graphic element is always the <code>&lt;svg&gt;</code> element along with the svg namespace declaration (unless a prefix has already been declared). An SVG Root dogument can be parsed on it's own without a wrapping html template, but it cannot be output to a document without the surrounding document definition template - at the moment.


| Element  | Tag  | Description |
|---|---|---|
| <a href='tags/svg.html' >SVG Root /Container</a>   | <code>&lt;svg&gt;</code> | Marks the start of a complete svg drawing template, and encapsulates all references, metadata and content for that template. It can either be free-standing, within a document template, or within another outer SVG graphic.  |


---

### Document Elements

The document level elements sit within an svg definition and hold reference information to be used whilst outputting the final document.

| Element  | Tag  | Description |
|---|---|---|
| <a href='tags/style.html' >Styles Declarations</a>   | <code>&lt;style&gt;</code> | This defines CSS style declarations that will be used by the graphic contents, and only the graphic contents to alter the way elements look in the final output document. **NOTE**: If this is an inline or embedded svg, then styles defined higher up in the outer document will also be used where appropriate.  |
| <a href='tags/defs.html' >Shape Definitions</a>   | <code>&lt;defs&gt;</code> | The <code>defs</code> must be a single child of the <code>svg</code> element, and contains the symbols and patterns for other graphic elements to use in the final output. |


---

### Graphic Elements

The following graphic shapes will output visual content into the drawing area of the final document.

| Element  | Tag  | Description |
|---|---|---|
| <a href='tags/rect.html' >Rectangle</a>   | <code>&lt;rect&gt;</code> | Defines the a single rectangle based on x, y, and width and height, with a stroke and a fill. |
| <a href='tags/circle.html' >Circle</a>   | <code>&lt;circle&gt;</code> | Defines a single circle with a centre cx, cy, a radius r, with a stroke and a fill. |
| <a href='tags/ellipse.html' >Ellipse</a>   | <code>&lt;ellipse&gt;</code> | Defines a singe ellipse with a centre cx, cy and radii rx, ty with a stroke and a fill.  |
| <a href='tags/line.html' >Single Line</a>   | <code>&lt;line&gt;</code> | Defines a single straight line from points x1, y1 to x2, y2 with a stroke.|
| <a href='tags/polygon.html' >Mulitpoint Closed Shape</a>   | <code>&lt;polygon&gt;</code> | Defines a single straight sided closed shape with any set of cartesian coordinate <code>points</code>, with a stroke and a fill. |
| <a href='tags/polyline.html' >Multipoint Open Line</a>   | <code>&lt;polyline&gt;</code> | Defines a single straight sided open shape with any set of cartesian coordinate <code>points</code>, with a stroke |
| <a href='tags/path.html' >Graphics Path</a>   | <code>&lt;path&gt;</code> | Defines a complex path of any set of data operations for arcs, curves, lines and offsets. Has both stroke and fill options. |
| <a href='tags/image.html' >Referenced Image</a>   | <code>&lt;image&gt;</code> | References an external raster image to be rendered within the content of the graphic when the document is output. |

---

### Structural Elements

All structural elements have no specific visual appearance, but contain other elements into a single group that can be used and modified as required.

| Element  | Tag  | Description |
|---|---|---|
| <a href='tags/g.html' >Group</a>   | <code>&lt;g&gt;</code> | A container for other, inner, graphic and structural elements that will inherit any applied styling, and can also be referenced elsewhere with the <code>use</code> element  |
| <a href='tags/use.html' >Graphic Use Reference</a>   | <code>&lt;use&gt;</code> | Clones an existing graphic element (identified from the href attribute) and re-renders the elements based on the shared attributes. |
| <a href='tags/symbol.html' >Symbol Definition</a>   | <code>&lt;symbol&gt;</code> | A symbol is a container for other, inner, graphic and structural elements that by default will not be output, but the <code>use</code> of the symbol will make the content visible on the output where required. |
| <a href='tags/a.html' >Anchor Link</a>   | <code>&lt;a&gt;</code> | A container for other, inner, graphic and structural elements that will become interactive elements in. the final output document to take the reader to a desired destination. |


---

### Text Content Elements

THe following elements will show textual graphics in the output at specific positions. The style for the typeface, fill and stroke are supported.


| Element  | Tag  | Description |
|---|---|---|
| <a href='tags/text.html' >Text</a>   | <code>&lt;text&gt;</code> | A graphic block of textual content that will be output at the desired x,y (or dy,dy) position. |
| <a href='tags/tspan.html' >Inner Text Span</a>   | <code>&lt;tspan&gt;</code> | A subtext element that will adjust the position and/or style of the inner characters for the final output. |

---

### Paint Elements

The paint elements allow the complex definition of how a shape or textual graphic element should be presented on the output document.

| Element  | Tag  | Description |
|---|---|---|
| <a href='tags/linearGradient.html' >Linear Gradient Definition</a>   | <code>&lt;linearGradient&gt;</code> | Defines one or more transitions from one color to the next along a straight line path, within the boundaries of the shape or textual content that it fills. |
| <a href='tags/radialGradient.html' >Radial Gradient Definition</a>   | <code>&lt;radialGradient&gt;</code> | Defines one or more transitions from one color to the next from a centre point outwards, within the boundaries of the shape or textual content that it fills. |
| <a href='tags/pattern.html' >Pattern Definition</a>   | <code>&lt;pattern&gt;</code> | Defines a group of graphic and/or textual elements that will be used as a repeating pattern, within the boundaries of any shape or textual content that is fills. |
| <a href='tags/marker.html' >Marker Definition</a>   | <code>&lt;marker&gt;</code> | Defines a group of graphic and/or textual elements that will be used on a line or set of lines/paths at pre-determined points on the line, at a specific size and angle. |
| <a href='tags/stop.html' >Gradient Stop Definition</a>   | <code>&lt;stop&gt;</code> | Specifies a color and optionally an offset within a linear or radial gradient, that forms a transition boundary between colors. |

### Meta-Data Elements

The meta-data elements allow the inclusion of additional information about the graphic content that is not directly related to the visual appearance.

| Element  | Tag  | Description |
|---|---|---|
| <a href='tags/title.html' >Title</a>   | <code>&lt;title&gt;</code> | Defines the short descriptive title about the outer parent element. This will also be shown in the document outline for visual (graphic) elements.
| <a href='tags/desc.html' >Description</a>   | <code>&lt;desc&gt;</code> | Defines the longer description about the outer parent element. This will not be shown.



