---
layout: default
title: sortBy
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# sortBy() : Sort Collection by Property
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

Sort a collection by a specific property in ascending order.

## Signature

```
sortBy(collection, propertyName)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array/Collection | Yes | The collection to sort |
| `propertyName` | String | Yes | Name of the property to sort by |

---

## Returns

**Type:** Array

A new sorted array with items ordered by the specified property in ascending order.

---

## Examples

### Sort by Name

```handlebars
<h3>Students (Alphabetical)</h3>
{{#each sortBy(model.students, 'name')}}
  <p>{{this.name}} - Grade: {{this.grade}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    students = new[] {
        new { name = "Charlie", grade = "A" },
        new { name = "Alice", grade = "B" },
        new { name = "Bob", grade = "A-" }
    }
};
```

**Output:**
```html
<h3>Students (Alphabetical)</h3>
<p>Alice - Grade: B</p>
<p>Bob - Grade: A-</p>
<p>Charlie - Grade: A</p>
```

### Sort by Price

```handlebars
<h3>Products (Lowest to Highest Price)</h3>
{{#each sortBy(model.products, 'price')}}
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
<h3>Products (Lowest to Highest Price)</h3>
<p>Widget B: $9.99</p>
<p>Widget C: $19.99</p>
<p>Widget A: $29.99</p>
```

### Sort by Date

```handlebars
<h3>Upcoming Events</h3>
{{#each sortBy(model.events, 'date')}}
  <p>{{format(this.date, 'MMM dd')}}: {{this.title}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    events = new[] {
        new { title = "Conference", date = new DateTime(2024, 4, 20) },
        new { title = "Workshop", date = new DateTime(2024, 3, 15) },
        new { title = "Webinar", date = new DateTime(2024, 3, 30) }
    }
};
```

**Output:**
```html
<h3>Upcoming Events</h3>
<p>Mar 15: Workshop</p>
<p>Mar 30: Webinar</p>
<p>Apr 20: Conference</p>
```

### Sort and Reverse for Descending Order

```handlebars
<h3>Top Scores</h3>
{{#each reverse(sortBy(model.players, 'score'))}}
  <p>{{@index + 1}}. {{this.name}}: {{this.score}} points</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    players = new[] {
        new { name = "Alice", score = 850 },
        new { name = "Bob", score = 1200 },
        new { name = "Charlie", score = 950 },
        new { name = "Diana", score = 1100 }
    }
};
```

**Output:**
```html
<h3>Top Scores</h3>
<p>1. Bob: 1200 points</p>
<p>2. Diana: 1100 points</p>
<p>3. Charlie: 950 points</p>
<p>4. Alice: 850 points</p>
```

### Sort Filtered Results

```handlebars
<h3>Active Tasks (By Priority)</h3>
{{#each sortBy(selectWhere(model.tasks, 'status', 'active'), 'priority')}}
  <p>{{this.name}} - {{this.priority}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    tasks = new[] {
        new { name = "Fix Bug", status = "active", priority = 3 },
        new { name = "Write Docs", status = "completed", priority = 2 },
        new { name = "Deploy", status = "active", priority = 1 },
        new { name = "Review", status = "active", priority = 2 }
    }
};
```

**Output:**
```html
<h3>Active Tasks (By Priority)</h3>
<p>Deploy - 1</p>
<p>Review - 2</p>
<p>Fix Bug - 3</p>
```

---

## Notes

- Returns new sorted array (does not modify original)
- Always sorts in **ascending order** (smallest to largest, A to Z)
- For descending order, combine with `reverse()`: `reverse(sortBy(items, 'prop'))`
- Property must exist on all items
- Supports sorting by:
  - Strings (alphabetical, case-sensitive)
  - Numbers (numerical order)
  - Dates (chronological order)
  - Booleans (false before true)
- Case-sensitive property names
- Null values sort to beginning
- Useful for:
  - Alphabetical lists
  - Price sorting
  - Date ordering
  - Priority ranking
  - Leaderboards (with reverse)
- For complex sorting (multiple properties), sort by most significant property last
- Performance: O(n log n) - efficient for most use cases

---

## See Also

- [reverse Function](./reverse.md)
- [selectWhere Function](./selectWhere.md)
- [collect Function](./collect.md)
- [#each Helper](../helpers/each.md)

---
