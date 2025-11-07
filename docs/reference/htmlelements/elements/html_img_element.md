---
layout: default
title: img
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;img&gt; : The Image Element
{: .no_toc }

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---


## Summary
The `<img>` element embeds images in PDF documents. It supports multiple image sources including local files, remote URLs, and data URIs. Images can be in various formats (JPEG, PNG, GIF) and can be dynamically sized, positioned, and styled.

## Usage

The `<img>` element displays images that:
- Load from local file paths, URLs, or data URIs
- Support JPEG, PNG, and GIF image formats
- Can be sized using width and height attributes or CSS
- Scale proportionally while maintaining aspect ratio
- Support inline, block, and floating display modes
- Can be positioned absolutely or relatively
- Support background images when used with CSS
- Allow dynamic image binding from data sources

```html
<!-- Basic image from file -->
<img src="images/photo.jpg" width="200pt" height="150pt" />

<!-- Image from URL -->
<img src="https://example.com/image.png" alt="Remote Image" />

<!-- Responsive image with CSS -->
<img src="logo.png" style="max-width: 100%; height: auto;" />
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the image. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Image-Specific Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `src` | string | **Required**. Image source: file path, URL, or data URI (data:image/...). |
| `width` | Unit | Image width. Supports units: pt, px, mm, cm, in, %, em. |
| `height` | Unit | Image height. Supports units: pt, px, mm, cm, in, %, em. |
| `alt` | string | Alternative text for the image (accessibility and when image fails to load). |
| `align` | FloatMode | Image alignment: `left`, `right`, `none`. Maps to CSS `float` property. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-img` | ImageData | Binds image data object directly (binding only). |
| `data-img-data` | byte[] | Binary image data (binding only). |
| `data-img-type` | MimeType | MIME type for binary data: `image/jpeg`, `image/png`, `image/gif`. |

### Configuration Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-allow-missing-images` | boolean | If true, missing images won't cause errors. Default: false. |
| `data-min-scale` | double | Minimum scale reduction for images. Prevents over-shrinking. |

### CSS Style Support

The `<img>` element supports extensive CSS styling:

**Sizing**:
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`

**Positioning**:
- `display`: `inline` (default), `block`, `inline-block`, `none`
- `position`: `static`, `relative`, `absolute`
- `float`: `left`, `right`, `none`
- `clear`: `both`, `left`, `right`, `none`
- `top`, `left`, `right`, `bottom` (for positioned elements)
- `vertical-align`: `top`, `middle`, `bottom`, `baseline`

**Spacing**:
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding` (all variants)

**Visual Effects**:
- `border`, `border-width`, `border-color`, `border-style`, `border-radius`
- `opacity`
- `transform` (rotation, scaling, translation)
- `object-fit`: `fill`, `contain`, `cover`

**Background (for container divs)**:
- `background-image`: Use with divs for background images
- `background-size`: `cover`, `contain`, or specific dimensions
- `background-position`: `center`, `top left`, etc.
- `background-repeat`: `no-repeat`, `repeat`, `repeat-x`, `repeat-y`

---

## Notes

### Image Sources

Scryber supports multiple image source types:

1. **Local File Paths**: Relative or absolute paths to image files
   ```html
   <img src="images/photo.jpg" />
   <img src="/absolute/path/image.png" />
   ```

2. **Remote URLs**: HTTP/HTTPS URLs to images
   ```html
   <img src="https://example.com/image.png" />
   ```

3. **Data URIs**: Base64-encoded images embedded directly
   ```html
   <img src="data:image/png;base64,iVBORw0KGgoAAAANS..." />
   ```

### Supported Image Formats

Scryber supports the following image formats:
- **JPEG** (.jpg, .jpeg): Best for photographs and complex images
- **PNG** (.png): Supports transparency, good for logos and graphics
- **GIF** (.gif): Animated and static GIF images

### Image Sizing

Images can be sized in several ways:

1. **Explicit Dimensions**: Set exact width and height
   ```html
   <img src="photo.jpg" width="200pt" height="150pt" />
   ```

2. **Proportional Sizing**: Set one dimension, other scales automatically
   ```html
   <img src="photo.jpg" width="200pt" />
   ```

3. **CSS Max/Min Constraints**: Constrain size while maintaining aspect ratio
   ```html
   <img src="photo.jpg" style="max-width: 300pt; height: auto;" />
   ```

4. **Percentage Sizing**: Size relative to container
   ```html
   <img src="photo.jpg" style="width: 50%;" />
   ```

### Aspect Ratio Preservation

By default, images maintain their aspect ratio:
- If only width is specified, height scales proportionally
- If only height is specified, width scales proportionally
- If both are specified, image may be stretched/compressed

### Image Positioning

Control image position using CSS:

```html
<!-- Inline image (default) -->
<img src="icon.png" width="20pt" />

<!-- Block image (full width) -->
<img src="banner.jpg" style="display: block; width: 100%;" />

<!-- Floating image -->
<img src="photo.jpg" style="float: left; margin-right: 10pt;" />

<!-- Absolutely positioned -->
<img src="watermark.png"
     style="position: absolute; top: 50pt; right: 50pt; opacity: 0.3;" />
```

### Background Images

For background images, use CSS on a container element:

```html
<div style="background-image: url('background.jpg');
            background-size: cover;
            background-position: center;
            width: 100%;
            height: 300pt;">
    <p style="color: white; padding: 20pt;">Content over background</p>
</div>
```

### Data URI Images

Embed images directly using base64 encoding:

```html
<img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUA
AAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO
9TXL0Y4OHwAAAABJRU5ErkJggg==" width="5pt" height="5pt" />
```

Data URIs are useful for:
- Small icons and graphics
- Embedding images without external files
- Single-file PDF templates

### Dynamic Image Binding

Bind image sources dynamically from data:

```html
<!-- With model = { imagePath: "photo.jpg" } -->
<img src="{{model.imagePath}}" width="200pt" />

<!-- Binary data binding -->
<img data-img-data="{{model.imageBytes}}"
     data-img-type="image/jpeg"
     width="150pt" height="100pt" />
```

### Missing Images

Control behavior when images fail to load:

```html
<!-- Strict mode: throws error if image missing (default) -->
<img src="photo.jpg" width="100pt" />

<!-- Lenient mode: continues without error -->
<img src="photo.jpg" width="100pt" data-allow-missing-images="true" />
```

Use `alt` attribute to provide fallback text:
```html
<img src="photo.jpg" alt="Profile Photo" />
```

### Image Scaling

Control minimum scale reduction to prevent over-shrinking:

```html
<img src="large-image.jpg"
     style="max-width: 300pt;"
     data-min-scale="0.5" />
```

This prevents images from being scaled below 50% of their original size.

### Class Hierarchy

In the Scryber codebase:
- `HTMLImage` extends `Image` extends `ImageBase` extends `VisualComponent`
- Default display mode: `inline`
- Supports inline, block, and floating layouts

---

## Examples

### Basic Image Display

```html
<!-- Simple image -->
<img src="photo.jpg" width="200pt" height="150pt" />

<!-- Image with alt text -->
<img src="logo.png" alt="Company Logo" width="100pt" />

<!-- Image from URL -->
<img src="https://example.com/banner.jpg" width="500pt" />
```

### Responsive Images

```html
<!-- Full width, maintain aspect ratio -->
<img src="banner.jpg" style="width: 100%; height: auto;" />

<!-- Constrained maximum size -->
<img src="photo.jpg" style="max-width: 400pt; max-height: 300pt;" />

<!-- Percentage-based sizing -->
<div style="width: 300pt;">
    <img src="image.jpg" style="width: 50%;" />
</div>
```

### Floating Images with Text Wrap

```html
<div>
    <img src="portrait.jpg" width="150pt" height="200pt"
         style="float: left; margin: 0 15pt 10pt 0;" />
    <p>
        This text will flow around the floating image on the left.
        The image has margins to create spacing between the image
        and the surrounding text content.
    </p>
    <p>
        Floating images are useful for magazine-style layouts where
        you want text to wrap naturally around visual elements.
    </p>
</div>

<!-- Clear the float -->
<div style="clear: both;"></div>

<div>
    <img src="diagram.png" width="200pt"
         style="float: right; margin: 0 0 10pt 15pt;" />
    <p>
        This image floats on the right side with text flowing on the left.
        Use the float property to control image positioning relative to text.
    </p>
</div>
```

### Centered Images

```html
<!-- Block image, centered with auto margins -->
<img src="photo.jpg" width="300pt"
     style="display: block; margin: 20pt auto;" />

<!-- Image in centered container -->
<div style="text-align: center;">
    <img src="logo.png" width="150pt" />
</div>
```

### Images with Borders and Effects

```html
<!-- Image with border -->
<img src="photo.jpg" width="200pt"
     style="border: 2pt solid #336699; padding: 5pt;" />

<!-- Rounded corners -->
<img src="profile.jpg" width="100pt" height="100pt"
     style="border-radius: 50pt; border: 3pt solid #fff;" />

<!-- Image with shadow effect -->
<img src="product.jpg" width="200pt"
     style="border: 1pt solid #ddd; padding: 10pt; background-color: white;" />

<!-- Semi-transparent image -->
<img src="watermark.png" width="150pt" style="opacity: 0.5;" />
```

### Positioned Images

```html
<!-- Absolutely positioned watermark -->
<div style="position: relative; min-height: 400pt;">
    <img src="watermark.png" width="200pt"
         style="position: absolute; top: 100pt; right: 50pt; opacity: 0.2;" />
    <p>Document content here...</p>
</div>

<!-- Top-right corner logo -->
<img src="logo.png" width="80pt"
     style="position: absolute; top: 20pt; right: 20pt;" />
```

### Image Gallery Layout

```html
<div style="text-align: center;">
    <img src="image1.jpg" width="150pt" height="150pt"
         style="margin: 5pt; border: 1pt solid #ccc;" />
    <img src="image2.jpg" width="150pt" height="150pt"
         style="margin: 5pt; border: 1pt solid #ccc;" />
    <img src="image3.jpg" width="150pt" height="150pt"
         style="margin: 5pt; border: 1pt solid #ccc;" />
</div>

<div style="text-align: center; margin-top: 10pt;">
    <img src="image4.jpg" width="150pt" height="150pt"
         style="margin: 5pt; border: 1pt solid #ccc;" />
    <img src="image5.jpg" width="150pt" height="150pt"
         style="margin: 5pt; border: 1pt solid #ccc;" />
    <img src="image6.jpg" width="150pt" height="150pt"
         style="margin: 5pt; border: 1pt solid #ccc;" />
</div>
```

### Images in Links

```html
<!-- Clickable image -->
<a href="https://example.com">
    <img src="banner.jpg" width="400pt" style="border: none;" />
</a>

<!-- Image with text link -->
<a href="#details" style="text-decoration: none; color: #333;">
    <img src="icon.png" width="30pt" style="vertical-align: middle;" />
    <span style="margin-left: 10pt;">View Details</span>
</a>
```

### Background Images

```html
<!-- Div with background image -->
<div style="background-image: url('background.jpg');
            background-size: cover;
            background-position: center;
            width: 100%;
            min-height: 300pt;
            padding: 30pt;">
    <h1 style="color: white; text-shadow: 2pt 2pt 4pt rgba(0,0,0,0.7);">
        Content Over Background
    </h1>
    <p style="color: white;">
        Background images can be used to create visually appealing layouts.
    </p>
</div>

<!-- Background image with repeat -->
<div style="background-image: url('pattern.png');
            background-repeat: repeat;
            background-size: 50pt 50pt;
            padding: 20pt;
            min-height: 200pt;">
    <p>Content with repeating background pattern</p>
</div>

<!-- Background image positioned -->
<div style="background-image: url('logo.png');
            background-repeat: no-repeat;
            background-position: top right;
            background-size: 100pt 40pt;
            padding: 20pt;
            min-height: 150pt;">
    <p>Content with logo in top-right corner</p>
</div>
```

### Data URI Images

```html
<!-- Small icon as data URI -->
<img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUA
AAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO
9TXL0Y4OHwAAAABJRU5ErkJggg==" width="5pt" height="5pt" />

<!-- Red dot example -->
<img src="data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTAiIGhlaWdodD0iMTAiPg
o8Y2lyY2xlIGN4PSI1IiBjeT0iNSIgcj0iNSIgZmlsbD0icmVkIi8+Cjwvc3ZnPg=="
     width="10pt" height="10pt" />
```

### Dynamic Image Binding

```html
<!-- Bind image source from model -->
<!-- With model = { productImage: "product.jpg", width: 200 } -->
<img src="{{model.productImage}}"
     width="{{model.width}}pt"
     alt="{{model.productName}}" />

<!-- Repeating images from collection -->
<!-- With model.images = [{src: "img1.jpg"}, {src: "img2.jpg"}] -->
<div style="text-align: center;">
    <template data-bind="{{model.images}}">
        <img src="{{.src}}" width="150pt"
             style="margin: 5pt; border: 1pt solid #ccc;" />
    </template>
</div>

<!-- Binary data binding -->
<img data-img-data="{{model.photoBytes}}"
     data-img-type="image/jpeg"
     width="200pt" height="150pt"
     alt="User Photo" />
```

### Image Cards

```html
<div style="border: 1pt solid #ddd; border-radius: 4pt;
            overflow: hidden; width: 250pt; margin: 10pt;">
    <img src="product.jpg" width="250pt" height="200pt"
         style="display: block;" />
    <div style="padding: 15pt;">
        <h3 style="margin: 0 0 10pt 0;">Product Name</h3>
        <p style="margin: 0; color: #666;">
            Product description goes here with details about the item.
        </p>
    </div>
</div>
```

### Image with Caption

```html
<div style="display: inline-block; text-align: center; margin: 10pt;">
    <img src="photo.jpg" width="200pt" height="150pt"
         style="display: block; border: 1pt solid #ccc;" />
    <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
        Figure 1: Image caption text
    </p>
</div>
```

### Responsive Image Grid

```html
<div style="width: 100%;">
    <div style="display: inline-block; width: 33%; padding: 5pt;">
        <img src="img1.jpg" style="width: 100%; height: auto;" />
    </div>
    <div style="display: inline-block; width: 33%; padding: 5pt;">
        <img src="img2.jpg" style="width: 100%; height: auto;" />
    </div>
    <div style="display: inline-block; width: 33%; padding: 5pt;">
        <img src="img3.jpg" style="width: 100%; height: auto;" />
    </div>
</div>
```

### Images with Text Overlay

```html
<div style="position: relative; width: 400pt; height: 300pt;">
    <img src="background.jpg" width="400pt" height="300pt" />
    <div style="position: absolute; bottom: 0; left: 0; right: 0;
                background-color: rgba(0,0,0,0.7); color: white;
                padding: 15pt;">
        <h2 style="margin: 0;">Image Title</h2>
        <p style="margin: 5pt 0 0 0;">Descriptive text over image</p>
    </div>
</div>
```

### Transformed Images

```html
<!-- Rotated image -->
<img src="badge.png" width="80pt" height="80pt"
     style="transform: rotate(15deg); margin: 20pt;" />

<!-- Scaled image -->
<img src="icon.png" width="50pt" height="50pt"
     style="transform: scale(1.5);" />

<!-- Skewed image -->
<img src="photo.jpg" width="100pt"
     style="transform: skewX(10deg);" />
```

### Image with Missing Fallback

```html
<!-- Image that won't cause error if missing -->
<img src="optional-image.jpg" width="200pt"
     data-allow-missing-images="true"
     alt="This image is optional" />

<!-- Conditional image display -->
<img src="{{model.imagePath}}"
     width="150pt"
     hidden="{{model.hideImage ? 'hidden' : ''}}"
     data-allow-missing-images="true" />
```

### Product Layout with Images

```html
<div style="border: 1pt solid #e0e0e0; padding: 20pt; margin: 15pt 0;">
    <div>
        <img src="product-main.jpg" width="300pt" height="300pt"
             style="float: left; margin-right: 20pt;
                    border: 1pt solid #ddd; padding: 5pt;" />
        <h2 style="margin-top: 0;">Product Name</h2>
        <p style="font-size: 14pt; color: #336699; font-weight: bold;">
            $99.99
        </p>
        <p>
            Detailed product description with specifications and features.
            The image floats to the left with text wrapping around it.
        </p>
        <h3>Features:</h3>
        <ul>
            <li>Feature one</li>
            <li>Feature two</li>
            <li>Feature three</li>
        </ul>
    </div>
    <div style="clear: both; margin-top: 20pt; text-align: center;">
        <img src="thumb1.jpg" width="80pt" height="80pt"
             style="margin: 5pt; border: 1pt solid #ddd;" />
        <img src="thumb2.jpg" width="80pt" height="80pt"
             style="margin: 5pt; border: 1pt solid #ddd;" />
        <img src="thumb3.jpg" width="80pt" height="80pt"
             style="margin: 5pt; border: 1pt solid #ddd;" />
    </div>
</div>
```

### Header with Logo and Title

```html
<div style="border-bottom: 2pt solid #336699; padding: 15pt;
            margin-bottom: 20pt;">
    <img src="company-logo.png" width="80pt" height="30pt"
         style="float: left; margin-right: 15pt; vertical-align: middle;" />
    <h1 style="margin: 0; line-height: 30pt; color: #336699;">
        Company Report
    </h1>
    <div style="clear: both;"></div>
</div>
```

---

## See Also

- [a](/reference/htmltags/a.html) - Anchor/link element (wraps images for clickable links)
- [div](/reference/htmltags/div.html) - Container element (for background images)
- [figure](/reference/htmltags/figure.html) - Figure element with caption
- [Image Component](/reference/components/image.html) - Base image component in Scryber namespace
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Data binding and expressions
- [Image Formats](/reference/images/formats.html) - Supported image formats and optimization

---
