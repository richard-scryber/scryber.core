---
layout: default
title: href
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @href : The Hyperlink Reference Attribute

The `href` attribute specifies the destination of a hyperlink in anchor (`<a>`) elements. It supports external URLs, internal document anchors, file references, email links, and special PDF navigation actions, enabling comprehensive linking capabilities in PDF documents.

## Usage

The `href` attribute defines link destinations:
- External URLs (https://, http://, ftp://, etc.)
- Internal document anchors (#elementId)
- External file paths (relative or absolute)
- Email addresses (mailto:)
- PDF navigation actions (!NextPage, !PrevPage, etc.)
- Supports data binding for dynamic link generation
- Used exclusively with `<a>` (anchor) elements

```html
<!-- External URL -->
<a href="https://www.example.com">Visit Example.com</a>

<!-- Internal anchor -->
<a href="#section2">Jump to Section 2</a>

<!-- Email link -->
<a href="mailto:info@example.com">Email Us</a>

<!-- PDF navigation -->
<a href="!NextPage">Next Page</a>

<!-- Dynamic link -->
<a href="{{model.documentUrl}}">{{model.linkText}}</a>
```

---

## Supported Elements

The `href` attribute is specifically used with:

### Anchor Element
- `<a>` - The anchor/link element (primary and only use)

While some HTML specifications allow `href` on other elements, in Scryber it is designed for use with `<a>` elements.

---

## Binding Values

The `href` attribute supports data binding for dynamic link destinations:

```html
<!-- Simple dynamic URL -->
<a href="{{model.websiteUrl}}">Visit Website</a>

<!-- Constructed URL -->
<a href="https://example.com/products/{{model.productId}}">
    View Product
</a>

<!-- Dynamic internal anchor -->
<a href="#{{model.targetSection}}">Jump to {{model.sectionName}}</a>

<!-- Conditional link destination -->
<a href="{{model.useExternal ? model.externalUrl : '#internal'}}">
    Link
</a>

<!-- Email with dynamic address -->
<a href="mailto:{{model.contactEmail}}">Contact {{model.contactName}}</a>

<!-- Dynamic file path -->
<a href="{{model.documentPath}}">Download {{model.documentName}}</a>

<!-- Repeating links -->
<template data-bind="{{model.menuItems}}">
    <a href="{{.url}}" style="margin-right: 15pt;">{{.title}}</a>
</template>
```

**Data Model Example:**
```json
{
  "websiteUrl": "https://www.example.com",
  "productId": "12345",
  "targetSection": "details",
  "sectionName": "Details",
  "useExternal": true,
  "externalUrl": "https://external.com",
  "contactEmail": "support@example.com",
  "contactName": "Support Team",
  "documentPath": "files/report.pdf",
  "documentName": "Annual Report",
  "menuItems": [
    { "url": "#intro", "title": "Introduction" },
    { "url": "#features", "title": "Features" },
    { "url": "#pricing", "title": "Pricing" }
  ]
}
```

---

## Notes

### Link Types

Scryber automatically determines the link type based on the `href` value:

#### 1. External URLs
Links starting with protocol identifiers open external resources:

```html
<a href="https://www.example.com">HTTPS Link</a>
<a href="http://www.example.com">HTTP Link</a>
<a href="ftp://ftp.example.com">FTP Link</a>
<a href="mailto:user@example.com">Email Link</a>
<a href="tel:+1234567890">Phone Link</a>
```

#### 2. Internal Anchors
Links starting with `#` navigate to elements with matching `id` within the document:

```html
<!-- Target element -->
<h2 id="section2">Section 2</h2>

<!-- Link to target -->
<a href="#section2">Go to Section 2</a>
```

#### 3. External Files
Relative or absolute file paths link to external documents:

```html
<a href="document.pdf">Open Document</a>
<a href="files/report.pdf">Open Report</a>
<a href="/documents/manual.pdf">Open Manual</a>
```

#### 4. PDF Navigation Actions
Special syntax starting with `!` triggers PDF navigation:

```html
<a href="!NextPage">Next</a>
<a href="!PrevPage">Previous</a>
<a href="!FirstPage">First</a>
<a href="!LastPage">Last</a>
```

### Internal Document Links

To create working internal links:

1. Add an `id` attribute to the target element:
```html
<div id="target">Target content</div>
```

2. Link to it using `#` + the id:
```html
<a href="#target">Go to target</a>
```

3. Optionally control how the target is displayed using `data-fit-to`:
```html
<a href="#target" data-fit-to="PageWidth">Go to target (fit width)</a>
```

**Fit-to options:**
- `FullPage` - Display the entire page
- `PageWidth` - Fit to page width (default)
- `PageHeight` - Fit to page height
- `BoundingBox` - Fit to the bounding box of target element

### External URLs

External URLs open in the default browser or PDF viewer:

```html
<!-- Website -->
<a href="https://www.example.com">Visit Website</a>

<!-- Open in new window -->
<a href="https://www.example.com" target="_blank">Open in New Window</a>

<!-- Email with subject and body -->
<a href="mailto:info@example.com?subject=Inquiry&body=Hello">Send Email</a>

<!-- Phone number -->
<a href="tel:+1-234-567-8900">Call Us</a>
```

### PDF Navigation Actions

Special action syntax provides PDF-specific navigation:

| href Value | Action | Description |
|-----------|--------|-------------|
| `!NextPage` | Navigate | Go to the next page |
| `!PrevPage` | Navigate | Go to the previous page |
| `!FirstPage` | Navigate | Go to the first page |
| `!LastPage` | Navigate | Go to the last page |

```html
<div style="position: fixed; bottom: 10pt;">
    <a href="!FirstPage">First</a> |
    <a href="!PrevPage">Previous</a> |
    <a href="!NextPage">Next</a> |
    <a href="!LastPage">Last</a>
</div>
```

### Linking to External PDFs

You can link to other PDF files and optionally specify destinations:

```html
<!-- Simple file link -->
<a href="other-document.pdf">Open Other Document</a>

<!-- Link to specific page/anchor in external PDF -->
<a href="other-document.pdf#section1">Open at Section 1</a>

<!-- Link with new window behavior -->
<a href="other-document.pdf" target="_blank">Open in New Window</a>
```

### Email Links (mailto:)

Email links can include additional parameters:

```html
<!-- Basic email -->
<a href="mailto:info@example.com">Email Us</a>

<!-- With subject -->
<a href="mailto:info@example.com?subject=Question">Email with Subject</a>

<!-- With subject and body -->
<a href="mailto:info@example.com?subject=Inquiry&body=Hello, I have a question...">
    Email with Content
</a>

<!-- Multiple recipients -->
<a href="mailto:info@example.com,support@example.com">Email Multiple</a>

<!-- CC and BCC -->
<a href="mailto:info@example.com?cc=manager@example.com&bcc=archive@example.com">
    Email with CC/BCC
</a>
```

### URL Encoding

Special characters in URLs should be properly encoded:

```html
<!-- Spaces encoded as %20 or + -->
<a href="https://example.com/my%20document.pdf">Link with Spaces</a>

<!-- Special characters encoded -->
<a href="mailto:user@example.com?subject=Question%20about%20product">
    Encoded Subject
</a>
```

### Link Content

Links can contain various content types:

```html
<!-- Text content -->
<a href="#section1">Click here</a>

<!-- Image -->
<a href="https://example.com">
    <img src="logo.png" width="100pt" height="40pt" />
</a>

<!-- Mixed content -->
<a href="#details">
    <img src="icon.png" width="16pt" height="16pt" />
    <span>More Details</span>
</a>

<!-- Block content -->
<a href="product.pdf" style="text-decoration: none;">
    <div style="border: 1pt solid #ccc; padding: 10pt;">
        <h3>Product Name</h3>
        <p>Click to view details</p>
    </div>
</a>
```

### Missing or Empty href

Links should always have a valid `href` value:

```html
<!-- Valid links -->
<a href="#section1">Internal link</a>
<a href="https://example.com">External link</a>

<!-- Avoid empty href -->
<a href="">Empty link</a>  <!-- May not work as expected -->
<a>No href</a>  <!-- Not a functional link -->
```

### Case Sensitivity

- Protocol schemes (http, https, mailto) are case-insensitive
- Anchor IDs are case-sensitive: `#Section1` ≠ `#section1`
- File paths may be case-sensitive depending on the file system
- PDF actions are case-insensitive: `!NextPage` = `!nextpage`

```html
<!-- These protocols are equivalent -->
<a href="HTTP://example.com">Link</a>
<a href="http://example.com">Link</a>

<!-- These anchors are DIFFERENT -->
<a href="#Section1">Link to Section1</a>
<a href="#section1">Link to section1</a>
```

---

## Examples

### Basic External Links

```html
<p>
    Visit our <a href="https://www.example.com">website</a> for more information.
</p>

<p>
    Read the <a href="https://example.com/docs">documentation</a> to get started.
</p>

<p>
    Check out our <a href="https://github.com/example/repo">GitHub repository</a>.
</p>
```

### Internal Navigation Links

```html
<!DOCTYPE html>
<html>
<body>
    <nav>
        <h2>Contents</h2>
        <ul>
            <li><a href="#introduction">Introduction</a></li>
            <li><a href="#getting-started">Getting Started</a></li>
            <li><a href="#advanced">Advanced Topics</a></li>
            <li><a href="#conclusion">Conclusion</a></li>
        </ul>
    </nav>

    <div style="page-break-after: always;"></div>

    <section id="introduction">
        <h1>Introduction</h1>
        <p>Introduction content...</p>
        <p><a href="#getting-started">Continue to Getting Started</a></p>
    </section>

    <div style="page-break-after: always;"></div>

    <section id="getting-started">
        <h1>Getting Started</h1>
        <p>Getting started content...</p>
        <p>
            <a href="#introduction">Back</a> |
            <a href="#advanced">Next</a>
        </p>
    </section>

    <div style="page-break-after: always;"></div>

    <section id="advanced">
        <h1>Advanced Topics</h1>
        <p>Advanced content...</p>
        <p>
            <a href="#getting-started">Back</a> |
            <a href="#conclusion">Next</a>
        </p>
    </section>

    <div style="page-break-after: always;"></div>

    <section id="conclusion">
        <h1>Conclusion</h1>
        <p>Conclusion content...</p>
        <p><a href="#introduction">Back to Start</a></p>
    </section>
</body>
</html>
```

### Email and Contact Links

```html
<div>
    <h2>Contact Information</h2>

    <p>
        General Inquiries:
        <a href="mailto:info@example.com">info@example.com</a>
    </p>

    <p>
        Technical Support:
        <a href="mailto:support@example.com?subject=Support Request">
            support@example.com
        </a>
    </p>

    <p>
        Sales Department:
        <a href="mailto:sales@example.com?subject=Sales Inquiry&body=I am interested in...">
            Contact Sales
        </a>
    </p>

    <p>
        Phone:
        <a href="tel:+1-800-555-0123">1-800-555-0123</a>
    </p>
</div>
```

### PDF Navigation Links

```html
<footer style="position: fixed; bottom: 0; width: 100%; padding: 10pt;
               border-top: 1pt solid #ccc; text-align: center;">
    <a href="!FirstPage" style="margin: 0 10pt;">First Page</a>
    <a href="!PrevPage" style="margin: 0 10pt;">Previous</a>
    <a href="!NextPage" style="margin: 0 10pt;">Next</a>
    <a href="!LastPage" style="margin: 0 10pt;">Last Page</a>
</footer>
```

### External File Links

```html
<div>
    <h2>Related Documents</h2>

    <ul>
        <li>
            <a href="user-manual.pdf">User Manual (PDF)</a>
        </li>
        <li>
            <a href="technical-specs.pdf">Technical Specifications (PDF)</a>
        </li>
        <li>
            <a href="documents/report-2025.pdf">Annual Report 2025 (PDF)</a>
        </li>
        <li>
            <a href="files/presentation.pdf#slide10">
                Presentation (PDF, opens at slide 10)
            </a>
        </li>
    </ul>
</div>
```

### Links with Custom Styling

```html
<style>
    .link-button {
        display: inline-block;
        padding: 10pt 20pt;
        background-color: #336699;
        color: white;
        text-decoration: none;
        border-radius: 5pt;
        font-weight: bold;
    }

    .link-subtle {
        color: #666;
        text-decoration: none;
        border-bottom: 1pt dotted #999;
    }

    .link-arrow::after {
        content: " →";
    }
</style>

<a href="https://example.com" class="link-button">Get Started</a>

<p>
    For more information, see our
    <a href="#details" class="link-subtle">detailed guide</a>.
</p>

<a href="#next-section" class="link-arrow">Continue reading</a>
```

### Data-Bound Dynamic Links

```html
<!-- Model: {
    products: [
        { id: "p1", name: "Widget A", url: "products/widget-a.pdf" },
        { id: "p2", name: "Widget B", url: "products/widget-b.pdf" }
    ],
    baseUrl: "https://example.com"
} -->

<div>
    <h2>Product Catalog</h2>

    <template data-bind="{{model.products}}">
        <div style="margin-bottom: 15pt; padding: 10pt; border: 1pt solid #ccc;">
            <h3>{{.name}}</h3>
            <p>
                <a href="{{.url}}">View Product Details (PDF)</a> |
                <a href="{{model.baseUrl}}/products/{{.id}}">View Online</a>
            </p>
        </div>
    </template>
</div>
```

### Navigation Menu

```html
<!-- Model: { menuItems: [
    { url: "#home", label: "Home" },
    { url: "#services", label: "Services" },
    { url: "#contact", label: "Contact" }
] } -->

<style>
    .nav-menu {
        background-color: #336699;
        padding: 15pt;
        margin-bottom: 20pt;
    }

    .nav-link {
        color: white;
        text-decoration: none;
        padding: 8pt 15pt;
        margin-right: 5pt;
        display: inline-block;
    }
</style>

<nav class="nav-menu">
    <template data-bind="{{model.menuItems}}">
        <a href="{{.url}}" class="nav-link">{{.label}}</a>
    </template>
</nav>
```

### Table of Contents with Page Links

```html
<div style="border: 2pt solid #336699; padding: 20pt; margin-bottom: 30pt;">
    <h1>Table of Contents</h1>

    <div style="margin-bottom: 10pt;">
        <a href="#chapter1" style="font-size: 14pt; font-weight: bold;">
            1. Introduction
        </a>
        <ul style="list-style: none; margin-left: 20pt;">
            <li><a href="#ch1-overview">1.1 Overview</a></li>
            <li><a href="#ch1-scope">1.2 Scope</a></li>
        </ul>
    </div>

    <div style="margin-bottom: 10pt;">
        <a href="#chapter2" style="font-size: 14pt; font-weight: bold;">
            2. Getting Started
        </a>
        <ul style="list-style: none; margin-left: 20pt;">
            <li><a href="#ch2-install">2.1 Installation</a></li>
            <li><a href="#ch2-config">2.2 Configuration</a></li>
        </ul>
    </div>

    <div>
        <a href="#chapter3" style="font-size: 14pt; font-weight: bold;">
            3. Advanced Topics
        </a>
    </div>
</div>
```

### Links with Icons/Images

```html
<div>
    <h2>Resources</h2>

    <p>
        <a href="https://www.example.com">
            <img src="icons/web.png" width="16pt" height="16pt"
                 style="vertical-align: middle;" />
            Visit Website
        </a>
    </p>

    <p>
        <a href="mailto:info@example.com">
            <img src="icons/email.png" width="16pt" height="16pt"
                 style="vertical-align: middle;" />
            Email Us
        </a>
    </p>

    <p>
        <a href="user-guide.pdf">
            <img src="icons/pdf.png" width="16pt" height="16pt"
                 style="vertical-align: middle;" />
            Download User Guide
        </a>
    </p>
</div>
```

### Back to Top Links

```html
<div id="top">
    <h1>Document Title</h1>
    <p>Document introduction...</p>
</div>

<section id="section1">
    <h2>Section 1</h2>
    <p>Long content for section 1...</p>
    <p style="text-align: right;">
        <a href="#top">↑ Back to Top</a>
    </p>
</section>

<section id="section2">
    <h2>Section 2</h2>
    <p>Long content for section 2...</p>
    <p style="text-align: right;">
        <a href="#top">↑ Back to Top</a>
    </p>
</section>

<section id="section3">
    <h2>Section 3</h2>
    <p>Long content for section 3...</p>
    <p style="text-align: right;">
        <a href="#top">↑ Back to Top</a>
    </p>
</section>
```

### Breadcrumb Navigation

```html
<div style="padding: 10pt; background-color: #f8f9fa; margin-bottom: 20pt;">
    <a href="#home">Home</a>
    <span> / </span>
    <a href="#products">Products</a>
    <span> / </span>
    <a href="#category">Category</a>
    <span> / </span>
    <span style="color: #666;">Current Page</span>
</div>
```

### Call-to-Action Links

```html
<div style="text-align: center; padding: 30pt; background-color: #f0f0f0;">
    <h2>Ready to Get Started?</h2>
    <p>Download our comprehensive guide today.</p>

    <a href="getting-started-guide.pdf"
       style="display: inline-block; padding: 15pt 30pt;
              background-color: #28a745; color: white;
              text-decoration: none; border-radius: 5pt;
              font-size: 16pt; font-weight: bold; margin: 10pt;">
        Download Guide
    </a>

    <a href="https://example.com/signup"
       style="display: inline-block; padding: 15pt 30pt;
              background-color: #336699; color: white;
              text-decoration: none; border-radius: 5pt;
              font-size: 16pt; font-weight: bold; margin: 10pt;">
        Sign Up Now
    </a>
</div>
```

### Footer with Multiple Link Types

```html
<footer style="margin-top: 40pt; padding: 20pt; background-color: #343a40; color: white;">
    <div style="display: flex; justify-content: space-between;">
        <div>
            <h3 style="color: white; margin-bottom: 10pt;">Company</h3>
            <p><a href="#about" style="color: #adb5bd;">About Us</a></p>
            <p><a href="#careers" style="color: #adb5bd;">Careers</a></p>
            <p><a href="#contact" style="color: #adb5bd;">Contact</a></p>
        </div>

        <div>
            <h3 style="color: white; margin-bottom: 10pt;">Resources</h3>
            <p><a href="documentation.pdf" style="color: #adb5bd;">Documentation</a></p>
            <p><a href="https://example.com/blog" style="color: #adb5bd;">Blog</a></p>
            <p><a href="https://example.com/support" style="color: #adb5bd;">Support</a></p>
        </div>

        <div>
            <h3 style="color: white; margin-bottom: 10pt;">Contact</h3>
            <p>
                <a href="mailto:info@example.com" style="color: #adb5bd;">
                    info@example.com
                </a>
            </p>
            <p>
                <a href="tel:+1-800-555-0123" style="color: #adb5bd;">
                    1-800-555-0123
                </a>
            </p>
        </div>
    </div>
</footer>
```

### Conditional Link Destinations

```html
<!-- Model: { user: { isPremium: true }, premiumUrl: "premium.pdf", standardUrl: "standard.pdf" } -->

<div>
    <h2>Access Your Content</h2>

    <a href="{{model.user.isPremium ? model.premiumUrl : model.standardUrl}}"
       style="display: inline-block; padding: 10pt 20pt;
              background-color: #336699; color: white;
              text-decoration: none; border-radius: 5pt;">
        {{model.user.isPremium ? 'Access Premium Content' : 'Access Standard Content'}}
    </a>
</div>
```

### Reference Links in Academic Style

```html
<div>
    <h2>Research Paper</h2>

    <p>
        As noted in the literature<a href="#ref1"><sup>1</sup></a>,
        this phenomenon has been well documented. Recent studies<a href="#ref2"><sup>2</sup></a>
        confirm these findings.
    </p>

    <div style="page-break-before: always;">
        <h2>References</h2>

        <p id="ref1">
            1. Smith, J. (2024). "Study Title".
            <a href="https://doi.org/10.1234/example">DOI Link</a>
        </p>

        <p id="ref2">
            2. Jones, M. (2023). "Another Study".
            <a href="https://doi.org/10.5678/example">DOI Link</a>
        </p>
    </div>
</div>
```

### Download Links with File Information

```html
<div>
    <h2>Available Downloads</h2>

    <div style="border: 1pt solid #ccc; padding: 15pt; margin-bottom: 15pt;">
        <h3 style="margin: 0 0 5pt 0;">User Manual</h3>
        <p style="margin: 0 0 10pt 0; color: #666;">
            Complete guide to using the product. PDF, 2.5 MB
        </p>
        <a href="user-manual.pdf"
           style="color: #336699; font-weight: bold;">
            Download Manual →
        </a>
    </div>

    <div style="border: 1pt solid #ccc; padding: 15pt; margin-bottom: 15pt;">
        <h3 style="margin: 0 0 5pt 0;">Quick Start Guide</h3>
        <p style="margin: 0 0 10pt 0; color: #666;">
            Get started in minutes. PDF, 500 KB
        </p>
        <a href="quick-start.pdf"
           style="color: #336699; font-weight: bold;">
            Download Guide →
        </a>
    </div>

    <div style="border: 1pt solid #ccc; padding: 15pt;">
        <h3 style="margin: 0 0 5pt 0;">API Reference</h3>
        <p style="margin: 0 0 10pt 0; color: #666;">
            Complete API documentation. PDF, 5.2 MB
        </p>
        <a href="api-reference.pdf"
           style="color: #336699; font-weight: bold;">
            Download Reference →
        </a>
    </div>
</div>
```

### Social Media Links

```html
<div style="text-align: center; padding: 20pt;">
    <h3>Follow Us</h3>

    <a href="https://twitter.com/example"
       style="display: inline-block; margin: 5pt;">
        <img src="icons/twitter.png" width="32pt" height="32pt" />
    </a>

    <a href="https://facebook.com/example"
       style="display: inline-block; margin: 5pt;">
        <img src="icons/facebook.png" width="32pt" height="32pt" />
    </a>

    <a href="https://linkedin.com/company/example"
       style="display: inline-block; margin: 5pt;">
        <img src="icons/linkedin.png" width="32pt" height="32pt" />
    </a>

    <a href="https://github.com/example"
       style="display: inline-block; margin: 5pt;">
        <img src="icons/github.png" width="32pt" height="32pt" />
    </a>
</div>
```

---

## See Also

- [a](/reference/htmltags/a.html) - Anchor/link element (uses href attribute)
- [id](/reference/htmlattributes/id.html) - Unique identifier (target for internal links)
- [target](/reference/htmlattributes/target.html) - Link target attribute
- [title](/reference/htmlattributes/title.html) - Title attribute for bookmarks
- [data-fit-to](/reference/htmlattributes/data-fit-to.html) - Control destination display
- [PDF Actions](/reference/actions/) - PDF navigation actions
- [Data Binding](/reference/binding/) - Dynamic link generation

---
