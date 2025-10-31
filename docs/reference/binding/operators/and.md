---
layout: default
title: "&& (Logical AND)"
parent: Binding Operators
parent_url: /reference/binding/operators/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# && : Logical AND Operator
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

Combine two boolean expressions. Returns true only if both operands are true.

## Syntax

```handlebars
{{condition1 && condition2}}
```

---

## Precedence

Priority level in expression evaluation (1 = highest, 10 = lowest): **9**

Evaluated after: `^`, `*`, `/`, `%`, `+`, `-`, `<`, `<=`, `>`, `>=`, `==`, `!=`, `??`

Evaluated before: `||`

---

## Operands

| Position | Type | Description |
|----------|------|-------------|
| Left | Boolean | First condition to evaluate |
| Right | Boolean | Second condition to evaluate |

---

## Returns

**Type:** Boolean

`true` if both operands are true, `false` otherwise.

---

## Truth Table

| Left | Right | Result |
|------|-------|--------|
| true | true | **true** |
| true | false | false |
| false | true | false |
| false | false | false |

---

## Examples

### Driving Eligibility

```handlebars
{{#if model.age >= 18 && model.hasLicense}}
  <div class="eligible">
    <p>✓ Eligible to drive</p>
  </div>
{{else}}
  <div class="not-eligible">
    <p>Not eligible to drive</p>
    {{#if model.age < 18}}
      <p>Reason: Must be 18 or older</p>
    {{/if}}
    {{#if !model.hasLicense}}
      <p>Reason: Valid license required</p>
    {{/if}}
  </div>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    age = 20,
    hasLicense = true
};
```

**Output:**
```html
<div class="eligible">
  <p>✓ Eligible to drive</p>
</div>
```

### Multiple Conditions

```handlebars
{{#if model.isActive && model.isPaid && model.isVerified}}
  <div class="status-valid">
    <h3>Account Status: Active</h3>
    <p>All requirements met</p>
  </div>
{{else}}
  <div class="status-incomplete">
    <h3>Account Incomplete</h3>
    <ul>
      {{#if !model.isActive}}<li>Account not activated</li>{{/if}}
      {{#if !model.isPaid}}<li>Payment required</li>{{/if}}
      {{#if !model.isVerified}}<li>Verification pending</li>{{/if}}
    </ul>
  </div>
{{/if}}
```

### Range Check

```handlebars
{{#if model.age >= 13 && model.age < 20}}
  <span class="age-group">Teenager</span>
{{else if model.age >= 20 && model.age < 65}}
  <span class="age-group">Adult</span>
{{else}}
  <span class="age-group">Other</span>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    age = 17
};
```

**Output:**
```html
<span class="age-group">Teenager</span>
```

### Stock and Price Validation

```handlebars
{{#each model.products}}
  {{#if this.stock > 0 && this.price <= model.maxBudget}}
    <div class="product-available">
      <h3>{{this.name}}</h3>
      <p>Price: ${{this.price}}</p>
      <p>Stock: {{this.stock}}</p>
      <button>Add to Cart</button>
    </div>
  {{/if}}
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    maxBudget = 100,
    products = new[] {
        new { name = "Widget A", price = 49.99m, stock = 10 },
        new { name = "Widget B", price = 149.99m, stock = 5 },
        new { name = "Widget C", price = 79.99m, stock = 0 }
    }
};
```

**Output:**
```html
<div class="product-available">
  <h3>Widget A</h3>
  <p>Price: $49.99</p>
  <p>Stock: 10</p>
  <button>Add to Cart</button>
</div>
```

### Priority Classification

```handlebars
{{#if model.isUrgent && model.isHighValue}}
  <div class="priority-critical">
    <strong>CRITICAL PRIORITY</strong>
  </div>
{{else if model.isUrgent}}
  <div class="priority-high">
    <strong>High Priority</strong>
  </div>
{{else if model.isHighValue}}
  <div class="priority-medium">
    <strong>Medium Priority</strong>
  </div>
{{else}}
  <div class="priority-low">
    <strong>Standard</strong>
  </div>
{{/if}}
```

### Date Range Validation

```handlebars
{{#if model.startDate <= model.today && model.endDate >= model.today}}
  <div class="active-period">
    <p>✓ Currently Active</p>
  </div>
{{else}}
  <div class="inactive-period">
    <p>Not currently active</p>
  </div>
{{/if}}
```

---

## Notes

- Both conditions must be true for result to be true
- Uses short-circuit evaluation: if first condition is false, second is not evaluated
- Can chain multiple AND conditions: `a && b && c`
- Use parentheses for complex expressions: `(a && b) || (c && d)`
- Commonly combined with comparison operators
- Has higher precedence than OR (`||`) operator
- Note: `!` (NOT) operator is not supported - use `!=` instead

---

## See Also

- [Logical OR Operator](./or.md)
- [Equality Operator](./equality.md)
- [Inequality Operator](./inequality.md)
- [#if Helper](../helpers/if.md)
- [Comparison Operators](./lessthan.md)

---
