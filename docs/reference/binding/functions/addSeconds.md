---
layout: default
title: addSeconds
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# addSeconds() : Add Seconds to Date
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

Add seconds to a datetime. Use negative values to subtract seconds.

## Signature

```
addSeconds(date, seconds)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `date` | DateTime | Yes | The datetime to modify |
| `seconds` | Number | Yes | Number of seconds to add (can be negative) |

---

## Returns

**Type:** DateTime

A new DateTime with the specified seconds added.

---

## Examples

### Add Seconds

```handlebars
<p>Timeout: {{format(addSeconds(model.startTime, 30), 'h:mm:ss tt')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    startTime = new DateTime(2024, 3, 15, 14, 30, 0)
};
```

**Output:**
```html
<p>Timeout: 2:30:30 PM</p>
```

### Countdown Display

```handlebars
<p>T-minus: {{secondsBetween(model.now, addSeconds(model.launchTime, 0))}} seconds</p>
```

### Processing Time

```handlebars
<p>Completed: {{format(addSeconds(model.started, model.elapsedSeconds), 'h:mm:ss')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    started = new DateTime(2024, 3, 15, 10, 0, 0),
    elapsedSeconds = 125
};
```

**Output:**
```html
<p>Completed: 10:02:05</p>
```

---

## Notes

- Input datetime is not modified (returns new datetime)
- Can add positive or negative seconds
- Automatically handles minute, hour, and day boundaries
- Use `addMilliseconds()` for millisecond precision
- Use `addMinutes()` for minute increments

---

## See Also

- [addMilliseconds Function](./addMilliseconds.md)
- [addMinutes Function](./addMinutes.md)
- [secondsBetween Function](./secondsBetween.md)
- [format Function](./format.md)

---
