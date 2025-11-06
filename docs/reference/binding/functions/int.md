---
layout: default
title: int
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# int() : Convert to Integer
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

Convert a value to a 32-bit integer. Truncates decimal values.

**Alias:** `integer()`

## Signature

```
int(value)
integer(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Any | Yes | The value to convert to integer |

---

## Returns

**Type:** Int32

A 32-bit integer representation of the value.

---

## Examples

### Convert String to Integer

```handlebars
<p>Value: {{int(model.stringValue)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    stringValue = "42"
};
```

**Output:**
```html
<p>Value: 42</p>
```

### Truncate Decimal

```handlebars
<p>Truncated: {{int(model.decimalValue)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    decimalValue = 19.95m
};
```

**Output:**
```html
<p>Truncated: 19</p>
```

### Calculate Whole Units

```handlebars
<p>Full boxes: {{int(model.items / model.boxSize)}}</p>
<p>Remaining items: {{model.items % model.boxSize}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    items = 47,
    boxSize = 12
};
```

**Output:**
```html
<p>Full boxes: 3</p>
<p>Remaining items: 11</p>
```

### Age Calculation

```handlebars
<p>Age: {{int(daysBetween(model.birthDate, model.today) / 365)}} years</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    birthDate = new DateTime(1990, 5, 15),
    today = new DateTime(2024, 3, 15)
};
```

**Output:**
```html
<p>Age: 33 years</p>
```

---

## Notes

- Truncates decimal places (does not round)
- Handles string-to-integer conversion
- Throws exception if value cannot be converted
- For rounding instead of truncating, use `round()` function
- Range: -2,147,483,648 to 2,147,483,647
- For larger values, use `long()` function
- Alias `integer()` provides same functionality

---

## See Also

- [long Function](./long.md)
- [double Function](./double.md)
- [decimal Function](./decimal.md)
- [round Function](./round.md)
- [truncate Function](./truncate.md)

---
