---
layout: default
title: Images
nav_order: 1
parent: Content Components
parent_url: /learning/06-content/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Images

Learn how to work with images in PDF documents, including formats, sizing, positioning, and data binding for dynamic image content.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Use different image formats (PNG, JPEG, GIF, SVG)
- Size and position images effectively
- Load images from local and remote sources
- Apply styling to images
- Bind image sources dynamically
- Optimize images for PDF generation

---

## Supported Image Formats

### Format Overview

```html
<!-- PNG - Best for logos, icons, transparency -->
<img src="./images/logo.png" alt="Company Logo" />

<!-- JPEG - Best for photos, smaller file sizes -->
<img src="./images/photo.jpg" alt="Product Photo" />

<!-- GIF - Basic support, limited use -->
<img src="./images/animation.gif" alt="Animation" />

<!-- SVG - Scalable vector graphics -->
<img src="./images/icon.svg" alt="Icon" />
```

### Format Recommendations

| Format | Best For | Transparency | File Size |
|--------|----------|--------------|-----------|
| PNG | Logos, icons, screenshots | Yes | Medium-Large |
| JPEG | Photos, complex images | No | Small-Medium |
| GIF | Simple graphics | Yes (1-bit) | Small |
| SVG | Icons, logos, charts | Yes | Very Small |

---

## Basic Image Usage

### Simple Image

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Simple Image</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            margin: 0;
        }

        img {
            max-width: 100%;  /* Responsive */
            height: auto;      /* Maintain aspect ratio */
        }
    </style>
</head>
<body>
    <h1>Product Image</h1>
    <img src="./images/product.jpg" alt="Product" />
</body>
</html>
```

### Image with Fixed Size

```html
<img src="./images/logo.png"
     alt="Company Logo"
     style="width: 150pt; height: 50pt;" />
```

**Note:** Use `pt` (points) for precise sizing in PDFs.

---

## Image Sizing

### Width and Height

```css
/* Fixed dimensions */
.logo {
    width: 200pt;
    height: 100pt;
}

/* Maintain aspect ratio */
.photo {
    width: 300pt;
    height: auto;  /* Scales proportionally */
}

/* Maximum constraints */
.responsive {
    max-width: 100%;  /* Never wider than container */
    height: auto;
}

/* Percentage sizing */
.half-width {
    width: 50%;
    height: auto;
}
```

### object-fit Property

```css
/* Cover - fills area, may crop */
.cover {
    width: 200pt;
    height: 200pt;
    object-fit: cover;  /* Crop to fill */
}

/* Contain - fits inside, may have gaps */
.contain {
    width: 200pt;
    height: 200pt;
    object-fit: contain;  /* Scale to fit */
}

/* Fill - stretches to fill */
.fill {
    width: 200pt;
    height: 200pt;
    object-fit: fill;  /* May distort */
}
```

**Note:** `object-fit` support may vary in PDF generators.

---

## Image Sources

### Local Files

```html
<!-- Relative path -->
<img src="./images/logo.png" alt="Logo" />

<!-- Absolute path (Windows) -->
<img src="C:/Projects/images/photo.jpg" alt="Photo" />

<!-- Absolute path (Unix/Mac) -->
<img src="/Users/username/images/photo.jpg" alt="Photo" />
```

### Remote URLs

```html
<!-- HTTP/HTTPS URL -->
<img src="https://example.com/images/photo.jpg" alt="Photo" />

<!-- Note: Ensure URL is accessible during PDF generation -->
```

### Base64 Embedded

```html
<!-- Base64 encoded image -->
<img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUA..."
     alt="Embedded Image" />
```

**Use cases:**
- Small images
- No external dependencies
- Single-file distribution

---

## Image Styling

### Borders and Spacing

```css
.styled-image {
    border: 2pt solid #2563eb;
    border-radius: 5pt;
    padding: 10pt;
    margin: 20pt 0;
}

.shadow {
    /* Limited shadow support in PDF */
    box-shadow: 0 4pt 6pt rgba(0, 0, 0, 0.1);
}
```

### Alignment

```css
/* Float images */
.float-left {
    float: left;
    margin-right: 15pt;
    margin-bottom: 15pt;
}

.float-right {
    float: right;
    margin-left: 15pt;
    margin-bottom: 15pt;
}

/* Center alignment */
.centered {
    display: block;
    margin-left: auto;
    margin-right: auto;
}

/* Text alignment (for block images) */
.text-center {
    text-align: center;
}
```

---

## Data Binding

### Dynamic Image Sources

{% raw %}
```html
<!-- Bind image source -->
<img src="{{product.imageUrl}}"
     alt="{{product.name}}" />

<!-- Conditional images -->
{{#if user.profileImage}}
<img src="{{user.profileImage}}" alt="Profile" />
{{else}}
<img src="./images/default-avatar.png" alt="Default" />
{{/if}}

<!-- Loop through images -->
{{#each gallery}}
<img src="{{this.url}}"
     alt="{{this.caption}}"
     style="width: 200pt; margin: 10pt;" />
{{/each}}
```
{% endraw %}

---

## Practical Examples

### Example 1: Product Catalog

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Product Catalog</title>
    <style>
        @page {
            size: Letter;
            margin: 0.75in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            margin: 0;
        }

        h1 {
            font-size: 24pt;
            text-align: center;
            margin-bottom: 30pt;
            color: #1e40af;
        }

        /* ==============================================
           PRODUCT GRID
           ============================================== */
        .product-grid {
            display: table;
            width: 100%;
            table-layout: fixed;
        }

        .product-row {
            display: table-row;
        }

        .product-card {
            display: table-cell;
            width: 50%;
            padding: 15pt;
            vertical-align: top;
            page-break-inside: avoid;
        }

        .product-card img {
            width: 100%;
            height: 200pt;
            object-fit: cover;
            border-radius: 5pt;
            border: 1pt solid #e5e7eb;
        }

        .product-info {
            margin-top: 10pt;
        }

        .product-name {
            font-size: 14pt;
            font-weight: 600;
            margin: 0 0 5pt 0;
        }

        .product-description {
            font-size: 10pt;
            color: #666;
            margin: 0 0 10pt 0;
        }

        .product-price {
            font-size: 16pt;
            font-weight: 700;
            color: #2563eb;
            margin: 0;
        }
    </style>
</head>
<body>
    <h1>Product Catalog 2024</h1>

    <div class="product-grid">
        {{#each products}}
        {{#if @even}}
        <div class="product-row">
        {{/if}}
            <div class="product-card">
                <img src="{{this.imageUrl}}"
                     alt="{{this.name}}" />
                <div class="product-info">
                    <p class="product-name">{{this.name}}</p>
                    <p class="product-description">{{this.description}}</p>
                    <p class="product-price">${{this.price}}</p>
                </div>
            </div>
        {{#if @odd}}
        </div>
        {{/if}}
        {{/each}}
    </div>
</body>
</html>
```
{% endraw %}

### Example 2: Report with Charts

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Sales Report</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
        }

        h1 {
            font-size: 28pt;
            margin: 0 0 10pt 0;
            color: #1e40af;
        }

        h2 {
            font-size: 20pt;
            margin: 30pt 0 15pt 0;
            color: #2563eb;
        }

        .report-header {
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 20pt;
            margin-bottom: 30pt;
        }

        .company-logo {
            width: 150pt;
            margin-bottom: 15pt;
        }

        /* ==============================================
           CHART CONTAINERS
           ============================================== */
        .chart-container {
            margin: 20pt 0;
            text-align: center;
            page-break-inside: avoid;
        }

        .chart-image {
            width: 100%;
            max-width: 500pt;
            height: auto;
            border: 1pt solid #e5e7eb;
            border-radius: 5pt;
        }

        .chart-caption {
            font-size: 10pt;
            color: #666;
            margin-top: 10pt;
            font-style: italic;
        }

        /* ==============================================
           METRICS GRID
           ============================================== */
        .metrics-grid {
            display: table;
            width: 100%;
            margin: 20pt 0;
        }

        .metrics-row {
            display: table-row;
        }

        .metric-card {
            display: table-cell;
            width: 33.33%;
            padding: 15pt;
            border: 1pt solid #e5e7eb;
            border-radius: 5pt;
            text-align: center;
            vertical-align: top;
        }

        .metric-card + .metric-card {
            border-left: none;
        }

        .metric-icon {
            width: 40pt;
            height: 40pt;
            margin-bottom: 10pt;
        }

        .metric-value {
            font-size: 24pt;
            font-weight: 700;
            color: #2563eb;
            margin: 0;
        }

        .metric-label {
            font-size: 10pt;
            color: #666;
            margin: 5pt 0 0 0;
        }
    </style>
</head>
<body>
    <div class="report-header">
        <img src="{{company.logoUrl}}"
             alt="Company Logo"
             class="company-logo" />
        <h1>Q4 Sales Report</h1>
        <p>Report Date: {{reportDate}}</p>
    </div>

    <h2>Key Metrics</h2>

    <div class="metrics-grid">
        <div class="metrics-row">
            <div class="metric-card">
                <img src="./images/icon-revenue.svg"
                     alt="Revenue"
                     class="metric-icon" />
                <p class="metric-value">${{metrics.revenue}}</p>
                <p class="metric-label">Total Revenue</p>
            </div>
            <div class="metric-card">
                <img src="./images/icon-customers.svg"
                     alt="Customers"
                     class="metric-icon" />
                <p class="metric-value">{{metrics.customers}}</p>
                <p class="metric-label">New Customers</p>
            </div>
            <div class="metric-card">
                <img src="./images/icon-growth.svg"
                     alt="Growth"
                     class="metric-icon" />
                <p class="metric-value">{{metrics.growth}}%</p>
                <p class="metric-label">Year-over-Year</p>
            </div>
        </div>
    </div>

    <h2>Sales Performance</h2>

    <div class="chart-container">
        <img src="{{charts.salesTrend}}"
             alt="Sales Trend"
             class="chart-image" />
        <p class="chart-caption">
            Figure 1: Monthly sales performance for Q4 2024
        </p>
    </div>

    <h2>Regional Distribution</h2>

    <div class="chart-container">
        <img src="{{charts.regionalBreakdown}}"
             alt="Regional Breakdown"
             class="chart-image" />
        <p class="chart-caption">
            Figure 2: Sales distribution by region
        </p>
    </div>

    <h2>Product Categories</h2>

    <div class="chart-container">
        <img src="{{charts.categoryAnalysis}}"
             alt="Category Analysis"
             class="chart-image" />
        <p class="chart-caption">
            Figure 3: Revenue breakdown by product category
        </p>
    </div>
</body>
</html>
```
{% endraw %}

---

## Try It Yourself

### Exercise 1: Image Gallery

Create a PDF with:
- 6-9 images in a grid layout
- Consistent sizing with object-fit: cover
- Captions below each image
- Page breaks avoided within image cards

### Exercise 2: Business Card

Design a business card with:
- Company logo (150pt wide)
- Profile photo (100pt square with border-radius)
- Contact information
- QR code image

### Exercise 3: Data-Driven Report

Create a report that:
- Displays company logo from data binding
- Shows multiple chart images from URLs
- Includes conditional image display
- Has proper captions and figure numbers

---

## Common Pitfalls

### ❌ Not Specifying Image Size

```html
<!-- Browser default, may be too large -->
<img src="./images/photo.jpg" alt="Photo" />
```

✅ **Solution:**

```html
<img src="./images/photo.jpg"
     alt="Photo"
     style="width: 300pt; height: auto;" />
```

### ❌ Using Huge Image Files

```html
<!-- 5MB photo at full resolution -->
<img src="./images/photo-original.jpg" alt="Photo" />
```

✅ **Solution:**

```html
<!-- Resized and optimized (200KB) -->
<img src="./images/photo-optimized.jpg"
     alt="Photo"
     style="width: 400pt;" />
```

### ❌ Broken Remote URLs

```html
<!-- URL not accessible during generation -->
<img src="https://example.com/temp/image.jpg" alt="Image" />
```

✅ **Solution:**

```html
<!-- Download and use local path -->
<img src="./images/downloaded-image.jpg" alt="Image" />

<!-- Or ensure URL is reliable -->
<img src="https://cdn.example.com/static/image.jpg" alt="Image" />
```

### ❌ Missing Alt Text

```html
<img src="./images/chart.png" />
```

✅ **Solution:**

```html
<img src="./images/chart.png"
     alt="Sales performance chart for Q4 2024" />
```

---

## Image Optimization Checklist

- [ ] Images resized to appropriate dimensions
- [ ] File sizes optimized (use compression)
- [ ] Correct format chosen (PNG/JPEG/SVG)
- [ ] Alt text provided for accessibility
- [ ] Paths are relative or reliable
- [ ] Images tested in generated PDF
- [ ] Aspect ratios maintained
- [ ] Remote images are accessible

---

## Best Practices

1. **Optimize Before Embedding** - Resize and compress images
2. **Use Appropriate Formats** - SVG for logos, JPEG for photos
3. **Specify Dimensions** - Use pt units for precision
4. **Provide Alt Text** - Improves accessibility
5. **Local Paths for Production** - More reliable than remote URLs
6. **Maintain Aspect Ratios** - Avoid distortion
7. **Test with Real Data** - Verify all image sources work
8. **Consider File Size** - Affects PDF generation speed

---

## Next Steps

1. **[SVG Basics](02_svg_basics.md)** - Scalable vector graphics
2. **[SVG Drawing](03_svg_drawing.md)** - Create dynamic charts
3. **[Content Best Practices](08_content_best_practices.md)** - Optimization tips

---

**Continue learning →** [SVG Basics](02_svg_basics.md)
