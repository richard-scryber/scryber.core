---
layout: default
title: src
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @src : The Source Attribute

The `src` attribute specifies the source location of embedded content such as images and iframes. It accepts file paths (relative or absolute), URLs, and data URIs, enabling flexible content embedding in PDF documents with support for various image formats and external HTML content.

## Usage

The `src` attribute specifies content sources:
- Image file paths (local or network)
- External URLs for images and content
- Data URIs for embedded base64 content
- Supports multiple image formats (PNG, JPG, GIF, BMP, etc.)
- Used with `<img>` and `<iframe>` elements
- Supports data binding for dynamic content sources
- Required attribute for functional image and iframe elements

```html
<!-- Local image file -->
<img src="logo.png" width="100pt" height="50pt" />

<!-- URL image -->
<img src="https://example.com/image.jpg" width="200pt" height="150pt" />

<!-- Data URI -->
<img src="data:image/png;base64,iVBORw0KGg..." width="50pt" height="50pt" />

<!-- Dynamic source -->
<img src="{{model.imagePath}}" width="150pt" height="100pt" />

<!-- Iframe content -->
<iframe src="external-content.html" width="500pt" height="300pt"></iframe>
```

---

## Supported Elements

The `src` attribute is used with:

### Image Element
- `<img>` - Displays images in various formats (primary use)

### Iframe Element
- `<iframe>` - Embeds external HTML content

---

## Binding Values

The `src` attribute supports data binding for dynamic content sources:

```html
<!-- Simple dynamic path -->
<img src="{{model.logoPath}}" width="100pt" height="50pt" />

<!-- Constructed path -->
<img src="images/{{model.category}}/{{model.filename}}"
     width="200pt" height="150pt" />

<!-- Conditional source -->
<img src="{{model.useHighRes ? model.highResImage : model.lowResImage}}"
     width="300pt" height="200pt" />

<!-- URL with parameters -->
<img src="https://api.example.com/image?id={{model.imageId}}&size=large"
     width="400pt" height="300pt" />

<!-- Dynamic iframe content -->
<iframe src="{{model.contentUrl}}" width="500pt" height="400pt"></iframe>

<!-- Repeating images -->
<template data-bind="{{model.gallery}}">
    <img src="{{.imageUrl}}"
         width="150pt" height="100pt"
         alt="{{.description}}"
         style="margin: 5pt;" />
</template>
```

**Data Model Example:**
```json
{
  "logoPath": "assets/company-logo.png",
  "category": "products",
  "filename": "widget-a.jpg",
  "useHighRes": true,
  "highResImage": "images/hero-4k.jpg",
  "lowResImage": "images/hero-hd.jpg",
  "imageId": "12345",
  "contentUrl": "reports/summary.html",
  "gallery": [
    {
      "imageUrl": "gallery/photo1.jpg",
      "description": "First photo"
    },
    {
      "imageUrl": "gallery/photo2.jpg",
      "description": "Second photo"
    }
  ]
}
```

---

## Notes

### Supported Image Formats

Scryber supports various image formats:

| Format | Extension | Notes |
|--------|-----------|-------|
| PNG | .png | Supports transparency, recommended for logos |
| JPEG | .jpg, .jpeg | Good for photographs, smaller file sizes |
| GIF | .gif | Supports transparency and animation |
| BMP | .bmp | Uncompressed bitmap format |
| TIFF | .tif, .tiff | High-quality image format |

```html
<img src="logo.png" width="100pt" height="50pt" />
<img src="photo.jpg" width="300pt" height="200pt" />
<img src="icon.gif" width="32pt" height="32pt" />
```

### File Path Types

The `src` attribute accepts different path formats:

#### 1. Relative Paths
Relative to the document or application location:

```html
<img src="logo.png" />                    <!-- Same directory -->
<img src="images/logo.png" />             <!-- Subdirectory -->
<img src="../images/logo.png" />          <!-- Parent directory -->
<img src="./assets/images/logo.png" />    <!-- Current directory explicit -->
```

#### 2. Absolute Paths
Full file system paths:

```html
<!-- Windows -->
<img src="C:\Projects\Images\logo.png" />

<!-- Unix/Mac -->
<img src="/Users/username/images/logo.png" />
```

#### 3. URLs
HTTP/HTTPS URLs to remote images:

```html
<img src="https://example.com/images/logo.png" />
<img src="http://cdn.example.com/photo.jpg" />
```

#### 4. Data URIs
Base64-encoded image data:

```html
<img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUA..." />
```

### Image Sizing

While `src` specifies the source, use `width` and `height` attributes to control dimensions:

```html
<!-- Explicit dimensions -->
<img src="photo.jpg" width="300pt" height="200pt" />

<!-- Width only (maintains aspect ratio) -->
<img src="photo.jpg" width="300pt" />

<!-- Using CSS styles -->
<img src="photo.jpg" style="width: 300pt; height: 200pt;" />

<!-- Percentage sizing -->
<img src="photo.jpg" style="width: 100%; height: auto;" />
```

### Missing or Invalid Sources

If the `src` path is invalid or the file doesn't exist:
- The image may not render
- An error may be logged
- A placeholder or broken image indicator may appear

Always ensure:
- File paths are correct
- Files exist at specified locations
- URLs are accessible
- Proper file permissions are set

```html
<!-- Best practice: verify paths -->
<img src="verified-path/logo.png" width="100pt" height="50pt"
     alt="Company Logo" />
```

### Image Resolution and Quality

For PDF output, consider:

- **High DPI**: Use high-resolution images for print quality
- **File Size**: Balance quality with document size
- **Format**: PNG for graphics/logos, JPEG for photos
- **Dimensions**: Specify appropriate display sizes

```html
<!-- High-quality logo for print -->
<img src="logo-300dpi.png" width="100pt" height="50pt" />

<!-- Optimized photo -->
<img src="photo-optimized.jpg" width="400pt" height="300pt" />
```

### Data URIs

Data URIs embed image data directly in the HTML:

**Advantages:**
- No external file dependencies
- Single-file distribution
- No network requests

**Disadvantages:**
- Larger HTML file size
- Harder to maintain
- Can't cache separately

```html
<!-- Small icon as data URI -->
<img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUA
AAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO
9TXL0Y4OHwAAAABJRU5ErkJggg==" width="20pt" height="20pt" />
```

### Iframe Source

For `<iframe>` elements, `src` specifies HTML content:

```html
<!-- External HTML file -->
<iframe src="content.html" width="500pt" height="400pt"></iframe>

<!-- External URL -->
<iframe src="https://example.com/widget" width="600pt" height="400pt"></iframe>

<!-- Relative path -->
<iframe src="../shared/header.html" width="100%" height="100pt"></iframe>
```

### Cross-Origin Considerations

When using external URLs:
- Ensure the resource is accessible
- Consider CORS (Cross-Origin Resource Sharing) policies
- External resources may affect PDF generation time
- Network failures can impact document creation

```html
<!-- External image - ensure accessibility -->
<img src="https://cdn.example.com/image.jpg" width="200pt" height="150pt" />
```

### Security Considerations

Be cautious with user-provided `src` values:
- Validate and sanitize input paths
- Avoid exposing system file paths
- Be careful with data URIs from untrusted sources
- Consider file access permissions

```html
<!-- Sanitize user input -->
<!-- Model: sanitizedImagePath from validated user input -->
<img src="{{model.sanitizedImagePath}}" width="200pt" height="150pt" />
```

### Image Caching

Scryber may cache images during document generation:
- Improves performance for repeated images
- Reduces memory usage
- Same source = reused image data

```html
<!-- This logo will be cached and reused -->
<img src="logo.png" width="100pt" height="50pt" />
<!-- ... more content ... -->
<img src="logo.png" width="50pt" height="25pt" />  <!-- Reuses cached data -->
```

---

## Examples

### Basic Image Usage

```html
<!-- Simple image -->
<img src="logo.png" width="150pt" height="75pt" />

<!-- Image with alt text -->
<img src="product.jpg" width="300pt" height="200pt" alt="Product photo" />

<!-- Styled image -->
<img src="banner.jpg"
     width="100%"
     style="border: 2pt solid #336699; border-radius: 5pt;" />
```

### Images from Different Locations

```html
<!-- Same directory -->
<img src="local-image.png" width="200pt" height="150pt" />

<!-- Subdirectory -->
<img src="images/photos/vacation.jpg" width="400pt" height="300pt" />

<!-- Parent directory -->
<img src="../shared/logo.png" width="100pt" height="50pt" />

<!-- Absolute path -->
<img src="/assets/images/header.jpg" width="600pt" height="200pt" />

<!-- External URL -->
<img src="https://example.com/cdn/image.jpg" width="300pt" height="200pt" />
```

### Dynamic Image Sources

```html
<!-- Model: { user: { avatar: "avatars/john.png", name: "John" } } -->

<div style="text-align: center; padding: 20pt;">
    <img src="{{model.user.avatar}}"
         width="100pt" height="100pt"
         alt="{{model.user.name}}"
         style="border-radius: 50pt; border: 2pt solid #336699;" />
    <p>{{model.user.name}}</p>
</div>
```

### Image Gallery

```html
<!-- Model: { images: [
    { url: "gallery/img1.jpg", caption: "Image 1" },
    { url: "gallery/img2.jpg", caption: "Image 2" }
] } -->

<div>
    <h2>Photo Gallery</h2>

    <template data-bind="{{model.images}}">
        <div style="display: inline-block; margin: 10pt;
                    border: 1pt solid #ccc; padding: 10pt;">
            <img src="{{.url}}" width="200pt" height="150pt"
                 alt="{{.caption}}" />
            <p style="text-align: center; margin: 5pt 0 0 0;">{{.caption}}</p>
        </div>
    </template>
</div>
```

### Conditional Images

```html
<!-- Model: { theme: "dark", darkLogo: "logo-dark.png", lightLogo: "logo-light.png" } -->

<img src="{{model.theme == 'dark' ? model.darkLogo : model.lightLogo}}"
     width="150pt" height="75pt"
     alt="Company Logo" />
```

### Product Catalog with Images

```html
<!-- Model: { products: [
    { name: "Widget A", image: "products/widget-a.jpg", price: "$29.99" },
    { name: "Widget B", image: "products/widget-b.jpg", price: "$39.99" }
] } -->

<div>
    <h1>Product Catalog</h1>

    <template data-bind="{{model.products}}">
        <div style="border: 1pt solid #ddd; padding: 15pt; margin-bottom: 20pt;
                    display: flex; align-items: center;">
            <img src="{{.image}}" width="150pt" height="150pt"
                 alt="{{.name}}"
                 style="margin-right: 20pt; border: 1pt solid #eee;" />
            <div>
                <h2 style="margin: 0 0 10pt 0;">{{.name}}</h2>
                <p style="font-size: 18pt; color: #336699; font-weight: bold;">
                    {{.price}}
                </p>
            </div>
        </div>
    </template>
</div>
```

### Logo Variations

```html
<style>
    .logo-small { width: 50pt; height: 25pt; }
    .logo-medium { width: 100pt; height: 50pt; }
    .logo-large { width: 200pt; height: 100pt; }
</style>

<!-- Small logo in header -->
<header style="padding: 10pt; border-bottom: 1pt solid #ccc;">
    <img src="logo.png" class="logo-small" alt="Company" />
</header>

<!-- Medium logo in body -->
<div style="text-align: center; padding: 30pt;">
    <img src="logo.png" class="logo-medium" alt="Company" />
</div>

<!-- Large logo on cover page -->
<div style="text-align: center; padding-top: 200pt;">
    <img src="logo.png" class="logo-large" alt="Company" />
</div>
```

### Icons and Inline Images

```html
<p>
    <img src="icons/info.png" width="16pt" height="16pt"
         style="vertical-align: middle;" />
    This is an informational message with an icon.
</p>

<p>
    <img src="icons/warning.png" width="16pt" height="16pt"
         style="vertical-align: middle;" />
    Warning: Please read carefully.
</p>

<p>
    <img src="icons/check.png" width="16pt" height="16pt"
         style="vertical-align: middle;" />
    Task completed successfully.
</p>
```

### Background-Style Images

```html
<div style="position: relative; height: 300pt; overflow: hidden;">
    <img src="background.jpg"
         style="position: absolute; top: 0; left: 0; width: 100%; height: 100%;
                z-index: -1; opacity: 0.3;" />
    <div style="position: relative; z-index: 1; padding: 40pt;">
        <h1>Content Over Background Image</h1>
        <p>This text appears over a semi-transparent background image.</p>
    </div>
</div>
```

### Charts and Diagrams

```html
<div>
    <h2>Sales Performance</h2>

    <div style="text-align: center; margin: 20pt 0;">
        <img src="charts/sales-2024.png"
             width="500pt" height="300pt"
             alt="Sales chart for 2024" />
        <p style="font-size: 10pt; color: #666; margin-top: 10pt;">
            Figure 1: Annual sales performance
        </p>
    </div>

    <h2>Regional Distribution</h2>

    <div style="text-align: center; margin: 20pt 0;">
        <img src="charts/regional-map.png"
             width="600pt" height="400pt"
             alt="Regional distribution map" />
        <p style="font-size: 10pt; color: #666; margin-top: 10pt;">
            Figure 2: Sales by region
        </p>
    </div>
</div>
```

### Profile Cards with Avatars

```html
<!-- Model: { team: [
    { name: "Alice", role: "Manager", avatar: "avatars/alice.jpg" },
    { name: "Bob", role: "Developer", avatar: "avatars/bob.jpg" }
] } -->

<div>
    <h2>Team Members</h2>

    <template data-bind="{{model.team}}">
        <div style="border: 1pt solid #ddd; border-radius: 8pt;
                    padding: 20pt; margin-bottom: 15pt;
                    display: flex; align-items: center;">
            <img src="{{.avatar}}" width="80pt" height="80pt"
                 alt="{{.name}}"
                 style="border-radius: 40pt; margin-right: 20pt;" />
            <div>
                <h3 style="margin: 0 0 5pt 0;">{{.name}}</h3>
                <p style="margin: 0; color: #666;">{{.role}}</p>
            </div>
        </div>
    </template>
</div>
```

### Image with Caption

```html
<figure style="margin: 20pt 0; text-align: center;">
    <img src="architecture-diagram.png"
         width="600pt" height="400pt"
         alt="System architecture diagram"
         style="border: 1pt solid #ddd;" />
    <figcaption style="margin-top: 10pt; font-style: italic; color: #666;">
        Figure 3.1: System Architecture Overview
    </figcaption>
</figure>
```

### Logos in Headers and Footers

```html
<header style="position: fixed; top: 0; width: 100%; padding: 15pt;
               border-bottom: 2pt solid #336699; background-color: white;">
    <img src="logo.png" width="120pt" height="40pt" alt="Company Logo"
         style="float: left;" />
    <h1 style="margin: 0; padding: 5pt 0 0 140pt; font-size: 18pt;">
        Document Title
    </h1>
</header>

<footer style="position: fixed; bottom: 0; width: 100%; padding: 10pt;
               border-top: 1pt solid #ccc; background-color: white;">
    <img src="logo-small.png" width="40pt" height="20pt" alt="Company"
         style="float: left; margin-right: 10pt;" />
    <span style="font-size: 9pt; color: #666;">
        © 2025 Company Name. All rights reserved.
    </span>
</footer>
```

### Responsive Images

```html
<!-- Full-width responsive -->
<img src="banner.jpg"
     style="width: 100%; height: auto; max-width: 800pt;" />

<!-- Constrained width -->
<img src="photo.jpg"
     style="width: 100%; max-width: 400pt; height: auto;" />

<!-- Centered with max width -->
<div style="text-align: center;">
    <img src="featured.jpg"
         style="max-width: 600pt; width: 100%; height: auto;" />
</div>
```

### Data URI Example

```html
<!-- Small icon embedded as data URI -->
<img src="data:image/png;base64,
iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlz
AAALEwAACxMBAJqcGAAAAQ5JREFUOI2lkz1KA0EUx//zZndnN1E3RhQLC0EQBBvxAILgDTxAClsr
C3sBL5DSSrxAWlMIHkBQsLCwshFRJLK7OzOvyGaTaKFP+DDzmPn95n1A..."
     width="16pt" height="16pt" alt="Icon" />

<!-- Useful for small, frequently used icons -->
```

### Image Grid Layout

```html
<!-- Model: { gallery: Array of 6 images } -->

<div style="display: grid; grid-template-columns: repeat(3, 1fr); gap: 10pt;">
    <template data-bind="{{model.gallery}}">
        <img src="{{.url}}" width="180pt" height="120pt"
             alt="{{.title}}"
             style="border: 1pt solid #ddd;" />
    </template>
</div>
```

### Before/After Images

```html
<div style="display: flex; justify-content: space-around; margin: 20pt 0;">
    <div style="text-align: center;">
        <img src="before.jpg" width="300pt" height="200pt"
             alt="Before" style="border: 2pt solid #ccc;" />
        <p style="font-weight: bold; margin-top: 10pt;">Before</p>
    </div>

    <div style="text-align: center;">
        <img src="after.jpg" width="300pt" height="200pt"
             alt="After" style="border: 2pt solid #28a745;" />
        <p style="font-weight: bold; margin-top: 10pt;">After</p>
    </div>
</div>
```

### Iframe Usage

```html
<!-- Embed external HTML content -->
<iframe src="external-report.html"
        width="600pt" height="400pt"
        style="border: 1pt solid #ccc;"></iframe>

<!-- Embed section from another document -->
<h2>Quarterly Summary</h2>
<iframe src="reports/q4-summary.html"
        width="100%" height="300pt"></iframe>

<!-- Dynamic iframe source -->
<!-- Model: { reportUrl: "reports/current.html" } -->
<iframe src="{{model.reportUrl}}"
        width="600pt" height="500pt"></iframe>
```

### Image with Link

```html
<!-- Clickable image -->
<a href="https://example.com">
    <img src="promotional-banner.jpg"
         width="600pt" height="200pt"
         alt="Special Promotion"
         style="border: none;" />
</a>

<!-- Logo linking to website -->
<a href="https://company.com">
    <img src="logo.png" width="150pt" height="50pt" alt="Company Name" />
</a>
```

### Image Thumbnails

```html
<!-- Model: { photos: Array of photo objects } -->

<div>
    <h2>Photo Thumbnails</h2>

    <div style="display: flex; flex-wrap: wrap; gap: 10pt;">
        <template data-bind="{{model.photos}}">
            <a href="#photo-{{.id}}">
                <img src="{{.thumbnailUrl}}"
                     width="100pt" height="75pt"
                     alt="{{.title}}"
                     style="border: 2pt solid #ddd; border-radius: 4pt;" />
            </a>
        </template>
    </div>

    <!-- Full-size photos on separate pages -->
    <template data-bind="{{model.photos}}">
        <div id="photo-{{.id}}" style="page-break-before: always;
                                       text-align: center; padding: 50pt;">
            <img src="{{.fullSizeUrl}}"
                 style="max-width: 100%; height: auto;"
                 alt="{{.title}}" />
            <p style="margin-top: 20pt;">{{.title}}</p>
        </div>
    </template>
</div>
```

---

## See Also

- [img](/reference/htmltags/img.html) - Image element (uses src attribute)
- [iframe](/reference/htmltags/iframe.html) - Iframe element for embedded content
- [width](/reference/htmlattributes/width.html) - Width attribute for sizing
- [height](/reference/htmlattributes/height.html) - Height attribute for sizing
- [alt](/reference/htmlattributes/alt.html) - Alternative text for images
- [Data Binding](/reference/binding/) - Dynamic content and attribute values
- [Image Formats](/reference/images/) - Supported image formats and optimization

---
