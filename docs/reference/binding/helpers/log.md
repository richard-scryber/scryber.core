---
layout: default
title: log
parent: Handlebars Helpers
parent_url: /reference/binding/helpers/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# {{log}} : Debug Logging Helper
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

Output debug messages to the trace log during template processing. Useful for debugging data binding and template logic. Does not render any visible content in the PDF.

**Based on:** [`<log>` element](../../components/log.md) for trace logging

## Syntax

```handlebars
{{log message}}
{{log message1 expression message2 level="info" category="debug"}}
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `message(s)` | String/Expression | Yes | One or more strings or expressions to log (concatenated) |
| `level` | String | No | Log level: `debug`, `info`, `warn`, `error` (default: `info`) |
| `category` | String | No | Category for filtering log messages (optional) |

---

## Log Levels

| Level | Description | Use Case |
|-------|-------------|----------|
| `debug` | Verbose debugging | Detailed tracing during development |
| `info` | Informational | General information (default) |
| `warn` | Warning | Potential issues that don't stop execution |
| `error` | Error | Significant problems worth noting |

---

## Examples

### Basic Logging

```handlebars
{{log "Processing user profile"}}
```

### Logging Values

```handlebars
{{#with model.user}}
  {{log "User: " name " (ID: " id ")"}}
{{/with}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    user = new {
        name = "John Doe",
        id = 12345
    }
};
```

**Log Output:**
```
User: John Doe (ID: 12345)
```

### With Log Level

```handlebars
{{log "Starting data processing" level="debug"}}

{{#if model.hasError}}
  {{log "Error processing order: " model.errorMessage level="error"}}
{{/if}}

{{log "Processing complete" level="info"}}
```

### With Category for Filtering

```handlebars
{{log "Validating user input" category="validation"}}

{{#if model.isValid}}
  {{log "Validation passed" category="validation" level="info"}}
{{else}}
  {{log "Validation failed: " model.validationErrors category="validation" level="warn"}}
{{/if}}
```

### Logging in Loops

```handlebars
{{#each model.orders}}
  {{log "Processing order #" this.orderNumber category="orders"}}

  {{#if this.total > 1000}}
    {{log "High-value order: $" this.total level="info" category="orders"}}
  {{/if}}
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    orders = new[] {
        new { orderNumber = "12345", total = 1250.00m },
        new { orderNumber = "12346", total = 45.99m },
        new { orderNumber = "12347", total = 2100.00m }
    }
};
```

**Log Output:**
```
Processing order #12345
High-value order: $1250
Processing order #12346
Processing order #12347
High-value order: $2100
```

### Logging with Expressions

```handlebars
{{#with model.user}}
  {{log if(isModerator, "User is a moderator", "User is not a moderator") level="debug" category="auth"}}

  {{log "User " name " has " concat(format(loginCount, 'N0'), " logins") category="stats"}}
{{/with}}
```

### Conditional Debugging

```handlebars
{{#each model.items}}
  {{#if this.price > 100}}
    {{log "Expensive item: " this.name " ($" this.price ")" level="warn" category="pricing"}}
  {{/if}}

  {{#if this.stock <= 0}}
    {{log "Out of stock: " this.name level="error" category="inventory"}}
  {{/if}}
{{/each}}
```

---

## Underlying Implementation

The `{{log}}` helper compiles to:

```xml
<log data-message="{{concatenated expressions}}"
     data-level="Verbose|Message|Warning|Error"
     data-category="category name" />
```

The log entry is written to the document's trace log during databinding and does not produce any visible output in the PDF.

---

## Notes

- Does not render any visible content in the PDF
- Messages are written to the document's trace log
- Multiple values are automatically concatenated with spaces
- Supports expressions (function calls, property access, etc.)
- Unknown parameters are silently ignored
- Use `doc.AppendTraceLog = true` to enable logging in C#
- View log output in CollectorTraceLog for unit testing
- Category helps filter log messages for specific features
- Log level affects visibility based on trace log configuration
- Useful for debugging complex conditional logic and iterations

---

## Enabling Trace Logging

To see log output, enable trace logging in your document:

```csharp
doc.AppendTraceLog = true;
doc.SaveAsPDF(output);

// Access log entries
var collector = doc.TraceLog as CollectorTraceLog;
foreach (var entry in collector)
{
    Console.WriteLine($"[{entry.Level}] {entry.Category}: {entry.Message}");
}
```

---

## See Also

- [#if Helper](./if.md)
- [#each Helper](./each.md)
- [#with Helper](./with.md)
- [Debugging Templates Guide](../../learning/02-data-binding/07_debugging.md)

---
