---
layout: default
title: secondOf
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# secondOf() : Extract Second from DateTime
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

Extract the second component from a datetime value as a number (0-59).

## Signature

```
secondOf(datetime)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `datetime` | DateTime | Yes | The datetime to extract the second from |

---

## Returns

**Type:** Number (Integer)

The second as a number from 0 to 59.

---

## Examples

### Display Second

```handlebars
<p>Second: {{secondOf(model.time)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    time = new DateTime(2024, 3, 15, 14, 30, 45)
};
```

**Output:**
```html
<p>Second: 45</p>
```

### Precise Time Display

```handlebars
<p>Time: {{hourOf(model.time)}}:{{padLeft(string(minuteOf(model.time)), 2, '0')}}:{{padLeft(string(secondOf(model.time)), 2, '0')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    time = new DateTime(2024, 3, 15, 9, 5, 3)
};
```

**Output:**
```html
<p>Time: 9:05:03</p>
```

### Timestamp Validation

```handlebars
{{#each model.events}}
  <p>{{format(this.timestamp, 'h:mm:ss tt')}}
  {{#if (secondOf(this.timestamp) == 0)}}
    (Rounded)
  {{/if}}
  </p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    events = new[] {
        new { timestamp = new DateTime(2024, 3, 15, 10, 30, 0) },
        new { timestamp = new DateTime(2024, 3, 15, 10, 32, 15) },
        new { timestamp = new DateTime(2024, 3, 15, 10, 35, 0) }
    }
};
```

**Output:**
```html
<p>10:30:00 AM (Rounded)</p>
<p>10:32:15 AM</p>
<p>10:35:00 AM (Rounded)</p>
```

### Performance Timing

```handlebars
<h3>Request Processing Time</h3>
<p>Start: {{format(model.startTime, 'h:mm:ss')}}</p>
<p>End: {{format(model.endTime, 'h:mm:ss')}}</p>
<p>Seconds: {{secondOf(model.endTime) - secondOf(model.startTime)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    startTime = new DateTime(2024, 3, 15, 10, 0, 12),
    endTime = new DateTime(2024, 3, 15, 10, 0, 47)
};
```

**Output:**
```html
<h3>Request Processing Time</h3>
<p>Start: 10:00:12</p>
<p>End: 10:00:47</p>
<p>Seconds: 35</p>
```

### Synchronization Check

```handlebars
<p>Sync Status:
{{#if (secondOf(model.lastSync) < 5)}}
  Recently synced
{{else}}
  {{secondOf(model.lastSync)}} seconds ago
{{/if}}
</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    lastSync = new DateTime(2024, 3, 15, 10, 30, 3)
};
```

**Output:**
```html
<p>Sync Status: Recently synced</p>
```

---

## Notes

- Returns 0-59
- Date, hour, and minute components are ignored
- For full time formatting, use `format()` function
- Useful for:
  - Precise timestamp display
  - Performance timing
  - Synchronization tracking
  - High-precision time calculations
- Combine with `hourOf()` and `minuteOf()` for complete time parsing
- Use `millisecondOf()` for sub-second precision
- Use `padLeft()` for zero-padded display (e.g., "05" instead of "5")

---

## See Also

- [millisecondOf Function](./millisecondOf.md)
- [minuteOf Function](./minuteOf.md)
- [addSeconds Function](./addSeconds.md)
- [format Function](./format.md)

---
