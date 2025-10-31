---
layout: default
title: length
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# length() : Get String/Array Length
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

Get the length of a string or count of items in an array.

## Signature

```
length(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | String/Array | Yes | The string or array to measure |

---

## Returns

**Type:** Integer

The number of characters in the string or items in the array.

---

## Examples

### String Length

```handlebars
<p>Length: {{length(model.text)}} characters</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "Hello World"
};
```

**Output:**
```html
<p>Length: 11 characters</p>
```

### Array Count

```handlebars
<p>Total items: {{length(model.items)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    items = new[] { "Item 1", "Item 2", "Item 3" }
};
```

**Output:**
```html
<p>Total items: 3</p>
```

### Conditional Truncation

```handlebars
{{#each model.descriptions}}
  {{#if length(this) > 50}}
    <p>{{substring(this, 0, 50)}}...</p>
  {{else}}
    <p>{{this}}</p>
  {{/if}}
{{/each}}
```

### Character Limit Validation

```handlebars
{{#if length(model.comment) > 200}}
  <p class="error">Comment too long ({{length(model.comment)}}/200 characters)</p>
{{else}}
  <p>{{model.comment}}</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    comment = "This is a sample comment."
};
```

**Output:**
```html
<p>This is a sample comment.</p>
```

### List Size Display

```handlebars
<h3>Products ({{length(model.products)}})</h3>
<ul>
{{#each model.products}}
  <li>{{this.name}}</li>
{{/each}}
</ul>
```

**Data:**
```csharp
doc.Params["model"] = new {
    products = new[] {
        new { name = "Widget A" },
        new { name = "Widget B" },
        new { name = "Widget C" }
    }
};
```

**Output:**
```html
<h3>Products (3)</h3>
<ul>
  <li>Widget A</li>
  <li>Widget B</li>
  <li>Widget C</li>
</ul>
```

---

## Notes

- Works with both strings and arrays
- Returns character count for strings
- Returns item count for arrays/collections
- Returns 0 for empty string or array
- Null values may throw exception
- For collection operations, also see `count()` function

---

## See Also

- [count Function](./count.md)
- [substring Function](./substring.md)
- [#each Helper](../helpers/each.md)
- [Greater Than Operator](../operators/greaterthan.md)

---
