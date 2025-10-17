---
layout: default
title: h1, h2, h3, h4, h5, h6
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;h1&gt; - &lt;h6&gt; : The HTML Heading Elements

The `<h1>` through `<h6>` elements represent six levels of section headings in HTML documents. `<h1>` is the highest (most important) heading level and `<h6>` is the lowest. Headings are block-level elements used to create document structure, establish hierarchy, and automatically generate PDF bookmarks/outlines when the `title` attribute is set.

## Usage

Heading elements create structured, hierarchical content in PDF documents:
- Provide semantic structure to documents
- Display as bold text with automatic font sizing based on heading level
- Take full width of their parent container
- Create document outlines/bookmarks for PDF navigation (when using `title` attribute)
- Support automatic numbering when configured
- Can contain text and inline elements
- Support all CSS styling properties for customization

```html
<h1>Main Document Title</h1>
<h2>Section Heading</h2>
<h3>Subsection Heading</h3>
<h4>Minor Heading</h4>
<h5>Sub-minor Heading</h5>
<h6>Least Important Heading</h6>
```

### Default Font Sizes

Each heading level has a default font size:

| Element | Default Size | Bold | Italic | Points |
|---------|--------------|------|--------|--------|
| `<h1>` | Largest | Yes | No | 36pt |
| `<h2>` | Large | Yes | Yes | 30pt |
| `<h3>` | Medium-Large | Yes | No | 24pt |
| `<h4>` | Medium | Yes | Yes | 20pt |
| `<h5>` | Small | Yes | No | 17pt |
| `<h6>` | Smallest | Yes | Yes | 15pt |

Note: The default sizing uses points (pt) rather than pixels, which is standard for PDF generation. All sizes can be overridden using CSS styles.

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for PDF navigation. When set, creates a bookmark entry in the PDF's outline/bookmark panel. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-bind` | expression | Binds the element to a data context for use with templates. |
| `data-content` | expression | Dynamically sets the content of the heading from bound data. |

### CSS Style Support

Heading elements support extensive CSS styling through the `style` attribute or CSS classes:

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `color` (text color)
- `text-align`: `left`, `center`, `right`, `justify`
- `text-decoration`, `text-transform`
- `line-height`, `letter-spacing`, `word-spacing`

**Box Model**:
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`
- `border`, `border-width`, `border-color`, `border-style`
- `border-top`, `border-right`, `border-bottom`, `border-left`

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `background-position`, `background-size`, `background-repeat`
- `opacity`

**Layout**:
- `display`: `block`, `inline`, `inline-block`, `none`
- `page-break-before`, `page-break-after`, `page-break-inside`
- `overflow`: `visible`, `hidden`, `clip`

---

## Notes

### Default Behavior

Heading elements have the following default behavior:

1. **Block Display**: All headings display as block-level elements (stack vertically)
2. **Full Width**: Take 100% of the parent container's width by default
3. **Bold Text**: All heading levels are bold by default
4. **Italic**: h2, h4, and h6 are italic by default
5. **Top Padding**: Default 10pt top padding for spacing
6. **No Split**: Headings won't split across pages (overflow-split: never)

### Outline/Bookmark Generation

When the `title` attribute is set on a heading element, Scryber automatically generates a bookmark entry in the PDF's outline/navigation panel:

```html
<h1 title="Introduction">Introduction</h1>
```

This creates a clickable bookmark in PDF readers that navigates to this heading. The outline respects the heading hierarchy (h1 > h2 > h3, etc.).

### Class Hierarchy

In the Scryber codebase:
- `HTMLHead1` extends `Head1` extends `HeadingBase` extends `Panel`
- Similar hierarchy for h2 through h6
- Each heading class is mapped to its HTML tag (h1, h2, h3, h4, h5, h6)
- Inherits all properties and behaviors from the base `Panel` class

### Layout Engine

Heading elements use the `LayoutEngineHeading` layout engine, which:
- Handles automatic numbering when configured with numbering groups
- Prevents splitting across page breaks by default
- Manages text layout and line breaking
- Supports outline/bookmark generation

### Automatic Numbering

Headings support automatic numbering through numbering groups. Each heading level has its own default numbering group:
- h1: `__scryber_head1`
- h2: `__scryber_head2`
- h3: `__scryber_head3`
- h4: `__scryber_head4`
- h5: `__scryber_head5`
- h6: `__scryber_head6`

### Page Breaking

Headings have default `overflow-split: never` behavior, meaning they won't split across pages. Control page breaking with:

```html
<h2 style="page-break-before: always;">New Page Section</h2>
<h3 style="page-break-after: always;">Forces Break After</h3>
```

---

## Examples

### Basic Heading Hierarchy

```html
<h1>Document Title</h1>

<h2>Chapter 1: Introduction</h2>
<p>Content for the introduction chapter...</p>

<h3>Section 1.1: Background</h3>
<p>Background information...</p>

<h3>Section 1.2: Purpose</h3>
<p>Purpose of the document...</p>

<h2>Chapter 2: Methods</h2>
<p>Methods description...</p>

<h3>Section 2.1: Approach</h3>
<p>Approach details...</p>

<h4>Subsection 2.1.1: Detailed Steps</h4>
<p>Step-by-step details...</p>
```

### All Heading Levels with Default Styling

```html
<h1>Heading 1 - Largest (36pt, Bold)</h1>
<p>This is the main title level, typically used once per document.</p>

<h2>Heading 2 - Large (30pt, Bold, Italic)</h2>
<p>Major section headings, used for primary document divisions.</p>

<h3>Heading 3 - Medium-Large (24pt, Bold)</h3>
<p>Subsection headings, used for secondary divisions.</p>

<h4>Heading 4 - Medium (20pt, Bold, Italic)</h4>
<p>Minor headings for smaller sections.</p>

<h5>Heading 5 - Small (17pt, Bold)</h5>
<p>Sub-minor headings for detailed organization.</p>

<h6>Heading 6 - Smallest (15pt, Bold, Italic)</h6>
<p>The lowest level heading, used sparingly.</p>
```

### Headings with Outlines/Bookmarks

```html
<h1 title="Introduction">Introduction</h1>
<p>The introduction provides an overview...</p>

<h2 title="Background">Background Information</h2>
<p>Historical context and background...</p>

<h3 title="Previous Research">Previous Research</h3>
<p>Summary of prior work in this area...</p>

<h2 title="Methodology">Research Methodology</h2>
<p>Description of research methods...</p>

<!-- This creates a hierarchical bookmark structure in the PDF -->
```

### Custom Styled Headings

```html
<h1 style="font-size: 48pt; color: #003366; text-align: center;
           border-bottom: 3pt solid #003366; padding-bottom: 10pt;">
    Corporate Report 2024
</h1>

<h2 style="font-size: 32pt; color: #006699;
           background-color: #f0f0f0; padding: 15pt;">
    Executive Summary
</h2>

<h3 style="font-size: 24pt; color: #009933;
           border-left: 5pt solid #009933; padding-left: 10pt;">
    Key Findings
</h3>

<h4 style="font-size: 18pt; color: #666;
           text-transform: uppercase; letter-spacing: 2pt;">
    Supporting Details
</h4>
```

### Headings with Background and Borders

```html
<h1 style="background-color: #003366; color: white;
           padding: 20pt; text-align: center;">
    Annual Report
</h1>

<h2 style="background: linear-gradient(to right, #006699, #0099cc);
           color: white; padding: 12pt; border-radius: 5pt;">
    Financial Overview
</h2>

<h3 style="border: 2pt solid #ccc; border-left: 8pt solid #006699;
           padding: 10pt; background-color: #f9f9f9;">
    Revenue Analysis
</h3>
```

### Headings with Data Binding

```html
<!-- With model = { reportTitle: "Q4 Report", chapter: "Sales Analysis", year: 2024 } -->

<h1>{{model.reportTitle}} - {{model.year}}</h1>
<!-- Output: Q4 Report - 2024 -->

<h2>{{model.chapter}}</h2>
<!-- Output: Sales Analysis -->

<h3>Total Sales: ${{model.totalSales}}</h3>
<!-- Output: Total Sales: $1,250,000 -->
```

### Dynamic Headings with Templates

```html
<!-- With model.sections = [{title: "Introduction", level: 1}, {title: "Methods", level: 2}] -->

<template data-bind="{{model.sections}}">
    <h2>{{.title}}</h2>
    <p>Content for section: {{.title}}</p>
</template>

<!-- Generates headings for each section in the array -->
```

### Headings with Page Break Control

```html
<h1 style="page-break-before: always;" title="Section 1">
    Section 1: Introduction
</h1>
<p>This heading always starts on a new page.</p>

<h2 style="page-break-after: always;">
    End of Section
</h2>
<!-- Content after this heading will start on a new page -->

<h3 style="page-break-inside: avoid;">
    Important Notice
</h3>
<p>This heading and its content will not split across pages.</p>
```

### Centered Headings

```html
<h1 style="text-align: center; margin-bottom: 30pt;">
    CONFIDENTIAL REPORT
</h1>

<h2 style="text-align: center; color: #666;">
    Prepared for Board Review
</h2>

<h3 style="text-align: center; font-style: italic; color: #999;">
    Date: January 2024
</h3>
```

### Headings with Mixed Content

```html
<h2>
    Sales Report for <span style="color: #cc0000;">Q4 2024</span>
</h2>

<h3>
    Region: <span style="font-weight: normal;">North America</span>
</h3>

<h4>
    <span style="font-size: 14pt;">Status:</span>
    <span style="color: green; font-weight: normal;">Active</span>
</h4>
```

### Headings with Icons/Special Characters

```html
<h2 style="color: #006699;">
    &#9733; Featured Products
</h2>

<h3>
    &#10004; Completed Tasks
</h3>

<h4>
    &#9888; Important Notice
</h4>
```

### Responsive Heading Sizes

```html
<!-- Using CSS classes for consistent sizing -->
<style>
    .display-heading {
        font-size: 48pt;
        line-height: 1.2;
    }

    .section-heading {
        font-size: 36pt;
        color: #333;
    }

    .subsection-heading {
        font-size: 24pt;
        color: #666;
        border-bottom: 1pt solid #ccc;
        padding-bottom: 5pt;
    }
</style>

<h1 class="display-heading">Main Title</h1>
<h2 class="section-heading">Section</h2>
<h3 class="subsection-heading">Subsection</h3>
```

### Numbered Document Structure

```html
<h1 title="1. Introduction">1. Introduction</h1>
<p>Overview of the document...</p>

<h2 title="1.1 Background">1.1 Background</h2>
<p>Background information...</p>

<h2 title="1.2 Scope">1.2 Scope</h2>
<p>Document scope...</p>

<h1 title="2. Analysis">2. Analysis</h1>
<p>Detailed analysis...</p>

<h2 title="2.1 Data Sources">2.1 Data Sources</h2>
<p>Sources of data...</p>

<h3 title="2.1.1 Primary Sources">2.1.1 Primary Sources</h3>
<p>Primary data sources...</p>
```

### Headings with Conditional Display

```html
<!-- With model = { showIntro: true, showMethods: false } -->

<h1 hidden="{{!model.showIntro ? 'hidden' : ''}}">
    Introduction
</h1>
<!-- Only shown when showIntro is true -->

<h2 hidden="{{!model.showMethods ? 'hidden' : ''}}">
    Methodology
</h2>
<!-- Hidden when showMethods is false -->
```

### Multi-line Headings

```html
<h1 style="line-height: 1.3;">
    Annual Corporate Report:<br/>
    Fiscal Year 2024
</h1>

<h2>
    Department of Research and Development<br/>
    <span style="font-size: 16pt; font-weight: normal; color: #666;">
        Innovation Division
    </span>
</h2>
```

### Headings with Different Alignment

```html
<h1 style="text-align: left;">Left Aligned Heading</h1>

<h2 style="text-align: center;">Centered Heading</h2>

<h3 style="text-align: right;">Right Aligned Heading</h3>

<h4 style="text-align: justify;">
    This is a very long heading that demonstrates justified alignment
    when it wraps to multiple lines in the document
</h4>
```

### Headings with Margins and Spacing

```html
<h1 style="margin-top: 0pt; margin-bottom: 30pt;">
    Document Title
</h1>

<h2 style="margin-top: 25pt; margin-bottom: 15pt;">
    Major Section
</h2>

<h3 style="margin-top: 20pt; margin-bottom: 10pt; margin-left: 20pt;">
    Indented Subsection
</h3>

<h4 style="margin: 15pt 0pt 8pt 40pt;">
    Further Indented Heading
</h4>
```

### Headings with Shadow Effect

```html
<h1 style="font-size: 42pt; color: #333; text-shadow: 2pt 2pt 4pt rgba(0,0,0,0.3);">
    Premium Report
</h1>

<h2 style="color: #006699; text-shadow: 1pt 1pt 2pt rgba(0,102,153,0.2);">
    Executive Summary
</h2>
```

### Complete Document Example

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 40pt;
        }

        h1 {
            color: #003366;
            border-bottom: 3pt solid #003366;
            padding-bottom: 10pt;
            margin-bottom: 20pt;
        }

        h2 {
            color: #006699;
            margin-top: 25pt;
            margin-bottom: 15pt;
        }

        h3 {
            color: #0099cc;
            border-left: 5pt solid #0099cc;
            padding-left: 10pt;
            margin-top: 20pt;
        }

        p {
            line-height: 1.6;
            margin-bottom: 12pt;
        }
    </style>
</head>
<body>
    <h1 title="Annual Report 2024">Annual Report 2024</h1>

    <h2 title="Executive Summary">Executive Summary</h2>
    <p>This report provides a comprehensive overview of our performance...</p>

    <h3 title="Financial Highlights">Financial Highlights</h3>
    <p>Key financial metrics for the year...</p>

    <h3 title="Strategic Initiatives">Strategic Initiatives</h3>
    <p>Major strategic initiatives undertaken...</p>

    <h2 style="page-break-before: always;" title="Financial Performance">
        Financial Performance
    </h2>
    <p>Detailed financial analysis...</p>

    <h3 title="Revenue Growth">Revenue Growth</h3>
    <p>Revenue increased by 15% year-over-year...</p>

    <h4>Regional Performance</h4>
    <p>Breaking down performance by region...</p>

    <h5>North America</h5>
    <p>North American operations showed strong growth...</p>

    <h5>Europe</h5>
    <p>European markets remained stable...</p>

    <h5>Asia Pacific</h5>
    <p>APAC region demonstrated exceptional growth...</p>

    <h2 style="page-break-before: always;" title="Conclusion">Conclusion</h2>
    <p>In summary, the organization has demonstrated...</p>
</body>
</html>
```

### Headings in Tables

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid #ccc; padding: 10pt;">
            <h3 style="margin: 0; color: #006699;">Section A</h3>
            <p>Content for section A...</p>
        </td>
        <td style="border: 1pt solid #ccc; padding: 10pt;">
            <h3 style="margin: 0; color: #009933;">Section B</h3>
            <p>Content for section B...</p>
        </td>
    </tr>
</table>
```

---

## See Also

- [p](/reference/htmltags/p.html) - Paragraph element
- [div](/reference/htmltags/div.html) - Generic block container
- [span](/reference/htmltags/span.html) - Inline text container
- [strong](/reference/htmltags/strong.html) - Bold text emphasis
- [em](/reference/htmltags/em.html) - Italic text emphasis
- [Heading Component](/reference/components/heading.html) - Base heading component in Scryber namespace
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Data binding and expressions
- [Document Structure](/reference/structure/) - Document organization and outlines

---
