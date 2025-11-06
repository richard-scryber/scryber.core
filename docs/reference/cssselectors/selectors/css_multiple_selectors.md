---
layout: default
title: Multiple Selectors
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# Multiple Selectors (Comma-Separated)

Multiple selectors allow you to apply the same styles to different elements by separating selectors with commas. This is also known as a selector list or grouping selectors.

## Usage

```css
selector1, selector2, selector3 {
    property: value;
}
```

Applies the same style rules to all listed selectors.

---

## Syntax Examples

```css
/* Multiple element types */
h1, h2, h3 {
    font-family: Arial, sans-serif;
    color: #333;
}

/* Mix of element and class selectors */
p, .text-content, .description {
    line-height: 1.6;
}

/* Complex selectors */
div p, section article, .container .item {
    margin-bottom: 10pt;
}

/* With pseudo-classes */
a:hover, button:hover {
    opacity: 0.8;
}
```

---

## Specificity

Each selector in the list is evaluated independently:
- `h1, h2, h3` - Each has specificity of 1 (element)
- `#main, .content` - #main has specificity 5, .content has specificity 2
- The most specific matching selector wins for each element

---

## Notes

- Selectors are separated by commas
- Each selector is evaluated independently
- Reduces code duplication by applying same styles to multiple targets
- Each selector has its own specificity calculation
- Particularly useful for normalizing styles across similar elements
- Can mix simple and complex selectors
- Whitespace around commas is optional but recommended for readability

---

## Examples

### Example 1: Heading normalization

```html
<style>
    h1, h2, h3, h4, h5, h6 {
        font-family: Arial, sans-serif;
        font-weight: bold;
        margin-bottom: 15pt;
        color: #333;
    }
</style>
<body>
    <h1>Main Heading</h1>
    <h2>Subheading</h2>
    <h3>Section Heading</h3>
</body>
```

### Example 2: Text elements

```html
<style>
    p, div, span, li {
        font-size: 12pt;
        line-height: 1.6;
        color: #444;
    }
</style>
<body>
    <p>Paragraph text</p>
    <div>Div text</div>
    <span>Span text</span>
    <ul>
        <li>List item</li>
    </ul>
</body>
```

### Example 3: Form controls

```html
<style>
    input, textarea, select {
        width: 100%;
        padding: 8pt;
        border: 1pt solid #ccc;
        font-family: inherit;
        font-size: 12pt;
    }
</style>
<body>
    <form>
        <input type="text" placeholder="Name" />
        <textarea placeholder="Message"></textarea>
        <select>
            <option>Option 1</option>
        </select>
    </form>
</body>
```

### Example 4: Links and buttons

```html
<style>
    a, button, .btn {
        color: #0066cc;
        text-decoration: none;
        padding: 5pt 10pt;
        display: inline-block;
    }

    a:hover, button:hover, .btn:hover {
        background-color: #f0f0f0;
    }
</style>
<body>
    <a href="#">Link</a>
    <button>Button</button>
    <span class="btn">Button-styled span</span>
</body>
```

### Example 5: Reset margins

```html
<style>
    body, h1, h2, h3, p, ul, ol, dl {
        margin: 0;
        padding: 0;
    }
</style>
<body>
    <h1>All margins reset</h1>
    <p>Consistent spacing</p>
</body>
```

### Example 6: Table cells

```html
<style>
    th, td {
        padding: 10pt;
        border: 1pt solid #ddd;
        text-align: left;
    }

    th {
        background-color: #f5f5f5;
        font-weight: bold;
    }
</style>
<body>
    <table>
        <tr>
            <th>Header</th>
            <td>Data</td>
        </tr>
    </table>
</body>
```

### Example 7: Semantic elements

```html
<style>
    article, section, aside, nav, header, footer {
        display: block;
        margin-bottom: 20pt;
    }

    header, footer {
        background-color: #333;
        color: white;
        padding: 15pt;
    }
</style>
<body>
    <header>Header content</header>
    <article>Article content</article>
    <footer>Footer content</footer>
</body>
```

### Example 8: Status classes

```html
<style>
    .success, .info, .warning, .error {
        padding: 12pt;
        margin-bottom: 10pt;
        border-radius: 4pt;
    }

    .success {
        background-color: #d4edda;
        color: #155724;
    }

    .error {
        background-color: #f8d7da;
        color: #721c24;
    }
</style>
<body>
    <div class="success">Success message</div>
    <div class="error">Error message</div>
</body>
```

### Example 9: Complex grouping

```html
<style>
    .container p,
    .container div,
    article section p,
    aside .content {
        font-size: 11pt;
        line-height: 1.5;
    }
</style>
<body>
    <div class="container">
        <p>Styled paragraph</p>
        <div>Styled div</div>
    </div>
    <article>
        <section>
            <p>Also styled</p>
        </section>
    </article>
</body>
```

### Example 10: Media-specific grouping

```html
<style>
    img, video, iframe, embed {
        max-width: 100%;
        height: auto;
        display: block;
    }

    img, video {
        border: 1pt solid #ddd;
    }
</style>
<body>
    <img src="photo.jpg" alt="Photo" />
    <video src="video.mp4"></video>
    <iframe src="page.html"></iframe>
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
