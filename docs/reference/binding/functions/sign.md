---
layout: default
title: sign
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# sign() : Sign of Number
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

Get the sign of a number (-1, 0, or 1).

## Signature

```
sign(value)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Number | Yes | The number to check |

---

## Returns

**Type:** Integer

- `-1` if value is negative
- `0` if value is zero
- `1` if value is positive

---

## Examples

### Check Sign

```handlebars
{{#if sign(model.balance) > 0}}
  <p class="positive">Credit: ${{model.balance}}</p>
{{else if sign(model.balance) < 0}}
  <p class="negative">Debit: ${{abs(model.balance)}}</p>
{{else}}
  <p>Zero balance</p>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    balance = -150.00m
};
```

**Output:**
```html
<p class="negative">Debit: $150</p>
```

### Profit/Loss Indicator

```handlebars
{{#each model.accounts}}
  <tr>
    <td>{{this.name}}</td>
    <td>{{if(sign(this.change) >= 0, '▲', '▼')}} {{abs(this.change)}}%</td>
  </tr>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    accounts = new[] {
        new { name = "Account A", change = 5.2 },
        new { name = "Account B", change = -3.1 }
    }
};
```

**Output:**
```html
<tr>
  <td>Account A</td>
  <td>▲ 5.2%</td>
</tr>
<tr>
  <td>Account B</td>
  <td>▼ 3.1%</td>
</tr>
```

### Direction Indicator

```handlebars
<p>Trend: {{if(sign(model.change) > 0, 'Increasing', if(sign(model.change) < 0, 'Decreasing', 'Stable'))}}</p>
```

---

## Notes

- Returns integer: -1, 0, or 1
- Useful for determining direction
- Often used with `abs()` for magnitude
- Common in financial and trend displays

---

## See Also

- [abs Function](./abs.md)
- [#if Helper](../helpers/if.md)
- [if Function](./if.md)

---
