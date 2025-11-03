---
layout: default
title: width and height
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @width and @height : The Sizing Attributes

The `width` and `height` attributes control the dimensions of elements in PDF documents. They are primarily used with images, iframes, tables, and table cells, supporting various units (points, pixels, percentages) and enabling precise layout control through both explicit values and data binding.

## Usage

The `width` and `height` attributes control element dimensions:
- Set explicit sizes for images, iframes, tables, and cells
- Support multiple units: points (pt), pixels (px), percentages (%)
- Can be used independently (one dimension with auto aspect ratio)
- Used together for explicit aspect control
- Support data binding for dynamic sizing
- Essential for consistent layout and spacing

```html
<!-- Image with explicit dimensions -->
<img src="photo.jpg" width="400pt" height="300pt" />

<!-- Image with width only (maintains aspect ratio) -->
<img src="logo.png" width="150pt" />

<!-- Percentage-based sizing -->
<img src="banner.jpg" width="100%" height="200pt" />

<!-- Table sizing -->
<table width="100%">
    <tr>
        <td width="30%">Left Column</td>
        <td width="70%">Right Column</td>
    </tr>
</table>

<!-- Dynamic sizing -->
<img src="{{model.imagePath}}" width="{{model.imageWidth}}pt" height="{{model.imageHeight}}pt" />
```

---

## Supported Elements

The `width` and `height` attributes are commonly used with:

### Images
- `<img>` - Image elements (primary use)

### Tables
- `<table>` - Table dimensions
- `<td>`, `<th>` - Table cell dimensions
- `<col>`, `<colgroup>` - Column sizing

### Embedded Content
- `<iframe>` - Iframe dimensions
- `<canvas>` - Canvas dimensions

### Block Elements
- `<div>` - Division container sizing (via style attribute typically)

**Note:** For most block elements, dimensions are typically set using CSS `style` attribute rather than dedicated `width`/`height` attributes.

---

## Binding Values

The `width` and `height` attributes support data binding:

```html
<!-- Dynamic image dimensions -->
<img src="{{model.imagePath}}"
     width="{{model.width}}pt"
     height="{{model.height}}pt" />

<!-- Calculated dimensions -->
<img src="photo.jpg"
     width="{{model.baseWidth * 2}}pt"
     height="{{model.baseHeight * 2}}pt" />

<!-- Conditional sizing -->
<img src="{{model.imagePath}}"
     width="{{model.isLarge ? '600pt' : '300pt'}}"
     height="{{model.isLarge ? '400pt' : '200pt'}}" />

<!-- Percentage from data -->
<table width="{{model.tableWidth}}%">
    <tr>
        <td width="{{model.leftColumnWidth}}%">Left</td>
        <td width="{{model.rightColumnWidth}}%">Right</td>
    </tr>
</table>

<!-- Repeating elements with varying sizes -->
<template data-bind="{{model.images}}">
    <img src="{{.url}}"
         width="{{.width}}pt"
         height="{{.height}}pt"
         alt="{{.description}}" />
</template>
```

**Data Model Example:**
```json
{
  "imagePath": "banner.jpg",
  "width": 800,
  "height": 400,
  "baseWidth": 150,
  "baseHeight": 100,
  "isLarge": true,
  "tableWidth": 100,
  "leftColumnWidth": 30,
  "rightColumnWidth": 70,
  "images": [
    {
      "url": "photo1.jpg",
      "width": 300,
      "height": 200,
      "description": "First photo"
    },
    {
      "url": "photo2.jpg",
      "width": 400,
      "height": 300,
      "description": "Second photo"
    }
  ]
}
```

---

## Notes

### Units of Measurement

Scryber supports multiple units for width and height:

| Unit | Description | Example | Best For |
|------|-------------|---------|----------|
| `pt` | Points (1/72 inch) | `width="400pt"` | PDF documents (recommended) |
| `px` | Pixels | `width="400px"` | Screen-based sizing |
| `%` | Percentage of container | `width="50%"` | Responsive layouts |
| `in` | Inches | `width="5.5in"` | Physical dimensions |
| `cm` | Centimeters | `width="14cm"` | Metric measurements |
| `mm` | Millimeters | `width="140mm"` | Precise metric sizing |

```html
<!-- Points (recommended for PDF) -->
<img src="photo.jpg" width="400pt" height="300pt" />

<!-- Percentage -->
<table width="100%">...</table>

<!-- Inches -->
<img src="print-image.jpg" width="8in" height="10in" />

<!-- Centimeters -->
<img src="metric-image.jpg" width="20cm" height="15cm" />
```

### Aspect Ratio Preservation

When only one dimension is specified, the aspect ratio is typically maintained:

```html
<!-- Width specified, height auto-calculated -->
<img src="photo.jpg" width="400pt" />

<!-- Height specified, width auto-calculated -->
<img src="photo.jpg" height="300pt" />

<!-- Both specified (may distort if ratio doesn't match) -->
<img src="photo.jpg" width="400pt" height="200pt" />
```

### Image Sizing Best Practices

**Do:**
- Specify at least width for consistent layout
- Use points (pt) for PDF documents
- Consider source image resolution
- Maintain aspect ratios when possible
- Use percentages for flexible layouts

**Don't:**
- Excessively upscale low-resolution images
- Specify dimensions that severely distort images
- Omit dimensions (may cause layout issues)

```html
<!-- Good: Maintains aspect ratio -->
<img src="photo.jpg" width="400pt" />

<!-- Good: Explicit dimensions matching source ratio -->
<img src="photo.jpg" width="400pt" height="300pt" />

<!-- Caution: May distort if ratio is wrong -->
<img src="photo.jpg" width="400pt" height="200pt" />
```

### Table Sizing

Tables and cells support width and height attributes:

```html
<!-- Table with full width -->
<table width="100%" style="border-collapse: collapse;">
    <tr>
        <!-- Fixed width column -->
        <td width="150pt" style="border: 1pt solid #ccc;">Fixed 150pt</td>
        <!-- Remaining space -->
        <td style="border: 1pt solid #ccc;">Flexible width</td>
    </tr>
</table>

<!-- Table with percentage columns -->
<table width="100%">
    <tr>
        <td width="25%">25% width</td>
        <td width="50%">50% width</td>
        <td width="25%">25% width</td>
    </tr>
</table>

<!-- Table with row height -->
<table width="100%">
    <tr height="50pt">
        <td>50pt tall row</td>
    </tr>
    <tr>
        <td>Auto height row</td>
    </tr>
</table>
```

### Percentage-Based Layouts

Percentages are relative to the parent container:

```html
<!-- 50% of parent width -->
<img src="photo.jpg" width="50%" />

<!-- Full width of container -->
<img src="banner.jpg" width="100%" height="200pt" />

<!-- Table with percentage columns -->
<table width="100%">
    <tr>
        <td width="30%">Sidebar</td>
        <td width="70%">Main Content</td>
    </tr>
</table>
```

### Maximum and Minimum Dimensions

While `width` and `height` set explicit dimensions, use CSS for constraints:

```html
<!-- Using style for max-width -->
<img src="large-image.jpg"
     style="max-width: 600pt; width: 100%; height: auto;" />

<!-- Minimum dimensions -->
<div style="min-width: 200pt; min-height: 100pt; border: 1pt solid #ccc;">
    Content with minimum dimensions
</div>
```

### Iframe Sizing

Iframes require explicit dimensions:

```html
<!-- Fixed dimensions -->
<iframe src="content.html" width="600pt" height="400pt"></iframe>

<!-- Percentage width -->
<iframe src="content.html" width="100%" height="500pt"></iframe>
```

### Responsive Image Patterns

Common responsive image patterns:

```html
<!-- Full width, auto height -->
<img src="banner.jpg" width="100%" height="auto" />

<!-- Constrained maximum -->
<img src="photo.jpg" style="max-width: 600pt; width: 100%; height: auto;" />

<!-- Fixed aspect ratio container -->
<div style="width: 100%; position: relative; padding-bottom: 56.25%;">
    <!-- 16:9 aspect ratio -->
    <img src="video-placeholder.jpg"
         style="position: absolute; width: 100%; height: 100%;" />
</div>
```

### Common Sizing Patterns

```html
<!-- Thumbnail size -->
<img src="thumb.jpg" width="100pt" height="75pt" />

<!-- Standard photo -->
<img src="photo.jpg" width="400pt" height="300pt" />

<!-- Large banner -->
<img src="banner.jpg" width="800pt" height="200pt" />

<!-- Full-width header -->
<img src="header.jpg" width="100%" height="150pt" />

<!-- Square avatar -->
<img src="avatar.jpg" width="120pt" height="120pt"
     style="border-radius: 60pt;" />
```

### Size and File Size

Image display size doesn't affect file size in PDF:

```html
<!-- Same source image, different display sizes -->
<img src="high-res-photo.jpg" width="200pt" height="150pt" />
<img src="high-res-photo.jpg" width="400pt" height="300pt" />

<!-- Both embed the same image data, just displayed at different sizes -->
```

### Zero or Invalid Dimensions

Avoid zero or missing dimensions:

```html
<!-- Invalid: zero dimensions -->
<img src="photo.jpg" width="0" height="0" />  <!-- Won't display -->

<!-- Invalid: negative dimensions -->
<img src="photo.jpg" width="-100pt" />  <!-- May cause errors -->

<!-- Always specify positive, valid dimensions -->
<img src="photo.jpg" width="100pt" height="75pt" />
```

---

## Examples

### Basic Image Sizing

```html
<!-- Small thumbnail -->
<img src="thumbnail.jpg" width="80pt" height="60pt" />

<!-- Medium image -->
<img src="photo.jpg" width="300pt" height="225pt" />

<!-- Large featured image -->
<img src="featured.jpg" width="600pt" height="400pt" />

<!-- Banner image -->
<img src="banner.jpg" width="100%" height="150pt" />
```

### Maintaining Aspect Ratio

```html
<!-- Original: 1600x1200 (4:3 ratio) -->
<img src="photo.jpg" width="400pt" height="300pt" />

<!-- Same ratio, different size -->
<img src="photo.jpg" width="200pt" height="150pt" />

<!-- Width only, height auto-calculated -->
<img src="photo.jpg" width="400pt" />

<!-- Height only, width auto-calculated -->
<img src="photo.jpg" height="300pt" />
```

### Responsive Table Layout

```html
<table width="100%" style="border-collapse: collapse;">
    <tr>
        <th width="20%" style="border: 1pt solid #ccc; padding: 8pt;">ID</th>
        <th width="40%" style="border: 1pt solid #ccc; padding: 8pt;">Name</th>
        <th width="40%" style="border: 1pt solid #ccc; padding: 8pt;">Description</th>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt;">001</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Widget A</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Description of Widget A</td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt;">002</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Widget B</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Description of Widget B</td>
    </tr>
</table>
```

### Fixed and Flexible Columns

```html
<table width="100%" style="border-collapse: collapse;">
    <tr>
        <!-- Fixed width sidebar -->
        <td width="200pt" style="background-color: #f0f0f0; padding: 15pt;
                                  vertical-align: top;">
            <h3>Sidebar</h3>
            <ul>
                <li>Link 1</li>
                <li>Link 2</li>
                <li>Link 3</li>
            </ul>
        </td>
        <!-- Flexible main content -->
        <td style="padding: 15pt; vertical-align: top;">
            <h2>Main Content</h2>
            <p>This column takes up the remaining space...</p>
        </td>
    </tr>
</table>
```

### Image Gallery with Uniform Sizing

```html
<div>
    <h2>Photo Gallery</h2>

    <!-- All images same size for uniform grid -->
    <img src="photo1.jpg" width="200pt" height="150pt" style="margin: 5pt;" />
    <img src="photo2.jpg" width="200pt" height="150pt" style="margin: 5pt;" />
    <img src="photo3.jpg" width="200pt" height="150pt" style="margin: 5pt;" />
    <img src="photo4.jpg" width="200pt" height="150pt" style="margin: 5pt;" />
    <img src="photo5.jpg" width="200pt" height="150pt" style="margin: 5pt;" />
    <img src="photo6.jpg" width="200pt" height="150pt" style="margin: 5pt;" />
</div>
```

### Dynamic Sizing with Data Binding

```html
<!-- Model: {
    thumbnail: { url: "thumb.jpg", width: 100, height: 75 },
    featured: { url: "featured.jpg", width: 600, height: 400 }
} -->

<div>
    <h3>Thumbnail</h3>
    <img src="{{model.thumbnail.url}}"
         width="{{model.thumbnail.width}}pt"
         height="{{model.thumbnail.height}}pt" />

    <h2>Featured Image</h2>
    <img src="{{model.featured.url}}"
         width="{{model.featured.width}}pt"
         height="{{model.featured.height}}pt" />
</div>
```

### Product Listing with Images

```html
<!-- Model: { products: [
    { name: "Widget A", image: "widget-a.jpg", width: 250, height: 250 },
    { name: "Widget B", image: "widget-b.jpg", width: 250, height: 250 }
] } -->

<template data-bind="{{model.products}}">
    <div style="border: 1pt solid #ddd; padding: 15pt; margin-bottom: 20pt;
                display: inline-block; width: 280pt;">
        <img src="{{.image}}"
             width="{{.width}}pt"
             height="{{.height}}pt"
             alt="{{.name}}"
             style="display: block;" />
        <h3 style="margin: 10pt 0 0 0; text-align: center;">{{.name}}</h3>
    </div>
</template>
```

### Logo Sizing Variations

```html
<style>
    .logo-xl { width: 300pt; height: 100pt; }
    .logo-lg { width: 200pt; height: 67pt; }
    .logo-md { width: 150pt; height: 50pt; }
    .logo-sm { width: 100pt; height: 33pt; }
    .logo-xs { width: 60pt; height: 20pt; }
</style>

<div>
    <h2>Logo Sizes</h2>

    <div style="margin-bottom: 20pt;">
        <p>Extra Large:</p>
        <img src="logo.png" class="logo-xl" alt="Company Logo" />
    </div>

    <div style="margin-bottom: 20pt;">
        <p>Large:</p>
        <img src="logo.png" class="logo-lg" alt="Company Logo" />
    </div>

    <div style="margin-bottom: 20pt;">
        <p>Medium:</p>
        <img src="logo.png" class="logo-md" alt="Company Logo" />
    </div>

    <div style="margin-bottom: 20pt;">
        <p>Small:</p>
        <img src="logo.png" class="logo-sm" alt="Company Logo" />
    </div>

    <div>
        <p>Extra Small:</p>
        <img src="logo.png" class="logo-xs" alt="Company Logo" />
    </div>
</div>
```

### Chart Sizing

```html
<div>
    <h2>Sales Performance</h2>

    <!-- Standard chart size -->
    <img src="sales-chart.png" width="600pt" height="400pt"
         alt="Sales performance chart" />

    <h2>Revenue Breakdown</h2>

    <!-- Smaller chart -->
    <img src="revenue-pie.png" width="400pt" height="400pt"
         alt="Revenue breakdown pie chart" />

    <h2>Growth Trend</h2>

    <!-- Wide chart -->
    <img src="growth-line.png" width="700pt" height="300pt"
         alt="Growth trend line chart" />
</div>
```

### Profile Photo Sizing

```html
<!-- Model: { user: { name: "Alice", photo: "alice.jpg" } } -->

<div style="text-align: center;">
    <!-- Large profile photo -->
    <img src="{{model.user.photo}}"
         width="200pt" height="200pt"
         alt="{{model.user.name}}"
         style="border-radius: 100pt; border: 4pt solid #336699;" />

    <h2>{{model.user.name}}</h2>
</div>

<!-- Small profile photo in list -->
<div style="display: flex; align-items: center; margin-bottom: 10pt;">
    <img src="{{model.user.photo}}"
         width="50pt" height="50pt"
         alt="{{model.user.name}}"
         style="border-radius: 25pt; margin-right: 10pt;" />
    <span>{{model.user.name}}</span>
</div>
```

### Full-Width Banner

```html
<!DOCTYPE html>
<html>
<body style="margin: 0;">
    <!-- Full-width header banner -->
    <img src="header-banner.jpg" width="100%" height="200pt"
         alt="Welcome banner" />

    <div style="padding: 20pt;">
        <h1>Welcome to Our Service</h1>
        <p>Content goes here...</p>
    </div>

    <!-- Full-width footer banner -->
    <img src="footer-banner.jpg" width="100%" height="100pt"
         alt="Footer banner" />
</body>
</html>
```

### Icon Sizing

```html
<p>
    <img src="icons/info.png" width="16pt" height="16pt"
         style="vertical-align: middle;" />
    Information message
</p>

<p>
    <img src="icons/warning.png" width="20pt" height="20pt"
         style="vertical-align: middle;" />
    Warning message (larger icon)
</p>

<p>
    <img src="icons/success.png" width="24pt" height="24pt"
         style="vertical-align: middle;" />
    Success message (even larger icon)
</p>

<!-- Feature icons -->
<div style="text-align: center; margin: 20pt;">
    <img src="icons/feature1.png" width="64pt" height="64pt" />
    <img src="icons/feature2.png" width="64pt" height="64pt" style="margin: 0 20pt;" />
    <img src="icons/feature3.png" width="64pt" height="64pt" />
</div>
```

### Conditional Sizing

```html
<!-- Model: { displayMode: "thumbnail" } -->

<img src="product.jpg"
     width="{{model.displayMode == 'thumbnail' ? '100pt' : '400pt'}}"
     height="{{model.displayMode == 'thumbnail' ? '75pt' : '300pt'}}"
     alt="Product photo" />
```

### Screenshot Sizing

```html
<div>
    <h2>Desktop View</h2>
    <img src="desktop-screenshot.png" width="800pt" height="600pt"
         alt="Desktop application screenshot"
         style="border: 1pt solid #ccc;" />

    <h2>Tablet View</h2>
    <img src="tablet-screenshot.png" width="600pt" height="800pt"
         alt="Tablet application screenshot"
         style="border: 1pt solid #ccc; margin-top: 20pt;" />

    <h2>Mobile View</h2>
    <img src="mobile-screenshot.png" width="300pt" height="600pt"
         alt="Mobile application screenshot"
         style="border: 1pt solid #ccc; margin-top: 20pt;" />
</div>
```

### Table with Row Heights

```html
<table width="100%" style="border-collapse: collapse;">
    <!-- Header row with specific height -->
    <tr height="50pt">
        <th style="background-color: #336699; color: white; padding: 10pt;">
            Column 1
        </th>
        <th style="background-color: #336699; color: white; padding: 10pt;">
            Column 2
        </th>
    </tr>

    <!-- Data rows with minimum height -->
    <tr height="40pt">
        <td style="border: 1pt solid #ccc; padding: 8pt;">Data 1</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Data 2</td>
    </tr>

    <tr height="40pt">
        <td style="border: 1pt solid #ccc; padding: 8pt;">Data 3</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Data 4</td>
    </tr>
</table>
```

### Certificate/Badge Sizing

```html
<div style="text-align: center; margin: 30pt;">
    <h2>Certifications</h2>

    <img src="cert-iso.png" width="150pt" height="150pt"
         alt="ISO 9001 certification"
         style="margin: 10pt;" />

    <img src="cert-security.png" width="150pt" height="150pt"
         alt="Security certification"
         style="margin: 10pt;" />

    <img src="cert-quality.png" width="150pt" height="150pt"
         alt="Quality certification"
         style="margin: 10pt;" />
</div>
```

### Iframe Sizing Examples

```html
<div>
    <h2>Embedded Content</h2>

    <!-- Standard iframe -->
    <iframe src="external-content.html" width="600pt" height="400pt"
            style="border: 1pt solid #ccc;"></iframe>

    <!-- Full-width iframe -->
    <iframe src="report-section.html" width="100%" height="500pt"
            style="border: none; margin-top: 20pt;"></iframe>

    <!-- Small embedded widget -->
    <iframe src="widget.html" width="300pt" height="200pt"
            style="border: 1pt solid #ddd;"></iframe>
</div>
```

### Mixed Size Gallery

```html
<!-- Model: { photos: varying sizes based on orientation } -->

<div>
    <h2>Mixed Photo Gallery</h2>

    <!-- Landscape photo -->
    <img src="landscape.jpg" width="400pt" height="300pt"
         style="margin: 5pt;" />

    <!-- Portrait photo -->
    <img src="portrait.jpg" width="300pt" height="400pt"
         style="margin: 5pt;" />

    <!-- Square photo -->
    <img src="square.jpg" width="300pt" height="300pt"
         style="margin: 5pt;" />

    <!-- Panorama -->
    <img src="panorama.jpg" width="600pt" height="200pt"
         style="margin: 5pt; display: block;" />
</div>
```

### Signature Block with Sizing

```html
<div style="margin-top: 50pt;">
    <p>Approved by:</p>

    <div style="margin: 20pt 0;">
        <img src="signature-ceo.png" width="200pt" height="60pt"
             alt="CEO signature" />
        <p style="margin: 5pt 0 0 0;">
            <strong>John Smith</strong><br/>
            Chief Executive Officer
        </p>
    </div>

    <div style="margin: 20pt 0;">
        <img src="signature-cfo.png" width="200pt" height="60pt"
             alt="CFO signature" />
        <p style="margin: 5pt 0 0 0;">
            <strong>Jane Doe</strong><br/>
            Chief Financial Officer
        </p>
    </div>
</div>
```

---

## See Also

- [img](/reference/htmltags/img.html) - Image element
- [table](/reference/htmltags/table.html) - Table element
- [td](/reference/htmltags/td.html) - Table cell element
- [iframe](/reference/htmltags/iframe.html) - Iframe element
- [src](/reference/htmlattributes/src.html) - Source attribute for images
- [alt](/reference/htmlattributes/alt.html) - Alternative text for images
- [style](/reference/htmlattributes/style.html) - Inline styling for dimensions
- [CSS Styles](/reference/styles/) - Comprehensive styling including sizing
- [Data Binding](/reference/binding/) - Dynamic attribute values

---
