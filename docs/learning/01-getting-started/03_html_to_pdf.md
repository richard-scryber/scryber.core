---
layout: default
title: HTML to PDF
nav_order: 3
parent: Getting Started
parent_url: /learning/01-getting-started/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# HTML to PDF

Understand how HTML elements translate to PDF documents and what's different from browser rendering.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Understand which HTML elements Scryber supports
- Recognize differences between browser and PDF rendering
- Avoid common HTML-to-PDF pitfalls
- Structure HTML documents optimally for PDFs

---

## Supported HTML Elements

Scryber supports a comprehensive subset of HTML5 elements:

### Document Structure

| Element | Supported | Notes |
|---------|-----------|-------|
| `<html>` | ✅ | Root element (with or without namespace) |
| `<head>` | ✅ | Contains metadata and styles |
| `<body>` | ✅ | Main content container |
| `<title>` | ✅ | Sets PDF document title |
| `<style>` | ✅ | Embedded CSS |
| `<link>` | ✅ | External stylesheets (rel="stylesheet") |
| `<meta>` | ✅ | Document metadata |

### Text Content

| Element | Supported | Notes |
|---------|-----------|-------|
| `<h1>` to `<h6>` | ✅ | Headings with hierarchy |
| `<p>` | ✅ | Paragraphs |
| `<div>` | ✅ | Generic container |
| `<span>` | ✅ | Inline container |
| `<br>` | ✅ | Line break |
| `<hr>` | ✅ | Horizontal rule |
| `<pre>` | ✅ | Preformatted text |
| `<blockquote>` | ✅ | Block quotation |

### Lists

| Element | Supported | Notes |
|---------|-----------|-------|
| `<ul>` | ✅ | Unordered list |
| `<ol>` | ✅ | Ordered list |
| `<li>` | ✅ | List item |
| `<dl>` | ✅ | Definition list |
| `<dt>` | ✅ | Definition term |
| `<dd>` | ✅ | Definition description |

### Tables

| Element | Supported | Notes |
|---------|-----------|-------|
| `<table>` | ✅ | Table container |
| `<thead>` | ✅ | Table header |
| `<tbody>` | ✅ | Table body |
| `<tfoot>` | ✅ | Table footer |
| `<tr>` | ✅ | Table row |
| `<th>` | ✅ | Header cell |
| `<td>` | ✅ | Data cell |
| `<colgroup>` | ✅ | Column group |
| `<col>` | ✅ | Column definition |

### Text Formatting

| Element | Supported | Notes |
|---------|-----------|-------|
| `<strong>`, `<b>` | ✅ | Bold text |
| `<em>`, `<i>` | ✅ | Italic text |
| `<u>` | ✅ | Underlined text |
| `<s>`, `<del>` | ✅ | Strikethrough |
| `<mark>` | ✅ | Highlighted text |
| `<small>` | ✅ | Smaller text |
| `<sup>` | ✅ | Superscript |
| `<sub>` | ✅ | Subscript |
| `<code>` | ✅ | Inline code |
| `<kbd>` | ✅ | Keyboard input |

### Links and Images

| Element | Supported | Notes |
|---------|-----------|-------|
| `<a>` | ✅ | Hyperlinks (internal and external) |
| `<img>` | ✅ | Images (PNG, JPEG, GIF, SVG) |
| `<svg>` | ✅ | Inline SVG graphics |

### Forms (Display Only)

| Element | Supported | Notes |
|---------|-----------|-------|
| `<form>` | ✅ | Form container (visual only) |
| `<input>` | ✅ | Input fields (visual representation) |
| `<textarea>` | ✅ | Text area (visual) |
| `<select>` | ✅ | Dropdown (visual) |
| `<button>` | ✅ | Button (visual) |
| `<label>` | ✅ | Form label |

### Semantic Elements

| Element | Supported | Notes |
|---------|-----------|-------|
| `<header>` | ✅ | Header section |
| `<footer>` | ✅ | Footer section |
| `<section>` | ✅ | Document section |
| `<article>` | ✅ | Article content |
| `<aside>` | ✅ | Sidebar content |
| `<nav>` | ✅ | Navigation |
| `<main>` | ✅ | Main content |

### Special Scryber Elements

| Element | Purpose |
|---------|---------|
| `<page-number />` | Current page number |
| `<page-count />` | Total page count |
| `<template>` | Data binding iteration |
| `<var>` | Variable storage |
| `<if>`, `<unless>` | Conditional rendering |
| `<frameset>`, `<frame>` | PDF merging |
| `<object>` | File attachments |

---

## NOT Supported (Browser-Specific)

These elements don't make sense in PDF context:

| Element | Why Not Supported |
|---------|-------------------|
| `<script>` | No JavaScript execution in PDFs |
| `<canvas>` | Use SVG instead |
| `<video>`, `<audio>` | No media playback in static PDFs |
| `<iframe>` | Limited support (use for content inclusion only) |
| Flexbox | Use tables or absolute positioning |
| CSS Grid | Use tables or absolute positioning |
| CSS Animations | PDFs are static |
| CSS Transitions | PDFs are static |

---

## Key Differences from Browser Rendering

### 1. Page-Based Layout

**Browser:** Continuous scrolling
**PDF:** Fixed-size pages with page breaks

```html
<style>
    /* Force page break before element */
    .new-section {
        page-break-before: always;
    }

    /* Avoid breaking inside element */
    .keep-together {
        page-break-inside: avoid;
    }
</style>

<div class="new-section">
    <h1>Chapter 2</h1>
    <p>This starts on a new page.</p>
</div>
```

### 2. Print Units (Points)

**Browser:** Pixels (px) - screen-based
**PDF:** Points (pt) - print-based (72pt = 1 inch)

```css
/* Prefer points for PDFs */
body {
    font-size: 11pt;  /* ✓ Recommended */
    margin: 40pt;     /* ✓ Recommended */
}

/* Pixels work but convert to points */
body {
    font-size: 14px;  /* Converts to ~10.5pt */
}
```

### 3. No Dynamic Content

**Browser:** Interactive JavaScript
**PDF:** Static output

```html
<!-- ❌ Won't work in PDF -->
<button onclick="alert('Hello')">Click Me</button>

<!-- ✓ Use data binding instead -->
<p>{{model.message}}</p>
```

### 4. Font Handling

**Browser:** Uses system fonts
**PDF:** Fonts must be embedded or use standard PDF fonts

```css
/* Standard PDF fonts (always available) */
body {
    font-family: Helvetica, Arial, sans-serif;
}

/* Custom fonts must be loaded */
@font-face {
    font-family: 'CustomFont';
    src: url('./fonts/CustomFont.ttf');
}
```

### 5. Image Loading

**Browser:** Lazy loading, async
**PDF:** All images loaded before generation

```html
<!-- Images must be accessible at generation time -->
<img src="./images/logo.png" />
<img src="https://example.com/image.jpg" />
```

### 6. Fixed Page Dimensions

**Browser:** Responsive, fluid width
**PDF:** Fixed page size (e.g., 8.5" × 11")

```css
@page {
    size: Letter;  /* 8.5in × 11in */
    margin: 1in;
}

/* Percentages relative to page width */
.container {
    width: 100%;  /* Full page width minus margins */
}
```

---

## HTML Document Structure for PDFs

### Minimal Valid Document

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Document Title</title>
</head>
<body>
    <p>Content</p>
</body>
</html>
```

### Recommended Structure

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Professional Document</title>
    <meta charset="UTF-8" />
    <meta name="author" content="Your Name" />
    <meta name="subject" content="Document Subject" />

    <style>
        /* Page setup */
        @page {
            size: Letter;
            margin: 1in;
        }

        /* Base styles */
        body {
            font-family: 'Helvetica', sans-serif;
            font-size: 11pt;
            color: #333;
            line-height: 1.6;
        }

        /* Headers */
        h1, h2, h3 {
            color: #2563eb;
        }
    </style>
</head>
<body>
    <!-- Header (optional) -->
    <header>
        <h1>Document Title</h1>
    </header>

    <!-- Main content -->
    <main>
        <section>
            <h2>Section 1</h2>
            <p>Content here.</p>
        </section>

        <section>
            <h2>Section 2</h2>
            <p>More content.</p>
        </section>
    </main>

    <!-- Footer (optional) -->
    <footer style="position: fixed; bottom: 0; width: 100%; text-align: center;">
        Page <page-number /> of <page-count />
    </footer>
</body>
</html>
```

---

## Common HTML Patterns

### 1. Two-Column Layout

```html
<style>
    .two-column {
        display: table;
        width: 100%;
    }
    .column {
        display: table-cell;
        width: 50%;
        padding: 10pt;
    }
</style>

<div class="two-column">
    <div class="column">
        <h3>Left Column</h3>
        <p>Content here.</p>
    </div>
    <div class="column">
        <h3>Right Column</h3>
        <p>Content here.</p>
    </div>
</div>
```

### 2. Header with Logo

```html
<style>
    .header {
        display: table;
        width: 100%;
        border-bottom: 2pt solid #2563eb;
        padding-bottom: 10pt;
        margin-bottom: 20pt;
    }
    .logo {
        display: table-cell;
        width: 100pt;
    }
    .title {
        display: table-cell;
        vertical-align: middle;
        padding-left: 20pt;
    }
</style>

<div class="header">
    <div class="logo">
        <img src="logo.png" style="width: 80pt;" />
    </div>
    <div class="title">
        <h1>Company Name</h1>
        <p>Document Title</p>
    </div>
</div>
```

### 3. Info Box

```html
<style>
    .info-box {
        border: 1pt solid #2563eb;
        border-left: 4pt solid #2563eb;
        background-color: #eff6ff;
        padding: 15pt;
        margin: 15pt 0;
    }
</style>

<div class="info-box">
    <strong>Important:</strong> This is an informational callout.
</div>
```

### 4. Styled Table

```html
<style>
    table {
        width: 100%;
        border-collapse: collapse;
        margin: 20pt 0;
    }
    th {
        background-color: #2563eb;
        color: white;
        padding: 10pt;
        text-align: left;
    }
    td {
        padding: 8pt;
        border-bottom: 1pt solid #e5e7eb;
    }
    tr:nth-child(even) {
        background-color: #f9fafb;
    }
</style>

<table>
    <thead>
        <tr>
            <th>Item</th>
            <th>Description</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Product A</td>
            <td>Description</td>
            <td>$100.00</td>
        </tr>
        <tr>
            <td>Product B</td>
            <td>Description</td>
            <td>$200.00</td>
        </tr>
    </tbody>
</table>
```

---

## Testing HTML for PDF Compatibility

### Browser Preview Method

1. Open your HTML in a browser
2. Use Print Preview (Ctrl+P / Cmd+P)
3. Check how content flows across pages
4. Note any page break issues

**Limitations:** Browser print preview doesn't perfectly match Scryber output

### Scryber Testing Method

```csharp
public void TestTemplate(string htmlPath)
{
    try
    {
        var doc = Document.ParseDocument(htmlPath);

        using (var ms = new MemoryStream())
        {
            doc.SaveAsPDF(ms);
            Console.WriteLine($"✓ Valid: {htmlPath} ({ms.Length} bytes)");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ Error in {htmlPath}: {ex.Message}");
    }
}
```

---

## Common Pitfalls

### ❌ Using Unsupported CSS

```html
<style>
    .container {
        display: flex;  /* Not supported */
        display: grid;  /* Not supported */
    }
</style>
```

✅ **Solution:** Use tables or absolute positioning

```html
<style>
    .container {
        display: table;
        width: 100%;
    }
    .item {
        display: table-cell;
    }
</style>
```

### ❌ Assuming JavaScript Works

```html
<script>
    // This won't execute in PDFs
    document.getElementById('date').innerText = new Date();
</script>
```

✅ **Solution:** Use data binding

{% raw %}
```html
<p>Date: {{model.currentDate}}</p>
```
{% endraw %}

### ❌ Relative URLs Without Base

```html
<!-- May not resolve correctly -->
<img src="../images/logo.png" />
```

✅ **Solution:** Use absolute paths or set base

```html
<head>
    <base href="https://example.com/" />
</head>
<body>
    <img src="images/logo.png" />
</body>
```

---

## Try It Yourself

### Exercise 1: Element Test

Create an HTML document using at least 10 different HTML elements from the supported list. Generate a PDF and verify all elements render correctly.

### Exercise 2: Browser vs PDF

Open one of your HTML templates in a browser and compare it to the PDF output. Note the differences.

### Exercise 3: Unsupported Element Test

Try using an unsupported element (like `<canvas>`) and observe the result. How does Scryber handle it?

---

## Next Steps

Now that you understand HTML-to-PDF conversion:

1. **[CSS Basics](04_css_basics.md)** - Learn CSS styling for PDFs
2. **[Pages & Sections](05_pages_sections.md)** - Master multi-page documents
3. **[HTML Element Reference](/reference/htmltags/)** - Complete element documentation

---

## Additional Resources

- **[HTML Element Reference](/reference/htmltags/)** - All supported elements
- **[CSS Property Reference](/reference/css/)** - All supported CSS properties
- **[Troubleshooting](08_troubleshooting.md)** - Common HTML issues

---

**Continue learning →** [CSS Basics](04_css_basics.md)
