---
layout: default
title: join
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# join() : Join Array with Separator
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

Join array elements into a single string with a separator.

## Signature

```
join(array, separator)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `array` | Array | Yes | The array to join |
| `separator` | String | Yes | The separator to use between elements |

---

## Returns

**Type:** String

A single string with all array elements joined by the separator.

---

## Examples

### Comma-Separated List

```handlebars
<p>Tags: {{join(model.tags, ', ')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    tags = new[] { "JavaScript", "HTML", "CSS", "React" }
};
```

**Output:**
```html
<p>Tags: JavaScript, HTML, CSS, React</p>
```

### Pipe-Separated Values

```handlebars
<p>Categories: {{join(model.categories, ' | ')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    categories = new[] { "Electronics", "Computers", "Accessories" }
};
```

**Output:**
```html
<p>Categories: Electronics | Computers | Accessories</p>
```

### Create Full Name

```handlebars
<p>Name: {{join(model.nameParts, ' ')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    nameParts = new[] { "John", "Q", "Doe" }
};
```

**Output:**
```html
<p>Name: John Q Doe</p>
```

### Bullet List

```handlebars
<p>Features: {{join(model.features, ' • ')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    features = new[] { "Fast", "Reliable", "Secure", "Scalable" }
};
```

**Output:**
```html
<p>Features: Fast • Reliable • Secure • Scalable</p>
```

### Create Path

```handlebars
<p>Path: {{join(model.pathParts, '/')}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    pathParts = new[] { "home", "user", "documents", "file.txt" }
};
```

**Output:**
```html
<p>Path: home/user/documents/file.txt</p>
```

---

## Notes

- Works with string arrays
- Non-string array elements are converted to strings
- Empty arrays return empty string
- Null elements are treated as empty strings
- For collection of objects, use with `collect()` to extract property first
- Alternative to manually iterating with `{{#each}}`

---

## See Also

- [concat Function](./concat.md)
- [split Function](./split.md)
- [collect Function](./collect.md)
- [#each Helper](../helpers/each.md)

---
