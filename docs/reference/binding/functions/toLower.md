---
layout: default
title: toLower
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# toLower() : Convert to Lowercase
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

Convert a string to lowercase letters.

## Signature

```
toLower(str)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The string to convert |

---

## Returns

**Type:** String

The string converted to lowercase.

---

## Examples

### Basic Lowercase

```handlebars
<p>{{toLower(model.text)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "HELLO WORLD"
};
```

**Output:**
```html
<p>hello world</p>
```

### Normalize for Comparison

```handlebars
{{#if toLower(model.status) == 'active'}}
  <span class="badge-active">Active</span>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    status = "ACTIVE"
};
```

**Output:**
```html
<span class="badge-active">Active</span>
```

### Email Display

```handlebars
<p>Email: {{toLower(model.email)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    email = "John.Doe@EXAMPLE.COM"
};
```

**Output:**
```html
<p>Email: john.doe@example.com</p>
```

### CSS Class Names

```handlebars
{{#each model.categories}}
  <div class="category-{{toLower(replace(this.name, ' ', '-'))}}">
    {{this.name}}
  </div>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    categories = new[] {
        new { name = "Product Reviews" },
        new { name = "Technical Support" }
    }
};
```

**Output:**
```html
<div class="category-product-reviews">
  Product Reviews
</div>
<div class="category-technical-support">
  Technical Support
</div>
```

---

## Notes

- Converts all uppercase letters to lowercase
- Leaves numbers and special characters unchanged
- Culture-dependent (uses current culture)
- Useful for case-insensitive comparisons
- Does not modify the original value
- For uppercase, use `toUpper()`

---

## See Also

- [toUpper Function](./toUpper.md)
- [replace Function](./replace.md)
- [Equality Operator](../operators/equality.md)

---
