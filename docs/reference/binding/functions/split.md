---
layout: default
title: split
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# split() : Split String into Array
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

Split a string into an array of substrings based on a separator.

## Signature

```
split(str, separator)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The string to split |
| `separator` | String | Yes | The separator to split on |

---

## Returns

**Type:** Array of strings

An array containing the split substrings.

---

## Examples

### Split CSV

```handlebars
<ul>
{{#each split(model.tags, ',')}}
  <li>{{trim(this)}}</li>
{{/each}}
</ul>
```

**Data:**
```csharp
doc.Params["model"] = new {
    tags = "javascript, html, css, react"
};
```

**Output:**
```html
<ul>
  <li>javascript</li>
  <li>html</li>
  <li>css</li>
  <li>react</li>
</ul>
```

### Split Full Name

```handlebars
<p>First Name: {{split(model.fullName, ' ')[0]}}</p>
<p>Last Name: {{split(model.fullName, ' ')[1]}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    fullName = "John Doe"
};
```

**Output:**
```html
<p>First Name: John</p>
<p>Last Name: Doe</p>
```

### Split Path

```handlebars
<ul class="breadcrumb">
{{#each split(model.path, '/')}}
  <li>{{this}}</li>
{{/each}}
</ul>
```

**Data:**
```csharp
doc.Params["model"] = new {
    path = "home/documents/reports/2024"
};
```

**Output:**
```html
<ul class="breadcrumb">
  <li>home</li>
  <li>documents</li>
  <li>reports</li>
  <li>2024</li>
</ul>
```

### Count Occurrences

```handlebars
<p>Words: {{length(split(model.sentence, ' '))}}</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    sentence = "This is a sample sentence"
};
```

**Output:**
```html
<p>Words: 5</p>
```

---

## Notes

- Returns array of substrings
- Empty strings included if separator appears consecutively
- If separator not found, returns array with single element (original string)
- Case-sensitive matching
- Use with `{{#each}}` to iterate results
- Use with `trim()` to clean up whitespace
- Opposite of `join()` function

---

## See Also

- [join Function](./join.md)
- [trim Function](./trim.md)
- [#each Helper](../helpers/each.md)
- [length Function](./length.md)

---
