---
layout: default
title: section
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;section&gt; : The Section Element
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

The `<section>` element represents a thematic grouping of content, typically with a heading. It is a semantic block-level element used to divide documents into logical sections, chapters, or themed content areas.

## Usage

The `<section>` element creates a semantic container that:
- Represents a thematic section of content with a specific purpose or topic
- Takes full width of its parent container by default
- Has a **default page break before** behavior (starts on a new page by default)
- Supports nested `<header>` and `<footer>` elements for structured content
- Can generate PDF bookmarks/outlines when a `title` attribute is set
- Automatically arranges header and footer elements at the top and bottom
- Supports all CSS styling properties for positioning, sizing, colors, borders, and backgrounds
- Ideal for document chapters, major divisions, and thematic groupings

```html
<section title="Introduction">
    <header>
        <h1>Introduction</h1>
    </header>

    <p>This section introduces the main concepts...</p>

    <footer>
        <p>End of introduction</p>
    </footer>
</section>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title in the PDF document. Sections with titles appear in PDF bookmarks. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-bind` | expression | Binds the element to a data context for use with templates. |
| `data-content` | expression | Dynamically sets the content of the section from bound data. |

### CSS Style Support

The `<section>` element supports extensive CSS styling through the `style` attribute or CSS classes:

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

The `<section>` element provides semantic meaning to your document structure:

1. **Thematic Grouping**: Use `<section>` for content that belongs together thematically - chapters, tabbed content, numbered sections of a thesis
2. **Heading Association**: Each section typically has a heading (h1-h6) that describes its theme
3. **Document Outline**: Sections create a hierarchical structure in your document
4. **PDF Chapters**: Perfect for creating chapter divisions in multi-section documents

### When to Use Section vs. Article vs. Div

Use `<section>` when:
- You have a thematic grouping of content with a specific topic or purpose
- You want to create document chapters or major divisions
- The content needs a descriptive heading
- You want automatic page breaks between major document divisions
- You're creating structured documents with clear thematic sections

Use `<article>` when:
- The content is self-contained and independently distributable
- The content could be syndicated or reused as a standalone piece

Use `<div>` when:
- You need a generic container for styling or layout purposes
- The content grouping has no semantic meaning
- You're creating purely presentational groupings

### Default Page Break Behavior

**IMPORTANT**: The `<section>` element has a default `page-break-before: true` style. This means:

1. Each section starts on a new page by default
2. This is ideal for document chapters and major divisions
3. You can override this behavior with CSS:

```html
<!-- Starts on new page (default) -->
<section>
    <h1>Chapter 1</h1>
</section>

<!-- No page break before -->
<section style="page-break-before: auto;">
    <h2>Subsection 1.1</h2>
</section>
```

### Header and Footer Arrangement

The `<section>` element automatically arranges `<header>` and `<footer>` elements:

1. All `<header>` elements are moved to the top of the section (in order they appear)
2. All `<footer>` elements are moved to the bottom of the section (in order they appear)
3. This arrangement happens during the pre-layout phase
4. Other content remains in its original position between headers and footers

```html
<section>
    <p>Some content</p>
    <header>This moves to top</header>
    <p>More content</p>
    <footer>This moves to bottom</footer>
    <p>Even more content</p>
</section>

<!-- Rendered order:
     1. header
     2. Some content
     3. More content
     4. Even more content
     5. footer
-->
```

### PDF Bookmarks and Outlines

When you add a `title` attribute to a `<section>`, it creates an entry in the PDF document outline:

```html
<section title="Chapter 1: Introduction">
    <h1>Introduction</h1>
    <!-- Section content -->
</section>

<section title="Chapter 2: Getting Started">
    <h1>Getting Started</h1>
    <!-- Section content -->
</section>
```

This creates clickable bookmarks in PDF readers that allow users to navigate directly to each section. Nested sections create hierarchical bookmarks.

### Class Hierarchy

In the Scryber codebase:
- `HTMLSection` extends `HTMLHeadFootContainer` extends `Panel` extends `VisualComponent`
- Inherits header/footer arrangement behavior from `HTMLHeadFootContainer`
- Overrides `GetBaseStyle()` to set `page-break-before: true` by default
- Shares implementation with `<article>` element but conveys different semantic meaning

### Default Behavior

The `<section>` element has the following default behavior:

1. **Block Display**: Displays as a block-level element (stacks vertically)
2. **Full Width**: Takes 100% of the parent container's width by default
3. **Page Break Before**: Automatically starts on a new page (override with CSS)
4. **Static Position**: Uses normal document flow positioning
5. **Header/Footer Management**: Automatically arranges header and footer elements

---

## Examples

### Basic Document Structure with Sections

```html
<body>
    <section title="Chapter 1: Introduction">
        <header>
            <h1>Chapter 1: Introduction</h1>
            <p style="color: #666;">Understanding the Basics</p>
        </header>

        <p>This chapter introduces the fundamental concepts...</p>

        <h2>Section 1.1: Overview</h2>
        <p>Content for section 1.1...</p>

        <h2>Section 1.2: Key Concepts</h2>
        <p>Content for section 1.2...</p>

        <footer>
            <p style="font-size: 9pt; color: #999;">End of Chapter 1</p>
        </footer>
    </section>

    <section title="Chapter 2: Getting Started">
        <header>
            <h1>Chapter 2: Getting Started</h1>
            <p style="color: #666;">Your First Steps</p>
        </header>

        <p>In this chapter, we'll walk through...</p>

        <footer>
            <p style="font-size: 9pt; color: #999;">End of Chapter 2</p>
        </footer>
    </section>

    <section title="Chapter 3: Advanced Topics">
        <header>
            <h1>Chapter 3: Advanced Topics</h1>
            <p style="color: #666;">Deep Dive</p>
        </header>

        <p>This chapter explores advanced features...</p>

        <footer>
            <p style="font-size: 9pt; color: #999;">End of Chapter 3</p>
        </footer>
    </section>
</body>
```

### Nested Sections (Hierarchical Structure)

```html
<section title="Part I: Fundamentals">
    <header style="background-color: #336699; color: white; padding: 20pt;">
        <h1>Part I: Fundamentals</h1>
    </header>

    <section title="Chapter 1: Introduction" style="page-break-before: auto;">
        <header style="border-bottom: 2pt solid #336699; padding-bottom: 10pt;">
            <h2>Chapter 1: Introduction</h2>
        </header>
        <p>Content for chapter 1...</p>
    </section>

    <section title="Chapter 2: Basic Concepts">
        <header style="border-bottom: 2pt solid #336699; padding-bottom: 10pt;">
            <h2>Chapter 2: Basic Concepts</h2>
        </header>
        <p>Content for chapter 2...</p>
    </section>
</section>

<section title="Part II: Advanced Topics">
    <header style="background-color: #336699; color: white; padding: 20pt;">
        <h1>Part II: Advanced Topics</h1>
    </header>

    <section title="Chapter 3: Advanced Features" style="page-break-before: auto;">
        <header style="border-bottom: 2pt solid #336699; padding-bottom: 10pt;">
            <h2>Chapter 3: Advanced Features</h2>
        </header>
        <p>Content for chapter 3...</p>
    </section>
</section>
```

### Section Without Page Break

```html
<!-- First section starts on new page (default) -->
<section title="Overview">
    <h1>Overview</h1>
    <p>This is the overview section...</p>
</section>

<!-- These subsections continue on same page -->
<section title="Key Features" style="page-break-before: auto;">
    <h2>Key Features</h2>
    <p>Here are the key features...</p>
</section>

<section title="Benefits" style="page-break-before: auto;">
    <h2>Benefits</h2>
    <p>The main benefits include...</p>
</section>

<!-- This section forces a new page -->
<section title="Next Major Topic" style="page-break-before: always;">
    <h1>Next Major Topic</h1>
    <p>Starting a completely new topic...</p>
</section>
```

### Section with Styled Header and Footer

```html
<section title="Executive Summary">
    <header style="background: linear-gradient(to right, #336699, #5588bb);
                   color: white;
                   padding: 25pt;
                   margin: -20pt -20pt 20pt -20pt;">
        <h1 style="margin: 0; font-size: 32pt;">Executive Summary</h1>
        <p style="margin: 10pt 0 0 0; font-size: 14pt; opacity: 0.9;">
            Annual Report 2025
        </p>
    </header>

    <div style="padding: 0 20pt;">
        <h2>Financial Highlights</h2>
        <p>Revenue increased by 25% year-over-year...</p>

        <h2>Operational Excellence</h2>
        <p>We achieved record efficiency metrics...</p>

        <h2>Strategic Initiatives</h2>
        <p>Our strategic initiatives focused on...</p>
    </div>

    <footer style="background-color: #f5f5f5;
                   padding: 15pt;
                   margin: 20pt -20pt -20pt -20pt;
                   border-top: 3pt solid #336699;">
        <p style="margin: 0; font-size: 10pt; color: #666;">
            This summary reflects data as of December 31, 2025
        </p>
    </footer>
</section>
```

### Multi-Column Section

```html
<section title="Feature Comparison">
    <header>
        <h1>Feature Comparison</h1>
    </header>

    <div style="column-count: 2; column-gap: 20pt; column-rule: 1pt solid #ddd;">
        <h3>Basic Plan</h3>
        <ul>
            <li>Feature A</li>
            <li>Feature B</li>
            <li>Feature C</li>
        </ul>

        <h3>Professional Plan</h3>
        <ul>
            <li>All Basic features</li>
            <li>Feature D</li>
            <li>Feature E</li>
            <li>Priority Support</li>
        </ul>

        <h3>Enterprise Plan</h3>
        <ul>
            <li>All Professional features</li>
            <li>Feature F</li>
            <li>Custom Integration</li>
            <li>Dedicated Support</li>
        </ul>
    </div>

    <footer style="margin-top: 15pt; padding-top: 10pt; border-top: 1pt solid #ccc;">
        <p style="font-size: 9pt; text-align: center;">
            Contact sales for custom pricing
        </p>
    </footer>
</section>
```

### Section with Data Binding

```html
<!-- With model.chapters = [
    { id: 1, title: "Introduction", content: "...", author: "John" },
    { id: 2, title: "Methods", content: "...", author: "Jane" },
    { id: 3, title: "Results", content: "...", author: "Bob" }
] -->

<template data-bind="{{model.chapters}}">
    <section title="Chapter {{.id}}: {{.title}}">
        <header style="border-bottom: 2pt solid #336699; padding-bottom: 15pt; margin-bottom: 20pt;">
            <h1>Chapter {{.id}}: {{.title}}</h1>
            <p style="color: #666; font-size: 11pt;">Author: {{.author}}</p>
        </header>

        <div style="text-align: justify; line-height: 1.6;">
            {{.content}}
        </div>

        <footer style="margin-top: 30pt; padding-top: 15pt; border-top: 1pt solid #ccc; text-align: center;">
            <p style="font-size: 9pt; color: #999;">
                End of Chapter {{.id}}
            </p>
        </footer>
    </section>
</template>
```

### Academic Paper Structure

```html
<body>
    <!-- Title page -->
    <div style="text-align: center; padding-top: 100pt;">
        <h1 style="font-size: 24pt;">Research Paper Title</h1>
        <p style="font-size: 14pt; margin-top: 20pt;">Authors</p>
        <p style="font-size: 12pt;">Date</p>
    </div>

    <!-- Abstract (no page break) -->
    <section title="Abstract" style="page-break-before: always; page-break-after: auto;">
        <header>
            <h1>Abstract</h1>
        </header>
        <p style="text-align: justify;">
            This paper presents a comprehensive analysis...
        </p>
    </section>

    <!-- Main sections -->
    <section title="1. Introduction">
        <header>
            <h1>1. Introduction</h1>
        </header>
        <p style="text-align: justify;">
            The field of PDF generation has evolved...
        </p>
    </section>

    <section title="2. Literature Review">
        <header>
            <h1>2. Literature Review</h1>
        </header>
        <p style="text-align: justify;">
            Previous research in this area includes...
        </p>
    </section>

    <section title="3. Methodology">
        <header>
            <h1>3. Methodology</h1>
        </header>
        <p style="text-align: justify;">
            Our research methodology consisted of...
        </p>
    </section>

    <section title="4. Results">
        <header>
            <h1>4. Results</h1>
        </header>
        <p style="text-align: justify;">
            The results of our analysis show...
        </p>
    </section>

    <section title="5. Discussion">
        <header>
            <h1>5. Discussion</h1>
        </header>
        <p style="text-align: justify;">
            These findings have several implications...
        </p>
    </section>

    <section title="6. Conclusion">
        <header>
            <h1>6. Conclusion</h1>
        </header>
        <p style="text-align: justify;">
            In conclusion, this research demonstrates...
        </p>
    </section>

    <section title="References">
        <header>
            <h1>References</h1>
        </header>
        <ol style="font-size: 10pt;">
            <li>Author 1 (2024). Title of Paper...</li>
            <li>Author 2 (2023). Title of Paper...</li>
        </ol>
    </section>
</body>
```

### Technical Manual Sections

```html
<section title="Installation Guide">
    <header style="background-color: #336699; color: white; padding: 15pt;">
        <h1 style="margin: 0;">Installation Guide</h1>
    </header>

    <div style="padding: 20pt;">
        <h2>System Requirements</h2>
        <ul>
            <li>.NET 6.0 or higher</li>
            <li>Windows, Linux, or macOS</li>
            <li>Minimum 4GB RAM</li>
        </ul>

        <h2>Installation Steps</h2>
        <ol>
            <li>Download the installer</li>
            <li>Run the setup wizard</li>
            <li>Configure settings</li>
            <li>Verify installation</li>
        </ol>

        <div style="background-color: #fff3cd; border-left: 4pt solid #ffc107; padding: 10pt; margin: 15pt 0;">
            <strong>Note:</strong> Administrator privileges are required for installation.
        </div>
    </div>

    <footer style="background-color: #f0f0f0; padding: 10pt; font-size: 9pt;">
        For support, visit our website or contact support@example.com
    </footer>
</section>

<section title="Configuration">
    <header style="background-color: #336699; color: white; padding: 15pt;">
        <h1 style="margin: 0;">Configuration</h1>
    </header>

    <div style="padding: 20pt;">
        <h2>Basic Configuration</h2>
        <p>Configure the application using the settings file...</p>

        <h2>Advanced Options</h2>
        <p>For advanced users, additional options are available...</p>
    </div>
</section>
```

### Mixing Sections and Articles

```html
<section title="News and Updates">
    <header style="padding: 15pt; background-color: #f5f5f5;">
        <h1>News and Updates</h1>
        <p style="color: #666;">Latest from our team</p>
    </header>

    <!-- Articles within a section -->
    <article title="Update 1" style="margin: 20pt; padding: 15pt; border: 1pt solid #ddd; page-break-before: auto;">
        <header>
            <h2>New Feature Released</h2>
            <p style="font-size: 10pt; color: #666;">October 10, 2025</p>
        </header>
        <p>We're excited to announce...</p>
    </article>

    <article title="Update 2" style="margin: 20pt; padding: 15pt; border: 1pt solid #ddd; page-break-before: auto;">
        <header>
            <h2>Performance Improvements</h2>
            <p style="font-size: 10pt; color: #666;">October 8, 2025</p>
        </header>
        <p>Version 2.0 includes significant performance improvements...</p>
    </article>

    <article title="Update 3" style="margin: 20pt; padding: 15pt; border: 1pt solid #ddd; page-break-before: auto;">
        <header>
            <h2>Community Spotlight</h2>
            <p style="font-size: 10pt; color: #666;">October 5, 2025</p>
        </header>
        <p>This month we're highlighting contributions from...</p>
    </article>
</section>
```

---

## See Also

- [article](/reference/htmltags/article.html) - Article element for self-contained content
- [div](/reference/htmltags/div.html) - Generic block container without semantic meaning
- [header](/reference/htmltags/header.html) - Header element for introductory content
- [footer](/reference/htmltags/footer.html) - Footer element for closing content
- [aside](/reference/htmltags/aside.html) - Aside element for tangentially related content
- [nav](/reference/htmltags/nav.html) - Navigation element for navigation links
- [Panel Component](/reference/components/panel.html) - Base panel component in Scryber namespace
- [PDF Bookmarks](/reference/bookmarks/) - Creating document outlines and bookmarks
- [Document Structure](/reference/structure/) - Structuring PDF documents
- [Page Breaking](/reference/pagebreaking/) - Controlling page breaks in PDFs

---
