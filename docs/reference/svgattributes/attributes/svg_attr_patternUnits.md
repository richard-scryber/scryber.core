---
layout: default
title: patternUnits
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @patternUnits : The Pattern Coordinate System Attribute

The `patternUnits` attribute defines the coordinate system for the pattern's positioning attributes (`x`, `y`, `width`, `height`). It determines whether the pattern tile dimensions are relative to the filled shape or absolute in document space.

## Usage

The `patternUnits` attribute is used to:
- Define whether pattern positioning is relative or absolute
- Create patterns that scale with filled shapes
- Build fixed-size repeating patterns
- Support data-driven coordinate system selection
- Control pattern tile sizing behavior
- Position patterns precisely in document space

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <!-- Relative pattern (scales with shape) -->
        <pattern id="relative" x="0" y="0" width="0.2" height="0.2"
                 patternUnits="objectBoundingBox">
            <circle cx="10" cy="10" r="8" fill="#3498db"/>
        </pattern>

        <!-- Absolute pattern (fixed size) -->
        <pattern id="absolute" x="0" y="0" width="30" height="30"
                 patternUnits="userSpaceOnUse">
            <circle cx="15" cy="15" r="12" fill="#e74c3c"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="150" height="100" fill="url(#relative)"/>
    <rect x="250" y="50" width="150" height="100" fill="url(#absolute)"/>
</svg>
```

---

## Supported Values

| Value | Description | Coordinates | Use Case |
|-------|-------------|-------------|----------|
| `objectBoundingBox` | Relative to shape (default) | 0-1 or 0%-100% | Patterns that scale with shapes |
| `userSpaceOnUse` | Absolute document space | pt, px, mm, cm, in | Fixed-size repeating patterns |

### Default Behavior

```html
<!-- These are equivalent (objectBoundingBox is default) -->
<pattern id="p1" width="0.25" height="0.25">
<pattern id="p2" width="0.25" height="0.25" patternUnits="objectBoundingBox">
```

---

## Supported Elements

The `patternUnits` attribute is supported on:

- **[&lt;pattern&gt;](/reference/svgtags/pattern.html)** - Pattern element coordinate system

---

## Data Binding

### Dynamic Coordinate System Selection

Choose coordinate system based on data:

```html
<!-- Model: { useRelative: true, tileSize: 50 } -->
<svg width="400" height="300">
    <defs>
        <pattern id="dynamicPattern"
                 x="0" y="0"
                 width="{{model.useRelative ? '0.2' : model.tileSize}}"
                 height="{{model.useRelative ? '0.2' : model.tileSize}}"
                 patternUnits="{{model.useRelative ? 'objectBoundingBox' : 'userSpaceOnUse'}}">
            <rect width="{{model.useRelative ? '10' : model.tileSize}}"
                  height="{{model.useRelative ? '10' : model.tileSize}}"
                  fill="#3498db" opacity="0.3"/>
            <circle cx="{{model.useRelative ? '5' : model.tileSize/2}}"
                    cy="{{model.useRelative ? '5' : model.tileSize/2}}"
                    r="{{model.useRelative ? '3' : model.tileSize/4}}"
                    fill="#e74c3c"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#dynamicPattern)" stroke="#2c3e50"/>
</svg>
```

### Adaptive Pattern Sizing

Switch between responsive and fixed patterns:

```html
<!-- Model: { responsive: true, shapeWidth: 200, shapeHeight: 150 } -->
<svg width="450" height="400">
    <defs>
        <pattern id="adaptivePattern"
                 x="0" y="0"
                 width="{{model.responsive ? '0.15' : '25'}}"
                 height="{{model.responsive ? '0.15' : '25'}}"
                 patternUnits="{{model.responsive ? 'objectBoundingBox' : 'userSpaceOnUse'}}">
            <path d="M 0,12.5 L 12.5,0 L 25,12.5 L 12.5,25 Z"
                  fill="none" stroke="#3498db" stroke-width="2"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="{{model.shapeWidth}}" height="{{model.shapeHeight}}"
          fill="url(#adaptivePattern)" stroke="#2c3e50" stroke-width="2"/>
    <text x="50" y="{{model.shapeHeight + 80}}" font-size="12">
        Mode: {{model.responsive ? 'Responsive (scales with shape)' : 'Fixed (constant tile size)'}}
    </text>
</svg>
```

### Background Pattern Library

Create reusable patterns with different coordinate systems:

```html
<!-- Model: {
    backgrounds: [
        {id: 'dots', type: 'relative', size: 0.1, name: 'Dot Pattern'},
        {id: 'grid', type: 'fixed', size: 30, name: 'Grid Pattern'},
        {id: 'stripes', type: 'fixed', size: 20, name: 'Stripe Pattern'}
    ],
    selectedBg: 'dots'
} -->
<svg width="500" height="400">
    <defs>
        <template data-bind="{{model.backgrounds}}">
            <pattern id="{{.id}}"
                     x="0" y="0"
                     width="{{.size}}"
                     height="{{.size}}"
                     patternUnits="{{.type === 'relative' ? 'objectBoundingBox' : 'userSpaceOnUse'}}">
                <!-- Pattern content varies by type -->
            </pattern>
        </template>

        <pattern id="dots" x="0" y="0" width="0.1" height="0.1"
                 patternUnits="objectBoundingBox">
            <circle cx="5" cy="5" r="3" fill="#3498db"/>
        </pattern>

        <pattern id="grid" x="0" y="0" width="30" height="30"
                 patternUnits="userSpaceOnUse">
            <path d="M 30,0 L 0,0 L 0,30" fill="none" stroke="#95a5a6" stroke-width="1"/>
        </pattern>

        <pattern id="stripes" x="0" y="0" width="20" height="20"
                 patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="10" height="20" fill="#ecf0f1"/>
            <rect x="10" y="0" width="10" height="20" fill="#bdc3c7"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300"
          fill="url(#{{model.selectedBg}})"
          stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Chart Background with Fixed Grid

Use userSpaceOnUse for consistent grid patterns:

```html
<!-- Model: { chartX: 50, chartY: 50, chartWidth: 400, chartHeight: 300, gridSize: 40 } -->
<svg width="500" height="400">
    <defs>
        <pattern id="chartGrid"
                 x="{{model.chartX}}" y="{{model.chartY}}"
                 width="{{model.gridSize}}" height="{{model.gridSize}}"
                 patternUnits="userSpaceOnUse">
            <path d="M {{model.gridSize}},0 L 0,0 L 0,{{model.gridSize}}"
                  fill="none" stroke="#e0e0e0" stroke-width="1"/>
        </pattern>
    </defs>

    <rect x="{{model.chartX}}" y="{{model.chartY}}"
          width="{{model.chartWidth}}" height="{{model.chartHeight}}"
          fill="url(#chartGrid)" stroke="#2c3e50" stroke-width="2"/>

    <!-- Chart content would go here -->
</svg>
```

---

## Notes

### objectBoundingBox (Default)

**Characteristics:**
- Pattern tile dimensions relative to shape's bounding box
- Values: 0-1 (decimal) or 0%-100% (percentage)
- (0, 0) = top-left of shape, (1, 1) = bottom-right of shape
- Pattern scales automatically with shape size
- Most common for decorative patterns

**How it works:**
- width="0.25" means pattern repeats 4 times horizontally (1 / 0.25 = 4)
- width="0.5" means pattern repeats 2 times horizontally (1 / 0.5 = 2)
- Tile count = 1 / pattern dimension
- Pattern content should use patternContentUnits for sizing

```html
<!-- Pattern tiles 5 times across, 10 times down -->
<pattern width="0.2" height="0.1" patternUnits="objectBoundingBox">
    <circle cx="10" cy="5" r="3" fill="blue"/>
</pattern>
```

**Advantages:**
- Automatically adapts to shape size
- Same pattern works for different sized shapes
- Predictable repetition count
- Good for decorative fills
- Percentages are intuitive

**Limitations:**
- Pattern content sizing can be tricky
- May not align perfectly across different shapes
- Requires understanding of bounding box coordinates
- Can't create pixel-perfect patterns

**Use cases:**
- Decorative background patterns
- Texture fills that scale with shapes
- Responsive UI element backgrounds
- Scalable pattern designs

### userSpaceOnUse

**Characteristics:**
- Pattern tile dimensions in absolute document units
- Values: pt, px, mm, cm, in
- Fixed size in SVG coordinate system
- Pattern doesn't scale with shape
- Useful for consistent tiling

**How it works:**
- width="30" means each tile is 30 units wide
- Pattern repeats at fixed intervals
- Independent of shape dimensions
- More predictable for detailed patterns

```html
<!-- Pattern tiles every 40 pixels -->
<pattern x="0" y="0" width="40" height="40" patternUnits="userSpaceOnUse">
    <rect width="20" height="20" fill="lightblue"/>
    <rect x="20" y="20" width="20" height="20" fill="lightblue"/>
</pattern>
```

**Advantages:**
- Precise control over tile size
- Consistent appearance across shapes
- Easier to design detailed patterns
- Pixel-perfect alignment possible
- Simpler coordinate calculations

**Limitations:**
- Doesn't adapt to shape size automatically
- May tile differently on different sized shapes
- Requires knowing exact dimensions
- Less flexible for responsive designs

**Use cases:**
- Grid backgrounds for charts
- Consistent texture patterns
- Technical diagrams
- Fixed-size decorative elements
- Pixel-art patterns

### Choosing the Right System

**Use objectBoundingBox when:**
- Pattern should scale with shape
- Creating decorative fills
- Building responsive patterns
- Repetition count matters more than tile size
- Working with variable-sized shapes

**Use userSpaceOnUse when:**
- Pattern should be consistent across shapes
- Creating technical backgrounds (grids, graph paper)
- Pixel-perfect patterns are needed
- Tile size matters more than repetition count
- Building chart backgrounds or data visualizations

### Pattern Tile Calculations

**objectBoundingBox:**
```
Tile count horizontal = 1 / width
Tile count vertical = 1 / height

Examples:
width="0.25" → 4 tiles horizontally
width="0.1" → 10 tiles horizontally
width="0.5" → 2 tiles horizontally
```

**userSpaceOnUse:**
```
Tile count horizontal = shape width / pattern width
Tile count vertical = shape height / pattern height

Examples:
Shape 200px wide, pattern width="40" → 5 tiles
Shape 300px wide, pattern width="50" → 6 tiles
```

### Interaction with patternContentUnits

- `patternUnits` affects pattern tile positioning
- `patternContentUnits` affects pattern content sizing
- These can be set independently
- Common combinations:
  - Both objectBoundingBox: fully relative
  - Both userSpaceOnUse: fully absolute
  - Mixed: advanced control over scaling

### Performance Considerations

- Simple patterns are more efficient
- Large patterns can increase file size
- Consider pattern complexity vs. repetition
- Reuse pattern definitions when possible
- Test rendering performance with many patterns

### Common Pitfalls

**Pattern doesn't repeat as expected:**
- Check width/height values
- Verify patternUnits setting
- Ensure pattern dimensions are appropriate

**Pattern appears distorted:**
- May need to match patternUnits with patternContentUnits
- Check aspect ratio of pattern tile
- Verify shape dimensions

**Pattern too large/small:**
- Adjust width/height values
- Consider switching coordinate systems
- Check if patternContentUnits affects appearance

---

## Examples

### Object Bounding Box (Relative)

```html
<svg width="500" height="300">
    <defs>
        <pattern id="dots-relative" x="0" y="0" width="0.1" height="0.1"
                 patternUnits="objectBoundingBox">
            <circle cx="5" cy="5" r="3" fill="#3498db"/>
        </pattern>
    </defs>

    <!-- Pattern scales with different sized shapes -->
    <rect x="50" y="50" width="180" height="100" fill="url(#dots-relative)" stroke="#2c3e50"/>
    <rect x="270" y="50" width="180" height="200" fill="url(#dots-relative)" stroke="#2c3e50"/>
</svg>
```

### User Space on Use (Absolute)

```html
<svg width="500" height="300">
    <defs>
        <pattern id="grid-absolute" x="0" y="0" width="30" height="30"
                 patternUnits="userSpaceOnUse">
            <path d="M 30,0 L 0,0 L 0,30" fill="none" stroke="#bdc3c7" stroke-width="1"/>
        </pattern>
    </defs>

    <!-- Pattern maintains fixed size across different shapes -->
    <rect x="50" y="50" width="180" height="100" fill="url(#grid-absolute)" stroke="#2c3e50"/>
    <rect x="270" y="50" width="180" height="200" fill="url(#grid-absolute)" stroke="#2c3e50"/>
</svg>
```

### Side-by-Side Comparison

```html
<svg width="600" height="400">
    <defs>
        <!-- Relative: scales with shape -->
        <pattern id="rel-pattern" x="0" y="0" width="0.15" height="0.15"
                 patternUnits="objectBoundingBox">
            <circle cx="7.5" cy="7.5" r="5" fill="#3498db" opacity="0.5"/>
        </pattern>

        <!-- Absolute: fixed size -->
        <pattern id="abs-pattern" x="0" y="0" width="30" height="30"
                 patternUnits="userSpaceOnUse">
            <circle cx="15" cy="15" r="10" fill="#e74c3c" opacity="0.5"/>
        </pattern>
    </defs>

    <text x="150" y="30" text-anchor="middle" font-weight="bold">objectBoundingBox</text>
    <rect x="50" y="50" width="100" height="80" fill="url(#rel-pattern)" stroke="#2c3e50"/>
    <rect x="170" y="50" width="180" height="80" fill="url(#rel-pattern)" stroke="#2c3e50"/>

    <text x="450" y="30" text-anchor="middle" font-weight="bold">userSpaceOnUse</text>
    <rect x="350" y="50" width="100" height="80" fill="url(#abs-pattern)" stroke="#2c3e50"/>
    <rect x="470" y="50" width="180" height="80" fill="url(#abs-pattern)" stroke="#2c3e50"/>

    <rect x="50" y="220" width="100" height="150" fill="url(#rel-pattern)" stroke="#2c3e50"/>
    <rect x="170" y="220" width="180" height="150" fill="url(#rel-pattern)" stroke="#2c3e50"/>
    <rect x="350" y="220" width="100" height="150" fill="url(#abs-pattern)" stroke="#2c3e50"/>
    <rect x="470" y="220" width="180" height="150" fill="url(#abs-pattern)" stroke="#2c3e50"/>
</svg>
```

### Checkerboard Pattern (Absolute)

```html
<svg width="400" height="300">
    <defs>
        <pattern id="checkerboard" x="0" y="0" width="40" height="40"
                 patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="20" height="20" fill="#ecf0f1"/>
            <rect x="20" y="0" width="20" height="20" fill="#bdc3c7"/>
            <rect x="0" y="20" width="20" height="20" fill="#bdc3c7"/>
            <rect x="20" y="20" width="20" height="20" fill="#ecf0f1"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#checkerboard)" stroke="#2c3e50"/>
</svg>
```

### Diagonal Stripes (Relative)

```html
<svg width="400" height="300">
    <defs>
        <pattern id="stripes-rel" x="0" y="0" width="0.1" height="0.1"
                 patternUnits="objectBoundingBox"
                 patternTransform="rotate(45)">
            <rect x="0" y="0" width="5" height="10" fill="#3498db"/>
        </pattern>
    </defs>

    <circle cx="200" cy="150" r="100" fill="url(#stripes-rel)" stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Polka Dots (Relative)

```html
<svg width="500" height="300">
    <defs>
        <pattern id="polkadots" x="0" y="0" width="0.08" height="0.08"
                 patternUnits="objectBoundingBox">
            <circle cx="4" cy="4" r="3" fill="#e74c3c"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="200" rx="20"
          fill="url(#polkadots)" stroke="#c0392b" stroke-width="2"/>
</svg>
```

### Graph Paper (Absolute)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="graph-paper" x="0" y="0" width="50" height="50"
                 patternUnits="userSpaceOnUse">
            <!-- Major grid -->
            <path d="M 50,0 L 0,0 L 0,50" fill="none" stroke="#bdc3c7" stroke-width="1"/>
            <!-- Minor grid -->
            <path d="M 10,0 L 10,50 M 20,0 L 20,50 M 30,0 L 30,50 M 40,0 L 40,50"
                  fill="none" stroke="#ecf0f1" stroke-width="0.5"/>
            <path d="M 0,10 L 50,10 M 0,20 L 50,20 M 0,30 L 50,30 M 0,40 L 50,40"
                  fill="none" stroke="#ecf0f1" stroke-width="0.5"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300" fill="url(#graph-paper)" stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Hexagon Pattern (Absolute)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="hexagons" x="0" y="0" width="43.3" height="50"
                 patternUnits="userSpaceOnUse">
            <path d="M 21.65,0 L 43.3,12.5 L 43.3,37.5 L 21.65,50 L 0,37.5 L 0,12.5 Z"
                  fill="none" stroke="#3498db" stroke-width="2"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300" fill="url(#hexagons)"
          stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Star Pattern (Relative)

```html
<svg width="400" height="300">
    <defs>
        <pattern id="stars" x="0" y="0" width="0.12" height="0.12"
                 patternUnits="objectBoundingBox">
            <path d="M 6,1 L 7,5 L 11,5 L 8,7 L 9,11 L 6,9 L 3,11 L 4,7 L 1,5 L 5,5 Z"
                  fill="#f39c12"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#stars)"
          stroke="#e67e22" stroke-width="2"/>
</svg>
```

### Brick Pattern (Absolute)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="bricks" x="0" y="0" width="60" height="30"
                 patternUnits="userSpaceOnUse">
            <!-- First row -->
            <rect x="0" y="0" width="30" height="15" fill="#c0392b" stroke="white" stroke-width="1"/>
            <rect x="30" y="0" width="30" height="15" fill="#e74c3c" stroke="white" stroke-width="1"/>
            <!-- Second row (offset) -->
            <rect x="-15" y="15" width="30" height="15" fill="#e74c3c" stroke="white" stroke-width="1"/>
            <rect x="15" y="15" width="30" height="15" fill="#c0392b" stroke="white" stroke-width="1"/>
            <rect x="45" y="15" width="30" height="15" fill="#e74c3c" stroke="white" stroke-width="1"/>
        </pattern>
    </defs>

    <path d="M 100,350 L 100,150 L 250,100 L 400,150 L 400,350 Z"
          fill="url(#bricks)" stroke="#922b21" stroke-width="2"/>
</svg>
```

### Circuit Board Pattern (Absolute)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="circuit" x="0" y="0" width="80" height="80"
                 patternUnits="userSpaceOnUse">
            <rect width="80" height="80" fill="#2c3e50"/>
            <circle cx="10" cy="10" r="3" fill="#2ecc71"/>
            <circle cx="70" cy="10" r="3" fill="#2ecc71"/>
            <circle cx="10" cy="70" r="3" fill="#2ecc71"/>
            <circle cx="70" cy="70" r="3" fill="#2ecc71"/>
            <path d="M 10,10 L 70,10 L 70,70 L 10,70" fill="none" stroke="#3498db" stroke-width="2"/>
            <line x1="40" y1="10" x2="40" y2="70" stroke="#e74c3c" stroke-width="1.5"/>
            <line x1="10" y1="40" x2="70" y2="40" stroke="#e74c3c" stroke-width="1.5"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300" fill="url(#circuit)"
          stroke="#34495e" stroke-width="2"/>
</svg>
```

### Wave Pattern (Absolute)

```html
<svg width="500" height="300">
    <defs>
        <pattern id="waves" x="0" y="0" width="40" height="20"
                 patternUnits="userSpaceOnUse">
            <path d="M 0,10 Q 10,0 20,10 T 40,10" fill="none" stroke="#3498db" stroke-width="2"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="200" fill="url(#waves)"
          stroke="#2980b9" stroke-width="2"/>
</svg>
```

### Diamond Pattern (Relative)

```html
<svg width="400" height="300">
    <defs>
        <pattern id="diamonds" x="0" y="0" width="0.1" height="0.1"
                 patternUnits="objectBoundingBox">
            <path d="M 5,0 L 10,5 L 5,10 L 0,5 Z"
                  fill="none" stroke="#9b59b6" stroke-width="1.5"/>
        </pattern>
    </defs>

    <ellipse cx="200" cy="150" rx="150" ry="100"
             fill="url(#diamonds)" stroke="#8e44ad" stroke-width="2"/>
</svg>
```

### Crosshatch Pattern (Absolute)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="crosshatch" x="0" y="0" width="20" height="20"
                 patternUnits="userSpaceOnUse">
            <path d="M 0,0 L 20,20 M 20,0 L 0,20"
                  fill="none" stroke="#34495e" stroke-width="1"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300" fill="url(#crosshatch)"
          stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Texture with Multiple Patterns

```html
<svg width="600" height="400">
    <defs>
        <!-- Fine texture (relative) -->
        <pattern id="fine" x="0" y="0" width="0.05" height="0.05"
                 patternUnits="objectBoundingBox">
            <circle cx="2" cy="2" r="1" fill="#34495e" opacity="0.3"/>
        </pattern>

        <!-- Coarse texture (absolute) -->
        <pattern id="coarse" x="0" y="0" width="25" height="25"
                 patternUnits="userSpaceOnUse">
            <circle cx="12.5" cy="12.5" r="8" fill="#2c3e50" opacity="0.2"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="250" height="300" fill="url(#fine)"
          stroke="#2c3e50" stroke-width="2"/>
    <text x="175" y="370" text-anchor="middle" font-size="12">Fine (Relative)</text>

    <rect x="320" y="50" width="250" height="300" fill="url(#coarse)"
          stroke="#2c3e50" stroke-width="2"/>
    <text x="445" y="370" text-anchor="middle" font-size="12">Coarse (Absolute)</text>
</svg>
```

### Offset Pattern for Variety

```html
<svg width="500" height="400">
    <defs>
        <pattern id="offset-dots" x="0" y="0" width="30" height="30"
                 patternUnits="userSpaceOnUse">
            <circle cx="7.5" cy="7.5" r="4" fill="#3498db"/>
            <circle cx="22.5" cy="22.5" r="4" fill="#e74c3c"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300" fill="url(#offset-dots)"
          stroke="#2c3e50" stroke-width="2"/>
</svg>
```

---

## See Also

- [pattern](/reference/svgtags/pattern.html) - Pattern definition element
- [patternContentUnits](/reference/svgattributes/patternContentUnits.html) - Pattern content coordinate system
- [patternTransform](/reference/svgattributes/patternTransform.html) - Pattern transformation
- [x](/reference/svgattributes/x.html), [y](/reference/svgattributes/y.html) - Pattern position
- [width](/reference/svgattributes/width.html), [height](/reference/svgattributes/height.html) - Pattern tile dimensions
- [gradientUnits](/reference/svgattributes/gradientUnits.html) - Similar attribute for gradients
- [Data Binding](/reference/binding/) - Data binding and expressions

---
