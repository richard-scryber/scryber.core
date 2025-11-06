---
layout: default
title: :hover Pseudo-class
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# :hover Pseudo-class Selector

The `:hover` pseudo-class selector targets elements in their "over" or hover state. This is particularly useful for interactive PDF documents where elements can have different appearances when moused over.

## Usage

```css
element:hover {
    property: value;
}
```

Applies styles to elements when they are in a hover/mouseover state.

---

## Syntax Examples

```css
/* Element hover */
a:hover {
    color: red;
    text-decoration: underline;
}

/* Class hover */
.button:hover {
    background-color: blue;
}

/* ID hover */
#menu-item:hover {
    font-weight: bold;
}
```

---

## Specificity

The pseudo-class adds to the base selector's specificity:
- `element:hover` = element specificity + pseudo-class state
- `.class:hover` = class specificity + pseudo-class state
- `#id:hover` = ID specificity + pseudo-class state

---

## Notes

- Maps to the ComponentState.Over state in Scryber
- Useful for creating interactive PDF documents
- Commonly used with links and buttons
- Hover styles are applied when the mouse cursor is over the element
- Can be combined with other selectors for specific targeting
- Multiple hover states can coexist in a document

---

## Examples

### Example 1: Link hover

```html
<style>
    a {
        color: #0066cc;
        text-decoration: none;
    }

    a:hover {
        color: #cc0000;
        text-decoration: underline;
    }
</style>
<body>
    <p>Visit <a href="https://example.com">our website</a> for more information.</p>
</body>
```

### Example 2: Button hover effect

```html
<style>
    .button {
        background-color: #4CAF50;
        color: white;
        padding: 10pt 20pt;
        border: none;
    }

    .button:hover {
        background-color: #45a049;
        cursor: pointer;
    }
</style>
<body>
    <div class="button">Click Me</div>
</body>
```

### Example 3: Table row hover

```html
<style>
    tr {
        background-color: white;
    }

    tr:hover {
        background-color: #f5f5f5;
    }
</style>
<body>
    <table>
        <tr><td>Row 1</td></tr>
        <tr><td>Row 2</td></tr>
        <tr><td>Row 3</td></tr>
    </table>
</body>
```

### Example 4: Menu item hover

```html
<style>
    .menu-item {
        padding: 10pt;
        color: #333;
    }

    .menu-item:hover {
        background-color: #0066cc;
        color: white;
    }
</style>
<body>
    <div class="menu-item">Home</div>
    <div class="menu-item">About</div>
    <div class="menu-item">Contact</div>
</body>
```

### Example 5: Image hover overlay

```html
<style>
    .image-container {
        position: relative;
    }

    .image-container:hover {
        opacity: 0.8;
    }
</style>
<body>
    <div class="image-container">
        <img src="photo.jpg" alt="Photo" />
    </div>
</body>
```

### Example 6: Card hover effect

```html
<style>
    .card {
        border: 1pt solid #ddd;
        padding: 15pt;
        background-color: white;
    }

    .card:hover {
        border-color: #0066cc;
        box-shadow: 0 2pt 8pt rgba(0,0,0,0.1);
    }
</style>
<body>
    <div class="card">
        <h3>Card Title</h3>
        <p>Card content goes here.</p>
    </div>
</body>
```

### Example 7: Text color change

```html
<style>
    .highlight-text {
        color: #333;
    }

    .highlight-text:hover {
        color: #ff6600;
    }
</style>
<body>
    <p class="highlight-text">Hover over this text to see the color change.</p>
</body>
```

### Example 8: Border hover

```html
<style>
    .box {
        border: 2pt solid transparent;
        padding: 10pt;
    }

    .box:hover {
        border-color: #0066cc;
    }
</style>
<body>
    <div class="box">Hover to see the border</div>
</body>
```

### Example 9: Scale effect

```html
<style>
    .scale-box {
        width: 100pt;
        height: 100pt;
        background-color: #4CAF50;
    }

    .scale-box:hover {
        transform: scale(1.1);
    }
</style>
<body>
    <div class="scale-box"></div>
</body>
```

### Example 10: Navigation link hover

```html
<style>
    nav a {
        color: #666;
        padding: 5pt 10pt;
        text-decoration: none;
    }

    nav a:hover {
        color: white;
        background-color: #0066cc;
    }
</style>
<body>
    <nav>
        <a href="#home">Home</a>
        <a href="#about">About</a>
        <a href="#services">Services</a>
        <a href="#contact">Contact</a>
    </nav>
</body>
```

---

## See Also

- [:before / ::before Pseudo-class](/reference/cssselectors/css_before_selector)
- [:after / ::after Pseudo-class](/reference/cssselectors/css_after_selector)
- [Element Selector](/reference/cssselectors/css_element_selector)
- [Class Selector](/reference/cssselectors/css_class_selector)
- [ComponentState Documentation](/reference/component_state)

---
