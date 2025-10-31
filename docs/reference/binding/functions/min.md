---
layout: default
title: min
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# min() : Find Minimum Value
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

Find the minimum (smallest) value in a collection of numeric values.

## Signature

```
min(collection)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array of Numbers | Yes | The collection of numeric values |

---

## Returns

**Type:** Number

The smallest value in the collection. Returns null for empty collections.

---

## Examples

### Simple Minimum

```handlebars
<p>Lowest score: {{min(model.scores)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    scores = new[] { 85, 92, 78, 95, 88 }
};
```

**Output:**
```html
<p>Lowest score: 78</p>
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

### Temperature Analysis

```handlebars
<h3>Weekly Temperature Summary</h3>
<p>High: {{max(model.temperatures)}}°F</p>
<p>Low: {{min(model.temperatures)}}°F</p>
<p>Average: {{round(average(model.temperatures), 1)}}°F</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    temperatures = new[] { 72, 68, 75, 70, 73, 69, 71 }
};
```

**Output:**
```html
<h3>Weekly Temperature Summary</h3>
<p>High: 75°F</p>
<p>Low: 68°F</p>
<p>Average: 71.1°F</p>
```

### Student Performance

```handlebars
{{#each model.students}}
  <p>{{this.name}}: Best {{max(this.scores)}}, Worst {{min(this.scores)}}, Average {{round(average(this.scores), 1)}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    students = new[] {
        new { name = "Alice", scores = new[] { 85, 90, 92, 88 } },
        new { name = "Bob", scores = new[] { 78, 82, 88, 75 } },
        new { name = "Charlie", scores = new[] { 95, 93, 97, 92 } }
    }
};
```

**Output:**
```html
<p>Alice: Best 92, Worst 85, Average 88.8</p>
<p>Bob: Best 88, Worst 75, Average 80.8</p>
<p>Charlie: Best 97, Worst 92, Average 94.2</p>
```

---

## Notes

- Returns null for empty or null collections
- All items must be numeric
- Supports both integers and decimals
- For finding minimum of a property across objects, use with `collect()`: `min(collect(items, 'price'))`
- For finding minimum property value, use `minOf()` as shorthand
- Useful for:
  - Price ranges
  - Performance metrics
  - Quality control
  - Temperature/measurement ranges
  - Threshold calculations

---

## See Also

- [max Function](./max.md)
- [minOf Function](./minOf.md)
- [average Function](./average.md)
- [collect Function](./collect.md)

---
