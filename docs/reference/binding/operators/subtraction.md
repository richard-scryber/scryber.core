---
layout: default
title: "- (Subtraction)"
parent: Binding Operators
parent_url: /reference/binding/operators/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# - : Subtraction Operator
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

Subtract one numeric value from another.

## Syntax

```handlebars
{{operand1 - operand2}}
```

---

## Precedence

Priority level in expression evaluation (1 = highest, 10 = lowest): **5**

Evaluated after: `^`, `*`, `/`, `%`

Evaluated before: `<`, `<=`, `>`, `>=`, `==`, `!=`, `??`, `&&`, `||`

---

## Operands

| Position | Type | Description |
|----------|------|-------------|
| Left | Number | The value to subtract from (minuend) |
| Right | Number | The value to subtract (subtrahend) |

---

## Returns

**Type:** Number (Int, Long, Double, or Decimal depending on operands)

The difference between the left and right operands.

---

## Examples

### Basic Subtraction

```handlebars
<p>Discount: ${{model.originalPrice - model.salePrice}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    originalPrice = 99.99m,
    salePrice = 79.99m
};
```

**Output:**
```html
<p>Discount: $20</p>
```

### Calculating Remaining Stock

```handlebars
<p>Remaining: {{model.totalStock - model.soldQuantity}} units</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    totalStock = 500,
    soldQuantity = 347
};
```

**Output:**
```html
<p>Remaining: 153 units</p>
```

### Date Difference (with days)

```handlebars
<p>Days remaining: {{daysBetween(model.today, model.deadline)}}</p>
```

### Variance Calculation

```handlebars
<p>Variance: {{model.actual - model.expected}}</p>
{{#if model.actual - model.expected > 0}}
  <span class="positive">Above target</span>
{{else}}
  <span class="negative">Below target</span>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    actual = 125000m,
    expected = 100000m
};
```

**Output:**
```html
<p>Variance: 25000</p>
<span class="positive">Above target</span>
```

### Combined with Other Operators

```handlebars
<!-- Discount percentage -->
<p>Save: {{format((model.originalPrice - model.salePrice) / model.originalPrice * 100, '0')}}%</p>

<!-- Net profit -->
<p>Profit: ${{format(model.revenue - model.costs - model.taxes, '0.00')}}</p>
```

---

## Notes

- Works with all numeric types (int, long, double, decimal)
- Result type depends on operand types (follows C# numeric promotion rules)
- Use parentheses to control order of operations
- For absolute difference, use `abs(a - b)`
- Cannot subtract non-numeric types
- Date subtraction should use date/time functions like `daysBetween()`

---

## See Also

- [Addition Operator](./addition.md)
- [Multiplication Operator](./multiplication.md)
- [Division Operator](./division.md)
- [abs Function](../functions/abs.md)
- [daysBetween Function](../functions/daysBetween.md)

---
