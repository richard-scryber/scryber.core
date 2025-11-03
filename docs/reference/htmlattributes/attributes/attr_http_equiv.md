---
layout: default
title: http-equiv
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @http-equiv : The HTTP Header Equivalent Attribute

The `http-equiv` attribute provides an HTTP header equivalent for HTML `<meta>` elements. In web contexts, it instructs browsers to behave as if a specific HTTP header was received. In PDF generation, it is primarily informational and maintains HTML compatibility.

## Usage

The `http-equiv` attribute is used with `<meta>` elements to:
- Declare document character encoding (though `charset` is preferred)
- Specify content type information
- Provide HTTP-header-like directives
- Maintain web-to-PDF conversion compatibility
- Document intended HTTP behavior

```html
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
</head>
```

---

## Supported Elements

The `http-equiv` attribute is supported by the following element:

| Element | Description |
|---------|-------------|
| `<meta>` | Metadata element for document properties |

---

## Attribute Values

### Syntax

```html
<meta http-equiv="directive" content="value" />
```

### Common Values

| Value | Content | Description | PDF Behavior |
|-------|---------|-------------|--------------|
| `Content-Type` | `text/html; charset=UTF-8` | Declares document MIME type and encoding | Informational only |
| `X-UA-Compatible` | `IE=edge` | Browser compatibility mode | Ignored in PDF |
| `refresh` | `30; url=...` | Page refresh/redirect | Not applicable to PDF |
| `default-style` | `stylesheet-name` | Preferred stylesheet | Not commonly used |
| `Content-Language` | `en-US` | Document language | Informational |

---

## Binding Values

The `http-equiv` and `content` attributes support data binding:

### Static Declaration

```html
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
```

### Dynamic Value

```html
<!-- Model: { charset: "UTF-8", language: "en-US" } -->
<meta http-equiv="Content-Type" content="text/html; charset={{model.charset}}" />
<meta http-equiv="Content-Language" content="{{model.language}}" />
```

---

## Notes

### Web Behavior vs PDF Behavior

**In Web Browsers:**
- `http-equiv` directives are processed as if they were HTTP headers
- Affects browser rendering, caching, and behavior
- Can trigger specific browser modes or actions

**In Scryber PDF Generation:**
- `http-equiv` attributes are **informational only**
- No HTTP headers are generated (PDFs are not served via HTTP)
- Maintains HTML document structure for compatibility
- Character encoding is handled automatically

### Character Encoding

While `http-equiv="Content-Type"` can specify character encoding, the standalone `charset` attribute is preferred:

```html
<!-- Old style (still valid) -->
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

<!-- Modern style (recommended) -->
<meta charset="UTF-8" />
```

### Limited Applicability

Most `http-equiv` directives are **not applicable to PDF generation**:
- `refresh`: PDFs are static documents
- `X-UA-Compatible`: No browser rendering involved
- Cache control directives: PDFs are files, not served resources

### Use Cases in PDF

1. **Documentation**: Preserve original HTML meta information
2. **Compatibility**: Maintain structure when converting web pages
3. **Standards Compliance**: Follow HTML specifications
4. **Metadata**: Provide additional document context

---

## Examples

### Example 1: Character Encoding Declaration

```html
<head>
    <title>Document with Encoding</title>

    <!-- Traditional encoding declaration -->
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    <!-- Modern equivalent (preferred) -->
    <meta charset="UTF-8" />
</head>
```

### Example 2: Content Type with Language

```html
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="Content-Language" content="en-US" />

    <title>English Language Document</title>
</head>
```

### Example 3: Multiple Meta Directives

```html
<head>
    <title>Complete Metadata Example</title>

    <!-- Character encoding -->
    <meta charset="UTF-8" />

    <!-- HTTP equivalent directives -->
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="Content-Language" content="en" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <!-- Standard metadata -->
    <meta name="author" content="John Doe" />
    <meta name="description" content="Sample document" />
</head>
```

### Example 4: Legacy Web Page Conversion

```html
<!-- Original web page headers -->
<head>
    <title>Legacy Web Page</title>

    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta http-equiv="imagetoolbar" content="no" />

    <!-- When converted to PDF, these are preserved but not processed -->
</head>
```

### Example 5: Multi-Language Document

```html
<head>
    <title>Bilingual Document</title>

    <meta charset="UTF-8" />
    <meta http-equiv="Content-Language" content="en-US, es-MX" />

    <meta name="description" content="English and Spanish content" />
</head>
```

### Example 6: Data-Bound Encoding

```html
<!-- Model: { encoding: "UTF-8", contentType: "text/html" } -->

<head>
    <title>Dynamic Metadata</title>

    <meta http-equiv="Content-Type"
          content="{{model.contentType}}; charset={{model.encoding}}" />
</head>
```

### Example 7: Complete Document Headers

```html
<head>
    <title>Professional Report</title>

    <!-- Character encoding -->
    <meta charset="UTF-8" />

    <!-- HTTP equivalents (informational in PDF) -->
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="Content-Language" content="en-US" />

    <!-- Standard metadata -->
    <meta name="author" content="Corporate Team" />
    <meta name="description" content="Annual corporate report" />
    <meta name="keywords" content="report, finance, annual" />

    <!-- Open Graph metadata -->
    <meta property="og:title" content="Annual Report 2024" />
    <meta property="og:type" content="document" />
</head>
```

### Example 8: Browser Compatibility Headers (Preserved)

```html
<head>
    <title>Cross-Browser Document</title>

    <!-- These are preserved when converting HTML to PDF -->
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="cleartype" content="on" />

    <meta charset="UTF-8" />
</head>
```

### Example 9: Content Security Policy (Informational)

```html
<head>
    <title>Secure Document</title>

    <!-- CSP directive (not enforced in PDF, but preserved) -->
    <meta http-equiv="Content-Security-Policy"
          content="default-src 'self'; script-src 'none';" />

    <meta charset="UTF-8" />
</head>
```

### Example 10: Conditional Encoding

```html
<!-- Model: { useUnicode: true } -->

<head>
    <title>Conditional Encoding Document</title>

    <meta http-equiv="Content-Type"
          content="text/html; charset={{model.useUnicode ? 'UTF-8' : 'ISO-8859-1'}}" />
</head>
```

### Example 11: International Character Set

```html
<head>
    <title>国际文档</title> <!-- International Document -->

    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="Content-Language" content="zh-CN" />

    <meta name="description" content="Chinese language document" />
</head>
```

### Example 12: Technical Documentation

```html
<head>
    <title>API Documentation</title>

    <meta charset="UTF-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="Content-Language" content="en" />

    <meta name="author" content="Development Team" />
    <meta name="description" content="Complete API reference documentation" />
</head>
```

### Example 13: Archive Document

```html
<head>
    <title>Historical Archive Document</title>

    <!-- Preserve original encoding declaration -->
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="Content-Language" content="en-GB" />

    <!-- Document archive metadata -->
    <meta name="archive-date" content="2024-01-15" />
    <meta name="original-url" content="https://example.com/original" />
</head>
```

### Example 14: Form Document

```html
<head>
    <title>Application Form</title>

    <meta charset="UTF-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    <meta name="author" content="HR Department" />
    <meta name="description" content="Employee application form" />
</head>
<body>
    <form id="applicationForm">
        <!-- Form content -->
    </form>
</body>
```

### Example 15: Web-to-PDF with All Meta Tags

```html
<head>
    <title>Complete Web Page - Converted to PDF</title>

    <!-- Character encoding -->
    <meta charset="UTF-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    <!-- Browser compatibility -->
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <!-- Language -->
    <meta http-equiv="Content-Language" content="en-US" />

    <!-- Standard metadata -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="author" content="Web Development Team" />
    <meta name="description" content="Complete web page example" />
    <meta name="keywords" content="web, HTML, PDF" />

    <!-- Open Graph -->
    <meta property="og:title" content="Complete Web Page" />
    <meta property="og:type" content="website" />
    <meta property="og:description" content="Example of complete web page metadata" />
</head>
```

---

## See Also

- [meta element](/reference/htmltags/meta.html) - The meta HTML element
- [content attribute](/reference/htmlattributes/content.html) - Meta content value
- [charset attribute](/reference/htmlattributes/charset.html) - Character encoding
- [name attribute](/reference/htmlattributes/name.html) - Standard metadata name
- [property attribute](/reference/htmlattributes/property.html) - Open Graph properties
- [Document Properties](/reference/document/properties.html) - PDF document properties
- [Character Encoding](/reference/encoding/) - Character encoding reference

---
