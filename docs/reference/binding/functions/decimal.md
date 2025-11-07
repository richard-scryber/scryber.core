---
layout: default
title: decimal
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# decimal() : Convert to Decimal
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

Convert a value to a decimal number with exact precision. Best for financial calculations.

## Signature

```
decimal(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Any | Yes | The value to convert to decimal |

---

## Returns

**Type:** Decimal

A decimal representation with exact precision (28-29 significant digits).

---

## Examples

### Currency Conversion

```handlebars
<p>Total: ${{format(decimal(model.price), '0.00')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    price = "19.99"
};
```

**Output:**
```html
<p>Total: $19.99</p>
```

### Financial Calculations

```handlebars
<p>Interest: ${{format(decimal(model.principal) * decimal(model.rate), '0.00')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    principal = "1000.00",
    rate = "0.05"
};
```

**Output:**
```html
<p>Interest: $50.00</p>
```

### Precise Arithmetic

```handlebars
<p>Subtotal: ${{format(decimal(model.price) * decimal(model.quantity), '0.00')}}</p>
<p>Tax: ${{format(decimal(model.price) * decimal(model.quantity) * decimal(model.taxRate), '0.00')}}</p>
<p>Total: ${{format(decimal(model.price) * decimal(model.quantity) * (1 + decimal(model.taxRate)), '0.00')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    price = "9.99",
    quantity = "3",
    taxRate = "0.08"
};
```

**Output:**
```html
<p>Subtotal: $29.97</p>
<p>Tax: $2.40</p>
<p>Total: $32.37</p>
```

---

## Notes

- Provides exact decimal precision (no floating-point errors)
- Ideal for financial and monetary calculations
- Handles string-to-decimal conversion
- Throws exception if value cannot be converted
- Range: ±1.0 × 10^−28 to ±7.9228 × 10^28
- More precise than `double()` but smaller range
- Avoids floating-point rounding errors (e.g., 0.1 + 0.2 = 0.3)

---

## See Also

- [double Function](./double.md)
- [int Function](./int.md)
- [format Function](./format.md)
- [round Function](./round.md)

---
