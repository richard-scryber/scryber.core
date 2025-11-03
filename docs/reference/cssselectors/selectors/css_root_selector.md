---
layout: default
title: :root Pseudo-class
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# :root Pseudo-class Selector

The `:root` pseudo-class selector matches the root element of the document. This is typically the `<html>` element or the top-level document container in Scryber.

## Usage

```css
:root {
    property: value;
}
```

Applies styles to the root element of the document.

---

## Syntax Examples

```css
/* Define document-wide variables or styles */
:root {
    font-family: Arial, sans-serif;
    font-size: 12pt;
    color: #333;
}

/* Set global margins */
:root {
    margin: 0;
    padding: 0;
}
```

---

## Specificity

The `:root` selector has a specificity equivalent to a pseudo-class applied to an element.

---

## Notes

- Matches the root element of the document tree
- Useful for setting document-wide defaults
- Has higher specificity than a simple element selector
- Commonly used to define CSS custom properties (variables) in modern CSS
- In Scryber, this targets the top-level document element
- Styles cascade down to all descendant elements unless overridden

---

## Examples

### Example 1: Document-wide font settings

```html
<style>
    :root {
        font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
        font-size: 12pt;
        line-height: 1.6;
    }
</style>
<body>
    <h1>Heading</h1>
    <p>All text inherits the root font settings.</p>
</body>
```

### Example 2: Default colors

```html
<style>
    :root {
        color: #333333;
        background-color: white;
    }
</style>
<body>
    <p>This text uses the root color settings.</p>
</body>
```

### Example 3: Global margins and padding

```html
<style>
    :root {
        margin: 0;
        padding: 20pt;
    }
</style>
<body>
    <h1>Document with 20pt padding</h1>
    <p>Content here.</p>
</body>
```

### Example 4: Base text color

```html
<style>
    :root {
        color: #2c3e50;
    }

    a {
        color: #3498db;
    }
</style>
<body>
    <p>Normal text in dark gray.</p>
    <p><a href="#">Links are blue.</a></p>
</body>
```

### Example 5: Document box sizing

```html
<style>
    :root {
        box-sizing: border-box;
    }

    * {
        box-sizing: inherit;
    }
</style>
<body>
    <div style="width: 200pt; padding: 20pt; border: 2pt solid black;">
        Box sizing includes padding and border.
    </div>
</body>
```

### Example 6: Global font weight

```html
<style>
    :root {
        font-weight: normal;
    }

    strong {
        font-weight: bold;
    }
</style>
<body>
    <p>Normal text weight.</p>
    <p><strong>Bold text weight.</strong></p>
</body>
```

### Example 7: Text rendering

```html
<style>
    :root {
        text-rendering: optimizeLegibility;
        -webkit-font-smoothing: antialiased;
    }
</style>
<body>
    <p>Text with optimized rendering.</p>
</body>
```

### Example 8: Page size defaults

```html
<style>
    :root {
        width: 595pt;  /* A4 width */
        height: 842pt; /* A4 height */
    }
</style>
<body>
    <p>Content sized for A4 page.</p>
</body>
```

### Example 9: Default link styling

```html
<style>
    :root {
        color: #333;
    }

    :root a {
        color: #0066cc;
        text-decoration: none;
    }

    :root a:hover {
        text-decoration: underline;
    }
</style>
<body>
    <p>Visit <a href="https://example.com">our website</a>.</p>
</body>
```

### Example 10: Document baseline

```html
<style>
    :root {
        font-family: Georgia, serif;
        font-size: 11pt;
        line-height: 1.8;
        color: #1a1a1a;
        margin: 30pt;
    }
</style>
<body>
    <h1>Document Title</h1>
    <p>All elements inherit from the root baseline settings.</p>
    <p>This creates a consistent appearance throughout the document.</p>
</body>
```

---

## See Also

- [Element Selector](/reference/cssselectors/css_element_selector)
- [Universal Selector](/reference/cssselectors/css_universal_selector)
- [CSS Specificity](/reference/cssselectors/css_specificity)
- [html Element](/reference/htmlelements/html_html_element)

---
