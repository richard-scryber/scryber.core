---
layout: default
title: patternContentUnits
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @patternContentUnits : The Pattern Content Coordinate System Attribute

The `patternContentUnits` attribute defines the coordinate system for the pattern's content elements (shapes, paths, text inside the pattern). It determines whether the pattern content uses relative coordinates or absolute document coordinates.

## Usage

The `patternContentUnits` attribute is used to:
- Define coordinate system for pattern content
- Create patterns with relative content sizing
- Build patterns with absolute content dimensions
- Support data-driven content coordinate selection
- Control how pattern graphics scale
- Separate pattern tile sizing from content sizing

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <!-- Content uses absolute coordinates (default) -->
        <pattern id="absolute-content" x="0" y="0" width="40" height="40"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <circle cx="20" cy="20" r="15" fill="#3498db"/>
        </pattern>

        <!-- Content uses relative coordinates -->
        <pattern id="relative-content" x="0" y="0" width="0.2" height="0.2"
                 patternUnits="objectBoundingBox"
                 patternContentUnits="objectBoundingBox">
            <circle cx="0.5" cy="0.5" r="0.4" fill="#e74c3c"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="150" height="100" fill="url(#absolute-content)" stroke="#2c3e50"/>
    <rect x="250" y="50" width="150" height="100" fill="url(#relative-content)" stroke="#2c3e50"/>
</svg>
```

---

## Supported Values

| Value | Description | Coordinates | Use Case |
|-------|-------------|-------------|----------|
| `userSpaceOnUse` | Absolute document units (default) | pt, px, mm, cm, in | Fixed-size pattern content |
| `objectBoundingBox` | Relative to shape | 0-1 or 0%-100% | Content that scales with shapes |

### Default Behavior

```html
<!-- These are equivalent (userSpaceOnUse is default) -->
<pattern id="p1" width="50" height="50">
    <circle cx="25" cy="25" r="20" fill="blue"/>
</pattern>
<pattern id="p2" width="50" height="50" patternContentUnits="userSpaceOnUse">
    <circle cx="25" cy="25" r="20" fill="blue"/>
</pattern>
```

---

## Supported Elements

The `patternContentUnits` attribute is supported on:

- **[&lt;pattern&gt;](/reference/svgtags/pattern.html)** - Pattern content coordinate system

---

## Data Binding

### Dynamic Content Coordinate System

Choose content coordinate system based on data:

```html
<!-- Model: { useRelativeContent: false, tileSize: 50, dotRadius: 18 } -->
<svg width="400" height="300">
    <defs>
        <pattern id="dynamicContent"
                 x="0" y="0" width="{{model.tileSize}}" height="{{model.tileSize}}"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="{{model.useRelativeContent ? 'objectBoundingBox' : 'userSpaceOnUse'}}">
            <circle cx="{{model.useRelativeContent ? '0.5' : model.tileSize/2}}"
                    cy="{{model.useRelativeContent ? '0.5' : model.tileSize/2}}"
                    r="{{model.useRelativeContent ? '0.3' : model.dotRadius}}"
                    fill="#3498db"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200"
          fill="url(#dynamicContent)" stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Adaptive Pattern Graphics

Switch between scaling and fixed-size content:

```html
<!-- Model: { patterns: [{id: 'small', scaleContent: true, size: 40}, {id: 'large', scaleContent: false, size: 60}] } -->
<svg width="500" height="400">
    <defs>
        <template data-bind="{{model.patterns}}">
            <pattern id="pattern-{{.id}}"
                     x="0" y="0" width="{{.size}}" height="{{.size}}"
                     patternUnits="userSpaceOnUse"
                     patternContentUnits="{{.scaleContent ? 'objectBoundingBox' : 'userSpaceOnUse'}}">
                <rect x="{{.scaleContent ? '0' : '0'}}"
                      y="{{.scaleContent ? '0' : '0'}}"
                      width="{{.scaleContent ? '0.4' : .size * 0.4}}"
                      height="{{.scaleContent ? '0.4' : .size * 0.4}}"
                      fill="#3498db"/>
                <rect x="{{.scaleContent ? '0.6' : .size * 0.6}}"
                      y="{{.scaleContent ? '0.6' : .size * 0.6}}"
                      width="{{.scaleContent ? '0.4' : .size * 0.4}}"
                      height="{{.scaleContent ? '0.4' : .size * 0.4}}"
                      fill="#e74c3c"/>
            </pattern>
        </template>
    </defs>

    <rect x="50" y="50" width="200" height="300"
          fill="url(#pattern-small)" stroke="#2c3e50" stroke-width="2"/>
    <rect x="280" y="50" width="200" height="300"
          fill="url(#pattern-large)" stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Responsive Pattern Content

Create patterns where content adapts to filled shape size:

```html
<!-- Model: { shapes: [{x: 50, y: 50, width: 150, height: 100}, {x: 250, y: 50, width: 200, height: 150}] } -->
<svg width="500" height="250">
    <defs>
        <pattern id="responsive-pattern"
                 x="0" y="0" width="0.25" height="0.25"
                 patternUnits="objectBoundingBox"
                 patternContentUnits="objectBoundingBox">
            <circle cx="0.5" cy="0.5" r="0.35" fill="#9b59b6" opacity="0.5"/>
            <path d="M 0.3,0.3 L 0.7,0.7 M 0.7,0.3 L 0.3,0.7"
                  stroke="#8e44ad" stroke-width="0.05"/>
        </pattern>
    </defs>

    <template data-bind="{{model.shapes}}">
        <rect x="{{.x}}" y="{{.y}}" width="{{.width}}" height="{{.height}}"
              fill="url(#responsive-pattern)" stroke="#2c3e50" stroke-width="2"/>
    </template>
</svg>
```

### Data Visualization Patterns

Create consistent patterns for chart backgrounds:

```html
<!-- Model: { gridSize: 50, dotSize: 4, useFixedDots: true } -->
<svg width="500" height="400">
    <defs>
        <pattern id="chartPattern"
                 x="0" y="0" width="{{model.gridSize}}" height="{{model.gridSize}}"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="{{model.useFixedDots ? 'userSpaceOnUse' : 'objectBoundingBox'}}">
            <circle cx="{{model.useFixedDots ? model.gridSize/2 : '0.5'}}"
                    cy="{{model.useFixedDots ? model.gridSize/2 : '0.5'}}"
                    r="{{model.useFixedDots ? model.dotSize : '0.08'}}"
                    fill="#95a5a6" opacity="0.3"/>
            <path d="M {{model.gridSize}},0 L 0,0 L 0,{{model.gridSize}}"
                  fill="none" stroke="#bdc3c7" stroke-width="1"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300"
          fill="url(#chartPattern)" stroke="#2c3e50" stroke-width="2"/>
</svg>
```

---

## Notes

### userSpaceOnUse (Default)

**Characteristics:**
- Pattern content uses absolute document coordinates
- Values: pt, px, mm, cm, in
- Content maintains fixed size regardless of pattern tile size
- Default and most common setting
- Easier to design and understand

**How it works:**
- Content coordinates are in same space as document
- A circle with r="10" is always 10 units radius
- Content size independent of pattern dimensions
- Straightforward coordinate mapping

```html
<!-- Circle is always 20 units in radius -->
<pattern width="50" height="50" patternContentUnits="userSpaceOnUse">
    <circle cx="25" cy="25" r="20" fill="blue"/>
</pattern>
```

**Advantages:**
- Intuitive coordinate system
- Easy to design pattern content
- Precise control over content size
- Content size doesn't change with pattern tile
- Simpler to maintain

**Use cases:**
- Most pattern designs
- Fixed-size decorative elements
- Technical diagrams
- Grid patterns
- Icon-based patterns

### objectBoundingBox

**Characteristics:**
- Pattern content uses relative coordinates (0-1)
- Content scales with shape's bounding box
- More complex but powerful
- Coordinates relative to pattern tile
- Content adapts to pattern dimensions

**How it works:**
- 0-1 coordinate system for content
- (0, 0) = top-left of pattern tile
- (1, 1) = bottom-right of pattern tile
- Content scales with both pattern and shape
- Complex interaction with patternUnits

```html
<!-- Circle scales with pattern tile and shape -->
<pattern width="0.2" height="0.2"
         patternUnits="objectBoundingBox"
         patternContentUnits="objectBoundingBox">
    <circle cx="0.5" cy="0.5" r="0.4" fill="blue"/>
</pattern>
```

**Advantages:**
- Content scales proportionally
- Adapts to different sized patterns
- Unified relative coordinate system
- Good for responsive designs

**Limitations:**
- More complex to design
- Harder to predict exact sizes
- Requires understanding coordinate interactions
- Can be confusing with mixed coordinate systems

**Use cases:**
- Fully responsive patterns
- Content that should scale with shape
- Abstract geometric patterns
- When pattern tile size varies

### Interaction with patternUnits

The combination of `patternUnits` and `patternContentUnits` creates four possible scenarios:

**1. Both userSpaceOnUse (Most Common):**
```html
<pattern width="50" height="50"
         patternUnits="userSpaceOnUse"
         patternContentUnits="userSpaceOnUse">
    <circle cx="25" cy="25" r="20" fill="blue"/>
</pattern>
```
- Fixed pattern tile size
- Fixed content size
- Predictable, easy to design
- Most intuitive combination

**2. Both objectBoundingBox (Fully Relative):**
```html
<pattern width="0.2" height="0.2"
         patternUnits="objectBoundingBox"
         patternContentUnits="objectBoundingBox">
    <circle cx="0.5" cy="0.5" r="0.4" fill="blue"/>
</pattern>
```
- Pattern tiles scale with shape
- Content scales with pattern tiles
- Everything is relative
- Good for fully responsive patterns

**3. patternUnits: userSpaceOnUse, patternContentUnits: objectBoundingBox:**
```html
<pattern width="50" height="50"
         patternUnits="userSpaceOnUse"
         patternContentUnits="objectBoundingBox">
    <circle cx="0.5" cy="0.5" r="0.4" fill="blue"/>
</pattern>
```
- Fixed pattern tile size
- Content uses relative coordinates (0-1 within tile)
- Content scales with tile dimensions
- Useful for variable-sized tiles with proportional content

**4. patternUnits: objectBoundingBox, patternContentUnits: userSpaceOnUse:**
```html
<pattern width="0.2" height="0.2"
         patternUnits="objectBoundingBox"
         patternContentUnits="userSpaceOnUse">
    <circle cx="25" cy="25" r="20" fill="blue"/>
</pattern>
```
- Pattern tiles scale with shape
- Content uses absolute coordinates
- Can create unexpected results
- Less common, needs careful design

### Design Guidelines

**For userSpaceOnUse (Recommended):**
- Design in actual pixel/unit dimensions
- Use familiar coordinate system
- Center content: use (width/2, height/2)
- Content coordinates match pattern dimensions

**For objectBoundingBox (Advanced):**
- Design in 0-1 coordinate system
- Center content at (0.5, 0.5)
- Use relative sizes (e.g., r="0.3")
- Consider how content scales with tile

### Common Patterns

**Standard checkerboard:**
```html
<pattern width="40" height="40" patternContentUnits="userSpaceOnUse">
    <rect width="20" height="20" fill="white"/>
    <rect x="20" y="20" width="20" height="20" fill="white"/>
</pattern>
```

**Responsive dots:**
```html
<pattern width="0.1" height="0.1"
         patternUnits="objectBoundingBox"
         patternContentUnits="objectBoundingBox">
    <circle cx="0.5" cy="0.5" r="0.3" fill="blue"/>
</pattern>
```

### Performance Considerations

- userSpaceOnUse is generally simpler to render
- objectBoundingBox requires additional calculations
- Simple content performs better than complex
- Test with various shape sizes
- Consider browser compatibility

### Common Pitfalls

**Content doesn't appear:**
- Check coordinates are within pattern bounds
- Verify patternContentUnits matches coordinate values
- Ensure content size isn't zero

**Content appears too large/small:**
- Verify coordinate system (absolute vs. relative)
- Check pattern tile dimensions
- Adjust content size to fit tile

**Content stretches unexpectedly:**
- Review patternUnits vs. patternContentUnits interaction
- Consider using matching coordinate systems
- Check if objectBoundingBox is appropriate

---

## Examples

### User Space on Use (Default - Absolute Content)

```html
<svg width="400" height="300">
    <defs>
        <pattern id="absolute-dots" x="0" y="0" width="40" height="40"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <circle cx="20" cy="20" r="12" fill="#3498db"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200"
          fill="url(#absolute-dots)" stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Object Bounding Box (Relative Content)

```html
<svg width="400" height="300">
    <defs>
        <pattern id="relative-dots" x="0" y="0" width="0.15" height="0.15"
                 patternUnits="objectBoundingBox"
                 patternContentUnits="objectBoundingBox">
            <circle cx="0.5" cy="0.5" r="0.35" fill="#e74c3c"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200"
          fill="url(#relative-dots)" stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Side-by-Side Comparison

```html
<svg width="600" height="400">
    <defs>
        <!-- Absolute content coordinates -->
        <pattern id="abs-content" x="0" y="0" width="50" height="50"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <circle cx="25" cy="25" r="18" fill="#3498db" opacity="0.6"/>
            <path d="M 15,15 L 35,35 M 35,15 L 15,35" stroke="white" stroke-width="2"/>
        </pattern>

        <!-- Relative content coordinates -->
        <pattern id="rel-content" x="0" y="0" width="0.12" height="0.12"
                 patternUnits="objectBoundingBox"
                 patternContentUnits="objectBoundingBox">
            <circle cx="0.5" cy="0.5" r="0.4" fill="#e74c3c" opacity="0.6"/>
            <path d="M 0.3,0.3 L 0.7,0.7 M 0.7,0.3 L 0.3,0.7"
                  stroke="white" stroke-width="0.05"/>
        </pattern>
    </defs>

    <text x="150" y="30" text-anchor="middle" font-weight="bold">userSpaceOnUse Content</text>
    <rect x="50" y="50" width="200" height="150" fill="url(#abs-content)" stroke="#2c3e50"/>

    <text x="450" y="30" text-anchor="middle" font-weight="bold">objectBoundingBox Content</text>
    <rect x="350" y="50" width="200" height="150" fill="url(#rel-content)" stroke="#2c3e50"/>

    <rect x="50" y="220" width="200" height="150" fill="url(#abs-content)" stroke="#2c3e50"/>
    <rect x="350" y="220" width="200" height="150" fill="url(#rel-content)" stroke="#2c3e50"/>
</svg>
```

### Detailed Icon Pattern (Absolute Content)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="heart-pattern" x="0" y="0" width="60" height="60"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <path d="M 30,50 C 30,50 10,35 10,25 C 10,15 20,10 25,15 C 27,17 30,20 30,20
                     C 30,20 33,17 35,15 C 40,10 50,15 50,25 C 50,35 30,50 30,50 Z"
                  fill="#e74c3c" opacity="0.4"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300" fill="url(#heart-pattern)"
          stroke="#c0392b" stroke-width="2"/>
</svg>
```

### Geometric Pattern (Relative Content)

```html
<svg width="400" height="300">
    <defs>
        <pattern id="geometric" x="0" y="0" width="0.125" height="0.125"
                 patternUnits="objectBoundingBox"
                 patternContentUnits="objectBoundingBox">
            <rect x="0.1" y="0.1" width="0.8" height="0.8"
                  fill="none" stroke="#3498db" stroke-width="0.05"/>
            <circle cx="0.5" cy="0.5" r="0.2" fill="#e74c3c"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#geometric)"
          stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Mixed Coordinate Systems (Tile Absolute, Content Relative)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="mixed-pattern" x="0" y="0" width="80" height="80"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="objectBoundingBox">
            <!-- Content uses 0-1 coordinates relative to 80x80 tile -->
            <circle cx="0.5" cy="0.5" r="0.35" fill="#9b59b6" opacity="0.4"/>
            <rect x="0.2" y="0.2" width="0.6" height="0.6"
                  fill="none" stroke="#8e44ad" stroke-width="0.03"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300" fill="url(#mixed-pattern)"
          stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Checkerboard (Absolute Content)

```html
<svg width="400" height="300">
    <defs>
        <pattern id="checkerboard" x="0" y="0" width="50" height="50"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <rect x="0" y="0" width="25" height="25" fill="#ecf0f1"/>
            <rect x="25" y="0" width="25" height="25" fill="#bdc3c7"/>
            <rect x="0" y="25" width="25" height="25" fill="#bdc3c7"/>
            <rect x="25" y="25" width="25" height="25" fill="#ecf0f1"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#checkerboard)"
          stroke="#95a5a6" stroke-width="2"/>
</svg>
```

### Star Field (Absolute Content)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="stars" x="0" y="0" width="100" height="100"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <circle cx="20" cy="30" r="2" fill="white"/>
            <circle cx="60" cy="20" r="1.5" fill="white"/>
            <circle cx="80" cy="70" r="2.5" fill="white"/>
            <circle cx="30" cy="80" r="1" fill="white"/>
            <circle cx="70" cy="50" r="1.8" fill="white"/>
        </pattern>
    </defs>

    <rect x="0" y="0" width="500" height="400" fill="#2c3e50"/>
    <rect x="0" y="0" width="500" height="400" fill="url(#stars)"/>
</svg>
```

### Hexagon Grid (Absolute Content)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="hexagons" x="0" y="0" width="86.6" height="100"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <path d="M 43.3,0 L 86.6,25 L 86.6,75 L 43.3,100 L 0,75 L 0,25 Z"
                  fill="none" stroke="#3498db" stroke-width="2"/>
            <circle cx="43.3" cy="50" r="15" fill="#e74c3c" opacity="0.3"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300" fill="url(#hexagons)"
          stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Diagonal Lines (Absolute Content)

```html
<svg width="400" height="300">
    <defs>
        <pattern id="diagonals" x="0" y="0" width="20" height="20"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <path d="M 0,20 L 20,0 M -5,5 L 5,-5 M 15,25 L 25,15"
                  stroke="#95a5a6" stroke-width="2"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#diagonals)"
          stroke="#7f8c8d" stroke-width="2"/>
</svg>
```

### Polka Dots with Text (Absolute Content)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="numbered-dots" x="0" y="0" width="60" height="60"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <circle cx="30" cy="30" r="22" fill="#f39c12" opacity="0.6"/>
            <text x="30" y="36" text-anchor="middle" font-size="20"
                  font-weight="bold" fill="white">★</text>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300" fill="url(#numbered-dots)"
          stroke="#e67e22" stroke-width="2"/>
</svg>
```

### Tile Pattern (Relative Content - Fully Scalable)

```html
<svg width="500" height="300">
    <defs>
        <pattern id="tiles" x="0" y="0" width="0.16" height="0.16"
                 patternUnits="objectBoundingBox"
                 patternContentUnits="objectBoundingBox">
            <rect x="0.05" y="0.05" width="0.9" height="0.9"
                  fill="#3498db" stroke="white" stroke-width="0.05"/>
            <circle cx="0.5" cy="0.5" r="0.15" fill="white"/>
        </pattern>
    </defs>

    <!-- Pattern scales with both shapes -->
    <rect x="50" y="50" width="180" height="200" fill="url(#tiles)" stroke="#2980b9"/>
    <rect x="270" y="50" width="200" height="200" fill="url(#tiles)" stroke="#2980b9"/>
</svg>
```

### Complex Pattern with Multiple Elements (Absolute)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="complex" x="0" y="0" width="80" height="80"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <rect width="80" height="80" fill="#ecf0f1"/>
            <circle cx="40" cy="40" r="30" fill="none" stroke="#3498db" stroke-width="3"/>
            <rect x="25" y="25" width="30" height="30" fill="#e74c3c" opacity="0.5"/>
            <line x1="0" y1="0" x2="80" y2="80" stroke="#2ecc71" stroke-width="2"/>
            <line x1="80" y1="0" x2="0" y2="80" stroke="#f39c12" stroke-width="2"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300" fill="url(#complex)"
          stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Gradient-Filled Pattern (Absolute Content)

```html
<svg width="400" height="300">
    <defs>
        <linearGradient id="dot-gradient" x1="0%" y1="0%" x2="100%" y2="100%">
            <stop offset="0%" stop-color="#667eea"/>
            <stop offset="100%" stop-color="#764ba2"/>
        </linearGradient>

        <pattern id="gradient-dots" x="0" y="0" width="50" height="50"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <circle cx="25" cy="25" r="18" fill="url(#dot-gradient)"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200" fill="url(#gradient-dots)"
          stroke="#5a67d8" stroke-width="2"/>
</svg>
```

### Layered Pattern (Absolute Content)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="layered" x="0" y="0" width="60" height="60"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <!-- Background -->
            <rect width="60" height="60" fill="#ecf0f1"/>
            <!-- Layer 1 -->
            <circle cx="30" cy="30" r="25" fill="#3498db" opacity="0.3"/>
            <!-- Layer 2 -->
            <circle cx="30" cy="30" r="15" fill="#e74c3c" opacity="0.5"/>
            <!-- Layer 3 -->
            <circle cx="30" cy="30" r="8" fill="#2ecc71" opacity="0.7"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300" fill="url(#layered)"
          stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Confetti Pattern (Absolute Content)

```html
<svg width="500" height="400">
    <defs>
        <pattern id="confetti" x="0" y="0" width="100" height="100"
                 patternUnits="userSpaceOnUse"
                 patternContentUnits="userSpaceOnUse">
            <rect x="15" y="20" width="10" height="15" fill="#e74c3c" transform="rotate(25 20 27)"/>
            <circle cx="60" cy="30" r="6" fill="#3498db"/>
            <rect x="80" y="15" width="8" height="12" fill="#f39c12" transform="rotate(-15 84 21)"/>
            <circle cx="25" cy="70" r="5" fill="#2ecc71"/>
            <rect x="65" y="75" width="12" height="8" fill="#9b59b6" transform="rotate(45 71 79)"/>
            <circle cx="40" cy="50" r="4" fill="#1abc9c"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="400" height="300" fill="url(#confetti)"
          stroke="#2c3e50" stroke-width="2"/>
</svg>
```

---

## See Also

- [pattern](/reference/svgtags/pattern.html) - Pattern definition element
- [patternUnits](/reference/svgattributes/patternUnits.html) - Pattern tile coordinate system
- [patternTransform](/reference/svgattributes/patternTransform.html) - Pattern transformation
- [x](/reference/svgattributes/x.html), [y](/reference/svgattributes/y.html) - Pattern position
- [width](/reference/svgattributes/width.html), [height](/reference/svgattributes/height.html) - Pattern tile dimensions
- [Data Binding](/reference/binding/) - Data binding and expressions

---
