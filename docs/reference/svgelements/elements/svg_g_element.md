---
layout: default
title: g
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;g&gt; : The SVG Group Container

The `<g>` (group) element is a container used to group related SVG elements together. Groups allow you to apply transformations, styles, and attributes to multiple elements at once. All child elements inherit styles from the group, and transformations applied to the group affect all its children.

## Usage

The `<g>` element creates a logical grouping that:
- Organizes related SVG elements for better structure and maintainability
- Applies transformations (translate, rotate, scale) to multiple elements as a unit
- Defines shared styles that cascade to all child elements
- Can be referenced by `id` for reuse with `<use>` elements
- Supports conditional visibility to show/hide groups of elements
- Provides semantic structure for complex graphics
- Can be nested for hierarchical organization

```html
<svg width="300pt" height="200pt">
    <g id="circle-group" fill="blue" stroke="black" stroke-width="2">
        <circle cx="50" cy="100" r="30"/>
        <circle cx="150" cy="100" r="30"/>
        <circle cx="250" cy="100" r="30"/>
    </g>
</svg>
```

---

## Supported Attributes

### Identification Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the group. Used for referencing with `<use>` or internal links. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS/SVG styles applied to the group and inherited by children. |

### Transformation Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `transform` | string | Transformation operations: `translate(x,y)`, `rotate(angle)`, `scale(x,y)`, `skewX(angle)`, `skewY(angle)`, or `matrix(...)`. Multiple transformations space-separated. |
| `transform-origin` | string | Origin point for transformations. Default: "0 0" |

### Graphics Attributes (Inherited by Children)

| Attribute | Type | Description |
|-----------|------|-------------|
| `fill` | color | Fill color inherited by child shapes. Values: color names, hex (#RGB, #RRGGBB), `none` |
| `fill-opacity` | number | Opacity for fills (0.0 to 1.0). Default: 1.0 |
| `stroke` | color | Stroke color inherited by child shapes |
| `stroke-width` | Unit | Stroke width inherited by children |
| `stroke-opacity` | number | Opacity for strokes (0.0 to 1.0). Default: 1.0 |
| `stroke-linecap` | string | Line cap style: `butt`, `round`, `square` |
| `stroke-linejoin` | string | Line join style: `miter`, `round`, `bevel` |
| `stroke-dasharray` | string | Dash pattern: "5,5" for dashed, "10,5,2,5" for complex patterns |

### Typography Attributes (Inherited by Text)

| Attribute | Type | Description |
|-----------|------|-------------|
| `font-family` | string | Font family inherited by text elements |
| `font-size` | Unit | Font size inherited by text elements |
| `font-weight` | number | Font weight (100-900) or names (normal, bold) |
| `font-style` | string | Font style: `normal`, `italic`, `oblique` |

### Content Elements

| Element | Description |
|---------|-------------|
| `title` | Accessible title/description for the group |
| `desc` | Longer description of the group content |
| Content | Any SVG shape, text, group, or container elements |

### Visibility Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `hidden` | string | Controls visibility. Set to "hidden" to hide the group and all children. |
| `display` | string | Display mode: `inline`, `block`, `none` |

---

## Notes

### Inheritance and Cascading

Groups establish a style inheritance hierarchy:

1. **Style Cascade**: Attributes set on a `<g>` element cascade to all descendants
2. **Override Priority**: Child elements can override inherited styles
3. **Transform Accumulation**: Nested groups accumulate transformations
4. **No Visual Rendering**: The `<g>` element itself has no visual representation

### Transformation Behavior

Transformations on groups affect all children:

```html
<g transform="translate(50,50) rotate(45)">
    <!-- All children are moved and rotated together -->
</g>
```

**Transformation order matters**:
- `translate(50,50) rotate(45)`: Move, then rotate around new origin
- `rotate(45) translate(50,50)`: Rotate, then move in rotated direction

**Nested transformations accumulate**:
```html
<g transform="translate(100,100)">
    <g transform="rotate(45)">
        <!-- Elements here are translated AND rotated -->
    </g>
</g>
```

### Reusable Groups

Groups can be defined and reused:

```html
<svg width="300pt" height="200pt">
    <defs>
        <g id="tree">
            <rect x="-5" y="0" width="10" height="30" fill="brown"/>
            <circle cx="0" cy="-10" r="20" fill="green"/>
        </g>
    </defs>
    <use href="#tree" x="50" y="150"/>
    <use href="#tree" x="150" y="150"/>
    <use href="#tree" x="250" y="150"/>
</svg>
```

### Layout Engine

In the Scryber codebase:
- `SVGGroup` extends `SVGBase` implements `IPDFViewPortComponent`
- Uses `LayoutEngineSVG` for rendering child content
- Does not clip content (overflow-action: none)
- Never splits across pages (overflow-split: never)
- Children maintain their drawing order (painter's algorithm)

### Cloning Support

SVGGroup implements `ICloneable`:
- Can be deep-cloned with all children
- Used by `<use>` element for referencing
- Preserves all styles and transformations

---

## Data Binding

### Dynamic Group Visibility

```html
<!-- Model: { showDetails: true } -->
<svg width="300pt" height="200pt">
    <g id="summary">
        <rect width="300" height="50" fill="#336699"/>
        <text x="150" y="30" text-anchor="middle" fill="white">Summary</text>
    </g>
    <g id="details" hidden="{{model.showDetails ? '' : 'hidden'}}">
        <rect y="60" width="300" height="140" fill="#e8f4f8"/>
        <text x="150" y="100" text-anchor="middle">Detail Information</text>
    </g>
</svg>
```

### Repeated Groups

```html
<!-- Model: { icons: [{x:50, icon:"check"}, {x:150, icon:"star"}] } -->
<svg width="300pt" height="100pt">
    <template data-bind="{{model.icons}}">
        <g transform="translate({{.x}}, 50)">
            <circle r="20" fill="#336699"/>
            <text y="5" text-anchor="middle" fill="white">{{.icon}}</text>
        </g>
    </template>
</svg>
```

### Dynamic Transformations

```html
<!-- Model: { rotation: 45, scale: 1.5 } -->
<svg width="200pt" height="200pt">
    <g transform="translate(100,100) rotate({{model.rotation}}) scale({{model.scale}})">
        <rect x="-25" y="-25" width="50" height="50" fill="#336699"/>
    </g>
</svg>
```

### Data-Driven Styling

```html
<!-- Model: { groups: [{color:"red", items:[...]}, {color:"blue", items:[...]}] } -->
<svg width="400pt" height="200pt">
    <template data-bind="{{model.groups}}">
        <g fill="{{.color}}" transform="translate({{$index * 200}}, 0)">
            <template data-bind="{{.items}}">
                <circle cx="100" cy="{{50 + $index * 40}}" r="15"/>
            </template>
        </g>
    </template>
</svg>
```

---

## Examples

### Basic Group with Shared Styling

```html
<svg width="300pt" height="100pt">
    <g fill="blue" stroke="black" stroke-width="2">
        <circle cx="50" cy="50" r="20"/>
        <rect x="100" y="30" width="40" height="40"/>
        <polygon points="200,30 220,70 180,70" fill="red"/>
    </g>
</svg>
```

### Group with Translation

```html
<svg width="300pt" height="200pt">
    <g transform="translate(50, 50)">
        <rect x="0" y="0" width="60" height="60" fill="#336699"/>
        <text x="30" y="35" text-anchor="middle" fill="white">Box</text>
    </g>
    <g transform="translate(150, 50)">
        <rect x="0" y="0" width="60" height="60" fill="#ff6347"/>
        <text x="30" y="35" text-anchor="middle" fill="white">Box</text>
    </g>
</svg>
```

### Group with Rotation

```html
<svg width="200pt" height="200pt">
    <g transform="translate(100,100)">
        <g transform="rotate(0)">
            <rect x="40" y="-5" width="40" height="10" fill="blue"/>
        </g>
        <g transform="rotate(45)">
            <rect x="40" y="-5" width="40" height="10" fill="red"/>
        </g>
        <g transform="rotate(90)">
            <rect x="40" y="-5" width="40" height="10" fill="green"/>
        </g>
        <g transform="rotate(135)">
            <rect x="40" y="-5" width="40" height="10" fill="orange"/>
        </g>
    </g>
</svg>
```

### Nested Groups

```html
<svg width="300pt" height="300pt">
    <g id="outer" fill="none" stroke="black" stroke-width="2">
        <circle cx="150" cy="150" r="100"/>
        <g id="middle" transform="translate(150,150)">
            <circle r="70" fill="lightblue" opacity="0.5"/>
            <g id="inner" transform="rotate(45)">
                <rect x="-30" y="-30" width="60" height="60" fill="yellow" opacity="0.7"/>
            </g>
        </g>
    </g>
</svg>
```

### Group with Scale Transformation

```html
<svg width="400pt" height="200pt">
    <g id="shape">
        <rect x="0" y="0" width="40" height="40" fill="#336699"/>
        <circle cx="20" cy="20" r="8" fill="white"/>
    </g>
    <g transform="translate(50,80)">
        <use href="#shape"/>
    </g>
    <g transform="translate(150,80) scale(1.5)">
        <use href="#shape"/>
    </g>
    <g transform="translate(280,80) scale(2)">
        <use href="#shape"/>
    </g>
</svg>
```

### Reusable Icon Groups

```html
<svg width="300pt" height="100pt">
    <defs>
        <g id="checkmark">
            <circle r="15" fill="#50c878" stroke="#2d7d4d" stroke-width="2"/>
            <path d="M -6,0 L -2,5 L 8,-8" stroke="white" stroke-width="3"
                  fill="none" stroke-linecap="round"/>
        </g>
        <g id="warning">
            <polygon points="0,-15 13,13 -13,13" fill="#ff9900" stroke="#cc7700" stroke-width="2"/>
            <text y="2" text-anchor="middle" font-size="20" font-weight="bold" fill="white">!</text>
        </g>
    </defs>
    <use href="#checkmark" x="75" y="50"/>
    <use href="#warning" x="225" y="50"/>
</svg>
```

### Grouped Chart Elements

```html
<svg width="400pt" height="300pt" viewBox="0 0 400 300">
    <!-- X-axis group -->
    <g id="x-axis" stroke="black" stroke-width="2">
        <line x1="50" y1="250" x2="350" y2="250"/>
        <text x="200" y="280" text-anchor="middle" font-size="14">Time</text>
    </g>

    <!-- Y-axis group -->
    <g id="y-axis" stroke="black" stroke-width="2">
        <line x1="50" y1="50" x2="50" y2="250"/>
        <text x="20" y="150" text-anchor="middle" font-size="14"
              transform="rotate(-90 20 150)">Value</text>
    </g>

    <!-- Data points group -->
    <g id="data" fill="blue">
        <circle cx="100" cy="200" r="5"/>
        <circle cx="150" cy="150" r="5"/>
        <circle cx="200" cy="100" r="5"/>
        <circle cx="250" cy="120" r="5"/>
        <circle cx="300" cy="80" r="5"/>
    </g>
</svg>
```

### Groups with Opacity

```html
<svg width="300pt" height="200pt">
    <g opacity="1.0" fill="red">
        <circle cx="75" cy="100" r="50"/>
    </g>
    <g opacity="0.7" fill="green">
        <circle cx="150" cy="100" r="50"/>
    </g>
    <g opacity="0.4" fill="blue">
        <circle cx="225" cy="100" r="50"/>
    </g>
</svg>
```

### Conditional Groups

```html
<!-- Model: { showGrid: true, showLabels: false } -->
<svg width="300pt" height="300pt">
    <g id="grid" stroke="#ddd" stroke-width="1" hidden="{{model.showGrid ? '' : 'hidden'}}">
        <line x1="0" y1="100" x2="300" y2="100"/>
        <line x1="0" y1="200" x2="300" y2="200"/>
        <line x1="100" y1="0" x2="100" y2="300"/>
        <line x1="200" y1="0" x2="200" y2="300"/>
    </g>
    <g id="labels" hidden="{{model.showLabels ? '' : 'hidden'}}">
        <text x="50" y="50">A</text>
        <text x="150" y="50">B</text>
        <text x="250" y="50">C</text>
    </g>
</svg>
```

### Layer Organization

```html
<svg width="400pt" height="300pt">
    <!-- Background layer -->
    <g id="background">
        <rect width="400" height="300" fill="#f0f0f0"/>
    </g>

    <!-- Content layer -->
    <g id="content">
        <rect x="50" y="50" width="300" height="200" fill="white"
              stroke="#ccc" stroke-width="2"/>
        <text x="200" y="150" text-anchor="middle" font-size="20">Content</text>
    </g>

    <!-- Overlay layer -->
    <g id="overlay" opacity="0.8">
        <circle cx="350" cy="50" r="30" fill="red"/>
        <text x="350" y="55" text-anchor="middle" fill="white"
              font-size="14" font-weight="bold">NEW</text>
    </g>
</svg>
```

### Animated Transformation (Static Result)

```html
<svg width="300pt" height="200pt">
    <g transform="translate(150,100)">
        <g id="spinner" stroke="#336699" stroke-width="4" fill="none">
            <path d="M 0,-40 A 40,40 0 0,1 28,-28" stroke-linecap="round"/>
        </g>
    </g>
</svg>
```

### Complex Icon with Multiple Groups

```html
<svg width="100pt" height="100pt" viewBox="0 0 100 100">
    <g id="home-icon">
        <!-- House body -->
        <g id="body" fill="#336699">
            <rect x="25" y="45" width="50" height="45"/>
        </g>
        <!-- Roof -->
        <g id="roof" fill="#254a70">
            <polygon points="50,20 15,50 85,50"/>
        </g>
        <!-- Door -->
        <g id="door" fill="white">
            <rect x="42" y="60" width="16" height="30"/>
            <circle cx="54" cy="75" r="2" fill="black"/>
        </g>
        <!-- Windows -->
        <g id="windows" fill="lightblue" stroke="white" stroke-width="2">
            <rect x="32" y="52" width="12" height="12"/>
            <rect x="56" y="52" width="12" height="12"/>
        </g>
    </g>
</svg>
```

### Data-Driven Bar Chart Groups

```html
<!-- Model: { data: [{label:"A", value:80, color:"#4a90e2"}, {label:"B", value:120, color:"#50c878"}] } -->
<svg width="400pt" height="300pt">
    <template data-bind="{{model.data}}">
        <g transform="translate({{50 + $index * 150}}, 250)">
            <rect x="0" y="{{-1 * .value}}" width="80" height="{{.value}}"
                  fill="{{.color}}" stroke="black" stroke-width="2"/>
            <text x="40" y="{{-1 * .value - 10}}" text-anchor="middle"
                  font-size="14" font-weight="bold">{{.value}}</text>
            <text x="40" y="25" text-anchor="middle" font-size="16">{{.label}}</text>
        </g>
    </template>
</svg>
```

### Grouped Pattern Elements

```html
<svg width="400pt" height="200pt">
    <defs>
        <g id="flower">
            <circle r="10" fill="pink"/>
            <circle cx="0" cy="-10" r="5" fill="red"/>
            <circle cx="10" cy="0" r="5" fill="red"/>
            <circle cx="0" cy="10" r="5" fill="red"/>
            <circle cx="-10" cy="0" r="5" fill="red"/>
            <circle r="3" fill="yellow"/>
        </g>
    </defs>
    <g>
        <use href="#flower" x="50" y="50"/>
        <use href="#flower" x="150" y="80"/>
        <use href="#flower" x="250" y="60"/>
        <use href="#flower" x="350" y="90"/>
        <use href="#flower" x="100" y="140"/>
        <use href="#flower" x="300" y="150"/>
    </g>
</svg>
```

### Group with Mixed Transformations

```html
<svg width="300pt" height="300pt">
    <g transform="translate(150,150)">
        <g id="arm1" transform="rotate(0)">
            <rect x="0" y="-5" width="80" height="10" fill="#336699"/>
            <circle cx="80" cy="0" r="8" fill="#ff6347"/>
        </g>
        <g id="arm2" transform="rotate(60)">
            <rect x="0" y="-5" width="80" height="10" fill="#336699"/>
            <circle cx="80" cy="0" r="8" fill="#ff6347"/>
        </g>
        <g id="arm3" transform="rotate(120)">
            <rect x="0" y="-5" width="80" height="10" fill="#336699"/>
            <circle cx="80" cy="0" r="8" fill="#ff6347"/>
        </g>
        <g id="arm4" transform="rotate(180)">
            <rect x="0" y="-5" width="80" height="10" fill="#336699"/>
            <circle cx="80" cy="0" r="8" fill="#ff6347"/>
        </g>
        <g id="arm5" transform="rotate(240)">
            <rect x="0" y="-5" width="80" height="10" fill="#336699"/>
            <circle cx="80" cy="0" r="8" fill="#ff6347"/>
        </g>
        <g id="arm6" transform="rotate(300)">
            <rect x="0" y="-5" width="80" height="10" fill="#336699"/>
            <circle cx="80" cy="0" r="8" fill="#ff6347"/>
        </g>
        <circle r="12" fill="white" stroke="black" stroke-width="2"/>
    </g>
</svg>
```

### Grouped Legend

```html
<svg width="400pt" height="250pt">
    <!-- Chart area -->
    <rect x="50" y="50" width="300" height="150" fill="#f9f9f9" stroke="#ccc"/>

    <!-- Legend group -->
    <g id="legend" transform="translate(60, 220)">
        <g id="legend-item-1">
            <rect x="0" y="0" width="15" height="15" fill="#4a90e2"/>
            <text x="20" y="12" font-size="12">Series A</text>
        </g>
        <g id="legend-item-2" transform="translate(100, 0)">
            <rect x="0" y="0" width="15" height="15" fill="#50c878"/>
            <text x="20" y="12" font-size="12">Series B</text>
        </g>
        <g id="legend-item-3" transform="translate(200, 0)">
            <rect x="0" y="0" width="15" height="15" fill="#ff9900"/>
            <text x="20" y="12" font-size="12">Series C</text>
        </g>
    </g>
</svg>
```

### Network Diagram with Groups

```html
<svg width="400pt" height="300pt">
    <defs>
        <g id="node">
            <circle r="20" fill="#336699" stroke="#254a70" stroke-width="2"/>
            <circle r="8" fill="lightblue"/>
        </g>
    </defs>

    <!-- Connections group -->
    <g id="connections" stroke="#999" stroke-width="2">
        <line x1="100" y1="100" x2="200" y2="100"/>
        <line x1="200" y1="100" x2="300" y2="100"/>
        <line x1="200" y1="100" x2="200" y2="200"/>
    </g>

    <!-- Nodes group -->
    <g id="nodes">
        <g transform="translate(100,100)">
            <use href="#node"/>
            <text y="45" text-anchor="middle" font-size="12">Node 1</text>
        </g>
        <g transform="translate(200,100)">
            <use href="#node"/>
            <text y="45" text-anchor="middle" font-size="12">Node 2</text>
        </g>
        <g transform="translate(300,100)">
            <use href="#node"/>
            <text y="45" text-anchor="middle" font-size="12">Node 3</text>
        </g>
        <g transform="translate(200,200)">
            <use href="#node"/>
            <text y="45" text-anchor="middle" font-size="12">Node 4</text>
        </g>
    </g>
</svg>
```

---

## See Also

- [svg](/reference/svgtags/svg.html) - SVG canvas container
- [defs](/reference/svgtags/defs.html) - Definitions container for reusable elements
- [use](/reference/svgtags/use.html) - Reference and reuse defined elements
- [a](/reference/svgtags/a.html) - SVG anchor/link element
- [SVG Shapes](/reference/svgtags/shapes.html) - rect, circle, ellipse, line, polyline, polygon, path
- [SVG Transformations](/reference/svgtags/transforms.html) - Transform operations guide
- [Data Binding](/reference/binding/) - Data binding and expressions
- [CSS Styles](/reference/styles/) - CSS styling reference

---
