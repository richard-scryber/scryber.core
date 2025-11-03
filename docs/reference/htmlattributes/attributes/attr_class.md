---
layout: default
title: class
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @class : The Class Attribute

The `class` attribute assigns one or more CSS class names to an HTML element, enabling reusable styling and consistent design patterns across multiple elements. Classes are the primary mechanism for applying shared styles and can be combined to create flexible, maintainable document designs.

## Usage

The `class` attribute applies CSS class styles to elements:
- Applies styles defined in `<style>` blocks or external stylesheets
- Supports multiple classes on a single element (space-separated)
- Enables reusable design patterns and consistent styling
- Can be combined with inline `style` attributes (inline styles take precedence)
- Supports dynamic class assignment through data binding
- More maintainable than inline styles for repeated patterns

```html
<!-- Single class -->
<div class="container">Content</div>

<!-- Multiple classes -->
<p class="text-large text-bold text-centered">Styled paragraph</p>

<!-- With style definitions -->
<style>
    .highlight {
        background-color: yellow;
        padding: 5pt;
    }
</style>
<span class="highlight">Important text</span>
```

---

## Supported Elements

The `class` attribute is supported on **all HTML elements** in Scryber, including:

### Block Elements
- `<div>`, `<section>`, `<article>`, `<aside>`, `<main>`, `<nav>`
- `<header>`, `<footer>`, `<address>`
- `<h1>` through `<h6>`, `<p>`, `<blockquote>`, `<pre>`
- `<ul>`, `<ol>`, `<li>`, `<dl>`, `<dt>`, `<dd>`
- `<table>`, `<thead>`, `<tbody>`, `<tfoot>`, `<tr>`, `<td>`, `<th>`
- `<fieldset>`, `<legend>`, `<figure>`, `<figcaption>`
- `<details>`, `<summary>`

### Inline Elements
- `<a>`, `<span>`, `<strong>`, `<em>`, `<b>`, `<i>`, `<u>`
- `<code>`, `<kbd>`, `<samp>`, `<var>`, `<mark>`, `<small>`
- `<sub>`, `<sup>`, `<abbr>`, `<cite>`, `<q>`

### Media and Embedded Elements
- `<img>`, `<iframe>`, `<canvas>`

### Form Elements
- `<form>`, `<input>`, `<textarea>`, `<select>`, `<button>`, `<label>`

---

## Binding Values

The `class` attribute supports data binding for dynamic class assignment:

```html
<!-- Dynamic single class -->
<div class="{{model.theme}}">
    Themed content
</div>

<!-- Combining static and dynamic classes -->
<p class="text-base {{model.emphasisClass}}">
    Paragraph text
</p>

<!-- Conditional classes -->
<div class="card {{model.isActive ? 'card-active' : 'card-inactive'}}">
    Card content
</div>

<!-- Multiple dynamic classes -->
<span class="{{model.sizeClass}} {{model.colorClass}} {{model.fontClass}}">
    Styled text
</span>

<!-- In repeated content -->
<template data-bind="{{model.items}}">
    <div class="item {{.category}}">
        <h3 class="item-title {{.priority}}">{{.title}}</h3>
        <p class="item-description">{{.description}}</p>
    </div>
</template>
```

**Data Model Example:**
```json
{
  "theme": "dark-mode",
  "emphasisClass": "text-bold",
  "isActive": true,
  "sizeClass": "text-large",
  "colorClass": "text-primary",
  "fontClass": "font-serif",
  "items": [
    {
      "category": "urgent",
      "priority": "high",
      "title": "Task 1",
      "description": "Description"
    }
  ]
}
```

---

## Notes

### Multiple Classes

Apply multiple classes by separating them with spaces. All matching class styles will be applied:

```html
<style>
    .text-large { font-size: 18pt; }
    .text-bold { font-weight: bold; }
    .text-primary { color: #336699; }
</style>

<!-- All three classes applied -->
<p class="text-large text-bold text-primary">
    Styled paragraph
</p>
```

### Class Naming Conventions

Follow these best practices for class names:
- Use lowercase letters, numbers, hyphens (-), and underscores (_)
- Start with a letter
- Use descriptive, meaningful names
- Consider using naming patterns (BEM, utility classes, etc.)
- Avoid special characters and spaces

```html
<!-- Good class names -->
<div class="container"></div>
<div class="nav-item"></div>
<div class="text-center"></div>
<div class="card-header"></div>
<div class="btn-primary"></div>

<!-- Avoid -->
<div class="Container"></div>  <!-- Capital letters work but lowercase is conventional -->
<div class="nav item"></div>   <!-- Space creates two classes -->
<div class="123test"></div>    <!-- Starts with number -->
```

### CSS Class Selectors

Define styles for classes using the `.classname` selector:

```html
<style>
    /* Single class selector */
    .container {
        width: 100%;
        padding: 20pt;
    }

    /* Multiple class selector (element must have both) */
    .card.featured {
        border: 2pt solid gold;
    }

    /* Descendant selector */
    .article .title {
        font-size: 24pt;
        font-weight: bold;
    }

    /* Child selector */
    .menu > .item {
        padding: 10pt;
    }
</style>

<div class="container">
    <div class="card featured">Featured card</div>
    <article class="article">
        <h2 class="title">Article Title</h2>
    </article>
    <ul class="menu">
        <li class="item">Menu Item</li>
    </ul>
</div>
```

### Specificity and Precedence

When multiple styles apply to an element, CSS specificity determines which style wins:

1. **Inline styles** (highest precedence): `style="..."`
2. **ID selectors**: `#id { ... }`
3. **Class selectors**: `.class { ... }`
4. **Element selectors** (lowest): `div { ... }`

```html
<style>
    div { color: black; }           /* Specificity: 1 */
    .text { color: blue; }          /* Specificity: 10 */
    #special { color: green; }      /* Specificity: 100 */
</style>

<div class="text">Blue text</div>
<div class="text" id="special">Green text (ID wins)</div>
<div class="text" style="color: red;">Red text (inline wins)</div>
```

### Class vs Inline Styles

- **Classes**: Best for reusable styles shared across multiple elements
- **Inline styles**: Best for unique, one-off styling or dynamic values

```html
<!-- Use classes for repeated patterns -->
<style>
    .button {
        padding: 10pt 20pt;
        background-color: #336699;
        color: white;
        border-radius: 5pt;
    }
</style>

<a class="button" href="#page1">Button 1</a>
<a class="button" href="#page2">Button 2</a>
<a class="button" href="#page3">Button 3</a>

<!-- Use inline styles for unique adjustments -->
<div class="button" style="background-color: #ff6347;">
    Special Red Button
</div>
```

### Utility Class Patterns

Create utility classes for common styling patterns:

```html
<style>
    /* Spacing utilities */
    .m-0 { margin: 0; }
    .m-10 { margin: 10pt; }
    .p-10 { padding: 10pt; }
    .p-20 { padding: 20pt; }

    /* Text utilities */
    .text-left { text-align: left; }
    .text-center { text-align: center; }
    .text-right { text-align: right; }

    /* Color utilities */
    .text-primary { color: #336699; }
    .text-danger { color: #dc3545; }
    .bg-light { background-color: #f8f9fa; }
</style>

<div class="p-20 bg-light">
    <h2 class="text-center text-primary m-0">Title</h2>
    <p class="text-left">Content paragraph</p>
</div>
```

### Component Class Patterns

Use classes to create reusable component patterns:

```html
<style>
    .card {
        border: 1pt solid #ddd;
        border-radius: 5pt;
        padding: 15pt;
        margin-bottom: 15pt;
    }

    .card-header {
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 10pt;
        padding-bottom: 10pt;
        border-bottom: 1pt solid #eee;
    }

    .card-body {
        line-height: 1.5;
    }

    .card-footer {
        margin-top: 10pt;
        padding-top: 10pt;
        border-top: 1pt solid #eee;
        font-size: 10pt;
        color: #666;
    }
</style>

<div class="card">
    <div class="card-header">Card Title</div>
    <div class="card-body">Card content goes here...</div>
    <div class="card-footer">Card footer information</div>
</div>
```

---

## Examples

### Basic Class Usage

```html
<style>
    .highlight {
        background-color: yellow;
        padding: 3pt 6pt;
    }
</style>

<p>This is normal text, but <span class="highlight">this is highlighted</span>.</p>
```

### Multiple Classes

```html
<style>
    .text-large { font-size: 18pt; }
    .text-bold { font-weight: bold; }
    .text-italic { font-style: italic; }
    .text-underline { text-decoration: underline; }
</style>

<p class="text-large">Large text</p>
<p class="text-large text-bold">Large and bold</p>
<p class="text-large text-bold text-italic">Large, bold, and italic</p>
<p class="text-large text-bold text-italic text-underline">All styles combined</p>
```

### Component Styling with Classes

```html
<style>
    .btn {
        display: inline-block;
        padding: 10pt 20pt;
        border-radius: 5pt;
        text-decoration: none;
        font-weight: bold;
        text-align: center;
    }

    .btn-primary {
        background-color: #336699;
        color: white;
    }

    .btn-secondary {
        background-color: #6c757d;
        color: white;
    }

    .btn-success {
        background-color: #28a745;
        color: white;
    }

    .btn-danger {
        background-color: #dc3545;
        color: white;
    }

    .btn-large {
        padding: 15pt 30pt;
        font-size: 16pt;
    }
</style>

<a class="btn btn-primary" href="#action1">Primary Button</a>
<a class="btn btn-secondary" href="#action2">Secondary Button</a>
<a class="btn btn-success btn-large" href="#action3">Large Success Button</a>
<a class="btn btn-danger" href="#action4">Danger Button</a>
```

### Card Layout with Classes

```html
<style>
    .card {
        border: 1pt solid #dee2e6;
        border-radius: 8pt;
        padding: 20pt;
        margin-bottom: 20pt;
        background-color: white;
    }

    .card-title {
        font-size: 20pt;
        font-weight: bold;
        color: #336699;
        margin-bottom: 10pt;
    }

    .card-subtitle {
        font-size: 14pt;
        color: #6c757d;
        margin-bottom: 15pt;
    }

    .card-text {
        line-height: 1.6;
        color: #333;
    }

    .card-link {
        color: #336699;
        text-decoration: none;
        font-weight: bold;
        margin-top: 15pt;
        display: inline-block;
    }
</style>

<div class="card">
    <h2 class="card-title">Card Title</h2>
    <h3 class="card-subtitle">Card Subtitle</h3>
    <p class="card-text">
        This is the card content. It can contain any amount of text
        and will be styled according to the card-text class.
    </p>
    <a class="card-link" href="#more">Learn More</a>
</div>

<div class="card">
    <h2 class="card-title">Another Card</h2>
    <p class="card-text">Different content in the same style.</p>
</div>
```

### Grid Layout with Classes

```html
<style>
    .grid {
        display: block;
        width: 100%;
    }

    .grid-row {
        display: flex;
        margin-bottom: 20pt;
    }

    .grid-col {
        flex: 1;
        padding: 10pt;
    }

    .grid-col-2 {
        flex: 2;
    }

    .grid-col-3 {
        flex: 3;
    }
</style>

<div class="grid">
    <div class="grid-row">
        <div class="grid-col" style="background-color: #f0f0f0;">
            <p>Column 1</p>
        </div>
        <div class="grid-col" style="background-color: #e0e0e0;">
            <p>Column 2</p>
        </div>
        <div class="grid-col" style="background-color: #d0d0d0;">
            <p>Column 3</p>
        </div>
    </div>

    <div class="grid-row">
        <div class="grid-col-2" style="background-color: #f0f0f0;">
            <p>2x width</p>
        </div>
        <div class="grid-col" style="background-color: #e0e0e0;">
            <p>1x width</p>
        </div>
    </div>
</div>
```

### Typography Classes

```html
<style>
    .heading-xl { font-size: 32pt; font-weight: bold; line-height: 1.2; }
    .heading-lg { font-size: 24pt; font-weight: bold; line-height: 1.3; }
    .heading-md { font-size: 18pt; font-weight: bold; line-height: 1.4; }
    .heading-sm { font-size: 14pt; font-weight: bold; line-height: 1.4; }

    .text-body { font-size: 12pt; line-height: 1.6; }
    .text-small { font-size: 10pt; line-height: 1.5; }
    .text-tiny { font-size: 8pt; line-height: 1.4; }

    .text-muted { color: #6c757d; }
    .text-primary { color: #336699; }
    .text-success { color: #28a745; }
    .text-danger { color: #dc3545; }
    .text-warning { color: #ffc107; }
</style>

<h1 class="heading-xl">Extra Large Heading</h1>
<h2 class="heading-lg text-primary">Large Primary Heading</h2>
<h3 class="heading-md">Medium Heading</h3>
<h4 class="heading-sm text-muted">Small Muted Heading</h4>

<p class="text-body">This is body text with standard sizing.</p>
<p class="text-small text-muted">This is smaller, muted text.</p>
<p class="text-tiny">This is tiny text.</p>

<p class="text-success">Success message</p>
<p class="text-danger">Error message</p>
<p class="text-warning">Warning message</p>
```

### Table Styling with Classes

```html
<style>
    .table {
        width: 100%;
        border-collapse: collapse;
    }

    .table-header {
        background-color: #336699;
        color: white;
        font-weight: bold;
        padding: 10pt;
        text-align: left;
    }

    .table-row {
        border-bottom: 1pt solid #dee2e6;
    }

    .table-row:nth-child(even) {
        background-color: #f8f9fa;
    }

    .table-cell {
        padding: 10pt;
    }

    .table-cell-right {
        text-align: right;
    }

    .table-cell-center {
        text-align: center;
    }
</style>

<table class="table">
    <thead>
        <tr>
            <th class="table-header">Product</th>
            <th class="table-header table-cell-center">Quantity</th>
            <th class="table-header table-cell-right">Price</th>
        </tr>
    </thead>
    <tbody>
        <tr class="table-row">
            <td class="table-cell">Widget A</td>
            <td class="table-cell table-cell-center">5</td>
            <td class="table-cell table-cell-right">$25.00</td>
        </tr>
        <tr class="table-row">
            <td class="table-cell">Widget B</td>
            <td class="table-cell table-cell-center">3</td>
            <td class="table-cell table-cell-right">$45.00</td>
        </tr>
        <tr class="table-row">
            <td class="table-cell">Widget C</td>
            <td class="table-cell table-cell-center">7</td>
            <td class="table-cell table-cell-right">$15.00</td>
        </tr>
    </tbody>
</table>
```

### Alert/Message Box Classes

```html
<style>
    .alert {
        padding: 15pt;
        margin-bottom: 15pt;
        border-radius: 5pt;
        border: 1pt solid transparent;
    }

    .alert-info {
        background-color: #d1ecf1;
        border-color: #bee5eb;
        color: #0c5460;
    }

    .alert-success {
        background-color: #d4edda;
        border-color: #c3e6cb;
        color: #155724;
    }

    .alert-warning {
        background-color: #fff3cd;
        border-color: #ffeeba;
        color: #856404;
    }

    .alert-danger {
        background-color: #f8d7da;
        border-color: #f5c6cb;
        color: #721c24;
    }

    .alert-title {
        font-weight: bold;
        margin-bottom: 5pt;
    }
</style>

<div class="alert alert-info">
    <div class="alert-title">Information</div>
    <p>This is an informational message.</p>
</div>

<div class="alert alert-success">
    <div class="alert-title">Success!</div>
    <p>Your operation completed successfully.</p>
</div>

<div class="alert alert-warning">
    <div class="alert-title">Warning</div>
    <p>Please review this important information.</p>
</div>

<div class="alert alert-danger">
    <div class="alert-title">Error</div>
    <p>An error occurred. Please try again.</p>
</div>
```

### Data Binding with Classes

```html
<style>
    .status-active { color: #28a745; font-weight: bold; }
    .status-inactive { color: #dc3545; }
    .status-pending { color: #ffc107; }

    .priority-high { background-color: #ffebee; border-left: 4pt solid #dc3545; }
    .priority-medium { background-color: #fff3e0; border-left: 4pt solid #ffc107; }
    .priority-low { background-color: #e8f5e9; border-left: 4pt solid #28a745; }
</style>

<!-- Model: { users: [{name: "John", status: "active"}, ...] } -->
<template data-bind="{{model.users}}">
    <div class="user-card">
        <span class="user-name">{{.name}}</span>
        <span class="status-{{.status}}">{{.status}}</span>
    </div>
</template>

<!-- Model: { tasks: [{title: "Task", priority: "high"}, ...] } -->
<template data-bind="{{model.tasks}}">
    <div class="task priority-{{.priority}}" style="padding: 10pt; margin: 5pt 0;">
        <h4>{{.title}}</h4>
        <p>{{.description}}</p>
    </div>
</template>
```

### Responsive Utility Classes

```html
<style>
    /* Spacing utilities */
    .m-0 { margin: 0; }
    .m-5 { margin: 5pt; }
    .m-10 { margin: 10pt; }
    .m-15 { margin: 15pt; }
    .m-20 { margin: 20pt; }

    .mt-10 { margin-top: 10pt; }
    .mb-10 { margin-bottom: 10pt; }
    .ml-10 { margin-left: 10pt; }
    .mr-10 { margin-right: 10pt; }

    .p-0 { padding: 0; }
    .p-5 { padding: 5pt; }
    .p-10 { padding: 10pt; }
    .p-15 { padding: 15pt; }
    .p-20 { padding: 20pt; }

    /* Display utilities */
    .d-block { display: block; }
    .d-inline { display: inline; }
    .d-inline-block { display: inline-block; }

    /* Text utilities */
    .text-left { text-align: left; }
    .text-center { text-align: center; }
    .text-right { text-align: right; }
    .text-justify { text-align: justify; }

    /* Font weight */
    .font-normal { font-weight: normal; }
    .font-bold { font-weight: bold; }

    /* Border utilities */
    .border { border: 1pt solid #dee2e6; }
    .border-top { border-top: 1pt solid #dee2e6; }
    .border-bottom { border-bottom: 1pt solid #dee2e6; }
</style>

<div class="p-20 border">
    <h2 class="m-0 mb-10 text-center font-bold">Utility Classes Demo</h2>
    <p class="m-0 mb-10 text-justify">
        This paragraph uses utility classes for spacing and alignment.
    </p>
    <div class="text-center border-top p-10">
        <span class="d-inline-block p-5 m-5 border">Box 1</span>
        <span class="d-inline-block p-5 m-5 border">Box 2</span>
        <span class="d-inline-block p-5 m-5 border">Box 3</span>
    </div>
</div>
```

### Navigation Menu with Classes

```html
<style>
    .nav {
        list-style: none;
        padding: 0;
        margin: 0;
        background-color: #336699;
    }

    .nav-item {
        display: inline-block;
    }

    .nav-link {
        display: block;
        padding: 12pt 20pt;
        color: white;
        text-decoration: none;
    }

    .nav-link:hover {
        background-color: #2c5a8a;
    }

    .nav-link.active {
        background-color: #1e3d5c;
        font-weight: bold;
    }
</style>

<ul class="nav">
    <li class="nav-item">
        <a class="nav-link active" href="#home">Home</a>
    </li>
    <li class="nav-item">
        <a class="nav-link" href="#about">About</a>
    </li>
    <li class="nav-item">
        <a class="nav-link" href="#services">Services</a>
    </li>
    <li class="nav-item">
        <a class="nav-link" href="#contact">Contact</a>
    </li>
</ul>
```

### Badge and Label Classes

```html
<style>
    .badge {
        display: inline-block;
        padding: 3pt 8pt;
        font-size: 10pt;
        font-weight: bold;
        border-radius: 3pt;
        text-align: center;
        white-space: nowrap;
    }

    .badge-primary { background-color: #336699; color: white; }
    .badge-success { background-color: #28a745; color: white; }
    .badge-warning { background-color: #ffc107; color: black; }
    .badge-danger { background-color: #dc3545; color: white; }
    .badge-info { background-color: #17a2b8; color: white; }
</style>

<h3>
    Product List
    <span class="badge badge-primary">New</span>
</h3>

<p>
    Status:
    <span class="badge badge-success">Active</span>
    <span class="badge badge-warning">Pending Review</span>
</p>

<div>
    Alerts:
    <span class="badge badge-danger">5</span>
    Critical Issues
</div>

<p>
    Version:
    <span class="badge badge-info">v2.0</span>
</p>
```

### Form Styling with Classes

```html
<style>
    .form-group {
        margin-bottom: 15pt;
    }

    .form-label {
        display: block;
        font-weight: bold;
        margin-bottom: 5pt;
        color: #333;
    }

    .form-input {
        width: 100%;
        padding: 8pt;
        border: 1pt solid #ced4da;
        border-radius: 4pt;
    }

    .form-help {
        font-size: 10pt;
        color: #6c757d;
        margin-top: 5pt;
    }

    .form-error {
        font-size: 10pt;
        color: #dc3545;
        margin-top: 5pt;
    }
</style>

<form>
    <div class="form-group">
        <label class="form-label">Full Name</label>
        <input class="form-input" type="text" />
        <div class="form-help">Enter your first and last name</div>
    </div>

    <div class="form-group">
        <label class="form-label">Email Address</label>
        <input class="form-input" type="email" />
        <div class="form-error">Please enter a valid email address</div>
    </div>

    <div class="form-group">
        <label class="form-label">Message</label>
        <textarea class="form-input" rows="4"></textarea>
    </div>
</form>
```

### Conditional Classes with Data Binding

```html
<style>
    .item { padding: 10pt; margin: 5pt 0; border: 1pt solid #ddd; }
    .item-featured { border-color: gold; border-width: 3pt; background-color: #fffacd; }
    .item-sold { opacity: 0.5; }
    .item-new { border-left: 4pt solid #28a745; }
</style>

<!-- Model: { products: [{name: "Widget", featured: true, sold: false, isNew: true}] } -->
<template data-bind="{{model.products}}">
    <div class="item {{.featured ? 'item-featured' : ''}} {{.sold ? 'item-sold' : ''}} {{.isNew ? 'item-new' : ''}}">
        <h3>{{.name}}</h3>
        <p>{{.description}}</p>
        <p>Price: {{.price}}</p>
    </div>
</template>
```

### Complex Layout with Classes

```html
<style>
    .container {
        width: 100%;
        padding: 20pt;
    }

    .header {
        background-color: #336699;
        color: white;
        padding: 20pt;
        text-align: center;
    }

    .main-content {
        display: flex;
    }

    .sidebar {
        width: 200pt;
        padding: 15pt;
        background-color: #f8f9fa;
        border-right: 1pt solid #dee2e6;
    }

    .content-area {
        flex: 1;
        padding: 15pt;
    }

    .footer {
        background-color: #343a40;
        color: white;
        padding: 15pt;
        text-align: center;
        margin-top: 20pt;
    }

    .section {
        margin-bottom: 20pt;
    }

    .section-title {
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 10pt;
        color: #336699;
        border-bottom: 2pt solid #336699;
        padding-bottom: 5pt;
    }
</style>

<div class="container">
    <div class="header">
        <h1>Company Name</h1>
        <p>Your trusted partner</p>
    </div>

    <div class="main-content">
        <aside class="sidebar">
            <h3>Navigation</h3>
            <ul>
                <li><a href="#section1">Section 1</a></li>
                <li><a href="#section2">Section 2</a></li>
            </ul>
        </aside>

        <main class="content-area">
            <div class="section">
                <h2 class="section-title">Section 1</h2>
                <p>Content for section 1...</p>
            </div>

            <div class="section">
                <h2 class="section-title">Section 2</h2>
                <p>Content for section 2...</p>
            </div>
        </main>
    </div>

    <footer class="footer">
        <p>&copy; 2025 Company Name. All rights reserved.</p>
    </footer>
</div>
```

---

## See Also

- [id](/reference/htmlattributes/id.html) - Unique identifier attribute
- [style](/reference/htmlattributes/style.html) - Inline CSS styling attribute
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Data binding and dynamic expressions
- [style element](/reference/htmltags/style.html) - Defining CSS styles
- [div](/reference/htmltags/div.html) - Generic container element
- [span](/reference/htmltags/span.html) - Generic inline element

---
