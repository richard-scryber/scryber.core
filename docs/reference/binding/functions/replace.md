---
layout: default
title: replace
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# replace() : Replace Text
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

Replace all occurrences of a substring with another string.

## Signature

```
replace(str, find, replacement)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The source string |
| `find` | String | Yes | The substring to find |
| `replacement` | String | Yes | The replacement text |

---

## Returns

**Type:** String

A new string with all occurrences replaced.

---

## Examples

### Simple Replacement

```handlebars
<p>{{replace(model.text, 'old', 'new')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "The old car was sold to an old friend."
};
```

**Output:**
```html
<p>The new car was snew to an new friend.</p>
```

### Remove Characters

```handlebars
<p>Phone: {{replace(model.phone, '-', '')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    phone = "555-123-4567"
};
```

**Output:**
```html
<p>Phone: 5551234567</p>
```

### Format Display

```handlebars
<p>{{replace(model.code, '_', ' ')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    code = "PREMIUM_CUSTOMER_DISCOUNT"
};
```

**Output:**
```html
<p>PREMIUM CUSTOMER DISCOUNT</p>
```

### Sanitize Input

```handlebars
{{#each model.items}}
  <p>{{replace(replace(this.name, '<', '&lt;'), '>', '&gt;')}}</p>
{{/each}}
```

### Template Variable Replacement

```handlebars
<p>{{replace(replace(model.template, '{name}', model.name), '{date}', model.date)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    template = "Hello {name}, today is {date}",
    name = "John",
    date = "March 15"
};
```

**Output:**
```html
<p>Hello John, today is March 15</p>
```

---

## Notes

- Replaces all occurrences (not just first)
- Case-sensitive matching
- Returns original string if find text not found
- To remove text, use empty string as replacement
- For pattern-based replacement, use `regexSwap()`
- For single character replacement, more efficient than regex

---

## See Also

- [regexSwap Function](./regexSwap.md)
- [concat Function](./concat.md)
- [substring Function](./substring.md)

---
