---
layout: default
title: defs
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;defs&gt; : The SVG Definitions Container

The `<defs>` element is a container for defining reusable SVG resources that can be referenced throughout the document. Elements placed inside `<defs>` are not directly rendered but can be instantiated multiple times using the `<use>` element or referenced by other elements (gradients, patterns, masks, etc.).

## Usage

The `<defs>` element is used for:
- Defining reusable graphics components (shapes, groups, symbols)
- Creating gradient definitions (linear and radial gradients)
- Defining pattern fills for repetitive designs
- Storing clip paths and masks
- Organizing filter effects
- Creating markers for path endpoints
- Maintaining a library of icons or symbols
- Optimizing file size by reusing common elements

```html
<svg width="300pt" height="200pt">
    <defs>
        <linearGradient id="blueGradient">
            <stop offset="0%" stop-color="#4a90e2"/>
            <stop offset="100%" stop-color="#0066cc"/>
        </linearGradient>
        <circle id="dot" r="10" fill="red"/>
    </defs>
    <rect width="300" height="200" fill="url(#blueGradient)"/>
    <use href="#dot" x="50" y="50"/>
    <use href="#dot" x="150" y="100"/>
    <use href="#dot" x="250" y="150"/>
</svg>
```

---

## Supported Attributes

### Standard Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the defs container (rarely needed). |

### Content Elements

The `<defs>` element can contain any SVG element:

| Element Type | Examples | Purpose |
|--------------|----------|---------|
| **Shapes** | `<rect>`, `<circle>`, `<path>`, `<polygon>` | Reusable shape definitions |
| **Groups** | `<g>`, `<symbol>` | Complex reusable graphics |
| **Gradients** | `<linearGradient>`, `<radialGradient>` | Color gradient definitions |
| **Patterns** | `<pattern>` | Repeating pattern fills |
| **Filters** | `<filter>` | Visual effects |
| **Markers** | `<marker>` | Arrow heads and path decorations |
| **Clip Paths** | `<clipPath>` | Clipping regions |
| **Masks** | `<mask>` | Opacity masks |

---

## Notes

### Visibility Behavior

**Important characteristics**:
1. Elements inside `<defs>` are **not rendered** directly
2. They must be referenced using `href` or `url()` to appear
3. The `<defs>` container itself has no visual output
4. Multiple references share the same definition

### Placement in SVG

The `<defs>` element:
- Is typically placed near the beginning of an `<svg>` element
- Can appear anywhere within an `<svg>` container
- Can contain multiple definitions
- Can be nested within `<svg>` elements at different levels

```html
<svg width="400pt" height="300pt">
    <defs>
        <!-- Definitions here -->
    </defs>
    <!-- Content here -->
</svg>
```

### Referencing Definitions

Definitions are referenced by `id`:

**Using `<use>` element**:
```html
<use href="#definitionId" x="100" y="100"/>
```

**Using `url()` for fills/strokes**:
```html
<rect fill="url(#gradientId)" stroke="url(#patternId)"/>
```

**Using attribute references**:
```html
<path marker-end="url(#arrowId)"/>
```

### ID Uniqueness

- Each definition must have a unique `id` within the SVG canvas
- The `id` is used to reference the definition from other elements
- IDs are case-sensitive
- Use descriptive names for maintainability

### Performance Benefits

Using `<defs>` improves performance and reduces file size:
1. **Single Definition**: Complex graphics defined once
2. **Multiple Instances**: Referenced many times with minimal overhead
3. **Smaller Output**: PDF output contains one copy of the definition
4. **Faster Rendering**: Reuse is more efficient than duplication

### SVGCanvas Integration

In Scryber.Core:
- `SVGCanvas` has a `Definitions` property that holds the `<defs>` content
- Accessible via `canvas.Definitions` collection
- The `TryFindComponentByID` method searches definitions first
- Definitions are processed during document initialization

### Organization Best Practices

Organize definitions logically:

```html
<defs>
    <!-- Gradients -->
    <linearGradient id="grad1">...</linearGradient>
    <radialGradient id="grad2">...</radialGradient>

    <!-- Patterns -->
    <pattern id="dots">...</pattern>
    <pattern id="stripes">...</pattern>

    <!-- Reusable shapes -->
    <circle id="smallDot" r="3"/>
    <g id="icon">...</g>

    <!-- Markers -->
    <marker id="arrow">...</marker>
</defs>
```

---

## Data Binding

### Dynamic Definition Creation

```html
<!-- Model: { gradientColors: [{offset: "0%", color: "#ff0000"}, {offset: "100%", color: "#0000ff"}] } -->
<svg width="300pt" height="200pt">
    <defs>
        <linearGradient id="dynamicGrad">
            <template data-bind="{{model.gradientColors}}">
                <stop offset="{{.offset}}" stop-color="{{.color}}"/>
            </template>
        </linearGradient>
    </defs>
    <rect width="300" height="200" fill="url(#dynamicGrad)"/>
</svg>
```

### Conditional Definitions

```html
<!-- Model: { usePattern: true } -->
<svg width="300pt" height="200pt">
    <defs>
        <pattern id="customPattern" width="20" height="20" patternUnits="userSpaceOnUse"
                 hidden="{{model.usePattern ? '' : 'hidden'}}">
            <circle cx="10" cy="10" r="5" fill="blue"/>
        </pattern>
    </defs>
    <rect width="300" height="200"
          fill="{{model.usePattern ? 'url(#customPattern)' : '#f0f0f0'}}"/>
</svg>
```

### Data-Driven Icon Library

```html
<!-- Model: { icons: [{id:"check", path:"M-6,0 L-2,5 L8,-8"}, {id:"x", path:"M-5,-5 L5,5 M-5,5 L5,-5"}] } -->
<svg width="300pt" height="100pt">
    <defs>
        <template data-bind="{{model.icons}}">
            <g id="icon-{{.id}}">
                <circle r="15" fill="#336699"/>
                <path d="{{.path}}" stroke="white" stroke-width="2"
                      fill="none" stroke-linecap="round"/>
            </g>
        </template>
    </defs>
    <use href="#icon-check" x="75" y="50"/>
    <use href="#icon-x" x="225" y="50"/>
</svg>
```

---

## Examples

### Basic Gradient Definition

```html
<svg width="300pt" height="200pt">
    <defs>
        <linearGradient id="sunset" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#ff9900"/>
            <stop offset="50%" stop-color="#ff6347"/>
            <stop offset="100%" stop-color="#cc3366"/>
        </linearGradient>
    </defs>
    <rect width="300" height="200" fill="url(#sunset)"/>
</svg>
```

### Reusable Shape Definitions

```html
<svg width="400pt" height="200pt">
    <defs>
        <circle id="smallCircle" r="10" fill="blue"/>
        <rect id="smallSquare" width="20" height="20" fill="red"/>
        <polygon id="triangle" points="0,-15 13,10 -13,10" fill="green"/>
    </defs>
    <use href="#smallCircle" x="50" y="100"/>
    <use href="#smallCircle" x="100" y="100"/>
    <use href="#smallSquare" x="150" y="90"/>
    <use href="#smallSquare" x="180" y="90"/>
    <use href="#triangle" x="250" y="100"/>
    <use href="#triangle" x="300" y="100"/>
</svg>
```

### Pattern Fill Definition

```html
<svg width="300pt" height="200pt">
    <defs>
        <pattern id="checkerboard" x="0" y="0" width="40" height="40"
                 patternUnits="userSpaceOnUse">
            <rect width="20" height="20" fill="black"/>
            <rect x="20" y="0" width="20" height="20" fill="white"/>
            <rect x="0" y="20" width="20" height="20" fill="white"/>
            <rect x="20" y="20" width="20" height="20" fill="black"/>
        </pattern>
    </defs>
    <rect width="300" height="200" fill="url(#checkerboard)"/>
</svg>
```

### Multiple Gradient Definitions

```html
<svg width="400pt" height="300pt">
    <defs>
        <linearGradient id="horizontalGrad" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#4a90e2"/>
            <stop offset="100%" stop-color="#0066cc"/>
        </linearGradient>
        <linearGradient id="verticalGrad" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#50c878"/>
            <stop offset="100%" stop-color="#2d7d4d"/>
        </linearGradient>
        <radialGradient id="radialGrad" cx="50%" cy="50%" r="50%">
            <stop offset="0%" stop-color="#ffff00"/>
            <stop offset="100%" stop-color="#ff9900"/>
        </radialGradient>
    </defs>
    <rect x="10" y="10" width="180" height="130" fill="url(#horizontalGrad)"/>
    <rect x="210" y="10" width="180" height="130" fill="url(#verticalGrad)"/>
    <circle cx="200" cy="225" r="65" fill="url(#radialGrad)"/>
</svg>
```

### Icon Library

```html
<svg width="400pt" height="100pt">
    <defs>
        <!-- Success icon -->
        <g id="iconSuccess">
            <circle r="20" fill="#50c878" stroke="#2d7d4d" stroke-width="2"/>
            <path d="M -8,0 L -3,8 L 10,-10" stroke="white" stroke-width="4"
                  fill="none" stroke-linecap="round" stroke-linejoin="round"/>
        </g>

        <!-- Warning icon -->
        <g id="iconWarning">
            <polygon points="0,-18 16,16 -16,16" fill="#ff9900"
                     stroke="#cc7700" stroke-width="2"/>
            <text y="3" text-anchor="middle" font-size="24"
                  font-weight="bold" fill="white">!</text>
        </g>

        <!-- Error icon -->
        <g id="iconError">
            <circle r="20" fill="#ff4444" stroke="#cc0000" stroke-width="2"/>
            <line x1="-10" y1="-10" x2="10" y2="10" stroke="white"
                  stroke-width="4" stroke-linecap="round"/>
            <line x1="10" y1="-10" x2="-10" y2="10" stroke="white"
                  stroke-width="4" stroke-linecap="round"/>
        </g>

        <!-- Info icon -->
        <g id="iconInfo">
            <circle r="20" fill="#4a90e2" stroke="#0066cc" stroke-width="2"/>
            <text y="8" text-anchor="middle" font-size="26"
                  font-weight="bold" fill="white">i</text>
        </g>
    </defs>

    <use href="#iconSuccess" x="50" y="50"/>
    <use href="#iconWarning" x="150" y="50"/>
    <use href="#iconError" x="250" y="50"/>
    <use href="#iconInfo" x="350" y="50"/>
</svg>
```

### Complex Reusable Component

```html
<svg width="400pt" height="200pt">
    <defs>
        <g id="tree">
            <!-- Trunk -->
            <rect x="-8" y="30" width="16" height="40" fill="#8b4513"/>
            <!-- Foliage layers -->
            <polygon points="0,-20 -30,20 30,20" fill="#228b22"/>
            <polygon points="0,-10 -25,25 25,25" fill="#32cd32"/>
            <polygon points="0,0 -20,30 20,30" fill="#90ee90"/>
        </g>
    </defs>

    <!-- Multiple instances at different scales -->
    <use href="#tree" x="80" y="130" transform="scale(0.8)"/>
    <use href="#tree" x="200" y="130"/>
    <use href="#tree" x="320" y="130" transform="scale(1.2)"/>
</svg>
```

### Arrow Marker Definitions

```html
<svg width="300pt" height="200pt">
    <defs>
        <marker id="arrowStart" markerWidth="10" markerHeight="10"
                refX="0" refY="3" orient="auto" markerUnits="strokeWidth">
            <path d="M0,0 L0,6 L9,3 z" fill="#336699"/>
        </marker>
        <marker id="arrowEnd" markerWidth="10" markerHeight="10"
                refX="9" refY="3" orient="auto" markerUnits="strokeWidth">
            <path d="M0,0 L0,6 L9,3 z" fill="#336699"/>
        </marker>
        <marker id="dot" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#ff6347"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="250" y2="50"
          stroke="#336699" stroke-width="2"
          marker-start="url(#arrowStart)" marker-end="url(#arrowEnd)"/>
    <path d="M 50,100 Q 150,50 250,100"
          stroke="#336699" stroke-width="2" fill="none"
          marker-start="url(#dot)" marker-end="url(#dot)"/>
</svg>
```

### Symbol Definitions

```html
<svg width="400pt" height="200pt">
    <defs>
        <symbol id="userIcon" viewBox="0 0 50 50">
            <circle cx="25" cy="15" r="10" fill="#336699"/>
            <path d="M 10,50 Q 25,35 40,50" fill="#336699"/>
        </symbol>
        <symbol id="settingsIcon" viewBox="0 0 50 50">
            <circle cx="25" cy="25" r="8" fill="none"
                    stroke="#336699" stroke-width="3"/>
            <circle cx="25" cy="10" r="3" fill="#336699"/>
            <circle cx="40" cy="25" r="3" fill="#336699"/>
            <circle cx="25" cy="40" r="3" fill="#336699"/>
            <circle cx="10" cy="25" r="3" fill="#336699"/>
        </symbol>
    </defs>

    <use href="#userIcon" x="50" y="50" width="100" height="100"/>
    <use href="#settingsIcon" x="250" y="50" width="100" height="100"/>
</svg>
```

### Pattern Library

```html
<svg width="400pt" height="300pt">
    <defs>
        <!-- Dots pattern -->
        <pattern id="dots" x="0" y="0" width="20" height="20"
                 patternUnits="userSpaceOnUse">
            <circle cx="10" cy="10" r="3" fill="#336699"/>
        </pattern>

        <!-- Diagonal stripes -->
        <pattern id="diagonalStripes" x="0" y="0" width="10" height="10"
                 patternUnits="userSpaceOnUse" patternTransform="rotate(45)">
            <rect width="5" height="10" fill="#ff6347"/>
        </pattern>

        <!-- Grid pattern -->
        <pattern id="grid" x="0" y="0" width="20" height="20"
                 patternUnits="userSpaceOnUse">
            <path d="M 0,0 L 20,0 L 20,20 L 0,20 Z"
                  fill="none" stroke="#ddd" stroke-width="1"/>
        </pattern>

        <!-- Crosshatch -->
        <pattern id="crosshatch" x="0" y="0" width="8" height="8"
                 patternUnits="userSpaceOnUse">
            <path d="M 0,0 L 8,8 M 8,0 L 0,8"
                  stroke="#999" stroke-width="1"/>
        </pattern>
    </defs>

    <rect x="10" y="10" width="180" height="130" fill="url(#dots)"
          stroke="black" stroke-width="2"/>
    <rect x="210" y="10" width="180" height="130" fill="url(#diagonalStripes)"
          stroke="black" stroke-width="2"/>
    <rect x="10" y="160" width="180" height="130" fill="url(#grid)"
          stroke="black" stroke-width="2"/>
    <rect x="210" y="160" width="180" height="130" fill="url(#crosshatch)"
          stroke="black" stroke-width="2"/>
</svg>
```

### Clip Path Definition

```html
<svg width="300pt" height="200pt">
    <defs>
        <clipPath id="circleClip">
            <circle cx="150" cy="100" r="80"/>
        </clipPath>
        <clipPath id="starClip">
            <path d="M 150,20 L 170,80 L 230,80 L 180,120 L 200,180 L 150,140 L 100,180 L 120,120 L 70,80 L 130,80 Z"/>
        </clipPath>
    </defs>

    <rect width="300" height="200" fill="url(#sunset)" clip-path="url(#circleClip)"/>
</svg>
```

### Gradient with Multiple Stops

```html
<svg width="300pt" height="200pt">
    <defs>
        <linearGradient id="rainbowGrad" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="red"/>
            <stop offset="16.67%" stop-color="orange"/>
            <stop offset="33.33%" stop-color="yellow"/>
            <stop offset="50%" stop-color="green"/>
            <stop offset="66.67%" stop-color="blue"/>
            <stop offset="83.33%" stop-color="indigo"/>
            <stop offset="100%" stop-color="violet"/>
        </linearGradient>
    </defs>
    <rect width="300" height="200" fill="url(#rainbowGrad)"/>
</svg>
```

### Reusable Chart Components

```html
<svg width="400pt" height="300pt">
    <defs>
        <!-- Bar template -->
        <g id="barTemplate">
            <rect x="0" y="0" width="40" height="100" fill="#4a90e2"
                  stroke="#0066cc" stroke-width="2"/>
        </g>

        <!-- Axis template -->
        <g id="axis">
            <line x1="0" y1="0" x2="300" y2="0"
                  stroke="black" stroke-width="2"/>
        </g>

        <!-- Grid lines -->
        <g id="gridLine">
            <line x1="0" y1="0" x2="300" y2="0"
                  stroke="#ddd" stroke-width="1" stroke-dasharray="5,5"/>
        </g>
    </defs>

    <!-- Y-axis -->
    <g transform="translate(50,250) rotate(-90)">
        <use href="#axis"/>
    </g>

    <!-- X-axis -->
    <g transform="translate(50,250)">
        <use href="#axis"/>
    </g>

    <!-- Grid lines -->
    <use href="#gridLine" x="50" y="200"/>
    <use href="#gridLine" x="50" y="150"/>
    <use href="#gridLine" x="50" y="100"/>

    <!-- Bars -->
    <g transform="translate(70,150) scale(1,1)">
        <use href="#barTemplate"/>
    </g>
    <g transform="translate(130,120) scale(1,1.3)">
        <use href="#barTemplate"/>
    </g>
    <g transform="translate(190,100) scale(1,1.5)">
        <use href="#barTemplate"/>
    </g>
</svg>
```

### Dynamic Badge Definitions

```html
<!-- Model: { badges: [{id:"new", color:"#ff4444", text:"NEW"}, {id:"sale", color:"#50c878", text:"SALE"}] } -->
<svg width="400pt" height="200pt">
    <defs>
        <template data-bind="{{model.badges}}">
            <g id="badge-{{.id}}">
                <rect width="60" height="30" rx="5" fill="{{.color}}"/>
                <text x="30" y="20" text-anchor="middle" fill="white"
                      font-size="12" font-weight="bold">{{.text}}</text>
            </g>
        </template>
    </defs>

    <rect x="50" y="50" width="100" height="100" fill="#f0f0f0"/>
    <use href="#badge-new" x="100" y="60"/>

    <rect x="250" y="50" width="100" height="100" fill="#f0f0f0"/>
    <use href="#badge-sale" x="300" y="60"/>
</svg>
```

### Organizational Template

```html
<svg width="400pt" height="300pt">
    <defs>
        <!-- ========== GRADIENTS ========== -->
        <linearGradient id="headerGrad">
            <stop offset="0%" stop-color="#336699"/>
            <stop offset="100%" stop-color="#254a70"/>
        </linearGradient>

        <!-- ========== PATTERNS ========== -->
        <pattern id="backgroundPattern" width="10" height="10"
                 patternUnits="userSpaceOnUse">
            <circle cx="5" cy="5" r="1" fill="#e0e0e0"/>
        </pattern>

        <!-- ========== SHAPES ========== -->
        <circle id="bullet" r="5" fill="#336699"/>
        <rect id="divider" width="2" height="30" fill="#ccc"/>

        <!-- ========== ICONS ========== -->
        <g id="homeIcon">
            <rect x="-10" y="0" width="20" height="15" fill="#336699"/>
            <polygon points="0,-10 -15,0 15,0" fill="#254a70"/>
        </g>

        <!-- ========== FILTERS ========== -->
        <filter id="dropShadow">
            <feGaussianBlur in="SourceAlpha" stdDeviation="2"/>
            <feOffset dx="2" dy="2" result="offsetblur"/>
            <feMerge>
                <feMergeNode/>
                <feMergeNode in="SourceGraphic"/>
            </feMerge>
        </filter>
    </defs>

    <!-- Use the defined resources -->
    <rect width="400" height="60" fill="url(#headerGrad)"/>
    <rect y="60" width="400" height="240" fill="url(#backgroundPattern)"/>
    <use href="#homeIcon" x="200" y="150"/>
</svg>
```

---

## See Also

- [svg](/reference/svgtags/svg.html) - SVG canvas container
- [use](/reference/svgtags/use.html) - Reference and reuse defined elements
- [g](/reference/svgtags/g.html) - Group container for organizing elements
- [symbol](/reference/svgtags/symbol.html) - Reusable symbol definition
- [linearGradient](/reference/svgtags/lineargradient.html) - Linear gradient fill
- [radialGradient](/reference/svgtags/radialgradient.html) - Radial gradient fill
- [pattern](/reference/svgtags/pattern.html) - Pattern fill definition
- [marker](/reference/svgtags/marker.html) - Path marker definition
- [clipPath](/reference/svgtags/clippath.html) - Clipping path definition
- [Data Binding](/reference/binding/) - Data binding and expressions

---
