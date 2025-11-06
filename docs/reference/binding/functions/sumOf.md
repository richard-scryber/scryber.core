---
layout: default
title: sumOf
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# sumOf() : Sum Property Across Collection
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

Calculate the sum of a specific numeric property across all items in a collection.

## Signature

```
sumOf(collection, propertyName)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array/Collection | Yes | The collection of objects |
| `propertyName` | String | Yes | Name of the numeric property to sum |

---

## Returns

**Type:** Number

The sum of the specified property across all items. Returns 0 for empty collections.

---

## Examples

### Simple Property Sum

```handlebars
<p>Total price: ${{sumOf(model.products, 'price')}}</p>
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
<p>Total price: $46.25</p>
```

### Sales Report

```handlebars
<h3>Monthly Sales Summary</h3>
<p>Total Revenue: ${{sumOf(model.sales, 'amount')}}</p>
<p>Total Units Sold: {{sumOf(model.sales, 'quantity')}}</p>
<p>Average Sale: ${{round(sumOf(model.sales, 'amount') / count(model.sales), 2)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    sales = new[] {
        new { date = "2024-03-01", amount = 150.00, quantity = 5 },
        new { date = "2024-03-02", amount = 200.00, quantity = 8 },
        new { date = "2024-03-03", amount = 175.50, quantity = 6 }
    }
};
```

**Output:**
```html
<h3>Monthly Sales Summary</h3>
<p>Total Revenue: $525.5</p>
<p>Total Units Sold: 19</p>
<p>Average Sale: $175.17</p>
```

### Inventory Value

```handlebars
<h3>Warehouse Inventory</h3>
{{#each model.warehouses}}
  <p>{{this.name}}: ${{sumOf(this.items, 'value')}} ({{sumOf(this.items, 'quantity')}} units)</p>
{{/each}}
<p><strong>Total Value: ${{sumOf(model.warehouses, 'totalValue')}}</strong></p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    warehouses = new[] {
        new {
            name = "Warehouse A",
            totalValue = 15000,
            items = new[] {
                new { sku = "A001", value = 5000, quantity = 100 },
                new { sku = "A002", value = 10000, quantity = 200 }
            }
        },
        new {
            name = "Warehouse B",
            totalValue = 22000,
            items = new[] {
                new { sku = "B001", value = 12000, quantity = 150 },
                new { sku = "B002", value = 10000, quantity = 180 }
            }
        }
    }
};
```

**Output:**
```html
<h3>Warehouse Inventory</h3>
<p>Warehouse A: $15000 (300 units)</p>
<p>Warehouse B: $22000 (330 units)</p>
<p><strong>Total Value: $37000</strong></p>
```

### Project Hours

```handlebars
<h3>Team Time Tracking</h3>
{{#each model.projects}}
  <p>{{this.name}}: {{sumOf(this.tasks, 'hours')}} hours</p>
{{/each}}
<p>Grand Total: {{sumOf(model.projects, 'totalHours')}} hours</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    projects = new[] {
        new {
            name = "Project Alpha",
            totalHours = 45,
            tasks = new[] {
                new { name = "Design", hours = 15 },
                new { name = "Development", hours = 30 }
            }
        },
        new {
            name = "Project Beta",
            totalHours = 60,
            tasks = new[] {
                new { name = "Research", hours = 20 },
                new { name = "Implementation", hours = 40 }
            }
        }
    }
};
```

**Output:**
```html
<h3>Team Time Tracking</h3>
<p>Project Alpha: 45 hours</p>
<p>Project Beta: 60 hours</p>
<p>Grand Total: 105 hours</p>
```

---

## Notes

- Returns 0 for empty or null collections
- Property must exist on all items and be numeric
- More concise than `sum(collect(collection, 'property'))`
- Non-numeric property values will cause an error
- Case-sensitive property names
- Useful for:
  - Financial totals
  - Quantity aggregation
  - Time tracking summaries
  - Inventory calculations
  - Performance metrics
- For summing simple numeric arrays, use `sum()` instead
- For conditional sums, filter with `selectWhere()` first

---

## See Also

- [sum Function](./sum.md)
- [averageOf Function](./averageOf.md)
- [collect Function](./collect.md)
- [count Function](./count.md)

---
