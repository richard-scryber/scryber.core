---
layout: default
title: figure
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;figure&gt; : The Figure with Caption Element

The `<figure>` element represents self-contained content, typically with a caption, that is referenced as a single unit from the main content. It is a semantic block-level element designed for encapsulating images, diagrams, code listings, illustrations, charts, or other content that can be moved away from the main flow without affecting the document's meaning.

## Usage

The `<figure>` element creates a semantic container for figures that:
- Encapsulates self-contained content like images, diagrams, illustrations, code, or charts
- Works in conjunction with `<figcaption>` to provide captions
- Displays as a block-level element with distinctive margins
- Has default margins: 1em top/bottom, 40pt left/right for visual indentation
- Takes full width of its parent container (minus margins)
- Can contain any content: images, videos, code blocks, tables, or multiple elements
- Supports all CSS styling properties for positioning, sizing, and decoration
- Can generate PDF bookmarks/outlines when a `title` attribute is set
- Ideal for creating numbered figures with captions in reports and documentation

```html
<figure>
    <img src="diagram.png" alt="System Architecture" style="width: 100%;"/>
    <figcaption>Figure 1: System Architecture Overview</figcaption>
</figure>
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
| `data-bind` | expression | Binds the element to a data context for use with templates. |
| `data-content` | expression | Dynamically sets the content of the figure from bound data. |

### CSS Style Support

The `<figure>` element supports extensive CSS styling through the `style` attribute or CSS classes:

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
- `text-align` (for centering captions and content)

**Typography** (inherited by child text):
- `font-family`, `font-size`, `font-weight`, `font-style`
- `line-height`, `letter-spacing`

---

## Notes

### Default Styling

The `<figure>` element has distinctive default styling to separate figures from main content:

**HTML figure (Scryber.Html.Components.HTMLFigure)**:
- **Top Margin**: 1em
- **Bottom Margin**: 1em
- **Left Margin**: 40pt
- **Right Margin**: 40pt
- **Display**: Block

These margins provide visual indentation and spacing that distinguishes figures from surrounding content, similar to traditional print layouts.

### Semantic Meaning and Purpose

The `<figure>` element provides semantic meaning to self-contained content:

1. **Self-Contained Content**: Content that can be moved to an appendix or sidebar without affecting document meaning
2. **Captionable Content**: Typically includes a `<figcaption>` element for description or numbering
3. **Referenced Content**: Often referenced from main text (e.g., "see Figure 1")
4. **Related Content**: Content related to main flow but not essential to understanding it

### Using figcaption

The `<figcaption>` element represents the caption for its parent `<figure>`:

- Can appear as the first or last child of `<figure>`
- Only one `<figcaption>` per `<figure>`
- Contains descriptive text, figure numbers, or attribution
- Can contain inline elements and formatting

```html
<figure>
    <figcaption>Figure 1: Before transformation</figcaption>
    <img src="before.png" alt="Before"/>
</figure>
```

### When to Use Figure

Use `<figure>` when:
- Displaying images, diagrams, or illustrations with captions
- Showing code examples with descriptive titles
- Presenting charts, graphs, or data visualizations
- Including quotations with attribution (alternative to blockquote)
- Displaying poems or verses as self-contained units
- Creating numbered figures in technical documentation

Do NOT use `<figure>` for:
- Decorative images without captions
- Images that are integral to understanding the surrounding text
- Simple content grouping (use `<div>` instead)

### Common Content Types

Typical content for figure elements includes:
- Images with captions (`<img>` + `<figcaption>`)
- Code listings with titles (`<pre>` + `<figcaption>`)
- Tables with captions (alternative to `<table><caption>`)
- Multiple related images in a grid
- SVG diagrams or illustrations
- Charts and data visualizations
- Video elements with descriptions

### Accessibility

For accessibility:
- Use descriptive `alt` text on images within figures
- Ensure captions provide meaningful descriptions
- Use semantic structure (figure + figcaption) for assistive technologies

### Class Hierarchy

In the Scryber codebase:
- `HTMLFigure` extends `HTMLDiv` extends `Panel` extends `VisualComponent`
- `HTMLFigureCaption` extends `HTMLDiv` extends `Panel` extends `VisualComponent`
- Both inherit all container functionality from `HTMLDiv`
- Figure applies distinctive margins for visual separation

---

## Examples

### Basic Figure with Image

```html
<figure>
    <img src="architecture.png" alt="System Architecture" style="width: 100%;"/>
    <figcaption>Figure 1: High-level system architecture diagram</figcaption>
</figure>
```

### Figure with Styled Caption

```html
<figure style="border: 1pt solid #ddd; padding: 15pt; margin: 20pt 0;">
    <img src="chart.png" alt="Sales Chart" style="width: 100%; display: block;"/>
    <figcaption style="margin-top: 10pt; text-align: center;
                       font-style: italic; color: #666; font-size: 10pt;">
        Figure 2: Quarterly sales performance for 2025
    </figcaption>
</figure>
```

### Code Listing as Figure

```html
<figure style="margin: 20pt 0;">
    <figcaption style="font-weight: bold; margin-bottom: 5pt;">
        Listing 1: Basic authentication implementation
    </figcaption>
    <pre style="background-color: #f5f5f5; padding: 15pt;
                border: 1pt solid #ddd; font-size: 9pt;">
function authenticate(username, password) {
    const hash = createHash(password);
    return validateCredentials(username, hash);
}
    </pre>
</figure>
```

### Multiple Images in Figure

```html
<figure style="text-align: center; page-break-inside: avoid;">
    <div style="display: inline-block; width: 48%; margin: 1%;">
        <img src="before.png" alt="Before" style="width: 100%; border: 1pt solid #ccc;"/>
        <div style="font-size: 9pt; margin-top: 5pt;">Before</div>
    </div>
    <div style="display: inline-block; width: 48%; margin: 1%;">
        <img src="after.png" alt="After" style="width: 100%; border: 1pt solid #ccc;"/>
        <div style="font-size: 9pt; margin-top: 5pt;">After</div>
    </div>
    <figcaption style="margin-top: 15pt; font-style: italic;">
        Figure 3: Comparison of interface before and after redesign
    </figcaption>
</figure>
```

### Figure with Background and Border

```html
<figure style="background-color: #f9f9f9; border-left: 4pt solid #336699;
               padding: 20pt; margin: 25pt 0;">
    <img src="workflow.png" alt="Workflow Diagram" style="width: 100%; display: block;"/>
    <figcaption style="margin-top: 15pt; font-size: 10pt; line-height: 1.5;">
        <strong>Figure 4:</strong> Complete workflow from data ingestion through
        processing to final output delivery.
    </figcaption>
</figure>
```

### Figure with Table

```html
<figure>
    <figcaption style="font-weight: bold; margin-bottom: 10pt;">
        Table 1: Performance comparison across platforms
    </figcaption>
    <table style="width: 100%; border-collapse: collapse;">
        <thead>
            <tr style="background-color: #f0f0f0;">
                <th style="border: 1pt solid #ddd; padding: 8pt;">Platform</th>
                <th style="border: 1pt solid #ddd; padding: 8pt;">Speed (ms)</th>
                <th style="border: 1pt solid #ddd; padding: 8pt;">Memory (MB)</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">Windows</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">45</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">128</td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 8pt;">Linux</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">38</td>
                <td style="border: 1pt solid #ddd; padding: 8pt;">112</td>
            </tr>
        </tbody>
    </table>
</figure>
```

### Floating Figure

```html
<figure style="float: right; width: 250pt; margin: 0 0 15pt 15pt;">
    <img src="product.png" alt="Product Photo" style="width: 100%;
         border: 1pt solid #ddd;"/>
    <figcaption style="font-size: 9pt; text-align: center;
                       margin-top: 5pt; color: #666;">
        Figure 5: Latest product model
    </figcaption>
</figure>

<p>
    Main article text flows around the floating figure, creating an
    integrated layout where the image relates to the surrounding content...
</p>
<p>
    Additional paragraphs continue to flow naturally around the figure...
</p>
```

### Figure with Citation/Attribution

```html
<figure style="margin: 25pt 60pt; text-align: center;">
    <img src="historical-photo.jpg" alt="Historical Photograph"
         style="width: 100%; max-width: 400pt;"/>
    <figcaption style="margin-top: 10pt; font-size: 9pt; line-height: 1.5;">
        <strong>Figure 6:</strong> Downtown Manhattan, circa 1920<br/>
        <span style="color: #666; font-style: italic;">
            Source: National Archives, Photo ID: 123456
        </span>
    </figcaption>
</figure>
```

### Figure with Data Binding

```html
<!-- With model.figures = [
    { number: 1, title: "Architecture", image: "arch.png", description: "System overview" },
    { number: 2, title: "Workflow", image: "flow.png", description: "Process flow" }
] -->

<template data-bind="{{model.figures}}">
    <figure style="page-break-inside: avoid; margin-bottom: 30pt;">
        <img src="{{.image}}" alt="{{.title}}" style="width: 100%;"/>
        <figcaption style="text-align: center; margin-top: 10pt; font-style: italic;">
            Figure {{.number}}: {{.description}}
        </figcaption>
    </figure>
</template>
```

### Figure for Mathematical Diagram

```html
<figure style="text-align: center; margin: 30pt 80pt;">
    <div style="background-color: #f9f9f9; padding: 30pt; border: 1pt solid #ddd;">
        <pre style="font-size: 12pt; display: inline-block; text-align: center;">
        y = mx + b

        where:
        m = slope
        b = y-intercept
        </pre>
    </div>
    <figcaption style="margin-top: 15pt; font-style: italic;">
        Equation 1: Linear function in slope-intercept form
    </figcaption>
</figure>
```

### Figure with Screenshot and Annotations

```html
<figure style="border: 2pt solid #336699; padding: 15pt; margin: 20pt 0;
               background-color: #f0f8ff;">
    <img src="screenshot.png" alt="Application Screenshot" style="width: 100%;"/>
    <figcaption style="margin-top: 12pt;">
        <strong style="color: #336699;">Figure 7: User Interface Overview</strong>
        <ul style="margin: 8pt 0 0 20pt; font-size: 9pt; line-height: 1.6;">
            <li>Navigation menu (left sidebar)</li>
            <li>Main content area (center)</li>
            <li>Properties panel (right sidebar)</li>
        </ul>
    </figcaption>
</figure>
```

### Figure in Scientific Document

```html
<figure style="page-break-inside: avoid; margin: 25pt 0;">
    <img src="experimental-results.png" alt="Experimental Results"
         style="width: 100%; border: 1pt solid #ccc;"/>
    <figcaption style="margin-top: 12pt; font-size: 10pt; line-height: 1.6;">
        <strong>Figure 8.</strong> Effect of temperature on reaction rate.
        Error bars represent standard deviation (n=3).
        Statistical significance: *p&lt;0.05, **p&lt;0.01.
    </figcaption>
</figure>
```

### Figure with Logo and Description

```html
<figure style="text-align: center; margin: 20pt auto; max-width: 300pt;">
    <img src="company-logo.png" alt="Company Logo" style="width: 200pt;"/>
    <figcaption style="margin-top: 15pt; font-size: 9pt; color: #666;
                       text-align: center; line-height: 1.5;">
        Corporate logo representing our commitment to innovation and quality.
        Designed by Studio XYZ, 2025.
    </figcaption>
</figure>
```

### Figure with Numbered Series

```html
<div style="column-count: 2; column-gap: 20pt;">
    <figure style="break-inside: avoid; margin-bottom: 20pt;
                   border: 1pt solid #e0e0e0; padding: 10pt;">
        <img src="step1.png" alt="Step 1" style="width: 100%;"/>
        <figcaption style="margin-top: 8pt; font-size: 9pt; text-align: center;">
            <strong>Step 1:</strong> Initial configuration
        </figcaption>
    </figure>

    <figure style="break-inside: avoid; margin-bottom: 20pt;
                   border: 1pt solid #e0e0e0; padding: 10pt;">
        <img src="step2.png" alt="Step 2" style="width: 100%;"/>
        <figcaption style="margin-top: 8pt; font-size: 9pt; text-align: center;">
            <strong>Step 2:</strong> Data processing
        </figcaption>
    </figure>
</div>
```

### Figure with Quote (Alternative to Blockquote)

```html
<figure style="margin: 30pt 60pt; text-align: center;">
    <blockquote style="font-size: 14pt; font-style: italic;
                       border: none; margin: 0; padding: 20pt;">
        "Innovation distinguishes between a leader and a follower."
    </blockquote>
    <figcaption style="margin-top: 15pt; font-size: 10pt;">
        — Steve Jobs, Apple Inc.
    </figcaption>
</figure>
```

### Technical Diagram Figure

```html
<figure style="background-color: #ffffff; border: 2pt solid #333;
               padding: 20pt; margin: 25pt 0; page-break-inside: avoid;">
    <div style="text-align: center;">
        <img src="network-diagram.png" alt="Network Topology"
             style="width: 100%; max-width: 500pt;"/>
    </div>
    <figcaption style="border-top: 1pt solid #ccc; padding-top: 12pt;
                       margin-top: 15pt; font-size: 10pt;">
        <strong>Figure 9: Enterprise Network Topology</strong>
        <p style="margin: 8pt 0 0 0; line-height: 1.5; color: #555;">
            This diagram illustrates the complete network architecture including
            DMZ, internal networks, and cloud integration points. Firewall rules
            and VPN connections are indicated by red and green lines respectively.
        </p>
    </figcaption>
</figure>
```

### Figure Gallery Layout

```html
<figure style="margin: 30pt 0;">
    <figcaption style="font-size: 12pt; font-weight: bold; margin-bottom: 15pt;">
        Figure 10: Product Evolution Timeline
    </figcaption>
    <div style="display: table; width: 100%; table-layout: fixed;">
        <div style="display: table-row;">
            <div style="display: table-cell; padding: 5pt; width: 25%;">
                <img src="v1.png" alt="Version 1" style="width: 100%;"/>
                <div style="text-align: center; font-size: 8pt; margin-top: 3pt;">2020</div>
            </div>
            <div style="display: table-cell; padding: 5pt; width: 25%;">
                <img src="v2.png" alt="Version 2" style="width: 100%;"/>
                <div style="text-align: center; font-size: 8pt; margin-top: 3pt;">2022</div>
            </div>
            <div style="display: table-cell; padding: 5pt; width: 25%;">
                <img src="v3.png" alt="Version 3" style="width: 100%;"/>
                <div style="text-align: center; font-size: 8pt; margin-top: 3pt;">2024</div>
            </div>
            <div style="display: table-cell; padding: 5pt; width: 25%;">
                <img src="v4.png" alt="Version 4" style="width: 100%;"/>
                <div style="text-align: center; font-size: 8pt; margin-top: 3pt;">2025</div>
            </div>
        </div>
    </div>
</figure>
```

---

## See Also

- [figcaption](/reference/htmltags/figcaption.html) - Figure caption element
- [img](/reference/htmltags/img.html) - Image element
- [table](/reference/htmltags/table.html) - Table element with native caption support
- [pre](/reference/htmltags/pre.html) - Preformatted text for code listings
- [blockquote](/reference/htmltags/blockquote.html) - Block quotation element
- [div](/reference/htmltags/div.html) - Generic block container
- [Panel Component](/reference/components/panel.html) - Base panel component in Scryber namespace

---
