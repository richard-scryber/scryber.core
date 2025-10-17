---
layout: default
title: picture and source
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;picture&gt; and &lt;source&gt; : The Responsive Picture Elements

The `<picture>` element provides responsive image selection for PDF documents. It contains multiple `<source>` elements that specify different image sources based on media queries and image formats. Scryber automatically selects the most appropriate image source for PDF rendering, typically choosing the highest quality image suitable for print media.

## Usage

The `<picture>` element enables responsive images that:
- Contain multiple `<source>` elements with different image options
- Support media queries to select appropriate images for print/screen
- Allow multiple image sources with density descriptors (1x, 2x, 3x)
- Enable art direction with different images for different contexts
- Support format fallbacks for optimal image type selection
- Include a fallback `<img>` element for default display
- Automatically select the best quality image for PDF output
- Support dynamic source selection through data binding
- Choose high-density images for print quality PDFs
- Filter by MIME type to ensure format compatibility

```html
<!-- Basic picture with sources -->
<picture>
    <source srcset="image-high.jpg 2x, image-medium.jpg 1x" type="image/jpeg">
    <img src="image-default.jpg" alt="Responsive Image" width="300pt" />
</picture>

<!-- Picture with media queries -->
<picture>
    <source media="print" srcset="print-version.jpg">
    <source srcset="screen-version.jpg">
    <img src="fallback.jpg" alt="Context-aware Image" />
</picture>
```

---

## Supported Attributes

### Picture Element Attributes

The `<picture>` element itself is a container with standard HTML attributes:

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |

### Source Element Attributes

The `<source>` element specifies alternative image sources:

| Attribute | Type | Description |
|-----------|------|-------------|
| `srcset` | string | **Required**. Comma-separated list of image sources with optional density (1x, 2x) or width (100w) descriptors. |
| `src` | string | Single image source (alternative to srcset). |
| `media` | MediaMatcher | Media query string (e.g., "print", "screen", "(min-width: 600px)"). |
| `type` | MimeType | MIME type of the image (e.g., "image/jpeg", "image/png", "image/webp"). |

### Image Element Attributes

The nested `<img>` element provides the fallback and display:

| Attribute | Type | Description |
|-----------|------|-------------|
| `src` | string | **Required**. Default/fallback image source. |
| `width` | Unit | Image width. Supports units: pt, px, mm, cm, in, %, em. |
| `height` | Unit | Image height. Supports units: pt, px, mm, cm, in, %, em. |
| `alt` | string | Alternative text for the image (accessibility). |
| All standard attributes | | The img element supports all standard image attributes. |

### CSS Style Support

The `<picture>` element supports CSS styling:

**Display**:
- `display`: `inline` (default), `block`, `inline-block`, `none`

The nested `<img>` element supports full image styling. See the [img element documentation](/reference/htmltags/img.html) for complete CSS support.

---

## Notes

### How Picture Selection Works in PDF

Scryber's picture element selection process for PDF generation:

1. **Media Query Filtering**: Filter sources by media query, preferring "print" media or no media query
2. **Format Validation**: Check MIME type compatibility (JPEG, PNG, GIF, TIFF, SVG supported)
3. **Density Selection**: From remaining sources, select the **highest density** image (2x over 1x)
4. **Quality Priority**: PDF documents benefit from high-resolution images for print quality
5. **Fallback**: If no sources match, use the `<img>` element's `src` attribute

This differs from browsers which may select based on viewport size and network conditions. PDF generation prioritizes quality over file size.

### Picture vs Image Element

**picture**:
- Container for multiple image sources
- Enables responsive image selection
- Automatically chooses best quality for PDF
- Supports media queries and format fallbacks
- Best for: Multiple image versions, print/screen variations, high-DPI displays

**img**:
- Single image source
- Direct image display
- Simpler and more common
- Best for: Single images, straightforward display

Use `<picture>` when you have multiple image versions and want automatic selection. Use `<img>` for standard single-source images.

### Source Element

The `<source>` element specifies alternative image sources:

**srcset Syntax**:

1. **Density Descriptors** (recommended for PDF):
   ```html
   <source srcset="image-1x.jpg 1x, image-2x.jpg 2x, image-3x.jpg 3x">
   ```
   - `1x`: Standard density (72-96 DPI)
   - `2x`: Double density (144-192 DPI)
   - `3x`: Triple density (216-288 DPI)

2. **Width Descriptors**:
   ```html
   <source srcset="small.jpg 400w, medium.jpg 800w, large.jpg 1200w">
   ```
   - Specifies intrinsic image width
   - Scryber selects the largest/highest quality

3. **Single Source**:
   ```html
   <source srcset="image.jpg">
   ```
   - Single image without descriptor
   - Used as-is if media/type match

**Multiple Sources**:
```html
<picture>
    <source srcset="ultra-high.jpg 3x, high.jpg 2x, standard.jpg 1x">
    <img src="fallback.jpg" alt="Image" />
</picture>
```

### Media Queries for Print

Common media queries for PDF documents:

```html
<!-- Print-specific image -->
<source media="print" srcset="print-optimized.jpg">

<!-- Screen-specific image (less relevant for PDF) -->
<source media="screen" srcset="screen-optimized.jpg">

<!-- No media query (matches all) -->
<source srcset="default.jpg">
```

For PDF generation:
- `media="print"` sources are prioritized
- Sources without media queries are considered universal
- Screen-specific sources are typically ignored

### Supported Image Formats

Scryber validates MIME types and supports:

- `image/jpeg` - JPEG images (widely supported, good compression)
- `image/png` - PNG images (lossless, supports transparency)
- `image/gif` - GIF images (animations become static in PDF)
- `image/tiff` - TIFF images (high quality, large files)
- `image/svg` or `image/svg+xml` - SVG images (vector graphics)
- `image/bmp` or `image/x-png` - Additional bitmap formats

Unsupported formats (like WebP) are automatically skipped during source selection.

### Image Selection Algorithm

Scryber's selection process:

```
1. Iterate through <source> elements in order
2. For each source:
   a. Check if media query matches (prefer "print" or no media)
   b. Check if MIME type is supported
   c. Parse srcset and extract highest density/width image
3. Select the first matching source with highest quality
4. If no sources match, use <img> src attribute
5. Set the selected source on the inner <img> element
```

This happens during the **DataBind** phase, before rendering.

### Dynamic Source Selection

Use data binding for dynamic image sources:

```html
<!-- With model = { printImage: "print.jpg", screenImage: "screen.jpg" } -->
<picture>
    <source media="print" srcset="{{model.printImage}}">
    <source srcset="{{model.screenImage}}">
    <img src="{{model.fallbackImage}}" alt="Dynamic Image" width="200pt" />
</picture>
```

All source elements are databound, allowing fully dynamic picture configurations.

### Fallback Image

The `<img>` element inside `<picture>` serves two purposes:

1. **Selection Target**: The selected source is applied to this image's `src`
2. **Fallback**: Used if no sources match or loading fails
3. **Display Properties**: Provides width, height, alt, and styling

The image element is required and provides the actual rendering.

### Art Direction

Use different images for different contexts:

```html
<picture>
    <!-- High-resolution detailed image for print -->
    <source media="print" srcset="detailed-print.jpg 2x">

    <!-- Simplified image for other contexts -->
    <source srcset="simplified.jpg">

    <img src="default.jpg" alt="Art Directed Image" width="100%" />
</picture>
```

### Format Fallbacks

Provide format alternatives for compatibility:

```html
<picture>
    <!-- SVG for vector rendering -->
    <source type="image/svg+xml" srcset="diagram.svg">

    <!-- PNG fallback with transparency -->
    <source type="image/png" srcset="diagram.png">

    <!-- JPEG fallback -->
    <img src="diagram.jpg" alt="Diagram" width="400pt" />
</picture>
```

Scryber will select the first supported format.

### Component Lifecycle

The picture element has special lifecycle handling:

1. **Init**: Initializes sources collection
2. **Load**: Loads all source elements
3. **DataBind**:
   - Binds all sources
   - Calls `EnsureCorrectSourceSet()` to select best source
   - Sets selected source on inner image
4. **Render**: Renders the inner image with selected source

The source selection happens automatically during data binding.

### Class Hierarchy

In the Scryber codebase:

**HTMLPicture**:
- Extends `Panel` → `ContainerComponent` → `VisualComponent`
- Contains `HTMLPictureSourceList` (collection of sources)
- Contains `HTMLImage` (the display image)
- Default display mode: `inline`
- Decorated with `[PDFParsableComponent("picture")]`

**HTMLPictureSource**:
- Extends `Component`
- Contains `MediaMatcher` for media query evaluation
- Parses `srcset` attribute for density/width descriptors
- Validates MIME types
- Provides `GetBestImageSource()` method
- Decorated with `[PDFParsableComponent("source")]`

---

## Examples

### Basic Picture with Multiple Densities

```html
<picture>
    <source srcset="photo-3x.jpg 3x, photo-2x.jpg 2x, photo-1x.jpg 1x" type="image/jpeg">
    <img src="photo-default.jpg" alt="High Quality Photo" width="300pt" height="200pt" />
</picture>
```

### Print-Optimized Images

```html
<picture>
    <!-- High-resolution for print -->
    <source media="print" srcset="print-quality.jpg 2x">

    <!-- Standard for screen -->
    <source media="screen" srcset="screen-quality.jpg">

    <!-- Fallback -->
    <img src="default.jpg" alt="Optimized Image" width="400pt" />
</picture>
```

### Multiple Format Support

```html
<picture>
    <!-- Try SVG first (vector, scalable) -->
    <source type="image/svg+xml" srcset="logo.svg">

    <!-- PNG fallback (supports transparency) -->
    <source type="image/png" srcset="logo-2x.png 2x, logo-1x.png 1x">

    <!-- JPEG fallback -->
    <img src="logo.jpg" alt="Company Logo" width="150pt" />
</picture>
```

### Art Direction Example

```html
<picture>
    <!-- Detailed diagram for print -->
    <source media="print"
            srcset="diagram-detailed.png 2x"
            type="image/png">

    <!-- Simplified diagram for screen -->
    <source media="screen"
            srcset="diagram-simple.png"
            type="image/png">

    <!-- Default -->
    <img src="diagram-basic.png" alt="System Diagram" width="500pt" />
</picture>
```

### Responsive Image Widths

```html
<picture>
    <source srcset="large.jpg 1200w, medium.jpg 800w, small.jpg 400w" type="image/jpeg">
    <img src="medium.jpg" alt="Responsive Image" style="width: 100%; height: auto;" />
</picture>
```

### Dynamic Picture Sources

```html
<!-- With model = {
    highRes: "product-high.jpg",
    medRes: "product-med.jpg",
    lowRes: "product-low.jpg",
    altText: "Product Photo"
} -->

<picture>
    <source srcset="{{model.highRes}} 2x, {{model.medRes}} 1x" type="image/jpeg">
    <img src="{{model.lowRes}}" alt="{{model.altText}}" width="250pt" />
</picture>
```

### Picture in Document Layout

```html
<div style="text-align: center; margin: 20pt 0;">
    <picture>
        <source media="print" srcset="featured-print.jpg 2x">
        <source srcset="featured-screen.jpg">
        <img src="featured.jpg" alt="Featured Image"
             style="width: 80%; border: 1pt solid #ccc; padding: 10pt;" />
    </picture>
    <p style="font-size: 9pt; color: #666; margin-top: 5pt;">
        Figure 1: Featured Product
    </p>
</div>
```

### Gallery with Picture Elements

```html
<div style="text-align: center;">
    <picture>
        <source srcset="gallery1-2x.jpg 2x, gallery1-1x.jpg 1x">
        <img src="gallery1.jpg" alt="Gallery Image 1"
             style="width: 200pt; height: 200pt; margin: 5pt; border: 1pt solid #ddd;" />
    </picture>

    <picture>
        <source srcset="gallery2-2x.jpg 2x, gallery2-1x.jpg 1x">
        <img src="gallery2.jpg" alt="Gallery Image 2"
             style="width: 200pt; height: 200pt; margin: 5pt; border: 1pt solid #ddd;" />
    </picture>

    <picture>
        <source srcset="gallery3-2x.jpg 2x, gallery3-1x.jpg 1x">
        <img src="gallery3.jpg" alt="Gallery Image 3"
             style="width: 200pt; height: 200pt; margin: 5pt; border: 1pt solid #ddd;" />
    </picture>
</div>
```

### Complex Source Selection

```html
<picture>
    <!-- High-DPI print version -->
    <source media="print"
            srcset="print-ultra.jpg 3x, print-high.jpg 2x"
            type="image/jpeg">

    <!-- Vector version for print -->
    <source media="print"
            srcset="vector.svg"
            type="image/svg+xml">

    <!-- Screen PNG version -->
    <source media="screen"
            srcset="screen-2x.png 2x, screen-1x.png 1x"
            type="image/png">

    <!-- Universal fallback -->
    <img src="fallback.jpg" alt="Complex Selection Example" width="300pt" />
</picture>
```

### Banner with Picture

```html
<div style="width: 100%; background-color: #336699; padding: 20pt; text-align: center;">
    <picture>
        <source srcset="banner-wide-2x.jpg 2x, banner-wide-1x.jpg 1x"
                type="image/jpeg">
        <img src="banner.jpg" alt="Company Banner"
             style="max-width: 100%; height: auto;" />
    </picture>
</div>
```

### Product Images

```html
<div class="product-card" style="border: 1pt solid #ddd; padding: 15pt; margin: 10pt;">
    <picture>
        <source media="print"
                srcset="product-print-3x.jpg 3x, product-print-2x.jpg 2x"
                type="image/jpeg">
        <source srcset="product-2x.jpg 2x, product-1x.jpg 1x"
                type="image/jpeg">
        <img src="product.jpg" alt="{{model.productName}}"
             style="width: 100%; height: auto; border-radius: 4pt;" />
    </picture>

    <h3 style="margin: 10pt 0;">{{model.productName}}</h3>
    <p style="color: #336699; font-weight: bold;">${{model.price}}</p>
</div>
```

### Magazine Layout

```html
<div style="column-count: 2; column-gap: 20pt;">
    <picture>
        <source srcset="article-image-2x.jpg 2x" type="image/jpeg">
        <img src="article-image.jpg" alt="Article Illustration"
             style="width: 100%; margin-bottom: 10pt;" />
    </picture>

    <p>
        Article text flows in columns around the picture element. The image
        automatically selects the best quality version for PDF rendering.
    </p>
    <p>
        Additional paragraphs continue in the column layout, creating a
        magazine-style appearance.
    </p>
</div>
```

### Conditional Picture Loading

```html
<!-- With model = { showHighRes: true, imagePath: "photos/" } -->

<picture hidden="{{model.showHighRes ? '' : 'hidden'}}">
    <source srcset="{{model.imagePath}}high-res-2x.jpg 2x">
    <img src="{{model.imagePath}}standard.jpg" alt="Conditional Image" width="300pt" />
</picture>
```

### Repeating Pictures from Data

```html
<!-- With model.images = [
    {high: "img1-2x.jpg", low: "img1.jpg", alt: "Image 1"},
    {high: "img2-2x.jpg", low: "img2.jpg", alt: "Image 2"}
] -->

<div style="text-align: center;">
    <template data-bind="{{model.images}}">
        <picture style="margin: 5pt;">
            <source srcset="{{.high}} 2x">
            <img src="{{.low}}" alt="{{.alt}}"
                 style="width: 150pt; height: 150pt; border: 1pt solid #ccc;" />
        </picture>
    </template>
</div>
```

### Report Header with Logo

```html
<div style="border-bottom: 2pt solid #336699; padding: 15pt; margin-bottom: 20pt;">
    <picture>
        <source media="print" srcset="logo-print-2x.png 2x" type="image/png">
        <source srcset="logo-screen.png" type="image/png">
        <img src="logo.png" alt="Company Logo"
             style="width: 120pt; height: 40pt; float: left; margin-right: 15pt;" />
    </picture>

    <h1 style="margin: 0; line-height: 40pt; color: #336699;">
        Annual Report 2024
    </h1>
    <div style="clear: both;"></div>
</div>
```

### Infographic Section

```html
<div class="infographic" style="background-color: #f5f5f5; padding: 20pt; margin: 20pt 0;">
    <h2 style="text-align: center; color: #336699;">Sales Statistics</h2>

    <picture>
        <source media="print"
                srcset="infographic-print.svg"
                type="image/svg+xml">
        <source srcset="infographic-high.png 2x, infographic-standard.png 1x"
                type="image/png">
        <img src="infographic.jpg" alt="Sales Infographic"
             style="width: 100%; height: auto; display: block; margin: 15pt 0;" />
    </picture>

    <p style="text-align: center; font-size: 9pt; color: #666;">
        Data visualization for Q1-Q4 2024
    </p>
</div>
```

### Certificate with Seal

```html
<div style="border: 3pt double #336699; padding: 40pt; text-align: center;">
    <h1 style="color: #336699;">Certificate of Achievement</h1>
    <p style="font-size: 14pt; margin: 20pt 0;">Awarded to: {{model.recipientName}}</p>

    <picture>
        <source srcset="seal-ultra-high.png 3x, seal-high.png 2x, seal-standard.png 1x"
                type="image/png">
        <img src="seal.png" alt="Official Seal"
             style="width: 100pt; height: 100pt; margin: 20pt 0;" />
    </picture>

    <p style="margin-top: 20pt;">Date: {{model.date}}</p>
</div>
```

### Cover Page Design

```html
<!DOCTYPE html>
<html>
<head>
    <title>Document Cover</title>
    <style>
        .cover-page {
            min-height: 800pt;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            background-color: #336699;
            color: white;
            text-align: center;
            padding: 50pt;
        }
    </style>
</head>
<body>
    <div class="cover-page">
        <picture>
            <source media="print" srcset="cover-logo-3x.png 3x" type="image/png">
            <source srcset="cover-logo-2x.png 2x, cover-logo-1x.png 1x" type="image/png">
            <img src="cover-logo.png" alt="Company Logo"
                 style="width: 200pt; margin-bottom: 40pt;" />
        </picture>

        <h1 style="font-size: 36pt; margin: 20pt 0;">{{model.documentTitle}}</h1>
        <h2 style="font-size: 24pt; margin: 10pt 0; font-weight: normal;">
            {{model.documentSubtitle}}
        </h2>

        <p style="margin-top: 60pt; font-size: 14pt;">{{model.year}}</p>
    </div>
</body>
</html>
```

### Data Visualization Images

```html
<div class="charts-section">
    <h2>Financial Analysis</h2>

    <div style="margin: 20pt 0;">
        <h3>Revenue Trends</h3>
        <picture>
            <source media="print" srcset="chart-revenue-print.svg" type="image/svg+xml">
            <source srcset="chart-revenue-2x.png 2x" type="image/png">
            <img src="chart-revenue.png" alt="Revenue Chart"
                 style="width: 100%; height: auto; border: 1pt solid #ddd; padding: 10pt;" />
        </picture>
    </div>

    <div style="margin: 20pt 0;">
        <h3>Market Share</h3>
        <picture>
            <source media="print" srcset="chart-market-print.svg" type="image/svg+xml">
            <source srcset="chart-market-2x.png 2x" type="image/png">
            <img src="chart-market.png" alt="Market Share Chart"
                 style="width: 100%; height: auto; border: 1pt solid #ddd; padding: 10pt;" />
        </picture>
    </div>
</div>
```

### Floating Picture with Text Wrap

```html
<div class="article">
    <h1>Article Title</h1>

    <picture style="float: left; margin: 0 15pt 10pt 0;">
        <source srcset="thumbnail-2x.jpg 2x, thumbnail-1x.jpg 1x">
        <img src="thumbnail.jpg" alt="Article Thumbnail"
             style="width: 150pt; height: 150pt; border: 1pt solid #ccc;" />
    </picture>

    <p>
        This article text flows around the floating picture element on the left.
        The picture automatically selects the appropriate image resolution for
        optimal print quality in the PDF document.
    </p>
    <p>
        Additional paragraphs continue to wrap around the floating image,
        creating a professional magazine-style layout.
    </p>

    <div style="clear: both;"></div>
</div>
```

### Email Newsletter Template

```html
<div style="max-width: 600pt; margin: 0 auto; font-family: Arial, sans-serif;">
    <!-- Header with logo -->
    <div style="background-color: #336699; padding: 20pt; text-align: center;">
        <picture>
            <source srcset="newsletter-logo-2x.png 2x">
            <img src="newsletter-logo.png" alt="Newsletter Logo"
                 style="width: 200pt; height: 60pt;" />
        </picture>
    </div>

    <!-- Featured article -->
    <div style="padding: 20pt;">
        <picture>
            <source srcset="featured-article-2x.jpg 2x">
            <img src="featured-article.jpg" alt="Featured Article"
                 style="width: 100%; height: auto; margin-bottom: 15pt;" />
        </picture>

        <h2>{{model.featuredTitle}}</h2>
        <p>{{model.featuredExcerpt}}</p>
    </div>
</div>
```

---

## See Also

- [img](/reference/htmltags/img.html) - Image element (used within picture)
- [source](/reference/htmltags/source.html) - Source element specification
- [iframe](/reference/htmltags/iframe.html) - Iframe element for embedded content
- [object](/reference/htmltags/object.html) - Object element for attachments
- [CSS Media Queries](/reference/styles/media.html) - Media query support
- [Image Formats](/reference/images/formats.html) - Supported image formats
- [Data Binding](/reference/binding/) - Data binding and expressions
- [Responsive Images](/reference/images/responsive.html) - Responsive image strategies

---
