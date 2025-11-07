---
layout: default
title: hourOf
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# hourOf() : Extract Hour from DateTime
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

Extract the hour component from a datetime value as a 24-hour format number (0-23).

## Signature

```
hourOf(datetime)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `datetime` | DateTime | Yes | The datetime to extract the hour from |

---

## Returns

**Type:** Number (Integer)

The hour in 24-hour format as a number from 0 (midnight) to 23 (11 PM).

---

## Examples

### Display Hour

```handlebars
<p>Hour: {{hourOf(model.time)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    time = new DateTime(2024, 3, 15, 14, 30, 0)
};
```

**Output:**
```html
<p>Hour: 14</p>
```

### Business Hours Check

```handlebars
<p>Status:
{{#if (hourOf(model.currentTime) >= 9 && hourOf(model.currentTime) < 17)}}
  Open
{{else}}
  Closed
{{/if}}
</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    currentTime = new DateTime(2024, 3, 15, 10, 30, 0)
};
```

**Output:**
```html
<p>Status: Open</p>
```

### Shift Assignment

```handlebars
<p>Shift:
{{#if (hourOf(model.clockIn) < 8)}}
  Early Morning (12 AM - 8 AM)
{{else if (hourOf(model.clockIn) < 16)}}
  Day Shift (8 AM - 4 PM)
{{else}}
  Evening Shift (4 PM - 12 AM)
{{/if}}
</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    clockIn = new DateTime(2024, 3, 15, 14, 0, 0)
};
```

**Output:**
```html
<p>Shift: Day Shift (8 AM - 4 PM)</p>
```

### Peak Hour Pricing

```handlebars
{{#each model.transactions}}
  <p>{{format(this.time, 'h:mm tt')}}: ${{this.amount}}
  {{#if (hourOf(this.time) >= 17 && hourOf(this.time) < 20)}}
    (Peak Rate)
  {{/if}}
  </p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    transactions = new[] {
        new { time = new DateTime(2024, 3, 15, 14, 30, 0), amount = 5.00 },
        new { time = new DateTime(2024, 3, 15, 18, 15, 0), amount = 7.50 },
        new { time = new DateTime(2024, 3, 15, 21, 0, 0), amount = 5.00 }
    }
};
```

**Output:**
```html
<p>2:30 PM: $5</p>
<p>6:15 PM: $7.5 (Peak Rate)</p>
<p>9:00 PM: $5</p>
```

---

## Notes

- Returns 0-23 in 24-hour format
- 0 = midnight, 12 = noon, 23 = 11 PM
- Date component is ignored
- For 12-hour format display, use `format(datetime, 'h tt')` or `format(datetime, 'hh tt')`
- Useful for:
  - Business hours logic
  - Shift scheduling
  - Time-of-day pricing
  - Activity categorization by hour
- For full time formatting, use `format()` function

---

## See Also

- [minuteOf Function](./minuteOf.md)
- [secondOf Function](./secondOf.md)
- [addHours Function](./addHours.md)
- [format Function](./format.md)

---
