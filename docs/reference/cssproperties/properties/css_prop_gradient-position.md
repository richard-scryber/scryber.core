---
layout: default
title: x1, y1, x2, y2 (SVG Gradient Position Attributes)
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# x1, y1, x2, y2 : SVG Linear Gradient Position Attributes

The `x1`, `y1`, `x2`, and `y2` attributes define the start and end points of a linear gradient vector in SVG. These attributes control the direction and positioning of the gradient, allowing you to create horizontal, vertical, diagonal, and custom-angled gradients.

## Usage

```html
<linearGradient id="myGradient" x1="value" y1="value" x2="value" y2="value">
    <stop offset="0%" stop-color="#3b82f6"/>
    <stop offset="100%" stop-color="#1e40af"/>
</linearGradient>
```

These attributes accept coordinate values that define where the gradient starts (x1, y1) and ends (x2, y2).

---

## Supported Values

### Coordinate Values

- **x1** - X-coordinate of the gradient start point (default: `0%`)
- **y1** - Y-coordinate of the gradient start point (default: `0%`)
- **x2** - X-coordinate of the gradient end point (default: `100%`)
- **y2** - Y-coordinate of the gradient end point (default: `0%`)

### Value Formats
- Percentages: `0%`, `50%`, `100%`
- Decimal values: `0`, `0.5`, `1`
- Units: Can use length units if gradientUnits is set appropriately

### Common Patterns
- **Horizontal**: `x1="0%" y1="0%" x2="100%" y2="0%"` (left to right)
- **Vertical**: `x1="0%" y1="0%" x2="0%" y2="100%"` (top to bottom)
- **Diagonal**: `x1="0%" y1="0%" x2="100%" y2="100%"` (top-left to bottom-right)

---

## Supported Elements

These attributes apply to:
- `<linearGradient>` elements only

Note: Radial gradients use `cx`, `cy`, `r`, `fx`, `fy` attributes instead.

---

## Notes

- Default values create a horizontal gradient (left to right)
- Coordinates are relative to the bounding box of the shape (0% to 100%)
- (x1, y1) defines the gradient start point where the first stop color appears
- (x2, y2) defines the gradient end point where the last stop color appears
- The gradient vector extends infinitely in both directions unless spreadMethod is set
- Diagonal gradients are created when both x and y values change
- Reverse gradients by swapping start and end coordinates
- Essential for controlling gradient direction and angle
- Vector gradients maintain direction at any zoom level in PDF viewers
- Can create any angle by adjusting the coordinate relationships

---

## Data Binding

The gradient position attributes (`x1`, `y1`, `x2`, `y2`) can be dynamically controlled through data binding, enabling data-driven gradient directions and angles. This is essential for creating responsive gradients that adapt to data values or design requirements.

### Example 1: Data-driven gradient directions

```html
<body>
    <svg width="500" height="300" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each gradientBars}}">
            <defs>
                <linearGradient id="grad{{@index}}"
                                x1="{{x1}}%" y1="{{y1}}%"
                                x2="{{x2}}%" y2="{{y2}}%">
                    <stop offset="0%" stop-color="{{startColor}}"/>
                    <stop offset="100%" stop-color="{{endColor}}"/>
                </linearGradient>
            </defs>
            <rect x="{{x}}" y="{{y}}" width="{{width}}" height="{{height}}"
                  style="fill: url(#grad{{@index}})"/>
        </template>
    </svg>
</body>
```

### Example 2: Dynamic angle gradients based on data

```html
<body>
    <svg width="600" height="400" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{visualization}}">
            <defs>
                <linearGradient id="angleGrad"
                                x1="{{gradientStartX}}%" y1="{{gradientStartY}}%"
                                x2="{{gradientEndX}}%" y2="{{gradientEndY}}%">
                    <stop offset="0%" stop-color="{{color1}}"/>
                    <stop offset="50%" stop-color="{{color2}}"/>
                    <stop offset="100%" stop-color="{{color3}}"/>
                </linearGradient>
            </defs>
            <rect fill="url(#angleGrad)" width="600" height="400"/>
        </template>
    </svg>
</body>
```

### Example 3: Conditional gradient positions for different layouts

```html
<body>
    <svg width="500" height="350" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each sections}}">
            <defs>
                <linearGradient id="sectionGrad{{@index}}"
                                x1="{{#if horizontal}}0{{else}}50{{/if}}%"
                                y1="{{#if horizontal}}50{{else}}0{{/if}}%"
                                x2="{{#if horizontal}}100{{else}}50{{/if}}%"
                                y2="{{#if horizontal}}50{{else}}100{{/if}}%">
                    <stop offset="0%" stop-color="{{colorA}}"/>
                    <stop offset="100%" stop-color="{{colorB}}"/>
                </linearGradient>
            </defs>
            <rect x="{{posX}}" y="{{posY}}" width="{{boxWidth}}" height="{{boxHeight}}"
                  style="fill: url(#sectionGrad{{@index}})"/>
        </template>
    </svg>
</body>
```

---

## Examples

### Example 1: Horizontal gradient (left to right)

```html
<body>
    <svg width="400" height="150">
        <defs>
            <linearGradient id="horizontal" x1="0%" y1="0%" x2="100%" y2="0%">
                <stop offset="0%" stop-color="#3b82f6"/>
                <stop offset="100%" stop-color="#1e40af"/>
            </linearGradient>
        </defs>
        <rect fill="url(#horizontal)" width="400" height="150"/>
    </svg>
</body>
```

### Example 2: Vertical gradient (top to bottom)

```html
<body>
    <svg width="400" height="200">
        <defs>
            <linearGradient id="vertical" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#10b981"/>
                <stop offset="100%" stop-color="#065f46"/>
            </linearGradient>
        </defs>
        <rect fill="url(#vertical)" width="400" height="200"/>
    </svg>
</body>
```

### Example 3: Diagonal gradient (top-left to bottom-right)

```html
<body>
    <svg width="400" height="300">
        <defs>
            <linearGradient id="diagonal" x1="0%" y1="0%" x2="100%" y2="100%">
                <stop offset="0%" stop-color="#fbbf24"/>
                <stop offset="100%" stop-color="#dc2626"/>
            </linearGradient>
        </defs>
        <rect fill="url(#diagonal)" width="400" height="300"/>
    </svg>
</body>
```

### Example 4: Reverse horizontal gradient (right to left)

```html
<body>
    <svg width="400" height="150">
        <defs>
            <linearGradient id="reverseHorizontal" x1="100%" y1="0%" x2="0%" y2="0%">
                <stop offset="0%" stop-color="#8b5cf6"/>
                <stop offset="100%" stop-color="#6d28d9"/>
            </linearGradient>
        </defs>
        <rect fill="url(#reverseHorizontal)" width="400" height="150"/>
    </svg>
</body>
```

### Example 5: Reverse diagonal (bottom-right to top-left)

```html
<body>
    <svg width="400" height="300">
        <defs>
            <linearGradient id="reverseDiagonal" x1="100%" y1="100%" x2="0%" y2="0%">
                <stop offset="0%" stop-color="#f59e0b"/>
                <stop offset="100%" stop-color="#fef3c7"/>
            </linearGradient>
        </defs>
        <rect fill="url(#reverseDiagonal)" width="400" height="300"/>
    </svg>
</body>
```

### Example 6: 45-degree angle gradient

```html
<body>
    <svg width="400" height="400">
        <defs>
            <linearGradient id="angle45" x1="0%" y1="100%" x2="100%" y2="0%">
                <stop offset="0%" stop-color="#ec4899"/>
                <stop offset="100%" stop-color="#a855f7"/>
            </linearGradient>
        </defs>
        <rect fill="url(#angle45)" width="400" height="400"/>
    </svg>
</body>
```

### Example 7: Center-out horizontal gradient

```html
<body>
    <svg width="400" height="150">
        <defs>
            <linearGradient id="centerOut" x1="50%" y1="0%" x2="100%" y2="0%">
                <stop offset="0%" stop-color="white"/>
                <stop offset="100%" stop-color="#3b82f6"/>
            </linearGradient>
        </defs>
        <rect fill="url(#centerOut)" width="400" height="150"/>
    </svg>
</body>
```

### Example 8: Narrow gradient band

```html
<body>
    <svg width="400" height="200">
        <defs>
            <linearGradient id="narrowBand" x1="30%" y1="0%" x2="70%" y2="0%">
                <stop offset="0%" stop-color="#ef4444"/>
                <stop offset="100%" stop-color="#fbbf24"/>
            </linearGradient>
        </defs>
        <rect fill="url(#narrowBand)" width="400" height="200"/>
    </svg>
</body>
```

### Example 9: Subtle angle gradient

```html
<body>
    <svg width="400" height="250">
        <defs>
            <linearGradient id="subtleAngle" x1="0%" y1="0%" x2="100%" y2="20%">
                <stop offset="0%" stop-color="#dbeafe"/>
                <stop offset="100%" stop-color="#3b82f6"/>
            </linearGradient>
        </defs>
        <rect fill="url(#subtleAngle)" width="400" height="250"/>
    </svg>
</body>
```

### Example 10: Comparing gradient directions

```html
<body>
    <svg width="450" height="550">
        <defs>
            <linearGradient id="dir1" x1="0%" y1="0%" x2="100%" y2="0%">
                <stop offset="0%" stop-color="#3b82f6"/>
                <stop offset="100%" stop-color="#1e40af"/>
            </linearGradient>
            <linearGradient id="dir2" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#10b981"/>
                <stop offset="100%" stop-color="#065f46"/>
            </linearGradient>
            <linearGradient id="dir3" x1="0%" y1="0%" x2="100%" y2="100%">
                <stop offset="0%" stop-color="#f59e0b"/>
                <stop offset="100%" stop-color="#dc2626"/>
            </linearGradient>
            <linearGradient id="dir4" x1="100%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#a855f7"/>
                <stop offset="100%" stop-color="#6d28d9"/>
            </linearGradient>
        </defs>

        <rect fill="url(#dir1)" x="25" y="25" width="200" height="100"/>
        <text x="125" y="145" text-anchor="middle" font-size="12">Horizontal →</text>

        <rect fill="url(#dir2)" x="25" y="175" width="200" height="100"/>
        <text x="125" y="295" text-anchor="middle" font-size="12">Vertical ↓</text>

        <rect fill="url(#dir3)" x="25" y="325" width="200" height="100"/>
        <text x="125" y="445" text-anchor="middle" font-size="12">Diagonal ↘</text>

        <rect fill="url(#dir4)" x="25" y="475" width="200" height="100"/>
        <text x="125" y="595" text-anchor="middle" font-size="12">Diagonal ↙</text>
    </svg>
</body>
```

### Example 11: Button with gradient direction

```html
<body>
    <svg width="300" height="100">
        <defs>
            <linearGradient id="buttonGrad" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#60a5fa"/>
                <stop offset="100%" stop-color="#3b82f6"/>
            </linearGradient>
        </defs>
        <rect fill="url(#buttonGrad)" x="50" y="25" width="200" height="50" rx="25"/>
    </svg>
</body>
```

### Example 12: Sky gradient effect

```html
<body>
    <svg width="500" height="400">
        <defs>
            <linearGradient id="skyGrad" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#0ea5e9"/>
                <stop offset="100%" stop-color="#7dd3fc"/>
            </linearGradient>
        </defs>
        <rect fill="url(#skyGrad)" width="500" height="400"/>
    </svg>
</body>
```

### Example 13: Side lighting effect

```html
<body>
    <svg width="300" height="300">
        <defs>
            <linearGradient id="sideLighting" x1="0%" y1="0%" x2="100%" y2="0%">
                <stop offset="0%" stop-color="#1f2937"/>
                <stop offset="50%" stop-color="#9ca3af"/>
                <stop offset="100%" stop-color="#1f2937"/>
            </linearGradient>
        </defs>
        <circle fill="url(#sideLighting)" cx="150" cy="150" r="120"/>
    </svg>
</body>
```

### Example 14: Gradient at specific angle (approx 30 degrees)

```html
<body>
    <svg width="400" height="300">
        <defs>
            <linearGradient id="angle30" x1="0%" y1="13.4%" x2="100%" y2="86.6%">
                <stop offset="0%" stop-color="#10b981"/>
                <stop offset="100%" stop-color="#059669"/>
            </linearGradient>
        </defs>
        <rect fill="url(#angle30)" width="400" height="300"/>
    </svg>
</body>
```

### Example 15: Multiple gradients with different directions

```html
<body>
    <svg width="400" height="400">
        <defs>
            <linearGradient id="quad1" x1="0%" y1="0%" x2="100%" y2="0%">
                <stop offset="0%" stop-color="#ef4444"/>
                <stop offset="100%" stop-color="#dc2626"/>
            </linearGradient>
            <linearGradient id="quad2" x1="0%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#3b82f6"/>
                <stop offset="100%" stop-color="#1e40af"/>
            </linearGradient>
            <linearGradient id="quad3" x1="0%" y1="0%" x2="100%" y2="100%">
                <stop offset="0%" stop-color="#10b981"/>
                <stop offset="100%" stop-color="#059669"/>
            </linearGradient>
            <linearGradient id="quad4" x1="100%" y1="0%" x2="0%" y2="100%">
                <stop offset="0%" stop-color="#f59e0b"/>
                <stop offset="100%" stop-color="#d97706"/>
            </linearGradient>
        </defs>

        <rect fill="url(#quad1)" x="0" y="0" width="200" height="200"/>
        <rect fill="url(#quad2)" x="200" y="0" width="200" height="200"/>
        <rect fill="url(#quad3)" x="0" y="200" width="200" height="200"/>
        <rect fill="url(#quad4)" x="200" y="200" width="200" height="200"/>
    </svg>
</body>
```

---

## See Also

- [stop-color](/reference/cssproperties/css_prop_stop-color) - Gradient stop color
- [stop-opacity](/reference/cssproperties/css_prop_stop-opacity) - Gradient stop transparency
- [spreadMethod](/reference/cssproperties/css_prop_spreadMethod) - Gradient spread behavior
- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
