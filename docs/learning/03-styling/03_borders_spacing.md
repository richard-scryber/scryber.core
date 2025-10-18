---
layout: default
title: Borders & Spacing
nav_order: 3
parent: Styling & Appearance
parent_url: /learning/03-styling/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Borders & Spacing

Master the CSS box model, borders, margins, padding, and spacing to control element layout and appearance.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Understand the CSS box model
- Apply borders with different styles and colors
- Use border-radius for rounded corners
- Control margins for outer spacing
- Use padding for inner spacing
- Master box-sizing property
- Create visually distinct content areas

---

## The CSS Box Model

Every element is a rectangular box with four areas:

```
┌─────────────── margin ──────────────┐
│ ┌────────── border ────────────┐   │
│ │ ┌─────── padding ─────────┐ │   │
│ │ │                          │ │   │
│ │ │       content            │ │   │
│ │ │                          │ │   │
│ │ └──────────────────────────┘ │   │
│ └──────────────────────────────┘   │
└────────────────────────────────────┘
```

- **Content:** The actual content (text, images, etc.)
- **Padding:** Space between content and border
- **Border:** Line around the padding
- **Margin:** Space outside the border

---

## Borders

### Border Properties

```css
/* All borders */
.box {
    border-width: 1pt;
    border-style: solid;
    border-color: #d1d5db;
}

/* Shorthand */
.box {
    border: 1pt solid #d1d5db;
}
```

### Border Styles

```css
.solid { border: 2pt solid black; }
.dashed { border: 2pt dashed black; }
.dotted { border: 2pt dotted black; }
.double { border: 4pt double black; }
.none { border: none; }
```

### Individual Borders

```css
/* One side only */
.top-border {
    border-top: 2pt solid #2563eb;
}

.left-border {
    border-left: 4pt solid #059669;
}

/* Different styles per side */
.mixed-borders {
    border-top: 2pt solid #2563eb;
    border-right: 1pt solid #d1d5db;
    border-bottom: 2pt solid #2563eb;
    border-left: 1pt solid #d1d5db;
}
```

### Border Colors

```css
/* All sides same color */
.box {
    border: 2pt solid #2563eb;
}

/* Different colors per side */
.colorful {
    border-width: 2pt;
    border-style: solid;
    border-top-color: #2563eb;
    border-right-color: #059669;
    border-bottom-color: #f59e0b;
    border-left-color: #dc2626;
}
```

### Border Width

```css
/* Keyword sizes */
.thin { border: thin solid black; }
.medium { border: medium solid black; }
.thick { border: thick solid black; }

/* Specific sizes */
.exact { border: 1pt solid black; }
.wider { border: 3pt solid black; }

/* Different widths per side */
.varied {
    border-style: solid;
    border-color: black;
    border-top-width: 1pt;
    border-right-width: 2pt;
    border-bottom-width: 3pt;
    border-left-width: 4pt;
}
```

---

## Border Radius

Create rounded corners.

### Basic Radius

```css
/* All corners */
.rounded {
    border: 2pt solid #2563eb;
    border-radius: 5pt;
}

/* Larger radius */
.more-rounded {
    border: 2pt solid #2563eb;
    border-radius: 10pt;
}

/* Pill shape */
.pill {
    border: 2pt solid #2563eb;
    border-radius: 20pt;
    padding: 10pt 20pt;
}

/* Circle */
.circle {
    border: 2pt solid #2563eb;
    border-radius: 50%;
    width: 100pt;
    height: 100pt;
}
```

### Individual Corners

```css
/* Top-left, top-right, bottom-right, bottom-left */
.corners {
    border-radius: 10pt 5pt 10pt 5pt;
}

/* Specific corners */
.top-rounded {
    border-top-left-radius: 10pt;
    border-top-right-radius: 10pt;
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
}
```

---

## Margin

Space **outside** the border.

### All Sides

```css
/* All sides equal */
.spaced {
    margin: 20pt;
}

/* Vertical and horizontal */
.spaced-vh {
    margin: 20pt 40pt;  /* 20pt top/bottom, 40pt left/right */
}

/* Individual sides */
.spaced-all {
    margin: 10pt 20pt 30pt 40pt;  /* top right bottom left (clockwise) */
}
```

### Specific Sides

```css
.margins {
    margin-top: 20pt;
    margin-right: 15pt;
    margin-bottom: 20pt;
    margin-left: 15pt;
}
```

### Common Patterns

```css
/* Center element horizontally */
.centered {
    width: 400pt;
    margin-left: auto;
    margin-right: auto;
}

/* No top margin, bottom margin only */
.stacked {
    margin: 0 0 20pt 0;
}

/* Negative margin (use carefully!) */
.overlap {
    margin-top: -10pt;
}
```

---

## Padding

Space **inside** the border, around the content.

### All Sides

```css
/* All sides equal */
.padded {
    padding: 20pt;
}

/* Vertical and horizontal */
.padded-vh {
    padding: 15pt 30pt;  /* 15pt top/bottom, 30pt left/right */
}

/* Individual sides */
.padded-all {
    padding: 10pt 15pt 20pt 25pt;  /* top right bottom left */
}
```

### Specific Sides

```css
.paddings {
    padding-top: 15pt;
    padding-right: 20pt;
    padding-bottom: 15pt;
    padding-left: 20pt;
}
```

### Common Patterns

```css
/* Button styling */
.button {
    padding: 10pt 25pt;  /* Vertical, horizontal */
    border: 2pt solid #2563eb;
    background-color: #2563eb;
    color: white;
}

/* Content box */
.content-box {
    padding: 20pt;
    border: 1pt solid #d1d5db;
    background-color: #f9fafb;
}

/* Emphasized text */
.callout {
    padding: 15pt;
    padding-left: 20pt;
    border-left: 4pt solid #2563eb;
    background-color: #eff6ff;
}
```

---

## Box Sizing

Controls how width and height are calculated.

### content-box (default)

Width/height applies to content only (excludes padding and border).

```css
.content-box {
    box-sizing: content-box;  /* Default */
    width: 200pt;
    padding: 20pt;
    border: 2pt solid black;
    /* Total width = 200pt (content) + 40pt (padding) + 4pt (border) = 244pt */
}
```

### border-box

Width/height includes padding and border.

```css
.border-box {
    box-sizing: border-box;
    width: 200pt;
    padding: 20pt;
    border: 2pt solid black;
    /* Total width = 200pt (includes content, padding, and border) */
}
```

### Best Practice

```css
/* Apply to all elements */
* {
    box-sizing: border-box;
}

/* Now widths are predictable */
.box {
    width: 100%;
    padding: 20pt;
    border: 1pt solid black;
    /* Width stays 100% including padding and border */
}
```

---

## Practical Examples

### Example 1: Card Layout

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Card Layout</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
            background-color: #f9fafb;
        }

        .card {
            background-color: white;
            border: 1pt solid #e5e7eb;
            border-radius: 8pt;
            padding: 20pt;
            margin-bottom: 20pt;
            box-shadow: 0 1pt 3pt rgba(0, 0, 0, 0.1);
        }

        .card-header {
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 15pt;
            margin-bottom: 15pt;
        }

        .card-header h2 {
            margin: 0;
            color: #1e40af;
        }

        .card-body {
            padding: 0;
        }

        .card-footer {
            border-top: 1pt solid #e5e7eb;
            padding-top: 15pt;
            margin-top: 15pt;
            font-size: 9pt;
            color: #666;
        }
    </style>
</head>
<body>
    <div class="card">
        <div class="card-header">
            <h2>Project Update</h2>
        </div>
        <div class="card-body">
            <p>The website redesign project is progressing well. We've completed the initial mockups and are now moving into the development phase.</p>
        </div>
        <div class="card-footer">
            Last updated: January 15, 2025
        </div>
    </div>

    <div class="card">
        <div class="card-header">
            <h2>Financial Summary</h2>
        </div>
        <div class="card-body">
            <p>Q4 revenue exceeded expectations with a 25% increase year-over-year.</p>
        </div>
        <div class="card-footer">
            Fiscal Year 2024
        </div>
    </div>
</body>
</html>
```

### Example 2: Alert Boxes

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Alert Boxes</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }

        .alert {
            padding: 15pt;
            padding-left: 20pt;
            margin-bottom: 20pt;
            border-radius: 5pt;
            border-left-width: 4pt;
            border-left-style: solid;
        }

        .alert-success {
            background-color: #d1fae5;
            border-left-color: #059669;
            color: #065f46;
        }

        .alert-info {
            background-color: #dbeafe;
            border-left-color: #2563eb;
            color: #1e40af;
        }

        .alert-warning {
            background-color: #fef3c7;
            border-left-color: #f59e0b;
            color: #92400e;
        }

        .alert-error {
            background-color: #fee2e2;
            border-left-color: #dc2626;
            color: #991b1b;
        }

        .alert-title {
            font-weight: bold;
            margin: 0 0 8pt 0;
        }

        .alert p {
            margin: 0;
        }
    </style>
</head>
<body>
    <h1>System Notifications</h1>

    <div class="alert alert-success">
        <p class="alert-title">Success!</p>
        <p>Your document has been generated successfully.</p>
    </div>

    <div class="alert alert-info">
        <p class="alert-title">Information</p>
        <p>System maintenance is scheduled for next weekend.</p>
    </div>

    <div class="alert alert-warning">
        <p class="alert-title">Warning</p>
        <p>Your storage space is running low. Please delete old files.</p>
    </div>

    <div class="alert alert-error">
        <p class="alert-title">Error</p>
        <p>Failed to connect to the database. Please try again later.</p>
    </div>
</body>
</html>
```

### Example 3: Table with Borders

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Styled Table</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }

        h1 {
            color: #1e40af;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            border: 2pt solid #2563eb;
            border-radius: 8pt;
            overflow: hidden;
        }

        thead {
            background-color: #2563eb;
            color: white;
        }

        th {
            padding: 12pt;
            text-align: left;
            font-weight: bold;
            border-bottom: 2pt solid #1e40af;
        }

        td {
            padding: 10pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        tbody tr:last-child td {
            border-bottom: none;
        }

        tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        tbody tr:hover {
            background-color: #eff6ff;
        }

        .amount {
            text-align: right;
            font-weight: bold;
        }

        tfoot {
            background-color: #eff6ff;
            border-top: 2pt solid #2563eb;
        }

        tfoot td {
            padding: 12pt;
            font-weight: bold;
            border-bottom: none;
        }
    </style>
</head>
<body>
    <h1>Sales Report</h1>

    <table>
        <thead>
            <tr>
                <th>Product</th>
                <th>Quantity</th>
                <th class="amount">Price</th>
                <th class="amount">Total</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget A</td>
                <td>5</td>
                <td class="amount">$10.00</td>
                <td class="amount">$50.00</td>
            </tr>
            <tr>
                <td>Widget B</td>
                <td>3</td>
                <td class="amount">$15.00</td>
                <td class="amount">$45.00</td>
            </tr>
            <tr>
                <td>Widget C</td>
                <td>2</td>
                <td class="amount">$20.00</td>
                <td class="amount">$40.00</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="3">Grand Total</td>
                <td class="amount">$135.00</td>
            </tr>
        </tfoot>
    </table>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Content Cards

Create a document with multiple cards:
- Rounded borders
- Consistent padding
- Margin between cards
- Header with bottom border
- Footer with top border

### Exercise 2: Highlight Boxes

Create different highlight boxes:
- Success box (green theme)
- Warning box (yellow theme)
- Error box (red theme)
- Info box (blue theme)
- Each with left border accent and appropriate colors

### Exercise 3: Layout Grid

Create a grid of boxes:
- Equal spacing between boxes
- Borders around each box
- Padding inside boxes
- Consistent sizing using border-box

---

## Common Pitfalls

### ❌ Forgetting box-sizing

```css
.box {
    width: 100%;
    padding: 20pt;
    border: 2pt solid black;
    /* Total width exceeds 100%! */
}
```

✅ **Solution:** Use border-box

```css
.box {
    box-sizing: border-box;
    width: 100%;
    padding: 20pt;
    border: 2pt solid black;
    /* Width stays 100% */
}
```

### ❌ Margin Collapsing

```css
.box1 {
    margin-bottom: 20pt;
}

.box2 {
    margin-top: 20pt;
}
/* Space between is 20pt, not 40pt! (margins collapse) */
```

✅ **Solution:** Use padding or single margin

```css
.box1 {
    margin-bottom: 20pt;
}

.box2 {
    margin-top: 0;
}
/* Space is 20pt as expected */
```

### ❌ Inconsistent Spacing

```css
.element1 { margin: 10pt; }
.element2 { margin: 15pt; }
.element3 { margin: 12pt; }
```

✅ **Solution:** Use consistent spacing scale

```css
.element1 { margin: 10pt; }
.element2 { margin: 20pt; }
.element3 { margin: 10pt; }
/* Use multiples: 10pt, 20pt, 30pt, etc. */
```

---

## Spacing Best Practices

1. **Use consistent spacing scale** (e.g., 10pt, 20pt, 30pt)
2. **Set box-sizing: border-box** globally
3. **Use padding for inner spacing**, margin for outer
4. **Keep borders subtle** (1-2pt for most cases)
5. **Use border-radius** sparingly (5-10pt is typical)
6. **Test with different content sizes** to ensure spacing works

---

## Next Steps

Now that you understand borders and spacing:

1. **[Units & Measurements](04_units_measurements.md)** - Master CSS units
2. **[Text Styling](05_text_styling.md)** - Advanced text formatting
3. **[Display & Visibility](06_display_visibility.md)** - Control element display

---

**Continue learning →** [Units & Measurements](04_units_measurements.md)
