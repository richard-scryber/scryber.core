---
layout: default
title: Descendant Combinator
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# Descendant Combinator (Space)

The descendant combinator (space) selects elements that are descendants of another element, regardless of how deeply nested they are in the document tree.

## Usage

```css
ancestor descendant {
    property: value;
}
```

Matches all `descendant` elements that are inside an `ancestor` element at any level of nesting.

---

## Syntax Examples

```css
/* All paragraphs inside divs */
div p {
    color: blue;
}

/* All spans inside paragraphs inside divs */
div p span {
    font-weight: bold;
}

/* Class-based descendant */
.container p {
    margin: 10pt;
}

/* ID-based ancestor */
#main-content a {
    color: red;
}
```

---

## Specificity

The specificity is the sum of all selectors in the chain:
- `div p` = 1 (element) + 1 (element) = 2 points
- `.container p` = 2 (class) + 1 (element) = 3 points
- `#main p span` = 5 (ID) + 1 (element) + 1 (element) = 7 points

The depth of the ancestor relationship also affects specificity in Scryber (1, 10, 100, 1000 multipliers).

---

## Notes

- Matches elements at any level of nesting (children, grandchildren, etc.)
- More flexible than the child combinator (>)
- Uses a space as the separator between selectors
- Can chain multiple levels: `div p span` matches spans inside paragraphs inside divs
- More specific than single selectors due to context
- Commonly used for scoped styling within containers

---

## Examples

### Example 1: Basic descendant selector

```html
<style>
    div p {
        color: blue;
        font-size: 14pt;
    }
</style>
<body>
    <div>
        <p>This paragraph is blue (direct child).</p>
        <section>
            <p>This paragraph is also blue (nested deeper).</p>
        </section>
    </div>
    <p>This paragraph is not styled (outside div).</p>
</body>
```

### Example 2: Multi-level descendant

```html
<style>
    article section p {
        line-height: 1.8;
        margin-bottom: 15pt;
    }
</style>
<body>
    <article>
        <section>
            <p>This paragraph has the styling.</p>
            <div>
                <p>This paragraph also has the styling.</p>
            </div>
        </section>
    </article>
</body>
```

### Example 3: Class-based container

```html
<style>
    .content p {
        font-family: Georgia, serif;
        font-size: 12pt;
    }

    .content strong {
        color: red;
    }
</style>
<body>
    <div class="content">
        <p>This paragraph is styled with Georgia font.</p>
        <p>This has <strong>red bold text</strong> inside.</p>
    </div>
</body>
```

### Example 4: Navigation menu styling

```html
<style>
    nav ul {
        list-style: none;
        padding: 0;
    }

    nav ul li {
        display: inline-block;
        margin-right: 15pt;
    }

    nav a {
        color: #333;
        text-decoration: none;
    }
</style>
<body>
    <nav>
        <ul>
            <li><a href="#home">Home</a></li>
            <li><a href="#about">About</a></li>
            <li><a href="#contact">Contact</a></li>
        </ul>
    </nav>
</body>
```

### Example 5: Table cell styling

```html
<style>
    table td {
        padding: 8pt;
        border: 1pt solid #ddd;
    }

    table thead th {
        background-color: #4CAF50;
        color: white;
        font-weight: bold;
    }
</style>
<body>
    <table>
        <thead>
            <tr>
                <th>Header 1</th>
                <th>Header 2</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Data 1</td>
                <td>Data 2</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 6: Card component

```html
<style>
    .card h3 {
        font-size: 16pt;
        margin-bottom: 10pt;
        color: #333;
    }

    .card p {
        font-size: 12pt;
        line-height: 1.6;
        color: #666;
    }

    .card a {
        color: #0066cc;
    }
</style>
<body>
    <div class="card">
        <h3>Card Title</h3>
        <p>Card description with <a href="#">a link</a>.</p>
    </div>
</body>
```

### Example 7: Form field styling

```html
<style>
    form label {
        display: block;
        margin-bottom: 5pt;
        font-weight: bold;
    }

    form input {
        width: 100%;
        padding: 8pt;
        margin-bottom: 15pt;
        border: 1pt solid #ccc;
    }
</style>
<body>
    <form>
        <label>Name:</label>
        <input type="text" />
        <label>Email:</label>
        <input type="email" />
    </form>
</body>
```

### Example 8: Sidebar styling

```html
<style>
    .sidebar h4 {
        border-bottom: 2pt solid #333;
        padding-bottom: 5pt;
        margin-bottom: 10pt;
    }

    .sidebar ul {
        list-style: none;
        padding-left: 0;
    }

    .sidebar li {
        margin-bottom: 8pt;
    }
</style>
<body>
    <div class="sidebar">
        <h4>Quick Links</h4>
        <ul>
            <li>Link 1</li>
            <li>Link 2</li>
        </ul>
    </div>
</body>
```

### Example 9: Nested blockquote

```html
<style>
    article blockquote {
        margin-left: 30pt;
        padding-left: 15pt;
        border-left: 3pt solid #ccc;
        font-style: italic;
    }

    article blockquote p {
        color: #555;
    }
</style>
<body>
    <article>
        <p>Regular paragraph text.</p>
        <blockquote>
            <p>Quoted text with special styling.</p>
        </blockquote>
    </article>
</body>
```

### Example 10: Complex nesting

```html
<style>
    #main-content .section .subsection p {
        font-size: 11pt;
        line-height: 1.5;
    }

    #main-content .section .subsection p strong {
        color: #d00;
        text-transform: uppercase;
    }
</style>
<body>
    <div id="main-content">
        <div class="section">
            <div class="subsection">
                <p>This paragraph is styled.</p>
                <p>This has <strong>emphasized text</strong>.</p>
            </div>
        </div>
    </div>
</body>
```

---

## See Also

- [Child Combinator](/reference/cssselectors/css_child_combinator)
- [Element Selector](/reference/cssselectors/css_element_selector)
- [Class Selector](/reference/cssselectors/css_class_selector)
- [CSS Specificity](/reference/cssselectors/css_specificity)

---
