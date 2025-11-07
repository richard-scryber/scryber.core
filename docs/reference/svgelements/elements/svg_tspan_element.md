---
layout: default
title: tspan (SVG)
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;tspan&gt; : The SVG Text Span Element

The `<tspan>` element is used to create styled spans of text within SVG `<text>` elements. It allows for fine-grained control over text formatting, positioning, and appearance without requiring separate text elements.

---

## Summary

The `<tspan>` element provides inline text formatting capabilities within SVG text. It can override parent text styles, adjust positioning, and create multi-styled text runs. Think of it as the SVG equivalent of an HTML `<span>` element, but with SVG-specific positioning capabilities.

Key features:
- Override font properties for text segments
- Adjust position with x, y coordinates or dx, dy offsets
- Apply unique fill and stroke colors
- Create multi-line text effects
- Support for data binding on content and styles
- Nest multiple tspan elements for complex layouts

---

## Usage

The `<tspan>` element must be placed within a `<text>` element or another `<tspan>`:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="60">
    <text x="10" y="30" font-size="16">
        Normal text <tspan font-weight="700" fill="blue">bold blue text</tspan> normal again
    </text>
</svg>
```

### Basic Syntax

```html
<!-- Styled span within text -->
<text x="10" y="30">
    Regular <tspan fill="red">colored</tspan> text
</text>

<!-- Position adjustment with dx, dy -->
<text x="10" y="30">
    Base<tspan dy="-5" font-size="10">superscript</tspan>
</text>

<!-- Absolute repositioning with x, y -->
<text x="10" y="30">
    First line
    <tspan x="10" y="50">Second line</tspan>
</text>

<!-- Multiple style overrides -->
<text x="10" y="30" font-size="14">
    <tspan font-weight="700" fill="#336699" font-size="18">Heading</tspan>
    <tspan dx="5" fill="#666">Details</tspan>
</text>
```

---

## Supported Attributes

### Position Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `x` | Unit | Absolute horizontal position (overrides parent) | Inherited |
| `y` | Unit | Absolute vertical position (overrides parent) | Inherited |
| `dx` | Unit | Relative horizontal offset from current position | 0 |
| `dy` | Unit | Relative vertical offset from current position | 0 |

### Font Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `font-family` | FontSelector | Font family name or list | Inherited |
| `font-size` | Unit | Font size | Inherited |
| `font-weight` | Integer | Font weight (100-900) | Inherited |
| `font-style` | Enum | Font style: `normal`, `italic`, `oblique` | Inherited |

### Appearance Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `fill` | Color/URL | Fill color or paint server reference | Inherited |
| `fill-opacity` | Double | Fill opacity (0.0-1.0) | Inherited |
| `stroke` | Color | Stroke color for text outline | Inherited |
| `stroke-width` | Unit | Width of text stroke | Inherited |
| `stroke-opacity` | Double | Stroke opacity (0.0-1.0) | Inherited |

### Stroke Style Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `stroke-linecap` | String | Line cap style: `butt`, `round`, `square` | Inherited |
| `stroke-linejoin` | String | Line join style: `miter`, `round`, `bevel` | Inherited |
| `stroke-dasharray` | Dash | Dash pattern for stroked text | Inherited |

### Text Formatting Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `text-decoration` | String | Text decoration: `none`, `underline`, `overline`, `line-through` | Inherited |
| `textLength` | Unit | Target length for text rendering | auto |
| `lengthAdjust` | Enum | Length adjustment method: `spacing`, `spacingAndGlyphs` | spacing |

### Common Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `class` | String | CSS class name(s) for styling |
| `style` | Style | Inline CSS-style properties |

---

## Data Binding

The `<tspan>` element supports comprehensive data binding for dynamic content and styling.

### Basic Content Binding

```html
<text x="10" y="30" font-size="16">
    Customer: <tspan font-weight="700">{{model.customerName}}</tspan>
</text>

<!-- Expression binding -->
<text x="10" y="30">
    Total: <tspan fill="#336699" font-size="20">${{model.total.toFixed(2)}}</tspan>
</text>

<!-- Conditional styling -->
<text x="10" y="30">
    Status:
    <tspan fill="{{model.isActive ? 'green' : 'red'}}" font-weight="700">
        {{model.isActive ? 'Active' : 'Inactive'}}
    </tspan>
</text>
```

### Dynamic Positioning

```html
<!-- Relative offset based on data -->
<text x="10" y="30" font-size="16">
    Value<tspan dy="{{model.isExponent ? -8 : 0}}" font-size="12">{{model.exponent}}</tspan>
</text>

<!-- Absolute positioning -->
<text x="10" y="30">
    Label
    <tspan x="{{model.valueX}}" y="{{model.valueY}}" fill="blue">
        {{model.value}}
    </tspan>
</text>
```

### Style Binding

```html
<!-- Data-driven font size -->
<text x="10" y="30">
    <tspan font-size="{{model.importance * 4 + 12}}" font-weight="700">
        {{model.message}}
    </tspan>
</text>

<!-- Conditional colors -->
<text x="10" y="30">
    Score:
    <tspan fill="{{model.score >= 80 ? '#00aa00' : model.score >= 60 ? '#ff6600' : '#cc0000'}}"
           font-weight="700"
           font-size="20">
        {{model.score}}
    </tspan>
</text>
```

### Template Iteration

```html
<!-- Generate styled text segments -->
<text x="10" y="30" font-size="14">
    <template data-bind="{{model.keywords}}">
        <tspan fill="#336699" font-weight="600">{{.}}</tspan>
        <tspan fill="#999"> • </tspan>
    </template>
</text>
```

---

## Notes

### Positioning Behavior

- `x` and `y` attributes create absolute positioning from the SVG origin
- `dx` and `dy` attributes create relative offsets from the current text position
- When `x` or `y` is set, the tspan starts a new text positioning context
- Nested tspans inherit position from their parent if not explicitly set

### Inheritance

- Most styling attributes inherit from the parent `<text>` or `<tspan>` element
- Explicitly set attributes override inherited values
- Font properties, colors, and text formatting all follow CSS inheritance rules
- Position attributes (x, y, dx, dy) do not inherit - they default to 0 or auto

### Multi-Line Text

Create multi-line text by setting absolute `y` positions:

```html
<text x="10" y="30">
    <tspan x="10" y="30">First line</tspan>
    <tspan x="10" y="50">Second line</tspan>
    <tspan x="10" y="70">Third line</tspan>
</text>
```

### Superscript and Subscript

Use `dy` for vertical offset:
- Superscript: negative dy value and smaller font-size
- Subscript: positive dy value and smaller font-size
- Remember to reset position after super/subscript

```html
<text x="10" y="30" font-size="16">
    E = mc<tspan dy="5" font-size="12">2</tspan>
    <tspan dy="-5"> </tspan> (subscript example)
</text>
```

### Nesting

- Tspan elements can be nested within other tspan elements
- Each level inherits from its parent
- Useful for creating complex hierarchical styling
- Avoid excessive nesting (3+ levels) for maintainability

### Performance

- Tspan elements are lightweight and efficient
- Use tspan instead of multiple text elements when possible
- Style inheritance reduces PDF file size
- Data binding on tspan is as efficient as on text elements

### Limitations

- Tspan cannot contain block-level elements
- Automatic text wrapping is not supported
- Complex text effects may require multiple tspan elements
- Some font features may have limited support depending on font

---

## Examples

### 1. Bold Emphasis

Emphasize specific words in text:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="60">
    <text x="10" y="30" font-size="14" fill="#333">
        This is <tspan font-weight="700">important</tspan> information.
    </text>
</svg>
```

### 2. Color Highlighting

Highlight text with different colors:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="60">
    <text x="10" y="30" font-size="16">
        Status: <tspan fill="#00aa00" font-weight="600">SUCCESS</tspan>
    </text>
</svg>
```

### 3. Multi-Line Text

Create multi-line text with consistent alignment:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="120">
    <text x="10" y="30" font-size="14" fill="#333">
        <tspan x="10" y="30">Line 1: First line of text</tspan>
        <tspan x="10" y="50">Line 2: Second line of text</tspan>
        <tspan x="10" y="70">Line 3: Third line of text</tspan>
    </text>
</svg>
```

### 4. Superscript Notation

Mathematical or scientific notation:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="60">
    <text x="10" y="35" font-size="18" fill="#333">
        Area = πr<tspan dy="-8" font-size="12">2</tspan>
    </text>
</svg>
```

### 5. Subscript Notation

Chemical formulas or mathematical subscripts:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="60">
    <text x="10" y="30" font-size="18" fill="#333">
        H<tspan dy="5" font-size="12">2</tspan>
        <tspan dy="-5">O</tspan>
    </text>
</svg>
```

### 6. Mixed Font Sizes

Combine different font sizes in one line:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="60">
    <text x="10" y="40" font-size="14" fill="#333">
        <tspan font-size="24" font-weight="700" fill="#336699">BIG</tspan>
        <tspan dx="5">regular</tspan>
        <tspan dx="5" font-size="10">small</tspan>
    </text>
</svg>
```

### 7. Currency Display

Format currency with styled components:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="250" height="60">
    <text x="10" y="40" font-size="14" fill="#666">
        <tspan font-size="20" fill="#333">$</tspan>
        <tspan font-size="32" font-weight="700" fill="#336699">1,234</tspan>
        <tspan font-size="20" fill="#666">.56</tspan>
    </text>
</svg>
```

### 8. Label with Value

Key-value pair with different styling:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="60">
    <text x="10" y="30" font-size="14">
        <tspan fill="#666">Customer Name:</tspan>
        <tspan dx="5" fill="#333" font-weight="600">Acme Corporation</tspan>
    </text>
</svg>
```

### 9. Status Badge Text

Multi-colored status indicator:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="60">
    <rect x="10" y="15" width="150" height="30" rx="5" fill="#f0f0f0"/>
    <text x="85" y="35" text-anchor="middle" font-size="14">
        <tspan fill="#666">Status:</tspan>
        <tspan dx="5" fill="#00aa00" font-weight="700">Active</tspan>
    </text>
</svg>
```

### 10. Data-Bound Customer Info

Dynamic customer information with styling:

```html
<!-- Model: { customerName: "John Doe", accountType: "Premium", balance: 5432.10 } -->
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="100">
    <text x="10" y="30" font-size="14" fill="#333">
        <tspan font-weight="700" font-size="16">{{model.customerName}}</tspan>
    </text>
    <text x="10" y="55" font-size="12">
        <tspan fill="#666">Account Type:</tspan>
        <tspan dx="5" fill="#336699" font-weight="600">{{model.accountType}}</tspan>
    </text>
    <text x="10" y="75" font-size="12">
        <tspan fill="#666">Balance:</tspan>
        <tspan dx="5" fill="#00aa00" font-weight="700" font-size="14">
            ${{model.balance.toFixed(2)}}
        </tspan>
    </text>
</svg>
```

### 11. Temperature with Units

Temperature display with styled units:

```html
<!-- Model: { temperature: 72.5, unit: "°F" } -->
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="80">
    <text x="100" y="50" text-anchor="middle">
        <tspan font-size="42" font-weight="700" fill="#ff6600">
            {{model.temperature.toFixed(1)}}
        </tspan>
        <tspan font-size="24" fill="#999">{{model.unit}}</tspan>
    </text>
</svg>
```

### 12. Score with Context

Score display with description:

```html
<!-- Model: { score: 87, maxScore: 100 } -->
<svg xmlns="http://www.w3.org/2000/svg" width="250" height="60">
    <text x="10" y="40" font-size="16">
        <tspan fill="#666">Score:</tspan>
        <tspan dx="10" font-size="32" font-weight="700" fill="#00aa00">
            {{model.score}}
        </tspan>
        <tspan dy="5" font-size="18" fill="#999">
            /{{model.maxScore}}
        </tspan>
    </text>
</svg>
```

### 13. Conditional Status Color

Status text with conditional coloring:

```html
<!-- Model: { orderStatus: "Shipped", isUrgent: false } -->
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="60">
    <text x="10" y="30" font-size="14">
        <tspan fill="#333">Order Status:</tspan>
        <tspan dx="5"
               fill="{{model.orderStatus === 'Shipped' ? '#00aa00' : '#ff6600'}}"
               font-weight="{{model.isUrgent ? 700 : 600}}">
            {{model.orderStatus}}
        </tspan>
    </text>
</svg>
```

### 14. Percentage Breakdown

Percentage display with components:

```html
<!-- Model: { completed: 45, inProgress: 30, pending: 25 } -->
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="40">
    <text x="10" y="25" font-size="14">
        <tspan fill="#00aa00" font-weight="600">{{model.completed}}%</tspan>
        <tspan dx="5" fill="#999">complete</tspan>
        <tspan dx="15" fill="#ff6600" font-weight="600">{{model.inProgress}}%</tspan>
        <tspan dx="5" fill="#999">in progress</tspan>
        <tspan dx="15" fill="#666" font-weight="600">{{model.pending}}%</tspan>
        <tspan dx="5" fill="#999">pending</tspan>
    </text>
</svg>
```

### 15. Product Name with SKU

Product information with different text styles:

```html
<!-- Model: { productName: "Premium Widget", sku: "WDG-001-BLU" } -->
<svg xmlns="http://www.w3.org/2000/svg" width="350" height="60">
    <text x="10" y="30" font-size="16">
        <tspan font-weight="700" fill="#336699">{{model.productName}}</tspan>
    </text>
    <text x="10" y="50" font-size="11">
        <tspan fill="#999">SKU:</tspan>
        <tspan dx="5" fill="#666" font-family="monospace">{{model.sku}}</tspan>
    </text>
</svg>
```

### 16. Metric with Trend Indicator

Value with trend arrow:

```html
<!-- Model: { revenue: 125000, trend: "up", change: 12.5 } -->
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="60">
    <text x="10" y="40" font-size="16">
        <tspan fill="#333">Revenue:</tspan>
        <tspan dx="10" font-size="24" font-weight="700" fill="#336699">
            ${{(model.revenue / 1000).toFixed(0)}}K
        </tspan>
        <tspan dx="10" font-size="18" fill="{{model.trend === 'up' ? '#00aa00' : '#cc0000'}}">
            {{model.trend === 'up' ? '↑' : '↓'}}
        </tspan>
        <tspan dx="5" font-size="14" fill="{{model.trend === 'up' ? '#00aa00' : '#cc0000'}}">
            {{model.change}}%
        </tspan>
    </text>
</svg>
```

### 17. Date Range Display

Formatted date range with separator:

```html
<!-- Model: { startDate: "2024-01-01", endDate: "2024-12-31" } -->
<svg xmlns="http://www.w3.org/2000/svg" width="350" height="60">
    <text x="10" y="30" font-size="14">
        <tspan fill="#666">Period:</tspan>
        <tspan dx="5" fill="#333" font-weight="600">{{model.startDate}}</tspan>
        <tspan dx="5" fill="#999">to</tspan>
        <tspan dx="5" fill="#333" font-weight="600">{{model.endDate}}</tspan>
    </text>
</svg>
```

### 18. Chart Legend Item

Legend entry with colored indicator text:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="30">
    <text x="10" y="20" font-size="14">
        <tspan fill="#336699" font-weight="700">■</tspan>
        <tspan dx="10" fill="#333">Revenue</tspan>
        <tspan dx="5" fill="#999" font-size="12">(in thousands)</tspan>
    </text>
</svg>
```

### 19. Highlighted Keywords

Text with multiple highlighted terms:

```html
<!-- Model: { keywords: ["important", "urgent", "required"] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="60">
    <text x="10" y="30" font-size="14" fill="#333">
        Please note the following
        <template data-bind="{{model.keywords}}">
            <tspan dx="5" fill="#cc0000" font-weight="700">{{.}}</tspan>
        </template>
        items in this document.
    </text>
</svg>
```

### 20. Complex Label with Multiple Styles

Multi-component label with various styling:

```html
<!-- Model: { department: "Sales", count: 24, status: "active", lastUpdate: "2h ago" } -->
<svg xmlns="http://www.w3.org/2000/svg" width="450" height="60">
    <text x="10" y="35" font-size="14">
        <tspan font-weight="700" font-size="16" fill="#336699">
            {{model.department}}
        </tspan>
        <tspan dx="10" fill="#999">•</tspan>
        <tspan dx="10" fill="#333" font-weight="600">
            {{model.count}}
        </tspan>
        <tspan dx="5" fill="#666" font-size="12">members</tspan>
        <tspan dx="10" fill="#999">•</tspan>
        <tspan dx="10" fill="#00aa00" font-size="12">
            ● {{model.status}}
        </tspan>
        <tspan dx="10" fill="#999">•</tspan>
        <tspan dx="10" fill="#999" font-size="11" font-style="italic">
            Updated {{model.lastUpdate}}
        </tspan>
    </text>
</svg>
```

---

## See Also

- [text element](/reference/svgtags/svg_text_element.html) - Parent SVG text element
- [svg element](/reference/svgtags/svg.html) - SVG container element
- [fill attribute](/reference/svgattributes/attr_fill.html) - Fill colors and patterns
- [Data Binding](/reference/binding/) - Complete data binding guide
- [SVG Styling](/reference/svg/styling/) - SVG style reference
- [Font Reference](/reference/fonts/) - Font configuration and usage

---
