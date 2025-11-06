---
layout: default
title: del, ins, u, strike
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;del&gt;, &lt;ins&gt;, &lt;u&gt;, &lt;strike&gt;: Text Formatting Elements
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

Specialized inline elements for marking edits and changes to textual content for deletions, insertions and underlines.

- `<del>`: Deleted text (strikethrough)
- `<ins>`: Inserted text (underlined)
- `<u>`: Underlined text (presentational)
- `<strike>` (`s`): Strikethrough text (presentational)

---

## Usage

```html
<!-- Deleted text -->
<p>The price was <del>$100</del> $80.</p>

<!-- Inserted text -->
<p>My favorite color is <ins>blue</ins>.</p>

<!-- Underlined -->
<p>This is <u>underlined text</u>.</p>

<!-- Strikethrough -->
<p>This is <s>no longer valid</s>.</p>
```

---

## Supported Attributes

### Standard Attributes (All Elements)

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the element. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element. |

### Element-Specific Attributes

**`<del>` Element**:
| Attribute | Type | Description |
|-----------|------|-------------|
| `cite` | string | URL or reference explaining the deletion. |
| `datetime` | string | Date/time when the text was deleted. |

**`<ins>` Element**:
| Attribute | Type | Description |
|-----------|------|-------------|
| `cite` | string | URL or reference explaining the insertion. |
| `datetime` | string | Date/time when the text was inserted. |

---

## Default Styling


**`<del>`, `<s>`, `<strike>`**:
- Text decoration: Strikethrough

**`<ins>`, `<u>`**:
- Text decoration: Underline

---

## CSS Style Support

All elements support standard CSS properties:

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `color`, `text-decoration`, `text-transform`
- `letter-spacing`, `word-spacing`

**Background and Borders**:
- `background-color`, `background-image`
- `border`, `border-radius`, `padding`

**Visual Effects**:
- `opacity`, `text-shadow`, `transform`

**Display**:
- `display`: `inline` (default), `inline-block`, `block`, `none`
- `vertical-align`

---

## Examples

### Deleted Text

#### Price Changes

```html
<p>Was: <del>$100.00</del> Now: <strong style="color: red;">$79.99</strong></p>

<p>Original price: <del style="color: #999;">$149.99</del>
   Sale price: $99.99</p>
```

#### Document Revisions

```html
<p>The meeting is scheduled for <del>Tuesday</del> <ins>Wednesday</ins> at 2 PM.</p>

<p>Contact: <del>john@oldcompany.com</del> <ins>john@newcompany.com</ins></p>

<p>Status: <del>Pending</del> <ins>Approved</ins></p>
```

#### With Citation

```html
<p>
    The event will be held <del cite="management-memo-2025" datetime="2025-01-10">
    in the main hall</del> <ins>in the conference center</ins>.
</p>
```

#### Styled Deletions

```html
<style>
    .deleted {
        text-decoration: line-through;
        color: #999;
        background-color: #ffe6e6;
    }
</style>

<p>Price: <del class="deleted">$250</del> <strong>$180</strong></p>

<p>
    <del style="text-decoration: line-through; color: red;">
        Discontinued model
    </del>
</p>
```

### Inserted Text

#### Added Content

```html
<p>My favorite colors are blue, green, and <ins>purple</ins>.</p>

<p>Available in sizes: S, M, L, <ins>XL</ins></p>

<p>Features: Fast, Secure, <ins>and Reliable</ins></p>
```

#### Document Edits

```html
<p>Please <ins>carefully</ins> review the attached documents.</p>

<p>The deadline is <ins>Friday, January 20</ins>.</p>

<p>Send the report to <ins>manager@company.com</ins>.</p>
```

#### With Citation

```html
<p>
    <ins cite="update-notice" datetime="2025-01-15">
        New payment methods now accepted.
    </ins>
</p>
```

#### Styled Insertions

```html
<style>
    .inserted {
        text-decoration: underline;
        background-color: #e6ffe6;
        color: #006600;
    }
</style>

<p>New feature: <ins class="inserted">Dark mode support</ins></p>

<p>
    <ins style="background-color: #d4edda; border-left: 3pt solid #28a745;
    padding: 2pt 6pt;">
        Now available in your region
    </ins>
</p>
```

### Underlined Text

#### Basic Underline

```html
<p>This is <u>underlined text</u>.</p>

<p>Important: <u>Do not remove this label</u>.</p>

<p>The <u>underlined term</u> is defined in the glossary.</p>
```

#### Styled Underlines

```html
<p>
    This has a <u style="text-decoration-color: red;">red underline</u>.
</p>

<p>
    Special term: <u style="text-decoration: underline wavy;
    text-decoration-color: blue;">wavy underline</u>
</p>

<p>
    <u style="text-decoration-thickness: 2pt; text-decoration-color: green;">
        Thick green underline
    </u>
</p>
```

#### Chinese/Japanese Proper Names

```html
<!-- Traditional use of <u> for proper nouns in Chinese text -->
<p>The author <u>李白</u> was a famous poet.</p>
```

### Strikethrough Text

#### Completed Tasks

```html
<ul>
    <li><s>Write documentation</s></li>
    <li><s>Review code</s></li>
    <li>Deploy to production</li>
</ul>
```

#### No Longer Valid

```html
<p>Office hours: <s>9 AM - 5 PM</s> Now 24/7</p>

<p>Old website: <s>www.oldsite.com</s></p>

<p><s>Out of stock</s> Back in stock!</p>
```

---

## See Also

- [code, kbd, samp](html_code_kbd_samp_elements.html) - Computer related text elements
- [mark, sub, sup](html_mark_sub_sup_elements.html) - Text modification elements
- [strong, b, em, i](html_strong_em_elements.html) - Bold and italic text elements
- [span](html_span_elements.html) - Generic inline container
- [CSS Styles](/learning/styles/) - Complete CSS styling reference
- [Data Binding](/learning/binding/) - Data binding and expressions

---
