---
layout: default
title: "?? (Null Coalescing)"
parent: Binding Operators
parent_url: /reference/binding/operators/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# ?? : Null Coalescing Operator
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

Provide a fallback value when an expression evaluates to null or undefined.

## Syntax

```handlebars
{{expression ?? fallbackValue}}
```

---

## Precedence

Priority level in expression evaluation (1 = highest, 10 = lowest): **8**

Evaluated after: `^`, `*`, `/`, `%`, `+`, `-`, `<`, `<=`, `>`, `>=`, `==`, `!=`

Evaluated before: `&&`, `||`

---

## Operands

| Position | Type | Description |
|----------|------|-------------|
| Left | Any | Expression that might be null |
| Right | Any | Fallback value if left is null |

---

## Returns

**Type:** Same as operands

Returns the left operand if it's not null, otherwise returns the right operand.

---

## Examples

### Default User Name

```handlebars
<h2>Welcome, {{model.user.name ?? 'Guest'}}!</h2>
```

**Data (with name):**
```csharp
doc.Params["model"] = new {
    user = new {
        name = "John Doe"
    }
};
```

**Output:**
```html
<h2>Welcome, John Doe!</h2>
```

**Data (without name):**
```csharp
doc.Params["model"] = new {
    user = new {
        name = (string)null
    }
};
```

**Output:**
```html
<h2>Welcome, Guest!</h2>
```

### Missing Description

```handlebars
{{#each model.products}}
  <div class="product">
    <h3>{{this.name}}</h3>
    <p>{{this.description ?? 'No description available'}}</p>
  </div>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    products = new[] {
        new { name = "Widget A", description = "High quality widget" },
        new { name = "Widget B", description = (string)null },
        new { name = "Widget C", description = "Premium widget" }
    }
};
```

**Output:**
```html
<div class="product">
  <h3>Widget A</h3>
  <p>High quality widget</p>
</div>
<div class="product">
  <h3>Widget B</h3>
  <p>No description available</p>
</div>
<div class="product">
  <h3>Widget C</h3>
  <p>Premium widget</p>
</div>
```

### Optional Contact Information

```handlebars
<div class="contact-info">
  <p>Email: {{model.email ?? 'Not provided'}}</p>
  <p>Phone: {{model.phone ?? 'Not provided'}}</p>
  <p>Address: {{model.address ?? 'Not provided'}}</p>
</div>
```

### Nested Property Access

```handlebars
<p>City: {{model.user.address.city ?? 'Unknown'}}</p>
```

**Data (nested null):**
```csharp
doc.Params["model"] = new {
    user = new {
        address = (object)null
    }
};
```

**Output:**
```html
<p>City: Unknown</p>
```

### Default Values in Lists

```handlebars
{{#each model.items}}
  <tr>
    <td>{{this.name}}</td>
    <td>{{this.category ?? 'Uncategorized'}}</td>
    <td>${{this.price ?? 0}}</td>
    <td>{{this.notes ?? '-'}}</td>
  </tr>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    items = new[] {
        new { name = "Item 1", category = "Electronics", price = 99.99m, notes = "In stock" },
        new { name = "Item 2", category = (string)null, price = (decimal?)null, notes = (string)null }
    }
};
```

**Output:**
```html
<tr>
  <td>Item 1</td>
  <td>Electronics</td>
  <td>$99.99</td>
  <td>In stock</td>
</tr>
<tr>
  <td>Item 2</td>
  <td>Uncategorized</td>
  <td>$0</td>
  <td>-</td>
</tr>
```

### Chaining Multiple Defaults

```handlebars
<p>Display Name: {{model.displayName ?? model.username ?? model.email ?? 'Anonymous'}}</p>
```

### Default Image

```handlebars
{{#each model.products}}
  <img src="{{this.imageUrl ?? '/images/placeholder.png'}}" alt="{{this.name}}" />
{{/each}}
```

---

## Notes

- Returns left operand if not null/undefined, otherwise returns right operand
- Very useful for providing default values
- Can chain multiple null coalescing operators
- Evaluates left-to-right when chained
- Does not check for empty strings - only null/undefined
- More concise than `{{#if value}}{{value}}{{else}}default{{/if}}`
- Similar to C# null-coalescing operator
- Use `ifError()` function for error handling instead of null checking

---

## Null vs Empty String

The `??` operator only checks for null/undefined, not empty strings:

```handlebars
<!-- Empty string is NOT null -->
{{model.name ?? 'Default'}}
<!-- If model.name = "", output is "" not "Default" -->

<!-- To handle empty strings, use a conditional -->
{{#if model.name}}{{model.name}}{{else}}Default{{/if}}
```

---

## See Also

- [Equality Operator](./equality.md)
- [Inequality Operator](./inequality.md)
- [#if Helper](../helpers/if.md)
- [#with Helper](../helpers/with.md)
- [ifError Function](../functions/ifError.md)

---
