---
layout: default
title: blockquote
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;blockquote&gt; : The Block Quotation Element
{: .no_toc }

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---

## Summary

The `<blockquote>` element represents a section that is quoted from another source. It is a semantic block-level element designed for displaying extended quotations with appropriate visual styling to distinguish quoted content from surrounding text.

---

## Usage

The `<blockquote>` element creates a block-level quotation container that:
- Represents extended quotations from external sources
- Displays with distinctive left border (2pt solid gray by default)
- Has italic font styling by default to distinguish quoted text
- Includes left padding (5pt) and vertical margins (5pt top and bottom)
- Takes full width of its parent container
- Can contain any block or inline elements including paragraphs, headings, and nested quotes
- Supports the `cite` attribute for referencing the source URL
- Can generate PDF bookmarks/outlines when a `title` attribute is set

```html
<blockquote>
    <p>The best way to predict the future is to invent it.</p>
    <cite>— Alan Kay</cite>
</blockquote>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title in the PDF document. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-content` | expression | Dynamically sets the content of the address element from bound data. |
| `data-content-type` | Mime Type | Specifies the type of bound content fragment - XHTML; HTML; Markdown. |
| `data-content-action` | Replace, Append, Prepend | Specifies the action to take when binding elements with existing inner content. |

---

## CSS Style Support

The `<blockquote>` element supports extensive CSS styling through the `style` attribute or CSS classes:

**Box Model**:
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`
- `border`, `border-width`, `border-color`, `border-style`
- `border-top`, `border-right`, `border-bottom`, `border-left`

**Positioning**:
- `position`: `static`, `relative`, `absolute`
- `display`: `block`, `inline`, `inline-block`, `none`
- `float`: `left`, `right`, `none`
- `clear`: `both`, `left`, `right`, `none`

**Layout**:
- `page-break-before`, `page-break-after`, `page-break-inside`

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `color` (text color)
- `opacity`

**Typography** (inherited by child text):
- `font-family`, `font-size`, `font-weight`, `font-style`
- `text-align`, `text-decoration`, `text-transform`
- `line-height`, `letter-spacing`

---

## Notes

### Default Styling

The `<blockquote>` element has distinctive default styling to visually separate quoted content:

**HTML blockquote :
- **Left Border**: 2pt solid gray
- **Left Padding**: 5pt
- **Top Margin**: 5pt
- **Bottom Margin**: 5pt
- **Display**: Block


The HTML blockquote overrides some of the base styles to provide a more modern appearance with a left border accent instead of all-around margins.

### Semantic Meaning and Purpose

The `<blockquote>` element provides semantic meaning to your document:

1. **Quotations**: Use for extended quotations from books, articles, speeches, or other sources
2. **Attribution**: Typically contains citation information via nested `<footer>` or `<cite>` elements
3. **Visual Distinction**: The default styling helps readers identify quoted material
4. **Accessibility**: Screen readers and assistive technologies recognize blockquotes as quoted content

### When to Use Blockquote vs. Paragraph

Use `<blockquote>` when:
- Quoting extended passages from external sources
- Citing references, testimonials, or reviews
- Displaying pull quotes or featured quotes in articles
- Representing dialogue or conversations in formal documents

Use `<p>` when:
- Writing original content
- Displaying brief inline quotes (consider `<q>` for inline quotes)
- Content is not a quotation from another source

### Nested Blockquotes

Blockquotes can be nested to represent quotes within quotes, such as in email threads or nested citations:

```html
<blockquote>
    <p>Original quote</p>
    <blockquote>
        <p>Nested quote</p>
    </blockquote>
</blockquote>
```

### Citation Information

While HTML5 supports the `cite` attribute for URLs, visual citation information is typically included within the blockquote content using `<footer>` or `<cite>` elements for better visibility.

---

## Class Hierarchy

```c#
Scryber.Html.Components.HTMLBlockQuote, Scryber.Components
```

In the library codebase:
- `HTMLBlockQuote` extends `BlockQuote` extends `Panel` extends `VisualComponent`
- The HTML version overrides the base style to provide a left border instead of all-around margins
- Both versions apply italic font styling to distinguish quoted text

```c#
using Scryber.Components;
using Scryber.HTML.Components;

var bquote = new HTMLBlockQuote();
bquote.ID = "notes";
var p = new HTMLParagraph();
p.Contents.Add("NOTE: This content will with a border on the left, and flow with the content");
bquote.Contents.Add(q);

//page.Contents.Add(bquote);
```

---

## Examples

### Basic Blockquote

```html
<blockquote>
    <p>The best way to predict the future is to invent it.</p>
</blockquote>
```

### Blockquote with Attribution

```html
<blockquote>
    <p>Two things are infinite: the universe and human stupidity;
    and I'm not sure about the universe.</p>
    <footer style="text-align: right; margin-top: 10pt; font-style: normal;">
        — Albert Einstein
    </footer>
</blockquote>
```

### Blockquote with Custom Border Color

```html
<blockquote style="border-left-color: #336699; border-left-width: 4pt;">
    <p>Innovation distinguishes between a leader and a follower.</p>
    <footer style="font-style: normal; color: #666;">— Steve Jobs</footer>
</blockquote>
```

### Blockquote with Background

```html
<blockquote style="background-color: #f9f9f9; padding: 15pt;
                   border-left: 4pt solid #e67e22;">
    <p style="margin: 0;">
        The only way to do great work is to love what you do.
        If you haven't found it yet, keep looking. Don't settle.
    </p>
    <footer style="margin-top: 10pt; font-style: normal; color: #555;">
        — Steve Jobs, Stanford Commencement Speech
    </footer>
</blockquote>
```

### Nested Blockquote (Email Thread Style)

```html
<blockquote>
    <p><strong>John:</strong> I agree with your proposal.</p>
    <blockquote style="border-left-color: #999;">
        <p><strong>Jane:</strong> We should move forward with the project.</p>
        <blockquote style="border-left-color: #ccc;">
            <p><strong>Alice:</strong> What's the timeline?</p>
        </blockquote>
    </blockquote>
</blockquote>
```

### Blockquote for Testimonial

```html
<blockquote style="border-left: none; border-top: 4pt solid #336699;
                   padding-top: 15pt; margin: 20pt 40pt;">
    <p style="font-size: 14pt; line-height: 1.6;">
        Working with this team has been an absolute pleasure.
        Their attention to detail and commitment to quality is outstanding.
    </p>
    <footer style="margin-top: 15pt; font-style: normal;">
        <strong style="color: #336699;">Sarah Johnson</strong><br/>
        <span style="color: #666; font-size: 10pt;">CEO, Tech Innovations Inc.</span>
    </footer>
</blockquote>
```

### Blockquote with Large Opening Quote

```html
<blockquote style="position: relative; padding-left: 50pt;">
    <div style="position: absolute; left: 10pt; top: -10pt;
                font-size: 48pt; color: #ccc; line-height: 1;">
        "
    </div>
    <p>
        Success is not final, failure is not fatal: it is the courage
        to continue that counts.
    </p>
    <footer style="font-style: normal; color: #666;">
        — Winston Churchill
    </footer>
</blockquote>
```

### Blockquote with Multiple Paragraphs

```html
<blockquote>
    <p>
        It is not the critic who counts; not the man who points out how
        the strong man stumbles, or where the doer of deeds could have
        done them better.
    </p>
    <p>
        The credit belongs to the man who is actually in the arena, whose
        face is marred by dust and sweat and blood; who strives valiantly.
    </p>
    <footer style="font-style: normal; margin-top: 10pt;">
        — Theodore Roosevelt, <cite>Citizenship in a Republic</cite>
    </footer>
</blockquote>
```

### Blockquote for Legal Text

```html
<blockquote style="font-style: normal; background-color: #fffbf0;
                   padding: 15pt; border: 1pt solid #e6d5b8;">
    <p style="font-size: 10pt; line-height: 1.5;">
        <strong>Section 2.1:</strong> The Licensee agrees to use the
        software in accordance with the terms set forth in this agreement.
        Any unauthorized use, reproduction, or distribution is strictly prohibited.
    </p>
    <footer style="margin-top: 10pt; font-size: 9pt; color: #666;">
        Source: Software License Agreement, Version 2.0
    </footer>
</blockquote>
```

### Blockquote with Data Binding

```html
{% raw %}<!-- With model.testimonials = [
    { quote: "Excellent service!", author: "John Doe", company: "ABC Corp" },
    { quote: "Highly recommended!", author: "Jane Smith", company: "XYZ Ltd" }
] -->

<template data-bind="{{model.testimonials}}">
    <blockquote style="margin-bottom: 20pt; page-break-inside: avoid;">
        <p>{{.quote}}</p>
        <footer style="font-style: normal; color: #666;">
            — <strong>{{.author}}</strong>, {{.company}}
        </footer>
    </blockquote>
</template>{% endraw %}
```

### Blockquote for Pull Quote

```html
<div style="float: right; width: 200pt; margin: 0 0 10pt 15pt;">
    <blockquote style="border-left: none; border-top: 3pt solid #336699;
                       border-bottom: 3pt solid #336699; padding: 15pt 0;
                       margin: 0; text-align: center;">
        <p style="font-size: 16pt; font-weight: bold; color: #336699;
                  line-height: 1.3; margin: 0;">
            Innovation is the key to future success
        </p>
    </blockquote>
</div>

<p>
    Main article text flows around the pull quote, creating an engaging
    layout that highlights key messages while maintaining readability...
</p>
```

### Blockquote with Icon/Symbol

```html
<blockquote style="padding-left: 40pt; position: relative;">
    <div style="position: absolute; left: 5pt; top: 0;
                font-size: 24pt; color: #336699;">
        i
    </div>
    <p>
        <strong>Important Note:</strong> All submissions must be received
        by the deadline to be considered. Late submissions will not be accepted.
    </p>
</blockquote>
```

### Blockquote for Academic Citation

```html
<blockquote style="font-style: normal; font-size: 11pt;
                   margin: 15pt 30pt; line-height: 1.6;">
    <p>
        The study revealed significant correlations between regular exercise
        and cognitive function in older adults, with participants showing
        improved memory retention and processing speed after 12 weeks of
        moderate physical activity (N=156, p&lt;0.01).
    </p>
    <footer style="margin-top: 10pt; font-size: 10pt; color: #666;">
        <cite>Johnson, M. & Smith, K. (2024). Exercise and Cognitive Function.
        Journal of Health Psychology, 45(3), 234-256.</cite>
    </footer>
</blockquote>
```

### Blockquote with Colored Background and Shadow Effect

```html
<blockquote style="background: linear-gradient(to right, #f0f8ff, #ffffff);
                   border-left: 5pt solid #336699; padding: 20pt;
                   margin: 20pt 0; border-radius: 5pt;">
    <p style="font-size: 13pt; color: #333; margin: 0;">
        Quality is not an act, it is a habit.
    </p>
    <footer style="margin-top: 12pt; font-style: normal;
                   font-size: 11pt; color: #555;">
        — Aristotle
    </footer>
</blockquote>
```

### Blockquote in Multi-Column Layout

```html
<div style="column-count: 2; column-gap: 20pt;">
    <p>
        Regular article text that flows in columns...
    </p>

    <blockquote style="break-inside: avoid; font-size: 11pt;
                       margin: 10pt 0;">
        <p>This quote remains intact within a single column and won't
        be split across columns.</p>
        <footer style="font-style: normal; color: #666;">— Author Name</footer>
    </blockquote>

    <p>
        More article text continues...
    </p>
</div>
```

### Blockquote with Dynamic Styling

```html
{% raw %}<!-- With model = { quoteStyle: "professional" } -->

<blockquote style="border-left-color: {{model.quoteColor}};
                   border-left-width: {{model.borderWidth}}pt;">
    <p>{{model.quoteText}}</p>
    <footer style="font-style: normal;">— {{model.attribution}}</footer>
</blockquote>{% endraw %}
```

---

## See Also

- [p](html_p_element.html) - Paragraph element
- [cite](html_cite_element.html) - Citation element for referencing sources
- [q](html_q_element.html) - Inline quotation element
- [div](html_div_element.html) - Generic block container
- [footer](html_header_footer_element.html) - Footer element for attribution

---
