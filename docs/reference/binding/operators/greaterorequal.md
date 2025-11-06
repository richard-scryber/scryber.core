---
layout: default
title: ">= (Greater Than or Equal)"
parent: Binding Operators
parent_url: /reference/binding/operators/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# >= : Greater Than or Equal Operator
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

Compare if the left value is greater than or equal to the right value.

## Syntax

```handlebars
{{operand1 >= operand2}}
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

`true` if left operand is greater than or equal to right operand, `false` otherwise.

---

## Examples

### Age Verification

```handlebars
{{#if model.age >= 18}}
  <div class="eligible">
    <p>✓ Age requirement met ({{model.age}} years old)</p>
  </div>
{{else}}
  <div class="ineligible">
    <p>Must be 18 or older (currently {{model.age}})</p>
  </div>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    age = 18
};
```

**Output:**
```html
<div class="eligible">
  <p>✓ Age requirement met (18 years old)</p>
</div>
```

### Passing Grade

```handlebars
{{#if model.score >= 70}}
  <div class="pass">
    <h3>Passed</h3>
    <p>Score: {{model.score}}/100</p>
  </div>
{{else}}
  <div class="fail">
    <h3>Did Not Pass</h3>
    <p>Score: {{model.score}}/100 (70 required)</p>
  </div>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    score = 85
};
```

**Output:**
```html
<div class="pass">
  <h3>Passed</h3>
  <p>Score: 85/100</p>
</div>
```

### Grade Classification

```handlebars
{{#if model.score >= 90}}
  <span class="grade-a">A - Excellent</span>
{{else if model.score >= 80}}
  <span class="grade-b">B - Good</span>
{{else if model.score >= 70}}
  <span class="grade-c">C - Satisfactory</span>
{{else if model.score >= 60}}
  <span class="grade-d">D - Needs Improvement</span>
{{else}}
  <span class="grade-f">F - Failing</span>
{{/if}}
```

### Free Shipping Eligibility

```handlebars
{{#if model.orderTotal >= 50}}
  <div class="free-shipping">
    <strong>✓ Free Shipping Eligible</strong>
    <p>Order total: ${{format(model.orderTotal, '0.00')}}</p>
  </div>
{{else}}
  <div class="shipping-required">
    <p>Add ${{format(50 - model.orderTotal, '0.00')}} more for free shipping</p>
  </div>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    orderTotal = 65.00m
};
```

**Output:**
```html
<div class="free-shipping">
  <strong>✓ Free Shipping Eligible</strong>
  <p>Order total: $65.00</p>
</div>
```

### Access Level Validation

```handlebars
{{#if model.accessLevel >= 5}}
  <div class="admin-panel">
    <h2>Administration</h2>
    <p>Full access granted (Level {{model.accessLevel}})</p>
  </div>
{{else if model.accessLevel >= 3}}
  <div class="moderator-panel">
    <h2>Moderation</h2>
    <p>Limited access (Level {{model.accessLevel}})</p>
  </div>
{{else}}
  <div class="user-panel">
    <h2>User Dashboard</h2>
  </div>
{{/if}}
```

### Minimum Quantity Check

```handlebars
{{#each model.items}}
  {{#if this.quantity >= this.minimumOrder}}
    <div class="valid-order">
      <p>{{this.name}}: {{this.quantity}} units</p>
    </div>
  {{else}}
    <div class="below-minimum">
      <p>{{this.name}}: {{this.quantity}} units</p>
      <small>Minimum order: {{this.minimumOrder}} units</small>
    </div>
  {{/if}}
{{/each}}
```

---

## Notes

- Works with numbers, dates, and comparable types
- Includes equality (`=`) unlike `>` operator
- String comparison is case-sensitive and uses lexicographic ordering
- Date comparison compares chronological order
- Cannot compare incompatible types
- Commonly used with `{{#if}}` for conditional rendering
- Can be combined with logical operators (`&&`, `||`)
- Very common for threshold checks and eligibility validation

---

## See Also

- [Greater Than Operator](./greaterthan.md)
- [Less Than Operator](./lessthan.md)
- [Less Than or Equal Operator](./lessorequal.md)
- [Equality Operator](./equality.md)
- [#if Helper](../helpers/if.md)

---
