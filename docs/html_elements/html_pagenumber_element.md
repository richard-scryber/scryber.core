---
layout: default
title: page
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;page&gt; : The Page Number Element

The `<page>` element is a special Scryber component that automatically displays page numbers in your PDF documents. It supports various formats, can reference specific components, and provides access to document, section, and total page counts.

## Usage

The `<page>` element displays page numbers that:
- Automatically updates to show the current page number
- Supports multiple numbering formats (numeric, roman, alphabetic)
- Can display total page count or section information
- Can reference the page number of other components
- Works in headers, footers, and document content
- Supports custom formatting with data-format attribute
- Integrates with data binding and styling

```html
<!-- Simple page number -->
<page></page>

<!-- Page X of Y format -->
Page <page></page> of <page property="total"></page>

<!-- Custom format with style -->
<page data-format="Page {0} of {1}" style="font-weight: bold;"></page>
```

---

## Supported Attributes

### Page Number Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `property` | string | Specifies what to display: current page (default), "total" or "t" for total pages, "section" or "s" for section page number, "sectiontotal" or "st" for section total pages. |
| `data-format` | string | Custom format string. Use {0} for current page, {1} for total pages, {2} for section page, {3} for section total. |
| `for` | string | ID or name of a component to reference. Displays the page number where that component appears. |
| `data-page-hint` | integer | Hint for total page count optimization (advanced usage). |

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the page number. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element. |

### CSS Style Support

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `color` (text color)
- `text-decoration`, `text-transform`
- `letter-spacing`, `word-spacing`

**Background and Borders**:
- `background-color`, `background-image`
- `border`, `border-radius`
- `padding`, `margin`

**Display**:
- `display`: `inline` (default), `inline-block`, `block`, `none`
- `vertical-align`

---

## Notes

### Default Behavior

The `<page>` element has the following default behavior:

1. **Current Page**: Without attributes, displays the current page number
2. **Auto-Update**: Automatically updates during PDF generation
3. **Format**: Default is numeric (1, 2, 3...) unless styled via CSS
4. **Inline Display**: Flows with surrounding text
5. **Two-Pass Layout**: Uses two-pass rendering for accurate total counts

### Number Formats

You can control the number format through CSS or the parent section's page numbering settings:

- **Numeric**: 1, 2, 3, 4... (default)
- **Roman Uppercase**: I, II, III, IV...
- **Roman Lowercase**: i, ii, iii, iv...
- **Alpha Uppercase**: A, B, C, D...
- **Alpha Lowercase**: a, b, c, d...

### Property Values

The `property` attribute supports these values:

- **Empty or omitted**: Current page number
- **"total" or "t"**: Total pages in document
- **"section" or "s"**: Current page within current section
- **"sectiontotal" or "st"**: Total pages in current section

### Component References

The `for` attribute can reference components by:
- **ID**: Use `for="#componentId"` (with # prefix)
- **Name**: Use `for="componentName"` (without prefix)

This displays the page number where the referenced component appears.

---

## Examples

### Basic Page Number

```html
<div style="text-align: center; padding: 10pt;">
    Page <page></page>
</div>
```

### Page X of Y Format

```html
<div style="text-align: center; font-size: 10pt;">
    Page <page></page> of <page property="total"></page>
</div>
```

### Custom Format String

```html
<!-- Using data-format attribute -->
<page data-format="Page {0} of {1}"></page>

<!-- More complex format -->
<page data-format="Page {0} of {1} (Section {2} of {3})"></page>
```

### Styled Page Numbers

```html
<style>
    .page-number {
        font-family: 'Courier New', monospace;
        font-size: 10pt;
        color: #666;
        padding: 5pt 10pt;
        background-color: #f0f0f0;
        border-radius: 3pt;
    }
</style>

<div style="text-align: right;">
    <page class="page-number"></page>
</div>
```

### In Document Footer

```html
<html>
<head>
    <style>
        @page {
            @bottom-center {
                content: "Page " attr(page-number) " of " attr(page-count);
            }
        }
        footer {
            text-align: center;
            border-top: 1pt solid #ccc;
            padding-top: 10pt;
            margin-top: 20pt;
        }
    </style>
</head>
<body>
    <footer>
        Page <page></page> of <page property="total"></page>
    </footer>

    <!-- Document content -->
</body>
</html>
```

### Section Page Numbers

```html
<!-- Each section has its own page numbering -->
<section>
    <header>
        <h1>Chapter 1</h1>
        <div>Page <page property="section"></page> of <page property="sectiontotal"></page></div>
    </header>

    <!-- Chapter content -->
</section>

<section>
    <header>
        <h1>Chapter 2</h1>
        <div>Page <page property="s"></page> of <page property="st"></page></div>
    </header>

    <!-- Chapter content -->
</section>
```

### Reference to Component Location

```html
<!-- Display the page where a specific component appears -->
<div>
    <h1 id="introduction">Introduction</h1>
    <!-- Content -->
</div>

<div style="margin-top: 50pt;">
    <p>See the Introduction on page <page for="#introduction"></page></p>
</div>
```

### Table of Contents with Page Numbers

```html
<div class="toc">
    <h1>Table of Contents</h1>

    <div class="toc-entry">
        <span>Chapter 1: Getting Started</span>
        <span style="float: right;">Page <page for="#chapter1"></page></span>
    </div>

    <div class="toc-entry">
        <span>Chapter 2: Advanced Topics</span>
        <span style="float: right;">Page <page for="#chapter2"></page></span>
    </div>

    <div class="toc-entry">
        <span>Chapter 3: Reference</span>
        <span style="float: right;">Page <page for="#chapter3"></page></span>
    </div>
</div>

<div id="chapter1" style="page-break-before: always;">
    <h1>Chapter 1: Getting Started</h1>
    <!-- Content -->
</div>

<div id="chapter2" style="page-break-before: always;">
    <h1>Chapter 2: Advanced Topics</h1>
    <!-- Content -->
</div>

<div id="chapter3" style="page-break-before: always;">
    <h1>Chapter 3: Reference</h1>
    <!-- Content -->
</div>
```

### Data Binding with Page Numbers

```html
<!-- Model: { documentTitle: "Annual Report 2024", currentYear: 2024 } -->
<div style="text-align: center;">
    <h1>{{model.documentTitle}}</h1>
    <div style="font-size: 9pt; color: #666;">
        Page <page></page> | {{model.currentYear}}
    </div>
</div>
```

### Complex Footer Layout

```html
<style>
    .footer {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 10pt 20pt;
        border-top: 2pt solid #336699;
        margin-top: 30pt;
        font-size: 9pt;
    }
</style>

<div class="footer">
    <div>{{model.companyName}}</div>
    <div>
        <page data-format="Page {0} of {1}"></page>
    </div>
    <div>{{model.date}}</div>
</div>
```

### Different Formats in Same Document

```html
<!-- Front matter with roman numerals -->
<section style="page-numbering: lower-roman;">
    <div style="text-align: center;">
        <page></page>
    </div>
    <!-- Preface, TOC, etc. -->
</section>

<!-- Main content with regular numbers -->
<section style="page-numbering: decimal;">
    <div style="text-align: center;">
        <page></page>
    </div>
    <!-- Main content -->
</section>
```

### Conditional Page Number Display

```html
<!-- Only show page numbers after first page -->
<div hidden="{{page.current == 1 ? 'hidden' : ''}}">
    Page <page></page> of <page property="total"></page>
</div>
```

### Alternating Left/Right Page Numbers

```html
<style>
    @page :left {
        @bottom-left {
            content: "Page " attr(page-number);
        }
    }

    @page :right {
        @bottom-right {
            content: "Page " attr(page-number);
        }
    }
</style>

<!-- Even pages: left aligned -->
<div class="page-number-left" style="text-align: left;">
    <page></page>
</div>

<!-- Odd pages: right aligned -->
<div class="page-number-right" style="text-align: right;">
    <page></page>
</div>
```

### Multi-Column Footer with Page Numbers

```html
<div style="display: table; width: 100%; border-top: 1pt solid #ccc; padding-top: 5pt; font-size: 9pt;">
    <div style="display: table-row;">
        <div style="display: table-cell; width: 33%; text-align: left;">
            {{model.documentTitle}}
        </div>
        <div style="display: table-cell; width: 34%; text-align: center;">
            Page <page></page> of <page property="total"></page>
        </div>
        <div style="display: table-cell; width: 33%; text-align: right;">
            {{model.date}}
        </div>
    </div>
</div>
```

### Index with Page References

```html
<div class="index">
    <h2>Index</h2>

    <template data-bind="{{model.indexEntries}}">
        <div style="margin-bottom: 5pt;">
            <span style="font-weight: bold;">{{.term}}</span>
            <span style="margin-left: 10pt;">
                <template data-bind="{{.references}}">
                    <page for="{{.componentId}}"></page><span hidden="{{$islast ? 'hidden' : ''}}">, </span>
                </template>
            </span>
        </div>
    </template>
</div>
```

### Watermark with Page Number

```html
<div style="position: absolute; top: 50%; left: 50%;
            transform: translate(-50%, -50%) rotate(-45deg);
            font-size: 72pt; color: rgba(0, 0, 0, 0.1);
            z-index: -1;">
    DRAFT - Page <page></page>
</div>
```

### Page Number in Multiple Languages

```html
<!-- English -->
<div lang="en">
    Page <page></page> of <page property="total"></page>
</div>

<!-- Spanish -->
<div lang="es">
    Página <page></page> de <page property="total"></page>
</div>

<!-- French -->
<div lang="fr">
    Page <page></page> sur <page property="total"></page>
</div>
```

### Report Header with Section Info

```html
<div style="padding: 15pt; background-color: #f5f5f5; border-bottom: 2pt solid #336699;">
    <div style="display: flex; justify-content: space-between;">
        <div>
            <h2 style="margin: 0;">{{model.reportTitle}}</h2>
            <div style="font-size: 9pt; color: #666;">{{model.reportDate}}</div>
        </div>
        <div style="text-align: right; font-size: 9pt;">
            <div>Document: Page <page></page> of <page property="total"></page></div>
            <div>Section: Page <page property="section"></page> of <page property="sectiontotal"></page></div>
        </div>
    </div>
</div>
```

---

## See Also

- [section](/reference/htmltags/section.html) - Section element for page numbering context
- [header](/reference/htmltags/header.html) - Header element for page headers
- [footer](/reference/htmltags/footer.html) - Footer element for page footers
- [Page Layout](/reference/pagelayout/) - Page layout and numbering settings
- [Data Binding](/reference/binding/) - Data binding expressions
- [template](/reference/htmltags/template.html) - Template element for repeating content

---
