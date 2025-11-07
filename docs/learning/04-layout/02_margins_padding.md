---
layout: default
title: Margins & Padding
nav_order: 2
parent: Layout & Positioning
parent_url: /learning/04-layout/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Margins & Padding

Master spacing control with margins and padding for professional, well-structured PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Understand the difference between margins and padding
- Set page margins with @page rule
- Control element margins and padding
- Use shorthand and longhand properties
- Apply negative margins strategically
- Create consistent spacing systems
- Avoid common spacing pitfalls

---

## Understanding the Box Model

Every element in PDF layout follows the CSS box model:

```
┌─────────────────────────────────────┐
│          Margin (transparent)        │
│  ┌───────────────────────────────┐  │
│  │     Border                    │  │
│  │  ┌─────────────────────────┐  │  │
│  │  │   Padding (inside)      │  │  │
│  │  │  ┌───────────────────┐  │  │  │
│  │  │  │                   │  │  │  │
│  │  │  │     Content       │  │  │  │
│  │  │  │                   │  │  │  │
│  │  │  └───────────────────┘  │  │  │
│  │  │                         │  │  │
│  │  └─────────────────────────┘  │  │
│  │                               │  │
│  └───────────────────────────────┘  │
│                                     │
└─────────────────────────────────────┘
```

**Key differences:**
- **Margin:** Space *outside* the element (transparent, collapses with adjacent margins)
- **Padding:** Space *inside* the element (includes background color/image)
- **Border:** Line between margin and padding

---

## Page Margins

Control the printable area of each page.

### Setting Page Margins

```css
@page {
    size: Letter;
    margin: 1in;  /* All sides: 1 inch */
}
```

### Individual Page Margins

```css
@page {
    size: Letter;
    margin-top: 1in;
    margin-right: 0.75in;
    margin-bottom: 1in;
    margin-left: 1in;
}
```

### Shorthand Syntax

```css
/* One value: all sides */
@page {
    margin: 1in;  /* top, right, bottom, left */
}

/* Two values: vertical, horizontal */
@page {
    margin: 1in 0.75in;  /* top/bottom, left/right */
}

/* Three values: top, horizontal, bottom */
@page {
    margin: 0.5in 1in 1in;  /* top, left/right, bottom */
}

/* Four values: clockwise from top */
@page {
    margin: 0.5in 0.75in 1in 1.25in;  /* top, right, bottom, left */
}
```

### Common Page Margin Values

```css
/* Standard business document */
@page {
    size: Letter;
    margin: 1in;  /* 72pt on all sides */
}

/* Narrow margins (more content) */
@page {
    size: Letter;
    margin: 0.5in;  /* 36pt on all sides */
}

/* Wide margins (formal, elegant) */
@page {
    size: Letter;
    margin: 1.5in;  /* 108pt on all sides */
}

/* Asymmetric (for binding) */
@page {
    size: Letter;
    margin: 1in 0.75in 1in 1.25in;  /* Extra left margin for binding */
}
```

---

## Element Margins

Control spacing between elements.

### Basic Margins

```css
p {
    margin-bottom: 12pt;  /* Space after paragraphs */
}

h1 {
    margin-top: 0;
    margin-bottom: 24pt;  /* More space after headings */
}

.section {
    margin-bottom: 40pt;  /* Large section spacing */
}
```

### Margin Shorthand

```css
/* One value: all sides */
.box {
    margin: 20pt;
}

/* Two values: vertical, horizontal */
.box {
    margin: 20pt 40pt;  /* 20pt top/bottom, 40pt left/right */
}

/* Four values: top, right, bottom, left */
.box {
    margin: 10pt 20pt 30pt 40pt;
}
```

### Auto Margins (Centering)

```css
.centered {
    width: 400pt;
    margin-left: auto;
    margin-right: auto;
    /* Or shorthand: */
    margin: 0 auto;
}
```

### Negative Margins

Pull elements closer or create overlaps.

```css
.overlap {
    margin-top: -20pt;  /* Pull up by 20pt */
}

.extend-left {
    margin-left: -40pt;  /* Extend into left margin */
    padding-left: 40pt;  /* Add padding back for content */
}
```

---

## Margin Collapse

Adjacent vertical margins collapse to the larger value.

### Example of Collapse

```css
.box1 {
    margin-bottom: 30pt;
}

.box2 {
    margin-top: 20pt;
}
```

**Result:** Gap between boxes is **30pt** (not 50pt)
- The margins collapse to the larger value (30pt)
- Only vertical margins collapse
- Horizontal margins never collapse

### Preventing Collapse

```css
/* Add border or padding to prevent collapse */
.no-collapse {
    padding-top: 1pt;  /* Any padding prevents collapse */
}

/* Or use border */
.no-collapse {
    border-top: 1pt solid transparent;
}
```

---

## Element Padding

Space inside the element, between border and content.

### Basic Padding

```css
.box {
    padding: 20pt;  /* All sides */
}

.card {
    padding-top: 15pt;
    padding-right: 20pt;
    padding-bottom: 15pt;
    padding-left: 20pt;
}
```

### Padding Shorthand

```css
/* One value: all sides */
.box {
    padding: 15pt;
}

/* Two values: vertical, horizontal */
.box {
    padding: 15pt 20pt;  /* 15pt top/bottom, 20pt left/right */
}

/* Four values: top, right, bottom, left */
.box {
    padding: 10pt 15pt 20pt 25pt;
}
```

### Padding with Background

```css
.highlighted {
    background-color: #fef3c7;
    padding: 15pt;  /* Background extends into padding */
    margin-bottom: 20pt;
}
```

---

## Box-Sizing Property

Controls how width/height calculations work.

### content-box (Default)

Width/height applies to content only, padding/border added on top.

```css
.content-box {
    box-sizing: content-box;  /* Default */
    width: 200pt;
    padding: 20pt;
    border: 2pt solid black;
    /* Total width: 200 + 40 (padding) + 4 (border) = 244pt */
}
```

### border-box (Recommended)

Width/height includes padding and border.

```css
.border-box {
    box-sizing: border-box;  /* Recommended */
    width: 200pt;
    padding: 20pt;
    border: 2pt solid black;
    /* Total width: 200pt (includes padding and border) */
    /* Content width: 200 - 40 (padding) - 4 (border) = 156pt */
}
```

### Universal Box-Sizing (Best Practice)

```css
* {
    box-sizing: border-box;  /* Apply to all elements */
}
```

---

## Spacing Scale

Use consistent spacing values throughout your document.

### Defining a Scale

```css
:root {
    --space-xs: 5pt;
    --space-sm: 10pt;
    --space-md: 20pt;
    --space-lg: 30pt;
    --space-xl: 40pt;
}

/* Or use Tailwind-inspired scale */
.m-0 { margin: 0; }
.m-1 { margin: 10pt; }
.m-2 { margin: 20pt; }
.m-3 { margin: 30pt; }
.m-4 { margin: 40pt; }

.p-0 { padding: 0; }
.p-1 { padding: 10pt; }
.p-2 { padding: 20pt; }
.p-3 { padding: 30pt; }
.p-4 { padding: 40pt; }
```

### Using the Scale

```css
h1 {
    margin-bottom: var(--space-lg);  /* 30pt */
}

p {
    margin-bottom: var(--space-sm);  /* 10pt */
}

.card {
    padding: var(--space-md);  /* 20pt */
    margin-bottom: var(--space-lg);  /* 30pt */
}
```

---

## Practical Examples

### Example 1: Business Letter with Proper Margins

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Business Letter</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;  /* Standard 1-inch margins */
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
        }

        .letterhead {
            text-align: center;
            padding-bottom: 20pt;
            margin-bottom: 30pt;
            border-bottom: 2pt solid #2563eb;
        }

        .company-name {
            font-size: 20pt;
            font-weight: bold;
            color: #1e40af;
            margin-bottom: 10pt;
        }

        .address {
            margin: 0;
            padding: 0;
        }

        .date {
            margin-bottom: 30pt;
        }

        .recipient {
            margin-bottom: 30pt;
        }

        p {
            margin-top: 0;
            margin-bottom: 12pt;
        }

        .closing {
            margin-top: 40pt;
        }

        .signature {
            margin-top: 60pt;
        }
    </style>
</head>
<body>
    <div class="letterhead">
        <div class="company-name">Acme Corporation</div>
        <p class="address">
            123 Business Street, Suite 100<br/>
            New York, NY 10001<br/>
            (555) 123-4567
        </p>
    </div>

    <div class="date">January 15, 2025</div>

    <div class="recipient">
        <strong>Mr. John Smith</strong><br/>
        XYZ Company<br/>
        456 Commerce Ave<br/>
        Boston, MA 02101
    </div>

    <p>Dear Mr. Smith,</p>

    <p>Thank you for your recent inquiry regarding our services. We are pleased to provide you with the information you requested.</p>

    <p>Our company has been serving businesses like yours for over 20 years, providing comprehensive solutions tailored to your specific needs.</p>

    <p>We would be happy to schedule a consultation at your convenience to discuss how we can help your organization achieve its goals.</p>

    <div class="closing">
        <p>Sincerely,</p>
        <div class="signature">
            <strong>Jane Doe</strong><br/>
            Sales Director<br/>
            Acme Corporation
        </div>
    </div>
</body>
</html>
```

### Example 2: Card Layout with Consistent Spacing

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Card Layout</title>
    <style>
        @page {
            size: Letter;
            margin: 0.75in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            margin: 0;
        }

        h1 {
            color: #1e40af;
            margin-top: 0;
            margin-bottom: 30pt;
            padding-bottom: 15pt;
            border-bottom: 2pt solid #e5e7eb;
        }

        .card {
            border: 1pt solid #d1d5db;
            border-radius: 5pt;
            padding: 20pt;  /* Inner spacing */
            margin-bottom: 20pt;  /* Outer spacing */
            background-color: white;
        }

        .card-header {
            margin: -20pt -20pt 15pt -20pt;  /* Negative to extend to edges */
            padding: 15pt 20pt;  /* Inner spacing */
            background-color: #2563eb;
            color: white;
            border-radius: 5pt 5pt 0 0;  /* Round top corners only */
        }

        .card-title {
            margin: 0;
            font-size: 16pt;
            font-weight: bold;
        }

        .card-body p {
            margin-top: 0;
            margin-bottom: 12pt;
        }

        .card-body p:last-child {
            margin-bottom: 0;  /* No margin on last paragraph */
        }

        .card-footer {
            margin: 15pt -20pt -20pt -20pt;  /* Extend to edges */
            padding: 10pt 20pt;
            background-color: #f9fafb;
            border-top: 1pt solid #e5e7eb;
            border-radius: 0 0 5pt 5pt;  /* Round bottom corners only */
            font-size: 9pt;
            color: #666;
        }
    </style>
</head>
<body>
    <h1>Product Catalog</h1>

    <div class="card">
        <div class="card-header">
            <h2 class="card-title">Premium Widget</h2>
        </div>
        <div class="card-body">
            <p><strong>Price:</strong> $149.99</p>
            <p><strong>Description:</strong> Our flagship product with advanced features and premium build quality.</p>
            <p><strong>Features:</strong></p>
            <ul style="margin-top: 5pt; margin-bottom: 0;">
                <li>Durable construction</li>
                <li>5-year warranty</li>
                <li>Free shipping</li>
            </ul>
        </div>
        <div class="card-footer">
            SKU: WDG-PREM-001 | In Stock
        </div>
    </div>

    <div class="card">
        <div class="card-header">
            <h2 class="card-title">Standard Widget</h2>
        </div>
        <div class="card-body">
            <p><strong>Price:</strong> $79.99</p>
            <p><strong>Description:</strong> Reliable performance at an affordable price point.</p>
            <p><strong>Features:</strong></p>
            <ul style="margin-top: 5pt; margin-bottom: 0;">
                <li>Quality construction</li>
                <li>2-year warranty</li>
                <li>Standard shipping</li>
            </ul>
        </div>
        <div class="card-footer">
            SKU: WDG-STD-001 | In Stock
        </div>
    </div>
</body>
</html>
```

### Example 3: Consistent Spacing System

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Spacing System</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;
        }

        /* ==============================================
           SPACING SCALE
           ============================================== */
        :root {
            --space-xs: 5pt;
            --space-sm: 10pt;
            --space-md: 20pt;
            --space-lg: 30pt;
            --space-xl: 40pt;
        }

        /* ==============================================
           BASE STYLES
           ============================================== */
        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
        }

        h1 {
            font-size: 24pt;
            color: #1e40af;
            margin-top: 0;
            margin-bottom: var(--space-lg);
        }

        h2 {
            font-size: 18pt;
            color: #2563eb;
            margin-top: var(--space-xl);
            margin-bottom: var(--space-md);
        }

        h3 {
            font-size: 14pt;
            margin-top: var(--space-lg);
            margin-bottom: var(--space-sm);
        }

        p {
            margin-top: 0;
            margin-bottom: var(--space-sm);
        }

        /* ==============================================
           COMPONENTS
           ============================================== */
        .alert {
            padding: var(--space-md);
            margin-bottom: var(--space-md);
            border-radius: var(--space-xs);
            border-left: 4pt solid #2563eb;
        }

        .alert-info {
            background-color: #eff6ff;
            color: #1e40af;
        }

        .alert-success {
            background-color: #d1fae5;
            color: #065f46;
            border-left-color: #059669;
        }

        .section {
            margin-bottom: var(--space-xl);
        }

        .box {
            border: 1pt solid #d1d5db;
            padding: var(--space-md);
            margin-bottom: var(--space-md);
            border-radius: var(--space-xs);
        }

        ul {
            margin-top: var(--space-xs);
            margin-bottom: var(--space-sm);
            padding-left: 20pt;
        }

        li {
            margin-bottom: var(--space-xs);
        }
    </style>
</head>
<body>
    <h1>Document with Consistent Spacing</h1>

    <div class="alert alert-info">
        <strong>Note:</strong> This document uses a consistent spacing scale throughout.
    </div>

    <div class="section">
        <h2>Introduction</h2>
        <p>This document demonstrates the use of a consistent spacing scale using CSS custom properties (variables).</p>
        <p>All spacing follows the scale: 5pt, 10pt, 20pt, 30pt, 40pt.</p>
    </div>

    <div class="section">
        <h2>Benefits</h2>
        <div class="box">
            <h3>Visual Consistency</h3>
            <p>Using a spacing scale ensures visual consistency throughout your document.</p>
            <ul>
                <li>Predictable spacing relationships</li>
                <li>Professional appearance</li>
                <li>Easier maintenance</li>
            </ul>
        </div>

        <div class="box">
            <h3>Maintainability</h3>
            <p>Changes to the scale automatically propagate throughout the document.</p>
        </div>
    </div>

    <div class="alert alert-success">
        <strong>Success!</strong> You now understand how to implement consistent spacing.
    </div>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Page Margin Comparison

Create three versions of the same document:
- Version 1: 0.5in margins (narrow)
- Version 2: 1in margins (standard)
- Version 3: 1.5in margins (wide)

Compare how the content flows and readability changes.

### Exercise 2: Card Component

Create a card component with:
- Border and border-radius
- Internal padding for content
- Header and footer with negative margins extending to edges
- External margins for spacing between cards

### Exercise 3: Spacing Scale

Implement a complete spacing scale:
- Define CSS variables for spacing (xs, sm, md, lg, xl)
- Apply to headings, paragraphs, and components
- Try changing the scale values and see everything adjust

---

## Common Pitfalls

### ❌ Forgetting box-sizing

```css
.box {
    width: 200pt;
    padding: 20pt;
    border: 2pt solid black;
    /* Total width is actually 244pt! */
}
```

✅ **Solution:** Use border-box

```css
* {
    box-sizing: border-box;
}

.box {
    width: 200pt;  /* Includes padding and border */
    padding: 20pt;
    border: 2pt solid black;
}
```

### ❌ Inconsistent Spacing

```css
.element1 { margin-bottom: 17pt; }
.element2 { margin-bottom: 23pt; }
.element3 { margin-bottom: 11pt; }
```

✅ **Solution:** Use a spacing scale

```css
.element1 { margin-bottom: 20pt; }  /* From scale */
.element2 { margin-bottom: 20pt; }
.element3 { margin-bottom: 10pt; }
```

### ❌ Not Accounting for Margin Collapse

```css
.box1 { margin-bottom: 30pt; }
.box2 { margin-top: 30pt; }
/* Expecting 60pt gap, but get 30pt */
```

✅ **Solution:** Use margin on one side only

```css
.box { margin-bottom: 30pt; }
/* Or use padding to prevent collapse */
```

### ❌ Using Padding Instead of Margin

```css
/* Padding extends background */
.element {
    background-color: yellow;
    padding-bottom: 40pt;  /* Yellow extends down */
}
```

✅ **Solution:** Use margin for external spacing

```css
.element {
    background-color: yellow;
    padding: 20pt;  /* Internal spacing */
    margin-bottom: 40pt;  /* External spacing, no background */
}
```

### ❌ Negative Margins Without Understanding

```css
.overlap {
    margin-top: -100pt;  /* May overlap content unintentionally */
}
```

✅ **Solution:** Use negative margins carefully

```css
.controlled-overlap {
    margin-top: -20pt;  /* Small, intentional overlap */
    position: relative;
    z-index: 10;  /* Control stacking if needed */
}
```

---

## Spacing Checklist

- [ ] @page margins defined (typically 0.5in to 1in)
- [ ] box-sizing: border-box applied globally
- [ ] Consistent spacing scale used
- [ ] Margins used for external spacing
- [ ] Padding used for internal spacing
- [ ] Margin collapse understood and controlled
- [ ] Last child margins removed where appropriate
- [ ] Negative margins used sparingly and intentionally

---

## Best Practices

1. **Use border-box** - Apply `* { box-sizing: border-box; }` globally
2. **Define a spacing scale** - Use consistent values (10pt, 20pt, 30pt, etc.)
3. **Margin for external spacing** - Between elements
4. **Padding for internal spacing** - Inside elements
5. **Remove last-child margins** - Prevent extra space at end of containers
6. **Use CSS variables** - For maintainable spacing systems
7. **Test margin collapse** - Understand vertical margin behavior
8. **Document your scale** - Comment your spacing decisions

---

## Next Steps

Now that you master margins and padding:

1. **[Sections & Page Breaks](03_sections_page_breaks.md)** - Control multi-page layouts
2. **[Positioning](05_positioning.md)** - Absolute and relative positioning
3. **[Styling Best Practices](/learning/03-styling/08_styling_best_practices.md)** - Professional patterns

---

**Continue learning →** [Sections & Page Breaks](03_sections_page_breaks.md)
