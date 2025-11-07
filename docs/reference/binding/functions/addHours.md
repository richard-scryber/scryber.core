---
layout: default
title: addHours
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# addHours() : Add Hours to Date
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

Add hours to a datetime. Use negative values to subtract hours.

## Signature

```
addHours(date, hours)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `date` | DateTime | Yes | The datetime to modify |
| `hours` | Number | Yes | Number of hours to add (can be negative) |

---

## Returns

**Type:** DateTime

A new DateTime with the specified hours added.

---

## Examples

### Add Hours

```handlebars
<p>Meeting ends: {{format(addHours(model.startTime, 2), 'h:mm tt')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    startTime = new DateTime(2024, 3, 15, 14, 30, 0)
};
```

**Output:**
```html
<p>Meeting ends: 4:30 PM</p>
```

### Delivery Window

```handlebars
<p>Delivery between {{format(addHours(model.orderTime, 2), 'h:mm tt')}} and {{format(addHours(model.orderTime, 4), 'h:mm tt')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    orderTime = new DateTime(2024, 3, 15, 10, 0, 0)
};
```

**Output:**
```html
<p>Delivery between 12:00 PM and 2:00 PM</p>
```

### Time Zone Adjustment

```handlebars
<p>EST: {{format(model.time, 'h:mm tt')}}</p>
<p>PST: {{format(addHours(model.time, -3), 'h:mm tt')}}</p>
```

---

## Notes

- Input datetime is not modified (returns new datetime)
- Can add positive or negative hours
- Automatically handles day boundaries
- Use `addMinutes()` for minute increments
- Use `addDays()` for day increments

---

## See Also

- [addMinutes Function](./addMinutes.md)
- [addDays Function](./addDays.md)
- [hoursBetween Function](./hoursBetween.md)
- [format Function](./format.md)

---
