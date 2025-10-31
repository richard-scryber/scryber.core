---
layout: default
title: else if
parent: Handlebars Helpers
parent_url: /reference/binding/helpers/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# {{else if}} : Alternative Condition Helper
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

Provides alternative conditional branches within an `{{#if}}` block. Multiple `{{else if}}` branches can be chained to create complex conditional logic.

**Based on:** Additional [`<when>` elements](../../components/choose.md) in the `<choose>` structure

## Syntax

```handlebars
{{#if condition1}}
  <!-- Rendered if condition1 is true -->
{{else if condition2}}
  <!-- Rendered if condition1 is false and condition2 is true -->
{{else if condition3}}
  <!-- Rendered if condition1 and condition2 are false and condition3 is true -->
{{else}}
  <!-- Rendered if all conditions are false -->
{{/if}}
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `condition` | Expression | Yes | Expression that evaluates to true/false |

---

## Supported Operators

Same as `{{#if}}`:

| Operator | Description | Example |
|----------|-------------|---------|
| `==` | Equality | `model.status == 'pending'` |
| `!=` | Inequality | `model.count != 0` |
| `<` | Less than | `model.age < 18` |
| `<=` | Less than or equal | `model.score <= 100` |
| `>` | Greater than | `model.value > 0` |
| `>=` | Greater than or equal | `model.age >= 18` |
| `&&` | Logical AND | `model.age >= 18 && model.hasLicense` |
| `||` | Logical OR | `model.isAdmin || model.isModerator` |

---

## Examples

### Grade Classification

```handlebars
{{#if model.score >= 90}}
  <span class="grade-a">A - Excellent</span>
{{else if model.score >= 80}}
  <span class="grade-b">B - Good</span>
{{else if model.score >= 70}}
  <span class="grade-c">C - Average</span>
{{else if model.score >= 60}}
  <span class="grade-d">D - Below Average</span>
{{else}}
  <span class="grade-f">F - Failing</span>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    score = 75
};
```

**Output:**
```html
<span class="grade-c">C - Average</span>
```

### Order Status

```handlebars
<div class="order-status">
  {{#if model.status == 'shipped'}}
    <span class="badge-success">✓ Shipped</span>
    <p>Tracking: {{model.trackingNumber}}</p>
  {{else if model.status == 'processing'}}
    <span class="badge-info">Processing Order</span>
  {{else if model.status == 'pending'}}
    <span class="badge-warning">Pending Payment</span>
  {{else if model.status == 'cancelled'}}
    <span class="badge-danger">Cancelled</span>
  {{else}}
    <span class="badge-secondary">Unknown Status</span>
  {{/if}}
</div>
```

**Data:**
```csharp
doc.Params["model"] = new {
    status = "processing"
};
```

**Output:**
```html
<div class="order-status">
  <span class="badge-info">Processing Order</span>
</div>
```

### Age Group Classification

```handlebars
{{#if model.age < 13}}
  <p class="child">Child (under 13)</p>
{{else if model.age >= 13 && model.age < 20}}
  <p class="teen">Teenager (13-19)</p>
{{else if model.age >= 20 && model.age < 65}}
  <p class="adult">Adult (20-64)</p>
{{else}}
  <p class="senior">Senior (65+)</p>
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
<p class="teen">Teenager (13-19)</p>
```

### Priority Level with Multiple Conditions

```handlebars
{{#if model.urgent && model.highValue}}
  <div class="priority-critical">
    <h3>CRITICAL PRIORITY</h3>
  </div>
{{else if model.urgent}}
  <div class="priority-high">
    <h3>High Priority</h3>
  </div>
{{else if model.highValue}}
  <div class="priority-medium">
    <h3>Medium Priority</h3>
  </div>
{{else}}
  <div class="priority-low">
    <h3>Standard Priority</h3>
  </div>
{{/if}}
```

---

## Underlying Implementation

The `{{else if}}` helper compiles to additional `<when>` elements in the Scryber `<choose>` structure:

```xml
<choose>
  <when data-test="{{condition1}}">
    <!-- First if block -->
  </when>
  <when data-test="{{condition2}}">
    <!-- First else if block -->
  </when>
  <when data-test="{{condition3}}">
    <!-- Second else if block -->
  </when>
  <otherwise>
    <!-- Final else block -->
  </otherwise>
</choose>
```

Conditions are evaluated in order from top to bottom. Only the first true condition is rendered.

---

## Notes

- Only used within `{{#if}}` blocks
- Can have unlimited `{{else if}}` branches
- Evaluated in order - first true condition wins
- Only one branch is ever rendered
- Must come after `{{#if}}` and before `{{else}}`
- Supports all comparison and logical operators
- Cannot use `!` (NOT) operator - use `!=` or reverse logic instead
- Use parentheses for complex expressions: `(a && b) || c`

---

## See Also

- [#if Helper](./if.md)
- [else Helper](./else.md)
- [Comparison Operators](../operators/equality.md)
- [Logical Operators](../operators/and.md)
- [Conditional Rendering Guide](../../learning/02-data-binding/04_conditional_rendering.md)

---
