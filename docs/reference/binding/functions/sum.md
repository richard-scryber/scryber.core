---
layout: default
title: sum
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# sum() : Sum Numeric Values
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

Calculate the sum of numeric values in a collection or array.

## Signature

```
sum(collection)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array of Numbers | Yes | The collection of numeric values to sum |

---

## Returns

**Type:** Number

The sum of all numeric values in the collection. Returns 0 for empty or null collections.

---

## Examples

### Simple Sum

```handlebars
<p>Total: {{sum(model.values)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    values = new[] { 10, 20, 30, 40 }
};
```

**Output:**
```html
<p>Total: 100</p>
```

### Order Total

```handlebars
<h3>Invoice</h3>
{{#each model.items}}
  <p>{{this.name}}: ${{this.price}}</p>
{{/each}}
<p><strong>Total: ${{sum(collect(model.items, 'price'))}}</strong></p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    items = new[] {
        new { name = "Widget A", price = 15.50 },
        new { name = "Widget B", price = 22.00 },
        new { name = "Widget C", price = 8.75 }
    }
};
```

**Output:**
```html
<h3>Invoice</h3>
<p>Widget A: $15.5</p>
<p>Widget B: $22</p>
<p>Widget C: $8.75</p>
<p><strong>Total: $46.25</strong></p>
```

### Budget Summary

```handlebars
<h3>Budget Overview</h3>
<p>Total Budget: ${{model.totalBudget}}</p>
<p>Total Spent: ${{sum(collect(model.expenses, 'amount'))}}</p>
<p>Remaining: ${{model.totalBudget - sum(collect(model.expenses, 'amount'))}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    totalBudget = 1000,
    expenses = new[] {
        new { category = "Office", amount = 250 },
        new { category = "Travel", amount = 400 },
        new { category = "Equipment", amount = 180 }
    }
};
```

**Output:**
```html
<h3>Budget Overview</h3>
<p>Total Budget: $1000</p>
<p>Total Spent: $830</p>
<p>Remaining: $170</p>
```

### Score Aggregation

```handlebars
{{#each model.students}}
  <p>{{this.name}}: {{sum(this.scores)}} total points</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    students = new[] {
        new { name = "Alice", scores = new[] { 85, 90, 92 } },
        new { name = "Bob", scores = new[] { 78, 82, 88 } },
        new { name = "Charlie", scores = new[] { 95, 93, 97 } }
    }
};
```

**Output:**
```html
<p>Alice: 267 total points</p>
<p>Bob: 248 total points</p>
<p>Charlie: 285 total points</p>
```

---

## Notes

- Returns 0 for empty or null collections
- All items in collection must be numeric
- Non-numeric values will cause an error (use `ifError()` for safety)
- For summing a property across objects, use with `collect()`: `sum(collect(items, 'price'))`
- For conditional sums, combine with `selectWhere()` first
- Supports integers and decimals
- Useful for:
  - Order totals
  - Budget calculations
  - Score aggregation
  - Quantity totals
  - Financial summaries

---

## See Also

- [sumOf Function](./sumOf.md)
- [average Function](./average.md)
- [count Function](./count.md)
- [collect Function](./collect.md)

---
