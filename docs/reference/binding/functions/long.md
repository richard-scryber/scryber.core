---
layout: default
title: long
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# long() : Convert to Long Integer
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

Convert a value to a 64-bit long integer. Useful for large numbers that exceed Int32 range.

## Signature

```
long(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Any | Yes | The value to convert to long integer |

---

## Returns

**Type:** Int64

A 64-bit integer representation of the value.

---

## Examples

### Large Number Conversion

```handlebars
<p>Population: {{format(long(model.population), 'N0')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    population = "7800000000"
};
```

**Output:**
```html
<p>Population: 7,800,000,000</p>
```

### File Size Calculation

```handlebars
<p>Total Size: {{format(long(model.bytes) / 1024 / 1024, '0.00')}} MB</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    bytes = "52428800"
};
```

**Output:**
```html
<p>Total Size: 50.00 MB</p>
```

### Timestamp Conversion

```handlebars
<p>Timestamp: {{long(model.timestamp)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    timestamp = "1679875200000"
};
```

**Output:**
```html
<p>Timestamp: 1679875200000</p>
```

---

## Notes

- Truncates decimal places (does not round)
- Handles string-to-long conversion
- Throws exception if value cannot be converted
- Range: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
- Use for large numbers that exceed Int32 range (-2.1B to 2.1B)
- For decimal precision, use `decimal()` instead

---

## See Also

- [int Function](./int.md)
- [double Function](./double.md)
- [decimal Function](./decimal.md)
- [format Function](./format.md)

---
