---
layout: default
title: minutesBetween
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# minutesBetween() : Calculate Minutes Between Dates
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

Calculate the number of minutes between two datetimes. Returns a positive or negative number depending on whether the second datetime is after or before the first.

## Signature

```
minutesBetween(startDateTime, endDateTime)
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

The number of minutes between the two datetimes. Positive if endDateTime is after startDateTime, negative if before.

---

## Examples

### Meeting Duration

```handlebars
<p>Meeting duration: {{minutesBetween(model.startTime, model.endTime)}} minutes</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    startTime = new DateTime(2024, 3, 15, 14, 0, 0),
    endTime = new DateTime(2024, 3, 15, 15, 30, 0)
};
```

**Output:**
```html
<p>Meeting duration: 90 minutes</p>
```

### Parking Duration and Cost

```handlebars
<h3>Parking Receipt</h3>
<p>Entry: {{format(model.entryTime, 'h:mm tt')}}</p>
<p>Exit: {{format(model.exitTime, 'h:mm tt')}}</p>
<p>Duration: {{minutesBetween(model.entryTime, model.exitTime)}} minutes</p>
<p>Cost: ${{round((minutesBetween(model.entryTime, model.exitTime) / 60) * model.hourlyRate, 2)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    entryTime = new DateTime(2024, 3, 15, 10, 15, 0),
    exitTime = new DateTime(2024, 3, 15, 14, 45, 0),
    hourlyRate = 5.00
};
```

**Output:**
```html
<h3>Parking Receipt</h3>
<p>Entry: 10:15 AM</p>
<p>Exit: 2:45 PM</p>
<p>Duration: 270 minutes</p>
<p>Cost: $22.5</p>
```

### Call Center Metrics

```handlebars
{{#each model.calls}}
  <p>Call {{this.id}}: {{minutesBetween(this.startTime, this.endTime)}} min
  {{#if (minutesBetween(this.startTime, this.endTime) > 10)}}
    (Extended)
  {{/if}}
  </p>
{{/each}}
<p>Average: {{average(collect(model.calls, 'duration'))}} min</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    calls = new[] {
        new {
            id = "001",
            startTime = new DateTime(2024, 3, 15, 9, 0, 0),
            endTime = new DateTime(2024, 3, 15, 9, 5, 30),
            duration = 5
        },
        new {
            id = "002",
            startTime = new DateTime(2024, 3, 15, 9, 10, 0),
            endTime = new DateTime(2024, 3, 15, 9, 25, 0),
            duration = 15
        }
    }
};
```

**Output:**
```html
<p>Call 001: 5 min</p>
<p>Call 002: 15 min (Extended)</p>
<p>Average: 10 min</p>
```

---

## Notes

- Returns integer number of minutes (fractional minutes are truncated)
- Positive result: endDateTime is after startDateTime
- Negative result: endDateTime is before startDateTime
- Includes both date and time components
- For second-level precision, use `secondsBetween()`
- For hour-level precision, use `hoursBetween()`
- For fractional minutes, calculate using `secondsBetween() / 60`

---

## See Also

- [secondsBetween Function](./secondsBetween.md)
- [hoursBetween Function](./hoursBetween.md)
- [addMinutes Function](./addMinutes.md)
- [format Function](./format.md)

---
