---
layout: default
title: cite, defn, q
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;cite&gt;, &lt;defn&gt;, &lt;q&gt; : Inline citataion, definition and quotation Elements
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

The library supports a variety of semantic HTML elements that provide meaning to inline text content. These elements not only convey semantic information but also apply default styling appropriate to their purpose. This document covers: `<cite>`, `<dfn>` and `<q>` inline text elements.

They can each be styled indepently or as a tag group, and support the full styling capabilities of a [span](html_span_element.html).

---

## Usage

These inline semantic elements help structure and style text with specific meanings:

- **`<cite>`**: Citations and references (italic by default)
- **`<dfn>`**: Definitions of terms (italic by default)
- **`<q>`**: Short inline quotations

---

## &lt;cite&gt; : Citation Element

The `<cite>` element represents a citation or reference to a creative work, publication, or source.

### Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `title` | string | Additional information about the citation (not used for outline). |
| `data-outline-title` | string | Optional outline/bookmark title. |
| `class` | string | CSS class names for styling. |
| `style` | string | Inline CSS styles. |
| `hidden` | string | Set to "hidden" to hide the element. |

### Default Styling

- Font style: **Italic**

## Class Hierarchy

```c#
Scryber.Html.Components.HTMLCite, Scryber.Components
```

In the library codebase:
- `HTMLCite` extends `HTMLSpan` extends `SpanBase` extends `VisualComponent`
- Inherits inline display behavior from `SpanBase`
- Overrides the default attribute name of OutlineTitle to `data-outline-title`
- Supports nested content through `Contents` collection

---

### Examples

```html
<!-- Book citation -->
<p>As described in <cite>The Art of Programming</cite>, best practices include...</p>

<!-- Article citation -->
<p>According to <cite>Journal of Modern Science</cite>, the results show...</p>

<!-- With styling -->
<p>The study <cite style="color: #336699;">Smith et al. 2023</cite> found that...</p>

<!-- Multiple citations -->
<p>Several sources (<cite>Brown 2022</cite>, <cite>Jones 2023</cite>) confirm this.</p>

<!-- Data-bound citation -->
{% raw %}<p>Reference: <cite>{{model.citationTitle}}</cite> ({{model.citationYear}})</p>{% endraw %}
```

---

## &lt;dfn&gt; : Definition Element

The `<dfn>` element represents the defining instance of a term, typically the first occurrence where the term is explained.

### Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `title` | string | Additional information about the definition (not used for outline). |
| `data-outline-title` | string | Optional outline/bookmark title. |
| `class` | string | CSS class names for styling. |
| `style` | string | Inline CSS styles. |
| `hidden` | string | Set to "hidden" to hide the element. |

### Default Styling

- Font style: **Italic**

## Class Hierarchy

```c#
Scryber.Html.Components.HTMLDefinition, Scryber.Components
```

In the library codebase:
- `HTMLDefinition` extends `HTMLSpan` extends `SpanBase` extends `VisualComponent`
- Inherits inline display behavior from `SpanBase`
- Overrides the default attribute name of OutlineTitle to `data-outline-title`
- Supports nested content through `Contents` collection

---

### Examples

```html
<!-- Basic definition -->
<p><dfn>Scryber</dfn> is a PDF generation library for .NET applications.</p>

<!-- Definition with context -->
<p>A <dfn>template</dfn> in Scryber is a reusable document structure that can be populated with dynamic data.</p>

<!-- Multiple definitions -->
<p>The <dfn>frameset</dfn> contains one or more <dfn>frames</dfn> that reference content sources.</p>

<!-- Styled definition -->
<p><dfn style="font-weight: bold; color: #336699;">Data binding</dfn> is the process of connecting template elements to data sources.</p>

<!-- With data binding -->
{% raw %}<p><dfn>{{model.termName}}</dfn>: {{model.termDefinition}}</p>{% endraw %}
```

---

## &lt;q&gt; : Inline Quotation Element

The `<q>` element represents a short inline quotation. Browsers typically add quotation marks automatically, but Scryber renders the content as-is.

### Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `class` | string | CSS class names for styling. |
| `style` | string | Inline CSS styles. |
| `hidden` | string | Set to "hidden" to hide the element. |
| `title` | string | Optional outline/bookmark title. |


## Class Hierarchy

```c#
Scryber.Html.Components.HTMLQuotedSpan, Scryber.Components
```

In the library codebase:
- `HTMLQuotedSpan` extends `HTMLSpan` extends `SpanBase` extends `VisualComponent`
- Inherits inline display behavior from `SpanBase`
- Supports nested content through `Contents` collection

---

### Examples

```html
<!-- Basic quotation -->
<p>The report stated <q>results exceeded expectations</q> in Q3.</p>

<!-- Nested quotations -->
<p>She said, <q>The manager mentioned <q>we need more time</q> yesterday</q>.</p>

<!-- Styled quotation -->
<p>The motto is <q style="font-style: italic; color: #666;">Innovation through collaboration</q>.</p>

<!-- With data binding -->
{% raw %}<p>Customer feedback: <q>{{model.customerQuote}}</q></p>{% endraw %}

<!-- Custom quote styling -->
<style>
    q {
        quotes: "«" "»" "‹" "›";
    }
    q:before { content: open-quote; }
    q:after { content: close-quote; }
</style>
<p>They remarked <q>this is excellent work</q>.</p>
```

---


## CSS Styling

All these elements support standard CSS styling:

```css
/* Override default italic styling */
cite, dfn {
    font-style: normal;
    font-weight: bold;
    color: #336699;
}

/* Quote styling */
q {
    font-style: italic;
    color: #666;
}
```

---

## See Also

- [span](html_span_elelemts.html) - Generic inline container element
- [output, slot, num](html_output_slot_num_elements.html) - Strong emphasis (bold)
- [var](html_var_element.html) - For storing or outputting document parameters.
- [Text Formatting](/library/templates/text.html) - Text formatting
- [Data Binding](/library/binding/) - Data binding expressions
- [CSS Styling](/library/styles/) - CSS styling reference

---
