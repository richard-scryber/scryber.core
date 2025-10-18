---
layout: default
title: Layout & Positioning
nav_order: 4
parent: Learning Guides
parent_url: /learning/
has_children: true
has_toc: false
---

# Layout & Positioning

Master page-based layout, positioning, and document structure to create professional multi-page PDF documents.

---

## Table of Contents

1. [Page Sizes & Orientation](01_page_sizes_orientation.md) - Standard/custom sizes, portrait/landscape
2. [Margins & Padding](02_margins_padding.md) - Page margins, element spacing, box model
3. [Sections & Page Breaks](03_sections_page_breaks.md) - Page break control, pagination, keeping content together
4. [Multi-Column Layouts](04_multi_column.md) - Table-cell layouts, column widths, gutters
5. [Positioning](05_positioning.md) - Static/relative/absolute, overlays, watermarks
6. [Table Layouts](06_tables.md) - Data tables, table sizing, borders, page breaks
7. [Headers & Footers](07_headers_footers.md) - Running headers/footers, page numbers
8. [Layout Best Practices](08_layout_best_practices.md) - Professional patterns, optimization, troubleshooting

---

## Overview

Unlike web pages that scroll continuously, PDF documents are page-based. This series teaches you how to control page layout, manage content flow across pages, position elements precisely, and structure complex multi-page documents.

## What Makes PDF Layout Unique?

PDF layout differs significantly from web layout:

- **Fixed Page Sizes** - Content flows across defined pages (A4, Letter, etc.)
- **Page Breaks** - Control where content splits across pages
- **Headers & Footers** - Repeat content on every page
- **Sections** - Different page settings within one document
- **Column Layout** - Multi-column text flow
- **Absolute Positioning** - Precise placement for overlays and watermarks

## Quick Example

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page {
            size: A4 portrait;
            margin: 1in;
        }

        body {
            font-family: Arial, sans-serif;
            font-size: 11pt;
        }

        .chapter {
            page-break-before: always;
        }

        header {
            text-align: center;
            border-bottom: 2pt solid #333;
            padding-bottom: 10pt;
        }

        footer {
            text-align: center;
            padding-top: 10pt;
            border-top: 1pt solid #ccc;
        }

        .watermark {
            position: absolute;
            top: 50%;
            left: 50%;
            opacity: 0.1;
            font-size: 72pt;
        }
    </style>
</head>
<body>
    <header>
        <h1>Document Title</h1>
    </header>

    <div class="watermark">DRAFT</div>

    <div class="chapter">
        <h2>Chapter 1</h2>
        <p>Content here...</p>
    </div>

    <div class="chapter">
        <h2>Chapter 2</h2>
        <p>More content...</p>
    </div>

    <footer>
        <page>Page <page-number /> of <page-count /></page>
    </footer>
</body>
</html>
```

## What You'll Learn

This series covers everything you need for professional document layout:

### 1. [Page Sizes & Orientation](01_page_sizes_orientation.md)
- Standard page sizes (Letter, A4, Legal)
- Custom page sizes
- Portrait vs landscape orientation
- Setting size with @page rule
- Mixing orientations in one document

### 2. [Margins & Padding](02_margins_padding.md)
- Understanding the box model
- Page margins with @page rule
- Element margins and padding
- Margin collapse behavior
- Box-sizing property
- Consistent spacing scales

### 3. [Sections & Page Breaks](03_sections_page_breaks.md)
- page-break-before and page-break-after properties
- page-break-inside to avoid splitting
- Controlling pagination
- Orphans and widows
- Keeping content together
- Multi-section documents

### 4. [Multi-Column Layouts](04_multi_column.md)
- Table-cell method for columns
- Equal and unequal column widths
- Adding gutters (spacing)
- Vertical alignment
- Borders between columns
- Why flexbox/grid don't work in PDF

### 5. [Positioning](05_positioning.md)
- Position property (static, relative, absolute)
- Positioning context and offset values
- Centering with absolute positioning
- Z-index for stacking order
- Overlays and watermarks
- Badge positioning

### 6. [Table Layouts](06_tables.md)
- Data tables vs layout tables
- Table sizing (auto vs fixed)
- Column width control
- Border styles and alternating rows
- Table page breaks
- Repeating headers

### 7. [Headers & Footers](07_headers_footers.md)
- Creating page headers and footers
- Page numbering
- Dynamic content in headers/footers
- Scryber-specific implementation
- Header/footer styling

### 8. [Layout Best Practices](08_layout_best_practices.md)
- Core layout principles
- Professional layout patterns
- Performance optimization
- Common pitfalls and solutions
- Responsive layout strategies
- Production checklist

## Prerequisites

Before starting this series:

- **Complete [Getting Started](/learning/01-getting-started/)** - Basic Scryber knowledge
- **Review [Styling & Appearance](/learning/03-styling/)** - CSS fundamentals

## Key Concepts

### Page Flow

Content flows from page to page automatically:

```
┌─────────┐     ┌─────────┐     ┌─────────┐
│ Page 1  │ → → │ Page 2  │ → → │ Page 3  │
│         │     │         │     │         │
│ Content │     │ Content │     │ Content │
│ flows   │     │ continues     │ ends    │
│ here    │     │ here    │     │ here    │
└─────────┘     └─────────┘     └─────────┘
```

### Page Anatomy

```
┌───────────────────────────────────┐
│  Top Margin                       │
│  ┌─────────────────────────────┐  │
│  │ Header Area                 │  │
│  ├─────────────────────────────┤  │
│L │                             │ R│
│e │                             │ i│
│f │  Content Area               │ g│
│t │  (Printable Area)           │ h│
│  │                             │ t│
│M │                             │  │
│a │                             │ M│
│r │                             │ a│
│g │                             │ r│
│i │                             │ g│
│n │                             │ i│
│  │                             │ n│
│  ├─────────────────────────────┤  │
│  │ Footer Area                 │  │
│  └─────────────────────────────┘  │
│  Bottom Margin                    │
└───────────────────────────────────┘
```

### Positioning Modes

**Static (Default)**
- Elements flow naturally in document order
- No positioning offsets

**Relative**
- Positioned relative to normal position
- Offsets don't affect other elements

**Absolute**
- Positioned relative to containing block
- Removed from normal flow
- Perfect for overlays, watermarks

## Common Page Sizes

### US Letter (8.5" × 11")
```css
@page {
    size: letter portrait;
}
```

### A4 (210mm × 297mm)
```css
@page {
    size: A4 portrait;
}
```

### Custom Size
```css
@page {
    size: 8in 10in;
}
```

### Landscape
```css
@page {
    size: letter landscape;
}
```

## Page Break Control

### Force Page Break

```html
<div style="page-break-after: always;">
    <p>This content ends the page.</p>
</div>
<p>This starts a new page.</p>
```

### Avoid Page Break

```html
<div style="page-break-inside: avoid;">
    <h2>Title</h2>
    <p>Keep this together on one page.</p>
</div>
```

### Conditional Breaks

{% raw %}
```html
{{#each chapters}}
<div style="page-break-before: {{if(@index == 0, 'auto', 'always')}};">
    <h2>{{this.title}}</h2>
    <p>{{this.content}}</p>
</div>
{{/each}}
```
{% endraw %}

## Column Layouts

### Two-Column Layout

```css
.content {
    column-count: 2;
    column-gap: 20pt;
    column-rule: 1pt solid #ccc;
}
```

### Magazine Style

```css
.article {
    column-count: 3;
    column-gap: 15pt;
}

.article h1 {
    column-span: all;  /* Span across all columns */
}
```

## Headers & Footers

### Running Header

```html
<header>
    <div style="text-align: center; border-bottom: 1pt solid #ccc;">
        <p>Document Title - Confidential</p>
    </div>
</header>
```

### Page Numbers

```html
<footer>
    <p style="text-align: center;">
        Page <page-number /> of <page-count />
    </p>
</footer>
```

### Continuation Headers

```html
<continuation-header>
    <p>Continued from previous page...</p>
</continuation-header>
```

## Real-World Layouts

### Report Layout

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page {
            size: letter portrait;
            margin: 1in 0.75in;
        }

        header {
            text-align: center;
            border-bottom: 2pt solid #1e40af;
            padding-bottom: 10pt;
            margin-bottom: 20pt;
        }

        .chapter {
            page-break-before: always;
        }

        footer {
            text-align: right;
            font-size: 9pt;
            color: #666;
        }
    </style>
</head>
<body>
    <header>
        <h1>{{report.title}}</h1>
        <p>{{report.date}}</p>
    </header>

    {{#each report.chapters}}
    <div class="chapter">
        <h2>{{this.title}}</h2>
        <div>{{this.content}}</div>
    </div>
    {{/each}}

    <footer>
        <page>Page <page-number /></page>
    </footer>
</body>
</html>
```

### Multi-Section Document

```html
<section style="page: A4 portrait; margin: 1in;">
    <!-- Cover page -->
    <div style="text-align: center; margin-top: 3in;">
        <h1 style="font-size: 36pt;">{{title}}</h1>
    </div>
</section>

<section style="page: A4 portrait; margin: 1in 0.75in;">
    <!-- Content pages -->
    <header>
        <p>{{documentTitle}}</p>
    </header>

    <div>
        <p>Main content here...</p>
    </div>

    <footer>
        <page>Page <page-number /></page>
    </footer>
</section>

<section style="page: A4 landscape; margin: 0.75in 1in;">
    <!-- Landscape section for wide tables -->
    <table>
        <!-- Wide data table -->
    </table>
</section>
```

### Watermark Overlay

{% raw %}
```html
<style>
    .watermark {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%) rotate(-45deg);
        font-size: 72pt;
        color: rgba(255, 0, 0, 0.1);
        font-weight: bold;
        pointer-events: none;
    }
</style>

<div class="watermark">{{if(isDraft, 'DRAFT', '')}}</div>
```
{% endraw %}

## Learning Path

**Recommended progression:**

1. **Start with Pages** - Understand sizes and orientation
2. **Set Margins** - Define printable area
3. **Use Sections** - Structure complex documents
4. **Control Breaks** - Manage pagination
5. **Add Columns** - Multi-column layouts
6. **Position Elements** - Overlays and watermarks
7. **Add Headers/Footers** - Running content
8. **Apply Best Practices** - Professional layouts

## Tips for Success

1. **Plan Page Structure First** - Sketch your layout before coding
2. **Test Page Breaks Early** - See how content flows across pages
3. **Use Sections for Different Layouts** - Mix orientations and sizes
4. **Keep Content Together** - Use page-break-inside: avoid
5. **Test with Varying Content** - Ensure layout works with different data
6. **Use Absolute Positioning Sparingly** - Can break page flow
7. **Consider Printing** - Ensure adequate margins for binding

## Common Patterns

### Chapter Starts on New Page

{% raw %}
```html
{{#each chapters}}
<div class="chapter" style="page-break-before: always;">
    <h1>Chapter {{@index}}: {{this.title}}</h1>
    <div>{{this.content}}</div>
</div>
{{/each}}
```
{% endraw %}

### Keep Heading with Content

```html
<h2 style="page-break-after: avoid;">Section Title</h2>
<div style="page-break-inside: avoid;">
    <p>This content stays with the heading.</p>
</div>
```

### Dynamic Page Numbers

{% raw %}
```html
<footer>
    <div style="display: flex; justify-content: space-between;">
        <span>{{documentTitle}}</span>
        <span>Page <page-number /> of <page-count /></span>
    </div>
</footer>
```
{% endraw %}

## Next Steps

Ready to master PDF layout? Start with [Page Sizes & Orientation](01_page_sizes_orientation.md) to understand the foundation.

Jump to specific topics:
- [Sections & Page Breaks](03_sections_page_breaks.md) for pagination control
- [Positioning](05_positioning.md) for overlays and watermarks
- [Headers & Footers](07_headers_footers.md) for running content
- [Table Layouts](06_tables.md) for data presentation

---

**Related Series:**
- [Styling & Appearance](/learning/03-styling/) - Visual design
- [Content Components](/learning/06-content/) - Tables, images, SVG
- [Practical Applications](/learning/08-practical/) - Real-world examples

---

**Begin learning layout →** [Page Sizes & Orientation](01_page_sizes_orientation.md)
