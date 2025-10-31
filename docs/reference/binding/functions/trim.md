---
layout: default
title: trim
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# trim() : Remove Whitespace
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

Remove leading and trailing whitespace from a string.

## Signature

```
trim(str)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The string to trim |

---

## Returns

**Type:** String

The string with leading and trailing whitespace removed.

---

## Examples

### Remove Extra Spaces

```handlebars
<p>"{{trim(model.text)}}"</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "  Hello World  "
};
```

**Output:**
```html
<p>"Hello World"</p>
```

### Clean User Input

```handlebars
{{#each model.names}}
  <li>{{trim(this)}}</li>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    names = new[] { "  John  ", " Jane ", "Bob   " }
};
```

**Output:**
```html
<li>John</li>
<li>Jane</li>
<li>Bob</li>
```

### Normalize for Comparison

```handlebars
{{#if trim(model.status) == 'active'}}
  <span class="active">Active</span>
{{/if}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    status = " active "
};
```

**Output:**
```html
<span class="active">Active</span>
```

### Clean Multi-Line Text

```handlebars
<p>{{trim(model.description)}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    description = @"
        This is a description
        with extra whitespace
    "
};
```

**Output:**
```html
<p>This is a description
        with extra whitespace</p>
```

---

## Notes

- Removes spaces, tabs, newlines from both ends
- Does not affect whitespace in the middle of string
- Returns original string if no whitespace at ends
- For trailing whitespace only, use `trimEnd()`
- Does not modify the original value
- Useful for cleaning user input and data

---

## See Also

- [trimEnd Function](./trimEnd.md)
- [replace Function](./replace.md)
- [length Function](./length.md)

---
