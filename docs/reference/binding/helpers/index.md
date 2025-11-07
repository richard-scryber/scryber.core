---
layout: default
title: Handlebars Helpers
parent: Data Binding Reference
parent_url: /reference/binding/
grand_parent: Reference
grand_parent_url: /reference/
has_children: true
has_toc: false
---

# Handlebars Helpers Reference
{: .no_toc }

Handlebars-style block helpers for control flow and context management in Scryber templates.

---

## Overview

Handlebars helpers provide familiar syntax for common template patterns like iteration, conditionals, and context switching. They compile to native Scryber XML elements at parse time.

---

## Block Helpers

These helpers use `{{#helper}}...{{/helper}}` syntax to wrap content.

| Helper | Description | Special Variables |
|--------|-------------|-------------------|
| [{{#each}}](./each.md) | Iterate over arrays and collections | `@index`, `@first`, `@last` |
| [{{#with}}](./with.md) | Switch data context to a specific object | `this`, `../` |
| [{{#if}}](./if.md) | Conditional rendering based on expressions | - |

---

## Branch Helpers

These helpers work within block helpers to create conditional branches.

| Helper | Description | Used With |
|--------|-------------|-----------|
| [{{else if}}](./elseif.md) | Alternative condition branch | `{{#if}}` |
| [{{else}}](./else.md) | Fallback branch when conditions are false | `{{#if}}`, `{{#each}}`, `{{#with}}` |

---

## Utility Helpers

| Helper | Description | Usage |
|--------|-------------|-------|
| [{{log}}](./log.md) | Debug output to console/trace | `{{log model.value level="debug"}}` |

---

## Common Patterns

### Iteration with Conditionals

```handlebars
{{#each model.items}}
  <div class="item {{if(@first, 'first', '')}}">
    {{#if this.isActive}}
      <span class="active">{{this.name}}</span>
    {{else}}
      <span class="inactive">{{this.name}}</span>
    {{/if}}
  </div>
{{/each}}
```

### Context Switching with Parent Access

```handlebars
{{#with model.order}}
  <h2>Order #{{orderNumber}}</h2>

  {{#each items}}
    <div>{{this.name}} - Order #{{../orderNumber}}</div>
  {{/each}}
{{/with}}
```

### Multiple Conditions

```handlebars
{{#if model.score >= 90}}
  <span class="grade-a">Excellent</span>
{{else if model.score >= 80}}
  <span class="grade-b">Good</span>
{{else if model.score >= 70}}
  <span class="grade-c">Average</span>
{{else}}
  <span class="grade-f">Needs Improvement</span>
{{/if}}
```

---

## See Also

- [Expression Functions](../functions/) - Built-in functions for data manipulation
- [Binding Operators](../operators/) - Mathematical, comparison, and logical operators
- [Data Binding Guide](../../learning/02-data-binding/01_data_binding_basics.md) - Complete guide to data binding

---
