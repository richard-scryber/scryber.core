---
layout: default
title: contains
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# contains() : Check if String Contains Substring
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

Check if a string contains a specified substring.

## Signature

```
contains(str, search)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The string to search in |
| `search` | String | Yes | The substring to search for |

---

## Returns

**Type:** Boolean

`true` if the substring is found, `false` otherwise.

---

## Examples

### Simple Search

```handlebars
{{#if contains(model.description, 'premium')}}
  <span class="badge-premium">Premium Product</span>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    description = "This is a premium quality product"
};
```

**Output:**
```html
<span class="badge-premium">Premium Product</span>
```

### Filter Items

```handlebars
{{#each model.products}}
  {{#if contains(this.name, 'Pro')}}
    <div class="product-pro">
      <h3>{{this.name}}</h3>
      <span>Professional Edition</span>
    </div>
  {{/if}}
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    products = new[] {
        new { name = "Widget" },
        new { name = "Widget Pro" },
        new { name = "Gadget Pro Max" }
    }
};
```

**Output:**
```html
<div class="product-pro">
  <h3>Widget Pro</h3>
  <span>Professional Edition</span>
</div>
<div class="product-pro">
  <h3>Gadget Pro Max</h3>
  <span>Professional Edition</span>
</div>
```

### Keyword Highlighting

```handlebars
{{#each model.items}}
  {{#if contains(toLower(this.title), toLower(model.searchTerm))}}
    <div class="search-result">
      <h4>{{this.title}}</h4>
      <p>{{this.description}}</p>
    </div>
  {{/if}}
{{/each}}
```

### Email Validation

```handlebars
{{#if contains(model.email, '@')}}
  <p>Email: {{model.email}}</p>
{{else}}
  <p class="error">Invalid email format</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    email = "user@example.com"
};
```

**Output:**
```html
<p>Email: user@example.com</p>
```

---

## Notes

- Case-sensitive by default
- Returns false if search string is not found
- Empty search string returns true
- For case-insensitive search, use with `toLower()`: `contains(toLower(str), toLower(search))`
- For position information, use `indexOf()`
- Returns false for null values

---

## See Also

- [indexOf Function](./indexOf.md)
- [startsWith Function](./startsWith.md)
- [endsWith Function](./endsWith.md)
- [toLower Function](./toLower.md)

---
