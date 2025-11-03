---
layout: default
title: abbr
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;abbr&gt; : The Abbreviation Element
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

The `<abbr>` element is an inline semantic HTML element that marks up abbreviations and acronyms. In the library, it renders text in italic by default and supports the `title` attribute to provide the full form of the abbreviation. Unlike the standard HTML elements, where `title` appears in the outline, title can be used for accessibility, annotations, or displayed contextually.

---

## Usage

```html
<!-- Basic abbreviation -->
<abbr title="HyperText Markup Language">HTML</abbr>

<!-- With styling -->
<abbr title="Cascading Style Sheets" class="technical-term">CSS</abbr>

<!-- With data binding -->
{% raw %}<abbr title="{{model.abbreviationExpansion}}">{{model.abbreviationText}}</abbr>{% endraw %}
```

---

## Supported Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `title` | string | The full expansion or description of the abbreviation |
| `data-outline-title` | string | Alternative title for document outline/bookmarks |
| `id` | string | Unique identifier for the element |
| `class` | string | CSS class name(s) for styling |
| `style` | string | Inline CSS styles |
| `hidden` | string | If set to "hidden", the element is not visible |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-content` | expression | Dynamically sets the content of the address element from bound data. |
| `data-content-type` | Mime Type | Specifies the type of bound content fragment - XHTML; HTML; Markdown. |
| `data-content-action` | Replace, Append, Prepend | Specifies the action to take when binding elements with existing inner content. |

---

## Default Styling

By default, the `<abbr>` element renders text in **italic** style to visually distinguish abbreviations from regular text. This can be overridden using CSS:

```css
abbr {
    font-style: normal; /* Remove italic */
    text-decoration: underline; /* Add underline */
}
```

---

## Notes

- The `title` attribute in `<abbr>` does NOT create an outline entry by default
- To create an outline entry, use the `data-outline-title` attribute instead
- By default renders in italic to match semantic HTML conventions
- The element is inline and flows with surrounding text
- Supports full data binding for both content and title
- Can be nested within other inline elements
- Useful for technical documents, medical reports, and any content with specialized terminology

---


## Class Hierarchy

```c#
Scryber.Html.Components.HTMLAbbreviation, Scryber.Components
```

In the library codebase:
- `HTMLAbbreviation` extends <a href='html_span_element.html'>`HTMLSpan`</a> extends `Span` extends `VisualComponent`
- Inherits inline display behavior from `Span`
- Supports nested content through `Contents` collection

```c#
using Scryber.Text;
using Scryber.HTML.Components;

var abbr = new HTMLAbbreviation();
abbr.Contents.Add("API");
abbr.AbbrTitle = "Application Programming Interface";
abbr.OutlineTitle = "API Details";
abbr.Style.Text.Decoration = TextDecoration.Underline;
//page.Contents.Add(abbr);
```

---

## Examples

### Basic Abbreviation

```html
<p>
    The <abbr title="World Wide Web Consortium">W3C</abbr> defines web standards.
</p>
```

### Technical Documentation

```html
<p>
    <abbr title="Application Programming Interface">API</abbr> endpoints use
    <abbr title="Representational State Transfer">REST</abbr> architecture
    with <abbr title="JavaScript Object Notation">JSON</abbr> payloads.
</p>
```

### Medical Terms

```html
<p>
    Patient shows elevated <abbr title="Blood Pressure">BP</abbr>
    (140/90 <abbr title="millimeters of mercury">mmHg</abbr>) and
    increased <abbr title="Heart Rate">HR</abbr>.
</p>
```

### With Custom Styling

```html
<style>
    abbr.technical {
        font-style: normal;
        font-weight: bold;
        color: #0066cc;
        border-bottom: 1pt dotted #0066cc;
    }
</style>
<p>
    Using <abbr class="technical" title="HyperText Markup Language">HTML</abbr>
    and <abbr class="technical" title="Cascading Style Sheets">CSS</abbr>.
</p>
```

### Data Binding with Abbreviations

```html
{% raw %}<p>
    The system uses
    <abbr title="{{model.protocolFullName}}">{{model.protocolAbbr}}</abbr>
    for communication.
</p>

<!-- With model data:
{
    "protocolAbbr": "HTTPS",
    "protocolFullName": "HyperText Transfer Protocol Secure"
}
-->{% endraw %}
```


### Multiple Abbreviations in Technical Report

```html
<h2>System Architecture</h2>
<p>
    The application uses a <abbr title="Model-View-Controller">MVC</abbr>
    architecture with <abbr title="Object-Relational Mapping">ORM</abbr>
    for database access. Authentication is handled via
    <abbr title="JSON Web Tokens">JWT</abbr> and
    <abbr title="OAuth 2.0">OAuth</abbr>.
</p>
```

### Time and Date Abbreviations

```html
{% raw %}<p>
    Meeting scheduled for
    <abbr title="{{string(date(), 'dddd MMMM yyyy ZZZ)}}">{{string(date(), "YY-MM-DD")}}</abbr>,
</p>{% endraw %}
```

### Repeating Template with Dynamic Abbreviations

```html
{% raw %}<template data-bind="{{model.technicalTerms}}">
    <p>
        <abbr title="{{.fullName}}">{{.abbr}}</abbr>: {{.description}}
    </p>
</template>

<!-- With model data:
{
    "technicalTerms": [
        { "abbr": "API", "fullName": "Application Programming Interface", "description": "..." },
        { "abbr": "SDK", "fullName": "Software Development Kit", "description": "..." },
        { "abbr": "IDE", "fullName": "Integrated Development Environment", "description": "..." }
    ]
}
-->{% endraw %}
```


---

## See Also

- [span Element](html_span_element)
- [CSS Styling](/reference/cssselectors/)
- [@title attribute](/reference/htmlattributes/attributes/attr_title)
- [@class attribute](/reference/htmlattributes/attributes/attr_class)
- [Data Binding](/reference/learning/binding/)

---
