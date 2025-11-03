---
layout: default
title: target
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @target : The Link Target Attribute

The `target` attribute specifies where to open a linked document or resource. Used with `<a>` (anchor) elements, it controls link behavior such as opening in a new window, the same frame, or the parent frame. In PDF documents, it primarily affects external links and can control whether links open in the same PDF viewer or a new window.

## Usage

The `target` attribute defines link opening behavior:
- Specifies where linked documents should open
- Used with `<a>` and `<area>` elements
- Common values: `_blank`, `_self`, `_parent`, `_top`
- Can reference named frames/windows
- In PDF context, mainly affects external links
- Supports data binding for dynamic target assignment

```html
<!-- Open in new window/tab -->
<a href="https://example.com" target="_blank">External Link</a>

<!-- Open in same window (default) -->
<a href="page2.html" target="_self">Same Window</a>

<!-- Open in parent frame -->
<a href="content.html" target="_parent">Parent Frame</a>

<!-- Open in topmost frame -->
<a href="index.html" target="_top">Top Frame</a>

<!-- Named target -->
<a href="content.html" target="contentFrame">Named Frame</a>

<!-- Dynamic target -->
<a href="{{model.url}}" target="{{model.targetWindow}}">Link</a>
```

---

## Supported Elements

The `target` attribute is used with:

### Anchor Element
- `<a>` - Hyperlink element (primary use)

### Area Element
- `<area>` - Image map area element

### Base Element
- `<base>` - Default target for all links in document

### Form Element
- `<form>` - Form submission target (limited PDF use)

---

## Binding Values

The `target` attribute supports data binding for dynamic target assignment:

```html
<!-- Dynamic target from model -->
<a href="{{model.linkUrl}}" target="{{model.linkTarget}}">
    {{model.linkText}}
</a>

<!-- Conditional target -->
<a href="{{model.url}}"
   target="{{model.isExternal ? '_blank' : '_self'}}">
    {{model.title}}
</a>

<!-- Different targets based on link type -->
<template data-bind="{{model.links}}">
    <a href="{{.url}}" target="{{.external ? '_blank' : '_self'}}">
        {{.text}}
    </a>
</template>

<!-- Open external links in new window -->
<template data-bind="{{model.resources}}">
    <a href="{{.url}}"
       target="{{.url.startsWith('http') ? '_blank' : '_self'}}">
        {{.name}}
    </a>
</template>
```

**Data Model Example:**
```json
{
  "linkUrl": "https://example.com",
  "linkTarget": "_blank",
  "linkText": "Visit Website",
  "url": "https://external.com",
  "isExternal": true,
  "title": "External Resource",
  "links": [
    {
      "url": "#section1",
      "text": "Internal Link",
      "external": false
    },
    {
      "url": "https://example.com",
      "text": "External Link",
      "external": true
    }
  ],
  "resources": [
    {
      "url": "https://docs.example.com",
      "name": "Documentation"
    },
    {
      "url": "#appendix",
      "name": "Appendix"
    }
  ]
}
```

---

## Notes

### Target Values

Standard target attribute values:

#### _blank

Opens link in new window/tab:

```html
<!-- Opens in new window or tab -->
<a href="https://example.com" target="_blank">
    Open in New Window
</a>

<!-- External links typically use _blank -->
<a href="https://docs.example.com" target="_blank">
    Documentation (opens in new window)
</a>
```

**Use cases:**
- External websites
- Links that shouldn't interrupt current document
- Reference materials
- Downloads

#### _self

Opens link in same frame/window (default):

```html
<!-- Explicit _self (same as no target) -->
<a href="page2.html" target="_self">
    Next Page (same window)
</a>

<!-- This is the default behavior -->
<a href="page2.html">Same as target="_self"</a>
```

**Use cases:**
- Internal navigation
- Default link behavior
- Sequential page flow

#### _parent

Opens link in parent frame:

```html
<!-- Opens in parent frame -->
<a href="main.html" target="_parent">
    Back to Main (parent frame)
</a>
```

**Use cases:**
- Breaking out of iframe
- Nested frame navigation
- Limited use in PDFs

#### _top

Opens link in topmost frame:

```html
<!-- Opens in topmost window, breaking out of all frames -->
<a href="index.html" target="_top">
    Home (top level)
</a>
```

**Use cases:**
- Escaping nested frames
- Returning to main document
- Limited use in PDFs

#### Named Targets

Custom target names for specific frames:

```html
<!-- Link to specific named frame -->
<a href="content.html" target="mainContent">
    Load in Main Content Frame
</a>

<!-- If frame doesn't exist, opens in new window -->
<a href="sidebar.html" target="sidebarFrame">
    Load in Sidebar
</a>
```

### Default Behavior

When target is omitted:

```html
<!-- No target specified - defaults to _self -->
<a href="page.html">Default Behavior</a>

<!-- Equivalent to: -->
<a href="page.html" target="_self">Explicit _self</a>
```

### PDF Context

In PDF documents:

1. **External links** - May honor `target="_blank"` to open in browser
2. **Internal links** - Typically ignore target (navigate within PDF)
3. **Limited frame support** - PDF viewers don't use frames
4. **New window** - `_blank` may open external URLs outside PDF viewer

```html
<!-- In PDF: Opens external site in browser -->
<a href="https://example.com" target="_blank">
    External Site
</a>

<!-- In PDF: Navigates within same PDF -->
<a href="#section2" target="_blank">
    Internal Link (target usually ignored)
</a>

<!-- In PDF: Opens external PDF -->
<a href="other-document.pdf" target="_blank">
    Another PDF (may open in new window)
</a>
```

### Security Considerations

Using `target="_blank"` has security implications:

```html
<!-- INSECURE: Vulnerable to tabnabbing -->
<a href="https://external.com" target="_blank">
    External Link
</a>

<!-- SECURE: Add rel="noopener noreferrer" -->
<a href="https://external.com" target="_blank" rel="noopener noreferrer">
    Secure External Link
</a>
```

**Why this matters:**
- `noopener` prevents new window from accessing `window.opener`
- `noreferrer` prevents referer information leakage
- Protects against tabnabbing attacks

### Base Target

Set default target for all links:

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Document with Base Target</title>

    <!-- All links open in new window by default -->
    <base target="_blank" />
</head>
<body>
    <!-- Opens in new window (inherits from base) -->
    <a href="https://example.com">External Link</a>

    <!-- Override base target -->
    <a href="#section1" target="_self">Internal Link (same window)</a>
</body>
</html>
```

### Case Sensitivity

Target names are case-sensitive:

```html
<!-- These are DIFFERENT targets -->
<a href="page.html" target="ContentFrame">Capital C</a>
<a href="page.html" target="contentFrame">Lowercase c</a>

<!-- Reserved keywords are case-insensitive -->
<a href="page.html" target="_blank">Lowercase</a>
<a href="page.html" target="_BLANK">Uppercase (same as _blank)</a>
```

**Best Practice:**
- Use lowercase for reserved keywords
- Be consistent with named targets

### Invalid Target Names

Valid vs invalid target names:

```html
<!-- Valid reserved keywords -->
<a href="page.html" target="_blank">Valid</a>
<a href="page.html" target="_self">Valid</a>
<a href="page.html" target="_parent">Valid</a>
<a href="page.html" target="_top">Valid</a>

<!-- Valid named targets -->
<a href="page.html" target="myFrame">Valid</a>
<a href="page.html" target="content123">Valid</a>

<!-- Invalid: Can't start with underscore unless reserved -->
<a href="page.html" target="_custom">Invalid (not reserved)</a>

<!-- Invalid: Special characters -->
<a href="page.html" target="my frame">Invalid (space)</a>
<a href="page.html" target="my-frame@123">Invalid (special chars)</a>
```

### Accessibility Considerations

Screen readers and target:

```html
<!-- Good: Indicate new window in link text -->
<a href="https://example.com" target="_blank" rel="noopener">
    Visit Example.com (opens in new window)
</a>

<!-- Good: Use aria-label for screen readers -->
<a href="https://example.com"
   target="_blank"
   rel="noopener"
   aria-label="Visit Example.com, opens in new window">
    Visit Example.com
</a>

<!-- Good: Visual indicator -->
<a href="https://example.com" target="_blank" rel="noopener">
    External Site <span aria-hidden="true">⧉</span>
</a>
```

### Form Target

Target for form submissions (limited PDF use):

```html
<!-- Submit form results to new window -->
<form action="process.php" method="post" target="_blank">
    <input type="text" name="query" />
    <button type="submit">Submit</button>
</form>

<!-- Submit to named frame -->
<form action="search.php" target="resultsFrame">
    <input type="text" name="search" />
    <button type="submit">Search</button>
</form>
```

---

## Examples

### Basic External Links

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>External References</title>
</head>
<body>
    <h1>Useful Resources</h1>

    <ul>
        <li>
            <a href="https://www.example.com" target="_blank" rel="noopener noreferrer">
                Example Website (opens in new window)
            </a>
        </li>
        <li>
            <a href="https://docs.example.com" target="_blank" rel="noopener noreferrer">
                Documentation (opens in new window)
            </a>
        </li>
        <li>
            <a href="https://support.example.com" target="_blank" rel="noopener noreferrer">
                Support Center (opens in new window)
            </a>
        </li>
    </ul>

    <p>
        <em>Note: External links open in a new window for your convenience.</em>
    </p>
</body>
</html>
```

### Mixed Internal and External Links

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Document Navigation</title>
</head>
<body>
    <h1>Table of Contents</h1>

    <!-- Internal links (same window) -->
    <nav>
        <h2>Sections</h2>
        <ul>
            <li><a href="#introduction" target="_self">Introduction</a></li>
            <li><a href="#methodology" target="_self">Methodology</a></li>
            <li><a href="#results" target="_self">Results</a></li>
            <li><a href="#conclusion" target="_self">Conclusion</a></li>
        </ul>
    </nav>

    <!-- External references (new window) -->
    <nav>
        <h2>External Resources</h2>
        <ul>
            <li>
                <a href="https://example.com/research" target="_blank" rel="noopener">
                    Related Research ⧉
                </a>
            </li>
            <li>
                <a href="https://example.com/data" target="_blank" rel="noopener">
                    Source Data ⧉
                </a>
            </li>
        </ul>
    </nav>

    <section id="introduction">
        <h2>Introduction</h2>
        <p>Content...</p>
    </section>
</body>
</html>
```

### Document with Base Target

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>External Links Document</title>

    <!-- Set default target for all links -->
    <base target="_blank" />

    <style>
        .external-icon::after {
            content: " ⧉";
            font-size: 0.8em;
        }
    </style>
</head>
<body>
    <h1>Reference Guide</h1>

    <p>This document contains numerous external references.</p>

    <!-- These all open in new window (inherit from base) -->
    <h2>Web Resources</h2>
    <ul>
        <li>
            <a href="https://www.w3.org" rel="noopener" class="external-icon">
                W3C Standards
            </a>
        </li>
        <li>
            <a href="https://developer.mozilla.org" rel="noopener" class="external-icon">
                MDN Web Docs
            </a>
        </li>
    </ul>

    <!-- Override base target for internal navigation -->
    <h2>Document Sections</h2>
    <ul>
        <li><a href="#section1" target="_self">Section 1 (this document)</a></li>
        <li><a href="#section2" target="_self">Section 2 (this document)</a></li>
    </ul>

    <section id="section1">
        <h2>Section 1</h2>
        <p>Internal content...</p>
    </section>
</body>
</html>
```

### Footer with Social Links

```html
<footer style="margin-top: 40pt; padding: 20pt; background-color: #f8f9fa;">
    <div style="text-align: center;">
        <h3>Connect With Us</h3>

        <!-- Social media links open in new windows -->
        <p>
            <a href="https://twitter.com/example"
               target="_blank"
               rel="noopener noreferrer"
               style="margin: 0 10pt;">
                Twitter
            </a>
            <a href="https://facebook.com/example"
               target="_blank"
               rel="noopener noreferrer"
               style="margin: 0 10pt;">
                Facebook
            </a>
            <a href="https://linkedin.com/company/example"
               target="_blank"
               rel="noopener noreferrer"
               style="margin: 0 10pt;">
                LinkedIn
            </a>
            <a href="https://github.com/example"
               target="_blank"
               rel="noopener noreferrer"
               style="margin: 0 10pt;">
                GitHub
            </a>
        </p>

        <!-- Internal links stay in same window -->
        <p style="margin-top: 20pt; font-size: 9pt;">
            <a href="#privacy" target="_self">Privacy Policy</a> |
            <a href="#terms" target="_self">Terms of Service</a> |
            <a href="#contact" target="_self">Contact Us</a>
        </p>
    </div>
</footer>
```

### Data-Bound Links

```html
<!-- Model: {
    externalLinks: [
        { url: "https://example.com", title: "Example Site", external: true },
        { url: "https://docs.example.com", title: "Documentation", external: true }
    ],
    internalLinks: [
        { url: "#section1", title: "Section 1", external: false },
        { url: "#section2", title: "Section 2", external: false }
    ]
} -->

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Dynamic Links</title>
</head>
<body>
    <h1>Quick Links</h1>

    <h2>External Resources</h2>
    <ul>
        <template data-bind="{{model.externalLinks}}">
            <li>
                <a href="{{.url}}"
                   target="{{.external ? '_blank' : '_self'}}"
                   rel="{{.external ? 'noopener noreferrer' : ''}}">
                    {{.title}}{{.external ? ' ⧉' : ''}}
                </a>
            </li>
        </template>
    </ul>

    <h2>Document Sections</h2>
    <ul>
        <template data-bind="{{model.internalLinks}}">
            <li>
                <a href="{{.url}}" target="_self">
                    {{.title}}
                </a>
            </li>
        </template>
    </ul>
</body>
</html>
```

### Research Paper References

```html
<article>
    <h1>Research Paper: Climate Change Impact</h1>

    <section>
        <h2>Introduction</h2>
        <p>
            This paper examines the impact of climate change on coastal regions.
            For background information, see the
            <a href="https://www.ipcc.ch" target="_blank" rel="noopener noreferrer">
                IPCC reports (opens in new window)
            </a>.
        </p>
    </section>

    <section>
        <h2>Methodology</h2>
        <p>Our research methodology follows established protocols...</p>
    </section>

    <section>
        <h2>References</h2>
        <ol>
            <li>
                Smith, J. et al. (2024).
                <a href="https://doi.org/10.1234/example1"
                   target="_blank"
                   rel="noopener noreferrer">
                    "Climate Patterns Analysis" ⧉
                </a>
            </li>
            <li>
                Jones, M. (2023).
                <a href="https://doi.org/10.5678/example2"
                   target="_blank"
                   rel="noopener noreferrer">
                    "Coastal Erosion Study" ⧉
                </a>
            </li>
            <li>
                Chen, L. et al. (2024).
                <a href="https://doi.org/10.9012/example3"
                   target="_blank"
                   rel="noopener noreferrer">
                    "Sea Level Rise Projections" ⧉
                </a>
            </li>
        </ol>
    </section>

    <footer style="margin-top: 30pt; padding: 15pt; background-color: #f8f9fa;">
        <p style="font-size: 9pt;">
            <em>
                Note: References marked with ⧉ open in a new window and link to external websites.
            </em>
        </p>
    </footer>
</article>
```

### Download Links

```html
<section>
    <h2>Available Downloads</h2>

    <div style="border: 1pt solid #ccc; padding: 15pt; margin: 10pt 0;">
        <h3>User Manual (PDF)</h3>
        <p>Complete guide to using the product.</p>
        <a href="downloads/user-manual.pdf"
           target="_blank"
           rel="noopener"
           style="display: inline-block; padding: 10pt 20pt;
                  background-color: #336699; color: white;
                  text-decoration: none; border-radius: 3pt;">
            Download Manual (opens in new window)
        </a>
    </div>

    <div style="border: 1pt solid #ccc; padding: 15pt; margin: 10pt 0;">
        <h3>Quick Start Guide (PDF)</h3>
        <p>Get started in minutes.</p>
        <a href="downloads/quick-start.pdf"
           target="_blank"
           rel="noopener"
           style="display: inline-block; padding: 10pt 20pt;
                  background-color: #336699; color: white;
                  text-decoration: none; border-radius: 3pt;">
            Download Guide (opens in new window)
        </a>
    </div>

    <div style="border: 1pt solid #ccc; padding: 15pt; margin: 10pt 0;">
        <h3>Technical Specifications (PDF)</h3>
        <p>Detailed technical information.</p>
        <a href="downloads/tech-specs.pdf"
           target="_blank"
           rel="noopener"
           style="display: inline-block; padding: 10pt 20pt;
                  background-color: #336699; color: white;
                  text-decoration: none; border-radius: 3pt;">
            Download Specs (opens in new window)
        </a>
    </div>
</section>
```

### Help Documentation

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Help Documentation</title>
</head>
<body>
    <h1>Getting Help</h1>

    <section>
        <h2>Internal Help</h2>
        <p>Navigate to different help sections within this document:</p>
        <ul>
            <li><a href="#faq" target="_self">Frequently Asked Questions</a></li>
            <li><a href="#troubleshooting" target="_self">Troubleshooting</a></li>
            <li><a href="#contact" target="_self">Contact Information</a></li>
        </ul>
    </section>

    <section>
        <h2>Online Resources</h2>
        <p>Access additional help resources online:</p>
        <ul>
            <li>
                <a href="https://support.example.com"
                   target="_blank"
                   rel="noopener noreferrer"
                   aria-label="Support Center, opens in new window">
                    Support Center ⧉
                </a>
            </li>
            <li>
                <a href="https://community.example.com"
                   target="_blank"
                   rel="noopener noreferrer"
                   aria-label="Community Forum, opens in new window">
                    Community Forum ⧉
                </a>
            </li>
            <li>
                <a href="https://video-tutorials.example.com"
                   target="_blank"
                   rel="noopener noreferrer"
                   aria-label="Video Tutorials, opens in new window">
                    Video Tutorials ⧉
                </a>
            </li>
        </ul>
    </section>

    <section id="faq">
        <h2>Frequently Asked Questions</h2>
        <p>Common questions and answers...</p>
    </section>

    <section id="troubleshooting">
        <h2>Troubleshooting</h2>
        <p>Solutions to common problems...</p>
    </section>

    <section id="contact">
        <h2>Contact Information</h2>
        <p>
            For additional support, visit our
            <a href="https://example.com/contact"
               target="_blank"
               rel="noopener noreferrer">
                contact page ⧉
            </a>
            or email
            <a href="mailto:support@example.com" target="_self">
                support@example.com
            </a>.
        </p>
    </section>
</body>
</html>
```

### Navigation Menu

```html
<nav style="background-color: #336699; padding: 15pt; margin-bottom: 20pt;">
    <div style="display: flex; justify-content: space-between; align-items: center;">
        <div>
            <!-- Internal navigation -->
            <a href="#home" target="_self"
               style="color: white; margin-right: 15pt; text-decoration: none;">
                Home
            </a>
            <a href="#about" target="_self"
               style="color: white; margin-right: 15pt; text-decoration: none;">
                About
            </a>
            <a href="#services" target="_self"
               style="color: white; margin-right: 15pt; text-decoration: none;">
                Services
            </a>
            <a href="#contact" target="_self"
               style="color: white; text-decoration: none;">
                Contact
            </a>
        </div>
        <div>
            <!-- External link -->
            <a href="https://example.com/login"
               target="_blank"
               rel="noopener noreferrer"
               style="color: white; padding: 8pt 15pt;
                      background-color: #28a745; text-decoration: none;
                      border-radius: 3pt;">
                Login ⧉
            </a>
        </div>
    </div>
</nav>
```

### Product Catalog with External Links

```html
<section>
    <h1>Product Catalog</h1>

    <div style="display: grid; grid-template-columns: repeat(2, 1fr); gap: 20pt;">
        <!-- Product 1 -->
        <article style="border: 1pt solid #ccc; padding: 15pt;">
            <h3>Widget A</h3>
            <p>High-quality widget for all your needs.</p>
            <p><strong>Price:</strong> $99.99</p>
            <p>
                <a href="#widget-a-details" target="_self"
                   style="margin-right: 10pt;">
                    Details
                </a>
                <a href="https://store.example.com/widget-a"
                   target="_blank"
                   rel="noopener noreferrer">
                    Buy Now ⧉
                </a>
            </p>
        </article>

        <!-- Product 2 -->
        <article style="border: 1pt solid #ccc; padding: 15pt;">
            <h3>Widget B</h3>
            <p>Premium widget with advanced features.</p>
            <p><strong>Price:</strong> $149.99</p>
            <p>
                <a href="#widget-b-details" target="_self"
                   style="margin-right: 10pt;">
                    Details
                </a>
                <a href="https://store.example.com/widget-b"
                   target="_blank"
                   rel="noopener noreferrer">
                    Buy Now ⧉
                </a>
            </p>
        </article>
    </div>
</section>
```

### Legal Document

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Terms of Service</title>
</head>
<body>
    <h1>Terms of Service</h1>

    <p>Last Updated: January 15, 2025</p>

    <nav>
        <h2>Table of Contents</h2>
        <ul>
            <li><a href="#section1" target="_self">1. Acceptance of Terms</a></li>
            <li><a href="#section2" target="_self">2. User Obligations</a></li>
            <li><a href="#section3" target="_self">3. Privacy Policy</a></li>
            <li><a href="#section4" target="_self">4. Intellectual Property</a></li>
            <li><a href="#section5" target="_self">5. Disclaimer</a></li>
        </ul>
    </nav>

    <section id="section1">
        <h2>1. Acceptance of Terms</h2>
        <p>
            By accessing or using our service, you agree to be bound by these terms.
            For more information, visit our
            <a href="https://example.com/legal"
               target="_blank"
               rel="noopener noreferrer">
                legal information page ⧉
            </a>.
        </p>
    </section>

    <section id="section3">
        <h2>3. Privacy Policy</h2>
        <p>
            Your privacy is important to us. Please review our
            <a href="https://example.com/privacy"
               target="_blank"
               rel="noopener noreferrer">
                Privacy Policy ⧉
            </a>
            for details on how we collect and use your information.
        </p>
    </section>

    <footer style="margin-top: 40pt; padding: 20pt; border-top: 2pt solid #ccc;">
        <p>
            Questions? Contact our legal team at
            <a href="mailto:legal@example.com" target="_self">
                legal@example.com
            </a>
        </p>
    </footer>
</body>
</html>
```

---

## See Also

- [a](/reference/htmltags/a.html) - Anchor element for hyperlinks
- [href](/reference/htmlattributes/href.html) - Hyperlink reference attribute
- [base](/reference/htmltags/base.html) - Base URL element
- [rel](/reference/htmlattributes/rel.html) - Relationship attribute
- [area](/reference/htmltags/area.html) - Image map area element
- [form](/reference/htmltags/form.html) - Form element

---
