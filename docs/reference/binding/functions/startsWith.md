---
layout: default
title: startsWith
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# startsWith() : Check String Prefix
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

Check if a string starts with a specified prefix.

## Signature

```
startsWith(str, prefix)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The string to check |
| `prefix` | String | Yes | The prefix to look for |

---

## Returns

**Type:** Boolean

`true` if the string starts with the prefix, `false` otherwise.

---

## Examples

### Protocol Detection

```handlebars
{{#if startsWith(model.url, 'https://')}}
  <span class="secure">🔒 Secure</span>
{{else}}
  <span class="insecure">⚠ Not Secure</span>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    url = "https://example.com"
};
```

**Output:**
```html
<span class="secure">🔒 Secure</span>
```

### File Type Detection

```handlebars
{{#each model.files}}
  {{#if startsWith(this.name, 'report_')}}
    <div class="file-report">
      <span class="icon">📄</span> {{this.name}}
    </div>
  {{/if}}
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    files = new[] {
        new { name = "report_2024.pdf" },
        new { name = "data.csv" },
        new { name = "report_summary.xlsx" }
    }
};
```

**Output:**
```html
<div class="file-report">
  <span class="icon">📄</span> report_2024.pdf
</div>
<div class="file-report">
  <span class="icon">📄</span> report_summary.xlsx
</div>
```

### Prefix-Based Filtering

```handlebars
{{#each model.codes}}
  {{#if startsWith(this, 'PRO-')}}
    <span class="code-professional">{{this}}</span>
  {{else if startsWith(this, 'STD-')}}
    <span class="code-standard">{{this}}</span>
  {{/if}}
{{/each}}
```

### Phone Number Validation

```handlebars
{{#if startsWith(model.phone, '+1')}}
  <p>US/Canada: {{model.phone}}</p>
{{else if startsWith(model.phone, '+44')}}
  <p>UK: {{model.phone}}</p>
{{else}}
  <p>International: {{model.phone}}</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    phone = "+1-555-123-4567"
};
```

**Output:**
```html
<p>US/Canada: +1-555-123-4567</p>
```

---

## Notes

- Case-sensitive by default
- Returns false if prefix is not found at beginning
- Empty prefix returns true
- For case-insensitive check, use with `toLower()`: `startsWith(toLower(str), toLower(prefix))`
- For suffix checking, use `endsWith()`
- For substring anywhere, use `contains()`

---

## See Also

- [endsWith Function](./endsWith.md)
- [contains Function](./contains.md)
- [substring Function](./substring.md)
- [toLower Function](./toLower.md)

---
