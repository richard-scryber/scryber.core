---
layout: default
title: link
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;link&gt; : The Link Element (External Stylesheets)

The `<link>` element links external resources to the PDF document, primarily used for loading external CSS stylesheets. It enables modular styling by separating CSS into external files that can be reused across multiple documents.

## Usage

The `<link>` element loads external resources that:
- Links external CSS stylesheet files (`.css`) to the document
- Supports both local file paths and remote URLs (HTTP/HTTPS)
- Loads and parses CSS styles during document processing
- Adds parsed styles to the document's style collection
- Supports media queries for conditional styling
- Can load multiple stylesheets in cascade order
- Supports relative and absolute paths
- Must be placed within the `<head>` element

```html
<head>
    <title>Styled Document</title>

    <!-- External stylesheet from local file -->
    <link rel="stylesheet" href="styles/main.css" />

    <!-- External stylesheet from URL -->
    <link rel="stylesheet" href="https://example.com/css/theme.css" />

    <!-- Media-specific stylesheet -->
    <link rel="stylesheet" href="print.css" media="print" />
</head>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for the element. |
| `hidden` | string | Controls visibility. Set to "hidden" to exclude the stylesheet, or omit to include. |

### Link-Specific Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `href` | string | **Required**. The path or URL to the stylesheet file. |
| `rel` | string | **Required**. The relationship type. Use `"stylesheet"` for CSS files. |
| `media` | string | Media query for conditional inclusion (e.g., "print", "all"). |

### Relationship Types

The `rel` attribute defines the resource type:

| Value | Description | Support |
|-------|-------------|---------|
| `stylesheet` | External CSS stylesheet | ✓ Supported |
| `import` | Import HTML content | ✓ Supported (limited) |
| Other values | Various HTML link types | Not supported |

### Media Attribute Values

The `media` attribute controls when the stylesheet is applied:

| Value | Description | Applied |
|-------|-------------|---------|
| `print` | Print media type | ✓ Yes (PDFs are print media) |
| `all` | All media types | ✓ Yes |
| `screen` | Screen media type | No (PDFs are not screen) |
| Not specified | Defaults to all media | ✓ Yes |

---

## Notes

### Loading and Processing

The `<link>` element loads external stylesheets through the following process:

1. **Registration**: Registers a remote file request with the document
2. **Loading**: Fetches the file from local path or remote URL
3. **Caching**: Caches the content for improved performance
4. **Parsing**: Parses CSS selectors and rules
5. **Adding**: Adds parsed styles to document style collection
6. **Application**: Applies styles during layout phase

This process occurs during the data binding or pre-layout phase, ensuring styles are available when content is rendered.

### Path Resolution

The `href` attribute supports multiple path formats:

**Relative Paths**:
```html
<!-- Relative to document location -->
<link rel="stylesheet" href="styles/main.css" />
<link rel="stylesheet" href="../shared/theme.css" />
```

**Absolute Paths**:
```html
<!-- Absolute local path -->
<link rel="stylesheet" href="/var/www/styles/main.css" />
<!-- Windows absolute path -->
<link rel="stylesheet" href="C:/styles/main.css" />
```

**URLs**:
```html
<!-- HTTP/HTTPS URLs -->
<link rel="stylesheet" href="https://example.com/css/style.css" />
<link rel="stylesheet" href="http://cdn.example.com/bootstrap.css" />
```

**Base Path Resolution**:
If a `<base>` element is present, relative paths resolve from the base:
```html
<head>
    <base href="https://example.com/documents/" />
    <link rel="stylesheet" href="css/style.css" />
    <!-- Resolves to: https://example.com/documents/css/style.css -->
</head>
```

### Cascade Order

Multiple stylesheets are applied in cascade order (top to bottom):

```html
<head>
    <!-- Applied first (lowest priority) -->
    <link rel="stylesheet" href="reset.css" />

    <!-- Applied second -->
    <link rel="stylesheet" href="layout.css" />

    <!-- Applied third -->
    <link rel="stylesheet" href="theme.css" />

    <!-- Applied last (highest priority) -->
    <link rel="stylesheet" href="overrides.css" />
</head>
```

Later stylesheets override earlier ones for conflicting rules, following standard CSS cascade rules.

### Caching

External stylesheets are cached to improve performance:

- **Cache Duration**: Uses default cache duration (configurable)
- **Cache Key**: Based on the resolved file path/URL
- **Revalidation**: Cached content is reused across document renders
- **Benefits**: Reduces network requests and file I/O operations

### Media Queries

The `media` attribute filters stylesheet inclusion based on output format:

```html
<!-- Included in PDF (print media) -->
<link rel="stylesheet" href="print.css" media="print" />

<!-- Included in PDF (all media) -->
<link rel="stylesheet" href="all.css" media="all" />

<!-- NOT included in PDF (screen only) -->
<link rel="stylesheet" href="screen.css" media="screen" />
```

PDFs are considered **print media**, so stylesheets with `media="print"` or `media="all"` are included.

### Error Handling

Loading errors are handled based on parser conformance mode:

**Lax Mode** (default):
- Logs errors and continues processing
- Missing files result in warnings
- Invalid CSS is logged but doesn't stop rendering

**Strict Mode**:
- Throws exceptions on loading errors
- Missing files cause document generation to fail
- Invalid CSS causes parsing errors

### Class Hierarchy

In the Scryber codebase:
- `HTMLLink` extends `Component` implements `ITemplate`, `ILoadableComponent`
- Uses `RemoteFileRequest` for async loading
- Contains `LinkContentBase` for parsed content
- `LinkContentCSS` holds parsed stylesheet collection
- Supports lazy loading and deferred parsing

### Performance Considerations

1. **Load Time**: External files add loading time
2. **Caching**: Use caching to minimize repeated loads
3. **File Size**: Large CSS files increase processing time
4. **Remote URLs**: Network latency affects load time
5. **Parsing**: Complex CSS requires more parsing time

**Best Practices**:
- Minimize the number of external stylesheets
- Combine multiple CSS files when possible
- Use local files instead of remote URLs when feasible
- Keep CSS files reasonably sized and optimized

---

## Examples

### Basic External Stylesheet

```html
<!DOCTYPE html>
<html>
<head>
    <title>Styled Document</title>
    <link rel="stylesheet" href="styles.css" />
</head>
<body>
    <h1 class="title">Hello World</h1>
    <p class="content">This document uses external styles.</p>
</body>
</html>
```

**styles.css**:
```css
.title {
    color: #336699;
    font-size: 24pt;
    border-bottom: 2pt solid #336699;
}

.content {
    font-family: Arial;
    font-size: 12pt;
    line-height: 1.6;
}
```

### Multiple Stylesheets in Cascade

```html
<!DOCTYPE html>
<html>
<head>
    <title>Multi-Style Document</title>

    <!-- Reset styles (lowest priority) -->
    <link rel="stylesheet" href="css/reset.css" />

    <!-- Base layout -->
    <link rel="stylesheet" href="css/layout.css" />

    <!-- Typography -->
    <link rel="stylesheet" href="css/typography.css" />

    <!-- Theme colors -->
    <link rel="stylesheet" href="css/theme.css" />

    <!-- Document-specific overrides (highest priority) -->
    <link rel="stylesheet" href="css/custom.css" />
</head>
<body>
    <!-- Content uses styles from all files -->
</body>
</html>
```

### Remote Stylesheet from CDN

```html
<!DOCTYPE html>
<html>
<head>
    <title>Document with CDN Styles</title>

    <!-- Load from remote CDN -->
    <link rel="stylesheet" href="https://cdn.example.com/css/corporate-styles.css" />

    <!-- Local overrides -->
    <style>
        /* Override CDN styles as needed */
        body {
            margin: 20pt;
        }
    </style>
</head>
<body>
    <h1>Corporate Document</h1>
</body>
</html>
```

### Media-Specific Stylesheets

```html
<!DOCTYPE html>
<html>
<head>
    <title>Media-Aware Document</title>

    <!-- Always included (no media attribute) -->
    <link rel="stylesheet" href="base.css" />

    <!-- Included for PDF (print media) -->
    <link rel="stylesheet" href="print.css" media="print" />

    <!-- Included for PDF (all media) -->
    <link rel="stylesheet" href="universal.css" media="all" />

    <!-- NOT included for PDF (screen only) -->
    <link rel="stylesheet" href="screen.css" media="screen" />
</head>
<body>
    <p>This document uses print-appropriate styles.</p>
</body>
</html>
```

### Stylesheet with Base Path

```html
<!DOCTYPE html>
<html>
<head>
    <title>Document with Base Path</title>

    <!-- Set base path for all resources -->
    <base href="https://example.com/templates/v2/" />

    <!-- These resolve relative to base path -->
    <link rel="stylesheet" href="css/reset.css" />
    <!-- https://example.com/templates/v2/css/reset.css -->

    <link rel="stylesheet" href="css/layout.css" />
    <!-- https://example.com/templates/v2/css/layout.css -->

    <link rel="stylesheet" href="css/theme.css" />
    <!-- https://example.com/templates/v2/css/theme.css -->
</head>
<body>
    <!-- Content -->
</body>
</html>
```

### Corporate Template with Modular Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Corporate Report</title>

    <!-- Corporate brand styles from shared location -->
    <base href="https://corporate.example.com/brand/" />

    <link rel="stylesheet" href="styles/corporate-reset.css" />
    <link rel="stylesheet" href="styles/corporate-typography.css" />
    <link rel="stylesheet" href="styles/corporate-colors.css" />
    <link rel="stylesheet" href="styles/corporate-layout.css" />

    <!-- Department-specific styles -->
    <link rel="stylesheet" href="https://hr.example.com/styles/hr-theme.css" />
</head>
<body>
    <div class="corporate-header">
        <h1 class="corporate-title">HR Annual Report</h1>
    </div>
</body>
</html>
```

### Invoice Template with External Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Invoice {{model.invoiceNumber}}</title>

    <!-- Shared invoice styles -->
    <link rel="stylesheet" href="/templates/invoice/base.css" />
    <link rel="stylesheet" href="/templates/invoice/tables.css" />

    <!-- Dynamic theme based on client -->
    <link rel="stylesheet" href="{{model.clientThemeUrl}}" />
</head>
<body class="invoice">
    <div class="invoice-header">
        <h1>Invoice</h1>
    </div>
    <!-- Invoice content -->
</body>
</html>
```

### Multi-Page Document with Section Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Technical Manual</title>

    <!-- Common styles for all sections -->
    <link rel="stylesheet" href="styles/manual-common.css" />

    <!-- Section-specific styles -->
    <link rel="stylesheet" href="styles/introduction.css" />
    <link rel="stylesheet" href="styles/code-examples.css" />
    <link rel="stylesheet" href="styles/diagrams.css" />
    <link rel="stylesheet" href="styles/reference.css" />
</head>
<body>
    <section class="introduction">
        <!-- Introduction content -->
    </section>

    <section class="code-examples">
        <!-- Code examples -->
    </section>
</body>
</html>
```

### Conditional Stylesheet Loading

```html
<!DOCTYPE html>
<html>
<head>
    <title>Conditional Styles</title>

    <!-- Always load base styles -->
    <link rel="stylesheet" href="base.css" />

    <!-- Conditionally hide theme stylesheet -->
    <link rel="stylesheet" href="dark-theme.css" hidden="{{model.useLightTheme ? 'hidden' : ''}}" />
    <link rel="stylesheet" href="light-theme.css" hidden="{{model.useLightTheme ? '' : 'hidden'}}" />
</head>
<body>
    <h1>Themed Document</h1>
</body>
</html>
```

### Print-Optimized Stylesheets

```html
<!DOCTYPE html>
<html>
<head>
    <title>Print-Optimized Report</title>

    <!-- Base styles -->
    <link rel="stylesheet" href="base.css" />

    <!-- Print-specific optimizations -->
    <link rel="stylesheet" href="print-layout.css" media="print" />
    <link rel="stylesheet" href="print-colors.css" media="print" />
    <link rel="stylesheet" href="print-fonts.css" media="print" />
</head>
<body>
    <h1>Report</h1>
    <p>Optimized for print output.</p>
</body>
</html>
```

### Newsletter with External Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Monthly Newsletter</title>
    <meta name="author" content="Marketing" />

    <!-- Newsletter framework -->
    <link rel="stylesheet" href="https://cdn.example.com/newsletter/v2/base.css" />
    <link rel="stylesheet" href="https://cdn.example.com/newsletter/v2/grid.css" />
    <link rel="stylesheet" href="https://cdn.example.com/newsletter/v2/components.css" />

    <!-- Custom branding -->
    <link rel="stylesheet" href="custom/newsletter-brand.css" />
</head>
<body class="newsletter">
    <div class="newsletter-header">
        <h1>October 2025 Newsletter</h1>
    </div>
    <!-- Newsletter content -->
</body>
</html>
```

### Academic Paper with Citation Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Research Paper</title>

    <!-- Academic formatting -->
    <link rel="stylesheet" href="styles/academic-base.css" />
    <link rel="stylesheet" href="styles/academic-typography.css" />

    <!-- Citation style (APA, MLA, Chicago, etc.) -->
    <link rel="stylesheet" href="styles/citations/apa.css" />

    <!-- Figure and table styles -->
    <link rel="stylesheet" href="styles/figures-tables.css" />
</head>
<body class="academic-paper">
    <section class="abstract">
        <h1>Abstract</h1>
        <p>This paper presents...</p>
    </section>
</body>
</html>
```

### Financial Report with Table Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Q3 Financial Report</title>

    <!-- Financial report styles -->
    <link rel="stylesheet" href="styles/financial-base.css" />
    <link rel="stylesheet" href="styles/financial-tables.css" />
    <link rel="stylesheet" href="styles/financial-charts.css" />
    <link rel="stylesheet" href="styles/financial-typography.css" />
</head>
<body class="financial-report">
    <h1>Q3 2025 Financial Report</h1>
    <!-- Financial tables and content -->
</body>
</html>
```

### Product Catalog with Grid Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Product Catalog 2025</title>

    <!-- Catalog framework -->
    <link rel="stylesheet" href="catalog/reset.css" />
    <link rel="stylesheet" href="catalog/grid-system.css" />
    <link rel="stylesheet" href="catalog/product-cards.css" />
    <link rel="stylesheet" href="catalog/pricing.css" />
    <link rel="stylesheet" href="catalog/images.css" />
</head>
<body class="catalog">
    <div class="product-grid">
        <!-- Product items -->
    </div>
</body>
</html>
```

### Legal Document with Specialized Formatting

```html
<!DOCTYPE html>
<html>
<head>
    <title>Service Agreement</title>

    <!-- Legal document styles -->
    <link rel="stylesheet" href="legal/base.css" />
    <link rel="stylesheet" href="legal/sections.css" />
    <link rel="stylesheet" href="legal/clauses.css" />
    <link rel="stylesheet" href="legal/signatures.css" />
</head>
<body class="legal-document">
    <h1>Service Agreement</h1>
    <section class="clause">
        <h2>1. Terms and Conditions</h2>
        <!-- Legal text -->
    </section>
</body>
</html>
```

### Presentation/Slides with External Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Business Presentation</title>

    <!-- Presentation framework -->
    <link rel="stylesheet" href="presentation/base.css" />
    <link rel="stylesheet" href="presentation/slides.css" />
    <link rel="stylesheet" href="presentation/transitions.css" />

    <!-- Theme -->
    <link rel="stylesheet" href="presentation/themes/corporate-blue.css" />
</head>
<body class="presentation">
    <section class="slide title-slide">
        <h1>Q4 Business Review</h1>
    </section>
    <section class="slide">
        <h2>Agenda</h2>
        <!-- Slide content -->
    </section>
</body>
</html>
```

### Wedding Invitation with Elegant Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Wedding Invitation</title>

    <!-- Elegant styling -->
    <link rel="stylesheet" href="https://fonts-cdn.example.com/elegant-fonts.css" />
    <link rel="stylesheet" href="invitations/wedding-base.css" />
    <link rel="stylesheet" href="invitations/wedding-ornaments.css" />
    <link rel="stylesheet" href="invitations/wedding-typography.css" />
</head>
<body class="wedding-invitation">
    <div class="invitation-card">
        <h1 class="couple-names">Jane & John</h1>
        <p class="invitation-text">Request the pleasure of your company...</p>
    </div>
</body>
</html>
```

---

## See Also

- [style](/reference/htmltags/style.html) - Style element for embedded CSS
- [head](/reference/htmltags/head.html) - Head element (container for link elements)
- [base](/reference/htmltags/base.html) - Base element for URL resolution
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [CSS Selectors](/reference/styles/selectors.html) - CSS selector syntax
- [Style Collection](/reference/styles/collection.html) - Document style collection
- [Remote Files](/reference/remote/) - Remote file loading and caching
- [Media Queries](/reference/styles/media.html) - Media query support
- [Performance](/reference/performance/) - Performance optimization

---
