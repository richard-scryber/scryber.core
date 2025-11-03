---
layout: default
title: ellipse
parent: SVG Elements
parent_url: /reference/svgelements/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;ellipse&gt; : SVG Ellipse Element

The `<ellipse>` element draws an oval or elliptical shape in SVG content within your PDF documents. It's perfect for creating ovals, rounded badges, icons, orbital diagrams, and decorative elements that require different horizontal and vertical radii.

## Usage

The `<ellipse>` element creates an elliptical shape defined by:
- Center position (`cx`, `cy`) - coordinates of the ellipse's center point
- Horizontal radius (`rx`) - distance from center to edge horizontally
- Vertical radius (`ry`) - distance from center to edge vertically
- Styling attributes for fill, stroke, and visual effects
- Transform operations for rotation and positioning

```html
<svg xmlns="http://www.w3.org/2000/svg" width="150" height="100">
    <ellipse cx="75" cy="50" rx="60" ry="35"
             fill="#9C27B0" stroke="#000" stroke-width="2"/>
</svg>
```

---

## Supported Attributes

### Geometry Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `cx` | Unit | X-coordinate of the ellipse's center. Default: 0 |
| `cy` | Unit | Y-coordinate of the ellipse's center. Default: 0 |
| `rx` | Unit | Horizontal radius (width). Required for visible output. |
| `ry` | Unit | Vertical radius (height). Required for visible output. |

### Styling Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `fill` | Color/URL | Fill color or reference to gradient/pattern (e.g., `#ff0000`, `rgb(255,0,0)`, `url(#gradient1)`). Default: black |
| `fill-opacity` | Number | Opacity of the fill (0.0 to 1.0). Default: 1.0 |
| `stroke` | Color | Stroke (border) color. Default: none |
| `stroke-width` | Unit | Width of the stroke line. Default: 1pt |
| `stroke-opacity` | Number | Opacity of the stroke (0.0 to 1.0). Default: 1.0 |
| `stroke-dasharray` | String | Dash pattern for elliptical borders (e.g., `5,3`) |

### Transform Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `transform` | String | SVG transform operations: `translate(x,y)`, `rotate(angle)`, `scale(x,y)`, `matrix(...)` |
| `transform-origin` | String | Origin point for transformations (e.g., `center`, `50% 50%`) |

### Common Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | String | Unique identifier for the element |
| `class` | String | CSS class name(s) for styling |
| `style` | String | Inline CSS styles |
| `title` | String | Title for accessibility and outline/bookmark |
| `hidden` | String | Set to "hidden" to hide the element |

---

## Data Binding

The `<ellipse>` element supports dynamic attribute values using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Size and Position

```html
<!-- Model: { oval: { cx: 100, cy: 60, rx: 80, ry: 45, color: '#FF5722' } } -->
<svg width="200" height="120">
    <ellipse cx="{{oval.cx}}" cy="{{oval.cy}}"
             rx="{{oval.rx}}" ry="{{oval.ry}}"
             fill="{{oval.color}}"/>
</svg>
```

### Example 2: Data-Driven Badges

```html
<!-- Model: { badge: { width: 50, height: 25, label: 'SALE' } } -->
<svg width="120" height="60">
    <ellipse cx="60" cy="30"
             rx="{{badge.width}}" ry="{{badge.height}}"
             fill="#f44336" stroke="#c62828" stroke-width="2"/>
</svg>
```

### Example 3: Repeating Ellipses

```html
<!-- Model: { bubbles: [
    {cx: 60, cy: 50, rx: 50, ry: 30, color: '#e74c3c'},
    {cx: 160, cy: 50, rx: 40, ry: 25, color: '#3498db'},
    {cx: 250, cy: 50, rx: 45, ry: 28, color: '#2ecc71'}
]} -->
<svg width="300" height="100">
    <template data-bind="{{bubbles}}">
        <ellipse cx="{{.cx}}" cy="{{.cy}}"
                 rx="{{.rx}}" ry="{{.ry}}"
                 fill="{{.color}}" fill-opacity="0.6"/>
    </template>
</svg>
```

---

## Notes

### Circle vs Ellipse

- When `rx` and `ry` are equal, the ellipse appears as a perfect circle
- Use `<circle>` for perfect circles (simpler syntax)
- Use `<ellipse>` when horizontal and vertical dimensions differ

### Center-Based Positioning

Like circles, ellipses are positioned by their center point, making radial arrangements and centered layouts intuitive.

### Radius Values

- Both `rx` and `ry` must be positive
- Zero values produce no visible output
- Different `rx` and `ry` values create the oval shape

### Rotation

Ellipses are commonly rotated using the `transform` attribute to create angled ovals:
```html
<ellipse cx="100" cy="50" rx="60" ry="30" transform="rotate(45, 100, 50)"/>
```

### Stroke Width

The stroke is drawn centered on the ellipse's outline, extending both inward and outward from the edge. Account for stroke width when calculating precise dimensions.

---

## Examples

### Basic Ellipse

```html
<svg width="150" height="100">
    <ellipse cx="75" cy="50" rx="60" ry="35" fill="#9C27B0"/>
</svg>
```

### Horizontal Oval

```html
<svg width="200" height="80">
    <ellipse cx="100" cy="40" rx="80" ry="25"
             fill="#fff" stroke="#2196F3" stroke-width="3"/>
</svg>
```

### Vertical Oval

```html
<svg width="100" height="150">
    <ellipse cx="50" cy="75" rx="30" ry="60"
             fill="#4CAF50"/>
</svg>
```

### Speech Bubble Background

```html
<svg width="200" height="100">
    <ellipse cx="100" cy="50" rx="90" ry="40"
             fill="#fff" stroke="#333" stroke-width="2"/>
</svg>
```

### Badge or Label

```html
<svg width="120" height="50">
    <ellipse cx="60" cy="25" rx="55" ry="20"
             fill="#f44336" stroke="#c62828" stroke-width="2"/>
</svg>
```

### Rotated Ellipse

```html
<svg width="150" height="150">
    <ellipse cx="75" cy="75" rx="60" ry="30"
             fill="#FF9800"
             transform="rotate(45, 75, 75)"/>
</svg>
```

### Orbital Diagram

```html
<svg width="200" height="200">
    <!-- Sun/center -->
    <circle cx="100" cy="100" r="20" fill="#FFC107"/>
    <!-- Inner orbit -->
    <ellipse cx="100" cy="100" rx="50" ry="50"
             fill="none" stroke="#ccc" stroke-width="1"
             stroke-dasharray="3,2"/>
    <!-- Outer orbit -->
    <ellipse cx="100" cy="100" rx="80" ry="80"
             fill="none" stroke="#ccc" stroke-width="1"
             stroke-dasharray="3,2"/>
</svg>
```

### Eye Icon

```html
<svg width="60" height="40">
    <!-- Eye outline -->
    <ellipse cx="30" cy="20" rx="25" ry="15"
             fill="none" stroke="#333" stroke-width="2"/>
    <!-- Pupil -->
    <circle cx="30" cy="20" r="7" fill="#333"/>
    <!-- Highlight -->
    <circle cx="28" cy="18" r="3" fill="#fff"/>
</svg>
```

### Cloud Shape

```html
<svg width="200" height="100">
    <ellipse cx="50" cy="60" rx="35" ry="25" fill="#90CAF9"/>
    <ellipse cx="90" cy="50" rx="45" ry="30" fill="#90CAF9"/>
    <ellipse cx="130" cy="55" rx="40" ry="28" fill="#90CAF9"/>
    <ellipse cx="160" cy="65" rx="30" ry="20" fill="#90CAF9"/>
</svg>
```

### Loading Ellipsis

```html
<svg width="120" height="40">
    <ellipse cx="20" cy="20" rx="12" ry="8" fill="#2196F3"/>
    <ellipse cx="60" cy="20" rx="12" ry="8" fill="#2196F3" opacity="0.6"/>
    <ellipse cx="100" cy="20" rx="12" ry="8" fill="#2196F3" opacity="0.3"/>
</svg>
```

### Concentric Ellipses

```html
<svg width="200" height="150">
    <ellipse cx="100" cy="75" rx="90" ry="65"
             fill="none" stroke="#e74c3c" stroke-width="2"/>
    <ellipse cx="100" cy="75" rx="70" ry="50"
             fill="none" stroke="#e74c3c" stroke-width="2"/>
    <ellipse cx="100" cy="75" rx="50" ry="35"
             fill="none" stroke="#e74c3c" stroke-width="2"/>
    <ellipse cx="100" cy="75" rx="30" ry="20"
             fill="#e74c3c"/>
</svg>
```

### Pill Button

```html
<svg width="150" height="50">
    <ellipse cx="25" cy="25" rx="20" ry="20" fill="#4CAF50"/>
    <rect x="25" y="5" width="100" height="40" fill="#4CAF50"/>
    <ellipse cx="125" cy="25" rx="20" ry="20" fill="#4CAF50"/>
</svg>
```

### Profile Avatar Frame

```html
<svg width="100" height="120">
    <ellipse cx="50" cy="60" rx="45" ry="55"
             fill="#E1BEE7" stroke="#9C27B0" stroke-width="3"/>
</svg>
```

### Magnifying Glass

```html
<svg width="80" height="80">
    <!-- Lens -->
    <ellipse cx="35" cy="35" rx="25" ry="25"
             fill="none" stroke="#333" stroke-width="4"/>
    <!-- Handle -->
    <line x1="54" y1="54" x2="70" y2="70"
          stroke="#333" stroke-width="4" stroke-linecap="round"/>
</svg>
```

### Venn Diagram with Ellipses

```html
<svg width="250" height="150">
    <ellipse cx="90" cy="75" rx="60" ry="45"
             fill="#e74c3c" fill-opacity="0.5"/>
    <ellipse cx="160" cy="75" rx="60" ry="45"
             fill="#3498db" fill-opacity="0.5"/>
</svg>
```

### Spotlight Effect

```html
<svg width="200" height="150">
    <ellipse cx="100" cy="75" rx="80" ry="60"
             fill="#FFF59D" fill-opacity="0.7"/>
    <ellipse cx="100" cy="75" rx="60" ry="45"
             fill="#FFEB3B" fill-opacity="0.8"/>
    <ellipse cx="100" cy="75" rx="40" ry="30"
             fill="#FDD835"/>
</svg>
```

### Perspective Circle (3D Effect)

```html
<svg width="150" height="100">
    <!-- Base (appears tilted) -->
    <ellipse cx="75" cy="75" rx="60" ry="20"
             fill="#424242" opacity="0.3"/>
    <!-- Cylinder top -->
    <ellipse cx="75" cy="35" rx="60" ry="20"
             fill="#2196F3" stroke="#1976D2" stroke-width="2"/>
</svg>
```

### Status Badge

```html
<svg width="100" height="40">
    <ellipse cx="50" cy="20" rx="45" ry="16"
             fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Gradient Fill Ellipse

```html
<svg width="200" height="120">
    <defs>
        <linearGradient id="ellipseGrad" x1="0%" y1="0%" x2="100%" y2="100%">
            <stop offset="0%" style="stop-color:#FF5722;stop-opacity:1"/>
            <stop offset="100%" style="stop-color:#FFC107;stop-opacity:1"/>
        </linearGradient>
    </defs>
    <ellipse cx="100" cy="60" rx="85" ry="50"
             fill="url(#ellipseGrad)"/>
</svg>
```

### Overlapping Ellipses Pattern

```html
<svg width="250" height="100">
    <ellipse cx="50" cy="50" rx="40" ry="30" fill="#e74c3c" opacity="0.5"/>
    <ellipse cx="90" cy="50" rx="40" ry="30" fill="#f39c12" opacity="0.5"/>
    <ellipse cx="130" cy="50" rx="40" ry="30" fill="#2ecc71" opacity="0.5"/>
    <ellipse cx="170" cy="50" rx="40" ry="30" fill="#3498db" opacity="0.5"/>
    <ellipse cx="210" cy="50" rx="40" ry="30" fill="#9b59b6" opacity="0.5"/>
</svg>
```

### Dynamic Size Ellipses

```html
<!-- Model: { sizes: [
    {cx: 60, size: 1.0, color: '#e74c3c'},
    {cx: 140, size: 0.7, color: '#f39c12'},
    {cx: 220, size: 0.5, color: '#2ecc71'}
]} -->
<svg width="280" height="100">
    <template data-bind="{{sizes}}">
        <ellipse cx="{{.cx}}" cy="50"
                 rx="{{50 * .size}}" ry="{{35 * .size}}"
                 fill="{{.color}}"/>
    </template>
</svg>
```

---

## See Also

- [circle](/reference/svgelements/circle.html) - SVG circle element for perfect circles
- [rect](/reference/svgelements/rect.html) - SVG rectangle element
- [polygon](/reference/svgelements/polygon.html) - SVG polygon element
- [path](/reference/svgelements/path.html) - SVG path element for complex shapes
- [svg](/reference/svgelements/svg.html) - SVG container element
- [SVG Gradients](/reference/svgelements/gradients.html) - Linear and radial gradients
- [SVG Transforms](/reference/svgelements/transforms.html) - Transformation operations
- [Data Binding](/reference/binding/) - Data binding and expressions

---
