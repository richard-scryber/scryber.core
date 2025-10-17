---
layout: default
title: a (SVG)
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;a&gt; : The SVG Anchor/Link Element

The SVG `<a>` (anchor) element creates interactive hyperlinks within SVG graphics. It wraps SVG content to make shapes, groups, text, and other elements clickable, enabling navigation to external URLs, internal document locations, or other PDF destinations. This is the SVG-specific version of the HTML anchor element, optimized for use within SVG contexts.

## Usage

The SVG `<a>` element enables:
- Making SVG shapes, groups, and graphics interactive and clickable
- Linking to external URLs from within vector graphics
- Creating internal document navigation within SVG-based layouts
- Building interactive diagrams, maps, and infographics
- Wrapping complex SVG groups to make entire components clickable
- Supporting transformations on linked content
- Creating clickable icons and buttons in SVG format
- Maintaining accessibility with title and description elements

```html
<svg width="300pt" height="200pt">
    <a href="https://www.example.com">
        <rect x="50" y="50" width="200" height="100"
              fill="#336699" rx="10"/>
        <text x="150" y="105" text-anchor="middle" fill="white"
              font-size="20" font-weight="bold">
            Click Me
        </text>
    </a>
</svg>
```

---

## Supported Attributes

### Link Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `href` | string | **Required**. The link destination: URL, file path, or internal anchor (prefixed with #). |
| `target` | string | Link target. Use `_blank` to open in a new window. |

### Standard Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the anchor element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS/SVG styles applied to the anchor and its contents. |

### Transformation Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `transform` | string | Transformation operations: `translate(x,y)`, `rotate(angle)`, `scale(x,y)`, etc. Applied to entire linked content. |
| `transform-origin` | string | Origin point for transformations |

### Content Elements

| Element | Description |
|---------|-------------|
| `title` | Accessible title/tooltip text for the link |
| `desc` | Longer description of the link destination |
| Content | Any SVG shapes, text, groups, or container elements |

---

## Notes

### Link Types

The SVG anchor supports the same link types as HTML anchors:

1. **External URLs**: Absolute URLs (http://, https://, mailto:, etc.)
2. **Internal Anchors**: Links to elements with matching `id` within the document (`#id`)
3. **External Files**: Relative or absolute file paths
4. **PDF Actions**: Special `!ActionName` format for PDF navigation

```html
<!-- External URL -->
<a href="https://www.example.com">...</a>

<!-- Internal document link -->
<a href="#section2">...</a>

<!-- PDF navigation -->
<a href="!NextPage">...</a>

<!-- Email link -->
<a href="mailto:info@example.com">...</a>
```

### Clickable Area

The clickable area of an SVG anchor includes:
- All child shapes and their fill areas
- All stroked paths (including their stroke width)
- Text content
- The bounding box of all child elements combined

**Note**: Unlike HTML anchors, there's no default visual indication (underline) for SVG links. You must style them explicitly.

### Transformation Support

The `transform` attribute affects all children:

```html
<a href="#target" transform="rotate(45) translate(100,100)">
    <!-- All children are rotated and translated together -->
    <rect width="50" height="50" fill="blue"/>
    <text x="25" y="30" text-anchor="middle">Link</text>
</a>
```

### Style Inheritance

The SVG anchor element:
- Does **not** apply default blue color or underline (unlike HTML `<a>`)
- Removes default text decoration and fill color (uses inherited values)
- Allows full control over link appearance
- Styles cascade to all child elements unless overridden

### Nested Content

SVG anchors can wrap:
- Individual shapes (rect, circle, path, etc.)
- Groups (`<g>`) containing multiple elements
- Text elements
- Images
- Other complex SVG structures
- Referenced elements (`<use>`)

```html
<a href="#details">
    <g id="clickable-group">
        <rect width="100" height="100" fill="#336699"/>
        <circle cx="50" cy="50" r="30" fill="white"/>
        <text x="50" y="55" text-anchor="middle">Info</text>
    </g>
</a>
```

### Accessibility

Use `title` and `desc` elements for accessibility:

```html
<a href="https://example.com">
    <title>Visit Example Website</title>
    <desc>Opens the Example.com homepage in a new window</desc>
    <rect width="100" height="40" fill="#336699" rx="5"/>
    <text x="50" y="25" text-anchor="middle" fill="white">Visit</text>
</a>
```

### Class Hierarchy

In the Scryber codebase:
- `SVGAnchor` extends `HTMLAnchor` implements `ICloneable`
- Inherits link behavior from HTML anchor base class
- Supports SVG-specific transformations
- Removes default HTML link styling (blue color, underline)
- Can be cloned for use with `<use>` element

### Interaction with Use Element

SVG anchors can be defined in `<defs>` and reused:

```html
<svg width="400pt" height="200pt">
    <defs>
        <a id="buttonLink" href="https://example.com">
            <rect width="100" height="40" fill="#336699" rx="5"/>
            <text x="50" y="25" text-anchor="middle" fill="white">Click</text>
        </a>
    </defs>
    <use href="#buttonLink" x="50" y="80"/>
    <use href="#buttonLink" x="250" y="80"/>
</svg>
```

---

## Data Binding

### Dynamic Link Destinations

```html
<!-- Model: { url: "https://example.com", label: "Visit Site" } -->
<svg width="300pt" height="200pt">
    <a href="{{model.url}}">
        <rect x="50" y="75" width="200" height="50" fill="#336699" rx="5"/>
        <text x="150" y="105" text-anchor="middle" fill="white" font-size="16">
            {{model.label}}
        </text>
    </a>
</svg>
```

### Repeated Linked Elements

```html
<!-- Model: { links: [{url:"#sec1", label:"Section 1", x:50}, {url:"#sec2", label:"Section 2", x:200}] } -->
<svg width="400pt" height="100pt">
    <template data-bind="{{model.links}}">
        <a href="{{.url}}">
            <g transform="translate({{.x}}, 50)">
                <rect width="120" height="40" fill="#4a90e2" rx="5"/>
                <text x="60" y="25" text-anchor="middle" fill="white">{{.label}}</text>
            </g>
        </a>
    </template>
</svg>
```

### Conditional Link Styling

```html
<!-- Model: { items: [{id:"item1", url:"#detail1", active:true}, {id:"item2", url:"#detail2", active:false}] } -->
<svg width="400pt" height="200pt">
    <template data-bind="{{model.items}}">
        <a href="{{.url}}">
            <circle cx="{{100 + $index * 150}}" cy="100" r="40"
                    fill="{{.active ? '#50c878' : '#cccccc'}}"
                    stroke="{{.active ? '#2d7d4d' : '#999999'}}"
                    stroke-width="3"/>
            <text x="{{100 + $index * 150}}" y="105" text-anchor="middle"
                  fill="white" font-size="14">{{.id}}</text>
        </a>
    </template>
</svg>
```

### Interactive Map with Links

```html
<!-- Model: { locations: [{name:"NYC", x:100, y:80, url:"#nyc"}, {name:"LA", x:250, y:120, url:"#la"}] } -->
<svg width="400pt" height="300pt">
    <rect width="400" height="300" fill="#e8f4f8"/>
    <template data-bind="{{model.locations}}">
        <a href="{{.url}}">
            <g transform="translate({{.x}}, {{.y}})">
                <circle r="15" fill="#ff6347" stroke="white" stroke-width="2"/>
                <text y="35" text-anchor="middle" font-size="12" font-weight="bold">
                    {{.name}}
                </text>
            </g>
        </a>
    </template>
</svg>
```

---

## Examples

### Basic Clickable Rectangle

```html
<svg width="300pt" height="200pt">
    <a href="https://www.scryber.com">
        <rect x="75" y="75" width="150" height="50"
              fill="#336699" stroke="#254a70" stroke-width="2" rx="5"/>
        <text x="150" y="105" text-anchor="middle" fill="white"
              font-size="16" font-weight="bold">
            Visit Scryber
        </text>
    </a>
</svg>
```

### Clickable Circle Icon

```html
<svg width="200pt" height="200pt">
    <a href="#details">
        <circle cx="100" cy="100" r="60" fill="#4a90e2" stroke="#0066cc" stroke-width="3"/>
        <text x="100" y="110" text-anchor="middle" fill="white"
              font-size="24" font-weight="bold">
            i
        </text>
    </a>
</svg>
```

### Linked Group

```html
<svg width="300pt" height="200pt">
    <a href="https://example.com/products">
        <g id="product-card">
            <rect x="50" y="50" width="200" height="100"
                  fill="white" stroke="#ccc" stroke-width="2" rx="10"/>
            <rect x="60" y="60" width="50" height="50" fill="#f0f0f0"/>
            <text x="125" y="85" font-size="16" font-weight="bold">Product Name</text>
            <text x="125" y="110" font-size="12" fill="#666">Click for details</text>
        </g>
    </a>
</svg>
```

### Navigation Menu Links

```html
<svg width="400pt" height="100pt">
    <a href="#home">
        <rect x="10" y="25" width="80" height="50" fill="#336699" rx="5"/>
        <text x="50" y="55" text-anchor="middle" fill="white" font-size="14">Home</text>
    </a>

    <a href="#about">
        <rect x="110" y="25" width="80" height="50" fill="#336699" rx="5"/>
        <text x="150" y="55" text-anchor="middle" fill="white" font-size="14">About</text>
    </a>

    <a href="#contact">
        <rect x="210" y="25" width="80" height="50" fill="#336699" rx="5"/>
        <text x="250" y="55" text-anchor="middle" fill="white" font-size="14">Contact</text>
    </a>

    <a href="#help">
        <rect x="310" y="25" width="80" height="50" fill="#336699" rx="5"/>
        <text x="350" y="55" text-anchor="middle" fill="white" font-size="14">Help</text>
    </a>
</svg>
```

### Clickable Icons with Hover Effect

```html
<svg width="400pt" height="150pt">
    <defs>
        <g id="downloadIcon">
            <rect width="40" height="40" fill="#50c878" rx="5"/>
            <path d="M 20,10 L 20,25 M 12,20 L 20,28 L 28,20"
                  stroke="white" stroke-width="3" fill="none"
                  stroke-linecap="round" stroke-linejoin="round"/>
        </g>
    </defs>

    <a href="document.pdf">
        <use href="#downloadIcon" x="50" y="55"/>
        <text x="70" y="115" text-anchor="middle" font-size="12">Download PDF</text>
    </a>

    <a href="spreadsheet.xlsx">
        <use href="#downloadIcon" x="150" y="55"/>
        <text x="170" y="115" text-anchor="middle" font-size="12">Download XLSX</text>
    </a>

    <a href="image.png">
        <use href="#downloadIcon" x="250" y="55"/>
        <text x="270" y="115" text-anchor="middle" font-size="12">Download Image</text>
    </a>
</svg>
```

### Interactive Chart Elements

```html
<svg width="400pt" height="300pt">
    <a href="#q1-details">
        <rect x="50" y="150" width="60" height="100" fill="#4a90e2"
              stroke="#0066cc" stroke-width="2"/>
        <text x="80" y="265" text-anchor="middle" font-size="14">Q1</text>
    </a>

    <a href="#q2-details">
        <rect x="130" y="100" width="60" height="150" fill="#4a90e2"
              stroke="#0066cc" stroke-width="2"/>
        <text x="160" y="265" text-anchor="middle" font-size="14">Q2</text>
    </a>

    <a href="#q3-details">
        <rect x="210" y="120" width="60" height="130" fill="#4a90e2"
              stroke="#0066cc" stroke-width="2"/>
        <text x="240" y="265" text-anchor="middle" font-size="14">Q3</text>
    </a>

    <a href="#q4-details">
        <rect x="290" y="80" width="60" height="170" fill="#4a90e2"
              stroke="#0066cc" stroke-width="2"/>
        <text x="320" y="265" text-anchor="middle" font-size="14">Q4</text>
    </a>
</svg>
```

### Linked Logo

```html
<svg width="200pt" height="100pt">
    <a href="https://www.company.com">
        <defs>
            <linearGradient id="logoGrad">
                <stop offset="0%" stop-color="#336699"/>
                <stop offset="100%" stop-color="#6699cc"/>
            </linearGradient>
        </defs>
        <rect width="200" height="100" fill="url(#logoGrad)" rx="10"/>
        <text x="100" y="55" text-anchor="middle" fill="white"
              font-size="32" font-family="Arial" font-weight="bold">
            LOGO
        </text>
        <text x="100" y="75" text-anchor="middle" fill="white"
              font-size="10" font-family="Arial">
            Click to visit website
        </text>
    </a>
</svg>
```

### Map with Clickable Regions

```html
<svg width="400pt" height="300pt" viewBox="0 0 400 300">
    <rect width="400" height="300" fill="#e8f4f8"/>

    <a href="#region-north">
        <title>North Region</title>
        <polygon points="200,50 150,150 250,150" fill="#336699" opacity="0.7"/>
        <text x="200" y="120" text-anchor="middle" fill="white"
              font-size="14" font-weight="bold">North</text>
    </a>

    <a href="#region-south">
        <title>South Region</title>
        <polygon points="200,250 150,150 250,150" fill="#50c878" opacity="0.7"/>
        <text x="200" y="180" text-anchor="middle" fill="white"
              font-size="14" font-weight="bold">South</text>
    </a>

    <a href="#region-east">
        <title>East Region</title>
        <polygon points="300,150 250,100 250,200" fill="#ff9900" opacity="0.7"/>
        <text x="270" y="155" text-anchor="middle" fill="white"
              font-size="14" font-weight="bold">East</text>
    </a>

    <a href="#region-west">
        <title>West Region</title>
        <polygon points="100,150 150,100 150,200" fill="#ff6347" opacity="0.7"/>
        <text x="130" y="155" text-anchor="middle" fill="white"
              font-size="14" font-weight="bold">West</text>
    </a>
</svg>
```

### Social Media Icons

```html
<svg width="300pt" height="100pt">
    <defs>
        <g id="socialCircle">
            <circle r="25" fill="#336699" stroke="#254a70" stroke-width="2"/>
        </g>
    </defs>

    <a href="https://twitter.com/company">
        <use href="#socialCircle" x="50" y="50"/>
        <text x="50" y="58" text-anchor="middle" fill="white"
              font-size="20" font-weight="bold">T</text>
    </a>

    <a href="https://facebook.com/company">
        <use href="#socialCircle" x="150" y="50"/>
        <text x="150" y="58" text-anchor="middle" fill="white"
              font-size="20" font-weight="bold">F</text>
    </a>

    <a href="https://linkedin.com/company">
        <use href="#socialCircle" x="250" y="50"/>
        <text x="250" y="58" text-anchor="middle" fill="white"
              font-size="20" font-weight="bold">in</text>
    </a>
</svg>
```

### Call-to-Action Button

```html
<svg width="300pt" height="150pt">
    <defs>
        <linearGradient id="buttonGrad" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#50c878"/>
            <stop offset="100%" stop-color="#2d7d4d"/>
        </linearGradient>
        <filter id="buttonShadow">
            <feGaussianBlur in="SourceAlpha" stdDeviation="3"/>
            <feOffset dx="0" dy="4" result="offsetblur"/>
            <feMerge>
                <feMergeNode/>
                <feMergeNode in="SourceGraphic"/>
            </feMerge>
        </filter>
    </defs>

    <a href="https://example.com/signup">
        <rect x="50" y="50" width="200" height="60"
              fill="url(#buttonGrad)" rx="10"
              filter="url(#buttonShadow)"/>
        <text x="150" y="85" text-anchor="middle" fill="white"
              font-size="22" font-weight="bold">
            Sign Up Now
        </text>
    </a>
</svg>
```

### PDF Navigation Controls

```html
<svg width="300pt" height="80pt">
    <a href="!FirstPage">
        <rect x="10" y="20" width="60" height="40" fill="#336699" rx="5"/>
        <text x="40" y="45" text-anchor="middle" fill="white" font-size="12">First</text>
    </a>

    <a href="!PrevPage">
        <rect x="85" y="20" width="60" height="40" fill="#336699" rx="5"/>
        <text x="115" y="45" text-anchor="middle" fill="white" font-size="12">Prev</text>
    </a>

    <a href="!NextPage">
        <rect x="160" y="20" width="60" height="40" fill="#336699" rx="5"/>
        <text x="190" y="45" text-anchor="middle" fill="white" font-size="12">Next</text>
    </a>

    <a href="!LastPage">
        <rect x="235" y="20" width="60" height="40" fill="#336699" rx="5"/>
        <text x="265" y="45" text-anchor="middle" fill="white" font-size="12">Last</text>
    </a>
</svg>
```

### Clickable Diagram Nodes

```html
<svg width="400pt" height="300pt">
    <defs>
        <g id="node">
            <rect width="80" height="50" fill="white"
                  stroke="#336699" stroke-width="2" rx="5"/>
        </g>
    </defs>

    <!-- Connections -->
    <line x1="100" y1="75" x2="300" y2="75" stroke="#999" stroke-width="2"/>
    <line x1="100" y1="75" x2="200" y2="175" stroke="#999" stroke-width="2"/>
    <line x1="300" y1="75" x2="200" y2="175" stroke="#999" stroke-width="2"/>

    <!-- Nodes -->
    <a href="#node1-details">
        <use href="#node" x="60" y="50"/>
        <text x="100" y="80" text-anchor="middle" font-size="14">Node 1</text>
    </a>

    <a href="#node2-details">
        <use href="#node" x="260" y="50"/>
        <text x="300" y="80" text-anchor="middle" font-size="14">Node 2</text>
    </a>

    <a href="#node3-details">
        <use href="#node" x="160" y="150"/>
        <text x="200" y="180" text-anchor="middle" font-size="14">Node 3</text>
    </a>
</svg>
```

### Badge with Link

```html
<svg width="150pt" height="150pt">
    <a href="https://example.com/awards">
        <circle cx="75" cy="75" r="60" fill="#ff9900" stroke="#cc7700" stroke-width="3"/>
        <circle cx="75" cy="75" r="45" fill="white"/>
        <text x="75" y="65" text-anchor="middle" font-size="14"
              font-weight="bold" fill="#ff9900">
            BEST
        </text>
        <text x="75" y="85" text-anchor="middle" font-size="20"
              font-weight="bold" fill="#ff9900">
            2024
        </text>
    </a>
</svg>
```

### Product Gallery Links

```html
<svg width="450pt" height="200pt">
    <a href="#product1">
        <rect x="10" y="10" width="130" height="180" fill="white"
              stroke="#ddd" stroke-width="2" rx="5"/>
        <rect x="20" y="20" width="110" height="110" fill="#f0f0f0"/>
        <text x="75" y="155" text-anchor="middle" font-size="14"
              font-weight="bold">Product 1</text>
        <text x="75" y="175" text-anchor="middle" font-size="12"
              fill="#666">$99.99</text>
    </a>

    <a href="#product2">
        <rect x="160" y="10" width="130" height="180" fill="white"
              stroke="#ddd" stroke-width="2" rx="5"/>
        <rect x="170" y="20" width="110" height="110" fill="#f0f0f0"/>
        <text x="225" y="155" text-anchor="middle" font-size="14"
              font-weight="bold">Product 2</text>
        <text x="225" y="175" text-anchor="middle" font-size="12"
              fill="#666">$149.99</text>
    </a>

    <a href="#product3">
        <rect x="310" y="10" width="130" height="180" fill="white"
              stroke="#ddd" stroke-width="2" rx="5"/>
        <rect x="320" y="20" width="110" height="110" fill="#f0f0f0"/>
        <text x="375" y="155" text-anchor="middle" font-size="14"
              font-weight="bold">Product 3</text>
        <text x="375" y="175" text-anchor="middle" font-size="12"
              fill="#666">$199.99</text>
    </a>
</svg>
```

### Interactive Timeline

```html
<svg width="500pt" height="150pt">
    <line x1="50" y1="75" x2="450" y2="75" stroke="#336699" stroke-width="4"/>

    <a href="#event2020">
        <circle cx="100" cy="75" r="12" fill="#336699" stroke="white" stroke-width="3"/>
        <text x="100" y="55" text-anchor="middle" font-size="12" font-weight="bold">2020</text>
        <text x="100" y="110" text-anchor="middle" font-size="10">Founded</text>
    </a>

    <a href="#event2021">
        <circle cx="200" cy="75" r="12" fill="#336699" stroke="white" stroke-width="3"/>
        <text x="200" y="55" text-anchor="middle" font-size="12" font-weight="bold">2021</text>
        <text x="200" y="110" text-anchor="middle" font-size="10">Series A</text>
    </a>

    <a href="#event2022">
        <circle cx="300" cy="75" r="12" fill="#336699" stroke="white" stroke-width="3"/>
        <text x="300" y="55" text-anchor="middle" font-size="12" font-weight="bold">2022</text>
        <text x="300" y="110" text-anchor="middle" font-size="10">Expansion</text>
    </a>

    <a href="#event2023">
        <circle cx="400" cy="75" r="12" fill="#336699" stroke="white" stroke-width="3"/>
        <text x="400" y="55" text-anchor="middle" font-size="12" font-weight="bold">2023</text>
        <text x="400" y="110" text-anchor="middle" font-size="10">IPO</text>
    </a>
</svg>
```

### Linked Status Indicators

```html
<!-- Model: { servers: [{name:"Web", status:"ok", url:"#web"}, {name:"DB", status:"warning", url:"#db"}] } -->
<svg width="400pt" height="200pt">
    <template data-bind="{{model.servers}}">
        <a href="{{.url}}">
            <g transform="translate({{50 + $index * 150}}, 100)">
                <rect x="-50" y="-40" width="100" height="80"
                      fill="white" stroke="#ddd" stroke-width="2" rx="10"/>
                <circle cy="-10" r="15"
                        fill="{{.status == 'ok' ? '#50c878' : '#ff9900'}}"/>
                <text y="25" text-anchor="middle" font-size="14"
                      font-weight="bold">{{.name}}</text>
            </g>
        </a>
    </template>
</svg>
```

### Transformed Link

```html
<svg width="300pt" height="300pt">
    <a href="#rotated-section" transform="translate(150,150) rotate(45)">
        <rect x="-60" y="-30" width="120" height="60"
              fill="#4a90e2" stroke="#0066cc" stroke-width="2" rx="5"/>
        <text y="5" text-anchor="middle" fill="white"
              font-size="16" font-weight="bold">
            Rotated Link
        </text>
    </a>
</svg>
```

---

## See Also

- [a (HTML)](/reference/htmltags/a.html) - HTML anchor element
- [g](/reference/svgtags/g.html) - Group container (often wrapped in anchors)
- [svg](/reference/svgtags/svg.html) - SVG canvas container
- [use](/reference/svgtags/use.html) - Reference reusable elements
- [SVG Shapes](/reference/svgtags/shapes.html) - rect, circle, ellipse, line, polyline, polygon, path
- [SVG Transformations](/reference/svgtags/transforms.html) - Transform operations guide
- [Data Binding](/reference/binding/) - Data binding and expressions
- [PDF Actions](/reference/actions/) - PDF-specific navigation actions

---
