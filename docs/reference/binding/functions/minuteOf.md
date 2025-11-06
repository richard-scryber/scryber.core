---
layout: default
title: minuteOf
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# minuteOf() : Extract Minute from DateTime
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

Extract the minute component from a datetime value as a number (0-59).

## Signature

```
minuteOf(datetime)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `datetime` | DateTime | Yes | The datetime to extract the minute from |

---

## Returns

**Type:** Number (Integer)

The minute as a number from 0 to 59.

---

## Examples

### Display Minute

```handlebars
<p>Minute: {{minuteOf(model.time)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    time = new DateTime(2024, 3, 15, 14, 30, 0)
};
```

**Output:**
```html
<p>Minute: 30</p>
```

### Custom Time Format

```handlebars
<p>Time: {{hourOf(model.time)}}:{{padLeft(string(minuteOf(model.time)), 2, '0')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    time = new DateTime(2024, 3, 15, 9, 5, 0)
};
```

**Output:**
```html
<p>Time: 9:05</p>
```

### Appointment Slot Detection

```handlebars
<p>Time Slot:
{{#if (minuteOf(model.appointmentTime) == 0)}}
  On the hour
{{else if (minuteOf(model.appointmentTime) == 30)}}
  Half past
{{else}}
  {{minuteOf(model.appointmentTime)}} minutes past
{{/if}}
</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    appointmentTime = new DateTime(2024, 3, 15, 14, 30, 0)
};
```

**Output:**
```html
<p>Time Slot: Half past</p>
```

### Parking Time Calculation

```handlebars
<h3>Parking Duration</h3>
<p>Hours: {{hoursBetween(model.entry, model.exit)}}</p>
<p>Additional minutes: {{minuteOf(model.exit) - minuteOf(model.entry)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    entry = new DateTime(2024, 3, 15, 10, 15, 0),
    exit = new DateTime(2024, 3, 15, 12, 45, 0)
};
```

**Output:**
```html
<h3>Parking Duration</h3>
<p>Hours: 2</p>
<p>Additional minutes: 30</p>
```

### Meeting Start Validation

```handlebars
{{#each model.meetings}}
  <p>{{format(this.startTime, 'h:mm tt')}}
  {{#if (minuteOf(this.startTime) != 0 && minuteOf(this.startTime) != 30)}}
    (Non-standard start time)
  {{/if}}
  </p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    meetings = new[] {
        new { startTime = new DateTime(2024, 3, 15, 9, 0, 0) },
        new { startTime = new DateTime(2024, 3, 15, 10, 15, 0) },
        new { startTime = new DateTime(2024, 3, 15, 11, 30, 0) }
    }
};
```

**Output:**
```html
<p>9:00 AM</p>
<p>10:15 AM (Non-standard start time)</p>
<p>11:30 AM</p>
```

---

## Notes

- Returns 0-59
- Date and hour components are ignored
- For full time formatting, use `format()` function
- Useful for:
  - Custom time display
  - Time slot validation
  - Duration calculations
  - Schedule adherence checking
- Combine with `hourOf()` for complete time parsing
- Use `padLeft()` for zero-padded display (e.g., "05" instead of "5")

---

## See Also

- [hourOf Function](./hourOf.md)
- [secondOf Function](./secondOf.md)
- [addMinutes Function](./addMinutes.md)
- [format Function](./format.md)

---
