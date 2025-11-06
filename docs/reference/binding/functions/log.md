---
layout: default
title: log
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# log() : Natural Logarithm
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

Calculate the natural logarithm (base e) of a number.

## Signature

```
log(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Number | Yes | The number (must be positive) |

---

## Returns

**Type:** Double

The natural logarithm of the value.

---

## Examples

### Basic Logarithm

```handlebars
<p>ln({{model.value}}) = {{format(log(model.value), '0.000')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    value = 10
};
```

**Output:**
```html
<p>ln(10) = 2.303</p>
```

### Time to Double

```handlebars
<p>Years to double: {{format(log(2) / model.rate, '0.0')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    rate = 0.07
};
```

**Output:**
```html
<p>Years to double: 9.9</p>
```

---

## Notes

- Inverse of `exp()` function
- Value must be positive (> 0)
- Returns double-precision floating-point
- For base-10 logarithm, use `log10()`
- Natural log uses base e ≈ 2.71828

---

## See Also

- [log10 Function](./log10.md)
- [exp Function](./exp.md)
- [e Function](./e.md)

---
