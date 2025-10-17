---
layout: default
title: a
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;a&gt; : The Anchor/Link Element

The `<a>` (anchor) element creates hyperlinks in PDF documents. It supports internal document links, external URLs, file references, and PDF-specific navigation actions. Links can contain text, images, or other inline content.

## Usage

The `<a>` element creates clickable hyperlinks that:
- Link to external URLs (websites, email addresses)
- Navigate to internal document locations using anchors
- Open external PDF files at specific destinations
- Trigger PDF navigation actions (next page, previous page, etc.)
- Support custom styling (default: blue text with underline)
- Can wrap any inline or block content

```html
<!-- External link -->
<a href="https://www.example.com">Visit Example.com</a>

<!-- Internal document link -->
<a href="#section2">Jump to Section 2</a>

<!-- Link with styled content -->
<a href="https://scryber.com" style="color: red; font-weight: bold;">
    Visit Scryber
</a>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the link. Also used as tooltip text. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Link-Specific Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `href` | string | **Required**. The link destination: URL, file path, or internal anchor (prefixed with #). |
| `target` | string | Link target. Use `_blank` to open in a new window (sets `new-window` behavior). |
| `data-fit-to` | OutlineFit | How the destination page should be displayed: `FullPage`, `PageWidth`, `PageHeight`, `BoundingBox`. Default: `PageWidth`. |

### PDF Action Attributes

Special `href` values can trigger PDF navigation actions by prefixing with `!`:

| href Value | Action | Description |
|-----------|--------|-------------|
| `!NextPage` | Navigate | Go to the next page in the document |
| `!PrevPage` | Navigate | Go to the previous page in the document |
| `!FirstPage` | Navigate | Go to the first page in the document |
| `!LastPage` | Navigate | Go to the last page in the document |

### CSS Style Support

The `<a>` element supports extensive CSS styling:

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `color`, `text-decoration`, `text-transform`
- `letter-spacing`, `word-spacing`

**Visual Styling**:
- `background`, `background-color`
- `border`, `border-radius`, `padding`
- `opacity`, `text-shadow`

**Default Styling**:
- `color`: Blue (#0000FF)
- `text-decoration`: Underline

---

## Notes

### Link Types

Scryber automatically determines the link type based on the `href` value:

1. **Internal Anchor Links** (`#id`): Links starting with `#` navigate to an element with matching `id` within the same document.
2. **External URLs**: Absolute URLs (http://, https://, mailto:, etc.) open external resources.
3. **External File Links**: Relative or absolute file paths open external documents.
4. **Named Actions**: Special `!ActionName` format triggers PDF navigation commands.

### Internal Document Links

To create internal links:

1. Add an `id` attribute to the target element:
   ```html
   <div id="section2">Target Section</div>
   ```

2. Link to it using `#` + the id:
   ```html
   <a href="#section2">Go to Section 2</a>
   ```

The `data-fit-to` attribute controls how the target page is displayed:
- `FullPage`: Display the entire page
- `PageWidth`: Fit to page width (default)
- `PageHeight`: Fit to page height
- `BoundingBox`: Fit to the bounding box of the target element

### External URLs

External URLs are automatically detected and open in the default browser or PDF viewer:

```html
<a href="https://www.example.com">External Website</a>
<a href="mailto:info@example.com">Email Us</a>
```

Set `target="_blank"` to open the link in a new window:
```html
<a href="https://www.example.com" target="_blank">Open in New Window</a>
```

### External File Links

Link to external PDF files or documents:

```html
<!-- Link to another PDF -->
<a href="document.pdf">Open Document</a>

<!-- Link to specific page in external PDF -->
<a href="document.pdf#section1">Open Document at Section 1</a>
```

### PDF Navigation Actions

Use special action syntax for PDF navigation:

```html
<a href="!NextPage">Next</a>
<a href="!PrevPage">Previous</a>
<a href="!FirstPage">First Page</a>
<a href="!LastPage">Last Page</a>
```

### Link Content

Links can contain:
- Text content
- Images
- Spans and other inline elements
- Block elements (divs, paragraphs)
- Mixed content

```html
<a href="#section1">
    <img src="icon.png" width="20pt" height="20pt" />
    Click here
</a>
```

### Accessibility

Use the `title` attribute to provide accessible descriptions:

```html
<a href="https://example.com" title="Visit Example.com homepage">
    Visit Example
</a>
```

### Class Hierarchy

In the Scryber codebase:
- `HTMLAnchor` extends `Link` extends `SpanBase` extends `VisualComponent`
- Inherits inline display behavior from `SpanBase`
- Supports nested content through `Contents` collection

---

## Examples

### Basic External Link

```html
<a href="https://www.scryber.com">Visit Scryber Website</a>
```

### Internal Document Navigation

```html
<!DOCTYPE html>
<html>
<head>
    <title>Document with Internal Links</title>
</head>
<body>
    <h1 id="top">Table of Contents</h1>
    <p>
        <a href="#section1">Section 1</a> |
        <a href="#section2">Section 2</a> |
        <a href="#section3">Section 3</a>
    </p>

    <div style="page-break-after: always;"></div>

    <h2 id="section1">Section 1</h2>
    <p>Content for section 1...</p>
    <a href="#top">Back to Top</a>

    <div style="page-break-after: always;"></div>

    <h2 id="section2">Section 2</h2>
    <p>Content for section 2...</p>
    <a href="#top">Back to Top</a>

    <div style="page-break-after: always;"></div>

    <h2 id="section3">Section 3</h2>
    <p>Content for section 3...</p>
    <a href="#top">Back to Top</a>
</body>
</html>
```

### Styled Links

```html
<style>
    .link-button {
        display: inline-block;
        padding: 8pt 16pt;
        background-color: #336699;
        color: white;
        text-decoration: none;
        border-radius: 4pt;
        font-weight: bold;
    }

    .link-subtle {
        color: #666;
        text-decoration: none;
        border-bottom: 1pt dotted #999;
    }
</style>

<a href="https://example.com" class="link-button">Button Style Link</a>

<a href="#section1" class="link-subtle">Subtle Link</a>

<a href="https://example.com"
   style="color: #ff6347; text-decoration: underline; font-style: italic;">
    Custom Styled Link
</a>
```

### Links with Images

```html
<!-- Image as link content -->
<a href="https://www.scryber.com">
    <img src="logo.png" width="100pt" height="40pt" alt="Scryber Logo" />
</a>

<!-- Text and image -->
<a href="#details" style="text-decoration: none;">
    <img src="info-icon.png" width="16pt" height="16pt"
         style="vertical-align: middle;" />
    <span style="margin-left: 5pt;">More Information</span>
</a>
```

### PDF Navigation Links

```html
<div style="position: fixed; bottom: 10pt; width: 100%;">
    <a href="!FirstPage" style="margin-right: 10pt;">First</a>
    <a href="!PrevPage" style="margin-right: 10pt;">Previous</a>
    <a href="!NextPage" style="margin-right: 10pt;">Next</a>
    <a href="!LastPage">Last</a>
</div>
```

### Email Links

```html
<a href="mailto:support@example.com">Email Support</a>

<a href="mailto:info@example.com?subject=Inquiry&body=Hello">
    Send Inquiry
</a>
```

### Link with Destination Fit

```html
<!-- Link that fits target to page width -->
<a href="#section1" data-fit-to="PageWidth">View Section 1 (Fit Width)</a>

<!-- Link that shows full page -->
<a href="#section2" data-fit-to="FullPage">View Section 2 (Full Page)</a>

<!-- Link that fits to bounding box -->
<a href="#diagram" data-fit-to="BoundingBox">View Diagram (Fit to Box)</a>
```

### External File Links

```html
<!-- Link to external PDF -->
<a href="manual.pdf">Download User Manual</a>

<!-- Link to external PDF with destination -->
<a href="report.pdf#summary">View Report Summary</a>

<!-- Link to external file with new window -->
<a href="document.pdf" target="_blank">Open in New Window</a>
```

### Data Binding in Links

```html
<!-- With model = { url: "https://example.com", linkText: "Click Here" } -->
<a href="{{model.url}}">{{model.linkText}}</a>

<!-- Dynamic styling -->
<a href="{{model.destination}}"
   style="color: {{model.linkColor}}; font-size: {{model.fontSize}}pt;">
    {{model.label}}
</a>

<!-- Repeating links -->
<template data-bind="{{model.menuItems}}">
    <a href="{{.url}}" style="margin-right: 15pt;">{{.title}}</a>
</template>
```

### Link with Block Content

```html
<a href="product-details.pdf" style="text-decoration: none; color: #333;">
    <div style="border: 1pt solid #ccc; padding: 10pt;
                background-color: #f9f9f9; margin: 10pt 0;">
        <h3 style="margin: 0 0 5pt 0; color: #336699;">Product Name</h3>
        <p style="margin: 0;">Click to view detailed specifications</p>
    </div>
</a>
```

### Conditional Links

```html
<!-- Link shown/hidden based on condition -->
<a href="{{model.detailsUrl}}" hidden="{{model.hideLink ? 'hidden' : ''}}">
    View Details
</a>
```

### Table of Contents with Styled Links

```html
<div style="border: 2pt solid #336699; padding: 15pt; margin: 20pt 0;">
    <h2 style="margin-top: 0;">Contents</h2>
    <div style="line-height: 2;">
        <a href="#chapter1" style="display: block; color: #333;
           text-decoration: none; padding: 5pt 0; border-bottom: 1pt solid #eee;">
            <span style="font-weight: bold;">1.</span> Introduction
        </a>
        <a href="#chapter2" style="display: block; color: #333;
           text-decoration: none; padding: 5pt 0; border-bottom: 1pt solid #eee;">
            <span style="font-weight: bold;">2.</span> Getting Started
        </a>
        <a href="#chapter3" style="display: block; color: #333;
           text-decoration: none; padding: 5pt 0;">
            <span style="font-weight: bold;">3.</span> Advanced Topics
        </a>
    </div>
</div>
```

### Footer Navigation

```html
<div style="margin-top: 30pt; padding-top: 10pt;
            border-top: 1pt solid #ccc; text-align: center;">
    <a href="#top" style="margin: 0 20pt;">Top</a>
    <a href="!PrevPage" style="margin: 0 20pt;">Previous Page</a>
    <a href="!NextPage" style="margin: 0 20pt;">Next Page</a>
    <a href="index.pdf" style="margin: 0 20pt;">Index</a>
</div>
```

### Link with Title/Tooltip

```html
<a href="https://www.example.com"
   title="Visit Example.com for more information">
    Example Link
</a>
```

---

## See Also

- [img](/reference/htmltags/img.html) - Image element (often used within links)
- [span](/reference/htmltags/span.html) - Inline container element
- [div](/reference/htmltags/div.html) - Block container (can be wrapped in links)
- [Link Component](/reference/components/link.html) - Base link component in Scryber namespace
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Data binding and expressions
- [PDF Actions](/reference/actions/) - PDF-specific actions and navigation

---
