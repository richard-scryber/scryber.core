---
layout: default
title: Template reference
has_children: true
nav_order: 3
has_toc: false
---

# Template content refererence
{: .no_toc }

All the elements, attributes, style selectors along with binding expressions and helpers that the core library supports.

---

## Template Content


### [Supported HTML Elements](htmltags/index.html)


The HTML tags such as <code>&lt;html&gt;</code>, or <code>&lt;div&gt;</code> within a template or referernced source give the structure and content to your document. This section details all the HTML tags that the core library supports, along with the available (and unavailable attributes) that the tag supports - including those that are specific to the library.



### <a href='htmlattributes/index.html'>HTML attribute refererence</a>

The HTML attributes alter the behaviour of their containing tag such as <code>&lt;div id='idAttrValue' &gt;</code>. This section details all the attributes that the core library supports, along with the supported values for the attribute, and also elements it can be used on (and cannot) - including those that are specific to the library.

## Template Styles

### [CSS selector and at rule reference](cssselectors/index.html)

Separating the visual design from the actual content of a template is done using the standard css notation within a <code>&lt;style&gt;</code> element or including references to css style sheets. The selectors such as <code>.className</code> and / or <code>#refId</code> .  This sections details all the supported (and not supported) selectors in the library, along with the at rules such as <code>@media</code> that allow selectorss to be regulated on their application within the library.


### <a href='cssproperties/index.html'>CSS style properties reference</a>

Styling properties such as <code>font-style: italic;</code> or <code>background-image: var(model.logo);</code> are defined within selectors to be applied to matching content tags, or drawing elements. This section details the available properties, what values can be assigned to them, and which tags/elements support them.

## Binding Data

### <a href='binding/index.html'>Binding Expressions reference</a>

Binding content within templates to external or calculated data is key to creating dynamic documents and can be within the <code>{% raw %}&lt;span&gt;{{ handle bars notation }}&lt;/span&gt;{% endraw %}</code> or attributes <code>{% raw %}&lt;rect width='{{model.width}}' .../&gt;{% endraw %}</code> or within style properties with the <code>width: calc(model.size + 10pt)</code>. This sections details all the available expressions and functions that can be used within a template (or passed content) to dynamically alter the output, or layout, or design of the resultant document.


## Drawing Vectors

### <a href='svgelements/index.html'>SVG drawing element refererence</a>

Drawing within a template is based around inline or databound svg elements in the content, and must be qualified with the svg namespace (e.g. <code>&lt;svg xmlns='http://www.w3.org/2000/svg' &gt;...&lt;svg&gt;</code>) or referenced via the <code>&lt;img src='mydrawing.svg' /&gt;</code> element. This section details all the SVG Drawing elements that the core library supports, along with the available (and unavailable attributes) that the tag supports.


### <a href='svgattributes/index.html'>SVG drawing attribute refererence</a>

The SVG attributes such as <code>@width</code> or <code>@fill</code> alter the appearance and behaviour of their containing element, e.g. <code>&lt;rect width="{% raw %}{{model.width}}{% endraw %}" height="40pt" fill="aqua" /&gt;</code>. This section details all the attributes that the core library supports, along with the supported values for the attribute, and also elements it can be used on (and cannot).
