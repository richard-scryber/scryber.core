---
layout: default
title: toUpper
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# toUpper() : Convert to Uppercase
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

Convert a string to uppercase letters.

## Signature

```
toUpper(str)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The string to convert |

---

## Returns

**Type:** String

The string converted to uppercase.

---

## Examples

### Basic Uppercase

```handlebars
<p>{{toUpper(model.text)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "hello world"
};
```

**Output:**
```html
<p>HELLO WORLD</p>
```

### Headers and Titles

```handlebars
<h1>{{toUpper(model.title)}}</h1>
```

**Data:**
```csharp
doc.Params["model"] = new {
    title = "Important Notice"
};
```

**Output:**
```html
<h1>IMPORTANT NOTICE</h1>
```

### Product Codes

```handlebars
<p>SKU: {{toUpper(model.sku)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    sku = "wdg-12345-a"
};
```

**Output:**
```html
<p>SKU: WDG-12345-A</p>
```

### Capitalize First Letter

```handlebars
<p>{{toUpper(substring(model.name, 0, 1))}}{{substring(model.name, 1)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    name = "john doe"
};
```

**Output:**
```html
<p>John doe</p>
```

### Emphasis

```handlebars
{{#each model.alerts}}
  <div class="alert">
    <strong>{{toUpper(this.level)}}</strong>: {{this.message}}
  </div>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    alerts = new[] {
        new { level = "warning", message = "Low disk space" },
        new { level = "error", message = "Connection failed" }
    }
};
```

**Output:**
```html
<div class="alert">
  <strong>WARNING</strong>: Low disk space
</div>
<div class="alert">
  <strong>ERROR</strong>: Connection failed
</div>
```

---

## Notes

- Converts all lowercase letters to uppercase
- Leaves numbers and special characters unchanged
- Culture-dependent (uses current culture)
- Useful for headings and emphasis
- Does not modify the original value
- For lowercase, use `toLower()`

---

## See Also

- [toLower Function](./toLower.md)
- [substring Function](./substring.md)
- [concat Function](./concat.md)

---
