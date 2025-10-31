---
layout: default
title: daysBetween
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# daysBetween() : Calculate Days Between Dates
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

Calculate the number of days between two dates. Returns a positive or negative number depending on whether the second date is after or before the first.

## Signature

```
daysBetween(startDate, endDate)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `startDate` | DateTime | Yes | The starting date |
| `endDate` | DateTime | Yes | The ending date |

---

## Returns

**Type:** Number (Integer)

The number of days between the two dates. Positive if endDate is after startDate, negative if before.

---

## Examples

### Calculate Days Until Event

```handlebars
<p>Days until launch: {{daysBetween(model.today, model.launchDate)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    today = new DateTime(2024, 3, 15),
    launchDate = new DateTime(2024, 4, 20)
};
```

**Output:**
```html
<p>Days until launch: 36</p>
```

### Project Duration

```handlebars
<h3>Project Timeline</h3>
<p>Start: {{format(model.startDate, 'MMMM dd, yyyy')}}</p>
<p>End: {{format(model.endDate, 'MMMM dd, yyyy')}}</p>
<p>Duration: {{daysBetween(model.startDate, model.endDate)}} days</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    startDate = new DateTime(2024, 1, 15),
    endDate = new DateTime(2024, 3, 30)
};
```

**Output:**
```html
<h3>Project Timeline</h3>
<p>Start: January 15, 2024</p>
<p>End: March 30, 2024</p>
<p>Duration: 75 days</p>
```

### Overdue Calculation

```handlebars
{{#each model.tasks}}
  <p>{{this.name}}:
  {{#if (daysBetween(this.dueDate, model.today) < 0)}}
    OVERDUE by {{abs(daysBetween(this.dueDate, model.today))}} days
  {{else}}
    Due in {{daysBetween(model.today, this.dueDate)}} days
  {{/if}}
  </p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    today = new DateTime(2024, 3, 15),
    tasks = new[] {
        new { name = "Design", dueDate = new DateTime(2024, 3, 10) },
        new { name = "Development", dueDate = new DateTime(2024, 3, 20) }
    }
};
```

**Output:**
```html
<p>Design: OVERDUE by 5 days</p>
<p>Development: Due in 5 days</p>
```

---

## Notes

- Returns integer number of days (fractional days are truncated)
- Positive result: endDate is after startDate
- Negative result: endDate is before startDate
- Time component is ignored (only date part is used)
- For hour-level precision, use `hoursBetween()`
- For fractional days, calculate using `hoursBetween() / 24`

---

## See Also

- [hoursBetween Function](./hoursBetween.md)
- [addDays Function](./addDays.md)
- [dayOfMonth Function](./dayOfMonth.md)
- [format Function](./format.md)

---
