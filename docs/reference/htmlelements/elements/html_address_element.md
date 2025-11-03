---
layout: default
title: address
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;address&gt; : The Contact Information Element
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

The `<address>` element represents contact information for a person, organization, or document. It is a semantic block-level element designed for displaying contact details such as physical addresses, email addresses, phone numbers, and social media links with appropriate styling to distinguish this information from regular content.

---

## Usage

The `<address>` element creates a semantic container for contact information that:
- Represents contact information for the nearest article or body element
- Displays with italic font styling by default to distinguish contact information
- Functions as a block-level element taking full width
- Can contain inline elements, text, links, and line breaks
- Should not contain nested address elements
- Typically used in headers, footers, or article bylines
- Supports all CSS styling properties for customization
- Can generate PDF bookmarks/outlines when a `title` attribute is set

```html
<address>
    Written by <a href="mailto:john@example.com">John Smith</a><br/>
    123 Main Street, Suite 100<br/>
    New York, NY 10001
</address>
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
| `data-content` | expression | Dynamically sets the content of the address element from bound data. |
| `data-content-type` | Mime Type | Specifies the type of bound content fragment - XHTML; HTML; Markdown. |
| `data-content-action` | Replace, Append, Prepend | Specifies the action to take when binding elements with existing inner content. |

---

## CSS Style Support

The `<address>` element supports extensive CSS styling through the `style` attribute or CSS classes:

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

**Layout**:
- `page-break-before`, `page-break-after`, `page-break-inside`

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `color` (text color)
- `opacity`

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `text-align`, `text-decoration`, `text-transform`
- `line-height`, `letter-spacing`

---

## Notes

### Default Styling

The `<address>` element has default italic styling to distinguish contact information:

**HTML address (Scryber.Html.Components.HTMLAddress)**:
- **Font Style**: Italic
- **Display**: Block
- **Full Width**: Takes 100% of parent container width

The italic styling helps visually separate contact information from surrounding content while maintaining semantic meaning.

### Semantic Meaning and Purpose

The `<address>` element provides semantic meaning to contact information:

1. **Contact Information**: Contains contact details for a person, organization, or entity
2. **Scope**: Applies to the nearest article ancestor or the entire document if in body
3. **Authorship**: Often used to identify the author of an article or document
4. **Not for All Addresses**: Should only be used for contact information, not arbitrary postal addresses in content

### When to Use Address

Use `<address>` when:
- Providing author contact information in an article
- Displaying organization contact details in a footer
- Showing contact information for a business or entity
- Including contact details in a byline or author bio

Do NOT use `<address>` for:
- Arbitrary postal addresses mentioned in content (use `<p>` instead)
- Mailing addresses in forms or shipping information
- Location descriptions that aren't contact information

### Common Patterns

Typical content for address elements includes:
- Email addresses (often with `mailto:` links)
- Phone numbers (often with `tel:` links)
- Physical addresses with line breaks
- Social media links
- Website URLs
- Fax numbers

### Formatting Contact Information

Use `<br/>` elements for line breaks within addresses to maintain proper formatting:

```html
<address>
    Company Name<br/>
    123 Street Address<br/>
    City, State ZIP<br/>
    Phone: (555) 123-4567
</address>
```

---

## Class Hierarchy

```c#
Scryber.Html.Components.HTMLAddress, Scryber.Components
```

In the library codebase:
- `HTMLAddress` extends `HTMLDiv` extends `Panel` extends `VisualComponent`
- Inherits all container functionality from `HTMLDiv`
- Applies italic font styling by default


```c#
using Scryber.Components;
using Scryber.HTML.Components;

var addr = new HTMLAddress();
addr.ID = "sendTo";
addr.Contents.AddRange(
    new TextLiteral("First Line of Address"), new LineBreak(),
    new TextLiteral("Second Line"), new LineBreak()
    );
addr.StyleClass = "postal-address";
//page.Contents.Add(addr);
```
---


## Examples

### Basic Contact Information

```html
<address>
    Contact: <a href="mailto:info@example.com">info@example.com</a><br/>
    Phone: (555) 123-4567
</address>
```

### Author Information in Article

```html
<article>
    <h1>Understanding PDF Generation</h1>

    <p>Article content goes here...</p>

    <address>
        Written by <strong>Jane Smith</strong><br/>
        Email: <a href="mailto:jane.smith@example.com">jane.smith@example.com</a><br/>
        Published: October 13, 2025
    </address>
</article>
```

### Company Contact Information in Footer

```html
<footer style="background-color: #f5f5f5; padding: 20pt; margin-top: 30pt;">
    <address style="font-style: normal;">
        <strong style="font-size: 12pt;">ABC Corporation</strong><br/>
        123 Business Park Drive<br/>
        Suite 200<br/>
        New York, NY 10001<br/>
        <br/>
        Phone: <a href="tel:+15551234567">(555) 123-4567</a><br/>
        Email: <a href="mailto:contact@abccorp.com">contact@abccorp.com</a><br/>
        Web: <a href="https://www.abccorp.com">www.abccorp.com</a>
    </address>
</footer>
```

### Multi-Location Contact Information

```html
<div style="column-count: 2; column-gap: 20pt;">
    <div>
        <h3>New York Office</h3>
        <address>
            123 Broadway<br/>
            New York, NY 10001<br/>
            Phone: (212) 555-1234<br/>
            Fax: (212) 555-1235
        </address>
    </div>

    <div>
        <h3>Los Angeles Office</h3>
        <address>
            456 Sunset Blvd<br/>
            Los Angeles, CA 90028<br/>
            Phone: (323) 555-5678<br/>
            Fax: (323) 555-5679
        </address>
    </div>
</div>
```

### Contact Card with Styling

```html
<address style="background-color: #f0f8ff; border-left: 4pt solid #336699;
                padding: 15pt; font-style: normal; margin: 20pt 0;">
    <div style="font-size: 14pt; font-weight: bold; margin-bottom: 8pt; color: #336699;">
        Contact Information
    </div>
    <div style="line-height: 1.6;">
        <strong>John Doe</strong><br/>
        Senior Developer<br/>
        <br/>
        <span style="color: #666;">Email:</span> john.doe@techcorp.com<br/>
        <span style="color: #666;">Phone:</span> +1 (555) 123-4567<br/>
        <span style="color: #666;">Office:</span> Building A, Room 301
    </div>
</address>
```

### Author Byline

```html
<article>
    <header>
        <h1>The Future of Technology</h1>
        <address style="font-size: 10pt; color: #666;">
            By <a href="mailto:alice@techblog.com" style="color: #336699;">Alice Johnson</a>
            on October 13, 2025
        </address>
    </header>

    <p>Article content...</p>
</article>
```

### Business Card Style

```html
<address style="border: 2pt solid #333; padding: 20pt;
                width: 300pt; background-color: #fff;
                font-style: normal; text-align: center;">
    <div style="font-size: 16pt; font-weight: bold; margin-bottom: 5pt;">
        JOHN DOE
    </div>
    <div style="font-size: 11pt; color: #666; margin-bottom: 15pt;">
        Chief Executive Officer
    </div>
    <div style="border-top: 1pt solid #ddd; padding-top: 15pt;
                font-size: 9pt; line-height: 1.6; text-align: left;">
        ABC Corporation<br/>
        123 Main Street, Suite 100<br/>
        New York, NY 10001<br/>
        <br/>
        <a href="tel:+15551234567">+1 (555) 123-4567</a><br/>
        <a href="mailto:john.doe@abccorp.com">john.doe@abccorp.com</a>
    </div>
</address>
```

### Contact with Social Media Icons

```html
<!-- Link required to the font-awesome ttf file 
 
 -->
<address style="font-style: normal; background-color: #f9f9f9;
                padding: 15pt; border-radius: 5pt;">
    <strong style="font-size: 12pt;">Get in Touch</strong><br/>
    <br/>
    <div style="line-height: 1.8;">
        <i class='far fa-envelope'></i> <a href="mailto:hello@company.com">hello@company.com</a><br/>
        <i class="fab fa-twitter-square"></i> <a href="https://twitter.com/company">twitter account</a><br/>
        <i class="fab fa-linkedin"></i> <a href="https://linkedin.com/company/company">linked-in account</a><br/>
        <i class="fas fa-phone-square"></i> <a href="tel:+15551234567">(555) 123-4567</a>
    </div>
</address>
```


### Data Binding with Contact Information

```html
{% raw %}<!-- With model.contacts = [
    { name: "Sales", email: "sales@company.com", phone: "(555) 100-0001" },
    { name: "Support", email: "support@company.com", phone: "(555) 100-0002" }
] -->

<template data-bind="{{model.contacts}}">
    <div style="margin-bottom: 15pt;">
        <h3>{{.name}} Department</h3>
        <address style="font-style: normal;">
            Email: <a href="mailto:{{.email}}">{{.email}}</a><br/>
            Phone: {{.phone}}
        </address>
    </div>
</template>
{% endraw %}```


### Legal/Copyright Notice with Contact

```html
<footer style="border-top: 1pt solid #ddd; padding-top: 20pt; margin-top: 40pt;">
    <div style="font-size: 9pt; color: #666; margin-bottom: 10pt;">
        © 2025 Company Name. All rights reserved.
    </div>
    <address style="font-size: 9pt; font-style: normal; color: #666;">
        For licensing inquiries: <a href="mailto:legal@company.com" style="color: #336699;">legal@company.com</a><br/>
        Legal Department, 123 Legal Plaza, New York, NY 10001
    </address>
</footer>
```

### International Contact Information

```html
<div style="column-count: 3; column-gap: 15pt; font-size: 9pt;">
    <div style='break-inside: avoid'>
        <strong>USA</strong>
        <address>
            New York Office<br/>
            123 Broadway<br/>
            New York, NY 10001<br/>
            +1 (555) 123-4567
        </address>
    </div>

    <div style='break-inside: avoid; break-before: always;'>
        <strong>UK</strong>
        <address>
            London Office<br/>
            45 Fleet Street<br/>
            London EC4Y 1AA<br/>
            +44 20 1234 5678
        </address>
    </div>

    <div style='break-inside: avoid; break-before: always;'>
        <strong>Japan</strong>
        <address>
            Tokyo Office<br/>
            1-2-3 Shibuya<br/>
            Tokyo 150-0002<br/>
            +81 3 1234 5678
        </address>
    </div>
</div>
```

---

## See Also

- [p](html_p_element.html) - Paragraph element for general content
- [footer](html_footer_element.html) - Footer element often containing address information
- [article](html_article_element.html) - Article element that can contain author address
- [a](html_a_element.html) - Anchor element for email and phone links
- [br](html_br_element.html) - Line break element for formatting addresses
- [div](html_div_element.html) - Generic block container
- [Fonts and Typefaces](/learning/styles/fonts.html) - Using fonts and typefaces in the library

---
