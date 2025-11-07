---
layout: default
title: yearOf
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# yearOf() : Extract Year from Date
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

Extract the year component from a datetime value as a 4-digit number.

## Signature

```
yearOf(date)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `date` | DateTime | Yes | The datetime to extract the year from |

---

## Returns

**Type:** Number (Integer)

The year as a 4-digit number (e.g., 2024).

---

## Examples

### Display Year

```handlebars
<p>Copyright {{yearOf(model.currentDate)}} Acme Corporation</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    currentDate = new DateTime(2024, 3, 15)
};
```

**Output:**
```html
<p>Copyright 2024 Acme Corporation</p>
```

### Calculate Age

```handlebars
<p>{{model.name}} is {{yearOf(model.today) - yearOf(model.birthDate)}} years old</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    name = "Alice",
    birthDate = new DateTime(1990, 5, 20),
    today = new DateTime(2024, 3, 15)
};
```

**Output:**
```html
<p>Alice is 34 years old</p>
```

### Group by Year

```handlebars
<h3>Orders by Year</h3>
{{#each model.orders}}
  <p>{{yearOf(this.orderDate)}}: {{this.total}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    orders = new[] {
        new { orderDate = new DateTime(2023, 6, 15), total = 150.00 },
        new { orderDate = new DateTime(2023, 8, 20), total = 200.00 },
        new { orderDate = new DateTime(2024, 1, 10), total = 175.00 }
    }
};
```

**Output:**
```html
<h3>Orders by Year</h3>
<p>2023: 150</p>
<p>2023: 200</p>
<p>2024: 175</p>
```

### Fiscal Year Calculation

```handlebars
<p>Fiscal Year: {{#if (monthOfYear(model.date) >= 7)}}{{yearOf(model.date) + 1}}{{else}}{{yearOf(model.date)}}{{/if}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    date = new DateTime(2024, 8, 15)
};
```

**Output:**
```html
<p>Fiscal Year: 2025</p>
```

---

## Notes

- Returns 4-digit year value
- Time component is ignored
- Useful for copyright notices, age calculations, and date grouping
- Combine with `monthOfYear()` and `dayOfMonth()` to build custom date formats
- For complete date formatting, use `format()` function

---

## See Also

- [monthOfYear Function](./monthOfYear.md)
- [dayOfMonth Function](./dayOfMonth.md)
- [addYears Function](./addYears.md)
- [format Function](./format.md)

---
