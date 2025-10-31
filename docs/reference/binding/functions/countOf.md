---
layout: default
title: countOf
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# countOf() : Count Items Matching Condition
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

Count the number of items in a collection where a specific property matches a value.

## Signature

```
countOf(collection, propertyName, value)
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

**Type:** Number (Integer)

The number of items where the specified property equals the given value.

---

## Examples

### Count by Status

```handlebars
<h3>Task Status</h3>
<p>Active: {{countOf(model.tasks, 'status', 'active')}}</p>
<p>Completed: {{countOf(model.tasks, 'status', 'completed')}}</p>
<p>Pending: {{countOf(model.tasks, 'status', 'pending')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    tasks = new[] {
        new { name = "Task 1", status = "active" },
        new { name = "Task 2", status = "completed" },
        new { name = "Task 3", status = "active" },
        new { name = "Task 4", status = "pending" },
        new { name = "Task 5", status = "completed" }
    }
};
```

**Output:**
```html
<h3>Task Status</h3>
<p>Active: 2</p>
<p>Completed: 2</p>
<p>Pending: 1</p>
```

### Boolean Property Count

```handlebars
<p>Verified users: {{countOf(model.users, 'verified', true)}}</p>
<p>Unverified users: {{countOf(model.users, 'verified', false)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    users = new[] {
        new { name = "Alice", verified = true },
        new { name = "Bob", verified = false },
        new { name = "Charlie", verified = true },
        new { name = "Diana", verified = true }
    }
};
```

**Output:**
```html
<p>Verified users: 3</p>
<p>Unverified users: 1</p>
```

### Inventory by Category

```handlebars
<h3>Inventory Summary</h3>
{{#each model.categories}}
  <p>{{this}}: {{countOf(model.products, 'category', this)}} items</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    categories = new[] { "Electronics", "Books", "Clothing" },
    products = new[] {
        new { name = "Laptop", category = "Electronics" },
        new { name = "Phone", category = "Electronics" },
        new { name = "Novel", category = "Books" },
        new { name = "Shirt", category = "Clothing" },
        new { name = "Tablet", category = "Electronics" }
    }
};
```

**Output:**
```html
<h3>Inventory Summary</h3>
<p>Electronics: 3 items</p>
<p>Books: 1 items</p>
<p>Clothing: 1 items</p>
```

### Priority Distribution

```handlebars
<h3>Priority Distribution</h3>
<p>High: {{countOf(model.issues, 'priority', 'high')}} ({{round((countOf(model.issues, 'priority', 'high') / count(model.issues)) * 100, 0)}}%)</p>
<p>Medium: {{countOf(model.issues, 'priority', 'medium')}} ({{round((countOf(model.issues, 'priority', 'medium') / count(model.issues)) * 100, 0)}}%)</p>
<p>Low: {{countOf(model.issues, 'priority', 'low')}} ({{round((countOf(model.issues, 'priority', 'low') / count(model.issues)) * 100, 0)}}%)</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    issues = new[] {
        new { id = 1, priority = "high" },
        new { id = 2, priority = "medium" },
        new { id = 3, priority = "high" },
        new { id = 4, priority = "low" },
        new { id = 5, priority = "high" }
    }
};
```

**Output:**
```html
<h3>Priority Distribution</h3>
<p>High: 3 (60%)</p>
<p>Medium: 1 (20%)</p>
<p>Low: 1 (20%)</p>
```

---

## Notes

- Uses equality comparison (==) to match values
- Case-sensitive for string comparisons
- Returns 0 if no items match
- Property must exist on all items (or be null)
- Useful for:
  - Grouping statistics
  - Category counting
  - Boolean flag tallies
  - Distribution analysis
  - Filtering summaries
- For simple collection size, use `count()` instead
- For more complex conditions, combine with `selectWhere()` and `count()`

---

## See Also

- [count Function](./count.md)
- [selectWhere Function](./selectWhere.md)
- [sumOf Function](./sumOf.md)
- [collect Function](./collect.md)

---
