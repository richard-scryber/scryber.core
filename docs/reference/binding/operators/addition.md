---
layout: default
title: addition (+)
parent: Binding Operators
parent_url: /reference/binding/operators/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# + : Addition Operator
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

Adds two numeric values together.

## Syntax

```handlebars
{{operand1 + operand2}}
```

---

## Operands

| Position | Type | Description |
|----------|------|-------------|
| Left | Number | First value to add |
| Right | Number | Second value to add |

---

## Returns

**Type:** Number (same type as operands, or promoted type)

The sum of the two operands.

---

## Examples

### Basic Addition

```handlebars
{{5 + 3}}
<!-- Output: 8 -->
```

### With Variables

```handlebars
<p>Subtotal: ${{model.price}}</p>
<p>Tax: ${{model.tax}}</p>
<p>Total: ${{model.price + model.tax}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    price = 99.99m,
    tax = 8.00m
};
```

**Output:**
```html
<p>Subtotal: $99.99</p>
<p>Tax: $8.00</p>
<p>Total: $107.99</p>
```

### Multiple Additions

```handlebars
<p>Total: {{model.base + model.shipping + model.tax}}</p>
```

### With Formatting

```handlebars
<p>Grand Total: {{format(model.subtotal + model.tax, 'C2')}}</p>
```

### In #each Loop

```handlebars
{{#each model.items}}
  <p>Item {{add(@index, 1)}}: {{this.name}}</p>
{{/each}}
```

---

## Precedence

Priority level in expression evaluation: **5** (after *, /, %)

---

## Notes

- Works with `int`, `long`, `double`, `decimal` types
- Mixed types are automatically converted (e.g., `int + double` = `double`)
- Left-to-right associativity: `a + b + c` = `(a + b) + c`
- Use parentheses to control order: `(a + b) * c`

---

## See Also

- [Subtraction (-)](./subtraction.md)
- [Multiplication (*)](./multiplication.md)
- [Operator Precedence](./index.md#operator-precedence)

---
