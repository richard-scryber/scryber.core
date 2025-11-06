---
layout: default
title: Styling & Appearance
nav_order: 3
parent: Learning Guides
parent_url: /learning/
has_children: true
has_toc: false
---

# Styling & Appearance

Master CSS styling to create beautiful, professional-looking PDF documents with Scryber.Core.

---

## Table of Contents

1. [CSS Selectors & Specificity](01_css_selectors_specificity.md) - Element, class, ID selectors, cascade, inheritance
2. [Colors & Backgrounds](02_colors_backgrounds.md) - Color formats, backgrounds, images, positioning
3. [Borders & Spacing](03_borders_spacing.md) - Borders, margins, padding, box model
4. [Units & Measurements](04_units_measurements.md) - Absolute/relative units, calc() function
5. [Text Styling](05_text_styling.md) - Font properties, alignment, decoration
6. [Display & Visibility](06_display_visibility.md) - Display modes, visibility, conditional display
7. [Style Organization](07_style_organization.md) - Inline/embedded/external styles, maintenance
8. [Styling Best Practices](08_styling_best_practices.md) - Performance, troubleshooting, patterns

---

## Overview

Styling transforms plain content into polished, professional documents. This series teaches you how to use CSS effectively in PDF generation, covering everything from basic colors to advanced measurements and calculations.

## What Makes PDF Styling Different?

While Scryber uses familiar CSS syntax, PDF styling has unique characteristics:

- **Page-Based Layout** - Content flows across pages, not continuous scroll
- **Print Units** - Points (pt) are standard, not pixels
- **Fixed Dimensions** - Pages have defined sizes (Letter, A4, etc.)
- **Subset of CSS** - Not all CSS properties are supported
- **No Browser Quirks** - Consistent rendering every time

## Quick Example

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        body {
            font-family: 'Arial', sans-serif;
            font-size: 11pt;
            margin: 40pt;
            color: #333;
        }

        h1 {
            color: #2c5282;
            font-size: 24pt;
            border-bottom: 2pt solid #2c5282;
            padding-bottom: 10pt;
            margin-bottom: 20pt;
        }

        .highlight {
            background-color: #fef3c7;
            padding: 10pt;
            border-left: 4pt solid #f59e0b;
        }

        .box {
            border: 1pt solid #cbd5e0;
            border-radius: 5pt;
            padding: 15pt;
            margin: 10pt 0;
        }
    </style>
</head>
<body>
    <h1>Professional Document</h1>
    <div class="highlight">
        <p>Important information stands out!</p>
    </div>
    <div class="box">
        <p>Clean, organized content.</p>
    </div>
</body>
</html>
```

## What You'll Learn

This series covers comprehensive CSS styling for PDFs:

### 1. [CSS Selectors & Specificity](01_css_selectors_specificity.md)
- Element, class, and ID selectors
- Attribute selectors
- Pseudo-classes
- Specificity rules and cascade
- Style inheritance

### 2. [Colors & Backgrounds](02_colors_backgrounds.md)
- Color formats (hex, rgb, named colors)
- Text and background colors
- Transparency and opacity
- Background images
- Background positioning and repeat

### 3. [Borders & Spacing](03_borders_spacing.md)
- Border width, style, and color
- Individual border sides
- Border radius
- Margin and padding
- The box model

### 4. [Units & Measurements](04_units_measurements.md)
- **Absolute units** (pt, px, in, cm, mm)
- **Relative units** (%, em, rem)
- **calc() function** for calculations
- Unit conversions
- Best practices for PDF units

### 5. [Text Styling](05_text_styling.md)
- Font size, weight, and style
- Text color and alignment
- Line height and letter spacing
- Text decoration and transform
- Text indentation

### 6. [Display & Visibility](06_display_visibility.md)
- Display property (block, inline, inline-block, none)
- Visibility control
- Hidden attribute
- Conditional display with data binding

### 7. [Style Organization](07_style_organization.md)
- Inline vs embedded vs external styles
- Creating reusable style classes
- Multiple classes
- External stylesheets
- Organizing and maintaining styles

### 8. [Styling Best Practices](08_styling_best_practices.md)
- Performance optimization
- Browser vs PDF differences
- Common pitfalls and solutions
- Troubleshooting styling issues
- Maintainable patterns

## Prerequisites

Before starting this series:

- **Complete [Getting Started](/learning/01-getting-started/)** - Basic Scryber knowledge
- **HTML Fundamentals** - Understanding of HTML structure
- **Basic CSS** - Familiarity with CSS concepts

## Key Concepts

### The Box Model

Every element in a PDF is a box with:

```
┌─────────────────────────────────┐
│         Margin (transparent)     │
│  ┌───────────────────────────┐  │
│  │   Border                  │  │
│  │  ┌─────────────────────┐  │  │
│  │  │  Padding            │  │  │
│  │  │  ┌───────────────┐  │  │  │
│  │  │  │   Content     │  │  │  │
│  │  │  └───────────────┘  │  │  │
│  │  └─────────────────────┘  │  │
│  └───────────────────────────┘  │
└─────────────────────────────────┘
```

### CSS Cascade

Styles are applied in order of specificity:

1. **Inline styles** - `style="color: red"` (highest priority)
2. **ID selectors** - `#header { }`
3. **Class selectors** - `.title { }`
4. **Element selectors** - `h1 { }` (lowest priority)

### Inheritance

Some properties inherit from parent to child:
- Font properties (family, size, weight)
- Text properties (color, alignment)
- Line height

Others don't inherit:
- Borders
- Margins and padding
- Background colors

## Units for PDF

### Absolute Units (Recommended)

```css
/* Points - standard PDF unit */
font-size: 12pt;
margin: 40pt;

/* Inches - useful for physical dimensions */
width: 8.5in;
height: 11in;

/* Millimeters - metric measurements */
margin: 25mm;

/* Centimeters */
padding: 2cm;

/* Pixels - converts to points (96dpi) */
width: 612px;  /* Equivalent to 8.5in */
```

### Relative Units

```css
/* Percentage - relative to parent */
width: 100%;
margin-left: 10%;

/* Em - relative to current font size */
padding: 1.5em;  /* If font is 12pt, padding is 18pt */

/* Rem - relative to root font size */
margin: 2rem;
```

### Calculations with calc()

```css
/* Mixed units */
width: calc(100% - 40pt);

/* Complex calculations */
height: calc((100% / 3) - 10pt);
```

{% raw %}
```css
/* With data binding */
width: calc({{itemCount}} * 50pt);
```
{% endraw %}

## Common Styling Patterns

### Professional Header

```css
.header {
    background: linear-gradient(to right, #1e40af, #3b82f6);
    color: white;
    padding: 20pt;
    margin-bottom: 30pt;
}

.header h1 {
    margin: 0;
    font-size: 28pt;
    font-weight: bold;
}
```

### Bordered Box

```css
.info-box {
    border: 1pt solid #d1d5db;
    border-radius: 8pt;
    padding: 15pt;
    margin: 10pt 0;
    background-color: #f9fafb;
}
```

### Alternating Rows

```css
table tr:nth-child(even) {
    background-color: #f3f4f6;
}

table tr:nth-child(odd) {
    background-color: white;
}
```

### Watermark

```css
.watermark {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: rotate(-45deg);
    font-size: 72pt;
    color: rgba(0, 0, 0, 0.1);
    z-index: -1;
}
```

## Color Best Practices

### Accessible Colors

```css
/* Good contrast for readability */
body {
    color: #1f2937;  /* Dark gray, not pure black */
    background-color: white;
}

/* Sufficient contrast for headers */
h1 {
    color: #1e40af;  /* Dark blue */
}
```

### Brand Colors

```css
:root {
    --brand-primary: #2563eb;
    --brand-secondary: #7c3aed;
    --brand-accent: #f59e0b;
    --gray-light: #f3f4f6;
    --gray-dark: #374151;
}

.header {
    background-color: var(--brand-primary);
}
```

## Real-World Examples

### Invoice Styling

```css
.invoice-header {
    background-color: #1e3a8a;
    color: white;
    padding: 25pt;
    margin-bottom: 20pt;
}

.invoice-table {
    width: 100%;
    border-collapse: collapse;
}

.invoice-table th {
    background-color: #eff6ff;
    padding: 10pt;
    text-align: left;
    border-bottom: 2pt solid #1e3a8a;
}

.invoice-table td {
    padding: 8pt;
    border-bottom: 1pt solid #e5e7eb;
}

.total-row {
    background-color: #f0f9ff;
    font-weight: bold;
    font-size: 14pt;
}
```

### Certificate Styling

```css
.certificate {
    border: 10pt double #92400e;
    padding: 40pt;
    text-align: center;
    background-color: #fffbeb;
}

.certificate h1 {
    font-size: 36pt;
    color: #92400e;
    text-transform: uppercase;
    letter-spacing: 2pt;
    margin-bottom: 30pt;
}

.certificate .recipient {
    font-size: 28pt;
    font-style: italic;
    color: #1e40af;
    margin: 20pt 0;
}
```

## Learning Path

**Recommended progression:**

1. **Master Selectors** - Understand how to target elements
2. **Learn Colors & Backgrounds** - Visual fundamentals
3. **Understand Box Model** - Spacing and borders
4. **Work with Units** - Essential for precise layouts
5. **Style Text** - Typography basics
6. **Control Display** - Show/hide content
7. **Organize Styles** - Maintainable CSS
8. **Apply Best Practices** - Professional results

## Tips for Success

1. **Use Points for Print** - `pt` units are standard for PDF
2. **Test Early** - Generate PDFs frequently to see results
3. **Keep It Simple** - Start with basic styles, add complexity gradually
4. **Use Classes** - Reusable styles are more maintainable
5. **Check Contrast** - Ensure text is readable
6. **Avoid Browser-Specific CSS** - Stick to standard properties
7. **Use calc() for Flexibility** - Dynamic sizing and spacing

## Common Pitfalls

❌ **Using px without understanding conversion**
```css
/* May not render as expected */
font-size: 12px;
```

✅ **Use points for predictable results**
```css
font-size: 12pt;
```

❌ **Assuming all CSS properties work**
```css
/* Not supported in Scryber */
display: flex;
transition: all 0.3s;
```

✅ **Use supported properties**
```css
display: block;
/* Instant changes, no transitions */
```

## Next Steps

Ready to create beautiful PDFs? Start with [CSS Selectors & Specificity](01_css_selectors_specificity.md) to master targeting elements.

Jump ahead to specific topics:
- [Units & Measurements](04_units_measurements.md) for calc() and relative units
- [Colors & Backgrounds](02_colors_backgrounds.md) for visual styling
- [Styling Best Practices](08_styling_best_practices.md) for professional results

---

**Related Series:**
- [Getting Started](/learning/01-getting-started/) - Prerequisites
- [Typography & Fonts](/learning/05-typography/) - Advanced text styling
- [Layout & Positioning](/learning/04-layout/) - Page structure

---

**Begin styling →** [CSS Selectors & Specificity](01_css_selectors_specificity.md)
