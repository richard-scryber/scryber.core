---
layout: default
title: body
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;body&gt; : The Document Body Element
{: .no_toc }

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---

## Summary

The `<body>` element represents the main content container of an HTML document. It contains all visible content that will be rendered in the PDF, including text, images, tables, and other elements. The body acts as a section with special properties for document-wide content.

---

## Usage

The `<body>` element is the primary content container that:
- Contains all visible content of the PDF document
- Acts as a `Section` component with page management capabilities
- Supports headers and footers that repeat on every page
- Provides continuation headers/footers for pages after the first
- Inherits all section properties and behaviors
- Has a default margin of 8pt on all sides
- Must be a direct child of the `<html>` element
- Automatically creates pages as content flows

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>My Document</title>
</head>
<body>
    <header>
        <h1>Document Header</h1>
        <p>Appears on every page</p>
    </header>

    <h1>Main Content</h1>
    <p>This is the document body content...</p>

    <footer>
        <p>Page Footer - Appears on every page</p>
    </footer>
</body>
</html>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the document body in PDF bookmarks. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-content` | expression | Dynamically sets the content of the address element from bound data. |
| `data-content-type` | Mime Type | Specifies the type of bound content fragment - XHTML; HTML; Markdown. |
| `data-content-action` | Replace, Append, Prepend | Specifies the action to take when binding elements with existing inner content. |

### Child Elements

The `<body>` element can contain:

| Element | Purpose | Repeating |
|---------|---------|-----------|
| `<header>` | Page header content | ✓ Yes (every page, or first page only) |
| `<footer>` | Page footer content | ✓ Yes (every page, or first page only) |
| `<continuation-header>` | Header for continuation pages | ✓ Yes (pages 2+) |
| `<continuation-footer>` | Footer for continuation pages | ✓ Yes (pages 2+) |
| Any content elements | Main document content | No |

---

## CSS Style Support

The `<body>` element supports extensive CSS styling through the `style` attribute or CSS classes:

**Box Model**:
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`
- `border`, `border-width`, `border-color`, `border-style`

**Page Layout**:
- `page-size`: Sets page dimensions (e.g., "A4", "Letter", "Legal")
- `page-orientation`: `portrait` or `landscape`
- `column-count`, `column-width`, `column-gap` (multi-column layout)

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `background-position`, `background-size`, `background-repeat`
- `color` (text color)
- `opacity`

**Typography** (inherited by child elements):
- `font-family`, `font-size`, `font-weight`, `font-style`
- `text-align`, `text-decoration`, `text-transform`
- `line-height`, `letter-spacing`, `word-spacing`

---

## Notes

### Body as a Section

The `<body>` element is implemented as a `Section` component in the library:

1. **Section Behavior**: Inherits all section properties and behaviors
2. **Page Management**: Automatically creates and manages pages
3. **Header/Footer Support**: Native support for repeating headers and footers
4. **Content Flow**: Content flows across multiple pages automatically
5. **Styling**: Supports all section styling capabilities

### Default Styling

The `<body>` element has default styling:

```css
body {
    margin: 8pt; /* Default margin on all sides */
}
```

You can override this with CSS:

```html
<body style="margin: 0;">
    <!-- No margins -->
</body>

<body style="margin: 72pt;">
    <!-- 1 inch (72pt) margins -->
</body>
```

### Headers and Footers

The `<body>` element supports repeating headers and footers:

**Regular Headers/Footers** (all pages):
```html
<body>
    <header>
        <div>Company Logo | Document Title</div>
    </header>

    <!-- Main content -->

    <footer>
        <div>Page <page /> of <page property='total'/></div>
    </footer>
</body>
```

**Continuation Headers/Footers** (pages 2+):
```html
<body>
    <header>
        <!-- First page header -->
        <h1>Title Page Header</h1>
    </header>

    <continuation-header>
        <!-- Pages 2+ header -->
        <div>Continued from previous page</div>
    </continuation-header>

    <!-- Main content -->

    <footer>
        <!-- First page footer -->
        <div>Introduction</div>
    </footer>

    <continuation-footer>
        <!-- Pages 2+ footer -->
        <div>Page <page /> of <page property='total' /></div>
    </continuation-footer>
</body>
```

### Header/Footer Positioning

Headers and footers are automatically positioned:

1. **Headers**: Positioned at the top of each page
2. **Footers**: Positioned at the bottom of each page
3. **Content Area**: Flows between headers and footers
4. **Margins**: Header/footer margins are separate from body margins

### Page Creation

The body automatically creates pages:

1. **First Page**: Created automatically when body is rendered
2. **Overflow**: New pages created when content exceeds page height
3. **Page Breaks**: Can be controlled with CSS `page-break-*` properties
4. **Headers/Footers**: Repeated on each new page

### Multi-Column Layout

The body supports multi-column layout:

```html
<body style="column-count: 2; column-gap: 20pt;">
    <p>Content flows across two columns...</p>
</body>
```

Content automatically flows from one column to the next, and across pages when needed.

### Class Hierarchy

```c#
Scryber.Html.Components.HTMLBody, Scryber.Components
```

In the library codebase:
- `HTMLBody` extends `Section`
- `Section` extends `Panel` extends `VisualComponent`
- Inherits all section capabilities (headers, footers, page management)
- Overrides `GetBaseStyle()` to set default 8pt margin

```c#
using Scryber.HTML.Components;

//documents can either contain a body **or** a frameset.
//Not both.
var doc = new HTMLDocument();
doc.Body = new HTMLBody();
doc.Body.Contents.Add("All the content goes here.");
```

### Page Numbering

Use the custom [page](html_pagenumber_element) element in headers/footers for page numbers:

```html
<footer>
    <page /> of <page property='total'/>
</footer>
```

Special expressions available:
- [none]: Current page number
- `property='total`: Total page count
- `data-format='{0} of {1}`: Custom page numbering format

### Performance Considerations

1. **Large Documents**: Body efficiently manages large documents with many pages
2. **Headers/Footers**: Repeated elements are cached for performance
3. **Content Flow**: Layout engine optimizes content flow across pages
4. **Memory**: Pages are rendered incrementally to manage memory

---

## Examples

### Basic HTML Document Structure

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Basic Document</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 40pt;
        }
    </style>
</head>
<body>
    <h1>Welcome to My Document</h1>
    <p>This is the main content of the document.</p>
    <p>Content automatically flows across pages as needed.</p>
</body>
</html>
```

### Document with Headers and Footers

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Report with Headers and Footers</title>
    <style>
        body {
            margin: 50pt 40pt;
            font-family: Arial, sans-serif;
        }
        header {
            border-bottom: 2pt solid #336699;
            padding-bottom: 10pt;
            margin-bottom: 20pt;
        }
        footer {
            border-top: 1pt solid #cccccc;
            padding-top: 10pt;
            margin-top: 20pt;
            text-align: center;
            font-size: 9pt;
            color: #666666;
        }
    </style>
</head>
<body>
    <header>
        <h1 style="margin: 0;">Annual Report 2025</h1>
        <p style="margin: 5pt 0 0 0; color: #666;">ACME Corporation</p>
    </header>

    <h2>Executive Summary</h2>
    <p>This report presents the financial performance...</p>

    <h2>Financial Highlights</h2>
    <p>Revenue increased by 25% year-over-year...</p>

    <footer>
        <p>Page <page /> of <page property='total'/> | Annual Report 2025 | Confidential</p>
    </footer>
</body>
</html>
```

### Document with Continuation Headers

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Long Document</title>
    <style>
        body {
            margin: 60pt 50pt;
        }
        header, continuation-header {
            padding-bottom: 10pt;
            margin-bottom: 15pt;
            border-bottom: 2pt solid #336699;
        }
    </style>
</head>
<body>
    <header>
        <!-- First page header -->
        <h1>Research Paper: PDF Generation</h1>
        <p>By Dr. Jane Smith</p>
        <p>October 2025</p>
    </header>

    <continuation-header>
        <!-- Pages 2+ header -->
        <div>Research Paper: PDF Generation | Dr. Jane Smith | Page <page /></div>
    </continuation-header>

    <h2>Abstract</h2>
    <p>This paper presents a comprehensive study...</p>

    <h2>Introduction</h2>
    <p>PDF generation has evolved significantly...</p>

    <!-- More content causing page breaks -->

    <footer>
        <p style="text-align: center; font-size: 9pt;">
            © 2025 Research Institute
        </p>
    </footer>

    <continuation-footer>
        <p style="text-align: center; font-size: 9pt;">
            © 2025 Research Institute | Page <page />
        </p>
    </continuation-footer>
</body>
</html>
```

### Multi-Column Body Layout

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Newsletter</title>
    <style>
        body {
            margin: 30pt;
            font-family: Georgia, serif;
            font-size: 11pt;
            column-count: 2;
            column-gap: 20pt;
            column-rule: 1pt solid #cccccc;
        }
        h1 {
            column-span: all;
            text-align: center;
            color: #336699;
            border-bottom: 3pt solid #336699;
            padding-bottom: 10pt;
        }
    </style>
</head>
<body>
    <h1>Monthly Newsletter - October 2025</h1>

    <h2>Feature Article</h2>
    <p>Content flows across two columns automatically...</p>

    <h2>Updates</h2>
    <p>More content in column layout...</p>

    <h2>Announcements</h2>
    <p>Additional content continues flowing...</p>
</body>
</html>
```

### Full-Page Background

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Branded Document</title>
    <style>
        body {
            margin: 0;
            padding: 50pt;
            background-color: #f8f9fa;
            background-image: url('watermark.png');
            background-position: center;
            background-repeat: no-repeat;
            background-size: 400pt 400pt;
            opacity: 0.1;
        }
    </style>
</head>
<body>
    <h1>Confidential Document</h1>
    <p>Document with watermark background...</p>
</body>
</html>
```

### No Margin Body (Full Bleed)

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Full Bleed Design</title>
    <style>
        body {
            margin: 0;
            padding: 0;
        }
        .full-width-header {
            background-color: #336699;
            color: white;
            padding: 30pt;
        }
        .content {
            padding: 30pt;
        }
    </style>
</head>
<body>
    <div class="full-width-header">
        <h1>Full Width Header</h1>
    </div>
    <div class="content">
        <p>Main content with padding...</p>
    </div>
</body>
</html>
```

### Invoice Body Layout

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Invoice #12345</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            font-size: 10pt;
            margin: 40pt;
        }
        header {
            display: flex;
            justify-content: space-between;
            margin-bottom: 30pt;
            padding-bottom: 15pt;
            border-bottom: 2pt solid #333333;
        }
        footer {
            position: fixed;
            bottom: 30pt;
            width: 100%;
            text-align: center;
            font-size: 9pt;
            color: #666666;
        }
    </style>
</head>
<body>
    <header>
        <div>
            <h1 style="margin: 0;">ACME Corporation</h1>
            <p>123 Business St, City, State 12345</p>
        </div>
        <div>
            <h2 style="margin: 0;">INVOICE</h2>
            <p>Invoice #: 12345<br/>Date: 10/13/2025</p>
        </div>
    </header>

    <h2>Bill To:</h2>
    <p>Customer Name<br/>Customer Address</p>

    <table style="width: 100%; border-collapse: collapse; margin: 20pt 0;">
        <thead>
            <tr style="background-color: #333; color: white;">
                <th style="padding: 8pt; text-align: left;">Description</th>
                <th style="padding: 8pt; text-align: right;">Amount</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">Service 1</td>
                <td style="padding: 8pt; text-align: right; border-bottom: 1pt solid #ddd;">$1,000.00</td>
            </tr>
        </tbody>
    </table>

    <div style="text-align: right; font-size: 14pt; font-weight: bold;">
        Total: $1,000.00
    </div>

    <footer>
        <p>Thank you for your business! | info@acme.com | (555) 123-4567</p>
    </footer>
</body>
</html>
```

### Resume Body Layout

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Resume - John Doe</title>
    <style>
        body {
            font-family: Calibri, Arial, sans-serif;
            font-size: 11pt;
            margin: 40pt;
            line-height: 1.5;
        }
    </style>
</head>
<body>
    <header style="text-align: center; margin-bottom: 30pt; padding-bottom: 15pt; border-bottom: 3pt solid #336699;">
        <h1 style="margin: 0 0 10pt 0; font-size: 28pt;">John Doe</h1>
        <p style="margin: 0;">john.doe@example.com | (555) 123-4567 | linkedin.com/in/johndoe</p>
    </header>

    <h2 style="color: #336699; border-bottom: 2pt solid #e0e0e0; padding-bottom: 5pt;">Experience</h2>
    <div style="margin-bottom: 15pt;">
        <h3 style="margin: 5pt 0;">Senior Developer - Tech Company</h3>
        <p style="margin: 0; color: #666;">2020 - Present</p>
        <ul>
            <li>Led development of PDF generation system</li>
            <li>Improved performance by 50%</li>
        </ul>
    </div>

    <h2 style="color: #336699; border-bottom: 2pt solid #e0e0e0; padding-bottom: 5pt;">Education</h2>
    <div>
        <h3 style="margin: 5pt 0;">Bachelor of Science in Computer Science</h3>
        <p style="margin: 0; color: #666;">University Name, 2016</p>
    </div>
</body>
</html>
```

### Book Body with Chapter Pages

```html
{% raw %}<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>My Book</title>
    <style>
        body {
            margin: 72pt;
            font-family: Georgia, serif;
            font-size: 12pt;
            line-height: 1.8;
            text-align: justify;
        }
        header {
            display: none; /* Hide on first page */
        }
        continuation-header {
            font-size: 9pt;
            font-style: italic;
            text-align: center;
            padding-bottom: 10pt;
        }
        footer {
            text-align: center;
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <continuation-header>
        <p>My Book Title | Chapter {{chapter-number}}</p>
    </continuation-header>

    <h1 style="text-align: center; margin-bottom: 40pt;">Chapter 1: The Beginning</h1>

    <p>It was a dark and stormy night...</p>
    <p>The story continues with many paragraphs...</p>

    <footer>
        <p><page /></p>
    </footer>
</body>
</html>{% endraw %}
```

### Technical Manual Body

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Technical Manual</title>
    <style>
        body {
            margin: 50pt 40pt;
            font-family: Arial, sans-serif;
            font-size: 11pt;
        }
        header {
            background-color: #336699;
            color: white;
            padding: 15pt;
            margin: -50pt -40pt 30pt -40pt;
        }
        continuation-header {
            background-color: #5588bb;
            color: white;
            padding: 10pt;
            margin: -50pt -40pt 20pt -40pt;
            font-size: 10pt;
        }
        footer {
            border-top: 1pt solid #cccccc;
            padding-top: 10pt;
            text-align: center;
            font-size: 9pt;
            color: #666666;
        }
    </style>
</head>
<body>
    <header>
        <h1 style="margin: 0;">Product User Manual</h1>
        <p style="margin: 5pt 0 0 0;">Version 2.0</p>
    </header>

    <continuation-header>
        <p style="margin: 0;">Product User Manual v2.0</p>
    </continuation-header>

    <h2>Introduction</h2>
    <p>Welcome to the user manual...</p>

    <h2>Getting Started</h2>
    <p>Follow these steps to begin...</p>

    <footer>
        <p>Page <page /> | © 2025 Company Name | support@example.com</p>
    </footer>
</body>
</html>
```

### Presentation Body (Slides)

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Business Presentation</title>
    <style>
        body {
            margin: 0;
            padding: 40pt;
        }
        .slide {
            page-break-after: always;
            min-height: 500pt;
            padding: 40pt;
            border: 2pt solid #336699;
        }
        .slide h1 {
            color: #336699;
            font-size: 32pt;
            text-align: center;
        }
        .slide-number {
            position: fixed;
            bottom: 20pt;
            right: 20pt;
            font-size: 10pt;
            color: #999999;
        }
    </style>
</head>
<body>
    <div class="slide">
        <h1>Q4 Business Review</h1>
        <p style="text-align: center; font-size: 18pt;">October 2025</p>
    </div>

    <div class="slide">
        <h1>Agenda</h1>
        <ul style="font-size: 16pt; line-height: 2;">
            <li>Financial Performance</li>
            <li>Market Analysis</li>
            <li>Future Outlook</li>
        </ul>
    </div>

    <div class="slide">
        <h1>Financial Performance</h1>
        <p>Revenue increased by 25%...</p>
    </div>

    <footer>
        <div class="slide-number">Slide <page /></div>
    </footer>
</body>
</html>
```

### Legal Document Body

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Service Agreement</title>
    <style>
        body {
            margin: 72pt;
            font-family: 'Times New Roman', serif;
            font-size: 12pt;
            line-height: 2;
            text-align: justify;
        }
        header {
            text-align: center;
            margin-bottom: 40pt;
        }
        .clause {
            margin: 20pt 0;
        }
        .clause-number {
            font-weight: bold;
            margin-right: 10pt;
        }
        footer {
            margin-top: 40pt;
            border-top: 2pt solid #000000;
            padding-top: 20pt;
        }
    </style>
</head>
<body>
    <header>
        <h1 style="margin: 0;">SERVICE AGREEMENT</h1>
        <p>Effective Date: October 13, 2025</p>
    </header>

    <div class="clause">
        <span class="clause-number">1.</span>
        <strong>PARTIES.</strong> This Service Agreement ("Agreement") is entered into
        between ACME Corporation ("Provider") and Client Company ("Client").
    </div>

    <div class="clause">
        <span class="clause-number">2.</span>
        <strong>SERVICES.</strong> Provider agrees to provide the following services...
    </div>

    <div class="clause">
        <span class="clause-number">3.</span>
        <strong>TERM.</strong> This Agreement shall commence on the Effective Date
        and continue for a period of twelve (12) months.
    </div>

    <footer>
        <div style="display: flex; justify-content: space-between; margin-top: 40pt;">
            <div style="width: 45%;">
                <div style="border-bottom: 1pt solid #000; margin-bottom: 5pt;"></div>
                <p style="margin: 0;">Provider Signature</p>
                <p style="margin: 0;">Date: _____________</p>
            </div>
            <div style="width: 45%;">
                <div style="border-bottom: 1pt solid #000; margin-bottom: 5pt;"></div>
                <p style="margin: 0;">Client Signature</p>
                <p style="margin: 0;">Date: _____________</p>
            </div>
        </div>
    </footer>
</body>
</html>
```

### Data-Bound Body

```html
{% raw %}<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>{{model.title}}</title>
    <style>
        body {
            margin: var(concat(model.pageMargin,'pt'));
            font-family: var(model.fontFamily);
            font-size: var(concat(model.fontSize,'pt'));
            background-color: var(model.backgroundColor);
        }
    </style>
</head>
<body class="{{model.documentClass}}">
    <header style="background-color: {{model.brandColor}}; color: white; padding: 20pt;">
        <h1>{{model.documentTitle}}</h1>
        <p>{{model.subtitle}}</p>
    </header>

    <h2>{{model.sectionTitle}}</h2>
    <!-- bind complex formatted content -->
    <p data-content='{{model.content}}'></p>

    <footer>
        <p style="text-align: center;">
            {{model.footerText}} | Page <page data-format="{0} of {1}" />
        </p>
    </footer>
</body>
</html>{% endraw %}
```

---

## See Also

- [html](html_html_element.html) - Root HTML element (parent of body)
- [head](html_head_element.html) - Head element for metadata
- [header](html_header_element.html) - Header element for page headers
- [footer](html_footer_element.html) - Footer element for page footers
- [section](html_section_element.html) - Section element for document sections
- [div](html_div_element.html) - Generic container element
- [Page Sizes](/learning/styles/page_sizes) - Page sizing
- [Page Layout](/learning/styles/page_layout) - Page breaks, numbering and sectioning.
- [Headers and Footers](/learing/header_footer/) - Header and footer details
- [Data Binding](/learning/binding/) - Data binding and expressions

---
