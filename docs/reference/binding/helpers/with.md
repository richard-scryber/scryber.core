---
layout: default
title: with
parent: Handlebars Helpers
parent_url: /reference/binding/helpers/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# {{#with}} : Context Switching Helper
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

Change the data context to a specific object, simplifying property access within the block. Properties can be accessed directly without the object prefix.

**Based on:** [`<template>` element](../../htmltags/template.md) with context binding and the [`With` component](../../components/with.md)

## Syntax

```handlebars
{{#with object}}
  <!-- Properties accessed directly -->
{{else}}
  <!-- Optional fallback if object is null -->
{{/with}}
```

**With Alias:**
```handlebars
{{#with object as | alias |}}
  <!-- Use alias to reference the object -->
{{/with}}
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `object` | Object | Yes | The object to set as the current context |
| `alias` | Identifier | No | Optional variable name for the object |

---

## Special Variables

| Variable | Description |
|----------|-------------|
| `this` / `.` | Reference to the current context object |
| `../` | Access parent context |

---

## Examples

### Basic Context Switching

```handlebars
{{#with model.user}}
  <h2>{{name}}</h2>
  <p>Email: {{email}}</p>
  <p>Age: {{age}}</p>
{{/with}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    user = new {
        name = "John Doe",
        email = "john@example.com",
        age = 30
    }
};
```

**Output:**
```html
<h2>John Doe</h2>
<p>Email: john@example.com</p>
<p>Age: 30</p>
```

### With Else Block

```handlebars
{{#with model.user}}
  <div class="user-profile">
    <h2>{{name}}</h2>
  </div>
{{else}}
  <div class="no-user">
    <p>No user profile available</p>
  </div>
{{/with}}
```

### Accessing Parent Properties

```handlebars
<h1>Order #{{model.orderNumber}}</h1>

{{#with model.customer}}
  <div class="customer">
    <h2>Customer: {{name}}</h2>
    <p>Order Number: {{../orderNumber}}</p>
  </div>
{{/with}}
```

### Using Alias Syntax

```handlebars
{{#with model.currentUser as | user |}}
  <div>
    <p>Logged in as: {{user.name}}</p>
    <p>Role: {{user.role}}</p>
  </div>
{{/with}}
```

### Nested With Blocks

```handlebars
{{#with model.company}}
  <h1>{{name}}</h1>

  {{#with address}}
    <address>
      {{street}}<br>
      {{city}}, {{state}} {{zip}}<br>
      Company: {{../../name}}
    </address>
  {{/with}}
{{/with}}
```

---

## Underlying Implementation

The `{{#with}}` helper compiles to the following Scryber template structure:

```xml
<template data-bind="{{object}}">
  <!-- Main content with direct property access -->
  <template data-visible="false">
    <!-- Optional else content if object is null -->
  </template>
</template>
```

For the alias syntax `{{#with object as | alias |}}`, it creates a variable binding that makes the object available under the specified name.

---

## Notes

- Empty/null objects trigger the `{{else}}` block
- Use `{{.}}` to reference the entire context object
- Use `{{../property}}` to go up one level
- Use `{{../../property}}` to go up two levels
- Alias names must be valid identifiers
- Context switching simplifies deeply nested property access

---

## See Also

- [#each Helper](./each.md)
- [#if Helper](./if.md)
- [Context & Scope Guide](../../learning/02-data-binding/06_context_scope.md)

---
