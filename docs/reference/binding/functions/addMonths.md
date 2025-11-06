---
layout: default
title: addMonths
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# addMonths() : Add Months to Date
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

Add months to a date. Use negative values to subtract months.

## Signature

```
addMonths(date, months)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `date` | DateTime | Yes | The date to modify |
| `months` | Number | Yes | Number of months to add (can be negative) |

---

## Returns

**Type:** DateTime

A new DateTime with the specified months added.

---

## Examples

### Add Months

```handlebars
<p>Due Date: {{format(addMonths(model.startDate, 3), 'MMMM dd, yyyy')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    startDate = new DateTime(2024, 3, 15)
};
```

**Output:**
```html
<p>Due Date: June 15, 2024</p>
```

### Subtract Months

```handlebars
<p>3 months ago: {{format(addMonths(model.today, -3), 'yyyy-MM-dd')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    today = new DateTime(2024, 3, 15)
};
```

**Output:**
```html
<p>3 months ago: 2023-12-15</p>
```

### Payment Schedule

```handlebars
{{#each model.payments}}
  <li>Payment {{add(@index, 1)}}: {{format(addMonths(model.startDate, @index), 'MMM dd, yyyy')}}</li>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    startDate = new DateTime(2024, 1, 15),
    payments = new[] { 1, 2, 3, 4, 5, 6 }
};
```

**Output:**
```html
<li>Payment 1: Jan 15, 2024</li>
<li>Payment 2: Feb 15, 2024</li>
<li>Payment 3: Mar 15, 2024</li>
<li>Payment 4: Apr 15, 2024</li>
<li>Payment 5: May 15, 2024</li>
<li>Payment 6: Jun 15, 2024</li>
```

### Annual Review Date

```handlebars
<p>Next Review: {{format(addMonths(model.lastReview, 12), 'MMMM dd, yyyy')}}</p>
```

---

## Notes

- Input date is not modified (returns new date)
- Can add positive or negative months
- Handles year boundaries automatically
- If resulting day doesn't exist (e.g., Feb 31), adjusts to last valid day
- Use `addYears()` for year increments
- Use `addDays()` for day increments

---

## See Also

- [addYears Function](./addYears.md)
- [addDays Function](./addDays.md)
- [format Function](./format.md)
- [Date & Time Functions](./index.md#date--time-functions)

---
