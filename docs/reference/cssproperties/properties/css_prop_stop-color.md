---
layout: default
title: stop-color
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# stop-color : SVG Gradient Stop Color Property

The `stop-color` property specifies the color value at a gradient stop within SVG gradient definitions. This property is essential for creating linear and radial gradients by defining the colors at specific positions along the gradient vector.

## Usage

```css
selector {
    stop-color: value;
}
```

The stop-color property accepts color values in various formats to define the color at a specific gradient stop.

---

## Supported Values

### Named Colors
Standard CSS color names such as `red`, `blue`, `green`, `black`, `orange`, etc.

### Hexadecimal Colors
- Short form: `#RGB` (e.g., `#f00` for red)
- Long form: `#RRGGBB` (e.g., `#ff0000` for red)

### RGB/RGBA Functions
- RGB: `rgb(red, green, blue)` where values are 0-255
- RGBA: `rgba(red, green, blue, alpha)` where alpha is 0.0-1.0

### HSL/HSLA Functions
- HSL: `hsl(hue, saturation%, lightness%)`
- HSLA: `hsla(hue, saturation%, lightness%, alpha)`

### Special Keywords
- `currentColor` - Uses the current color value of the element

---

## Supported Elements

The `stop-color` property applies to:
- `<stop>` elements within `<linearGradient>` definitions
- `<stop>` elements within `<radialGradient>` definitions

Note: This property only works on `<stop>` elements, not on shape elements directly.

---

## Notes

- Must be used within `<stop>` elements inside gradient definitions
- Each `<stop>` element defines a color at a specific offset (0% to 100%)
- Multiple stops create color transitions in gradients
- The `offset` attribute determines where the color appears in the gradient
- Gradients interpolate colors between stops
- Use with `stop-opacity` for transparent gradient stops
- Default stop-color is typically black if not specified
- Gradients are reusable and can be referenced by multiple elements
- Essential for creating smooth color transitions in backgrounds, fills, and shapes
- Vector gradients maintain quality at any zoom level in PDF viewers
- Can create complex multi-color gradients with multiple stops

---

## Data Binding

The `stop-color` property can be dynamically controlled through data binding, enabling data-driven gradient colors that respond to values, states, or categories. This is essential for creating dynamic visualizations with color-coded data representations.

### Example 1: Data-driven gradient colors for status indicators

```html
<style>
    .status-bar { fill: url(#statusGrad); }
</style>
<body>
    <svg width="400" height="150" xmlns="http://www.w3.org/2000/svg">
        <defs>
            <template data-bind="{{status}}">
                <linearGradient id="statusGrad" x1="0%" y1="0%" x2="100%" y2="0%">
                    <stop offset="0%" stop-color="{{startColor}}"/>
                    <stop offset="100%" stop-color="{{endColor}}"/>
                </linearGradient>
            </template>
        </defs>
        <rect class="status-bar" width="400" height="150"/>
    </svg>
</body>
```

### Example 2: Dynamic multi-stop gradients from data

```html
<style>
    .data-viz { fill: url(#dataGradient); }
</style>
<body>
    <svg width="500" height="300" xmlns="http://www.w3.org/2000/svg">
        <defs>
            <linearGradient id="dataGradient" x1="0%" y1="0%" x2="0%" y2="100%">
                <template data-bind="{{#each colorStops}}">
                    <stop offset="{{position}}%" stop-color="{{color}}"/>
                </template>
            </linearGradient>
        </defs>
        <rect class="data-viz" width="500" height="300"/>
    </svg>
</body>
```

### Example 3: Conditional gradient colors based on thresholds

```html
<style>
    .metric-bar { fill: url(#metricGrad); }
</style>
<body>
    <svg width="400" height="100" xmlns="http://www.w3.org/2000/svg">
        <defs>
            <template data-bind="{{metric}}">
                <linearGradient id="metricGrad" x1="0%" y1="0%" x2="100%" y2="0%">
                    <stop offset="0%" stop-color="{{#if value | lessThan: 30}}#ef4444{{else if value | lessThan: 70}}#f59e0b{{else}}#10b981{{/if}}"/>
                    <stop offset="100%" stop-color="{{#if value | lessThan: 30}}#dc2626{{else if value | lessThan: 70}}#d97706{{else}}#059669{{/if}}"/>
                </linearGradient>
            </template>
        </defs>
        <rect class="metric-bar" x="25" y="25" width="350" height="50" rx="25"/>
    </svg>
</body>
```

---

## Examples

### Example 1: Simple two-color linear gradient

```html
<style>
    .gradient-rect {
        fill: url(#blueGradient);
    }
</style>
<body>
    <svg width="300" height="150">
        <defs>
            <linearGradient id="blueGradient">
                <stop offset="0%" stop-color="#3b82f6"/>
                <stop offset="100%" stop-color="#1e40af"/>
            </linearGradient>
        </defs>
        <rect class="gradient-rect" x="25" y="25" width="250" height="100"/>
    </svg>
</body>
```

### Example 2: Three-color gradient

```html
<style>
    .tricolor {
        fill: url(#triGradient);
    }
</style>
<body>
    <svg width="300" height="150">
        <defs>
            <linearGradient id="triGradient">
                <stop offset="0%" stop-color="#ef4444"/>
                <stop offset="50%" stop-color="#fbbf24"/>
                <stop offset="100%" stop-color="#10b981"/>
            </linearGradient>
        </defs>
        <rect class="tricolor" x="25" y="25" width="250" height="100"/>
    </svg>
</body>
```

### Example 3: Radial gradient from center

```html
<style>
    .radial-circle {
        fill: url(#radialGrad);
    }
</style>
<body>
    <svg width="300" height="300">
        <defs>
            <radialGradient id="radialGrad">
                <stop offset="0%" stop-color="#fbbf24"/>
                <stop offset="100%" stop-color="#dc2626"/>
            </radialGradient>
        </defs>
        <circle class="radial-circle" cx="150" cy="150" r="120"/>
    </svg>
</body>
```

### Example 4: Sunset gradient with multiple stops

```html
<style>
    .sunset {
        fill: url(#sunsetGradient);
    }
</style>
<body>
    <svg width="400" height="250">
        <defs>
            <linearGradient id="sunsetGradient" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#1e3a8a"/>
                <stop offset="30%" stop-color="#7c3aed"/>
                <stop offset="50%" stop-color="#f97316"/>
                <stop offset="70%" stop-color="#fbbf24"/>
                <stop offset="100%" stop-color="#fef3c7"/>
            </linearGradient>
        </defs>
        <rect class="sunset" width="400" height="250"/>
    </svg>
</body>
```

### Example 5: Button with gradient background

```html
<style>
    .gradient-button {
        fill: url(#buttonGradient);
    }
</style>
<body>
    <svg width="250" height="100">
        <defs>
            <linearGradient id="buttonGradient" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#60a5fa"/>
                <stop offset="100%" stop-color="#3b82f6"/>
            </linearGradient>
        </defs>
        <rect class="gradient-button" x="25" y="25" width="200" height="50" rx="25"/>
    </svg>
</body>
```

### Example 6: Metal effect gradient

```html
<style>
    .metal {
        fill: url(#metalGradient);
    }
</style>
<body>
    <svg width="300" height="150">
        <defs>
            <linearGradient id="metalGradient" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#d1d5db"/>
                <stop offset="25%" stop-color="#f3f4f6"/>
                <stop offset="50%" stop-color="#9ca3af"/>
                <stop offset="75%" stop-color="#e5e7eb"/>
                <stop offset="100%" stop-color="#6b7280"/>
            </linearGradient>
        </defs>
        <rect class="metal" x="50" y="25" width="200" height="100" rx="10"/>
    </svg>
</body>
```

### Example 7: Rainbow gradient

```html
<style>
    .rainbow {
        fill: url(#rainbowGradient);
    }
</style>
<body>
    <svg width="400" height="100">
        <defs>
            <linearGradient id="rainbowGradient">
                <stop offset="0%" stop-color="#ef4444"/>
                <stop offset="17%" stop-color="#f97316"/>
                <stop offset="33%" stop-color="#fbbf24"/>
                <stop offset="50%" stop-color="#10b981"/>
                <stop offset="67%" stop-color="#3b82f6"/>
                <stop offset="83%" stop-color="#6366f1"/>
                <stop offset="100%" stop-color="#a855f7"/>
            </linearGradient>
        </defs>
        <rect class="rainbow" width="400" height="100"/>
    </svg>
</body>
```

### Example 8: Glossy highlight effect

```html
<style>
    .glossy {
        fill: url(#glossyGradient);
    }
</style>
<body>
    <svg width="250" height="250">
        <defs>
            <radialGradient id="glossyGradient" cx="40%" cy="40%">
                <stop offset="0%" stop-color="white"/>
                <stop offset="50%" stop-color="#3b82f6"/>
                <stop offset="100%" stop-color="#1e40af"/>
            </radialGradient>
        </defs>
        <circle class="glossy" cx="125" cy="125" r="100"/>
    </svg>
</body>
```

### Example 9: Water effect gradient

```html
<style>
    .water {
        fill: url(#waterGradient);
    }
</style>
<body>
    <svg width="400" height="250">
        <defs>
            <linearGradient id="waterGradient" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#06b6d4"/>
                <stop offset="50%" stop-color="#0891b2"/>
                <stop offset="100%" stop-color="#164e63"/>
            </linearGradient>
        </defs>
        <rect class="water" width="400" height="250"/>
    </svg>
</body>
```

### Example 10: Fire/flame gradient

```html
<style>
    .fire {
        fill: url(#fireGradient);
    }
</style>
<body>
    <svg width="300" height="300">
        <defs>
            <radialGradient id="fireGradient" cx="50%" cy="80%">
                <stop offset="0%" stop-color="#fef3c7"/>
                <stop offset="20%" stop-color="#fbbf24"/>
                <stop offset="50%" stop-color="#f97316"/>
                <stop offset="80%" stop-color="#dc2626"/>
                <stop offset="100%" stop-color="#7f1d1d"/>
            </radialGradient>
        </defs>
        <circle class="fire" cx="150" cy="150" r="120"/>
    </svg>
</body>
```

### Example 11: Pastel gradient background

```html
<style>
    .pastel {
        fill: url(#pastelGradient);
    }
</style>
<body>
    <svg width="400" height="300">
        <defs>
            <linearGradient id="pastelGradient" x1="0%" y1="0%" x2="100%" y2="100%">
                <stop offset="0%" stop-color="#dbeafe"/>
                <stop offset="33%" stop-color="#fce7f3"/>
                <stop offset="67%" stop-color="#fef3c7"/>
                <stop offset="100%" stop-color="#d1fae5"/>
            </linearGradient>
        </defs>
        <rect class="pastel" width="400" height="300"/>
    </svg>
</body>
```

### Example 12: Neon glow effect

```html
<style>
    .neon {
        fill: url(#neonGradient);
    }
</style>
<body>
    <svg width="300" height="150">
        <defs>
            <radialGradient id="neonGradient">
                <stop offset="0%" stop-color="#a78bfa"/>
                <stop offset="50%" stop-color="#8b5cf6"/>
                <stop offset="100%" stop-color="#6d28d9"/>
            </radialGradient>
        </defs>
        <rect class="neon" x="50" y="25" width="200" height="100" rx="50"/>
    </svg>
</body>
```

### Example 13: Progress bar gradient

```html
<style>
    .progress-bg {
        fill: #e5e7eb;
    }
    .progress-fill {
        fill: url(#progressGradient);
    }
</style>
<body>
    <svg width="350" height="80">
        <defs>
            <linearGradient id="progressGradient">
                <stop offset="0%" stop-color="#10b981"/>
                <stop offset="100%" stop-color="#059669"/>
            </linearGradient>
        </defs>
        <rect class="progress-bg" x="25" y="25" width="300" height="30" rx="15"/>
        <rect class="progress-fill" x="25" y="25" width="210" height="30" rx="15"/>
    </svg>
</body>
```

### Example 14: Sky gradient for landscape

```html
<style>
    .sky {
        fill: url(#skyGradient);
    }
    .ground {
        fill: #10b981;
    }
</style>
<body>
    <svg width="500" height="400">
        <defs>
            <linearGradient id="skyGradient" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#0ea5e9"/>
                <stop offset="100%" stop-color="#7dd3fc"/>
            </linearGradient>
        </defs>
        <rect class="sky" width="500" height="280"/>
        <rect class="ground" y="280" width="500" height="120"/>
    </svg>
</body>
```

### Example 15: Badge with gradient fill

```html
<style>
    .badge-gradient {
        fill: url(#badgeGradient);
    }
</style>
<body>
    <svg width="400" height="200">
        <defs>
            <linearGradient id="badgeGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                <stop offset="0%" stop-color="#f59e0b"/>
                <stop offset="50%" stop-color="#fbbf24"/>
                <stop offset="100%" stop-color="#f59e0b"/>
            </linearGradient>
        </defs>
        <circle class="badge-gradient" cx="200" cy="100" r="80"/>
    </svg>
</body>
```

---

## See Also

- [stop-opacity](/reference/cssproperties/css_prop_stop-opacity) - Gradient stop transparency
- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [opacity](/reference/cssproperties/css_prop_opacity) - Element transparency
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
