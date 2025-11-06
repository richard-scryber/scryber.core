---
layout: default
title: href
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @href : The Hyperlink Reference Attribute

The `href` attribute (or `xlink:href` in older SVG) specifies a reference to another resource or element. It's used to link to external resources (images, documents), reference reusable elements (gradients, patterns, markers), and create hyperlinks.

## Usage

The `href` attribute is used to:
- Reference reusable gradient and pattern definitions
- Link to external images and resources
- Reference markers for paths and lines
- Create clickable hyperlinks in SVG
- Support data-driven resource references
- Build component libraries with shared definitions

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <linearGradient id="myGradient" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db"/>
            <stop offset="100%" stop-color="#e74c3c"/>
        </linearGradient>
    </defs>

    <!-- Use href to reference gradient -->
    <rect x="50" y="50" width="300" height="200" fill="url(#myGradient)"/>

    <!-- Link to image -->
    <image href="image.png" x="100" y="100" width="200" height="100"/>

    <!-- Clickable link -->
    <a href="https://example.com">
        <text x="200" y="280" text-anchor="middle" fill="#3498db">Click Me</text>
    </a>
</svg>
```

---

## Supported Values

| Value Format | Description | Example |
|--------------|-------------|---------|
| `#id` | Reference to element in same document | `href="#myGradient"` or `fill="url(#myGradient)"` |
| `url(#id)` | URL reference to element (for fill/stroke) | `fill="url(#pattern1)"` |
| `file.ext` | Relative file path | `href="image.png"` |
| `/path/file.ext` | Absolute file path | `href="/images/logo.svg"` |
| `http://...` | Full URL | `href="https://example.com"` |
| `data:...` | Data URI | `href="data:image/svg+xml,..."` |

### Reference Syntax

```html
<!-- Gradient/pattern reference in fill/stroke -->
<rect fill="url(#gradientID)"/>
<circle stroke="url(#patternID)"/>

<!-- Direct element reference -->
<use href="#symbolID"/>
<image href="photo.jpg"/>

<!-- Hyperlink -->
<a href="https://example.com">
    <rect width="100" height="50"/>
</a>
```

---

## Supported Elements

The `href` attribute is supported on:

- **[&lt;use&gt;](/reference/svgtags/use.html)** - Reference to reusable element
- **[&lt;image&gt;](/reference/svgtags/image.html)** - External image reference
- **[&lt;a&gt;](/reference/svgtags/a.html)** - Hyperlink reference
- **[&lt;linearGradient&gt;](/reference/svgtags/linearGradient.html)** - Reference to another gradient (inheritance)
- **[&lt;radialGradient&gt;](/reference/svgtags/radialGradient.html)** - Reference to another gradient (inheritance)
- **[&lt;pattern&gt;](/reference/svgtags/pattern.html)** - Reference to another pattern (inheritance)
- **fill** and **stroke** attributes - Reference to paint servers via `url(#id)`

---

## Data Binding

### Dynamic Gradient References

Reference different gradients based on data:

```html
<!-- Model: { gradientType: 'blue', status: 'active' } -->
<svg width="400" height="300">
    <defs>
        <linearGradient id="blue-gradient" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db"/>
            <stop offset="100%" stop-color="#2980b9"/>
        </linearGradient>
        <linearGradient id="red-gradient" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#e74c3c"/>
            <stop offset="100%" stop-color="#c0392b"/>
        </linearGradient>
        <linearGradient id="green-gradient" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#2ecc71"/>
            <stop offset="100%" stop-color="#27ae60"/>
        </linearGradient>
    </defs>

    <rect x="50" y="50" width="300" height="200"
          fill="url(#{{model.gradientType}}-gradient)"
          stroke="#2c3e50" stroke-width="2"/>
    <text x="200" y="270" text-anchor="middle">
        Status: {{model.status}} ({{model.gradientType}})
    </text>
</svg>
```

### Dynamic Image Sources

Load images from data properties:

```html
<!-- Model: { images: [{src: 'photo1.jpg', x: 50, y: 50}, {src: 'photo2.jpg', x: 250, y: 50}] } -->
<svg width="500" height="300">
    <template data-bind="{{model.images}}">
        <image href="{{.src}}" x="{{.x}}" y="{{.y}}"
               width="180" height="150"
               preserveAspectRatio="xMidYMid slice"/>
        <rect x="{{.x}}" y="{{.y}}" width="180" height="150"
              fill="none" stroke="#2c3e50" stroke-width="2"/>
    </template>
</svg>
```

### Reusable Components with Use

Reference symbols dynamically:

```html
<!-- Model: { icons: [{type: 'check', x: 50, y: 50}, {type: 'cross', x: 150, y: 50}, {type: 'star', x: 250, y: 50}] } -->
<svg width="400" height="200">
    <defs>
        <symbol id="check" viewBox="0 0 24 24">
            <path d="M 5,12 L 10,17 L 20,7" fill="none" stroke="#2ecc71" stroke-width="3"/>
        </symbol>
        <symbol id="cross" viewBox="0 0 24 24">
            <path d="M 7,7 L 17,17 M 17,7 L 7,17" fill="none" stroke="#e74c3c" stroke-width="3"/>
        </symbol>
        <symbol id="star" viewBox="0 0 24 24">
            <path d="M 12,2 L 14,9 L 22,9 L 16,14 L 18,22 L 12,17 L 6,22 L 8,14 L 2,9 L 10,9 Z"
                  fill="#f39c12"/>
        </symbol>
    </defs>

    <template data-bind="{{model.icons}}">
        <use href="#{{.type}}" x="{{.x}}" y="{{.y}}" width="80" height="80"/>
    </template>
</svg>
```

### Dynamic Hyperlinks

Create clickable elements with data-driven URLs:

```html
<!-- Model: { buttons: [{label: 'Home', url: '/home', color: '#3498db'}, {label: 'About', url: '/about', color: '#2ecc71'}] } -->
<svg width="400" height="200">
    <template data-bind="{{model.buttons}}">
        <a href="{{.url}}">
            <rect x="{{$index * 150 + 50}}" y="50" width="120" height="50"
                  rx="5" fill="{{.color}}" style="cursor:pointer"/>
            <text x="{{$index * 150 + 110}}" y="82"
                  text-anchor="middle" fill="white"
                  font-size="16" font-weight="bold">{{.label}}</text>
        </a>
    </template>
</svg>
```

### Pattern Library References

Reference patterns from a library:

```html
<!-- Model: { surfaces: [{pattern: 'dots', x: 50, y: 50}, {pattern: 'stripes', x: 250, y: 50}] } -->
<svg width="500" height="300">
    <defs>
        <pattern id="dots" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <circle cx="10" cy="10" r="5" fill="#3498db"/>
        </pattern>
        <pattern id="stripes" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="10" height="20" fill="#e74c3c"/>
            <rect x="10" y="0" width="10" height="20" fill="#c0392b"/>
        </pattern>
    </defs>

    <template data-bind="{{model.surfaces}}">
        <rect x="{{.x}}" y="{{.y}}" width="180" height="150"
              fill="url(#{{.pattern}})" stroke="#2c3e50" stroke-width="2"/>
    </template>
</svg>
```

---

## Notes

### URL Reference Format

**For fill and stroke:**
```html
<!-- Use url(#id) format -->
<rect fill="url(#myGradient)"/>
<circle stroke="url(#myPattern)"/>
```

**For element references:**
```html
<!-- Direct href -->
<use href="#mySymbol"/>
<image href="photo.jpg"/>
```

### ID References (#id)

- Reference elements defined in `<defs>` section
- ID must be unique within document
- Use with gradients, patterns, markers, symbols
- Fragment identifier syntax: `#elementID`

```html
<defs>
    <linearGradient id="grad1">...</linearGradient>
</defs>
<rect fill="url(#grad1)"/>
```

### External Resources

**Image files:**
```html
<image href="image.png"/>        <!-- Relative path -->
<image href="/assets/logo.svg"/> <!-- Absolute path -->
<image href="https://example.com/photo.jpg"/> <!-- Full URL -->
```

**Supported formats:**
- PNG, JPEG, GIF (raster images)
- SVG (vector images)
- Data URIs (embedded data)

### Gradient and Pattern Inheritance

Gradients and patterns can reference others for inheritance:

```html
<defs>
    <!-- Base gradient -->
    <linearGradient id="base-gradient" x1="0%" y1="0%" x2="100%" y2="0%">
        <stop offset="0%" stop-color="#3498db"/>
        <stop offset="100%" stop-color="#2980b9"/>
    </linearGradient>

    <!-- Inherits colors, changes direction -->
    <linearGradient id="vertical-gradient" href="#base-gradient"
                    x1="0%" y1="0%" x2="0%" y2="100%"/>
</defs>
```

### xlink:href vs href

**Modern SVG (SVG 2):**
```html
<use href="#myElement"/>
<image href="photo.jpg"/>
```

**Legacy SVG (SVG 1.1):**
```html
<use xlink:href="#myElement"/>
<image xlink:href="photo.jpg"/>
```

**Browser compatibility:**
- Modern browsers support both
- Use `href` for new documents
- Use `xlink:href` for legacy support
- Some tools/libraries may require xlink namespace

### Hyperlinks with &lt;a&gt;

Create clickable regions:

```html
<a href="https://example.com">
    <rect width="100" height="50" fill="blue"/>
    <text x="50" y="30" text-anchor="middle" fill="white">Click</text>
</a>
```

**Link targets:**
```html
<a href="https://example.com" target="_blank">  <!-- New window -->
<a href="https://example.com" target="_self">   <!-- Same window -->
<a href="https://example.com" target="_parent"> <!-- Parent frame -->
```

### Use Element References

The `<use>` element clones referenced content:

```html
<defs>
    <g id="shape">
        <circle cx="20" cy="20" r="15" fill="blue"/>
        <text x="20" y="25" text-anchor="middle" fill="white">A</text>
    </g>
</defs>

<!-- Reuse multiple times -->
<use href="#shape" x="0" y="0"/>
<use href="#shape" x="50" y="0"/>
<use href="#shape" x="100" y="0"/>
```

### Security Considerations

**External resources:**
- Be cautious with user-provided URLs
- Validate image sources
- Consider CORS restrictions
- Use HTTPS for secure resources

**XSS prevention:**
- Sanitize href values from user input
- Avoid javascript: URLs
- Validate URL protocols

### Performance Considerations

**Reusable elements:**
- Define once in `<defs>`, reference multiple times
- Reduces file size
- Improves rendering performance
- Better for maintenance

**External images:**
- Consider image loading time
- Use appropriate image formats
- Optimize image sizes
- Consider lazy loading for many images

### Common Use Cases

**Gradient fills:**
```html
<rect fill="url(#myGradient)"/>
```

**Pattern fills:**
```html
<circle fill="url(#myPattern)"/>
```

**Reusable graphics:**
```html
<use href="#icon-star"/>
```

**External images:**
```html
<image href="photo.jpg"/>
```

**Clickable links:**
```html
<a href="https://example.com">
    <text>Click here</text>
</a>
```

**Marker references:**
```html
<line marker-end="url(#arrow)"/>
```

---

## Examples

### Gradient Reference

```html
<svg width="400" height="200">
    <defs>
        <linearGradient id="blueGrad" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db"/>
            <stop offset="100%" stop-color="#2980b9"/>
        </linearGradient>
    </defs>

    <rect x="50" y="50" width="300" height="100"
          fill="url(#blueGrad)" stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Multiple Pattern References

```html
<svg width="500" height="300">
    <defs>
        <pattern id="dots" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <circle cx="10" cy="10" r="5" fill="#3498db"/>
        </pattern>
        <pattern id="stripes" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="10" height="20" fill="#e74c3c"/>
            <rect x="10" y="0" width="10" height="20" fill="#c0392b"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="180" height="200" fill="url(#dots)" stroke="#2c3e50"/>
    <rect x="270" y="50" width="180" height="200" fill="url(#stripes)" stroke="#2c3e50"/>
</svg>
```

### Using Symbols with Use

```html
<svg width="400" height="200">
    <defs>
        <symbol id="star" viewBox="0 0 24 24">
            <path d="M 12,2 L 14,9 L 22,9 L 16,14 L 18,22 L 12,17 L 6,22 L 8,14 L 2,9 L 10,9 Z"
                  fill="#f39c12"/>
        </symbol>
    </defs>

    <use href="#star" x="50" y="50" width="60" height="60"/>
    <use href="#star" x="150" y="50" width="60" height="60"/>
    <use href="#star" x="250" y="50" width="60" height="60"/>
    <use href="#star" x="350" y="50" width="60" height="60"/>
</svg>
```

### External Image Reference

```html
<svg width="400" height="300">
    <image href="photo.jpg" x="50" y="50" width="300" height="200"
           preserveAspectRatio="xMidYMid slice"/>
    <rect x="50" y="50" width="300" height="200"
          fill="none" stroke="#2c3e50" stroke-width="3"/>
</svg>
```

### Clickable Button Link

```html
<svg width="300" height="150">
    <a href="https://example.com" target="_blank">
        <rect x="50" y="50" width="200" height="50" rx="10"
              fill="#3498db" style="cursor:pointer"/>
        <text x="150" y="82" text-anchor="middle"
              fill="white" font-size="18" font-weight="bold"
              style="pointer-events:none">Visit Website</text>
    </a>
</svg>
```

### Icon Library with Reuse

```html
<svg width="500" height="200">
    <defs>
        <symbol id="home" viewBox="0 0 24 24">
            <path d="M 12,3 L 20,10 L 20,20 L 4,20 L 4,10 Z M 9,20 L 9,14 L 15,14 L 15,20"
                  fill="#3498db" stroke="#2c3e50" stroke-width="1"/>
        </symbol>
        <symbol id="mail" viewBox="0 0 24 24">
            <rect x="2" y="5" width="20" height="14" rx="2"
                  fill="none" stroke="#3498db" stroke-width="2"/>
            <path d="M 2,7 L 12,13 L 22,7" fill="none" stroke="#3498db" stroke-width="2"/>
        </symbol>
        <symbol id="settings" viewBox="0 0 24 24">
            <circle cx="12" cy="12" r="3" fill="#3498db"/>
            <path d="M 12,1 L 12,5 M 12,19 L 12,23 M 1,12 L 5,12 M 19,12 L 23,12
                     M 4.93,4.93 L 7.76,7.76 M 16.24,16.24 L 19.07,19.07
                     M 4.93,19.07 L 7.76,16.24 M 16.24,7.76 L 19.07,4.93"
                  stroke="#3498db" stroke-width="2"/>
        </symbol>
    </defs>

    <use href="#home" x="50" y="50" width="80" height="80"/>
    <use href="#mail" x="180" y="50" width="80" height="80"/>
    <use href="#settings" x="310" y="50" width="80" height="80"/>
</svg>
```

### Gradient Inheritance

```html
<svg width="500" height="300">
    <defs>
        <!-- Base gradient with color stops -->
        <linearGradient id="baseGrad">
            <stop offset="0%" stop-color="#3498db"/>
            <stop offset="100%" stop-color="#e74c3c"/>
        </linearGradient>

        <!-- Horizontal gradient (inherits colors) -->
        <linearGradient id="horizGrad" href="#baseGrad"
                        x1="0%" y1="0%" x2="100%" y2="0%"/>

        <!-- Vertical gradient (inherits colors) -->
        <linearGradient id="vertGrad" href="#baseGrad"
                        x1="0%" y1="0%" x2="0%" y2="100%"/>

        <!-- Diagonal gradient (inherits colors) -->
        <linearGradient id="diagGrad" href="#baseGrad"
                        x1="0%" y1="0%" x2="100%" y2="100%"/>
    </defs>

    <rect x="50" y="50" width="120" height="200" fill="url(#horizGrad)" stroke="#2c3e50"/>
    <rect x="190" y="50" width="120" height="200" fill="url(#vertGrad)" stroke="#2c3e50"/>
    <rect x="330" y="50" width="120" height="200" fill="url(#diagGrad)" stroke="#2c3e50"/>
</svg>
```

### Marker References on Paths

```html
<svg width="400" height="300">
    <defs>
        <marker id="arrow" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
        <marker id="dot" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#e74c3c"/>
        </marker>
    </defs>

    <path d="M 50,50 L 200,50 L 200,150 L 350,150"
          fill="none" stroke="#2c3e50" stroke-width="3"
          marker-start="url(#dot)" marker-end="url(#arrow)"/>

    <path d="M 50,200 Q 200,100 350,200"
          fill="none" stroke="#2c3e50" stroke-width="3"
          marker-start="url(#dot)" marker-end="url(#arrow)"/>
</svg>
```

### Multiple Image Gallery

```html
<svg width="600" height="400">
    <image href="image1.jpg" x="20" y="20" width="180" height="150"
           preserveAspectRatio="xMidYMid slice"/>
    <rect x="20" y="20" width="180" height="150" fill="none" stroke="#2c3e50" stroke-width="2"/>

    <image href="image2.jpg" x="210" y="20" width="180" height="150"
           preserveAspectRatio="xMidYMid slice"/>
    <rect x="210" y="20" width="180" height="150" fill="none" stroke="#2c3e50" stroke-width="2"/>

    <image href="image3.jpg" x="400" y="20" width="180" height="150"
           preserveAspectRatio="xMidYMid slice"/>
    <rect x="400" y="20" width="180" height="150" fill="none" stroke="#2c3e50" stroke-width="2"/>

    <image href="image4.jpg" x="20" y="230" width="180" height="150"
           preserveAspectRatio="xMidYMid slice"/>
    <rect x="20" y="230" width="180" height="150" fill="none" stroke="#2c3e50" stroke-width="2"/>

    <image href="image5.jpg" x="210" y="230" width="180" height="150"
           preserveAspectRatio="xMidYMid slice"/>
    <rect x="210" y="230" width="180" height="150" fill="none" stroke="#2c3e50" stroke-width="2"/>

    <image href="image6.jpg" x="400" y="230" width="180" height="150"
           preserveAspectRatio="xMidYMid slice"/>
    <rect x="400" y="230" width="180" height="150" fill="none" stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Navigation Menu with Links

```html
<svg width="600" height="80">
    <defs>
        <linearGradient id="btnGrad" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db"/>
            <stop offset="100%" stop-color="#2980b9"/>
        </linearGradient>
    </defs>

    <a href="/home">
        <rect x="20" y="20" width="100" height="40" rx="5"
              fill="url(#btnGrad)" style="cursor:pointer"/>
        <text x="70" y="45" text-anchor="middle" fill="white"
              font-weight="bold" style="pointer-events:none">Home</text>
    </a>

    <a href="/products">
        <rect x="140" y="20" width="100" height="40" rx="5"
              fill="url(#btnGrad)" style="cursor:pointer"/>
        <text x="190" y="45" text-anchor="middle" fill="white"
              font-weight="bold" style="pointer-events:none">Products</text>
    </a>

    <a href="/about">
        <rect x="260" y="20" width="100" height="40" rx="5"
              fill="url(#btnGrad)" style="cursor:pointer"/>
        <text x="310" y="45" text-anchor="middle" fill="white"
              font-weight="bold" style="pointer-events:none">About</text>
    </a>

    <a href="/contact">
        <rect x="380" y="20" width="100" height="40" rx="5"
              fill="url(#btnGrad)" style="cursor:pointer"/>
        <text x="430" y="45" text-anchor="middle" fill="white"
              font-weight="bold" style="pointer-events:none">Contact</text>
    </a>
</svg>
```

### Reusable Component System

```html
<svg width="600" height="400">
    <defs>
        <!-- Badge component -->
        <g id="badge">
            <rect width="80" height="30" rx="15" fill="#2ecc71"/>
            <text x="40" y="20" text-anchor="middle" fill="white"
                  font-size="12" font-weight="bold">NEW</text>
        </g>

        <!-- Warning icon -->
        <g id="warning">
            <path d="M 12,2 L 22,20 L 2,20 Z" fill="#f39c12"/>
            <path d="M 12,8 L 12,14 M 12,16 L 12,18" stroke="white" stroke-width="2"/>
        </g>

        <!-- Success checkmark -->
        <g id="success">
            <circle cx="12" cy="12" r="10" fill="#2ecc71"/>
            <path d="M 7,12 L 11,16 L 17,8" fill="none" stroke="white" stroke-width="2"/>
        </g>
    </defs>

    <g transform="translate(50, 50)">
        <rect width="150" height="100" fill="#ecf0f1" stroke="#bdc3c7" stroke-width="2"/>
        <use href="#badge" x="70" y="5"/>
        <text x="75" y="60" text-anchor="middle">Product A</text>
    </g>

    <g transform="translate(230, 50)">
        <rect width="150" height="100" fill="#ecf0f1" stroke="#bdc3c7" stroke-width="2"/>
        <use href="#warning" x="119" y="10" width="24" height="24"/>
        <text x="75" y="60" text-anchor="middle">Product B</text>
    </g>

    <g transform="translate(410, 50)">
        <rect width="150" height="100" fill="#ecf0f1" stroke="#bdc3c7" stroke-width="2"/>
        <use href="#success" x="119" y="10" width="24" height="24"/>
        <text x="75" y="60" text-anchor="middle">Product C</text>
    </g>
</svg>
```

### Radial Gradient Reference

```html
<svg width="400" height="300">
    <defs>
        <radialGradient id="radGrad" cx="50%" cy="50%" r="50%">
            <stop offset="0%" stop-color="#ffffff"/>
            <stop offset="100%" stop-color="#3498db"/>
        </radialGradient>
    </defs>

    <circle cx="200" cy="150" r="120" fill="url(#radGrad)" stroke="#2c3e50" stroke-width="2"/>
</svg>
```

### Data URI Image Embedding

```html
<svg width="400" height="300">
    <image href="data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='100' height='100'%3E%3Crect width='100' height='100' fill='%233498db'/%3E%3Ccircle cx='50' cy='50' r='30' fill='white'/%3E%3C/svg%3E"
           x="150" y="100" width="100" height="100"/>
</svg>
```

### Pattern Reference in Stroke

```html
<svg width="400" height="300">
    <defs>
        <pattern id="dashPattern" x="0" y="0" width="10" height="10" patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="5" height="10" fill="#3498db"/>
        </pattern>
    </defs>

    <rect x="50" y="50" width="300" height="200"
          fill="none" stroke="url(#dashPattern)" stroke-width="10"/>
</svg>
```

### Animated Element Reference

```html
<svg width="400" height="200">
    <defs>
        <g id="animatedShape">
            <circle cx="20" cy="20" r="15" fill="#e74c3c">
                <animate attributeName="r" from="15" to="20" dur="1s"
                         repeatCount="indefinite" direction="alternate"/>
            </circle>
        </g>
    </defs>

    <use href="#animatedShape" x="50" y="50"/>
    <use href="#animatedShape" x="150" y="50"/>
    <use href="#animatedShape" x="250" y="50"/>
</svg>
```

### Complex Reusable Graphics

```html
<svg width="500" height="300">
    <defs>
        <g id="card">
            <rect width="120" height="160" rx="8" fill="white" stroke="#bdc3c7" stroke-width="2"/>
            <rect x="10" y="10" width="100" height="80" rx="4" fill="#ecf0f1"/>
            <rect x="10" y="100" width="100" height="8" rx="4" fill="#95a5a6"/>
            <rect x="10" y="118" width="60" height="8" rx="4" fill="#bdc3c7"/>
            <rect x="10" y="136" width="80" height="8" rx="4" fill="#bdc3c7"/>
        </g>
    </defs>

    <use href="#card" x="30" y="50"/>
    <use href="#card" x="180" y="50"/>
    <use href="#card" x="330" y="50"/>
</svg>
```

---

## See Also

- [use](/reference/svgtags/use.html) - Use element for referencing
- [image](/reference/svgtags/image.html) - Image element
- [a](/reference/svgtags/a.html) - Anchor/link element
- [linearGradient](/reference/svgtags/linearGradient.html) - Linear gradient element
- [radialGradient](/reference/svgtags/radialGradient.html) - Radial gradient element
- [pattern](/reference/svgtags/pattern.html) - Pattern element
- [marker](/reference/svgtags/marker.html) - Marker element
- [symbol](/reference/svgtags/symbol.html) - Symbol element for reuse
- [defs](/reference/svgtags/defs.html) - Definitions container
- [Data Binding](/reference/binding/) - Data binding and expressions

---
