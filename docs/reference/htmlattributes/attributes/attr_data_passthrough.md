---
layout: default
title: data-passthrough
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-passthrough : The Style Passthrough Attribute

The `data-passthrough` attribute controls style inheritance for embedded content in `<iframe>` elements. It determines whether styles from the parent document should be applied to externally loaded content, enabling both style isolation and consistent theming across document composition.

## Summary

The `data-passthrough` attribute manages style inheritance for iframe content:

- Controls whether parent document styles affect embedded content
- Defaults to `false` (style isolation)
- When `true`, enables style passthrough for consistent theming
- Only applies to `<iframe>` elements, not `<embed>`
- Critical for modular document composition with unified styling
- Enables both isolated and themed content inclusion scenarios

This attribute is essential for:
- Creating themed multi-section documents
- Maintaining consistent typography across embedded content
- Isolating external content styles when needed
- Building reusable templates with or without parent styling
- Controlling visual consistency in document assembly
- Managing style inheritance in complex document structures

---

## Usage

The `data-passthrough` attribute is used exclusively with `<iframe>` elements:

```html
<!-- Default: style isolation (passthrough=false) -->
<iframe src="external-content.html"></iframe>

<!-- Explicit isolation -->
<iframe src="external-content.html" data-passthrough="false"></iframe>

<!-- Enable style passthrough -->
<iframe src="external-content.html" data-passthrough="true"></iframe>

<!-- With data binding -->
<iframe src="{{model.contentUrl}}"
        data-passthrough="{{model.useTheme}}"></iframe>
```

---

## Supported Elements

The `data-passthrough` attribute is supported exclusively on:

### Iframe Element
- `<iframe>` - The inline frame element for embedded content

**Not supported on**:
- `<embed>` - Embed always inherits parent styles
- `<object>` - Object is for file attachments, not parsed content
- Other elements - Attribute is specific to iframe style management

---

## Binding Values

### Boolean Values

**Type**: `boolean`
**Default**: `false`
**Binding**: Supports data binding expressions

```html
<!-- Static values -->
<iframe src="content.html" data-passthrough="true"></iframe>
<iframe src="content.html" data-passthrough="false"></iframe>

<!-- Data binding -->
<iframe src="{{model.templateUrl}}"
        data-passthrough="{{model.applyTheme}}"></iframe>

<!-- Conditional passthrough -->
<iframe src="{{model.contentUrl}}"
        data-passthrough="{{model.sectionType == 'themed'}}"></iframe>

<!-- Expression -->
<iframe src="content.html"
        data-passthrough="{{model.useCustomTheme ? false : true}}"></iframe>
```

**Data Model Example**:
```csharp
public class DocumentModel
{
    public string TemplateUrl { get; set; }
    public bool ApplyTheme { get; set; }
    public string SectionType { get; set; }
    public bool UseCustomTheme { get; set; }
}
```

---

## Notes

### How Style Passthrough Works

The `data-passthrough` attribute fundamentally changes how styles are applied to iframe content:

**passthrough="false" (default)**:
```html
<style>
    body {
        font-family: Arial;
        color: #333;
        font-size: 12pt;
    }
</style>

<iframe src="section.html" data-passthrough="false">
    <!-- Embedded content uses ONLY its own styles -->
    <!-- Parent body styles are NOT applied -->
    <!-- Content renders with its own defined styles -->
</iframe>
```

**passthrough="true"**:
```html
<style>
    body {
        font-family: Arial;
        color: #333;
        font-size: 12pt;
    }
</style>

<iframe src="section.html" data-passthrough="true">
    <!-- Embedded content INHERITS parent styles -->
    <!-- Parent body styles ARE applied -->
    <!-- Content uses parent typography, colors, etc. -->
</iframe>
```

### Style Isolation (passthrough=false)

When style passthrough is disabled (default behavior):

1. **Isolated Rendering**: Embedded content uses only its own defined styles
2. **No Inheritance**: Parent document styles do not affect embedded content
3. **Independent Theming**: Each embedded document controls its own appearance
4. **Clean Separation**: Useful for including third-party content
5. **Predictable Output**: Embedded content looks exactly as designed

**Use cases**:
- Including content with its own complete styling
- Embedding third-party templates
- Maintaining style independence between sections
- Loading content that should not be themed
- Testing embedded content in isolation

```html
<!-- Each section maintains its own styles -->
<iframe src="marketing-section.html" data-passthrough="false"></iframe>
<iframe src="technical-section.html" data-passthrough="false"></iframe>
<iframe src="legal-section.html" data-passthrough="false"></iframe>
```

### Style Passthrough (passthrough=true)

When style passthrough is enabled:

1. **Style Inheritance**: Embedded content inherits parent styles
2. **Unified Theming**: Consistent look across all sections
3. **Typography Consistency**: Fonts, sizes, and colors match parent
4. **Theme Application**: Brand colors and styles flow through
5. **Reduced Duplication**: No need to repeat styles in each section

**Use cases**:
- Creating themed multi-section documents
- Maintaining brand consistency
- Building document assembly systems
- Applying corporate style guides
- Creating unified reports from multiple sources

```html
<!-- All sections inherit parent theme -->
<style>
    body {
        font-family: 'Corporate Sans', Arial, sans-serif;
        color: #2c3e50;
        font-size: 11pt;
        line-height: 1.6;
    }
    h1 { color: #336699; font-size: 18pt; }
    h2 { color: #5588bb; font-size: 14pt; }
</style>

<iframe src="section1.html" data-passthrough="true"></iframe>
<iframe src="section2.html" data-passthrough="true"></iframe>
<iframe src="section3.html" data-passthrough="true"></iframe>
```

### iframe vs embed Behavior

Key difference in style handling:

**iframe with data-passthrough**:
- Extends `Div` component
- **Supports** `data-passthrough` attribute
- **Default**: Style isolation (passthrough=false)
- Controllable style inheritance
- Best for complex content with style control needs

**embed element**:
- Extends `VisualComponent` directly
- **Does not support** `data-passthrough`
- **Always** inherits parent styles
- No style isolation capability
- Best for simple content fragments that should match parent theme

```html
<!-- iframe: controlled style inheritance -->
<iframe src="content.html" data-passthrough="false">
    <!-- Isolated styles -->
</iframe>

<!-- embed: always inherits parent styles -->
<embed src="content.html" />
<!-- Always styled like parent -->
```

### Which Styles Pass Through?

When `data-passthrough="true"`, these styles are inherited:

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `line-height`, `letter-spacing`, `word-spacing`
- `text-decoration`, `text-transform`

**Colors**:
- `color` (text color)
- `background-color`

**Text Layout**:
- `text-align`, `text-indent`
- `white-space`, `word-break`

**Spacing** (from parent container):
- Inherited spacing properties

**Embedded content can still override**:
```html
<!-- Parent document -->
<style>
    body { font-family: Arial; color: #333; }
</style>

<iframe src="section.html" data-passthrough="true"></iframe>
```

```html
<!-- section.html can override -->
<style>
    h1 { color: #ff0000; }  /* Overrides parent color for h1 */
</style>
```

### Default Behavior

If `data-passthrough` is not specified:

```html
<!-- These are equivalent -->
<iframe src="content.html"></iframe>
<iframe src="content.html" data-passthrough="false"></iframe>
```

The default is **style isolation** (passthrough=false) to maintain backward compatibility and prevent unexpected style bleeding.

### Performance Implications

Style passthrough has minimal performance impact:

- **passthrough=false**: Returns empty style object, fastest
- **passthrough=true**: Applies parent styles, minimal overhead
- No significant difference for typical documents
- Consider style complexity, not passthrough setting

### Document Assembly Patterns

Common patterns for document composition:

**Pattern 1: Fully Themed Document**
```html
<!-- All sections inherit company theme -->
<style>
    @import url('company-theme.css');
</style>

<iframe src="sections/intro.html" data-passthrough="true"></iframe>
<iframe src="sections/analysis.html" data-passthrough="true"></iframe>
<iframe src="sections/conclusion.html" data-passthrough="true"></iframe>
```

**Pattern 2: Mixed Theming**
```html
<!-- Some themed, some isolated -->
<iframe src="branded-header.html" data-passthrough="true"></iframe>
<iframe src="third-party-content.html" data-passthrough="false"></iframe>
<iframe src="branded-footer.html" data-passthrough="true"></iframe>
```

**Pattern 3: Conditional Theming**
```html
<!-- Theme based on document type -->
<iframe src="{{.section}}.html"
        data-passthrough="{{model.documentType == 'branded'}}"></iframe>
```

### Debugging Style Issues

To diagnose style inheritance problems:

1. **Check passthrough setting**: Verify true/false value
2. **Inspect embedded content**: Look at content's own styles
3. **Review parent styles**: Ensure parent has styles to inherit
4. **Test isolation**: Try both true and false to see difference
5. **Use fallback**: Include complete styles in embedded content

```html
<!-- Debug: explicitly show both versions -->
<h2>With Passthrough (inherits parent)</h2>
<iframe src="test.html" data-passthrough="true"></iframe>

<h2>Without Passthrough (isolated)</h2>
<iframe src="test.html" data-passthrough="false"></iframe>
```

### Best Practices

**When to use passthrough=true**:
- Building themed document assemblies
- Maintaining brand consistency
- Applying corporate style guides
- Creating unified multi-section reports
- Developing reusable template libraries

**When to use passthrough=false**:
- Including third-party content
- Embedding content with complete styling
- Maintaining style independence
- Testing embedded content in isolation
- Preventing style conflicts

**General guidance**:
- Be consistent within a document
- Document your choice in comments
- Test both isolated and themed views
- Provide complete fallback styles in embedded content
- Use conditional passthrough for flexible systems

---

## Examples

### 1. Basic Style Isolation (Default)

Embedded content maintains its own styles:

```html
<!-- main.html -->
<!DOCTYPE html>
<html>
<head>
    <style>
        body {
            font-family: 'Georgia', serif;
            color: #000;
            font-size: 14pt;
        }
    </style>
</head>
<body>
    <h1>Main Document</h1>

    <!-- Embedded content uses its own styles -->
    <iframe src="section.html"></iframe>
</body>
</html>
```

```html
<!-- section.html -->
<style>
    /* These styles apply, parent styles don't -->
    body {
        font-family: 'Arial', sans-serif;
        color: #333;
        font-size: 11pt;
    }
</style>

<h2>Section Content</h2>
<p>This uses Arial 11pt, not Georgia 14pt.</p>
```

### 2. Enable Style Passthrough for Theming

All sections inherit parent theme:

```html
<!-- main.html -->
<!DOCTYPE html>
<html>
<head>
    <style>
        body {
            font-family: 'Helvetica Neue', sans-serif;
            color: #2c3e50;
            font-size: 11pt;
            line-height: 1.6;
        }

        h1 {
            color: #336699;
            font-size: 18pt;
            margin-bottom: 15pt;
        }

        h2 {
            color: #5588bb;
            font-size: 14pt;
            margin-top: 20pt;
        }
    </style>
</head>
<body>
    <h1>Annual Report 2024</h1>

    <!-- All sections inherit parent styles -->
    <iframe src="sections/executive-summary.html" data-passthrough="true"></iframe>
    <iframe src="sections/financial-overview.html" data-passthrough="true"></iframe>
    <iframe src="sections/future-outlook.html" data-passthrough="true"></iframe>
</body>
</html>
```

### 3. Conditional Style Passthrough

Apply theme based on content type:

```html
<!-- Model: { sections: [{url: "a.html", themed: true}, {url: "b.html", themed: false}] } -->

<style>
    body {
        font-family: 'Open Sans', sans-serif;
        color: #333;
    }
</style>

<template data-bind="{{model.sections}}">
    <iframe src="{{.url}}"
            data-passthrough="{{.themed}}"
            style="margin-bottom: 20pt;"></iframe>
</template>
```

### 4. Corporate Theme Application

Apply corporate branding to all sections:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Corporate Document</title>
    <style>
        /* Corporate theme */
        @import url('https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap');

        body {
            font-family: 'Roboto', Arial, sans-serif;
            color: #1a1a1a;
            font-size: 10pt;
            line-height: 1.5;
        }

        h1, h2, h3 {
            color: #0066cc;
            font-weight: 700;
        }

        h1 { font-size: 20pt; }
        h2 { font-size: 16pt; }
        h3 { font-size: 13pt; }

        a {
            color: #0066cc;
            text-decoration: none;
        }

        .highlight {
            background-color: #ffeb3b;
            padding: 2pt 4pt;
        }
    </style>
</head>
<body>
    <div style="text-align: center; margin-bottom: 40pt;">
        <h1>Acme Corporation</h1>
        <p>Quarterly Business Review</p>
    </div>

    <!-- All sections use corporate theme -->
    <iframe src="sections/overview.html" data-passthrough="true"></iframe>
    <iframe src="sections/metrics.html" data-passthrough="true"></iframe>
    <iframe src="sections/goals.html" data-passthrough="true"></iframe>
</body>
</html>
```

### 5. Mixed Themed and Isolated Content

Some sections themed, others isolated:

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        body {
            font-family: 'Arial', sans-serif;
            color: #333;
            font-size: 11pt;
        }

        h1 { color: #336699; }
    </style>
</head>
<body>
    <h1>Project Report</h1>

    <!-- Company header: uses theme -->
    <iframe src="templates/company-header.html"
            data-passthrough="true"></iframe>

    <!-- Main content: uses theme -->
    <iframe src="sections/project-overview.html"
            data-passthrough="true"></iframe>

    <!-- External vendor report: isolated -->
    <iframe src="vendor-reports/technical-audit.html"
            data-passthrough="false"></iframe>

    <!-- Company footer: uses theme -->
    <iframe src="templates/company-footer.html"
            data-passthrough="true"></iframe>
</body>
</html>
```

### 6. Data-Driven Theme Control

Control theming per section via data:

```csharp
public class DocumentSection
{
    public string Title { get; set; }
    public string Url { get; set; }
    public bool ApplyBranding { get; set; }
    public string Type { get; set; }
}

public class ReportModel
{
    public List<DocumentSection> Sections { get; set; } = new List<DocumentSection>
    {
        new DocumentSection
        {
            Title = "Cover Page",
            Url = "cover.html",
            ApplyBranding = true,
            Type = "branded"
        },
        new DocumentSection
        {
            Title = "Third-Party Analysis",
            Url = "external-analysis.html",
            ApplyBranding = false,
            Type = "external"
        },
        new DocumentSection
        {
            Title = "Conclusion",
            Url = "conclusion.html",
            ApplyBranding = true,
            Type = "branded"
        }
    };
}
```

```html
<style>
    body {
        font-family: 'Helvetica', sans-serif;
        color: #2c3e50;
    }
</style>

<h1>Multi-Source Report</h1>

<template data-bind="{{model.Sections}}">
    <div style="page-break-after: always;">
        <h2>{{.Title}}</h2>
        <iframe src="{{.Url}}"
                data-passthrough="{{.ApplyBranding}}"
                title="{{.Title}}"></iframe>
    </div>
</template>
```

### 7. Newsletter with Themed Articles

Newsletter with consistent article styling:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Monthly Newsletter</title>
    <style>
        body {
            font-family: 'Georgia', serif;
            color: #333;
            font-size: 11pt;
            line-height: 1.8;
        }

        h2 {
            color: #c0392b;
            font-size: 16pt;
            border-bottom: 2pt solid #c0392b;
            padding-bottom: 5pt;
            margin-top: 30pt;
        }

        .article-meta {
            color: #7f8c8d;
            font-size: 9pt;
            font-style: italic;
        }
    </style>
</head>
<body>
    <h1 style="text-align: center; color: #c0392b;">Tech Monthly Newsletter</h1>
    <p style="text-align: center; color: #7f8c8d;">January 2024 Edition</p>

    <!-- All articles inherit newsletter theme -->
    <iframe src="articles/feature-story.html" data-passthrough="true"></iframe>
    <iframe src="articles/tech-review.html" data-passthrough="true"></iframe>
    <iframe src="articles/industry-news.html" data-passthrough="true"></iframe>
    <iframe src="articles/upcoming-events.html" data-passthrough="true"></iframe>
</body>
</html>
```

### 8. Dynamic Theme Switching

Switch themes based on user preference:

```csharp
public class DocumentSettings
{
    public string ThemeMode { get; set; } // "light" or "dark"
    public bool ApplyTheme => ThemeMode == "light" || ThemeMode == "dark";
}
```

```html
<!-- Model: { settings: { themeMode: "light" } } -->

<style>
    /* Theme based on mode */
    body {
        font-family: 'Segoe UI', sans-serif;
        background-color: {{model.settings.themeMode == 'dark' ? '#1a1a1a' : '#ffffff'}};
        color: {{model.settings.themeMode == 'dark' ? '#e0e0e0' : '#333333'}};
        font-size: 11pt;
    }

    h1, h2, h3 {
        color: {{model.settings.themeMode == 'dark' ? '#4da6ff' : '#0066cc'}};
    }
</style>

<h1>Themed Document</h1>

<!-- Apply theme to all sections -->
<iframe src="section1.html" data-passthrough="{{model.settings.applyTheme}}"></iframe>
<iframe src="section2.html" data-passthrough="{{model.settings.applyTheme}}"></iframe>
<iframe src="section3.html" data-passthrough="{{model.settings.applyTheme}}"></iframe>
```

### 9. Multi-Language Document with Consistent Typography

Different languages, same typography:

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        body {
            font-family: 'Noto Sans', Arial, sans-serif;
            color: #2c3e50;
            font-size: 11pt;
            line-height: 1.7;
        }

        h1 {
            font-size: 18pt;
            color: #16a085;
            margin-bottom: 15pt;
        }

        h2 {
            font-size: 14pt;
            color: #27ae60;
            margin-top: 20pt;
        }
    </style>
</head>
<body>
    <!-- Model: { language: "en" } -->

    <h1>Multilingual Report</h1>

    <!-- All language sections use same typography -->
    <iframe src="content/{{model.language}}/summary.html"
            data-passthrough="true"></iframe>
    <iframe src="content/{{model.language}}/details.html"
            data-passthrough="true"></iframe>
    <iframe src="content/{{model.language}}/conclusion.html"
            data-passthrough="true"></iframe>
</body>
</html>
```

### 10. Template Library with Style Control

Reusable templates with configurable theming:

```csharp
public class TemplateSection
{
    public string TemplateName { get; set; }
    public string Path { get; set; }
    public bool UseParentStyles { get; set; }
}

public class TemplateLibraryModel
{
    public string ThemeName { get; set; }
    public List<TemplateSection> Sections { get; set; }
}
```

```html
<!-- Load theme CSS -->
<link rel="stylesheet" href="themes/{{model.ThemeName}}.css" />

<h1>Document Assembly</h1>

<template data-bind="{{model.Sections}}">
    <div class="section">
        <h2>{{.TemplateName}}</h2>
        <iframe src="templates/{{.Path}}"
                data-passthrough="{{.UseParentStyles}}"
                title="{{.TemplateName}}"></iframe>
    </div>
</template>
```

### 11. White-Label Document Generation

Generate documents with different branding:

```csharp
public class BrandingConfig
{
    public string ClientName { get; set; }
    public string PrimaryColor { get; set; }
    public string SecondaryColor { get; set; }
    public string FontFamily { get; set; }
    public bool ApplyBranding { get; set; }
}
```

```html
<!-- Model: { branding: {clientName: "Acme", primaryColor: "#0066cc", ...} } -->

<style>
    body {
        font-family: {{model.branding.fontFamily}}, sans-serif;
        color: #333;
        font-size: 11pt;
    }

    h1, h2, h3 {
        color: {{model.branding.primaryColor}};
    }

    .highlight {
        background-color: {{model.branding.secondaryColor}};
        opacity: 0.2;
        padding: 5pt;
    }
</style>

<div style="text-align: center; margin-bottom: 40pt;">
    <h1>{{model.branding.clientName}} Report</h1>
</div>

<!-- Apply client branding to all sections -->
<iframe src="sections/overview.html"
        data-passthrough="{{model.branding.applyBranding}}"></iframe>
<iframe src="sections/analysis.html"
        data-passthrough="{{model.branding.applyBranding}}"></iframe>
<iframe src="sections/recommendations.html"
        data-passthrough="{{model.branding.applyBranding}}"></iframe>
```

### 12. Academic Paper with Consistent Formatting

Research paper with uniform citation styles:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Research Paper</title>
    <style>
        body {
            font-family: 'Times New Roman', serif;
            color: #000;
            font-size: 12pt;
            line-height: 2.0;
            text-align: justify;
        }

        h1 {
            font-size: 16pt;
            text-align: center;
            font-weight: bold;
            margin-top: 0;
        }

        h2 {
            font-size: 14pt;
            font-weight: bold;
            margin-top: 24pt;
            margin-bottom: 12pt;
        }

        .citation {
            font-size: 10pt;
            margin-left: 40pt;
        }

        sup {
            font-size: 9pt;
        }
    </style>
</head>
<body>
    <h1>Impact of Technology on Modern Society</h1>
    <p style="text-align: center;">Jane Doe, Ph.D.</p>

    <!-- All sections maintain academic formatting -->
    <iframe src="sections/abstract.html" data-passthrough="true"></iframe>
    <iframe src="sections/introduction.html" data-passthrough="true"></iframe>
    <iframe src="sections/methodology.html" data-passthrough="true"></iframe>
    <iframe src="sections/results.html" data-passthrough="true"></iframe>
    <iframe src="sections/discussion.html" data-passthrough="true"></iframe>
    <iframe src="sections/conclusion.html" data-passthrough="true"></iframe>
    <iframe src="sections/references.html" data-passthrough="true"></iframe>
</body>
</html>
```

### 13. Magazine Layout with Section Independence

Magazine with varied section styles:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Design Magazine</title>
    <style>
        body {
            font-family: 'Futura', 'Century Gothic', sans-serif;
            color: #333;
            font-size: 11pt;
        }

        .magazine-header {
            font-size: 32pt;
            font-weight: bold;
            color: #ff6b6b;
            text-align: center;
            margin-bottom: 40pt;
        }
    </style>
</head>
<body>
    <div class="magazine-header">DESIGN WEEKLY</div>

    <!-- Cover story: uses magazine theme -->
    <iframe src="articles/cover-story.html" data-passthrough="true"></iframe>

    <!-- Feature articles: each has unique styling -->
    <iframe src="articles/fashion-feature.html" data-passthrough="false"></iframe>
    <iframe src="articles/architecture-feature.html" data-passthrough="false"></iframe>
    <iframe src="articles/graphic-design-feature.html" data-passthrough="false"></iframe>

    <!-- Editorial: uses magazine theme -->
    <iframe src="articles/editorial.html" data-passthrough="true"></iframe>
</body>
</html>
```

### 14. Financial Report with Regulatory Sections

Corporate report with external regulatory content:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Annual Financial Report</title>
    <style>
        body {
            font-family: 'Arial', sans-serif;
            color: #1a1a1a;
            font-size: 10pt;
            line-height: 1.5;
        }

        h1 {
            color: #003d7a;
            font-size: 18pt;
            border-bottom: 3pt solid #003d7a;
            padding-bottom: 10pt;
        }

        h2 {
            color: #0066cc;
            font-size: 14pt;
            margin-top: 25pt;
        }

        .financial-table {
            font-family: 'Courier New', monospace;
            font-size: 9pt;
        }
    </style>
</head>
<body>
    <h1>XYZ Corporation Annual Report 2024</h1>

    <!-- Company sections: use corporate theme -->
    <iframe src="sections/letter-to-shareholders.html" data-passthrough="true"></iframe>
    <iframe src="sections/business-overview.html" data-passthrough="true"></iframe>
    <iframe src="sections/financial-highlights.html" data-passthrough="true"></iframe>

    <!-- Regulatory sections: maintain original formatting -->
    <iframe src="regulatory/sec-filing.html" data-passthrough="false"></iframe>
    <iframe src="regulatory/audit-report.html" data-passthrough="false"></iframe>

    <!-- Company sections: use corporate theme -->
    <iframe src="sections/management-discussion.html" data-passthrough="true"></iframe>
    <iframe src="sections/future-outlook.html" data-passthrough="true"></iframe>
</body>
</html>
```

### 15. Debug and Comparison View

Compare styled vs. unstyled rendering:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Style Passthrough Comparison</title>
    <style>
        body {
            font-family: 'Verdana', sans-serif;
            color: #c0392b;
            font-size: 14pt;
            font-weight: bold;
        }

        h1 {
            color: #16a085;
            font-size: 24pt;
        }

        .comparison-container {
            display: flex;
            margin: 20pt 0;
        }

        .comparison-column {
            width: 50%;
            padding: 10pt;
            border: 1pt solid #ccc;
        }

        .label {
            font-size: 12pt;
            color: #333;
            font-weight: normal;
            margin-bottom: 10pt;
            padding: 5pt;
            background-color: #f0f0f0;
        }
    </style>
</head>
<body>
    <h1>Style Passthrough Comparison</h1>
    <p>Parent styles: Verdana 14pt Bold Red</p>

    <div class="comparison-container">
        <div class="comparison-column">
            <div class="label">With Passthrough (inherits parent)</div>
            <iframe src="test-content.html"
                    data-passthrough="true"
                    style="width: 100%; min-height: 200pt;"></iframe>
        </div>

        <div class="comparison-column">
            <div class="label">Without Passthrough (isolated)</div>
            <iframe src="test-content.html"
                    data-passthrough="false"
                    style="width: 100%; min-height: 200pt;"></iframe>
        </div>
    </div>
</body>
</html>
```

---

## See Also

- [iframe element](/reference/htmltags/iframe.html) - Iframe element documentation
- [embed element](/reference/htmltags/embed.html) - Embed element documentation
- [style attribute](/reference/htmlattributes/style.html) - Inline style attribute
- [class attribute](/reference/htmlattributes/class.html) - CSS class attribute
- [CSS Styling](/reference/styles/) - CSS styling in Scryber
- [Document Composition](/reference/composition/) - Building modular documents
- [Style Inheritance](/reference/styles/inheritance.html) - Style inheritance rules

---
