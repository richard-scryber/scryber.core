---
layout: default
title: ceiling
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# ceiling() : Round Up to Integer
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

Round a number up to the nearest integer.

## Signature

```
ceiling(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Number | Yes | The number to round up |

---

## Returns

**Type:** Integer

The smallest integer greater than or equal to the value.

---

## Examples

### Round Up Prices

```handlebars
<p>Rounded Price: ${{ceiling(model.price)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    price = 19.01m
};
```

**Output:**
```html
<p>Rounded Price: $20</p>
```

### Calculate Pages Needed

```handlebars
<p>Pages required: {{ceiling(model.items / model.itemsPerPage)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    items = 47,
    itemsPerPage = 10
};
```

**Output:**
```html
<p>Pages required: 5</p>
```

### Minimum Purchase Quantity

```handlebars
<p>Boxes needed: {{ceiling(model.quantity / model.boxSize)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    quantity = 38,
    boxSize = 12
};
```

**Output:**
```html
<p>Boxes needed: 4</p>
```

### Always Round Up

```handlebars
{{#each model.values}}
  <li>{{this}} → {{ceiling(this)}}</li>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    values = new[] { 1.1, 2.5, 3.9, 4.0 }
};
```

**Output:**
```html
<li>1.1 → 2</li>
<li>2.5 → 3</li>
<li>3.9 → 4</li>
<li>4.0 → 4</li>
```

---

## Notes

- Always rounds up (towards positive infinity)
- Returns integer type
- For down rounding, use `floor()`
- For nearest rounding, use `round()`
- For removing decimals without rounding, use `truncate()`
- Negative numbers round towards zero: `-1.5` → `-1`

---

## See Also

- [floor Function](./floor.md)
- [round Function](./round.md)
- [truncate Function](./truncate.md)
- [Division Operator](../operators/division.md)

---
