---
layout: default
title: max
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# max() : Find Maximum Value
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

Find the maximum (largest) value in a collection of numeric values.

## Signature

```
max(collection)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array of Numbers | Yes | The collection of numeric values |

---

## Returns

**Type:** Number

The largest value in the collection. Returns null for empty collections.

---

## Examples

### Simple Maximum

```handlebars
<p>Highest score: {{max(model.scores)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    scores = new[] { 85, 92, 78, 95, 88 }
};
```

**Output:**
```html
<p>Highest score: 95</p>
```

### Price Range

```handlebars
<p>Price range: ${{min(collect(model.products, 'price'))}} - ${{max(collect(model.products, 'price'))}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    products = new[] {
        new { name = "Widget A", price = 15.50 },
        new { name = "Widget B", price = 22.00 },
        new { name = "Widget C", price = 8.75 },
        new { name = "Widget D", price = 31.25 }
    }
};
```

**Output:**
```html
<p>Price range: $8.75 - $31.25</p>
```

### Daily Sales Peak

```handlebars
<h3>Sales Analysis</h3>
<p>Peak daily sales: ${{max(collect(model.dailySales, 'amount'))}}</p>
<p>Average daily sales: ${{round(average(collect(model.dailySales, 'amount')), 2)}}</p>
<p>Total weekly sales: ${{sum(collect(model.dailySales, 'amount'))}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    dailySales = new[] {
        new { day = "Mon", amount = 1250.00 },
        new { day = "Tue", amount = 1580.50 },
        new { day = "Wed", amount = 1420.00 },
        new { day = "Thu", amount = 1890.25 },
        new { day = "Fri", amount = 2150.00 }
    }
};
```

**Output:**
```html
<h3>Sales Analysis</h3>
<p>Peak daily sales: $2150</p>
<p>Average daily sales: $1658.15</p>
<p>Total weekly sales: $8290.75</p>
```

### Class Performance

```handlebars
{{#each model.classes}}
  <p>{{this.name}}: Top score {{max(this.scores)}}, Class average {{round(average(this.scores), 1)}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    classes = new[] {
        new { name = "Math 101", scores = new[] { 85, 90, 92, 88, 95 } },
        new { name = "English 101", scores = new[] { 78, 82, 88, 75, 91 } },
        new { name = "Science 101", scores = new[] { 95, 93, 97, 92, 89 } }
    }
};
```

**Output:**
```html
<p>Math 101: Top score 95, Class average 90.0</p>
<p>English 101: Top score 91, Class average 82.8</p>
<p>Science 101: Top score 97, Class average 93.2</p>
```

---

## Notes

- Returns null for empty or null collections
- All items must be numeric
- Supports both integers and decimals
- For finding maximum of a property across objects, use with `collect()`: `max(collect(items, 'price'))`
- For finding maximum property value, use `maxOf()` as shorthand
- Useful for:
  - Price ranges
  - Performance metrics
  - Peak values
  - High scores
  - Capacity limits

---

## See Also

- [min Function](./min.md)
- [maxOf Function](./maxOf.md)
- [average Function](./average.md)
- [collect Function](./collect.md)

---
