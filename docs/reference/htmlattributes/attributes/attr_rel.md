---
layout: default
title: rel
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @rel : The Relationship Attribute

The `rel` attribute specifies the relationship between the current document and a linked resource. Used primarily with `<link>` elements, it defines resource types such as stylesheets, icons, alternate versions, and preload hints. In PDF generation, it's essential for linking CSS files and defining document metadata relationships.

## Usage

The `rel` attribute defines resource relationships:
- Specifies the type of linked resource in `<link>` elements
- Most commonly used with `stylesheet` value for CSS files
- Defines document relationships like `alternate`, `icon`, `canonical`
- Supports multiple relationship types (space-separated)
- Critical for proper stylesheet loading in PDF generation
- Supports data binding for dynamic relationship specification

```html
<!-- Stylesheet (most common for PDF) -->
<link rel="stylesheet" href="styles.css" />

<!-- Icon/favicon -->
<link rel="icon" href="favicon.ico" />

<!-- Alternate language version -->
<link rel="alternate" hreflang="es" href="document-es.pdf" />

<!-- Canonical URL -->
<link rel="canonical" href="https://example.com/document" />

<!-- Multiple relationships -->
<link rel="stylesheet alternate" href="alternate-styles.css" title="Alternate Theme" />

<!-- Dynamic rel -->
<link rel="{{model.linkType}}" href="{{model.resourceUrl}}" />
```

---

## Supported Elements

The `rel` attribute is used with:

### Link Element
- `<link>` - Defines relationship to external resources (primary use)

### Anchor Element
- `<a>` - Defines relationship for navigation links (limited PDF use)

### Area Element
- `<area>` - Defines relationship for image map areas (rare in PDF)

---

## Binding Values

The `rel` attribute supports data binding for dynamic relationship types:

```html
<!-- Dynamic relationship type -->
<link rel="{{model.relationType}}" href="{{model.resourcePath}}" />

<!-- Conditional stylesheet loading -->
<link rel="{{model.useStylesheet ? 'stylesheet' : 'alternate stylesheet'}}"
      href="theme.css" />

<!-- Dynamic icon -->
<link rel="icon" type="{{model.iconType}}" href="{{model.iconUrl}}" />

<!-- Repeating link relationships -->
<template data-bind="{{model.resources}}">
    <link rel="{{.relationship}}" href="{{.url}}" type="{{.type}}" />
</template>

<!-- Multiple alternates -->
<template data-bind="{{model.alternates}}">
    <link rel="alternate" hreflang="{{.lang}}" href="{{.url}}" />
</template>
```

**Data Model Example:**
```json
{
  "relationType": "stylesheet",
  "resourcePath": "custom.css",
  "useStylesheet": true,
  "iconType": "image/png",
  "iconUrl": "icon.png",
  "resources": [
    {
      "relationship": "stylesheet",
      "url": "base.css",
      "type": "text/css"
    },
    {
      "relationship": "icon",
      "url": "favicon.ico",
      "type": "image/x-icon"
    }
  ],
  "alternates": [
    {
      "lang": "es",
      "url": "document-es.pdf"
    },
    {
      "lang": "fr",
      "url": "document-fr.pdf"
    }
  ]
}
```

---

## Notes

### Common Relationship Types

Standard `rel` values for PDF generation:

#### stylesheet
The most important for PDF generation:

```html
<!-- Standard stylesheet -->
<link rel="stylesheet" href="styles.css" />

<!-- Multiple stylesheets -->
<link rel="stylesheet" href="base.css" />
<link rel="stylesheet" href="theme.css" />
<link rel="stylesheet" href="print.css" media="print" />

<!-- With media query -->
<link rel="stylesheet" href="mobile.css" media="screen and (max-width: 600px)" />
```

#### icon
Document icon/favicon:

```html
<!-- Standard favicon -->
<link rel="icon" href="favicon.ico" />

<!-- PNG icon -->
<link rel="icon" type="image/png" href="icon.png" />

<!-- Multiple sizes -->
<link rel="icon" type="image/png" sizes="16x16" href="icon-16.png" />
<link rel="icon" type="image/png" sizes="32x32" href="icon-32.png" />
<link rel="icon" type="image/png" sizes="48x48" href="icon-48.png" />

<!-- Apple touch icon -->
<link rel="apple-touch-icon" href="apple-icon.png" />
```

#### alternate
Alternate versions of the document:

```html
<!-- Alternate language versions -->
<link rel="alternate" hreflang="es" href="document-es.pdf" />
<link rel="alternate" hreflang="fr" href="document-fr.pdf" />
<link rel="alternate" hreflang="de" href="document-de.pdf" />

<!-- Alternate format -->
<link rel="alternate" type="application/pdf" href="document.pdf" />
<link rel="alternate" type="text/html" href="document.html" />

<!-- RSS/Atom feed -->
<link rel="alternate" type="application/rss+xml" href="feed.rss" />
```

#### canonical
Canonical/preferred URL:

```html
<!-- Canonical URL -->
<link rel="canonical" href="https://example.com/document" />
```

#### author
Link to author information:

```html
<!-- Author page -->
<link rel="author" href="https://example.com/author/john-doe" />

<!-- Author contact -->
<link rel="author" href="mailto:author@example.com" />
```

#### license
Copyright/license information:

```html
<!-- License document -->
<link rel="license" href="https://example.com/license" />

<!-- Creative Commons -->
<link rel="license" href="https://creativecommons.org/licenses/by/4.0/" />
```

#### help
Help documentation:

```html
<!-- Help page -->
<link rel="help" href="https://example.com/help" />

<!-- Context-specific help -->
<link rel="help" href="https://example.com/help/section1" />
```

### Multiple Relationships

Combine multiple relationships (space-separated):

```html
<!-- Alternate stylesheet -->
<link rel="stylesheet alternate" href="high-contrast.css" title="High Contrast" />

<!-- Icon and shortcut icon -->
<link rel="icon shortcut icon" href="favicon.ico" />
```

### Stylesheet Specifics

Important for PDF styling:

```html
<!-- Persistent stylesheet (always applied) -->
<link rel="stylesheet" href="base.css" />

<!-- Preferred stylesheet (default) -->
<link rel="stylesheet" href="default-theme.css" title="Default" />

<!-- Alternate stylesheet (user-selectable) -->
<link rel="alternate stylesheet" href="dark-theme.css" title="Dark" />
<link rel="alternate stylesheet" href="large-text.css" title="Large Text" />
```

**In PDF Context:**
- Use `rel="stylesheet"` without `title` for always-applied styles
- Alternate stylesheets may not be supported in PDF generation
- Focus on `rel="stylesheet"` for primary styling

### Case Sensitivity

The `rel` attribute values are case-insensitive:

```html
<!-- All equivalent -->
<link rel="stylesheet" href="styles.css" />
<link rel="STYLESHEET" href="styles.css" />
<link rel="StyleSheet" href="styles.css" />
```

**Best Practice:** Use lowercase for consistency.

### Required vs Optional

For `<link>` elements:
- `rel` attribute is **required**
- `href` attribute is usually required (except for some rel values)
- Other attributes like `type`, `media`, `title` are optional

```html
<!-- Valid: has both rel and href -->
<link rel="stylesheet" href="styles.css" />

<!-- Invalid: missing rel -->
<link href="styles.css" />

<!-- Invalid: missing href (for stylesheet) -->
<link rel="stylesheet" />
```

### Deprecated Relationships

Some rel values are deprecated or rarely used:

```html
<!-- Deprecated/Rarely used -->
<link rel="prev" href="page1.html" />  <!-- Previous page -->
<link rel="next" href="page3.html" />  <!-- Next page -->
<link rel="chapter" href="chapter1.html" />  <!-- Document chapter -->
<link rel="section" href="section1.html" />  <!-- Document section -->
<link rel="subsection" href="subsection1.html" />  <!-- Document subsection -->
<link rel="appendix" href="appendix.html" />  <!-- Document appendix -->
```

### PDF-Specific Usage

For PDF generation with Scryber:

1. **Primary use: `stylesheet`** for linking CSS files
2. **Use `icon`** for document metadata (may appear in PDF properties)
3. **Use `alternate`** to reference related documents
4. **Use `author`, `license`** for document metadata
5. **Avoid complex navigation rel values** (prev, next, etc.)

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>PDF Document</title>

    <!-- Essential for PDF styling -->
    <link rel="stylesheet" href="base.css" />
    <link rel="stylesheet" href="print.css" media="print" />

    <!-- Metadata -->
    <link rel="icon" href="company-logo.png" />
    <link rel="author" href="https://example.com/authors/john-doe" />
    <link rel="license" href="https://example.com/license" />

    <!-- Alternate versions -->
    <link rel="alternate" hreflang="es" href="document-es.pdf" />
</head>
<body>
    <h1>Document Content</h1>
</body>
</html>
```

### Anchor Element Usage

The `rel` attribute on `<a>` elements (limited PDF relevance):

```html
<!-- Link relationship types -->
<a href="https://example.com" rel="external">External Site</a>
<a href="page2.html" rel="next">Next Page</a>
<a href="page0.html" rel="prev">Previous Page</a>
<a href="author.html" rel="author">About the Author</a>
<a href="https://untrusted.com" rel="nofollow">No Follow Link</a>
<a href="https://example.com" rel="noopener noreferrer">Secure Link</a>
```

**Note:** Most anchor `rel` values are for web browsers and have limited effect in PDFs.

### Preload and Prefetch

For web optimization (limited PDF use):

```html
<!-- Preload resources -->
<link rel="preload" href="font.woff2" as="font" type="font/woff2" crossorigin />
<link rel="preload" href="image.jpg" as="image" />
<link rel="preload" href="styles.css" as="style" />

<!-- DNS prefetch -->
<link rel="dns-prefetch" href="https://cdn.example.com" />

<!-- Preconnect -->
<link rel="preconnect" href="https://fonts.googleapis.com" />
```

**Note:** These optimization hints are primarily for browsers, not PDFs.

---

## Examples

### Basic Stylesheet Linking

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Document with Styles</title>

    <!-- Link to external stylesheet -->
    <link rel="stylesheet" href="styles.css" />
</head>
<body>
    <h1>Styled Document</h1>
    <p>This document uses an external stylesheet.</p>
</body>
</html>
```

### Multiple Stylesheets

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Multi-Stylesheet Document</title>

    <!-- Base styles -->
    <link rel="stylesheet" href="reset.css" />
    <link rel="stylesheet" href="base.css" />

    <!-- Layout styles -->
    <link rel="stylesheet" href="layout.css" />

    <!-- Component styles -->
    <link rel="stylesheet" href="components.css" />

    <!-- Print-specific styles -->
    <link rel="stylesheet" href="print.css" media="print" />

    <!-- Theme -->
    <link rel="stylesheet" href="theme-blue.css" />
</head>
<body>
    <h1>Document with Multiple Stylesheets</h1>
    <p>Styles are loaded from multiple CSS files in order.</p>
</body>
</html>
```

### Document with Metadata Links

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Technical Report 2025</title>

    <!-- Styles -->
    <link rel="stylesheet" href="report.css" />

    <!-- Document icon -->
    <link rel="icon" type="image/png" href="report-icon.png" />

    <!-- Author information -->
    <link rel="author" href="https://example.com/authors/jane-smith" />

    <!-- License -->
    <link rel="license" href="https://creativecommons.org/licenses/by/4.0/" />

    <!-- Canonical URL -->
    <link rel="canonical" href="https://example.com/reports/2025/technical-report" />

    <!-- Alternate language versions -->
    <link rel="alternate" hreflang="es" href="technical-report-es.pdf" />
    <link rel="alternate" hreflang="fr" href="technical-report-fr.pdf" />
    <link rel="alternate" hreflang="de" href="technical-report-de.pdf" />
</head>
<body>
    <h1>Technical Report 2025</h1>
    <p>Comprehensive analysis of technical developments.</p>
</body>
</html>
```

### Responsive Stylesheets

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Responsive Document</title>

    <!-- Base styles for all media -->
    <link rel="stylesheet" href="base.css" media="all" />

    <!-- Print styles -->
    <link rel="stylesheet" href="print.css" media="print" />

    <!-- Portrait orientation -->
    <link rel="stylesheet" href="portrait.css"
          media="print and (orientation: portrait)" />

    <!-- Landscape orientation -->
    <link rel="stylesheet" href="landscape.css"
          media="print and (orientation: landscape)" />

    <!-- A4 paper size -->
    <link rel="stylesheet" href="a4.css"
          media="print and (width: 210mm)" />

    <!-- Letter paper size -->
    <link rel="stylesheet" href="letter.css"
          media="print and (width: 8.5in)" />
</head>
<body>
    <h1>Responsive Layout</h1>
    <p>Different stylesheets apply based on output characteristics.</p>
</body>
</html>
```

### Icon Variations

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Document with Icons</title>

    <link rel="stylesheet" href="styles.css" />

    <!-- Standard favicon -->
    <link rel="icon" href="favicon.ico" />

    <!-- PNG icons in multiple sizes -->
    <link rel="icon" type="image/png" sizes="16x16" href="favicon-16x16.png" />
    <link rel="icon" type="image/png" sizes="32x32" href="favicon-32x32.png" />
    <link rel="icon" type="image/png" sizes="48x48" href="favicon-48x48.png" />
    <link rel="icon" type="image/png" sizes="64x64" href="favicon-64x64.png" />

    <!-- Apple touch icon -->
    <link rel="apple-touch-icon" sizes="180x180" href="apple-touch-icon.png" />

    <!-- Safari pinned tab icon -->
    <link rel="mask-icon" href="safari-pinned-tab.svg" color="#336699" />
</head>
<body>
    <h1>Document with Multiple Icon Formats</h1>
</body>
</html>
```

### Alternate Versions

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <title>Multi-Language Document</title>

    <link rel="stylesheet" href="styles.css" />

    <!-- Current version (English) -->
    <link rel="canonical" href="https://example.com/document-en.pdf" />

    <!-- Alternate language versions -->
    <link rel="alternate" hreflang="es" href="document-es.pdf"
          title="Spanish version" />
    <link rel="alternate" hreflang="fr" href="document-fr.pdf"
          title="French version" />
    <link rel="alternate" hreflang="de" href="document-de.pdf"
          title="German version" />
    <link rel="alternate" hreflang="it" href="document-it.pdf"
          title="Italian version" />
    <link rel="alternate" hreflang="pt" href="document-pt.pdf"
          title="Portuguese version" />
    <link rel="alternate" hreflang="ja" href="document-ja.pdf"
          title="Japanese version" />
    <link rel="alternate" hreflang="zh" href="document-zh.pdf"
          title="Chinese version" />

    <!-- Alternate formats -->
    <link rel="alternate" type="text/html" href="document.html"
          title="HTML version" />
    <link rel="alternate" type="application/epub+zip" href="document.epub"
          title="EPUB version" />
</head>
<body>
    <h1>Document Available in Multiple Languages</h1>

    <section>
        <h2>Translations</h2>
        <ul>
            <li><a href="document-es.pdf">Español (Spanish)</a></li>
            <li><a href="document-fr.pdf">Français (French)</a></li>
            <li><a href="document-de.pdf">Deutsch (German)</a></li>
            <li><a href="document-it.pdf">Italiano (Italian)</a></li>
            <li><a href="document-pt.pdf">Português (Portuguese)</a></li>
            <li><a href="document-ja.pdf">日本語 (Japanese)</a></li>
            <li><a href="document-zh.pdf">中文 (Chinese)</a></li>
        </ul>
    </section>
</body>
</html>
```

### License and Copyright

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Licensed Content</title>

    <link rel="stylesheet" href="styles.css" />

    <!-- Author information -->
    <link rel="author" href="https://example.com/author/john-doe" />

    <!-- Creative Commons license -->
    <link rel="license" href="https://creativecommons.org/licenses/by-nc-sa/4.0/" />

    <!-- Help documentation -->
    <link rel="help" href="https://example.com/help/about-licenses" />
</head>
<body>
    <h1>Creative Commons Licensed Document</h1>

    <article>
        <p>This work is licensed under Creative Commons Attribution-NonCommercial-ShareAlike 4.0.</p>
    </article>

    <footer style="margin-top: 40pt; padding: 20pt; background-color: #f8f9fa;">
        <p>
            <strong>License:</strong>
            <a href="https://creativecommons.org/licenses/by-nc-sa/4.0/">
                CC BY-NC-SA 4.0
            </a>
        </p>
        <p>
            <strong>Author:</strong>
            <a href="https://example.com/author/john-doe">John Doe</a>
        </p>
    </footer>
</body>
</html>
```

### Data-Bound Stylesheets

```html
<!-- Model: {
    theme: "blue",
    stylesheets: [
        { rel: "stylesheet", href: "base.css", media: "all" },
        { rel: "stylesheet", href: "blue-theme.css", media: "print" },
        { rel: "stylesheet", href: "print.css", media: "print" }
    ]
} -->

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Dynamic Stylesheets</title>

    <!-- Dynamic stylesheet loading -->
    <template data-bind="{{model.stylesheets}}">
        <link rel="{{.rel}}" href="{{.href}}" media="{{.media}}" />
    </template>

    <!-- Dynamic theme -->
    <link rel="stylesheet" href="themes/{{model.theme}}-theme.css" />
</head>
<body>
    <h1>Document with Dynamic Styling</h1>
    <p>Current theme: {{model.theme}}</p>
</body>
</html>
```

### Corporate Document Template

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Corporate Report Template</title>

    <!-- Corporate stylesheets -->
    <link rel="stylesheet" href="corporate/reset.css" />
    <link rel="stylesheet" href="corporate/base.css" />
    <link rel="stylesheet" href="corporate/typography.css" />
    <link rel="stylesheet" href="corporate/layout.css" />
    <link rel="stylesheet" href="corporate/components.css" />
    <link rel="stylesheet" href="corporate/print.css" media="print" />
    <link rel="stylesheet" href="corporate/brand-colors.css" />

    <!-- Company branding -->
    <link rel="icon" type="image/png" href="corporate/logo-icon.png" />

    <!-- Document metadata -->
    <link rel="author" href="https://company.com/about" />
    <link rel="license" href="https://company.com/legal/copyright" />
    <link rel="canonical" href="https://company.com/reports/2025/annual-report" />

    <!-- Help and support -->
    <link rel="help" href="https://company.com/support" />
</head>
<body>
    <header>
        <h1>Annual Report 2025</h1>
        <p>Company Name Inc.</p>
    </header>

    <main>
        <section>
            <h2>Executive Summary</h2>
            <p>Report content...</p>
        </section>
    </main>

    <footer>
        <p>© 2025 Company Name Inc. All rights reserved.</p>
    </footer>
</body>
</html>
```

### Academic Paper

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Research Paper: PDF Generation Techniques</title>

    <!-- Academic formatting styles -->
    <link rel="stylesheet" href="academic/base.css" />
    <link rel="stylesheet" href="academic/citations.css" />
    <link rel="stylesheet" href="academic/figures.css" />
    <link rel="stylesheet" href="academic/print.css" media="print" />

    <!-- Author information -->
    <link rel="author" href="https://university.edu/faculty/jane-smith" />

    <!-- License (Open Access) -->
    <link rel="license" href="https://creativecommons.org/licenses/by/4.0/" />

    <!-- DOI/Canonical -->
    <link rel="canonical" href="https://doi.org/10.1234/example.2025.001" />

    <!-- Supplementary materials -->
    <link rel="related" href="supplementary-data.pdf" title="Supplementary Data" />
</head>
<body>
    <article>
        <header>
            <h1>PDF Generation Techniques: A Comprehensive Analysis</h1>
            <p>
                <strong>Author:</strong> Dr. Jane Smith<br/>
                <strong>Institution:</strong> University of Technology<br/>
                <strong>Published:</strong> January 2025
            </p>
        </header>

        <section>
            <h2>Abstract</h2>
            <p>This paper examines...</p>
        </section>

        <section>
            <h2>Introduction</h2>
            <p>Content...</p>
        </section>
    </article>
</body>
</html>
```

### Product Catalog

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Product Catalog 2025</title>

    <!-- Catalog styles -->
    <link rel="stylesheet" href="catalog/base.css" />
    <link rel="stylesheet" href="catalog/grid.css" />
    <link rel="stylesheet" href="catalog/products.css" />
    <link rel="stylesheet" href="catalog/pricing.css" />
    <link rel="stylesheet" href="catalog/print-optimized.css" media="print" />

    <!-- Branding -->
    <link rel="icon" href="catalog/brand-icon.png" />

    <!-- Alternate versions -->
    <link rel="alternate" hreflang="es" href="catalog-2025-es.pdf" />
    <link rel="alternate" hreflang="fr" href="catalog-2025-fr.pdf" />

    <!-- Related documents -->
    <link rel="related" href="price-list-2025.pdf" title="Price List" />
    <link rel="related" href="ordering-guide-2025.pdf" title="Ordering Guide" />
</head>
<body>
    <header>
        <h1>Product Catalog 2025</h1>
    </header>

    <main>
        <section class="product-grid">
            <!-- Product listings -->
        </section>
    </main>
</body>
</html>
```

### Newsletter Template

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Monthly Newsletter - January 2025</title>

    <!-- Newsletter styles -->
    <link rel="stylesheet" href="newsletter/base.css" />
    <link rel="stylesheet" href="newsletter/layout.css" />
    <link rel="stylesheet" href="newsletter/typography.css" />
    <link rel="stylesheet" href="newsletter/print.css" media="print" />

    <!-- Newsletter icon -->
    <link rel="icon" href="newsletter/icon.png" />

    <!-- Archive links -->
    <link rel="prev" href="newsletter-2024-12.pdf" title="December 2024" />
    <link rel="next" href="newsletter-2025-02.pdf" title="February 2025" />

    <!-- Subscribe link -->
    <link rel="related" href="https://example.com/subscribe" title="Subscribe" />
</head>
<body>
    <header>
        <h1>Monthly Newsletter</h1>
        <p>January 2025 • Volume 5, Issue 1</p>
    </header>

    <main>
        <article>
            <h2>Feature Article</h2>
            <p>Newsletter content...</p>
        </article>
    </main>

    <footer>
        <p>
            <a href="newsletter-2024-12.pdf">← Previous Issue</a> |
            <a href="newsletter-2025-02.pdf">Next Issue →</a>
        </p>
    </footer>
</body>
</html>
```

---

## See Also

- [link](/reference/htmltags/link.html) - Link element for external resources
- [href](/reference/htmlattributes/href.html) - Hyperlink reference attribute
- [media](/reference/htmlattributes/media.html) - Media query attribute
- [type](/reference/htmlattributes/type.html) - MIME type attribute
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [a](/reference/htmltags/a.html) - Anchor element

---
