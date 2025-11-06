---
layout: default
title: random
parent: Expression Functions
parent_url: /reference/binding/functions/
grand_parent: Data Binding Reference
grand_parent_url: /reference/binding/
has_children: false
has_toc: false
---

# random() : Random Number
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

Generate a random number between 0 and 1.

## Signature

```
random()
```

---

## Parameters

None.

---

## Returns

**Type:** Double

A random value between 0.0 (inclusive) and 1.0 (exclusive).

---

## Examples

### Basic Random

```handlebars
<p>Random: {{format(random(), '0.000')}}</p>
```

**Output (example):**
```html
<p>Random: 0.742</p>
```

### Random Integer Range

```handlebars
<!-- Random integer from 1 to 10 -->
<p>Random (1-10): {{floor(random() * 10) + 1}}</p>
```

### Random Selection

```handlebars
<!-- Randomly select from array -->
<p>Selected: {{model.items[floor(random() * length(model.items))]}}</p>
```

### Random Percentage

```handlebars
<p>Random percentage: {{format(random() * 100, '0.0')}}%</p>
```

---

## Notes

- Returns value ≥ 0 and < 1
- Different value each time (non-deterministic)
- **Warning**: PDFs are static documents - random values are generated once at template processing time
- Not cryptographically secure
- For range [min, max]: `random() * (max - min) + min`
- For integer range: `floor(random() * (max - min + 1)) + min`

---

## Important

PDF documents are static - random values are calculated during document generation and remain fixed in the output. If you need consistent values across multiple document generations, provide them in your model data instead of using `random()`.

---

## See Also

- [floor Function](./floor.md)
- [round Function](./round.md)
- [Multiplication Operator](../operators/multiplication.md)

---
