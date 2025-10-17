---
layout: default
title: hidden
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @hidden : The Hidden Attribute

The `hidden` attribute controls the visibility of an element in the rendered PDF document. When set to "hidden", the element and all its contents are excluded from the output, making it useful for conditional content display, draft sections, and dynamic visibility control through data binding.

## Usage

The `hidden` attribute controls element visibility:
- Set to `"hidden"` to hide the element and its contents
- Omit the attribute or use empty value to show the element
- Hidden elements do not take up space in the layout
- Useful for conditional content display based on data
- Can hide any type of element (block, inline, media, etc.)
- Supports data binding for dynamic visibility control

```html
<!-- Visible element (no hidden attribute) -->
<div>This content is visible</div>

<!-- Hidden element -->
<div hidden="hidden">This content is not rendered</div>

<!-- Dynamic visibility with data binding -->
<p hidden="{{model.hideWarning ? 'hidden' : ''}}">
    Warning message
</p>
```

---

## Supported Elements

The `hidden` attribute is supported on **all HTML elements** in Scryber, including:

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

### Media Elements
- `<img>`, `<iframe>`, `<canvas>`

### Structural Elements
- `<template>`, `<style>` (though styles are typically processed regardless)

---

## Binding Values

The `hidden` attribute is particularly powerful with data binding for conditional visibility:

```html
<!-- Simple boolean control -->
<div hidden="{{model.isHidden ? 'hidden' : ''}}">
    Conditionally visible content
</div>

<!-- Hide based on condition -->
<p hidden="{{model.userRole != 'admin' ? 'hidden' : ''}}">
    Admin-only content
</p>

<!-- Multiple conditions -->
<section hidden="{{model.isDraft || model.isArchived ? 'hidden' : ''}}">
    Published content only
</section>

<!-- Hide when value is empty -->
<div hidden="{{model.optionalText == '' ? 'hidden' : ''}}">
    <p>{{model.optionalText}}</p>
</div>

<!-- Show/hide based on list presence -->
<div hidden="{{model.items.length == 0 ? 'hidden' : ''}}">
    <h3>Items</h3>
    <template data-bind="{{model.items}}">
        <p>{{.name}}</p>
    </template>
</div>

<!-- Conditional sections in reports -->
<template data-bind="{{model.sections}}">
    <section hidden="{{.includeInReport ? '' : 'hidden'}}">
        <h2>{{.title}}</h2>
        <p>{{.content}}</p>
    </section>
</template>
```

**Data Model Example:**
```json
{
  "isHidden": false,
  "userRole": "admin",
  "isDraft": false,
  "isArchived": false,
  "optionalText": "Some text",
  "items": [
    { "name": "Item 1" },
    { "name": "Item 2" }
  ],
  "sections": [
    {
      "title": "Section 1",
      "content": "Content",
      "includeInReport": true
    },
    {
      "title": "Section 2",
      "content": "Draft content",
      "includeInReport": false
    }
  ]
}
```

---

## Notes

### Hidden vs Display None

The `hidden` attribute in Scryber works similarly to `display: none` in CSS:

- Element is completely removed from layout
- Does not take up any space
- Child elements are also hidden
- Element does not appear in the PDF output

```html
<!-- These are functionally equivalent -->
<div hidden="hidden">Content</div>
<div style="display: none;">Content</div>
```

### Attribute Values

The `hidden` attribute recognizes these values:

- `hidden="hidden"` - Element is hidden
- `hidden=""` - Element is visible (empty value)
- Attribute omitted - Element is visible (default)

```html
<!-- Hidden -->
<div hidden="hidden">Hidden</div>

<!-- Visible -->
<div hidden="">Visible</div>
<div>Visible (no attribute)</div>
```

### Boolean Logic in Data Binding

Use boolean expressions to control visibility dynamically:

```html
<!-- Show if true -->
<div hidden="{{model.showContent ? '' : 'hidden'}}">
    Content shown when showContent is true
</div>

<!-- Hide if true -->
<div hidden="{{model.hideContent ? 'hidden' : ''}}">
    Content hidden when hideContent is true
</div>

<!-- Complex conditions -->
<div hidden="{{(model.status == 'published' && model.approved) ? '' : 'hidden'}}">
    Only shown when published AND approved
</div>
```

### Hiding Table Rows and Cells

You can conditionally hide table rows, cells, and other table elements:

```html
<table>
    <tr>
        <th>Name</th>
        <th>Status</th>
        <th hidden="{{model.showDetails ? '' : 'hidden'}}">Details</th>
    </tr>
    <template data-bind="{{model.users}}">
        <tr hidden="{{.isDeleted ? 'hidden' : ''}}">
            <td>{{.name}}</td>
            <td>{{.status}}</td>
            <td hidden="{{model.showDetails ? '' : 'hidden'}}">{{.details}}</td>
        </tr>
    </template>
</table>
```

### Hiding vs Conditional Rendering

There are two approaches to conditional content:

**1. Using `hidden` attribute:**
```html
<div hidden="{{model.condition ? 'hidden' : ''}}">
    Content always in source, conditionally rendered
</div>
```

**2. Using conditional template logic (if available):**
```html
<!-- Some templating systems support conditional blocks -->
<!-- Check Scryber's template documentation for specific syntax -->
```

The `hidden` attribute is simpler and works consistently across all elements.

### Performance Considerations

Hidden elements:
- Are still parsed and processed
- Don't consume space in the final layout
- Are excluded from the PDF output
- Don't significantly impact performance

For large documents with many conditionally hidden sections, this is generally efficient.

### Debugging Hidden Content

When debugging, temporarily remove `hidden` attributes to see all content:

```html
<!-- During development, comment out hidden to see all content -->
<!-- <div hidden="hidden"> -->
<div>
    Debug content - normally hidden
</div>
```

### Hiding Entire Sections

You can hide large document sections including all nested content:

```html
<section hidden="{{model.includeDraftSections ? '' : 'hidden'}}">
    <h1>Draft Section</h1>
    <p>This entire section and all its contents will be hidden...</p>
    <div>
        <h2>Subsection</h2>
        <p>All nested elements are hidden too</p>
    </div>
</section>
```

---

## Examples

### Basic Visibility Control

```html
<!-- Always visible -->
<p>This paragraph is always visible</p>

<!-- Always hidden -->
<p hidden="hidden">This paragraph is never shown</p>

<!-- Conditionally visible -->
<p hidden="">This paragraph is visible (empty value)</p>
```

### Conditional Content Based on User Role

```html
<!-- Model: { user: { role: "admin" } } -->

<div>
    <h1>Dashboard</h1>

    <!-- Visible to everyone -->
    <section>
        <h2>Overview</h2>
        <p>General information visible to all users</p>
    </section>

    <!-- Admin-only section -->
    <section hidden="{{model.user.role != 'admin' ? 'hidden' : ''}}">
        <h2>Administration</h2>
        <p>This section is only visible to administrators</p>
    </section>

    <!-- Non-admin section -->
    <section hidden="{{model.user.role == 'admin' ? 'hidden' : ''}}">
        <h2>Standard User Information</h2>
        <p>This section is hidden for administrators</p>
    </section>
</div>
```

### Show/Hide Based on Data Presence

```html
<!-- Model: { orderNotes: "", specialInstructions: "Handle with care" } -->

<div>
    <h2>Order Information</h2>

    <!-- Hidden when orderNotes is empty -->
    <div hidden="{{model.orderNotes == '' ? 'hidden' : ''}}">
        <h3>Order Notes</h3>
        <p>{{model.orderNotes}}</p>
    </div>

    <!-- Hidden when specialInstructions is empty -->
    <div hidden="{{model.specialInstructions == '' ? 'hidden' : ''}}">
        <h3>Special Instructions</h3>
        <p>{{model.specialInstructions}}</p>
    </div>
</div>
```

### Conditional Report Sections

```html
<!-- Model: { report: { includeExecutiveSummary: true, includeAppendix: false } } -->

<div>
    <h1>Annual Report</h1>

    <section hidden="{{model.report.includeExecutiveSummary ? '' : 'hidden'}}">
        <h2>Executive Summary</h2>
        <p>High-level overview of the report...</p>
    </section>

    <section>
        <h2>Main Content</h2>
        <p>Always included...</p>
    </section>

    <section hidden="{{model.report.includeAppendix ? '' : 'hidden'}}">
        <h2>Appendix</h2>
        <p>Additional reference materials...</p>
    </section>
</div>
```

### Hiding Table Columns

```html
<!-- Model: { showPricing: true, showStock: false } -->

<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Product</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;"
                hidden="{{model.showPricing ? '' : 'hidden'}}">
                Price
            </th>
            <th style="border: 1pt solid #ccc; padding: 8pt;"
                hidden="{{model.showStock ? '' : 'hidden'}}">
                Stock
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Widget A</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;"
                hidden="{{model.showPricing ? '' : 'hidden'}}">
                $25.00
            </td>
            <td style="border: 1pt solid #ccc; padding: 8pt;"
                hidden="{{model.showStock ? '' : 'hidden'}}">
                150
            </td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Widget B</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;"
                hidden="{{model.showPricing ? '' : 'hidden'}}">
                $35.00
            </td>
            <td style="border: 1pt solid #ccc; padding: 8pt;"
                hidden="{{model.showStock ? '' : 'hidden'}}">
                200
            </td>
        </tr>
    </tbody>
</table>
```

### Conditional Warning Messages

```html
<!-- Model: { hasErrors: true, hasWarnings: false, hasInfo: true } -->

<div>
    <h2>Status Messages</h2>

    <div hidden="{{model.hasErrors ? '' : 'hidden'}}"
         style="padding: 10pt; background-color: #f8d7da;
                border: 1pt solid #f5c6cb; color: #721c24; margin-bottom: 10pt;">
        <strong>Error:</strong> There are errors that need attention.
    </div>

    <div hidden="{{model.hasWarnings ? '' : 'hidden'}}"
         style="padding: 10pt; background-color: #fff3cd;
                border: 1pt solid #ffeeba; color: #856404; margin-bottom: 10pt;">
        <strong>Warning:</strong> Please review the warnings.
    </div>

    <div hidden="{{model.hasInfo ? '' : 'hidden'}}"
         style="padding: 10pt; background-color: #d1ecf1;
                border: 1pt solid #bee5eb; color: #0c5460;">
        <strong>Info:</strong> Additional information is available.
    </div>
</div>
```

### Hiding List Items

```html
<!-- Model: { tasks: [{name: "Task 1", completed: true}, {name: "Task 2", completed: false}] } -->

<div>
    <h2>Active Tasks</h2>
    <ul>
        <template data-bind="{{model.tasks}}">
            <!-- Only show incomplete tasks -->
            <li hidden="{{.completed ? 'hidden' : ''}}">
                {{.name}}
            </li>
        </template>
    </ul>

    <h2>Completed Tasks</h2>
    <ul>
        <template data-bind="{{model.tasks}}">
            <!-- Only show completed tasks -->
            <li hidden="{{.completed ? '' : 'hidden'}}">
                {{.name}}
            </li>
        </template>
    </ul>
</div>
```

### Draft vs Final Document

```html
<!-- Model: { documentMode: "final" } -->

<div>
    <h1>Document Title</h1>

    <!-- Draft watermark -->
    <div hidden="{{model.documentMode == 'final' ? 'hidden' : ''}}"
         style="position: fixed; top: 50%; left: 50%; transform: rotate(-45deg);
                font-size: 72pt; color: rgba(255, 0, 0, 0.2);
                z-index: -1;">
        DRAFT
    </div>

    <!-- Draft notes section -->
    <section hidden="{{model.documentMode == 'final' ? 'hidden' : ''}}"
             style="background-color: #fff3cd; padding: 15pt; border: 2pt solid #ffc107;">
        <h2>Draft Notes</h2>
        <p>This section contains notes for reviewers and will not appear in the final version.</p>
    </section>

    <!-- Main content -->
    <section>
        <h2>Main Content</h2>
        <p>This content appears in both draft and final versions.</p>
    </section>
</div>
```

### Conditional Page Sections

```html
<!-- Model: { includeTableOfContents: true, includeCoverPage: true, includeIndex: false } -->

<!DOCTYPE html>
<html>
<body>
    <!-- Cover page -->
    <div hidden="{{model.includeCoverPage ? '' : 'hidden'}}">
        <div style="text-align: center; padding-top: 200pt;">
            <h1 style="font-size: 36pt;">Document Title</h1>
            <p style="font-size: 18pt;">Subtitle</p>
        </div>
        <div style="page-break-after: always;"></div>
    </div>

    <!-- Table of contents -->
    <div hidden="{{model.includeTableOfContents ? '' : 'hidden'}}">
        <h1>Table of Contents</h1>
        <ul>
            <li><a href="#section1">Section 1</a></li>
            <li><a href="#section2">Section 2</a></li>
        </ul>
        <div style="page-break-after: always;"></div>
    </div>

    <!-- Main content -->
    <section id="section1">
        <h1>Section 1</h1>
        <p>Content...</p>
    </section>

    <section id="section2">
        <h1>Section 2</h1>
        <p>Content...</p>
    </section>

    <!-- Index -->
    <div hidden="{{model.includeIndex ? '' : 'hidden'}}">
        <div style="page-break-before: always;"></div>
        <h1>Index</h1>
        <p>Index entries...</p>
    </div>
</body>
</html>
```

### Hiding Sensitive Information

```html
<!-- Model: { user: { clearanceLevel: 2 }, document: { classificationLevel: 1 } } -->

<div>
    <h1>Report</h1>

    <!-- Public information (clearance 0) -->
    <section>
        <h2>Public Summary</h2>
        <p>This information is available to everyone.</p>
    </section>

    <!-- Confidential (clearance 1) -->
    <section hidden="{{model.user.clearanceLevel < 1 ? 'hidden' : ''}}">
        <h2>Confidential Details</h2>
        <p>This section requires clearance level 1 or higher.</p>
    </section>

    <!-- Secret (clearance 2) -->
    <section hidden="{{model.user.clearanceLevel < 2 ? 'hidden' : ''}}">
        <h2>Secret Information</h2>
        <p>This section requires clearance level 2 or higher.</p>
    </section>

    <!-- Top Secret (clearance 3) -->
    <section hidden="{{model.user.clearanceLevel < 3 ? 'hidden' : ''}}">
        <h2>Top Secret</h2>
        <p>This section requires clearance level 3.</p>
    </section>
</div>
```

### Conditional Graphics and Images

```html
<!-- Model: { includeCharts: true, includePhotos: false } -->

<div>
    <h2>Data Analysis</h2>
    <p>Analysis summary text...</p>

    <!-- Charts section -->
    <div hidden="{{model.includeCharts ? '' : 'hidden'}}">
        <h3>Charts and Graphs</h3>
        <img src="chart1.png" style="width: 400pt; height: 300pt;" />
        <img src="chart2.png" style="width: 400pt; height: 300pt;" />
    </div>

    <!-- Photos section -->
    <div hidden="{{model.includePhotos ? '' : 'hidden'}}">
        <h3>Photographs</h3>
        <img src="photo1.jpg" style="width: 300pt; height: 200pt;" />
        <img src="photo2.jpg" style="width: 300pt; height: 200pt;" />
    </div>
</div>
```

### Dynamic Form Sections

```html
<!-- Model: { formType: "business", includeShipping: true } -->

<div>
    <h2>Registration Form</h2>

    <!-- Always visible -->
    <div>
        <h3>Basic Information</h3>
        <p>Name: [field]</p>
        <p>Email: [field]</p>
    </div>

    <!-- Only for business forms -->
    <div hidden="{{model.formType == 'business' ? '' : 'hidden'}}">
        <h3>Business Information</h3>
        <p>Company Name: [field]</p>
        <p>Tax ID: [field]</p>
    </div>

    <!-- Only for personal forms -->
    <div hidden="{{model.formType == 'personal' ? '' : 'hidden'}}">
        <h3>Personal Information</h3>
        <p>Date of Birth: [field]</p>
    </div>

    <!-- Conditional shipping -->
    <div hidden="{{model.includeShipping ? '' : 'hidden'}}">
        <h3>Shipping Address</h3>
        <p>Address: [field]</p>
        <p>City: [field]</p>
        <p>Postal Code: [field]</p>
    </div>
</div>
```

### Hiding Empty Sections

```html
<!-- Model: { notes: "", recommendations: "Follow up in 30 days", attachments: [] } -->

<div>
    <h2>Report Details</h2>

    <!-- Only show if notes exist -->
    <div hidden="{{model.notes != '' ? '' : 'hidden'}}">
        <h3>Notes</h3>
        <p>{{model.notes}}</p>
    </div>

    <!-- Only show if recommendations exist -->
    <div hidden="{{model.recommendations != '' ? '' : 'hidden'}}">
        <h3>Recommendations</h3>
        <p>{{model.recommendations}}</p>
    </div>

    <!-- Only show if attachments exist -->
    <div hidden="{{model.attachments.length > 0 ? '' : 'hidden'}}">
        <h3>Attachments</h3>
        <ul>
            <template data-bind="{{model.attachments}}">
                <li>{{.filename}}</li>
            </template>
        </ul>
    </div>
</div>
```

### Multi-Language Documents

```html
<!-- Model: { language: "en" } -->

<div>
    <!-- English version -->
    <div hidden="{{model.language == 'en' ? '' : 'hidden'}}">
        <h1>Welcome</h1>
        <p>This is the English version of the document.</p>
    </div>

    <!-- French version -->
    <div hidden="{{model.language == 'fr' ? '' : 'hidden'}}">
        <h1>Bienvenue</h1>
        <p>Ceci est la version française du document.</p>
    </div>

    <!-- Spanish version -->
    <div hidden="{{model.language == 'es' ? '' : 'hidden'}}">
        <h1>Bienvenido</h1>
        <p>Esta es la versión en español del documento.</p>
    </div>
</div>
```

### Conditional Footer Information

```html
<!-- Model: { includeConfidential: true, includePageNumbers: true, documentVersion: "1.2" } -->

<footer style="position: fixed; bottom: 0; width: 100%; padding: 10pt;
               border-top: 1pt solid #ccc; font-size: 9pt;">
    <div style="display: flex; justify-content: space-between;">
        <span hidden="{{model.includeConfidential ? '' : 'hidden'}}"
              style="color: red; font-weight: bold;">
            CONFIDENTIAL
        </span>
        <span>Document v{{model.documentVersion}}</span>
        <span hidden="{{model.includePageNumbers ? '' : 'hidden'}}">
            Page [page-number]
        </span>
    </div>
</footer>
```

### Status-Based Content

```html
<!-- Model: { order: { status: "shipped", trackingNumber: "123456" } } -->

<div>
    <h2>Order Status</h2>

    <div hidden="{{model.order.status == 'pending' ? '' : 'hidden'}}">
        <p style="color: #ffc107;">Your order is pending confirmation.</p>
    </div>

    <div hidden="{{model.order.status == 'processing' ? '' : 'hidden'}}">
        <p style="color: #336699;">Your order is being processed.</p>
    </div>

    <div hidden="{{model.order.status == 'shipped' ? '' : 'hidden'}}">
        <p style="color: #28a745;">Your order has been shipped!</p>
        <p>Tracking number: {{model.order.trackingNumber}}</p>
    </div>

    <div hidden="{{model.order.status == 'delivered' ? '' : 'hidden'}}">
        <p style="color: #28a745; font-weight: bold;">Your order has been delivered.</p>
    </div>

    <div hidden="{{model.order.status == 'cancelled' ? '' : 'hidden'}}">
        <p style="color: #dc3545;">Your order has been cancelled.</p>
    </div>
</div>
```

---

## See Also

- [id](/reference/htmlattributes/id.html) - Unique identifier attribute
- [class](/reference/htmlattributes/class.html) - CSS class attribute
- [style](/reference/htmlattributes/style.html) - Inline CSS styling (including display property)
- [Data Binding](/reference/binding/) - Dynamic data binding and expressions
- [template](/reference/htmltags/template.html) - Template element for repeating content
- [Conditional Rendering](/reference/binding/conditionals.html) - Conditional content patterns

---
