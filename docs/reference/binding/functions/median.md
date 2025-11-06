---
layout: default
title: median
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# median() : Calculate Median Value
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

Calculate the median (middle value) of numeric values in a collection. The median is the value that separates the higher half from the lower half of the data.

## Signature

```
median(collection)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array of Numbers | Yes | The collection of numeric values |

---

## Returns

**Type:** Number

The median value. For even-length collections, returns the average of the two middle values. Returns null for empty collections.

---

## Examples

### Basic Median

```handlebars
<p>Median score: {{median(model.scores)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    scores = new[] { 78, 82, 85, 88, 92 }  // Odd count: middle is 85
};
```

**Output:**
```html
<p>Median score: 85</p>
```

### Even Count Median

```handlebars
<p>Median value: {{median(model.values)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    values = new[] { 10, 20, 30, 40 }  // Even count: (20 + 30) / 2 = 25
};
```

**Output:**
```html
<p>Median value: 25</p>
```

### Robust Against Outliers

```handlebars
<h3>Price Analysis</h3>
<p>Mean price: ${{round(mean(model.prices), 2)}}</p>
<p>Median price: ${{round(median(model.prices), 2)}}</p>
<p style="color: gray;">Median is less affected by extreme values</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    prices = new[] { 10.00, 12.50, 15.00, 18.00, 95.00 }  // 95.00 is outlier
};
```

**Output:**
```html
<h3>Price Analysis</h3>
<p>Mean price: $30.10</p>
<p>Median price: $15.00</p>
<p style="color: gray;">Median is less affected by extreme values</p>
```

### Income Distribution

```handlebars
<h3>Household Income Statistics</h3>
<p>Median income: ${{round(median(collect(model.households, 'income')), 0)}}</p>
<p>Mean income: ${{round(mean(collect(model.households, 'income')), 0)}}</p>
{{#if (median(collect(model.households, 'income')) < mean(collect(model.households, 'income')))}}
  <p style="color: blue;">Income distribution is right-skewed (high earners pull mean up)</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    households = new[] {
        new { id = 1, income = 45000 },
        new { id = 2, income = 52000 },
        new { id = 3, income = 48000 },
        new { id = 4, income = 150000 },  // High earner
        new { id = 5, income = 50000 }
    }
};
```

**Output:**
```html
<h3>Household Income Statistics</h3>
<p>Median income: $50000</p>
<p>Mean income: $69000</p>
<p style="color: blue;">Income distribution is right-skewed (high earners pull mean up)</p>
```

### Statistical Comparison

```handlebars
<h3>Test Score Analysis</h3>
<p>Number of students: {{count(model.scores)}}</p>
<p>Mean: {{round(mean(model.scores), 1)}}</p>
<p>Median: {{round(median(model.scores), 1)}}</p>
<p>Mode: {{mode(model.scores)}}</p>
<p>Range: {{min(model.scores)}} - {{max(model.scores)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    scores = new[] { 85, 90, 85, 92, 88, 85, 95, 78 }
};
```

**Output:**
```html
<h3>Test Score Analysis</h3>
<p>Number of students: 8</p>
<p>Mean: 87.2</p>
<p>Median: 86.5</p>
<p>Mode: 85</p>
<p>Range: 78 - 95</p>
```

---

## Notes

- Returns null for empty or null collections
- Automatically sorts values internally (no need to pre-sort)
- For odd-length collections: returns the exact middle value
- For even-length collections: returns average of two middle values
- Not affected by extreme outliers (robust statistic)
- All items must be numeric
- Useful for:
  - Income/salary statistics
  - Real estate pricing (median home price)
  - Test scores with outliers
  - Any dataset with extreme values
  - Understanding typical values
- Preferred over `mean()` when:
  - Data has outliers
  - Distribution is skewed
  - You want the "typical" value
- For property-based median, use with `collect()`: `median(collect(items, 'property'))`

---

## See Also

- [mean Function](./mean.md)
- [average Function](./average.md)
- [mode Function](./mode.md)
- [min Function](./min.md)
- [max Function](./max.md)

---
