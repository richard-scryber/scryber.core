---
layout: default
title: round
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# round() : Round to Nearest
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

Round a number to the nearest integer or specified decimal places.

## Signature

```
round(value, decimals?)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Number | Yes | The number to round |
| `decimals` | Number | No | Number of decimal places (default: 0) |

---

## Returns

**Type:** Number

The rounded value.

---

## Examples

### Round to Integer

```handlebars
<p>Rounded: {{round(model.value)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    value = 19.6
};
```

**Output:**
```html
<p>Rounded: 20</p>
```

### Round to 2 Decimal Places

```handlebars
<p>Price: ${{round(model.price, 2)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    price = 19.995m
};
```

**Output:**
```html
<p>Price: $20.00</p>
```

### Round Percentage

```handlebars
<p>{{round((model.completed / model.total) * 100, 1)}}% complete</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    completed = 17,
    total = 25
};
```

**Output:**
```html
<p>68.0% complete</p>
```

### Multiple Decimal Places

```handlebars
{{#each model.measurements}}
  <li>{{this.name}}: {{round(this.value, 3)}} mm</li>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    measurements = new[] {
        new { name = "Width", value = 12.3456 },
        new { name = "Height", value = 8.7654 }
    }
};
```

**Output:**
```html
<li>Width: 12.346 mm</li>
<li>Height: 8.765 mm</li>
```

---

## Notes

- Uses standard rounding (0.5 rounds up)
- Default is 0 decimal places (rounds to integer)
- Specify decimal places for precision
- For always rounding up, use `ceiling()`
- For always rounding down, use `floor()`
- For truncation without rounding, use `truncate()`

---

## See Also

- [ceiling Function](./ceiling.md)
- [floor Function](./floor.md)
- [truncate Function](./truncate.md)
- [format Function](./format.md)

---
