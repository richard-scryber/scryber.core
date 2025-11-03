---
layout: default
title: use
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;use&gt; : The SVG Reference Element

The `<use>` element references and reuses existing SVG elements defined elsewhere in the document, typically within a `<defs>` container. It creates instances of shapes, groups, or symbols by cloning the referenced element, allowing for efficient reuse of graphics with different positions, sizes, and styling.

## Usage

The `<use>` element enables:
- Creating multiple instances of a single definition
- Positioning instances independently using `x` and `y` attributes
- Scaling instances with `width` and `height` attributes
- Applying transformations to instances
- Overriding inherited styles on instances
- Building complex graphics from reusable components
- Reducing document size and improving maintainability
- Creating icon systems and component libraries

```html
<svg width="300pt" height="200pt">
    <defs>
        <circle id="dot" r="10" fill="blue"/>
        <g id="star">
            <polygon points="0,-10 2,-2 10,-2 4,2 6,10 0,5 -6,10 -4,2 -10,-2 -2,-2"
                     fill="gold" stroke="orange" stroke-width="1"/>
        </g>
    </defs>
    <use href="#dot" x="50" y="50"/>
    <use href="#dot" x="150" y="100"/>
    <use href="#star" x="250" y="150"/>
</svg>
```

---

## Supported Attributes

### Reference Attribute

| Attribute | Type | Description |
|-----------|------|-------------|
| `href` | string | **Required**. Reference to the element to clone. Must start with `#` followed by the target element's `id`. Example: `href="#myShape"` |

### Positioning Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `x` | Unit | Horizontal offset for the instance. Adds to the referenced element's position. Default: 0 |
| `y` | Unit | Vertical offset for the instance. Adds to the referenced element's position. Default: 0 |
| `width` | Unit | Width override for the instance. Only affects elements with intrinsic dimensions (like symbols). |
| `height` | Unit | Height override for the instance. Only affects elements with intrinsic dimensions. |

### Standard Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for this use instance. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS/SVG styles applied to the instance. Can override inherited styles. |

### Graphics Attributes (Override Inherited Styles)

| Attribute | Type | Description |
|-----------|------|-------------|
| `fill` | color | Fill color for the instance. Overrides the referenced element's fill. |
| `fill-opacity` | number | Opacity for fills (0.0 to 1.0) |
| `stroke` | color | Stroke color for the instance |
| `stroke-width` | Unit | Stroke width for the instance |
| `stroke-opacity` | number | Opacity for strokes (0.0 to 1.0) |
| `stroke-linecap` | string | Line cap style: `butt`, `round`, `square` |
| `stroke-linejoin` | string | Line join style: `miter`, `round`, `bevel` |
| `stroke-dasharray` | string | Dash pattern for strokes |

### Transformation Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `transform` | string | Transformation operations: `translate(x,y)`, `rotate(angle)`, `scale(x,y)`, etc. |
| `transform-origin` | string | Origin point for transformations |

### Corner Radius (for Shape Instances)

| Attribute | Type | Description |
|-----------|------|-------------|
| `rx` | Unit | Horizontal corner radius (affects rect instances) |
| `ry` | Unit | Vertical corner radius (affects rect instances) |

### Content Elements

| Element | Description |
|---------|-------------|
| `title` | Accessible title for the use instance |
| `desc` | Description of the use instance |

### Visibility Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `hidden` | string | Controls visibility. Set to "hidden" to hide. |
| `display` | string | Display mode: `inline`, `block`, `none` |

---

## Notes

### Reference Resolution

The `href` attribute must reference an element by ID:

1. **Internal References Only**: Currently only `#id` format is supported
2. **Search Order**: Scryber searches in `<defs>` first, then the document tree
3. **Must Exist**: The referenced element must exist when the use element is processed
4. **Case Sensitive**: IDs are case-sensitive

**Supported**:
```html
<use href="#myShape" x="100" y="100"/>
```

**Not Currently Supported**:
```html
<!-- External file references -->
<use href="shapes.svg#myShape" x="100" y="100"/>
<use href="http://example.com/shapes.svg#myShape" x="100" y="100"/>
```

### Cloning Behavior

When a `<use>` element is processed:

1. **Deep Clone**: The referenced element and all its children are cloned
2. **New ID**: The cloned instance receives a new generated ID
3. **Style Application**: Styles on the `<use>` element are applied to the clone
4. **Position Offset**: The `x` and `y` values offset the clone's position
5. **Transform Application**: Any transform on the `<use>` affects the entire cloned tree

### Position Offset Behavior

The `x` and `y` attributes add to various position properties:

- **Shapes with x/y**: Adds to the shape's x/y coordinates
- **Shapes with cx/cy**: Adds to the shape's cx/cy (center) coordinates
- **Shapes with x1/y1, x2/y2**: Adds to both coordinate pairs
- **Groups**: Creates a translation effect for all children

```html
<defs>
    <circle id="dot" cx="0" cy="0" r="10" fill="blue"/>
</defs>
<!-- The circle appears at (50,50), not (0,0) -->
<use href="#dot" x="50" y="50"/>
```

### Style Override Priority

Styles cascade with the following priority (highest to lowest):

1. **Direct styles on `<use>`**: Highest priority
2. **Styles on referenced element**: Medium priority
3. **Inherited parent styles**: Lowest priority

**Important**: Not all properties can be overridden. If a property is explicitly set on the cloned element, it may not be overridable via the `<use>` element.

### Width and Height Behavior

The `width` and `height` attributes:

- Only affect symbols and elements with intrinsic dimensions
- For shapes (circle, rect, etc.), use `transform` with `scale()` instead
- Provide a scaling mechanism for symbol instances

```html
<defs>
    <symbol id="icon" viewBox="0 0 100 100">
        <circle cx="50" cy="50" r="40"/>
    </symbol>
</defs>
<use href="#icon" x="10" y="10" width="50" height="50"/>
```

### Performance Considerations

Using `<use>` is highly efficient:

1. **Single Definition**: Complex graphics stored once in memory
2. **Multiple Instances**: References are lightweight
3. **PDF Optimization**: Output PDF contains one definition, multiple references
4. **Rendering Speed**: Faster than duplicating elements

### Class Hierarchy

In the Scryber codebase:
- `SVGUse` extends `PlaceHolder` implements `IPassThroughStyleContainer`, `IStyledComponent`
- Acts as a container that generates content during parsing
- The `CopiedComponent` property holds the cloned element
- Style application happens during layout, not parsing

### Cloneable Requirements

Referenced elements must implement `ICloneable`:
- All SVG shapes implement cloning
- Groups and paths are cloneable
- If an element cannot be cloned, a runtime error occurs

---

## Data Binding

### Dynamic References

```html
<!-- Model: { shapeId: "star", x: 100, y: 100 } -->
<svg width="300pt" height="200pt">
    <defs>
        <circle id="circle" r="20" fill="blue"/>
        <polygon id="star" points="0,-20 5,-5 20,-5 8,5 12,20 0,10 -12,20 -8,5 -20,-5 -5,-5" fill="gold"/>
    </defs>
    <use href="#{{model.shapeId}}" x="{{model.x}}" y="{{model.y}}"/>
</svg>
```

### Repeated Instances

```html
<!-- Model: { positions: [{x:50,y:50}, {x:150,y:100}, {x:250,y:50}] } -->
<svg width="300pt" height="200pt">
    <defs>
        <circle id="marker" r="15" fill="red" stroke="black" stroke-width="2"/>
    </defs>
    <template data-bind="{{model.positions}}">
        <use href="#marker" x="{{.x}}" y="{{.y}}"/>
    </template>
</svg>
```

### Dynamic Styling

```html
<!-- Model: { instances: [{x:50, color:"red"}, {x:150, color:"blue"}, {x:250, color:"green"}] } -->
<svg width="300pt" height="200pt">
    <defs>
        <circle id="dot" r="20"/>
    </defs>
    <template data-bind="{{model.instances}}">
        <use href="#dot" x="{{.x}}" y="100" fill="{{.color}}"/>
    </template>
</svg>
```

### Data-Driven Transformations

```html
<!-- Model: { icons: [{id:"check", x:75, scale:1}, {id:"star", x:225, scale:1.5}] } -->
<svg width="300pt" height="200pt">
    <defs>
        <g id="check">
            <circle r="15" fill="green"/>
            <path d="M-6,0 L-2,5 L8,-8" stroke="white" stroke-width="3" fill="none"/>
        </g>
        <g id="star">
            <polygon points="0,-15 4,-4 15,-4 6,2 9,13 0,7 -9,13 -6,2 -15,-4 -4,-4" fill="gold"/>
        </g>
    </defs>
    <template data-bind="{{model.icons}}">
        <use href="#{{.id}}" x="{{.x}}" y="100"
             transform="scale({{.scale}})"/>
    </template>
</svg>
```

---

## Examples

### Basic Use Reference

```html
<svg width="300pt" height="200pt">
    <defs>
        <circle id="blueDot" r="20" fill="blue"/>
    </defs>
    <use href="#blueDot" x="50" y="100"/>
    <use href="#blueDot" x="150" y="100"/>
    <use href="#blueDot" x="250" y="100"/>
</svg>
```

### Use with Style Override

```html
<svg width="300pt" height="200pt">
    <defs>
        <circle id="dot" r="20" fill="gray"/>
    </defs>
    <use href="#dot" x="50" y="100" fill="red"/>
    <use href="#dot" x="150" y="100" fill="green"/>
    <use href="#dot" x="250" y="100" fill="blue"/>
</svg>
```

### Reusing Complex Groups

```html
<svg width="400pt" height="300pt">
    <defs>
        <g id="tree">
            <rect x="-5" y="30" width="10" height="30" fill="#8b4513"/>
            <polygon points="0,0 -20,30 20,30" fill="#228b22"/>
            <polygon points="0,10 -15,35 15,35" fill="#32cd32"/>
        </g>
    </defs>
    <use href="#tree" x="100" y="220"/>
    <use href="#tree" x="200" y="220"/>
    <use href="#tree" x="300" y="220"/>
</svg>
```

### Use with Transformation

```html
<svg width="300pt" height="300pt">
    <defs>
        <rect id="square" width="40" height="40" fill="#336699"/>
    </defs>
    <use href="#square" x="150" y="150"/>
    <use href="#square" x="150" y="150" transform="rotate(15)"/>
    <use href="#square" x="150" y="150" transform="rotate(30)"/>
    <use href="#square" x="150" y="150" transform="rotate(45)"/>
    <use href="#square" x="150" y="150" transform="rotate(60)"/>
    <use href="#square" x="150" y="150" transform="rotate(75)"/>
</svg>
```

### Icon Pattern

```html
<svg width="400pt" height="200pt">
    <defs>
        <g id="icon">
            <circle r="15" fill="#4a90e2" stroke="#0066cc" stroke-width="2"/>
            <circle r="5" fill="white"/>
        </g>
    </defs>
    <use href="#icon" x="50" y="50"/>
    <use href="#icon" x="150" y="50"/>
    <use href="#icon" x="250" y="50"/>
    <use href="#icon" x="350" y="50"/>
    <use href="#icon" x="50" y="150"/>
    <use href="#icon" x="150" y="150"/>
    <use href="#icon" x="250" y="150"/>
    <use href="#icon" x="350" y="150"/>
</svg>
```

### Scaled Instances

```html
<svg width="400pt" height="200pt">
    <defs>
        <g id="shape">
            <rect width="30" height="30" fill="#ff6347"/>
            <circle cx="15" cy="15" r="8" fill="white"/>
        </g>
    </defs>
    <use href="#shape" x="50" y="100" transform="scale(0.5)"/>
    <use href="#shape" x="150" y="100" transform="scale(1)"/>
    <use href="#shape" x="250" y="100" transform="scale(1.5)"/>
    <use href="#shape" x="350" y="100" transform="scale(2)"/>
</svg>
```

### Grid of Icons

```html
<svg width="400pt" height="400pt">
    <defs>
        <g id="gridIcon">
            <rect width="30" height="30" fill="#e8f4f8" stroke="#336699" stroke-width="2"/>
            <circle cx="15" cy="15" r="5" fill="#336699"/>
        </g>
    </defs>
    <use href="#gridIcon" x="50" y="50"/>
    <use href="#gridIcon" x="100" y="50"/>
    <use href="#gridIcon" x="150" y="50"/>
    <use href="#gridIcon" x="200" y="50"/>
    <use href="#gridIcon" x="50" y="100"/>
    <use href="#gridIcon" x="100" y="100"/>
    <use href="#gridIcon" x="150" y="100"/>
    <use href="#gridIcon" x="200" y="100"/>
    <use href="#gridIcon" x="50" y="150"/>
    <use href="#gridIcon" x="100" y="150"/>
    <use href="#gridIcon" x="150" y="150"/>
    <use href="#gridIcon" x="200" y="150"/>
</svg>
```

### Use with Position Offset

```html
<svg width="300pt" height="200pt">
    <defs>
        <!-- Circle centered at origin -->
        <circle id="originCircle" cx="0" cy="0" r="20" fill="green"/>
    </defs>
    <!-- x and y offset the circle's center position -->
    <use href="#originCircle" x="50" y="100"/>
    <use href="#originCircle" x="150" y="100"/>
    <use href="#originCircle" x="250" y="100"/>
</svg>
```

### Reusing Path Definitions

```html
<svg width="400pt" height="200pt">
    <defs>
        <path id="wave" d="M 0,50 Q 25,0 50,50 T 100,50"
              fill="none" stroke="blue" stroke-width="3"/>
    </defs>
    <use href="#wave" x="0" y="50"/>
    <use href="#wave" x="150" y="50" stroke="red"/>
    <use href="#wave" x="0" y="150" stroke="green" stroke-width="5"/>
</svg>
```

### Building Complex Graphics

```html
<svg width="400pt" height="300pt">
    <defs>
        <!-- Component parts -->
        <circle id="wheel" r="20" fill="black" stroke="gray" stroke-width="3"/>
        <rect id="body" width="80" height="40" fill="red" stroke="darkred" stroke-width="2"/>
        <polygon id="roof" points="0,0 40,-20 80,0" fill="darkred"/>

        <!-- Assembled car -->
        <g id="car">
            <use href="#body" y="0"/>
            <use href="#roof" y="0"/>
            <use href="#wheel" x="20" y="35"/>
            <use href="#wheel" x="60" y="35"/>
        </g>
    </defs>

    <!-- Multiple car instances -->
    <use href="#car" x="50" y="100"/>
    <use href="#car" x="250" y="100" transform="scale(-1,1)"/>
</svg>
```

### Status Indicators

```html
<svg width="400pt" height="100pt">
    <defs>
        <g id="statusOk">
            <circle r="15" fill="#50c878"/>
            <path d="M-7,0 L-3,6 L9,-8" stroke="white" stroke-width="3"
                  fill="none" stroke-linecap="round"/>
        </g>
        <g id="statusWarning">
            <polygon points="0,-15 13,13 -13,13" fill="#ff9900"/>
            <text y="2" text-anchor="middle" font-size="16" font-weight="bold" fill="white">!</text>
        </g>
        <g id="statusError">
            <circle r="15" fill="#ff4444"/>
            <line x1="-7" y1="-7" x2="7" y2="7" stroke="white" stroke-width="3" stroke-linecap="round"/>
            <line x1="7" y1="-7" x2="-7" y2="7" stroke="white" stroke-width="3" stroke-linecap="round"/>
        </g>
    </defs>

    <use href="#statusOk" x="100" y="50"/>
    <use href="#statusWarning" x="200" y="50"/>
    <use href="#statusError" x="300" y="50"/>
</svg>
```

### Data-Driven Chart Points

```html
<!-- Model: { points: [{x:50, y:150, value:25}, {x:150, y:100, value:50}, {x:250, y:50, value:75}] } -->
<svg width="300pt" height="200pt">
    <defs>
        <g id="dataPoint">
            <circle r="8" fill="#4a90e2" stroke="#0066cc" stroke-width="2"/>
        </g>
    </defs>
    <line x1="30" y1="180" x2="280" y2="180" stroke="black" stroke-width="2"/>
    <line x1="30" y1="30" x2="30" y2="180" stroke="black" stroke-width="2"/>

    <template data-bind="{{model.points}}">
        <use href="#dataPoint" x="{{.x}}" y="{{.y}}"/>
        <text x="{{.x}}" y="{{.y - 15}}" text-anchor="middle" font-size="12">{{.value}}</text>
    </template>
</svg>
```

### Repeated Pattern with Variations

```html
<svg width="400pt" height="300pt">
    <defs>
        <g id="flower">
            <circle r="8" fill="pink"/>
            <circle cx="0" cy="-8" r="4" fill="red"/>
            <circle cx="8" cy="0" r="4" fill="red"/>
            <circle cx="0" cy="8" r="4" fill="red"/>
            <circle cx="-8" cy="0" r="4" fill="red"/>
            <circle r="2" fill="yellow"/>
        </g>
    </defs>

    <use href="#flower" x="50" y="50"/>
    <use href="#flower" x="150" y="80" transform="scale(1.2)"/>
    <use href="#flower" x="250" y="60" fill="lightblue"/>
    <use href="#flower" x="350" y="90" transform="scale(0.8)"/>
    <use href="#flower" x="100" y="200" transform="rotate(45)"/>
    <use href="#flower" x="200" y="220"/>
    <use href="#flower" x="300" y="190" transform="scale(1.5)"/>
</svg>
```

### Menu Icons Library

```html
<svg width="400pt" height="100pt">
    <defs>
        <g id="iconHome">
            <rect x="-10" y="0" width="20" height="15" fill="#336699"/>
            <polygon points="0,-12 -15,0 15,0" fill="#254a70"/>
        </g>
        <g id="iconSettings">
            <circle r="6" fill="none" stroke="#336699" stroke-width="2"/>
            <circle cy="-10" r="2" fill="#336699"/>
            <circle cx="8.66" cy="5" r="2" fill="#336699"/>
            <circle cx="-8.66" cy="5" r="2" fill="#336699"/>
        </g>
        <g id="iconUser">
            <circle cy="-5" r="5" fill="#336699"/>
            <path d="M-10,15 Q0,5 10,15" fill="#336699"/>
        </g>
        <g id="iconSearch">
            <circle r="8" fill="none" stroke="#336699" stroke-width="2"/>
            <line x1="6" y1="6" x2="12" y2="12" stroke="#336699" stroke-width="2" stroke-linecap="round"/>
        </g>
    </defs>

    <use href="#iconHome" x="50" y="50"/>
    <use href="#iconSettings" x="150" y="50"/>
    <use href="#iconUser" x="250" y="50"/>
    <use href="#iconSearch" x="350" y="50"/>
</svg>
```

### Decorative Border Pattern

```html
<svg width="400pt" height="300pt">
    <defs>
        <g id="corner">
            <circle r="8" fill="#336699"/>
            <circle r="4" fill="white"/>
        </g>
    </defs>

    <rect x="20" y="20" width="360" height="260"
          fill="none" stroke="#ccc" stroke-width="2"/>

    <!-- Corners -->
    <use href="#corner" x="20" y="20"/>
    <use href="#corner" x="380" y="20"/>
    <use href="#corner" x="20" y="280"/>
    <use href="#corner" x="380" y="280"/>
</svg>
```

### Conditional Icon Display

```html
<!-- Model: { status: "success", x: 100, y: 100 } -->
<svg width="300pt" height="200pt">
    <defs>
        <g id="success">
            <circle r="20" fill="#50c878"/>
            <path d="M-10,0 L-4,8 L12,-10" stroke="white" stroke-width="4" fill="none"/>
        </g>
        <g id="error">
            <circle r="20" fill="#ff4444"/>
            <line x1="-10" y1="-10" x2="10" y2="10" stroke="white" stroke-width="4"/>
            <line x1="10" y1="-10" x2="-10" y2="10" stroke="white" stroke-width="4"/>
        </g>
    </defs>

    <use href="#{{model.status}}" x="{{model.x}}" y="{{model.y}}"/>
</svg>
```

### Network Nodes

```html
<svg width="400pt" height="300pt">
    <defs>
        <g id="node">
            <circle r="25" fill="#4a90e2" stroke="#0066cc" stroke-width="3"/>
            <circle r="10" fill="white"/>
        </g>
    </defs>

    <!-- Connections -->
    <line x1="100" y1="100" x2="300" y2="100" stroke="#999" stroke-width="2"/>
    <line x1="100" y1="100" x2="200" y2="200" stroke="#999" stroke-width="2"/>
    <line x1="300" y1="100" x2="200" y2="200" stroke="#999" stroke-width="2"/>

    <!-- Nodes -->
    <use href="#node" x="100" y="100"/>
    <use href="#node" x="300" y="100"/>
    <use href="#node" x="200" y="200"/>
</svg>
```

### Game Sprites

```html
<svg width="400pt" height="200pt">
    <defs>
        <g id="enemy">
            <rect x="-15" y="-15" width="30" height="30" fill="red" rx="5"/>
            <circle cx="-7" cy="-5" r="3" fill="black"/>
            <circle cx="7" cy="-5" r="3" fill="black"/>
            <rect x="-10" y="5" width="20" height="3" fill="black"/>
        </g>
    </defs>

    <rect width="400" height="200" fill="#e8f4f8"/>
    <use href="#enemy" x="80" y="80"/>
    <use href="#enemy" x="200" y="120"/>
    <use href="#enemy" x="320" y="60"/>
</svg>
```

---

## See Also

- [defs](/reference/svgtags/defs.html) - Definitions container for reusable elements
- [svg](/reference/svgtags/svg.html) - SVG canvas container
- [g](/reference/svgtags/g.html) - Group container for organizing elements
- [symbol](/reference/svgtags/symbol.html) - Reusable symbol definition with viewBox
- [SVG Shapes](/reference/svgtags/shapes.html) - rect, circle, ellipse, line, polyline, polygon, path
- [SVG Transformations](/reference/svgtags/transforms.html) - Transform operations guide
- [Data Binding](/reference/binding/) - Data binding and expressions
- [CSS Styles](/reference/styles/) - CSS styling reference

---
