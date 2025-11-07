---
layout: default
title: ID Selector
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# ID Selector

The ID selector matches a single element by its unique id attribute value. It is denoted by a hash (#) followed by the ID name and has the highest specificity of 5 points.

## Usage

```css
#idname {
    property: value;
}
```

ID selectors target a single, unique element in the document.

---

## Syntax Examples

```css
/* Target element with id="header" */
#header {
    font-size: 24pt;
    font-weight: bold;
}

/* Combine with element type (optional) */
div#main-content {
    padding: 20pt;
}

/* ID with pseudo-class */
#footer:before {
    content: "Footer: ";
}
```

---

## Specificity

ID selectors have the highest specificity value of **5 points**.

---

## Notes

- IDs must be unique within a document - only one element should have a given ID
- ID names are case-sensitive
- IDs have higher specificity than classes and elements
- Use IDs sparingly; prefer classes for styling multiple elements
- IDs are useful for targeting specific, unique page sections
- Can be used as anchor targets for internal links

---

## Examples

### Example 1: Basic ID styling

```html
<style>
    #header {
        background-color: #333;
        color: white;
        padding: 20pt;
        text-align: center;
    }
</style>
<body>
    <div id="header">
        <h1>Website Header</h1>
    </div>
</body>
```

### Example 2: Multiple unique sections

```html
<style>
    #main-content {
        padding: 20pt;
        background-color: white;
    }

    #sidebar {
        width: 200pt;
        background-color: #f5f5f5;
        padding: 15pt;
    }

    #footer {
        background-color: #333;
        color: white;
        padding: 10pt;
        text-align: center;
    }
</style>
<body>
    <div id="main-content">Main content area</div>
    <div id="sidebar">Sidebar content</div>
    <div id="footer">Footer content</div>
</body>
```

### Example 3: Overriding with high specificity

```html
<style>
    .highlight {
        color: blue;
    }

    #special {
        color: red; /* This wins due to higher specificity */
    }
</style>
<body>
    <p class="highlight">This is blue.</p>
    <p id="special" class="highlight">This is red (ID overrides class).</p>
</body>
```

### Example 4: Form element targeting

```html
<style>
    #username-input {
        width: 200pt;
        padding: 5pt;
        border: 1pt solid #ccc;
    }

    #submit-button {
        background-color: #4CAF50;
        color: white;
        padding: 10pt 20pt;
    }
</style>
<body>
    <form>
        <input id="username-input" type="text" />
        <input id="submit-button" type="submit" value="Submit" />
    </form>
</body>
```

### Example 5: Navigation menu

```html
<style>
    #main-nav {
        background-color: #444;
        padding: 0;
    }

    #main-nav li {
        display: inline-block;
        padding: 10pt 15pt;
        color: white;
    }
</style>
<body>
    <ul id="main-nav">
        <li>Home</li>
        <li>About</li>
        <li>Contact</li>
    </ul>
</body>
```

### Example 6: Hero section

```html
<style>
    #hero {
        background-color: #0066cc;
        color: white;
        padding: 60pt 20pt;
        text-align: center;
    }

    #hero h1 {
        font-size: 36pt;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div id="hero">
        <h1>Welcome to Our Site</h1>
        <p>Your tagline here</p>
    </div>
</body>
```

### Example 7: Specific table styling

```html
<style>
    #data-table {
        width: 100%;
        border-collapse: collapse;
    }

    #data-table th {
        background-color: #4CAF50;
        color: white;
        padding: 10pt;
    }

    #data-table td {
        border: 1pt solid #ddd;
        padding: 8pt;
    }
</style>
<body>
    <table id="data-table">
        <tr>
            <th>Name</th>
            <th>Value</th>
        </tr>
        <tr>
            <td>Item 1</td>
            <td>100</td>
        </tr>
    </table>
</body>
```

### Example 8: Logo container

```html
<style>
    #logo-container {
        width: 150pt;
        height: 50pt;
        margin: 20pt auto;
        text-align: center;
    }

    #logo-container img {
        max-width: 100%;
        height: auto;
    }
</style>
<body>
    <div id="logo-container">
        <img src="logo.png" alt="Company Logo" />
    </div>
</body>
```

### Example 9: Alert message

```html
<style>
    #error-message {
        background-color: #f8d7da;
        color: #721c24;
        border: 1pt solid #f5c6cb;
        padding: 15pt;
        margin: 20pt 0;
    }
</style>
<body>
    <div id="error-message">
        An error has occurred. Please try again.
    </div>
</body>
```

### Example 10: Data binding with ID

```html
<style>
    #customer-name {
        font-size: 18pt;
        font-weight: bold;
        color: #333;
    }

    #customer-balance {
        font-size: 24pt;
        color: #28a745;
        margin-top: 10pt;
    }
</style>
<body>
    <div id="customer-name" data-bind="{{customer.name}}"></div>
    <div id="customer-balance" data-bind="${{customer.balance}}"></div>
</body>
```

---

## See Also

- [Element Selector](/reference/cssselectors/css_element_selector)
- [Class Selector](/reference/cssselectors/css_class_selector)
- [CSS Specificity](/reference/cssselectors/css_specificity)
- [@id attribute](/reference/htmlattributes/attr_id)

---
