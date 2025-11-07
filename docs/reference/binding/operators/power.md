---
layout: default
title: "^ (Power)"
parent: Binding Operators
parent_url: /reference/binding/operators/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# ^ : Power (Exponentiation) Operator
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

Raise a number to a specified power (exponentiation).

## Syntax

```handlebars
{{base ^ exponent}}
```

---

## Precedence

Priority level in expression evaluation (1 = highest, 10 = lowest): **3**

Evaluated after: `()`, `.`

Evaluated before: `*`, `/`, `%`, `+`, `-`, comparison, equality, `??`, `&&`, `||`

---

## Operands

| Position | Type | Description |
|----------|------|-------------|
| Left | Number | Base value |
| Right | Number | Exponent (power to raise to) |

---

## Returns

**Type:** Double

The base raised to the power of the exponent.

---

## Examples

### Square

```handlebars
<p>Area of square: {{model.side ^ 2}} sq ft</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    side = 5
};
```

**Output:**
```html
<p>Area of square: 25 sq ft</p>
```

### Cube

```handlebars
<p>Volume of cube: {{model.side ^ 3}} cubic ft</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    side = 3
};
```

**Output:**
```html
<p>Volume of cube: 27 cubic ft</p>
```

### Compound Interest

```handlebars
<p>Future Value: ${{format(model.principal * (1 + model.rate) ^ model.years, '0.00')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    principal = 1000m,
    rate = 0.05,
    years = 10
};
```

**Output:**
```html
<p>Future Value: $1628.89</p>
```

### Scientific Notation

```handlebars
<p>Speed of light: {{3 ^ 8}} m/s</p>
```

### Power of 10

```handlebars
<p>{{model.value * (10 ^ model.decimals)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    value = 1.5,
    decimals = 3
};
```

**Output:**
```html
<p>1500</p>
```

---

## Notes

- Highest precedence among arithmetic operators
- Result is always a double-precision floating point number
- Use `pow(base, exponent)` function for same functionality
- For square root, use `sqrt()` function or `value ^ 0.5`
- Negative exponents calculate reciprocal (1/x^n)
- Fractional exponents calculate roots (x^(1/n) = nth root)
- Cannot use with non-numeric types

---

## Common Powers

| Expression | Description | Example |
|------------|-------------|---------|
| `x ^ 2` | Square | `5 ^ 2 = 25` |
| `x ^ 3` | Cube | `3 ^ 3 = 27` |
| `x ^ 0.5` | Square root | `16 ^ 0.5 = 4` |
| `x ^ -1` | Reciprocal | `2 ^ -1 = 0.5` |
| `10 ^ n` | Power of 10 | `10 ^ 3 = 1000` |

---

## See Also

- [Multiplication Operator](./multiplication.md)
- [pow Function](../functions/pow.md)
- [sqrt Function](../functions/sqrt.md)
- [exp Function](../functions/exp.md)

---
