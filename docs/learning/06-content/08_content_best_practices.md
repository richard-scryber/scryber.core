---
layout: default
title: Content Best Practices
nav_order: 8
parent: Content Components
parent_url: /learning/06-content/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Content Best Practices

Master professional patterns, optimization techniques, accessibility guidelines, and common gotchas for all content components in PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Optimize images and graphics for PDFs
- Improve table performance
- Enhance accessibility
- Apply professional content patterns
- Troubleshoot common issues
- Build production-ready documents

---

## Image Optimization

### File Size Optimization

```html
<!-- ❌ Unoptimized: 5MB original photo -->
<img src="./photos/product-original.jpg"
     alt="Product"
     style="width: 300pt;" />

<!-- ✅ Optimized: 150KB resized and compressed -->
<img src="./photos/product-optimized.jpg"
     alt="Product"
     style="width: 300pt;" />
```

**Optimization Steps:**
1. Resize to target dimensions (e.g., 2× display size for quality)
2. Compress appropriately for format
3. Choose right format (JPEG for photos, PNG for graphics, SVG for logos)

### Image Format Selection

| Use Case | Format | Reason |
|----------|--------|--------|
| Photos | JPEG | Smaller file size, good quality |
| Logos | SVG or PNG | Sharp edges, scalable |
| Screenshots | PNG | Preserves text clarity |
| Charts | SVG | Scalable, small file size |
| Icons | SVG | Perfect scaling, tiny files |

### Resolution Guidelines

```html
<!-- For 300pt wide image in PDF: -->

<!-- ❌ Too low: 300px (1× DPI) -->
<img src="./image-300px.jpg" style="width: 300pt;" />

<!-- ✅ Good: 600px (2× DPI) -->
<img src="./image-600px.jpg" style="width: 300pt;" />

<!-- ⚠️ Overkill: 1200px (4× DPI) - larger file, minimal benefit -->
<img src="./image-1200px.jpg" style="width: 300pt;" />
```

**Recommendation:** 2× the display size at 96 DPI.

---

## SVG Optimization

### Simplify SVG Code

```html
<!-- ❌ Exported SVG with unnecessary complexity -->
<svg viewBox="0 0 100 100">
    <defs>
        <clipPath id="clip0">...</clipPath>
        <filter id="filter0">...</filter>
    </defs>
    <g transform="matrix(1.2, 0, 0, 1.2, 0, 0)">
        <path d="M 1.234567 2.345678 L 3.456789..." fill="#3B82F6" />
        <!-- Hundreds of unnecessary precision decimals -->
    </g>
</svg>

<!-- ✅ Optimized SVG -->
<svg viewBox="0 0 100 100" width="100" height="100">
    <circle cx="50" cy="50" r="40" fill="#3b82f6" />
</svg>
```

**Optimization Tools:**
- SVGO (CLI tool)
- SVGOMG (online tool)
- Manual simplification for simple graphics

### Inline vs External

```html
<!-- ✅ Inline for data binding and small graphics -->
<svg viewBox="0 0 100 100" width="50" height="50">
    <rect width="100" height="{{dataValue}}" fill="#3b82f6" />
</svg>

<!-- ✅ External for complex, reusable graphics -->
<img src="./graphics/complex-diagram.svg"
     alt="System Architecture"
     style="width: 400pt;" />
```

---

## Table Performance

### Column Width Optimization

```css
/* ❌ Auto layout - slow for large tables */
table {
    table-layout: auto;  /* Calculates based on content */
}

/* ✅ Fixed layout - fast */
table {
    table-layout: fixed;  /* Uses specified widths */
    width: 100%;
}

th:nth-child(1) { width: 40%; }
th:nth-child(2) { width: 30%; }
th:nth-child(3) { width: 30%; }
```

### Limit Table Complexity

```html
<!-- ❌ Overly complex table -->
<table>
    <tbody>
        <!-- 10,000+ rows with complex styling -->
        <!-- Heavy calculations in every cell -->
        <!-- Multiple nested elements per cell -->
    </tbody>
</table>

<!-- ✅ Optimized table -->
<table>
    <tbody>
        <!-- Reasonable row count (or paginated) -->
        <!-- Simple cell content -->
        <!-- Calculations outside loops when possible -->
    </tbody>
</table>
```

### Page Break Strategy

```css
/* Control table pagination */
table {
    page-break-inside: auto;  /* Allow table to span pages */
}

thead {
    display: table-header-group;  /* Repeat headers */
}

tbody tr {
    page-break-inside: avoid;  /* Don't break rows */
}

/* For very large cells, allow breaks */
tbody tr.large-content {
    page-break-inside: auto;
}
```

---

## List Best Practices

### Appropriate List Length

```html
<!-- ❌ Too long - consider pagination or sectioning -->
<ul>
    <!-- 500+ items -->
</ul>

<!-- ✅ Manageable length or grouped -->
<h2>Section A</h2>
<ul>
    <!-- 10-20 items -->
</ul>

<h2>Section B</h2>
<ul>
    <!-- 10-20 items -->
</ul>
```

### Limit Nesting Depth

```html
<!-- ❌ Too many levels -->
<ul>
    <li>Level 1
        <ul>
            <li>Level 2
                <ul>
                    <li>Level 3
                        <ul>
                            <li>Level 4</li>  <!-- Too deep! -->
                        </ul>
                    </li>
                </ul>
            </li>
        </ul>
    </li>
</ul>

<!-- ✅ 2-3 levels maximum -->
<ul>
    <li>Level 1
        <ul>
            <li>Level 2</li>
            <li>Level 2</li>
        </ul>
    </li>
</ul>
```

---

## Accessibility

### Alternative Text

```html
<!-- ❌ Missing alt text -->
<img src="./chart.png" />

<!-- ❌ Useless alt text -->
<img src="./chart.png" alt="image" />

<!-- ✅ Descriptive alt text -->
<img src="./chart.png"
     alt="Bar chart showing quarterly revenue growth from Q1 to Q4 2024" />
```

### Table Headers

```html
<!-- ❌ No proper headers -->
<table>
    <tr>
        <td><strong>Name</strong></td>
        <td><strong>Age</strong></td>
    </tr>
    <tr>
        <td>John</td>
        <td>30</td>
    </tr>
</table>

<!-- ✅ Proper semantic headers -->
<table>
    <thead>
        <tr>
            <th>Name</th>
            <th>Age</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>John</td>
            <td>30</td>
        </tr>
    </tbody>
</table>
```

### Document Structure

```html
<!-- ❌ Poor structure -->
<div style="font-size: 24pt; font-weight: bold;">Title</div>
<div style="font-size: 18pt; font-weight: bold;">Section</div>

<!-- ✅ Semantic structure -->
<h1>Title</h1>
<h2>Section</h2>
```

---

## Professional Patterns

### Pattern 1: Data Dashboard

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Dashboard Pattern</title>
    <style>
        @page {
            size: Letter landscape;
            margin: 0.75in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 11pt;
            margin: 0;
        }

        /* ==============================================
           METRICS GRID
           ============================================== */
        .metrics {
            display: table;
            width: 100%;
            margin-bottom: 30pt;
        }

        .metric-row {
            display: table-row;
        }

        .metric-card {
            display: table-cell;
            width: 25%;
            padding: 15pt;
            text-align: center;
            border: 1pt solid #e5e7eb;
            border-radius: 5pt;
        }

        .metric-value {
            font-size: 32pt;
            font-weight: 700;
            color: #2563eb;
        }

        .metric-label {
            font-size: 10pt;
            color: #666;
            text-transform: uppercase;
        }

        /* ==============================================
           CHART SECTION
           ============================================== */
        .chart-container {
            margin: 20pt 0;
            text-align: center;
        }

        .chart-image {
            width: 100%;
            max-width: 600pt;
            height: auto;
        }
    </style>
</head>
<body>
    <!-- KPIs at top -->
    <div class="metrics">
        <div class="metric-row">
            <div class="metric-card">
                <div class="metric-value">$2.4M</div>
                <div class="metric-label">Revenue</div>
            </div>
            <div class="metric-card">
                <div class="metric-value">1,247</div>
                <div class="metric-label">Customers</div>
            </div>
            <div class="metric-card">
                <div class="metric-value">94%</div>
                <div class="metric-label">Satisfaction</div>
            </div>
            <div class="metric-card">
                <div class="metric-value">+18%</div>
                <div class="metric-label">Growth</div>
            </div>
        </div>
    </div>

    <!-- Charts below -->
    <div class="chart-container">
        <img src="./charts/revenue-trend.svg"
             alt="Revenue Trend Chart"
             class="chart-image" />
    </div>
</body>
</html>
```

### Pattern 2: Product Catalog

```html
<!-- Responsive grid with images and details -->
<div class="catalog-grid">
    {{#each products}}
    <div class="product-card">
        <!-- Optimized product image -->
        <img src="{{this.imageUrl}}"
             alt="{{this.name}}"
             style="width: 100%; height: 200pt; object-fit: cover;" />

        <h3>{{this.name}}</h3>

        <!-- Bulleted features -->
        <ul>
            {{#each this.features}}
            <li>{{this}}</li>
            {{/each}}
        </ul>

        <p class="price">${{this.price}}</p>
    </div>
    {{/each}}
</div>
```

### Pattern 3: Report with Tables and Charts

```html
<!-- Mixed content with optimal ordering -->
<h1>Annual Report</h1>

<!-- Executive summary first -->
<section class="summary">
    <p>...</p>
</section>

<!-- Key chart -->
<img src="./charts/overview.svg" alt="Overview Chart" style="width: 100%;" />

<!-- Detailed data table -->
<table>
    <!-- Large data table -->
</table>

<!-- Supporting charts -->
<img src="./charts/breakdown.svg" alt="Breakdown Chart" style="width: 100%;" />
```

---

## Performance Optimization

### Lazy Loading Considerations

```html
<!-- For very large documents, consider splitting -->

<!-- Document 1: Main content -->
main-report.html

<!-- Document 2: Appendix with large tables -->
appendix.html

<!-- Combine during generation or provide as separate PDFs -->
```

### Caching Strategy

```html
<!-- Cache frequently used images -->
<!-- Load once, reference multiple times -->

<img src="./logos/company-logo.svg" alt="Logo" />
<!-- Logo appears on every page -->

<!-- Generator may cache this image -->
```

### Resource Limits

**Guidelines:**
- Images: < 2MB each
- Total document: < 50MB for good performance
- Tables: < 1000 rows per table
- SVG: < 100KB per graphic
- Attachments: < 10MB each

---

## Troubleshooting Guide

### Images Not Showing

**Possible Causes:**
1. Incorrect file path
2. File doesn't exist
3. Unsupported format
4. Corrupted image file

**Solutions:**
- Verify file exists at specified path
- Use supported formats (PNG, JPEG, SVG)
- Test with simple image first
- Check file permissions

### Table Layout Issues

**Possible Causes:**
1. Missing table-layout property
2. No column widths specified
3. Content too wide for columns
4. border-collapse not set

**Solutions:**
- Set `table-layout: fixed`
- Specify column widths
- Use `overflow-wrap: break-word` for long text
- Always use `border-collapse: collapse`

### SVG Not Rendering

**Possible Causes:**
1. Missing viewBox
2. No width/height specified
3. Complex SVG unsupported
4. Incorrect SVG syntax

**Solutions:**
- Always include viewBox
- Specify width and height
- Simplify SVG code
- Validate SVG syntax

### Page Break Problems

**Possible Causes:**
1. Content forced together
2. No break control specified
3. Container prevents breaks

**Solutions:**
- Use `page-break-inside: avoid` on elements
- Allow breaks in tbody: `page-break-inside: auto`
- Test with real data volumes

---

## Content Checklist

### Images
- [ ] Optimized file sizes (< 500KB each)
- [ ] Appropriate format chosen
- [ ] 2× resolution for quality
- [ ] Alt text provided
- [ ] Paths verified

### SVG
- [ ] viewBox defined
- [ ] Width and height specified
- [ ] Code optimized/simplified
- [ ] Tested in PDF

### Lists
- [ ] 2-3 nesting levels maximum
- [ ] Proper spacing between items
- [ ] Semantic HTML used
- [ ] Data binding tested

### Tables
- [ ] table-layout: fixed
- [ ] Column widths defined
- [ ] border-collapse: collapse
- [ ] Headers use <thead>
- [ ] Page breaks controlled
- [ ] Calculations validated

### General
- [ ] Tested with real data
- [ ] Performance acceptable
- [ ] Accessible structure
- [ ] PDF renders correctly

---

## Best Practices Summary

1. **Optimize Images** - Resize and compress before embedding
2. **Use SVG for Scalability** - Logos, icons, charts
3. **Fixed Table Layout** - Performance and predictability
4. **Limit Complexity** - Simple is faster and more reliable
5. **Semantic HTML** - Proper heading structure
6. **Test Thoroughly** - Real data, various sizes
7. **Control Page Breaks** - Prevent awkward splits
8. **Provide Alt Text** - Accessibility and context
9. **Cache Resources** - Reuse images and graphics
10. **Monitor File Sizes** - Keep documents manageable

---

## Performance Targets

| Metric | Target | Maximum |
|--------|--------|---------|
| Page generation time | < 1s per page | < 5s per page |
| Image size | < 500KB | < 2MB |
| Total PDF size | < 10MB | < 50MB |
| Table rows | < 500 per table | < 1000 per table |
| SVG complexity | < 50KB | < 100KB |

---

## Next Steps

You've completed the Content Components series! Continue your journey:

1. **[Configuration](/learning/07-configuration/)** - Document settings and optimization
2. **[Practical Applications](/learning/08-practical/)** - Real-world document examples
3. **Review previous series** - Reinforce learning

---

**Continue learning →** [Configuration Series](/learning/07-configuration/)
