---
layout: default
title: secondsBetween
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# secondsBetween() : Calculate Seconds Between Dates
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

Calculate the number of seconds between two datetimes. Returns a positive or negative number depending on whether the second datetime is after or before the first.

## Signature

```
secondsBetween(startDateTime, endDateTime)
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

The number of seconds between the two datetimes. Positive if endDateTime is after startDateTime, negative if before.

---

## Examples

### Countdown Timer

```handlebars
<p>T-minus {{secondsBetween(model.currentTime, model.launchTime)}} seconds</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    currentTime = new DateTime(2024, 3, 15, 10, 0, 0),
    launchTime = new DateTime(2024, 3, 15, 10, 5, 30)
};
```

**Output:**
```html
<p>T-minus 330 seconds</p>
```

### Performance Timing

```handlebars
<h3>Operation Performance</h3>
{{#each model.operations}}
  <p>{{this.name}}: {{secondsBetween(this.startTime, this.endTime)}}s
  {{#if (secondsBetween(this.startTime, this.endTime) > this.threshold)}}
    (Slow)
  {{/if}}
  </p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    operations = new[] {
        new {
            name = "Database Query",
            startTime = new DateTime(2024, 3, 15, 10, 0, 0),
            endTime = new DateTime(2024, 3, 15, 10, 0, 2),
            threshold = 5
        },
        new {
            name = "API Call",
            startTime = new DateTime(2024, 3, 15, 10, 0, 5),
            endTime = new DateTime(2024, 3, 15, 10, 0, 12),
            threshold = 5
        }
    }
};
```

**Output:**
```html
<h3>Operation Performance</h3>
<p>Database Query: 2s</p>
<p>API Call: 7s (Slow)</p>
```

### Video Duration

```handlebars
<p>Duration: {{floor(secondsBetween(model.startTime, model.endTime) / 60)}}:{{padLeft(string(secondsBetween(model.startTime, model.endTime) % 60), 2, '0')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    startTime = new DateTime(2024, 3, 15, 0, 0, 0),
    endTime = new DateTime(2024, 3, 15, 0, 3, 45)
};
```

**Output:**
```html
<p>Duration: 3:45</p>
```

### Response Time Analysis

```handlebars
<h3>API Response Times</h3>
<p>Average: {{average(collect(model.requests, 'responseTime'))}}s</p>
<p>Min: {{min(collect(model.requests, 'responseTime'))}}s</p>
<p>Max: {{max(collect(model.requests, 'responseTime'))}}s</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    requests = new[] {
        new { responseTime = 2 },
        new { responseTime = 5 },
        new { responseTime = 3 },
        new { responseTime = 8 }
    }
};
```

**Output:**
```html
<h3>API Response Times</h3>
<p>Average: 4.5s</p>
<p>Min: 2s</p>
<p>Max: 8s</p>
```

---

## Notes

- Returns integer number of seconds (fractional seconds are truncated)
- Positive result: endDateTime is after startDateTime
- Negative result: endDateTime is before startDateTime
- Includes both date and time components
- For minute-level precision, use `minutesBetween()`
- Most precise standard duration function
- Does not include milliseconds (use direct calculation if needed)

---

## See Also

- [minutesBetween Function](./minutesBetween.md)
- [hoursBetween Function](./hoursBetween.md)
- [addSeconds Function](./addSeconds.md)
- [format Function](./format.md)

---
