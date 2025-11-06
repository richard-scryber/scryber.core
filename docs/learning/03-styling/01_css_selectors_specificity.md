---
layout: default
title: CSS Selectors & Specificity
nav_order: 1
parent: Styling & Appearance
parent_url: /learning/03-styling/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# CSS Selectors & Specificity

Master CSS selectors and understand how styles cascade and override in PDF generation.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Use element, class, and ID selectors effectively
- Apply attribute selectors
- Understand pseudo-classes and pseudo-elements
- Calculate specificity values
- Master the cascade and inheritance
- Debug conflicting styles

---

## CSS Selectors Overview

Selectors determine which elements receive specific styles. Scryber supports most standard CSS selectors used in web development.

---

## Element Selectors

Target elements by their HTML tag name.

```css
/* Style all paragraphs */
p {
    font-size: 11pt;
    line-height: 1.6;
    margin-bottom: 10pt;
}

/* Style all headings level 1 */
h1 {
    font-size: 24pt;
    color: #1e40af;
    margin-bottom: 20pt;
}

/* Style all tables */
table {
    width: 100%;
    border-collapse: collapse;
}
```

---

## Class Selectors

Target elements by their class attribute. Most flexible and commonly used.

**HTML:**
```html
<p class="highlight">This is highlighted text.</p>
<p class="warning">This is a warning.</p>
<p class="highlight important">This has multiple classes.</p>
```

**CSS:**
```css
/* Single class */
.highlight {
    background-color: #fef3c7;
    padding: 10pt;
}

.warning {
    color: #dc2626;
    border-left: 4pt solid #dc2626;
    padding-left: 15pt;
}

.important {
    font-weight: bold;
}
```

---

## ID Selectors

Target a single unique element by its ID attribute.

**HTML:**
```html
<div id="header">Company Header</div>
<div id="footer">Page Footer</div>
```

**CSS:**
```css
#header {
    background-color: #2563eb;
    color: white;
    padding: 20pt;
}

#footer {
    font-size: 9pt;
    color: #666;
    text-align: center;
}
```

**Best Practice:** Use IDs sparingly. Prefer classes for reusability.

---

## Descendant Selectors

Target elements inside other elements (any level deep).

```css
/* All paragraphs inside divs */
div p {
    margin: 10pt 0;
}

/* All links inside list items */
li a {
    color: #2563eb;
    text-decoration: none;
}

/* Headings inside .content class */
.content h2 {
    color: #1e40af;
    margin-top: 30pt;
}
```

---

## Child Selectors

Target direct children only (one level).

```css
/* Direct children only */
.container > p {
    font-size: 12pt;
}

/* Direct list items */
ul > li {
    margin-bottom: 8pt;
}
```

**Difference:**
- `div p` matches all `<p>` at any depth inside `<div>`
- `div > p` matches only direct child `<p>` elements

---

## Multiple Selectors

Apply the same styles to multiple selectors.

```css
/* Same styles for h1, h2, and h3 */
h1, h2, h3 {
    color: #1e40af;
    font-weight: bold;
}

/* Multiple classes and elements */
.highlight, .important, strong {
    font-weight: bold;
}
```

---

## Attribute Selectors

Target elements by their attributes.

### Attribute Existence

```css
/* Elements with title attribute */
[title] {
    border-bottom: 1pt dotted #666;
}

/* Links with href attribute */
a[href] {
    color: #2563eb;
}
```

### Exact Attribute Value

```css
/* Input type="text" */
input[type="text"] {
    border: 1pt solid #d1d5db;
    padding: 5pt;
}

/* Links with specific href */
a[href="https://example.com"] {
    color: #059669;
}
```

### Attribute Contains

```css
/* href contains "example" */
a[href*="example"] {
    color: #059669;
}

/* class contains "btn" */
[class*="btn"] {
    padding: 10pt 20pt;
    border-radius: 5pt;
}
```

### Attribute Starts With

```css
/* href starts with "https" */
a[href^="https"] {
    padding-right: 15pt;
}

/* class starts with "icon-" */
[class^="icon-"] {
    font-size: 14pt;
}
```

### Attribute Ends With

```css
/* src ends with ".pdf" */
a[href$=".pdf"]::after {
    content: " (PDF)";
    font-size: 8pt;
}
```

---

## Pseudo-Classes

Select elements based on their state or position.

### Structural Pseudo-Classes

```css
/* First child */
p:first-child {
    margin-top: 0;
}

/* Last child */
p:last-child {
    margin-bottom: 0;
}

/* Nth child */
tr:nth-child(even) {
    background-color: #f9fafb;
}

tr:nth-child(odd) {
    background-color: white;
}

/* Every 3rd element */
div:nth-child(3n) {
    page-break-before: always;
}

/* First of type */
h2:first-of-type {
    margin-top: 0;
}

/* Last of type */
p:last-of-type {
    margin-bottom: 0;
}
```

---

## Specificity

When multiple rules target the same element, specificity determines which style wins.

### Specificity Calculation

Specificity is calculated as: **(a, b, c, d)**

- **a** = Inline styles (1 if inline, 0 otherwise)
- **b** = Number of ID selectors
- **c** = Number of class selectors, attribute selectors, and pseudo-classes
- **d** = Number of element selectors and pseudo-elements

### Examples

```css
/* (0, 0, 0, 1) - One element */
p {
    color: black;
}

/* (0, 0, 1, 0) - One class */
.highlight {
    color: blue;
}

/* (0, 0, 1, 1) - One class + one element */
p.highlight {
    color: red;
}

/* (0, 1, 0, 0) - One ID */
#special {
    color: green;
}

/* (0, 1, 1, 1) - One ID + one class + one element */
div#special.highlight {
    color: purple;
}
```

### Specificity Rules

1. **Inline styles** always win (unless !important is used)
2. **Higher specificity** wins
3. **Equal specificity**: last rule wins (cascade)
4. **!important** overrides everything (use sparingly!)

### Practical Example

```html
<p id="intro" class="highlight">Hello World</p>
```

```css
p { color: black; }                  /* (0,0,0,1) */
.highlight { color: blue; }           /* (0,0,1,0) - WINS */
p.highlight { color: red; }           /* (0,0,1,1) - WINS */
#intro { color: green; }              /* (0,1,0,0) - WINS */
p#intro.highlight { color: purple; }  /* (0,1,1,1) - WINS */
```

**Result:** Text is purple (highest specificity)

---

## The Cascade

When specificity is equal, the last rule wins.

```css
p {
    color: black;
}

p {
    color: blue;  /* This wins - same specificity, later in file */
}
```

---

## Inheritance

Some CSS properties inherit from parent elements.

### Properties That Inherit

```css
/* These inherit to children */
body {
    font-family: Helvetica, sans-serif;  /* Inherits */
    font-size: 11pt;                      /* Inherits */
    color: #333;                          /* Inherits */
    line-height: 1.6;                     /* Inherits */
}
```

### Properties That Don't Inherit

```css
/* These don't inherit */
div {
    border: 1pt solid black;    /* Doesn't inherit */
    margin: 10pt;                /* Doesn't inherit */
    padding: 15pt;               /* Doesn't inherit */
    background-color: #f9fafb;   /* Doesn't inherit */
}
```

---

## Practical Examples

### Example 1: Document with Specificity

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Specificity Example</title>
    <style>
        /* Base styles - lowest specificity */
        p {
            color: #333;
            font-size: 11pt;
            line-height: 1.6;
        }

        /* Class selector - medium specificity */
        .important {
            color: #dc2626;
            font-weight: bold;
        }

        /* Element + class - higher specificity */
        p.important {
            border-left: 4pt solid #dc2626;
            padding-left: 15pt;
        }

        /* ID selector - very high specificity */
        #critical {
            color: white;
            background-color: #dc2626;
            padding: 10pt;
        }

        /* Combining selectors */
        .content p {
            margin-bottom: 15pt;
        }

        .content p:first-child {
            margin-top: 0;
        }

        .content p:last-child {
            margin-bottom: 0;
        }
    </style>
</head>
<body>
    <div class="content">
        <p>This is a regular paragraph.</p>
        <p class="important">This is an important paragraph.</p>
        <p id="critical">This is critical information!</p>
    </div>
</body>
</html>
```

### Example 2: Table with Alternating Rows

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Table Styling</title>
    <style>
        table {
            width: 100%;
            border-collapse: collapse;
            font-size: 10pt;
        }

        /* All cells */
        td, th {
            padding: 8pt;
            text-align: left;
        }

        /* Header cells */
        th {
            background-color: #2563eb;
            color: white;
            font-weight: bold;
        }

        /* Alternating row colors */
        tbody tr:nth-child(odd) {
            background-color: white;
        }

        tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        /* Specific column styling */
        td:first-child {
            font-weight: bold;
        }

        td:last-child {
            text-align: right;
        }

        /* Footer row */
        tfoot td {
            background-color: #eff6ff;
            font-weight: bold;
            border-top: 2pt solid #2563eb;
        }
    </style>
</head>
<body>
    <table>
        <thead>
            <tr>
                <th>Product</th>
                <th>Quantity</th>
                <th>Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget A</td>
                <td>5</td>
                <td>$50.00</td>
            </tr>
            <tr>
                <td>Widget B</td>
                <td>3</td>
                <td>$45.00</td>
            </tr>
            <tr>
                <td>Widget C</td>
                <td>2</td>
                <td>$40.00</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="2">Total</td>
                <td>$135.00</td>
            </tr>
        </tfoot>
    </table>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Specificity Challenge

Create styles with different specificities for the same element. Predict which style will win, then test it.

### Exercise 2: Table Styling

Create a table with:
- Header row with background color
- Alternating row colors (odd/even)
- First column bold
- Last column right-aligned
- Footer row with different styling

### Exercise 3: Nested Selectors

Create a document structure with:
- Container div
- Multiple sections inside
- Paragraphs and headings
- Use descendant and child selectors to style different levels

---

## Common Pitfalls

### ❌ Too Specific Selectors

```css
body div.container div.content p.text {
    color: blue;
}
```

**Problem:** Hard to override, difficult to maintain

✅ **Solution:** Keep selectors simple

```css
.text {
    color: blue;
}
```

### ❌ Over-using IDs

```css
#intro { color: blue; }
#header-intro { color: red; }
```

**Problem:** IDs have very high specificity, hard to override

✅ **Solution:** Use classes instead

```css
.intro { color: blue; }
.header-intro { color: red; }
```

### ❌ Relying on Cascade Order

```css
/* File 1 */
.button { background: blue; }

/* File 2 */
.button { background: red; }
```

**Problem:** Order-dependent, fragile

✅ **Solution:** Use unique, descriptive classes

```css
.button-primary { background: blue; }
.button-danger { background: red; }
```

---

## Debugging Specificity Issues

### Check Specificity Calculator

When styles don't apply as expected:

1. Calculate specificity of each rule
2. Check cascade order for equal specificity
3. Look for inherited vs direct styles
4. Check for !important (sparingly!)

### Example Debugging

```css
/* Not working? Check specificity! */
p { color: black; }           /* (0,0,0,1) */
.content p { color: blue; }   /* (0,0,1,1) - WINS */
```

To override:

```css
/* Increase specificity */
.highlight { color: red; }     /* (0,0,1,0) - Won't work */
p.highlight { color: red; }    /* (0,0,1,1) - Won't work */
.content p.highlight { color: red; }  /* (0,0,2,1) - WORKS! */
```

---

## Next Steps

Now that you understand selectors and specificity:

1. **[Colors & Backgrounds](02_colors_backgrounds.md)** - Apply colors and backgrounds
2. **[Borders & Spacing](03_borders_spacing.md)** - Control borders and box model
3. **[Units & Measurements](04_units_measurements.md)** - Master CSS units

---

**Continue learning →** [Colors & Backgrounds](02_colors_backgrounds.md)
