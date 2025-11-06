---
layout: default
title: stop
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;stop&gt; : The Gradient Color Stop Element

The `<stop>` element defines a color and position within a gradient definition. It must be used as a child element of `<linearGradient>` or `<radialGradient>` elements. Multiple stop elements create smooth color transitions at specified positions along the gradient.

## Usage

The `<stop>` element defines color stops that:
- Specify colors at precise positions within a gradient (0%-100%)
- Support full RGB/RGBA color specifications
- Allow opacity control for transparent gradients
- Can be styled via CSS classes and inline styles
- Support data binding for dynamic color values
- Are automatically sorted by offset position
- Require minimum 2 stops per gradient for color transitions
- Enable complex multi-color gradients with many stops

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="100">
    <defs>
        <linearGradient id="gradient">
            <stop offset="0%" stop-color="#3498db" stop-opacity="1" />
            <stop offset="50%" stop-color="#9b59b6" stop-opacity="0.8" />
            <stop offset="100%" stop-color="#e74c3c" stop-opacity="1" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#gradient)" />
</svg>
```

---

## Supported Attributes

### Standard Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Optional unique identifier for the stop element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the stop. |

### Color Stop Attributes

| Attribute | Type | Default | Description |
|-----------|------|---------|-------------|
| `offset` | Unit | **Required** | Position of the color stop along the gradient. Supports: 0-1, 0%-100%, or absolute units. |
| `stop-color` | Color | black | Color at this stop position. Supports: named colors, hex (#RGB, #RRGGBB), rgb(), rgba(). |
| `stop-opacity` | double | 1.0 | Opacity of the color at this stop. Range: 0.0 (transparent) to 1.0 (opaque). |

### CSS Style Support

Stop attributes can be set via CSS:

**Color Properties**:
- `stop-color` - Color value at this position
- `stop-opacity` - Transparency level (0-1)
- `offset` - Position along gradient (via attribute)

---

## Notes

### Offset Values

The `offset` attribute accepts multiple formats:

1. **Percentage** (most common): `0%` to `100%`
   ```html
   <stop offset="0%" stop-color="blue" />
   <stop offset="50%" stop-color="green" />
   <stop offset="100%" stop-color="red" />
   ```

2. **Decimal** (0 to 1): `0` to `1.0`
   ```html
   <stop offset="0" stop-color="blue" />
   <stop offset="0.5" stop-color="green" />
   <stop offset="1" stop-color="red" />
   ```

3. **Absolute units** (with userSpaceOnUse): pt, px, mm, cm, in
   ```html
   <stop offset="0pt" stop-color="blue" />
   <stop offset="50pt" stop-color="red" />
   ```

### Color Formats

The `stop-color` attribute supports multiple color formats:

```html
<!-- Named colors -->
<stop offset="0%" stop-color="blue" />
<stop offset="100%" stop-color="red" />

<!-- Hex colors (3-digit) -->
<stop offset="0%" stop-color="#39f" />

<!-- Hex colors (6-digit) -->
<stop offset="0%" stop-color="#3498db" />

<!-- RGB function -->
<stop offset="0%" stop-color="rgb(52, 152, 219)" />

<!-- RGBA function (opacity in color) -->
<stop offset="0%" stop-color="rgba(52, 152, 219, 0.8)" />
```

### Stop Ordering

Stops are processed by offset position:
- Automatically sorted from lowest to highest offset
- Declaration order in HTML doesn't affect gradient
- Duplicate offsets create hard color transitions
- Stops outside 0%-100% range work with spreadMethod

### Minimum Stop Requirements

Gradient stop requirements:
- **Minimum**: 2 stops (start and end colors)
- **Optimal**: 3-5 stops for smooth multi-color gradients
- **Maximum**: No practical limit (performance considerations apply)

### Opacity and Transparency

Two ways to control stop transparency:

1. **stop-opacity attribute**: Separate from color
   ```html
   <stop offset="50%" stop-color="#3498db" stop-opacity="0.5" />
   ```

2. **RGBA color**: Opacity in color value
   ```html
   <stop offset="50%" stop-color="rgba(52, 152, 219, 0.5)" />
   ```

Note: If both are specified, they multiply together.

### Hard Color Transitions

Create sharp color boundaries without blending:

```html
<!-- Duplicate offset creates instant color change -->
<stop offset="50%" stop-color="blue" />
<stop offset="50%" stop-color="red" />
```

### Gradient Patterns

Common stop patterns:

**Two-Color Blend**:
```html
<stop offset="0%" stop-color="blue" />
<stop offset="100%" stop-color="red" />
```

**Three-Color Gradient**:
```html
<stop offset="0%" stop-color="blue" />
<stop offset="50%" stop-color="green" />
<stop offset="100%" stop-color="red" />
```

**Fade to Transparent**:
```html
<stop offset="0%" stop-color="blue" stop-opacity="1" />
<stop offset="100%" stop-color="blue" stop-opacity="0" />
```

**Symmetric Gradient**:
```html
<stop offset="0%" stop-color="blue" />
<stop offset="50%" stop-color="white" />
<stop offset="100%" stop-color="blue" />
```

### Class Hierarchy

In the Scryber codebase:
- `SVGGradientStop` extends `Component`
- Implements `IStyledComponent` for CSS styling
- Implements `ICloneable` for stop duplication
- Cached styles for performance during layout
- Supports full data binding on all attributes

---

## Data Binding

### Dynamic Stop Colors

Bind stop colors to model data:

```html
<!-- Model: { primaryColor: "#3498db", accentColor: "#e74c3c" } -->
<linearGradient id="dataColors">
    <stop offset="0%" stop-color="{{model.primaryColor}}" />
    <stop offset="100%" stop-color="{{model.accentColor}}" />
</linearGradient>
```

### Dynamic Stop Positions

Create data-driven stop positions:

```html
<!-- Model: { midpoint: 30 } -->
<linearGradient id="dynamicPosition">
    <stop offset="0%" stop-color="#2ecc71" />
    <stop offset="{{model.midpoint}}%" stop-color="#f39c12" />
    <stop offset="100%" stop-color="#e74c3c" />
</linearGradient>
```

### Conditional Stop Colors

Set colors based on data conditions:

```html
<!-- Model: { status: "error", errorColor: "#e74c3c", successColor: "#2ecc71" } -->
<linearGradient id="statusGrad">
    <stop offset="0%" stop-color="white" />
    <stop offset="100%"
          stop-color="{{model.status === 'error' ? model.errorColor : model.successColor}}" />
</linearGradient>
```

### Dynamic Opacity

Control stop transparency with data:

```html
<!-- Model: { opacity: 0.7 } -->
<radialGradient id="glowEffect">
    <stop offset="0%" stop-color="#3498db" stop-opacity="{{model.opacity}}" />
    <stop offset="100%" stop-color="#3498db" stop-opacity="0" />
</radialGradient>
```

### Generate Stops from Array

Create multiple stops from data collections:

```html
<!-- Model: { colorStops: [
    {position: 0, color: "#e74c3c"},
    {position: 33, color: "#f39c12"},
    {position: 66, color: "#2ecc71"},
    {position: 100, color: "#3498db"}
] } -->
<linearGradient id="arrayStops">
    <template data-bind="{{model.colorStops}}">
        <stop offset="{{.position}}%" stop-color="{{.color}}" />
    </template>
</linearGradient>
```

### Data-Driven Color Scales

Create color scales from data ranges:

```html
<!-- Model: { values: [0, 25, 50, 75, 100], colors: ["#2ecc71", "#f1c40f", "#e67e22", "#e74c3c", "#c0392b"] } -->
<linearGradient id="heatmap">
    <template data-bind="{{model.values}}">
        <stop offset="{{.}}%"
              stop-color="{{model.colors[$index]}}" />
    </template>
</linearGradient>
```

---

## Examples

### Basic Two-Color Stop

```html
<svg width="400" height="100">
    <linearGradient id="simple">
        <stop offset="0%" stop-color="#3498db" />
        <stop offset="100%" stop-color="#e74c3c" />
    </linearGradient>
    <rect width="400" height="100" fill="url(#simple)" />
</svg>
```

### Three-Color Gradient

```html
<svg width="400" height="100">
    <linearGradient id="threeColor">
        <stop offset="0%" stop-color="#3498db" />
        <stop offset="50%" stop-color="#9b59b6" />
        <stop offset="100%" stop-color="#e74c3c" />
    </linearGradient>
    <rect width="400" height="100" fill="url(#threeColor)" />
</svg>
```

### Rainbow Gradient

```html
<svg width="400" height="100">
    <linearGradient id="rainbow">
        <stop offset="0%" stop-color="#e74c3c" />
        <stop offset="17%" stop-color="#f39c12" />
        <stop offset="33%" stop-color="#f1c40f" />
        <stop offset="50%" stop-color="#2ecc71" />
        <stop offset="67%" stop-color="#3498db" />
        <stop offset="83%" stop-color="#9b59b6" />
        <stop offset="100%" stop-color="#e74c3c" />
    </linearGradient>
    <rect width="400" height="100" fill="url(#rainbow)" />
</svg>
```

### Fade to Transparent

```html
<svg width="400" height="100">
    <linearGradient id="fadeOut">
        <stop offset="0%" stop-color="#3498db" stop-opacity="1" />
        <stop offset="70%" stop-color="#3498db" stop-opacity="0.5" />
        <stop offset="100%" stop-color="#3498db" stop-opacity="0" />
    </linearGradient>
    <rect width="400" height="100" fill="url(#fadeOut)" />
</svg>
```

### Hard Color Transition

```html
<svg width="400" height="100">
    <linearGradient id="sharp">
        <stop offset="0%" stop-color="#3498db" />
        <stop offset="50%" stop-color="#3498db" />
        <stop offset="50%" stop-color="#e74c3c" />
        <stop offset="100%" stop-color="#e74c3c" />
    </linearGradient>
    <rect width="400" height="100" fill="url(#sharp)" />
</svg>
```

### Symmetric Gradient

```html
<svg width="400" height="100">
    <linearGradient id="symmetric">
        <stop offset="0%" stop-color="#2c3e50" />
        <stop offset="50%" stop-color="#ecf0f1" />
        <stop offset="100%" stop-color="#2c3e50" />
    </linearGradient>
    <rect width="400" height="100" fill="url(#symmetric)" />
</svg>
```

### Gradient with Multiple Opacity Levels

```html
<svg width="400" height="100">
    <linearGradient id="multiOpacity">
        <stop offset="0%" stop-color="#3498db" stop-opacity="0" />
        <stop offset="25%" stop-color="#3498db" stop-opacity="0.5" />
        <stop offset="75%" stop-color="#3498db" stop-opacity="0.5" />
        <stop offset="100%" stop-color="#3498db" stop-opacity="0" />
    </linearGradient>
    <rect width="400" height="100" fill="url(#multiOpacity)" />
</svg>
```

### Heatmap Color Scale

```html
<svg width="400" height="100">
    <linearGradient id="heatmap">
        <stop offset="0%" stop-color="#2ecc71" />
        <stop offset="25%" stop-color="#f1c40f" />
        <stop offset="50%" stop-color="#e67e22" />
        <stop offset="75%" stop-color="#e74c3c" />
        <stop offset="100%" stop-color="#c0392b" />
    </linearGradient>
    <rect width="400" height="100" fill="url(#heatmap)" />
</svg>
```

### Traffic Light Gradient

```html
<svg width="400" height="100">
    <linearGradient id="traffic">
        <stop offset="0%" stop-color="#2ecc71" />
        <stop offset="33%" stop-color="#2ecc71" />
        <stop offset="33%" stop-color="#f1c40f" />
        <stop offset="67%" stop-color="#f1c40f" />
        <stop offset="67%" stop-color="#e74c3c" />
        <stop offset="100%" stop-color="#e74c3c" />
    </linearGradient>
    <rect width="400" height="100" fill="url(#traffic)" />
</svg>
```

### Ocean Depth Gradient

```html
<svg width="400" height="200">
    <linearGradient id="ocean" x1="0%" y1="0%" x2="0%" y2="100%">
        <stop offset="0%" stop-color="#e8f8f5" />
        <stop offset="30%" stop-color="#48c9b0" />
        <stop offset="60%" stop-color="#16a085" />
        <stop offset="100%" stop-color="#0e6655" />
    </linearGradient>
    <rect width="400" height="200" fill="url(#ocean)" />
</svg>
```

### Sunset Gradient

```html
<svg width="400" height="200">
    <linearGradient id="sunset" x1="0%" y1="0%" x2="0%" y2="100%">
        <stop offset="0%" stop-color="#ffeaa7" />
        <stop offset="30%" stop-color="#fdcb6e" />
        <stop offset="60%" stop-color="#e17055" />
        <stop offset="100%" stop-color="#d63031" />
    </linearGradient>
    <rect width="400" height="200" fill="url(#sunset)" />
</svg>
```

### Metallic Effect

```html
<svg width="400" height="100">
    <linearGradient id="metal" x1="0%" y1="0%" x2="0%" y2="100%">
        <stop offset="0%" stop-color="#95a5a6" />
        <stop offset="40%" stop-color="#ecf0f1" />
        <stop offset="60%" stop-color="#ecf0f1" />
        <stop offset="100%" stop-color="#7f8c8d" />
    </linearGradient>
    <rect width="400" height="100" fill="url(#metal)" />
</svg>
```

### Radial with Multiple Stops

```html
<svg width="300" height="300">
    <radialGradient id="radialMulti">
        <stop offset="0%" stop-color="#ffffff" />
        <stop offset="30%" stop-color="#f1c40f" />
        <stop offset="60%" stop-color="#e67e22" />
        <stop offset="80%" stop-color="#e74c3c" />
        <stop offset="100%" stop-color="#c0392b" />
    </radialGradient>
    <circle cx="150" cy="150" r="120" fill="url(#radialMulti)" />
</svg>
```

### Glow Effect with Stops

```html
<svg width="300" height="300">
    <radialGradient id="glowStops">
        <stop offset="0%" stop-color="#3498db" stop-opacity="1" />
        <stop offset="50%" stop-color="#3498db" stop-opacity="0.6" />
        <stop offset="80%" stop-color="#3498db" stop-opacity="0.2" />
        <stop offset="100%" stop-color="#3498db" stop-opacity="0" />
    </radialGradient>
    <circle cx="150" cy="150" r="120" fill="url(#glowStops)" />
</svg>
```

### Chart Color Scale

```html
<svg width="400" height="50">
    <linearGradient id="chartScale">
        <stop offset="0%" stop-color="#27ae60" />
        <stop offset="20%" stop-color="#2ecc71" />
        <stop offset="40%" stop-color="#f1c40f" />
        <stop offset="60%" stop-color="#f39c12" />
        <stop offset="80%" stop-color="#e67e22" />
        <stop offset="100%" stop-color="#e74c3c" />
    </linearGradient>
    <rect width="400" height="50" fill="url(#chartScale)" />
</svg>
```

### Data-Driven Status Colors

```html
<!-- Model: { level: 75, lowColor: "#2ecc71", midColor: "#f39c12", highColor: "#e74c3c" } -->
<svg width="400" height="100">
    <linearGradient id="statusLevel">
        <stop offset="0%"
              stop-color="{{model.level < 30 ? model.lowColor : model.level < 70 ? model.midColor : model.highColor}}" />
        <stop offset="100%"
              stop-color="{{model.level < 30 ? model.lowColor : model.level < 70 ? model.midColor : model.highColor}}"
              stop-opacity="0.5" />
    </linearGradient>
    <rect width="{{model.level * 4}}" height="100" fill="url(#statusLevel)" />
</svg>
```

### Temperature Scale with Data Stops

```html
<!-- Model: { temps: [-20, 0, 20, 40], colors: ["#3498db", "#2ecc71", "#f39c12", "#e74c3c"] } -->
<svg width="400" height="50">
    <linearGradient id="tempScale">
        <template data-bind="{{model.temps}}">
            <stop offset="{{($index / (model.temps.length - 1)) * 100}}%"
                  stop-color="{{model.colors[$index]}}" />
        </template>
    </linearGradient>
    <rect width="400" height="50" fill="url(#tempScale)" />
</svg>
```

### Dynamic Opacity Based on Value

```html
<!-- Model: { strength: 0.8 } -->
<svg width="300" height="300">
    <radialGradient id="strengthGrad">
        <stop offset="0%" stop-color="#3498db" stop-opacity="{{model.strength}}" />
        <stop offset="100%" stop-color="#3498db" stop-opacity="0" />
    </radialGradient>
    <circle cx="150" cy="150" r="120" fill="url(#strengthGrad)" />
</svg>
```

### Conditional Color Stops

```html
<!-- Model: { showMiddle: true, startColor: "#3498db", midColor: "#9b59b6", endColor: "#e74c3c" } -->
<svg width="400" height="100">
    <linearGradient id="conditional">
        <stop offset="0%" stop-color="{{model.startColor}}" />
        <stop hidden="{{!model.showMiddle ? 'hidden' : ''}}"
              offset="50%" stop-color="{{model.midColor}}" />
        <stop offset="100%" stop-color="{{model.endColor}}" />
    </linearGradient>
    <rect width="400" height="100" fill="url(#conditional)" />
</svg>
```

### Percentage-Based Color Stops

```html
<!-- Model: { percentage: 65, color: "#2ecc71" } -->
<svg width="400" height="100">
    <linearGradient id="percentGrad">
        <stop offset="0%" stop-color="{{model.color}}" stop-opacity="1" />
        <stop offset="{{model.percentage}}%" stop-color="{{model.color}}" stop-opacity="1" />
        <stop offset="{{model.percentage}}%" stop-color="{{model.color}}" stop-opacity="0.2" />
        <stop offset="100%" stop-color="{{model.color}}" stop-opacity="0.2" />
    </linearGradient>
    <rect width="400" height="100" fill="url(#percentGrad)" />
</svg>
```

---

## See Also

- [linearGradient](/reference/svgtags/linearGradient.html) - Linear gradient container element
- [radialGradient](/reference/svgtags/radialGradient.html) - Radial gradient container element
- [svg](/reference/htmltags/svg.html) - SVG canvas element
- [defs](/reference/svgtags/defs.html) - Definitions container for reusable elements
- [CSS Colors](/reference/styles/colors.html) - Color value formats and specifications
- [CSS Opacity](/reference/styles/opacity.html) - Transparency properties
- [Data Binding](/reference/binding/) - Data binding and expressions
- [SVG Fills](/reference/svg/fills.html) - SVG fill patterns and gradients

---
