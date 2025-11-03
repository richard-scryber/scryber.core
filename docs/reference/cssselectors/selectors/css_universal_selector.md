---
layout: default
title: Universal Selector
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# Universal Selector

The universal selector matches all elements in the document. It is denoted by an asterisk (*) and has a specificity of 1 point (same as an element selector).

## Usage

```css
* {
    property: value;
}
```

The universal selector can be used to apply styles globally to all elements.

---

## Syntax Examples

```css
/* Match all elements */
* {
    margin: 0;
    padding: 0;
}

/* Match all elements with namespace */
*|* {
    box-sizing: border-box;
}

/* Universal with descendant combinator */
div * {
    color: inherit;
}
```

---

## Specificity

The universal selector has a specificity value of **1 point** (equivalent to an element selector).

---

## Notes

- Matches every element in the document
- Often used for CSS resets to normalize default browser styles
- Can impact performance if overused, as it matches everything
- Useful for setting baseline styles that can be overridden
- Both `*` and `*|*` syntax are supported and equivalent

---

## Examples

### Example 1: CSS reset

```html
<style>
    * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
    }
</style>
<body>
    <h1>Heading</h1>
    <p>All elements have their margin and padding reset.</p>
</body>
```

### Example 2: Global font family

```html
<style>
    * {
        font-family: Arial, Helvetica, sans-serif;
    }
</style>
<body>
    <h1>Heading</h1>
    <p>Paragraph text</p>
    <div>All text uses Arial.</div>
</body>
```

### Example 3: Border box model

```html
<style>
    * {
        box-sizing: border-box;
    }

    div {
        width: 200pt;
        padding: 20pt;
        border: 2pt solid black;
    }
</style>
<body>
    <div>
        Width includes padding and border.
    </div>
</body>
```

### Example 4: All descendants of a container

```html
<style>
    .container * {
        color: blue;
        font-size: 12pt;
    }
</style>
<body>
    <div class="container">
        <h1>Heading</h1>
        <p>Paragraph</p>
        <span>Span</span>
        <div>All children are styled blue.</div>
    </div>
</body>
```

### Example 5: Global color inheritance

```html
<style>
    * {
        color: inherit;
    }

    body {
        color: #333;
    }
</style>
<body>
    <h1>Heading inherits color</h1>
    <p>Paragraph also inherits</p>
</body>
```

### Example 6: Debug borders

```html
<style>
    * {
        border: 1pt solid red;
    }
</style>
<body>
    <div>
        <p>Every element has a red border.</p>
        <span>Useful for debugging layout.</span>
    </div>
</body>
```

### Example 7: Line height baseline

```html
<style>
    * {
        line-height: 1.6;
    }
</style>
<body>
    <p>All text elements have improved readability with consistent line height.</p>
    <div>This applies to all elements.</div>
</body>
```

### Example 8: Smooth property transitions

```html
<style>
    * {
        transition: all 0.3s ease;
    }

    .box {
        width: 100pt;
        background-color: blue;
    }

    .box:hover {
        background-color: red;
    }
</style>
<body>
    <div class="box">Hover me</div>
</body>
```

### Example 9: Print styles

```html
<style>
    @media print {
        * {
            color: black;
            background: white;
        }
    }
</style>
<body>
    <p>When printed, all elements use black text on white background.</p>
</body>
```

### Example 10: Remove default outline

```html
<style>
    * {
        outline: none;
    }

    *:focus {
        outline: 2pt solid blue;
    }
</style>
<body>
    <input type="text" placeholder="Custom focus outline" />
</body>
```

---

## See Also

- [Element Selector](/reference/cssselectors/css_element_selector)
- [Class Selector](/reference/cssselectors/css_class_selector)
- [ID Selector](/reference/cssselectors/css_id_selector)
- [Descendant Combinator](/reference/cssselectors/css_descendant_combinator)
- [CSS Specificity](/reference/cssselectors/css_specificity)

---
