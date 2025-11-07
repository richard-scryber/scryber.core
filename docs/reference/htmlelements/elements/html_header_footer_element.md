---
layout: default
title: header and footer
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;header&gt; and &lt;footer&gt; : Header and Footer Elements
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
The `<header>` and `<footer>` elements represent introductory and closing content respectively. They are semantic block-level elements used to define headers and footers within containing elements like `<article>`, `<section>`, and `<body>`. These elements have special positioning behavior when used as direct children of certain containers.

## Usage

The `<header>` and `<footer>` elements create semantic containers that:
- Represent introductory content (header) or closing content (footer)
- Take full width of their parent container by default
- **Automatically position at the top (header) or bottom (footer) of parent containers** like `<article>` and `<section>`
- Support all CSS styling properties for positioning, sizing, colors, borders, and backgrounds
- Can generate PDF bookmarks/outlines when a `title` attribute is set
- Behave like `<div>` elements but with semantic meaning and automatic positioning
- Can contain headings, navigation, metadata, logos, author information, and more

```html
<article>
    <header style="background-color: #336699; color: white; padding: 15pt;">
        <h1>Article Title</h1>
        <p>Published: October 13, 2025</p>
    </header>

    <p>Main article content goes here...</p>

    <footer style="background-color: #f5f5f5; padding: 10pt; text-align: center;">
        <p>Author: John Doe | License: CC BY 4.0</p>
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
| `title` | string | Sets the outline/bookmark title in the PDF document. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-bind` | expression | Binds the element to a data context for use with templates. |
| `data-content` | expression | Dynamically sets the content from bound data. |

### CSS Style Support

Both `<header>` and `<footer>` elements support extensive CSS styling:

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

The `<header>` and `<footer>` elements provide semantic structure:

**Header Uses:**
1. **Article/Section Headers**: Introductory content with title, metadata, author information
2. **Page Headers**: Masthead, logo, site navigation, search functionality
3. **Document Headers**: Title page, document metadata, table of contents
4. **Group Headers**: Introduction to a group of content

**Footer Uses:**
1. **Article/Section Footers**: Author bio, publication date, tags, related links
2. **Page Footers**: Copyright, legal notices, contact information, site links
3. **Document Footers**: References, appendices, glossary
4. **Group Footers**: Summary or conclusion of grouped content

### Automatic Positioning Behavior

**IMPORTANT**: When `<header>` and `<footer>` are direct children of `<article>` or `<section>` elements:

1. **Headers move to the top** of their parent container
2. **Footers move to the bottom** of their parent container
3. This happens automatically during the pre-layout phase
4. Multiple headers/footers maintain their relative order
5. Other content remains in its original position between headers and footers

```html
<article>
    <p>This content will appear in the middle</p>
    <footer>I automatically move to the bottom</footer>
    <p>This content also appears in the middle</p>
    <header>I automatically move to the top</header>
</article>

<!-- Rendered order:
     1. header (moved to top)
     2. First paragraph
     3. Second paragraph
     4. footer (moved to bottom)
-->
```

### When to Use Header/Footer vs. Div

Use `<header>` when:
- Content serves as introductory material for its parent
- You're creating a title section with metadata
- You're defining a masthead or page header
- The semantic meaning of "introduction" or "heading" applies

Use `<footer>` when:
- Content serves as closing material for its parent
- You're adding author information, copyright, or metadata
- You're creating page footers with legal notices
- The semantic meaning of "conclusion" or "ending" applies

Use `<div>` when:
- You need generic containers without semantic meaning
- The content doesn't specifically serve as introduction or conclusion

### Special Layout Behaviors

**Header Special Styles** (from source code):
- `full-width: true` - Always takes full width of parent
- `display: block` - Block-level display mode
- `overflow-action: none` - Content doesn't overflow to next page
- `overflow-split: never` - Never splits across page boundaries
- `repeat-header: true` - **Headers repeat on continuation pages**

**Footer Special Styles** (from source code):
- `full-width: true` - Always takes full width of parent
- `display: block` - Block-level display mode
- `overflow-action: none` - Content doesn't overflow to next page
- `overflow-split: never` - Never splits across page boundaries

**Key Implication**: Headers and footers are designed to stay together and not break across pages.

### Page Header and Footer Positioning

While `<header>` and `<footer>` inside `<article>` or `<section>` are positioned within those containers, you can also use them for document-level page headers and footers by placing them at the document level or using CSS positioning.

### Class Hierarchy

In the Scryber codebase:
- `HTMLComponentHeader` extends `Panel` extends `VisualComponent`
- `HTMLComponentFooter` extends `Panel` extends `VisualComponent`
- Both provide special layout behaviors via `GetBaseStyle()` override
- Tagged with `[PDFParsableComponent("header")]` and `[PDFParsableComponent("footer")]`

### Default Behavior

**Header:**
1. **Block Display**: Full-width block element
2. **Top Position**: Automatically moves to top of parent article/section
3. **No Overflow**: Cannot overflow to next page
4. **Repeating**: Repeats on continuation pages (when parent content spans multiple pages)

**Footer:**
1. **Block Display**: Full-width block element
2. **Bottom Position**: Automatically moves to bottom of parent article/section
3. **No Overflow**: Cannot overflow to next page
4. **Non-Repeating**: Does not repeat on continuation pages

---

## Examples

### Basic Article with Header and Footer

```html
<article style="padding: 20pt;">
    <header style="border-bottom: 2pt solid #336699; padding-bottom: 15pt; margin-bottom: 20pt;">
        <h1 style="margin: 0; color: #336699;">Understanding PDF Generation</h1>
        <p style="margin: 5pt 0 0 0; color: #666; font-size: 10pt;">
            By Jane Smith | Published October 13, 2025 | 5 min read
        </p>
    </header>

    <p>The main content of the article appears here between the header and footer...</p>
    <p>More content paragraphs...</p>

    <footer style="border-top: 1pt solid #ddd; padding-top: 15pt; margin-top: 20pt; font-size: 9pt; color: #666;">
        <p style="margin: 0;">
            <strong>Tags:</strong> PDF, Tutorial, Scryber
        </p>
        <p style="margin: 5pt 0 0 0;">
            © 2025 Tech Publications. All rights reserved.
        </p>
    </footer>
</article>
```

### Document Header with Logo and Navigation

```html
<header style="background-color: #2c3e50; color: white; padding: 20pt;">
    <div style="display: table; width: 100%;">
        <div style="display: table-cell; vertical-align: middle; width: 150pt;">
            <img src="images/logo.png" style="height: 40pt;" />
        </div>
        <div style="display: table-cell; vertical-align: middle; text-align: right;">
            <span style="margin-left: 20pt;">Home</span>
            <span style="margin-left: 20pt;">Products</span>
            <span style="margin-left: 20pt;">About</span>
            <span style="margin-left: 20pt;">Contact</span>
        </div>
    </div>
</header>
```

### Article Header with Multiple Metadata Lines

```html
<article>
    <header style="background: linear-gradient(to bottom, #667eea, #764ba2); color: white; padding: 30pt; text-align: center;">
        <div style="font-size: 10pt; font-weight: bold; letter-spacing: 2pt; margin-bottom: 10pt;">
            FEATURED ARTICLE
        </div>
        <h1 style="margin: 0; font-size: 28pt; margin-bottom: 15pt;">
            The Future of Document Generation
        </h1>
        <div style="font-size: 11pt; opacity: 0.9;">
            <span>John Developer</span>
            <span style="margin: 0 10pt;">•</span>
            <span>October 13, 2025</span>
            <span style="margin: 0 10pt;">•</span>
            <span>10 minute read</span>
        </div>
    </header>

    <div style="padding: 20pt;">
        <p>Article content goes here...</p>
    </div>
</article>
```

### Section Header with Chapter Number

```html
<section title="Chapter 3: Advanced Topics">
    <header style="page-break-before: always; padding: 40pt 20pt 20pt 20pt;">
        <div style="font-size: 48pt; font-weight: bold; color: #336699; margin-bottom: 10pt;">
            03
        </div>
        <h1 style="margin: 0; font-size: 32pt; color: #333;">Advanced Topics</h1>
        <div style="width: 100pt; height: 3pt; background-color: #336699; margin-top: 15pt;"></div>
    </header>

    <div style="padding: 0 20pt;">
        <p>Chapter content...</p>
    </div>
</section>
```

### Footer with Author Bio

```html
<article>
    <header>
        <h1>Machine Learning in Practice</h1>
    </header>

    <p>Main article content...</p>

    <footer style="background-color: #f9f9f9; padding: 20pt; margin-top: 30pt; border-top: 2pt solid #336699;">
        <div style="display: table; width: 100%;">
            <div style="display: table-cell; width: 80pt; vertical-align: top;">
                <img src="images/author.jpg" style="width: 70pt; height: 70pt; border-radius: 35pt;" />
            </div>
            <div style="display: table-cell; vertical-align: top; padding-left: 15pt;">
                <h3 style="margin: 0 0 5pt 0; color: #336699;">About the Author</h3>
                <p style="margin: 0; font-size: 10pt; color: #666;">
                    <strong>Dr. Sarah Johnson</strong> is a machine learning researcher and educator with over 15 years of experience in the field. She currently leads the AI research team at Tech Innovations Inc.
                </p>
            </div>
        </div>
    </footer>
</article>
```

### Multiple Headers and Footers

```html
<section>
    <!-- Multiple headers - will all move to top in order -->
    <header style="background-color: #336699; color: white; padding: 15pt; text-align: center;">
        <h1 style="margin: 0;">Document Title</h1>
    </header>

    <header style="background-color: #f0f0f0; padding: 10pt; font-size: 9pt; text-align: center;">
        <span>Confidential | Internal Use Only | © 2025</span>
    </header>

    <!-- Main content -->
    <div style="padding: 20pt;">
        <p>Document content goes here...</p>
    </div>

    <!-- Multiple footers - will all move to bottom in order -->
    <footer style="border-top: 1pt solid #ddd; padding-top: 10pt; font-size: 9pt;">
        <p style="margin: 0;"><strong>Disclaimer:</strong> Information subject to change.</p>
    </footer>

    <footer style="background-color: #f5f5f5; padding: 10pt; text-align: center; font-size: 8pt;">
        <p style="margin: 0;">For questions, contact support@example.com</p>
    </footer>
</section>
```

### Report Header with Company Info

```html
<header style="border-bottom: 3pt solid #336699; padding-bottom: 20pt; margin-bottom: 30pt;">
    <div style="display: table; width: 100%;">
        <div style="display: table-cell; width: 50%;">
            <img src="images/company-logo.png" style="height: 50pt;" />
        </div>
        <div style="display: table-cell; width: 50%; text-align: right; vertical-align: bottom;">
            <div style="font-size: 18pt; font-weight: bold; color: #336699;">
                Annual Financial Report
            </div>
            <div style="font-size: 11pt; color: #666; margin-top: 5pt;">
                Fiscal Year 2025
            </div>
        </div>
    </div>
</header>
```

### Document Footer with Page Number

```html
<footer style="position: absolute; bottom: 20pt; left: 20pt; right: 20pt; border-top: 1pt solid #ddd; padding-top: 10pt; font-size: 9pt; color: #666;">
    <div style="display: table; width: 100%;">
        <div style="display: table-cell; text-align: left;">
            © 2025 Company Name
        </div>
        <div style="display: table-cell; text-align: center;">
            Confidential
        </div>
        <div style="display: table-cell; text-align: right;">
            Page {{$page}} of {{$pages}}
        </div>
    </div>
</footer>
```

### Newsletter Header

```html
<header style="background-color: #fff; border-bottom: 4pt solid #ff6b6b;">
    <div style="text-align: center; padding: 20pt;">
        <h1 style="margin: 0; font-size: 32pt; color: #333; font-family: Georgia, serif;">
            Tech Weekly
        </h1>
        <p style="margin: 10pt 0 0 0; font-size: 12pt; color: #666; font-style: italic;">
            Your source for technology news and insights
        </p>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #999;">
            Issue #247 | October 13, 2025
        </p>
    </div>
</header>
```

### Legal Document Footer

```html
<footer style="background-color: #f8f8f8; padding: 20pt; margin-top: 40pt; font-size: 8pt; color: #666; line-height: 1.4;">
    <p style="margin: 0 0 10pt 0; font-weight: bold;">LEGAL NOTICE</p>
    <p style="margin: 0;">
        This document contains confidential and proprietary information. Any unauthorized review, use, disclosure, or distribution is prohibited. If you are not the intended recipient, please contact the sender and destroy all copies of this document.
    </p>
    <p style="margin: 10pt 0 0 0;">
        The information contained in this document is provided "as is" without warranty of any kind. The sender assumes no responsibility for errors or omissions in this document.
    </p>
</footer>
```

### Invoice Header

```html
<header style="margin-bottom: 30pt;">
    <div style="display: table; width: 100%;">
        <div style="display: table-cell; width: 50%; vertical-align: top;">
            <img src="images/company-logo.png" style="height: 40pt; margin-bottom: 15pt;" />
            <div style="font-size: 9pt; line-height: 1.5;">
                <strong>TechCorp Inc.</strong><br/>
                123 Business Street<br/>
                San Francisco, CA 94105<br/>
                Phone: (555) 123-4567<br/>
                Email: billing@techcorp.com
            </div>
        </div>
        <div style="display: table-cell; width: 50%; text-align: right; vertical-align: top;">
            <h1 style="margin: 0; font-size: 32pt; color: #336699;">INVOICE</h1>
            <div style="font-size: 10pt; margin-top: 10pt;">
                <strong>Invoice #:</strong> INV-2025-001234<br/>
                <strong>Date:</strong> October 13, 2025<br/>
                <strong>Due Date:</strong> November 13, 2025
            </div>
        </div>
    </div>
</header>
```

### Certificate Header

```html
<header style="text-align: center; padding: 40pt 20pt 20pt 20pt; border-bottom: 2pt solid gold;">
    <div style="font-size: 48pt; color: gold; margin-bottom: 10pt;">🏆</div>
    <h1 style="margin: 0; font-size: 36pt; font-family: Georgia, serif; color: #333; letter-spacing: 2pt;">
        CERTIFICATE
    </h1>
    <p style="margin: 10pt 0 0 0; font-size: 14pt; color: #666;">
        of Achievement
    </p>
</header>
```

### Data-Bound Article Header

```html
<!-- With model = { title: "My Article", author: "John", date: "2025-10-13", category: "Technology" } -->
<article>
    <header style="background-color: #f5f5f5; padding: 20pt; border-left: 5pt solid #336699;">
        <div style="font-size: 9pt; color: #336699; font-weight: bold; text-transform: uppercase; margin-bottom: 10pt;">
            {{model.category}}
        </div>
        <h1 style="margin: 0 0 10pt 0; color: #333;">{{model.title}}</h1>
        <div style="font-size: 10pt; color: #666;">
            By <strong>{{model.author}}</strong> | {{model.date}}
        </div>
    </header>

    <div style="padding: 20pt;">
        <p>Article content...</p>
    </div>
</article>
```

### Magazine Article Header

```html
<article>
    <header style="position: relative; height: 200pt; background-image: url('images/header-bg.jpg'); background-size: cover; background-position: center;">
        <div style="position: absolute; bottom: 0; left: 0; right: 0; background: linear-gradient(to top, rgba(0,0,0,0.8), transparent); padding: 30pt 20pt 20pt 20pt; color: white;">
            <div style="font-size: 10pt; font-weight: bold; margin-bottom: 10pt;">
                FEATURE STORY
            </div>
            <h1 style="margin: 0; font-size: 32pt; text-shadow: 2px 2px 4px rgba(0,0,0,0.5);">
                The Rise of AI in Modern Business
            </h1>
            <div style="margin-top: 10pt; font-size: 11pt; opacity: 0.9;">
                How artificial intelligence is transforming industries
            </div>
        </div>
    </header>

    <div style="padding: 20pt;">
        <p>Article content...</p>
    </div>
</article>
```

### Academic Paper Header

```html
<header style="text-align: center; padding: 40pt 60pt; font-family: 'Times New Roman', serif;">
    <h1 style="margin: 0 0 20pt 0; font-size: 18pt; font-weight: bold;">
        Applications of Machine Learning in Healthcare: A Systematic Review
    </h1>
    <p style="margin: 0 0 5pt 0; font-size: 12pt;">
        Sarah Johnson<sup>1</sup>, Michael Chen<sup>2</sup>, Emily Rodriguez<sup>1</sup>
    </p>
    <p style="margin: 0; font-size: 10pt; font-style: italic; color: #666;">
        <sup>1</sup>Department of Computer Science, Tech University<br/>
        <sup>2</sup>School of Medicine, Health Institute
    </p>
    <p style="margin: 20pt 0 0 0; font-size: 10pt; color: #666;">
        <strong>Correspondence:</strong> sarah.johnson@techuni.edu
    </p>
</header>
```

### Product Datasheet Header

```html
<header style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30pt;">
    <div style="display: table; width: 100%;">
        <div style="display: table-cell; width: 70%; vertical-align: middle;">
            <div style="font-size: 10pt; font-weight: bold; margin-bottom: 5pt; opacity: 0.9;">
                PRODUCT DATASHEET
            </div>
            <h1 style="margin: 0; font-size: 28pt;">Enterprise Server Pro</h1>
            <p style="margin: 10pt 0 0 0; font-size: 12pt; opacity: 0.9;">
                High-performance server solution for demanding workloads
            </p>
        </div>
        <div style="display: table-cell; width: 30%; text-align: right; vertical-align: middle;">
            <div style="background-color: white; color: #667eea; padding: 15pt; border-radius: 5pt; display: inline-block;">
                <div style="font-size: 10pt;">Model</div>
                <div style="font-size: 20pt; font-weight: bold;">ESP-5000</div>
            </div>
        </div>
    </div>
</header>
```

### Event Program Footer

```html
<footer style="background-color: #2c3e50; color: white; padding: 25pt; margin-top: 40pt;">
    <div style="text-align: center; margin-bottom: 15pt;">
        <h3 style="margin: 0 0 10pt 0;">Thank You for Attending!</h3>
        <p style="margin: 0; font-size: 11pt; opacity: 0.8;">
            We hope you enjoyed the Annual Tech Conference 2025
        </p>
    </div>
    <div style="border-top: 1pt solid rgba(255,255,255,0.2); padding-top: 15pt; display: table; width: 100%; font-size: 9pt;">
        <div style="display: table-cell; width: 33%; text-align: left;">
            <strong>Website</strong><br/>
            www.techconf.com
        </div>
        <div style="display: table-cell; width: 33%; text-align: center;">
            <strong>Email</strong><br/>
            info@techconf.com
        </div>
        <div style="display: table-cell; width: 33%; text-align: right;">
            <strong>Phone</strong><br/>
            (555) 123-4567
        </div>
    </div>
</footer>
```

### Resume Header

```html
<header style="background-color: #2c3e50; color: white; padding: 30pt;">
    <div style="display: table; width: 100%;">
        <div style="display: table-cell; width: 70%; vertical-align: middle;">
            <h1 style="margin: 0 0 5pt 0; font-size: 32pt;">JOHN DEVELOPER</h1>
            <p style="margin: 0; font-size: 14pt; color: #3498db;">
                Senior Software Engineer
            </p>
        </div>
        <div style="display: table-cell; width: 30%; vertical-align: middle; text-align: right; font-size: 10pt;">
            <div style="margin-bottom: 5pt;">john@developer.com</div>
            <div style="margin-bottom: 5pt;">(555) 123-4567</div>
            <div>San Francisco, CA</div>
        </div>
    </div>
</header>
```

### Quote Block with Footer Attribution

```html
<article style="margin: 30pt; padding: 30pt; border: 2pt solid #ddd;">
    <header style="margin-bottom: 20pt;">
        <h2 style="margin: 0; color: #336699;">Testimonial</h2>
    </header>

    <p style="font-size: 14pt; font-style: italic; line-height: 1.6; text-align: justify;">
        "Scryber has completely transformed how we generate reports. The combination of HTML familiarity with PDF precision is exactly what we needed. Our document generation time has been reduced by 75%, and the quality has never been better."
    </p>

    <footer style="margin-top: 20pt; padding-top: 15pt; border-top: 1pt solid #ddd;">
        <div style="display: table; width: 100%;">
            <div style="display: table-cell; width: 60pt; vertical-align: top;">
                <img src="images/client.jpg" style="width: 50pt; height: 50pt; border-radius: 25pt;" />
            </div>
            <div style="display: table-cell; vertical-align: middle;">
                <div style="font-weight: bold; color: #333;">Emily Rodriguez</div>
                <div style="font-size: 10pt; color: #666;">CTO, TechCorp Industries</div>
                <div style="font-size: 9pt; color: #999; margin-top: 3pt;">Fortune 500 Company</div>
            </div>
        </div>
    </footer>
</article>
```

### Blog Post with Meta Footer

```html
<article style="padding: 20pt;">
    <header>
        <h1>10 Tips for Better PDF Generation</h1>
        <p style="color: #666; font-size: 10pt;">
            Published October 13, 2025 by John Blogger
        </p>
    </header>

    <div style="margin: 20pt 0;">
        <p>Blog post content goes here...</p>
    </div>

    <footer style="background-color: #f9f9f9; padding: 15pt; border-radius: 5pt;">
        <div style="margin-bottom: 15pt;">
            <strong style="color: #336699;">Tags:</strong>
            <span style="display: inline-block; background-color: #e0e0e0; padding: 3pt 8pt; margin: 0 5pt; border-radius: 3pt; font-size: 9pt;">PDF</span>
            <span style="display: inline-block; background-color: #e0e0e0; padding: 3pt 8pt; margin: 0 5pt; border-radius: 3pt; font-size: 9pt;">Tutorial</span>
            <span style="display: inline-block; background-color: #e0e0e0; padding: 3pt 8pt; margin: 0 5pt; border-radius: 3pt; font-size: 9pt;">Scryber</span>
        </div>
        <div style="padding-top: 15pt; border-top: 1pt solid #ddd; font-size: 10pt; color: #666;">
            <div style="display: table; width: 100%;">
                <div style="display: table-cell; text-align: left;">
                    <span>👁 1,234 views</span>
                </div>
                <div style="display: table-cell; text-align: center;">
                    <span>💬 45 comments</span>
                </div>
                <div style="display: table-cell; text-align: right;">
                    <span>👍 89 likes</span>
                </div>
            </div>
        </div>
    </footer>
</article>
```

### Chapter Header with Decorative Border

```html
<section title="Chapter 5: Conclusion">
    <header style="position: relative; padding: 40pt 20pt 30pt 20pt; margin-bottom: 30pt;">
        <!-- Decorative corner elements -->
        <div style="position: absolute; top: 0; left: 0; width: 40pt; height: 40pt; border-top: 3pt solid #336699; border-left: 3pt solid #336699;"></div>
        <div style="position: absolute; top: 0; right: 0; width: 40pt; height: 40pt; border-top: 3pt solid #336699; border-right: 3pt solid #336699;"></div>

        <div style="text-align: center;">
            <div style="font-size: 14pt; color: #336699; font-weight: bold; letter-spacing: 3pt; margin-bottom: 15pt;">
                CHAPTER FIVE
            </div>
            <h1 style="margin: 0; font-size: 36pt; color: #333; font-family: Georgia, serif;">
                Conclusion
            </h1>
            <div style="width: 80pt; height: 2pt; background-color: #336699; margin: 20pt auto 0 auto;"></div>
        </div>
    </header>

    <div style="padding: 0 20pt;">
        <p>Chapter content...</p>
    </div>
</section>
```

---

## See Also

- [article](/reference/htmltags/article.html) - Article element (automatically positions headers/footers)
- [section](/reference/htmltags/section.html) - Section element (automatically positions headers/footers)
- [div](/reference/htmltags/div.html) - Generic block container
- [aside](/reference/htmltags/aside.html) - Aside element for tangentially related content
- [nav](/reference/htmltags/nav.html) - Navigation element
- [Panel Component](/reference/components/panel.html) - Base panel component
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Data binding and expressions
- [Page Headers and Footers](/reference/page-headers-footers/) - Document-level page headers and footers
- [Layout](/reference/layout/) - Layout and positioning guide

---
