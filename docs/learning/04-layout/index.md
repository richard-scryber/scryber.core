---
layout: default
title: Layout & Positioning
nav_order: 4
parent: Learning Guides
parent_url: /learning/
has_children: true
has_toc: true
---

# Layout & Positioning

Master page-based layout, positioning, and document structure to create professional multi-page PDF documents.

---

## Table of Contents

1. [Page Sizes & Orientation](01_page_sizes_orientation.md) - Standard/custom sizes, portrait/landscape
2. [Page Margins](02_page_margins.md) - Margins, printable area, margin boxes
3. [Sections](03_sections.md) - Multiple sections, section-specific settings
4. [Page Breaks](04_page_breaks.md) - Page break control, pagination, orphans/widows
5. [Column Layout](05_column_layout.md) - Multi-column text, column breaks, spans
6. [Positioning](06_positioning.md) - Static/relative/absolute, overlays, position mode
7. [Headers & Footers](07_headers_footers.md) - Running headers/footers, page numbers
8. [Layout Best Practices](08_layout_best_practices.md) - Strategies, patterns, troubleshooting

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
- Portrait vs landscape
- Setting size in CSS and sections
- Mixing orientations in one document

### 2. [Page Margins](02_page_margins.md)
- Page margins and printable area
- Setting margins in CSS
- Margin boxes for headers/footers
- Section-specific margins

### 3. [Sections](03_sections.md)
- Section element for document parts
- Multiple sections in one document
- Section-specific settings (size, orientation, margins)
- Section breaks and navigation

### 4. [Page Breaks](04_page_breaks.md)
- page-break-before and page-break-after
- page-break-inside to avoid splitting
- Controlling pagination
- Orphans and widows
- Keeping content together

### 5. [Column Layout](05_column_layout.md)
- Multi-column text flow
- Column count and width
- Column gap and rules (dividers)
- Column breaks
- Column spans

### 6. [Positioning](06_positioning.md)
- Position property (static, relative, absolute)
- Top, left, right, bottom properties
- Positioning context
- Overlays and watermarks
- **Position mode for precise placement**

### 7. [Headers & Footers](07_headers_footers.md)
- Page headers and footers
- Running headers/footers
- First/last page variations
- Continuation headers/footers
- Dynamic content in headers
- Page numbers and counts

### 8. [Layout Best Practices](08_layout_best_practices.md)
- Layout strategies
- Performance considerations
- Cross-page content handling
- Common layout patterns
- Troubleshooting layout issues

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
- [Page Breaks](04_page_breaks.md) for pagination control
- [Positioning](06_positioning.md) for overlays and watermarks
- [Headers & Footers](07_headers_footers.md) for running content

---

**Related Series:**
- [Styling & Appearance](/learning/03-styling/) - Visual design
- [Content Components](/learning/06-content/) - Tables, images, SVG
- [Practical Applications](/learning/08-practical/) - Real-world examples

---

**Begin learning layout →** [Page Sizes & Orientation](01_page_sizes_orientation.md)
