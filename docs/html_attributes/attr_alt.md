---
layout: default
title: alt
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @alt : The Alternative Text Attribute

The `alt` attribute provides alternative text for images when they cannot be displayed or for accessibility purposes. While PDFs typically display images successfully, the `alt` attribute serves as important metadata for accessibility tools, screen readers, and PDF document structure, enhancing document usability and compliance.

## Usage

The `alt` attribute provides alternative text that:
- Describes image content for accessibility
- Provides context when images fail to load
- Improves PDF document accessibility and screen reader support
- Serves as metadata for PDF structure
- Should be concise but descriptive
- Supports data binding for dynamic descriptions
- Used exclusively with `<img>` elements

```html
<!-- Descriptive alt text -->
<img src="company-logo.png" width="150pt" height="75pt"
     alt="Acme Corporation Logo" />

<!-- Informative image -->
<img src="sales-chart.png" width="400pt" height="300pt"
     alt="Sales performance chart showing 25% increase in Q4 2024" />

<!-- Decorative image -->
<img src="decorative-line.png" width="100%" height="2pt" alt="" />

<!-- Dynamic alt text -->
<img src="{{model.productImage}}" width="200pt" height="200pt"
     alt="{{model.productName}} - {{model.productDescription}}" />
```

---

## Supported Elements

The `alt` attribute is used with:

### Image Element
- `<img>` - Image element (exclusive use)

---

## Binding Values

The `alt` attribute supports data binding for dynamic descriptions:

```html
<!-- Simple dynamic alt text -->
<img src="{{model.imagePath}}" width="200pt" height="150pt"
     alt="{{model.imageDescription}}" />

<!-- Constructed description -->
<img src="products/{{model.productId}}.jpg" width="300pt" height="200pt"
     alt="Product: {{model.productName}} - {{model.category}}" />

<!-- Detailed description -->
<img src="{{model.chartUrl}}" width="500pt" height="300pt"
     alt="{{model.chartType}} showing {{model.dataDescription}} for {{model.period}}" />

<!-- User-generated content -->
<img src="{{model.userPhoto}}" width="100pt" height="100pt"
     alt="Profile photo of {{model.userName}}" />

<!-- Repeating images with alt text -->
<template data-bind="{{model.gallery}}">
    <img src="{{.url}}" width="150pt" height="100pt"
         alt="{{.title}}: {{.description}}"
         style="margin: 5pt;" />
</template>
```

**Data Model Example:**
```json
{
  "imagePath": "images/dashboard.png",
  "imageDescription": "Analytics dashboard overview",
  "productId": "12345",
  "productName": "Premium Widget",
  "category": "Electronics",
  "chartUrl": "charts/sales.png",
  "chartType": "Bar chart",
  "dataDescription": "monthly sales figures",
  "period": "2024",
  "userPhoto": "avatars/jane.jpg",
  "userName": "Jane Smith",
  "gallery": [
    {
      "url": "gallery/photo1.jpg",
      "title": "Mountain Vista",
      "description": "Sunrise over mountain peaks"
    },
    {
      "url": "gallery/photo2.jpg",
      "title": "Ocean View",
      "description": "Sunset at the beach"
    }
  ]
}
```

---

## Notes

### Purpose of Alt Text

The `alt` attribute serves multiple purposes:

1. **Accessibility**: Describes images for screen readers and assistive technologies
2. **Context**: Provides information when images can't be displayed
3. **PDF Structure**: Enhances PDF document accessibility compliance
4. **SEO**: Helps with searchability and indexing (in web contexts)
5. **Documentation**: Clarifies image content for maintainers

### Writing Good Alt Text

Follow these guidelines for effective alt text:

**Do:**
- Be concise but descriptive
- Describe the content and purpose
- Include relevant context
- Use proper punctuation
- Keep it under 125 characters when possible
- Describe what's in the image, not just label it

**Don't:**
- Start with "Image of" or "Picture of" (it's implied)
- Be overly verbose
- Include file names or technical details
- Use ALL CAPS (unless acronyms)
- Leave it blank for meaningful images

```html
<!-- Good alt text -->
<img src="ceo-photo.jpg" width="200pt" height="250pt"
     alt="John Smith, Chief Executive Officer" />

<img src="sales-chart.png" width="500pt" height="300pt"
     alt="Line graph showing 30% sales increase from January to December 2024" />

<!-- Poor alt text -->
<img src="ceo-photo.jpg" width="200pt" height="250pt"
     alt="image of person" />  <!-- Too vague -->

<img src="sales-chart.png" width="500pt" height="300pt"
     alt="sales-chart.png" />  <!-- File name, not description -->
```

### Decorative Images

For purely decorative images that don't convey information, use an empty `alt` attribute:

```html
<!-- Decorative elements -->
<img src="decorative-border.png" width="100%" height="5pt" alt="" />
<img src="spacer.gif" width="10pt" height="10pt" alt="" />
<img src="gradient.png" width="100%" height="50pt" alt="" />
```

Empty alt text (`alt=""`) tells assistive technologies to skip the image.

### Informative vs Decorative

**Informative images** require descriptive alt text:
- Photos of people, products, places
- Charts, graphs, diagrams
- Logos with text
- Icons with meaning
- Screenshots
- Infographics

**Decorative images** should have empty alt (`alt=""`):
- Background patterns
- Borders and dividers
- Spacer images
- Purely aesthetic elements

```html
<!-- Informative -->
<img src="product.jpg" width="300pt" height="200pt"
     alt="XYZ Smartphone in silver with 6.5-inch display" />

<!-- Decorative -->
<img src="decorative-flourish.png" width="50pt" height="50pt" alt="" />
```

### Complex Images

For complex images like charts or diagrams, consider:

1. **Brief alt text** with **longer description** nearby
2. **Reference to caption** or adjacent text
3. **Link to detailed description** in a separate section

```html
<!-- Chart with brief alt and detailed caption -->
<figure>
    <img src="complex-chart.png" width="600pt" height="400pt"
         alt="Multi-line chart comparing revenue across five product categories" />
    <figcaption>
        Figure 2.1: Revenue comparison showing Electronics (blue line) at $5M,
        Furniture (red line) at $3.5M, Clothing (green line) at $4M,
        Food (orange line) at $2.5M, and Other (purple line) at $1M for 2024.
    </figcaption>
</figure>

<!-- Diagram with reference -->
<img src="architecture.png" width="700pt" height="500pt"
     alt="System architecture diagram (detailed description below)" />
<p>
    The diagram illustrates the three-tier architecture consisting of...
    [detailed text description]
</p>
```

### Logos and Brand Images

For logos, include the company or product name:

```html
<img src="company-logo.png" width="150pt" height="50pt"
     alt="Acme Corporation" />

<img src="product-logo.png" width="100pt" height="100pt"
     alt="SuperApp Pro logo" />
```

### Icons

For functional icons, describe their purpose:

```html
<a href="mailto:info@example.com">
    <img src="email-icon.png" width="24pt" height="24pt"
         alt="Email" />
    Contact us
</a>

<a href="download.pdf">
    <img src="download-icon.png" width="20pt" height="20pt"
         alt="Download" />
    Annual Report
</a>

<!-- When icon is next to text, alt can be empty or brief -->
<img src="checkmark.png" width="16pt" height="16pt" alt="Yes" />
Task completed

<img src="warning.png" width="16pt" height="16pt" alt="Warning" />
Please review carefully
```

### Linked Images

When an image is inside a link, describe the link destination:

```html
<a href="product-details.pdf">
    <img src="product-thumb.jpg" width="150pt" height="100pt"
         alt="View detailed specifications for Model XYZ-123" />
</a>

<a href="https://example.com">
    <img src="logo.png" width="120pt" height="40pt"
         alt="Visit Example Corporation website" />
</a>
```

### Missing Alt Attribute

Always include the `alt` attribute, even if empty:

```html
<!-- Correct: Always include alt -->
<img src="image.jpg" width="200pt" height="150pt" alt="Description" />
<img src="decorative.png" width="100pt" height="10pt" alt="" />

<!-- Avoid: Missing alt attribute -->
<img src="image.jpg" width="200pt" height="150pt" />  <!-- Not accessible -->
```

### Accessibility Best Practices

For optimal accessibility:

1. **Always include `alt`**: Never omit the attribute
2. **Be descriptive**: Provide meaningful descriptions
3. **Context matters**: Include relevant context
4. **Empty for decorative**: Use `alt=""` for decorative images
5. **Avoid redundancy**: Don't repeat adjacent text
6. **Test with screen readers**: Verify alt text makes sense when read aloud

### Alt Text Length

**Recommended lengths:**
- **Short**: Under 50 characters for simple images
- **Medium**: 50-125 characters for most images
- **Long**: Use longer descriptions in captions or separate text

```html
<!-- Short (logo) -->
<img src="logo.png" width="100pt" height="50pt" alt="TechCorp" />

<!-- Medium (product) -->
<img src="widget.jpg" width="200pt" height="150pt"
     alt="Blue wireless mouse with ergonomic design and three programmable buttons" />

<!-- Use caption for long descriptions -->
<figure>
    <img src="complex-infographic.png" width="600pt" height="800pt"
         alt="Employee satisfaction survey results infographic" />
    <figcaption>
        [Extended description of all infographic elements, statistics,
        and relationships shown]
    </figcaption>
</figure>
```

---

## Examples

### Basic Image Alt Text

```html
<img src="office-building.jpg" width="400pt" height="300pt"
     alt="Modern glass office building with five stories" />

<img src="team-photo.jpg" width="600pt" height="400pt"
     alt="Company team of 15 employees at annual retreat" />

<img src="signature.png" width="150pt" height="50pt"
     alt="Signature of John Smith, CEO" />
```

### Product Images

```html
<div>
    <h2>Premium Widget</h2>
    <img src="premium-widget.jpg" width="400pt" height="300pt"
         alt="Premium Widget in brushed aluminum finish with LED indicators" />
    <p>Our flagship product featuring advanced technology...</p>
</div>

<div>
    <h2>Standard Widget</h2>
    <img src="standard-widget.jpg" width="400pt" height="300pt"
         alt="Standard Widget in matte black with simple control panel" />
    <p>Perfect for everyday use...</p>
</div>
```

### Charts and Graphs

```html
<div>
    <h2>Quarterly Revenue</h2>
    <img src="revenue-chart.png" width="600pt" height="400pt"
         alt="Bar chart showing quarterly revenue: Q1 $2.5M, Q2 $3.1M, Q3 $3.8M, Q4 $4.2M" />
</div>

<div>
    <h2>Market Share</h2>
    <img src="market-pie.png" width="500pt" height="400pt"
         alt="Pie chart: Company A 35%, Company B 28%, Company C 20%, Others 17%" />
</div>

<div>
    <h2>Growth Trend</h2>
    <img src="growth-line.png" width="700pt" height="400pt"
         alt="Line graph showing steady 15% year-over-year growth from 2020 to 2024" />
</div>
```

### Profile Photos

```html
<!-- Model: { team: [
    { name: "Alice Johnson", role: "CEO", photo: "alice.jpg" },
    { name: "Bob Smith", role: "CTO", photo: "bob.jpg" }
] } -->

<div>
    <h2>Leadership Team</h2>

    <template data-bind="{{model.team}}">
        <div style="margin-bottom: 20pt;">
            <img src="{{.photo}}" width="120pt" height="120pt"
                 alt="{{.name}}, {{.role}}"
                 style="border-radius: 60pt; border: 3pt solid #336699;" />
            <h3>{{.name}}</h3>
            <p>{{.role}}</p>
        </div>
    </template>
</div>
```

### Logos with Alt Text

```html
<!-- Company logo in header -->
<header style="padding: 15pt; border-bottom: 2pt solid #336699;">
    <img src="company-logo.png" width="150pt" height="50pt"
         alt="TechCorp Solutions" />
</header>

<!-- Partner logos -->
<div>
    <h3>Our Partners</h3>
    <img src="partner1.png" width="120pt" height="60pt"
         alt="Partner One Inc." style="margin: 10pt;" />
    <img src="partner2.png" width="120pt" height="60pt"
         alt="Partner Two Ltd." style="margin: 10pt;" />
    <img src="partner3.png" width="120pt" height="60pt"
         alt="Partner Three Corp." style="margin: 10pt;" />
</div>
```

### Icons with Descriptive Alt

```html
<div>
    <h2>Features</h2>

    <div style="margin-bottom: 15pt;">
        <img src="icons/speed.png" width="32pt" height="32pt"
             alt="Fast performance" style="vertical-align: middle;" />
        <strong>Lightning Fast</strong>
        <p>Process data in milliseconds...</p>
    </div>

    <div style="margin-bottom: 15pt;">
        <img src="icons/secure.png" width="32pt" height="32pt"
             alt="Secure" style="vertical-align: middle;" />
        <strong>Bank-Level Security</strong>
        <p>Your data is encrypted...</p>
    </div>

    <div>
        <img src="icons/support.png" width="32pt" height="32pt"
             alt="24/7 support" style="vertical-align: middle;" />
        <strong>24/7 Support</strong>
        <p>We're here when you need us...</p>
    </div>
</div>
```

### Decorative Images

```html
<h1>Annual Report</h1>

<!-- Decorative border -->
<img src="decorative-border.png" width="100%" height="3pt" alt="" />

<p>This report covers the fiscal year...</p>

<!-- Decorative flourish -->
<div style="text-align: center; margin: 20pt 0;">
    <img src="flourish.png" width="100pt" height="50pt" alt="" />
</div>

<h2>Executive Summary</h2>

<!-- Decorative spacer -->
<img src="spacer.gif" width="10pt" height="20pt" alt="" />
```

### Screenshots and UI Images

```html
<div>
    <h2>User Interface</h2>

    <img src="dashboard-screenshot.png" width="700pt" height="500pt"
         alt="Dashboard interface showing main navigation on left, analytics charts in center, and notification panel on right" />

    <p>The dashboard provides an intuitive overview...</p>
</div>

<div>
    <h2>Mobile App</h2>

    <img src="mobile-app-screen.png" width="300pt" height="600pt"
         alt="Mobile app home screen with profile icon top-left, search bar at top, and grid of six feature buttons below" />
</div>
```

### Architectural Diagrams

```html
<div>
    <h2>System Architecture</h2>

    <img src="architecture-diagram.png" width="800pt" height="600pt"
         alt="Three-tier architecture diagram: presentation layer at top, business logic layer in middle, data layer at bottom, with bidirectional arrows showing communication" />

    <p><strong>Detailed Description:</strong></p>
    <ul>
        <li><strong>Presentation Layer:</strong> Web and mobile interfaces</li>
        <li><strong>Business Logic Layer:</strong> API servers and application logic</li>
        <li><strong>Data Layer:</strong> Database cluster and cache servers</li>
    </ul>
</div>
```

### Before/After Images

```html
<div>
    <h2>Website Redesign</h2>

    <div style="display: flex; gap: 20pt; margin-bottom: 20pt;">
        <div style="flex: 1;">
            <h3>Before</h3>
            <img src="old-design.png" width="350pt" height="250pt"
                 alt="Old website design with cluttered layout, multiple sidebars, and outdated color scheme" />
        </div>
        <div style="flex: 1;">
            <h3>After</h3>
            <img src="new-design.png" width="350pt" height="250pt"
                 alt="New website design with clean layout, modern typography, and cohesive blue color scheme" />
        </div>
    </div>
</div>
```

### Image Gallery with Alt Text

```html
<!-- Model: { photos: [
    { url: "photo1.jpg", title: "Mountain Peak", desc: "Snow-capped mountain at sunset" },
    { url: "photo2.jpg", title: "Ocean Waves", desc: "Waves crashing on rocky shore" }
] } -->

<div>
    <h2>Photo Gallery</h2>

    <template data-bind="{{model.photos}}">
        <figure style="margin: 20pt; display: inline-block;">
            <img src="{{.url}}" width="300pt" height="200pt"
                 alt="{{.desc}}" />
            <figcaption style="text-align: center; margin-top: 5pt;">
                {{.title}}
            </figcaption>
        </figure>
    </template>
</div>
```

### Product Catalog with Detailed Alt

```html
<!-- Model: { products: [{
    name: "Ergonomic Chair",
    image: "chair.jpg",
    features: "Adjustable height, lumbar support, mesh back"
}] } -->

<template data-bind="{{model.products}}">
    <div style="border: 1pt solid #ddd; padding: 20pt; margin-bottom: 20pt;">
        <img src="{{.image}}" width="300pt" height="300pt"
             alt="{{.name}}: {{.features}}" />
        <h3>{{.name}}</h3>
        <p>{{.features}}</p>
    </div>
</template>
```

### Linked Images with Descriptive Alt

```html
<div>
    <h2>Download Resources</h2>

    <a href="user-manual.pdf" style="text-decoration: none; color: inherit;">
        <div style="border: 1pt solid #ccc; padding: 15pt; margin-bottom: 10pt;">
            <img src="pdf-icon.png" width="48pt" height="48pt"
                 alt="Download User Manual PDF"
                 style="vertical-align: middle; margin-right: 15pt;" />
            <strong>User Manual</strong>
            <p style="margin: 5pt 0 0 0; color: #666;">Complete guide (PDF, 5MB)</p>
        </div>
    </a>

    <a href="quick-start.pdf" style="text-decoration: none; color: inherit;">
        <div style="border: 1pt solid #ccc; padding: 15pt;">
            <img src="pdf-icon.png" width="48pt" height="48pt"
                 alt="Download Quick Start Guide PDF"
                 style="vertical-align: middle; margin-right: 15pt;" />
            <strong>Quick Start Guide</strong>
            <p style="margin: 5pt 0 0 0; color: #666;">Get started (PDF, 1MB)</p>
        </div>
    </a>
</div>
```

### Infographics

```html
<div>
    <h2>Company Growth Statistics</h2>

    <img src="growth-infographic.png" width="700pt" height="1000pt"
         alt="Growth infographic: 500+ customers, 50 employees, 10 offices worldwide, $10M revenue, 98% customer satisfaction, 5-star rating" />

    <p><strong>Key Statistics:</strong></p>
    <ul>
        <li>500+ satisfied customers</li>
        <li>50 dedicated employees</li>
        <li>10 offices worldwide</li>
        <li>$10M annual revenue</li>
        <li>98% customer satisfaction rate</li>
        <li>5-star average rating</li>
    </ul>
</div>
```

### Maps and Location Images

```html
<div>
    <h2>Office Locations</h2>

    <img src="world-map-offices.png" width="800pt" height="500pt"
         alt="World map showing office locations: New York, London, Tokyo, Sydney, and São Paulo marked with blue pins" />

    <h3>Contact Our Offices</h3>
    <ul>
        <li>New York: +1-212-555-0100</li>
        <li>London: +44-20-7123-4567</li>
        <li>Tokyo: +81-3-1234-5678</li>
        <li>Sydney: +61-2-9123-4567</li>
        <li>São Paulo: +55-11-1234-5678</li>
    </ul>
</div>
```

### Certificate and Award Images

```html
<div>
    <h2>Certifications</h2>

    <img src="iso-certification.jpg" width="400pt" height="500pt"
         alt="ISO 9001:2015 Quality Management certification dated January 2024" />

    <img src="industry-award.jpg" width="400pt" height="500pt"
         alt="Industry Excellence Award 2024 for Best Innovation, presented by Tech Association"
         style="margin-left: 20pt;" />
</div>
```

### Conditional Alt Text

```html
<!-- Model: { user: { name: "Alice", hasPhoto: true, role: "Manager" } } -->

<img src="{{model.user.hasPhoto ? 'photos/' + model.user.name + '.jpg' : 'placeholder.png'}}"
     width="100pt" height="100pt"
     alt="{{model.user.hasPhoto ? 'Photo of ' + model.user.name + ', ' + model.user.role : 'Placeholder photo'}}" />
```

### Signature Images

```html
<div style="margin-top: 40pt;">
    <p>Sincerely,</p>
    <img src="ceo-signature.png" width="200pt" height="60pt"
         alt="Signature of Michael Chen" />
    <p style="margin-top: 5pt;">
        <strong>Michael Chen</strong><br/>
        Chief Executive Officer
    </p>
</div>
```

---

## See Also

- [img](/reference/htmltags/img.html) - Image element (uses alt attribute)
- [src](/reference/htmlattributes/src.html) - Source attribute for image paths
- [width](/reference/htmlattributes/width.html) - Width sizing attribute
- [height](/reference/htmlattributes/height.html) - Height sizing attribute
- [title](/reference/htmlattributes/title.html) - Title attribute for additional information
- [Accessibility](/reference/accessibility/) - PDF accessibility guidelines
- [Data Binding](/reference/binding/) - Dynamic content and attributes

---
