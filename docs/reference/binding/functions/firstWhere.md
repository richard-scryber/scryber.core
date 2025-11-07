---
layout: default
title: firstWhere
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# firstWhere() : Find First Matching Item
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

Find and return the first item in a collection where a specific property matches a value.

## Signature

```
firstWhere(collection, propertyName, value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array/Collection | Yes | The collection to search |
| `propertyName` | String | Yes | Name of the property to check |
| `value` | Any | Yes | Value to match against |

---

## Returns

**Type:** Object or null

The first item where the specified property equals the given value, or null if no match found.

---

## Examples

### Find User by ID

```handlebars
{{#with firstWhere(model.users, 'id', model.currentUserId)}}
  <p>Welcome, {{this.name}}!</p>
  <p>Email: {{this.email}}</p>
{{/with}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    currentUserId = 2,
    users = new[] {
        new { id = 1, name = "Alice", email = "alice@example.com" },
        new { id = 2, name = "Bob", email = "bob@example.com" },
        new { id = 3, name = "Charlie", email = "charlie@example.com" }
    }
};
```

**Output:**
```html
<p>Welcome, Bob!</p>
<p>Email: bob@example.com</p>
```

### Get Product Details

```handlebars
{{#with firstWhere(model.products, 'sku', 'WIDGET-001')}}
  <h3>{{this.name}}</h3>
  <p>Price: ${{this.price}}</p>
  <p>Stock: {{this.quantity}} units</p>
{{/with}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    products = new[] {
        new { sku = "WIDGET-001", name = "Premium Widget", price = 29.99, quantity = 45 },
        new { sku = "WIDGET-002", name = "Standard Widget", price = 19.99, quantity = 120 },
        new { sku = "WIDGET-003", name = "Basic Widget", price = 9.99, quantity = 200 }
    }
};
```

**Output:**
```html
<h3>Premium Widget</h3>
<p>Price: $29.99</p>
<p>Stock: 45 units</p>
```

### Find First Active Item

```handlebars
{{#with firstWhere(model.subscriptions, 'status', 'active')}}
  <p>Active plan: {{this.planName}}</p>
  <p>Renews: {{format(this.renewalDate, 'MMMM dd, yyyy')}}</p>
{{else}}
  <p>No active subscription</p>
{{/with}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    subscriptions = new[] {
        new { planName = "Basic", status = "expired", renewalDate = new DateTime(2024, 1, 15) },
        new { planName = "Pro", status = "active", renewalDate = new DateTime(2024, 4, 20) },
        new { planName = "Enterprise", status = "pending", renewalDate = new DateTime(2024, 3, 30) }
    }
};
```

**Output:**
```html
<p>Active plan: Pro</p>
<p>Renews: April 20, 2024</p>
```

### Lookup and Display Related Data

```handlebars
{{#each model.orderItems}}
  {{#with firstWhere(../model.products, 'id', this.productId)}}
    <p>{{this.name}}: ${{this.price}} x {{../this.quantity}} = ${{this.price * ../this.quantity}}</p>
  {{/with}}
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    products = new[] {
        new { id = 101, name = "Widget A", price = 10.00 },
        new { id = 102, name = "Widget B", price = 15.00 },
        new { id = 103, name = "Widget C", price = 20.00 }
    },
    orderItems = new[] {
        new { productId = 102, quantity = 2 },
        new { productId = 101, quantity = 3 },
        new { productId = 103, quantity = 1 }
    }
};
```

**Output:**
```html
<p>Widget B: $15 x 2 = $30</p>
<p>Widget A: $10 x 3 = $30</p>
<p>Widget C: $20 x 1 = $20</p>
```

---

## Notes

- Returns null if no match is found
- Only returns the **first** matching item (stops searching after first match)
- Uses equality comparison (==) for matching
- Case-sensitive for string comparisons
- More efficient than `selectWhere()` when you only need one item
- Useful for:
  - ID-based lookups
  - Finding first active/enabled item
  - Configuration lookups
  - Related data joins
  - Default item selection
- Use with `{{#with}}` helper for clean null handling
- For finding all matches, use `selectWhere()` instead
- For counting matches, use `countOf()` instead
- Performance: O(n) worst case, but stops at first match

---

## See Also

- [selectWhere Function](./selectWhere.md)
- [countOf Function](./countOf.md)
- [#with Helper](../helpers/with.md)
- [collect Function](./collect.md)

---
