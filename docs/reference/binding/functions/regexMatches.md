---
layout: default
title: regexMatches
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# regexMatches() : Find All Pattern Matches
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

Find all occurrences of a regular expression pattern in a string.

## Signature

```
regexMatches(str, pattern)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The string to search |
| `pattern` | String | Yes | The regular expression pattern |

---

## Returns

**Type:** Array of strings

An array containing all matches found.

---

## Examples

### Extract All Numbers

```handlebars
<ul>
{{#each regexMatches(model.text, '\\d+')}}
  <li>Number: {{this}}</li>
{{/each}}
</ul>
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "Order 123 contains 45 items at $67.89"
};
```

**Output:**
```html
<ul>
  <li>Number: 123</li>
  <li>Number: 45</li>
  <li>Number: 67</li>
  <li>Number: 89</li>
</ul>
```

### Extract Email Addresses

```handlebars
<h3>Found Emails:</h3>
{{#each regexMatches(model.text, '[\\w\\.]+@[\\w\\.]+')}}
  <p>{{this}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "Contact john@example.com or jane.doe@company.co.uk for details"
};
```

**Output:**
```html
<h3>Found Emails:</h3>
<p>john@example.com</p>
<p>jane.doe@company.co.uk</p>
```

### Extract Hashtags

```handlebars
<div class="tags">
{{#each regexMatches(model.post, '#\\w+')}}
  <span class="tag">{{this}}</span>
{{/each}}
</div>
```

**Data:**
```csharp
doc.Params["model"] = new {
    post = "Great day! #travel #adventure #nature #photography"
};
```

**Output:**
```html
<div class="tags">
  <span class="tag">#travel</span>
  <span class="tag">#adventure</span>
  <span class="tag">#nature</span>
  <span class="tag">#photography</span>
</div>
```

### Extract URLs

```handlebars
<h3>Links:</h3>
{{#each regexMatches(model.content, 'https?://[^\\s]+')}}
  <a href="{{this}}">{{this}}</a>
{{/each}}
```

### Count Matches

```handlebars
<p>Found {{length(regexMatches(model.text, '\\d'))}} digits</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "ABC123XYZ456"
};
```

**Output:**
```html
<p>Found 6 digits</p>
```

---

## Notes

- Uses .NET regex syntax
- Returns array of all matches (not just first)
- Returns empty array if no matches found
- Backslashes must be escaped: `\\d`, `\\w`, etc.
- Use with `{{#each}}` to iterate matches
- For testing if match exists, use `regexIsMatch()`
- For replacement, use `regexSwap()`

---

## Common Patterns

| Pattern | Description | Example |
|---------|-------------|---------|
| `\\d+` | One or more digits | `123`, `45` |
| `\\w+` | Word characters | `hello`, `test123` |
| `[A-Z]+` | Uppercase letters | `ABC`, `XYZ` |
| `#\\w+` | Hashtags | `#tag1`, `#tag2` |
| `@\\w+` | Mentions | `@user1`, `@user2` |
| `\\$\\d+\\.\\d{2}` | Currency | `$19.99`, `$100.00` |

---

## See Also

- [regexIsMatch Function](./regexIsMatch.md)
- [regexSwap Function](./regexSwap.md)
- [#each Helper](../helpers/each.md)
- [length Function](./length.md)

---
