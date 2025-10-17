---
layout: default
title: preserveAspectRatio
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @preserveAspectRatio : The Aspect Ratio Preservation Attribute

The `preserveAspectRatio` attribute controls how an SVG's viewBox is scaled and positioned within the viewport when their aspect ratios differ. It determines whether to maintain the aspect ratio, how to align the content, and whether to fit the content inside or fill the viewport.

## Usage

The `preserveAspectRatio` attribute controls SVG scaling behavior:
- Maintain aspect ratio when viewBox and viewport dimensions differ
- Control content alignment (left/center/right, top/middle/bottom)
- Choose between fitting content inside viewport or filling viewport
- Prevent distortion of graphics and text
- Support responsive designs with predictable scaling
- Enable data binding for dynamic aspect ratio control
- Create letterbox or pillarbox effects

```html
<!-- Default: center and fit inside -->
<svg width="300pt" height="200pt" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid meet">
    <circle cx="50" cy="50" r="40" fill="blue"/>
</svg>

<!-- Top-left aligned, fit inside -->
<svg width="300pt" height="200pt" viewBox="0 0 100 100" preserveAspectRatio="xMinYMin meet">
    <circle cx="50" cy="50" r="40" fill="red"/>
</svg>

<!-- No aspect ratio preservation (stretch to fill) -->
<svg width="300pt" height="200pt" viewBox="0 0 100 100" preserveAspectRatio="none">
    <circle cx="50" cy="50" r="40" fill="green"/>
</svg>

<!-- Dynamic aspect ratio control -->
<svg width="300pt" height="200pt" viewBox="0 0 100 100"
     preserveAspectRatio="{{model.alignMode}} {{model.scaleMode}}">
    <rect x="10" y="10" width="80" height="80" fill="purple"/>
</svg>
```

---

## Supported Values

The `preserveAspectRatio` attribute has two parts separated by a space:

### Syntax
```
preserveAspectRatio="<align> <meetOrSlice>"
```

### Align Values

Controls how the viewBox is positioned within the viewport:

| Value | X Alignment | Y Alignment | Description |
|-------|-------------|-------------|-------------|
| `none` | - | - | Do not preserve aspect ratio; stretch to fill |
| `xMinYMin` | Left | Top | Align top-left corner |
| `xMidYMin` | Center | Top | Align top-center |
| `xMaxYMin` | Right | Top | Align top-right corner |
| `xMinYMid` | Left | Middle | Align middle-left |
| `xMidYMid` | Center | Middle | Align center (default) |
| `xMaxYMid` | Right | Middle | Align middle-right |
| `xMinYMax` | Left | Bottom | Align bottom-left corner |
| `xMidYMax` | Center | Bottom | Align bottom-center |
| `xMaxYMax` | Right | Bottom | Align bottom-right corner |

### Meet or Slice Values

Controls how the viewBox is scaled relative to the viewport:

| Value | Behavior | Description |
|-------|----------|-------------|
| `meet` | Fit inside | Scale to fit entire viewBox inside viewport (may have empty space) |
| `slice` | Fill viewport | Scale to fill viewport with viewBox (may crop content) |

### Default Value

If omitted, the default is:
```
preserveAspectRatio="xMidYMid meet"
```

This centers the content and scales it to fit inside the viewport.

### Common Combinations

```html
<!-- Centered, fit inside (default) -->
preserveAspectRatio="xMidYMid meet"

<!-- Top-left corner, fit inside -->
preserveAspectRatio="xMinYMin meet"

<!-- Centered, fill viewport (may crop) -->
preserveAspectRatio="xMidYMid slice"

<!-- No aspect ratio preservation (stretch) -->
preserveAspectRatio="none"

<!-- Bottom-right, fit inside -->
preserveAspectRatio="xMaxYMax meet"
```

---

## Supported Elements

The `preserveAspectRatio` attribute is supported on:

### Primary Elements
- `<svg>` - Main SVG canvas
- `<symbol>` - Symbol definition
- `<image>` - Embedded image
- `<pattern>` - Pattern definition
- `<marker>` - Marker definition
- `<view>` - View definition

**Note:** Most commonly used with `<svg>` element in conjunction with `viewBox` attribute.

---

## Data Binding

The `preserveAspectRatio` attribute supports data binding for dynamic aspect ratio control:

### Dynamic Alignment

```html
<!-- Model: { alignment: "xMidYMid", scale: "meet" } -->
<svg width="400pt" height="300pt" viewBox="0 0 100 100"
     preserveAspectRatio="{{model.alignment}} {{model.scale}}">
    <rect x="10" y="10" width="80" height="80" fill="blue"/>
    <circle cx="50" cy="50" r="30" fill="red"/>
</svg>
```

### Conditional Scaling

```html
<!-- Model: { fillViewport: false } -->
<svg width="400pt" height="200pt" viewBox="0 0 100 100"
     preserveAspectRatio="xMidYMid {{model.fillViewport ? 'slice' : 'meet'}}">
    <rect x="0" y="0" width="100" height="100" fill="lightblue"/>
    <text x="50" y="55" text-anchor="middle" font-size="12" fill="navy">Content</text>
</svg>
```

### User-Selected Alignment

```html
<!-- Model: { userAlign: "xMinYMin" } -->
<svg width="300pt" height="300pt" viewBox="0 0 100 50"
     preserveAspectRatio="{{model.userAlign}} meet">
    <rect x="0" y="0" width="100" height="50" fill="yellow"/>
    <text x="50" y="28" text-anchor="middle" font-size="10">Aligned Content</text>
</svg>
```

### Toggle Stretch Mode

```html
<!-- Model: { maintainAspect: true } -->
<svg width="400pt" height="200pt" viewBox="0 0 100 100"
     preserveAspectRatio="{{model.maintainAspect ? 'xMidYMid meet' : 'none'}}">
    <circle cx="50" cy="50" r="40" fill="purple"/>
</svg>
```

### Responsive Image Fitting

```html
<!-- Model: { images: [{url:"photo.jpg", fit:"meet"}, {url:"banner.jpg", fit:"slice"}] } -->
<template data-bind="{{model.images}}">
    <svg width="300pt" height="200pt" viewBox="0 0 400 300"
         preserveAspectRatio="xMidYMid {{.fit}}">
        <image href="{{.url}}" width="400" height="300"/>
    </svg>
</template>
```

---

## Notes

### How Aspect Ratio Preservation Works

When the viewBox and viewport have different aspect ratios, `preserveAspectRatio` controls the behavior:

**Example: Wide viewport, square viewBox**
```html
<!-- Viewport: 300×100 (3:1), ViewBox: 100×100 (1:1) -->
<svg width="300pt" height="100pt" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid meet">
    <!-- Content maintains square aspect, centered with space on sides -->
</svg>
```

### Meet vs Slice

**meet**: Scales to show entire viewBox (letterbox/pillarbox effect)
- Entire content visible
- May have empty space in viewport
- Default behavior
- Best for: logos, icons, complete diagrams

**slice**: Scales to fill viewport (crops content)
- No empty space in viewport
- Content may be cropped
- Best for: background images, hero sections

```html
<!-- meet: Shows full circle with space on sides -->
<svg width="300pt" height="100pt" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid meet">
    <circle cx="50" cy="50" r="45" fill="blue"/>
</svg>

<!-- slice: Fills height, crops left/right of circle -->
<svg width="300pt" height="100pt" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid slice">
    <circle cx="50" cy="50" r="45" fill="blue"/>
</svg>
```

### Alignment Reference

The align value uses a coordinate system:
- **X**: `Min` (left), `Mid` (center), `Max` (right)
- **Y**: `Min` (top), `Mid` (middle), `Max` (bottom)

```
┌─────────────────┐
│ xMinYMin        │ (top-left)
│        xMidYMin │ (top-center)
│        xMaxYMin │ (top-right)
│                 │
│ xMinYMid        │ (middle-left)
│        xMidYMid │ (center)
│        xMaxYMid │ (middle-right)
│                 │
│ xMinYMax        │ (bottom-left)
│        xMidYMax │ (bottom-center)
│        xMaxYMax │ (bottom-right)
└─────────────────┘
```

### None Value

When `preserveAspectRatio="none"`:
- ViewBox is stretched to exactly fill viewport
- Aspect ratio is not preserved
- Can cause distortion of circles, squares, and text
- Useful for patterns that should stretch

```html
<!-- Circle becomes ellipse when stretched -->
<svg width="300pt" height="100pt" viewBox="0 0 100 100" preserveAspectRatio="none">
    <circle cx="50" cy="50" r="40" fill="blue"/>  <!-- Renders as ellipse -->
</svg>
```

### Interaction with ViewBox

`preserveAspectRatio` only has effect when:
1. A `viewBox` attribute is present
2. The aspect ratio of viewBox differs from viewport
3. The value is not `none`

```html
<!-- No effect: no viewBox specified -->
<svg width="200pt" height="200pt" preserveAspectRatio="xMinYMin meet">

<!-- No effect: matching aspect ratios -->
<svg width="200pt" height="200pt" viewBox="0 0 100 100" preserveAspectRatio="xMinYMin meet">

<!-- Has effect: different aspect ratios -->
<svg width="300pt" height="100pt" viewBox="0 0 100 100" preserveAspectRatio="xMinYMin meet">
```

### Responsive Design Patterns

**Pattern 1: Centered Logo (Default)**
```html
<svg width="100%" height="100pt" viewBox="0 0 200 50" preserveAspectRatio="xMidYMid meet">
    <!-- Logo always centered, maintains aspect ratio -->
</svg>
```

**Pattern 2: Background Image (Fill)**
```html
<svg width="100%" height="100%" viewBox="0 0 1920 1080" preserveAspectRatio="xMidYMid slice">
    <!-- Background fills viewport, may crop edges -->
</svg>
```

**Pattern 3: Left-Aligned Content**
```html
<svg width="100%" height="300pt" viewBox="0 0 800 600" preserveAspectRatio="xMinYMin meet">
    <!-- Content aligns to top-left, scales proportionally -->
</svg>
```

### Common Use Cases

| Use Case | Recommended Setting | Reasoning |
|----------|-------------------|-----------|
| Logo | `xMidYMid meet` | Center and show completely |
| Icon | `xMidYMid meet` | Center and show completely |
| Background image | `xMidYMid slice` | Fill viewport, crop edges |
| Banner/hero | `xMidYMid slice` | Fill width, crop top/bottom |
| Chart/diagram | `xMidYMid meet` | Show all content |
| Pattern/texture | `none` | Stretch to fill |
| Profile photo | `xMidYMid slice` | Fill frame, crop edges |

### Performance

- No significant performance impact from different values
- `slice` may reduce rendering area (crops content)
- `meet` renders full viewBox content

---

## Examples

### Nine Alignment Positions

```html
<!-- Top row -->
<svg width="150pt" height="150pt" viewBox="0 0 100 50" preserveAspectRatio="xMinYMin meet"
     style="border: 1pt solid #ccc;">
    <rect width="100" height="50" fill="lightblue"/>
    <text x="50" y="28" text-anchor="middle" font-size="8">xMinYMin</text>
</svg>

<svg width="150pt" height="150pt" viewBox="0 0 100 50" preserveAspectRatio="xMidYMin meet"
     style="border: 1pt solid #ccc;">
    <rect width="100" height="50" fill="lightblue"/>
    <text x="50" y="28" text-anchor="middle" font-size="8">xMidYMin</text>
</svg>

<svg width="150pt" height="150pt" viewBox="0 0 100 50" preserveAspectRatio="xMaxYMin meet"
     style="border: 1pt solid #ccc;">
    <rect width="100" height="50" fill="lightblue"/>
    <text x="50" y="28" text-anchor="middle" font-size="8">xMaxYMin</text>
</svg>

<!-- Middle row -->
<svg width="150pt" height="150pt" viewBox="0 0 100 50" preserveAspectRatio="xMinYMid meet"
     style="border: 1pt solid #ccc;">
    <rect width="100" height="50" fill="lightgreen"/>
    <text x="50" y="28" text-anchor="middle" font-size="8">xMinYMid</text>
</svg>

<svg width="150pt" height="150pt" viewBox="0 0 100 50" preserveAspectRatio="xMidYMid meet"
     style="border: 1pt solid #ccc;">
    <rect width="100" height="50" fill="lightgreen"/>
    <text x="50" y="28" text-anchor="middle" font-size="8">xMidYMid</text>
</svg>

<svg width="150pt" height="150pt" viewBox="0 0 100 50" preserveAspectRatio="xMaxYMid meet"
     style="border: 1pt solid #ccc;">
    <rect width="100" height="50" fill="lightgreen"/>
    <text x="50" y="28" text-anchor="middle" font-size="8">xMaxYMid</text>
</svg>

<!-- Bottom row -->
<svg width="150pt" height="150pt" viewBox="0 0 100 50" preserveAspectRatio="xMinYMax meet"
     style="border: 1pt solid #ccc;">
    <rect width="100" height="50" fill="lightyellow"/>
    <text x="50" y="28" text-anchor="middle" font-size="8">xMinYMax</text>
</svg>

<svg width="150pt" height="150pt" viewBox="0 0 100 50" preserveAspectRatio="xMidYMax meet"
     style="border: 1pt solid #ccc;">
    <rect width="100" height="50" fill="lightyellow"/>
    <text x="50" y="28" text-anchor="middle" font-size="8">xMidYMax</text>
</svg>

<svg width="150pt" height="150pt" viewBox="0 0 100 50" preserveAspectRatio="xMaxYMax meet"
     style="border: 1pt solid #ccc;">
    <rect width="100" height="50" fill="lightyellow"/>
    <text x="50" y="28" text-anchor="middle" font-size="8">xMaxYMax</text>
</svg>
```

### Meet vs Slice Comparison

```html
<!-- meet: Full content visible, space on sides -->
<svg width="400pt" height="200pt" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid meet"
     style="border: 2pt solid blue;">
    <rect width="100" height="100" fill="lightblue"/>
    <circle cx="50" cy="50" r="45" fill="blue"/>
    <text x="50" y="55" text-anchor="middle" fill="white" font-size="12" font-weight="bold">MEET</text>
</svg>

<!-- slice: Viewport filled, content cropped -->
<svg width="400pt" height="200pt" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid slice"
     style="border: 2pt solid red;">
    <rect width="100" height="100" fill="lightcoral"/>
    <circle cx="50" cy="50" r="45" fill="red"/>
    <text x="50" y="55" text-anchor="middle" fill="white" font-size="12" font-weight="bold">SLICE</text>
</svg>
```

### None vs Preserved Aspect Ratio

```html
<!-- Preserved aspect ratio (circle stays circular) -->
<svg width="300pt" height="100pt" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid meet"
     style="border: 1pt solid #ccc;">
    <circle cx="50" cy="50" r="40" fill="green"/>
    <text x="50" y="95" text-anchor="middle" font-size="6">Preserved</text>
</svg>

<!-- No aspect ratio (circle becomes ellipse) -->
<svg width="300pt" height="100pt" viewBox="0 0 100 100" preserveAspectRatio="none"
     style="border: 1pt solid #ccc;">
    <circle cx="50" cy="50" r="40" fill="orange"/>
    <text x="50" y="95" text-anchor="middle" font-size="6">Stretched</text>
</svg>
```

### Logo Centering

```html
<svg width="500pt" height="150pt" viewBox="0 0 200 80" preserveAspectRatio="xMidYMid meet"
     style="background-color: #f0f0f0;">
    <!-- Logo content -->
    <rect x="10" y="10" width="30" height="60" fill="#336699"/>
    <rect x="50" y="20" width="30" height="40" fill="#4a90e2"/>
    <text x="90" y="50" font-family="Arial" font-size="24" font-weight="bold" fill="#336699">
        COMPANY
    </text>
</svg>
```

### Hero Banner with Slice

```html
<svg width="100%" height="400pt" viewBox="0 0 1920 600" preserveAspectRatio="xMidYMid slice">
    <!-- Background gradient -->
    <defs>
        <linearGradient id="hero-gradient" x1="0%" y1="0%" x2="100%" y2="100%">
            <stop offset="0%" style="stop-color:#667eea;stop-opacity:1" />
            <stop offset="100%" style="stop-color:#764ba2;stop-opacity:1" />
        </linearGradient>
    </defs>

    <rect width="1920" height="600" fill="url(#hero-gradient)"/>

    <!-- Centered content -->
    <text x="960" y="280" text-anchor="middle" font-size="72" font-weight="bold" fill="white">
        Welcome to Our Site
    </text>
    <text x="960" y="350" text-anchor="middle" font-size="36" fill="white" opacity="0.9">
        Discover amazing things
    </text>
</svg>
```

### Icon Set with Consistent Sizing

```html
<div style="display: flex; gap: 20pt;">
    <!-- All icons maintain aspect ratio and center -->
    <svg width="80pt" height="80pt" viewBox="0 0 24 24" preserveAspectRatio="xMidYMid meet"
         style="border: 1pt solid #ccc;">
        <circle cx="12" cy="12" r="10" fill="none" stroke="blue" stroke-width="2"/>
        <path d="M 8,12 L 11,15 L 16,9" fill="none" stroke="blue" stroke-width="2"/>
    </svg>

    <svg width="80pt" height="80pt" viewBox="0 0 24 24" preserveAspectRatio="xMidYMid meet"
         style="border: 1pt solid #ccc;">
        <circle cx="12" cy="12" r="10" fill="none" stroke="red" stroke-width="2"/>
        <path d="M 8,8 L 16,16 M 16,8 L 8,16" fill="none" stroke="red" stroke-width="2"/>
    </svg>

    <svg width="80pt" height="80pt" viewBox="0 0 24 24" preserveAspectRatio="xMidYMid meet"
         style="border: 1pt solid #ccc;">
        <circle cx="12" cy="12" r="10" fill="none" stroke="orange" stroke-width="2"/>
        <path d="M 12,7 L 12,13 M 12,16 L 12,17" fill="none" stroke="orange" stroke-width="2" stroke-linecap="round"/>
    </svg>
</div>
```

### Gallery Images with Different Ratios

```html
<!-- Portrait image with slice -->
<svg width="200pt" height="300pt" viewBox="0 0 400 600" preserveAspectRatio="xMidYMid slice"
     style="border: 2pt solid #ccc;">
    <rect width="400" height="600" fill="linear-gradient(to bottom, #ffecd2, #fcb69f)"/>
    <text x="200" y="310" text-anchor="middle" font-size="36" fill="white">Portrait</text>
</svg>

<!-- Landscape image with slice -->
<svg width="400pt" height="200pt" viewBox="0 0 800 400" preserveAspectRatio="xMidYMid slice"
     style="border: 2pt solid #ccc;">
    <rect width="800" height="400" fill="linear-gradient(to right, #a8edea, #fed6e3)"/>
    <text x="400" y="215" text-anchor="middle" font-size="36" fill="white">Landscape</text>
</svg>

<!-- Square image with meet -->
<svg width="250pt" height="250pt" viewBox="0 0 500 500" preserveAspectRatio="xMidYMid meet"
     style="border: 2pt solid #ccc;">
    <rect width="500" height="500" fill="linear-gradient(135deg, #667eea, #764ba2)"/>
    <text x="250" y="265" text-anchor="middle" font-size="36" fill="white">Square</text>
</svg>
```

### Chart with Top Alignment

```html
<svg width="600pt" height="400pt" viewBox="0 0 800 500" preserveAspectRatio="xMidYMin meet"
     style="border: 1pt solid #ccc; background-color: white;">
    <!-- Title at top -->
    <text x="400" y="40" text-anchor="middle" font-size="32" font-weight="bold" fill="#333">
        Sales Report
    </text>

    <!-- Chart area -->
    <rect x="50" y="80" width="700" height="380" fill="#f9f9f9" stroke="#ddd" stroke-width="2"/>

    <!-- Bars -->
    <rect x="100" y="250" width="80" height="180" fill="#4a90e2"/>
    <rect x="250" y="180" width="80" height="250" fill="#50c878"/>
    <rect x="400" y="200" width="80" height="230" fill="#ff9900"/>
    <rect x="550" y="150" width="80" height="280" fill="#e74c3c"/>
</svg>
```

### Profile Photo Circle Crop

```html
<svg width="200pt" height="200pt" viewBox="0 0 200 200" preserveAspectRatio="xMidYMid slice">
    <defs>
        <clipPath id="circle-clip">
            <circle cx="100" cy="100" r="95"/>
        </clipPath>
    </defs>

    <!-- Image that fills circle (would be actual image) -->
    <rect width="200" height="200" fill="linear-gradient(135deg, #667eea, #764ba2)" clip-path="url(#circle-clip)"/>

    <!-- Border -->
    <circle cx="100" cy="100" r="95" fill="none" stroke="white" stroke-width="6"/>
    <circle cx="100" cy="100" r="95" fill="none" stroke="#ccc" stroke-width="2"/>
</svg>
```

### Card with Image Background

```html
<svg width="350pt" height="200pt" viewBox="0 0 350 200" preserveAspectRatio="xMidYMid slice"
     style="border-radius: 10pt; overflow: hidden;">
    <!-- Background image (simulated) -->
    <rect width="350" height="200" fill="linear-gradient(to bottom right, #f093fb, #f5576c)"/>

    <!-- Overlay -->
    <rect width="350" height="200" fill="black" opacity="0.3"/>

    <!-- Content -->
    <text x="175" y="100" text-anchor="middle" font-size="36" font-weight="bold" fill="white">
        Card Title
    </text>
    <text x="175" y="135" text-anchor="middle" font-size="18" fill="white" opacity="0.9">
        Subtitle text goes here
    </text>
</svg>
```

### Map with Corner Alignment

```html
<!-- Top-left aligned map -->
<svg width="400pt" height="300pt" viewBox="0 0 1000 800" preserveAspectRatio="xMinYMin meet"
     style="border: 2pt solid #336699;">
    <rect width="1000" height="800" fill="#e0f2f7"/>

    <!-- Map markers -->
    <circle cx="200" cy="150" r="20" fill="red"/>
    <text x="200" y="190" text-anchor="middle" font-size="24">City A</text>

    <circle cx="500" cy="300" r="20" fill="red"/>
    <text x="500" y="340" text-anchor="middle" font-size="24">City B</text>

    <circle cx="800" cy="200" r="20" fill="red"/>
    <text x="800" y="240" text-anchor="middle" font-size="24">City C</text>
</svg>
```

### Testimonial Card

```html
<svg width="100%" height="250pt" viewBox="0 0 600 250" preserveAspectRatio="xMidYMid meet">
    <!-- Card background -->
    <rect x="50" y="25" width="500" height="200" rx="10" fill="white" stroke="#e0e0e0" stroke-width="2"/>

    <!-- Quote mark -->
    <text x="80" y="80" font-size="48" fill="#4a90e2" opacity="0.3">"</text>

    <!-- Quote text -->
    <text x="300" y="110" text-anchor="middle" font-size="16" fill="#333">
        "This is an amazing product that has"
    </text>
    <text x="300" y="135" text-anchor="middle" font-size="16" fill="#333">
        "transformed our business operations."
    </text>

    <!-- Author -->
    <text x="300" y="180" text-anchor="middle" font-size="14" font-weight="bold" fill="#336699">
        — John Smith, CEO
    </text>
</svg>
```

### Pattern that Stretches

```html
<svg width="500pt" height="200pt" viewBox="0 0 100 100" preserveAspectRatio="none">
    <!-- Pattern stretches to fill -->
    <defs>
        <pattern id="stripe-pattern" x="0" y="0" width="10" height="10" patternUnits="userSpaceOnUse">
            <rect width="5" height="10" fill="#4a90e2"/>
            <rect x="5" width="5" height="10" fill="#e3f2fd"/>
        </pattern>
    </defs>

    <rect width="100" height="100" fill="url(#stripe-pattern)"/>
</svg>
```

### Video Player Thumbnail

```html
<!-- 16:9 video thumbnail in various containers -->
<svg width="400pt" height="300pt" viewBox="0 0 1920 1080" preserveAspectRatio="xMidYMid meet"
     style="background-color: black;">
    <!-- Video content area -->
    <rect width="1920" height="1080" fill="linear-gradient(135deg, #667eea, #764ba2)"/>

    <!-- Play button -->
    <circle cx="960" cy="540" r="120" fill="white" opacity="0.9"/>
    <polygon points="900,480 900,600 1050,540" fill="black"/>
</svg>
```

### Infographic Section

```html
<svg width="100%" height="500pt" viewBox="0 0 1200 500" preserveAspectRatio="xMidYMid meet">
    <!-- Background -->
    <rect width="1200" height="500" fill="#f9f9f9"/>

    <!-- Three columns -->
    <g id="column1">
        <circle cx="200" cy="150" r="80" fill="#4a90e2"/>
        <text x="200" y="160" text-anchor="middle" font-size="48" font-weight="bold" fill="white">1</text>
        <text x="200" y="280" text-anchor="middle" font-size="24" font-weight="bold" fill="#333">
            Step One
        </text>
        <text x="200" y="320" text-anchor="middle" font-size="16" fill="#666">
            Description here
        </text>
    </g>

    <g id="column2">
        <circle cx="600" cy="150" r="80" fill="#50c878"/>
        <text x="600" y="160" text-anchor="middle" font-size="48" font-weight="bold" fill="white">2</text>
        <text x="600" y="280" text-anchor="middle" font-size="24" font-weight="bold" fill="#333">
            Step Two
        </text>
        <text x="600" y="320" text-anchor="middle" font-size="16" fill="#666">
            Description here
        </text>
    </g>

    <g id="column3">
        <circle cx="1000" cy="150" r="80" fill="#ff9900"/>
        <text x="1000" y="160" text-anchor="middle" font-size="48" font-weight="bold" fill="white">3</text>
        <text x="1000" y="280" text-anchor="middle" font-size="24" font-weight="bold" fill="#333">
            Step Three
        </text>
        <text x="1000" y="320" text-anchor="middle" font-size="16" fill="#666">
            Description here
        </text>
    </g>
</svg>
```

### Dynamic Fit Mode

```html
<!-- Model: { displayMode: "contain" } -->
<!-- displayMode can be "contain" (meet) or "cover" (slice) -->
<svg width="400pt" height="300pt" viewBox="0 0 800 800"
     preserveAspectRatio="xMidYMid {{model.displayMode == 'contain' ? 'meet' : 'slice'}}"
     style="border: 2pt solid #336699;">
    <rect width="800" height="800" fill="lightblue"/>
    <circle cx="400" cy="400" r="300" fill="blue"/>
    <text x="400" y="420" text-anchor="middle" font-size="48" fill="white" font-weight="bold">
        {{model.displayMode}}
    </text>
</svg>
```

### Mobile vs Desktop Layout

```html
<!-- Model: { isMobile: false } -->
<svg width="100%" height="400pt"
     viewBox="0 0 {{model.isMobile ? 400 : 1200}} 400"
     preserveAspectRatio="xMidYMid meet">
    <!-- Header -->
    <rect width="{{model.isMobile ? 400 : 1200}}" height="80" fill="#336699"/>
    <text x="{{model.isMobile ? 200 : 600}}" y="50" text-anchor="middle"
          font-size="32" fill="white" font-weight="bold">
        {{model.isMobile ? 'Mobile' : 'Desktop'}} View
    </text>

    <!-- Content area -->
    <rect y="80" width="{{model.isMobile ? 400 : 1200}}" height="320" fill="#f0f0f0"/>
</svg>
```

---

## See Also

- [viewBox](/reference/svgattributes/viewBox.html) - SVG viewport and coordinate system
- [svg](/reference/svgtags/svg.html) - SVG canvas element
- [width and height](/reference/htmlattributes/width-height.html) - Viewport dimensions
- [image](/reference/svgtags/image.html) - Embedded image element
- [symbol](/reference/svgtags/symbol.html) - Symbol definition
- [Data Binding](/reference/binding/) - Dynamic data binding
- [SVG Coordinate Systems](https://www.w3.org/TR/SVG2/coords.html) - W3C specification

---
