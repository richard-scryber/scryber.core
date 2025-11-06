---
layout: default
title: average
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# average() : Calculate Average of Values
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

Calculate the arithmetic mean (average) of numeric values in a collection.

## Signature

```
average(collection)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array of Numbers | Yes | The collection of numeric values |

---

## Returns

**Type:** Number (Decimal)

The arithmetic mean of all values. Returns null for empty collections.

---

## Examples

### Simple Average

```handlebars
<p>Average score: {{round(average(model.scores), 1)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    scores = new[] { 85, 92, 78, 95, 88 }
};
```

**Output:**
```html
<p>Average score: 87.6</p>
```

### Grade Report

```handlebars
<h3>Student Performance</h3>
{{#each model.students}}
  <p>{{this.name}}:
    Total {{sum(this.scores)}},
    Average {{round(average(this.scores), 1)}}
  </p>
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
<h3>Student Performance</h3>
<p>Alice: Total 267, Average 89.0</p>
<p>Bob: Total 248, Average 82.7</p>
<p>Charlie: Total 285, Average 95.0</p>
```

### Performance Metrics

```handlebars
<h3>API Performance Summary</h3>
<p>Average response time: {{round(average(collect(model.requests, 'responseTime')), 0)}}ms</p>
<p>Fastest: {{min(collect(model.requests, 'responseTime'))}}ms</p>
<p>Slowest: {{max(collect(model.requests, 'responseTime'))}}ms</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    requests = new[] {
        new { endpoint = "/api/users", responseTime = 250 },
        new { endpoint = "/api/orders", responseTime = 180 },
        new { endpoint = "/api/products", responseTime = 320 },
        new { endpoint = "/api/inventory", responseTime = 290 }
    }
};
```

**Output:**
```html
<h3>API Performance Summary</h3>
<p>Average response time: 260ms</p>
<p>Fastest: 180ms</p>
<p>Slowest: 320ms</p>
```

### Price Analysis

```handlebars
<h3>Product Pricing</h3>
<p>Average price: ${{round(average(collect(model.products, 'price')), 2)}}</p>
<p>Total inventory value: ${{sum(collect(model.products, 'price'))}}</p>
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
<h3>Product Pricing</h3>
<p>Average price: $19.38</p>
<p>Total inventory value: $77.5</p>
<p>Price range: $8.75 - $31.25</p>
```

---

## Notes

- Returns null for empty or null collections
- All items must be numeric
- Result is a decimal (use `round()` for formatting)
- Formula: sum of all values divided by count
- For averaging a property across objects, use with `collect()`: `average(collect(items, 'price'))`
- For averaging property values, use `averageOf()` as shorthand
- Useful for:
  - Grade calculations
  - Performance metrics
  - Price analysis
  - Quality scores
  - Statistical summaries
- Same as `mean()` function (mathematical synonym)
- Consider using `median()` for data with outliers

---

## See Also

- [averageOf Function](./averageOf.md)
- [mean Function](./mean.md)
- [sum Function](./sum.md)
- [median Function](./median.md)

---
