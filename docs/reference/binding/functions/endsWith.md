---
layout: default
title: endsWith
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# endsWith() : Check String Suffix
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

Check if a string ends with a specified suffix.

## Signature

```
endsWith(str, suffix)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The string to check |
| `suffix` | String | Yes | The suffix to look for |

---

## Returns

**Type:** Boolean

`true` if the string ends with the suffix, `false` otherwise.

---

## Examples

### File Extension Check

```handlebars
{{#each model.files}}
  {{#if endsWith(this.name, '.pdf')}}
    <div class="file-pdf">
      <span class="icon">📄</span> {{this.name}}
    </div>
  {{else if endsWith(this.name, '.jpg') || endsWith(this.name, '.png')}}
    <div class="file-image">
      <span class="icon">🖼️</span> {{this.name}}
    </div>
  {{/if}}
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    files = new[] {
        new { name = "document.pdf" },
        new { name = "photo.jpg" },
        new { name = "report.docx" }
    }
};
```

**Output:**
```html
<div class="file-pdf">
  <span class="icon">📄</span> document.pdf
</div>
<div class="file-image">
  <span class="icon">🖼️</span> photo.jpg
</div>
```

### Domain Validation

```handlebars
{{#if endsWith(model.email, '@company.com')}}
  <span class="badge-internal">Internal</span>
{{else}}
  <span class="badge-external">External</span>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    email = "john.doe@company.com"
};
```

**Output:**
```html
<span class="badge-internal">Internal</span>
```

### URL Path Checking

```handlebars
{{#if endsWith(model.url, '/')}}
  <a href="{{model.url}}index.html">{{model.url}}index.html</a>
{{else}}
  <a href="{{model.url}}">{{model.url}}</a>
{{/if}}
```

### Version Suffix

```handlebars
{{#each model.products}}
  <div class="product">
    <h3>{{this.name}}</h3>
    {{#if endsWith(this.version, '-beta')}}
      <span class="badge-beta">Beta</span>
    {{else if endsWith(this.version, '-alpha')}}
      <span class="badge-alpha">Alpha</span>
    {{else}}
      <span class="badge-stable">Stable</span>
    {{/if}}
  </div>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    products = new[] {
        new { name = "Product A", version = "1.0.0" },
        new { name = "Product B", version = "2.0.0-beta" },
        new { name = "Product C", version = "3.0.0-alpha" }
    }
};
```

**Output:**
```html
<div class="product">
  <h3>Product A</h3>
  <span class="badge-stable">Stable</span>
</div>
<div class="product">
  <h3>Product B</h3>
  <span class="badge-beta">Beta</span>
</div>
<div class="product">
  <h3>Product C</h3>
  <span class="badge-alpha">Alpha</span>
</div>
```

---

## Notes

- Case-sensitive by default
- Returns false if suffix is not found at end
- Empty suffix returns true
- For case-insensitive check, use with `toLower()`: `endsWith(toLower(str), toLower(suffix))`
- For prefix checking, use `startsWith()`
- For substring anywhere, use `contains()`

---

## See Also

- [startsWith Function](./startsWith.md)
- [contains Function](./contains.md)
- [substring Function](./substring.md)
- [toLower Function](./toLower.md)

---
