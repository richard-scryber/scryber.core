---
layout: default
title: equality (==)
parent: Binding Operators
parent_url: /reference/binding/operators/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# == : Equality Operator
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

Compares two values for equality. Returns `true` if values are equal, `false` otherwise.

## Syntax

```handlebars
{{operand1 == operand2}}
```

---

## Operands

| Position | Type | Description |
|----------|------|-------------|
| Left | Any | First value to compare |
| Right | Any | Second value to compare |

---

## Returns

**Type:** Boolean

`true` if the operands are equal, `false` otherwise.

---

## Examples

### Basic Comparison

```handlebars
{{5 == 5}}
<!-- Output: true -->

{{5 == 3}}
<!-- Output: false -->
```

### String Comparison

```handlebars
{{#if model.status == 'active'}}
  <span class="badge-active">Active</span>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    status = "active"
};
```

### Number Comparison

```handlebars
{{#if model.quantity == 0}}
  <span class="out-of-stock">Out of Stock</span>
{{else}}
  <span>{{model.quantity}} in stock</span>
{{/if}}
```

### Boolean Comparison

```handlebars
{{#if model.isPremium == true}}
  <span class="premium-badge">Premium Member</span>
{{/if}}
```

### Multiple Conditions

```handlebars
{{#if model.role == 'admin'}}
  <button>Delete</button>
{{else if model.role == 'editor'}}
  <button>Edit</button>
{{else if model.role == 'viewer'}}
  <button>View</button>
{{/if}}
```

### With #each

```handlebars
{{#each model.products}}
  {{#if this.category == 'Electronics'}}
    <div class="electronics-product">
      <h3>{{this.name}}</h3>
    </div>
  {{/if}}
{{/each}}
```

---

## Precedence

Priority level in expression evaluation: **7** (after comparison operators)

---

## Notes

- String comparisons are **case-sensitive**: `"Hello" == "hello"` is `false`
- `null == null` returns `true`
- Comparing `null` to any non-null value returns `false`
- Numbers and strings can be compared (automatic type conversion)
- Boolean values: `true` != `false`

---

## See Also

- [Inequality (!=)](./inequality.md)
- [Greater Than (>)](./greaterthan.md)
- [Less Than (<)](./lessthan.md)
- [#if Helper](../helpers/if.md)

---
