---
layout: default
title: iframe and embed
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;iframe&gt; and &lt;embed&gt; : The Embedded Content Elements

The `<iframe>` and `<embed>` elements allow embedding external content into PDF documents. They support loading remote HTML/PDF files that are dynamically parsed and embedded at render time. These elements enable content reuse, modular document composition, and dynamic content inclusion.

## Usage

The `<iframe>` and `<embed>` elements enable embedded content that:
- Load and parse external HTML/PDF documents via the `src` attribute
- Support remote content loading from URLs or local file paths
- Parse and render external content within the current document context
- Support styling through CSS and inline styles
- Allow dynamic content sources through data binding
- Support visibility control and conditional rendering
- Enable document composition from multiple sources
- Can be nested within any container element
- Support passthrough styling mode for iframe content

```html
<!-- Basic iframe loading external HTML -->
<iframe src="header.html"></iframe>

<!-- Embed element with external content -->
<embed src="footer.html" />

<!-- Styled iframe with dimensions -->
<iframe src="content.html" style="width: 100%; min-height: 400pt;"></iframe>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the embedded content. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Embedded Content Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `src` | string | **Required**. Source URL or file path for the external content to load and parse. |
| `data-passthrough` | boolean | **(iframe only)** If true, parent styles pass through to embedded content. Default: false. |

### CSS Style Support

Both `<iframe>` and `<embed>` elements support extensive CSS styling:

**Sizing**:
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`

**Positioning**:
- `display`: `block` (default), `inline`, `inline-block`, `none`
- `position`: `static`, `relative`, `absolute`
- `float`: `left`, `right`, `none`
- `clear`: `both`, `left`, `right`, `none`
- `top`, `left`, `right`, `bottom` (for positioned elements)

**Spacing**:
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding` (all variants)

**Visual Effects**:
- `border`, `border-width`, `border-color`, `border-style`, `border-radius`
- `background`, `background-color`
- `opacity`

---

## Notes

### How Embedded Content Works in PDF

Unlike web browsers where iframes create separate browsing contexts, Scryber's `<iframe>` and `<embed>` elements work as **content inclusion mechanisms**:

1. **Parse-Time Loading**: External content is fetched and parsed during document generation
2. **Content Merging**: The parsed content becomes part of the parent document's content tree
3. **No Sandboxing**: Embedded content shares the same PDF document context
4. **Style Inheritance**: Styles can be controlled via the `data-passthrough` attribute (iframe only)
5. **Static Inclusion**: Content is resolved at generation time, not at viewing time

This approach is similar to server-side includes (SSI) or template partials rather than browser iframe behavior.

### iframe vs embed

Both elements function similarly with one key difference:

**iframe**:
- Extends the `Div` component
- Supports `data-passthrough` attribute for style control
- By default, isolates embedded content from parent styles (passthrough=false)
- Can contain fallback content in its body
- Best for complete HTML documents or sections

**embed**:
- Extends `VisualComponent` directly
- Always inherits parent styles
- Simpler component model
- Typically used for smaller content fragments
- Best for reusable snippets and partials

### External Content Sources

The `src` attribute supports multiple source types:

1. **Relative File Paths**:
   ```html
   <iframe src="partials/header.html"></iframe>
   <embed src="../shared/footer.html" />
   ```

2. **Absolute File Paths**:
   ```html
   <iframe src="/templates/navigation.html"></iframe>
   ```

3. **Remote URLs**:
   ```html
   <iframe src="https://example.com/content.html"></iframe>
   <embed src="https://api.example.com/template?id=123" />
   ```

4. **Dynamic Sources via Data Binding**:
   ```html
   <iframe src="{{model.contentUrl}}"></iframe>
   ```

### Supported Content Types

The embedded content source should be valid HTML that Scryber can parse:

- **HTML Documents**: Complete HTML with `<html>`, `<head>`, and `<body>` tags
- **HTML Fragments**: Partial HTML containing just body content
- **XML Documents**: Well-formed XML that follows Scryber's namespace conventions
- **PDF Templates**: Other PDF template files in HTML format

Content must be parseable by Scryber's HTML parser. Standard HTML5 elements and Scryber-specific components are supported.

### Style Passthrough (iframe only)

The `data-passthrough` attribute controls style inheritance for iframe elements:

**passthrough="false" (default)**:
- Embedded content renders with isolated styles
- Parent document styles do not affect embedded content
- Embedded content uses only its own defined styles
- Useful for completely independent content blocks

**passthrough="true"**:
- Parent styles are applied to embedded content
- Embedded content inherits typography, colors, and other styles
- Allows consistent theming across embedded content
- Useful for themed document composition

```html
<!-- Isolated styling (default) -->
<iframe src="content.html"></iframe>

<!-- Inherit parent styles -->
<iframe src="content.html" data-passthrough="true"></iframe>
```

### Remote Content Loading

When using remote URLs, consider:

1. **Network Access**: Scryber must have network access to fetch remote content
2. **Performance**: Remote content loading adds network latency to document generation
3. **Caching**: Consider implementing caching mechanisms for frequently used remote content
4. **Error Handling**: Use proper error handling for failed remote requests
5. **Security**: Validate and trust remote content sources

### Error Handling

When external content fails to load:

- In **Strict** conformance mode: An error is thrown, stopping document generation
- In **Lax** conformance mode: The error is logged and generation continues
- Use try-catch blocks or conformance settings to handle loading failures gracefully

### Content Security

When embedding external content:

1. **Trust Content Sources**: Only embed content from trusted sources
2. **Validate URLs**: Sanitize and validate dynamic URL sources
3. **Input Validation**: Validate any user-provided source paths
4. **Local File Access**: Be cautious with local file system access in web applications

### Fallback Content

The `<iframe>` element can contain fallback content displayed if loading fails:

```html
<iframe src="content.html">
    <p>This content will be shown if content.html fails to load</p>
</iframe>
```

Note: With `<embed>` being a self-closing element, fallback content should be handled externally.

### Class Hierarchy

In the Scryber codebase:

**HTMLiFrame**:
- Extends `Div` → `Panel` → `ContainerComponent` → `VisualComponent`
- Decorated with `[PDFRemoteParsableComponent("iframe", SourceAttribute = "src")]`
- Supports `data-passthrough` for style isolation control
- Default display mode: `block`

**HTMLEmbed**:
- Extends `VisualComponent` → `Component`
- Decorated with `[PDFRemoteParsableComponent("embed", SourceAttribute = "src")]`
- Implements `IInvisibleContainer` interface
- Simpler component model without style isolation

---

## Examples

### Basic Content Inclusion

```html
<!-- Load header from external file -->
<iframe src="templates/header.html"></iframe>

<div class="content">
    <h1>Main Document Content</h1>
    <p>This is the main document content...</p>
</div>

<!-- Load footer from external file -->
<embed src="templates/footer.html" />
```

### Modular Document Composition

```html
<!DOCTYPE html>
<html>
<head>
    <title>Modular Report</title>
    <style>
        body { font-family: Arial, sans-serif; }
        .section { margin: 20pt 0; }
    </style>
</head>
<body>
    <!-- Company header -->
    <iframe src="partials/company-header.html"></iframe>

    <!-- Executive summary -->
    <div class="section">
        <iframe src="reports/executive-summary.html"></iframe>
    </div>

    <!-- Financial data -->
    <div class="section">
        <iframe src="reports/financial-data.html"></iframe>
    </div>

    <!-- Charts and graphs -->
    <div class="section">
        <iframe src="reports/charts.html"></iframe>
    </div>

    <!-- Legal footer -->
    <embed src="partials/legal-footer.html" />
</body>
</html>
```

### Dynamic Content Loading

```html
<!-- With model = { headerTemplate: "header-v2.html", footerTemplate: "footer-standard.html" } -->

<iframe src="{{model.headerTemplate}}"></iframe>

<div class="main-content">
    <h1>{{model.title}}</h1>
    <p>{{model.description}}</p>
</div>

<iframe src="{{model.footerTemplate}}"></iframe>
```

### Styled Iframe with Dimensions

```html
<style>
    .embedded-content {
        width: 100%;
        min-height: 300pt;
        border: 1pt solid #ccc;
        padding: 15pt;
        background-color: #f9f9f9;
    }
</style>

<iframe src="content/article.html" class="embedded-content"></iframe>
```

### Iframe with Passthrough Styling

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        body {
            font-family: 'Helvetica', sans-serif;
            color: #333;
            font-size: 11pt;
        }

        h1 { color: #336699; }
        h2 { color: #5588bb; }
    </style>
</head>
<body>
    <!-- Content inherits parent styles -->
    <iframe src="section1.html" data-passthrough="true"></iframe>

    <!-- Content uses its own styles only -->
    <iframe src="section2.html" data-passthrough="false"></iframe>

    <!-- Default behavior (no passthrough) -->
    <iframe src="section3.html"></iframe>
</body>
</html>
```

### Conditional Content Loading

```html
<!-- With model = { showHeader: true, showFooter: false } -->

<iframe src="header.html" hidden="{{model.showHeader ? '' : 'hidden'}}"></iframe>

<div class="content">
    <h1>Document Content</h1>
</div>

<iframe src="footer.html" hidden="{{model.showFooter ? '' : 'hidden'}}"></iframe>
```

### Loading Remote Content

```html
<!-- Load from remote API -->
<iframe src="https://api.example.com/templates/header?format=html"></iframe>

<!-- Load from CDN -->
<embed src="https://cdn.example.com/shared/footer.html" />

<!-- Load from internal network -->
<iframe src="http://internal.company.com/templates/disclaimer.html"></iframe>
```

### Multi-Language Support

```html
<!-- With model = { language: "en", region: "US" } -->

<iframe src="i18n/{{model.language}}/header.html"></iframe>

<div class="content">
    <iframe src="i18n/{{model.language}}/{{model.region}}/content.html"></iframe>
</div>

<iframe src="i18n/{{model.language}}/footer.html"></iframe>
```

### Nested Iframes

```html
<!-- main-document.html -->
<div>
    <h1>Main Document</h1>
    <iframe src="section-with-subsections.html"></iframe>
</div>

<!-- section-with-subsections.html -->
<div class="section">
    <h2>Section Title</h2>
    <iframe src="subsection-a.html"></iframe>
    <iframe src="subsection-b.html"></iframe>
</div>
```

### Template Variations

```html
<!-- With model = { templateVersion: 2, customerType: "premium" } -->

<iframe src="headers/header-v{{model.templateVersion}}.html"></iframe>

<div class="main-content">
    <!-- Customer-specific content -->
    <iframe src="content/{{model.customerType}}/main.html"></iframe>
</div>

<iframe src="footers/footer-{{model.customerType}}.html"></iframe>
```

### Iframe with Fallback Content

```html
<iframe src="remote-content.html">
    <!-- Fallback content if loading fails -->
    <div style="padding: 20pt; background-color: #fff3cd; border: 1pt solid #ffc107;">
        <h3 style="margin-top: 0;">Content Unavailable</h3>
        <p>The remote content could not be loaded. This is fallback content.</p>
    </div>
</iframe>
```

### Reusable Components

```html
<!-- Load reusable table header -->
<table style="width: 100%;">
    <thead>
        <tr>
            <embed src="components/table-header.html" />
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Data 1</td>
            <td>Data 2</td>
            <td>Data 3</td>
        </tr>
    </tbody>
</table>
```

### Dynamic Report Sections

```html
<!-- With model.sections = [{src: "intro.html"}, {src: "analysis.html"}, {src: "conclusion.html"}] -->

<div class="report">
    <h1>{{model.title}}</h1>

    <template data-bind="{{model.sections}}">
        <div class="report-section">
            <iframe src="sections/{{.src}}" style="width: 100%; margin: 15pt 0;"></iframe>
        </div>
    </template>
</div>
```

### Themed Document Assembly

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        /* Global theme styles */
        body {
            font-family: 'Georgia', serif;
            color: #2c3e50;
            line-height: 1.6;
        }

        .theme-primary { color: #3498db; }
        .theme-secondary { color: #2ecc71; }
    </style>
</head>
<body>
    <!-- All iframes with passthrough will inherit theme -->
    <iframe src="sections/cover.html" data-passthrough="true"></iframe>
    <iframe src="sections/toc.html" data-passthrough="true"></iframe>
    <iframe src="sections/chapter1.html" data-passthrough="true"></iframe>
    <iframe src="sections/chapter2.html" data-passthrough="true"></iframe>
</body>
</html>
```

### Page-Specific Headers/Footers

```html
<!DOCTYPE html>
<html>
<head>
    <title>Multi-Section Report</title>
</head>
<body>
    <!-- Section 1 with specific header -->
    <div style="page-break-after: always;">
        <iframe src="headers/section1-header.html"></iframe>
        <div class="content">Section 1 content...</div>
    </div>

    <!-- Section 2 with different header -->
    <div style="page-break-after: always;">
        <iframe src="headers/section2-header.html"></iframe>
        <div class="content">Section 2 content...</div>
    </div>

    <!-- Section 3 with another header -->
    <div>
        <iframe src="headers/section3-header.html"></iframe>
        <div class="content">Section 3 content...</div>
    </div>
</body>
</html>
```

### Email Template Embedding

```html
<!-- Load email signature from shared template -->
<div class="email-body">
    <p>Dear {{model.recipientName}},</p>
    <p>{{model.messageBody}}</p>

    <iframe src="templates/signatures/{{model.senderDepartment}}.html"></iframe>
</div>
```

### Form Components

```html
<div class="application-form">
    <h1>Application Form</h1>

    <!-- Applicant information section -->
    <iframe src="form-sections/applicant-info.html"></iframe>

    <!-- Employment history section -->
    <iframe src="form-sections/employment-history.html"></iframe>

    <!-- References section -->
    <iframe src="form-sections/references.html"></iframe>

    <!-- Legal disclaimers -->
    <embed src="form-sections/legal-disclaimers.html" />
</div>
```

### API-Driven Content

```html
<!-- With model = { apiEndpoint: "https://api.example.com", reportId: "12345" } -->

<div class="report">
    <!-- Load report header from API -->
    <iframe src="{{model.apiEndpoint}}/reports/{{model.reportId}}/header.html"></iframe>

    <!-- Load report body from API -->
    <iframe src="{{model.apiEndpoint}}/reports/{{model.reportId}}/body.html"
            style="min-height: 500pt;"></iframe>

    <!-- Load report footer from API -->
    <iframe src="{{model.apiEndpoint}}/reports/{{model.reportId}}/footer.html"></iframe>
</div>
```

### Responsive Container Sizing

```html
<style>
    .responsive-container {
        width: 100%;
        min-height: 200pt;
        max-height: 600pt;
        overflow: hidden;
    }
</style>

<div class="responsive-container">
    <iframe src="flexible-content.html" style="width: 100%; height: auto;"></iframe>
</div>
```

### Conditional Regional Content

```html
<!-- With model = { country: "US", state: "CA" } -->

<div class="regional-document">
    <!-- Country-specific header -->
    <iframe src="content/{{model.country}}/header.html"></iframe>

    <!-- State/province-specific content -->
    <iframe src="content/{{model.country}}/{{model.state}}/main.html"></iframe>

    <!-- Country-specific footer with legal info -->
    <iframe src="content/{{model.country}}/legal-footer.html"></iframe>
</div>
```

### Complex Document Structure

```html
<!DOCTYPE html>
<html>
<head>
    <title>Annual Report</title>
</head>
<body>
    <!-- Cover page -->
    <div style="page-break-after: always;">
        <iframe src="report/cover.html"></iframe>
    </div>

    <!-- Table of contents -->
    <div style="page-break-after: always;">
        <iframe src="report/toc.html"></iframe>
    </div>

    <!-- Executive summary -->
    <div style="page-break-after: always;">
        <iframe src="report/executive-summary.html" data-passthrough="true"></iframe>
    </div>

    <!-- Financial statements -->
    <div style="page-break-after: always;">
        <h1>Financial Statements</h1>
        <iframe src="report/balance-sheet.html"></iframe>
        <iframe src="report/income-statement.html"></iframe>
        <iframe src="report/cash-flow.html"></iframe>
    </div>

    <!-- Notes to financial statements -->
    <div style="page-break-after: always;">
        <iframe src="report/financial-notes.html"></iframe>
    </div>

    <!-- Management discussion -->
    <div style="page-break-after: always;">
        <iframe src="report/management-discussion.html"></iframe>
    </div>

    <!-- Appendices -->
    <div>
        <h1>Appendices</h1>
        <iframe src="report/appendix-a.html"></iframe>
        <iframe src="report/appendix-b.html"></iframe>
        <iframe src="report/appendix-c.html"></iframe>
    </div>
</body>
</html>
```

---

## See Also

- [object](/reference/htmltags/object.html) - Object element for file attachments
- [picture](/reference/htmltags/picture.html) - Picture element for responsive images
- [img](/reference/htmltags/img.html) - Image element
- [div](/reference/htmltags/div.html) - Container element
- [template](/reference/htmltags/template.html) - Template element for data binding
- [Data Binding](/reference/binding/) - Data binding and expressions
- [Document Parser](/reference/parser/) - HTML/XML parsing in Scryber
- [Remote Resources](/reference/resources/) - Loading remote content

---
