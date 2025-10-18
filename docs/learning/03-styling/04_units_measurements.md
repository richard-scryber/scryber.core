---
layout: default
title: Units & Measurements
nav_order: 4
parent: Styling & Appearance
parent_url: /learning/03-styling/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Units & Measurements

Master CSS measurement units for precise control of sizes, spacing, and layout in PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Use absolute units (pt, px, in, cm, mm)
- Apply relative units (%, em, rem)
- Understand unit conversions
- Use calc() for calculations
- Choose appropriate units for different scenarios
- Create consistent, scalable designs

---

## Why Units Matter in PDF

Unlike web browsers with variable screen sizes, PDFs have fixed dimensions. Understanding units is crucial for:
- **Precise positioning** of elements
- **Consistent sizing** across documents
- **Print-ready output** with correct dimensions
- **Scalable designs** that adapt to page sizes

---

## Absolute Units

Fixed measurements that don't change based on context.

### Points (pt) - Recommended for PDF

The standard unit in print and PDF. **1 point = 1/72 inch.**

```css
/* Points are ideal for PDF */
body {
    font-size: 11pt;
    line-height: 16pt;
}

h1 {
    font-size: 24pt;
    margin-bottom: 20pt;
}

.box {
    width: 400pt;
    padding: 15pt;
    border: 1pt solid black;
}
```

**Why use points?**
- Native PDF unit
- Print industry standard
- Precise, predictable sizing
- Direct mapping to physical dimensions

### Pixels (px)

Absolute in PDF context (not device-dependent like in web).

```css
/* Pixels work but less precise */
.element {
    width: 300px;
    font-size: 12px;
    margin: 10px;
}
```

**Conversion:** 1px = 0.75pt (typically)

### Inches (in)

Physical measurement units.

```css
/* Inches for page margins */
@page {
    size: Letter;
    margin: 1in;  /* 72pt */
}

.sidebar {
    width: 2in;  /* 144pt */
}
```

**Conversion:** 1in = 72pt = 96px

### Centimeters (cm) and Millimeters (mm)

Metric measurements.

```css
/* Metric units */
@page {
    size: A4;
    margin: 2.5cm;  /* ~71pt */
}

.box {
    width: 10cm;   /* ~283pt */
    height: 50mm;  /* ~142pt */
}
```

**Conversions:**
- 1cm = 28.35pt
- 1mm = 2.835pt
- 10mm = 1cm

---

## Relative Units

Values relative to other measurements.

### Percentages (%)

Relative to parent element's size.

```css
/* Width relative to parent */
.container {
    width: 100%;  /* Full width of parent */
}

.half {
    width: 50%;   /* Half of parent width */
}

.sidebar {
    width: 25%;   /* Quarter of parent width */
}

/* Height percentages (require parent height) */
.full-height {
    height: 100%;
}
```

**Common Uses:**
- Fluid layouts
- Responsive sizing
- Proportional spacing

### EM Units

Relative to parent element's font size.

```css
body {
    font-size: 12pt;
}

h1 {
    font-size: 2em;      /* 24pt (2 × 12pt) */
    margin-bottom: 1em;  /* 24pt (1 × 24pt, h1's font size) */
}

p {
    font-size: 1em;      /* 12pt (1 × 12pt) */
    margin-bottom: 1em;  /* 12pt (1 × 12pt) */
}

.small {
    font-size: 0.875em;  /* 10.5pt (0.875 × 12pt) */
}
```

**Key Point:** `1em` equals the element's font size, not the parent's.

### REM Units

Relative to root (html/body) element's font size.

```css
html {
    font-size: 12pt;  /* Base size */
}

h1 {
    font-size: 2rem;      /* 24pt (2 × 12pt) */
    margin-bottom: 1.5rem; /* 18pt (1.5 × 12pt) */
}

p {
    font-size: 1rem;      /* 12pt (1 × 12pt) */
    margin-bottom: 1rem;  /* 12pt (1 × 12pt) */
}

.small {
    font-size: 0.875rem;  /* 10.5pt (0.875 × 12pt) */
}
```

**Advantage:** More predictable than em, always relative to root.

---

## The calc() Function

Perform calculations mixing different units.

### Basic Arithmetic

```css
/* Addition */
.box {
    width: calc(100% - 40pt);  /* Full width minus 40pt */
}

/* Subtraction */
.content {
    height: calc(100% - 2in);  /* Full height minus 2 inches */
}

/* Multiplication */
.double {
    width: calc(200pt * 2);  /* 400pt */
}

/* Division */
.third {
    width: calc(100% / 3);  /* One third */
}
```

### Practical Examples

```css
/* Sidebar layout */
.sidebar {
    width: 200pt;
}

.main-content {
    width: calc(100% - 200pt);  /* Remaining width */
}

/* Centered with fixed margins */
.centered {
    width: calc(100% - 2 * 40pt);  /* Full width minus margins */
    margin: 0 40pt;
}

/* Grid columns with gaps */
.column {
    width: calc((100% - 60pt) / 3);  /* 3 columns with 20pt gaps */
}
```

### Complex Calculations

```css
/* Nested calculations */
.complex {
    width: calc((100% - 3 * 20pt) / 4);  /* 4 columns with gaps */
    margin-right: 20pt;
}

/* Mixing units */
.mixed {
    height: calc(100vh - 2in - 40pt);  /* Viewport minus fixed sizes */
}

/* With percentages */
.proportional {
    padding: calc(5% + 10pt);  /* Percentage plus fixed */
}
```

---

## Unit Conversion Reference

### Common Conversions

| From | To | Multiply by |
|------|-----|-------------|
| 1 in | pt | 72 |
| 1 cm | pt | 28.35 |
| 1 mm | pt | 2.835 |
| 1 px | pt | 0.75 |
| 1 in | cm | 2.54 |
| 1 in | mm | 25.4 |

### Conversion Examples

```css
/* 1 inch = 72 points */
.inch { width: 1in; }    /* Same as width: 72pt; */

/* 1 cm ≈ 28.35 points */
.cm { width: 1cm; }      /* Same as width: 28.35pt; */

/* 10mm = 1cm ≈ 28.35 points */
.mm { width: 10mm; }     /* Same as width: 28.35pt; */
```

---

## Choosing the Right Unit

### Use Points (pt) For:
- **Font sizes** - Standard for typography
- **Line heights** - Precise spacing
- **Borders** - 1pt, 2pt borders
- **Small spacing** - Padding, margins
- **Fixed dimensions** - Element sizes

```css
.recommended {
    font-size: 11pt;
    line-height: 16pt;
    padding: 15pt;
    border: 1pt solid black;
    margin-bottom: 20pt;
}
```

### Use Inches (in) For:
- **Page margins** - Physical page spacing
- **Large measurements** - When thinking in physical units
- **Print specifications** - Matching print requirements

```css
@page {
    margin: 1in;  /* Standard page margin */
}

.section {
    margin-bottom: 0.5in;
}
```

### Use Percentages (%) For:
- **Fluid widths** - Responsive to parent
- **Proportional sizing** - Relative layouts
- **Scalable designs** - Adapt to container

```css
.fluid {
    width: 100%;  /* Full width */
}

.half {
    width: 50%;   /* Half width */
}
```

### Use REM For:
- **Scalable spacing** - Consistent throughout document
- **Responsive typography** - Font size relationships
- **Maintainable layouts** - Easy to adjust globally

```css
html {
    font-size: 12pt;  /* Base size */
}

.scalable {
    font-size: 1.5rem;     /* Scales with base */
    margin-bottom: 1rem;   /* Consistent spacing */
}
```

---

## Practical Examples

### Example 1: Standard Document Layout

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Standard Layout</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;  /* Physical margin */
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;  /* Base font size */
            line-height: 1.6;
            margin: 0;
        }

        h1 {
            font-size: 24pt;      /* Fixed size */
            margin-bottom: 20pt;  /* Fixed spacing */
            color: #1e40af;
        }

        h2 {
            font-size: 18pt;
            margin-top: 30pt;
            margin-bottom: 15pt;
            color: #2563eb;
        }

        p {
            margin-bottom: 12pt;
        }

        .box {
            border: 1pt solid #d1d5db;
            padding: 15pt;
            margin-bottom: 20pt;
        }
    </style>
</head>
<body>
    <h1>Document Title</h1>
    <p>This document uses standard point measurements for consistent sizing.</p>

    <h2>Section Heading</h2>
    <div class="box">
        <p>Content in a box with fixed padding and borders.</p>
    </div>
</body>
</html>
```

### Example 2: Fluid Layout with calc()

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Fluid Layout</title>
    <style>
        * {
            box-sizing: border-box;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            margin: 40pt;
        }

        .container {
            display: table;
            width: 100%;
        }

        .sidebar {
            display: table-cell;
            width: 200pt;  /* Fixed sidebar */
            background-color: #f9fafb;
            padding: 15pt;
            vertical-align: top;
        }

        .main-content {
            display: table-cell;
            width: calc(100% - 200pt);  /* Remaining width */
            padding: 15pt;
            padding-left: 30pt;
            vertical-align: top;
        }

        .card {
            border: 1pt solid #d1d5db;
            border-radius: 5pt;
            padding: 15pt;
            margin-bottom: 15pt;
        }

        .card h3 {
            margin: 0 0 10pt 0;
            color: #1e40af;
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="sidebar">
            <h3>Navigation</h3>
            <p>Section 1</p>
            <p>Section 2</p>
            <p>Section 3</p>
        </div>

        <div class="main-content">
            <h1>Main Content</h1>

            <div class="card">
                <h3>Card Title</h3>
                <p>This card uses a fluid layout that adapts to the available space.</p>
            </div>

            <div class="card">
                <h3>Another Card</h3>
                <p>The main content area uses calc() to fill remaining space.</p>
            </div>
        </div>
    </div>
</body>
</html>
```

### Example 3: Responsive Typography with REM

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>REM Typography</title>
    <style>
        html {
            font-size: 12pt;  /* Base size */
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 1rem;  /* 12pt */
            line-height: 1.6;
            margin: 40pt;
        }

        h1 {
            font-size: 2.5rem;      /* 30pt */
            margin-bottom: 1.5rem;  /* 18pt */
            color: #1e40af;
        }

        h2 {
            font-size: 2rem;        /* 24pt */
            margin-top: 2rem;       /* 24pt */
            margin-bottom: 1rem;    /* 12pt */
            color: #2563eb;
        }

        h3 {
            font-size: 1.5rem;      /* 18pt */
            margin-top: 1.5rem;     /* 18pt */
            margin-bottom: 0.75rem; /* 9pt */
        }

        p {
            font-size: 1rem;        /* 12pt */
            margin-bottom: 1rem;    /* 12pt */
        }

        .small {
            font-size: 0.875rem;    /* 10.5pt */
        }

        .large {
            font-size: 1.25rem;     /* 15pt */
        }

        .box {
            padding: 1.5rem;        /* 18pt */
            margin-bottom: 2rem;    /* 24pt */
            border: 1pt solid #d1d5db;
            border-radius: 0.5rem;  /* 6pt */
        }
    </style>
</head>
<body>
    <h1>Typography Scale</h1>
    <p>This document uses rem units for scalable, consistent typography.</p>

    <h2>Benefits of REM</h2>
    <p>All sizes scale proportionally by changing the root font-size.</p>

    <div class="box">
        <h3>Example Box</h3>
        <p>Padding and margins also use rem for consistency.</p>
        <p class="small">This is small text (0.875rem).</p>
        <p class="large">This is large text (1.25rem).</p>
    </div>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Mixed Units

Create a document using:
- Inches for page margins
- Points for font sizes and spacing
- Percentages for column widths
- Calc() to combine them

### Exercise 2: Scalable Design

Create a design using rem:
- Set base font-size on html element
- Use rem for all fonts, spacing, and sizes
- Try changing the base size to see everything scale

### Exercise 3: Grid Layout

Create a 3-column layout:
- Use calc() for column widths with gaps
- Fixed gutter width in points
- Percentage-based columns

---

## Common Pitfalls

### ❌ Mixing Units Inconsistently

```css
.inconsistent {
    width: 200px;
    padding: 15pt;
    margin: 0.5in;
    border: 2mm solid black;
}
```

✅ **Solution:** Stick to one unit system

```css
.consistent {
    width: 200pt;
    padding: 15pt;
    margin: 36pt;  /* 0.5in = 36pt */
    border: 2pt solid black;
}
```

### ❌ Using Pixels for Print

```css
.web-thinking {
    font-size: 16px;  /* Less precise for PDF */
}
```

✅ **Solution:** Use points

```css
.pdf-ready {
    font-size: 12pt;  /* Native PDF unit */
}
```

### ❌ Forgetting calc() Spaces

```css
.wrong {
    width: calc(100%-40pt);  /* Error: missing spaces */
}
```

✅ **Solution:** Always use spaces around operators

```css
.correct {
    width: calc(100% - 40pt);  /* Correct */
}
```

---

## Unit Selection Checklist

- [ ] Use **points** for most measurements
- [ ] Use **inches** for page margins
- [ ] Use **percentages** for fluid widths
- [ ] Use **rem** for scalable typography
- [ ] Use **calc()** to combine units
- [ ] Convert units consistently
- [ ] Test with different page sizes
- [ ] Verify print dimensions

---

## Next Steps

Now that you understand units and measurements:

1. **[Text Styling](05_text_styling.md)** - Advanced text formatting
2. **[Display & Visibility](06_display_visibility.md)** - Control element display
3. **[Style Organization](07_style_organization.md)** - Organize your CSS

---

**Continue learning →** [Text Styling](05_text_styling.md)
