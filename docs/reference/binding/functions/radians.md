---
layout: default
title: radians
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# radians() : Convert Degrees to Radians
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

Convert an angle from degrees to radians.

## Signature

```
radians(degrees)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `degrees` | Number | Yes | The angle in degrees |

---

## Returns

**Type:** Double

The angle in radians.

---

## Examples

### Convert Common Angles

```handlebars
<p>30° = {{format(radians(30), '0.000')}} radians</p>
<p>45° = {{format(radians(45), '0.000')}} radians</p>
<p>90° = {{format(radians(90), '0.000')}} radians</p>
<p>180° = {{format(radians(180), '0.000')}} radians</p>
```

**Output:**
```html
<p>30° = 0.524 radians</p>
<p>45° = 0.785 radians</p>
<p>90° = 1.571 radians</p>
<p>180° = 3.142 radians</p>
```

### Use with Trigonometric Functions

```handlebars
<p>sin(30°) = {{format(sin(radians(30)), '0.000')}}</p>
<p>cos(60°) = {{format(cos(radians(60)), '0.000')}}</p>
<p>tan(45°) = {{format(tan(radians(45)), '0.000')}}</p>
```

**Output:**
```html
<p>sin(30°) = 0.500</p>
<p>cos(60°) = 0.500</p>
<p>tan(45°) = 1.000</p>
```

### Calculate Circular Position

```handlebars
<p>X: {{format(model.radius * cos(radians(model.angle)), '0.00')}}</p>
<p>Y: {{format(model.radius * sin(radians(model.angle)), '0.00')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    radius = 10,
    angle = 45  // degrees
};
```

**Output:**
```html
<p>X: 7.07</p>
<p>Y: 7.07</p>
```

---

## Notes

- Converts degrees to radians
- Formula: radians = degrees × (π/180)
- Inverse of `degrees()` function
- Required for trigonometric functions (sin, cos, tan)
- 360° = 2π radians, 180° = π radians

---

## See Also

- [degrees Function](./degrees.md)
- [pi Function](./pi.md)
- [sin Function](./sin.md)
- [cos Function](./cos.md)
- [tan Function](./tan.md)

---
