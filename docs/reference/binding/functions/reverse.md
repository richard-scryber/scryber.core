---
layout: default
title: reverse
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# reverse() : Reverse Collection Order
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

Reverse the order of items in a collection, returning a new array with items in opposite order.

## Signature

```
reverse(collection)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array/Collection | Yes | The collection to reverse |

---

## Returns

**Type:** Array

A new array with items in reverse order (last item becomes first, etc.).

---

## Examples

### Simple Reverse

```handlebars
<p>Original: {{join(model.items, ', ')}}</p>
<p>Reversed: {{join(reverse(model.items), ', ')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    items = new[] { "First", "Second", "Third", "Fourth" }
};
```

**Output:**
```html
<p>Original: First, Second, Third, Fourth</p>
<p>Reversed: Fourth, Third, Second, First</p>
```

### Descending Sort

```handlebars
<h3>Products (Highest to Lowest Price)</h3>
{{#each reverse(sortBy(model.products, 'price'))}}
  <p>{{this.name}}: ${{this.price}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    products = new[] {
        new { name = "Widget A", price = 29.99 },
        new { name = "Widget B", price = 9.99 },
        new { name = "Widget C", price = 19.99 }
    }
};
```

**Output:**
```html
<h3>Products (Highest to Lowest Price)</h3>
<p>Widget A: $29.99</p>
<p>Widget C: $19.99</p>
<p>Widget B: $9.99</p>
```

### Latest Items First

```handlebars
<h3>Recent Orders</h3>
{{#each reverse(sortBy(model.orders, 'date'))}}
  <p>{{format(this.date, 'MMM dd')}}: Order #{{this.id}} - ${{this.total}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    orders = new[] {
        new { id = "001", date = new DateTime(2024, 3, 10), total = 150.00 },
        new { id = "002", date = new DateTime(2024, 3, 15), total = 200.00 },
        new { id = "003", date = new DateTime(2024, 3, 12), total = 175.50 }
    }
};
```

**Output:**
```html
<h3>Recent Orders</h3>
<p>Mar 15: Order #002 - $200</p>
<p>Mar 12: Order #003 - $175.5</p>
<p>Mar 10: Order #001 - $150</p>
```

### Countdown List

```handlebars
<h3>Countdown</h3>
{{#each reverse(model.numbers)}}
  <p>{{this}}...</p>
{{/each}}
<p>Launch!</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    numbers = new[] { 1, 2, 3, 4, 5 }
};
```

**Output:**
```html
<h3>Countdown</h3>
<p>5...</p>
<p>4...</p>
<p>3...</p>
<p>2...</p>
<p>1...</p>
<p>Launch!</p>
```

### Leaderboard (High Scores First)

```handlebars
<h3>Top Players</h3>
{{#each reverse(sortBy(model.players, 'score'))}}
  <p>{{@index + 1}}. {{this.name}}: {{this.score}} points
  {{#if (@index == 0)}}🏆{{/if}}
  </p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    players = new[] {
        new { name = "Alice", score = 850 },
        new { name = "Bob", score = 1200 },
        new { name = "Charlie", score = 950 }
    }
};
```

**Output:**
```html
<h3>Top Players</h3>
<p>1. Bob: 1200 points 🏆</p>
<p>2. Charlie: 950 points</p>
<p>3. Alice: 850 points</p>
```

---

## Notes

- Returns new array (does not modify original collection)
- Works with any array or collection
- Commonly used with `sortBy()` to create descending sorts
- Empty or null collections return empty array
- Useful for:
  - Descending sorts (newest/highest first)
  - Countdown displays
  - Leaderboards
  - Timeline reversals
  - Navigation breadcrumbs (reverse order)
- Performance: O(n) - efficient operation
- Pattern: `reverse(sortBy(items, 'property'))` for descending sort by property
- Does not affect special iteration variables like `@index` (they still increment)

---

## See Also

- [sortBy Function](./sortBy.md)
- [selectWhere Function](./selectWhere.md)
- [#each Helper](../helpers/each.md)
- [join Function](./join.md)

---
