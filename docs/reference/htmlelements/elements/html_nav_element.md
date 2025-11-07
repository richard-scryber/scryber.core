---
layout: default
title: nav
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;nav&gt; : The Navigation Element
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

The `<nav>` element represents a section of navigation links. It is a semantic block-level element specifically designed for major navigation blocks such as table of contents, site menus, page indexes, and cross-references.

## Usage

The `<nav>` element creates a semantic navigation container that:
- Represents a section containing navigation links
- Takes full width of its parent container by default
- Supports all CSS styling properties for positioning, sizing, colors, borders, and backgrounds
- Can generate PDF bookmarks/outlines when a `title` attribute is set
- Ideal for table of contents, menus, cross-references, and navigation structures
- Behaves like a `<div>` but conveys semantic meaning for document navigation
- Can contain lists, links, and other navigation-related content

```html
<nav title="Table of Contents">
    <h2>Contents</h2>
    <ul>
        <li><a href="#section1">Section 1</a></li>
        <li><a href="#section2">Section 2</a></li>
        <li><a href="#section3">Section 3</a></li>
    </ul>
</nav>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title in the PDF document. Nav elements with titles appear in PDF bookmarks. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-bind` | expression | Binds the element to a data context for use with templates. |
| `data-content` | expression | Dynamically sets the content of the nav from bound data. |

### CSS Style Support

The `<nav>` element supports extensive CSS styling through the `style` attribute or CSS classes:

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

The `<nav>` element provides semantic meaning to your document structure:

1. **Navigation Identification**: Clearly identifies navigation structures in your document
2. **Table of Contents**: Perfect for creating document-level navigation and TOCs
3. **Cross-References**: Ideal for linking to different sections within the same document
4. **Menu Structures**: Suitable for creating menu-like navigation interfaces
5. **Document Outline**: Can contribute to PDF bookmarks when using the `title` attribute

### When to Use Nav vs. Div

Use `<nav>` when:
- You're creating a table of contents for the document
- You're building a menu or navigation structure
- You're creating a list of cross-references to other sections
- You're building an index or directory of content
- The semantic meaning of "navigation" applies to the content

Use `<div>` when:
- You need a generic container for styling or layout purposes
- The content is not specifically for navigation
- You're creating purely presentational groupings

### When NOT to Use Nav

Don't use `<nav>` for:
- Simple lists of links that aren't primary navigation
- Footer links (copyright, legal, etc.)
- Social media links
- Advertising links
- Individual links scattered throughout content

Reserve `<nav>` for significant navigation blocks.

### PDF Context

In PDF documents, `<nav>` elements are particularly useful for:

1. **Table of Contents**: Creating interactive TOCs at the beginning of documents
2. **Chapter Navigation**: Providing links between chapters or sections
3. **Page Indexes**: Creating alphabetical or categorical indexes
4. **Cross-References**: Linking related sections or topics
5. **Bookmarks**: When given a `title`, nav can appear in PDF bookmarks

### Class Hierarchy

In the Scryber codebase:
- `HTMLNav` extends `HTMLDiv` extends `Div` extends `Panel` extends `VisualComponent`
- Inherits all properties and behaviors from `HTMLDiv`
- Provides semantic meaning without changing behavior
- Identical rendering to `<div>` but with navigation semantics

### Default Behavior

The `<nav>` element has the following default behavior:

1. **Block Display**: Displays as a block-level element (stacks vertically)
2. **Full Width**: Takes 100% of the parent container's width by default
3. **Static Position**: Uses normal document flow positioning
4. **No Special Layout**: Behaves identically to `<div>` in terms of layout

---

## Examples

### Basic Table of Contents

```html
<nav title="Table of Contents" style="border: 2pt solid #336699; padding: 20pt; margin-bottom: 30pt;">
    <h1 style="margin-top: 0; color: #336699;">Table of Contents</h1>

    <ul style="list-style-type: none; padding-left: 0;">
        <li style="margin-bottom: 8pt;">
            <a href="#chapter1" style="color: #336699; text-decoration: none;">
                <strong>Chapter 1:</strong> Introduction
            </a>
        </li>
        <li style="margin-bottom: 8pt;">
            <a href="#chapter2" style="color: #336699; text-decoration: none;">
                <strong>Chapter 2:</strong> Getting Started
            </a>
        </li>
        <li style="margin-bottom: 8pt;">
            <a href="#chapter3" style="color: #336699; text-decoration: none;">
                <strong>Chapter 3:</strong> Advanced Topics
            </a>
        </li>
        <li style="margin-bottom: 8pt;">
            <a href="#chapter4" style="color: #336699; text-decoration: none;">
                <strong>Chapter 4:</strong> Best Practices
            </a>
        </li>
    </ul>
</nav>

<section id="chapter1" title="Chapter 1: Introduction">
    <h1>Chapter 1: Introduction</h1>
    <p>Content for chapter 1...</p>
</section>

<section id="chapter2" title="Chapter 2: Getting Started">
    <h1>Chapter 2: Getting Started</h1>
    <p>Content for chapter 2...</p>
</section>
```

### Nested Navigation (Hierarchical TOC)

```html
<nav title="Contents" style="background-color: #f9f9f9; padding: 20pt; border-left: 4pt solid #336699;">
    <h2 style="margin-top: 0; color: #336699;">Contents</h2>

    <ul style="list-style-type: none; padding-left: 0;">
        <li style="margin-bottom: 10pt;">
            <a href="#part1" style="color: #336699; font-weight: bold; text-decoration: none;">
                Part I: Fundamentals
            </a>
            <ul style="list-style-type: none; padding-left: 20pt; margin-top: 5pt;">
                <li style="margin-bottom: 5pt;">
                    <a href="#chapter1" style="color: #666; text-decoration: none;">
                        Chapter 1: Basic Concepts
                    </a>
                </li>
                <li style="margin-bottom: 5pt;">
                    <a href="#chapter2" style="color: #666; text-decoration: none;">
                        Chapter 2: Core Features
                    </a>
                </li>
            </ul>
        </li>

        <li style="margin-bottom: 10pt;">
            <a href="#part2" style="color: #336699; font-weight: bold; text-decoration: none;">
                Part II: Advanced Topics
            </a>
            <ul style="list-style-type: none; padding-left: 20pt; margin-top: 5pt;">
                <li style="margin-bottom: 5pt;">
                    <a href="#chapter3" style="color: #666; text-decoration: none;">
                        Chapter 3: Complex Layouts
                    </a>
                </li>
                <li style="margin-bottom: 5pt;">
                    <a href="#chapter4" style="color: #666; text-decoration: none;">
                        Chapter 4: Optimization
                    </a>
                </li>
            </ul>
        </li>

        <li style="margin-bottom: 10pt;">
            <a href="#appendix" style="color: #336699; font-weight: bold; text-decoration: none;">
                Appendix
            </a>
        </li>
    </ul>
</nav>
```

### Multi-Column Navigation

```html
<nav title="Quick Reference" style="page-break-before: always; background-color: #f5f5f5; padding: 20pt;">
    <h1 style="text-align: center; color: #336699;">Quick Reference Guide</h1>

    <div style="column-count: 3; column-gap: 20pt; column-rule: 1pt solid #ddd; margin-top: 15pt;">
        <div>
            <h3 style="color: #336699;">HTML Elements</h3>
            <ul style="font-size: 10pt; line-height: 1.6;">
                <li><a href="#div">div</a></li>
                <li><a href="#span">span</a></li>
                <li><a href="#p">p</a></li>
                <li><a href="#h1">h1-h6</a></li>
                <li><a href="#article">article</a></li>
                <li><a href="#section">section</a></li>
            </ul>
        </div>

        <div>
            <h3 style="color: #336699;">Styling</h3>
            <ul style="font-size: 10pt; line-height: 1.6;">
                <li><a href="#colors">Colors</a></li>
                <li><a href="#fonts">Fonts</a></li>
                <li><a href="#borders">Borders</a></li>
                <li><a href="#spacing">Spacing</a></li>
                <li><a href="#layout">Layout</a></li>
            </ul>
        </div>

        <div>
            <h3 style="color: #336699;">Advanced</h3>
            <ul style="font-size: 10pt; line-height: 1.6;">
                <li><a href="#databinding">Data Binding</a></li>
                <li><a href="#templates">Templates</a></li>
                <li><a href="#expressions">Expressions</a></li>
                <li><a href="#components">Components</a></li>
            </ul>
        </div>
    </div>
</nav>
```

### Navigation with Page Numbers

```html
<nav title="Document Index" style="padding: 20pt;">
    <h1 style="border-bottom: 3pt solid #336699; padding-bottom: 10pt; color: #336699;">
        Document Index
    </h1>

    <table style="width: 100%; margin-top: 20pt; border-collapse: collapse;">
        <tbody>
            <tr style="border-bottom: 1pt solid #ddd;">
                <td style="padding: 10pt;">
                    <a href="#introduction" style="color: #336699; text-decoration: none;">Introduction</a>
                </td>
                <td style="padding: 10pt; text-align: right; color: #666;">1</td>
            </tr>
            <tr style="border-bottom: 1pt solid #ddd;">
                <td style="padding: 10pt;">
                    <a href="#methodology" style="color: #336699; text-decoration: none;">Methodology</a>
                </td>
                <td style="padding: 10pt; text-align: right; color: #666;">15</td>
            </tr>
            <tr style="border-bottom: 1pt solid #ddd;">
                <td style="padding: 10pt;">
                    <a href="#results" style="color: #336699; text-decoration: none;">Results and Analysis</a>
                </td>
                <td style="padding: 10pt; text-align: right; color: #666;">42</td>
            </tr>
            <tr style="border-bottom: 1pt solid #ddd;">
                <td style="padding: 10pt;">
                    <a href="#discussion" style="color: #336699; text-decoration: none;">Discussion</a>
                </td>
                <td style="padding: 10pt; text-align: right; color: #666;">78</td>
            </tr>
            <tr style="border-bottom: 1pt solid #ddd;">
                <td style="padding: 10pt;">
                    <a href="#conclusion" style="color: #336699; text-decoration: none;">Conclusion</a>
                </td>
                <td style="padding: 10pt; text-align: right; color: #666;">95</td>
            </tr>
        </tbody>
    </table>
</nav>
```

### Sidebar Navigation

```html
<div style="display: table; width: 100%;">
    <!-- Sidebar with navigation -->
    <nav style="display: table-cell; width: 150pt; background-color: #336699; color: white; padding: 20pt; vertical-align: top;">
        <h3 style="margin-top: 0; color: white;">Navigation</h3>

        <ul style="list-style-type: none; padding-left: 0;">
            <li style="margin-bottom: 10pt;">
                <a href="#home" style="color: white; text-decoration: none;">Home</a>
            </li>
            <li style="margin-bottom: 10pt;">
                <a href="#features" style="color: white; text-decoration: none;">Features</a>
            </li>
            <li style="margin-bottom: 10pt;">
                <a href="#documentation" style="color: white; text-decoration: none;">Documentation</a>
            </li>
            <li style="margin-bottom: 10pt;">
                <a href="#examples" style="color: white; text-decoration: none;">Examples</a>
            </li>
            <li style="margin-bottom: 10pt;">
                <a href="#support" style="color: white; text-decoration: none;">Support</a>
            </li>
        </ul>
    </nav>

    <!-- Main content area -->
    <div style="display: table-cell; padding: 20pt; vertical-align: top;">
        <h1>Main Content</h1>
        <p>The main document content goes here...</p>
    </div>
</div>
```

### Data-Bound Navigation

```html
<!-- With model.chapters = [
    { id: "intro", title: "Introduction", pageNum: 1 },
    { id: "chapter1", title: "Getting Started", pageNum: 5 },
    { id: "chapter2", title: "Advanced Features", pageNum: 25 }
] -->

<nav title="Table of Contents" style="border: 2pt solid #336699; padding: 20pt;">
    <h2 style="margin-top: 0; color: #336699; text-align: center;">Table of Contents</h2>

    <template data-bind="{{model.chapters}}">
        <div style="border-bottom: 1pt solid #eee; padding: 10pt 0; display: flex; justify-content: space-between;">
            <a href="#{{.id}}" style="color: #336699; text-decoration: none; flex-grow: 1;">
                {{.title}}
            </a>
            <span style="color: #666; margin-left: 20pt;">
                {{.pageNum}}
            </span>
        </div>
    </template>
</nav>
```

### Styled Navigation Menu

```html
<nav style="background: linear-gradient(to right, #336699, #5588bb); padding: 15pt; margin-bottom: 20pt;">
    <ul style="list-style-type: none; padding: 0; margin: 0; display: table; width: 100%;">
        <li style="display: table-cell; text-align: center; padding: 5pt;">
            <a href="#home" style="color: white; text-decoration: none; font-weight: bold; padding: 8pt 15pt; display: inline-block;">
                Home
            </a>
        </li>
        <li style="display: table-cell; text-align: center; padding: 5pt;">
            <a href="#products" style="color: white; text-decoration: none; font-weight: bold; padding: 8pt 15pt; display: inline-block;">
                Products
            </a>
        </li>
        <li style="display: table-cell; text-align: center; padding: 5pt;">
            <a href="#services" style="color: white; text-decoration: none; font-weight: bold; padding: 8pt 15pt; display: inline-block;">
                Services
            </a>
        </li>
        <li style="display: table-cell; text-align: center; padding: 5pt;">
            <a href="#about" style="color: white; text-decoration: none; font-weight: bold; padding: 8pt 15pt; display: inline-block;">
                About
            </a>
        </li>
        <li style="display: table-cell; text-align: center; padding: 5pt;">
            <a href="#contact" style="color: white; text-decoration: none; font-weight: bold; padding: 8pt 15pt; display: inline-block;">
                Contact
            </a>
        </li>
    </ul>
</nav>
```

### Alphabetical Index Navigation

```html
<nav title="Index" style="page-break-before: always;">
    <h1 style="text-align: center; color: #336699; border-bottom: 3pt solid #336699; padding-bottom: 15pt;">
        Alphabetical Index
    </h1>

    <div style="column-count: 2; column-gap: 30pt; margin-top: 20pt;">
        <h3 style="color: #336699;">A</h3>
        <ul style="list-style-type: none; padding-left: 10pt; margin-bottom: 15pt;">
            <li><a href="#article">Article Element</a></li>
            <li><a href="#aside">Aside Element</a></li>
            <li><a href="#attributes">Attributes</a></li>
        </ul>

        <h3 style="color: #336699;">B</h3>
        <ul style="list-style-type: none; padding-left: 10pt; margin-bottom: 15pt;">
            <li><a href="#bookmarks">Bookmarks</a></li>
            <li><a href="#borders">Borders</a></li>
        </ul>

        <h3 style="color: #336699;">C</h3>
        <ul style="list-style-type: none; padding-left: 10pt; margin-bottom: 15pt;">
            <li><a href="#css">CSS Styling</a></li>
            <li><a href="#columns">Columns</a></li>
            <li><a href="#components">Components</a></li>
        </ul>

        <h3 style="color: #336699;">D</h3>
        <ul style="list-style-type: none; padding-left: 10pt; margin-bottom: 15pt;">
            <li><a href="#databinding">Data Binding</a></li>
            <li><a href="#div">Div Element</a></li>
        </ul>

        <h3 style="color: #336699;">E</h3>
        <ul style="list-style-type: none; padding-left: 10pt; margin-bottom: 15pt;">
            <li><a href="#elements">Elements</a></li>
            <li><a href="#expressions">Expressions</a></li>
        </ul>
    </div>
</nav>
```

### Cross-Reference Navigation

```html
<section id="overview">
    <h1>Overview</h1>
    <p>This document covers multiple related topics...</p>

    <nav title="Related Topics" style="background-color: #f0f0f0; padding: 15pt; margin: 20pt 0; border-left: 4pt solid #336699;">
        <h3 style="margin-top: 0; color: #336699;">Related Topics</h3>
        <ul style="margin-bottom: 0;">
            <li><a href="#implementation">Implementation Details</a> - Technical implementation information</li>
            <li><a href="#examples">Examples</a> - Practical usage examples</li>
            <li><a href="#best-practices">Best Practices</a> - Recommended approaches</li>
            <li><a href="#troubleshooting">Troubleshooting</a> - Common issues and solutions</li>
        </ul>
    </nav>

    <p>Continue reading for more information...</p>
</section>
```

### Compact Navigation Bar

```html
<nav style="background-color: #333; color: white; padding: 10pt; text-align: center; font-size: 10pt;">
    <a href="#top" style="color: white; text-decoration: none; margin: 0 15pt;">Top</a>
    <span style="color: #666;">|</span>
    <a href="#contents" style="color: white; text-decoration: none; margin: 0 15pt;">Contents</a>
    <span style="color: #666;">|</span>
    <a href="#previous" style="color: white; text-decoration: none; margin: 0 15pt;">Previous</a>
    <span style="color: #666;">|</span>
    <a href="#next" style="color: white; text-decoration: none; margin: 0 15pt;">Next</a>
    <span style="color: #666;">|</span>
    <a href="#index" style="color: white; text-decoration: none; margin: 0 15pt;">Index</a>
</nav>
```

### Navigation with Icons/Bullets

```html
<nav title="Site Map" style="padding: 20pt; border: 2pt solid #336699;">
    <h2 style="color: #336699; margin-top: 0;">Site Map</h2>

    <div style="margin-top: 15pt;">
        <div style="margin-bottom: 15pt;">
            <span style="color: #336699; font-size: 14pt; margin-right: 8pt;">▶</span>
            <a href="#getting-started" style="color: #336699; text-decoration: none; font-weight: bold;">
                Getting Started
            </a>
            <ul style="list-style-type: none; padding-left: 30pt; margin-top: 8pt;">
                <li style="margin-bottom: 5pt;">
                    <span style="color: #999;">▸</span>
                    <a href="#installation" style="color: #666; text-decoration: none;">Installation</a>
                </li>
                <li style="margin-bottom: 5pt;">
                    <span style="color: #999;">▸</span>
                    <a href="#quick-start" style="color: #666; text-decoration: none;">Quick Start</a>
                </li>
            </ul>
        </div>

        <div style="margin-bottom: 15pt;">
            <span style="color: #336699; font-size: 14pt; margin-right: 8pt;">▶</span>
            <a href="#documentation" style="color: #336699; text-decoration: none; font-weight: bold;">
                Documentation
            </a>
            <ul style="list-style-type: none; padding-left: 30pt; margin-top: 8pt;">
                <li style="margin-bottom: 5pt;">
                    <span style="color: #999;">▸</span>
                    <a href="#api" style="color: #666; text-decoration: none;">API Reference</a>
                </li>
                <li style="margin-bottom: 5pt;">
                    <span style="color: #999;">▸</span>
                    <a href="#guides" style="color: #666; text-decoration: none;">Guides</a>
                </li>
            </ul>
        </div>
    </div>
</nav>
```

---

## See Also

- [article](/reference/htmltags/article.html) - Article element for self-contained content
- [section](/reference/htmltags/section.html) - Section element for thematic groupings
- [aside](/reference/htmltags/aside.html) - Aside element for tangentially related content
- [div](/reference/htmltags/div.html) - Generic block container
- [a](/reference/htmltags/a.html) - Anchor/link element
- [ul](/reference/htmltags/ul.html) - Unordered list element
- [ol](/reference/htmltags/ol.html) - Ordered list element
- [PDF Bookmarks](/reference/bookmarks/) - Creating document outlines and bookmarks
- [Document Structure](/reference/structure/) - Structuring PDF documents
- [Links and Navigation](/reference/links/) - Working with links in PDFs

---
