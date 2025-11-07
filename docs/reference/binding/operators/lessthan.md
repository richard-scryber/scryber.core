---
layout: default
title: "< (Less Than)"
parent: Binding Operators
parent_url: /reference/binding/operators/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# < : Less Than Operator
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

Compare if the left value is less than the right value.

## Syntax

```handlebars
{{operand1 < operand2}}
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

`true` if left operand is less than right operand, `false` otherwise.

---

## Examples

### Age Restriction

```handlebars
{{#if model.age < 18}}
  <div class="minor-warning">
    <p>Parental consent required (Age: {{model.age}})</p>
  </div>
{{else}}
  <p>Age verified: {{model.age}} years old</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    age = 15
};
```

**Output:**
```html
<div class="minor-warning">
  <p>Parental consent required (Age: 15)</p>
</div>
```

### Low Stock Warning

```handlebars
{{#if model.quantity < model.threshold}}
  <div class="alert-warning">
    <strong>Low Stock Alert!</strong>
    <p>Only {{model.quantity}} units remaining</p>
  </div>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    quantity = 5,
    threshold = 10
};
```

**Output:**
```html
<div class="alert-warning">
  <strong>Low Stock Alert!</strong>
  <p>Only 5 units remaining</p>
</div>
```

### Temperature Check

```handlebars
{{#if model.temperature < 32}}
  <span class="freezing">⚠ Freezing conditions</span>
{{else if model.temperature < 60}}
  <span class="cold">Cold</span>
{{else}}
  <span class="normal">Normal</span>
{{/if}}
```

### Discount Eligibility

```handlebars
{{#each model.products}}
  <div class="product">
    <h3>{{this.name}}</h3>
    <p>Price: ${{this.price}}</p>

    {{#if this.price < 50}}
      <span class="badge">Budget Friendly</span>
    {{/if}}
  </div>
{{/each}}
```

### Progress Tracking

```handlebars
{{#if model.progress < 0.25}}
  <div class="progress-bar red" style="width: {{model.progress * 100}}%">
    {{format(model.progress * 100, '0')}}%
  </div>
{{else if model.progress < 0.75}}
  <div class="progress-bar yellow" style="width: {{model.progress * 100}}%">
    {{format(model.progress * 100, '0')}}%
  </div>
{{else}}
  <div class="progress-bar green" style="width: {{model.progress * 100}}%">
    {{format(model.progress * 100, '0')}}%
  </div>
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
- For "less than or equal", use `<=` operator

---

## See Also

- [Less Than or Equal Operator](./lessorequal.md)
- [Greater Than Operator](./greaterthan.md)
- [Greater Than or Equal Operator](./greaterorequal.md)
- [#if Helper](../helpers/if.md)
- [Equality Operator](./equality.md)

---
