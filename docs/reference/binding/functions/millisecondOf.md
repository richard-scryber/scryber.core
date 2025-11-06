---
layout: default
title: millisecondOf
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# millisecondOf() : Extract Millisecond from DateTime
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

Extract the millisecond component from a datetime value as a number (0-999).

## Signature

```
millisecondOf(datetime)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `datetime` | DateTime | Yes | The datetime to extract the millisecond from |

---

## Returns

**Type:** Number (Integer)

The millisecond as a number from 0 to 999.

---

## Examples

### Display Millisecond

```handlebars
<p>Millisecond: {{millisecondOf(model.time)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    time = new DateTime(2024, 3, 15, 14, 30, 45, 123)
};
```

**Output:**
```html
<p>Millisecond: 123</p>
```

### High-Precision Timestamp

```handlebars
<p>Timestamp: {{format(model.time, 'HH:mm:ss')}}.{{padLeft(string(millisecondOf(model.time)), 3, '0')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    time = new DateTime(2024, 3, 15, 14, 30, 45, 5)
};
```

**Output:**
```html
<p>Timestamp: 14:30:45.005</p>
```

### Performance Metrics

```handlebars
<h3>API Response Times</h3>
{{#each model.requests}}
  <p>{{this.endpoint}}: {{secondOf(this.duration)}}.{{padLeft(string(millisecondOf(this.duration)), 3, '0')}}s</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    requests = new[] {
        new { endpoint = "/api/users", duration = new DateTime(1, 1, 1, 0, 0, 2, 150) },
        new { endpoint = "/api/orders", duration = new DateTime(1, 1, 1, 0, 0, 0, 850) },
        new { endpoint = "/api/products", duration = new DateTime(1, 1, 1, 0, 0, 1, 500) }
    }
};
```

**Output:**
```html
<h3>API Response Times</h3>
<p>/api/users: 2.150s</p>
<p>/api/orders: 0.850s</p>
<p>/api/products: 1.500s</p>
```

### Precision Timing Validation

```handlebars
{{#each model.measurements}}
  <p>{{this.name}}: {{format(this.timestamp, 'HH:mm:ss.fff')}}
  {{#if (millisecondOf(this.timestamp) == 0)}}
    (Low precision)
  {{else}}
    (High precision)
  {{/if}}
  </p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    measurements = new[] {
        new { name = "Sensor A", timestamp = new DateTime(2024, 3, 15, 10, 0, 0, 0) },
        new { name = "Sensor B", timestamp = new DateTime(2024, 3, 15, 10, 0, 0, 125) },
        new { name = "Sensor C", timestamp = new DateTime(2024, 3, 15, 10, 0, 1, 0) }
    }
};
```

**Output:**
```html
<p>Sensor A: 10:00:00.000 (Low precision)</p>
<p>Sensor B: 10:00:00.125 (High precision)</p>
<p>Sensor C: 10:00:01.000 (Low precision)</p>
```

### Race Timing

```handlebars
<h3>Race Results</h3>
{{#each model.racers}}
  <p>{{this.name}}: {{minuteOf(this.finishTime)}}:{{padLeft(string(secondOf(this.finishTime)), 2, '0')}}.{{padLeft(string(millisecondOf(this.finishTime)), 3, '0')}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    racers = new[] {
        new { name = "Runner 1", finishTime = new DateTime(1, 1, 1, 0, 5, 23, 450) },
        new { name = "Runner 2", finishTime = new DateTime(1, 1, 1, 0, 5, 25, 120) },
        new { name = "Runner 3", finishTime = new DateTime(1, 1, 1, 0, 5, 23, 890) }
    }
};
```

**Output:**
```html
<h3>Race Results</h3>
<p>Runner 1: 5:23.450</p>
<p>Runner 2: 5:25.120</p>
<p>Runner 3: 5:23.890</p>
```

---

## Notes

- Returns 0-999
- Most precise time component extraction function
- Date, hour, minute, and second components are ignored
- Useful for:
  - High-precision timing
  - Performance monitoring
  - Race/competition timing
  - Scientific measurements
  - API latency tracking
- Always use with `padLeft()` for proper display (e.g., "005" not "5")
- For complete high-precision time formatting, use `format(datetime, 'HH:mm:ss.fff')`
- Combine with `secondOf()` for sub-second timing displays

---

## See Also

- [secondOf Function](./secondOf.md)
- [minuteOf Function](./minuteOf.md)
- [addMilliseconds Function](./addMilliseconds.md)
- [format Function](./format.md)

---
