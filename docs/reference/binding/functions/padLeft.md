---
layout: default
title: padLeft
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# padLeft() : Pad String on Left
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

Pad a string on the left side to reach a specified length.

## Signature

```
padLeft(str, totalLength, padChar?)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The string to pad |
| `totalLength` | Number | Yes | The desired total length |
| `padChar` | String | No | The padding character (default: space) |

---

## Returns

**Type:** String

The padded string.

---

## Examples

### Zero-Padded Numbers

```handlebars
<p>Order #{{padLeft(format(model.orderNumber, '0'), 8, '0')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    orderNumber = 12345
};
```

**Output:**
```html
<p>Order #00012345</p>
```

### Invoice Numbers

```handlebars
{{#each model.invoices}}
  <div>INV-{{padLeft(format(this.id, '0'), 6, '0')}}</div>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    invoices = new[] {
        new { id = 1 },
        new { id = 42 },
        new { id = 999 }
    }
};
```

**Output:**
```html
<div>INV-000001</div>
<div>INV-000042</div>
<div>INV-000999</div>
```

### Align Text

```handlebars
<pre>
{{#each model.items}}
{{padLeft(this.name, 20, ' ')}} | ${{format(this.price, '0.00')}}
{{/each}}
</pre>
```

**Data:**
```csharp
doc.Params["model"] = new {
    items = new[] {
        new { name = "Widget", price = 19.99m },
        new { name = "Gadget", price = 29.99m },
        new { name = "Tool", price = 9.99m }
    }
};
```

**Output:**
```html
<pre>
              Widget | $19.99
              Gadget | $29.99
                Tool | $9.99
</pre>
```

### Custom Padding

```handlebars
<p>{{padLeft(model.code, 10, '-')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    code = "ABC"
};
```

**Output:**
```html
<p>-------ABC</p>
```

---

## Notes

- Pads on the left (adds characters before the string)
- If string is already longer than totalLength, returns unchanged
- Default padding character is space
- Pad character must be a single character
- Useful for formatting numbers and aligning text
- For right padding, use `padRight()`

---

## See Also

- [padRight Function](./padRight.md)
- [format Function](./format.md)
- [concat Function](./concat.md)

---
