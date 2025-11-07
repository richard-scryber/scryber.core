---
layout: default
title: svg
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;svg&gt; : The SVG Canvas Container

The `<svg>` element is the root container for Scalable Vector Graphics (SVG) content in PDF documents. It creates a self-contained graphics canvas that can contain shapes, paths, text, images, and other SVG elements. The SVG canvas supports coordinate systems, viewports, transformations, and reusable definitions.

## Usage

The `<svg>` element creates a vector graphics container that:
- Defines a coordinate system and viewport for vector graphics rendering
- Supports all SVG shape elements (rect, circle, path, line, etc.)
- Can be embedded inline in HTML content or used as a standalone graphic
- Scales perfectly in PDF output without quality loss
- Supports the `<defs>` element for reusable graphics definitions
- Handles transformations, gradients, and patterns
- Can be data-bound for dynamic graphics generation

```html
<svg width="200pt" height="100pt" viewBox="0 0 200 100">
    <rect x="10" y="10" width="180" height="80"
          fill="#336699" stroke="#000" stroke-width="2"/>
    <text x="100" y="55" text-anchor="middle" fill="white" font-size="20">
        SVG in PDF
    </text>
</svg>
```

---

## Supported Attributes

### Dimensional Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `width` | Unit | Width of the SVG canvas. Default: 300pt (when not embedded) |
| `height` | Unit | Height of the SVG canvas. Default: 150pt (when not embedded) |
| `x` | Unit | Horizontal position when SVG is nested or positioned |
| `y` | Unit | Vertical position when SVG is nested or positioned |

### Viewport Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `viewBox` | string | Defines the coordinate system: "min-x min-y width height". Controls scaling and aspect ratio. |
| `preserveAspectRatio` | string | Controls how the viewBox is scaled to fit the viewport. Values: `none`, `xMinYMin`, `xMidYMid` (default), `xMaxYMax`, etc. |

### Standard Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the SVG canvas. Required for internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS/SVG styles applied to the canvas and inherited by children. |
| `title` | string | Title for accessibility and outline/bookmark generation. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide, or omit to show. |
| `xmlns` | string | XML namespace (typically "http://www.w3.org/2000/svg"). Usually auto-applied. |

### Graphics Attributes (Inherited by Children)

| Attribute | Type | Description |
|-----------|------|-------------|
| `fill` | color | Default fill color for child shapes. Values: color names, hex (#RGB, #RRGGBB), `none` |
| `fill-opacity` | number | Opacity for fills (0.0 to 1.0). Default: 1.0 |
| `stroke` | color | Default stroke color for child shapes |
| `stroke-width` | Unit | Default stroke width. Default: 1pt |
| `stroke-opacity` | number | Opacity for strokes (0.0 to 1.0). Default: 1.0 |
| `stroke-linecap` | string | Line cap style: `butt`, `round`, `square` |
| `stroke-linejoin` | string | Line join style: `miter`, `round`, `bevel` |
| `stroke-dasharray` | string | Dash pattern for strokes: "5,5" for dashed, "10,5,2,5" for complex patterns |

### Content Elements

| Element | Description |
|---------|-------------|
| `<defs>` | Container for reusable definitions (gradients, patterns, symbols, shapes) |
| Content | Any SVG shape, text, group, or container elements |

---

## Notes

### SVG Canvas Behavior

1. **Default Size**: When used inline in HTML, defaults to 300pt x 150pt if no dimensions specified
2. **Inline Display**: Renders as `inline-block` by default, flows with surrounding content
3. **Coordinate System**: Uses its own coordinate space, independent of PDF page coordinates
4. **Clipping**: Content outside the SVG bounds is automatically clipped
5. **Never Splits**: SVG canvases never split across pages (overflow-split: never)

### ViewBox and Coordinate Systems

The `viewBox` attribute defines the user coordinate system:

```html
<!-- viewBox="min-x min-y width height" -->
<svg width="200pt" height="100pt" viewBox="0 0 400 200">
    <!-- Canvas is 200pt wide, but coordinates use 0-400 scale -->
    <!-- Graphics are scaled down 2x to fit -->
</svg>
```

**Key concepts**:
- The `width`/`height` define the canvas size in the document
- The `viewBox` defines the internal coordinate system
- Content is scaled to map viewBox coordinates to canvas dimensions
- Useful for creating resolution-independent graphics

### Preserve Aspect Ratio

The `preserveAspectRatio` attribute controls scaling behavior:

- **`none`**: Stretch to fill, ignoring aspect ratio
- **`xMidYMid meet`** (default): Scale uniformly, centered, entire viewBox visible
- **`xMidYMid slice`**: Scale uniformly, centered, fill entire viewport
- **Alignment**: `xMin`, `xMid`, `xMax` with `YMin`, `YMid`, `YMax`
- **Meet/Slice**: `meet` (show all), `slice` (fill viewport)

### Definitions Container

The `<defs>` element inside `<svg>` holds reusable content:

```html
<svg width="200pt" height="200pt">
    <defs>
        <linearGradient id="grad1">
            <stop offset="0%" stop-color="blue"/>
            <stop offset="100%" stop-color="red"/>
        </linearGradient>
        <circle id="dot" r="5" fill="black"/>
    </defs>
    <rect width="200" height="200" fill="url(#grad1)"/>
    <use href="#dot" x="50" y="50"/>
</svg>
```

### Nested SVG

SVG elements can be nested for complex compositions:

```html
<svg width="400pt" height="200pt">
    <svg x="0" y="0" width="200" height="200" viewBox="0 0 100 100">
        <!-- First graphic -->
    </svg>
    <svg x="200" y="0" width="200" height="200" viewBox="0 0 100 100">
        <!-- Second graphic -->
    </svg>
</svg>
```

### Style Application

Styles defined on the `<svg>` element cascade to children:

```html
<svg width="200pt" height="100pt" fill="blue" stroke="black" stroke-width="2">
    <!-- All child shapes inherit these styles unless overridden -->
    <circle cx="50" cy="50" r="30"/>
    <rect x="120" y="20" width="60" height="60" fill="red"/>
</svg>
```

### Class Hierarchy

In the Scryber codebase:
- `SVGCanvas` extends `Canvas` implements `IResourceContainer`, `ICanvas`, `INamingContainer`
- Maintains its own resource list for gradients, patterns, etc.
- Acts as naming container for ID-based references within the SVG
- Uses specialized `LayoutEngineSVG` for rendering

---

## Data Binding

### Dynamic SVG Generation

SVG content can be generated dynamically using data binding:

```html
<!-- Model: { items: [{x:50, y:50, r:20, color:"red"}, {x:150, y:50, r:30, color:"blue"}] } -->
<svg width="300pt" height="150pt" viewBox="0 0 300 150">
    <template data-bind="{{model.items}}">
        <circle cx="{{.x}}" cy="{{.y}}" r="{{.r}}" fill="{{.color}}"/>
    </template>
</svg>
```

### Parameterized Graphics

```html
<!-- Model: { chartData: { width: 400, height: 200, bars: [...] } } -->
<svg width="{{model.chartData.width}}pt" height="{{model.chartData.height}}pt">
    <template data-bind="{{model.chartData.bars}}">
        <rect x="{{.x}}" y="{{.y}}" width="{{.width}}" height="{{.height}}"
              fill="{{.color}}" opacity="{{.opacity}}"/>
    </template>
</svg>
```

### Conditional SVG Content

```html
<svg width="200pt" height="200pt">
    <rect width="200" height="200" fill="#f0f0f0"/>
    <g hidden="{{model.showDetails ? '' : 'hidden'}}">
        <circle cx="100" cy="100" r="50" fill="blue"/>
        <text x="100" y="105" text-anchor="middle">Details</text>
    </g>
</svg>
```

### Data-Driven Styling

```html
<!-- Model: { status: "warning", bgColor: "#ff9900", message: "Alert" } -->
<svg width="300pt" height="100pt">
    <rect width="300" height="100" fill="{{model.bgColor}}" rx="5"/>
    <text x="150" y="55" text-anchor="middle" fill="white" font-size="20">
        {{model.message}}
    </text>
</svg>
```

---

## Examples

### Basic SVG with Shapes

```html
<svg width="200pt" height="200pt" viewBox="0 0 200 200">
    <rect x="10" y="10" width="180" height="180"
          fill="none" stroke="#336699" stroke-width="2"/>
    <circle cx="100" cy="100" r="60" fill="#ff6347" opacity="0.7"/>
    <line x1="40" y1="40" x2="160" y2="160"
          stroke="black" stroke-width="2"/>
</svg>
```

### SVG with ViewBox Scaling

```html
<!-- Canvas is 150pt square, but uses 0-100 coordinate system -->
<svg width="150pt" height="150pt" viewBox="0 0 100 100">
    <circle cx="50" cy="50" r="45" fill="#336699" stroke="white" stroke-width="2"/>
    <text x="50" y="55" text-anchor="middle" fill="white"
          font-size="12" font-family="Arial">
        Scaled
    </text>
</svg>
```

### SVG with Gradient Definitions

```html
<svg width="250pt" height="150pt" viewBox="0 0 250 150">
    <defs>
        <linearGradient id="blueGradient" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#4a90e2"/>
            <stop offset="100%" stop-color="#0066cc"/>
        </linearGradient>
        <radialGradient id="sunGradient" cx="50%" cy="50%" r="50%">
            <stop offset="0%" stop-color="#ffff00"/>
            <stop offset="100%" stop-color="#ff9900"/>
        </radialGradient>
    </defs>
    <rect width="250" height="150" fill="url(#blueGradient)"/>
    <circle cx="200" cy="40" r="30" fill="url(#sunGradient)"/>
</svg>
```

### Nested SVG Canvases

```html
<svg width="400pt" height="200pt">
    <svg x="10" y="10" width="180" height="180" viewBox="0 0 100 100">
        <rect width="100" height="100" fill="#e8f4f8"/>
        <circle cx="50" cy="50" r="40" fill="#336699"/>
    </svg>
    <svg x="210" y="10" width="180" height="180" viewBox="0 0 100 100">
        <rect width="100" height="100" fill="#fff4e8"/>
        <rect x="20" y="20" width="60" height="60" fill="#ff9900"/>
    </svg>
</svg>
```

### SVG with Reusable Symbols

```html
<svg width="300pt" height="200pt">
    <defs>
        <g id="star">
            <path d="M 0,-10 L 2,-2 L 10,-2 L 4,2 L 6,10 L 0,5 L -6,10 L -4,2 L -10,-2 L -2,-2 Z"
                  fill="gold" stroke="#cc9900" stroke-width="0.5"/>
        </g>
    </defs>
    <use href="#star" x="50" y="50"/>
    <use href="#star" x="150" y="50" transform="scale(1.5)"/>
    <use href="#star" x="250" y="50" transform="scale(2)"/>
</svg>
```

### SVG with Inherited Styles

```html
<svg width="300pt" height="100pt" fill="none" stroke="#336699" stroke-width="3">
    <!-- All children inherit the stroke settings -->
    <circle cx="50" cy="50" r="30"/>
    <circle cx="150" cy="50" r="30" fill="red"/>
    <circle cx="250" cy="50" r="30" fill="blue" stroke="red"/>
</svg>
```

### Responsive SVG (PreserveAspectRatio)

```html
<!-- Different scaling behaviors -->
<svg width="200pt" height="100pt" viewBox="0 0 100 100"
     preserveAspectRatio="none">
    <!-- Stretches to fill container, distorts aspect ratio -->
    <circle cx="50" cy="50" r="40" fill="#336699"/>
</svg>

<svg width="200pt" height="100pt" viewBox="0 0 100 100"
     preserveAspectRatio="xMidYMid meet">
    <!-- Maintains aspect ratio, centered, all visible -->
    <circle cx="50" cy="50" r="40" fill="#336699"/>
</svg>
```

### SVG Chart with Data Binding

```html
<!-- Model: { chartData: [
    {label: "Q1", value: 75, color: "#4a90e2"},
    {label: "Q2", value: 120, color: "#50c878"},
    {label: "Q3", value: 95, color: "#ff9900"}
]} -->
<svg width="400pt" height="250pt" viewBox="0 0 400 250">
    <rect width="400" height="250" fill="#f9f9f9"/>
    <text x="200" y="30" text-anchor="middle" font-size="20" font-weight="bold">
        Quarterly Results
    </text>
    <template data-bind="{{model.chartData}}">
        <g transform="translate({{50 + $index * 100}}, 200)">
            <rect x="0" y="{{-1 * .value}}" width="60" height="{{.value}}"
                  fill="{{.color}}" stroke="black" stroke-width="1"/>
            <text x="30" y="20" text-anchor="middle" font-size="12">{{.label}}</text>
            <text x="30" y="{{-1 * .value - 5}}" text-anchor="middle" font-size="10">
                {{.value}}
            </text>
        </g>
    </template>
</svg>
```

### SVG with Patterns

```html
<svg width="300pt" height="200pt">
    <defs>
        <pattern id="dots" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <circle cx="10" cy="10" r="3" fill="#336699"/>
        </pattern>
        <pattern id="stripes" x="0" y="0" width="10" height="10" patternUnits="userSpaceOnUse">
            <rect width="5" height="10" fill="#ff6347"/>
        </pattern>
    </defs>
    <rect x="10" y="10" width="130" height="180" fill="url(#dots)"/>
    <rect x="160" y="10" width="130" height="180" fill="url(#stripes)"/>
</svg>
```

### Icon Library with Defs

```html
<svg width="300pt" height="100pt">
    <defs>
        <g id="checkIcon">
            <circle r="15" fill="#50c878" stroke="#2d7d4d" stroke-width="2"/>
            <path d="M -6,0 L -2,5 L 8,-8" stroke="white" stroke-width="3"
                  fill="none" stroke-linecap="round" stroke-linejoin="round"/>
        </g>
        <g id="errorIcon">
            <circle r="15" fill="#ff4444" stroke="#cc0000" stroke-width="2"/>
            <line x1="-6" y1="-6" x2="6" y2="6" stroke="white" stroke-width="3"
                  stroke-linecap="round"/>
            <line x1="6" y1="-6" x2="-6" y2="6" stroke="white" stroke-width="3"
                  stroke-linecap="round"/>
        </g>
    </defs>
    <use href="#checkIcon" x="75" y="50"/>
    <use href="#errorIcon" x="225" y="50"/>
</svg>
```

### SVG Logo with Title

```html
<svg width="200pt" height="80pt" viewBox="0 0 200 80" title="Company Logo">
    <defs>
        <linearGradient id="logoGradient">
            <stop offset="0%" stop-color="#336699"/>
            <stop offset="100%" stop-color="#6699cc"/>
        </linearGradient>
    </defs>
    <rect width="200" height="80" fill="url(#logoGradient)" rx="10"/>
    <text x="100" y="45" text-anchor="middle" fill="white"
          font-size="28" font-family="Arial" font-weight="bold">
        COMPANY
    </text>
    <text x="100" y="65" text-anchor="middle" fill="white"
          font-size="12" font-family="Arial">
        Making PDFs Better
    </text>
</svg>
```

### Dynamic Gauge Chart

```html
<!-- Model: { percentage: 75, status: "good" } -->
<svg width="200pt" height="200pt" viewBox="0 0 200 200">
    <circle cx="100" cy="100" r="80" fill="none"
            stroke="#e0e0e0" stroke-width="20"/>
    <circle cx="100" cy="100" r="80" fill="none"
            stroke="{{model.status == 'good' ? '#50c878' : '#ff4444'}}"
            stroke-width="20"
            stroke-dasharray="{{model.percentage * 5.024}}, 502.4"
            transform="rotate(-90 100 100)"/>
    <text x="100" y="105" text-anchor="middle" font-size="40" font-weight="bold">
        {{model.percentage}}%
    </text>
</svg>
```

### SVG Map with Interactive Elements

```html
<svg width="400pt" height="300pt" viewBox="0 0 400 300">
    <defs>
        <filter id="shadow">
            <feGaussianBlur in="SourceAlpha" stdDeviation="3"/>
            <feOffset dx="2" dy="2" result="offsetblur"/>
            <feMerge>
                <feMergeNode/>
                <feMergeNode in="SourceGraphic"/>
            </feMerge>
        </filter>
    </defs>
    <rect width="400" height="300" fill="#e8f4f8"/>
    <g id="location1" filter="url(#shadow)">
        <circle cx="100" cy="100" r="20" fill="#ff6347"/>
        <text x="100" y="140" text-anchor="middle" font-size="12">Location A</text>
    </g>
    <g id="location2" filter="url(#shadow)">
        <circle cx="300" cy="150" r="20" fill="#336699"/>
        <text x="300" y="190" text-anchor="middle" font-size="12">Location B</text>
    </g>
</svg>
```

### SVG Timeline

```html
<!-- Model: { events: [{year: "2020", title: "Founded", x: 50}, ...] } -->
<svg width="500pt" height="150pt" viewBox="0 0 500 150">
    <line x1="50" y1="75" x2="450" y2="75"
          stroke="#336699" stroke-width="4"/>
    <template data-bind="{{model.events}}">
        <g>
            <circle cx="{{.x}}" cy="75" r="8" fill="#336699" stroke="white" stroke-width="2"/>
            <text x="{{.x}}" y="55" text-anchor="middle" font-size="12" font-weight="bold">
                {{.year}}
            </text>
            <text x="{{.x}}" y="105" text-anchor="middle" font-size="10">
                {{.title}}
            </text>
        </g>
    </template>
</svg>
```

### SVG Status Badge

```html
<!-- Model: { count: 5, type: "error" } -->
<svg width="80pt" height="30pt" viewBox="0 0 80 30">
    <rect width="80" height="30"
          fill="{{model.type == 'error' ? '#ff4444' : '#50c878'}}"
          rx="5"/>
    <text x="40" y="20" text-anchor="middle" fill="white"
          font-size="14" font-weight="bold">
        {{model.type == 'error' ? 'Errors' : 'Success'}}: {{model.count}}
    </text>
</svg>
```

### Complex Dashboard Panel

```html
<svg width="600pt" height="400pt" viewBox="0 0 600 400">
    <defs>
        <linearGradient id="headerGrad" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#336699"/>
            <stop offset="100%" stop-color="#254a70"/>
        </linearGradient>
    </defs>

    <!-- Header -->
    <rect width="600" height="60" fill="url(#headerGrad)"/>
    <text x="20" y="40" fill="white" font-size="24" font-weight="bold">
        Dashboard Overview
    </text>

    <!-- Grid panels -->
    <rect x="20" y="80" width="270" height="140" fill="white"
          stroke="#ddd" stroke-width="2" rx="5"/>
    <rect x="310" y="80" width="270" height="140" fill="white"
          stroke="#ddd" stroke-width="2" rx="5"/>
    <rect x="20" y="240" width="270" height="140" fill="white"
          stroke="#ddd" stroke-width="2" rx="5"/>
    <rect x="310" y="240" width="270" height="140" fill="white"
          stroke="#ddd" stroke-width="2" rx="5"/>

    <!-- Panel content placeholders -->
    <text x="155" y="155" text-anchor="middle" font-size="16" fill="#999">
        Chart A
    </text>
    <text x="445" y="155" text-anchor="middle" font-size="16" fill="#999">
        Chart B
    </text>
    <text x="155" y="315" text-anchor="middle" font-size="16" fill="#999">
        Chart C
    </text>
    <text x="445" y="315" text-anchor="middle" font-size="16" fill="#999">
        Chart D
    </text>
</svg>
```

---

## See Also

- [g](/reference/svgtags/g.html) - Group container for organizing SVG elements
- [defs](/reference/svgtags/defs.html) - Definitions container for reusable elements
- [use](/reference/svgtags/use.html) - Reference and reuse defined elements
- [a](/reference/svgtags/a.html) - SVG anchor/link element
- [SVG Shapes](/reference/svgtags/shapes.html) - rect, circle, ellipse, line, polyline, polygon, path
- [SVG Text](/reference/svgtags/text.html) - Text rendering in SVG
- [Canvas Component](/reference/components/canvas.html) - Base canvas component
- [Data Binding](/reference/binding/) - Data binding and expressions
- [Gradients](/reference/svgtags/gradients.html) - Linear and radial gradients
- [Patterns](/reference/svgtags/patterns.html) - Pattern fills

---
