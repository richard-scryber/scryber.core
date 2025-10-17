---
layout: default
title: spreadMethod
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @spreadMethod : The Gradient Spread Behavior Attribute

The `spreadMethod` attribute defines how a gradient behaves beyond its defined start and end points (0% to 100%). It controls whether the gradient extends its end colors, repeats in a tiling pattern, or reflects in an alternating mirror pattern.

## Usage

The `spreadMethod` attribute is used to:
- Control gradient behavior outside the 0%-100% range
- Create repeating stripe patterns with gradients
- Generate mirrored gradient effects
- Extend end colors to fill remaining space (default)
- Enable data-driven pattern selection
- Create visual effects for backgrounds and decorations
- Build complex repeating patterns for data visualizations

```html
<svg xmlns="http://www.w3.org/2000/svg" width="600" height="300">
    <defs>
        <!-- Default: pad (extends end colors) -->
        <linearGradient id="pad" x1="20%" x2="80%" spreadMethod="pad">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>

        <!-- Repeat: tiles the gradient -->
        <linearGradient id="repeat" x1="0%" x2="20%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>

        <!-- Reflect: mirrors the gradient -->
        <linearGradient id="reflect" x1="0%" x2="25%" spreadMethod="reflect">
            <stop offset="0%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#8e44ad" />
        </linearGradient>
    </defs>

    <rect y="0" width="600" height="90" fill="url(#pad)" />
    <rect y="105" width="600" height="90" fill="url(#repeat)" />
    <rect y="210" width="600" height="90" fill="url(#reflect)" />
</svg>
```

---

## Supported Values

| Value | Description | Visual Effect | Use Cases |
|-------|-------------|---------------|-----------|
| `pad` | Extends end colors (default) | Solid color beyond gradient range | Most gradients, backgrounds, fills |
| `repeat` | Tiles gradient pattern | Repeating stripes or bands | Patterns, striped backgrounds |
| `reflect` | Mirrors gradient alternately | Symmetric wave pattern | Decorative effects, ripples |

### Default Behavior

```html
<!-- These are equivalent (pad is default) -->
<linearGradient id="g1" x1="0%" x2="100%">
<linearGradient id="g2" x1="0%" x2="100%" spreadMethod="pad">
```

---

## Supported Elements

The `spreadMethod` attribute is supported on:

- **[&lt;linearGradient&gt;](/reference/svgtags/linearGradient.html)** - Linear gradient spread behavior
- **[&lt;radialGradient&gt;](/reference/svgtags/radialGradient.html)** - Radial gradient spread behavior

---

## Data Binding

### Dynamic Spread Method Selection

Choose spread method based on data:

```html
<!-- Model: { pattern: "striped" } -->
<svg width="400" height="200">
    <defs>
        <linearGradient id="dynamicSpread"
                        x1="0%" x2="{{model.pattern === 'striped' ? '10%' : '100%'}}"
                        spreadMethod="{{model.pattern === 'striped' ? 'repeat' : 'pad'}}">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2980b9" />
        </linearGradient>
    </defs>
    <rect width="400" height="200" fill="url(#dynamicSpread)" />
</svg>
```

### Data-Driven Pattern Density

Control stripe density with data:

```html
<!-- Model: { stripeWidth: 15 } -->
<svg width="600" height="200">
    <defs>
        <linearGradient id="stripes"
                        x1="0%"
                        x2="{{model.stripeWidth}}%"
                        spreadMethod="repeat">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#2980b9" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="600" height="200" fill="url(#stripes)" />
</svg>
```

### Conditional Pattern Effects

Apply patterns based on data state:

```html
<!-- Model: { usePattern: true, isReflected: false } -->
<svg width="500" height="250">
    <defs>
        <linearGradient id="conditionalPattern"
                        x1="0%"
                        x2="{{model.usePattern ? '20%' : '100%'}}"
                        spreadMethod="{{!model.usePattern ? 'pad' :
                                       model.isReflected ? 'reflect' : 'repeat'}}">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>
    <rect width="500" height="250" fill="url(#conditionalPattern)" />
</svg>
```

### Data Visualization Patterns

Create patterns for chart backgrounds:

```html
<!-- Model: { chartType: "bar", showGrid: true } -->
<svg width="600" height="400">
    <defs>
        <linearGradient id="chartPattern"
                        x1="0%" y1="0%"
                        x2="{{model.showGrid ? '5%' : '0%'}}"
                        y2="0%"
                        spreadMethod="{{model.showGrid ? 'repeat' : 'pad'}}">
            <stop offset="0%" stop-color="#ecf0f1" />
            <stop offset="50%" stop-color="#bdc3c7" />
            <stop offset="100%" stop-color="#ecf0f1" />
        </linearGradient>
    </defs>

    <rect width="600" height="400" fill="url(#chartPattern)" />
</svg>
```

### Ripple Effect Intensity

Control ripple pattern based on intensity data:

```html
<!-- Model: { rippleIntensity: 3, rippleWidth: 25 } -->
<svg width="400" height="400">
    <defs>
        <radialGradient id="ripples"
                        cx="50%" cy="50%"
                        r="{{model.rippleWidth}}%"
                        spreadMethod="reflect">
            <stop offset="0%" stop-color="#3498db" stop-opacity="{{model.rippleIntensity / 10}}" />
            <stop offset="100%" stop-color="#2980b9" stop-opacity="0.1" />
        </radialGradient>
    </defs>

    <rect width="400" height="400" fill="url(#ripples)" />
</svg>
```

---

## Notes

### Spread Method Behaviors

#### 1. pad (Default)

Extends the first and last stop colors:
- Before 0%: extends first stop color
- After 100%: extends last stop color
- Creates solid color regions beyond gradient
- Most common for standard gradients

```html
<!-- Gradient from 20% to 80% -->
<linearGradient x1="20%" x2="80%" spreadMethod="pad">
  <stop offset="0%" stop-color="blue" />
  <stop offset="100%" stop-color="red" />
</linearGradient>
<!-- 0-20%: solid blue, 20-80%: gradient, 80-100%: solid red -->
```

#### 2. repeat

Tiles the gradient pattern:
- Repeats the entire 0%-100% gradient pattern
- Creates stripe or band effects
- Pattern repeats seamlessly
- Useful for backgrounds and decorative elements

```html
<!-- Creates repeating stripes -->
<linearGradient x1="0%" x2="10%" spreadMethod="repeat">
  <stop offset="0%" stop-color="blue" />
  <stop offset="100%" stop-color="red" />
</linearGradient>
```

#### 3. reflect

Mirrors the gradient alternately:
- First repetition: 0% to 100% (normal)
- Second repetition: 100% to 0% (reversed)
- Third repetition: 0% to 100% (normal)
- Creates symmetric wave patterns
- Useful for ripple and echo effects

```html
<!-- Creates mirrored wave pattern -->
<linearGradient x1="0%" x2="15%" spreadMethod="reflect">
  <stop offset="0%" stop-color="blue" />
  <stop offset="100%" stop-color="red" />
</linearGradient>
```

### Gradient Range and Spread Method

The spread method activates when gradient vector is shorter than shape:

```html
<!-- Short gradient (20% of width) triggers spread method -->
<linearGradient x1="40%" x2="60%" spreadMethod="repeat">

<!-- Full gradient (100% of width) doesn't use spread method -->
<linearGradient x1="0%" x2="100%" spreadMethod="repeat">
```

### Radial Gradient Spread

For radial gradients, spread methods work concentrically:
- **pad**: Extends outer color beyond radius
- **repeat**: Creates concentric rings
- **reflect**: Creates alternating rings

```html
<radialGradient cx="50%" cy="50%" r="20%" spreadMethod="repeat">
```

### Performance Considerations

- pad: Most efficient (default)
- repeat: Moderate overhead for pattern tiling
- reflect: Similar to repeat
- Use sparingly on complex gradients
- Consider caching for frequently used patterns

### Common Use Cases

**pad:**
- Standard gradients and fills
- Background colors with subtle gradients
- Button and UI element fills
- Most common gradient applications

**repeat:**
- Striped backgrounds
- Decorative patterns
- Barber pole effects
- Progress indicators with stripes
- Grid patterns

**reflect:**
- Water ripples
- Echo effects
- Symmetric patterns
- Decorative borders
- Wave animations

---

## Examples

### Pad Method (Default)

```html
<svg width="600" height="100">
    <defs>
        <linearGradient id="padGrad" x1="20%" x2="80%" spreadMethod="pad">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="600" height="100" fill="url(#padGrad)" />
</svg>
```

### Repeat Method - Vertical Stripes

```html
<svg width="600" height="100">
    <defs>
        <linearGradient id="stripes" x1="0%" x2="5%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#2980b9" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="600" height="100" fill="url(#stripes)" />
</svg>
```

### Reflect Method - Wave Pattern

```html
<svg width="600" height="100">
    <defs>
        <linearGradient id="waves" x1="0%" x2="10%" spreadMethod="reflect">
            <stop offset="0%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#8e44ad" />
        </linearGradient>
    </defs>
    <rect width="600" height="100" fill="url(#waves)" />
</svg>
```

### Diagonal Stripes

```html
<svg width="400" height="400">
    <defs>
        <linearGradient id="diagonal" x1="0%" y1="0%" x2="5%" y2="5%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="50%" stop-color="#27ae60" />
            <stop offset="100%" stop-color="#2ecc71" />
        </linearGradient>
    </defs>
    <rect width="400" height="400" fill="url(#diagonal)" />
</svg>
```

### Radial Concentric Rings (Repeat)

```html
<svg width="400" height="400">
    <defs>
        <radialGradient id="rings" cx="50%" cy="50%" r="15%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#2980b9" />
            <stop offset="100%" stop-color="#3498db" />
        </radialGradient>
    </defs>
    <rect width="400" height="400" fill="url(#rings)" />
</svg>
```

### Radial Ripple Effect (Reflect)

```html
<svg width="400" height="400">
    <defs>
        <radialGradient id="ripples" cx="50%" cy="50%" r="20%" spreadMethod="reflect">
            <stop offset="0%" stop-color="#16a085" />
            <stop offset="100%" stop-color="#1abc9c" />
        </radialGradient>
    </defs>
    <rect width="400" height="400" fill="url(#ripples)" />
</svg>
```

### Barber Pole Pattern

```html
<svg width="150" height="400">
    <defs>
        <linearGradient id="barber" x1="0%" y1="0%" x2="15%" y2="15%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#e74c3c" />
            <stop offset="33%" stop-color="#e74c3c" />
            <stop offset="33%" stop-color="#ffffff" />
            <stop offset="67%" stop-color="#ffffff" />
            <stop offset="67%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>

    <rect x="25" y="0" width="100" height="400" rx="50" fill="url(#barber)" />
</svg>
```

### Grid Pattern Background

```html
<svg width="600" height="400">
    <defs>
        <linearGradient id="gridH" x1="0%" x2="5%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#ecf0f1" />
            <stop offset="10%" stop-color="#bdc3c7" />
            <stop offset="20%" stop-color="#ecf0f1" />
        </linearGradient>
    </defs>

    <rect width="600" height="400" fill="url(#gridH)" />
</svg>
```

### Candy Cane Stripes

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="candy" x1="0%" y1="0%" x2="10%" y2="0%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#e74c3c" />
            <stop offset="50%" stop-color="#e74c3c" />
            <stop offset="50%" stop-color="#ffffff" />
            <stop offset="100%" stop-color="#ffffff" />
        </linearGradient>
    </defs>

    <rect width="400" height="100" fill="url(#candy)" />
</svg>
```

### Progress Bar with Animated Stripes

```html
<svg width="400" height="60">
    <defs>
        <linearGradient id="progressStripes" x1="0%" x2="8%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="50%" stop-color="#27ae60" />
            <stop offset="100%" stop-color="#2ecc71" />
        </linearGradient>
    </defs>

    <rect x="0" y="0" width="400" height="60" rx="30" fill="#ecf0f1" />
    <rect x="0" y="0" width="280" height="60" rx="30" fill="url(#progressStripes)" />
</svg>
```

### Zebra Stripes

```html
<svg width="600" height="400">
    <defs>
        <linearGradient id="zebra" x1="0%" y1="0%" x2="3%" y2="0%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#2c3e50" />
            <stop offset="50%" stop-color="#2c3e50" />
            <stop offset="50%" stop-color="#ecf0f1" />
            <stop offset="100%" stop-color="#ecf0f1" />
        </linearGradient>
    </defs>

    <rect width="600" height="400" fill="url(#zebra)" />
</svg>
```

### Reflected Wave Pattern

```html
<svg width="600" height="150">
    <defs>
        <linearGradient id="reflectWave" x1="0%" x2="12%" spreadMethod="reflect">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#5dade2" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>

    <rect width="600" height="150" fill="url(#reflectWave)" />
</svg>
```

### Comparison of All Three Methods

```html
<svg width="600" height="450">
    <defs>
        <linearGradient id="comparePad" x1="20%" x2="80%" spreadMethod="pad">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>

        <linearGradient id="compareRepeat" x1="20%" x2="40%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>

        <linearGradient id="compareReflect" x1="20%" x2="40%" spreadMethod="reflect">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <text x="10" y="30" font-size="18" font-weight="bold">pad (default)</text>
    <rect x="10" y="40" width="580" height="100" fill="url(#comparePad)" />

    <text x="10" y="180" font-size="18" font-weight="bold">repeat</text>
    <rect x="10" y="190" width="580" height="100" fill="url(#compareRepeat)" />

    <text x="10" y="330" font-size="18" font-weight="bold">reflect</text>
    <rect x="10" y="340" width="580" height="100" fill="url(#compareReflect)" />
</svg>
```

### Data-Driven Stripe Pattern

```html
<!-- Model: { stripePattern: "repeat", stripeWidth: 8 } -->
<svg width="500" height="200">
    <defs>
        <linearGradient id="dataStripes"
                        x1="0%"
                        x2="{{model.stripeWidth}}%"
                        spreadMethod="{{model.stripePattern}}">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#2980b9" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>

    <rect width="500" height="200" fill="url(#dataStripes)" />
</svg>
```

### Chart with Striped Background

```html
<svg width="600" height="400">
    <defs>
        <linearGradient id="chartStripes" x1="0%" y1="0%" x2="0%" y2="10%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#ecf0f1" stop-opacity="0.5" />
            <stop offset="50%" stop-color="#ffffff" stop-opacity="0.2" />
            <stop offset="100%" stop-color="#ecf0f1" stop-opacity="0.5" />
        </linearGradient>
    </defs>

    <!-- Chart background -->
    <rect x="50" y="50" width="500" height="300" fill="url(#chartStripes)" />

    <!-- Chart data would go here -->
</svg>
```

---

## See Also

- [linearGradient](/reference/svgtags/linearGradient.html) - Linear gradient definition element
- [radialGradient](/reference/svgtags/radialGradient.html) - Radial gradient definition element
- [x1](/reference/svgattributes/x1.html), [y1](/reference/svgattributes/y1.html), [x2](/reference/svgattributes/x2.html), [y2](/reference/svgattributes/y2.html) - Linear gradient coordinates
- [cx](/reference/svgattributes/cx.html), [cy](/reference/svgattributes/cy.html), [r](/reference/svgattributes/r.html) - Radial gradient coordinates
- [gradientUnits](/reference/svgattributes/gradientUnits.html) - Coordinate system mode
- [stop](/reference/svgtags/stop.html) - Gradient color stop element
- [offset](/reference/svgattributes/offset.html) - Gradient stop position
- [Data Binding](/reference/binding/) - Data binding and expressions

---
