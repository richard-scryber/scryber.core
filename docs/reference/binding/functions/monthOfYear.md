---
layout: default
title: monthOfYear
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# monthOfYear() : Extract Month from Date
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

Extract the month component from a datetime value as a number (1-12).

## Signature

```
monthOfYear(date)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `date` | DateTime | Yes | The datetime to extract the month from |

---

## Returns

**Type:** Number (Integer)

The month as a number from 1 (January) to 12 (December).

---

## Examples

### Display Month Number

```handlebars
<p>Month: {{monthOfYear(model.date)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    date = new DateTime(2024, 3, 15)
};
```

**Output:**
```html
<p>Month: 3</p>
```

### Quarter Calculation

```handlebars
<p>Quarter: Q{{ceiling(monthOfYear(model.date) / 3)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    date = new DateTime(2024, 8, 15)
};
```

**Output:**
```html
<p>Quarter: Q3</p>
```

### Seasonal Classification

```handlebars
<p>Season:
{{#if (monthOfYear(model.date) >= 3 && monthOfYear(model.date) <= 5)}}
  Spring
{{else if (monthOfYear(model.date) >= 6 && monthOfYear(model.date) <= 8)}}
  Summer
{{else if (monthOfYear(model.date) >= 9 && monthOfYear(model.date) <= 11)}}
  Fall
{{else}}
  Winter
{{/if}}
</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    date = new DateTime(2024, 7, 15)
};
```

**Output:**
```html
<p>Season: Summer</p>
```

### Sales by Month

```handlebars
<h3>Monthly Sales</h3>
{{#each model.sales}}
  <p>Month {{monthOfYear(this.date)}}: ${{this.amount}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    sales = new[] {
        new { date = new DateTime(2024, 1, 15), amount = 5000 },
        new { date = new DateTime(2024, 2, 15), amount = 6000 },
        new { date = new DateTime(2024, 3, 15), amount = 5500 }
    }
};
```

**Output:**
```html
<h3>Monthly Sales</h3>
<p>Month 1: $5000</p>
<p>Month 2: $6000</p>
<p>Month 3: $5500</p>
```

---

## Notes

- Returns numeric value 1-12
- 1 = January, 12 = December
- Time component is ignored
- For month names, use `format(date, 'MMMM')` or `format(date, 'MMM')`
- Combine with conditional logic for seasonal or quarterly grouping
- Use with `yearOf()` and `dayOfMonth()` for complete date parsing

---

## See Also

- [yearOf Function](./yearOf.md)
- [dayOfMonth Function](./dayOfMonth.md)
- [addMonths Function](./addMonths.md)
- [format Function](./format.md)

---
