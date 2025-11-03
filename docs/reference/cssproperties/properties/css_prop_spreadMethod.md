---
layout: default
title: spreadMethod (SVG Attribute)
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# spreadMethod : SVG Gradient Spread Method Attribute

The `spreadMethod` attribute determines how a gradient behaves beyond its defined start and end points. This SVG attribute controls whether the gradient pads with the final color, reflects back and forth, or repeats the gradient pattern.

## Usage

```html
<linearGradient id="myGradient" spreadMethod="value">
    <stop offset="0%" stop-color="#3b82f6"/>
    <stop offset="100%" stop-color="#1e40af"/>
</linearGradient>
```

The spreadMethod attribute accepts keyword values that determine gradient behavior outside the defined range.

---

## Supported Values

### Keywords

- `pad` - Default value. The first and last stop colors extend to the edges of the shape. The gradient does not repeat or reflect.

- `reflect` - The gradient reflects back and forth, creating a mirror effect. After reaching the end, it reverses back to the start.

- `repeat` - The gradient repeats continuously from start to end, creating a tiled pattern.

---

## Supported Elements

The `spreadMethod` attribute applies to:
- `<linearGradient>` elements
- `<radialGradient>` elements

---

## Notes

- Default value is `pad` (extend edge colors)
- `pad` is the most commonly used spread method
- `reflect` creates seamless mirror transitions
- `repeat` creates tiled gradient patterns
- Particularly useful when gradients don't fill the entire shape
- Essential for creating striped or banded effects
- Works with both linear and radial gradients
- The gradient's defined range is 0% to 100% offset
- spreadMethod defines behavior outside this range
- Can create interesting visual patterns with small gradient definitions
- Vector gradients with any spread method maintain quality at all zoom levels

---

## Data Binding

The `spreadMethod` attribute can be dynamically controlled through data binding, enabling conditional gradient behaviors based on data states or design requirements. This is useful for creating adaptive patterns and responsive gradient effects.

### Example 1: Conditional spread method for pattern styles

```html
<body>
    <svg width="400" height="300" xmlns="http://www.w3.org/2000/svg">
        <defs>
            <template data-bind="{{pattern}}">
                <linearGradient id="dynamicGrad" x1="20%" y1="0%" x2="80%" y2="0%" spreadMethod="{{spreadType}}">
                    <stop offset="0%" stop-color="{{color1}}"/>
                    <stop offset="100%" stop-color="{{color2}}"/>
                </linearGradient>
            </template>
        </defs>
        <rect fill="url(#dynamicGrad)" width="400" height="300"/>
    </svg>
</body>
```

### Example 2: Data-driven striped patterns

```html
<body>
    <svg width="500" height="250" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each backgrounds}}">
            <defs>
                <linearGradient id="stripeGrad{{@index}}" x1="0%" y1="0%" x2="{{patternWidth}}%" y2="0%" spreadMethod="{{#if repeat}}repeat{{else}}pad{{/if}}">
                    <stop offset="0%" stop-color="{{primaryColor}}"/>
                    <stop offset="50%" stop-color="{{primaryColor}}"/>
                    <stop offset="50%" stop-color="{{secondaryColor}}"/>
                    <stop offset="100%" stop-color="{{secondaryColor}}"/>
                </linearGradient>
            </defs>
            <rect x="{{x}}" y="{{y}}" width="{{width}}" height="{{height}}"
                  style="fill: url(#stripeGrad{{@index}})"/>
        </template>
    </svg>
</body>
```

### Example 3: Dynamic radial patterns with reflection

```html
<body>
    <svg width="400" height="400" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{radialEffect}}">
            <defs>
                <radialGradient id="radialPattern" cx="50%" cy="50%" r="{{radius}}%" spreadMethod="{{method}}">
                    <stop offset="0%" stop-color="{{centerColor}}"/>
                    <stop offset="100%" stop-color="{{edgeColor}}"/>
                </radialGradient>
            </defs>
            <rect fill="url(#radialPattern)" width="400" height="400"/>
        </template>
    </svg>
</body>
```

---

## Examples

### Example 1: Comparing all spread methods

```html
<body>
    <svg width="400" height="450">
        <defs>
            <!-- Pad (default) -->
            <linearGradient id="gradPad" x1="25%" y1="0%" x2="75%" y2="0%" spreadMethod="pad">
                <stop offset="0%" stop-color="#3b82f6"/>
                <stop offset="100%" stop-color="#1e40af"/>
            </linearGradient>

            <!-- Reflect -->
            <linearGradient id="gradReflect" x1="25%" y1="0%" x2="75%" y2="0%" spreadMethod="reflect">
                <stop offset="0%" stop-color="#3b82f6"/>
                <stop offset="100%" stop-color="#1e40af"/>
            </linearGradient>

            <!-- Repeat -->
            <linearGradient id="gradRepeat" x1="25%" y1="0%" x2="75%" y2="0%" spreadMethod="repeat">
                <stop offset="0%" stop-color="#3b82f6"/>
                <stop offset="100%" stop-color="#1e40af"/>
            </linearGradient>
        </defs>

        <rect fill="url(#gradPad)" x="25" y="25" width="350" height="100"/>
        <text x="200" y="145" text-anchor="middle" fill="#1f2937">pad (default)</text>

        <rect fill="url(#gradReflect)" x="25" y="175" width="350" height="100"/>
        <text x="200" y="295" text-anchor="middle" fill="#1f2937">reflect</text>

        <rect fill="url(#gradRepeat)" x="25" y="325" width="350" height="100"/>
        <text x="200" y="445" text-anchor="middle" fill="#1f2937">repeat</text>
    </svg>
</body>
```

### Example 2: Striped pattern with repeat

```html
<body>
    <svg width="400" height="200">
        <defs>
            <linearGradient id="stripes" x1="0%" y1="0%" x2="10%" y2="0%" spreadMethod="repeat">
                <stop offset="0%" stop-color="#3b82f6"/>
                <stop offset="50%" stop-color="#3b82f6"/>
                <stop offset="50%" stop-color="white"/>
                <stop offset="100%" stop-color="white"/>
            </linearGradient>
        </defs>
        <rect fill="url(#stripes)" width="400" height="200"/>
    </svg>
</body>
```

### Example 3: Barber pole effect with repeat

```html
<body>
    <svg width="150" height="400">
        <defs>
            <linearGradient id="barberPole" x1="0%" y1="0%" x2="0%" y2="15%" spreadMethod="repeat">
                <stop offset="0%" stop-color="#ef4444"/>
                <stop offset="33%" stop-color="#ef4444"/>
                <stop offset="33%" stop-color="white"/>
                <stop offset="67%" stop-color="white"/>
                <stop offset="67%" stop-color="#3b82f6"/>
                <stop offset="100%" stop-color="#3b82f6"/>
            </linearGradient>
        </defs>
        <rect fill="url(#barberPole)" x="25" y="25" width="100" height="350"/>
    </svg>
</body>
```

### Example 4: Radial gradient with reflect

```html
<body>
    <svg width="400" height="400">
        <defs>
            <radialGradient id="radialReflect" cx="50%" cy="50%" r="20%" spreadMethod="reflect">
                <stop offset="0%" stop-color="#fbbf24"/>
                <stop offset="100%" stop-color="#dc2626"/>
            </radialGradient>
        </defs>
        <rect fill="url(#radialReflect)" width="400" height="400"/>
    </svg>
</body>
```

### Example 5: Rainbow repeating pattern

```html
<body>
    <svg width="400" height="100">
        <defs>
            <linearGradient id="rainbow" x1="0%" y1="0%" x2="20%" y2="0%" spreadMethod="repeat">
                <stop offset="0%" stop-color="#ef4444"/>
                <stop offset="17%" stop-color="#f97316"/>
                <stop offset="33%" stop-color="#fbbf24"/>
                <stop offset="50%" stop-color="#10b981"/>
                <stop offset="67%" stop-color="#3b82f6"/>
                <stop offset="83%" stop-color="#6366f1"/>
                <stop offset="100%" stop-color="#a855f7"/>
            </linearGradient>
        </defs>
        <rect fill="url(#rainbow)" width="400" height="100"/>
    </svg>
</body>
```

### Example 6: Diagonal stripes with repeat

```html
<body>
    <svg width="400" height="300">
        <defs>
            <linearGradient id="diagStripes" x1="0%" y1="0%" x2="5%" y2="5%" spreadMethod="repeat">
                <stop offset="0%" stop-color="#1e40af"/>
                <stop offset="50%" stop-color="#1e40af"/>
                <stop offset="50%" stop-color="#60a5fa"/>
                <stop offset="100%" stop-color="#60a5fa"/>
            </linearGradient>
        </defs>
        <rect fill="url(#diagStripes)" width="400" height="300"/>
    </svg>
</body>
```

### Example 7: Concentric circles with reflect

```html
<body>
    <svg width="400" height="400">
        <defs>
            <radialGradient id="circles" cx="50%" cy="50%" r="15%" spreadMethod="reflect">
                <stop offset="0%" stop-color="#3b82f6"/>
                <stop offset="50%" stop-color="white"/>
                <stop offset="100%" stop-color="#3b82f6"/>
            </radialGradient>
        </defs>
        <circle fill="url(#circles)" cx="200" cy="200" r="180"/>
    </svg>
</body>
```

### Example 8: Checkerboard pattern attempt

```html
<body>
    <svg width="300" height="300">
        <defs>
            <linearGradient id="checker" x1="0%" y1="0%" x2="10%" y2="0%" spreadMethod="repeat">
                <stop offset="0%" stop-color="#1f2937"/>
                <stop offset="50%" stop-color="#1f2937"/>
                <stop offset="50%" stop-color="#f3f4f6"/>
                <stop offset="100%" stop-color="#f3f4f6"/>
            </linearGradient>
        </defs>
        <rect fill="url(#checker)" width="300" height="300"/>
    </svg>
</body>
```

### Example 9: Wave pattern with repeat

```html
<body>
    <svg width="400" height="150">
        <defs>
            <linearGradient id="waves" x1="0%" y1="0%" x2="15%" y2="0%" spreadMethod="repeat">
                <stop offset="0%" stop-color="#06b6d4"/>
                <stop offset="25%" stop-color="#22d3ee"/>
                <stop offset="50%" stop-color="#67e8f9"/>
                <stop offset="75%" stop-color="#22d3ee"/>
                <stop offset="100%" stop-color="#06b6d4"/>
            </linearGradient>
        </defs>
        <rect fill="url(#waves)" width="400" height="150"/>
    </svg>
</body>
```

### Example 10: Target/bullseye with reflect

```html
<body>
    <svg width="300" height="300">
        <defs>
            <radialGradient id="target" cx="50%" cy="50%" r="10%" spreadMethod="reflect">
                <stop offset="0%" stop-color="#ef4444"/>
                <stop offset="50%" stop-color="white"/>
                <stop offset="100%" stop-color="#ef4444"/>
            </radialGradient>
        </defs>
        <circle fill="url(#target)" cx="150" cy="150" r="140"/>
    </svg>
</body>
```

### Example 11: Vertical blinds effect

```html
<body>
    <svg width="400" height="300">
        <defs>
            <linearGradient id="blinds" x1="0%" y1="0%" x2="8%" y2="0%" spreadMethod="repeat">
                <stop offset="0%" stop-color="#9ca3af"/>
                <stop offset="30%" stop-color="#d1d5db"/>
                <stop offset="50%" stop-color="#f3f4f6"/>
                <stop offset="70%" stop-color="#d1d5db"/>
                <stop offset="100%" stop-color="#6b7280"/>
            </linearGradient>
        </defs>
        <rect fill="url(#blinds)" width="400" height="300"/>
    </svg>
</body>
```

### Example 12: Candy cane with repeat

```html
<body>
    <svg width="100" height="400">
        <defs>
            <linearGradient id="candyCane" x1="0%" y1="0%" x2="0%" y2="10%" spreadMethod="repeat">
                <stop offset="0%" stop-color="#ef4444"/>
                <stop offset="50%" stop-color="#ef4444"/>
                <stop offset="50%" stop-color="white"/>
                <stop offset="100%" stop-color="white"/>
            </linearGradient>
        </defs>
        <rect fill="url(#candyCane)" x="20" y="20" width="60" height="360" rx="30"/>
    </svg>
</body>
```

### Example 13: Ripple effect with reflect

```html
<body>
    <svg width="400" height="400">
        <defs>
            <radialGradient id="ripple" cx="50%" cy="50%" r="10%" spreadMethod="reflect">
                <stop offset="0%" stop-color="#3b82f6" stop-opacity="0.8"/>
                <stop offset="100%" stop-color="#3b82f6" stop-opacity="0"/>
            </radialGradient>
        </defs>
        <rect fill="#e0f2fe" width="400" height="400"/>
        <circle fill="url(#ripple)" cx="200" cy="200" r="180"/>
    </svg>
</body>
```

### Example 14: Progress bar with repeating segments

```html
<body>
    <svg width="400" height="80">
        <defs>
            <linearGradient id="progressRepeat" x1="0%" y1="0%" x2="5%" y2="0%" spreadMethod="repeat">
                <stop offset="0%" stop-color="#10b981"/>
                <stop offset="70%" stop-color="#10b981"/>
                <stop offset="70%" stop-color="#065f46"/>
                <stop offset="100%" stop-color="#065f46"/>
            </linearGradient>
        </defs>
        <rect fill="#e5e7eb" x="25" y="25" width="350" height="30" rx="15"/>
        <rect fill="url(#progressRepeat)" x="25" y="25" width="250" height="30" rx="15"/>
    </svg>
</body>
```

### Example 15: Optical illusion pattern

```html
<body>
    <svg width="400" height="400">
        <defs>
            <radialGradient id="optical" cx="50%" cy="50%" r="8%" spreadMethod="repeat">
                <stop offset="0%" stop-color="black"/>
                <stop offset="50%" stop-color="black"/>
                <stop offset="50%" stop-color="white"/>
                <stop offset="100%" stop-color="white"/>
            </radialGradient>
        </defs>
        <rect fill="url(#optical)" width="400" height="400"/>
    </svg>
</body>
```

---

## See Also

- [stop-color](/reference/cssproperties/css_prop_stop-color) - Gradient stop color
- [stop-opacity](/reference/cssproperties/css_prop_stop-opacity) - Gradient stop transparency
- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
