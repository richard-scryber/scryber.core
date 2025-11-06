---
layout: default
title: hoursBetween
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# hoursBetween() : Calculate Hours Between Dates
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

Calculate the number of hours between two datetimes. Returns a positive or negative number depending on whether the second datetime is after or before the first.

## Signature

```
hoursBetween(startDateTime, endDateTime)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `startDateTime` | DateTime | Yes | The starting datetime |
| `endDateTime` | DateTime | Yes | The ending datetime |

---

## Returns

**Type:** Number (Integer)

The number of hours between the two datetimes. Positive if endDateTime is after startDateTime, negative if before.

---

## Examples

### Calculate Hours Until Event

```handlebars
<p>Hours until meeting: {{hoursBetween(model.now, model.meetingTime)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    now = new DateTime(2024, 3, 15, 9, 0, 0),
    meetingTime = new DateTime(2024, 3, 15, 14, 30, 0)
};
```

**Output:**
```html
<p>Hours until meeting: 5</p>
```

### Shift Duration

```handlebars
<h3>Work Shift Report</h3>
{{#each model.shifts}}
  <p>{{this.employee}}: {{hoursBetween(this.clockIn, this.clockOut)}} hours</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    shifts = new[] {
        new {
            employee = "Alice",
            clockIn = new DateTime(2024, 3, 15, 8, 0, 0),
            clockOut = new DateTime(2024, 3, 15, 17, 0, 0)
        },
        new {
            employee = "Bob",
            clockIn = new DateTime(2024, 3, 15, 9, 0, 0),
            clockOut = new DateTime(2024, 3, 15, 18, 30, 0)
        }
    }
};
```

**Output:**
```html
<h3>Work Shift Report</h3>
<p>Alice: 9 hours</p>
<p>Bob: 9 hours</p>
```

### SLA Compliance

```handlebars
<p>Response time: {{hoursBetween(model.ticketCreated, model.firstResponse)}} hours</p>
<p>Status:
{{#if (hoursBetween(model.ticketCreated, model.firstResponse) <= model.slaHours)}}
  Within SLA
{{else}}
  SLA Breach
{{/if}}
</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    ticketCreated = new DateTime(2024, 3, 15, 10, 0, 0),
    firstResponse = new DateTime(2024, 3, 15, 11, 30, 0),
    slaHours = 4
};
```

**Output:**
```html
<p>Response time: 1 hours</p>
<p>Status: Within SLA</p>
```

---

## Notes

- Returns integer number of hours (fractional hours are truncated)
- Positive result: endDateTime is after startDateTime
- Negative result: endDateTime is before startDateTime
- Includes both date and time components
- For minute-level precision, use `minutesBetween()`
- For day-level precision, use `daysBetween()`
- For fractional hours, calculate using `minutesBetween() / 60`

---

## See Also

- [minutesBetween Function](./minutesBetween.md)
- [daysBetween Function](./daysBetween.md)
- [addHours Function](./addHours.md)
- [format Function](./format.md)

---
