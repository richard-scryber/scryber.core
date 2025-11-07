---
layout: default
title: mean
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# mean() : Calculate Mean (Average)
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

Calculate the arithmetic mean of numeric values in a collection. This is a mathematical synonym for the `average()` function.

## Signature

```
mean(collection)
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

### Basic Mean Calculation

```handlebars
<p>Mean temperature: {{round(mean(model.temperatures), 1)}}°F</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    temperatures = new[] { 72, 68, 75, 70, 73 }
};
```

**Output:**
```html
<p>Mean temperature: 71.6°F</p>
```

### Statistical Summary

```handlebars
<h3>Data Analysis</h3>
<p>Mean: {{round(mean(model.values), 2)}}</p>
<p>Median: {{round(median(model.values), 2)}}</p>
<p>Min: {{min(model.values)}}</p>
<p>Max: {{max(model.values)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    values = new[] { 10, 15, 20, 25, 30, 35, 40 }
};
```

**Output:**
```html
<h3>Data Analysis</h3>
<p>Mean: 25.00</p>
<p>Median: 25.00</p>
<p>Min: 10</p>
<p>Max: 40</p>
```

### Comparing Mean and Median

```handlebars
<h3>Score Distribution</h3>
<p>Mean score: {{round(mean(model.scores), 1)}}</p>
<p>Median score: {{round(median(model.scores), 1)}}</p>
{{#if (abs(mean(model.scores) - median(model.scores)) > 5)}}
  <p style="color: orange;">Note: Large difference suggests outliers in data</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    scores = new[] { 85, 87, 90, 92, 95, 45 }  // 45 is an outlier
};
```

**Output:**
```html
<h3>Score Distribution</h3>
<p>Mean score: 82.3</p>
<p>Median score: 88.5</p>
<p style="color: orange;">Note: Large difference suggests outliers in data</p>
```

### Property-Based Mean

```handlebars
<p>Mean rating: {{round(mean(collect(model.reviews, 'rating')), 1)}} stars</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    reviews = new[] {
        new { author = "Alice", rating = 5 },
        new { author = "Bob", rating = 4 },
        new { author = "Charlie", rating = 5 },
        new { author = "Diana", rating = 3 }
    }
};
```

**Output:**
```html
<p>Mean rating: 4.2 stars</p>
```

---

## Notes

- Identical functionality to `average()` function
- Mathematical terminology preferred in statistical contexts
- Returns null for empty or null collections
- All items must be numeric
- Result is a decimal (use `round()` for formatting)
- Formula: sum of all values divided by count
- For property-based means, combine with `collect()`: `mean(collect(items, 'property'))`
- Useful for:
  - Statistical reports
  - Scientific calculations
  - Academic grading
  - Data analysis
  - Quality metrics
- Sensitive to outliers (extreme values affect the result)
- Consider using `median()` when outliers are present
- Use `average()` for more intuitive business contexts

---

## See Also

- [average Function](./average.md)
- [median Function](./median.md)
- [sum Function](./sum.md)
- [mode Function](./mode.md)

---
