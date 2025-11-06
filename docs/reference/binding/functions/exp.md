---
layout: default
title: exp
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# exp() : Exponential (e^x)
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

Calculate e (Euler's number) raised to a specified power.

## Signature

```
exp(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Number | Yes | The exponent |

---

## Returns

**Type:** Double

e raised to the power of the value (e^value).

---

## Examples

### Basic Exponential

```handlebars
<p>e^{{model.value}} = {{format(exp(model.value), '0.00')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    value = 2
};
```

**Output:**
```html
<p>e^2 = 7.39</p>
```

### Growth Calculation

```handlebars
<p>Population: {{format(model.initial * exp(model.rate * model.time), '0')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    initial = 1000,
    rate = 0.05,
    time = 10
};
```

**Output:**
```html
<p>Population: 1649</p>
```

### Exponential Decay

```handlebars
<p>Remaining: {{format(model.initial * exp(-model.decayRate * model.time), '0.00')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    initial = 100,
    decayRate = 0.1,
    time = 5
};
```

**Output:**
```html
<p>Remaining: 60.65</p>
```

---

## Notes

- Returns e^x where e ≈ 2.71828
- Inverse of `log()` (natural logarithm)
- Used in continuous growth/decay models
- Returns double-precision floating-point
- For other bases, use `pow(base, exponent)`

---

## See Also

- [log Function](./log.md)
- [pow Function](./pow.md)
- [e Function](./e.md)

---
