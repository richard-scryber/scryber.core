---
layout: default
title: aside
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;aside&gt; : The Aside Element
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

The `<aside>` element represents content that is tangentially related to the content around it. It is a semantic block-level element typically used for sidebars, pull quotes, callout boxes, glossaries, related links, and supplementary information that enhances but is separate from the main content.

---

## Usage

The `<aside>` element creates a semantic container that:
- Represents content tangentially related to the surrounding content
- Takes full width of its parent container by default
- Behaves identically to `<div>` in terms of layout and styling
- Provides semantic meaning for supplementary or sidebar content
- Supports PDF bookmarks/outlines via the `title` attribute
- Can contain any type of content including text, images, lists, and other elements
- Supports all CSS styling properties for positioning, sizing, colors, borders, and backgrounds
- Ideal for sidebars, callouts, pull quotes, and related information boxes

```html
<aside style="background-color: #f0f0f0; padding: 15pt; margin: 10pt 0; border-left: 4pt solid #336699;">
    <h3>Did You Know?</h3>
    <p>This is supplementary information related to the main content.</p>
</aside>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title in the PDF document. Asides with titles appear in PDF bookmarks. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-content` | expression | Dynamically sets the content of the address element from bound data. |
| `data-content-type` | Mime Type | Specifies the type of bound content fragment - XHTML; HTML; Markdown. |
| `data-content-action` | Replace, Append, Prepend | Specifies the action to take when binding elements with existing inner content. |

---

## CSS Style Support

The `<aside>` element supports extensive CSS styling through the `style` attribute or CSS classes:

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

The `<aside>` element provides semantic meaning to your document structure:

1. **Tangential Content**: Use `<aside>` for content that is related to but separate from the main content
2. **Supplementary Information**: Perfect for additional context, explanations, or related facts
3. **Sidebars**: Ideal for sidebar content that complements the main article
4. **Callout Boxes**: Great for highlighting important notes, tips, warnings, or related information
5. **Pull Quotes**: Can be used for extracting and emphasizing quotes from the main content

### When to Use Aside vs. Div

Use `<aside>` when:
- Content is tangentially related to the surrounding content
- You're creating sidebars with related information
- You're adding callout boxes, tips, notes, or warnings
- You're including pull quotes or highlighted excerpts
- You're adding glossary definitions or explanatory notes
- The content could be removed without affecting the main narrative flow

Use `<div>` when:
- You need a generic container for styling or layout purposes
- The content is part of the main content flow
- You're creating purely presentational groupings with no semantic meaning

### How Aside Differs from Div

**Behavioral Differences:**
- **Layout**: Both `<aside>` and `<div>` behave identically in terms of layout
- **Styling**: Both support the same CSS properties and styling
- **Rendering**: Both use the same layout engine and produce identical visual output

**Semantic Differences:**
- **Meaning**: `<aside>` conveys that content is supplementary or tangential
- **Purpose**: `<aside>` indicates the content relationship to surrounding material
- **Accessibility**: Semantic meaning helps with document structure understanding
- **Best Practice**: Use `<aside>` when the semantic meaning applies, `<div>` otherwise

---

### Default Behavior

The `<aside>` element has the following default behavior:

1. **Block Display**: Displays as a block-level element (stacks vertically)
2. **Full Width**: Takes 100% of the parent container's width by default
3. **Static Position**: Uses normal document flow positioning
4. **No Special Styling**: No default visual styling (same as `<div>`)

### PDF Bookmarks

When you add a `title` attribute to an `<aside>`, it creates an entry in the PDF document outline:

```html
<aside title="Important Note">
    <p>This creates a bookmark in the PDF outline.</p>
</aside>
```

### Typical Styling Patterns

Common styling patterns for asides include:
- Background colors to distinguish from main content
- Border-left accents for visual emphasis
- Padding for comfortable reading
- Different font sizes or styles
- Float positioning for sidebar layouts
- Box shadows or borders for callout boxes

---

## Class Hierarchy

```c#
Scryber.Html.Components.HTMLAnchor, Scryber.Components
```

In the library codebase:
- `HTMLAside` extends `HTMLDiv` extends `Div` extends `Panel` extends `VisualComponent`
- Inherits all properties and behaviors from `HTMLDiv`
- Provides semantic meaning without changing layout behavior
- Identical rendering to `<div>` but with aside semantics


```c#
using Scryber.Components;
using Scryber.HTML.Components;

var aside = new HTMLAside();
aside.ID = "pushedRight";
addr.Contents.Add("NOTE: This content will appear on the right of the page, and flow with the content");

aside.Style.Position.Float = FloatMode.Right;
aside.Style.Padding.All = 10;
aside.Style.Margins.Left = 10;
//page.Contents.Add(aside);
```

---

## Examples

### Basic Callout Box

```html
<p>This is the main content of the document. It flows normally and contains the primary information.</p>

<aside style="background-color: #e7f3ff; padding: 15pt; margin: 15pt 0; border-left: 4pt solid #2196f3;">
    <strong>Note:</strong> This is supplementary information that provides additional context but isn't essential to the main narrative.
</aside>

<p>The main content continues here after the aside.</p>
```

### Sidebar with Float

```html
<article>
    <aside style="width: 200pt; float: right; margin-left: 15pt; margin-bottom: 15pt; background-color: #f9f9f9; padding: 15pt; border: 1pt solid #ddd;">
        <h3 style="margin-top: 0; color: #336699;">Related Topics</h3>
        <ul style="margin-bottom: 0; font-size: 10pt;">
            <li>Introduction to PDF</li>
            <li>Advanced Layouts</li>
            <li>Data Binding</li>
            <li>Best Practices</li>
        </ul>
    </aside>

    <h1>Main Article Title</h1>
    <p>The main article content flows around the floated aside. This creates a traditional sidebar layout where supplementary information appears to the side of the main content.</p>
    <p>Additional paragraphs continue to flow around the aside until the aside height is exceeded, then the content flows full width again.</p>
</article>
```

### Warning Callout

```html
<aside style="background-color: #fff3cd; border: 2pt solid #ffc107; padding: 15pt; margin: 15pt 0; border-radius: 5pt;">
    <div style="display: flex; align-items: center;">
        <span style="font-size: 24pt; margin-right: 10pt;">⚠️</span>
        <div>
            <strong style="color: #856404; font-size: 12pt;">Warning</strong>
            <p style="margin: 5pt 0 0 0; color: #856404;">
                This operation cannot be undone. Make sure you have a backup before proceeding.
            </p>
        </div>
    </div>
</aside>
```

### Tip Callout

```html
<aside style="background-color: #d4edda; border-left: 4pt solid #28a745; padding: 15pt; margin: 15pt 0;">
    <strong style="color: #155724; font-size: 11pt;">💡 Pro Tip</strong>
    <p style="margin: 5pt 0 0 0; color: #155724;">
        You can significantly improve performance by caching frequently used templates and reusing them across multiple documents.
    </p>
</aside>
```

### Information Box

```html
<aside style="background-color: #e7f3ff; border: 1pt solid #2196f3; padding: 15pt; margin: 15pt 0; border-radius: 5pt;">
    <div style="display: flex; align-items: flex-start;">
        <span style="font-size: 20pt; margin-right: 10pt; color: #2196f3;">ℹ️</span>
        <div>
            <strong style="color: #0c5460; font-size: 11pt;">Information</strong>
            <p style="margin: 5pt 0 0 0; color: #0c5460;">
                The PDF/A format is designed for long-term archiving and ensures document accessibility and preservation.
            </p>
        </div>
    </div>
</aside>
```

### Error Message

```html
<aside style="background-color: #f8d7da; border: 2pt solid #dc3545; padding: 15pt; margin: 15pt 0; border-radius: 5pt;">
    <div style="display: flex; align-items: center;">
        <span style="font-size: 24pt; margin-right: 10pt; color: #721c24;">❌</span>
        <div>
            <strong style="color: #721c24; font-size: 12pt;">Error</strong>
            <p style="margin: 5pt 0 0 0; color: #721c24;">
                The template file could not be found. Please check the file path and try again.
            </p>
        </div>
    </div>
</aside>
```

### Pull Quote

```html
<article>
    <p>The research team spent three years analyzing the data and conducting experiments. Their findings revolutionized the field and opened up new avenues for exploration.</p>

    <aside style="width: 40%; margin: 20pt auto; padding: 15pt; border-top: 2pt solid #336699; border-bottom: 2pt solid #336699; text-align: center; font-style: italic;">
        <p style="font-size: 14pt; margin: 0; color: #336699;">
            "Their findings revolutionized the field and opened up new avenues for exploration."
        </p>
    </aside>

    <p>The implications of this research continue to be felt across multiple disciplines, influencing both theoretical frameworks and practical applications.</p>
</article>
```

### Glossary Sidebar

```html
<div style="position: relative;">
    <aside style="position: absolute; right: 0; top: 0; width: 150pt; background-color: #f5f5f5; padding: 10pt; border: 1pt solid #ddd;">
        <h4 style="margin-top: 0; font-size: 10pt; color: #336699;">Glossary</h4>
        <dl style="font-size: 9pt;">
            <dt style="font-weight: bold; margin-top: 5pt;">PDF</dt>
            <dd style="margin-left: 10pt;">Portable Document Format</dd>

            <dt style="font-weight: bold; margin-top: 5pt;">HTML</dt>
            <dd style="margin-left: 10pt;">HyperText Markup Language</dd>

            <dt style="font-weight: bold; margin-top: 5pt;">CSS</dt>
            <dd style="margin-left: 10pt;">Cascading Style Sheets</dd>
        </dl>
    </aside>

    <div style="margin-right: 170pt;">
        <h1>Main Content</h1>
        <p>The main document content appears here with the glossary positioned absolutely to the right.</p>
    </div>
</div>
```

### Author Bio Sidebar

```html
<article>
    <h1>Understanding Modern PDF Generation</h1>

    <aside style="float: right; width: 180pt; margin: 0 0 15pt 15pt; background-color: #f9f9f9; padding: 15pt; border: 1pt solid #ddd; border-radius: 5pt;">
        <img src="images/author.jpg" style="width: 100%; border-radius: 50%; margin-bottom: 10pt;" />
        <h4 style="margin: 0 0 5pt 0; text-align: center;">About the Author</h4>
        <p style="font-size: 9pt; text-align: center; margin: 0; color: #666;">
            John Doe is a software architect with 15 years of experience in document generation and PDF technologies.
        </p>
    </aside>

    <p>PDF generation has evolved significantly over the past decade. Modern libraries enable developers to create sophisticated documents using familiar HTML and CSS syntax.</p>

    <p>This article explores the key concepts, best practices, and advanced techniques for generating professional PDFs from web templates.</p>
</article>
```

### Statistics Callout

```html
<aside style="background-color: #fff; border: 2pt solid #336699; padding: 20pt; margin: 20pt 0; text-align: center;">
    <div style="font-size: 36pt; font-weight: bold; color: #336699; margin-bottom: 5pt;">87%</div>
    <p style="font-size: 11pt; color: #666; margin: 0;">
        of enterprises use PDF for official documentation
    </p>
</aside>
```

### Quote with Attribution

```html
<aside style="background-color: #f5f5f5; border-left: 4pt solid #666; padding: 15pt; margin: 20pt 0; font-style: italic;">
    <p style="font-size: 13pt; margin: 0 0 10pt 0; line-height: 1.6;">
        "The best way to predict the future is to invent it."
    </p>
    <footer style="text-align: right; font-size: 10pt; font-style: normal; color: #666;">
        — Alan Kay, Computer Scientist
    </footer>
</aside>
```

### Definition Box

```html
<p>The concept of PDF/A is important for long-term document preservation.</p>

<aside style="background-color: #fffacd; border: 1pt solid #ffd700; padding: 15pt; margin: 15pt 0;">
    <h4 style="margin-top: 0; color: #b8860b;">Definition: PDF/A</h4>
    <p style="margin-bottom: 0; font-size: 10pt;">
        PDF/A is an ISO-standardized version of PDF specialized for use in archiving and long-term preservation of electronic documents. It prohibits features unsuitable for long-term archiving, such as font linking and encryption.
    </p>
</aside>

<p>Organizations should consider PDF/A when archival compliance is required.</p>
```

### Related Links Box

```html
<aside style="background-color: #e8f4f8; padding: 15pt; margin: 15pt 0; border-radius: 5pt;">
    <h3 style="margin-top: 0; color: #336699; font-size: 12pt;">🔗 Related Articles</h3>
    <ul style="margin-bottom: 0; line-height: 1.8; font-size: 10pt;">
        <li><a href="#basics" style="color: #336699;">Getting Started</a></li>
        <li><a href="#templates" style="color: #336699;">Working with Templates</a></li>
        <li><a href="#data-binding" style="color: #336699;">Data Binding Guide</a></li>
        <li><a href="#styling" style="color: #336699;">CSS Styling Reference</a></li>
    </ul>
</aside>
```

### Technical Specification Box

```html
<aside style="background-color: #f0f0f0; border: 1pt dashed #999; padding: 15pt; margin: 15pt 0; font-family: 'Courier New', monospace;">
    <h4 style="margin-top: 0; font-family: Arial, sans-serif;">Technical Specifications</h4>
    <table style="width: 100%; font-size: 9pt;">
        <tr>
            <td style="padding: 3pt; font-weight: bold;">Version:</td>
            <td style="padding: 3pt;">2.0.5</td>
        </tr>
        <tr>
            <td style="padding: 3pt; font-weight: bold;">Platform:</td>
            <td style="padding: 3pt;">.NET 6.0+</td>
        </tr>
        <tr>
            <td style="padding: 3pt; font-weight: bold;">License:</td>
            <td style="padding: 3pt;">MIT</td>
        </tr>
        <tr>
            <td style="padding: 3pt; font-weight: bold;">Size:</td>
            <td style="padding: 3pt;">2.5 MB</td>
        </tr>
    </table>
</aside>
```

### Data-Bound Tips

```html
<!-- With model.tips = [
    { icon: "lightbulb", title: "Performance", text: "Cache templates for better performance" },
    { icon: "lock", title: "Security", text: "Always sanitize user input" },
    { icon: "chart-simple", title: "Reporting", text: "Use data binding for dynamic reports" }
] -->

{% raw %}<template data-bind="{{model.tips}}">
    <aside style="background-color: #f0f8ff; border-left: 4pt solid #4a90e2; padding: 15pt; margin: 10pt 0;">
        <div style="display: flex; align-items: flex-start;">
            <i class='{{concat("fa-solid fa-",.icon)}}' style="font-size: 24pt; margin-right: 10pt;" ></i>
            <div>
                <strong style="color: #2c5282; font-size: 11pt;">{{.title}}</strong>
                <p style="margin: 5pt 0 0 0; color: #2c5282; font-size: 10pt;">{{.text}}</p>
            </div>
        </div>
    </aside>
</template>{% endraw %}
```

### Feature Highlight

```html
<aside style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 20pt; margin: 20pt 0; border-radius: 10pt;">
    <h3 style="margin-top: 0; color: white; font-size: 16pt;">✨ New Feature</h3>
    <p style="margin: 0 0 10pt 0; font-size: 11pt;">
        Introduction to advanced layout capabilities including CSS Grid and Flexbox support for complex document layouts.
    </p>
    <div style="background-color: rgba(255,255,255,0.2); padding: 10pt; border-radius: 5pt;">
        <a href="#learn-more" style="color: white; font-weight: bold; text-decoration: none;">
            Learn More →
        </a>
    </div>
</aside>
```

### Code Example Callout

```html
<aside style="background-color: #272822; color: #f8f8f2; padding: 15pt; margin: 15pt 0; border-radius: 5pt; font-family: 'Courier New', monospace;">
    <div style="color: #75715e; font-size: 9pt; margin-bottom: 8pt; font-family: Arial, sans-serif;">
        Example Code:
    </div>
    {% raw %}<pre style="margin: 0; font-size: 10pt; white-space: pre-wrap;">
var doc = Document.Parse("template.html");
doc.Params["title"] = "My Document";
doc.SaveAsPDF("output.pdf");
    </pre>{% endraw %}
</aside>
```

### Timeline Entry

```html
<aside style="border-left: 3pt solid #336699; padding-left: 15pt; margin: 15pt 0; position: relative;">
    <div style="position: absolute; left: -8pt; top: 0; width: 12pt; height: 12pt; background-color: #336699; border-radius: 6pt; border: 2pt solid white;"></div>
    <div style="font-weight: bold; color: #336699; margin-bottom: 5pt;">October 2025</div>
    <p style="margin: 0; font-size: 10pt; color: #666;">
        Version 9.0 released with major performance improvements and new layout features.
    </p>
</aside>
```

### Key Points Summary

```html
<aside style="background-color: #fff9e6; border: 2pt solid #ffd966; padding: 20pt; margin: 20pt 0;">
    <h3 style="margin-top: 0; color: #b8860b; text-align: center;">📋 Key Points</h3>
    <ul style="margin-bottom: 0; line-height: 1.8; font-size: 11pt; color: #5a4a00;">
        <li>Always validate input data before generating PDFs</li>
        <li>Use CSS classes for consistent styling across documents</li>
        <li>Leverage data binding for dynamic content</li>
        <li>Test with various data scenarios</li>
        <li>Optimize images for better performance</li>
    </ul>
</aside>
```

### Comparison Box

```html
<aside style="background-color: #f5f5f5; padding: 15pt; margin: 15pt 0; border: 1pt solid #ddd;">
    <h4 style="margin-top: 0; color: #336699;">Comparison: PDF vs. HTML</h4>
    <table style="width: 100%; font-size: 10pt; border-collapse: collapse;">
        <tr style="background-color: #336699; color: white;">
            <th style="padding: 8pt; text-align: left;">Feature</th>
            <th style="padding: 8pt; text-align: center;">PDF</th>
            <th style="padding: 8pt; text-align: center;">HTML</th>
        </tr>
        <tr>
            <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">Print Fidelity</td>
            <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: center;">✓</td>
            <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: center;">✗</td>
        </tr>
        <tr>
            <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">Dynamic Content</td>
            <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: center;">✗</td>
            <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: center;">✓</td>
        </tr>
        <tr>
            <td style="padding: 8pt;">Archival</td>
            <td style="padding: 8pt; text-align: center;">✓</td>
            <td style="padding: 8pt; text-align: center;">✗</td>
        </tr>
    </table>
</aside>
```

### Best Practice Callout

```html
<aside style="background-color: #f0fdf4; border: 2pt solid #22c55e; padding: 15pt; margin: 15pt 0; border-radius: 8pt;">
    <div style="display: flex; align-items: flex-start;">
        <span style="font-size: 24pt; margin-right: 10pt;">✓</span>
        <div>
            <strong style="color: #166534; font-size: 12pt;">Best Practice</strong>
            <p style="margin: 5pt 0 0 0; color: #166534; font-size: 10pt;">
                Always test your PDF templates with edge cases and boundary conditions to ensure robust document generation in production environments.
            </p>
        </div>
    </div>
</aside>
```

---

## See Also

- [div](html_div_element.html) - Generic block container (identical behavior, no semantic meaning)
- [article](html_article_element.html) - Article element for self-contained content
- [section](html_section_element.html) - Section element for thematic groupings
- [nav](html_nav_element.html) - Navigation element
- [blockquote](html_blockquote_element.html) - Block quotation element
- [CSS Styles](/learning/styles/) - Complete CSS styling reference
- [Data Binding](/learning/binding/) - Data binding and expressions

---
