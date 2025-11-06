---
layout: default
title: mode
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# mode() : Find Most Common Value
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

Find the mode (most frequently occurring value) in a collection. Returns the value that appears most often.

## Signature

```
mode(collection)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `collection` | Array | Yes | The collection of values |

---

## Returns

**Type:** Same as collection item type

The most frequently occurring value. If multiple values tie for most frequent, returns the first one encountered. Returns null for empty collections.

---

## Examples

### Basic Mode

```handlebars
<p>Most common score: {{mode(model.scores)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    scores = new[] { 85, 90, 85, 92, 88, 85, 95 }  // 85 appears 3 times
};
```

**Output:**
```html
<p>Most common score: 85</p>
```

### Statistical Summary

```handlebars
<h3>Test Results</h3>
<p>Mean: {{round(mean(model.scores), 1)}}</p>
<p>Median: {{median(model.scores)}}</p>
<p>Mode: {{mode(model.scores)}}</p>
<p>The most common score was {{mode(model.scores)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    scores = new[] { 78, 85, 85, 90, 85, 92, 88 }
};
```

**Output:**
```html
<h3>Test Results</h3>
<p>Mean: 86.1</p>
<p>Median: 85</p>
<p>Mode: 85</p>
<p>The most common score was 85</p>
```

### Most Popular Product

```handlebars
<p>Most ordered product: {{mode(collect(model.orders, 'productId'))}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    orders = new[] {
        new { orderId = 1, productId = "A001" },
        new { orderId = 2, productId = "A002" },
        new { orderId = 3, productId = "A001" },
        new { orderId = 4, productId = "A003" },
        new { orderId = 5, productId = "A001" }
    }
};
```

**Output:**
```html
<p>Most ordered product: A001</p>
```

### Customer Preferences

```handlebars
<h3>Survey Results</h3>
<p>Responses collected: {{count(model.responses)}}</p>
<p>Most popular choice: {{mode(collect(model.responses, 'choice'))}}</p>
{{#with mode(collect(model.responses, 'choice'))}}
  <p>{{countOf(model.responses, 'choice', this)}} people chose this option</p>
{{/with}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    responses = new[] {
        new { respondent = "User1", choice = "Option A" },
        new { respondent = "User2", choice = "Option B" },
        new { respondent = "User3", choice = "Option A" },
        new { respondent = "User4", choice = "Option C" },
        new { respondent = "User5", choice = "Option A" },
        new { respondent = "User6", choice = "Option B" }
    }
};
```

**Output:**
```html
<h3>Survey Results</h3>
<p>Responses collected: 6</p>
<p>Most popular choice: Option A</p>
<p>3 people chose this option</p>
```

### String Mode

```handlebars
<p>Most common status: {{mode(collect(model.tasks, 'status'))}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    tasks = new[] {
        new { id = 1, status = "active" },
        new { id = 2, status = "completed" },
        new { id = 3, status = "active" },
        new { id = 4, status = "pending" },
        new { id = 5, status = "active" }
    }
};
```

**Output:**
```html
<p>Most common status: active</p>
```

---

## Notes

- Returns null for empty or null collections
- Works with any comparable type (numbers, strings, dates, etc.)
- If multiple values tie for most frequent, returns first encountered
- Case-sensitive for string comparisons
- Does not return frequency count (use `countOf()` for that)
- Useful for:
  - Finding most popular items
  - Survey analysis
  - Identifying common patterns
  - Customer preference analysis
  - Quality control (most common defect)
- For property-based mode, use with `collect()`: `mode(collect(items, 'property'))`
- Different from mean and median:
  - Mean: arithmetic average
  - Median: middle value
  - Mode: most common value
- Can be used with non-numeric data (unlike mean/median)

---

## See Also

- [mean Function](./mean.md)
- [median Function](./median.md)
- [countOf Function](./countOf.md)
- [collect Function](./collect.md)

---
