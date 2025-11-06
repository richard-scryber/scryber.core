---
layout: default
title: page-break-inside
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# page-break-inside : Page Break Inside Property

The `page-break-inside` property controls whether page breaks are allowed within an element when generating PDF documents. This property is crucial for keeping content blocks together, preventing tables from splitting awkwardly, and maintaining visual coherence. Note that this is a legacy CSS2 property; consider using the newer `break-inside` property for more options.

## Usage

```css
selector {
    page-break-inside: value;
}
```

The page-break-inside property prevents or allows page breaks from occurring within the bounds of an element, ensuring content integrity in your PDF documents.

---

## Supported Values

### auto (default)
Automatic page breaking behavior. The browser/PDF generator may insert page breaks within the element if needed to fit content on pages. This is the default value.

### avoid
Attempts to avoid page breaks within the element. The PDF generator will try to keep the entire element on a single page. If the element is too large to fit on one page, this directive may be overridden.

---

## Supported Elements

The `page-break-inside` property can be applied to:
- Block-level elements (`<div>`, `<section>`, `<article>`)
- Tables (`<table>`)
- Lists (`<ul>`, `<ol>`)
- Paragraphs (`<p>`)
- Figures and images with captions
- Code blocks (`<pre>`)
- Block quotes (`<blockquote>`)
- Any container element

---

## Notes

- This is a CSS2 property; the newer `break-inside` property provides more options
- The `avoid` value is a suggestion and may be overridden if content is too large for a single page
- Particularly useful for tables, preventing rows from splitting across pages
- Excellent for keeping figures with their captions on the same page
- Helps prevent orphaned list items or code block fragments
- Should be used on container elements rather than inline elements
- Most effective with content that fits reasonably on a single page
- May increase blank space at page bottoms if content cannot fit
- Does not prevent breaks before or after the element, only within it

---

## Data Binding

The `page-break-inside` property supports data binding, allowing you to dynamically control whether content blocks should remain together based on data attributes, content importance, or configuration settings. This is valuable for maintaining content integrity in data-driven documents.

### Example 1: Conditional content grouping

```html
<style>
    .content-item {
        page-break-inside: {{item.keepTogether ? 'avoid' : 'auto'}};
        border: {{item.importance === 'high' ? '2pt solid #dc2626' : '1pt solid #d1d5db'}};
        padding: 15pt;
        margin: 20pt 0;
    }
    .item-header {
        font-weight: bold;
        font-size: {{item.importance === 'high' ? '16pt' : '14pt'}};
    }
</style>
<body>
    {{#each contentItems}}
    <div class="content-item">
        <div class="item-header">{{itemTitle}}</div>
        <div>{{itemContent}}</div>
    </div>
    {{/each}}
</body>
```

### Example 2: Data-driven table integrity

```html
<style>
    .data-table-wrapper {
        page-break-inside: {{tableSettings.preventSplit ? 'avoid' : 'auto'}};
        margin: {{tableSettings.spacing}}pt 0;
    }
    .table-caption {
        font-weight: bold;
        margin-bottom: 10pt;
        color: {{tableSettings.emphasize ? '#1e3a8a' : '#374151'}};
    }
</style>
<body>
    {{#each dataTables}}
    <div class="data-table-wrapper">
        <div class="table-caption">{{caption}}</div>
        <table>
            <thead>
                <tr>
                    {{#each headers}}
                    <th>{{this}}</th>
                    {{/each}}
                </tr>
            </thead>
            <tbody>
                {{#each rows}}
                <tr>
                    {{#each cells}}
                    <td>{{this}}</td>
                    {{/each}}
                </tr>
                {{/each}}
            </tbody>
        </table>
    </div>
    {{/each}}
</body>
```

### Example 3: Variable break avoidance for important content

```html
<style>
    .info-box {
        page-break-inside: {{content.priority === 'critical' ? 'avoid' : 'auto'}};
        background-color: {{content.priority === 'critical' ? '#fef2f2' : '#f9fafb'}};
        border-left: {{content.priority === 'critical' ? '5pt' : '3pt'}} solid
                     {{content.priority === 'critical' ? '#dc2626' : '#3b82f6'}};
        padding: 20pt;
        margin: 25pt 0;
    }
    .box-title {
        font-size: 16pt;
        font-weight: bold;
        margin-bottom: 12pt;
    }
</style>
<body>
    {{#each infoBoxes}}
    <div class="info-box">
        <div class="box-title">{{boxTitle}}</div>
        <div>{{boxContent}}</div>
    </div>
    {{/each}}
</body>
```

---

## Examples

### Example 1: Keep tables together on one page

```html
<style>
    .data-table {
        page-break-inside: avoid;
        width: 100%;
        border-collapse: collapse;
        margin: 20pt 0;
    }
    .data-table th {
        background-color: #1e3a8a;
        color: white;
        padding: 10pt;
        text-align: left;
        font-weight: bold;
    }
    .data-table td {
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
    .table-caption {
        font-weight: bold;
        margin-bottom: 10pt;
        color: #1f2937;
    }
</style>
<body>
    <p>The following table presents quarterly sales data:</p>

    <div style="page-break-inside: avoid;">
        <div class="table-caption">Table 1: Quarterly Sales Performance</div>
        <table class="data-table">
            <thead>
                <tr>
                    <th>Quarter</th>
                    <th>Revenue</th>
                    <th>Growth</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Q1 2025</td>
                    <td>$1,250,000</td>
                    <td>+15%</td>
                </tr>
                <tr>
                    <td>Q2 2025</td>
                    <td>$1,420,000</td>
                    <td>+18%</td>
                </tr>
                <tr>
                    <td>Q3 2025</td>
                    <td>$1,380,000</td>
                    <td>+12%</td>
                </tr>
                <tr>
                    <td>Q4 2025</td>
                    <td>$1,550,000</td>
                    <td>+20%</td>
                </tr>
            </tbody>
        </table>
    </div>

    <p>Analysis of the data follows...</p>
</body>
```

### Example 2: Keep figures with captions together

```html
<style>
    .figure-container {
        page-break-inside: avoid;
        margin: 25pt 0;
        text-align: center;
    }
    .figure-image {
        width: 400pt;
        height: 300pt;
        background-color: #e5e7eb;
        border: 2pt solid #9ca3af;
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 0 auto;
    }
    .figure-caption {
        margin-top: 10pt;
        font-size: 11pt;
        color: #4b5563;
        font-style: italic;
    }
</style>
<body>
    <p>The architectural design incorporates modern elements as shown below.</p>

    <div class="figure-container">
        <div class="figure-image">[Building Design Rendering]</div>
        <div class="figure-caption">
            Figure 1: Proposed building facade showing glass curtain wall system
            and integrated solar panels on the roof structure.
        </div>
    </div>

    <p>The design features several innovative aspects...</p>
</body>
```

### Example 3: Keep code blocks intact

```html
<style>
    .code-block {
        page-break-inside: avoid;
        background-color: #1f2937;
        color: #f9fafb;
        padding: 15pt;
        border-radius: 5pt;
        font-family: 'Courier New', monospace;
        font-size: 10pt;
        margin: 20pt 0;
        overflow-x: auto;
    }
    .code-title {
        font-weight: bold;
        margin-bottom: 10pt;
        color: #60a5fa;
    }
</style>
<body>
    <p>The following code demonstrates the implementation:</p>

    <div class="code-block">
        <div class="code-title">Example 1: User Authentication</div>
        <pre>
function authenticateUser(username, password) {
    const hashedPassword = hash(password);
    const user = database.findUser(username);

    if (user && user.password === hashedPassword) {
        return generateToken(user);
    }

    throw new AuthenticationError('Invalid credentials');
}
        </pre>
    </div>

    <p>This function ensures secure authentication...</p>
</body>
```

### Example 4: Keep quote boxes together

```html
<style>
    .quote-box {
        page-break-inside: avoid;
        background-color: #eff6ff;
        border-left: 5pt solid #3b82f6;
        padding: 20pt;
        margin: 25pt 0;
        font-style: italic;
    }
    .quote-text {
        font-size: 14pt;
        line-height: 1.6;
        color: #1e40af;
    }
    .quote-author {
        text-align: right;
        margin-top: 10pt;
        font-size: 11pt;
        font-weight: bold;
        color: #1e3a8a;
    }
</style>
<body>
    <p>Leadership experts emphasize the importance of communication:</p>

    <div class="quote-box">
        <div class="quote-text">
            "The single biggest problem in communication is the illusion
            that it has taken place. Effective leaders ensure their message
            is not only heard but understood and acted upon."
        </div>
        <div class="quote-author">— George Bernard Shaw</div>
    </div>

    <p>This principle applies to all levels of management...</p>
</body>
```

### Example 5: Keep warning boxes intact

```html
<style>
    .warning-box {
        page-break-inside: avoid;
        background-color: #fef3c7;
        border: 2pt solid #f59e0b;
        border-radius: 8pt;
        padding: 15pt;
        margin: 20pt 0;
    }
    .warning-icon {
        display: inline-block;
        width: 30pt;
        height: 30pt;
        background-color: #f59e0b;
        color: white;
        text-align: center;
        line-height: 30pt;
        border-radius: 50%;
        font-weight: bold;
        margin-right: 10pt;
        vertical-align: middle;
    }
    .warning-title {
        display: inline;
        font-size: 14pt;
        font-weight: bold;
        color: #92400e;
        vertical-align: middle;
    }
    .warning-content {
        margin-top: 10pt;
        color: #78350f;
        line-height: 1.5;
    }
</style>
<body>
    <p>Before proceeding with the installation, please note:</p>

    <div class="warning-box">
        <div>
            <span class="warning-icon">!</span>
            <span class="warning-title">Important Safety Warning</span>
        </div>
        <div class="warning-content">
            Always disconnect power before servicing electrical equipment.
            Failure to follow proper safety procedures may result in serious
            injury or death. Ensure all personnel are trained and certified.
        </div>
    </div>

    <p>Continue with the installation steps...</p>
</body>
```

### Example 6: Keep contact information cards together

```html
<style>
    .contact-card {
        page-break-inside: avoid;
        border: 2pt solid #d1d5db;
        border-radius: 10pt;
        padding: 20pt;
        margin: 15pt 0;
        background-color: #fafafa;
    }
    .contact-name {
        font-size: 16pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 5pt;
    }
    .contact-role {
        font-size: 12pt;
        color: #6b7280;
        margin-bottom: 12pt;
    }
    .contact-details {
        font-size: 10pt;
        line-height: 1.8;
    }
</style>
<body>
    <h2>Emergency Contacts</h2>

    <div class="contact-card">
        <div class="contact-name">Dr. Sarah Johnson</div>
        <div class="contact-role">Medical Director</div>
        <div class="contact-details">
            Phone: (555) 123-4567<br/>
            Email: s.johnson@hospital.com<br/>
            Available: 24/7 for emergencies
        </div>
    </div>

    <div class="contact-card">
        <div class="contact-name">Michael Chen</div>
        <div class="contact-role">Facilities Manager</div>
        <div class="contact-details">
            Phone: (555) 234-5678<br/>
            Email: m.chen@facility.com<br/>
            Available: Monday-Friday 8AM-6PM
        </div>
    </div>
</body>
```

### Example 7: Keep process steps together

```html
<style>
    .process-step {
        page-break-inside: avoid;
        display: flex;
        gap: 15pt;
        margin: 20pt 0;
        padding: 15pt;
        border-left: 4pt solid #2563eb;
        background-color: #f0f9ff;
    }
    .step-number {
        flex-shrink: 0;
        width: 40pt;
        height: 40pt;
        background-color: #2563eb;
        color: white;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 18pt;
        font-weight: bold;
    }
    .step-content {
        flex: 1;
    }
    .step-title {
        font-size: 14pt;
        font-weight: bold;
        margin-bottom: 8pt;
        color: #1e40af;
    }
    .step-description {
        line-height: 1.6;
        color: #1e3a8a;
    }
</style>
<body>
    <h2>Installation Procedure</h2>

    <div class="process-step">
        <div class="step-number">1</div>
        <div class="step-content">
            <div class="step-title">Prepare the Work Area</div>
            <div class="step-description">
                Clear all debris and ensure adequate lighting. Verify that
                all tools and materials are readily available before beginning.
            </div>
        </div>
    </div>

    <div class="process-step">
        <div class="step-number">2</div>
        <div class="step-content">
            <div class="step-title">Install Mounting Brackets</div>
            <div class="step-description">
                Using the provided hardware, secure mounting brackets to the
                wall ensuring they are level and properly anchored.
            </div>
        </div>
    </div>
</body>
```

### Example 8: Keep product specifications together

```html
<style>
    .spec-card {
        page-break-inside: avoid;
        border: 1pt solid #e5e7eb;
        border-radius: 8pt;
        padding: 20pt;
        margin: 20pt 0;
        background-color: white;
        box-shadow: 0 2pt 4pt rgba(0,0,0,0.1);
    }
    .spec-header {
        background-color: #1e3a8a;
        color: white;
        padding: 12pt;
        margin: -20pt -20pt 15pt -20pt;
        border-radius: 8pt 8pt 0 0;
        font-size: 16pt;
        font-weight: bold;
    }
    .spec-row {
        display: flex;
        padding: 8pt 0;
        border-bottom: 1pt solid #f3f4f6;
    }
    .spec-label {
        flex: 0 0 150pt;
        font-weight: bold;
        color: #374151;
    }
    .spec-value {
        flex: 1;
        color: #6b7280;
    }
</style>
<body>
    <h2>Product Specifications</h2>

    <div class="spec-card">
        <div class="spec-header">Technical Specifications - Model XYZ-2000</div>
        <div class="spec-row">
            <div class="spec-label">Dimensions:</div>
            <div class="spec-value">24" x 18" x 36"</div>
        </div>
        <div class="spec-row">
            <div class="spec-label">Weight:</div>
            <div class="spec-value">125 lbs</div>
        </div>
        <div class="spec-row">
            <div class="spec-label">Power:</div>
            <div class="spec-value">120V, 60Hz, 15A</div>
        </div>
        <div class="spec-row">
            <div class="spec-label">Operating Temp:</div>
            <div class="spec-value">32°F to 104°F (0°C to 40°C)</div>
        </div>
        <div class="spec-row">
            <div class="spec-label">Warranty:</div>
            <div class="spec-value">2 years parts and labor</div>
        </div>
    </div>
</body>
```

### Example 9: Keep testimonials intact

```html
<style>
    .testimonial {
        page-break-inside: avoid;
        background: linear-gradient(135deg, #fef3c7 0%, #fde68a 100%);
        border-radius: 10pt;
        padding: 25pt;
        margin: 25pt 0;
        position: relative;
    }
    .testimonial-quote {
        font-size: 13pt;
        line-height: 1.7;
        color: #78350f;
        margin-bottom: 15pt;
        font-style: italic;
    }
    .testimonial-author {
        display: flex;
        align-items: center;
        gap: 15pt;
    }
    .author-photo {
        width: 50pt;
        height: 50pt;
        border-radius: 50%;
        background-color: #f59e0b;
    }
    .author-info {
        flex: 1;
    }
    .author-name {
        font-weight: bold;
        color: #92400e;
        font-size: 12pt;
    }
    .author-title {
        font-size: 10pt;
        color: #b45309;
    }
</style>
<body>
    <h2>Customer Testimonials</h2>

    <div class="testimonial">
        <div class="testimonial-quote">
            "Working with this team has been an absolute pleasure. Their attention
            to detail and commitment to quality exceeded our expectations. The
            project was delivered on time and within budget."
        </div>
        <div class="testimonial-author">
            <div class="author-photo"></div>
            <div class="author-info">
                <div class="author-name">Jennifer Williams</div>
                <div class="author-title">CEO, TechCorp Industries</div>
            </div>
        </div>
    </div>

    <div class="testimonial">
        <div class="testimonial-quote">
            "Exceptional service from start to finish. The team was responsive,
            professional, and delivered results that transformed our business."
        </div>
        <div class="testimonial-author">
            <div class="author-photo"></div>
            <div class="author-info">
                <div class="author-name">Robert Martinez</div>
                <div class="author-title">Director of Operations, Global Solutions</div>
            </div>
        </div>
    </div>
</body>
```

### Example 10: Keep summary boxes together

```html
<style>
    .summary-box {
        page-break-inside: avoid;
        background-color: #ecfdf5;
        border: 2pt solid #10b981;
        border-radius: 8pt;
        padding: 20pt;
        margin: 25pt 0;
    }
    .summary-header {
        font-size: 16pt;
        font-weight: bold;
        color: #065f46;
        margin-bottom: 15pt;
        display: flex;
        align-items: center;
        gap: 10pt;
    }
    .summary-icon {
        width: 30pt;
        height: 30pt;
        background-color: #10b981;
        color: white;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 16pt;
    }
    .summary-list {
        list-style-type: none;
        padding: 0;
    }
    .summary-list li {
        padding: 8pt 0;
        padding-left: 25pt;
        position: relative;
    }
    .summary-list li:before {
        content: "✓";
        position: absolute;
        left: 0;
        color: #10b981;
        font-weight: bold;
    }
</style>
<body>
    <p>After reviewing the quarterly performance data, we conclude:</p>

    <div class="summary-box">
        <div class="summary-header">
            <div class="summary-icon">✓</div>
            <span>Key Findings</span>
        </div>
        <ul class="summary-list">
            <li>Revenue increased by 23% compared to previous quarter</li>
            <li>Customer satisfaction scores improved to 4.8/5.0</li>
            <li>Operating costs reduced by 12% through efficiency gains</li>
            <li>Market share expanded in three key regions</li>
            <li>Employee retention rate reached 95%</li>
        </ul>
    </div>

    <p>These results demonstrate strong organizational performance...</p>
</body>
```

### Example 11: Keep calendar entries together

```html
<style>
    .calendar-entry {
        page-break-inside: avoid;
        border-left: 5pt solid #8b5cf6;
        background-color: #faf5ff;
        padding: 15pt;
        margin: 15pt 0;
    }
    .entry-date {
        font-size: 12pt;
        font-weight: bold;
        color: #6b21a8;
        margin-bottom: 8pt;
    }
    .entry-title {
        font-size: 14pt;
        font-weight: bold;
        color: #5b21b6;
        margin-bottom: 5pt;
    }
    .entry-details {
        font-size: 10pt;
        color: #7c3aed;
        line-height: 1.6;
    }
</style>
<body>
    <h2>Upcoming Events</h2>

    <div class="calendar-entry">
        <div class="entry-date">Monday, March 15, 2025 - 10:00 AM</div>
        <div class="entry-title">Quarterly Planning Meeting</div>
        <div class="entry-details">
            Location: Conference Room A<br/>
            Attendees: Executive Team<br/>
            Duration: 2 hours<br/>
            Agenda: Q2 objectives, budget review, strategic initiatives
        </div>
    </div>

    <div class="calendar-entry">
        <div class="entry-date">Wednesday, March 17, 2025 - 2:00 PM</div>
        <div class="entry-title">Client Presentation</div>
        <div class="entry-details">
            Location: Client Site<br/>
            Attendees: Sales Team, Client Executives<br/>
            Duration: 90 minutes<br/>
            Topic: Product roadmap and implementation timeline
        </div>
    </div>
</body>
```

### Example 12: Keep pricing tables intact

```html
<style>
    .pricing-tier {
        page-break-inside: avoid;
        border: 2pt solid #d1d5db;
        border-radius: 10pt;
        padding: 20pt;
        margin: 20pt 0;
        text-align: center;
        background-color: white;
    }
    .tier-name {
        font-size: 20pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 10pt;
    }
    .tier-price {
        font-size: 36pt;
        font-weight: bold;
        color: #2563eb;
        margin: 15pt 0;
    }
    .price-period {
        font-size: 12pt;
        color: #6b7280;
    }
    .features-list {
        text-align: left;
        list-style-type: none;
        padding: 0;
        margin: 20pt 0;
    }
    .features-list li {
        padding: 8pt 0;
        padding-left: 25pt;
        position: relative;
    }
    .features-list li:before {
        content: "✓";
        position: absolute;
        left: 0;
        color: #10b981;
        font-weight: bold;
    }
</style>
<body>
    <h2>Pricing Plans</h2>

    <div class="pricing-tier">
        <div class="tier-name">Professional</div>
        <div class="tier-price">$49<span class="price-period">/month</span></div>
        <ul class="features-list">
            <li>Up to 10 users</li>
            <li>50GB storage</li>
            <li>Email support</li>
            <li>Basic analytics</li>
            <li>API access</li>
        </ul>
    </div>

    <div class="pricing-tier">
        <div class="tier-name">Enterprise</div>
        <div class="tier-price">$199<span class="price-period">/month</span></div>
        <ul class="features-list">
            <li>Unlimited users</li>
            <li>1TB storage</li>
            <li>24/7 phone support</li>
            <li>Advanced analytics</li>
            <li>API access</li>
            <li>Custom integrations</li>
            <li>Dedicated account manager</li>
        </ul>
    </div>
</body>
```

### Example 13: Keep FAQ items together

```html
<style>
    .faq-item {
        page-break-inside: avoid;
        margin: 20pt 0;
        padding: 15pt;
        background-color: #f9fafb;
        border-radius: 8pt;
    }
    .faq-question {
        font-size: 14pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 10pt;
        display: flex;
        align-items: start;
        gap: 10pt;
    }
    .faq-q-icon {
        flex-shrink: 0;
        width: 24pt;
        height: 24pt;
        background-color: #2563eb;
        color: white;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: bold;
    }
    .faq-answer {
        padding-left: 34pt;
        line-height: 1.6;
        color: #374151;
    }
</style>
<body>
    <h2>Frequently Asked Questions</h2>

    <div class="faq-item">
        <div class="faq-question">
            <div class="faq-q-icon">Q</div>
            <span>What is the return policy?</span>
        </div>
        <div class="faq-answer">
            We offer a 30-day money-back guarantee on all purchases. If you're
            not satisfied with your product, simply contact our support team
            to initiate a return. The item must be in original condition.
        </div>
    </div>

    <div class="faq-item">
        <div class="faq-question">
            <div class="faq-q-icon">Q</div>
            <span>Do you offer technical support?</span>
        </div>
        <div class="faq-answer">
            Yes, we provide comprehensive technical support through multiple
            channels including email, phone, and live chat. Premium customers
            receive 24/7 support with guaranteed response times.
        </div>
    </div>
</body>
```

### Example 14: Keep ingredient lists with recipes

```html
<style>
    .recipe-section {
        page-break-inside: avoid;
        margin: 25pt 0;
    }
    .recipe-heading {
        font-size: 20pt;
        font-weight: bold;
        color: #be123c;
        margin-bottom: 15pt;
        padding-bottom: 10pt;
        border-bottom: 2pt solid #be123c;
    }
    .ingredients-box {
        background-color: #fef2f2;
        border: 2pt solid #f87171;
        border-radius: 8pt;
        padding: 15pt;
        margin-bottom: 20pt;
    }
    .ingredients-title {
        font-size: 14pt;
        font-weight: bold;
        color: #991b1b;
        margin-bottom: 10pt;
    }
    .ingredients-list {
        list-style-type: disc;
        margin-left: 20pt;
        color: #7f1d1d;
    }
    .instructions {
        line-height: 1.8;
    }
</style>
<body>
    <div class="recipe-section">
        <h2 class="recipe-heading">Classic Spaghetti Carbonara</h2>

        <div class="ingredients-box">
            <div class="ingredients-title">Ingredients (Serves 4)</div>
            <ul class="ingredients-list">
                <li>400g spaghetti</li>
                <li>200g pancetta or guanciale, diced</li>
                <li>4 large eggs</li>
                <li>100g Pecorino Romano, grated</li>
                <li>Black pepper to taste</li>
                <li>Salt for pasta water</li>
            </ul>
        </div>

        <div class="instructions">
            <strong>Instructions:</strong>
            <ol>
                <li>Bring a large pot of salted water to boil</li>
                <li>Cook pasta according to package directions</li>
                <li>Meanwhile, cook pancetta until crispy</li>
                <li>Beat eggs with cheese and black pepper</li>
            </ol>
        </div>
    </div>
</body>
```

### Example 15: Keep sidebar notes with content

```html
<style>
    .content-with-sidebar {
        page-break-inside: avoid;
        display: flex;
        gap: 20pt;
        margin: 25pt 0;
    }
    .main-content {
        flex: 1;
    }
    .sidebar-note {
        flex: 0 0 180pt;
        background-color: #fef9c3;
        border: 1pt solid #fcd34d;
        border-radius: 5pt;
        padding: 15pt;
        font-size: 10pt;
    }
    .note-title {
        font-weight: bold;
        color: #92400e;
        margin-bottom: 8pt;
        font-size: 11pt;
    }
    .note-content {
        color: #78350f;
        line-height: 1.5;
    }
</style>
<body>
    <div class="content-with-sidebar">
        <div class="main-content">
            <h3>Network Configuration</h3>
            <p>
                To configure the network settings, access the administration
                panel and navigate to Network > Settings. Ensure that the
                subnet mask matches your network topology.
            </p>
            <p>
                Default gateway should be set to your router's IP address,
                typically 192.168.1.1 for home networks.
            </p>
        </div>
        <div class="sidebar-note">
            <div class="note-title">Important Note</div>
            <div class="note-content">
                Always backup your current configuration before making changes.
                Incorrect network settings may result in loss of connectivity.
            </div>
        </div>
    </div>
</body>
```

---

## See Also

- [page-break-before](/reference/cssproperties/css_prop_page-break-before) - Control page breaks before elements
- [page-break-after](/reference/cssproperties/css_prop_page-break-after) - Control page breaks after elements
- [break-inside](/reference/cssproperties/css_prop_break-inside) - Modern alternative with more options
- [break-before](/reference/cssproperties/css_prop_break-before) - Modern page and column break control
- [break-after](/reference/cssproperties/css_prop_break-after) - Modern page and column break control

---
