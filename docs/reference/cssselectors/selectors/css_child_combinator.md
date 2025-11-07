---
layout: default
title: Child Combinator
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# Child Combinator (>)

The child combinator (>) selects elements that are direct children of a specified parent element. Unlike the descendant combinator, it only matches immediate children, not deeper descendants.

## Usage

```css
parent > child {
    property: value;
}
```

Matches only `child` elements that are direct children of a `parent` element.

---

## Syntax Examples

```css
/* Direct paragraph children of div */
div > p {
    color: blue;
}

/* Direct list items of unordered list */
ul > li {
    font-weight: bold;
}

/* Class-based parent with direct children */
.container > div {
    margin: 10pt;
}

/* ID-based parent */
#header > h1 {
    font-size: 24pt;
}
```

---

## Specificity

The specificity is the sum of all selectors in the chain:
- `div > p` = 1 (element) + 1 (element) = 2 points
- `.container > p` = 2 (class) + 1 (element) = 3 points
- `#main > div` = 5 (ID) + 1 (element) = 6 points

In Scryber, child combinators have higher precedence than descendant combinators, using multipliers of 2, 20, 200, 2000 based on depth.

---

## Notes

- Only matches direct children (one level down)
- More specific and restrictive than descendant combinator (space)
- Useful when you want to target immediate children only
- Does not match grandchildren or deeper descendants
- Denoted by the greater-than symbol (>)
- Provides more precise control over element selection
- Has higher specificity than descendant combinators in Scryber

---

## Examples

### Example 1: Direct children only

```html
<style>
    div > p {
        color: blue;
    }
</style>
<body>
    <div>
        <p>This paragraph is blue (direct child).</p>
        <section>
            <p>This paragraph is NOT blue (grandchild).</p>
        </section>
    </div>
</body>
```

### Example 2: List styling

```html
<style>
    ul > li {
        font-weight: bold;
        color: #333;
    }

    /* Nested list items are not bold */
</style>
<body>
    <ul>
        <li>Bold item (direct child)</li>
        <li>Bold item (direct child)
            <ul>
                <li>Not bold (not direct child of outer ul)</li>
            </ul>
        </li>
    </ul>
</body>
```

### Example 3: Navigation structure

```html
<style>
    nav > ul {
        display: flex;
        list-style: none;
        padding: 0;
    }

    nav > ul > li {
        margin-right: 20pt;
    }
</style>
<body>
    <nav>
        <ul>
            <li>Home</li>
            <li>About</li>
            <li>Contact</li>
        </ul>
    </nav>
</body>
```

### Example 4: Card layout

```html
<style>
    .card > h3 {
        border-bottom: 2pt solid #ddd;
        padding-bottom: 10pt;
        margin-bottom: 15pt;
    }

    .card > p {
        margin: 0;
    }
</style>
<body>
    <div class="card">
        <h3>Card Title</h3>
        <p>Direct paragraph child.</p>
        <div>
            <h3>Nested heading (not styled)</h3>
        </div>
    </div>
</body>
```

### Example 5: Form field groups

```html
<style>
    form > fieldset {
        border: 2pt solid #ccc;
        padding: 15pt;
        margin-bottom: 20pt;
    }

    form > fieldset > legend {
        font-weight: bold;
        padding: 0 5pt;
    }
</style>
<body>
    <form>
        <fieldset>
            <legend>Personal Information</legend>
            <input type="text" />
        </fieldset>
    </form>
</body>
```

### Example 6: Table structure

```html
<style>
    table > thead > tr {
        background-color: #4CAF50;
    }

    table > tbody > tr {
        background-color: white;
    }

    table > tbody > tr:hover {
        background-color: #f5f5f5;
    }
</style>
<body>
    <table>
        <thead>
            <tr>
                <th>Header</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Data</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 7: Container with direct sections

```html
<style>
    .container > section {
        border: 1pt solid #ddd;
        padding: 20pt;
        margin-bottom: 15pt;
    }

    .container > section > h2 {
        margin-top: 0;
        color: #0066cc;
    }
</style>
<body>
    <div class="container">
        <section>
            <h2>Section Title</h2>
            <p>Section content.</p>
        </section>
    </div>
</body>
```

### Example 8: Article structure

```html
<style>
    article > header {
        background-color: #f0f0f0;
        padding: 15pt;
        margin-bottom: 20pt;
    }

    article > header > h1 {
        margin: 0;
        font-size: 24pt;
    }
</style>
<body>
    <article>
        <header>
            <h1>Article Title</h1>
        </header>
        <p>Article content...</p>
    </article>
</body>
```

### Example 9: Sidebar widgets

```html
<style>
    .sidebar > .widget {
        background-color: #f9f9f9;
        padding: 15pt;
        margin-bottom: 15pt;
    }

    .sidebar > .widget > h4 {
        margin-top: 0;
        border-bottom: 1pt solid #ddd;
        padding-bottom: 8pt;
    }
</style>
<body>
    <div class="sidebar">
        <div class="widget">
            <h4>Widget Title</h4>
            <p>Widget content</p>
        </div>
    </div>
</body>
```

### Example 10: Grid layout

```html
<style>
    .grid {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 10pt;
    }

    .grid > .grid-item {
        border: 1pt solid #ddd;
        padding: 10pt;
    }

    /* Only direct children are grid items */
</style>
<body>
    <div class="grid">
        <div class="grid-item">Item 1</div>
        <div class="grid-item">Item 2</div>
        <div class="grid-item">Item 3</div>
    </div>
</body>
```

---

## See Also

- [Descendant Combinator](/reference/cssselectors/css_descendant_combinator)
- [Element Selector](/reference/cssselectors/css_element_selector)
- [Class Selector](/reference/cssselectors/css_class_selector)
- [CSS Specificity](/reference/cssselectors/css_specificity)

---
