---
layout: default
title: regexSwap
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# regexSwap() : Replace Using Regular Expression
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

Replace all occurrences matching a regular expression pattern with a replacement string.

## Signature

```
regexSwap(str, pattern, replacement)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The source string |
| `pattern` | String | Yes | The regular expression pattern |
| `replacement` | String | Yes | The replacement text |

---

## Returns

**Type:** String

The string with all pattern matches replaced.

---

## Examples

### Remove All Digits

```handlebars
<p>{{regexSwap(model.text, '\\d', '')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "Product ABC123 costs $45.99"
};
```

**Output:**
```html
<p>Product ABC costs $.</p>
```

### Mask Phone Numbers

```handlebars
<p>{{regexSwap(model.phone, '\\d', 'X')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    phone = "555-123-4567"
};
```

**Output:**
```html
<p>XXX-XXX-XXXX</p>
```

### Replace Multiple Spaces

```handlebars
<p>{{regexSwap(model.text, '\\s+', ' ')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "Too    many     spaces"
};
```

**Output:**
```html
<p>Too many spaces</p>
```

### Sanitize Special Characters

```handlebars
<p>{{regexSwap(model.filename, '[^a-zA-Z0-9_.-]', '_')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    filename = "My File (2024)!.pdf"
};
```

**Output:**
```html
<p>My_File__2024__.pdf</p>
```

### Format Numbers with Separators

```handlebars
<p>{{regexSwap(model.number, '(\\d)(?=(\\d{3})+$)', '$1,')}}</p>
```

### Remove HTML Tags

```handlebars
<p>{{regexSwap(model.html, '<[^>]+>', '')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    html = "<strong>Bold</strong> and <em>italic</em> text"
};
```

**Output:**
```html
<p>Bold and italic text</p>
```

### Convert Dates

```handlebars
<!-- Convert MM/DD/YYYY to YYYY-MM-DD -->
<p>{{regexSwap(model.date, '(\\d{2})/(\\d{2})/(\\d{4})', '$3-$1-$2')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    date = "03/15/2024"
};
```

**Output:**
```html
<p>2024-03-15</p>
```

---

## Notes

- Uses .NET regex syntax
- Replaces all occurrences (not just first)
- Backslashes must be escaped: `\\d`, `\\w`, etc.
- Supports capture groups: `$1`, `$2`, etc.
- For simple replacement, `replace()` is faster
- More powerful but slower than `replace()`
- Returns original string if pattern doesn't match

---

## Common Patterns

| Pattern | Replacement | Description |
|---------|-------------|-------------|
| `\\s+` | `' '` | Normalize whitespace |
| `\\d` | `'X'` | Mask digits |
| `<[^>]+>` | `''` | Remove HTML tags |
| `[^a-zA-Z0-9]` | `'_'` | Replace special chars |
| `(\\d{3})(\\d{3})(\\d{4})` | `'($1) $2-$3'` | Format phone |

---

## See Also

- [regexIsMatch Function](./regexIsMatch.md)
- [regexMatches Function](./regexMatches.md)
- [replace Function](./replace.md)

---
