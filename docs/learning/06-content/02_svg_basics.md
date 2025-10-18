---
layout: default
title: SVG Basics
nav_order: 2
parent: Content Components
parent_url: /learning/06-content/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# SVG Basics

Learn the fundamentals of SVG (Scalable Vector Graphics) in PDF documents, including inline SVG, external files, sizing, and positioning.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Understand SVG advantages for PDFs
- Use inline SVG elements
- Reference external SVG files
- Control SVG sizing with viewBox
- Position and style SVG graphics
- Choose between inline and external SVG

---

## What is SVG?

SVG (Scalable Vector Graphics) is an XML-based vector image format that's perfect for PDFs:

**Advantages:**
- **Scalable** - No loss of quality at any size
- **Small file size** - Text-based format
- **Crisp rendering** - Perfect for logos and icons
- **Data binding** - Dynamic content with inline SVG
- **Styling** - Full CSS control

**Best Uses:**
- Logos and icons
- Charts and graphs
- Diagrams and flowcharts
- Geometric shapes
- Data visualizations

---

## Inline SVG

### Basic SVG Element

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Inline SVG</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }
    </style>
</head>
<body>
    <h1>Circle Example</h1>

    <svg width="200" height="200">
        <circle cx="100" cy="100" r="50" fill="blue" />
    </svg>
</body>
</html>
```

### Common SVG Shapes

```html
<svg width="600" height="400">
    <!-- Circle -->
    <circle cx="100" cy="100" r="50" fill="#3b82f6" />

    <!-- Rectangle -->
    <rect x="200" y="50" width="100" height="100" fill="#10b981" />

    <!-- Ellipse -->
    <ellipse cx="450" cy="100" rx="75" ry="50" fill="#f59e0b" />

    <!-- Line -->
    <line x1="50" y1="200" x2="550" y2="200" stroke="#ef4444" stroke-width="2" />

    <!-- Polyline -->
    <polyline points="50,300 150,250 250,300 350,250 450,300"
              fill="none" stroke="#8b5cf6" stroke-width="3" />

    <!-- Polygon -->
    <polygon points="100,350 150,350 125,300"
             fill="#ec4899" />
</svg>
```

---

## External SVG Files

### Referencing SVG Files

```html
<!-- Using img tag -->
<img src="./images/logo.svg"
     alt="Company Logo"
     style="width: 150pt; height: auto;" />

<!-- Using object tag -->
<object data="./graphics/chart.svg"
        type="image/svg+xml"
        width="400"
        height="300">
    Fallback text if SVG doesn't load
</object>
```

### When to Use External SVG

**✅ Use external SVG files for:**
- Reusable graphics (logos, icons)
- Complex illustrations
- Static content
- Graphics created in design tools

**✅ Use inline SVG for:**
- Data-driven graphics
- Dynamic content with binding
- Simple shapes
- Charts that change with data

---

## SVG Sizing

### Width and Height

```html
<!-- Fixed dimensions -->
<svg width="400" height="300">
    <rect width="400" height="300" fill="#f3f4f6" />
</svg>

<!-- Using CSS -->
<svg style="width: 400pt; height: 300pt;">
    <rect width="400" height="300" fill="#f3f4f6" />
</svg>
```

### viewBox Attribute

The `viewBox` defines the coordinate system:

```html
<!-- viewBox="minX minY width height" -->
<svg viewBox="0 0 100 100" width="200" height="200">
    <!-- Drawing coordinates are 0-100, but displays at 200×200 -->
    <circle cx="50" cy="50" r="40" fill="blue" />
</svg>
```

**Benefits:**
- Scalable coordinate system
- Easier math for positioning
- Responsive sizing
- Maintains aspect ratio

### Responsive SVG

```css
/* Make SVG responsive */
.responsive-svg {
    width: 100%;
    height: auto;
}
```

```html
<svg viewBox="0 0 600 400"
     class="responsive-svg"
     preserveAspectRatio="xMidYMid meet">
    <rect width="600" height="400" fill="#f3f4f6" />
    <circle cx="300" cy="200" r="100" fill="#3b82f6" />
</svg>
```

---

## SVG Coordinate System

### Understanding Coordinates

```html
<svg width="400" height="300" style="border: 1pt solid black;">
    <!-- Origin (0,0) is top-left -->
    <!-- x increases to the right -->
    <!-- y increases downward -->

    <!-- Point at origin -->
    <circle cx="0" cy="0" r="5" fill="red" />

    <!-- Point at center -->
    <circle cx="200" cy="150" r="5" fill="blue" />

    <!-- Point at bottom-right -->
    <circle cx="400" cy="300" r="5" fill="green" />
</svg>
```

---

## SVG Styling

### Inline Styles

```html
<svg width="200" height="200">
    <circle cx="100" cy="100" r="50"
            fill="#3b82f6"
            stroke="#1e40af"
            stroke-width="3"
            opacity="0.8" />
</svg>
```

### CSS Styling

```html
<style>
    .styled-circle {
        fill: #3b82f6;
        stroke: #1e40af;
        stroke-width: 3;
        opacity: 0.8;
    }

    .styled-circle:hover {
        fill: #2563eb;  /* Note: hover may not work in PDF */
    }
</style>

<svg width="200" height="200">
    <circle cx="100" cy="100" r="50" class="styled-circle" />
</svg>
```

---

## Practical Examples

### Example 1: Icon Library

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>SVG Icons</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            margin: 0;
        }

        h1 {
            font-size: 24pt;
            margin-bottom: 30pt;
            color: #1e40af;
        }

        /* ==============================================
           ICON GRID
           ============================================== */
        .icon-grid {
            display: table;
            width: 100%;
        }

        .icon-row {
            display: table-row;
        }

        .icon-cell {
            display: table-cell;
            width: 25%;
            padding: 20pt;
            text-align: center;
            vertical-align: top;
            border: 1pt solid #e5e7eb;
        }

        .icon-svg {
            width: 60pt;
            height: 60pt;
            margin-bottom: 10pt;
        }

        .icon-label {
            font-size: 10pt;
            color: #666;
        }

        /* ==============================================
           ICON STYLES
           ============================================== */
        .icon-check {
            fill: #10b981;
        }

        .icon-warning {
            fill: #f59e0b;
        }

        .icon-error {
            fill: #ef4444;
        }

        .icon-info {
            fill: #3b82f6;
        }
    </style>
</head>
<body>
    <h1>SVG Icon Library</h1>

    <div class="icon-grid">
        <div class="icon-row">
            <!-- Check Icon -->
            <div class="icon-cell">
                <svg viewBox="0 0 100 100" class="icon-svg icon-check">
                    <circle cx="50" cy="50" r="45" stroke="currentColor"
                            stroke-width="5" fill="none" />
                    <path d="M 30 50 L 45 65 L 70 35"
                          stroke="currentColor" stroke-width="5"
                          fill="none" stroke-linecap="round" />
                </svg>
                <div class="icon-label">Check / Success</div>
            </div>

            <!-- Warning Icon -->
            <div class="icon-cell">
                <svg viewBox="0 0 100 100" class="icon-svg icon-warning">
                    <path d="M 50 10 L 90 85 L 10 85 Z"
                          fill="currentColor" />
                    <text x="50" y="75" text-anchor="middle"
                          font-size="40" font-weight="bold" fill="white">!</text>
                </svg>
                <div class="icon-label">Warning</div>
            </div>

            <!-- Error Icon -->
            <div class="icon-cell">
                <svg viewBox="0 0 100 100" class="icon-svg icon-error">
                    <circle cx="50" cy="50" r="45" fill="currentColor" />
                    <line x1="35" y1="35" x2="65" y2="65"
                          stroke="white" stroke-width="5" stroke-linecap="round" />
                    <line x1="65" y1="35" x2="35" y2="65"
                          stroke="white" stroke-width="5" stroke-linecap="round" />
                </svg>
                <div class="icon-label">Error</div>
            </div>

            <!-- Info Icon -->
            <div class="icon-cell">
                <svg viewBox="0 0 100 100" class="icon-svg icon-info">
                    <circle cx="50" cy="50" r="45" fill="currentColor" />
                    <text x="50" y="75" text-anchor="middle"
                          font-size="50" font-weight="bold" fill="white">i</text>
                </svg>
                <div class="icon-label">Information</div>
            </div>
        </div>
    </div>
</body>
</html>
```

### Example 2: Logo and Branding

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Company Branding</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            margin: 0;
        }

        /* ==============================================
           LETTERHEAD
           ============================================== */
        .letterhead {
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 20pt;
            margin-bottom: 30pt;
        }

        .logo-container {
            text-align: center;
            margin-bottom: 15pt;
        }

        .company-logo {
            width: 200pt;
            height: auto;
        }

        .company-name {
            font-size: 20pt;
            font-weight: 700;
            color: #1e40af;
            text-align: center;
            margin: 10pt 0 5pt 0;
        }

        .company-tagline {
            font-size: 11pt;
            color: #666;
            text-align: center;
            margin: 0;
        }

        /* ==============================================
           CONTENT
           ============================================== */
        .content {
            font-size: 11pt;
            line-height: 1.6;
        }

        h1 {
            font-size: 18pt;
            margin: 0 0 20pt 0;
        }

        p {
            margin: 0 0 12pt 0;
        }

        /* ==============================================
           FOOTER
           ============================================== */
        .footer {
            margin-top: 40pt;
            padding-top: 20pt;
            border-top: 1pt solid #e5e7eb;
            font-size: 9pt;
            color: #666;
            text-align: center;
        }

        .footer-icon {
            width: 15pt;
            height: 15pt;
            vertical-align: middle;
            margin-right: 5pt;
        }
    </style>
</head>
<body>
    <!-- Letterhead with SVG Logo -->
    <div class="letterhead">
        <div class="logo-container">
            <!-- Custom SVG Logo -->
            <svg viewBox="0 0 200 80" class="company-logo">
                <!-- Background -->
                <rect width="200" height="80" fill="#2563eb" rx="10" />

                <!-- Geometric logo shape -->
                <circle cx="40" cy="40" r="25" fill="white" opacity="0.2" />
                <circle cx="60" cy="40" r="25" fill="white" opacity="0.4" />
                <circle cx="50" cy="40" r="20" fill="white" />

                <!-- Company name -->
                <text x="90" y="50" font-size="28" font-weight="bold"
                      fill="white" font-family="Arial">
                    ACME
                </text>
            </svg>
        </div>

        <p class="company-name">Acme Corporation</p>
        <p class="company-tagline">Innovation Through Excellence</p>
    </div>

    <!-- Document Content -->
    <div class="content">
        <h1>Business Proposal</h1>

        <p>
            Dear Valued Client,
        </p>

        <p>
            Thank you for considering Acme Corporation for your business needs.
            We are pleased to present this proposal outlining our services and
            commitment to excellence.
        </p>

        <p>
            Our team of experts has over 20 years of combined experience in
            delivering innovative solutions that drive business growth and
            operational efficiency.
        </p>
    </div>

    <!-- Footer with SVG Icons -->
    <div class="footer">
        <p>
            <svg viewBox="0 0 20 20" class="footer-icon">
                <path d="M 10 2 L 10 18 M 2 10 L 18 10" stroke="#666"
                      stroke-width="2" />
            </svg>
            123 Business St, Suite 100, City, State 12345
        </p>
        <p>
            <svg viewBox="0 0 20 20" class="footer-icon">
                <circle cx="10" cy="10" r="8" stroke="#666"
                        stroke-width="2" fill="none" />
                <path d="M 10 6 L 10 10 L 14 10" stroke="#666"
                      stroke-width="2" />
            </svg>
            info@acmecorp.com | (555) 123-4567
        </p>
    </div>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Create Icon Set

Design 4-6 simple SVG icons:
- Use basic shapes (circle, rect, path)
- Make them reusable with consistent sizing
- Apply color theming with CSS
- Arrange in a grid layout

### Exercise 2: Company Logo

Create a custom SVG logo featuring:
- Geometric shapes or text
- 2-3 colors
- Scalable design (test at different sizes)
- Use in letterhead layout

### Exercise 3: Compare Formats

Create the same graphic as:
- Inline SVG
- External SVG file referenced with `<img>`
- PNG image

Compare file sizes and rendering quality.

---

## Common Pitfalls

### ❌ Missing viewBox

```html
<svg width="400" height="300">
    <!-- Coordinate system unclear -->
    <circle cx="200" cy="150" r="100" fill="blue" />
</svg>
```

✅ **Solution:**

```html
<svg viewBox="0 0 400 300" width="400" height="300">
    <!-- Clear coordinate system -->
    <circle cx="200" cy="150" r="100" fill="blue" />
</svg>
```

### ❌ Not Specifying Dimensions

```html
<!-- Size may be unpredictable -->
<svg viewBox="0 0 100 100">
    <circle cx="50" cy="50" r="40" fill="blue" />
</svg>
```

✅ **Solution:**

```html
<svg viewBox="0 0 100 100" width="200" height="200">
    <circle cx="50" cy="50" r="40" fill="blue" />
</svg>
```

### ❌ Complex SVG from Design Tools

```html
<!-- Exported SVG with excessive complexity -->
<svg>
    <g transform="matrix(...)">
        <path d="M 1.234 5.678 L 2.345 ..." />
        <!-- Hundreds of unnecessary elements -->
    </g>
</svg>
```

✅ **Solution:**

```html
<!-- Optimize/simplify exported SVG -->
<svg viewBox="0 0 100 100" width="100" height="100">
    <circle cx="50" cy="50" r="40" fill="blue" />
</svg>
```

---

## SVG Best Practices Checklist

- [ ] viewBox defined for scalability
- [ ] Width and height specified
- [ ] Coordinate system is logical (0-100 or 0-1000)
- [ ] Colors use CSS or data binding
- [ ] SVG is optimized (remove unnecessary elements)
- [ ] Alt text or fallback provided
- [ ] Tested at different sizes
- [ ] Renders correctly in PDF

---

## Best Practices

1. **Always Use viewBox** - Enables perfect scaling
2. **Specify Dimensions** - Width and height for predictability
3. **Simple Coordinates** - Use 0-100 or 0-1000 for easy math
4. **Optimize Exported SVG** - Remove unnecessary code
5. **Use CSS for Styling** - Easier to maintain
6. **Test Rendering** - Verify in generated PDF
7. **Inline for Dynamic** - External for static graphics
8. **Keep It Simple** - Complex SVG may render slowly

---

## Next Steps

1. **[SVG Drawing](03_svg_drawing.md)** - Create charts and visualizations
2. **[Lists](04_lists.md)** - Structured content
3. **[Content Best Practices](08_content_best_practices.md)** - Optimization

---

**Continue learning →** [SVG Drawing](03_svg_drawing.md)
