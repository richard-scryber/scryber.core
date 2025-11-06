---
layout: default
title: Pages & Sections
nav_order: 5
parent: Getting Started
parent_url: /learning/01-getting-started/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Pages & Sections

Master multi-page documents with page setup, sections, headers, and footers.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Set page size and orientation
- Control page margins
- Create multi-section documents
- Add headers and footers
- Use page numbers
- Control page breaks

---

## Page Setup with @page

The `@page` rule defines page properties for all pages:

```css
@page {
    size: Letter;           /* Page size */
    margin: 1in;            /* All margins */
    orientation: portrait;  /* or landscape */
}
```

### Page Sizes

```css
/* Standard sizes */
@page { size: Letter; }      /* 8.5in × 11in */
@page { size: Legal; }       /* 8.5in × 14in */
@page { size: A4; }          /* 210mm × 297mm */
@page { size: A3; }          /* 297mm × 420mm */
@page { size: Tabloid; }     /* 11in × 17in */

/* Custom sizes */
@page { size: 6in 9in; }     /* Width × Height */
@page { size: 200mm 300mm; } /* Metric */
```

### Page Margins

```css
/* All margins */
@page {
    margin: 1in;
}

/* Individual margins */
@page {
    margin-top: 1in;
    margin-right: 0.75in;
    margin-bottom: 1in;
    margin-left: 0.75in;
}

/* Shorthand (top, right, bottom, left) */
@page {
    margin: 1in 0.75in 1in 0.75in;
}

/* Shorthand (vertical, horizontal) */
@page {
    margin: 1in 0.75in;
}
```

---

## Complete Document with Pages

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Multi-Page Document</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
        }

        h1 {
            font-size: 24pt;
            color: #2563eb;
        }
    </style>
</head>
<body>
    <h1>Page 1</h1>
    <p>Content that will automatically flow across pages as needed.</p>

    <!-- Content continues... -->
</body>
</html>
```

---

## Sections

Sections allow different page settings within one document:

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Multi-Section Document</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        .portrait-section {
            /* Portrait orientation */
        }

        .landscape-section {
            /* Different settings */
        }
    </style>
</head>
<body>
    <!-- Section 1: Portrait -->
    <section class="portrait-section">
        <h1>Portrait Section</h1>
        <p>This section uses portrait orientation.</p>
    </section>

    <!-- Section 2: Landscape -->
    <section class="landscape-section" style="page-break-before: always;">
        <h1>Landscape Section</h1>
        <p>This section uses landscape orientation.</p>
    </section>
</body>
</html>
```

### Section-Specific Page Settings

```css
/* Default page settings */
@page {
    size: Letter portrait;
    margin: 1in;
}

/* Landscape section */
.landscape {
    page-break-before: always;
}

/* Use inline styles for section-specific page settings */
```

```html
<section class="landscape" style="page-break-before: always;">
    <!-- Content in landscape mode -->
</section>
```

---

## Page Breaks

Control where pages break:

### CSS Page Break Properties

```css
/* Break before element */
.chapter {
    page-break-before: always;  /* Always start new page */
}

/* Break after element */
.section-end {
    page-break-after: always;   /* Always break after */
}

/* Prevent breaking inside */
.keep-together {
    page-break-inside: avoid;   /* Keep on same page if possible */
}
```

### Example: Chapter Breaks

```html
<style>
    .chapter {
        page-break-before: always;
    }

    .keep-together {
        page-break-inside: avoid;
    }
</style>

<div class="chapter">
    <h1>Chapter 1</h1>
    <p>This starts on a new page.</p>
</div>

<div class="chapter">
    <h1>Chapter 2</h1>
    <p>This also starts on a new page.</p>
</div>

<div class="keep-together">
    <h2>Important Section</h2>
    <p>This will not break across pages if possible.</p>
</div>
```

### Page Break Values

| Value | Behavior |
|-------|----------|
| `auto` | Default, break as needed |
| `always` | Always force a page break |
| `avoid` | Avoid breaking if possible |
| `left` | Break to next left (even) page |
| `right` | Break to next right (odd) page |

---

## Headers and Footers

### Simple Footer with Page Numbers

```html
<style>
    .footer {
        position: fixed;
        bottom: 20pt;
        left: 0;
        right: 0;
        text-align: center;
        font-size: 9pt;
        color: #666;
    }
</style>

<body>
    <div class="footer">
        Page <page-number /> of <page-count />
    </div>

    <!-- Main content -->
    <h1>Document Title</h1>
    <p>Content here...</p>
</body>
```

### Header with Logo

```html
<style>
    .header {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        height: 60pt;
        border-bottom: 2pt solid #2563eb;
        padding: 10pt 40pt;
    }

    .header img {
        height: 40pt;
        float: left;
    }

    .header h1 {
        font-size: 14pt;
        margin: 10pt 0 0 60pt;
        color: #2563eb;
    }

    body {
        margin-top: 80pt; /* Space for fixed header */
    }
</style>

<body>
    <div class="header">
        <img src="logo.png" alt="Logo" />
        <h1>Company Name</h1>
    </div>

    <!-- Content -->
    <h1>Document Content</h1>
    <p>Main content starts below the header...</p>
</body>
```

### Complete Header and Footer

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Document with Header and Footer</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            margin-top: 80pt;    /* Space for header */
            margin-bottom: 60pt; /* Space for footer */
        }

        /* Fixed header */
        .header {
            position: fixed;
            top: 10pt;
            left: 40pt;
            right: 40pt;
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 10pt;
        }

        .header-content {
            display: table;
            width: 100%;
        }

        .header-logo {
            display: table-cell;
            width: 80pt;
        }

        .header-text {
            display: table-cell;
            vertical-align: middle;
            padding-left: 20pt;
        }

        /* Fixed footer */
        .footer {
            position: fixed;
            bottom: 10pt;
            left: 40pt;
            right: 40pt;
            border-top: 1pt solid #ccc;
            padding-top: 10pt;
            font-size: 9pt;
            color: #666;
        }

        .footer-content {
            display: table;
            width: 100%;
        }

        .footer-left {
            display: table-cell;
            width: 50%;
            text-align: left;
        }

        .footer-right {
            display: table-cell;
            width: 50%;
            text-align: right;
        }
    </style>
</head>
<body>
    <!-- Header -->
    <div class="header">
        <div class="header-content">
            <div class="header-logo">
                <img src="logo.png" style="width: 60pt;" />
            </div>
            <div class="header-text">
                <h2 style="margin: 0; color: #2563eb;">Company Name</h2>
                <p style="margin: 0; font-size: 9pt;">Tagline or subtitle</p>
            </div>
        </div>
    </div>

    <!-- Main content -->
    <h1>Document Title</h1>
    <p>This is the main content of the document. It will flow across
       multiple pages automatically, with the header and footer appearing
       on every page.</p>

    <!-- More content... -->

    <!-- Footer -->
    <div class="footer">
        <div class="footer-content">
            <div class="footer-left">
                Document ID: DOC-12345 | Date: 2025-01-15
            </div>
            <div class="footer-right">
                Page <page-number /> of <page-count />
            </div>
        </div>
    </div>
</body>
</html>
```

---

## Page Numbers

Scryber provides special elements for page numbering:

### Basic Page Numbers

```html
<page-number />          <!-- Current page: 1, 2, 3... -->
<page-count />           <!-- Total pages: 10 -->
```

### Page Number Formatting

```html
<!-- Default (Arabic numerals) -->
Page <page-number />

<!-- Roman numerals (not built-in, use CSS or data) -->

<!-- With formatting -->
<span style="font-weight: bold;">
    Page <page-number /> of <page-count />
</span>
```

### Page Number Styles

```html
<style>
    .page-num {
        font-family: 'Courier New', monospace;
        font-size: 10pt;
        color: #666;
    }
</style>

<div class="footer">
    <span class="page-num">
        <page-number /> / <page-count />
    </span>
</div>
```

---

## Practical Examples

### Example 1: Report with Sections

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Quarterly Report</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
        }

        .cover-page {
            page-break-after: always;
            text-align: center;
            padding-top: 200pt;
        }

        .cover-page h1 {
            font-size: 36pt;
            color: #1e40af;
        }

        .section {
            page-break-before: always;
        }

        .footer {
            position: fixed;
            bottom: 20pt;
            text-align: center;
            width: 100%;
            font-size: 9pt;
            color: #666;
        }
    </style>
</head>
<body>
    <!-- Cover page -->
    <div class="cover-page">
        <h1>Q4 2024 Report</h1>
        <p style="font-size: 18pt; margin-top: 30pt;">Quarterly Business Review</p>
        <p style="margin-top: 50pt;">December 31, 2024</p>
    </div>

    <!-- Section 1 -->
    <div class="section">
        <h1>Executive Summary</h1>
        <p>Key findings and highlights from Q4 2024...</p>
    </div>

    <!-- Section 2 -->
    <div class="section">
        <h1>Financial Performance</h1>
        <p>Detailed financial analysis...</p>
    </div>

    <!-- Section 3 -->
    <div class="section">
        <h1>Recommendations</h1>
        <p>Strategic recommendations for Q1 2025...</p>
    </div>

    <!-- Footer -->
    <div class="footer">
        Q4 2024 Report | Page <page-number /> of <page-count /> | Confidential
    </div>
</body>
</html>
```

### Example 2: Newsletter with Multiple Columns

```html
<style>
    @page {
        size: Letter;
        margin: 0.5in;
    }

    .two-column {
        column-count: 2;
        column-gap: 20pt;
    }

    .article {
        page-break-inside: avoid;
        margin-bottom: 20pt;
    }

    .article h3 {
        column-span: all; /* Span across columns */
    }
</style>

<body>
    <h1 style="text-align: center;">Monthly Newsletter</h1>

    <div class="two-column">
        <div class="article">
            <h3>Article 1</h3>
            <p>Content flows across two columns...</p>
        </div>

        <div class="article">
            <h3>Article 2</h3>
            <p>More content...</p>
        </div>
    </div>
</body>
```

---

## Try It Yourself

### Exercise 1: Multi-Page Report

Create a 3-page document with:
- Cover page (no header/footer)
- Content pages (with header and footer)
- Page numbers starting from page 2

### Exercise 2: Section Breaks

Create a document with:
- Portrait section for text
- Landscape section for a wide table
- Back to portrait for conclusion

### Exercise 3: Custom Headers

Create headers that show:
- Left: Document title
- Center: Section name
- Right: Page number

---

## Common Pitfalls

### ❌ Forgetting Margin Space for Headers/Footers

```css
body {
    /* Fixed header/footer overlap content */
}
```

✅ **Solution:** Add top/bottom margin

```css
body {
    margin-top: 80pt;    /* Space for header */
    margin-bottom: 60pt; /* Space for footer */
}
```

### ❌ Using Absolute Heights for Content

```css
.content {
    height: 500pt; /* Might overflow page */
}
```

✅ **Solution:** Let content flow naturally

```css
.content {
    /* Height determined by content */
    page-break-inside: avoid; /* Keep together if possible */
}
```

### ❌ Complex Page Break Logic

```css
.element {
    page-break-before: always;
    page-break-after: always;
    page-break-inside: avoid;
}
```

✅ **Solution:** Keep it simple

```css
.chapter {
    page-break-before: always;
}
```

---

## Next Steps

Now that you understand pages and sections:

1. **[Basic Content](06_basic_content.md)** - Add various content types
2. **[Layout & Positioning](/learning/04-layout/)** - Advanced page layout
3. **[Output Options](07_output_options.md)** - Configure PDF generation

---

**Continue learning →** [Basic Content](06_basic_content.md)
