---
layout: default
title: :before / ::before Pseudo-class
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# :before / ::before Pseudo-class Selector

The `:before` and `::before` pseudo-class selectors target the "before" state of an element, allowing you to style content or elements that appear before the main element. Both single-colon (`:before`) and double-colon (`::before`) syntax are supported.

## Usage

```css
element:before {
    property: value;
}

element::before {
    property: value;
}
```

Both syntaxes are equivalent and can be used interchangeably.

---

## Syntax Examples

```css
/* Single colon syntax */
p:before {
    content: "Note: ";
    font-weight: bold;
}

/* Double colon syntax */
p::before {
    content: "Note: ";
    font-weight: bold;
}

/* With class selector */
.warning:before {
    content: "⚠ ";
    color: red;
}

/* With ID selector */
#header::before {
    content: "";
    display: block;
    height: 5pt;
    background-color: blue;
}
```

---

## Specificity

The pseudo-class adds to the base selector's specificity:
- `element:before` = element specificity + pseudo-class state
- `.class:before` = class specificity + pseudo-class state
- `#id:before` = ID specificity + pseudo-class state

---

## Notes

- Both `:before` and `::before` syntax are supported and equivalent
- Commonly used with the `content` property to insert text or symbols
- The before state is rendered before the element's actual content
- Can be used for decorative elements without modifying HTML
- Inherits properties from the parent element unless overridden
- Useful for adding prefixes, icons, or decorative elements

---

## Examples

### Example 1: Adding prefix text

```html
<style>
    .note:before {
        content: "Note: ";
        font-weight: bold;
        color: blue;
    }
</style>
<body>
    <p class="note">This is important information.</p>
</body>
```

### Example 2: Warning symbol

```html
<style>
    .warning::before {
        content: "⚠ ";
        color: red;
        font-size: 14pt;
    }
</style>
<body>
    <p class="warning">This action cannot be undone.</p>
</body>
```

### Example 3: Decorative border

```html
<style>
    h1::before {
        content: "";
        display: block;
        width: 50pt;
        height: 3pt;
        background-color: #0066cc;
        margin-bottom: 10pt;
    }
</style>
<body>
    <h1>Section Title</h1>
    <p>Content follows...</p>
</body>
```

### Example 4: Quote marks

```html
<style>
    blockquote:before {
        content: """;
        font-size: 24pt;
        color: #ccc;
        margin-right: 5pt;
    }
</style>
<body>
    <blockquote>This is a quoted text.</blockquote>
</body>
```

### Example 5: List item prefix

```html
<style>
    .checklist li:before {
        content: "✓ ";
        color: green;
        font-weight: bold;
    }
</style>
<body>
    <ul class="checklist">
        <li>Item one</li>
        <li>Item two</li>
        <li>Item three</li>
    </ul>
</body>
```

### Example 6: Label prefix

```html
<style>
    .price:before {
        content: "Price: $";
        font-weight: bold;
        color: #333;
    }
</style>
<body>
    <div class="price">29.99</div>
</body>
```

### Example 7: Section numbering

```html
<style>
    .section-1::before {
        content: "1. ";
        font-weight: bold;
    }

    .section-2::before {
        content: "2. ";
        font-weight: bold;
    }
</style>
<body>
    <h2 class="section-1">Introduction</h2>
    <p>Content for section 1...</p>
    <h2 class="section-2">Details</h2>
    <p>Content for section 2...</p>
</body>
```

### Example 8: Status indicator

```html
<style>
    .status-active:before {
        content: "● ";
        color: green;
        font-size: 16pt;
    }

    .status-inactive:before {
        content: "● ";
        color: red;
        font-size: 16pt;
    }
</style>
<body>
    <p class="status-active">Service is running</p>
    <p class="status-inactive">Service is stopped</p>
</body>
```

### Example 9: Link icon

```html
<style>
    a.external::before {
        content: "→ ";
        color: #0066cc;
    }
</style>
<body>
    <p>Visit <a href="https://example.com" class="external">our website</a> for more.</p>
</body>
```

### Example 10: Table header decoration

```html
<style>
    th:before {
        content: "▸ ";
        color: #666;
    }
</style>
<body>
    <table>
        <tr>
            <th>Name</th>
            <th>Value</th>
            <th>Status</th>
        </tr>
        <tr>
            <td>Item 1</td>
            <td>100</td>
            <td>Active</td>
        </tr>
    </table>
</body>
```

---

## See Also

- [:after / ::after Pseudo-class](/reference/cssselectors/css_after_selector)
- [:hover Pseudo-class](/reference/cssselectors/css_hover_selector)
- [Element Selector](/reference/cssselectors/css_element_selector)
- [Class Selector](/reference/cssselectors/css_class_selector)

---
