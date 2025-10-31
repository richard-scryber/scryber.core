---
layout: default
title: maxOf
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# maxOf() : Find Maximum Property Value
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

Find the maximum (largest) value of a specific numeric property across all items in a collection.

## Signature

```
maxOf(collection, propertyName)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array/Collection | Yes | The collection of objects |
| `propertyName` | String | Yes | Name of the numeric property to compare |

---

## Returns

**Type:** Number

The largest value of the specified property. Returns null for empty collections.

---

## Examples

### Highest Price

```handlebars
<p>Highest price: ${{maxOf(model.products, 'price')}}</p>
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
<p>Highest price: $22</p>
```

### Order Size Analysis

```handlebars
<h3>Order Statistics</h3>
<p>Largest order: ${{maxOf(model.orders, 'total')}}</p>
<p>Smallest order: ${{minOf(model.orders, 'total')}}</p>
<p>Average order: ${{round(averageOf(model.orders, 'total'), 2)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    orders = new[] {
        new { id = "ORD001", total = 150.00 },
        new { id = "ORD002", total = 520.50 },
        new { id = "ORD003", total = 85.25 },
        new { id = "ORD004", total = 310.75 }
    }
};
```

**Output:**
```html
<h3>Order Statistics</h3>
<p>Largest order: $520.5</p>
<p>Smallest order: $85.25</p>
<p>Average order: $266.62</p>
```

### Server Load Monitoring

```handlebars
<h3>Server Performance</h3>
{{#each model.servers}}
  <p>{{this.name}}: Peak load {{maxOf(this.samples, 'cpuUsage')}}%, Current {{this.currentCpu}}%</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    servers = new[] {
        new {
            name = "Server-01",
            currentCpu = 45,
            samples = new[] {
                new { timestamp = "10:00", cpuUsage = 35 },
                new { timestamp = "11:00", cpuUsage = 68 },
                new { timestamp = "12:00", cpuUsage = 45 }
            }
        },
        new {
            name = "Server-02",
            currentCpu = 32,
            samples = new[] {
                new { timestamp = "10:00", cpuUsage = 28 },
                new { timestamp = "11:00", cpuUsage = 52 },
                new { timestamp = "12:00", cpuUsage = 32 }
            }
        }
    }
};
```

**Output:**
```html
<h3>Server Performance</h3>
<p>Server-01: Peak load 68%, Current 45%</p>
<p>Server-02: Peak load 52%, Current 32%</p>
```

### Capacity Planning

```handlebars
<p>Maximum capacity: {{maxOf(model.locations, 'capacity')}} people</p>
<p>Current total attendance: {{sumOf(model.locations, 'attendance')}}</p>
{{#if (sumOf(model.locations, 'attendance') > maxOf(model.locations, 'capacity'))}}
  <p style="color: red;">Overcapacity warning!</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    locations = new[] {
        new { name = "Room A", capacity = 50, attendance = 45 },
        new { name = "Room B", capacity = 100, attendance = 85 },
        new { name = "Room C", capacity = 75, attendance = 60 }
    }
};
```

**Output:**
```html
<p>Maximum capacity: 100 people</p>
<p>Current total attendance: 190</p>
<p style="color: red;">Overcapacity warning!</p>
```

---

## Notes

- Returns null for empty or null collections
- Property must exist on all items and be numeric
- More concise than `max(collect(collection, 'property'))`
- Case-sensitive property names
- Useful for:
  - Finding highest prices
  - Identifying peak values
  - Capacity planning
  - Performance monitoring
  - Threshold checking
- For simple numeric arrays, use `max()` instead
- For finding the item with maximum value (not just the value), use `sortBy()` and access last item

---

## See Also

- [max Function](./max.md)
- [minOf Function](./minOf.md)
- [averageOf Function](./averageOf.md)
- [collect Function](./collect.md)

---
