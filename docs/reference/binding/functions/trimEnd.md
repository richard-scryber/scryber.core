---
layout: default
title: trimEnd
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# trimEnd() : Remove Trailing Whitespace
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

Remove trailing whitespace from the end of a string.

## Signature

```
trimEnd(str)
```

---

## Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `str` | String | Yes | The string to trim |

---

## Returns

**Type:** String

The string with trailing whitespace removed.

---

## Examples

### Remove Trailing Spaces

```handlebars
<p>"{{trimEnd(model.text)}}"</p>
```

**Data:**
```csharp
doc.Params["model"] = new {
    text = "Hello World   "
};
```

**Output:**
```html
<p>"Hello World"</p>
```

### Clean Line Endings

```handlebars
{{#each model.lines}}
  <p>{{trimEnd(this)}}</p>
{{/each}}
```

**Data:**
```csharp
doc.Params["model"] = new {
    lines = new[] {
        "Line 1   ",
        "Line 2\t\t",
        "Line 3\n"
    }
};
```

**Output:**
```html
<p>Line 1</p>
<p>Line 2</p>
<p>Line 3</p>
```

### Preserve Leading Spaces

```handlebars
<pre>{{trimEnd(model.code)}}</pre>
```

**Data:**
```csharp
doc.Params["model"] = new {
    code = "    function test() {   "
};
```

**Output:**
```html
<pre>    function test() {</pre>
```

---

## Notes

- Only removes whitespace from the end of string
- Preserves leading whitespace
- Removes spaces, tabs, newlines, carriage returns
- Returns original string if no trailing whitespace
- For both ends, use `trim()`
- Does not modify the original value

---

## See Also

- [trim Function](./trim.md)
- [replace Function](./replace.md)
- [length Function](./length.md)

---
