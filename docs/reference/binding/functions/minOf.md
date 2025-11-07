---
layout: default
title: minOf
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# minOf() : Find Minimum Property Value
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

Find the minimum (smallest) value of a specific numeric property across all items in a collection.

## Signature

```
minOf(collection, propertyName)
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

The smallest value of the specified property. Returns null for empty collections.

---

## Examples

### Lowest Price

```handlebars
<p>Lowest price: ${{minOf(model.products, 'price')}}</p>
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
<p>Lowest price: $8.75</p>
```

### Inventory Low Stock Alert

```handlebars
<h3>Inventory Status</h3>
<p>Lowest stock level: {{minOf(model.inventory, 'quantity')}} units</p>
{{#if (minOf(model.inventory, 'quantity') < 10)}}
  <p style="color: red;">Warning: Low stock detected!</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    inventory = new[] {
        new { sku = "A001", quantity = 45 },
        new { sku = "A002", quantity = 5 },
        new { sku = "A003", quantity = 32 }
    }
};
```

**Output:**
```html
<h3>Inventory Status</h3>
<p>Lowest stock level: 5 units</p>
<p style="color: red;">Warning: Low stock detected!</p>
```

### Performance Metrics

```handlebars
<h3>API Performance</h3>
<p>Fastest response: {{minOf(model.requests, 'responseTime')}}ms</p>
<p>Slowest response: {{maxOf(model.requests, 'responseTime')}}ms</p>
<p>Average response: {{round(averageOf(model.requests, 'responseTime'), 0)}}ms</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    requests = new[] {
        new { endpoint = "/api/users", responseTime = 250 },
        new { endpoint = "/api/orders", responseTime = 180 },
        new { endpoint = "/api/products", responseTime = 320 }
    }
};
```

**Output:**
```html
<h3>API Performance</h3>
<p>Fastest response: 180ms</p>
<p>Slowest response: 320ms</p>
<p>Average response: 250ms</p>
```

### Age Range

```handlebars
<p>Age range: {{minOf(model.participants, 'age')}} - {{maxOf(model.participants, 'age')}} years</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    participants = new[] {
        new { name = "Alice", age = 28 },
        new { name = "Bob", age = 35 },
        new { name = "Charlie", age = 22 },
        new { name = "Diana", age = 41 }
    }
};
```

**Output:**
```html
<p>Age range: 22 - 41 years</p>
```

---

## Notes

- Returns null for empty or null collections
- Property must exist on all items and be numeric
- More concise than `min(collect(collection, 'property'))`
- Case-sensitive property names
- Useful for:
  - Finding lowest prices
  - Identifying minimum stock levels
  - Performance analysis
  - Age/date ranges
  - Quality thresholds
- For simple numeric arrays, use `min()` instead
- For finding the item with minimum value (not just the value), use `sortBy()` and access first item

---

## See Also

- [min Function](./min.md)
- [maxOf Function](./maxOf.md)
- [averageOf Function](./averageOf.md)
- [collect Function](./collect.md)

---
