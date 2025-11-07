---
layout: default
title: calc
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# calc() : CSS Calculation Expression
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

Generate a CSS calc() expression for dynamic style calculations. This allows mathematical operations in CSS property values with units.

## Signature

```
calc(expression)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `expression` | String | Yes | The CSS calculation expression |

---

## Returns

**Type:** String

A formatted CSS calc() expression string.

---

## Examples

### Dynamic Width Calculation

```handlebars
<div style="width: {{calc('100% - 20px')}}; border: 1px solid black;">
  <p>Content with calculated width</p>
</div>
```

**Data:**
```csharp
doc.Params["model"] = new { };
```

**Output:**
```html
<div style="width: calc(100% - 20px); border: 1px solid black;">
  <p>Content with calculated width</p>
</div>
```

### Variable-Based Calculations

```handlebars
<div style="margin-left: {{calc(string(model.indent) + 'px + 1em')}};">
  <p>Indented content</p>
</div>
```

**Data:**
```csharp
doc.Params["model"] = new {
    indent = 20
};
```

**Output:**
```html
<div style="margin-left: calc(20px + 1em);">
  <p>Indented content</p>
</div>
```

### Responsive Sizing

```handlebars
<table style="width: {{calc('100% - ' + string(model.sidebarWidth) + 'px')}};">
  <tr>
    <td>Table content</td>
  </tr>
</table>
```

**Data:**
```csharp
doc.Params["model"] = new {
    sidebarWidth = 200
};
```

**Output:**
```html
<table style="width: calc(100% - 200px);">
  <tr>
    <td>Table content</td>
  </tr>
</table>
```

### Grid Layout Calculations

```handlebars
{{#each model.columns}}
  <div style="width: {{calc('100% / ' + string(count(../model.columns)))}}; float: left;">
    <p>{{this.content}}</p>
  </div>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    columns = new[] {
        new { content = "Column 1" },
        new { content = "Column 2" },
        new { content = "Column 3" }
    }
};
```

**Output:**
```html
<div style="width: calc(100% / 3); float: left;">
  <p>Column 1</p>
</div>
<div style="width: calc(100% / 3); float: left;">
  <p>Column 2</p>
</div>
<div style="width: calc(100% / 3); float: left;">
  <p>Column 3</p>
</div>
```

### Font Size Scaling

```handlebars
<h1 style="font-size: {{calc(string(model.baseFontSize) + 'pt * ' + string(model.scale))}};">
  {{model.title}}
</h1>
```

**Data:**
```csharp
doc.Params["model"] = new {
    title = "Scaled Heading",
    baseFontSize = 12,
    scale = 2.5
};
```

**Output:**
```html
<h1 style="font-size: calc(12pt * 2.5);">
  Scaled Heading
</h1>
```

---

## Notes

- Returns a formatted `calc()` string for use in CSS properties
- Supports standard CSS calc() operations:
  - Addition: `+`
  - Subtraction: `-`
  - Multiplication: `*`
  - Division: `/`
- Can mix different units (px, %, em, pt, etc.)
- Whitespace around operators is recommended for clarity
- Useful for:
  - Responsive layouts
  - Dynamic spacing
  - Proportional sizing
  - Grid calculations
  - Font scaling
- Not all CSS properties support calc() (check PDF rendering engine)
- Expression is evaluated by the CSS engine, not at binding time
- For pure numeric calculations without units, use standard math operators instead
- Common patterns:
  - `calc('100% - Npx')` - full width minus fixed offset
  - `calc('100% / N')` - equal divisions
  - `calc('Npx + Nem')` - mixed unit addition

---

## See Also

- [var Function](./var.md)
- [string Function](./string.md)
- [concat Function](./concat.md)

---
