---
layout: default
title: Colors & Backgrounds
nav_order: 2
parent: Styling & Appearance
parent_url: /learning/03-styling/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Colors & Backgrounds

Master color formats, backgrounds, opacity, and visual styling for professional PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Use different color formats (hex, RGB, named colors)
- Apply text and background colors
- Work with opacity and transparency
- Add background images
- Position and repeat backgrounds
- Create visually appealing color schemes

---

## Color Formats

Scryber supports multiple color formats for flexibility.

### Hex Colors

Most common format, using hexadecimal notation.

```css
/* 6-digit hex */
.text {
    color: #2563eb;              /* Blue */
    background-color: #f9fafb;   /* Light gray */
}

/* 3-digit shorthand */
.text {
    color: #f00;                 /* Red (#ff0000) */
    background-color: #fff;      /* White (#ffffff) */
}
```

### RGB Colors

Specify red, green, blue values (0-255).

```css
.text {
    color: rgb(37, 99, 235);           /* Blue */
    background-color: rgb(249, 250, 251);  /* Light gray */
}
```

### RGBA Colors

RGB with alpha channel for transparency (0-1).

```css
.overlay {
    background-color: rgba(0, 0, 0, 0.5);  /* 50% transparent black */
}

.highlight {
    background-color: rgba(254, 243, 199, 0.8);  /* 80% opaque yellow */
}
```

### Named Colors

Use standard CSS color names.

```css
.text {
    color: blue;
    background-color: white;
    border-color: red;
}
```

**Common Named Colors:**
- `black`, `white`, `red`, `green`, `blue`
- `yellow`, `orange`, `purple`, `pink`
- `gray`, `silver`, `maroon`, `navy`

---

## Text Colors

### Basic Text Color

```css
p {
    color: #333333;  /* Dark gray text */
}

h1 {
    color: #1e40af;  /* Blue heading */
}

.warning {
    color: #dc2626;  /* Red warning text */
}

.success {
    color: #059669;  /* Green success text */
}
```

### Color Inheritance

Text color inherits from parent elements.

```css
body {
    color: #333;  /* All text inherits this color */
}

.special {
    color: #2563eb;  /* Override for this class */
}
```

---

## Background Colors

### Solid Backgrounds

```css
.container {
    background-color: #f9fafb;  /* Light gray background */
}

.header {
    background-color: #2563eb;  /* Blue background */
    color: white;  /* White text on blue */
}

.highlight {
    background-color: #fef3c7;  /* Yellow highlight */
    padding: 10pt;
}
```

### Transparent Backgrounds

```css
.box {
    background-color: transparent;  /* No background */
}

.overlay {
    background-color: rgba(0, 0, 0, 0.3);  /* 30% black overlay */
}
```

---

## Opacity

Control the transparency of entire elements.

```css
.watermark {
    opacity: 0.3;  /* 30% visible */
    color: #666;
    font-size: 72pt;
}

.faded {
    opacity: 0.5;  /* 50% visible */
}

.solid {
    opacity: 1.0;  /* 100% visible (default) */
}
```

**Note:** Opacity affects the entire element and its children. Use RGBA for background-only transparency.

---

## Background Images

### Basic Background Image

```css
.header {
    background-image: url('./images/header-bg.png');
    background-size: cover;
    height: 150pt;
}
```

### Background Size

```css
/* Cover entire area */
.cover {
    background-image: url('./images/photo.jpg');
    background-size: cover;  /* Scale to cover, may crop */
}

/* Contain within area */
.contain {
    background-image: url('./images/logo.png');
    background-size: contain;  /* Scale to fit, no cropping */
}

/* Specific dimensions */
.fixed {
    background-image: url('./images/icon.png');
    background-size: 50pt 50pt;  /* Exact size */
}

/* Width only, maintain aspect ratio */
.width {
    background-image: url('./images/banner.jpg');
    background-size: 100% auto;
}
```

### Background Position

```css
/* Keyword positioning */
.top-left {
    background-image: url('./images/logo.png');
    background-position: top left;
}

.centered {
    background-image: url('./images/watermark.png');
    background-position: center center;
}

.bottom-right {
    background-image: url('./images/corner.png');
    background-position: bottom right;
}

/* Precise positioning */
.positioned {
    background-image: url('./images/element.png');
    background-position: 50pt 30pt;  /* 50pt from left, 30pt from top */
}

/* Percentage positioning */
.percent {
    background-image: url('./images/bg.jpg');
    background-position: 25% 75%;
}
```

### Background Repeat

```css
/* No repeat (default for most use cases) */
.no-repeat {
    background-image: url('./images/logo.png');
    background-repeat: no-repeat;
}

/* Repeat horizontally and vertically */
.repeat {
    background-image: url('./images/pattern.png');
    background-repeat: repeat;
}

/* Repeat horizontally only */
.repeat-x {
    background-image: url('./images/border-h.png');
    background-repeat: repeat-x;
}

/* Repeat vertically only */
.repeat-y {
    background-image: url('./images/border-v.png');
    background-repeat: repeat-y;
}
```

### Background Shorthand

```css
.element {
    /* background: color image repeat position / size */
    background: #f9fafb url('./images/bg.png') no-repeat center center / cover;
}

/* Equivalent to: */
.element {
    background-color: #f9fafb;
    background-image: url('./images/bg.png');
    background-repeat: no-repeat;
    background-position: center center;
    background-size: cover;
}
```

---

## Color Schemes

### Professional Color Palettes

```css
/* Blue theme */
.blue-theme {
    --primary: #2563eb;
    --secondary: #3b82f6;
    --accent: #60a5fa;
    --text: #1e293b;
    --background: #f8fafc;
}

/* Green theme */
.green-theme {
    --primary: #059669;
    --secondary: #10b981;
    --accent: #34d399;
    --text: #064e3b;
    --background: #f0fdf4;
}

/* Red theme */
.red-theme {
    --primary: #dc2626;
    --secondary: #ef4444;
    --accent: #f87171;
    --text: #7f1d1d;
    --background: #fef2f2;
}
```

### Status Colors

```css
.status {
    padding: 8pt;
    border-radius: 3pt;
    font-weight: bold;
}

.status-success {
    background-color: #d1fae5;
    color: #065f46;
    border: 1pt solid #059669;
}

.status-warning {
    background-color: #fef3c7;
    color: #92400e;
    border: 1pt solid #f59e0b;
}

.status-error {
    background-color: #fee2e2;
    color: #991b1b;
    border: 1pt solid #dc2626;
}

.status-info {
    background-color: #dbeafe;
    color: #1e40af;
    border: 1pt solid: #2563eb;
}
```

---

## Practical Examples

### Example 1: Branded Document Header

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Branded Document</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 0;
            color: #333;
        }

        .header {
            background-color: #2563eb;
            background-image: url('./images/header-pattern.png');
            background-repeat: repeat-x;
            background-position: bottom;
            color: white;
            padding: 30pt;
            text-align: center;
        }

        .header h1 {
            margin: 0;
            font-size: 28pt;
            color: white;
        }

        .header p {
            margin: 10pt 0 0 0;
            font-size: 14pt;
            opacity: 0.9;
        }

        .content {
            padding: 40pt;
            background-color: #f9fafb;
        }

        .content-box {
            background-color: white;
            padding: 20pt;
            margin-bottom: 20pt;
            border-left: 4pt solid #2563eb;
        }
    </style>
</head>
<body>
    <div class="header">
        <h1>Company Annual Report</h1>
        <p>Fiscal Year 2024</p>
    </div>

    <div class="content">
        <div class="content-box">
            <h2 style="color: #2563eb; margin-top: 0;">Executive Summary</h2>
            <p>Our company achieved record growth this year...</p>
        </div>

        <div class="content-box">
            <h2 style="color: #2563eb; margin-top: 0;">Financial Highlights</h2>
            <p>Revenue increased by 25% year over year...</p>
        </div>
    </div>
</body>
</html>
```

### Example 2: Status Indicators

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Project Status Report</title>
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
            margin: 20pt 0;
        }

        th {
            background-color: #2563eb;
            color: white;
            padding: 10pt;
            text-align: left;
        }

        td {
            padding: 10pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        .status-badge {
            display: inline-block;
            padding: 5pt 12pt;
            border-radius: 15pt;
            font-size: 9pt;
            font-weight: bold;
        }

        .status-on-track {
            background-color: #d1fae5;
            color: #065f46;
        }

        .status-at-risk {
            background-color: #fef3c7;
            color: #92400e;
        }

        .status-delayed {
            background-color: #fee2e2;
            color: #991b1b;
        }

        .status-completed {
            background-color: #dbeafe;
            color: #1e40af;
        }
    </style>
</head>
<body>
    <h1>Project Status Dashboard</h1>

    <table>
        <thead>
            <tr>
                <th>Project</th>
                <th>Owner</th>
                <th>Status</th>
                <th>Progress</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Website Redesign</td>
                <td>Alice Johnson</td>
                <td><span class="status-badge status-on-track">On Track</span></td>
                <td>75%</td>
            </tr>
            <tr>
                <td>Mobile App v2</td>
                <td>Bob Smith</td>
                <td><span class="status-badge status-at-risk">At Risk</span></td>
                <td>45%</td>
            </tr>
            <tr>
                <td>API Integration</td>
                <td>Carol White</td>
                <td><span class="status-badge status-delayed">Delayed</span></td>
                <td>30%</td>
            </tr>
            <tr>
                <td>Documentation</td>
                <td>David Brown</td>
                <td><span class="status-badge status-completed">Completed</span></td>
                <td>100%</td>
            </tr>
        </tbody>
    </table>
</body>
</html>
```

### Example 3: Watermark Background

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Confidential Document</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, sans-serif;
            position: relative;
        }

        .watermark {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%) rotate(-45deg);
            font-size: 100pt;
            color: rgba(220, 38, 38, 0.1);
            font-weight: bold;
            z-index: -1;
        }

        .content {
            position: relative;
            z-index: 1;
        }

        h1 {
            color: #1e40af;
        }
    </style>
</head>
<body>
    <div class="watermark">CONFIDENTIAL</div>

    <div class="content">
        <h1>Confidential Report</h1>
        <p>This document contains sensitive information...</p>
    </div>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Color Scheme

Create a document with:
- Consistent color palette (primary, secondary, accent)
- Colored section headers
- Background colors for different content types
- Status indicators with appropriate colors

### Exercise 2: Background Patterns

Create a document with:
- Header with background image
- Repeating pattern in sidebar
- Watermark in background
- Image positioned at specific location

### Exercise 3: Opacity Effects

Create a document with:
- Semi-transparent overlays
- Faded watermark
- Transparent backgrounds with colored text
- Layered elements with different opacity levels

---

## Common Pitfalls

### ❌ Poor Color Contrast

```css
.bad-contrast {
    color: #f0f0f0;               /* Light gray text */
    background-color: white;      /* White background - Hard to read! */
}
```

✅ **Solution:** Ensure sufficient contrast

```css
.good-contrast {
    color: #333;                  /* Dark text */
    background-color: white;      /* White background - Easy to read */
}
```

### ❌ Using Only Color for Information

```css
.error {
    color: red;  /* Only uses color - accessibility issue */
}
```

✅ **Solution:** Combine color with other indicators

```css
.error {
    color: #dc2626;
    border-left: 4pt solid #dc2626;
    font-weight: bold;
    padding-left: 15pt;
}

.error::before {
    content: "⚠ ";
}
```

### ❌ Background Image Not Found

```css
.header {
    background-image: url('header.png');  /* Relative path may not work */
}
```

✅ **Solution:** Use absolute or correct relative paths

```css
.header {
    background-image: url('./images/header.png');  /* Explicit relative */
    /* or */
    background-image: url('/Users/project/images/header.png');  /* Absolute */
}
```

---

## Color Accessibility Tips

1. **Contrast Ratio:** Aim for at least 4.5:1 for body text
2. **Don't rely solely on color** to convey information
3. **Test in grayscale** to ensure clarity without color
4. **Use patterns or icons** along with color
5. **Provide text labels** for color-coded information

---

## Next Steps

Now that you understand colors and backgrounds:

1. **[Borders & Spacing](03_borders_spacing.md)** - Master borders and box model
2. **[Units & Measurements](04_units_measurements.md)** - Learn CSS units
3. **[Text Styling](05_text_styling.md)** - Advanced text formatting

---

**Continue learning →** [Borders & Spacing](03_borders_spacing.md)
