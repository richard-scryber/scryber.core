---
layout: default
title: else
parent: Handlebars Helpers
parent_url: /reference/binding/helpers/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# {{else}} : Fallback Branch Helper
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

Provides a fallback branch when conditional expressions are false or when collections are empty.

**Based on:** [`<otherwise>` element](../../components/choose.md) in conditionals, or fallback content in iteration contexts

## Syntax

**With {{#if}}:**
```handlebars
{{#if condition}}
  <!-- Content when true -->
{{else}}
  <!-- Content when false -->
{{/if}}
```

**With {{#each}}:**
```handlebars
{{#each collection}}
  <!-- Content for each item -->
{{else}}
  <!-- Content when collection is empty -->
{{/each}}
```

**With {{#with}}:**
```handlebars
{{#with object}}
  <!-- Content when object exists -->
{{else}}
  <!-- Content when object is null -->
{{/with}}
```

---

## Parameters

No parameters. The `{{else}}` helper is used as a separator within block helpers.

---

## Examples

### With If Statement

```handlebars
{{#if model.hasStock}}
  <button class="btn-primary">Add to Cart</button>
{{else}}
  <button class="btn-disabled" disabled>Out of Stock</button>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    hasStock = false
};
```

**Output:**
```html
<button class="btn-disabled" disabled>Out of Stock</button>
```

### With Each Loop (Empty Collection)

```handlebars
<ul>
{{#each model.items}}
  <li>{{this.name}}</li>
{{else}}
  <li class="empty">No items available</li>
{{/each}}
</ul>
```

**Data:**
```csharp
doc.Params["model"] = new {
    items = new object[] { }  // Empty array
};
```

**Output:**
```html
<ul>
  <li class="empty">No items available</li>
</ul>
```

### With Context Switch (Null Object)

```handlebars
{{#with model.user}}
  <div class="user-profile">
    <h2>{{name}}</h2>
    <p>{{email}}</p>
  </div>
{{else}}
  <div class="no-profile">
    <p>User profile not available</p>
  </div>
{{/with}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    user = (object)null
};
```

**Output:**
```html
<div class="no-profile">
  <p>User profile not available</p>
</div>
```

### Multiple Else If with Final Else

```handlebars
{{#if model.grade >= 90}}
  <span class="a">A - Excellent</span>
{{else if model.grade >= 80}}
  <span class="b">B - Good</span>
{{else if model.grade >= 70}}
  <span class="c">C - Average</span>
{{else}}
  <span class="f">F - Needs Improvement</span>
{{/if}}
```

---

## Underlying Implementation

**In {{#if}} blocks**, `{{else}}` compiles to:
```xml
<choose>
  <when data-test="{{condition}}">
    <!-- If content -->
  </when>
  <otherwise>
    <!-- Else content -->
  </otherwise>
</choose>
```

**In {{#each}} blocks**, `{{else}}` creates a conditional wrapper:
```xml
<template data-bind="{{collection}}">
  <!-- Each content -->
</template>
<template data-test="count({{collection}}) == 0">
  <!-- Else content shown when empty -->
</template>
```

---

## Notes

- Can be used with `{{#if}}`, `{{#each}}`, and `{{#with}}`
- Only one `{{else}}` block allowed per helper (use `{{else if}}` for multiple conditions)
- For `{{#if}}`, the else block renders when all conditions are false
- For `{{#each}}`, the else block renders when the collection is empty
- For `{{#with}}`, the else block renders when the object is null or undefined
- Position matters: must come after all `{{else if}}` blocks

---

## See Also

- [#if Helper](./if.md)
- [else if Helper](./elseif.md)
- [#each Helper](./each.md)
- [#with Helper](./with.md)
- [Conditional Rendering Guide](../../learning/02-data-binding/04_conditional_rendering.md)

---
