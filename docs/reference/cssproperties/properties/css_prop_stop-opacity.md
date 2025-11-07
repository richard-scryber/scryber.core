---
layout: default
title: stop-opacity
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# stop-opacity : SVG Gradient Stop Transparency Property

The `stop-opacity` property specifies the opacity (transparency) value at a gradient stop within SVG gradient definitions. This property enables creating gradients that fade to transparent, allowing backgrounds and underlying elements to show through.

## Usage

```css
selector {
    stop-opacity: value;
}
```

The stop-opacity property accepts numeric values between 0 and 1, where 0 is fully transparent and 1 is fully opaque.

---

## Supported Values

### Numeric Values
- `stop-opacity: 0` - Fully transparent (invisible)
- `stop-opacity: 0.25` - 25% opaque (75% transparent)
- `stop-opacity: 0.5` - 50% opaque (semi-transparent)
- `stop-opacity: 0.75` - 75% opaque (25% transparent)
- `stop-opacity: 1` - Fully opaque (default, no transparency)

### Percentage Values (if supported)
- `stop-opacity: 0%` - Fully transparent
- `stop-opacity: 50%` - Semi-transparent
- `stop-opacity: 100%` - Fully opaque

---

## Supported Elements

The `stop-opacity` property applies to:
- `<stop>` elements within `<linearGradient>` definitions
- `<stop>` elements within `<radialGradient>` definitions

Note: This property only works on `<stop>` elements, not on shape elements directly.

---

## Notes

- Must be used within `<stop>` elements inside gradient definitions
- Default value is `1` (fully opaque)
- Values range from 0 (transparent) to 1 (opaque)
- Works in combination with `stop-color` to create transparent gradients
- Essential for fade effects and glass/transparency designs
- Can create gradients that fade from color to transparent
- Allows underlying content to show through gradient areas
- Useful for overlays, highlights, and shadow effects
- Each stop can have a different opacity value
- Vector transparent gradients maintain quality at any zoom level in PDF viewers
- Commonly used for vignette effects and soft edges

---

## Data Binding

The `stop-opacity` property can be dynamically controlled through data binding, enabling data-driven transparency effects in gradients. This is particularly useful for creating dynamic overlays, fade effects, and visual emphasis based on data values.

### Example 1: Data-driven fade overlays

```html
<style>
    .overlay { fill: url(#fadeOverlay); }
</style>
<body>
    <svg width="400" height="300" xmlns="http://www.w3.org/2000/svg">
        <rect fill="#3b82f6" width="400" height="300"/>
        <defs>
            <template data-bind="{{overlay}}">
                <linearGradient id="fadeOverlay" x1="0%" y1="0%" x2="100%" y2="0%">
                    <stop offset="0%" stop-color="black" stop-opacity="{{startOpacity}}"/>
                    <stop offset="100%" stop-color="black" stop-opacity="{{endOpacity}}"/>
                </linearGradient>
            </template>
        </defs>
        <rect class="overlay" width="400" height="300"/>
    </svg>
</body>
```

### Example 2: Dynamic vignette effect with data-bound opacity

```html
<style>
    .vignette { fill: url(#vignetteGrad); }
</style>
<body>
    <svg width="500" height="400" xmlns="http://www.w3.org/2000/svg">
        <rect fill="white" width="500" height="400"/>
        <defs>
            <template data-bind="{{vignette}}">
                <radialGradient id="vignetteGrad" cx="50%" cy="50%">
                    <stop offset="0%" stop-color="black" stop-opacity="0"/>
                    <stop offset="{{innerRadius}}%" stop-color="black" stop-opacity="0"/>
                    <stop offset="100%" stop-color="black" stop-opacity="{{intensity}}"/>
                </radialGradient>
            </template>
        </defs>
        <rect class="vignette" width="500" height="400"/>
    </svg>
</body>
```

### Example 3: Conditional transparency for emphasis

```html
<style>
    .highlight-area { fill: url(#highlightGrad); }
</style>
<body>
    <svg width="600" height="250" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each regions}}">
            <defs>
                <linearGradient id="highlightGrad{{@index}}" x1="0%" y1="0%" x2="0%" y2="100%">
                    <stop offset="0%" stop-color="{{color}}" stop-opacity="{{#if emphasized}}0.8{{else}}0.3{{/if}}"/>
                    <stop offset="100%" stop-color="{{color}}" stop-opacity="{{#if emphasized}}0.4{{else}}0.1{{/if}}"/>
                </linearGradient>
            </defs>
            <rect class="highlight-area"
                  x="{{x}}" y="{{y}}" width="{{width}}" height="{{height}}"
                  style="fill: url(#highlightGrad{{@index}})"/>
        </template>
    </svg>
</body>
```

---

## Examples

### Example 1: Fade to transparent gradient

```html
<style>
    .fade-gradient {
        fill: url(#fadeGradient);
    }
</style>
<body>
    <svg width="300" height="150">
        <rect fill="#fbbf24" width="300" height="150"/>
        <defs>
            <linearGradient id="fadeGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                <stop offset="0%" stop-color="#3b82f6" stop-opacity="1"/>
                <stop offset="100%" stop-color="#3b82f6" stop-opacity="0"/>
            </linearGradient>
        </defs>
        <rect class="fade-gradient" width="300" height="150"/>
    </svg>
</body>
```

### Example 2: Vignette effect

```html
<style>
    .vignette {
        fill: url(#vignetteGradient);
    }
</style>
<body>
    <svg width="400" height="300">
        <rect fill="#ffffff" width="400" height="300"/>
        <defs>
            <radialGradient id="vignetteGradient" cx="50%" cy="50%">
                <stop offset="0%" stop-color="black" stop-opacity="0"/>
                <stop offset="70%" stop-color="black" stop-opacity="0"/>
                <stop offset="100%" stop-color="black" stop-opacity="0.7"/>
            </radialGradient>
        </defs>
        <rect class="vignette" width="400" height="300"/>
    </svg>
</body>
```

### Example 3: Glass/frosted effect overlay

```html
<style>
    .glass-overlay {
        fill: url(#glassGradient);
    }
</style>
<body>
    <svg width="300" height="200">
        <rect fill="#3b82f6" width="300" height="200"/>
        <defs>
            <linearGradient id="glassGradient" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="white" stop-opacity="0.8"/>
                <stop offset="50%" stop-color="white" stop-opacity="0.3"/>
                <stop offset="100%" stop-color="white" stop-opacity="0.1"/>
            </linearGradient>
        </defs>
        <rect class="glass-overlay" width="300" height="200"/>
    </svg>
</body>
```

### Example 4: Spotlight effect

```html
<style>
    .spotlight {
        fill: url(#spotlightGradient);
    }
</style>
<body>
    <svg width="400" height="300">
        <rect fill="#1f2937" width="400" height="300"/>
        <defs>
            <radialGradient id="spotlightGradient" cx="50%" cy="40%">
                <stop offset="0%" stop-color="white" stop-opacity="0.9"/>
                <stop offset="30%" stop-color="white" stop-opacity="0.5"/>
                <stop offset="60%" stop-color="white" stop-opacity="0.2"/>
                <stop offset="100%" stop-color="white" stop-opacity="0"/>
            </radialGradient>
        </defs>
        <rect class="spotlight" width="400" height="300"/>
    </svg>
</body>
```

### Example 5: Soft shadow effect

```html
<style>
    .shadow {
        fill: url(#shadowGradient);
    }
</style>
<body>
    <svg width="300" height="300">
        <defs>
            <radialGradient id="shadowGradient" cx="50%" cy="50%">
                <stop offset="0%" stop-color="black" stop-opacity="0.6"/>
                <stop offset="100%" stop-color="black" stop-opacity="0"/>
            </radialGradient>
        </defs>
        <rect fill="white" width="300" height="300"/>
        <circle fill="#3b82f6" cx="150" cy="130" r="60"/>
        <ellipse class="shadow" cx="150" cy="220" rx="70" ry="20"/>
    </svg>
</body>
```

### Example 6: Top highlight with transparency

```html
<style>
    .button-base {
        fill: #3b82f6;
    }
    .button-highlight {
        fill: url(#highlightGradient);
    }
</style>
<body>
    <svg width="250" height="100">
        <defs>
            <linearGradient id="highlightGradient" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="white" stop-opacity="0.5"/>
                <stop offset="50%" stop-color="white" stop-opacity="0.1"/>
                <stop offset="100%" stop-color="white" stop-opacity="0"/>
            </linearGradient>
        </defs>
        <rect class="button-base" x="25" y="25" width="200" height="50" rx="25"/>
        <rect class="button-highlight" x="25" y="25" width="200" height="25" rx="25"/>
    </svg>
</body>
```

### Example 7: Fading text effect

```html
<style>
    .fading-rect {
        fill: url(#fadingGradient);
    }
</style>
<body>
    <svg width="400" height="150">
        <defs>
            <linearGradient id="fadingGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                <stop offset="0%" stop-color="#1f2937" stop-opacity="1"/>
                <stop offset="70%" stop-color="#1f2937" stop-opacity="1"/>
                <stop offset="100%" stop-color="#1f2937" stop-opacity="0"/>
            </linearGradient>
        </defs>
        <rect class="fading-rect" width="400" height="150"/>
    </svg>
</body>
```

### Example 8: Aurora/Northern Lights effect

```html
<style>
    .aurora {
        fill: url(#auroraGradient);
    }
</style>
<body>
    <svg width="500" height="300">
        <rect fill="#0c4a6e" width="500" height="300"/>
        <defs>
            <linearGradient id="auroraGradient" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#10b981" stop-opacity="0"/>
                <stop offset="30%" stop-color="#10b981" stop-opacity="0.7"/>
                <stop offset="50%" stop-color="#3b82f6" stop-opacity="0.6"/>
                <stop offset="70%" stop-color="#a855f7" stop-opacity="0.5"/>
                <stop offset="100%" stop-color="#a855f7" stop-opacity="0"/>
            </linearGradient>
        </defs>
        <rect class="aurora" width="500" height="300"/>
    </svg>
</body>
```

### Example 9: Depth/distance fade effect

```html
<style>
    .distance-fade {
        fill: url(#distanceGradient);
    }
</style>
<body>
    <svg width="400" height="300">
        <rect fill="#e5e7eb" width="400" height="300"/>
        <defs>
            <linearGradient id="distanceGradient" x1="0%" y1="100%" x2="0%" y2="0%">
                <stop offset="0%" stop-color="#1f2937" stop-opacity="0"/>
                <stop offset="50%" stop-color="#6b7280" stop-opacity="0.3"/>
                <stop offset="100%" stop-color="#9ca3af" stop-opacity="0.8"/>
            </linearGradient>
        </defs>
        <rect class="distance-fade" width="400" height="300"/>
    </svg>
</body>
```

### Example 10: Glow effect around shape

```html
<style>
    .glow {
        fill: url(#glowGradient);
    }
</style>
<body>
    <svg width="300" height="300">
        <rect fill="#1f2937" width="300" height="300"/>
        <defs>
            <radialGradient id="glowGradient" cx="50%" cy="50%">
                <stop offset="50%" stop-color="#fbbf24" stop-opacity="1"/>
                <stop offset="70%" stop-color="#fbbf24" stop-opacity="0.6"/>
                <stop offset="90%" stop-color="#f59e0b" stop-opacity="0.3"/>
                <stop offset="100%" stop-color="#f59e0b" stop-opacity="0"/>
            </radialGradient>
        </defs>
        <circle class="glow" cx="150" cy="150" r="100"/>
    </svg>
</body>
```

### Example 11: Reflection effect

```html
<style>
    .shape {
        fill: #3b82f6;
    }
    .reflection {
        fill: url(#reflectionGradient);
    }
</style>
<body>
    <svg width="300" height="400">
        <defs>
            <linearGradient id="reflectionGradient" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#3b82f6" stop-opacity="0.5"/>
                <stop offset="100%" stop-color="#3b82f6" stop-opacity="0"/>
            </linearGradient>
        </defs>
        <rect fill="#f3f4f6" width="300" height="400"/>
        <circle class="shape" cx="150" cy="120" r="60"/>
        <ellipse class="reflection" cx="150" cy="260" rx="60" ry="30"/>
    </svg>
</body>
```

### Example 12: Overlay modal background

```html
<style>
    .modal-overlay {
        fill: url(#modalGradient);
    }
</style>
<body>
    <svg width="400" height="300">
        <rect fill="#3b82f6" width="400" height="300"/>
        <defs>
            <radialGradient id="modalGradient" cx="50%" cy="50%">
                <stop offset="0%" stop-color="black" stop-opacity="0.3"/>
                <stop offset="100%" stop-color="black" stop-opacity="0.8"/>
            </radialGradient>
        </defs>
        <rect class="modal-overlay" width="400" height="300"/>
    </svg>
</body>
```

### Example 13: Edge fade for seamless tiling

```html
<style>
    .edge-fade {
        fill: url(#edgeFadeGradient);
    }
</style>
<body>
    <svg width="300" height="200">
        <rect fill="#10b981" width="300" height="200"/>
        <defs>
            <linearGradient id="edgeFadeGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                <stop offset="0%" stop-color="black" stop-opacity="0.4"/>
                <stop offset="10%" stop-color="black" stop-opacity="0"/>
                <stop offset="90%" stop-color="black" stop-opacity="0"/>
                <stop offset="100%" stop-color="black" stop-opacity="0.4"/>
            </linearGradient>
        </defs>
        <rect class="edge-fade" width="300" height="200"/>
    </svg>
</body>
```

### Example 14: Shimmer/shine effect

```html
<style>
    .shimmer {
        fill: url(#shimmerGradient);
    }
</style>
<body>
    <svg width="350" height="100">
        <rect fill="#6b7280" width="350" height="100"/>
        <defs>
            <linearGradient id="shimmerGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                <stop offset="0%" stop-color="white" stop-opacity="0"/>
                <stop offset="40%" stop-color="white" stop-opacity="0"/>
                <stop offset="50%" stop-color="white" stop-opacity="0.8"/>
                <stop offset="60%" stop-color="white" stop-opacity="0"/>
                <stop offset="100%" stop-color="white" stop-opacity="0"/>
            </linearGradient>
        </defs>
        <rect class="shimmer" width="350" height="100"/>
    </svg>
</body>
```

### Example 15: Soft focus/blur simulation

```html
<style>
    .focus-center {
        fill: url(#focusGradient);
    }
</style>
<body>
    <svg width="400" height="400">
        <rect fill="#dbeafe" width="400" height="400"/>
        <circle fill="#3b82f6" cx="200" cy="200" r="80"/>
        <defs>
            <radialGradient id="focusGradient" cx="50%" cy="50%">
                <stop offset="0%" stop-color="white" stop-opacity="0"/>
                <stop offset="40%" stop-color="white" stop-opacity="0"/>
                <stop offset="70%" stop-color="white" stop-opacity="0.5"/>
                <stop offset="100%" stop-color="white" stop-opacity="0.9"/>
            </radialGradient>
        </defs>
        <rect class="focus-center" width="400" height="400"/>
    </svg>
</body>
```

---

## See Also

- [stop-color](/reference/cssproperties/css_prop_stop-color) - Gradient stop color
- [fill-opacity](/reference/cssproperties/css_prop_fill-opacity) - Fill transparency
- [stroke-opacity](/reference/cssproperties/css_prop_stroke-opacity) - SVG stroke transparency
- [opacity](/reference/cssproperties/css_prop_opacity) - Element transparency
- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
