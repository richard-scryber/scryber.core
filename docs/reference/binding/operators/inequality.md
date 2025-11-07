---
layout: default
title: "!= (Inequality)"
parent: Binding Operators
parent_url: /reference/binding/operators/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# != : Inequality Operator
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

Compare two values for inequality. Returns true if values are not equal.

## Syntax

```handlebars
{{operand1 != operand2}}
```

---

## Precedence

Priority level in expression evaluation (1 = highest, 10 = lowest): **7**

Evaluated after: `^`, `*`, `/`, `%`, `+`, `-`, `<`, `<=`, `>`, `>=`

Evaluated before: `??`, `&&`, `||`

---

## Operands

| Position | Type | Description |
|----------|------|-------------|
| Left | Any | First value to compare |
| Right | Any | Second value to compare |

---

## Returns

**Type:** Boolean

`true` if the operands are not equal, `false` if they are equal.

---

## Examples

### Exclude Specific Status

```handlebars
{{#if model.status != 'cancelled'}}
  <div class="order-active">
    <h3>Order #{{model.orderNumber}}</h3>
    <p>Status: {{model.status}}</p>
  </div>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    status = "pending",
    orderNumber = "12345"
};
```

**Output:**
```html
<div class="order-active">
  <h3>Order #12345</h3>
  <p>Status: pending</p>
</div>
```

### Check for Non-Zero

```handlebars
{{#if model.balance != 0}}
  <p class="outstanding">Balance due: ${{format(model.balance, '0.00')}}</p>
{{/if}}
```

### Filter Out Null Values

```handlebars
{{#each model.users}}
  {{#if this.email != null}}
    <p>{{this.name}}: {{this.email}}</p>
  {{/if}}
{{/each}}
```

### Highlight Changes

```handlebars
{{#each model.items}}
  {{#if this.currentPrice != this.previousPrice}}
    <div class="price-changed">
      <span>{{this.name}}</span>
      <span class="old">${{this.previousPrice}}</span>
      <span class="new">${{this.currentPrice}}</span>
    </div>
  {{/if}}
{{/each}}
```

### Show Alert for Specific Conditions

```handlebars
{{#if model.stock != model.expectedStock}}
  <div class="alert-warning">
    <p>Stock discrepancy detected!</p>
    <p>Expected: {{model.expectedStock}}, Actual: {{model.stock}}</p>
  </div>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    stock = 47,
    expectedStock = 50
};
```

**Output:**
```html
<div class="alert-warning">
  <p>Stock discrepancy detected!</p>
  <p>Expected: 50, Actual: 47</p>
</div>
```

---

## Notes

- Works with all data types (strings, numbers, booleans, objects)
- String comparison is case-sensitive
- For null checks, use `value != null` or null coalescing `??`
- Opposite of `==` (equality) operator
- Commonly used with `{{#if}}` for conditional rendering
- Can be combined with logical operators (`&&`, `||`)
- Type conversion may occur between compatible types

---

## See Also

- [Equality Operator](./equality.md)
- [#if Helper](../helpers/if.md)
- [Logical AND Operator](./and.md)
- [Logical OR Operator](./or.md)
- [Null Coalescing Operator](./nullcoalesce.md)

---
