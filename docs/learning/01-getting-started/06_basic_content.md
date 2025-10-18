---
layout: default
title: Basic Content
nav_order: 6
parent: Getting Started
parent_url: /learning/01-getting-started/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Basic Content

Learn to add text, images, lists, tables, and links to your PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Format text with headings and paragraphs
- Add and style images
- Create ordered and unordered lists
- Build basic tables
- Add hyperlinks and anchors
- Use common content patterns

---

## Text Content

### Headings

HTML provides six heading levels:

```html
<h1>Heading Level 1 - Most Important</h1>
<h2>Heading Level 2</h2>
<h3>Heading Level 3</h3>
<h4>Heading Level 4</h4>
<h5>Heading Level 5</h5>
<h6>Heading Level 6 - Least Important</h6>
```

**Styling headings:**

```css
h1 {
    font-size: 24pt;
    color: #1e40af;
    margin-bottom: 20pt;
}

h2 {
    font-size: 20pt;
    color: #2563eb;
    margin-top: 25pt;
    margin-bottom: 15pt;
}

h3 {
    font-size: 16pt;
    color: #3b82f6;
    margin-top: 20pt;
    margin-bottom: 10pt;
}
```

### Paragraphs

```html
<p>This is a paragraph. It can contain multiple sentences and will
   automatically wrap to fit the page width.</p>

<p>Another paragraph with proper spacing.</p>
```

**Styling paragraphs:**

```css
p {
    line-height: 1.6;
    margin-bottom: 10pt;
    text-align: justify; /* or left, right, center */
}
```

### Text Formatting

```html
<!-- Bold text -->
<strong>Important text</strong> or <b>Bold text</b>

<!-- Italic text -->
<em>Emphasized text</em> or <i>Italic text</i>

<!-- Underline -->
<u>Underlined text</u>

<!-- Strikethrough -->
<s>Deleted text</s> or <del>Removed content</del>

<!-- Highlighted -->
<mark>Highlighted text</mark>

<!-- Small text -->
<small>Fine print</small>

<!-- Superscript and subscript -->
E = mc<sup>2</sup>
H<sub>2</sub>O

<!-- Code -->
Use the <code>Document.ParseDocument()</code> method.
```

### Preformatted Text

```html
<pre>
    This text preserves
        spacing and
    line breaks exactly
        as written.
</pre>
```

**Styling:**

```css
pre {
    font-family: 'Courier New', monospace;
    background-color: #f3f4f6;
    border: 1pt solid #d1d5db;
    padding: 10pt;
    overflow-x: auto;
}
```

### Block Quotes

```html
<blockquote>
    <p>"The best way to predict the future is to invent it."</p>
    <cite>— Alan Kay</cite>
</blockquote>
```

**Styling:**

```css
blockquote {
    border-left: 4pt solid #2563eb;
    padding-left: 20pt;
    margin: 20pt 0;
    font-style: italic;
    color: #4b5563;
}

cite {
    display: block;
    margin-top: 10pt;
    font-size: 10pt;
    font-style: normal;
}
```

---

## Images

### Basic Image

```html
<img src="./images/logo.png" alt="Company Logo" />
```

### Image with Sizing

```html
<img src="./images/photo.jpg"
     alt="Photo"
     style="width: 200pt; height: 150pt;" />

<!-- Maintain aspect ratio -->
<img src="./images/photo.jpg"
     alt="Photo"
     style="width: 200pt; height: auto;" />
```

### Image Sources

```html
<!-- Local file (relative path) -->
<img src="./images/logo.png" />

<!-- Local file (absolute path) -->
<img src="/Users/username/images/logo.png" />

<!-- Remote URL -->
<img src="https://example.com/image.jpg" />

<!-- Base64 embedded -->
<img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUg..." />
```

### Styled Images

```css
.logo {
    width: 120pt;
    height: auto;
    display: block;
    margin: 20pt auto;
}

.photo {
    width: 300pt;
    border: 2pt solid #d1d5db;
    border-radius: 5pt;
    padding: 5pt;
}

.thumbnail {
    width: 80pt;
    height: 80pt;
    object-fit: cover;
    border-radius: 50%;
}
```

### Image Alignment

```html
<!-- Center aligned -->
<div style="text-align: center;">
    <img src="logo.png" style="width: 150pt;" />
</div>

<!-- Float left with text wrap -->
<img src="photo.jpg"
     style="float: left; margin-right: 15pt; margin-bottom: 10pt; width: 120pt;" />
<p>Text flows around the image...</p>

<!-- Float right -->
<img src="photo.jpg"
     style="float: right; margin-left: 15pt; margin-bottom: 10pt; width: 120pt;" />
<p>Text flows around the image...</p>
```

---

## Lists

### Unordered Lists (Bullets)

```html
<ul>
    <li>First item</li>
    <li>Second item</li>
    <li>Third item</li>
</ul>
```

### Ordered Lists (Numbers)

```html
<ol>
    <li>First step</li>
    <li>Second step</li>
    <li>Third step</li>
</ol>
```

### Nested Lists

```html
<ul>
    <li>Main item 1
        <ul>
            <li>Sub-item 1.1</li>
            <li>Sub-item 1.2</li>
        </ul>
    </li>
    <li>Main item 2
        <ul>
            <li>Sub-item 2.1</li>
            <li>Sub-item 2.2</li>
        </ul>
    </li>
</ul>
```

### Definition Lists

```html
<dl>
    <dt>PDF</dt>
    <dd>Portable Document Format</dd>

    <dt>HTML</dt>
    <dd>HyperText Markup Language</dd>

    <dt>CSS</dt>
    <dd>Cascading Style Sheets</dd>
</dl>
```

### Styling Lists

```css
ul {
    margin: 15pt 0;
    padding-left: 30pt;
}

li {
    margin-bottom: 8pt;
    line-height: 1.5;
}

/* Custom bullet style */
ul {
    list-style-type: square; /* or circle, disc, none */
}

/* Custom numbers */
ol {
    list-style-type: decimal; /* or lower-alpha, upper-roman, etc. */
}

/* Inline list items */
ul.horizontal li {
    display: inline-block;
    margin-right: 20pt;
}
```

---

## Tables

### Basic Table

```html
<table>
    <tr>
        <th>Header 1</th>
        <th>Header 2</th>
        <th>Header 3</th>
    </tr>
    <tr>
        <td>Data 1</td>
        <td>Data 2</td>
        <td>Data 3</td>
    </tr>
    <tr>
        <td>Data 4</td>
        <td>Data 5</td>
        <td>Data 6</td>
    </tr>
</table>
```

### Table with Header and Body

```html
<table>
    <thead>
        <tr>
            <th>Product</th>
            <th>Price</th>
            <th>Quantity</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Widget A</td>
            <td>$10.00</td>
            <td>5</td>
        </tr>
        <tr>
            <td>Widget B</td>
            <td>$15.00</td>
            <td>3</td>
        </tr>
    </tbody>
</table>
```

### Table with Footer

```html
<table>
    <thead>
        <tr>
            <th>Item</th>
            <th>Amount</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Item 1</td>
            <td>$100.00</td>
        </tr>
        <tr>
            <td>Item 2</td>
            <td>$200.00</td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
            <td><strong>Total</strong></td>
            <td><strong>$300.00</strong></td>
        </tr>
    </tfoot>
</table>
```

### Styled Table

```css
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

tfoot {
    background-color: #eff6ff;
    font-weight: bold;
}
```

### Column Widths

```html
<table style="width: 100%;">
    <colgroup>
        <col style="width: 40%;" />
        <col style="width: 30%;" />
        <col style="width: 30%;" />
    </colgroup>
    <tr>
        <th>Wide Column</th>
        <th>Medium Column</th>
        <th>Medium Column</th>
    </tr>
    <!-- rows -->
</table>
```

---

## Links

### External Links

```html
<a href="https://example.com">Visit Example.com</a>
```

### Internal Links (Anchors)

```html
<!-- Define anchor point -->
<h2 id="section-2">Section 2</h2>

<!-- Link to anchor -->
<a href="#section-2">Go to Section 2</a>
```

### Email Links

```html
<a href="mailto:contact@example.com">Email Us</a>
```

### Styled Links

```css
a {
    color: #2563eb;
    text-decoration: none;
}

a:hover {
    text-decoration: underline;
}

/* External link indicator */
a[href^="http"]::after {
    content: " ↗";
    font-size: 0.8em;
}
```

---

## Practical Example: Complete Document

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Product Catalog</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            color: #333;
        }

        h1 {
            color: #1e40af;
            font-size: 28pt;
            border-bottom: 3pt solid #1e40af;
            padding-bottom: 10pt;
            margin-bottom: 20pt;
        }

        h2 {
            color: #2563eb;
            font-size: 20pt;
            margin-top: 30pt;
            margin-bottom: 15pt;
        }

        .product {
            page-break-inside: avoid;
            margin-bottom: 30pt;
        }

        .product img {
            width: 150pt;
            float: left;
            margin-right: 20pt;
            border: 1pt solid #d1d5db;
        }

        .product-info {
            overflow: auto;
        }

        .price {
            font-size: 18pt;
            color: #059669;
            font-weight: bold;
        }

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
    <h1>Product Catalog 2025</h1>

    <h2>Featured Products</h2>

    <!-- Product 1 -->
    <div class="product">
        <img src="./images/product1.jpg" alt="Product 1" />
        <div class="product-info">
            <h3>Premium Widget</h3>
            <p class="price">$299.99</p>
            <p>High-quality widget with advanced features. Perfect for
               professional use.</p>
            <ul>
                <li>Durable construction</li>
                <li>5-year warranty</li>
                <li>Free shipping</li>
            </ul>
        </div>
    </div>

    <!-- Product 2 -->
    <div class="product">
        <img src="./images/product2.jpg" alt="Product 2" />
        <div class="product-info">
            <h3>Standard Widget</h3>
            <p class="price">$149.99</p>
            <p>Reliable widget for everyday use. Great value for money.</p>
            <ul>
                <li>Compact design</li>
                <li>2-year warranty</li>
                <li>Multiple colors</li>
            </ul>
        </div>
    </div>

    <h2>Price Comparison</h2>
    <table>
        <thead>
            <tr>
                <th>Product</th>
                <th>Features</th>
                <th>Warranty</th>
                <th>Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Premium Widget</td>
                <td>Advanced</td>
                <td>5 years</td>
                <td>$299.99</td>
            </tr>
            <tr>
                <td>Standard Widget</td>
                <td>Basic</td>
                <td>2 years</td>
                <td>$149.99</td>
            </tr>
            <tr>
                <td>Economy Widget</td>
                <td>Essential</td>
                <td>1 year</td>
                <td>$79.99</td>
            </tr>
        </tbody>
    </table>

    <div class="footer">
        Page <page-number /> of <page-count /> | www.example.com | 1-800-WIDGETS
    </div>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Biography Page

Create a biography with:
- Heading with person's name
- Photo floated to the left
- 2-3 paragraphs of text
- Bulleted list of achievements
- Quote in a blockquote

### Exercise 2: Menu Document

Create a restaurant menu with:
- Header with restaurant name and logo
- Multiple sections (Appetizers, Entrees, Desserts)
- Table of items with descriptions and prices
- Footer with contact information

### Exercise 3: Technical Document

Create a technical guide with:
- Table of contents with anchor links
- Multiple sections with headings
- Code snippets in `<pre>` tags
- Warning boxes with styled borders
- Numbered steps in an ordered list

---

## Common Pitfalls

### ❌ Large Images Without Sizing

```html
<!-- May cause layout issues -->
<img src="huge-photo.jpg" />
```

✅ **Solution:** Always specify dimensions

```html
<img src="huge-photo.jpg" style="width: 300pt; height: auto;" />
```

### ❌ Tables Without Column Widths

```html
<!-- May render slowly -->
<table>
    <tr>
        <td>Very long content...</td>
        <td>Short</td>
    </tr>
</table>
```

✅ **Solution:** Specify column widths

```html
<table>
    <colgroup>
        <col style="width: 70%;" />
        <col style="width: 30%;" />
    </colgroup>
    <!-- rows -->
</table>
```

### ❌ Mixing Content and Presentation

```html
<p><font size="14" color="red">Text</font></p>
```

✅ **Solution:** Use CSS

```html
<p style="font-size: 14pt; color: red;">Text</p>
```

---

## Next Steps

Now that you can add basic content:

1. **[Output Options](07_output_options.md)** - Configure PDF generation
2. **[Content Components](/learning/06-content/)** - Advanced content types
3. **[Data Binding](/learning/02-data-binding/)** - Dynamic content

---

**Continue learning →** [Output Options](07_output_options.md)
