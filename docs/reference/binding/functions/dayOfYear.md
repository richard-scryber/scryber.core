---
layout: default
title: dayOfYear
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# dayOfYear() : Extract Day of Year from Date
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

Extract the day of the year from a datetime value as a number (1-366), representing which day it is in the calendar year.

## Signature

```
dayOfYear(date)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `date` | DateTime | Yes | The datetime to extract the day of year from |

---

## Returns

**Type:** Number (Integer)

The day of the year as a number from 1 (January 1st) to 365 or 366 (December 31st in a leap year).

---

## Examples

### Display Day of Year

```handlebars
<p>Day {{dayOfYear(model.date)}} of {{yearOf(model.date)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    date = new DateTime(2024, 3, 15)
};
```

**Output:**
```html
<p>Day 75 of 2024</p>
```

### Progress Through Year

```handlebars
<p>Year Progress: {{round((dayOfYear(model.today) / 365) * 100, 1)}}%</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    today = new DateTime(2024, 6, 30)
};
```

**Output:**
```html
<p>Year Progress: 50.1%</p>
```

### Manufacturing Day Code

```handlebars
<p>Production Code: {{yearOf(model.date) % 100}}{{padLeft(string(dayOfYear(model.date)), 3, '0')}}-{{model.batchNumber}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    date = new DateTime(2024, 3, 15),
    batchNumber = "A42"
};
```

**Output:**
```html
<p>Production Code: 24075-A42</p>
```

### Seasonal Marketing Periods

```handlebars
<p>Marketing Period:
{{#if (dayOfYear(model.date) <= 90)}}
  Q1 Campaign
{{else if (dayOfYear(model.date) <= 181)}}
  Q2 Campaign
{{else if (dayOfYear(model.date) <= 273)}}
  Q3 Campaign
{{else}}
  Q4 Campaign
{{/if}}
</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    date = new DateTime(2024, 8, 15)
};
```

**Output:**
```html
<p>Marketing Period: Q3 Campaign</p>
```

### Days Remaining in Year

```handlebars
<p>Days left in {{yearOf(model.today)}}: {{365 - dayOfYear(model.today)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    today = new DateTime(2024, 12, 20)
};
```

**Output:**
```html
<p>Days left in 2024: 11</p>
```

---

## Notes

- Returns 1-365 for regular years, 1-366 for leap years
- January 1st is always day 1
- December 31st is day 365 (or 366 in leap year)
- Time component is ignored
- Useful for:
  - Julian date calculations
  - Manufacturing date codes
  - Year progress tracking
  - Seasonal period calculations
- Does not account for leap years automatically in calculations (use 366 for leap years)

---

## See Also

- [dayOfMonth Function](./dayOfMonth.md)
- [dayOfWeek Function](./dayOfWeek.md)
- [yearOf Function](./yearOf.md)
- [format Function](./format.md)

---
