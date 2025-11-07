---
layout: default
title: sin
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# sin() : Sine Function
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

Calculate the sine of an angle (in radians).

## Signature

```
sin(angleInRadians)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `angleInRadians` | Number | Yes | The angle in radians |

---

## Returns

**Type:** Double

The sine of the angle (value between -1 and 1).

---

## Examples

### Basic Sine

```handlebars
<p>sin(π/2) = {{format(sin(pi() / 2), '0.000')}}</p>
```

**Output:**
```html
<p>sin(π/2) = 1.000</p>
```

### Convert Degrees to Radians First

```handlebars
<p>sin(30°) = {{format(sin(radians(30)), '0.000')}}</p>
<p>sin(45°) = {{format(sin(radians(45)), '0.000')}}</p>
<p>sin(90°) = {{format(sin(radians(90)), '0.000')}}</p>
```

**Output:**
```html
<p>sin(30°) = 0.500</p>
<p>sin(45°) = 0.707</p>
<p>sin(90°) = 1.000</p>
```

### Calculate Wave Position

```handlebars
<p>Y = {{format(model.amplitude * sin(model.angle), '0.00')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    amplitude = 10,
    angle = Math.PI / 4  // 45 degrees
};
```

**Output:**
```html
<p>Y = 7.07</p>
```

---

## Notes

- Input must be in radians (not degrees)
- Returns value between -1 and 1
- For degrees, use: `sin(radians(degrees))`
- Common values:
  - sin(0) = 0
  - sin(π/6) = 0.5
  - sin(π/4) ≈ 0.707
  - sin(π/2) = 1

---

## See Also

- [cos Function](./cos.md)
- [tan Function](./tan.md)
- [asin Function](./asin.md)
- [radians Function](./radians.md)
- [pi Function](./pi.md)

---
