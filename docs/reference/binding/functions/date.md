---
layout: default
title: date
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# date() : Convert to DateTime
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

Convert a string or number to a DateTime object.

## Signature

```
date(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | String/Number | Yes | The value to convert to DateTime |

---

## Returns

**Type:** DateTime

A DateTime object representing the parsed date.

---

## Supported Formats

| Format | Example | Description |
|--------|---------|-------------|
| ISO 8601 | `"2024-03-15"` | yyyy-MM-dd |
| ISO DateTime | `"2024-03-15T14:30:00"` | With time component |
| US Format | `"3/15/2024"` | M/d/yyyy |
| Long Format | `"March 15, 2024"` | Full month name |
| Unix Timestamp | `1710511200` | Seconds since 1970-01-01 |

---

## Examples

### Parse ISO Date

```handlebars
<p>Order Date: {{format(date(model.orderDate), 'MMMM dd, yyyy')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    orderDate = "2024-03-15"
};
```

**Output:**
```html
<p>Order Date: March 15, 2024</p>
```

### Calculate Days Until

```handlebars
<p>Days until deadline: {{daysBetween(model.today, date(model.deadline))}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    today = new DateTime(2024, 3, 1),
    deadline = "2024-03-31"
};
```

**Output:**
```html
<p>Days until deadline: 30</p>
```

### Format Different Date Strings

```handlebars
{{#each model.events}}
  <div class="event">
    <h3>{{this.name}}</h3>
    <p>{{format(date(this.dateString), 'dddd, MMMM dd, yyyy')}}</p>
  </div>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    events = new[] {
        new { name = "Conference", dateString = "2024-04-15" },
        new { name = "Workshop", dateString = "2024-05-20" },
        new { name = "Seminar", dateString = "2024-06-10" }
    }
};
```

**Output:**
```html
<div class="event">
  <h3>Conference</h3>
  <p>Monday, April 15, 2024</p>
</div>
<div class="event">
  <h3>Workshop</h3>
  <p>Monday, May 20, 2024</p>
</div>
<div class="event">
  <h3>Seminar</h3>
  <p>Monday, June 10, 2024</p>
</div>
```

### Add Days to String Date

```handlebars
<p>Ship Date: {{format(date(model.orderDate), 'yyyy-MM-dd')}}</p>
<p>Est. Delivery: {{format(addDays(date(model.orderDate), 5), 'yyyy-MM-dd')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    orderDate = "2024-03-15"
};
```

**Output:**
```html
<p>Ship Date: 2024-03-15</p>
<p>Est. Delivery: 2024-03-20</p>
```

---

## Notes

- Automatically detects common date formats
- Throws exception if string cannot be parsed as date
- Use with date manipulation functions (addDays, addMonths, etc.)
- Always store dates as DateTime in model when possible
- For formatting output, use `format()` function
- Time zone handling depends on input format

---

## See Also

- [format Function](./format.md)
- [addDays Function](./addDays.md)
- [addMonths Function](./addMonths.md)
- [daysBetween Function](./daysBetween.md)

---
