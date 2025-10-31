---
layout: default
title: if
parent: Handlebars Helpers
parent_url: /reference/binding/helpers/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# {{#if}} : Conditional Rendering Helper
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

Conditionally render content based on expressions. Supports multiple branches with `{{else if}}` and `{{else}}`.

**Based on:** [`<choose>`, `<when>`, and `<otherwise>` elements](../../components/choose.md) for conditional template rendering

## Syntax

```handlebars
{{#if condition}}
  <!-- Rendered if condition is true -->
{{else if condition2}}
  <!-- Rendered if condition2 is true -->
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

| Operator | Description | Example |
|----------|-------------|---------|
| `==` | Equality | `model.status == 'active'` |
| `!=` | Inequality | `model.count != 0` |
| `<` | Less than | `model.age < 18` |
| `<=` | Less than or equal | `model.score <= 100` |
| `>` | Greater than | `model.value > 0` |
| `>=` | Greater than or equal | `model.age >= 18` |
| `&&` | Logical AND | `model.age >= 18 && model.hasLicense` |
| `||` | Logical OR | `model.isAdmin || model.isModerator` |

---

## Examples

### Simple Condition

```handlebars
{{#if model.isActive}}
  <span class="badge-active">Active</span>
{{/if}}
```

### If-Else

```handlebars
{{#if model.stock > 0}}
  <button>Add to Cart</button>
{{else}}
  <span class="out-of-stock">Out of Stock</span>
{{/if}}
```

### Multiple Conditions

```handlebars
{{#if model.score >= 90}}
  <span class="grade-a">Excellent (A)</span>
{{else if model.score >= 80}}
  <span class="grade-b">Good (B)</span>
{{else if model.score >= 70}}
  <span class="grade-c">Average (C)</span>
{{else}}
  <span class="grade-f">Needs Improvement (F)</span>
{{/if}}
```

### Comparison Operators

```handlebars
<!-- Equality -->
{{#if model.status == 'approved'}}
  <span>✓ Approved</span>
{{/if}}

<!-- Greater than or equal -->
{{#if model.score >= 70}}
  <p class="pass">Passed</p>
{{/if}}

<!-- Less than -->
{{#if model.stock <= 10}}
  <p class="alert">Low stock warning!</p>
{{/if}}
```

### Logical Operators

```handlebars
<!-- AND (&&) -->
{{#if model.age >= 18 && model.hasLicense}}
  <p>Eligible to drive</p>
{{else}}
  <p>Not eligible to drive</p>
{{/if}}

<!-- OR (||) -->
{{#if model.isAdmin || model.isModerator}}
  <div class="admin-panel">
    <h2>Administration</h2>
  </div>
{{/if}}
```

### With #each Context

```handlebars
{{#each model.users}}
  <div class="user">
    <h3>{{this.name}}</h3>

    {{#if this.role == 'admin'}}
      <span class="badge-admin">Administrator</span>
    {{else if this.role == 'moderator'}}
      <span class="badge-mod">Moderator</span>
    {{else}}
      <span class="badge-user">User</span>
    {{/if}}
  </div>
{{/each}}
```

---

## Underlying Implementation

The `{{#if}}` helper compiles to Scryber's conditional rendering elements:

```xml
<choose>
  <when data-test="{{condition}}">
    <!-- Content when true -->
  </when>
  <when data-test="{{condition2}}">
    <!-- Content for else if -->
  </when>
  <otherwise>
    <!-- Content for else -->
  </otherwise>
</choose>
```

This structure allows Scryber to evaluate conditions at databinding time and only render the matching branch.

---

## Notes

- Expressions are evaluated at databinding time
- **Falsy values**: `false`, `null`, `0`, empty string
- **Truthy values**: everything else
- Use parentheses for complex expressions: `(a && b) || c`
- Cannot use `!` (NOT) operator - use `!=` or reverse logic instead
- Multiple `{{else if}}` branches are supported
- Only one branch is rendered in the output

---

## See Also

- [else if Helper](./elseif.md)
- [else Helper](./else.md)
- [Comparison Operators](../operators/equality.md)
- [Logical Operators](../operators/and.md)
- [Conditional Rendering Guide](../../learning/02-data-binding/04_conditional_rendering.md)

---
