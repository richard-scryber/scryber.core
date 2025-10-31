---
layout: default
title: pi
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# pi() : Pi Constant
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

Get the mathematical constant π (pi), approximately 3.14159.

## Signature

```
pi()
```

---

## Parameters

None.

---

## Returns

**Type:** Double

The value of π (approximately 3.14159265358979).

---

## Examples

### Circle Calculations

```handlebars
<p>Circumference: {{format(2 * pi() * model.radius, '0.00')}}</p>
<p>Area: {{format(pi() * pow(model.radius, 2), '0.00')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    radius = 5
};
```

**Output:**
```html
<p>Circumference: 31.42</p>
<p>Area: 78.54</p>
```

### Angle Conversions

```handlebars
<p>90° = {{format(pi() / 2, '0.000')}} radians</p>
<p>180° = {{format(pi(), '0.000')}} radians</p>
<p>360° = {{format(2 * pi(), '0.000')}} radians</p>
```

**Output:**
```html
<p>90° = 1.571 radians</p>
<p>180° = 3.142 radians</p>
<p>360° = 6.283 radians</p>
```

### Trigonometric Calculations

```handlebars
<p>sin(π/6) = {{format(sin(pi() / 6), '0.000')}}</p>
<p>cos(π/3) = {{format(cos(pi() / 3), '0.000')}}</p>
```

**Output:**
```html
<p>sin(π/6) = 0.500</p>
<p>cos(π/3) = 0.500</p>
```

---

## Notes

- Returns constant value (no parameters)
- Value: 3.14159265358979...
- Used in circle, trigonometry, and wave calculations
- More accurate than using 3.14 or 22/7
- For Euler's number, use `e()`

---

## See Also

- [e Function](./e.md)
- [sin Function](./sin.md)
- [cos Function](./cos.md)
- [radians Function](./radians.md)
- [degrees Function](./degrees.md)

---
