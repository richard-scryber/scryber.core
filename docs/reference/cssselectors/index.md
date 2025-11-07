---
layout: default
title: CSS Selectors
parent: Template reference
parent_url: /reference/
has_children: true
has_toc: false
nav_order: 3
---

# CSS Style Selector Reference
{: .no_toc }

---

<details open markdown="block">
  <summary>
    Table of contents
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---

## Overview

A style selector contains one or more style properties and defines a set of conditions that an element must match in order to have any properties applied to it.
A rule contains one or more selectors and defines a condition that the current environment must meet to have the selectors considered.

The library supports the use of a sub-set of all the current CSS selectors and at-rules. These are detailed below.

```css
/* A simple selector matching all div elements */
div {
  color: red;
}

/* A simple selector matching all elements tagged with the 'blue' class */
.blue {
  color: blue;
}

/* Adds a border and padding to all blue tagged divs */
div.blue{
  border: solid 1pt blue;
  padding: 5pt;
}

@media print{
  /* All styles within this rule will be applied by the library */
  div {
    color: black;
  }
}
```

For more information on selectors see the <a href='/learning/styles/'>Using Styles</a> articles.

---

### Unsupported Rules

If the library encounters a css rule it cannot understand the entire content of that rule will be skipped.

```css
@scope (.article-body) to (figure) {
  /* Nothing after this will be checked
  img {
    border: 5px solid black;
    background-color: goldenrod;
  }
  */
}

.next{
  /* This will be included */
  color: red;
}
```

---

### Unsupported Selectors

If the library encounters a css selector it cannot understand, then it will be skipped. But any valid selectors will be maintained.

```css
.myclass[lang="pt"] {
  /* Nothing after this will be checked.
  img {
    border: 5px solid black;
    background-color: goldenrod;
  */
}

.next{
  /* This will be included */
  color: red;
}
```

---

### Combining Selectors

If the library encounters an individual css selector within a selector list, that it cannot understand, then it will be skipped. But any valid selectors will be maintained.

```css
.myclass[lang="pt"], .next {
  /* This will be included for .next */
  color: red;
}
```

---


## Selector Reference

The library supports the use of the following selectors and rules, sorted by category.

---

### Style Selectors

The following selectors are supported in the library.


| Selector  | Example  | Description |
|---|---|---|
| <a href='selectors/css_class_selector.html' >Class Name</a>   | .name | Will match against any element that has that class name associated with it, and apply the style properties.   |
| <a href='selectors/css_id_selector.html' >Element ID</a>   | #id | Will match against an element that has that identifier, and apply the style properties.   |
| <a href='selectors/css_element_selector.html' >Element Name</a>   | div | Will match against any element that has that *is* that element, and apply the style properties.   |
| <a href='selectors/css_root_selector.html' >Document Root</a>   | :root | Sets the default properties, or CSS variable values for that document to use during processing.   |
| <a href='selectors/css_universal_selector.html' >Catch All</a>   | \* | Matches all elements and applies the style properties. Can be used in combination with another selector to reduce the scope.   |

---

### Style Combinators


Combinators allow mutiple selectors to be combined into a single match check. The following combinators are supported in the library .


| Combinator  | Character  | Description |
|---|---|---|
| <a href='selectors/css_descendant_combinator.html' >Descendant</a>   | space (" ") | Whitespace between one selector and the next will match elements on the last selector that have any parent(s) in the hierarchy that match the preceeding selector(s).   |
| <a href='selectors/css_child_combinator.html' >Direct Child</a>   | &gt; | A greater-than symbol (&gt;) between one selector and the next will match elements on the last selector that have **direct** parent(s) in the hierarchy that match the preceeding selector(s).   |
| <a href='selectors/css_multiple_selectors.html' >Selector List</a>   | comma (", ") | A comma between one selector and the next will make a list of selectors and match on any of the selectors present.   |

---

### Style Rules Reference


| Rule Namme  | Example  | Description |
|---|---|---|
| <a href='selectors/css_media_rule.html' >Media Rule</a>   | @media | The media rule groups styles that are most appropriate for a display device. Most specifically the library will only use styles with a 'print' media query within it's list.   |
| <a href='selectors/css_page_rule.html' >Page Size Rule</a>   | @page | The page rule allows modification of the page paper sizing, along with margins on that page. They can be named to be applied to individual sections of the resultant document.  |
| <a href='selectors/css_font_face_rule.html' >Font Face Rule</a>   | @font-face | The font-face rule defines a custom font available to be used from a local or remote source along with the format it uses. See <a href='/learning/05-typography/' >Using Fonts</a> for more information on font support.  |

---

### Style Pseudo-Element Reference

Pseudo element are keywords added to a selector to identify particular parts of a matching element 

| Name  | Example  | Description |
|---|---|---|
| <a href='selectors/css_before_selector.html' >Before Content</a>   | ::before | ::before creates new pseudo element that is the first child of the matched element, often used to add cosmetic content. Also supports <a href='/learning/styles/counters.html' >counters</a>  |
| <a href='selectors/css_after_selector.html' >After Content</a>   | ::after | ::after in a style selector creates new pseudo element that is the last child of the matched element, often used to add cosmetic content. Also supports <a href='/learning/styles/counters.html' >counters</a>  |

---


