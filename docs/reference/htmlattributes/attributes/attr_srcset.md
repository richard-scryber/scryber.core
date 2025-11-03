---
layout: default
title: srcset
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @srcset : The Source Set Attribute

The `srcset` attribute specifies multiple image sources for responsive images, allowing different versions to be selected based on device capabilities, screen resolution, or viewport dimensions. Used with `<img>` and `<source>` elements, it enables resolution-adaptive and art-directed images. In PDF generation, it can optimize image quality for high-resolution printing.

## Usage

The `srcset` attribute defines image source alternatives:
- Provides multiple image versions at different resolutions
- Enables selection based on pixel density (1x, 2x, 3x)
- Supports width descriptors for responsive layouts
- Used with `<img>` and `<source>` elements
- Works in conjunction with `sizes` attribute
- Supports data binding for dynamic image sets

```html
<!-- Resolution-based srcset (pixel density) -->
<img src="image.jpg"
     srcset="image-1x.jpg 1x,
             image-2x.jpg 2x,
             image-3x.jpg 3x"
     alt="Responsive image" />

<!-- Width-based srcset -->
<img src="image.jpg"
     srcset="image-400.jpg 400w,
             image-800.jpg 800w,
             image-1200.jpg 1200w"
     sizes="(max-width: 600px) 400px, 800px"
     alt="Responsive image" />

<!-- With picture element -->
<picture>
    <source srcset="image-large.jpg" media="(min-width: 800px)" />
    <source srcset="image-medium.jpg" media="(min-width: 400px)" />
    <img src="image-small.jpg" alt="Fallback image" />
</picture>

<!-- Dynamic srcset -->
<img src="{{model.defaultImage}}"
     srcset="{{model.imageSources}}"
     alt="{{model.altText}}" />
```

---

## Supported Elements

The `srcset` attribute is used with:

### Image Element
- `<img>` - Image element with responsive sources (primary use)

### Source Element
- `<source>` - Media source for `<picture>` element

---

## Binding Values

The `srcset` attribute supports data binding for dynamic image sources:

```html
<!-- Dynamic srcset from model -->
<img src="{{model.defaultSrc}}"
     srcset="{{model.responsiveSources}}"
     alt="{{model.description}}" />

<!-- Constructed srcset -->
<img src="products/{{model.productId}}.jpg"
     srcset="products/{{model.productId}}-1x.jpg 1x,
             products/{{model.productId}}-2x.jpg 2x"
     alt="{{model.productName}}" />

<!-- Conditional srcset -->
<img src="{{model.image.default}}"
     srcset="{{model.image.highRes ? model.image.srcset : ''}}"
     alt="{{model.image.alt}}" />

<!-- Repeating images with srcset -->
<template data-bind="{{model.gallery}}">
    <img src="{{.src}}"
         srcset="{{.srcset}}"
         sizes="{{.sizes}}"
         alt="{{.alt}}" />
</template>

<!-- Picture with dynamic sources -->
<picture>
    <template data-bind="{{model.sources}}">
        <source srcset="{{.srcset}}" media="{{.media}}" />
    </template>
    <img src="{{model.fallback}}" alt="{{model.alt}}" />
</picture>
```

**Data Model Example:**
```json
{
  "defaultSrc": "image.jpg",
  "responsiveSources": "image-400.jpg 400w, image-800.jpg 800w, image-1200.jpg 1200w",
  "description": "Product image",
  "productId": "widget-123",
  "productName": "Widget A",
  "image": {
    "default": "product.jpg",
    "highRes": true,
    "srcset": "product-1x.jpg 1x, product-2x.jpg 2x, product-3x.jpg 3x",
    "alt": "Product photo"
  },
  "gallery": [
    {
      "src": "gallery-1.jpg",
      "srcset": "gallery-1-400.jpg 400w, gallery-1-800.jpg 800w",
      "sizes": "(max-width: 600px) 400px, 800px",
      "alt": "Gallery image 1"
    }
  ],
  "sources": [
    {
      "srcset": "image-large.jpg",
      "media": "(min-width: 800px)"
    },
    {
      "srcset": "image-small.jpg",
      "media": "(max-width: 799px)"
    }
  ],
  "fallback": "image-default.jpg",
  "alt": "Responsive image"
}
```

---

## Notes

### Srcset Syntax

Two types of descriptors in srcset:

#### Resolution Descriptors (x)

Pixel density multipliers for high-DPI displays:

```html
<!-- 1x = standard resolution, 2x = double density (Retina), 3x = triple -->
<img src="logo.jpg"
     srcset="logo-1x.jpg 1x,
             logo-2x.jpg 2x,
             logo-3x.jpg 3x"
     alt="Logo" />

<!-- Browser selects based on device pixel ratio -->
<!-- Standard display: uses 1x -->
<!-- Retina/HiDPI: uses 2x or 3x -->
```

#### Width Descriptors (w)

Actual image widths in pixels:

```html
<!-- w = width in pixels of the image file -->
<img src="photo.jpg"
     srcset="photo-400.jpg 400w,
             photo-800.jpg 800w,
             photo-1200.jpg 1200w,
             photo-1600.jpg 1600w"
     sizes="(max-width: 600px) 400px,
            (max-width: 1000px) 800px,
            1200px"
     alt="Photo" />

<!-- Browser calculates which image to use based on:
     - viewport width
     - sizes attribute
     - device pixel ratio -->
```

### Resolution-Based Selection

Use x descriptors for fixed-size images:

```html
<!-- Logo at different resolutions -->
<img src="logo.png"
     srcset="logo.png 1x,
             logo@2x.png 2x,
             logo@3x.png 3x"
     width="200"
     height="100"
     alt="Company Logo" />

<!-- Icon at different resolutions -->
<img src="icon-24.png"
     srcset="icon-24.png 1x,
             icon-48.png 2x,
             icon-72.png 3x"
     width="24"
     height="24"
     alt="Icon" />
```

**Use cases:**
- Logos and icons with fixed dimensions
- UI elements that don't resize
- Images with specific display sizes

### Width-Based Selection

Use w descriptors for flexible layouts:

```html
<!-- Responsive hero image -->
<img src="hero.jpg"
     srcset="hero-400.jpg 400w,
             hero-800.jpg 800w,
             hero-1200.jpg 1200w,
             hero-1600.jpg 1600w,
             hero-2000.jpg 2000w"
     sizes="100vw"
     alt="Hero image" />

<!-- Article image with sizes -->
<img src="article.jpg"
     srcset="article-320.jpg 320w,
             article-640.jpg 640w,
             article-960.jpg 960w,
             article-1280.jpg 1280w"
     sizes="(max-width: 600px) 100vw,
            (max-width: 1000px) 80vw,
            960px"
     alt="Article illustration" />
```

**Use cases:**
- Full-width or flexible-width images
- Images in responsive layouts
- Content images that resize with viewport

### Sizes Attribute

Required with width descriptors to inform selection:

```html
<!-- Single size -->
<img srcset="image-400.jpg 400w, image-800.jpg 800w"
     sizes="400px"
     src="image-400.jpg"
     alt="Image" />

<!-- Responsive sizes with media queries -->
<img srcset="image-320.jpg 320w,
             image-640.jpg 640w,
             image-960.jpg 960w,
             image-1280.jpg 1280w"
     sizes="(max-width: 320px) 280px,
            (max-width: 640px) 580px,
            (max-width: 960px) 860px,
            1200px"
     src="image-640.jpg"
     alt="Image" />

<!-- Viewport-relative sizes -->
<img srcset="image-400.jpg 400w, image-800.jpg 800w"
     sizes="(max-width: 600px) 100vw, 50vw"
     src="image-400.jpg"
     alt="Image" />
```

### Picture Element Integration

Use `<source>` with srcset for art direction:

```html
<picture>
    <!-- Desktop: wide landscape image -->
    <source media="(min-width: 1000px)"
            srcset="landscape-large.jpg 1x,
                    landscape-large@2x.jpg 2x" />

    <!-- Tablet: medium landscape -->
    <source media="(min-width: 600px)"
            srcset="landscape-medium.jpg 1x,
                    landscape-medium@2x.jpg 2x" />

    <!-- Mobile: portrait crop -->
    <source media="(max-width: 599px)"
            srcset="portrait-small.jpg 1x,
                    portrait-small@2x.jpg 2x" />

    <!-- Fallback -->
    <img src="landscape-medium.jpg" alt="Responsive image" />
</picture>
```

### Format-Based Selection

Combine with type attribute for modern formats:

```html
<picture>
    <!-- WebP format (if supported) -->
    <source type="image/webp"
            srcset="image.webp 1x, image@2x.webp 2x" />

    <!-- AVIF format (if supported) -->
    <source type="image/avif"
            srcset="image.avif 1x, image@2x.avif 2x" />

    <!-- Fallback to JPEG -->
    <img src="image.jpg"
         srcset="image.jpg 1x, image@2x.jpg 2x"
         alt="Image" />
</picture>
```

### PDF Context Considerations

For PDF generation with Scryber:

1. **High-resolution printing** - Use 2x or 3x for print quality
2. **File size** - Balance quality vs PDF size
3. **Resolution selection** - PDF renderer may select high-res by default
4. **Fallback important** - Always provide `src` attribute

```html
<!-- Optimized for print (300 DPI) -->
<img src="chart.png"
     srcset="chart-72dpi.png 1x,
             chart-300dpi.png 4x"
     width="400"
     height="300"
     alt="Sales chart" />

<!-- For PDF, higher resolution version may be selected -->
```

### Fallback src Attribute

Always include `src` for compatibility:

```html
<!-- CORRECT: Has both srcset and src -->
<img src="image.jpg"
     srcset="image-1x.jpg 1x, image-2x.jpg 2x"
     alt="Image" />

<!-- INCORRECT: Missing src -->
<img srcset="image-1x.jpg 1x, image-2x.jpg 2x"
     alt="Image" />

<!-- The src is used as:
     1. Fallback for browsers that don't support srcset
     2. Default image before srcset is evaluated
     3. Required attribute per HTML specification -->
```

### Browser Selection Algorithm

Browser selects image based on:
1. **Device pixel ratio** - Screen density (1x, 2x, 3x)
2. **Viewport width** - Current browser width
3. **Sizes attribute** - Intended display size
4. **Network conditions** - May affect selection

```html
<!-- Example: How browser chooses -->
<img srcset="image-400.jpg 400w,
             image-800.jpg 800w,
             image-1200.jpg 1200w"
     sizes="(max-width: 600px) 400px, 800px"
     src="image-400.jpg"
     alt="Example" />

<!-- If viewport is 500px and device is 2x:
     - sizes says 400px display size (matches first media query)
     - 400px × 2 (device pixel ratio) = 800px needed
     - Browser selects image-800.jpg (800w) -->
```

### Image Optimization Tips

Best practices for srcset:

1. **Provide 3-5 sizes** - Cover common breakpoints
2. **Consistent aspect ratio** - All variants should match
3. **Optimize file size** - Compress appropriately
4. **Name systematically** - Use clear naming convention
5. **Test selections** - Verify correct image loads

```html
<!-- Good example with multiple sizes -->
<img src="product.jpg"
     srcset="product-small.jpg 320w,
             product-medium.jpg 640w,
             product-large.jpg 960w,
             product-xlarge.jpg 1280w,
             product-xxlarge.jpg 1920w"
     sizes="(max-width: 320px) 280px,
            (max-width: 640px) 600px,
            (max-width: 960px) 900px,
            1200px"
     alt="Product image" />
```

### Common Mistakes

Avoid these errors:

```html
<!-- WRONG: Mixing x and w descriptors -->
<img srcset="image-1x.jpg 1x, image-800.jpg 800w"
     src="image.jpg" alt="Wrong" />

<!-- WRONG: Using w without sizes -->
<img srcset="image-400.jpg 400w, image-800.jpg 800w"
     src="image.jpg" alt="Missing sizes" />

<!-- WRONG: Inconsistent aspect ratios -->
<img srcset="landscape.jpg 800w, portrait.jpg 400w"
     sizes="800px" src="landscape.jpg" alt="Inconsistent" />

<!-- CORRECT: Use picture for different aspect ratios -->
<picture>
    <source media="(min-width: 800px)" srcset="landscape.jpg" />
    <img src="portrait.jpg" alt="Correct" />
</picture>
```

---

## Examples

### Basic Resolution Switching

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Responsive Logo</title>
</head>
<body>
    <header>
        <!-- Logo for different screen densities -->
        <img src="logo.png"
             srcset="logo-1x.png 1x,
                     logo-2x.png 2x,
                     logo-3x.png 3x"
             width="200"
             height="50"
             alt="Company Logo" />

        <h1>Welcome to Our Site</h1>
    </header>
</body>
</html>
```

### Product Image with Multiple Resolutions

```html
<article>
    <h1>Product Details</h1>

    <!-- High-resolution product image -->
    <img src="product.jpg"
         srcset="product-1x.jpg 1x,
                 product-2x.jpg 2x,
                 product-3x.jpg 3x"
         width="400"
         height="400"
         alt="Widget A Product Photo"
         style="border: 1pt solid #ddd; padding: 10pt;" />

    <p>Premium quality widget with advanced features.</p>
</article>
```

### Responsive Hero Image

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Hero Image Example</title>
    <style>
        .hero-image {
            width: 100%;
            height: auto;
            display: block;
        }
    </style>
</head>
<body>
    <!-- Full-width responsive hero image -->
    <img src="hero-medium.jpg"
         srcset="hero-small.jpg 400w,
                 hero-medium.jpg 800w,
                 hero-large.jpg 1200w,
                 hero-xlarge.jpg 1600w,
                 hero-xxlarge.jpg 2000w"
         sizes="100vw"
         class="hero-image"
         alt="Mountain landscape at sunset" />

    <h1>Adventure Awaits</h1>
    <p>Discover breathtaking destinations around the world.</p>
</body>
</html>
```

### Article with Responsive Images

```html
<article style="max-width: 800pt; margin: 0 auto;">
    <h1>The Art of Photography</h1>

    <p>Photography is more than just capturing moments...</p>

    <!-- Article image with responsive sizes -->
    <figure>
        <img src="photo-640.jpg"
             srcset="photo-320.jpg 320w,
                     photo-640.jpg 640w,
                     photo-960.jpg 960w,
                     photo-1280.jpg 1280w"
             sizes="(max-width: 600px) 100vw,
                    (max-width: 1000px) 80vw,
                    800px"
             alt="Photographer capturing sunset"
             style="width: 100%; height: auto;" />
        <figcaption>A photographer at work during golden hour</figcaption>
    </figure>

    <p>Understanding light is fundamental to great photography...</p>
</article>
```

### Art Direction with Picture

```html
<article>
    <h1>Responsive Design Showcase</h1>

    <!-- Different images for different screen sizes -->
    <picture>
        <!-- Desktop: wide landscape shot -->
        <source media="(min-width: 1200px)"
                srcset="desktop-wide.jpg 1x,
                        desktop-wide@2x.jpg 2x" />

        <!-- Tablet: medium landscape -->
        <source media="(min-width: 768px)"
                srcset="tablet-landscape.jpg 1x,
                        tablet-landscape@2x.jpg 2x" />

        <!-- Mobile: portrait crop focusing on subject -->
        <source media="(max-width: 767px)"
                srcset="mobile-portrait.jpg 1x,
                        mobile-portrait@2x.jpg 2x" />

        <!-- Fallback for older browsers -->
        <img src="tablet-landscape.jpg"
             alt="City skyline"
             style="width: 100%; height: auto;" />
    </picture>

    <p>This image adapts not just in size, but in composition...</p>
</article>
```

### Print-Quality Image

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>High-Resolution Document</title>
</head>
<body>
    <h1>Annual Report 2025</h1>

    <!-- High-resolution image optimized for print -->
    <figure>
        <img src="chart-150dpi.png"
             srcset="chart-72dpi.png 1x,
                     chart-150dpi.png 2x,
                     chart-300dpi.png 4x"
             width="600"
             height="400"
             alt="Sales performance chart"
             style="border: 1pt solid #ccc;" />
        <figcaption>Figure 1: Annual sales growth by region</figcaption>
    </figure>

    <p>
        The chart above demonstrates exceptional growth across all regions,
        with particularly strong performance in Asia-Pacific markets.
    </p>
</body>
</html>
```

### Gallery with Multiple Images

```html
<section>
    <h2>Photo Gallery</h2>

    <div style="display: grid; grid-template-columns: repeat(3, 1fr); gap: 15pt;">
        <!-- Gallery image 1 -->
        <figure>
            <img src="gallery1.jpg"
                 srcset="gallery1-small.jpg 300w,
                         gallery1-medium.jpg 600w,
                         gallery1-large.jpg 900w"
                 sizes="(max-width: 600px) 100vw,
                        (max-width: 1000px) 50vw,
                        33vw"
                 alt="Mountain peak"
                 style="width: 100%; height: auto;" />
            <figcaption>Mountain Peak</figcaption>
        </figure>

        <!-- Gallery image 2 -->
        <figure>
            <img src="gallery2.jpg"
                 srcset="gallery2-small.jpg 300w,
                         gallery2-medium.jpg 600w,
                         gallery2-large.jpg 900w"
                 sizes="(max-width: 600px) 100vw,
                        (max-width: 1000px) 50vw,
                        33vw"
                 alt="Forest trail"
                 style="width: 100%; height: auto;" />
            <figcaption>Forest Trail</figcaption>
        </figure>

        <!-- Gallery image 3 -->
        <figure>
            <img src="gallery3.jpg"
                 srcset="gallery3-small.jpg 300w,
                         gallery3-medium.jpg 600w,
                         gallery3-large.jpg 900w"
                 sizes="(max-width: 600px) 100vw,
                        (max-width: 1000px) 50vw,
                        33vw"
                 alt="Coastal sunset"
                 style="width: 100%; height: auto;" />
            <figcaption>Coastal Sunset</figcaption>
        </figure>
    </div>
</section>
```

### Format Selection with Picture

```html
<article>
    <h1>Modern Image Formats</h1>

    <!-- Serve modern formats with fallbacks -->
    <picture>
        <!-- AVIF format (best compression) -->
        <source type="image/avif"
                srcset="photo.avif 1x,
                        photo@2x.avif 2x" />

        <!-- WebP format (good compression, wide support) -->
        <source type="image/webp"
                srcset="photo.webp 1x,
                        photo@2x.webp 2x" />

        <!-- JPEG fallback (universal support) -->
        <img src="photo.jpg"
             srcset="photo.jpg 1x,
                     photo@2x.jpg 2x"
             width="800"
             height="600"
             alt="Beautiful landscape"
             style="width: 100%; height: auto;" />
    </picture>

    <p>
        This image is served in the most efficient format
        supported by your viewing device.
    </p>
</article>
```

### Data-Bound Responsive Images

```html
<!-- Model: {
    products: [
        {
            name: "Widget A",
            image: {
                src: "widget-a.jpg",
                srcset: "widget-a-400.jpg 400w, widget-a-800.jpg 800w",
                sizes: "(max-width: 600px) 400px, 800px"
            }
        },
        {
            name: "Widget B",
            image: {
                src: "widget-b.jpg",
                srcset: "widget-b-400.jpg 400w, widget-b-800.jpg 800w",
                sizes: "(max-width: 600px) 400px, 800px"
            }
        }
    ]
} -->

<section>
    <h2>Featured Products</h2>

    <template data-bind="{{model.products}}">
        <article style="margin: 20pt 0; padding: 15pt; border: 1pt solid #ccc;">
            <h3>{{.name}}</h3>
            <img src="{{.image.src}}"
                 srcset="{{.image.srcset}}"
                 sizes="{{.image.sizes}}"
                 alt="{{.name}}"
                 style="width: 100%; height: auto; max-width: 400pt;" />
        </article>
    </template>
</section>
```

### Comparison Chart

```html
<article>
    <h1>Product Comparison</h1>

    <table style="width: 100%; border-collapse: collapse;">
        <thead>
            <tr>
                <th style="border: 1pt solid #ddd; padding: 10pt;">Feature</th>
                <th style="border: 1pt solid #ddd; padding: 10pt;">Basic</th>
                <th style="border: 1pt solid #ddd; padding: 10pt;">Pro</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="border: 1pt solid #ddd; padding: 10pt;">Product Image</td>
                <td style="border: 1pt solid #ddd; padding: 10pt; text-align: center;">
                    <img src="basic.jpg"
                         srcset="basic-1x.jpg 1x, basic-2x.jpg 2x"
                         width="100"
                         height="100"
                         alt="Basic model" />
                </td>
                <td style="border: 1pt solid #ddd; padding: 10pt; text-align: center;">
                    <img src="pro.jpg"
                         srcset="pro-1x.jpg 1x, pro-2x.jpg 2x"
                         width="100"
                         height="100"
                         alt="Pro model" />
                </td>
            </tr>
        </tbody>
    </table>
</article>
```

### Infographic with High-Resolution

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Infographic</title>
</head>
<body>
    <article>
        <h1>2025 Industry Statistics</h1>

        <!-- High-resolution infographic for printing -->
        <figure style="margin: 30pt 0; text-align: center;">
            <img src="infographic-medium.png"
                 srcset="infographic-low.png 1x,
                         infographic-medium.png 2x,
                         infographic-high.png 3x,
                         infographic-print.png 4x"
                 width="800"
                 alt="Industry statistics infographic"
                 style="max-width: 100%; height: auto; border: 1pt solid #ddd;" />
            <figcaption>
                <strong>Figure 1:</strong> Key industry metrics for 2025
            </figcaption>
        </figure>

        <p>
            As illustrated in the infographic above, the industry has seen
            remarkable growth across all key performance indicators.
        </p>
    </article>
</body>
</html>
```

### Responsive Banner

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Promotional Banner</title>
    <style>
        .banner {
            width: 100%;
            height: auto;
            display: block;
        }
    </style>
</head>
<body>
    <!-- Responsive promotional banner -->
    <picture>
        <!-- Desktop: full banner with all text -->
        <source media="(min-width: 1024px)"
                srcset="banner-desktop.jpg 1x,
                        banner-desktop@2x.jpg 2x" />

        <!-- Tablet: medium banner -->
        <source media="(min-width: 768px)"
                srcset="banner-tablet.jpg 1x,
                        banner-tablet@2x.jpg 2x" />

        <!-- Mobile: simplified banner -->
        <source media="(max-width: 767px)"
                srcset="banner-mobile.jpg 1x,
                        banner-mobile@2x.jpg 2x" />

        <!-- Fallback -->
        <img src="banner-desktop.jpg"
             alt="Special offer: 30% off all products"
             class="banner" />
    </picture>

    <section>
        <h1>Limited Time Offer</h1>
        <p>Save 30% on all products this week only!</p>
    </section>
</body>
</html>
```

### Technical Documentation with Diagrams

```html
<article>
    <h1>System Architecture</h1>

    <section>
        <h2>Overview</h2>
        <p>The system follows a microservices architecture...</p>

        <!-- Architecture diagram with high resolution for clarity -->
        <figure style="margin: 30pt 0;">
            <img src="architecture-diagram.png"
                 srcset="architecture-72dpi.png 1x,
                         architecture-150dpi.png 2x,
                         architecture-300dpi.png 4x"
                 width="700"
                 height="500"
                 alt="System architecture diagram"
                 style="width: 100%; max-width: 700pt; height: auto; border: 1pt solid #ccc;" />
            <figcaption>
                <strong>Figure 2:</strong> High-level system architecture
            </figcaption>
        </figure>
    </section>

    <section>
        <h2>Component Details</h2>
        <p>Each component serves a specific purpose...</p>
    </section>
</article>
```

### Certificate with Logo

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Certificate of Achievement</title>
    <style>
        .certificate {
            border: 5pt double #336699;
            padding: 40pt;
            text-align: center;
        }
        .logo {
            margin-bottom: 20pt;
        }
    </style>
</head>
<body>
    <div class="certificate">
        <!-- High-resolution logo for print -->
        <img src="logo.png"
             srcset="logo-1x.png 1x,
                     logo-2x.png 2x,
                     logo-3x.png 3x,
                     logo-print.png 4x"
             width="150"
             height="150"
             alt="Company logo"
             class="logo" />

        <h1 style="font-size: 28pt; margin: 20pt 0;">
            Certificate of Achievement
        </h1>

        <p style="font-size: 14pt; margin: 20pt 0;">
            This certifies that
        </p>

        <p style="font-size: 22pt; margin: 20pt 0; font-weight: bold;">
            John Doe
        </p>

        <p style="font-size: 14pt; margin: 20pt 0;">
            has successfully completed the course<br/>
            <strong>Advanced PDF Generation</strong>
        </p>

        <p style="font-size: 12pt; margin-top: 30pt;">
            January 15, 2025
        </p>
    </div>
</body>
</html>
```

---

## See Also

- [img](/reference/htmltags/img.html) - Image element
- [picture](/reference/htmltags/picture.html) - Picture element for art direction
- [source](/reference/htmltags/source.html) - Source element for media
- [src](/reference/htmlattributes/src.html) - Source attribute
- [alt](/reference/htmlattributes/alt.html) - Alternative text attribute
- [width](/reference/htmlattributes/width.html) - Width attribute
- [height](/reference/htmlattributes/height.html) - Height attribute
- [sizes](/reference/htmlattributes/sizes.html) - Sizes attribute for responsive images

---
