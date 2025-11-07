---
layout: default
title: indexOf
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# indexOf() : Find Substring Position
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

Find the first occurrence position of a substring within a string.

## Signature

```
indexOf(str, search)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The string to search in |
| `search` | String | Yes | The substring to find |

---

## Returns

**Type:** Integer

The zero-based index of the first occurrence, or -1 if not found.

---

## Examples

### Check if Found

```handlebars
{{#if indexOf(model.text, '@') >= 0}}
  <p>Email format detected</p>
{{else}}
  <p>Not an email</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "user@example.com"
};
```

**Output:**
```html
<p>Email format detected</p>
```

### Extract After Position

```handlebars
<p>Domain: {{substring(model.email, indexOf(model.email, '@') + 1)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    email = "john@company.com"
};
```

**Output:**
```html
<p>Domain: company.com</p>
```

### Split on First Occurrence

```handlebars
<p>First part: {{substring(model.text, 0, indexOf(model.text, ','))}}</p>
<p>Second part: {{substring(model.text, indexOf(model.text, ',') + 2)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "apple, orange, banana"
};
```

**Output:**
```html
<p>First part: apple</p>
<p>Second part: orange, banana</p>
```

### Conditional Based on Position

```handlebars
{{#each model.items}}
  {{#if indexOf(this.code, '-') > 0}}
    <p>Code with separator: {{this.code}}</p>
  {{else}}
    <p>Simple code: {{this.code}}</p>
  {{/if}}
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    items = new[] {
        new { code = "ABC-123" },
        new { code = "XYZ789" }
    }
};
```

**Output:**
```html
<p>Code with separator: ABC-123</p>
<p>Simple code: XYZ789</p>
```

---

## Notes

- Returns zero-based index (0 = first character)
- Returns -1 if substring not found
- Case-sensitive search
- Returns index of first occurrence only
- For case-insensitive search, use with `toLower()`
- Commonly used with `substring()` for text extraction
- For existence check only, use `contains()`

---

## See Also

- [contains Function](./contains.md)
- [substring Function](./substring.md)
- [split Function](./split.md)
- [Greater Than or Equal Operator](../operators/greaterorequal.md)

---
