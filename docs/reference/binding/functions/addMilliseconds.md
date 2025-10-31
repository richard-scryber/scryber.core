---
layout: default
title: addMilliseconds
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# addMilliseconds() : Add Milliseconds to Date
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

Add milliseconds to a datetime. Use negative values to subtract milliseconds.

## Signature

```
addMilliseconds(date, milliseconds)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `date` | DateTime | Yes | The datetime to modify |
| `milliseconds` | Number | Yes | Number of milliseconds to add (can be negative) |

---

## Returns

**Type:** DateTime

A new DateTime with the specified milliseconds added.

---

## Examples

### Add Milliseconds

```handlebars
<p>Precise time: {{format(addMilliseconds(model.time, 500), 'h:mm:ss.fff tt')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    time = new DateTime(2024, 3, 15, 14, 30, 0, 0)
};
```

**Output:**
```html
<p>Precise time: 2:30:00.500 PM</p>
```

### Performance Timing

```handlebars
<p>Duration: {{model.elapsedMs}} ms</p>
<p>Ended: {{format(addMilliseconds(model.started, model.elapsedMs), 'h:mm:ss.fff')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    started = new DateTime(2024, 3, 15, 10, 0, 0, 0),
    elapsedMs = 1250
};
```

**Output:**
```html
<p>Duration: 1250 ms</p>
<p>Ended: 10:00:01.250</p>
```

### High-Precision Timestamps

```handlebars
<p>Timestamp: {{format(addMilliseconds(model.base, model.offset), 'yyyy-MM-dd HH:mm:ss.fff')}}</p>
```

---

## Notes

- Input datetime is not modified (returns new datetime)
- Can add positive or negative milliseconds
- Automatically handles second, minute, hour, and day boundaries
- Most precise time addition function
- Use `addSeconds()` for second-level precision

---

## See Also

- [addSeconds Function](./addSeconds.md)
- [millisecondOf Function](./millisecondOf.md)
- [format Function](./format.md)

---
