---
layout: default
title: averageOf
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# averageOf() : Calculate Average of Property
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

Calculate the arithmetic mean (average) of a specific numeric property across all items in a collection.

## Signature

```
averageOf(collection, propertyName)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array/Collection | Yes | The collection of objects |
| `propertyName` | String | Yes | Name of the numeric property to average |

---

## Returns

**Type:** Number (Decimal)

The arithmetic mean of the specified property. Returns null for empty collections.

---

## Examples

### Average Price

```handlebars
<p>Average product price: ${{round(averageOf(model.products, 'price'), 2)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    products = new[] {
        new { name = "Widget A", price = 15.50 },
        new { name = "Widget B", price = 22.00 },
        new { name = "Widget C", price = 8.75 }
    }
};
```

**Output:**
```html
<p>Average product price: $15.42</p>
```

### Sales Performance

```handlebars
<h3>Sales Summary</h3>
<p>Average daily sales: ${{round(averageOf(model.dailySales, 'amount'), 2)}}</p>
<p>Total sales: ${{sumOf(model.dailySales, 'amount')}}</p>
<p>Number of days: {{count(model.dailySales)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    dailySales = new[] {
        new { date = "2024-03-01", amount = 1250.00 },
        new { date = "2024-03-02", amount = 1580.50 },
        new { date = "2024-03-03", amount = 1420.00 },
        new { date = "2024-03-04", amount = 1890.25 }
    }
};
```

**Output:**
```html
<h3>Sales Summary</h3>
<p>Average daily sales: $1535.19</p>
<p>Total sales: $6140.75</p>
<p>Number of days: 4</p>
```

### Team Statistics

```handlebars
{{#each model.teams}}
  <h4>{{this.name}}</h4>
  <p>Average experience: {{round(averageOf(this.members, 'yearsExperience'), 1)}} years</p>
  <p>Team size: {{count(this.members)}} members</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    teams = new[] {
        new {
            name = "Development",
            members = new[] {
                new { name = "Alice", yearsExperience = 5 },
                new { name = "Bob", yearsExperience = 3 },
                new { name = "Charlie", yearsExperience = 7 }
            }
        },
        new {
            name = "Design",
            members = new[] {
                new { name = "Diana", yearsExperience = 4 },
                new { name = "Eve", yearsExperience = 6 }
            }
        }
    }
};
```

**Output:**
```html
<h4>Development</h4>
<p>Average experience: 5.0 years</p>
<p>Team size: 3 members</p>
<h4>Design</h4>
<p>Average experience: 5.0 years</p>
<p>Team size: 2 members</p>
```

### Performance Benchmarks

```handlebars
<h3>Server Performance</h3>
<p>Average CPU usage: {{round(averageOf(model.servers, 'cpuUsage'), 1)}}%</p>
<p>Average memory usage: {{round(averageOf(model.servers, 'memoryUsage'), 1)}}%</p>
<p>Peak CPU: {{maxOf(model.servers, 'cpuUsage')}}%</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    servers = new[] {
        new { name = "Server-01", cpuUsage = 45, memoryUsage = 62 },
        new { name = "Server-02", cpuUsage = 52, memoryUsage = 58 },
        new { name = "Server-03", cpuUsage = 38, memoryUsage = 71 }
    }
};
```

**Output:**
```html
<h3>Server Performance</h3>
<p>Average CPU usage: 45.0%</p>
<p>Average memory usage: 63.7%</p>
<p>Peak CPU: 52%</p>
```

---

## Notes

- Returns null for empty or null collections
- Property must exist on all items and be numeric
- More concise than `average(collect(collection, 'property'))`
- Result is a decimal (use `round()` for formatting)
- Case-sensitive property names
- Useful for:
  - Price analysis
  - Performance metrics
  - Statistical reports
  - Quality scores
  - Resource utilization
- For simple numeric arrays, use `average()` instead
- Consider `median()` for datasets with outliers
- Formula: sum of property values divided by count

---

## See Also

- [average Function](./average.md)
- [sumOf Function](./sumOf.md)
- [median Function](./median.md)
- [collect Function](./collect.md)

---
