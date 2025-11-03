---
layout: default
title: main
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;main&gt; : The Main Content Element
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

The `<main>` element represents the dominant content of the document body. It is a semantic block-level container that identifies the primary content area, excluding headers, footers, navigation, sidebars, and other ancillary content. There should be only one `<main>` element per document.

## Usage

The `<main>` element creates a semantic container for primary content that:
- Represents the main content area of a document
- Contains content directly related to or expands upon the central topic
- Should be unique to the document (only one per page)
- Excludes repeated content like navigation, headers, footers, and sidebars
- Displays as a block-level element taking full width
- Automatically arranges `<header>` and `<footer>` elements at top and bottom
- Can generate PDF bookmarks/outlines when a `title` attribute is set
- Supports all CSS styling properties for layout and decoration
- Helps define document structure and improve accessibility

```html
<body>
    <header>Site Header</header>
    <nav>Navigation Menu</nav>

    <main>
        <h1>Article Title</h1>
        <p>This is the primary content of the document...</p>
    </main>

    <footer>Site Footer</footer>
</body>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title in the PDF document. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-bind` | expression | Binds the element to a data context for use with templates. |
| `data-content` | expression | Dynamically sets the content of the main element from bound data. |

### CSS Style Support

The `<main>` element supports extensive CSS styling through the `style` attribute or CSS classes:

**Box Model**:
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`
- `border`, `border-width`, `border-color`, `border-style`
- `border-top`, `border-right`, `border-bottom`, `border-left`

**Positioning**:
- `position`: `static`, `relative`, `absolute`
- `display`: `block`, `inline`, `inline-block`, `none`
- `float`: `left`, `right`, `none`
- `clear`: `both`, `left`, `right`, `none`

**Layout**:
- `column-count`, `column-width`, `column-gap` (multi-column layout)
- `page-break-before`, `page-break-after`, `page-break-inside`

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `color` (text color)
- `opacity`

**Typography** (inherited by child text):
- `font-family`, `font-size`, `font-weight`, `font-style`
- `text-align`, `text-decoration`, `text-transform`
- `line-height`, `letter-spacing`, `word-spacing`

---

## Notes

### Default Styling

The `<main>` element has minimal default styling:

**HTML main (Scryber.Html.Components.HTMLMain)**:
- **Display**: Block
- **Full Width**: Takes 100% of parent container width
- **Position**: Static
- **No default margins or padding**: Allows full control over spacing

The main element behaves as a standard block container with header/footer arrangement capabilities.

### Semantic Meaning and Purpose

The `<main>` element provides crucial semantic meaning to document structure:

1. **Primary Content**: Identifies the main content area of the document
2. **Unique Content**: Content specific to this document, not repeated across pages
3. **Central Topic**: Content directly related to or expanding upon the central topic
4. **Accessibility**: Helps screen readers and assistive technologies identify main content area
5. **Skip Navigation**: Enables "skip to main content" functionality for accessibility

### Uniqueness Requirement

Important considerations:
- **One per document**: Only one `<main>` element should exist per page
- **Not a descendant**: Should not be nested within `<article>`, `<aside>`, `<footer>`, `<header>`, or `<nav>` elements
- **Direct child of body**: Typically a direct child of `<body>` or the main document structure

### Header and Footer Arrangement

The `<main>` element automatically arranges `<header>` and `<footer>` elements:

1. All `<header>` elements within `<main>` are moved to the top (in order they appear)
2. All `<footer>` elements within `<main>` are moved to the bottom (in order they appear)
3. This arrangement happens during the pre-layout phase
4. Other content remains in its original position between headers and footers

```html
<main>
    <p>Some content</p>
    <header>This moves to top</header>
    <p>More content</p>
    <footer>This moves to bottom</footer>
    <p>Even more content</p>
</main>

<!-- Rendered order:
     1. header (moved to top)
     2. Some content
     3. More content
     4. Even more content
     5. footer (moved to bottom)
-->
```

### When to Use Main

Use `<main>` when:
- Identifying the primary content area of a document
- Creating the central content region of a page
- Wrapping unique, document-specific content
- Improving document structure and accessibility

Do NOT use `<main>` for:
- Content repeated across multiple pages (headers, footers, navigation)
- Sidebars or tangential content
- Multiple content areas (only one `<main>` per document)
- Nested within article, aside, footer, header, or nav elements

### Main vs. Article vs. Section

- **`<main>`**: The dominant content of the entire document (one per page)
- **`<article>`**: Self-contained, independently distributable content (can be multiple)
- **`<section>`**: Thematic grouping of content (can be multiple)

A `<main>` element can contain multiple `<article>` and `<section>` elements.

### Class Hierarchy

In the Scryber codebase:
- `HTMLMain` extends `HTMLHeadFootContainer` extends `Panel` extends `VisualComponent`
- Inherits header/footer arrangement behavior from `HTMLHeadFootContainer`
- Shares implementation with `<article>` and `<section>` but with different semantic meaning

### Best Practices

1. Use one `<main>` element per document
2. Place it as a direct child of `<body>` or main document structure
3. Do not nest `<main>` within semantic elements like `<article>` or `<aside>`
4. Include all primary content within `<main>`
5. Exclude repeated elements like site navigation and branding

---

## Examples

### Basic Document Structure

```html
<body>
    <header style="background-color: #336699; color: white; padding: 20pt;">
        <h1>Website Name</h1>
    </header>

    <nav style="background-color: #f0f0f0; padding: 10pt;">
        <a href="#home">Home</a> |
        <a href="#about">About</a> |
        <a href="#contact">Contact</a>
    </nav>

    <main style="padding: 20pt; min-height: 400pt;">
        <h1>Welcome to Our Site</h1>
        <p>This is the main content area of the document.</p>
    </main>

    <footer style="background-color: #333; color: white; padding: 15pt;">
        <p>© 2025 Company Name. All rights reserved.</p>
    </footer>
</body>
```

### Report Document with Main Content

```html
<body>
    <header style="text-align: center; padding: 30pt; border-bottom: 2pt solid #336699;">
        <h1>Annual Report 2025</h1>
        <p style="color: #666;">Fiscal Year Ending December 31, 2025</p>
    </header>

    <main style="padding: 30pt; max-width: 600pt; margin: 0 auto;">
        <h2>Executive Summary</h2>
        <p>
            This report presents the financial performance and operational
            highlights for the fiscal year 2025...
        </p>

        <h2>Financial Performance</h2>
        <p>Revenue increased by 25% year-over-year...</p>

        <h2>Operational Highlights</h2>
        <p>Key achievements include...</p>
    </main>

    <footer style="text-align: center; padding: 20pt; background-color: #f5f5f5;">
        <p style="font-size: 9pt; color: #666;">
            Confidential - For Internal Use Only
        </p>
    </footer>
</body>
```

### Main with Sidebar Layout

```html
<body style="display: table; width: 100%;">
    <aside style="display: table-cell; width: 200pt; padding: 20pt;
                  background-color: #f0f0f0; vertical-align: top;">
        <h3>Quick Links</h3>
        <ul>
            <li><a href="#section1">Section 1</a></li>
            <li><a href="#section2">Section 2</a></li>
            <li><a href="#section3">Section 3</a></li>
        </ul>
    </aside>

    <main style="display: table-cell; padding: 20pt; vertical-align: top;">
        <h1>Main Content Area</h1>
        <p>
            This is the primary content of the document. The sidebar is
            excluded from the main element as it is ancillary content.
        </p>
    </main>
</body>
```

### Article Collection in Main

```html
<body>
    <header style="background-color: #336699; color: white; padding: 15pt;">
        <h1>Tech Blog</h1>
    </header>

    <main style="padding: 20pt;">
        <h1>Latest Articles</h1>

        <article style="margin-bottom: 30pt; padding-bottom: 20pt;
                        border-bottom: 1pt solid #ddd;">
            <h2>Getting Started with PDF Generation</h2>
            <p style="color: #666; font-size: 10pt;">Published: October 10, 2025</p>
            <p>Learn the basics of generating PDFs programmatically...</p>
        </article>

        <article style="margin-bottom: 30pt; padding-bottom: 20pt;
                        border-bottom: 1pt solid #ddd;">
            <h2>Advanced Styling Techniques</h2>
            <p style="color: #666; font-size: 10pt;">Published: October 5, 2025</p>
            <p>Explore advanced CSS techniques for PDF layouts...</p>
        </article>

        <article style="margin-bottom: 30pt;">
            <h2>Data Binding Best Practices</h2>
            <p style="color: #666; font-size: 10pt;">Published: October 1, 2025</p>
            <p>Master data binding for dynamic PDF generation...</p>
        </article>
    </main>

    <footer style="text-align: center; padding: 20pt; background-color: #f5f5f5;">
        <p>© 2025 Tech Blog</p>
    </footer>
</body>
```

### Main with Header and Footer Arrangement

```html
<main style="padding: 20pt;">
    <p>This content will appear between the header and footer.</p>

    <header style="background-color: #f0f8ff; padding: 15pt; border-bottom: 2pt solid #336699;">
        <h1>Document Title</h1>
        <p>Subtitle and metadata</p>
    </header>

    <p>More content that will also appear in the middle.</p>

    <footer style="background-color: #f5f5f5; padding: 10pt; border-top: 1pt solid #ddd;">
        <p style="font-size: 9pt; text-align: center;">
            Document generated on October 13, 2025
        </p>
    </footer>

    <p>Even more content.</p>
</main>

<!-- Renders as:
     1. Header (moved to top)
     2. This content will appear between the header and footer.
     3. More content that will also appear in the middle.
     4. Even more content.
     5. Footer (moved to bottom)
-->
```

### Multi-Column Main Content

```html
<main style="padding: 30pt; column-count: 2; column-gap: 20pt;
             column-rule: 1pt solid #ddd;">
    <h1 style="column-span: all; text-align: center; margin-bottom: 20pt;">
        Magazine Article
    </h1>

    <p>
        Lorem ipsum dolor sit amet, consectetur adipiscing elit.
        The text flows in two columns creating a magazine-style layout...
    </p>

    <p>
        Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
        Content continues across columns naturally...
    </p>

    <p>
        Ut enim ad minim veniam, quis nostrud exercitation ullamco
        laboris nisi ut aliquip ex ea commodo consequat...
    </p>
</main>
```

### Main with Styled Content Sections

```html
<main style="padding: 20pt;">
    <section style="background-color: #f9f9f9; padding: 20pt; margin-bottom: 20pt;">
        <h2>Introduction</h2>
        <p>This section introduces the main topic...</p>
    </section>

    <section style="background-color: #fff; padding: 20pt; margin-bottom: 20pt;
                    border-left: 4pt solid #336699;">
        <h2>Methodology</h2>
        <p>Our approach involves several key steps...</p>
    </section>

    <section style="background-color: #f9f9f9; padding: 20pt;">
        <h2>Results</h2>
        <p>The findings demonstrate significant improvements...</p>
    </section>
</main>
```

### Main Content with Data Binding

```html
<!-- With model = {
    title: "Product Catalog",
    description: "Complete product listing",
    products: [...]
} -->

<main style="padding: 20pt;">
    <header style="border-bottom: 2pt solid #336699; padding-bottom: 15pt; margin-bottom: 20pt;">
        <h1>{{model.title}}</h1>
        <p style="color: #666;">{{model.description}}</p>
    </header>

    <template data-bind="{{model.products}}">
        <article style="margin-bottom: 20pt; padding: 15pt;
                        border: 1pt solid #ddd;">
            <h2>{{.name}}</h2>
            <p>{{.description}}</p>
            <p style="font-size: 14pt; font-weight: bold; color: #336699;">
                ${{.price}}
            </p>
        </article>
    </template>

    <footer style="margin-top: 30pt; padding-top: 15pt;
                   border-top: 1pt solid #ddd; text-align: center;">
        <p style="font-size: 9pt; color: #666;">
            Product catalog generated: {{model.date}}
        </p>
    </footer>
</main>
```

### Main with Table of Contents

```html
<body>
    <header style="padding: 20pt; background-color: #f0f0f0;">
        <h1>Technical Documentation</h1>
    </header>

    <nav style="padding: 15pt; background-color: #f9f9f9; border-bottom: 1pt solid #ddd;">
        <strong>Table of Contents</strong>
        <ul style="list-style-type: none; margin: 10pt 0 0 0; padding: 0;">
            <li style="margin: 5pt 0;"><a href="#intro">1. Introduction</a></li>
            <li style="margin: 5pt 0;"><a href="#setup">2. Setup</a></li>
            <li style="margin: 5pt 0;"><a href="#usage">3. Usage</a></li>
            <li style="margin: 5pt 0;"><a href="#api">4. API Reference</a></li>
        </ul>
    </nav>

    <main style="padding: 30pt;">
        <section id="intro">
            <h2>1. Introduction</h2>
            <p>Welcome to the technical documentation...</p>
        </section>

        <section id="setup" style="margin-top: 30pt;">
            <h2>2. Setup</h2>
            <p>Follow these steps to get started...</p>
        </section>

        <section id="usage" style="margin-top: 30pt;">
            <h2>3. Usage</h2>
            <p>Basic usage examples...</p>
        </section>

        <section id="api" style="margin-top: 30pt;">
            <h2>4. API Reference</h2>
            <p>Complete API documentation...</p>
        </section>
    </main>
</body>
```

### Main with Highlighted Call-to-Action

```html
<main style="padding: 30pt;">
    <h1>Product Launch Announcement</h1>

    <p>
        We're excited to announce the launch of our latest product.
        This revolutionary solution will transform the way you work...
    </p>

    <div style="background-color: #336699; color: white; padding: 20pt;
                margin: 30pt 0; text-align: center; border-radius: 5pt;">
        <h2 style="margin-top: 0; color: white;">Special Launch Offer</h2>
        <p style="font-size: 18pt; font-weight: bold;">
            Get 30% off for the first month
        </p>
        <p style="font-size: 12pt;">
            Use code: LAUNCH2025
        </p>
    </div>

    <p>
        This limited-time offer is available until the end of October.
        Don't miss this opportunity...
    </p>
</main>
```

### Main with Background Image

```html
<main style="background-image: url('watermark.png');
             background-position: center center;
             background-size: 400pt 400pt;
             background-repeat: no-repeat;
             padding: 40pt; min-height: 600pt;">
    <div style="background-color: rgba(255, 255, 255, 0.9); padding: 30pt;">
        <h1>Confidential Report</h1>
        <p>
            This document contains confidential information and should
            be handled according to company security policies...
        </p>

        <h2>Executive Summary</h2>
        <p>Key findings and recommendations...</p>

        <h2>Detailed Analysis</h2>
        <p>In-depth analysis of the situation...</p>
    </div>
</main>
```

### Main with Print-Specific Styling

```html
<main style="padding: 20pt; page-break-after: always;">
    <h1>Chapter 1: Introduction</h1>
    <p>This is the first chapter of the document...</p>

    <section style="page-break-before: always;">
        <h2>Section 1.1</h2>
        <p>Content that starts on a new page...</p>
    </section>

    <section style="page-break-inside: avoid; margin-top: 20pt;">
        <h2>Section 1.2</h2>
        <p>This entire section will be kept together on one page if possible...</p>
    </section>
</main>
```

### Responsive Main with Max Width

```html
<main style="max-width: 800pt; margin: 0 auto; padding: 30pt;">
    <h1>Centered Content Layout</h1>
    <p>
        This main content area has a maximum width and is centered
        on the page, creating a more readable layout for long-form content...
    </p>

    <p>
        The padding ensures content doesn't touch the edges, while the
        max-width prevents lines from becoming too long for comfortable reading.
    </p>
</main>
```

### Main with Shadow and Border Effects

```html
<main style="margin: 40pt; padding: 30pt; border: 1pt solid #ddd;
             background-color: #ffffff;">
    <header style="text-align: center; padding-bottom: 20pt;
                   border-bottom: 2pt solid #336699; margin-bottom: 30pt;">
        <h1 style="color: #336699;">Professional Document</h1>
        <p style="color: #666;">Prepared for: Client Name</p>
        <p style="color: #666; font-size: 10pt;">Date: October 13, 2025</p>
    </header>

    <h2>Overview</h2>
    <p>This document provides a comprehensive overview of...</p>

    <h2>Recommendations</h2>
    <p>Based on our analysis, we recommend...</p>

    <footer style="margin-top: 40pt; padding-top: 20pt;
                   border-top: 1pt solid #ddd;">
        <p style="font-size: 9pt; color: #666; text-align: center;">
            Prepared by: Your Company Name<br/>
            Contact: info@company.com | +1 (555) 123-4567
        </p>
    </footer>
</main>
```

### Main with Grid Layout

```html
<main style="padding: 20pt;">
    <h1 style="text-align: center; margin-bottom: 30pt;">Product Showcase</h1>

    <div style="display: table; width: 100%; table-layout: fixed;">
        <div style="display: table-row;">
            <div style="display: table-cell; padding: 10pt; width: 50%;
                        border: 1pt solid #ddd;">
                <h3>Product A</h3>
                <p>Description of product A...</p>
            </div>
            <div style="display: table-cell; padding: 10pt; width: 50%;
                        border: 1pt solid #ddd;">
                <h3>Product B</h3>
                <p>Description of product B...</p>
            </div>
        </div>
        <div style="display: table-row;">
            <div style="display: table-cell; padding: 10pt;
                        border: 1pt solid #ddd;">
                <h3>Product C</h3>
                <p>Description of product C...</p>
            </div>
            <div style="display: table-cell; padding: 10pt;
                        border: 1pt solid #ddd;">
                <h3>Product D</h3>
                <p>Description of product D...</p>
            </div>
        </div>
    </div>
</main>
```

---

## See Also

- [article](/reference/htmltags/article.html) - Self-contained composition element
- [section](/reference/htmltags/section.html) - Thematic section element
- [header](/reference/htmltags/header.html) - Header element for introductory content
- [footer](/reference/htmltags/footer.html) - Footer element for closing content
- [nav](/reference/htmltags/nav.html) - Navigation element
- [aside](/reference/htmltags/aside.html) - Sidebar or tangential content
- [div](/reference/htmltags/div.html) - Generic block container
- [Panel Component](/reference/components/panel.html) - Base panel component in Scryber namespace

---
