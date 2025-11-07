---
layout: default
title: details and summary
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;details&gt; and &lt;summary&gt; : Collapsible Content Sections

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

The `<details>` and `<summary>` elements create collapsible content sections. In static PDF output, these elements render in either expanded or collapsed state based on the `open` attribute. The `<summary>` provides a visible heading, while the remaining content inside `<details>` can be shown or hidden.

**NOTE:** It is expected that this element, along with `abbr`, `cite`, `defn` will use the interactive features for comments and call outs in output documents, <u>in future versions</u>>.

---

## Usage

The `<details>` element creates a disclosure widget that:
- Renders as a static expanded or collapsed section in PDF (not interactive)
- Contains a `<summary>` element as the heading/label
- Shows or hides additional content based on the `open` attribute
- Uses full width by default (block-level element)
- Can be styled to create visual hierarchy and organization
- Automatically positions the `<summary>` first, regardless of order
- Supports nested details for complex document structures
- Works with data binding for dynamic content

```html
<!-- Expanded by default -->
<details open="open">
    <summary>Click to expand</summary>
    <p>This content is visible in the output document</p>
</details>

<!-- Collapsed (content hidden) -->
<details>
    <summary>Collapsed Section</summary>
    <p>This content will not appear in the output document</p>
</details>
```

---

## Supported Attributes

### &lt;details&gt; Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the section. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |
| `open` | string | Controls expanded state. Set to "open", "true", or "" to show content. Set to "false" or "closed" to hide content. |

### &lt;summary&gt; Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the summary. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-content` | expression | Dynamically sets the content from bound data. |
| `data-content-type` | Mime Type | Specifies the type of bound content fragment - XHTML; HTML; Markdown. |
| `data-content-action` | Replace, Append, Prepend | Specifies the action to take when binding elements with existing inner content. |

---

## CSS Style Support

Both `<details>` and `<summary>` support extensive CSS styling:

**Box Model**:
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`
- `border`, `border-width`, `border-color`, `border-style`, `border-radius`

**Positioning**:
- `position`: `static`, `relative`, `absolute`
- `display`: `block`, `inline-block`, `none`
- `float`: `left`, `right`, `none`

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `color` (text color)
- `opacity`

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `text-align`, `text-decoration`, `text-transform`
- `line-height`, `letter-spacing`

---

## Notes

### PDF Static Rendering

In PDF output, the `<details>` element is **not interactive**. Unlike web browsers where users can click to expand/collapse:

1. **Open State**: When `open` attribute is present and set to a empty string or a value that isn't "false" or "closed":
   - Both `<summary>` and content are rendered
   - Content is fully visible in the PDF

2. **Closed State**: When `open` is "false" or "closed", or the attribute is absent:
   - Only the `<summary>` is rendered
   - All other content within `<details>` is hidden (not rendered in PDF)

```html
<!-- These will show content -->
 <details open="">...</details>
<details open="open">...</details>
<details open="true">...</details>
<details open="yes">...</details>

<!-- These will hide content -->
<details>...</details>
<details open="false">...</details>
<details open="closed">...</details>
```

### Summary Element Behavior

The `<summary>` element:
- Is automatically moved to the first position within `<details>` during layout
- If multiple `<summary>` elements exist, only the first is used (warning logged)
- If no `<summary>` is provided, content flows normally
- Uses full width by default (block-level)
- Can be styled independently with its own classes and styles

### Use Cases for PDF

While not interactive in PDF, these elements are useful for:
1. **Document Versions**: Generate expanded PDFs with all content, or collapsed PDFs showing only summaries
2. **Dynamic Content**: Use data binding to conditionally show/hide sections
3. **FAQ Documents**: Create expandable FAQ sections where some answers are shown, others hidden
4. **Report Sections**: Hide detailed data while showing summaries
5. **Conditional Rendering**: Control what content appears based on template parameters

---

## Class Hierarchy

In the Scryber codebase:
- `HTMLDetails` extends `Panel` - Block-level container
- `HTMLDetailsSummary` extends `Panel` - Block-level heading
- Both inherit all properties from `Panel` and `VisualComponent`
- Both use full width by default


---

## Examples

### Basic Details - Expanded

```html
<details open="open">
    <summary>System Requirements</summary>
    <ul>
        <li>Operating System: Windows 10 or later</li>
        <li>Memory: 8GB RAM minimum</li>
        <li>Storage: 50GB available space</li>
        <li>Processor: 2.0 GHz dual-core</li>
    </ul>
</details>
```

### Basic Details - Collapsed

```html
<details>
    <summary>Advanced Configuration</summary>
    <p>This content will not appear in the PDF because the details element is not open.</p>
    <p>Use this pattern to create summary-only PDF views.</p>
</details>
```

### Styled Details with Visual Indicators

```html
<style>
    .faq-details {
        border: 1pt solid #ddd;
        border-radius: 4pt;
        margin: 10pt 0;
        overflow: hidden;
    }

    .faq-summary {
        background: linear-gradient(to bottom, #f9f9f9, #e9e9e9);
        padding: 12pt 15pt;
        font-weight: bold;
        font-size: 12pt;
        border-bottom: 1pt solid #ddd;
    }

    .faq-summary::before {
        content: "▶ ";
        color: #666;
        margin-right: 8pt;
    }

    .faq-content {
        padding: 15pt;
        background-color: #fff;
    }
</style>

<details class="faq-details" open="open">
    <summary class="faq-summary">What is Scryber?</summary>
    <div class="faq-content">
        <p>Scryber is an open-source .NET library for creating PDF documents
        using HTML and CSS templates.</p>
    </div>
</details>

<details class="faq-details">
    <summary class="faq-summary">How do I install Scryber?</summary>
    <div class="faq-content">
        <p>This answer is hidden in the PDF output.</p>
    </div>
</details>
```

### Nested Details Sections

```html
<details open="open" style="border: 1pt solid #333; padding: 10pt; margin: 10pt 0;">
    <summary style="font-size: 14pt; font-weight: bold; color: #336699;">
        Chapter 1: Introduction
    </summary>

    <p style="margin: 10pt 0;">This chapter covers the fundamentals...</p>

    <details open="open" style="margin-left: 20pt; border: 1pt solid #999; padding: 8pt; margin-top: 10pt;">
        <summary style="font-size: 12pt; font-weight: bold; color: #666;">
            Section 1.1: Overview
        </summary>
        <p style="margin: 8pt 0;">Detailed overview content here...</p>
    </details>

    <details style="margin-left: 20pt; border: 1pt solid #999; padding: 8pt; margin-top: 10pt;">
        <summary style="font-size: 12pt; font-weight: bold; color: #666;">
            Section 1.2: Advanced Topics
        </summary>
        <p>This section is collapsed and won't appear in the PDF.</p>
    </details>
</details>
```

### Details with Icons and Colors

```html
<style>
    .info-details { border-left: 4pt solid #2196F3; }
    .success-details { border-left: 4pt solid #4CAF50; }
    .warning-details { border-left: 4pt solid #FF9800; }
    .error-details { border-left: 4pt solid #F44336; }

    .icon-summary {
        padding: 12pt;
        background-color: #f5f5f5;
        font-weight: 600;
    }

    .detail-content {
        padding: 12pt;
        background-color: #fafafa;
    }
</style>

<details class="info-details" open="open">
    <summary class="icon-summary">ℹ Information</summary>
    <div class="detail-content">
        <p>This is informational content with a blue accent.</p>
    </div>
</details>

<details class="success-details" open="open">
    <summary class="icon-summary">✓ Success Notice</summary>
    <div class="detail-content">
        <p>Operation completed successfully with a green accent.</p>
    </div>
</details>

<details class="warning-details">
    <summary class="icon-summary">⚠ Warning</summary>
    <div class="detail-content">
        <p>This warning is collapsed and won't show in PDF.</p>
    </div>
</details>
```

### Data-Bound Details

```html
{% raw %}<!-- With model = { showDetails: true, title: "Product Info", description: "..." } -->
<details open="{{model.showDetails ? 'open' : 'closed'}}">
    <summary style="font-weight: bold; padding: 10pt; background-color: #e8e8e8;">
        {{model.title}}
    </summary>
    <div style="padding: 10pt;">
        <p>{{model.description}}</p>
    </div>
</details>{% endraw %}
```

### Repeating Details from Collection

```html
{% raw %}<!-- With model.faqs = [{q: "Question 1?", a: "Answer 1", open: true}, {q: "Question 2?", a: "Answer 2", open: false}] -->
<template data-bind="{{model.faqs}}">
    <details open="{{.open ? 'open' : 'closed'}}"
             style="border: 1pt solid #ddd; margin: 8pt 0; border-radius: 4pt;">
        <summary style="padding: 10pt; background-color: #f9f9f9; font-weight: bold;">
            {{.q}}
        </summary>
        <div style="padding: 10pt; background-color: #fff;">
            <p>{{.a}}</p>
        </div>
    </details>
</template>{% endraw %}
```

### Card-Style Details

```html
<details open="open" style="
    border: 1pt solid #e0e0e0;
    border-radius: 8pt;
    box-shadow: 0 2pt 4pt rgba(0,0,0,0.1);
    margin: 15pt 0;
    overflow: hidden;">

    <summary style="
        padding: 15pt 20pt;
        background: linear-gradient(to right, #667eea, #764ba2);
        color: white;
        font-size: 14pt;
        font-weight: bold;
        cursor: pointer;">
        Premium Features
    </summary>

    <div style="padding: 20pt;">
        <ul style="margin: 0; padding-left: 20pt;">
            <li>Advanced analytics and reporting</li>
            <li>Priority customer support</li>
            <li>Custom branding options</li>
            <li>API access</li>
            <li>SSO integration</li>
        </ul>
    </div>
</details>
```

### Details with Table Content

```html
<details open="open" style="border: 2pt solid #336699; margin: 15pt 0;">
    <summary style="padding: 12pt; background-color: #336699; color: white; font-weight: bold; font-size: 13pt;">
        Pricing Tiers
    </summary>

    <table style="width: 100%; border-collapse: collapse;">
        <thead>
            <tr style="background-color: #f0f0f0;">
                <th style="padding: 10pt; border: 1pt solid #ddd; text-align: left;">Tier</th>
                <th style="padding: 10pt; border: 1pt solid #ddd; text-align: right;">Price</th>
                <th style="padding: 10pt; border: 1pt solid #ddd; text-align: center;">Users</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="padding: 8pt; border: 1pt solid #ddd;">Basic</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: right;">$9.99</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: center;">1-5</td>
            </tr>
            <tr style="background-color: #f9f9f9;">
                <td style="padding: 8pt; border: 1pt solid #ddd;">Professional</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: right;">$29.99</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: center;">6-20</td>
            </tr>
            <tr>
                <td style="padding: 8pt; border: 1pt solid #ddd;">Enterprise</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: right;">$99.99</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: center;">Unlimited</td>
            </tr>
        </tbody>
    </table>
</details>
```

### Accordion-Style Multiple Details

```html
<style>
    .accordion-item {
        border: 1pt solid #ccc;
        border-bottom: none;
    }

    .accordion-summary {
        padding: 12pt 15pt;
        background-color: #f7f7f7;
        font-weight: 600;
        border-left: 4pt solid #666;
    }

    .accordion-content {
        padding: 15pt;
        border-left: 4pt solid transparent;
    }
</style>

<div>
    <details class="accordion-item" open="open">
        <summary class="accordion-summary">Getting Started</summary>
        <div class="accordion-content">
            <p>Follow these steps to begin using the application...</p>
        </div>
    </details>

    <details class="accordion-item">
        <summary class="accordion-summary">Configuration</summary>
        <div class="accordion-content">
            <p>Configure your settings here... (hidden in PDF)</p>
        </div>
    </details>

    <details class="accordion-item">
        <summary class="accordion-summary">Advanced Usage</summary>
        <div class="accordion-content">
            <p>Advanced features and techniques... (hidden in PDF)</p>
        </div>
    </details>
</div>
```

### Details with Code Example

```html
<details open="open" style="
    background-color: #f6f8fa;
    border: 1pt solid #d1d5da;
    border-radius: 6pt;
    margin: 15pt 0;">

    <summary style="
        padding: 12pt;
        background-color: #e8eaed;
        font-family: 'Courier New', monospace;
        font-weight: bold;
        border-bottom: 1pt solid #d1d5da;">
        Example Code Implementation
    </summary>

    <pre style="
        margin: 0;
        padding: 15pt;
        background-color: #fff;
        border-radius: 0 0 6pt 6pt;
        overflow: auto;
        font-family: 'Courier New', monospace;
        font-size: 9pt;
        line-height: 1.4;">
public class DocumentGenerator
{
    public void Generate()
    {
        // Initialize PDF document
        var doc = new Document();
        // Add content here
        doc.Save("output.pdf");
    }
}</pre>
</details>
```

### Timeline with Details

```html
<style>
    .timeline-item {
        margin-left: 30pt;
        border-left: 2pt solid #336699;
        padding-left: 15pt;
        margin-bottom: 15pt;
        position: relative;
    }

    .timeline-item::before {
        content: "●";
        position: absolute;
        left: -7pt;
        color: #336699;
        font-size: 12pt;
    }

    .timeline-summary {
        font-weight: bold;
        font-size: 11pt;
        color: #336699;
        margin-bottom: 5pt;
    }

    .timeline-content {
        color: #666;
        margin-top: 5pt;
    }
</style>

<div>
    <details class="timeline-item" open="open">
        <summary class="timeline-summary">2024-01: Project Initialization</summary>
        <div class="timeline-content">
            <p>Project requirements gathered and initial planning completed.</p>
        </div>
    </details>

    <details class="timeline-item" open="open">
        <summary class="timeline-summary">2024-02: Development Phase</summary>
        <div class="timeline-content">
            <p>Core features implemented and tested.</p>
        </div>
    </details>

    <details class="timeline-item">
        <summary class="timeline-summary">2024-03: Testing</summary>
        <div class="timeline-content">
            <p>QA testing and bug fixes... (collapsed in PDF)</p>
        </div>
    </details>
</div>
```

### Product Specifications Details

```html
<details open="open" style="
    border: 2pt solid #4CAF50;
    border-radius: 8pt;
    padding: 0;
    overflow: hidden;
    margin: 15pt 0;">

    <summary style="
        padding: 15pt 20pt;
        background-color: #4CAF50;
        color: white;
        font-size: 13pt;
        font-weight: bold;">
        Technical Specifications
    </summary>

    <div style="padding: 20pt;">
        <table style="width: 100%;">
            <tr>
                <td style="padding: 8pt 0; font-weight: bold; width: 30%;">Dimensions</td>
                <td style="padding: 8pt 0;">15.6" x 10.2" x 0.7"</td>
            </tr>
            <tr style="background-color: #f9f9f9;">
                <td style="padding: 8pt 0; font-weight: bold;">Weight</td>
                <td style="padding: 8pt 0;">4.2 lbs (1.9 kg)</td>
            </tr>
            <tr>
                <td style="padding: 8pt 0; font-weight: bold;">Display</td>
                <td style="padding: 8pt 0;">15.6" Full HD (1920x1080) IPS</td>
            </tr>
            <tr style="background-color: #f9f9f9;">
                <td style="padding: 8pt 0; font-weight: bold;">Processor</td>
                <td style="padding: 8pt 0;">Intel Core i7-11800H (8-core, 16-thread)</td>
            </tr>
            <tr>
                <td style="padding: 8pt 0; font-weight: bold;">Memory</td>
                <td style="padding: 8pt 0;">32GB DDR4 RAM</td>
            </tr>
            <tr style="background-color: #f9f9f9;">
                <td style="padding: 8pt 0; font-weight: bold;">Storage</td>
                <td style="padding: 8pt 0;">1TB NVMe SSD</td>
            </tr>
        </table>
    </div>
</details>
```

### Expandable Section with Image

```html
<details open="open" style="border: 1pt solid #ddd; margin: 15pt 0; border-radius: 4pt;">
    <summary style="
        padding: 12pt 15pt;
        background-color: #f5f5f5;
        font-weight: bold;
        font-size: 12pt;
        border-bottom: 1pt solid #ddd;">
        Product Gallery
    </summary>

    <div style="padding: 15pt; text-align: center;">
        <img src="product-front.jpg" width="200pt" height="200pt"
             style="margin: 10pt; border: 1pt solid #ddd;" />
        <img src="product-side.jpg" width="200pt" height="200pt"
             style="margin: 10pt; border: 1pt solid #ddd;" />
        <img src="product-back.jpg" width="200pt" height="200pt"
             style="margin: 10pt; border: 1pt solid #ddd;" />
    </div>
</details>
```

### Conditional Details Based on User Role

```html
{% raw %}<!-- With model = { userRole: "admin", adminContent: "Admin details...", userContent: "User details..." } -->
<details open="{{model.userRole === 'admin' ? 'open' : 'closed'}}">
    <summary style="padding: 10pt; background-color: #ffebee; font-weight: bold;">
        Administrator Settings
    </summary>
    <div style="padding: 15pt; background-color: #fff;">
        <p>{{model.adminContent}}</p>
        <p>Only visible when user is admin and details are open.</p>
    </div>
</details>

<details open="open">
    <summary style="padding: 10pt; background-color: #e3f2fd; font-weight: bold;">
        👤 User Information
    </summary>
    <div style="padding: 15pt; background-color: #fff;">
        <p>{{model.userContent}}</p>
        <p>Visible to all users.</p>
    </div>
</details>{% endraw %}
```

### Multi-Level Documentation Structure

```html
<style>
    .doc-section { border: 1pt solid #ccc; margin: 10pt 0; }
    .doc-chapter { border: 2pt solid #336699; margin: 15pt 0; }
    .chapter-summary {
        background-color: #336699;
        color: white;
        padding: 12pt 15pt;
        font-size: 14pt;
        font-weight: bold;
    }
    .section-summary {
        background-color: #f0f0f0;
        padding: 10pt 12pt;
        font-weight: bold;
        margin-left: 15pt;
    }
    .subsection-summary {
        background-color: #f8f8f8;
        padding: 8pt 10pt;
        font-weight: 600;
        margin-left: 30pt;
    }
</style>

<details class="doc-chapter" open="open">
    <summary class="chapter-summary">Chapter 1: Introduction to PDF Generation</summary>

    <div style="padding: 15pt;">
        <p>This chapter introduces the core concepts of PDF generation...</p>

        <details class="doc-section" open="open">
            <summary class="section-summary">1.1 Understanding PDF Structure</summary>
            <div style="padding: 12pt; margin-left: 15pt;">
                <p>PDF documents are composed of objects that define the document structure...</p>

                <details open="closed" style="margin-left: 30pt; margin-top: 10pt;">
                    <summary class="subsection-summary">1.1.1 Document Catalog</summary>
                    <div style="padding: 10pt; margin-left: 30pt;">
                        <p>This subsection is collapsed and won't appear in the PDF.</p>
                    </div>
                </details>
            </div>
        </details>

        <details open="closed" class="doc-section">
            <summary class="section-summary">1.2 Getting Started</summary>
            <div style="padding: 12pt; margin-left: 15pt;">
                <p>This section is collapsed in PDF output.</p>
            </div>
        </details>
    </div>
</details>
```

---

## See Also

- [div](html_div_element.html) - Generic container element
- [section](html_div_element.html) - Semantic section element
- [template](html_template_element.html) - Template element for data binding
- [CSS Styles](/learning/styles/) - Complete CSS styling
- [Data Binding](/learning/binding/) - Data binding and expressions

---
