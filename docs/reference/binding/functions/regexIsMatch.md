---
layout: default
title: regexIsMatch
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# regexIsMatch() : Test Regular Expression
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

Test if a string matches a regular expression pattern.

## Signature

```
regexIsMatch(str, pattern)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The string to test |
| `pattern` | String | Yes | The regular expression pattern |

---

## Returns

**Type:** Boolean

`true` if the string matches the pattern, `false` otherwise.

---

## Examples

### Email Validation

```handlebars
{{#if regexIsMatch(model.email, '^[^@]+@[^@]+\\.[^@]+$')}}
  <p class="valid">Valid email: {{model.email}}</p>
{{else}}
  <p class="invalid">Invalid email format</p>
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
<p class="valid">Valid email: user@example.com</p>
```

### Phone Number Format

```handlebars
{{#if regexIsMatch(model.phone, '^\\d{3}-\\d{3}-\\d{4}$')}}
  <p>US Format: {{model.phone}}</p>
{{else}}
  <p>Invalid phone format</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    phone = "555-123-4567"
};
```

**Output:**
```html
<p>US Format: 555-123-4567</p>
```

### Postal Code Validation

```handlebars
{{#each model.addresses}}
  <div class="address">
    <p>{{this.city}}, {{this.state}}</p>
    {{#if regexIsMatch(this.zip, '^\\d{5}(-\\d{4})?$')}}
      <p>ZIP: {{this.zip}}</p>
    {{else}}
      <p class="error">Invalid ZIP code</p>
    {{/if}}
  </div>
{{/each}}
```

### Contains Digits

```handlebars
{{#if regexIsMatch(model.text, '\\d')}}
  <p>Contains numbers</p>
{{else}}
  <p>No numbers found</p>
{{/if}}
```

### Alphanumeric Check

```handlebars
{{#if regexIsMatch(model.code, '^[A-Za-z0-9]+$')}}
  <p>Valid code: {{model.code}}</p>
{{else}}
  <p>Code must be alphanumeric</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    code = "ABC123"
};
```

**Output:**
```html
<p>Valid code: ABC123</p>
```

---

## Notes

- Uses .NET regex syntax
- Backslashes must be escaped: `\\d` for digit, `\\w` for word character
- Case-sensitive by default
- Returns true if any part of string matches (use ^ and $ for exact match)
- For extracting matches, use `regexMatches()`
- For replacement, use `regexSwap()`
- More powerful than `contains()` but slower

---

## Common Patterns

| Pattern | Description |
|---------|-------------|
| `^\\d+$` | Only digits |
| `^[A-Za-z]+$` | Only letters |
| `^[A-Za-z0-9]+$` | Alphanumeric |
| `^.+@.+\\..+$` | Basic email |
| `^\\d{3}-\\d{3}-\\d{4}$` | US phone (XXX-XXX-XXXX) |
| `^\\d{5}$` | US ZIP code |

---

## See Also

- [regexMatches Function](./regexMatches.md)
- [regexSwap Function](./regexSwap.md)
- [contains Function](./contains.md)
- [#if Helper](../helpers/if.md)

---
