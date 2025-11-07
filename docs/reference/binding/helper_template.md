---
layout: default
title: helper_name
parent: Handlebars Helpers
parent_url: /reference/binding/helpers/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# {{#helper_name}} : Helper Description
{: .no_toc }

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{:toc}
</details>

---

## Summary

Brief description of what this helper does.

**Based on:** [Link to underlying Scryber element/component](../../htmltags/element.md)

## Syntax

```handlebars
{{#helper_name expression}}
  <!-- Content -->
{{/helper_name}}
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `expression` | Expression | Yes | Description of the parameter |

---

## Special Variables

| Variable | Description |
|----------|-------------|
| `variable` | Description |

---

## Examples

### Basic Example

```handlebars
{{#helper_name model.data}}
  <p>{{this.property}}</p>
{{/helper_name}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    data = new { property = "value" }
};
```

**Output:**
```html
<p>value</p>
```

---

## Underlying Implementation

The `{{#helper_name}}` helper compiles to the following Scryber template structure:

```xml
<element data-attribute="{{expression}}">
  <!-- Content -->
</element>
```

Brief explanation of how it works internally.

---

## Notes

- Additional notes
- Implementation details
- Common pitfalls
- Avoid using "live" data (DateTime.Now, etc.) - PDFs are static, use model data

---

## See Also

- [Related Helper](./related.md)
- [Data Binding Basics](../../learning/02-data-binding/01_data_binding_basics.md)

---
