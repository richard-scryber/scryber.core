---
layout: default
title: var
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# var() : CSS Variable Reference
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

Generate a CSS var() expression to reference CSS custom properties (variables). Allows dynamic CSS variable references with optional fallback values.

## Signature

```
var(variableName)
var(variableName, fallbackValue)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `variableName` | String | Yes | The CSS variable name (without `--` prefix) |
| `fallbackValue` | String | No | Optional fallback value if variable is not defined |

---

## Returns

**Type:** String

A formatted CSS var() expression string.

---

## Examples

### Simple Variable Reference

```handlebars
<div style="color: {{var('primary-color')}}; background: {{var('bg-color')}};">
  <p>Styled with CSS variables</p>
</div>
```

**Data:**
```csharp
doc.Params["model"] = new { };
```

**Output:**
```html
<div style="color: var(--primary-color); background: var(--bg-color);">
  <p>Styled with CSS variables</p>
</div>
```

### Variable with Fallback

```handlebars
<p style="color: {{var('text-color', '#333333')}};">
  Text with fallback color
</p>
```

**Data:**
```csharp
doc.Params["model"] = new { };
```

**Output:**
```html
<p style="color: var(--text-color, #333333);">
  Text with fallback color
</p>
```

### Dynamic Variable Names

```handlebars
{{#each model.themes}}
  <div style="background: {{var(concat(this.name, '-bg'))}};">
    <p style="color: {{var(concat(this.name, '-text'))}};">
      {{this.name}} theme
    </p>
  </div>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    themes = new[] {
        new { name = "light" },
        new { name = "dark" }
    }
};
```

**Output:**
```html
<div style="background: var(--light-bg);">
  <p style="color: var(--light-text);">
    light theme
  </p>
</div>
<div style="background: var(--dark-bg);">
  <p style="color: var(--dark-text);">
    dark theme
  </p>
</div>
```

### Themed Components

```handlebars
<div class="card" style="
  border-color: {{var('card-border', '#cccccc')}};
  background: {{var('card-bg', '#ffffff')}};
  padding: {{var('card-padding', '10pt')}};
">
  <h3 style="color: {{var('card-heading-color', '#000000')}};">
    {{model.title}}
  </h3>
  <p style="color: {{var('card-text-color', '#333333')}};">
    {{model.content}}
  </p>
</div>
```

**Data:**
```csharp
doc.Params["model"] = new {
    title = "Card Title",
    content = "Card content with CSS variables for theming."
};
```

**Output:**
```html
<div class="card" style="
  border-color: var(--card-border, #cccccc);
  background: var(--card-bg, #ffffff);
  padding: var(--card-padding, 10pt);
">
  <h3 style="color: var(--card-heading-color, #000000);">
    Card Title
  </h3>
  <p style="color: var(--card-text-color, #333333);">
    Card content with CSS variables for theming.
  </p>
</div>
```

### Responsive Sizing

```handlebars
<table style="width: {{var('table-width', '100%')}}; font-size: {{var('table-font-size', '10pt')}};">
  <tr>
    <th style="padding: {{var('cell-padding', '5pt')}};">Header</th>
  </tr>
  <tr>
    <td style="padding: {{var('cell-padding', '5pt')}};">Data</td>
  </tr>
</table>
```

**Data:**
```csharp
doc.Params["model"] = new { };
```

**Output:**
```html
<table style="width: var(--table-width, 100%); font-size: var(--table-font-size, 10pt);">
  <tr>
    <th style="padding: var(--cell-padding, 5pt);">Header</th>
  </tr>
  <tr>
    <td style="padding: var(--cell-padding, 5pt);">Data</td>
  </tr>
</table>
```

---

## Notes

- Automatically adds `--` prefix to variable names
- Returns a formatted `var()` string for CSS properties
- Fallback value is optional but recommended
- CSS variables must be defined in a stylesheet or inline style
- Variable names are case-sensitive
- Useful for:
  - Theme systems
  - Consistent styling
  - Responsive design
  - Brand color management
  - Reusable style tokens
- Variables can reference other variables
- Fallback provides graceful degradation
- Common naming conventions:
  - Colors: `primary-color`, `secondary-color`, `text-color`
  - Spacing: `spacing-sm`, `spacing-md`, `spacing-lg`
  - Fonts: `font-heading`, `font-body`, `font-size-base`
- Not all PDF rendering engines fully support CSS custom properties
- Consider providing fallbacks for better compatibility

---

## See Also

- [calc Function](./calc.md)
- [concat Function](./concat.md)
- [if Function](./if.md)

---
