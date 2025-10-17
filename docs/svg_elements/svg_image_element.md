---
layout: default
title: image (SVG)
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;image&gt; : The SVG Image Element

The `<image>` element is used to embed raster images (PNG, JPEG, etc.) within SVG graphics. It provides precise positioning and sizing control for images in vector-based documents and supports transformations.

---

## Summary

The `<image>` element embeds external image resources into SVG content. Unlike HTML img elements, SVG images use a coordinate-based positioning system and can be transformed using SVG transformation matrices. Images can be sourced from local files, URLs, or data URIs.

Key features:
- Embed PNG, JPEG, GIF, and other image formats
- Precise positioning with x, y coordinates
- Explicit width and height control
- Aspect ratio preservation options
- Transformation support (rotate, scale, skew)
- Opacity control
- Data binding for dynamic image sources
- Support for base64 encoded data URIs

---

## Usage

The `<image>` element is placed within an SVG container with position and size attributes:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <image href="./images/logo.png" x="50" y="50" width="100" height="100"/>
</svg>
```

### Basic Syntax

```html
<!-- Simple image with position and size -->
<image href="logo.png" x="10" y="10" width="80" height="80"/>

<!-- Image from URL -->
<image href="https://example.com/image.jpg" x="20" y="20" width="200" height="150"/>

<!-- Image with data URI -->
<image href="data:image/png;base64,iVBORw0KG..." x="0" y="0" width="100" height="100"/>

<!-- Image with aspect ratio preservation -->
<image href="photo.jpg" x="50" y="50" width="300" height="200" preserveAspectRatio="xMidYMid meet"/>

<!-- Image with transformation -->
<image href="icon.png" x="100" y="100" width="50" height="50" transform="rotate(45 125 125)"/>
```

---

## Supported Attributes

### Source Attribute

| Attribute | Type | Description | Required |
|-----------|------|-------------|----------|
| `href` | String | Path or URL to the image resource | Yes |

### Position Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `x` | Unit | Horizontal position of the top-left corner | 0 |
| `y` | Unit | Vertical position of the top-left corner | 0 |

### Size Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `width` | Unit | Width of the image | Required |
| `height` | Unit | Height of the image | Required |

### Aspect Ratio Attribute

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `preserveAspectRatio` | String | How to preserve aspect ratio when scaling | `xMidYMid meet` |

Valid `preserveAspectRatio` values:
- `none` - Do not preserve aspect ratio, stretch to fill
- `xMinYMin meet` - Align to top-left, scale proportionally
- `xMidYMid meet` - Center and scale proportionally (default)
- `xMaxYMax meet` - Align to bottom-right, scale proportionally
- `xMinYMin slice` - Align to top-left, scale to cover
- `xMidYMid slice` - Center and scale to cover
- Additional combinations with Min/Mid/Max for both axes

### Transform Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `transform` | Transform | Transformation operations | none |
| `opacity` | Double | Image opacity (0.0-1.0) | 1.0 |

### Common Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | String | Unique identifier for the element |
| `class` | String | CSS class name(s) for styling |
| `style` | Style | Inline CSS-style properties |
| `title` | String | Tooltip or title text |
| `desc` | String | Description for accessibility |

---

## Data Binding

The `<image>` element supports dynamic source URLs, positioning, and sizing through data binding.

### Dynamic Image Source

```html
<!-- Simple source binding -->
<image href="{{model.logoUrl}}" x="10" y="10" width="100" height="100"/>

<!-- Conditional image source -->
<image href="{{model.isPremium ? 'premium-badge.png' : 'standard-badge.png'}}"
       x="50" y="50" width="80" height="80"/>

<!-- Path construction -->
<image href="./images/{{model.category}}/{{model.imageFile}}"
       x="20" y="20" width="200" height="150"/>
```

### Dynamic Positioning

```html
<!-- Data-driven coordinates -->
<image href="icon.png"
       x="{{model.posX}}"
       y="{{model.posY}}"
       width="50"
       height="50"/>

<!-- Calculated positions -->
<image href="marker.png"
       x="{{model.chartWidth * 0.5 - 25}}"
       y="{{model.dataValue * 2}}"
       width="50"
       height="50"/>
```

### Dynamic Sizing

```html
<!-- Data-driven dimensions -->
<image href="thumbnail.jpg"
       x="10"
       y="10"
       width="{{model.thumbnailSize}}"
       height="{{model.thumbnailSize}}"/>

<!-- Proportional sizing -->
<image href="background.png"
       x="0"
       y="0"
       width="{{model.containerWidth}}"
       height="{{model.containerHeight}}"/>
```

### Conditional Display

```html
<!-- Show/hide with opacity -->
<image href="watermark.png"
       x="100"
       y="100"
       width="200"
       height="200"
       opacity="{{model.showWatermark ? 0.3 : 0}}"/>

<!-- Conditional rendering with hidden attribute -->
<image href="badge.png"
       x="20"
       y="20"
       width="60"
       height="60"
       hidden="{{!model.hasBadge}}"/>
```

### Template Iteration

```html
<!-- Generate multiple images from data -->
<svg xmlns="http://www.w3.org/2000/svg" width="500" height="400">
    <template data-bind="{{model.icons}}">
        <image href="{{.iconUrl}}"
               x="{{$index * 100 + 20}}"
               y="50"
               width="80"
               height="80"/>
    </template>
</svg>

<!-- Grid layout of images -->
<template data-bind="{{model.thumbnails}}">
    <image href="{{.url}}"
           x="{{($index % 4) * 120 + 10}}"
           y="{{Math.floor($index / 4) * 120 + 10}}"
           width="100"
           height="100"/>
</template>
```

---

## Notes

### Supported Image Formats

Scryber supports common raster image formats:
- PNG (recommended for logos, icons, transparency)
- JPEG (recommended for photographs)
- GIF (supported, limited animation support)
- BMP (supported, not recommended)
- TIFF (limited support)

### Image Sources

Images can be loaded from:
1. **Relative paths** - `./images/logo.png`
2. **Absolute paths** - `/assets/images/logo.png`
3. **URLs** - `https://example.com/image.jpg`
4. **Data URIs** - `data:image/png;base64,...`
5. **File system paths** - Platform-specific absolute paths

### Coordinate System

- The `x` and `y` attributes position the top-left corner of the image
- Coordinates are in the SVG coordinate system (user units)
- Default user units are typically points (1pt = 1/72 inch)
- Use `transform` for rotation or other positioning adjustments

### Aspect Ratio Preservation

The `preserveAspectRatio` attribute controls how images are scaled:
- **meet** - Scale to fit entirely within the viewport (letterbox/pillarbox)
- **slice** - Scale to cover the viewport entirely (may crop)
- **none** - Ignore aspect ratio, stretch to fill

### Width and Height

- Both `width` and `height` are required attributes
- Omitting these may result in rendering errors
- Set dimensions to match your design requirements
- Use `preserveAspectRatio` to control scaling behavior

### Transformations

Images support SVG transformations:
- `translate(x, y)` - Move the image
- `rotate(angle)` or `rotate(angle, cx, cy)` - Rotate around origin or point
- `scale(sx, sy)` - Scale by factors
- Multiple transformations can be combined

### Performance Considerations

- Large images increase PDF file size significantly
- Consider image compression before embedding
- Use appropriate resolutions (72-150 DPI for screen, 300+ for print)
- Data URIs embed images directly but increase markup size
- External URLs require network access during generation

### Missing Images

- Missing images may show a placeholder or error
- Use the `data-allow-missing-images` attribute to suppress errors
- Check image paths are correct and accessible
- Verify file permissions and network access

### Opacity and Blending

- Use the `opacity` attribute for transparency effects
- Opacity affects the entire image uniformly
- For selective transparency, use PNG images with alpha channels
- Blending modes have limited support

---

## Examples

### 1. Simple Logo

Basic logo placement:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="100">
    <image href="./images/company-logo.png" x="20" y="20" width="120" height="60"/>
</svg>
```

### 2. Centered Image

Image centered in viewport:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <image href="./images/banner.jpg" x="50" y="75" width="300" height="150"/>
</svg>
```

### 3. Photo with Aspect Ratio

Photograph with proportional scaling:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="500" height="400">
    <image href="./photos/team-photo.jpg"
           x="50" y="50"
           width="400" height="300"
           preserveAspectRatio="xMidYMid meet"/>
</svg>
```

### 4. Icon with Rotation

Rotated icon graphic:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="200">
    <image href="./icons/arrow.png"
           x="75" y="75"
           width="50" height="50"
           transform="rotate(45 100 100)"/>
</svg>
```

### 5. Watermark with Opacity

Semi-transparent watermark:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="400">
    <!-- Content -->
    <rect width="600" height="400" fill="#f0f0f0"/>

    <!-- Watermark -->
    <image href="./images/watermark.png"
           x="200" y="150"
           width="200" height="100"
           opacity="0.2"/>
</svg>
```

### 6. Profile Picture

Circular profile photo (clipped with clipPath):

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="200">
    <defs>
        <clipPath id="circleClip">
            <circle cx="100" cy="100" r="80"/>
        </clipPath>
    </defs>

    <image href="./images/profile.jpg"
           x="20" y="20"
           width="160" height="160"
           clip-path="url(#circleClip)"/>
</svg>
```

### 7. Data-Bound Logo

Dynamic logo based on model:

```html
<!-- Model: { companyLogo: "./images/acme-corp-logo.png" } -->
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="100">
    <image href="{{model.companyLogo}}"
           x="10" y="10"
           width="100" height="80"/>
</svg>
```

### 8. Conditional Badge

Badge shown based on status:

```html
<!-- Model: { isPremium: true, badgeImage: "premium-badge.png" } -->
<svg xmlns="http://www.w3.org/2000/svg" width="150" height="150">
    <image href="{{model.isPremium ? model.badgeImage : 'standard-badge.png'}}"
           x="25" y="25"
           width="100" height="100"/>
</svg>
```

### 9. Product Thumbnail Grid

Grid of product images:

```html
<!-- Model: { products: [{image: "prod1.jpg"}, {image: "prod2.jpg"}, ...] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="450" height="300">
    <template data-bind="{{model.products}}">
        <image href="./images/{{.image}}"
               x="{{($index % 3) * 150 + 10}}"
               y="{{Math.floor($index / 3) * 150 + 10}}"
               width="130"
               height="130"/>
    </template>
</svg>
```

### 10. Chart with Icon Markers

Data points with icon markers:

```html
<!-- Model: { dataPoints: [{x: 50, y: 100, icon: "up.png"}, ...] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="500" height="300">
    <!-- Chart background -->
    <rect width="500" height="300" fill="#f9f9f9"/>

    <!-- Data point icons -->
    <template data-bind="{{model.dataPoints}}">
        <circle cx="{{.x}}" cy="{{.y}}" r="8" fill="#336699"/>
        <image href="./icons/{{.icon}}"
               x="{{.x - 12}}"
               y="{{.y - 30}}"
               width="24"
               height="24"/>
    </template>
</svg>
```

### 11. Status Icon

Status indicator icon with color coding:

```html
<!-- Model: { status: "success", statusIcon: "check-circle.png" } -->
<svg xmlns="http://www.w3.org/2000/svg" width="100" height="100">
    <circle cx="50" cy="50" r="45"
            fill="{{model.status === 'success' ? '#00aa00' : '#cc0000'}}"
            opacity="0.2"/>
    <image href="./icons/{{model.statusIcon}}"
           x="25" y="25"
           width="50" height="50"/>
</svg>
```

### 12. Background Image

Full background image with content overlay:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="800" height="600">
    <!-- Background -->
    <image href="./images/background.jpg"
           x="0" y="0"
           width="800" height="600"
           preserveAspectRatio="xMidYMid slice"/>

    <!-- Overlay content -->
    <rect x="0" y="0" width="800" height="600" fill="black" opacity="0.3"/>
    <text x="400" y="300" text-anchor="middle" font-size="48" fill="white" font-weight="700">
        Welcome
    </text>
</svg>
```

### 13. Company Logos Row

Horizontal row of partner logos:

```html
<!-- Model: { partners: [{logo: "partner1.png", name: "ABC"}, ...] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="120">
    <text x="300" y="30" text-anchor="middle" font-size="16" fill="#333">
        Our Partners
    </text>

    <template data-bind="{{model.partners}}">
        <image href="./logos/{{.logo}}"
               x="{{$index * 150 + 50}}"
               y="50"
               width="100"
               height="60"
               preserveAspectRatio="xMidYMid meet"/>
    </template>
</svg>
```

### 14. Image with Border Frame

Image inside decorative border:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="350" height="250">
    <rect x="10" y="10" width="330" height="230" fill="white" stroke="#336699" stroke-width="3"/>
    <rect x="20" y="20" width="310" height="210" fill="none" stroke="#999" stroke-width="1"/>

    <image href="./images/featured.jpg"
           x="30" y="30"
           width="290" height="190"
           preserveAspectRatio="xMidYMid meet"/>
</svg>
```

### 15. Dynamic Size Avatar

Avatar with size based on user tier:

```html
<!-- Model: { userAvatar: "user123.jpg", userTier: 2 } -->
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="200">
    <defs>
        <clipPath id="avatarClip">
            <circle cx="100" cy="100" r="{{model.userTier * 20 + 40}}"/>
        </clipPath>
    </defs>

    <image href="./avatars/{{model.userAvatar}}"
           x="{{100 - (model.userTier * 20 + 40)}}"
           y="{{100 - (model.userTier * 20 + 40)}}"
           width="{{(model.userTier * 20 + 40) * 2}}"
           height="{{(model.userTier * 20 + 40) * 2}}"
           clip-path="url(#avatarClip)"/>
</svg>
```

### 16. Comparison Layout

Side-by-side image comparison:

```html
<!-- Model: { beforeImage: "before.jpg", afterImage: "after.jpg" } -->
<svg xmlns="http://www.w3.org/2000/svg" width="650" height="300">
    <text x="150" y="30" text-anchor="middle" font-size="16" font-weight="600">Before</text>
    <image href="{{model.beforeImage}}"
           x="25" y="50"
           width="250" height="200"
           preserveAspectRatio="xMidYMid meet"/>

    <text x="500" y="30" text-anchor="middle" font-size="16" font-weight="600">After</text>
    <image href="{{model.afterImage}}"
           x="375" y="50"
           width="250" height="200"
           preserveAspectRatio="xMidYMid meet"/>
</svg>
```

### 17. Category Icons

Category selection with icons:

```html
<!-- Model: { categories: [{name: "Home", icon: "home.png", selected: true}, ...] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="500" height="100">
    <template data-bind="{{model.categories}}">
        <g>
            <rect x="{{$index * 100 + 10}}"
                  y="20"
                  width="80"
                  height="80"
                  rx="5"
                  fill="{{.selected ? '#336699' : '#f0f0f0'}}"
                  opacity="{{.selected ? 0.2 : 1}}"/>

            <image href="./icons/{{.icon}}"
                   x="{{$index * 100 + 30}}"
                   y="35"
                   width="40"
                   height="40"/>

            <text x="{{$index * 100 + 50}}"
                  y="85"
                  text-anchor="middle"
                  font-size="11"
                  fill="{{.selected ? '#336699' : '#333'}}">
                {{.name}}
            </text>
        </g>
    </template>
</svg>
```

### 18. Report Header with Logo

Report header combining logo and text:

```html
<!-- Model: { companyLogo: "logo.png", reportTitle: "Q4 Financial Report", reportDate: "2024-12-31" } -->
<svg xmlns="http://www.w3.org/2000/svg" width="700" height="120">
    <rect width="700" height="120" fill="#336699"/>

    <image href="{{model.companyLogo}}"
           x="20" y="30"
           width="100" height="60"
           preserveAspectRatio="xMidYMid meet"/>

    <text x="140" y="55" font-size="24" font-weight="700" fill="white">
        {{model.reportTitle}}
    </text>
    <text x="140" y="80" font-size="14" fill="white" opacity="0.9">
        Generated: {{model.reportDate}}
    </text>
</svg>
```

### 19. Image Gallery with Captions

Gallery layout with captions:

```html
<!-- Model: { gallery: [{image: "img1.jpg", caption: "Sunset"}, ...] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="650" height="450">
    <template data-bind="{{model.gallery}}">
        <g>
            <image href="./gallery/{{.image}}"
                   x="{{($index % 3) * 210 + 10}}"
                   y="{{Math.floor($index / 3) * 220 + 10}}"
                   width="190"
                   height="170"
                   preserveAspectRatio="xMidYMid slice"/>

            <text x="{{($index % 3) * 210 + 105}}"
                  y="{{Math.floor($index / 3) * 220 + 195}}"
                  text-anchor="middle"
                  font-size="12"
                  fill="#333">
                {{.caption}}
            </text>
        </g>
    </template>
</svg>
```

### 20. QR Code Placement

QR code with instructions:

```html
<!-- Model: { qrCodeUrl: "data:image/png;base64,..." } -->
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="350">
    <rect x="10" y="10" width="280" height="330" rx="5" fill="#f9f9f9" stroke="#ccc" stroke-width="1"/>

    <text x="150" y="40" text-anchor="middle" font-size="18" font-weight="700" fill="#336699">
        Scan to Connect
    </text>

    <image href="{{model.qrCodeUrl}}"
           x="75" y="60"
           width="150" height="150"/>

    <text x="150" y="240" text-anchor="middle" font-size="12" fill="#666">
        Scan this QR code with your mobile device
    </text>
    <text x="150" y="260" text-anchor="middle" font-size="12" fill="#666">
        to access the online portal
    </text>
</svg>
```

---

## See Also

- [svg element](/reference/svgtags/svg.html) - SVG container element
- [preserveAspectRatio attribute](/reference/svgattributes/attr_preserveAspectRatio.html) - Aspect ratio control
- [transform attribute](/reference/svgattributes/attr_transform.html) - Transformation operations
- [clipPath element](/reference/svgtags/clipPath.html) - Clipping paths for images
- [Data Binding](/reference/binding/) - Complete data binding guide
- [Image Handling](/reference/images/) - Image loading and processing
- [PDF Resources](/reference/resources/) - Resource management in PDFs

---
