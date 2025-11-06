---
layout: default
title: data-min-scale
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-min-scale : The Minimum Scaling Factor Attribute

The `data-min-scale` attribute specifies the minimum scaling factor for image elements when they are automatically resized to fit within their container. It prevents images from being scaled down too much, maintaining a minimum readable or visible size in PDF documents.

## Usage

The `data-min-scale` attribute is used with `<img>` elements to:
- Set a minimum scale limit for automatic image resizing
- Prevent images from becoming too small
- Maintain image legibility and visibility
- Control image scaling behavior in constrained spaces
- Ensure minimum quality standards for scaled images

```html
<!-- Image will not scale below 50% of original size -->
<img src="large-diagram.png" data-min-scale="0.5" style="max-width: 100%;" />
```

---

## Supported Elements

The `data-min-scale` attribute is supported by:

| Element | Description |
|---------|-------------|
| `<img>` | Image element |

---

## Attribute Values

### Syntax

```html
<img src="image.jpg" data-min-scale="factor" />
```

### Value Type

| Type | Range | Description | Example |
|------|-------|-------------|---------|
| Decimal | 0.0 - 1.0 | Minimum scale as fraction of original | `data-min-scale="0.5"` = 50% minimum |
| Percentage | 1% - 100% | Minimum scale as percentage | `data-min-scale="0.75"` = 75% minimum |

### Common Values

| Value | Meaning | Use Case |
|-------|---------|----------|
| `1.0` | No scaling down allowed | Preserve original size always |
| `0.75` | Minimum 75% of original | Slight scaling allowed |
| `0.5` | Minimum 50% of original | Moderate scaling allowed |
| `0.25` | Minimum 25% of original | Significant scaling allowed |
| `0.1` | Minimum 10% of original | Maximum flexibility |

### Default Behavior

If `data-min-scale` is **not specified**:
- Images can scale down to fit available space without limit
- Very small images may result if container is narrow
- No minimum size constraint is applied

---

## Binding Values

The `data-min-scale` attribute supports data binding:

### Static Minimum Scale

```html
<img src="diagram.png" data-min-scale="0.6" style="max-width: 100%;" />
```

### Dynamic Minimum Scale

```html
<!-- Model: { minImageScale: 0.5 } -->
<img src="chart.png" data-min-scale="{{model.minImageScale}}" />
```

### Conditional Scaling

```html
<!-- Model: { allowSmallImages: false } -->
<img src="photo.jpg"
     data-min-scale="{{model.allowSmallImages ? '0.25' : '0.75'}}" />
```

---

## Notes

### Scaling Behavior

When an image is placed in a container:

1. Image tries to fit within available space
2. If image is larger, it scales down
3. Scaling stops at `data-min-scale` limit
4. If minimum scale is reached and image still doesn't fit, it may overflow or clip

### Aspect Ratio Preservation

The `data-min-scale` works alongside aspect ratio constraints:
- Images maintain their aspect ratio while scaling
- The minimum scale applies to both dimensions proportionally
- Width and height scale together

### Use Cases

**Maintain Readability:**
```html
<!-- Diagram with text that must remain readable -->
<img src="flowchart.png" data-min-scale="0.7" />
```

**Quality Control:**
```html
<!-- Photo that shouldn't be too pixelated -->
<img src="product-photo.jpg" data-min-scale="0.5" />
```

**Technical Diagrams:**
```html
<!-- Technical drawing with fine details -->
<img src="technical-drawing.svg" data-min-scale="0.8" />
```

### Interaction with CSS Sizing

The `data-min-scale` works with CSS properties:

```html
<!-- CSS max-width limits container size -->
<!-- data-min-scale limits how small image can get -->
<img src="large-image.png"
     style="max-width: 100%; height: auto;"
     data-min-scale="0.6" />
```

### Container Constraints

If container is too small and minimum scale is reached:
- Image may overflow the container
- Consider using `overflow: hidden` on container
- Or adjust layout to accommodate minimum size

### PDF-Specific Considerations

In PDF generation:
- Image scaling affects file size and quality
- Smaller scales may reduce clarity
- Minimum scale ensures acceptable quality
- Balance between size constraints and quality needs

---

## Examples

### Example 1: Basic Minimum Scale

```html
<!-- Image will not scale below 60% of original size -->
<div style="width: 300pt;">
    <img src="large-diagram.png"
         data-min-scale="0.6"
         style="max-width: 100%; height: auto;" />
</div>
```

### Example 2: Technical Diagram Protection

```html
<!-- Ensure technical details remain visible -->
<div class="diagram-container" style="max-width: 500pt;">
    <img src="architecture-diagram.svg"
         alt="System Architecture"
         data-min-scale="0.75"
         style="width: 100%; height: auto;" />
    <p style="font-size: 9pt; text-align: center;">
        Figure 1: System Architecture Overview
    </p>
</div>
```

### Example 3: Product Photos

```html
<!-- Don't let product images get too small -->
<div style="width: 200pt; height: 200pt; overflow: hidden;">
    <img src="product-image.jpg"
         alt="Product Photo"
         data-min-scale="0.5"
         style="width: 100%; height: auto;" />
</div>
```

### Example 4: Report with Charts

```html
<h2>Quarterly Sales Performance</h2>

<div style="width: 400pt; text-align: center;">
    <img src="sales-chart-q4.png"
         alt="Q4 Sales Chart"
         data-min-scale="0.7"
         style="max-width: 100%;" />
    <p style="font-size: 10pt; margin-top: 10pt;">
        Figure 2.1: Q4 Sales by Region
    </p>
</div>
```

### Example 5: Multiple Images with Different Scales

```html
<div style="display: grid; grid-template-columns: 1fr 1fr; gap: 20pt;">
    <!-- Logo: never scale down -->
    <div>
        <img src="company-logo.png"
             data-min-scale="1.0"
             style="width: 100%;" />
    </div>

    <!-- Photo: can scale more -->
    <div>
        <img src="team-photo.jpg"
             data-min-scale="0.4"
             style="width: 100%;" />
    </div>
</div>
```

### Example 6: Responsive Image Sizing

```html
<!-- Large screen: full size -->
<!-- Small container: scales down but not below 65% -->
<img src="infographic.png"
     alt="Data Infographic"
     data-min-scale="0.65"
     style="max-width: 100%; height: auto;
            border: 1pt solid #ddd; padding: 10pt;" />
```

### Example 7: Data-Bound Image Scaling

```html
<!-- Model: { images: [{src: "...", minScale: 0.5}, ...] } -->

<template data-bind="{{model.images}}">
    <div style="margin-bottom: 20pt;">
        <img src="{{.src}}"
             data-min-scale="{{.minScale}}"
             style="max-width: 100%;" />
    </div>
</template>
```

### Example 8: Scientific Figures

```html
<div class="figure" style="page-break-inside: avoid;">
    <div style="width: 500pt; margin: 0 auto;">
        <img src="experiment-results.png"
             alt="Experiment Results"
             data-min-scale="0.8"
             style="width: 100%; border: 1pt solid #333;" />
    </div>
    <p style="text-align: center; font-style: italic; margin-top: 10pt;">
        Figure 3: Experimental results showing temperature correlation
    </p>
</div>
```

### Example 9: Invoice with Logo

```html
<div style="display: flex; justify-content: space-between; margin-bottom: 30pt;">
    <!-- Company logo must remain readable -->
    <div style="width: 150pt;">
        <img src="company-logo.svg"
             alt="Company Logo"
             data-min-scale="0.9"
             style="width: 100%;" />
    </div>

    <div style="text-align: right;">
        <h1 style="margin: 0;">INVOICE</h1>
        <p>Invoice #: INV-2024-001</p>
    </div>
</div>
```

### Example 10: Technical Manual with Detailed Diagrams

```html
<h2>3. Installation Steps</h2>

<div style="margin: 20pt 0;">
    <h3>Step 1: Connect Components</h3>
    <img src="step1-diagram.png"
         alt="Step 1 Connection Diagram"
         data-min-scale="0.75"
         style="max-width: 100%; border: 2pt solid #336699; padding: 10pt;" />
</div>

<div style="margin: 20pt 0;">
    <h3>Step 2: Configure Settings</h3>
    <img src="step2-screenshot.png"
         alt="Step 2 Configuration Screen"
         data-min-scale="0.7"
         style="max-width: 100%; border: 2pt solid #336699; padding: 10pt;" />
</div>
```

### Example 11: Image Gallery with Minimum Sizes

```html
<h2>Product Gallery</h2>

<div style="display: grid; grid-template-columns: repeat(3, 1fr); gap: 15pt;">
    <div style="border: 1pt solid #ddd; padding: 10pt;">
        <img src="product1.jpg" data-min-scale="0.5" style="width: 100%;" />
        <p style="text-align: center; margin-top: 5pt;">Product A</p>
    </div>

    <div style="border: 1pt solid #ddd; padding: 10pt;">
        <img src="product2.jpg" data-min-scale="0.5" style="width: 100%;" />
        <p style="text-align: center; margin-top: 5pt;">Product B</p>
    </div>

    <div style="border: 1pt solid #ddd; padding: 10pt;">
        <img src="product3.jpg" data-min-scale="0.5" style="width: 100%;" />
        <p style="text-align: center; margin-top: 5pt;">Product C</p>
    </div>
</div>
```

### Example 12: Certificate with Seal

```html
<div style="text-align: center; padding: 40pt; border: 5pt double #336699;">
    <h1 style="font-size: 32pt;">Certificate of Achievement</h1>

    <p style="margin: 20pt 0;">This certifies that</p>
    <h2 style="font-size: 24pt; color: #336699;">John Doe</h2>

    <p style="margin: 20pt 0;">has completed the course</p>

    <!-- Official seal must remain visible -->
    <div style="margin: 30pt auto; width: 100pt;">
        <img src="official-seal.png"
             alt="Official Seal"
             data-min-scale="0.95"
             style="width: 100%;" />
    </div>

    <p style="font-size: 11pt;">Date: January 15, 2024</p>
</div>
```

### Example 13: Dashboard with Metric Visualizations

```html
<h1>Performance Dashboard</h1>

<div style="display: grid; grid-template-columns: 1fr 1fr; gap: 20pt;">
    <!-- Revenue chart -->
    <div style="padding: 15pt; border: 1pt solid #ddd;">
        <h3>Revenue Trend</h3>
        <img src="revenue-chart.png"
             data-min-scale="0.6"
             style="width: 100%;" />
    </div>

    <!-- Growth chart -->
    <div style="padding: 15pt; border: 1pt solid #ddd;">
        <h3>Growth Rate</h3>
        <img src="growth-chart.png"
             data-min-scale="0.6"
             style="width: 100%;" />
    </div>

    <!-- User chart -->
    <div style="padding: 15pt; border: 1pt solid #ddd;">
        <h3>Active Users</h3>
        <img src="users-chart.png"
             data-min-scale="0.6"
             style="width: 100%;" />
    </div>

    <!-- Conversion chart -->
    <div style="padding: 15pt; border: 1pt solid #ddd;">
        <h3>Conversion Rate</h3>
        <img src="conversion-chart.png"
             data-min-scale="0.6"
             style="width: 100%;" />
    </div>
</div>
```

### Example 14: Conditional Minimum Scale Based on Image Type

```html
<!-- Model: { images: [{src: "...", type: "diagram|photo", ...}] } -->

<template data-bind="{{model.images}}">
    <div style="margin-bottom: 20pt;">
        <!-- Diagrams need higher minimum scale than photos -->
        <img src="{{.src}}"
             alt="{{.description}}"
             data-min-scale="{{.type === 'diagram' ? '0.75' : '0.4'}}"
             style="max-width: 100%; border: 1pt solid #ddd; padding: 10pt;" />
        <p style="text-align: center; font-size: 10pt; margin-top: 5pt;">
            {{.caption}}
        </p>
    </div>
</template>
```

### Example 15: Complex Report with Multiple Image Types

```html
<h1>Annual Report 2024</h1>

<!-- Company logo header -->
<div style="margin-bottom: 30pt;">
    <img src="company-logo.svg"
         alt="Company Logo"
         data-min-scale="1.0"
         style="width: 200pt;" />
</div>

<!-- Executive summary chart -->
<h2>Executive Summary</h2>
<div style="width: 500pt; margin: 20pt 0;">
    <img src="summary-metrics.png"
         alt="Key Metrics"
         data-min-scale="0.7"
         style="width: 100%;" />
</div>

<!-- Financial charts -->
<h2>Financial Performance</h2>
<div style="display: grid; grid-template-columns: 1fr 1fr; gap: 20pt;">
    <div>
        <h3>Revenue</h3>
        <img src="revenue-breakdown.png"
             data-min-scale="0.65"
             style="width: 100%;" />
    </div>
    <div>
        <h3>Profit Margins</h3>
        <img src="profit-margins.png"
             data-min-scale="0.65"
             style="width: 100%;" />
    </div>
</div>

<!-- Team photos -->
<h2>Leadership Team</h2>
<div style="display: grid; grid-template-columns: repeat(4, 1fr); gap: 15pt;">
    <div>
        <img src="ceo-photo.jpg" data-min-scale="0.4" style="width: 100%;" />
        <p style="text-align: center; font-size: 9pt;">CEO</p>
    </div>
    <div>
        <img src="cfo-photo.jpg" data-min-scale="0.4" style="width: 100%;" />
        <p style="text-align: center; font-size: 9pt;">CFO</p>
    </div>
    <div>
        <img src="cto-photo.jpg" data-min-scale="0.4" style="width: 100%;" />
        <p style="text-align: center; font-size: 9pt;">CTO</p>
    </div>
    <div>
        <img src="coo-photo.jpg" data-min-scale="0.4" style="width: 100%;" />
        <p style="text-align: center; font-size: 9pt;">COO</p>
    </div>
</div>

<!-- Technical architecture diagram -->
<h2>Technology Infrastructure</h2>
<div style="page-break-inside: avoid; margin: 20pt 0;">
    <img src="architecture-diagram.svg"
         alt="System Architecture"
         data-min-scale="0.8"
         style="width: 100%; border: 2pt solid #336699; padding: 15pt;" />
    <p style="text-align: center; font-style: italic; margin-top: 10pt;">
        Figure 5.1: Complete system architecture showing all components and integrations
    </p>
</div>
```

---

## See Also

- [img element](/reference/htmltags/img.html) - Image element
- [src attribute](/reference/htmlattributes/src.html) - Image source
- [alt attribute](/reference/htmlattributes/alt.html) - Alternative text
- [width and height attributes](/reference/htmlattributes/width_height.html) - Image dimensions
- [CSS Sizing](/reference/styles/sizing.html) - CSS sizing properties
- [Image Handling](/reference/images/) - Complete image handling reference
- [Performance](/reference/performance/images.html) - Image performance optimization

---
