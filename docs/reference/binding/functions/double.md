---
layout: default
title: double
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# double() : Convert to Double
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

Convert a value to a double-precision floating-point number.

## Signature

```
double(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Any | Yes | The value to convert to double |

---

## Returns

**Type:** Double

A double-precision floating-point representation of the value.

---

## Examples

### Convert String to Double

```handlebars
<p>Value: {{double(model.stringValue)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    stringValue = "3.14159"
};
```

**Output:**
```html
<p>Value: 3.14159</p>
```

### Scientific Calculations

```handlebars
<p>Result: {{format(double(model.coefficient) * pow(10, model.exponent), '0.00e+00')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    coefficient = "6.626",
    exponent = -34
};
```

**Output:**
```html
<p>Result: 6.63e-34</p>
```

### Percentage to Decimal

```handlebars
<p>Rate: {{double(model.percentage) / 100}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    percentage = "8.5"
};
```

**Output:**
```html
<p>Rate: 0.085</p>
```

---

## Notes

- Preserves decimal precision
- Handles string-to-double conversion
- Throws exception if value cannot be converted
- Range: ±5.0 × 10^−324 to ±1.7 × 10^308
- For exact decimal calculations (like currency), use `decimal()` instead
- Subject to floating-point precision limitations

---

## See Also

- [decimal Function](./decimal.md)
- [int Function](./int.md)
- [format Function](./format.md)
- [round Function](./round.md)

---
