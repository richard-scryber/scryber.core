---
layout: default
title: pattern (SVG)
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;pattern&gt; : The SVG Pattern Fill Element

The `<pattern>` element defines a reusable graphic pattern that can be used to fill or stroke shapes. Patterns tile repeatedly to cover the area of the shape they're applied to, enabling textures, backgrounds, and decorative fills.

---

## Summary

The `<pattern>` element creates tiled graphics that repeat to fill shapes. Patterns are defined once and can be referenced multiple times across different elements. They are ideal for backgrounds, textures, hatching, decorative fills, and data visualization patterns.

Key features:
- Define reusable tiled patterns
- Tile size and positioning control
- ViewBox support for scalable patterns
- Nested SVG content (shapes, images, text)
- Pattern coordinate system options
- Transformation support
- Reference with URL notation in fill attributes
- Data binding for dynamic pattern content

---

## Usage

Patterns are defined in the `<defs>` section and applied to shapes using the `fill` or `stroke` attributes:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="dots" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <circle cx="10" cy="10" r="3" fill="#336699"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#dots)"/>
</svg>
```

### Basic Syntax

```html
<!-- Simple dot pattern -->
<pattern id="dotPattern" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
    <circle cx="10" cy="10" r="4" fill="blue"/>
</pattern>

<!-- Stripe pattern -->
<pattern id="stripes" x="0" y="0" width="10" height="10" patternUnits="userSpaceOnUse">
    <rect x="0" y="0" width="5" height="10" fill="#336699"/>
</pattern>

<!-- Pattern with viewBox -->
<pattern id="scalable" x="0" y="0" width="20" height="20"
         patternUnits="userSpaceOnUse"
         viewBox="0 0 10 10">
    <circle cx="5" cy="5" r="3" fill="red"/>
</pattern>

<!-- Pattern with image content -->
<pattern id="texture" x="0" y="0" width="50" height="50" patternUnits="userSpaceOnUse">
    <image href="texture.png" x="0" y="0" width="50" height="50"/>
</pattern>
```

### Applying Patterns

```html
<!-- Fill with pattern -->
<rect x="10" y="10" width="200" height="100" fill="url(#dotPattern)"/>

<!-- Stroke with pattern -->
<circle cx="100" cy="100" r="50" fill="white" stroke="url(#stripes)" stroke-width="10"/>

<!-- Multiple shapes using same pattern -->
<rect x="10" y="10" width="100" height="100" fill="url(#dots)"/>
<circle cx="200" cy="60" r="40" fill="url(#dots)"/>
<polygon points="300,10 350,100 250,100" fill="url(#dots)"/>
```

---

## Supported Attributes

### Identification Attribute

| Attribute | Type | Description | Required |
|-----------|------|-------------|----------|
| `id` | String | Unique identifier for referencing the pattern | Yes |

### Position Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `x` | Unit | X coordinate of pattern tile origin | 0 |
| `y` | Unit | Y coordinate of pattern tile origin | 0 |

### Size Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `width` | Unit | Width of pattern tile | Required |
| `height` | Unit | Height of pattern tile | Required |

### Coordinate System Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `patternUnits` | Enum | Coordinate system for x, y, width, height | objectBoundingBox |
| `patternContentUnits` | Enum | Coordinate system for pattern content | userSpaceOnUse |

Valid values for both:
- `objectBoundingBox` - Relative to bounding box (0-1 range)
- `userSpaceOnUse` - Absolute coordinates in user space

### ViewBox Attribute

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `viewBox` | Rect | Coordinate system for pattern content | none |

Format: `minX minY width height`

### Aspect Ratio Attribute

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `preserveAspectRatio` | String | How to preserve aspect ratio with viewBox | xMidYMid meet |

### Reference Attribute

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `href` | String | Reference to another pattern as template | none |

### Common Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `class` | String | CSS class name(s) for styling |
| `style` | Style | Inline CSS-style properties |
| `title` | String | Tooltip or title text |
| `desc` | String | Description for accessibility |

---

## Data Binding

Patterns support data binding for dynamic content, colors, and sizing.

### Dynamic Pattern Content

```html
<!-- Pattern with data-driven colors -->
<defs>
    <pattern id="dynamicDots" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
        <circle cx="10" cy="10" r="4" fill="{{model.dotColor}}"/>
    </pattern>
</defs>

<!-- Conditional pattern shapes -->
<defs>
    <pattern id="conditional" x="0" y="0" width="25" height="25" patternUnits="userSpaceOnUse">
        <circle cx="12.5" cy="12.5" r="{{model.useCircles ? 8 : 0}}" fill="#336699"/>
        <rect x="5" y="5" width="15" height="15" fill="{{model.useSquares ? '#ff6600' : 'none'}}"/>
    </pattern>
</defs>
```

### Dynamic Pattern Size

```html
<!-- Variable tile size -->
<defs>
    <pattern id="sizedPattern"
             x="0" y="0"
             width="{{model.tileSize}}"
             height="{{model.tileSize}}"
             patternUnits="userSpaceOnUse">
        <circle cx="{{model.tileSize / 2}}"
                cy="{{model.tileSize / 2}}"
                r="{{model.tileSize / 4}}"
                fill="#336699"/>
    </pattern>
</defs>
```

### Template-Generated Patterns

```html
<!-- Create multiple pattern variations -->
<defs>
    <template data-bind="{{model.patternStyles}}">
        <pattern id="pattern-{{.id}}"
                 x="0" y="0"
                 width="{{.size}}"
                 height="{{.size}}"
                 patternUnits="userSpaceOnUse">
            <circle cx="{{.size / 2}}"
                    cy="{{.size / 2}}"
                    r="{{.size / 3}}"
                    fill="{{.color}}"/>
        </pattern>
    </template>
</defs>

<!-- Apply patterns dynamically -->
<template data-bind="{{model.regions}}">
    <rect x="{{.x}}" y="{{.y}}" width="{{.width}}" height="{{.height}}"
          fill="url(#pattern-{{.patternId}})"/>
</template>
```

### Data-Driven Pattern Application

```html
<!-- Model: { dataPoints: [{value: 75, threshold: 80}, ...] } -->
<template data-bind="{{model.dataPoints}}">
    <rect x="{{$index * 60 + 20}}"
          y="50"
          width="50"
          height="{{.value * 2}}"
          fill="{{.value >= .threshold ? 'url(#alertPattern)' : 'url(#normalPattern)'}}"/>
</template>
```

---

## Notes

### Pattern Tiling

- Patterns automatically repeat (tile) to fill the shape
- The `width` and `height` define the size of each tile
- Tiles start from the pattern origin (x, y)
- Partial tiles are clipped at shape boundaries

### Coordinate Systems

**patternUnits** controls pattern positioning and sizing:
- `objectBoundingBox` - Values are fractions (0-1) of shape size
- `userSpaceOnUse` (default) - Values use the SVG coordinate system

**patternContentUnits** controls pattern content coordinates:
- `userSpaceOnUse` (default) - Content uses SVG coordinate system
- `objectBoundingBox` - Content scaled to shape size

### ViewBox Scaling

When a `viewBox` is specified:
- Pattern content is scaled to fit pattern tile size
- Enables resolution-independent patterns
- Maintains aspect ratio of pattern content
- Useful for responsive designs

### Pattern Content

Patterns can contain:
- Basic shapes (circle, rect, line, polygon, etc.)
- Paths and complex graphics
- Images (for texture patterns)
- Text elements
- Groups with multiple elements
- Other nested patterns (with care)

### Performance Considerations

- Define patterns once in `<defs>` and reuse
- Patterns are rendered efficiently through caching
- Very small tile sizes may impact performance
- Complex pattern graphics should be optimized
- Consider pattern complexity for large filled areas

### Pattern vs Gradient

- **Patterns** - Repeating graphical content
- **Gradients** - Smooth color transitions
- Patterns can contain gradients
- Choose based on visual effect needed

### Pattern Alignment

- By default, patterns align to the SVG coordinate system
- Not aligned to shape position
- Use `x` and `y` to adjust pattern origin
- Transform patterns for rotation or skewing

### Limitations

- Pattern animations have limited support
- Infinite recursion with nested patterns will fail
- Very large patterns increase memory usage
- Some pattern features may have limited PDF support

---

## Examples

### 1. Simple Dot Pattern

Basic repeating dots:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="dots" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <circle cx="10" cy="10" r="3" fill="#336699"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#dots)" stroke="#336699" stroke-width="2"/>
</svg>
```

### 2. Vertical Stripes

Simple stripe pattern:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="stripes" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="10" height="20" fill="#336699"/>
            <rect x="10" y="0" width="10" height="20" fill="#6699cc"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#stripes)"/>
</svg>
```

### 3. Grid Pattern

Cross-hatch grid:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="grid" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <path d="M 20 0 L 0 0 0 20" fill="none" stroke="#ccc" stroke-width="1"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="white" stroke="#333" stroke-width="1"/>
    <rect x="50" y="50" width="300" height="200" fill="url(#grid)"/>
</svg>
```

### 4. Diagonal Lines

Diagonal hatching pattern:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="diagonals" x="0" y="0" width="10" height="10" patternUnits="userSpaceOnUse">
            <line x1="0" y1="0" x2="10" y2="10" stroke="#336699" stroke-width="1"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#diagonals)" stroke="#336699" stroke-width="2"/>
</svg>
```

### 5. Checkerboard Pattern

Alternating squares:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="checkerboard" x="0" y="0" width="40" height="40" patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="20" height="20" fill="#f0f0f0"/>
            <rect x="20" y="0" width="20" height="20" fill="white"/>
            <rect x="0" y="20" width="20" height="20" fill="white"/>
            <rect x="20" y="20" width="20" height="20" fill="#f0f0f0"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#checkerboard)" stroke="#333" stroke-width="1"/>
</svg>
```

### 6. Star Pattern

Decorative star pattern:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="stars" x="0" y="0" width="30" height="30" patternUnits="userSpaceOnUse">
            <path d="M 15 5 L 17 12 L 24 12 L 18 17 L 20 24 L 15 19 L 10 24 L 12 17 L 6 12 L 13 12 Z"
                  fill="#ff6600" opacity="0.3"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#stars)" stroke="#ff6600" stroke-width="2"/>
</svg>
```

### 7. Polka Dots

Large decorative dots:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="polkaDots" x="0" y="0" width="40" height="40" patternUnits="userSpaceOnUse">
            <circle cx="20" cy="20" r="8" fill="#336699" opacity="0.4"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="#f0f0f0"/>
    <rect x="50" y="50" width="300" height="200" fill="url(#polkaDots)"/>
</svg>
```

### 8. Brick Pattern

Brick wall texture:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="bricks" x="0" y="0" width="60" height="40" patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="60" height="40" fill="#cc6633"/>
            <rect x="0" y="0" width="30" height="20" fill="#aa5522" stroke="#996644" stroke-width="1"/>
            <rect x="30" y="0" width="30" height="20" fill="#aa5522" stroke="#996644" stroke-width="1"/>
            <rect x="0" y="20" width="60" height="20" fill="#aa5522" stroke="#996644" stroke-width="1"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#bricks)"/>
</svg>
```

### 9. Hexagon Pattern

Honeycomb texture:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="hexagons" x="0" y="0" width="30" height="26" patternUnits="userSpaceOnUse">
            <polygon points="15,0 27,7.5 27,19.5 15,26 3,19.5 3,7.5"
                     fill="none" stroke="#336699" stroke-width="1"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="white"/>
    <rect x="50" y="50" width="300" height="200" fill="url(#hexagons)"/>
</svg>
```

### 10. Data-Driven Pattern Fill

Bar chart with pattern fills based on data:

```html
<!-- Model: { data: [{value: 120, category: "A", usePattern: true}, ...] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="500" height="300">
    <defs>
        <pattern id="dataPattern" x="0" y="0" width="10" height="10" patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="5" height="10" fill="#336699"/>
            <rect x="5" y="0" width="5" height="10" fill="#6699cc"/>
        </pattern>
    </defs>

    <template data-bind="{{model.data}}">
        <rect x="{{$index * 100 + 50}}"
              y="{{250 - .value}}"
              width="80"
              height="{{.value}}"
              fill="{{.usePattern ? 'url(#dataPattern)' : '#336699'}}"/>

        <text x="{{$index * 100 + 90}}"
              y="270"
              text-anchor="middle"
              font-size="12">
            {{.category}}
        </text>
    </template>
</svg>
```

### 11. Image Texture Pattern

Pattern using an image:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="texture" x="0" y="0" width="50" height="50" patternUnits="userSpaceOnUse">
            <image href="./textures/fabric.jpg" x="0" y="0" width="50" height="50"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#texture)" stroke="#666" stroke-width="2"/>
</svg>
```

### 12. Gradient Within Pattern

Pattern containing gradient:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <linearGradient id="tileGradient" x1="0%" y1="0%" x2="100%" y2="100%">
            <stop offset="0%" style="stop-color:#336699;stop-opacity:1"/>
            <stop offset="100%" style="stop-color:#6699cc;stop-opacity:1"/>
        </linearGradient>

        <pattern id="gradientPattern" x="0" y="0" width="40" height="40" patternUnits="userSpaceOnUse">
            <rect x="2" y="2" width="36" height="36" rx="5" fill="url(#tileGradient)"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#gradientPattern)"/>
</svg>
```

### 13. Dynamic Pattern Color

Pattern with data-driven colors:

```html
<!-- Model: { primaryColor: "#336699", secondaryColor: "#6699cc" } -->
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="coloredPattern" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="10" height="20" fill="{{model.primaryColor}}"/>
            <rect x="10" y="0" width="10" height="20" fill="{{model.secondaryColor}}"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#coloredPattern)"/>
</svg>
```

### 14. Status Indicator Pattern

Different patterns for different statuses:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="500" height="200">
    <defs>
        <!-- Success pattern -->
        <pattern id="successPattern" x="0" y="0" width="15" height="15" patternUnits="userSpaceOnUse">
            <circle cx="7.5" cy="7.5" r="3" fill="#00aa00" opacity="0.3"/>
        </pattern>

        <!-- Warning pattern -->
        <pattern id="warningPattern" x="0" y="0" width="15" height="15" patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="7.5" height="15" fill="#ff6600" opacity="0.3"/>
        </pattern>

        <!-- Error pattern -->
        <pattern id="errorPattern" x="0" y="0" width="15" height="15" patternUnits="userSpaceOnUse">
            <line x1="0" y1="0" x2="15" y2="15" stroke="#cc0000" stroke-width="2" opacity="0.3"/>
        </pattern>
    </defs>

    <rect x="20" y="50" width="120" height="100" fill="url(#successPattern)" stroke="#00aa00" stroke-width="2"/>
    <text x="80" y="165" text-anchor="middle" font-size="12">Success</text>

    <rect x="190" y="50" width="120" height="100" fill="url(#warningPattern)" stroke="#ff6600" stroke-width="2"/>
    <text x="250" y="165" text-anchor="middle" font-size="12">Warning</text>

    <rect x="360" y="50" width="120" height="100" fill="url(#errorPattern)" stroke="#cc0000" stroke-width="2"/>
    <text x="420" y="165" text-anchor="middle" font-size="12">Error</text>
</svg>
```

### 15. Cross-Hatch Pattern

Multiple direction hatching:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="crosshatch" x="0" y="0" width="10" height="10" patternUnits="userSpaceOnUse">
            <line x1="0" y1="0" x2="10" y2="10" stroke="#666" stroke-width="0.5"/>
            <line x1="10" y1="0" x2="0" y2="10" stroke="#666" stroke-width="0.5"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#crosshatch)" stroke="#333" stroke-width="2"/>
</svg>
```

### 16. Map Legend Patterns

Different patterns for map regions:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="250">
    <defs>
        <!-- Residential pattern -->
        <pattern id="residential" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <rect x="5" y="5" width="10" height="10" fill="#336699" opacity="0.4"/>
        </pattern>

        <!-- Commercial pattern -->
        <pattern id="commercial" x="0" y="0" width="15" height="15" patternUnits="userSpaceOnUse">
            <line x1="0" y1="0" x2="15" y2="15" stroke="#ff6600" stroke-width="2" opacity="0.5"/>
        </pattern>

        <!-- Industrial pattern -->
        <pattern id="industrial" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <circle cx="10" cy="10" r="4" fill="#999" opacity="0.5"/>
        </pattern>
    </defs>

    <!-- Legend -->
    <text x="20" y="30" font-size="16" font-weight="700">Zone Types</text>

    <rect x="20" y="50" width="40" height="40" fill="url(#residential)" stroke="#336699" stroke-width="1"/>
    <text x="70" y="75" font-size="12">Residential</text>

    <rect x="20" y="110" width="40" height="40" fill="url(#commercial)" stroke="#ff6600" stroke-width="1"/>
    <text x="70" y="135" font-size="12">Commercial</text>

    <rect x="20" y="170" width="40" height="40" fill="url(#industrial)" stroke="#999" stroke-width="1"/>
    <text x="70" y="195" font-size="12">Industrial</text>
</svg>
```

### 17. Scatter Plot with Pattern Backgrounds

Chart regions with pattern fills:

```html
<!-- Model: { zones: [{x: 0, y: 0, w: 200, h: 150, pattern: "safe"}, ...] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="500" height="400">
    <defs>
        <pattern id="safeZone" x="0" y="0" width="30" height="30" patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="30" height="30" fill="#00aa00" opacity="0.1"/>
        </pattern>

        <pattern id="warningZone" x="0" y="0" width="30" height="30" patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="30" height="30" fill="#ff6600" opacity="0.1"/>
        </pattern>

        <pattern id="dangerZone" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <line x1="0" y1="0" x2="20" y2="20" stroke="#cc0000" stroke-width="1" opacity="0.2"/>
        </pattern>
    </defs>

    <!-- Background zones -->
    <template data-bind="{{model.zones}}">
        <rect x="{{.x}}" y="{{.y}}" width="{{.w}}" height="{{.h}}"
              fill="url(#{{.pattern}}Zone)"/>
    </template>

    <!-- Axes -->
    <line x1="50" y1="350" x2="450" y2="350" stroke="#333" stroke-width="2"/>
    <line x1="50" y1="350" x2="50" y2="50" stroke="#333" stroke-width="2"/>
</svg>
```

### 18. Circular Pattern

Radial dot pattern:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="400">
    <defs>
        <pattern id="radialDots" x="0" y="0" width="40" height="40" patternUnits="userSpaceOnUse">
            <circle cx="20" cy="20" r="2" fill="#336699"/>
            <circle cx="10" cy="10" r="2" fill="#336699"/>
            <circle cx="30" cy="10" r="2" fill="#336699"/>
            <circle cx="10" cy="30" r="2" fill="#336699"/>
            <circle cx="30" cy="30" r="2" fill="#336699"/>
        </pattern>
    </defs>

    <circle cx="200" cy="200" r="150" fill="url(#radialDots)" stroke="#336699" stroke-width="2"/>
</svg>
```

### 19. Variable Density Pattern

Pattern density based on data value:

```html
<!-- Model: { density: 0.75 } -->
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <pattern id="densityPattern"
                 x="0" y="0"
                 width="{{20 / model.density}}"
                 height="{{20 / model.density}}"
                 patternUnits="userSpaceOnUse">
            <circle cx="{{10 / model.density}}"
                    cy="{{10 / model.density}}"
                    r="3"
                    fill="#336699"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#densityPattern)" stroke="#336699" stroke-width="2"/>
    <text x="200" y="275" text-anchor="middle" font-size="14">
        Density: {{(model.density * 100).toFixed(0)}}%
    </text>
</svg>
```

### 20. Multi-Pattern Comparison

Side-by-side pattern comparisons:

```html
<!-- Model: { patterns: [{id: "p1", name: "Pattern A", color: "#336699"}, ...] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="650" height="300">
    <defs>
        <template data-bind="{{model.patterns}}">
            <pattern id="{{.id}}" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
                <circle cx="10" cy="10" r="5" fill="{{.color}}" opacity="0.5"/>
            </pattern>
        </template>
    </defs>

    <template data-bind="{{model.patterns}}">
        <g>
            <rect x="{{$index * 210 + 10}}"
                  y="50"
                  width="190"
                  height="150"
                  fill="url(#{{.id}})"
                  stroke="{{.color}}"
                  stroke-width="2"/>

            <text x="{{$index * 210 + 105}}"
                  y="220"
                  text-anchor="middle"
                  font-size="14"
                  font-weight="600">
                {{.name}}
            </text>
        </g>
    </template>
</svg>
```

---

## See Also

- [defs element](/reference/svgtags/defs.html) - Definitions container for patterns
- [linearGradient element](/reference/svgtags/linearGradient.html) - Linear gradient fills
- [radialGradient element](/reference/svgtags/radialGradient.html) - Radial gradient fills
- [fill attribute](/reference/svgattributes/attr_fill.html) - Fill colors and patterns
- [rect element](/reference/svgtags/rect.html) - Rectangle shape
- [circle element](/reference/svgtags/circle.html) - Circle shape
- [path element](/reference/svgtags/path.html) - Path shape
- [Data Binding](/reference/binding/) - Complete data binding guide
- [SVG Styling](/reference/svg/styling/) - SVG style reference

---
