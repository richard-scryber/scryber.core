---
layout: default
title: viewBox
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @viewBox : The SVG Viewport and Coordinate System Attribute

The `viewBox` attribute defines the coordinate system and aspect ratio for an SVG canvas. It establishes which portion of the SVG's coordinate space is visible within the viewport, enabling responsive scaling, zooming, and panning effects without modifying the actual SVG content.

## Usage

The `viewBox` attribute controls the SVG coordinate system and viewport:
- Define custom coordinate systems independent of physical dimensions
- Enable responsive SVG that scales with container size
- Create zoom and pan effects by adjusting the viewBox
- Maintain aspect ratios across different display sizes
- Control which portion of the SVG content is visible
- Support data binding for dynamic viewport adjustments
- Simplify coordinate calculations with custom units

```html
<!-- Basic viewBox: shows coordinates 0,0 to 100,100 -->
<svg width="200pt" height="200pt" viewBox="0 0 100 100">
    <circle cx="50" cy="50" r="40" fill="blue"/>
</svg>

<!-- Custom coordinate system -->
<svg width="300pt" height="200pt" viewBox="-150 -100 300 200">
    <circle cx="0" cy="0" r="80" fill="red"/>
</svg>

<!-- Dynamic viewBox for zoom/pan -->
<svg width="400pt" height="400pt" viewBox="{{model.x}} {{model.y}} {{model.width}} {{model.height}}">
    <rect x="0" y="0" width="100" height="100" fill="green"/>
</svg>
```

---

## Supported Values

The `viewBox` attribute takes four space-separated or comma-separated numbers:

### Syntax
```
viewBox="min-x min-y width height"
```

### Parameters

| Parameter | Description | Example |
|-----------|-------------|---------|
| `min-x` | The minimum X coordinate of the viewport | `0`, `-100`, `50` |
| `min-y` | The minimum Y coordinate of the viewport | `0`, `-100`, `50` |
| `width` | The width of the viewport in user units | `100`, `500`, `1000` |
| `height` | The height of the viewport in user units | `100`, `500`, `1000` |

### Format Variations

All of these formats are valid:

```html
<!-- Space-separated -->
<svg viewBox="0 0 100 100">

<!-- Comma-separated -->
<svg viewBox="0,0,100,100">

<!-- Mixed separators -->
<svg viewBox="0, 0, 100, 100">
```

### Common ViewBox Values

```html
<!-- Standard 100x100 coordinate system -->
viewBox="0 0 100 100"

<!-- Wide aspect ratio -->
viewBox="0 0 200 100"

<!-- Centered coordinate system -->
viewBox="-50 -50 100 100"

<!-- Large coordinate space -->
viewBox="0 0 1000 1000"
```

---

## Supported Elements

The `viewBox` attribute is supported on:

### Primary Element
- `<svg>` - The main SVG canvas element

### Container Elements
- `<symbol>` - Symbol definition (for reusable graphics)
- `<pattern>` - Pattern definition
- `<marker>` - Marker definition

**Note:** The `viewBox` is most commonly used on the root `<svg>` element to define the entire canvas coordinate system.

---

## Data Binding

The `viewBox` attribute supports data binding for dynamic viewport control:

### Dynamic Zoom and Pan

```html
<!-- Model: { x: 0, y: 0, width: 100, height: 100 } -->
<svg width="400pt" height="400pt"
     viewBox="{{model.x}} {{model.y}} {{model.width}} {{model.height}}">
    <rect x="10" y="10" width="80" height="80" fill="blue"/>
    <circle cx="50" cy="50" r="30" fill="red"/>
</svg>
```

### Zoom Effect

```html
<!-- Model: { zoomLevel: 1 } -->
<!-- zoomLevel: 1 = normal, 2 = 2x zoom, 0.5 = zoom out -->
<svg width="300pt" height="300pt"
     viewBox="{{50 - (50 / model.zoomLevel)}} {{50 - (50 / model.zoomLevel)}} {{100 / model.zoomLevel}} {{100 / model.zoomLevel}}">
    <rect x="0" y="0" width="100" height="100" fill="#e0e0e0"/>
    <circle cx="50" cy="50" r="20" fill="blue"/>
    <text x="50" y="55" text-anchor="middle" fill="white" font-size="8">Target</text>
</svg>
```

### Pan Effect

```html
<!-- Model: { panX: 0, panY: 0 } -->
<svg width="400pt" height="300pt"
     viewBox="{{model.panX}} {{model.panY}} 200 150">
    <rect x="0" y="0" width="300" height="200" fill="#f0f0f0"/>
    <circle cx="50" cy="50" r="20" fill="red"/>
    <circle cx="150" cy="100" r="20" fill="green"/>
    <circle cx="250" cy="50" r="20" fill="blue"/>
</svg>
```

### Responsive ViewBox

```html
<!-- Model: { contentWidth: 800, contentHeight: 600 } -->
<svg width="100%" height="400pt"
     viewBox="0 0 {{model.contentWidth}} {{model.contentHeight}}"
     preserveAspectRatio="xMidYMid meet">
    <rect x="0" y="0" width="{{model.contentWidth}}" height="{{model.contentHeight}}" fill="#f9f9f9"/>
    <text x="{{model.contentWidth / 2}}" y="{{model.contentHeight / 2}}"
          text-anchor="middle" font-size="48" fill="#333">Content</text>
</svg>
```

### Focus on Region

```html
<!-- Model: { focusRegion: { x: 100, y: 100, width: 200, height: 200 } } -->
<svg width="400pt" height="400pt"
     viewBox="{{model.focusRegion.x}} {{model.focusRegion.y}} {{model.focusRegion.width}} {{model.focusRegion.height}}">
    <rect x="0" y="0" width="500" height="500" fill="#e0e0e0"/>
    <rect x="100" y="100" width="200" height="200" fill="yellow" stroke="orange" stroke-width="3"/>
    <text x="200" y="210" text-anchor="middle" font-size="24" font-weight="bold">Focus Area</text>
</svg>
```

### Animated Viewport Transition

```html
<!-- Model: { currentView: 0, views: [{x:0,y:0,w:100,h:100}, {x:50,y:50,w:50,h:50}] } -->
<svg width="400pt" height="400pt"
     viewBox="{{model.views[model.currentView].x}} {{model.views[model.currentView].y}} {{model.views[model.currentView].w}} {{model.views[model.currentView].h}}">
    <rect x="0" y="0" width="100" height="100" fill="#f0f0f0"/>
    <rect x="25" y="25" width="50" height="50" fill="blue"/>
    <rect x="50" y="50" width="25" height="25" fill="red"/>
</svg>
```

---

## Notes

### How ViewBox Works

The viewBox establishes a mapping between:
1. **SVG Coordinate Space**: The internal coordinate system (defined by viewBox)
2. **Viewport**: The physical display area (defined by width/height attributes)

```html
<!-- Viewport is 200pt × 200pt, coordinates are 0-100 -->
<svg width="200pt" height="200pt" viewBox="0 0 100 100">
    <!-- This circle at (50,50) with radius 25 appears centered -->
    <circle cx="50" cy="50" r="25"/>
</svg>
```

### Scaling Behavior

The viewBox automatically scales content to fit the viewport:

```html
<!-- Case 1: ViewBox matches viewport (1:1 scaling) -->
<svg width="100pt" height="100pt" viewBox="0 0 100 100">
    <rect width="50" height="50" fill="blue"/>  <!-- 50pt × 50pt -->
</svg>

<!-- Case 2: ViewBox half of viewport (2x scaling) -->
<svg width="100pt" height="100pt" viewBox="0 0 50 50">
    <rect width="25" height="25" fill="blue"/>  <!-- 50pt × 50pt (2x scaled) -->
</svg>

<!-- Case 3: ViewBox double viewport (0.5x scaling) -->
<svg width="100pt" height="100pt" viewBox="0 0 200 200">
    <rect width="100" height="100" fill="blue"/>  <!-- 50pt × 50pt (0.5x scaled) -->
</svg>
```

### Negative Coordinates

ViewBox can use negative coordinates to center the origin:

```html
<!-- Origin at top-left (standard) -->
<svg viewBox="0 0 100 100">
    <circle cx="50" cy="50" r="20"/>  <!-- Center is at 50,50 -->
</svg>

<!-- Origin at center (using negative min-x, min-y) -->
<svg viewBox="-50 -50 100 100">
    <circle cx="0" cy="0" r="20"/>  <!-- Center is at 0,0 -->
</svg>
```

### Aspect Ratio

When viewBox aspect ratio differs from viewport aspect ratio, use `preserveAspectRatio` to control behavior:

```html
<!-- Wide viewBox in square viewport -->
<svg width="200pt" height="200pt" viewBox="0 0 200 100" preserveAspectRatio="xMidYMid meet">
    <rect width="200" height="100" fill="blue"/>
</svg>
```

See the `preserveAspectRatio` attribute documentation for details.

### Units

- ViewBox values are unitless (user units)
- Viewport (width/height) can use any CSS units (pt, px, %, em, etc.)
- Scaling is calculated automatically between the two systems

```html
<!-- User units 0-100 map to physical 400pt × 300pt -->
<svg width="400pt" height="300pt" viewBox="0 0 100 100">
    <!-- 1 user unit = 4pt horizontally, 3pt vertically (non-uniform scaling) -->
</svg>
```

### Responsive SVG

ViewBox is key to creating responsive, scalable SVG:

```html
<!-- SVG scales with container, maintains 16:9 aspect ratio -->
<svg width="100%" height="100%" viewBox="0 0 1600 900" preserveAspectRatio="xMidYMid meet">
    <rect x="0" y="0" width="1600" height="900" fill="#f0f0f0"/>
    <text x="800" y="450" text-anchor="middle" font-size="100">Responsive</text>
</svg>
```

### ViewBox vs Width/Height

| Attribute | Purpose | Units | Effect |
|-----------|---------|-------|--------|
| `width`, `height` | Physical viewport size | CSS units (pt, px, %) | Display dimensions |
| `viewBox` | Coordinate system | User units (unitless) | Internal coordinate space |

### Common Patterns

**Pattern 1: Fixed Coordinates, Scalable Display**
```html
<!-- Always work in 0-100 coordinates, scales to any display size -->
<svg width="100%" height="400pt" viewBox="0 0 100 100">
```

**Pattern 2: Match Viewport (No Scaling)**
```html
<!-- 1:1 pixel mapping -->
<svg width="500pt" height="500pt" viewBox="0 0 500 500">
```

**Pattern 3: Centered Origin**
```html
<!-- Origin at center, ±100 units in each direction -->
<svg width="400pt" height="400pt" viewBox="-100 -100 200 200">
```

**Pattern 4: Large Coordinate Space**
```html
<!-- Detailed canvas with many elements -->
<svg width="600pt" height="400pt" viewBox="0 0 1920 1080">
```

### Performance

- Larger viewBox values don't impact performance
- Use convenient coordinate systems (e.g., 0-100, 0-1000)
- Avoid extremely small or large viewBox values that may cause precision issues

---

## Examples

### Basic ViewBox Scaling

```html
<!-- Same circle, different viewBox values -->
<div style="display: flex; gap: 20pt;">
    <!-- Normal -->
    <svg width="100pt" height="100pt" viewBox="0 0 100 100" style="border: 1pt solid #ccc;">
        <circle cx="50" cy="50" r="40" fill="blue"/>
    </svg>

    <!-- Zoomed in (smaller viewBox = larger content) -->
    <svg width="100pt" height="100pt" viewBox="25 25 50 50" style="border: 1pt solid #ccc;">
        <circle cx="50" cy="50" r="40" fill="blue"/>
    </svg>

    <!-- Zoomed out (larger viewBox = smaller content) -->
    <svg width="100pt" height="100pt" viewBox="0 0 200 200" style="border: 1pt solid #ccc;">
        <circle cx="50" cy="50" r="40" fill="blue"/>
    </svg>
</div>
```

### Centered Coordinate System

```html
<svg width="300pt" height="300pt" viewBox="-150 -150 300 300">
    <!-- Axes -->
    <line x1="-150" y1="0" x2="150" y2="0" stroke="black" stroke-width="2"/>
    <line x1="0" y1="-150" x2="0" y2="150" stroke="black" stroke-width="2"/>

    <!-- Origin at center -->
    <circle cx="0" cy="0" r="10" fill="red"/>

    <!-- Quadrants -->
    <circle cx="80" cy="-80" r="30" fill="blue" opacity="0.5"/>
    <circle cx="80" cy="80" r="30" fill="green" opacity="0.5"/>
    <circle cx="-80" cy="80" r="30" fill="orange" opacity="0.5"/>
    <circle cx="-80" cy="-80" r="30" fill="purple" opacity="0.5"/>
</svg>
```

### Responsive Icon

```html
<!-- Scales to any size while maintaining detail -->
<svg width="50pt" height="50pt" viewBox="0 0 24 24">
    <path d="M12 2 L15 8 L22 9 L17 14 L18 21 L12 18 L6 21 L7 14 L2 9 L9 8 Z"
          fill="gold" stroke="orange" stroke-width="1"/>
</svg>

<svg width="100pt" height="100pt" viewBox="0 0 24 24">
    <path d="M12 2 L15 8 L22 9 L17 14 L18 21 L12 18 L6 21 L7 14 L2 9 L9 8 Z"
          fill="gold" stroke="orange" stroke-width="1"/>
</svg>

<svg width="200pt" height="200pt" viewBox="0 0 24 24">
    <path d="M12 2 L15 8 L22 9 L17 14 L18 21 L12 18 L6 21 L7 14 L2 9 L9 8 Z"
          fill="gold" stroke="orange" stroke-width="1"/>
</svg>
```

### Wide Aspect Ratio

```html
<svg width="600pt" height="200pt" viewBox="0 0 300 100">
    <!-- Background -->
    <rect width="300" height="100" fill="#e3f2fd"/>

    <!-- Timeline elements -->
    <circle cx="50" cy="50" r="15" fill="#2196f3"/>
    <circle cx="125" cy="50" r="15" fill="#2196f3"/>
    <circle cx="200" cy="50" r="15" fill="#2196f3"/>
    <circle cx="275" cy="50" r="15" fill="#2196f3"/>

    <!-- Connecting lines -->
    <line x1="65" y1="50" x2="110" y2="50" stroke="#2196f3" stroke-width="3"/>
    <line x1="140" y1="50" x2="185" y2="50" stroke="#2196f3" stroke-width="3"/>
    <line x1="215" y1="50" x2="260" y2="50" stroke="#2196f3" stroke-width="3"/>
</svg>
```

### Pan Effect - Showing Different Regions

```html
<!-- View left region -->
<svg width="300pt" height="200pt" viewBox="0 0 300 200">
    <rect x="0" y="0" width="900" height="200" fill="#f0f0f0"/>
    <rect x="50" y="50" width="200" height="100" fill="blue"/>
    <rect x="350" y="50" width="200" height="100" fill="green"/>
    <rect x="650" y="50" width="200" height="100" fill="red"/>
</svg>

<!-- View center region -->
<svg width="300pt" height="200pt" viewBox="300 0 300 200">
    <rect x="0" y="0" width="900" height="200" fill="#f0f0f0"/>
    <rect x="50" y="50" width="200" height="100" fill="blue"/>
    <rect x="350" y="50" width="200" height="100" fill="green"/>
    <rect x="650" y="50" width="200" height="100" fill="red"/>
</svg>

<!-- View right region -->
<svg width="300pt" height="200pt" viewBox="600 0 300 200">
    <rect x="0" y="0" width="900" height="200" fill="#f0f0f0"/>
    <rect x="50" y="50" width="200" height="100" fill="blue"/>
    <rect x="350" y="50" width="200" height="100" fill="green"/>
    <rect x="650" y="50" width="200" height="100" fill="red"/>
</svg>
```

### Zoom Sequence

```html
<!-- Zoom level 1: Full view -->
<svg width="200pt" height="200pt" viewBox="0 0 200 200">
    <rect x="0" y="0" width="200" height="200" fill="#e0e0e0"/>
    <rect x="50" y="50" width="100" height="100" fill="blue"/>
    <circle cx="100" cy="100" r="30" fill="red"/>
</svg>

<!-- Zoom level 2: 2x zoom -->
<svg width="200pt" height="200pt" viewBox="50 50 100 100">
    <rect x="0" y="0" width="200" height="200" fill="#e0e0e0"/>
    <rect x="50" y="50" width="100" height="100" fill="blue"/>
    <circle cx="100" cy="100" r="30" fill="red"/>
</svg>

<!-- Zoom level 3: 4x zoom on center -->
<svg width="200pt" height="200pt" viewBox="75 75 50 50">
    <rect x="0" y="0" width="200" height="200" fill="#e0e0e0"/>
    <rect x="50" y="50" width="100" height="100" fill="blue"/>
    <circle cx="100" cy="100" r="30" fill="red"/>
</svg>
```

### Map Viewport

```html
<!-- Show different regions of a map -->
<svg width="400pt" height="300pt" viewBox="0 0 1000 750">
    <!-- Full map content -->
    <rect x="0" y="0" width="1000" height="750" fill="#b3e5fc"/>

    <!-- Cities -->
    <circle cx="200" cy="200" r="20" fill="red"/>
    <text x="200" y="250" text-anchor="middle" font-size="30">City A</text>

    <circle cx="500" cy="400" r="20" fill="red"/>
    <text x="500" y="450" text-anchor="middle" font-size="30">City B</text>

    <circle cx="800" cy="300" r="20" fill="red"/>
    <text x="800" y="350" text-anchor="middle" font-size="30">City C</text>

    <!-- Roads -->
    <line x1="200" y1="200" x2="500" y2="400" stroke="gray" stroke-width="5"/>
    <line x1="500" y1="400" x2="800" y2="300" stroke="gray" stroke-width="5"/>
</svg>
```

### Graph with Custom Scale

```html
<svg width="500pt" height="300pt" viewBox="0 -100 100 100">
    <!-- Y-axis from -100 to 0 (inverted) -->

    <!-- Grid lines -->
    <line x1="0" y1="0" x2="100" y2="0" stroke="#ccc" stroke-width="0.5"/>
    <line x1="0" y1="-25" x2="100" y2="-25" stroke="#ccc" stroke-width="0.5"/>
    <line x1="0" y1="-50" x2="100" y2="-50" stroke="#ccc" stroke-width="0.5"/>
    <line x1="0" y1="-75" x2="100" y2="-75" stroke="#ccc" stroke-width="0.5"/>
    <line x1="0" y1="-100" x2="100" y2="-100" stroke="#ccc" stroke-width="0.5"/>

    <!-- Data line -->
    <polyline points="0,0 10,-20 20,-35 30,-55 40,-65 50,-75 60,-70 70,-60 80,-45 90,-30 100,-20"
              fill="none" stroke="blue" stroke-width="2"/>

    <!-- Axes -->
    <line x1="0" y1="0" x2="100" y2="0" stroke="black" stroke-width="1"/>
    <line x1="0" y1="0" x2="0" y2="-100" stroke="black" stroke-width="1"/>
</svg>
```

### Normalized Coordinates (0 to 1)

```html
<svg width="400pt" height="400pt" viewBox="0 0 1 1">
    <!-- Using decimal coordinates from 0 to 1 -->
    <rect x="0.1" y="0.1" width="0.8" height="0.8" fill="#e0e0e0"/>
    <circle cx="0.5" cy="0.5" r="0.3" fill="blue"/>
    <rect x="0.4" y="0.4" width="0.2" height="0.2" fill="red"/>
</svg>
```

### Circular Crop

```html
<svg width="200pt" height="200pt" viewBox="0 0 200 200">
    <defs>
        <clipPath id="circle-clip">
            <circle cx="100" cy="100" r="90"/>
        </clipPath>
    </defs>

    <!-- Content clipped to circle -->
    <g clip-path="url(#circle-clip)">
        <rect x="0" y="0" width="200" height="200" fill="lightblue"/>
        <circle cx="50" cy="50" r="30" fill="red"/>
        <circle cx="150" cy="50" r="30" fill="green"/>
        <circle cx="100" cy="150" r="30" fill="blue"/>
        <text x="100" y="110" text-anchor="middle" font-size="24" font-weight="bold">Clipped</text>
    </g>

    <!-- Border circle -->
    <circle cx="100" cy="100" r="90" fill="none" stroke="black" stroke-width="4"/>
</svg>
```

### Dashboard Gauge with ViewBox

```html
<svg width="300pt" height="200pt" viewBox="-100 -100 200 100" preserveAspectRatio="xMidYMid meet">
    <!-- Semi-circular gauge -->
    <path d="M -90,0 A 90,90 0 0,1 90,0" fill="none" stroke="#e0e0e0" stroke-width="15"/>
    <path d="M -90,0 A 90,90 0 0,1 0,-90" fill="none" stroke="#4a90e2" stroke-width="15"/>

    <!-- Needle -->
    <line x1="0" y1="0" x2="0" y2="-70" stroke="red" stroke-width="3" transform="rotate(-45)"/>
    <circle cx="0" cy="0" r="5" fill="red"/>

    <!-- Labels -->
    <text x="-90" y="20" text-anchor="middle" font-size="12">0</text>
    <text x="0" y="-95" text-anchor="middle" font-size="12">50</text>
    <text x="90" y="20" text-anchor="middle" font-size="12">100</text>
</svg>
```

### Dynamic Zoom to Element

```html
<!-- Model: { selectedElement: { x: 200, y: 150, width: 100, height: 100 } } -->
<svg width="400pt" height="300pt"
     viewBox="{{model.selectedElement.x - 20}} {{model.selectedElement.y - 20}} {{model.selectedElement.width + 40}} {{model.selectedElement.height + 40}}">
    <!-- Full content -->
    <rect x="0" y="0" width="600" height="400" fill="#f0f0f0"/>
    <rect x="50" y="50" width="100" height="100" fill="blue"/>
    <rect x="200" y="150" width="100" height="100" fill="red"/>
    <rect x="400" y="100" width="100" height="100" fill="green"/>
</svg>
```

### Minimap Overview

```html
<!-- Main view -->
<svg width="500pt" height="400pt" viewBox="200 150 300 250">
    <rect x="0" y="0" width="800" height="600" fill="#e3f2fd"/>
    <!-- Content elements -->
    <rect x="100" y="100" width="150" height="150" fill="blue"/>
    <rect x="400" y="200" width="100" height="200" fill="green"/>
    <rect x="600" y="100" width="150" height="100" fill="red"/>

    <!-- Viewport indicator -->
    <rect x="200" y="150" width="300" height="250" fill="none" stroke="orange" stroke-width="5"/>
</svg>

<!-- Minimap showing full content -->
<svg width="200pt" height="150pt" viewBox="0 0 800 600">
    <rect x="0" y="0" width="800" height="600" fill="#e3f2fd" opacity="0.5"/>
    <rect x="100" y="100" width="150" height="150" fill="blue" opacity="0.7"/>
    <rect x="400" y="200" width="100" height="200" fill="green" opacity="0.7"/>
    <rect x="600" y="100" width="150" height="100" fill="red" opacity="0.7"/>

    <!-- Viewport indicator -->
    <rect x="200" y="150" width="300" height="250" fill="yellow" opacity="0.3" stroke="orange" stroke-width="4"/>
</svg>
```

### Focus + Context

```html
<!-- Context view: full diagram -->
<svg width="200pt" height="150pt" viewBox="0 0 400 300">
    <rect width="400" height="300" fill="#f9f9f9"/>
    <circle cx="100" cy="150" r="30" fill="blue"/>
    <circle cx="200" cy="150" r="30" fill="red"/>
    <circle cx="300" cy="150" r="30" fill="green"/>
</svg>

<!-- Focus view: zoom on middle circle -->
<svg width="400pt" height="300pt" viewBox="160 110 80 80">
    <rect width="400" height="300" fill="#f9f9f9"/>
    <circle cx="100" cy="150" r="30" fill="blue"/>
    <circle cx="200" cy="150" r="30" fill="red"/>
    <circle cx="300" cy="150" r="30" fill="green"/>
</svg>
```

### Scrollable Content

```html
<!-- Viewing first section of long content -->
<svg width="500pt" height="300pt" viewBox="0 0 500 300">
    <rect x="0" y="0" width="500" height="1000" fill="#f0f0f0"/>
    <text x="250" y="100" text-anchor="middle" font-size="48">Section 1</text>
    <text x="250" y="400" text-anchor="middle" font-size="48">Section 2</text>
    <text x="250" y="700" text-anchor="middle" font-size="48">Section 3</text>
</svg>

<!-- Viewing second section -->
<svg width="500pt" height="300pt" viewBox="0 300 500 300">
    <rect x="0" y="0" width="500" height="1000" fill="#f0f0f0"/>
    <text x="250" y="100" text-anchor="middle" font-size="48">Section 1</text>
    <text x="250" y="400" text-anchor="middle" font-size="48">Section 2</text>
    <text x="250" y="700" text-anchor="middle" font-size="48">Section 3</text>
</svg>
```

### Animated Pan Through Scene

```html
<!-- Model: { frame: 0 } -->
<!-- frame animates from 0 to 400 to pan across scene -->
<svg width="400pt" height="300pt" viewBox="{{model.frame}} 0 400 300">
    <rect x="0" y="0" width="1200" height="300" fill="linear-gradient(to right, #87ceeb, #ffffff, #ffb347)"/>

    <!-- Scene elements -->
    <circle cx="100" cy="150" r="40" fill="yellow"/>  <!-- Sun -->
    <rect x="300" y="200" width="60" height="100" fill="brown"/>  <!-- Tree trunk -->
    <circle cx="330" cy="180" r="50" fill="green"/>  <!-- Tree -->
    <rect x="700" y="220" width="100" height="80" fill="red"/>  <!-- House -->
    <polygon points="750,220 700,160 800,160" fill="darkred"/>  <!-- Roof -->
    <circle cx="1000" cy="150" r="50" fill="white"/>  <!-- Cloud -->
</svg>
```

### Responsive Logo

```html
<svg width="100%" height="100pt" viewBox="0 0 200 50" preserveAspectRatio="xMinYMid meet">
    <rect x="0" y="0" width="40" height="50" fill="#336699"/>
    <rect x="50" y="10" width="40" height="30" fill="#4a90e2"/>
    <text x="100" y="35" font-family="Arial" font-size="24" font-weight="bold" fill="#336699">Company</text>
</svg>
```

### Grid System with ViewBox

```html
<svg width="600pt" height="600pt" viewBox="0 0 10 10">
    <!-- 10x10 grid system for easy layout -->
    <rect x="0" y="0" width="10" height="10" fill="#f0f0f0"/>

    <!-- Header: full width, 1 unit tall -->
    <rect x="0" y="0" width="10" height="1" fill="#336699"/>

    <!-- Sidebar: 2 units wide -->
    <rect x="0" y="1" width="2" height="8" fill="#e0e0e0"/>

    <!-- Main content: 8 units wide -->
    <rect x="2" y="1" width="8" height="8" fill="white"/>

    <!-- Footer: full width, 1 unit tall -->
    <rect x="0" y="9" width="10" height="1" fill="#666"/>
</svg>
```

---

## See Also

- [preserveAspectRatio](/reference/svgattributes/preserveAspectRatio.html) - Aspect ratio control
- [svg](/reference/svgtags/svg.html) - SVG canvas element
- [width and height](/reference/htmlattributes/width-height.html) - Viewport dimensions
- [transform](/reference/svgattributes/transform.html) - Transform attribute
- [Data Binding](/reference/binding/) - Dynamic data binding
- [SVG Coordinate Systems](https://www.w3.org/TR/SVG2/coords.html) - W3C specification

---
