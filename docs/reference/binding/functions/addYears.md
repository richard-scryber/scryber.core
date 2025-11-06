---
layout: default
title: addYears
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# addYears() : Add Years to Date
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

Add years to a date. Use negative values to subtract years.

## Signature

```
addYears(date, years)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `date` | DateTime | Yes | The date to modify |
| `years` | Number | Yes | Number of years to add (can be negative) |

---

## Returns

**Type:** DateTime

A new DateTime with the specified years added.

---

## Examples

### Add Years

```handlebars
<p>Expiry: {{format(addYears(model.issued, 5), 'MMMM dd, yyyy')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    issued = new DateTime(2024, 3, 15)
};
```

**Output:**
```html
<p>Expiry: March 15, 2029</p>
```

### Calculate Age Next Year

```handlebars
<p>Next year you'll be {{yearOf(addYears(model.today, 1)) - yearOf(model.birthDate)}} years old</p>
```

### Contract Renewal Dates

```handlebars
<h3>Renewal Schedule</h3>
{{#each model.renewalYears}}
  <p>Year {{this}}: {{format(addYears(model.startDate, this), 'yyyy-MM-dd')}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    startDate = new DateTime(2024, 1, 1),
    renewalYears = new[] { 1, 2, 3, 5, 10 }
};
```

**Output:**
```html
<h3>Renewal Schedule</h3>
<p>Year 1: 2025-01-01</p>
<p>Year 2: 2026-01-01</p>
<p>Year 3: 2027-01-01</p>
<p>Year 5: 2029-01-01</p>
<p>Year 10: 2034-01-01</p>
```

### Subtract Years

```handlebars
<p>5 years ago: {{format(addYears(model.today, -5), 'MMMM yyyy')}}</p>
```

---

## Notes

- Input date is not modified (returns new date)
- Can add positive or negative years
- Handles leap years correctly
- If Feb 29 on leap year + 1 year = Feb 28
- Use `addMonths()` for month increments
- Use `addDays()` for day increments

---

## See Also

- [addMonths Function](./addMonths.md)
- [addDays Function](./addDays.md)
- [yearOf Function](./yearOf.md)
- [format Function](./format.md)

---
