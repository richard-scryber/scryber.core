---
layout: default
title: id
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @id : The ID Attribute

The `id` attribute provides a unique identifier for an HTML element within a document. It serves multiple purposes: creating anchor points for internal links, targeting elements for styling, establishing references for PDF bookmarks, and enabling element identification in data binding scenarios.

## Usage

The `id` attribute assigns a unique identifier to an element that:
- Must be unique within the entire document
- Creates anchor points for internal navigation using `<a href="#id">`
- Allows CSS targeting with ID selectors (`#id`)
- Establishes references for PDF outline entries and bookmarks
- Enables element lookup and manipulation in code
- Supports data binding and dynamic content references

```html
<!-- Basic ID assignment -->
<h1 id="main-title">Document Title</h1>

<!-- Link to element with ID -->
<a href="#main-title">Jump to Title</a>

<!-- CSS targeting by ID -->
<div id="sidebar" style="width: 200pt;"></div>
```

---

## Supported Elements

The `id` attribute is supported on **all HTML elements** in Scryber, including:

### Block Elements
- `<div>`, `<section>`, `<article>`, `<aside>`, `<main>`, `<nav>`
- `<header>`, `<footer>`, `<address>`
- `<h1>` through `<h6>`, `<p>`, `<blockquote>`, `<pre>`
- `<ul>`, `<ol>`, `<li>`, `<dl>`, `<dt>`, `<dd>`
- `<table>`, `<thead>`, `<tbody>`, `<tfoot>`, `<tr>`, `<td>`, `<th>`
- `<fieldset>`, `<legend>`, `<figure>`, `<figcaption>`

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

The `id` attribute supports data binding with dynamic values:

```html
<!-- Dynamic ID from data model -->
<div id="{{model.sectionId}}">
    Section content
</div>

<!-- Computed ID from expression -->
<h2 id="section-{{model.index}}">
    Section {{model.index}}
</h2>

<!-- Conditional ID generation -->
<article id="{{model.type}}-article-{{model.articleId}}">
    Article content
</article>

<!-- In a loop with unique IDs -->
<template data-bind="{{model.chapters}}">
    <div id="chapter-{{.number}}">
        <h2>{{.title}}</h2>
        <p>{{.content}}</p>
    </div>
</template>
```

**Data Model Example:**
```json
{
  "sectionId": "introduction",
  "index": 1,
  "type": "news",
  "articleId": "12345",
  "chapters": [
    { "number": 1, "title": "Getting Started" },
    { "number": 2, "title": "Advanced Topics" }
  ]
}
```

---

## Notes

### Uniqueness Requirement

The `id` attribute value **must be unique** within the entire document. Duplicate IDs can cause:
- Unpredictable behavior with internal links
- CSS selector conflicts
- Broken PDF bookmarks and navigation
- Issues with document structure

```html
<!-- INCORRECT: Duplicate IDs -->
<div id="content">First section</div>
<div id="content">Second section</div>

<!-- CORRECT: Unique IDs -->
<div id="content-section1">First section</div>
<div id="content-section2">Second section</div>
```

### ID Naming Rules

Valid ID values should follow these conventions:
- Start with a letter (a-z, A-Z)
- Can contain letters, digits, hyphens (-), underscores (_), and periods (.)
- Are case-sensitive (`myId` ≠ `MyId`)
- Should not contain spaces or special characters
- Should be descriptive and meaningful

```html
<!-- Valid IDs -->
<div id="main-content"></div>
<div id="section_1"></div>
<div id="nav.primary"></div>
<div id="userId123"></div>

<!-- Avoid these -->
<div id="123section"></div>  <!-- Starts with number -->
<div id="my content"></div>  <!-- Contains space -->
<div id="section@home"></div>  <!-- Special character -->
```

### Internal Navigation

Elements with `id` attributes serve as anchor points for internal links:

```html
<!-- Define anchor points -->
<h2 id="overview">Overview</h2>
<h2 id="features">Features</h2>
<h2 id="pricing">Pricing</h2>

<!-- Link to anchor points -->
<nav>
    <a href="#overview">Overview</a>
    <a href="#features">Features</a>
    <a href="#pricing">Pricing</a>
</nav>
```

When a user clicks a link with `href="#overview"`, the PDF viewer will navigate to the element with `id="overview"`.

### CSS Targeting

Use ID selectors in CSS to target specific elements:

```html
<style>
    #header {
        background-color: #336699;
        color: white;
        padding: 20pt;
    }

    #footer {
        border-top: 2pt solid #ccc;
        text-align: center;
    }
</style>

<div id="header">
    <h1>Document Header</h1>
</div>

<div id="footer">
    <p>© 2025 Company Name</p>
</div>
```

### PDF Bookmarks and Outlines

Elements with `id` attributes can appear in the PDF document outline. Use the `title` attribute to set the bookmark text:

```html
<h1 id="chapter1" title="Chapter 1: Introduction">
    Chapter 1: Introduction
</h1>
```

This creates a navigable bookmark in the PDF outline panel that links to the element.

### ID vs Class

- **ID**: Use for unique elements that appear once per document
- **Class**: Use for reusable styles applied to multiple elements

```html
<!-- ID for unique elements -->
<header id="site-header">...</header>
<main id="main-content">...</main>
<footer id="site-footer">...</footer>

<!-- Class for repeated styling -->
<div class="card">Card 1</div>
<div class="card">Card 2</div>
<div class="card">Card 3</div>
```

---

## Examples

### Basic ID Assignment

```html
<div id="container">
    <h1 id="page-title">Welcome</h1>
    <p id="intro">This is the introduction paragraph.</p>
</div>
```

### Internal Document Navigation

```html
<!DOCTYPE html>
<html>
<head>
    <title>Document Navigation</title>
</head>
<body>
    <!-- Navigation menu -->
    <nav id="main-nav">
        <h2>Contents</h2>
        <ul>
            <li><a href="#intro">Introduction</a></li>
            <li><a href="#methodology">Methodology</a></li>
            <li><a href="#results">Results</a></li>
            <li><a href="#conclusion">Conclusion</a></li>
        </ul>
    </nav>

    <div style="page-break-after: always;"></div>

    <!-- Content sections -->
    <section id="intro">
        <h2>Introduction</h2>
        <p>Introduction content goes here...</p>
        <a href="#main-nav">Back to Contents</a>
    </section>

    <div style="page-break-after: always;"></div>

    <section id="methodology">
        <h2>Methodology</h2>
        <p>Methodology content goes here...</p>
        <a href="#main-nav">Back to Contents</a>
    </section>

    <div style="page-break-after: always;"></div>

    <section id="results">
        <h2>Results</h2>
        <p>Results content goes here...</p>
        <a href="#main-nav">Back to Contents</a>
    </section>

    <div style="page-break-after: always;"></div>

    <section id="conclusion">
        <h2>Conclusion</h2>
        <p>Conclusion content goes here...</p>
        <a href="#main-nav">Back to Contents</a>
    </section>
</body>
</html>
```

### CSS ID Selectors

```html
<style>
    #banner {
        background: linear-gradient(to right, #336699, #66aacc);
        color: white;
        padding: 30pt;
        text-align: center;
        font-size: 24pt;
        font-weight: bold;
    }

    #sidebar {
        float: left;
        width: 200pt;
        padding: 15pt;
        background-color: #f5f5f5;
        border-right: 1pt solid #ddd;
    }

    #main-content {
        margin-left: 230pt;
        padding: 15pt;
    }

    #cta-button {
        display: inline-block;
        padding: 12pt 24pt;
        background-color: #ff6347;
        color: white;
        text-decoration: none;
        border-radius: 5pt;
        font-weight: bold;
    }
</style>

<div id="banner">Welcome to Our Service</div>

<div id="sidebar">
    <h3>Quick Links</h3>
    <ul>
        <li><a href="#about">About</a></li>
        <li><a href="#services">Services</a></li>
        <li><a href="#contact">Contact</a></li>
    </ul>
</div>

<div id="main-content">
    <h1>Main Content Area</h1>
    <p>Your content here...</p>
    <a id="cta-button" href="#signup">Sign Up Now</a>
</div>
```

### Table of Contents with IDs

```html
<div id="toc" style="border: 2pt solid #336699; padding: 20pt; margin-bottom: 30pt;">
    <h2>Table of Contents</h2>
    <ol>
        <li><a href="#chapter1">Getting Started</a></li>
        <li><a href="#chapter2">Basic Concepts</a></li>
        <li><a href="#chapter3">Advanced Features</a></li>
        <li><a href="#chapter4">Best Practices</a></li>
        <li><a href="#chapter5">Troubleshooting</a></li>
    </ol>
</div>

<h1 id="chapter1" title="Chapter 1: Getting Started">Chapter 1: Getting Started</h1>
<p>Content for chapter 1...</p>

<div style="page-break-after: always;"></div>

<h1 id="chapter2" title="Chapter 2: Basic Concepts">Chapter 2: Basic Concepts</h1>
<p>Content for chapter 2...</p>

<div style="page-break-after: always;"></div>

<h1 id="chapter3" title="Chapter 3: Advanced Features">Chapter 3: Advanced Features</h1>
<p>Content for chapter 3...</p>
```

### Data Binding with IDs

```html
<!-- Model: { sections: [{ id: "intro", title: "Introduction" }, ...] } -->

<nav>
    <template data-bind="{{model.sections}}">
        <a href="#{{.id}}">{{.title}}</a><br/>
    </template>
</nav>

<template data-bind="{{model.sections}}">
    <section id="{{.id}}">
        <h2>{{.title}}</h2>
        <p>{{.content}}</p>
    </section>
</template>
```

### Unique IDs in Repeated Content

```html
<!-- Model: { products: [{id: 1, name: "Widget"}, {id: 2, name: "Gadget"}] } -->

<template data-bind="{{model.products}}">
    <div id="product-{{.id}}" style="border: 1pt solid #ccc; padding: 10pt; margin: 10pt 0;">
        <h3 id="product-title-{{.id}}">{{.name}}</h3>
        <p id="product-desc-{{.id}}">{{.description}}</p>
        <a href="#product-details-{{.id}}">View Details</a>
    </div>
</template>
```

### Anchor Navigation with Page Breaks

```html
<!DOCTYPE html>
<html>
<body>
    <!-- Quick reference guide on page 1 -->
    <div id="quick-ref" style="background-color: #f0f0f0; padding: 15pt;">
        <h2>Quick Reference</h2>
        <ul>
            <li><a href="#installation">Installation Guide</a> (Page 2)</li>
            <li><a href="#configuration">Configuration</a> (Page 3)</li>
            <li><a href="#api-reference">API Reference</a> (Page 4)</li>
        </ul>
    </div>

    <div style="page-break-after: always;"></div>

    <!-- Page 2 -->
    <div id="installation">
        <h1>Installation Guide</h1>
        <p>Step-by-step installation instructions...</p>
        <a href="#quick-ref">Return to Quick Reference</a>
    </div>

    <div style="page-break-after: always;"></div>

    <!-- Page 3 -->
    <div id="configuration">
        <h1>Configuration</h1>
        <p>Configuration details...</p>
        <a href="#quick-ref">Return to Quick Reference</a>
    </div>

    <div style="page-break-after: always;"></div>

    <!-- Page 4 -->
    <div id="api-reference">
        <h1>API Reference</h1>
        <p>API documentation...</p>
        <a href="#quick-ref">Return to Quick Reference</a>
    </div>
</body>
</html>
```

### Descriptive ID Naming

```html
<!-- Well-named IDs that describe content -->
<header id="site-header">
    <div id="logo-container">
        <img id="company-logo" src="logo.png" />
    </div>
    <nav id="primary-navigation">
        <a href="#home">Home</a>
        <a href="#about-us">About</a>
        <a href="#contact-info">Contact</a>
    </nav>
</header>

<main id="main-content-area">
    <article id="featured-article">
        <h1 id="article-headline">Breaking News</h1>
        <div id="article-body">
            <p>Article content...</p>
        </div>
    </article>
</main>

<footer id="site-footer">
    <div id="copyright-notice">© 2025</div>
</footer>
```

### Complex Document Structure

```html
<div id="report-wrapper">
    <section id="executive-summary">
        <h1>Executive Summary</h1>
        <div id="summary-highlights">
            <h2>Key Highlights</h2>
            <ul id="highlights-list">
                <li id="highlight-1">Revenue increased 25%</li>
                <li id="highlight-2">Customer base grew 40%</li>
                <li id="highlight-3">New markets opened</li>
            </ul>
        </div>
    </section>

    <section id="financial-data">
        <h1>Financial Data</h1>
        <table id="quarterly-results">
            <thead id="results-header">
                <tr id="header-row">
                    <th id="quarter-col">Quarter</th>
                    <th id="revenue-col">Revenue</th>
                </tr>
            </thead>
            <tbody id="results-body">
                <tr id="q1-data">
                    <td>Q1</td>
                    <td>$1.2M</td>
                </tr>
                <tr id="q2-data">
                    <td>Q2</td>
                    <td>$1.5M</td>
                </tr>
            </tbody>
        </table>
    </section>
</div>
```

### Conditional ID Assignment

```html
<!-- Model: { user: { role: "admin", userId: 123 } } -->

<div id="{{model.user.role}}-dashboard-{{model.user.userId}}">
    <h1>Dashboard for {{model.user.role}}</h1>
    <p>Welcome, user #{{model.user.userId}}</p>
</div>

<!-- Results in: <div id="admin-dashboard-123"> -->
```

### Footer Navigation with IDs

```html
<footer id="document-footer" style="position: fixed; bottom: 0; width: 100%;
                                     border-top: 2pt solid #336699; padding: 10pt;">
    <div id="footer-nav">
        <a href="#top" id="back-to-top">Back to Top</a>
        <a href="!PrevPage" id="prev-page-link">Previous Page</a>
        <a href="!NextPage" id="next-page-link">Next Page</a>
        <a href="#index" id="go-to-index">Index</a>
    </div>
</footer>
```

### Multi-level Document Structure

```html
<article id="main-article">
    <header id="article-header">
        <h1 id="article-title">Complete Guide to PDF Generation</h1>
    </header>

    <nav id="article-toc">
        <h2>Contents</h2>
        <ul>
            <li><a href="#section-intro">Introduction</a>
                <ul>
                    <li><a href="#subsection-overview">Overview</a></li>
                    <li><a href="#subsection-benefits">Benefits</a></li>
                </ul>
            </li>
            <li><a href="#section-implementation">Implementation</a></li>
        </ul>
    </nav>

    <section id="section-intro">
        <h2>Introduction</h2>

        <div id="subsection-overview">
            <h3>Overview</h3>
            <p>Overview content...</p>
        </div>

        <div id="subsection-benefits">
            <h3>Benefits</h3>
            <p>Benefits content...</p>
        </div>
    </section>

    <section id="section-implementation">
        <h2>Implementation</h2>
        <p>Implementation details...</p>
    </section>
</article>
```

### IDs with Bookmarks

```html
<!-- These IDs create PDF bookmarks when title attribute is present -->
<h1 id="ch1" title="Chapter 1: Getting Started">
    Chapter 1: Getting Started
</h1>

<h2 id="ch1-sec1" title="1.1 Installation">
    1.1 Installation
</h2>

<h2 id="ch1-sec2" title="1.2 Configuration">
    1.2 Configuration
</h2>

<h1 id="ch2" title="Chapter 2: Advanced Topics">
    Chapter 2: Advanced Topics
</h1>
```

### Sidebar Navigation with IDs

```html
<div id="page-layout" style="display: flex;">
    <aside id="left-sidebar" style="width: 200pt; padding: 15pt;">
        <nav id="sidebar-nav">
            <h3 id="nav-title">Navigation</h3>
            <ul id="nav-list">
                <li><a href="#home-section">Home</a></li>
                <li><a href="#about-section">About</a></li>
                <li><a href="#services-section">Services</a></li>
                <li><a href="#contact-section">Contact</a></li>
            </ul>
        </nav>
    </aside>

    <main id="main-area" style="flex: 1; padding: 15pt;">
        <div id="home-section">
            <h1>Home</h1>
            <p>Welcome to our site...</p>
        </div>

        <div id="about-section">
            <h1>About</h1>
            <p>About us...</p>
        </div>

        <div id="services-section">
            <h1>Services</h1>
            <p>Our services...</p>
        </div>

        <div id="contact-section">
            <h1>Contact</h1>
            <p>Contact information...</p>
        </div>
    </main>
</div>
```

---

## See Also

- [class](/reference/htmlattributes/class.html) - CSS class attribute for styling multiple elements
- [title](/reference/htmlattributes/title.html) - Title attribute for bookmarks and tooltips
- [a](/reference/htmltags/a.html) - Anchor element for creating links to IDs
- [href](/reference/htmlattributes/href.html) - Hyperlink reference attribute
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Data binding and dynamic expressions
- [PDF Bookmarks](/reference/document/bookmarks.html) - Document outline and bookmarks

---
