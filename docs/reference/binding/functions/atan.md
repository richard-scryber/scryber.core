---
layout: default
title: atan
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# atan() : Arctangent (Inverse Tangent)
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

Calculate the arctangent (inverse tangent) of a value, returning the angle in radians.

## Signature

```
atan(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Number | Yes | The tangent value |

---

## Returns

**Type:** Double

The angle in radians (between -π/2 and π/2).

---

## Examples

### Basic Arctangent

```handlebars
<p>atan(1) = {{format(atan(1), '0.000')}} radians</p>
<p>atan(1) = {{format(degrees(atan(1)), '0.0')}}°</p>
```

**Output:**
```html
<p>atan(1) = 0.785 radians</p>
<p>atan(1) = 45.0°</p>
```

### Find Angle from Slope

```handlebars
<p>Angle: {{format(degrees(atan(model.rise / model.run)), '0.0')}}°</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    rise = 5.77,
    run = 10
};
```

**Output:**
```html
<p>Angle: 30.0°</p>
```

### Calculate Bearing

```handlebars
<p>Bearing: {{format(degrees(atan(model.opposite / model.adjacent)), '0.1')}}°</p>
```

---

## Notes

- Accepts any numeric value
- Returns angle in radians
- Result range: -π/2 to π/2
- Inverse of `tan()` function
- For degrees, use: `degrees(atan(value))`

---

## See Also

- [tan Function](./tan.md)
- [asin Function](./asin.md)
- [acos Function](./acos.md)
- [degrees Function](./degrees.md)

---
