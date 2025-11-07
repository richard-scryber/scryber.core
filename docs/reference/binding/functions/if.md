---
layout: default
title: if
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# if() : Ternary Conditional
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

Evaluate a condition and return one value if true, another if false. Similar to the ternary operator in programming languages.

## Signature

```
if(condition, trueValue, falseValue)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `condition` | Boolean | Yes | Expression that evaluates to true/false |
| `trueValue` | Any | Yes | Value to return if condition is true |
| `falseValue` | Any | Yes | Value to return if condition is false |

---

## Returns

**Type:** Any (type of trueValue or falseValue)

Returns `trueValue` if condition is true, `falseValue` otherwise.

---

## Examples

### Basic Usage

```handlebars
{{if(model.isActive, 'Active', 'Inactive')}}
<!-- Output: Active (if isActive is true) -->
```

### Conditional CSS Class

```handlebars
<div class="{{if(model.isPremium, 'premium', 'standard')}}">
  <h3>{{model.name}}</h3>
</div>
```

**Data:**
```csharp
doc.Params["model"] = new {
    name = "John Doe",
    isPremium = true
};
```

**Output:**
```html
<div class="premium">
  <h3>John Doe</h3>
</div>
```

### With Comparison

```handlebars
{{if(model.score >= 70, 'Pass', 'Fail')}}
```

### Nested Conditions

```handlebars
{{if(model.age >= 18, if(model.hasLicense, 'Can drive', 'No license'), 'Too young')}}
```

### With Special Variables

```handlebars
{{#each model.items}}
  <tr class="{{if(@index % 2 == 0, 'even-row', 'odd-row')}}">
    <td>{{this.name}}</td>
  </tr>
{{/each}}
```

### Avoid Division by Zero

```handlebars
<p>Average: {{if(model.count > 0, model.total / model.count, 0)}}</p>
```

---

## Notes

- Both `trueValue` and `falseValue` are evaluated (no short-circuit)
- Can be nested for complex logic
- Consider using `{{#if}}` helper for cleaner multi-line conditionals
- Useful for inline conditional values

---

## See Also

- [#if Helper](../helpers/if.md)
- [Comparison Operators](../operators/equality.md)
- [Logical Functions](./index.md#logical-functions)

---
