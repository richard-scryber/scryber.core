---
layout: default
title: article
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;article&gt; : The Article Element

The `<article>` element represents a self-contained composition in a document that is independently distributable or reusable. It is a semantic block-level element ideal for blog posts, news articles, product cards, user comments, or any content that makes sense on its own.

## Usage

The `<article>` element creates a semantic container that:
- Represents self-contained, independently distributable content
- Takes full width of its parent container by default
- Supports nested `<header>` and `<footer>` elements for structured content
- Can generate PDF bookmarks/outlines when a `title` attribute is set
- Automatically arranges header and footer elements at the top and bottom
- Supports all CSS styling properties for positioning, sizing, colors, borders, and backgrounds
- Can contain any type of content including other articles

```html
<article title="Getting Started Guide">
    <header>
        <h1>Getting Started with Scryber</h1>
        <p>Published: October 13, 2025</p>
    </header>

    <p>This article explains the basics of PDF generation...</p>

    <footer>
        <p>Author: John Doe</p>
    </footer>
</article>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title in the PDF document. Articles with titles appear in PDF bookmarks. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-bind` | expression | Binds the element to a data context for use with templates. |
| `data-content` | expression | Dynamically sets the content of the article from bound data. |

### CSS Style Support

The `<article>` element supports extensive CSS styling through the `style` attribute or CSS classes:

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
- `top`, `left`, `right`, `bottom` (for positioned elements)

**Layout**:
- `overflow`: `visible`, `hidden`, `clip`
- `column-count`, `column-width`, `column-gap` (multi-column layout)
- `page-break-before`, `page-break-after`, `page-break-inside`

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `background-position`, `background-size`, `background-repeat`
- `color` (text color)
- `opacity`
- `transform` (rotation, scaling, translation)

**Typography** (inherited by child text):
- `font-family`, `font-size`, `font-weight`, `font-style`
- `text-align`, `text-decoration`, `text-transform`
- `line-height`, `letter-spacing`, `word-spacing`

---

## Notes

### Semantic Meaning and Purpose

The `<article>` element provides semantic meaning to your document structure:

1. **Self-Contained Content**: Use `<article>` for content that makes sense independently - blog posts, news articles, forum posts, product descriptions, widgets, etc.
2. **Reusability**: Content within an `<article>` should be suitable for syndication or reuse
3. **Nesting**: Articles can contain other articles (e.g., blog post with comments as nested articles)
4. **Document Structure**: Helps create meaningful document outlines and bookmarks in PDFs

### When to Use Article vs. Div

Use `<article>` when:
- The content is self-contained and independently distributable
- You want to create a PDF bookmark/outline entry for the content
- The content could stand alone as a separate document or RSS feed item
- You're creating blog posts, news articles, product cards, or user-generated content

Use `<div>` when:
- You need a generic container for styling or layout purposes
- The content is not independently meaningful
- You're creating purely presentational groupings

### Header and Footer Arrangement

The `<article>` element automatically arranges `<header>` and `<footer>` elements:

1. All `<header>` elements are moved to the top of the article (in order they appear)
2. All `<footer>` elements are moved to the bottom of the article (in order they appear)
3. This arrangement happens during the pre-layout phase
4. Other content remains in its original position between headers and footers

```html
<article>
    <p>Some content</p>
    <header>This moves to top</header>
    <p>More content</p>
    <footer>This moves to bottom</footer>
    <p>Even more content</p>
</article>

<!-- Rendered order:
     1. header
     2. Some content
     3. More content
     4. Even more content
     5. footer
-->
```

### PDF Bookmarks and Outlines

When you add a `title` attribute to an `<article>`, it creates an entry in the PDF document outline:

```html
<article title="Chapter 1: Introduction">
    <!-- Article content -->
</article>
```

This creates a clickable bookmark in PDF readers that allows users to navigate directly to this article. Nested articles create hierarchical bookmarks.

### Class Hierarchy

In the Scryber codebase:
- `HTMLArticle` extends `HTMLHeadFootContainer` extends `Panel` extends `VisualComponent`
- Inherits header/footer arrangement behavior from `HTMLHeadFootContainer`
- Shares implementation with `<section>` element but conveys different semantic meaning

### Default Behavior

The `<article>` element has the following default behavior:

1. **Block Display**: Displays as a block-level element (stacks vertically)
2. **Full Width**: Takes 100% of the parent container's width by default
3. **Static Position**: Uses normal document flow positioning
4. **Header/Footer Management**: Automatically arranges header and footer elements

---

## Examples

### Basic Article Structure

```html
<article title="Understanding PDF Generation">
    <header>
        <h1>Understanding PDF Generation</h1>
        <p style="color: #666; font-size: 10pt;">By Jane Smith | October 13, 2025</p>
    </header>

    <p>
        PDF generation has become an essential part of modern web applications.
        This article explores the key concepts and best practices.
    </p>

    <h2>Key Concepts</h2>
    <p>There are several important concepts to understand...</p>

    <footer>
        <p style="font-size: 9pt; color: #999;">
            Copyright 2025. All rights reserved.
        </p>
    </footer>
</article>
```

### Multiple Articles in a Document

```html
<body>
    <h1>News Feed</h1>

    <article title="Article 1: Breaking News" style="margin-bottom: 20pt; padding: 15pt; border: 1pt solid #ccc;">
        <header>
            <h2>Breaking News: New Feature Released</h2>
            <p>Posted 2 hours ago</p>
        </header>
        <p>We're excited to announce a new feature...</p>
    </article>

    <article title="Article 2: Tutorial" style="margin-bottom: 20pt; padding: 15pt; border: 1pt solid #ccc;">
        <header>
            <h2>Tutorial: Getting Started</h2>
            <p>Posted yesterday</p>
        </header>
        <p>This tutorial will walk you through...</p>
    </article>

    <article title="Article 3: Tips and Tricks" style="margin-bottom: 20pt; padding: 15pt; border: 1pt solid #ccc;">
        <header>
            <h2>5 Tips for Better PDFs</h2>
            <p>Posted last week</p>
        </header>
        <p>Here are five tips to improve your PDF output...</p>
    </article>
</body>
```

### Nested Articles (Comments)

```html
<article title="Main Post: Product Review">
    <header>
        <h2>Product Review: Scryber PDF Library</h2>
        <p>By John Doe | October 10, 2025</p>
    </header>

    <p>Scryber is an excellent library for generating PDFs from HTML...</p>

    <section>
        <h3>Comments</h3>

        <article title="Comment by Alice" style="margin: 10pt 0; padding: 10pt; background-color: #f9f9f9;">
            <header>
                <strong>Alice</strong> <span style="color: #666;">- October 11, 2025</span>
            </header>
            <p>Great review! I've been using Scryber for months and love it.</p>
        </article>

        <article title="Comment by Bob" style="margin: 10pt 0; padding: 10pt; background-color: #f9f9f9;">
            <header>
                <strong>Bob</strong> <span style="color: #666;">- October 12, 2025</span>
            </header>
            <p>Thanks for the detailed write-up. Very helpful!</p>
        </article>
    </section>
</article>
```

### Article with Multi-Column Layout

```html
<article title="Magazine Article" style="padding: 20pt;">
    <header style="margin-bottom: 15pt;">
        <h1 style="font-size: 24pt; margin: 0;">The Future of PDF Technology</h1>
        <p style="color: #666; margin: 5pt 0 0 0;">An in-depth analysis</p>
    </header>

    <div style="column-count: 2; column-gap: 20pt; column-rule: 1pt solid #ddd; text-align: justify;">
        <p>
            PDF technology has evolved significantly over the past decades.
            From simple document format to rich interactive experiences,
            PDFs continue to be a cornerstone of digital publishing.
        </p>
        <p>
            Modern PDF generation tools like Scryber enable developers
            to create sophisticated documents with ease, combining the
            flexibility of HTML with the precision of PDF output.
        </p>
        <p>
            Looking ahead, we can expect further innovations in areas
            such as accessibility, interactivity, and integration with
            modern web technologies.
        </p>
    </div>

    <footer style="margin-top: 15pt; padding-top: 10pt; border-top: 1pt solid #ccc;">
        <p style="font-size: 9pt; color: #999;">Source: Tech Journal, 2025</p>
    </footer>
</article>
```

### Article with Data Binding

```html
<!-- With model.articles = [
    { title: "First Post", author: "John", date: "Oct 10", content: "..." },
    { title: "Second Post", author: "Jane", date: "Oct 11", content: "..." }
] -->

<template data-bind="{{model.articles}}">
    <article title="{{.title}}" style="page-break-after: always; padding: 20pt;">
        <header style="border-bottom: 2pt solid #336699; padding-bottom: 10pt; margin-bottom: 15pt;">
            <h1>{{.title}}</h1>
            <p style="color: #666;">By {{.author}} | {{.date}}</p>
        </header>

        <div>{{.content}}</div>

        <footer style="margin-top: 20pt; font-size: 9pt; color: #999;">
            Article by {{.author}}
        </footer>
    </article>
</template>
```

### Product Card Article

```html
<div style="column-count: 3; column-gap: 15pt;">
    <article style="break-inside: avoid; border: 1pt solid #ddd; padding: 10pt; margin-bottom: 15pt;">
        <header style="border-bottom: 1pt solid #eee; padding-bottom: 8pt; margin-bottom: 8pt;">
            <h3 style="margin: 0; font-size: 14pt;">Product Name</h3>
            <p style="color: #666; margin: 3pt 0 0 0; font-size: 10pt;">SKU: 12345</p>
        </header>

        <p style="font-size: 11pt;">
            High-quality product with excellent features.
            Perfect for all your needs.
        </p>

        <footer style="margin-top: 8pt; padding-top: 8pt; border-top: 1pt solid #eee;">
            <p style="font-size: 16pt; font-weight: bold; color: #336699; margin: 0;">
                $99.99
            </p>
        </footer>
    </article>
</div>
```

### Article with Styled Header and Footer

```html
<article title="Annual Report 2025" style="page-break-before: always;">
    <header style="background-color: #336699; color: white; padding: 20pt; margin: -20pt -20pt 20pt -20pt;">
        <h1 style="margin: 0; font-size: 28pt;">Annual Report 2025</h1>
        <p style="margin: 5pt 0 0 0; font-size: 14pt; opacity: 0.9;">
            Company Performance Overview
        </p>
    </header>

    <div style="padding: 0 20pt;">
        <h2>Executive Summary</h2>
        <p>This year has been marked by significant growth...</p>

        <h2>Financial Performance</h2>
        <p>Revenue increased by 25% year-over-year...</p>

        <h2>Looking Ahead</h2>
        <p>We anticipate continued growth in the coming year...</p>
    </div>

    <footer style="background-color: #f0f0f0; padding: 15pt; margin: 20pt -20pt -20pt -20pt; font-size: 9pt; color: #666;">
        <p style="margin: 0;">© 2025 Company Name. Confidential and Proprietary.</p>
        <p style="margin: 3pt 0 0 0;">For internal use only.</p>
    </footer>
</article>
```

### Article with Page Breaking Control

```html
<!-- This article starts on a new page -->
<article title="Important Section" style="page-break-before: always; page-break-after: always;">
    <header>
        <h1>Important Section</h1>
    </header>

    <p style="page-break-inside: avoid;">
        This content will be kept together on one page if possible.
    </p>
</article>

<!-- Next article starts on a new page due to previous article's page-break-after -->
<article title="Next Section">
    <header>
        <h1>Next Section</h1>
    </header>
    <p>Content continues here...</p>
</article>
```

---

## See Also

- [section](/reference/htmltags/section.html) - Semantic section element for thematic groupings
- [div](/reference/htmltags/div.html) - Generic block container without semantic meaning
- [header](/reference/htmltags/header.html) - Header element for introductory content
- [footer](/reference/htmltags/footer.html) - Footer element for closing content
- [aside](/reference/htmltags/aside.html) - Aside element for tangentially related content
- [nav](/reference/htmltags/nav.html) - Navigation element for navigation links
- [Panel Component](/reference/components/panel.html) - Base panel component in Scryber namespace
- [PDF Bookmarks](/reference/bookmarks/) - Creating document outlines and bookmarks
- [Document Structure](/reference/structure/) - Structuring PDF documents

---
