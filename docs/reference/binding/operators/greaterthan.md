---
layout: default
title: "> (Greater Than)"
parent: Binding Operators
parent_url: /reference/binding/operators/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# > : Greater Than Operator
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

Compare if the left value is greater than the right value.

## Syntax

```handlebars
{{operand1 > operand2}}
```

---

## Precedence

Priority level in expression evaluation (1 = highest, 10 = lowest): **6**

Evaluated after: `^`, `*`, `/`, `%`, `+`, `-`

Evaluated before: `==`, `!=`, `??`, `&&`, `||`

---

## Operands

| Position | Type | Description |
|----------|------|-------------|
| Left | Comparable | First value to compare |
| Right | Comparable | Second value to compare |

---

## Returns

**Type:** Boolean

`true` if left operand is greater than right operand, `false` otherwise.

---

## Examples

### Adult Verification

```handlebars
{{#if model.age > 18}}
  <div class="access-granted">
    <p>Age verified: {{model.age}} years old</p>
  </div>
{{else}}
  <div class="access-denied">
    <p>Must be 18 or older</p>
  </div>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    age = 25
};
```

**Output:**
```html
<div class="access-granted">
  <p>Age verified: 25 years old</p>
</div>
```

### High Value Order

```handlebars
{{#if model.orderTotal > 1000}}
  <div class="vip-order">
    <strong>High-Value Order</strong>
    <p>Expedited processing approved</p>
  </div>
{{/if}}
```

### Stock Availability

```handlebars
{{#if model.quantity > 0}}
  <button class="btn-primary">Add to Cart</button>
  <p>{{model.quantity}} in stock</p>
{{else}}
  <button class="btn-disabled" disabled>Out of Stock</button>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    quantity = 15
};
```

**Output:**
```html
<button class="btn-primary">Add to Cart</button>
<p>15 in stock</p>
```

### Performance Indicator

```handlebars
{{#if model.revenue > model.target}}
  <div class="success">
    <h3>Target Exceeded!</h3>
    <p>Revenue: ${{format(model.revenue, 'N0')}}</p>
    <p>Target: ${{format(model.target, 'N0')}}</p>
    <p>Surplus: ${{format(model.revenue - model.target, 'N0')}}</p>
  </div>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    revenue = 125000m,
    target = 100000m
};
```

**Output:**
```html
<div class="success">
  <h3>Target Exceeded!</h3>
  <p>Revenue: $125,000</p>
  <p>Target: $100,000</p>
  <p>Surplus: $25,000</p>
</div>
```

### Temperature Warning

```handlebars
{{#if model.temperature > 100}}
  <span class="danger">⚠ High Temperature Alert: {{model.temperature}}°F</span>
{{else if model.temperature > 80}}
  <span class="warning">Warm: {{model.temperature}}°F</span>
{{else}}
  <span class="normal">{{model.temperature}}°F</span>
{{/if}}
```

---

## Notes

- Works with numbers, dates, and comparable types
- String comparison is case-sensitive and uses lexicographic ordering
- Date comparison compares chronological order
- Cannot compare incompatible types
- Commonly used with `{{#if}}` for conditional rendering
- Can be combined with logical operators (`&&`, `||`)
- For "greater than or equal", use `>=` operator

---

## See Also

- [Greater Than or Equal Operator](./greaterorequal.md)
- [Less Than Operator](./lessthan.md)
- [Less Than or Equal Operator](./lessorequal.md)
- [#if Helper](../helpers/if.md)
- [Equality Operator](./equality.md)

---
