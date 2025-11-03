---
layout: default
title: float
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# float : Float Property

The `float` property specifies whether an element should float to the left or right side of its container, allowing text and inline elements to wrap around it. This is essential for creating magazine-style layouts, image captions, sidebars, and multi-column designs in PDF documents.

## Usage

```css
selector {
    float: value;
}
```

The float property accepts three values that control how elements are positioned within the document flow.

---

## Supported Values

### none (default)
The element does not float and will be displayed where it appears in the normal document flow. This is the default behavior.

### left
The element floats to the left side of its containing block. Content flows around the right side of the floated element.

### right
The element floats to the right side of its containing block. Content flows around the left side of the floated element.

---

## Supported Elements

The `float` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Images (`<img>`)
- Figures (`<figure>`)
- Tables (`<table>`)
- Paragraphs (`<p>`)
- All container elements

---

## Notes

- Floated elements are taken out of the normal document flow
- Text and inline elements wrap around floated elements
- Multiple floated elements will stack horizontally if space permits
- Use `clear` property to control element behavior after floats
- Floated elements should typically have a defined width
- Floats are ideal for text wrapping around images
- Parent containers may need `overflow: auto` or clearfix to properly contain floats
- Margins on floated elements create space between the float and surrounding content
- Floats work within their containing block and don't extend beyond it

---

## Data Binding

The `float` property supports data binding, allowing dynamic control of element floating behavior based on your data model. This enables configurable layouts where image positions, sidebar alignments, and content flow can be controlled through data.

### Example 1: Dynamic image float direction based on layout preferences

```html
<style>
    .article-image {
        float: {{image.floatDirection}};
        width: {{image.width}}pt;
        margin: {{image.marginTop}}pt {{image.marginRight}}pt {{image.marginBottom}}pt {{image.marginLeft}}pt;
        border: {{image.borderWidth}}pt solid {{image.borderColor}};
        padding: {{image.padding}}pt;
        background-color: {{image.backgroundColor}};
    }
    .article-text {
        text-align: {{text.alignment}};
    }
</style>
<body>
    <div>
        <img class="article-image" src="{{image.source}}" />
        <p class="article-text">{{article.content}}</p>
    </div>
</body>
```

Data model:
```json
{
  "image": {
    "floatDirection": "left",
    "width": 150,
    "marginTop": 0,
    "marginRight": 15,
    "marginBottom": 10,
    "marginLeft": 0,
    "borderWidth": 1,
    "borderColor": "#d1d5db",
    "padding": 5,
    "backgroundColor": "white",
    "source": "image.jpg"
  },
  "text": {
    "alignment": "justify"
  },
  "article": {
    "content": "This is the article text that wraps around the floated image..."
  }
}
```

### Example 2: Configurable sidebar float position

```html
<style>
    .sidebar {
        float: {{sidebar.floatDirection}};
        width: {{sidebar.width}}pt;
        background-color: {{sidebar.backgroundColor}};
        padding: {{sidebar.padding}}pt;
        margin-right: {{sidebar.marginRight}}pt;
        margin-left: {{sidebar.marginLeft}}pt;
        border: {{sidebar.borderWidth}}pt solid {{sidebar.borderColor}};
    }
    .sidebar h3 {
        margin: 0 0 10pt 0;
        font-size: {{sidebar.titleFontSize}}pt;
        color: {{sidebar.titleColor}};
    }
    .main-content {
        margin-left: {{content.leftMargin}}pt;
        margin-right: {{content.rightMargin}}pt;
    }
</style>
<body>
    <div>
        <div class="sidebar">
            <h3>{{sidebar.title}}</h3>
            <ul>
                <li>{{nav.item1}}</li>
                <li>{{nav.item2}}</li>
                <li>{{nav.item3}}</li>
            </ul>
        </div>
        <div class="main-content">
            <h1>{{content.title}}</h1>
            <p>{{content.body}}</p>
        </div>
    </div>
</body>
```

Data model:
```json
{
  "sidebar": {
    "floatDirection": "left",
    "width": 150,
    "backgroundColor": "#f3f4f6",
    "padding": 15,
    "marginRight": 20,
    "marginLeft": 0,
    "borderWidth": 1,
    "borderColor": "#d1d5db",
    "title": "Navigation",
    "titleFontSize": 12,
    "titleColor": "#1e3a8a"
  },
  "nav": {
    "item1": "Home",
    "item2": "Products",
    "item3": "Services"
  },
  "content": {
    "leftMargin": 190,
    "rightMargin": 0,
    "title": "Main Content Area",
    "body": "The main content appears next to the floated sidebar..."
  }
}
```

### Example 3: Data-driven callout box positioning

```html
<style>
    .callout {
        float: {{callout.float}};
        width: {{callout.width}}pt;
        margin: {{callout.marginTop}}pt {{callout.marginRight}}pt {{callout.marginBottom}}pt {{callout.marginLeft}}pt;
        padding: {{callout.padding}}pt;
        background-color: {{callout.backgroundColor}};
        border: {{callout.borderWidth}}pt solid {{callout.borderColor}};
        border-radius: {{callout.borderRadius}}pt;
    }
    .callout h4 {
        margin: 0 0 5pt 0;
        font-size: {{callout.titleFontSize}}pt;
        color: {{callout.titleColor}};
    }
    .callout p {
        margin: 0;
        font-size: {{callout.textFontSize}}pt;
    }
</style>
<body>
    <div>
        <h1>{{document.title}}</h1>
        <div class="callout">
            <h4>{{callout.title}}</h4>
            <p>{{callout.content}}</p>
        </div>
        <p>{{document.mainContent}}</p>
    </div>
</body>
```

Data model:
```json
{
  "document": {
    "title": "User Guide",
    "mainContent": "Main instructional text flows around the floating callout box..."
  },
  "callout": {
    "float": "right",
    "width": 160,
    "marginTop": 0,
    "marginRight": 0,
    "marginBottom": 15,
    "marginLeft": 20,
    "padding": 10,
    "backgroundColor": "#dcfce7",
    "borderWidth": 2,
    "borderColor": "#16a34a",
    "borderRadius": 5,
    "title": "Tip",
    "titleFontSize": 11,
    "titleColor": "#166534",
    "content": "Helpful advice for users...",
    "textFontSize": 9
  }
}
```

---

## Examples

### Example 1: Image floating left with text wrap

```html
<style>
    .article-image {
        float: left;
        width: 150pt;
        margin: 0 15pt 10pt 0;
    }
    .article-text {
        text-align: justify;
    }
</style>
<body>
    <div>
        <img class="article-image" src="image.jpg" />
        <p class="article-text">
            This is the article text that wraps around the floated image.
            The image appears on the left side while text flows naturally
            around its right edge. This creates a professional magazine-style
            layout that is perfect for reports, newsletters, and documents.
        </p>
    </div>
</body>
```

### Example 2: Image floating right with text wrap

```html
<style>
    .photo {
        float: right;
        width: 180pt;
        margin: 0 0 10pt 15pt;
        border: 1pt solid #d1d5db;
        padding: 5pt;
        background-color: white;
    }
    .caption {
        font-size: 9pt;
        color: #6b7280;
        text-align: center;
        margin-top: 5pt;
    }
</style>
<body>
    <div>
        <div class="photo">
            <img src="photo.jpg" width="100%" />
            <div class="caption">Figure 1: Product image</div>
        </div>
        <h2>Product Description</h2>
        <p>
            Our innovative product brings cutting-edge technology to your
            fingertips. The image shown floats to the right, allowing the
            text to flow naturally around it, creating an engaging and
            professional layout for your documents.
        </p>
    </div>
</body>
```

### Example 3: Sidebar layout with float

```html
<style>
    .sidebar {
        float: left;
        width: 150pt;
        background-color: #f3f4f6;
        padding: 15pt;
        margin-right: 20pt;
    }
    .sidebar h3 {
        margin: 0 0 10pt 0;
        font-size: 12pt;
        color: #1e3a8a;
    }
    .sidebar ul {
        margin: 0;
        padding: 0 0 0 15pt;
        font-size: 10pt;
    }
    .main-content {
        margin-left: 190pt;
    }
</style>
<body>
    <div>
        <div class="sidebar">
            <h3>Navigation</h3>
            <ul>
                <li>Home</li>
                <li>Products</li>
                <li>Services</li>
                <li>Contact</li>
            </ul>
        </div>
        <div class="main-content">
            <h1>Main Content Area</h1>
            <p>The main content appears next to the floated sidebar...</p>
        </div>
    </div>
</body>
```

### Example 4: Two-column layout with floats

```html
<style>
    .column {
        float: left;
        width: 48%;
        margin-right: 4%;
    }
    .column:last-child {
        margin-right: 0;
    }
    .column h2 {
        color: #1e3a8a;
        font-size: 14pt;
        margin: 0 0 10pt 0;
    }
    .column p {
        text-align: justify;
        font-size: 10pt;
    }
</style>
<body>
    <h1 style="text-align: center;">Newsletter</h1>
    <div class="column">
        <h2>Column One</h2>
        <p>First column content goes here. This creates a professional
        two-column layout perfect for newsletters and reports.</p>
    </div>
    <div class="column">
        <h2>Column Two</h2>
        <p>Second column content goes here. Both columns appear side
        by side using the float property.</p>
    </div>
</body>
```

### Example 5: Pull quote floating right

```html
<style>
    .pull-quote {
        float: right;
        width: 200pt;
        margin: 10pt 0 10pt 20pt;
        padding: 15pt;
        background-color: #f5f5f5;
        border-left: 4pt solid #2563eb;
        font-size: 14pt;
        font-style: italic;
        color: #1e3a8a;
    }
    .article p {
        text-align: justify;
        line-height: 1.6;
    }
</style>
<body>
    <div class="article">
        <h1>Article Title</h1>
        <p>First paragraph of the article text...</p>
        <div class="pull-quote">
            "This important quote stands out and draws the reader's attention."
        </div>
        <p>The article continues with more paragraphs that wrap around
        the floated pull quote, creating visual interest...</p>
    </div>
</body>
```

### Example 6: Info box floating left

```html
<style>
    .info-box {
        float: left;
        width: 180pt;
        margin: 0 20pt 15pt 0;
        padding: 12pt;
        background-color: #dbeafe;
        border: 2pt solid #2563eb;
        border-radius: 5pt;
    }
    .info-box h4 {
        margin: 0 0 8pt 0;
        color: #1e40af;
        font-size: 12pt;
    }
    .info-box p {
        margin: 0;
        font-size: 9pt;
        line-height: 1.4;
    }
</style>
<body>
    <div>
        <div class="info-box">
            <h4>Did You Know?</h4>
            <p>Interesting fact or additional information that complements
            the main text.</p>
        </div>
        <h1>Main Article</h1>
        <p>The main article text flows around the floated info box,
        creating an engaging layout that draws attention to key facts...</p>
    </div>
</body>
```

### Example 7: Multiple images floating

```html
<style>
    .thumbnail {
        float: left;
        width: 100pt;
        height: 100pt;
        margin: 5pt;
        border: 1pt solid #d1d5db;
        background-color: #f3f4f6;
    }
    .gallery-text {
        clear: both;
        padding-top: 10pt;
    }
</style>
<body>
    <h2>Image Gallery</h2>
    <div class="thumbnail">Image 1</div>
    <div class="thumbnail">Image 2</div>
    <div class="thumbnail">Image 3</div>
    <div class="thumbnail">Image 4</div>
    <div class="gallery-text">
        <p>Gallery description appears after all floated images...</p>
    </div>
</body>
```

### Example 8: Product listing with floats

```html
<style>
    .product {
        margin-bottom: 20pt;
        overflow: auto;
    }
    .product-image {
        float: left;
        width: 120pt;
        height: 120pt;
        margin-right: 15pt;
        border: 1pt solid #d1d5db;
        background-color: #f9fafb;
    }
    .product-details h3 {
        margin: 0 0 8pt 0;
        color: #1e3a8a;
    }
    .product-price {
        font-size: 16pt;
        font-weight: bold;
        color: #16a34a;
    }
</style>
<body>
    <div class="product">
        <div class="product-image">Product Image</div>
        <div class="product-details">
            <h3>Product Name</h3>
            <p>Product description and features...</p>
            <div class="product-price">$99.99</div>
        </div>
    </div>
</body>
```

### Example 9: Document header with floating logo

```html
<style>
    .document-header {
        border-bottom: 2pt solid #1e3a8a;
        padding-bottom: 10pt;
        margin-bottom: 20pt;
        overflow: auto;
    }
    .logo {
        float: left;
        width: 80pt;
        height: 60pt;
        background-color: #1e3a8a;
        color: white;
        text-align: center;
        padding-top: 20pt;
        font-weight: bold;
    }
    .header-text {
        margin-left: 100pt;
    }
    .header-text h1 {
        margin: 0;
        color: #1e3a8a;
    }
    .header-text p {
        margin: 5pt 0 0 0;
        color: #6b7280;
    }
</style>
<body>
    <div class="document-header">
        <div class="logo">LOGO</div>
        <div class="header-text">
            <h1>Company Report</h1>
            <p>Annual Financial Statement 2025</p>
        </div>
    </div>
</body>
```

### Example 10: Floating callout boxes

```html
<style>
    .callout {
        float: right;
        width: 160pt;
        margin: 0 0 15pt 20pt;
        padding: 10pt;
        border-radius: 5pt;
    }
    .callout-tip {
        background-color: #dcfce7;
        border: 2pt solid #16a34a;
    }
    .callout-warning {
        background-color: #fef3c7;
        border: 2pt solid #f59e0b;
    }
    .callout h4 {
        margin: 0 0 5pt 0;
        font-size: 11pt;
    }
</style>
<body>
    <h1>User Guide</h1>
    <div class="callout callout-tip">
        <h4>Tip</h4>
        <p style="font-size: 9pt; margin: 0;">Helpful advice for users...</p>
    </div>
    <p>Main instructional text flows around the floating callout box...</p>
    <div class="callout callout-warning">
        <h4>Warning</h4>
        <p style="font-size: 9pt; margin: 0;">Important warning message...</p>
    </div>
    <p>Additional instructions continue here...</p>
</body>
```

### Example 11: Three-column layout

```html
<style>
    .three-col {
        float: left;
        width: 31%;
        margin-right: 3.5%;
        background-color: #f9fafb;
        padding: 15pt;
        border: 1pt solid #d1d5db;
    }
    .three-col:last-child {
        margin-right: 0;
    }
    .three-col h3 {
        margin: 0 0 10pt 0;
        font-size: 12pt;
        color: #1e3a8a;
    }
</style>
<body>
    <h1 style="text-align: center;">Three Column Layout</h1>
    <div class="three-col">
        <h3>Column One</h3>
        <p style="font-size: 10pt;">First column content...</p>
    </div>
    <div class="three-col">
        <h3>Column Two</h3>
        <p style="font-size: 10pt;">Second column content...</p>
    </div>
    <div class="three-col">
        <h3>Column Three</h3>
        <p style="font-size: 10pt;">Third column content...</p>
    </div>
</body>
```

### Example 12: Magazine-style layout

```html
<style>
    .magazine-layout {
        column-count: 2;
        column-gap: 20pt;
    }
    .feature-image {
        float: left;
        width: 140pt;
        margin: 0 15pt 10pt 0;
    }
    .dropcap {
        float: left;
        font-size: 48pt;
        line-height: 40pt;
        margin: 0 8pt 0 0;
        color: #1e3a8a;
        font-weight: bold;
    }
</style>
<body>
    <h1>Feature Article</h1>
    <div class="magazine-layout">
        <img class="feature-image" src="feature.jpg" />
        <p><span class="dropcap">T</span>his article demonstrates a
        professional magazine-style layout with floating images and
        drop caps. The text flows naturally around the floated elements...</p>
        <p>Additional paragraphs continue the flow...</p>
    </div>
</body>
```

### Example 13: Alternating image positions

```html
<style>
    .section {
        margin-bottom: 30pt;
        overflow: auto;
    }
    .image-left {
        float: left;
        width: 150pt;
        margin: 0 15pt 10pt 0;
        border: 1pt solid #d1d5db;
    }
    .image-right {
        float: right;
        width: 150pt;
        margin: 0 0 10pt 15pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="section">
        <img class="image-left" src="img1.jpg" />
        <h2>Section One</h2>
        <p>Text flows to the right of the left-floated image...</p>
    </div>
    <div class="section">
        <img class="image-right" src="img2.jpg" />
        <h2>Section Two</h2>
        <p>Text flows to the left of the right-floated image...</p>
    </div>
</body>
```

### Example 14: Contact card with floating elements

```html
<style>
    .contact-card {
        border: 2pt solid #d1d5db;
        padding: 20pt;
        background-color: #f9fafb;
        overflow: auto;
    }
    .contact-photo {
        float: left;
        width: 80pt;
        height: 80pt;
        margin-right: 15pt;
        border-radius: 50%;
        background-color: #dbeafe;
        border: 2pt solid #2563eb;
    }
    .contact-info h3 {
        margin: 0 0 5pt 0;
        color: #1e3a8a;
    }
    .contact-info p {
        margin: 3pt 0;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="contact-card">
        <div class="contact-photo"></div>
        <div class="contact-info">
            <h3>John Smith</h3>
            <p>Senior Manager</p>
            <p>john.smith@company.com</p>
            <p>+1 (555) 123-4567</p>
        </div>
    </div>
</body>
```

### Example 15: Complex document layout

```html
<style>
    .document {
        padding: 30pt;
    }
    .header-logo {
        float: left;
        width: 60pt;
        height: 60pt;
        margin-right: 15pt;
        background-color: #1e3a8a;
        color: white;
        text-align: center;
        padding-top: 20pt;
        font-weight: bold;
    }
    .header-info {
        margin-left: 80pt;
        border-bottom: 2pt solid #1e3a8a;
        padding-bottom: 10pt;
        margin-bottom: 20pt;
    }
    .sidebar-note {
        float: right;
        width: 180pt;
        margin: 0 0 15pt 20pt;
        padding: 12pt;
        background-color: #fef3c7;
        border-left: 4pt solid #f59e0b;
        font-size: 10pt;
    }
    .content-image {
        float: left;
        width: 140pt;
        margin: 0 15pt 10pt 0;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="document">
        <div class="header-logo">LOGO</div>
        <div class="header-info">
            <h1 style="margin: 0;">Annual Report 2025</h1>
            <p style="margin: 5pt 0 0 0; color: #6b7280;">
                Financial Year Summary
            </p>
        </div>

        <div class="sidebar-note">
            <strong>Note:</strong> Key highlights and important points
            appear in this sidebar area.
        </div>

        <img class="content-image" src="chart.jpg" />

        <p>The main content text flows around both the sidebar note and
        the floated image, creating a sophisticated multi-element layout...</p>
        <p>Additional paragraphs continue the professional presentation...</p>
    </div>
</body>
```

---

## See Also

- [position](/reference/cssproperties/css_prop_position) - Set positioning method
- [top](/reference/cssproperties/css_prop_top) - Set top offset for positioned elements
- [left](/reference/cssproperties/css_prop_left) - Set left offset for positioned elements
- [right](/reference/cssproperties/css_prop_right) - Set right offset for positioned elements
- [bottom](/reference/cssproperties/css_prop_bottom) - Set bottom offset for positioned elements
- [margin](/reference/cssproperties/css_prop_margin) - Set margin spacing around elements
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
