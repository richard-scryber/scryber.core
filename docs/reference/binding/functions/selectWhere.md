---
layout: default
title: selectWhere
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# selectWhere() : Filter Collection by Condition
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

Filter a collection to return only items where a specific property matches a value.

## Signature

```
selectWhere(collection, propertyName, value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array/Collection | Yes | The collection to filter |
| `propertyName` | String | Yes | Name of the property to check |
| `value` | Any | Yes | Value to match against |

---

## Returns

**Type:** Array

A new array containing only items where the specified property equals the given value.

---

## Examples

### Filter by Status

```handlebars
<h3>Active Tasks</h3>
{{#each selectWhere(model.tasks, 'status', 'active')}}
  <p>{{this.name}} - Priority: {{this.priority}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    tasks = new[] {
        new { name = "Task 1", status = "active", priority = "high" },
        new { name = "Task 2", status = "completed", priority = "medium" },
        new { name = "Task 3", status = "active", priority = "low" },
        new { name = "Task 4", status = "pending", priority = "high" }
    }
};
```

**Output:**
```html
<h3>Active Tasks</h3>
<p>Task 1 - Priority: high</p>
<p>Task 3 - Priority: low</p>
```

### Filter and Calculate

```handlebars
<h3>Category Sales</h3>
<p>Electronics: ${{sumOf(selectWhere(model.products, 'category', 'Electronics'), 'sales')}}</p>
<p>Books: ${{sumOf(selectWhere(model.products, 'category', 'Books'), 'sales')}}</p>
<p>Clothing: ${{sumOf(selectWhere(model.products, 'category', 'Clothing'), 'sales')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    products = new[] {
        new { name = "Laptop", category = "Electronics", sales = 2500.00 },
        new { name = "Phone", category = "Electronics", sales = 1800.00 },
        new { name = "Novel", category = "Books", sales = 350.00 },
        new { name = "Shirt", category = "Clothing", sales = 450.00 },
        new { name = "Tablet", category = "Electronics", sales = 1200.00 }
    }
};
```

**Output:**
```html
<h3>Category Sales</h3>
<p>Electronics: $5500</p>
<p>Books: $350</p>
<p>Clothing: $450</p>
```

### Filter Boolean Properties

```handlebars
<h3>Verified Users ({{count(selectWhere(model.users, 'verified', true))}})</h3>
{{#each selectWhere(model.users, 'verified', true)}}
  <p>{{this.name}} - {{this.email}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    users = new[] {
        new { name = "Alice", email = "alice@example.com", verified = true },
        new { name = "Bob", email = "bob@example.com", verified = false },
        new { name = "Charlie", email = "charlie@example.com", verified = true }
    }
};
```

**Output:**
```html
<h3>Verified Users (2)</h3>
<p>Alice - alice@example.com</p>
<p>Charlie - charlie@example.com</p>
```

### Multi-Step Filtering

```handlebars
<h3>High Priority Active Tasks</h3>
{{#with selectWhere(model.tasks, 'status', 'active') as | activeTasks |}}
  {{#each selectWhere(activeTasks, 'priority', 'high')}}
    <p>{{this.name}} - Due: {{format(this.dueDate, 'MMM dd')}}</p>
  {{/each}}
{{/with}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    tasks = new[] {
        new { name = "Fix Bug", status = "active", priority = "high", dueDate = new DateTime(2024, 3, 20) },
        new { name = "Write Docs", status = "active", priority = "low", dueDate = new DateTime(2024, 3, 25) },
        new { name = "Deploy", status = "active", priority = "high", dueDate = new DateTime(2024, 3, 18) },
        new { name = "Review Code", status = "completed", priority = "high", dueDate = new DateTime(2024, 3, 15) }
    }
};
```

**Output:**
```html
<h3>High Priority Active Tasks</h3>
<p>Fix Bug - Due: Mar 20</p>
<p>Deploy - Due: Mar 18</p>
```

---

## Notes

- Returns empty array if no items match
- Uses equality comparison (==) for matching
- Case-sensitive for string comparisons
- Does not modify original collection
- Property must exist on items (null properties match null value)
- Useful for:
  - Status-based filtering
  - Category segregation
  - Boolean flag filtering
  - Multi-step filtering pipelines
  - Conditional aggregations
- For counting filtered items, use `countOf()` instead
- For finding single item, use `firstWhere()`
- Can be nested for complex filters
- Performance: O(n) - iterates through entire collection

---

## See Also

- [firstWhere Function](./firstWhere.md)
- [countOf Function](./countOf.md)
- [sortBy Function](./sortBy.md)
- [#each Helper](../helpers/each.md)

---
