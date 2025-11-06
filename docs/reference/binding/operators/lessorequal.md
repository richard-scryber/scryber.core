---
layout: default
title: "<= (Less Than or Equal)"
parent: Binding Operators
parent_url: /reference/binding/operators/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# <= : Less Than or Equal Operator
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

Compare if the left value is less than or equal to the right value.

## Syntax

```handlebars
{{operand1 <= operand2}}
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

`true` if left operand is less than or equal to right operand, `false` otherwise.

---

## Examples

### Low Stock Alert

```handlebars
{{#if model.stock <= 10}}
  <div class="alert-warning">
    <strong>Low Stock Warning</strong>
    <p>Only {{model.stock}} units left - reorder soon!</p>
  </div>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    stock = 8
};
```

**Output:**
```html
<div class="alert-warning">
  <strong>Low Stock Warning</strong>
  <p>Only 8 units left - reorder soon!</p>
</div>
```

### Score Validation

```handlebars
{{#if model.score <= 100}}
  <p class="valid">Score: {{model.score}}/100</p>
{{else}}
  <p class="error">Invalid score: {{model.score}} (must be ≤ 100)</p>
{{/if}}
```

### Capacity Check

```handlebars
{{#if model.attendees <= model.capacity}}
  <span class="available">{{model.capacity - model.attendees}} spots available</span>
{{else}}
  <span class="full">Event is full</span>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    attendees = 98,
    capacity = 100
};
```

**Output:**
```html
<span class="available">2 spots available</span>
```

### Date Deadline Check

```handlebars
{{#if model.dueDate <= model.today}}
  <div class="overdue">
    <strong>Overdue!</strong>
    <p>Due: {{format(model.dueDate, 'MMM dd, yyyy')}}</p>
  </div>
{{/if}}
```

### Price Range Filter

```handlebars
{{#each model.products}}
  {{#if this.price <= model.maxBudget}}
    <div class="product-affordable">
      <h3>{{this.name}}</h3>
      <p>${{this.price}}</p>
      <span class="badge">Within Budget</span>
    </div>
  {{/if}}
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    maxBudget = 50,
    products = new[] {
        new { name = "Basic Widget", price = 29.99m },
        new { name = "Premium Widget", price = 79.99m },
        new { name = "Standard Widget", price = 49.99m }
    }
};
```

**Output:**
```html
<div class="product-affordable">
  <h3>Basic Widget</h3>
  <p>$29.99</p>
  <span class="badge">Within Budget</span>
</div>
<div class="product-affordable">
  <h3>Standard Widget</h3>
  <p>$49.99</p>
  <span class="badge">Within Budget</span>
</div>
```

---

## Notes

- Works with numbers, dates, and comparable types
- Includes equality (`=`) unlike `<` operator
- String comparison is case-sensitive and uses lexicographic ordering
- Date comparison compares chronological order
- Cannot compare incompatible types
- Commonly used with `{{#if}}` for conditional rendering
- Can be combined with logical operators (`&&`, `||`)

---

## See Also

- [Less Than Operator](./lessthan.md)
- [Greater Than Operator](./greaterthan.md)
- [Greater Than or Equal Operator](./greaterorequal.md)
- [Equality Operator](./equality.md)
- [#if Helper](../helpers/if.md)

---
