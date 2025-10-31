---
layout: default
title: each
parent: Handlebars Helpers
parent_url: /reference/binding/helpers/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# {{#each}} : Iteration Helper
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

Iterate over arrays or collections, rendering the enclosed template for each item. The data context changes to the current item within the block.

**Based on:** [`<template>` element](../../htmltags/template.md) with data binding and the [`ForEach` component](../../components/foreach.md)

## Syntax

```handlebars
{{#each collection}}
  <!-- Content repeated for each item -->
{{/each}}
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array/IEnumerable | Yes | The collection to iterate over |

---

## Special Variables

| Variable | Description |
|----------|-------------|
| `@index` | Zero-based index of current item (0, 1, 2, ...) |
| `@first` | Boolean, `true` for the first item |
| `@last` | Boolean, `true` for the last item |
| `this` | Reference to the current item |
| `../` | Access parent context |

---

## Examples

### Basic Iteration

```handlebars
<ul>
{{#each model.items}}
  <li>{{this}}</li>
{{/each}}
</ul>
```

**Data:**
```csharp
doc.Params["model"] = new {
    items = new[] { "Apple", "Banana", "Orange" }
};
```

**Output:**
```html
<ul>
  <li>Apple</li>
  <li>Banana</li>
  <li>Orange</li>
</ul>
```

### With Object Properties

```handlebars
<table>
  {{#each model.products}}
  <tr>
    <td>{{add(@index, 1)}}</td>
    <td>{{this.name}}</td>
    <td>${{format(this.price, '0.00')}}</td>
  </tr>
  {{/each}}
</table>
```

**Data:**
```csharp
doc.Params["model"] = new {
    products = new[] {
        new { name = "Widget", price = 19.99m },
        new { name = "Gadget", price = 29.99m }
    }
};
```

### Using Special Variables

```handlebars
{{#each model.items}}
  <div class="{{if(@first, 'first', '')}} {{if(@last, 'last', '')}}">
    <span>Item {{add(@index, 1)}}</span>
    <h3>{{this.title}}</h3>
  </div>
{{/each}}
```

### Accessing Parent Context

```handlebars
<h1>{{model.companyName}}</h1>
{{#each model.departments}}
  <h2>{{this.name}}</h2>
  {{#each this.employees}}
    <li>{{this.name}} works at {{../../companyName}}</li>
  {{/each}}
{{/each}}
```

---

## Underlying Implementation

The `{{#each}}` helper compiles to the following Scryber template element:

```xml
<template data-bind="{{collection}}">
  <!-- Your content here with {{this}} context -->
</template>
```

This uses Scryber's [`ForEach` component](../../components/foreach.md) internally, which provides the iteration and context management.

---

## Notes

- Empty arrays render nothing (no error)
- Works with any IEnumerable collection
- Can be nested for multi-dimensional data
- Context changes to current item within the block
- Use `{{.}}` to reference the entire current item
- Provides `@index`, `@first`, and `@last` special variables

---

## See Also

- [#with Helper](./with.md)
- [#if Helper](./if.md)
- [Template Iteration Guide](../../learning/02-data-binding/03_template_iteration.md)

---
