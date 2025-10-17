---
layout: default
title: CSS Specificity
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# CSS Specificity

CSS specificity determines which styles are applied when multiple rules target the same element. In Scryber, specificity is calculated based on selector type, ancestor depth, and combinator type.

## Specificity Values

Scryber uses the following base specificity values:

| Selector Type | Base Value |
|--------------|------------|
| Element | 1 |
| Class (single) | 2 |
| Class (double) | 3 |
| Class (triple) | 4 |
| ID | 5 |

---

## Ancestor Depth Multipliers

Specificity increases based on ancestor depth in the selector:

| Depth Level | Descendant (space) | Direct Parent (>) |
|-------------|-------------------|-------------------|
| 0 (no ancestor) | 1 | 1 |
| 1 ancestor | 10 | 20 |
| 2 ancestors | 100 | 200 |
| 3 ancestors | 1000 | 2000 |

Direct parent combinator (>) has **twice the specificity** of descendant combinator (space).

---

## Specificity Calculation

The total specificity is calculated as:

```
Base Value × Depth Multiplier
```

---

## Specificity Examples

### Single Selectors (no ancestors)

```css
p { }           /* Specificity: 1 */
.class { }      /* Specificity: 2 */
.class1.class2 { }  /* Specificity: 3 */
#id { }         /* Specificity: 5 */
* { }           /* Specificity: 1 */
```

### With One Ancestor

```css
div p { }       /* 1 × 10 = 10 */
div > p { }     /* 1 × 20 = 20 */
.container p { }    /* 2 × 10 = 20 */
#main p { }     /* 5 × 10 = 50 */
```

### With Multiple Ancestors

```css
div section p { }       /* 1 × 100 = 100 */
div > section > p { }   /* 1 × 200 = 200 */
.container .content p { }   /* 2 × 100 = 200 */
#main .content p { }    /* 5 × 100 = 500 */
```

---

## Specificity Rules

1. **Higher specificity wins**: When multiple rules match, the one with higher specificity is applied
2. **Last rule wins**: When specificity is equal, the last declared rule wins
3. **Direct parent is more specific**: `>` combinator has 2× specificity of space combinator
4. **IDs are most specific**: ID selectors have the highest base value
5. **Multiple classes increase specificity**: `.class1.class2` is more specific than `.class1`

---

## Notes

- Inline styles (if supported) would have even higher specificity
- `!important` declarations override normal specificity (use sparingly)
- Pseudo-classes (:hover, :before, :after) add to the base selector's specificity
- Universal selector (*) has minimal specificity but still counts
- Specificity is calculated per selector in a comma-separated list

---

## Examples

### Example 1: Element vs. Class

```html
<style>
    p {
        color: blue;  /* Specificity: 1 */
    }

    .highlight {
        color: red;   /* Specificity: 2 - WINS */
    }
</style>
<body>
    <p class="highlight">This text is red.</p>
</body>
```

### Example 2: Class vs. ID

```html
<style>
    .special {
        font-size: 12pt;  /* Specificity: 2 */
    }

    #unique {
        font-size: 18pt;  /* Specificity: 5 - WINS */
    }
</style>
<body>
    <p id="unique" class="special">This is 18pt.</p>
</body>
```

### Example 3: Descendant depth matters

```html
<style>
    p {
        color: blue;  /* Specificity: 1 */
    }

    div p {
        color: green; /* Specificity: 10 - WINS */
    }

    div section p {
        color: red;   /* Specificity: 100 - WINS if nested deeper */
    }
</style>
<body>
    <p>Blue text</p>
    <div>
        <p>Green text</p>
        <section>
            <p>Red text</p>
        </section>
    </div>
</body>
```

### Example 4: Direct parent vs. Descendant

```html
<style>
    div p {
        color: blue;  /* Specificity: 10 */
    }

    div > p {
        color: red;   /* Specificity: 20 - WINS */
    }
</style>
<body>
    <div>
        <p>Red text (direct child)</p>
        <section>
            <p>Blue text (not direct child of div)</p>
        </section>
    </div>
</body>
```

### Example 5: Multiple classes

```html
<style>
    .text {
        font-size: 12pt;  /* Specificity: 2 */
    }

    .large.bold {
        font-size: 18pt;  /* Specificity: 3 - WINS */
    }
</style>
<body>
    <p class="text">12pt text</p>
    <p class="large bold">18pt text</p>
</body>
```

### Example 6: Complex selector comparison

```html
<style>
    .container p {
        color: blue;      /* Specificity: 20 */
    }

    div > .content p {
        color: green;     /* Specificity: 40 */
    }

    #main p {
        color: red;       /* Specificity: 50 - WINS */
    }
</style>
<body>
    <div id="main" class="container">
        <div class="content">
            <p>Red text (ID wins)</p>
        </div>
    </div>
</body>
```

### Example 7: Pseudo-class specificity

```html
<style>
    a {
        color: blue;      /* Specificity: 1 */
    }

    a:hover {
        color: red;       /* Specificity: 1 + hover state */
    }

    .link:hover {
        color: green;     /* Specificity: 2 + hover state - WINS */
    }
</style>
<body>
    <a href="#" class="link">Hover me (turns green)</a>
</body>
```

### Example 8: Last rule wins with equal specificity

```html
<style>
    .highlight {
        color: blue;      /* Specificity: 2 */
    }

    .special {
        color: red;       /* Specificity: 2 - WINS (declared last) */
    }
</style>
<body>
    <p class="highlight special">Red text</p>
</body>
```

### Example 9: Nested context

```html
<style>
    article p {
        font-size: 12pt;  /* Specificity: 10 */
    }

    article section p {
        font-size: 14pt;  /* Specificity: 100 */
    }

    article > section > p {
        font-size: 16pt;  /* Specificity: 200 - WINS */
    }
</style>
<body>
    <article>
        <section>
            <p>16pt text (most specific)</p>
        </section>
    </article>
</body>
```

### Example 10: Real-world cascade

```html
<style>
    /* Base styles - Low specificity */
    p {
        color: #333;
        font-size: 12pt;
    }

    /* Component styles - Medium specificity */
    .card p {
        color: #666;
        line-height: 1.6;
    }

    /* Specific overrides - High specificity */
    .card.featured p {
        color: #000;
        font-weight: bold;
    }

    /* ID override - Highest specificity */
    #special-card p {
        color: red;
    }
</style>
<body>
    <p>Default paragraph (gray #333)</p>

    <div class="card">
        <p>Card paragraph (lighter gray #666)</p>
    </div>

    <div class="card featured">
        <p>Featured card (black, bold)</p>
    </div>

    <div id="special-card" class="card featured">
        <p>Special card (red, ID wins)</p>
    </div>
</body>
```

---

## Specificity Best Practices

1. **Keep specificity low**: Use single classes when possible
2. **Avoid ID selectors**: Use classes for reusability
3. **Don't over-nest**: Deep nesting increases specificity unnecessarily
4. **Use child combinator (>) intentionally**: It doubles specificity
5. **Organize by increasing specificity**: Base → Components → Overrides
6. **Avoid !important**: Indicates poor specificity management

---

## See Also

- [Element Selector](/reference/cssselectors/css_element_selector)
- [Class Selector](/reference/cssselectors/css_class_selector)
- [ID Selector](/reference/cssselectors/css_id_selector)
- [Descendant Combinator](/reference/cssselectors/css_descendant_combinator)
- [Child Combinator](/reference/cssselectors/css_child_combinator)
- [Multiple Selectors](/reference/cssselectors/css_multiple_selectors)

---
