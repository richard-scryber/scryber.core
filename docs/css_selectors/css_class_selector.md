---
layout: default
title: Class Selector
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# Class Selector

The class selector matches elements by their class attribute value. It is denoted by a period (.) followed by the class name and has a specificity of 2-4 points depending on the number of classes.

## Usage

```css
.classname {
    property: value;
}
```

Class selectors can match multiple elements that share the same class and can be combined for more specific targeting.

---

## Syntax Examples

```css
/* Single class */
.highlight {
    background-color: yellow;
}

/* Multiple classes (both must be present) */
.important.urgent {
    color: red;
    font-weight: bold;
}

/* Class with element type */
p.intro {
    font-size: 14pt;
}
```

---

## Specificity

- Single class: **2 points**
- Double class: **3 points**
- Triple class: **4 points**

---

## Notes

- Class names are case-sensitive
- An element can have multiple classes (space-separated in HTML)
- The selector `.class1.class2` matches elements that have both class1 AND class2
- Class selectors are reusable across multiple elements
- More specific than element selectors but less specific than ID selectors

---

## Examples

### Example 1: Basic class styling

```html
<style>
    .highlight {
        background-color: yellow;
        padding: 5pt;
    }
</style>
<body>
    <p class="highlight">This paragraph is highlighted.</p>
    <p>This paragraph is not.</p>
    <span class="highlight">This span is also highlighted.</span>
</body>
```

### Example 2: Multiple classes on one element

```html
<style>
    .large {
        font-size: 16pt;
    }

    .bold {
        font-weight: bold;
    }

    .red {
        color: red;
    }
</style>
<body>
    <p class="large bold">Large and bold text.</p>
    <p class="large red">Large and red text.</p>
    <p class="bold red">Bold and red text.</p>
</body>
```

### Example 3: Multi-class selector

```html
<style>
    .warning.important {
        color: red;
        font-weight: bold;
        border: 2pt solid red;
        padding: 10pt;
    }
</style>
<body>
    <div class="warning">This is just a warning.</div>
    <div class="important">This is just important.</div>
    <div class="warning important">This is both - styled with red border!</div>
</body>
```

### Example 4: Element with class

```html
<style>
    p.intro {
        font-size: 14pt;
        font-weight: bold;
        color: #333;
    }

    div.intro {
        background-color: #f0f0f0;
        padding: 20pt;
    }
</style>
<body>
    <p class="intro">Intro paragraph with special styling.</p>
    <div class="intro">Intro div with different styling.</div>
</body>
```

### Example 5: Card layout pattern

```html
<style>
    .card {
        border: 1pt solid #ddd;
        border-radius: 4pt;
        padding: 15pt;
        margin-bottom: 10pt;
    }

    .card-header {
        font-size: 16pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }

    .card-body {
        font-size: 12pt;
        line-height: 1.5;
    }
</style>
<body>
    <div class="card">
        <div class="card-header">Card Title</div>
        <div class="card-body">Card content goes here.</div>
    </div>
</body>
```

### Example 6: Alert boxes

```html
<style>
    .alert {
        padding: 12pt;
        margin-bottom: 10pt;
        border: 1pt solid transparent;
    }

    .alert-success {
        color: #155724;
        background-color: #d4edda;
        border-color: #c3e6cb;
    }

    .alert-danger {
        color: #721c24;
        background-color: #f8d7da;
        border-color: #f5c6cb;
    }
</style>
<body>
    <div class="alert alert-success">Operation successful!</div>
    <div class="alert alert-danger">Error occurred!</div>
</body>
```

### Example 7: Text utility classes

```html
<style>
    .text-center {
        text-align: center;
    }

    .text-right {
        text-align: right;
    }

    .text-muted {
        color: #6c757d;
    }

    .text-uppercase {
        text-transform: uppercase;
    }
</style>
<body>
    <p class="text-center">Centered text</p>
    <p class="text-right text-muted">Right-aligned muted text</p>
    <p class="text-uppercase">uppercase text</p>
</body>
```

### Example 8: Spacing utilities

```html
<style>
    .m-0 { margin: 0; }
    .m-1 { margin: 5pt; }
    .m-2 { margin: 10pt; }

    .p-0 { padding: 0; }
    .p-1 { padding: 5pt; }
    .p-2 { padding: 10pt; }
</style>
<body>
    <div class="m-2 p-2">Box with margin and padding</div>
    <div class="m-0 p-1">Different spacing</div>
</body>
```

### Example 9: Table row styling

```html
<style>
    .table-row-even {
        background-color: #f2f2f2;
    }

    .table-row-highlight {
        background-color: #ffffcc;
        font-weight: bold;
    }
</style>
<body>
    <table>
        <tr><td>Row 1</td></tr>
        <tr class="table-row-even"><td>Row 2</td></tr>
        <tr><td>Row 3</td></tr>
        <tr class="table-row-even table-row-highlight"><td>Row 4</td></tr>
    </table>
</body>
```

### Example 10: Combined with data binding

```html
<style>
    .product-name {
        font-size: 14pt;
        font-weight: bold;
    }

    .product-price {
        color: #28a745;
        font-size: 16pt;
    }
</style>
<body>
    <div data-bind="{{model.products}}">
        <div class="product-name">{{.name}}</div>
        <div class="product-price">${{.price}}</div>
    </div>
</body>
```

---

## See Also

- [Element Selector](/reference/cssselectors/css_element_selector)
- [ID Selector](/reference/cssselectors/css_id_selector)
- [Multiple Selectors](/reference/cssselectors/css_multiple_selectors)
- [CSS Specificity](/reference/cssselectors/css_specificity)
- [@class attribute](/reference/htmlattributes/attr_class)

---
