---
layout: default
title: "|| (Logical OR)"
parent: Binding Operators
parent_url: /reference/binding/operators/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# || : Logical OR Operator
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

Combine two boolean expressions. Returns true if at least one operand is true.

## Syntax

```handlebars
{{condition1 || condition2}}
```

---

## Precedence

Priority level in expression evaluation (1 = highest, 10 = lowest): **10** (lowest)

Evaluated after: All other operators

---

## Operands

| Position | Type | Description |
|----------|------|-------------|
| Left | Boolean | First condition to evaluate |
| Right | Boolean | Second condition to evaluate |

---

## Returns

**Type:** Boolean

`true` if either operand is true, `false` only if both are false.

---

## Truth Table

| Left | Right | Result |
|------|-------|--------|
| true | true | **true** |
| true | false | **true** |
| false | true | **true** |
| false | false | false |

---

## Examples

### Administrative Access

```handlebars
{{#if model.isAdmin || model.isModerator}}
  <div class="admin-panel">
    <h2>Administration</h2>
    <p>Access Level: {{if(model.isAdmin, 'Administrator', 'Moderator')}}</p>
  </div>
{{else}}
  <div class="user-panel">
    <h2>User Dashboard</h2>
  </div>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    isAdmin = false,
    isModerator = true
};
```

**Output:**
```html
<div class="admin-panel">
  <h2>Administration</h2>
  <p>Access Level: Moderator</p>
</div>
```

### Multiple Status Check

```handlebars
{{#if model.status == 'shipped' || model.status == 'delivered'}}
  <div class="order-completed">
    <p>✓ Order in transit or completed</p>
    <p>Status: {{model.status}}</p>
  </div>
{{else}}
  <div class="order-processing">
    <p>Order being prepared</p>
    <p>Status: {{model.status}}</p>
  </div>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    status = "shipped"
};
```

**Output:**
```html
<div class="order-completed">
  <p>✓ Order in transit or completed</p>
  <p>Status: shipped</p>
</div>
```

### Weekend Detector

```handlebars
{{#if model.dayOfWeek == 0 || model.dayOfWeek == 6}}
  <div class="weekend">
    <h3>Weekend Hours</h3>
    <p>10:00 AM - 6:00 PM</p>
  </div>
{{else}}
  <div class="weekday">
    <h3>Weekday Hours</h3>
    <p>8:00 AM - 8:00 PM</p>
  </div>
{{/if}}
```

### Alert Conditions

```handlebars
{{#each model.items}}
  {{#if this.stock <= 10 || this.expirationDays < 30}}
    <div class="alert-warning">
      <strong>{{this.name}}</strong>
      {{#if this.stock <= 10}}
        <p>⚠ Low stock: {{this.stock}} units</p>
      {{/if}}
      {{#if this.expirationDays < 30}}
        <p>⚠ Expiring soon: {{this.expirationDays}} days</p>
      {{/if}}
    </div>
  {{/if}}
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    items = new[] {
        new { name = "Item A", stock = 5, expirationDays = 60 },
        new { name = "Item B", stock = 50, expirationDays = 15 },
        new { name = "Item C", stock = 8, expirationDays = 10 }
    }
};
```

**Output:**
```html
<div class="alert-warning">
  <strong>Item A</strong>
  <p>⚠ Low stock: 5 units</p>
</div>
<div class="alert-warning">
  <strong>Item B</strong>
  <p>⚠ Expiring soon: 15 days</p>
</div>
<div class="alert-warning">
  <strong>Item C</strong>
  <p>⚠ Low stock: 8 units</p>
  <p>⚠ Expiring soon: 10 days</p>
</div>
```

### Role-Based Display

```handlebars
{{#if model.role == 'admin' || model.role == 'manager' || model.role == 'supervisor'}}
  <div class="leadership-tools">
    <h3>Leadership Dashboard</h3>
    <p>Role: {{model.role}}</p>
  </div>
{{/if}}
```

### Priority Orders

```handlebars
{{#if model.total > 1000 || model.isPriority || model.customerTier == 'VIP'}}
  <div class="expedited-processing">
    <strong>Expedited Processing</strong>
    {{#if model.total > 1000}}
      <p>High-value order</p>
    {{/if}}
    {{#if model.isPriority}}
      <p>Priority flagged</p>
    {{/if}}
    {{#if model.customerTier == 'VIP'}}
      <p>VIP customer</p>
    {{/if}}
  </div>
{{/if}}
```

---

## Notes

- Returns true if either or both conditions are true
- Uses short-circuit evaluation: if first condition is true, second is not evaluated
- Can chain multiple OR conditions: `a || b || c`
- Has lowest precedence - use parentheses for complex expressions: `(a || b) && c`
- Commonly used for checking multiple acceptable values
- Opposite of AND (`&&`) operator logic
- Note: `!` (NOT) operator is not supported - use `!=` instead

---

## See Also

- [Logical AND Operator](./and.md)
- [Equality Operator](./equality.md)
- [Inequality Operator](./inequality.md)
- [#if Helper](../helpers/if.md)
- [in Function](../functions/in.md)

---
