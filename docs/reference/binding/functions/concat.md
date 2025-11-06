---
layout: default
title: concat
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# concat() : Concatenate Strings
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

Concatenate multiple strings into one.

## Signature

```
concat(str1, str2, str3, ...)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str1, str2, ...` | String | Yes | Strings to concatenate (variable number) |

---

## Returns

**Type:** String

All input strings joined together.

---

## Examples

### Basic Concatenation

```handlebars
{{concat('Hello', ' ', 'World')}}
<!-- Output: Hello World -->
```

### With Variables

```handlebars
<p>{{concat(model.firstName, ' ', model.lastName)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    firstName = "John",
    lastName = "Doe"
};
```

**Output:**
```html
<p>John Doe</p>
```

### Multiple Values

```handlebars
{{concat('Order #', model.orderNumber, ' - ', model.status)}}
<!-- Output: Order #12345 - Shipped -->
```

### With Formatting

```handlebars
{{concat('Total: ', format(model.total, 'C2'))}}
<!-- Output: Total: $99.99 -->
```

### In Log Statement

```handlebars
{{log concat('Processing order ', model.id)}}
```

---

## Notes

- Null values are treated as empty strings
- Non-string values are converted to strings
- Empty strings are preserved
- More efficient than multiple `+` operations for many strings

---

## See Also

- [join Function](./join.md)
- [String Functions](./index.md#string-functions)

---
