---
layout: default
title: Element Selector
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# Element Selector (Type Selector)

The element selector matches elements by their HTML tag name. It is the most basic CSS selector and has a specificity of 1.

## Usage

```css
element {
    property: value;
}
```

Element selectors match all instances of a specific HTML element type in the document.

---

## Syntax Examples

```css
/* Style all paragraphs */
p {
    font-size: 12pt;
    line-height: 1.5;
}

/* Style all divs */
div {
    padding: 10pt;
}

/* Style all headings */
h1 {
    font-size: 24pt;
    font-weight: bold;
}
```

---

## Specificity

Element selectors have the lowest specificity value of **1 point**.

---

## Notes

- Element selectors are case-insensitive in HTML mode
- They match all elements of the specified type throughout the document
- Can be combined with other selectors for more specific targeting
- More efficient than complex selectors as they match directly on element type

---

## Examples

### Example 1: Basic paragraph styling

```html
<style>
    p {
        font-family: Helvetica;
        font-size: 11pt;
        color: #333;
    }
</style>
<body>
    <p>This paragraph will be styled.</p>
    <p>This paragraph will also be styled.</p>
</body>
```

### Example 2: Multiple element selectors

```html
<style>
    h1 {
        font-size: 24pt;
        margin-bottom: 20pt;
    }

    h2 {
        font-size: 18pt;
        margin-bottom: 15pt;
    }

    p {
        font-size: 12pt;
        margin-bottom: 10pt;
    }
</style>
<body>
    <h1>Main Heading</h1>
    <p>Paragraph text.</p>
    <h2>Subheading</h2>
    <p>More paragraph text.</p>
</body>
```

### Example 3: Styling tables

```html
<style>
    table {
        border-collapse: collapse;
        width: 100%;
    }

    th {
        background-color: #4CAF50;
        color: white;
        padding: 8pt;
    }

    td {
        border: 1pt solid #ddd;
        padding: 8pt;
    }
</style>
<body>
    <table>
        <tr>
            <th>Header 1</th>
            <th>Header 2</th>
        </tr>
        <tr>
            <td>Data 1</td>
            <td>Data 2</td>
        </tr>
    </table>
</body>
```

### Example 4: List styling

```html
<style>
    ul {
        margin-left: 20pt;
    }

    li {
        margin-bottom: 5pt;
        color: #555;
    }
</style>
<body>
    <ul>
        <li>First item</li>
        <li>Second item</li>
        <li>Third item</li>
    </ul>
</body>
```

### Example 5: Block-level elements

```html
<style>
    div {
        padding: 15pt;
        margin-bottom: 10pt;
    }

    section {
        border: 1pt solid #ccc;
        background-color: #f9f9f9;
    }
</style>
<body>
    <section>
        <div>Content in a div</div>
        <div>Another div</div>
    </section>
</body>
```

### Example 6: Text formatting elements

```html
<style>
    strong {
        font-weight: bold;
        color: #d00;
    }

    em {
        font-style: italic;
        color: #00d;
    }
</style>
<body>
    <p>This is <strong>bold text</strong> and <em>italic text</em>.</p>
</body>
```

### Example 7: Links

```html
<style>
    a {
        color: #0066cc;
        text-decoration: underline;
    }
</style>
<body>
    <p>Visit our <a href="https://example.com">website</a> for more info.</p>
</body>
```

### Example 8: Spans

```html
<style>
    span {
        background-color: yellow;
        padding: 2pt;
    }
</style>
<body>
    <p>This has a <span>highlighted</span> word.</p>
</body>
```

### Example 9: Images

```html
<style>
    img {
        max-width: 100%;
        border: 2pt solid #ccc;
    }
</style>
<body>
    <img src="image.jpg" alt="Sample" />
</body>
```

### Example 10: Blockquotes

```html
<style>
    blockquote {
        margin-left: 30pt;
        padding-left: 15pt;
        border-left: 3pt solid #ccc;
        font-style: italic;
    }
</style>
<body>
    <blockquote>
        This is a quoted text.
    </blockquote>
</body>
```

---

## See Also

- [Class Selector](/reference/cssselectors/css_class_selector)
- [ID Selector](/reference/cssselectors/css_id_selector)
- [Universal Selector](/reference/cssselectors/css_universal_selector)
- [Descendant Combinator](/reference/cssselectors/css_descendant_combinator)
- [CSS Specificity](/reference/cssselectors/css_specificity)

---
