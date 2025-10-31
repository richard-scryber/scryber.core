---
layout: default
title: count
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# count() : Count Items in Collection
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

Count the number of items in a collection or array.

## Signature

```
count(collection)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array/Collection | Yes | The collection to count |

---

## Returns

**Type:** Number (Integer)

The number of items in the collection. Returns 0 for null or empty collections.

---

## Examples

### Simple Count

```handlebars
<p>Total items: {{count(model.items)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    items = new[] { "Apple", "Banana", "Cherry", "Date" }
};
```

**Output:**
```html
<p>Total items: 4</p>
```

### Summary Statistics

```handlebars
<h3>Order Summary</h3>
<p>Number of items: {{count(model.orderItems)}}</p>
<p>Total cost: ${{sum(collect(model.orderItems, 'price'))}}</p>
<p>Average price: ${{round(sum(collect(model.orderItems, 'price')) / count(model.orderItems), 2)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    orderItems = new[] {
        new { name = "Widget A", price = 10.50 },
        new { name = "Widget B", price = 15.00 },
        new { name = "Widget C", price = 8.75 }
    }
};
```

**Output:**
```html
<h3>Order Summary</h3>
<p>Number of items: 3</p>
<p>Total cost: $34.25</p>
<p>Average price: $11.42</p>
```

### Conditional Display Based on Count

```handlebars
{{#if (count(model.notifications) > 0)}}
  <p>You have {{count(model.notifications)}} new notifications</p>
{{else}}
  <p>No new notifications</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    notifications = new[] { "Message 1", "Message 2", "Message 3" }
};
```

**Output:**
```html
<p>You have 3 new notifications</p>
```

### Progress Indicator

```handlebars
<p>Progress: {{count(selectWhere(model.tasks, 'completed', true))}} of {{count(model.tasks)}} tasks completed</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    tasks = new[] {
        new { name = "Task 1", completed = true },
        new { name = "Task 2", completed = false },
        new { name = "Task 3", completed = true },
        new { name = "Task 4", completed = true }
    }
};
```

**Output:**
```html
<p>Progress: 3 of 4 tasks completed</p>
```

---

## Notes

- Returns 0 for null or empty collections
- Does not count nested collections (only top-level items)
- Efficient O(1) operation for most collection types
- Useful for:
  - Displaying item counts
  - Calculating averages
  - Progress tracking
  - Conditional logic based on size
  - Pagination calculations
- For conditional counting (e.g., count items where condition is true), use `countOf()` instead
- For string length, use `length()` function

---

## See Also

- [countOf Function](./countOf.md)
- [sum Function](./sum.md)
- [length Function](./length.md)
- [#each Helper](../helpers/each.md)

---
