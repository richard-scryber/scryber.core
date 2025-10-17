---
layout: default
title: @media Rule
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @media At-Rule

The `@media` at-rule allows you to define conditional styles that apply based on media features such as screen type, width, height, or print settings. This is essential for creating responsive PDF documents or different styles for screen vs. print.

## Usage

```css
@media media-type and (media-feature) {
    selector {
        property: value;
    }
}
```

Applies styles conditionally based on media queries.

---

## Syntax Examples

```css
/* Print-specific styles */
@media print {
    body {
        font-size: 10pt;
    }
}

/* Screen-specific styles */
@media screen {
    body {
        font-size: 12pt;
    }
}

/* Width-based responsive design */
@media (max-width: 600pt) {
    .container {
        width: 100%;
    }
}

/* Orientation-based */
@media (orientation: landscape) {
    body {
        margin: 20pt 40pt;
    }
}
```

---

## Common Media Types

- `print` - For printed documents (PDF generation)
- `screen` - For screen display
- `all` - Applies to all media types (default)

---

## Common Media Features

- `width` - Width of the viewport
- `height` - Height of the viewport
- `min-width` - Minimum width
- `max-width` - Maximum width
- `min-height` - Minimum height
- `max-height` - Maximum height
- `orientation` - Portrait or landscape orientation

---

## Notes

- Media queries allow conditional styling for different output contexts
- Multiple media features can be combined with `and`, `or`, or `not`
- Particularly useful in Scryber for differentiating screen preview from final PDF output
- Print media type is commonly used for PDF-specific styling
- Nested selectors within @media inherit the media condition

---

## Examples

### Example 1: Print vs. Screen styling

```html
<style>
    body {
        font-family: Arial, sans-serif;
    }

    @media screen {
        body {
            background-color: #f0f0f0;
            padding: 20pt;
        }
    }

    @media print {
        body {
            background-color: white;
            padding: 0;
        }
    }
</style>
<body>
    <h1>Document Title</h1>
    <p>Content appears differently on screen vs. print.</p>
</body>
```

### Example 2: Hide elements in print

```html
<style>
    .no-print {
        display: block;
    }

    @media print {
        .no-print {
            display: none;
        }
    }
</style>
<body>
    <div class="no-print">
        <p>This won't appear in the PDF output.</p>
    </div>
    <p>This will appear in the PDF.</p>
</body>
```

### Example 3: Font size adjustments

```html
<style>
    body {
        font-size: 12pt;
    }

    @media print {
        body {
            font-size: 10pt;
        }

        h1 {
            font-size: 18pt;
        }

        h2 {
            font-size: 14pt;
        }
    }
</style>
<body>
    <h1>Main Heading</h1>
    <h2>Subheading</h2>
    <p>Body text adjusts for print output.</p>
</body>
```

### Example 4: Page width responsive design

```html
<style>
    .container {
        width: 800pt;
        margin: 0 auto;
    }

    @media (max-width: 600pt) {
        .container {
            width: 100%;
            padding: 10pt;
        }
    }
</style>
<body>
    <div class="container">
        <p>Container width adapts to page size.</p>
    </div>
</body>
```

### Example 5: Landscape vs. Portrait

```html
<style>
    @media (orientation: portrait) {
        body {
            margin: 30pt 20pt;
        }

        .content {
            width: 100%;
        }
    }

    @media (orientation: landscape) {
        body {
            margin: 20pt 40pt;
        }

        .content {
            width: 80%;
        }
    }
</style>
<body>
    <div class="content">
        <p>Layout adjusts based on page orientation.</p>
    </div>
</body>
```

### Example 6: Color adjustments for print

```html
<style>
    .highlight {
        background-color: yellow;
        color: black;
    }

    @media print {
        .highlight {
            background-color: white;
            color: black;
            font-weight: bold;
        }
    }
</style>
<body>
    <p class="highlight">Highlighted text optimized for print.</p>
</body>
```

### Example 7: Link styling for print

```html
<style>
    a {
        color: #0066cc;
        text-decoration: underline;
    }

    @media print {
        a {
            color: black;
            text-decoration: none;
        }

        a:after {
            content: " (" attr(href) ")";
            font-size: 9pt;
            color: #666;
        }
    }
</style>
<body>
    <p>Visit <a href="https://example.com">our website</a> for more info.</p>
</body>
```

### Example 8: Multi-column layout

```html
<style>
    .article {
        column-count: 1;
    }

    @media (min-width: 800pt) {
        .article {
            column-count: 2;
            column-gap: 20pt;
        }
    }
</style>
<body>
    <div class="article">
        <p>Text content flows in columns on wider pages.</p>
    </div>
</body>
```

### Example 9: Table styling for print

```html
<style>
    table {
        width: 100%;
        border-collapse: collapse;
    }

    @media print {
        table {
            font-size: 9pt;
        }

        th, td {
            padding: 4pt;
            border: 1pt solid black;
        }
    }
</style>
<body>
    <table>
        <tr>
            <th>Column 1</th>
            <th>Column 2</th>
        </tr>
        <tr>
            <td>Data 1</td>
            <td>Data 2</td>
        </tr>
    </table>
</body>
```

### Example 10: Combined media features

```html
<style>
    .sidebar {
        width: 200pt;
        float: left;
    }

    @media print and (max-width: 600pt) {
        .sidebar {
            width: 100%;
            float: none;
        }
    }

    @media screen and (min-width: 1000pt) {
        .sidebar {
            width: 250pt;
        }
    }
</style>
<body>
    <div class="sidebar">
        <p>Sidebar adapts to media type and width.</p>
    </div>
</body>
```

---

## See Also

- [@page Rule](/reference/cssselectors/css_page_rule)
- [Element Selector](/reference/cssselectors/css_element_selector)
- [Class Selector](/reference/cssselectors/css_class_selector)
- [Responsive Design in Scryber](/guides/responsive_design)

---
