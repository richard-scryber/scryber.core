---
layout: default
title: dominant-baseline
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @dominant-baseline : The Text Vertical Alignment Attribute

The `dominant-baseline` attribute controls the vertical alignment of text relative to its y position. It determines which part of the text (baseline, middle, top, etc.) aligns with the specified y coordinate.

## Usage

The `dominant-baseline` attribute is used to:
- Align text vertically (top, middle, bottom)
- Center text in buttons and badges
- Position text precisely in layouts
- Create aligned multi-font text
- Support data-driven vertical alignment
- Build responsive text positioning

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="400">
    <!-- Reference line at y=200 -->
    <line x1="50" y1="200" x2="350" y2="200" stroke="#bdc3c7" stroke-width="1"/>

    <!-- Auto baseline (default) -->
    <text x="100" y="200" dominant-baseline="auto" fill="#3498db">
        Auto Baseline
    </text>

    <!-- Middle alignment -->
    <text x="100" y="200" dominant-baseline="middle" fill="#e74c3c">
        Middle
    </text>

    <!-- Hanging baseline -->
    <text x="100" y="200" dominant-baseline="hanging" fill="#2ecc71">
        Hanging
    </text>
</svg>
```

---

## Supported Values

| Value | Description | Alignment | Use Case |
|-------|-------------|-----------|----------|
| `auto` | Browser default (usually alphabetic) | Varies | Default text |
| `text-bottom` | Bottom of text | Aligns text bottom with y | Bottom alignment |
| `alphabetic` | Alphabetic baseline | Standard baseline | Normal text |
| `ideographic` | Ideographic baseline | Below alphabetic | CJK characters |
| `middle` | Vertical center of text | Centers text on y | Centered labels |
| `central` | Approximate middle | Between middle and alphabetic | Alternative center |
| `mathematical` | Mathematical baseline | Special math alignment | Math formulas |
| `hanging` | Hanging baseline | Top of text | Top-aligned text |
| `text-top` | Top of text | Aligns text top with y | Top alignment |

### Common Values

The most commonly used values are:
- **auto** or **alphabetic** - Standard text (sits on y coordinate)
- **middle** - Centered vertically (most common for labels)
- **hanging** - Top-aligned (text hangs from y coordinate)
- **text-top** - Explicit top alignment
- **text-bottom** - Explicit bottom alignment

---

## Supported Elements

The `dominant-baseline` attribute is supported on:

- **[&lt;text&gt;](/reference/svgtags/text.html)** - Text element
- **[&lt;tspan&gt;](/reference/svgtags/tspan.html)** - Text span element
- **[&lt;textPath&gt;](/reference/svgtags/textPath.html)** - Text on path element

---

## Data Binding

### Dynamic Vertical Alignment

Change vertical alignment based on data:

```html
<!-- Model: { labels: [{text: 'Top', align: 'hanging', x: 100}, {text: 'Middle', align: 'middle', x: 200}, {text: 'Bottom', align: 'text-bottom', x: 300}] } -->
<svg width="400" height="200">
    <line x1="50" y1="100" x2="350" y2="100" stroke="#bdc3c7" stroke-width="1"/>

    <template data-bind="{{model.labels}}">
        <circle cx="{{.x}}" cy="100" r="3" fill="#e74c3c"/>
        <text x="{{.x}}" y="100" dominant-baseline="{{.align}}"
              text-anchor="middle" font-size="14" fill="#3498db">{{.text}}</text>
    </template>
</svg>
```

### Centered Button Text

Center text perfectly in buttons:

```html
<!-- Model: { buttons: [{label: 'Save', x: 80, y: 75, color: '#3498db'}, {label: 'Cancel', x: 220, y: 75, color: '#e74c3c'}] } -->
<svg width="400" height="150">
    <template data-bind="{{model.buttons}}">
        <rect x="{{.x - 60}}" y="{{.y - 25}}" width="120" height="50" rx="8" fill="{{.color}}"/>
        <text x="{{.x}}" y="{{.y}}"
              text-anchor="middle" dominant-baseline="middle"
              fill="white" font-size="16" font-weight="bold">{{.label}}</text>
    </template>
</svg>
```

### Badge Labels with Centered Text

Create badges with perfectly centered numbers:

```html
<!-- Model: { badges: [{count: 5, x: 80, y: 80}, {count: 23, x: 200, y: 80}, {count: 147, x: 320, y: 80}] } -->
<svg width="400" height="160">
    <template data-bind="{{model.badges}}">
        <circle cx="{{.x}}" cy="{{.y}}" r="40" fill="#e74c3c"/>
        <text x="{{.x}}" y="{{.y}}"
              text-anchor="middle" dominant-baseline="middle"
              fill="white" font-size="24" font-weight="bold">{{.count}}</text>
    </template>
</svg>
```

### Multi-Line Aligned Text

Align multi-line text blocks:

```html
<!-- Model: { items: [{title: 'Item 1', desc: 'Description', y: 60}, {title: 'Item 2', desc: 'Description', y: 140}] } -->
<svg width="400" height="200">
    <template data-bind="{{model.items}}">
        <text x="50" y="{{.y}}" dominant-baseline="hanging" font-size="16" font-weight="bold" fill="#2c3e50">
            {{.title}}
        </text>
        <text x="50" y="{{.y + 25}}" dominant-baseline="hanging" font-size="12" fill="#7f8c8d">
            {{.desc}}
        </text>
    </template>
</svg>
```

### Position-Specific Alignment

Use different alignment based on position:

```html
<!-- Model: { markers: [{label: 'Top', x: 100, y: 50, position: 'top'}, {label: 'Center', x: 200, y: 150, position: 'center'}, {label: 'Bottom', x: 300, y: 250, position: 'bottom'}] } -->
<svg width="400" height="300">
    <template data-bind="{{model.markers}}">
        <circle cx="{{.x}}" cy="{{.y}}" r="5" fill="#e74c3c"/>
        <text x="{{.x}}" y="{{.y}}"
              text-anchor="middle"
              dominant-baseline="{{.position === 'top' ? 'text-bottom' : (.position === 'center' ? 'middle' : 'hanging')}}"
              font-size="12" fill="#2c3e50">{{.label}}</text>
    </template>
</svg>
```

---

## Notes

### Baseline Types

**alphabetic (default):**
- Standard text baseline for Latin scripts
- Bottom edge of most letters (excluding descenders)
- Most common for regular text
- Same as `auto` in most browsers

**middle:**
- Vertical center of text
- Most useful for centered labels
- Perfect for buttons, badges, icons
- Independent of font metrics

**hanging:**
- Top of text (hangs from y coordinate)
- Used in Devanagari and similar scripts
- Good for top-aligned text
- Opposite of alphabetic

**text-top:**
- Absolute top of text box
- Includes ascenders (like 'h', 'l')
- More precise than hanging
- Good for tight layouts

**text-bottom:**
- Absolute bottom of text box
- Includes descenders (like 'g', 'y', 'p')
- Opposite of text-top
- Good for bottom alignment

**central:**
- Alternative center alignment
- Between middle and alphabetic
- Less common than middle
- Font-metrics based

**mathematical:**
- For mathematical formulas
- Aligns with math symbols
- Specialized use case
- Font-specific

**ideographic:**
- For CJK (Chinese, Japanese, Korean) text
- Below alphabetic baseline
- Script-specific
- Used with Asian fonts

### Visual Comparison

```html
<!-- All text at y=100 -->
<text y="100" dominant-baseline="hanging">Hanging</text>      <!-- Top at 100 -->
<text y="100" dominant-baseline="text-top">Text Top</text>    <!-- Top at 100 -->
<text y="100" dominant-baseline="middle">Middle</text>         <!-- Center at 100 -->
<text y="100" dominant-baseline="central">Central</text>       <!-- Near center at 100 -->
<text y="100" dominant-baseline="alphabetic">Alphabetic</text> <!-- Baseline at 100 -->
<text y="100" dominant-baseline="text-bottom">Bottom</text>    <!-- Bottom at 100 -->
```

### Combining with text-anchor

Full text positioning control:

```html
<!-- Centered both horizontally and vertically -->
<text x="200" y="100"
      text-anchor="middle"           <!-- Horizontal center -->
      dominant-baseline="middle">    <!-- Vertical center -->
    Perfectly Centered
</text>
```

### Multi-line Text

Each line can have different baseline:

```html
<text x="100" y="100">
    <tspan x="100" dy="0" dominant-baseline="hanging">First Line</tspan>
    <tspan x="100" dy="20" dominant-baseline="middle">Second Line</tspan>
    <tspan x="100" dy="20" dominant-baseline="alphabetic">Third Line</tspan>
</text>
```

### Browser Compatibility

**Well-supported values:**
- auto, alphabetic, middle, hanging
- text-top, text-bottom

**Less common values:**
- central, mathematical, ideographic
- May have inconsistent behavior across browsers

**Fallback strategy:**
```html
<!-- Use middle for best compatibility -->
<text dominant-baseline="middle">Centered</text>
```

### Common Use Cases

**Buttons:**
```html
<text x="{{buttonCenterX}}" y="{{buttonCenterY}}"
      text-anchor="middle" dominant-baseline="middle">
    Click Me
</text>
```

**Badges:**
```html
<text x="{{badgeX}}" y="{{badgeY}}"
      text-anchor="middle" dominant-baseline="middle">
    {{count}}
</text>
```

**Chart labels:**
```html
<text x="{{pointX}}" y="{{pointY - 10}}"
      text-anchor="middle" dominant-baseline="text-bottom">
    {{value}}
</text>
```

**Top-aligned headings:**
```html
<text x="50" y="50" dominant-baseline="hanging">
    Heading Text
</text>
```

### Coordinate Reference

The y coordinate refers to different positions based on baseline:

- **hanging** / **text-top**: y = top of text
- **middle**: y = vertical center of text
- **alphabetic**: y = text baseline
- **text-bottom**: y = bottom of text (including descenders)

### Font Metrics

- Baseline positioning is font-dependent
- Different fonts may have different metrics
- `middle` is most consistent across fonts
- Test with actual fonts used

### Performance Considerations

- All baseline values have similar performance
- No significant rendering overhead
- Can be applied to many text elements
- Doesn't affect text rendering quality

---

## Examples

### Baseline Comparison

```html
<svg width="400" height="500">
    <!-- Reference lines -->
    <line x1="50" y1="100" x2="350" y2="100" stroke="#e74c3c" stroke-width="1"/>
    <line x1="50" y1="180" x2="350" y2="180" stroke="#e74c3c" stroke-width="1"/>
    <line x1="50" y1="260" x2="350" y2="260" stroke="#e74c3c" stroke-width="1"/>
    <line x1="50" y1="340" x2="350" y2="340" stroke="#e74c3c" stroke-width="1"/>
    <line x1="50" y1="420" x2="350" y2="420" stroke="#e74c3c" stroke-width="1"/>

    <!-- Hanging -->
    <circle cx="200" cy="100" r="3" fill="#e74c3c"/>
    <text x="200" y="100" text-anchor="middle" dominant-baseline="hanging"
          font-size="24" fill="#3498db">Hanging Baseline</text>
    <text x="350" y="100" text-anchor="end" font-size="10" fill="#7f8c8d">hanging</text>

    <!-- Text-top -->
    <circle cx="200" cy="180" r="3" fill="#e74c3c"/>
    <text x="200" y="180" text-anchor="middle" dominant-baseline="text-top"
          font-size="24" fill="#3498db">Text Top</text>
    <text x="350" y="180" text-anchor="end" font-size="10" fill="#7f8c8d">text-top</text>

    <!-- Middle -->
    <circle cx="200" cy="260" r="3" fill="#e74c3c"/>
    <text x="200" y="260" text-anchor="middle" dominant-baseline="middle"
          font-size="24" fill="#3498db">Middle</text>
    <text x="350" y="260" text-anchor="end" font-size="10" fill="#7f8c8d">middle</text>

    <!-- Alphabetic -->
    <circle cx="200" cy="340" r="3" fill="#e74c3c"/>
    <text x="200" y="340" text-anchor="middle" dominant-baseline="alphabetic"
          font-size="24" fill="#3498db">Alphabetic gypq</text>
    <text x="350" y="340" text-anchor="end" font-size="10" fill="#7f8c8d">alphabetic</text>

    <!-- Text-bottom -->
    <circle cx="200" cy="420" r="3" fill="#e74c3c"/>
    <text x="200" y="420" text-anchor="middle" dominant-baseline="text-bottom"
          font-size="24" fill="#3498db">Text Bottom gypq</text>
    <text x="350" y="420" text-anchor="end" font-size="10" fill="#7f8c8d">text-bottom</text>
</svg>
```

### Centered Button Text

```html
<svg width="500" height="200">
    <defs>
        <linearGradient id="btn1" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db"/>
            <stop offset="100%" stop-color="#2980b9"/>
        </linearGradient>
        <linearGradient id="btn2" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#2ecc71"/>
            <stop offset="100%" stop-color="#27ae60"/>
        </linearGradient>
        <linearGradient id="btn3" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#e74c3c"/>
            <stop offset="100%" stop-color="#c0392b"/>
        </linearGradient>
    </defs>

    <!-- Button 1 -->
    <rect x="30" y="70" width="130" height="60" rx="10" fill="url(#btn1)"/>
    <text x="95" y="100" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="18" font-weight="bold">Submit</text>

    <!-- Button 2 -->
    <rect x="185" y="70" width="130" height="60" rx="10" fill="url(#btn2)"/>
    <text x="250" y="100" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="18" font-weight="bold">Save</text>

    <!-- Button 3 -->
    <rect x="340" y="70" width="130" height="60" rx="10" fill="url(#btn3)"/>
    <text x="405" y="100" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="18" font-weight="bold">Cancel</text>
</svg>
```

### Badge Notifications

```html
<svg width="500" height="200">
    <!-- Badge 1 -->
    <circle cx="100" cy="100" r="50" fill="#e74c3c"/>
    <text x="100" y="100" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="32" font-weight="bold">5</text>
    <text x="100" y="165" text-anchor="middle" font-size="12" fill="#2c3e50">Unread</text>

    <!-- Badge 2 -->
    <circle cx="250" cy="100" r="50" fill="#f39c12"/>
    <text x="250" y="100" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="32" font-weight="bold">12</text>
    <text x="250" y="165" text-anchor="middle" font-size="12" fill="#2c3e50">Pending</text>

    <!-- Badge 3 -->
    <circle cx="400" cy="100" r="50" fill="#2ecc71"/>
    <text x="400" y="100" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="32" font-weight="bold">99+</text>
    <text x="400" y="165" text-anchor="middle" font-size="12" fill="#2c3e50">Messages</text>
</svg>
```

### Chart Data Labels

```html
<svg width="500" height="300">
    <defs>
        <linearGradient id="barGrad" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db"/>
            <stop offset="100%" stop-color="#2980b9"/>
        </linearGradient>
    </defs>

    <!-- Bars with labels -->
    <rect x="60" y="150" width="60" height="100" fill="url(#barGrad)"/>
    <text x="90" y="140" text-anchor="middle" dominant-baseline="text-bottom"
          font-size="16" font-weight="bold" fill="#2c3e50">45</text>

    <rect x="160" y="100" width="60" height="150" fill="url(#barGrad)"/>
    <text x="190" y="90" text-anchor="middle" dominant-baseline="text-bottom"
          font-size="16" font-weight="bold" fill="#2c3e50">78</text>

    <rect x="260" y="120" width="60" height="130" fill="url(#barGrad)"/>
    <text x="290" y="110" text-anchor="middle" dominant-baseline="text-bottom"
          font-size="16" font-weight="bold" fill="#2c3e50">62</text>

    <rect x="360" y="80" width="60" height="170" fill="url(#barGrad)"/>
    <text x="390" y="70" text-anchor="middle" dominant-baseline="text-bottom"
          font-size="16" font-weight="bold" fill="#2c3e50">89</text>

    <!-- Baseline -->
    <line x1="40" y1="250" x2="440" y2="250" stroke="#2c3e50" stroke-width="2"/>

    <!-- Category labels (hanging from baseline) -->
    <text x="90" y="265" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#7f8c8d">Q1</text>
    <text x="190" y="265" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#7f8c8d">Q2</text>
    <text x="290" y="265" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#7f8c8d">Q3</text>
    <text x="390" y="265" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#7f8c8d">Q4</text>
</svg>
```

### Icon Labels

```html
<svg width="500" height="200">
    <!-- Icon 1 with centered label -->
    <circle cx="100" cy="80" r="35" fill="#3498db"/>
    <text x="100" y="80" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="32">📁</text>
    <text x="100" y="140" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#2c3e50">Files</text>

    <!-- Icon 2 with centered label -->
    <circle cx="250" cy="80" r="35" fill="#2ecc71"/>
    <text x="250" y="80" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="32">📧</text>
    <text x="250" y="140" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#2c3e50">Mail</text>

    <!-- Icon 3 with centered label -->
    <circle cx="400" cy="80" r="35" fill="#e74c3c"/>
    <text x="400" y="80" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="32">⚙️</text>
    <text x="400" y="140" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#2c3e50">Settings</text>
</svg>
```

### Top-Aligned Headings

```html
<svg width="400" height="300">
    <!-- Section dividers -->
    <line x1="50" y1="50" x2="350" y2="50" stroke="#3498db" stroke-width="2"/>
    <line x1="50" y1="150" x2="350" y2="150" stroke="#2ecc71" stroke-width="2"/>
    <line x1="50" y1="250" x2="350" y2="250" stroke="#e74c3c" stroke-width="2"/>

    <!-- Headings hanging from lines -->
    <text x="50" y="50" dominant-baseline="hanging" font-size="20" font-weight="bold" fill="#3498db">
        Introduction
    </text>
    <text x="50" y="75" dominant-baseline="hanging" font-size="12" fill="#7f8c8d">
        This is the introduction section with some text content.
    </text>

    <text x="50" y="150" dominant-baseline="hanging" font-size="20" font-weight="bold" fill="#2ecc71">
        Main Content
    </text>
    <text x="50" y="175" dominant-baseline="hanging" font-size="12" fill="#7f8c8d">
        This is the main content section with details.
    </text>

    <text x="50" y="250" dominant-baseline="hanging" font-size="20" font-weight="bold" fill="#e74c3c">
        Conclusion
    </text>
    <text x="50" y="275" dominant-baseline="hanging" font-size="12" fill="#7f8c8d">
        This is the conclusion section wrapping up.
    </text>
</svg>
```

### Gauge Display

```html
<svg width="400" height="300" viewBox="0 0 400 300">
    <!-- Gauge arc -->
    <path d="M 50,200 A 150,150 0 0,1 350,200"
          fill="none" stroke="#ecf0f1" stroke-width="30" stroke-linecap="round"/>
    <path d="M 50,200 A 150,150 0 0,1 275,90"
          fill="none" stroke="#3498db" stroke-width="30" stroke-linecap="round"/>

    <!-- Centered value -->
    <text x="200" y="200" text-anchor="middle" dominant-baseline="middle"
          font-size="72" font-weight="bold" fill="#2c3e50">75</text>

    <!-- Centered label below value -->
    <text x="200" y="240" text-anchor="middle" dominant-baseline="hanging"
          font-size="18" fill="#7f8c8d">Completion</text>

    <!-- Min/Max labels -->
    <text x="50" y="220" text-anchor="start" dominant-baseline="hanging"
          font-size="14" fill="#95a5a6">0</text>
    <text x="350" y="220" text-anchor="end" dominant-baseline="hanging"
          font-size="14" fill="#95a5a6">100</text>
</svg>
```

### Card Layout

```html
<svg width="300" height="400">
    <!-- Card -->
    <rect x="20" y="20" width="260" height="360" rx="10"
          fill="white" stroke="#bdc3c7" stroke-width="2"/>

    <!-- Image placeholder -->
    <rect x="30" y="30" width="240" height="160" rx="5" fill="#ecf0f1"/>

    <!-- Title (hanging from top of section) -->
    <text x="150" y="210" text-anchor="middle" dominant-baseline="hanging"
          font-size="20" font-weight="bold" fill="#2c3e50">Product Title</text>

    <!-- Price (centered) -->
    <text x="150" y="280" text-anchor="middle" dominant-baseline="middle"
          font-size="36" font-weight="bold" fill="#e74c3c">$99.99</text>

    <!-- Description (top aligned) -->
    <text x="150" y="320" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#7f8c8d">Premium quality</text>
    <text x="150" y="340" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#7f8c8d">Limited edition</text>
</svg>
```

### Timeline Events

```html
<svg width="600" height="300">
    <!-- Timeline -->
    <line x1="100" y1="150" x2="500" y2="150" stroke="#3498db" stroke-width="4"/>

    <!-- Event 1 (above timeline) -->
    <circle cx="150" cy="150" r="8" fill="#e74c3c"/>
    <line x1="150" y1="150" x2="150" y2="80" stroke="#95a5a6" stroke-width="2"/>
    <text x="150" y="80" text-anchor="middle" dominant-baseline="text-bottom"
          font-size="14" font-weight="bold" fill="#2c3e50">2020</text>
    <text x="150" y="60" text-anchor="middle" dominant-baseline="text-bottom"
          font-size="11" fill="#7f8c8d">Launch</text>

    <!-- Event 2 (below timeline) -->
    <circle cx="250" cy="150" r="8" fill="#2ecc71"/>
    <line x1="250" y1="150" x2="250" y2="220" stroke="#95a5a6" stroke-width="2"/>
    <text x="250" y="220" text-anchor="middle" dominant-baseline="hanging"
          font-size="14" font-weight="bold" fill="#2c3e50">2021</text>
    <text x="250" y="240" text-anchor="middle" dominant-baseline="hanging"
          font-size="11" fill="#7f8c8d">Growth</text>

    <!-- Event 3 (above timeline) -->
    <circle cx="350" cy="150" r="8" fill="#f39c12"/>
    <line x1="350" y1="150" x2="350" y2="80" stroke="#95a5a6" stroke-width="2"/>
    <text x="350" y="80" text-anchor="middle" dominant-baseline="text-bottom"
          font-size="14" font-weight="bold" fill="#2c3e50">2022</text>
    <text x="350" y="60" text-anchor="middle" dominant-baseline="text-bottom"
          font-size="11" fill="#7f8c8d">Expansion</text>

    <!-- Event 4 (below timeline) -->
    <circle cx="450" cy="150" r="8" fill="#9b59b6"/>
    <line x1="450" y1="150" x2="450" y2="220" stroke="#95a5a6" stroke-width="2"/>
    <text x="450" y="220" text-anchor="middle" dominant-baseline="hanging"
          font-size="14" font-weight="bold" fill="#2c3e50">2023</text>
    <text x="450" y="240" text-anchor="middle" dominant-baseline="hanging"
          font-size="11" fill="#7f8c8d">Innovation</text>
</svg>
```

### Progress Steps

```html
<svg width="600" height="200">
    <!-- Progress line -->
    <line x1="100" y1="100" x2="500" y2="100" stroke="#ecf0f1" stroke-width="6"/>
    <line x1="100" y1="100" x2="300" y2="100" stroke="#3498db" stroke-width="6"/>

    <!-- Step 1 (completed) -->
    <circle cx="100" cy="100" r="20" fill="#2ecc71"/>
    <text x="100" y="100" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="20" font-weight="bold">✓</text>
    <text x="100" y="140" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#2c3e50">Register</text>

    <!-- Step 2 (completed) -->
    <circle cx="300" cy="100" r="20" fill="#2ecc71"/>
    <text x="300" y="100" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="20" font-weight="bold">✓</text>
    <text x="300" y="140" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#2c3e50">Verify</text>

    <!-- Step 3 (current) -->
    <circle cx="400" cy="100" r="20" fill="#3498db"/>
    <text x="400" y="100" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="20" font-weight="bold">3</text>
    <text x="400" y="140" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#3498db">Profile</text>

    <!-- Step 4 (pending) -->
    <circle cx="500" cy="100" r="20" fill="#bdc3c7"/>
    <text x="500" y="100" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="20" font-weight="bold">4</text>
    <text x="500" y="140" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" fill="#95a5a6">Complete</text>
</svg>
```

### Score Display

```html
<svg width="400" height="300">
    <!-- Score box -->
    <rect x="100" y="80" width="200" height="140" rx="10"
          fill="#3498db" stroke="#2980b9" stroke-width="3"/>

    <!-- Score value (centered) -->
    <text x="200" y="130" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="64" font-weight="bold">95</text>

    <!-- Label (below score) -->
    <text x="200" y="180" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="16">out of 100</text>

    <!-- Title (above box) -->
    <text x="200" y="50" text-anchor="middle" dominant-baseline="text-bottom"
          font-size="18" font-weight="bold" fill="#2c3e50">Your Score</text>

    <!-- Description (below box) -->
    <text x="200" y="240" text-anchor="middle" dominant-baseline="hanging"
          font-size="14" fill="#7f8c8d">Excellent Performance!</text>
</svg>
```

### Data Table Grid

```html
<svg width="600" height="300">
    <!-- Grid lines -->
    <line x1="50" y1="60" x2="550" y2="60" stroke="#bdc3c7" stroke-width="1"/>
    <line x1="50" y1="100" x2="550" y2="100" stroke="#ecf0f1" stroke-width="1"/>
    <line x1="50" y1="140" x2="550" y2="140" stroke="#ecf0f1" stroke-width="1"/>
    <line x1="50" y1="180" x2="550" y2="180" stroke="#ecf0f1" stroke-width="1"/>
    <line x1="50" y1="220" x2="550" y2="220" stroke="#ecf0f1" stroke-width="1"/>

    <!-- Headers (middle aligned) -->
    <text x="100" y="40" text-anchor="middle" dominant-baseline="middle"
          font-weight="bold" font-size="12" fill="#2c3e50">ID</text>
    <text x="250" y="40" text-anchor="middle" dominant-baseline="middle"
          font-weight="bold" font-size="12" fill="#2c3e50">Name</text>
    <text x="450" y="40" text-anchor="middle" dominant-baseline="middle"
          font-weight="bold" font-size="12" fill="#2c3e50">Status</text>

    <!-- Data rows (middle aligned) -->
    <text x="100" y="80" text-anchor="middle" dominant-baseline="middle"
          font-size="11" fill="#34495e">001</text>
    <text x="250" y="80" text-anchor="middle" dominant-baseline="middle"
          font-size="11" fill="#34495e">Item Alpha</text>
    <text x="450" y="80" text-anchor="middle" dominant-baseline="middle"
          font-size="11" fill="#2ecc71">Active</text>

    <text x="100" y="120" text-anchor="middle" dominant-baseline="middle"
          font-size="11" fill="#34495e">002</text>
    <text x="250" y="120" text-anchor="middle" dominant-baseline="middle"
          font-size="11" fill="#34495e">Item Beta</text>
    <text x="450" y="120" text-anchor="middle" dominant-baseline="middle"
          font-size="11" fill="#e74c3c">Inactive</text>

    <text x="100" y="160" text-anchor="middle" dominant-baseline="middle"
          font-size="11" fill="#34495e">003</text>
    <text x="250" y="160" text-anchor="middle" dominant-baseline="middle"
          font-size="11" fill="#34495e">Item Gamma</text>
    <text x="450" y="160" text-anchor="middle" dominant-baseline="middle"
          font-size="11" fill="#2ecc71">Active</text>

    <text x="100" y="200" text-anchor="middle" dominant-baseline="middle"
          font-size="11" fill="#34495e">004</text>
    <text x="250" y="200" text-anchor="middle" dominant-baseline="middle"
          font-size="11" fill="#34495e">Item Delta</text>
    <text x="450" y="200" text-anchor="middle" dominant-baseline="middle"
          font-size="11" fill="#f39c12">Pending</text>
</svg>
```

### Thermometer Display

```html
<svg width="200" height="400">
    <!-- Thermometer body -->
    <rect x="80" y="50" width="40" height="280" rx="20" fill="#ecf0f1" stroke="#bdc3c7" stroke-width="2"/>
    <circle cx="100" cy="350" r="30" fill="#e74c3c" stroke="#c0392b" stroke-width="2"/>

    <!-- Mercury -->
    <rect x="85" y="150" width="30" height="180" fill="#e74c3c"/>
    <rect x="85" y="148" width="30" height="4" rx="2" fill="#e74c3c"/>

    <!-- Temperature marks and labels (right-aligned) -->
    <line x1="120" y1="80" x2="135" y2="80" stroke="#7f8c8d" stroke-width="1"/>
    <text x="145" y="80" text-anchor="start" dominant-baseline="middle"
          font-size="12" fill="#2c3e50">100°</text>

    <line x1="120" y1="150" x2="135" y2="150" stroke="#7f8c8d" stroke-width="1"/>
    <text x="145" y="150" text-anchor="start" dominant-baseline="middle"
          font-size="12" fill="#2c3e50">75°</text>

    <line x1="120" y1="220" x2="135" y2="220" stroke="#7f8c8d" stroke-width="1"/>
    <text x="145" y="220" text-anchor="start" dominant-baseline="middle"
          font-size="12" fill="#2c3e50">50°</text>

    <line x1="120" y1="290" x2="135" y2="290" stroke="#7f8c8d" stroke-width="1"/>
    <text x="145" y="290" text-anchor="start" dominant-baseline="middle"
          font-size="12" fill="#2c3e50">25°</text>

    <!-- Current temperature -->
    <text x="100" y="380" text-anchor="middle" dominant-baseline="middle"
          font-size="16" font-weight="bold" fill="white">75°F</text>
</svg>
```

### Tooltip Display

```html
<svg width="400" height="300">
    <!-- Chart point -->
    <circle cx="200" cy="150" r="8" fill="#3498db"/>

    <!-- Tooltip box -->
    <rect x="220" y="100" width="140" height="70" rx="5"
          fill="white" stroke="#bdc3c7" stroke-width="2"/>
    <path d="M 220,130 L 210,135 L 220,140" fill="white" stroke="#bdc3c7" stroke-width="2"/>

    <!-- Tooltip content (top-aligned) -->
    <text x="290" y="115" text-anchor="middle" dominant-baseline="hanging"
          font-size="12" font-weight="bold" fill="#2c3e50">Value</text>
    <text x="290" y="137" text-anchor="middle" dominant-baseline="middle"
          font-size="24" font-weight="bold" fill="#3498db">78</text>
    <text x="290" y="160" text-anchor="middle" dominant-baseline="text-bottom"
          font-size="10" fill="#7f8c8d">June 2023</text>
</svg>
```

---

## See Also

- [text](/reference/svgtags/text.html) - Text element
- [tspan](/reference/svgtags/tspan.html) - Text span element
- [text-anchor](/reference/svgattributes/text-anchor.html) - Horizontal text alignment
- [x](/reference/svgattributes/x.html), [y](/reference/svgattributes/y.html) - Text position coordinates
- [font-size](/reference/svgattributes/font-size.html) - Text size
- [fill](/reference/svgattributes/fill.html) - Text color
- [Data Binding](/reference/binding/) - Data binding and expressions

---
