---
layout: default
title: truncate
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# truncate() : Remove Decimal Portion
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

Remove the decimal portion of a number without rounding.

## Signature

```
truncate(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Number | Yes | The number to truncate |

---

## Returns

**Type:** Integer

The integer portion of the number.

---

## Examples

### Remove Decimals

```handlebars
<p>Whole number: {{truncate(model.value)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    value = 19.99m
};
```

**Output:**
```html
<p>Whole number: 19</p>
```

### Extract Integer Part

```handlebars
<p>Dollars: ${{truncate(model.price)}}</p>
<p>Cents: {{round((model.price - truncate(model.price)) * 100)}}¢</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    price = 19.75m
};
```

**Output:**
```html
<p>Dollars: $19</p>
<p>Cents: 75¢</p>
```

### No Rounding Behavior

```handlebars
{{#each model.values}}
  <li>{{this}} → {{truncate(this)}}</li>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    values = new[] { 1.1, 2.5, 3.9, 4.0, -2.8 }
};
```

**Output:**
```html
<li>1.1 → 1</li>
<li>2.5 → 2</li>
<li>3.9 → 3</li>
<li>4.0 → 4</li>
<li>-2.8 → -2</li>
```

---

## Notes

- Simply removes decimal portion (no rounding)
- Returns integer type
- Different from `floor()` for negative numbers
- `truncate(-2.8)` = -2, but `floor(-2.8)` = -3
- For rounding, use `round()`, `ceiling()`, or `floor()`
- Same as `int()` function behavior

---

## See Also

- [int Function](./int.md)
- [floor Function](./floor.md)
- [ceiling Function](./ceiling.md)
- [round Function](./round.md)

---
