---
layout: default
title: addDays
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# addDays() : Add Days to Date
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

Add days to a date. Use negative values to subtract days.

## Signature

```
addDays(date, days)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `date` | DateTime | Yes | The date to modify |
| `days` | Number | Yes | Number of days to add (can be negative) |

---

## Returns

**Type:** DateTime

A new DateTime with the specified days added.

---

## Examples

### Add Days

```handlebars
{{format(addDays(model.orderDate, 7), 'yyyy-MM-dd')}}
<!-- Adds 7 days to order date -->
```

**Data:**
```csharp
doc.Params["model"] = new {
    orderDate = new DateTime(2024, 3, 15)
};
```

**Output:**
```html
2024-03-22
```

### Subtract Days

```handlebars
<p>7 days ago: {{format(addDays(model.today, -7), 'yyyy-MM-dd')}}</p>
```

### Calculate Delivery Date

```handlebars
<p>Est. Delivery: {{format(addDays(model.shippedDate, 3), 'MMMM dd, yyyy')}}</p>
```

### Calculate Deadline

```handlebars
<p>Due: {{format(addDays(model.startDate, model.durationDays), 'MM/dd/yyyy')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    startDate = new DateTime(2024, 3, 15),
    durationDays = 30
};
```

**Output:**
```html
<p>Due: 04/14/2024</p>
```

---

## Notes

- Input date is not modified (returns new date)
- Can add positive or negative days
- Works with leap years automatically
- Use `addMonths` or `addYears` for larger intervals

---

## See Also

- [addMonths Function](./addMonths.md)
- [addYears Function](./addYears.md)
- [daysBetween Function](./daysBetween.md)
- [Date & Time Functions](./index.md#date--time-functions)

---
