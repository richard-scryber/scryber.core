---
layout: default
title: hr
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;hr&gt; : The Horizontal Rule Element
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
The `<hr>` element creates a horizontal line (rule) that visually separates content sections in a document. It is a block-level element used as a thematic divider.

## Usage

The `<hr>` element:
- Creates a horizontal line across the page or container
- Is a self-closing block-level element
- Has default top and bottom margins (0.5em each)
- Takes full width of its container by default
- Can be extensively styled for color, width, height, and borders
- Provides a visual break between sections
- Can serve as an alternative to page breaks for content separation

```html
<p>Content before the rule.</p>
<hr/>
<p>Content after the rule.</p>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `class` | string | CSS class name(s) for styling. |
| `style` | string | Inline CSS styles for customizing appearance. |
| `title` | string | Outline/bookmark title for the rule. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the rule. |

### CSS Style Support

The `<hr>` element supports extensive styling:

**Size and Dimensions**:
- `width` - Rule width (default is 100% of container)
- `height` - Rule thickness (default is 1pt)
- `margin` - Spacing above and below (default 0.5em top/bottom)

**Visual Styling**:
- `border` - Border style, width, and color
- `border-top`, `border-bottom` - Top and bottom border customization
- `background-color` - Fill color for the rule
- `opacity` - Transparency level

**Positioning**:
- `margin-left`, `margin-right` - Horizontal positioning/centering
- `text-align` - Center, left, or right alignment (affects the rule position)

**Page Breaking**:
- `page-break-before` - Force page break before the rule
- `page-break-after` - Force page break after the rule

---

## Notes

### Default Styling

The `<hr>` element has the following default behavior:
- **Width**: 100% of parent container (full width)
- **Thickness**: 1pt stroke
- **Color**: Black
- **Margins**: 0.5em top and bottom
- **Display**: Block level
- **Border**: None by default (uses stroke for the line)

These defaults can be overridden with CSS styling.

### Line Rendering

The horizontal rule is rendered as:
- A line component that draws from left to right
- Respects container width and positioning
- Can be styled with stroke properties for thickness and color
- Can have background fill if styled

### Versus Page Breaks

**Horizontal Rule (`<hr>`):**
- Visual divider between content sections
- Content continues on the same page (if space available)
- Provides a visible separator
- Use for thematic breaks within a page

**Page Break (`page-break-after: always`):**
- Forces content to start on a new page
- No visible element is rendered
- Use for major section breaks

```html
<!-- Visual divider (stays on same page if possible) -->
<hr/>

<!-- Force new page -->
<div style="page-break-after: always;"></div>
```

### Styling Best Practices

1. **Consistency**: Use CSS classes for consistent rule styling across documents
2. **Contrast**: Ensure sufficient contrast with background
3. **Spacing**: Adjust margins to create appropriate visual separation
4. **Thickness**: Use appropriate thickness for the context (subtle vs prominent)

---

## Examples

### Basic Horizontal Rule

```html
<p>Section one content.</p>
<hr/>
<p>Section two content.</p>
```

### Styled Rule with Color and Thickness

```html
<hr style="border: none; height: 2pt; background-color: #336699;"/>
```

### Narrow Centered Rule

```html
<hr style="width: 50%; margin: 20pt auto; border: none; height: 1pt; background-color: #ccc;"/>
```

### Double Line Effect

```html
<hr style="border: none; border-top: 1pt solid #333; border-bottom: 1pt solid #333; height: 3pt; background-color: white;"/>
```

### Thick Accent Bar

```html
<hr style="border: none; height: 5pt; background-color: #ff6347; margin: 30pt 0;"/>
```

### Dotted Divider

```html
<hr style="border: none; border-top: 2pt dotted #999; background: none;"/>
```

### Dashed Separator

```html
<hr style="border: none; border-top: 1pt dashed #666; background: none;"/>
```

### Gradient-Style Rule

```html
<hr style="border: none; height: 2pt; background: linear-gradient(to right, #fff, #336699, #fff); margin: 20pt 0;"/>
```

### Section Divider with Spacing

```html
<div style="margin: 40pt 0;">
    <hr style="border: none; height: 1pt; background-color: #e0e0e0;"/>
</div>
```

### Decorative Heavy Rule

```html
<hr style="border: 2pt solid #333; height: 4pt; background-color: #666; margin: 30pt 0;"/>
```

### Left-Aligned Short Rule

```html
<hr style="width: 100pt; margin: 15pt 0; text-align: left; border: none; height: 2pt; background-color: #336699;"/>
```

### Right-Aligned Rule

```html
<hr style="width: 200pt; margin-left: auto; margin-right: 0; border: none; height: 1pt; background-color: #999;"/>
```

### Chapter Divider

```html
<div style="text-align: center; margin: 50pt 0;">
    <hr style="width: 30%; display: inline-block; border: none; height: 1pt; background-color: #333;"/>
    <span style="margin: 0 20pt; font-size: 18pt;">***</span>
    <hr style="width: 30%; display: inline-block; border: none; height: 1pt; background-color: #333;"/>
</div>
```

### Subtle Shadow Effect

```html
<hr style="border: none; height: 1pt; background-color: #ddd; box-shadow: 0 1pt 2pt rgba(0,0,0,0.1);"/>
```

### Between Sections with Headers

```html
<h2>Introduction</h2>
<p>Introduction content here...</p>

<hr style="margin: 30pt 0; border: none; height: 1pt; background-color: #ccc;"/>

<h2>Main Content</h2>
<p>Main content here...</p>

<hr style="margin: 30pt 0; border: none; height: 1pt; background-color: #ccc;"/>

<h2>Conclusion</h2>
<p>Conclusion content here...</p>
```

### Invoice Section Separator

```html
<div style="margin: 20pt 0;">
    <h3>Items</h3>
    <!-- Item list here -->

    <hr style="margin: 15pt 0; border: none; height: 1pt; background-color: #999;"/>

    <div style="text-align: right; font-weight: bold;">
        Total: $1,234.56
    </div>
</div>
```

### Footer Divider

```html
<div style="margin-top: 100pt;">
    <hr style="border: none; height: 2pt; background-color: #333;"/>
    <div style="text-align: center; font-size: 9pt; color: #666; margin-top: 10pt;">
        Page Footer Content
    </div>
</div>
```

### Card-Style Separator

```html
<div style="border: 1pt solid #ddd; padding: 20pt; margin: 10pt 0;">
    <h3>Card Title</h3>
    <hr style="margin: 10pt -20pt; border: none; height: 1pt; background-color: #e0e0e0;"/>
    <p>Card content goes here.</p>
</div>
```

### Colored Accent Rules

```html
<style>
    .red-rule { border: none; height: 3pt; background-color: #e74c3c; margin: 20pt 0; }
    .blue-rule { border: none; height: 3pt; background-color: #3498db; margin: 20pt 0; }
    .green-rule { border: none; height: 3pt; background-color: #2ecc71; margin: 20pt 0; }
</style>

<h2>Warning Section</h2>
<hr class="red-rule"/>
<p>Warning content...</p>

<h2>Information Section</h2>
<hr class="blue-rule"/>
<p>Information content...</p>

<h2>Success Section</h2>
<hr class="green-rule"/>
<p>Success content...</p>
```

### Form Section Dividers

```html
<div>
    <h3>Personal Information</h3>
    <p>Name: _______________________</p>
    <p>Email: ______________________</p>

    <hr style="margin: 20pt 0; border: none; border-top: 1pt dashed #999;"/>

    <h3>Address Information</h3>
    <p>Street: _____________________</p>
    <p>City: _______________________</p>

    <hr style="margin: 20pt 0; border: none; border-top: 1pt dashed #999;"/>

    <h3>Additional Details</h3>
    <p>Notes: ______________________</p>
</div>
```

### Before Page Break Alternative

```html
<!-- Use HR for visual break without page break -->
<div>
    <h2>Section 1</h2>
    <p>Content for section 1...</p>

    <hr style="margin: 40pt 0; border: none; height: 2pt; background-color: #666;"/>

    <h2>Section 2</h2>
    <p>Content for section 2 continues on same page...</p>
</div>

<!-- Use page break for new page -->
<div style="page-break-after: always;">
    <h2>Section 3</h2>
    <p>Content for section 3...</p>
</div>

<h2>Section 4</h2>
<p>Section 4 starts on new page...</p>
```

### Newsletter Article Separator

```html
<article style="margin-bottom: 30pt;">
    <h2>Article Title One</h2>
    <p>Article content goes here...</p>
</article>

<hr style="width: 80%; margin: 40pt auto; border: none; height: 1pt; background-color: #999;"/>

<article style="margin-bottom: 30pt;">
    <h2>Article Title Two</h2>
    <p>Article content goes here...</p>
</article>
```

### Data Table Footer Divider

```html
<table style="width: 100%;">
    <thead>
        <tr>
            <th>Item</th>
            <th style="text-align: right;">Amount</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Item 1</td>
            <td style="text-align: right;">$100.00</td>
        </tr>
        <tr>
            <td>Item 2</td>
            <td style="text-align: right;">$200.00</td>
        </tr>
    </tbody>
</table>
<hr style="margin: 10pt 0; border: none; height: 2pt; background-color: #333;"/>
<div style="text-align: right; font-weight: bold; font-size: 12pt;">
    Total: $300.00
</div>
```

### Quote Divider

```html
<p>Regular paragraph content here.</p>

<hr style="width: 60pt; margin: 30pt auto; border: none; height: 1pt; background-color: #666;"/>

<blockquote style="text-align: center; font-style: italic; color: #666; margin: 30pt 60pt;">
    "This is an important quote that stands out from the main content."
</blockquote>

<hr style="width: 60pt; margin: 30pt auto; border: none; height: 1pt; background-color: #666;"/>

<p>Regular paragraph content continues.</p>
```

### Appendix Separator

```html
<div style="margin-top: 50pt;">
    <hr style="border: none; height: 3pt; background-color: #000; margin-bottom: 20pt;"/>
    <h1 style="text-align: center;">APPENDIX A</h1>
    <hr style="border: none; height: 3pt; background-color: #000; margin-top: 20pt; margin-bottom: 30pt;"/>
    <p>Appendix content...</p>
</div>
```

### Dynamic Rule with Data Binding

```html
<!-- With model = { sectionColor: "#336699" } -->
<hr style="border: none; height: 2pt; background-color: {{model.sectionColor}}; margin: 20pt 0;"/>
```

### Conditional Rule

```html
<!-- Only show rule if condition is met -->
<hr hidden="{{model.hideRule ? 'hidden' : ''}}"
    style="border: none; height: 1pt; background-color: #ccc; margin: 15pt 0;"/>
```

---

## See Also

- [br](/reference/htmltags/br.html) - Line break element
- [div](/reference/htmltags/div.html) - Block container element
- [p](/reference/htmltags/p.html) - Paragraph element
- [Line Component](/reference/components/line.html) - Base line component in Scryber namespace
- [Page Breaks](/reference/layout/pagebreaks.html) - Page breaking guide
- [Borders and Styling](/reference/styles/borders.html) - Border styling reference

---
