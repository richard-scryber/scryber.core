---
layout: default
title: sqrt
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# sqrt() : Square Root
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

Calculate the square root of a number.

## Signature

```
sqrt(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Number | Yes | The number (must be non-negative) |

---

## Returns

**Type:** Double

The square root of the value.

---

## Examples

### Basic Square Root

```handlebars
<p>√{{model.value}} = {{format(sqrt(model.value), '0.00')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    value = 25
};
```

**Output:**
```html
<p>√25 = 5.00</p>
```

### Calculate Distance

```handlebars
<p>Distance: {{format(sqrt(model.dx * model.dx + model.dy * model.dy), '0.00')}} units</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    dx = 3,
    dy = 4
};
```

**Output:**
```html
<p>Distance: 5.00 units</p>
```

### Standard Deviation Component

```handlebars
<p>Std Dev: {{format(sqrt(model.variance), '0.000')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    variance = 15.625
};
```

**Output:**
```html
<p>Std Dev: 3.953</p>
```

---

## Notes

- Returns double-precision floating-point
- Value must be non-negative (≥ 0)
- Negative values will throw an exception
- For nth root, use `pow(value, 1/n)`
- Common in geometric and statistical calculations

---

## See Also

- [pow Function](./pow.md)
- [Power Operator](../operators/power.md)
- [Multiplication Operator](../operators/multiplication.md)

---
