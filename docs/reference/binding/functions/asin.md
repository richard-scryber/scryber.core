---
layout: default
title: asin
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# asin() : Arcsine (Inverse Sine)
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

Calculate the arcsine (inverse sine) of a value, returning the angle in radians.

## Signature

```
asin(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Number | Yes | Value between -1 and 1 |

---

## Returns

**Type:** Double

The angle in radians (between -π/2 and π/2).

---

## Examples

### Basic Arcsine

```handlebars
<p>asin(0.5) = {{format(asin(0.5), '0.000')}} radians</p>
<p>asin(0.5) = {{format(degrees(asin(0.5)), '0.0')}}°</p>
```

**Output:**
```html
<p>asin(0.5) = 0.524 radians</p>
<p>asin(0.5) = 30.0°</p>
```

### Find Angle from Ratio

```handlebars
<p>Angle: {{format(degrees(asin(model.opposite / model.hypotenuse)), '0.0')}}°</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    opposite = 5,
    hypotenuse = 10
};
```

**Output:**
```html
<p>Angle: 30.0°</p>
```

---

## Notes

- Value must be between -1 and 1
- Returns angle in radians
- Result range: -π/2 to π/2
- Inverse of `sin()` function
- For degrees, use: `degrees(asin(value))`

---

## See Also

- [sin Function](./sin.md)
- [acos Function](./acos.md)
- [atan Function](./atan.md)
- [degrees Function](./degrees.md)

---
