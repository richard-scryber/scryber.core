---
layout: default
title: log10
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# log10() : Base-10 Logarithm
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

Calculate the base-10 logarithm of a number.

## Signature

```
log10(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Number | Yes | The number (must be positive) |

---

## Returns

**Type:** Double

The base-10 logarithm of the value.

---

## Examples

### Basic Log10

```handlebars
<p>log₁₀({{model.value}}) = {{format(log10(model.value), '0.000')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    value = 100
};
```

**Output:**
```html
<p>log₁₀(100) = 2.000</p>
```

### Calculate Magnitude

```handlebars
<p>Order of magnitude: 10^{{floor(log10(model.value))}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    value = 45000
};
```

**Output:**
```html
<p>Order of magnitude: 10^4</p>
```

### Decibel Calculation

```handlebars
<p>dB: {{format(10 * log10(model.power / model.reference), '0.0')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    power = 100,
    reference = 1
};
```

**Output:**
```html
<p>dB: 20.0</p>
```

---

## Notes

- Value must be positive (> 0)
- Returns double-precision floating-point
- Common in scientific and engineering calculations
- For natural logarithm, use `log()`
- log10(10) = 1, log10(100) = 2, log10(1000) = 3

---

## See Also

- [log Function](./log.md)
- [pow Function](./pow.md)
- [floor Function](./floor.md)

---
