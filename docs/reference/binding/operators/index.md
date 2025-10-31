---
layout: default
title: Binding Operators
parent: Data Binding Reference
parent_url: /reference/binding/
grand_parent: Reference
grand_parent_url: /reference/
has_children: true
has_toc: false
---

# Binding Operators Reference
{: .no_toc }

Operators for building expressions in Scryber data binding.

---

## Overview

Binding operators allow you to perform calculations, comparisons, and logical operations within binding expressions. All operators follow standard precedence rules and can be combined in complex expressions.

---

## Arithmetic Operators

Perform mathematical calculations on numeric values.

| Operator | Description | Precedence | Example |
|----------|-------------|------------|---------|
| [+](./addition.md) | Addition | 5 | `{{model.price + model.tax}}` |
| [-](./subtraction.md) | Subtraction | 5 | `{{model.total - model.discount}}` |
| [*](./multiplication.md) | Multiplication | 4 | `{{model.quantity * model.price}}` |
| [/](./division.md) | Division | 4 | `{{model.total / model.count}}` |
| [%](./modulus.md) | Modulus (remainder) | 4 | `{{model.index % 2}}` |
| [^](./power.md) | Exponentiation | 3 | `{{model.base ^ model.exponent}}` |

---

## Comparison Operators

Compare values and return boolean results. Used primarily in `{{#if}}` conditionals.

| Operator | Description | Precedence | Example |
|----------|-------------|------------|---------|
| [==](./equality.md) | Equality | 7 | `{{#if model.status == 'active'}}` |
| [!=](./inequality.md) | Inequality | 7 | `{{#if model.count != 0}}` |
| [<](./lessthan.md) | Less than | 6 | `{{#if model.age < 18}}` |
| [<=](./lessorequal.md) | Less than or equal | 6 | `{{#if model.score <= 100}}` |
| [>](./greaterthan.md) | Greater than | 6 | `{{#if model.value > 0}}` |
| [>=](./greaterorequal.md) | Greater than or equal | 6 | `{{#if model.age >= 18}}` |

---

## Logical Operators

Combine boolean expressions.

| Operator | Description | Precedence | Example |
|----------|-------------|------------|---------|
| [&&](./and.md) | Logical AND | 9 | `{{#if model.age >= 18 && model.hasLicense}}` |
| [\|\|](./or.md) | Logical OR | 10 | `{{#if model.isAdmin \|\| model.isModerator}}` |

---

## Null Coalescing Operator

Handle null or undefined values with fallback.

| Operator | Description | Precedence | Example |
|----------|-------------|------------|---------|
| [??](./nullcoalesce.md) | Null coalescing | 8 | `{{model.name ?? 'Unknown'}}` |

---

## Operator Precedence

Operators are evaluated in order of precedence (1 = highest, 10 = lowest):

1. Parentheses `()`
2. Member access `.`
3. Exponentiation `^`
4. Multiplication, Division, Modulus `*`, `/`, `%`
5. Addition, Subtraction `+`, `-`
6. Comparison `<`, `<=`, `>`, `>=`
7. Equality `==`, `!=`
8. Null coalescing `??`
9. Logical AND `&&`
10. Logical OR `||`

Use parentheses to override precedence:
```handlebars
{{(model.price + model.tax) * model.quantity}}
```

---

## Common Patterns

### Complex Arithmetic

```handlebars
<p>Subtotal: ${{model.quantity * model.price}}</p>
<p>Tax: ${{(model.quantity * model.price) * model.taxRate}}</p>
<p>Total: ${{(model.quantity * model.price) * (1 + model.taxRate)}}</p>
```

### Multiple Comparisons

```handlebars
{{#if model.age >= 13 && model.age < 20}}
  <span class="teen">Teenager</span>
{{/if}}
```

### Null Safety

```handlebars
<h2>{{model.user.name ?? 'Guest User'}}</h2>
<p>{{model.description ?? 'No description available'}}</p>
```

### Conditional Classes

```handlebars
<div class="item {{#if model.stock > 0}}in-stock{{else}}out-of-stock{{/if}}">
  <p>Price: ${{model.price}}</p>
  {{#if model.discount > 0}}
    <p class="sale">Save: ${{model.price * model.discount}}</p>
  {{/if}}
</div>
```

---

## See Also

- [Expression Functions](../functions/) - Built-in functions for data manipulation
- [Handlebars Helpers](../helpers/) - Control flow and context management
- [Expressions Guide](../../learning/02-data-binding/02_expressions.md) - Complete guide to expressions

---
