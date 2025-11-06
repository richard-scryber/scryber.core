---
layout: default
title: CSS Basics
nav_order: 4
parent: Getting Started
parent_url: /learning/01-getting-started/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# CSS Basics

Master CSS styling fundamentals to create professional-looking PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Apply CSS styles using inline, embedded, and external methods
- Understand CSS selectors and specificity
- Use common CSS properties for PDFs
- Create reusable style classes
- Understand which CSS properties work in PDFs

---

## Three Ways to Apply CSS

### 1. Inline Styles

Apply styles directly to elements using the `style` attribute:

```html
<h1 style="color: #2563eb; font-size: 24pt;">Title</h1>
<p style="margin: 10pt 0; line-height: 1.6;">Paragraph text.</p>
```

**Pros:**
- Highest specificity
- Quick for testing
- Element-specific styling

**Cons:**
- Hard to maintain
- Not reusable
- Mixes content and presentation

**When to use:** Dynamic styling with data binding

{% raw %}
```html
<div style="color: {{model.themeColor}};">
    Dynamic color based on data
</div>
```
{% endraw %}

### 2. Embedded Styles

Use `<style>` tag in the `<head>` section:

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Document</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            margin: 40pt;
        }

        h1 {
            color: #2563eb;
            font-size: 24pt;
        }

        p {
            line-height: 1.6;
            margin-bottom: 10pt;
        }
    </style>
</head>
<body>
    <h1>Title</h1>
    <p>Content</p>
</body>
</html>
```

**Pros:**
- Reusable within document
- Clean HTML markup
- Easy to maintain

**Cons:**
- Not shared across documents
- Increases file size if duplicated

**When to use:** Single-file templates

### 3. External Stylesheets

Link to external CSS files:

```html
<head>
    <link rel="stylesheet" href="./styles/common.css" />
    <link rel="stylesheet" href="./styles/invoice.css" />
</head>
```

**common.css:**
```css
body {
    font-family: Helvetica, sans-serif;
    font-size: 11pt;
    margin: 40pt;
}

h1 {
    color: #2563eb;
    font-size: 24pt;
    margin-bottom: 20pt;
}
```

**Pros:**
- Shared across multiple templates
- Easier to maintain
- Cacheable (with proper configuration)
- Separation of concerns

**Cons:**
- Additional file to manage
- Path resolution required

**When to use:** Production applications with multiple templates

---

## CSS Selectors

### Element Selectors

Target all elements of a type:

```css
p {
    margin: 10pt 0;
}

h1 {
    font-size: 24pt;
}

table {
    width: 100%;
}
```

### Class Selectors

Target elements with specific classes:

```html
<style>
    .highlight {
        background-color: #fef3c7;
        padding: 10pt;
    }

    .error {
        color: #dc2626;
        font-weight: bold;
    }
</style>

<p class="highlight">This is highlighted.</p>
<p class="error">This is an error message.</p>
```

### ID Selectors

Target a specific element by ID:

```html
<style>
    #header {
        background-color: #2563eb;
        color: white;
        padding: 20pt;
    }
</style>

<div id="header">
    <h1>Document Header</h1>
</div>
```

### Multiple Classes

Apply multiple classes to an element:

```html
<style>
    .box {
        border: 1pt solid #ccc;
        padding: 15pt;
    }

    .warning {
        border-color: #f59e0b;
        background-color: #fef3c7;
    }

    .large {
        font-size: 14pt;
    }
</style>

<div class="box warning large">
    This has multiple styles applied.
</div>
```

### Descendant Selectors

Target nested elements:

```css
/* All paragraphs inside .content */
.content p {
    line-height: 1.8;
}

/* All links inside table cells */
table td a {
    color: #2563eb;
    text-decoration: none;
}
```

### Child Selectors

Target direct children only:

```css
/* Direct children only */
.menu > li {
    font-weight: bold;
}

/* Nested items won't be affected */
.menu li li {
    font-weight: normal;
}
```

### Attribute Selectors

Target elements by attribute:

```css
/* Links with href */
a[href] {
    color: #2563eb;
}

/* Images with alt text */
img[alt] {
    border: 1pt solid #ccc;
}

/* Inputs of type text */
input[type="text"] {
    border: 1pt solid #999;
}
```

### Pseudo-classes

Special selectors for states:

```css
/* First child */
li:first-child {
    font-weight: bold;
}

/* Last child */
li:last-child {
    margin-bottom: 0;
}

/* Nth child (odd/even rows) */
tr:nth-child(odd) {
    background-color: #f9fafb;
}

tr:nth-child(even) {
    background-color: white;
}

/* Specific nth child */
li:nth-child(3) {
    color: red;
}
```

---

## Specificity and Cascade

CSS specificity determines which styles apply when there are conflicts:

### Specificity Order (Highest to Lowest)

1. **Inline styles** - `style="..."`  (1,0,0,0)
2. **ID selectors** - `#header` (0,1,0,0)
3. **Class, attribute, pseudo-class** - `.highlight`, `[type]`, `:first-child` (0,0,1,0)
4. **Element selectors** - `p`, `div` (0,0,0,1)

### Examples

```css
/* Specificity: 0,0,0,1 */
p {
    color: black;
}

/* Specificity: 0,0,1,0 - WINS */
.highlight {
    color: blue;
}

/* Specificity: 0,1,0,0 - WINS */
#special {
    color: red;
}
```

```html
<p>Black text</p>
<p class="highlight">Blue text</p>
<p id="special">Red text</p>
<p style="color: green;">Green text (inline always wins)</p>
```

### Increasing Specificity

```css
/* Low specificity */
p {
    color: black;
}

/* Higher specificity */
div p {
    color: blue;
}

/* Even higher */
.content div p {
    color: red;
}

/* Highest (without inline) */
#main .content div p {
    color: green;
}
```

---

## Common CSS Properties for PDFs

### Text Styling

```css
p {
    /* Font */
    font-family: Helvetica, Arial, sans-serif;
    font-size: 11pt;
    font-weight: bold;  /* or normal, 100-900 */
    font-style: italic; /* or normal */

    /* Color */
    color: #333;

    /* Spacing */
    line-height: 1.6;
    letter-spacing: 0.5pt;
    word-spacing: 1pt;

    /* Alignment */
    text-align: left;   /* or right, center, justify */
    text-indent: 20pt;

    /* Decoration */
    text-decoration: underline; /* or none, line-through */
    text-transform: uppercase;  /* or lowercase, capitalize */
}
```

### Box Model

```css
.box {
    /* Dimensions */
    width: 400pt;
    height: 200pt;

    /* Padding (inside) */
    padding: 15pt;
    padding-top: 10pt;
    padding-right: 20pt;
    padding-bottom: 10pt;
    padding-left: 20pt;

    /* Border */
    border: 1pt solid #ccc;
    border-top: 2pt solid #2563eb;
    border-radius: 5pt;

    /* Margin (outside) */
    margin: 20pt;
    margin-top: 10pt;
    margin-bottom: 20pt;
}
```

### Colors and Backgrounds

```css
.element {
    /* Text color */
    color: #2563eb;           /* Hex */
    color: rgb(37, 99, 235);  /* RGB */
    color: rgba(37, 99, 235, 0.5); /* RGBA with transparency */

    /* Background */
    background-color: #eff6ff;
    background-image: url('./images/bg.png');
    background-repeat: no-repeat;
    background-position: center;
    background-size: cover;
}
```

### Display and Visibility

```css
.element {
    /* Display type */
    display: block;        /* Block-level */
    display: inline;       /* Inline */
    display: inline-block; /* Inline with dimensions */
    display: none;         /* Hidden */

    /* Visibility */
    visibility: visible;   /* Visible */
    visibility: hidden;    /* Hidden but takes space */
}
```

### Positioning

```css
.element {
    position: static;    /* Default flow */
    position: relative;  /* Relative to normal position */
    position: absolute;  /* Absolute positioning */
    position: fixed;     /* Fixed on page */

    top: 10pt;
    right: 20pt;
    bottom: 10pt;
    left: 20pt;

    z-index: 10;        /* Stacking order */
}
```

---

## Practical Example: Styled Document

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Professional Document</title>
    <style>
        /* Base styles */
        body {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 11pt;
            margin: 40pt;
            color: #333;
            line-height: 1.6;
        }

        /* Typography hierarchy */
        h1 {
            color: #1e40af;
            font-size: 28pt;
            font-weight: bold;
            border-bottom: 3pt solid #1e40af;
            padding-bottom: 10pt;
            margin-bottom: 20pt;
        }

        h2 {
            color: #2563eb;
            font-size: 20pt;
            font-weight: bold;
            margin-top: 30pt;
            margin-bottom: 15pt;
        }

        h3 {
            color: #3b82f6;
            font-size: 16pt;
            font-weight: 600;
            margin-top: 20pt;
            margin-bottom: 10pt;
        }

        /* Paragraphs */
        p {
            margin-bottom: 10pt;
        }

        /* Reusable components */
        .info-box {
            border: 1pt solid #2563eb;
            border-left: 4pt solid #2563eb;
            background-color: #eff6ff;
            padding: 15pt;
            margin: 20pt 0;
        }

        .warning-box {
            border: 1pt solid #f59e0b;
            border-left: 4pt solid #f59e0b;
            background-color: #fef3c7;
            padding: 15pt;
            margin: 20pt 0;
        }

        .highlight {
            background-color: #fef08a;
            padding: 2pt 4pt;
        }

        /* Lists */
        ul, ol {
            margin: 10pt 0;
            padding-left: 30pt;
        }

        li {
            margin-bottom: 5pt;
        }

        /* Tables */
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }

        thead {
            background-color: #2563eb;
            color: white;
        }

        th {
            padding: 10pt;
            text-align: left;
            font-weight: bold;
        }

        td {
            padding: 8pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        /* Footer */
        .footer {
            position: fixed;
            bottom: 20pt;
            left: 40pt;
            right: 40pt;
            text-align: center;
            font-size: 9pt;
            color: #666;
            border-top: 1pt solid #ccc;
            padding-top: 10pt;
        }
    </style>
</head>
<body>
    <h1>Professional Document</h1>

    <div class="info-box">
        <strong>Info:</strong> This document demonstrates CSS styling best practices.
    </div>

    <h2>Introduction</h2>
    <p>
        This is a paragraph with <span class="highlight">highlighted text</span>.
        The document uses consistent spacing, colors, and typography.
    </p>

    <h2>Features</h2>
    <ul>
        <li>Consistent typography hierarchy</li>
        <li>Reusable style classes</li>
        <li>Professional color scheme</li>
        <li>Proper spacing and alignment</li>
    </ul>

    <div class="warning-box">
        <strong>Warning:</strong> Always test your styles in actual PDF output.
    </div>

    <h2>Data Table</h2>
    <table>
        <thead>
            <tr>
                <th>Item</th>
                <th>Description</th>
                <th>Value</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Item 1</td>
                <td>First item description</td>
                <td>$100</td>
            </tr>
            <tr>
                <td>Item 2</td>
                <td>Second item description</td>
                <td>$200</td>
            </tr>
            <tr>
                <td>Item 3</td>
                <td>Third item description</td>
                <td>$300</td>
            </tr>
        </tbody>
    </table>

    <div class="footer">
        Page <page-number /> of <page-count /> | Professional Document | Generated with Scryber.Core
    </div>
</body>
</html>
```

---

## CSS Properties NOT Supported

These CSS properties don't work in Scryber (PDF limitations):

### Layout
- `display: flex` - Use tables instead
- `display: grid` - Use tables instead
- `float` - Use positioning or tables

### Animation/Interaction
- `transition` - PDFs are static
- `animation` - PDFs are static
- `transform` - Limited support (rotation only)
- `:hover`, `:active` - No interaction in PDFs

### Advanced Features
- `box-shadow` - Not supported
- `text-shadow` - Not supported
- `clip-path` - Not supported
- `filter` - Not supported

---

## Try It Yourself

### Exercise 1: Three Styling Methods

Create the same styled paragraph using:
1. Inline styles
2. Embedded `<style>` tag
3. External CSS file

Compare which feels most maintainable.

### Exercise 2: Specificity Test

Create CSS rules with different specificity levels:
```css
p { color: black; }
.highlight { color: blue; }
#special { color: red; }
```

Apply them to paragraphs and observe which wins.

### Exercise 3: Reusable Component Library

Create a small library of reusable classes:
- `.box-info`, `.box-warning`, `.box-error`
- `.btn-primary`, `.btn-secondary`
- `.text-small`, `.text-large`

Use them in a document.

---

## Common Pitfalls

### ❌ Using Browser-Specific CSS

```css
.container {
    display: flex;  /* Won't work */
}
```

✅ **Solution:** Use supported properties

```css
.container {
    display: table;
    width: 100%;
}
```

### ❌ Forgetting Units

```css
p {
    margin: 10;  /* Missing unit */
}
```

✅ **Solution:** Always specify units

```css
p {
    margin: 10pt;
}
```

### ❌ Overly Specific Selectors

```css
body div.content div.section div.subsection p.text {
    color: black;
}
```

✅ **Solution:** Keep selectors simple

```css
.subsection p {
    color: black;
}
```

---

## Next Steps

Now that you understand CSS basics:

1. **[Pages & Sections](05_pages_sections.md)** - Structure multi-page documents
2. **[Styling & Appearance](/learning/03-styling/)** - Deep dive into styling
3. **[CSS Property Reference](/reference/css/)** - All supported properties

---

**Continue learning →** [Pages & Sections](05_pages_sections.md)
