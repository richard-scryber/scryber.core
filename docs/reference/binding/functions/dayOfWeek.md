---
layout: default
title: dayOfWeek
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# dayOfWeek() : Extract Day of Week from Date
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

Extract the day of the week from a datetime value as a number (0-6), where 0 is Sunday and 6 is Saturday.

## Signature

```
dayOfWeek(date)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `date` | DateTime | Yes | The datetime to extract the day of week from |

---

## Returns

**Type:** Number (Integer)

The day of the week as a number from 0 (Sunday) to 6 (Saturday).

---

## Examples

### Display Day Number

```handlebars
<p>Day of week: {{dayOfWeek(model.date)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    date = new DateTime(2024, 3, 15)  // Friday
};
```

**Output:**
```html
<p>Day of week: 5</p>
```

### Weekend Detection

```handlebars
<p>{{format(model.date, 'dddd, MMMM dd')}}:
{{#if (dayOfWeek(model.date) == 0 || dayOfWeek(model.date) == 6)}}
  Weekend
{{else}}
  Weekday
{{/if}}
</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    date = new DateTime(2024, 3, 16)  // Saturday
};
```

**Output:**
```html
<p>Saturday, March 16: Weekend</p>
```

### Business Days Filter

```handlebars
<h3>Business Day Appointments</h3>
{{#each model.appointments}}
  {{#if (dayOfWeek(this.date) >= 1 && dayOfWeek(this.date) <= 5)}}
    <p>{{format(this.date, 'ddd MMM dd')}}: {{this.description}}</p>
  {{/if}}
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    appointments = new[] {
        new { date = new DateTime(2024, 3, 15), description = "Team Meeting" },      // Friday
        new { date = new DateTime(2024, 3, 16), description = "Workshop" },          // Saturday
        new { date = new DateTime(2024, 3, 18), description = "Client Call" }       // Monday
    }
};
```

**Output:**
```html
<h3>Business Day Appointments</h3>
<p>Fri Mar 15: Team Meeting</p>
<p>Mon Mar 18: Client Call</p>
```

### Shift Schedule

```handlebars
{{#each model.dates}}
  <p>{{format(this, 'MMM dd')}}:
  {{#if (dayOfWeek(this) == 1)}}
    Morning Shift
  {{else if (dayOfWeek(this) == 3)}}
    Afternoon Shift
  {{else if (dayOfWeek(this) == 5)}}
    Night Shift
  {{else}}
    Off
  {{/if}}
  </p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    dates = new[] {
        new DateTime(2024, 3, 18),  // Monday
        new DateTime(2024, 3, 20),  // Wednesday
        new DateTime(2024, 3, 22),  // Friday
        new DateTime(2024, 3, 23)   // Saturday
    }
};
```

**Output:**
```html
<p>Mar 18: Morning Shift</p>
<p>Mar 20: Afternoon Shift</p>
<p>Mar 22: Night Shift</p>
<p>Mar 23: Off</p>
```

---

## Notes

- Returns 0-6 where 0 = Sunday, 1 = Monday, ..., 6 = Saturday
- Based on .NET DayOfWeek enumeration
- Time component is ignored
- For day names, use `format(date, 'dddd')` for full name or `format(date, 'ddd')` for abbreviated
- Useful for weekend/weekday logic and scheduling patterns
- Common patterns:
  - Weekend: `dayOfWeek == 0 || dayOfWeek == 6`
  - Weekday: `dayOfWeek >= 1 && dayOfWeek <= 5`

---

## See Also

- [dayOfMonth Function](./dayOfMonth.md)
- [dayOfYear Function](./dayOfYear.md)
- [addDays Function](./addDays.md)
- [format Function](./format.md)

---
