---
layout: default
title: in
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# in() : Check Value in Collection
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

Check if a value exists in a collection or array. Returns true if the value is found, false otherwise.

## Signature

```
in(value, collection)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `value` | Any | Yes | The value to search for |
| `collection` | Array/Collection | Yes | The collection to search in |

---

## Returns

**Type:** Boolean

Returns `true` if the value exists in the collection, `false` otherwise.

---

## Examples

### Simple Membership Check

```handlebars
<p>Has premium: {{in('premium', model.features)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    features = new[] { "basic", "premium", "analytics" }
};
```

**Output:**
```html
<p>Has premium: true</p>
```

### Conditional Display Based on Membership

```handlebars
{{#each model.users}}
  <p>{{this.name}}:
  {{#if (in(this.role, model.adminRoles))}}
    Administrator
  {{else}}
    User
  {{/if}}
  </p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    adminRoles = new[] { "admin", "superadmin", "moderator" },
    users = new[] {
        new { name = "Alice", role = "admin" },
        new { name = "Bob", role = "user" },
        new { name = "Charlie", role = "moderator" }
    }
};
```

**Output:**
```html
<p>Alice: Administrator</p>
<p>Bob: User</p>
<p>Charlie: Administrator</p>
```

### Status Badge Display

```handlebars
<p>Status:
{{#if (in(model.status, model.activeStatuses))}}
  <span style="color: green;">Active</span>
{{else if (in(model.status, model.warningStatuses))}}
  <span style="color: orange;">Warning</span>
{{else}}
  <span style="color: red;">Error</span>
{{/if}}
</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    status = "pending",
    activeStatuses = new[] { "active", "running", "ok" },
    warningStatuses = new[] { "pending", "degraded", "slow" }
};
```

**Output:**
```html
<p>Status: <span style="color: orange;">Warning</span></p>
```

### Access Control

```handlebars
{{#each model.features}}
  {{#if (in(this.id, model.user.permissions))}}
    <p>{{this.name}}: Enabled</p>
  {{/if}}
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    user = new {
        permissions = new[] { "read", "write", "delete" }
    },
    features = new[] {
        new { id = "read", name = "View Data" },
        new { id = "write", name = "Edit Data" },
        new { id = "admin", name = "Admin Panel" }
    }
};
```

**Output:**
```html
<p>View Data: Enabled</p>
<p>Edit Data: Enabled</p>
```

### Category Filter

```handlebars
<h3>Priority Items</h3>
{{#each model.items}}
  {{#if (in(this.category, model.priorityCategories))}}
    <p>{{this.name}} ({{this.category}})</p>
  {{/if}}
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    priorityCategories = new[] { "urgent", "critical", "high" },
    items = new[] {
        new { name = "Bug Fix", category = "urgent" },
        new { name = "Feature Request", category = "low" },
        new { name = "Security Patch", category = "critical" }
    }
};
```

**Output:**
```html
<h3>Priority Items</h3>
<p>Bug Fix (urgent)</p>
<p>Security Patch (critical)</p>
```

---

## Notes

- Case-sensitive comparison by default
- Works with arrays and enumerable collections
- Returns false if collection is null or empty
- Value comparison uses equality (==)
- Useful for:
  - Role-based access checks
  - Feature flag evaluation
  - Status filtering
  - Whitelist/blacklist checks
  - Category membership
- Performance: O(n) linear search through collection
- For string containment within a single string, use `contains()` instead
- Can check numeric values, strings, or any comparable type
- Does not support wildcard or pattern matching

---

## See Also

- [contains Function](./contains.md)
- [if Function](./if.md)
- [count Function](./count.md)
- [#if Helper](../helpers/if.md)

---
