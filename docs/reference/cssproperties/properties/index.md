---
layout: default
title: CSS Properties
parent: Template reference
parent_url: /reference/
has_children: true
has_toc: false
nav_order: 4
---

# CSS Style Property Reference
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

Within the style attribute of a visual element, or a selector in a stylesheet or group, properties alter the actual visual apperance of the element they are on, or matched to. Each property has a name and one or more values, depending on what is being set.

Within the element style

```html

  {% raw %} <div style='property-name: value [ , value2, ...]; property-name2: value [ , value2, ...]' >...</div> {% endraw %}

```

As part of a referenced style-sheet or <a href='/reference/htmltags/tags/style.html' >style</a> element.

```css
{% raw %}
selector {
   property-name: value [ , value2, ... ];
   property-name2: value [ , value2, ... ];
}
{% endraw %}
```


---

### Property Value Keywords

The style property global values (initial, revert, inherit, etc.) are **not** supported by the library, and that property will be ignored. If there has been a value set on that property previously (or with lower precedence) then it will be maintained for the content that would have had the global value set on it. See the <a href='/learning/styles/precedence.html' >CSS Selector Precedence</a> article for more information on ordering and assigning values.

---

### Unsupported Properties

If the style group, style sheet or style attribute contains an unknown or unsupported property, then that property will be skipped over. However further properties that are known will still be parsed and used during document output.

---

## Supported Style Properties

The library supports the use of the following properties with associated values. The actual output result will vary based on the element the property is applied to and the value of other properties.

---

### Element Fills.

The following properties are supported to alter the basic color of elements.


| Property  | Description |
|---|---|
| <a href='properties/color.html' >color</a>   |  Defines the fill color of any character content, and any decoration applied. |
| <a href='properties/opacity.html' >opacity</a>   | Defines the opacity of the element itself. |

---


### Element Backgrounds.

The following properties are supported to alter the background appearance of 'boxed'* elements **NOTE**: The background area is a rectangular shape including any padding are that is applied to the elelemt (and chaildren).


| Property  | Description |
|---|---|
| <a href='properties/background.html' >background</a>   | A shorthand pproperty for setting the background properties of an element. |
| <a href='properties/background-color.html' >background-color</a>   | Specifies the colour that will fill the entire background. |
| <a href='properties/background-image.html' >background-image</a>   | Specifies an image, or gradient, that will fill the entire background. |
| <a href='properties/background-repeat.html' >background-repeat</a>   | Specifies how that image, if it is smaller than the element size will repeat.  |
| <a href='properties/background-size.html' >background-size</a>   | Specifies the horizontal and vertical size of the image to repeat |
| <a href='properties/background-position.html' >background-position</a>   | Specifies both the horizontal and vertical starting postion of the image (including the repeat). |
| <a href='properties/background-position-x.html' >background-position-x</a>   | Specifies just the horizontal starting postion of the image (including the repeat). |
| <a href='properties/background-position-y.html' >background-psition-y</a>   | Specifies Just the vertical starting postion of the image (including the repeat). |

---

### Element Borders

The following properties alter the border appearance on 'boxed'* elements. **NOTE**: By default borders do not affect the spacing around an element. Thick borders will impinge on outer and inner content, if no margins or padding are applied.

| Property  | Description |
|---|---|
| <a href='properties/border.html' >border</a>   | A shorthand property of setting the style, width and color of all the borders |
| <a href='properties/border-width.html' >border-width</a>   | Sets the width of all the borders around an element. |
| <a href='properties/borde-color.html' >border-color</a>   | Sets the color of all the borders around an element.  |
| <a href='properties/border-style.html' >border-style</a>   | Sets the style (solid, dash, none) of all the borders around an element. |
| <a href='properties/border-radius.html' >border-radius</a>   | Sets the corner radius, of a border when switching between sides. Only one value is (currently) supported. |
| <a href='properties/border-top.html' >border-top</a>   | A shorthand property of setting the style, width and color of the top borders.  |
| <a href='properties/border-top-color.html' >border-top-color</a>   | Sets the color of the top border of an element. |
| <a href='properties/border-top-width.html' >border-top-width</a>   | Sets the width of the top border of an element. |
| <a href='properties/border-top-style.html' >border-top-style</a>   | Sets the style of the top border of an element. |
| <a href='properties/border-left.html' >border-left</a>   | A shorthand property of setting the style, width and color of the left borders. |
| <a href='properties/border-left-color.html' >border-left-color</a>   | Sets the color of the left border of an element. |
| <a href='properties/border-left-width.html' >border-left-width</a>   | Sets the width of the left border of an element. |
| <a href='properties/border-left-style.html' >border-left-style</a>   | Sets the style of the left border of an element. |
| <a href='properties/border-bottom.html' >border-bottom</a>   | A shorthand property of setting the style, width and color of the bottom borders. |
| <a href='properties/border-bottom-color.html' >border-bottom-color</a>   | Sets the color of the bottom border of an element. |
| <a href='properties/border-bottom-width.html' >border-bottom-width</a>   | Sets the width of the bottom border of an element. |
| <a href='properties/border-bottom-style.html' >border-bottom-style</a>   | Sets the style of the bottom border of an element. |
| <a href='properties/border-right.html' >border-top</a>   | A shorthand property of setting the style, width and color of the right borders. |
| <a href='properties/border-right-color.html' >border-right-color</a>   | Sets the color of the right border of an element. |
| <a href='properties/border-right-width.html' >border-right-width</a>   | Sets the width of the right border of an element. |
| <a href='properties/border-right-style.html' >border-right-style</a>   | Sets the style of the right border of an element. |

---

### Element Position and Size.

The following properties are supported to alter the position and size appearance of 'boxed'* elements.

| Property  | Description |
|---|---|
| <a href='properties/position.html' >position</a> | Defines the way the element is positioned within the current page at the time of layout.  |
| <a href='properties/display.html' >display</a> | Defines how the elements contents should be laid-out - in a block, inline with the flow, or just ignored. |
| <a href='properties/float.html' >float</a> | Defines the positioning of the element on the current line. And how content will move around it. |
| <a href='properties/top.html' >top</a> | Specifies the top postion for an element that can be explictly moved. |
| <a href='properties/left.html' >left</a> | Specifies the left postion for an element that can be explictly moved. |
| <a href='properties/bottom.html' >bottom</a> | Specifies the bottom postion for an element that can be explictly moved, as long as top is not set. |
| <a href='properties/right.html' >right</a> | Specifies the top postion for an element that can be explictly moved, as long as left is not set. |
| <a href='properties/width.html' >width</a>  | Specifies the width taken up for the 'boxed' element. |
| <a href='properties/height.html' >height</a> |  Specifies the height taken up for the 'boxed' element.  |
| <a href='properties/min-width.html' >min-width</a>   | Specifies the minimim width for the 'boxed' element.   |
| <a href='properties/min-height.html' >min-height</a>   | Specifies the minimim height for the 'boxed' element. |
| <a href='properties/max-width.html' >max-width</a>   | Specifies the maximum width the 'boxed' element is allowed consume on the page |
| <a href='properties/max-height.html' >max-height</a>   | Specifies the maximum width the 'boxed' element is allowed consume on the page |
| <a href='properties/transform.html' >transform</a>   | Specifies (a series of) transform operations that should be performed on the final output of the element and its children when finally output |
| <a href='properties/overflow.html' >overflow</a>   | Stipulates the action (or non-action) that will be taken if the contents of the element exceed the available size of this container element.  |

---

### Element Spacing

The following properties manage the spacing in and around elements. The border rectangle is the boundary between the margins and padding and is where a border around the element would be shown if one is specified. The content rectangle is where the flowing children of the element (if any) are going to be positioned. **NOTE**: By default, and by design, the library does not collapse any margin spacing, nor does it allocate space for the width of a border.

| Property  | Description |
|---|---|
| <a href='properties/padding.html' >padding</a>   | A shorthand property of setting the space between the border rectangle and the rectangle where the content is displayed for a *boxed* element. |
| <a href='properties/padding-top.html' >padding-top</a>   | Sets the space between the top of the border rectangle and the start of any content for a *boxed* element. |
| <a href='properties/padding-left.html' >padding-left</a>   | Sets the space between the left of the border rectangle and the start of any content for a *boxed* element. |
| <a href='properties/padding-bottom.html' >padding-bottom</a>   | Sets the space between the bottom of the border rectangle and the start of any content for a *boxed* element. |
| <a href='properties/padding-right.html' >padding-right</a>   | Sets the space between the right of the border rectangle and the start of any content for a *boxed* element. |
| <a href='properties/padding-inline.html' >padding-inline</a>   |  Sets the start and end padding of an *inline* element to either one value or two values. |
| <a href='properties/padding-inline-start.html' >padding-inline-start</a>   | Sets the start padding of an *inline* element to a specific value. |
| <a href='properties/padding-inline-end.html' >padding-inline-end</a>   | Sets the end padding of an *inline* element to a specific value. |
| <a href='properties/margin.html' >margin</a>   | A shorthand property of setting the space between other sibling content and the border rectangle of any inner the content for a *boxed* element. |
| <a href='properties/margin-top.html' >margin-top</a>   |  Sets the space between other content and the top of the border rectangle of any inner content for a *boxed* element. |
| <a href='properties/margin-left.html' >margin-left</a>   | Sets the space between other content and the left of the border rectangle of any inner content for a *boxed* element. |
| <a href='properties/margin-bottom.html' >margin-bottom</a>   | Sets the space between the bottom of the border rectangle and other content for a *boxed* element. |
| <a href='properties/margin-right.html' >margin-right</a>   | Sets the space between the right of the border rectangle and other content for a *boxed* element. |
| <a href='properties/margin-inline.html' >margin-inline</a>   | Sets the start and end margins of an *inline* element to either one value or two values. |
| <a href='properties/margin-inline-start.html' >margin-inline-start</a>   | Sets the start margins of an *inline* element to a specific value. |
| <a href='properties/margin-inline-end.html' >margin-inline-end</a>   | Sets the start margins of an *inline* element to a specific value. |

---

### Pages and Columns

The following properties control the page sizes, columns and breaks within. The <a href='/reference/cssselectors/rules/page.html'>'page'</a> at-rule allows definition of custom page sizes, overall and in-groups.

| Property  | Description |
|---|---|
| <a href='properties/column-count.html' >column-count</a>   | Specifies the number of columns within a block |
| <a href='properties/column-gap.html' >column-gap</a>   | Specifies the alley width between columns |
| <a href='properties/column-width.html' >column-width</a>   | Specifies ideal column widths for content within an container so they are not smaller than the desired width but fill the container's width  |
| <a href='properties/break-inside.html' >break-inside</a>   | Setting to avoid, will ensure that the content within is in a single region, column and/or page where possible. |
| <a href='properties/break-after.html' >break-after</a>   | Setting to always, will ensure that any following content begins on a new region, column and/or page where possible.  |
| <a href='properties/break-before.html' >break-before</a>   | Setting to always, will ensure that the content begins on a new region, column and/or page where possible. |
| <a href='properties/page-break-inside.html' >page-break-inside</a>   | Setting to avoid, will ensure that the content within is in a single page where possible. |
| <a href='properties/page-break-after.html' >page-break-after</a>   | Setting to always, will ensure that any following content begins on a new page where possible. |
| <a href='properties/page-break-before.html' >page-break-before</a>   | Setting to always, will ensure that the content begins on a new page where possible. |
| <a href='properties/page.html' >page</a>   | Specifies the name of the page group style from an at-rule, that should be used to define the size and layout af any pages within this section. |

---

### Fonts and Type Faces

The following properties control the font that any text will use, including families and styles, and remote font registration. The <a href='/reference/cssselectors/rules/font-face.html'>'font-face'</a> at-rule allows definition of custom fonts, that can be used here.

| Property  | Description |
|---|---|
| <a href='properties/font.html' >font</a>   | Shorthand property for setting the font family, style, weight and size. |
| <a href='properties/font-style.html' >font-style</a>   | Specifies the style of the font-face in the family. If the style is not available, then the library will fallback to an available style within the family. |
| <a href='properties/font-family.html' >font-family</a>   |  Specifies one or more family names to use, in order of preference. The library has built in fonts for the standard 'serif', 'sans-serif' and 'monospace' fonts, and will always be available. |
| <a href='properties/font-weight.html' >font-weight</a>   | Specifies the numeric weight (100 - 900), or 'normal' or 'bold'. If the actual font-weight is not available, then the library will fallback to the nearest available weight. |
| <a href='properties/font-size.html' >font-size</a>   | Specifies the numeric size of the text to display when the document is output. This can be an absolute unit or a relative unit or percentage. The absolute (x-large, small etc.) and relative (larger / smaller) are supported. |

<!-- | <a href='properties/font-display.html' >font-display</a>   |  |
| <a href='properties/font-stretch.html' >font-stretch</a>   |  | -->
<!-- | <a href='properties/src.html' >src</a>   |  Used by the font-face at rule | -->

--- 

### Text and Character Adjustment

The following properties control the way any text within the element will be output.

| Property  | Description |
|---|---|
| <a href='properties/text-align.html' >text-align</a>   | Specifies the horizontal alignment of content on a line, or content within a block with a specific width (inc. full width) or a table cell. |
| <a href='properties/vertical-align.html' >vertical-align</a>   | Specifies the vertical alignment of content on a line, or content within a block with a specific height or a table cell |
| <a href='properties/line-height.html' >line-height</a>   | Specifies the leading of a line which can either be a relative or an absolute value.  |
| <a href='properties/text-decoration.html' >text-decoration</a>   | Specifies any decoration on the text - A line above, below or in the middle of the characters is supported. |
| <a href='properties/text-decoration.html' >text-decoration-line</a>   | As per text-decoration - Specifies any decoration on the text - A line above, below or in the middle of the characters is supported. |
| <a href='properties/letter-spacing.html' >letter-spacing</a>   | Defines an adjustment to the space applied between letters on a word, phrase or run of text. O is the standard value. |
| <a href='properties/hyphens.html' >hyphens</a>   | Sets the model for hypenation, auto or none. NOTE: The library default is none, the manual option is considered equivalent to none. |
| <a href='properties/hyphenate-limit-chars.html' >hyphenate-limit-chars</a>   | Specifies upt to 3 values representing the minimum word length to hyphenate and then the minimum left and or right length of characters, allowed before or after a hyphen. |
| <a href='properties/hyphenate-character.html' >hyphenate-character</a>   | Specifies the character to use for a hyphen. Default is '-' |
| <a href='properties/word-spacing.html' >word-spacing</a>   | Defines an adjustment to the space applied between words on a phrase or run of text. O is the standard value. |
| <a href='properties/white-space.html' >white-space</a>   | Specifies if white space (tags, returns, spaces, etc.) should be preserved in the output document. |


---

### Lists, Counters and Content

The following properties manage counter values, updating and displaying dynamic incrementing content. The <a href='/reference/cssselectors/rules/counter-style.html'>'counter-style'</a> at-rule allows definition of custom fonts, that can be used here.

| Property  | Description |
|---|---|
| <a href='properties/content.html' >content</a>   | The content propety is commonly used to add some textual content before (CSS <code>::before</code> selector) or after (CSS <code>::after</code> selector), the element that the style is applied to.  |
| <a href='properties/counter-reset.html' >counter-reset</a>   | Accepts a space separated list of names of 'counter-identies' and optionally a following number for an initial value to reset to. If not provided, then the default for that counter will be used  |
| <a href='properties/counter-increment.html' >counter-increment</a>   | Accepts a space separated list of names of 'counter-identites' and optionally a following number for teh value to increment by. If a number is not provided then the counter is incremented by 1 |
| <a href='properties/list-style.html' >list-style</a>   | Specifies the format of the prefixed value that will be displayed against a list item - e.g. lower-roman, along with options for inside or outside (not currently supported) |
| <a href='properties/list-style-type.html' >list-style-type</a>   | Expicity specifies the format of the prefixed value that will be displayed against a list item - e.g. lower-roman, or none if no value should be shown. |



---

* 'boxed' encapsulate all their content within a rectangular 'box', e.g. block, or inline-block.