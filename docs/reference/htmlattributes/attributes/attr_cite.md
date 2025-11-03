---
layout: default
title: cite
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @cite : The Citation Source Attribute

The `cite` attribute specifies the URL of a source document or resource that explains a quotation, insertion, or deletion. It provides attribution and reference information for `<blockquote>`, `<q>`, `<ins>`, and `<del>` elements, enabling proper citation and documentation of content changes in PDF documents.

## Usage

The `cite` attribute defines source URLs:
- Provides attribution for quoted content in `<blockquote>` and `<q>` elements
- Documents the source of inserted content in `<ins>` elements
- References the reason for deleted content in `<del>` elements
- Accepts absolute or relative URLs
- Supports data binding for dynamic source references
- Not visually displayed by default but accessible in document metadata

```html
<!-- Citation for blockquote -->
<blockquote cite="https://example.com/article">
    This is a quoted passage from an external source.
</blockquote>

<!-- Citation for inline quote -->
<q cite="https://example.com/quote">Brief quotation</q>

<!-- Citation for insertion -->
<ins cite="https://example.com/change-log" datetime="2025-01-15">
    New content added based on review.
</ins>

<!-- Citation for deletion -->
<del cite="https://example.com/errata" datetime="2025-01-15">
    Incorrect information removed.
</del>

<!-- Dynamic citation -->
<blockquote cite="{{model.sourceUrl}}">
    {{model.quotation}}
</blockquote>
```

---

## Supported Elements

The `cite` attribute is used with:

### Quotation Elements
- `<blockquote>` - Block-level quotation with source citation
- `<q>` - Inline quotation with source citation

### Change Tracking Elements
- `<ins>` - Inserted content with source reference
- `<del>` - Deleted content with source reference

---

## Binding Values

The `cite` attribute supports data binding for dynamic source URLs:

```html
<!-- Dynamic citation URL -->
<blockquote cite="{{model.article.sourceUrl}}">
    {{model.article.quote}}
</blockquote>

<!-- Constructed URL -->
<q cite="https://example.com/quotes/{{model.quoteId}}">
    {{model.quoteText}}
</q>

<!-- Conditional citation -->
<ins cite="{{model.hasSource ? model.sourceUrl : '#'}}" datetime="{{model.dateAdded}}">
    {{model.newContent}}
</ins>

<!-- Citation from array of sources -->
<template data-bind="{{model.citations}}">
    <blockquote cite="{{.url}}">
        <p>{{.text}}</p>
        <footer>— {{.author}}</footer>
    </blockquote>
</template>

<!-- Change log with citations -->
<template data-bind="{{model.changes}}">
    <del cite="{{.reason}}" datetime="{{.date}}">{{.oldValue}}</del>
    <ins cite="{{.reason}}" datetime="{{.date}}">{{.newValue}}</ins>
</template>
```

**Data Model Example:**
```json
{
  "article": {
    "sourceUrl": "https://example.com/article/2025",
    "quote": "This is an important statement."
  },
  "quoteId": "q12345",
  "quoteText": "Brief quote",
  "hasSource": true,
  "sourceUrl": "https://example.com/source",
  "dateAdded": "2025-01-15T10:00:00",
  "newContent": "Updated information",
  "citations": [
    {
      "url": "https://example.com/ref1",
      "text": "First citation",
      "author": "John Doe"
    }
  ],
  "changes": [
    {
      "oldValue": "Original text",
      "newValue": "Updated text",
      "reason": "https://example.com/changelog",
      "date": "2025-01-15"
    }
  ]
}
```

---

## Notes

### Purpose of Cite Attribute

The `cite` attribute serves several purposes:

1. **Attribution**: Credits the original source of quoted content
2. **Documentation**: Records the reason or source for content changes
3. **Reference**: Provides URL for readers to verify or learn more
4. **Metadata**: Adds semantic information to the document

```html
<!-- Attribution for quote -->
<blockquote cite="https://example.com/article">
    <p>Important industry insight from expert analysis.</p>
    <footer>Source: Industry Report 2025</footer>
</blockquote>

<!-- Documentation of change -->
<p>
    The deadline was <del cite="https://example.com/meeting-notes" datetime="2025-01-15">
    January 20</del> <ins cite="https://example.com/meeting-notes" datetime="2025-01-15">
    January 25</ins> due to schedule adjustment.
</p>
```

### Not Visually Displayed

The `cite` attribute value is **not displayed** in the rendered PDF by default:

```html
<!-- The URL is in metadata but not shown -->
<blockquote cite="https://example.com/source">
    This quote appears in the PDF, but the cite URL does not.
</blockquote>

<!-- To display the source, add it explicitly -->
<blockquote cite="https://example.com/source">
    <p>This quote appears in the PDF.</p>
    <footer>
        Source: <a href="https://example.com/source">example.com/source</a>
    </footer>
</blockquote>
```

### URL Format

The `cite` attribute accepts various URL formats:

```html
<!-- Absolute URL -->
<blockquote cite="https://www.example.com/article">
    Quote text
</blockquote>

<!-- Relative URL -->
<blockquote cite="/articles/2025/january/article-name">
    Quote text
</blockquote>

<!-- URL with fragment -->
<blockquote cite="https://example.com/article#section2">
    Quote text from specific section
</blockquote>

<!-- DOI (Digital Object Identifier) -->
<blockquote cite="https://doi.org/10.1234/example.2025.01">
    Academic citation
</blockquote>

<!-- ISBN reference -->
<blockquote cite="urn:isbn:978-0-123456-78-9">
    Book quotation
</blockquote>
```

### Combining with Other Attributes

The `cite` attribute works with other attributes:

```html
<!-- Cite with datetime for insertions/deletions -->
<ins cite="https://example.com/changelog" datetime="2025-01-15T14:30:00">
    New content added
</ins>

<!-- Cite with class for styling -->
<blockquote cite="https://example.com/source" class="important-quote">
    <p>Critical information</p>
</blockquote>

<!-- Cite with id for linking -->
<blockquote id="quote1" cite="https://example.com/article">
    <p>Referenced quote</p>
</blockquote>
<p>As mentioned in the <a href="#quote1">quotation above</a>...</p>

<!-- Cite with title attribute -->
<blockquote cite="https://example.com/source" title="From: Industry Report 2025">
    <p>Statistical data</p>
</blockquote>
```

### Accessibility and Metadata

While not visually displayed, the `cite` attribute:
- Adds semantic meaning to the document
- May be used by PDF readers or tools for metadata extraction
- Helps with document analysis and indexing
- Provides context for automated processing

### Blockquote vs Q Element

Both support `cite`, but differ in usage:

```html
<!-- Blockquote for long, block-level quotes -->
<blockquote cite="https://example.com/article">
    <p>This is a longer quotation that spans multiple sentences
    or paragraphs. It is displayed as a block element with
    appropriate spacing and styling.</p>
</blockquote>

<!-- Q for short, inline quotes -->
<p>
    The report states <q cite="https://example.com/report">
    sales increased by 25%</q> in the fourth quarter.
</p>
```

### Version Control and Change Tracking

The `cite` attribute is particularly useful for documenting changes:

```html
<!-- Track content changes with sources -->
<p>
    Product price: <del cite="https://internal/pricing-update-2025-01">
    $99.99</del> <ins cite="https://internal/pricing-update-2025-01">
    $89.99</ins>
</p>

<!-- Document corrections -->
<p>
    The conference will be held in
    <del cite="https://example.com/errata" datetime="2025-01-10">
    New York</del> <ins cite="https://example.com/errata" datetime="2025-01-10">
    Boston</ins>.
</p>
```

### Citation vs Footer Attribution

For visible attribution, use `<footer>` within `<blockquote>`:

```html
<!-- Cite attribute (not visible) -->
<blockquote cite="https://example.com/article">
    <p>Invisible source URL in metadata.</p>
</blockquote>

<!-- Footer attribution (visible) -->
<blockquote cite="https://example.com/article">
    <p>The only way to do great work is to love what you do.</p>
    <footer>
        — Steve Jobs,
        <cite><a href="https://example.com/article">Stanford Commencement, 2005</a></cite>
    </footer>
</blockquote>
```

Note: `<cite>` element (for titles) is different from `cite` attribute (for URLs).

### Empty or Missing Cite

The `cite` attribute is optional:

```html
<!-- Valid without cite -->
<blockquote>
    <p>Quote without source reference</p>
</blockquote>

<!-- Empty cite (avoid) -->
<blockquote cite="">
    <p>Better to omit cite than leave it empty</p>
</blockquote>

<!-- With cite for proper attribution -->
<blockquote cite="https://example.com/source">
    <p>Quote with proper source reference</p>
</blockquote>
```

---

## Examples

### Basic Blockquote Citation

```html
<article>
    <h1>Industry Analysis</h1>

    <p>According to recent research:</p>

    <blockquote cite="https://example.com/reports/2025/industry-trends">
        <p>
            The market is expected to grow by 35% over the next five years,
            driven primarily by technological innovation and increased consumer
            demand for sustainable solutions.
        </p>
    </blockquote>

    <p>This trend is consistent across multiple sectors.</p>
</article>
```

### Blockquote with Visible Attribution

```html
<article>
    <h1>Leadership Insights</h1>

    <blockquote cite="https://example.com/interviews/ceo-2025">
        <p>
            Innovation distinguishes between a leader and a follower.
            We must continually push boundaries and challenge the status quo
            to remain competitive in today's fast-paced market.
        </p>
        <footer>
            — Jane Smith, CEO<br/>
            Source: <cite><a href="https://example.com/interviews/ceo-2025">
            Annual Leadership Conference 2025</a></cite>
        </footer>
    </blockquote>
</article>
```

### Inline Quote Citation

```html
<article>
    <h1>Market Update</h1>

    <p>
        The quarterly report indicates that
        <q cite="https://example.com/reports/q4-2024">revenue increased by 23%
        compared to the same period last year</q>, marking the strongest
        performance in company history.
    </p>

    <p>
        According to the CFO,
        <q cite="https://example.com/press-releases/2025-01-15">this growth
        reflects our strategic investments in product development and market
        expansion</q>.
    </p>
</article>
```

### Document Revisions with Cite

```html
<article>
    <h1>Policy Document</h1>

    <h2>Section 3: Work Hours</h2>

    <p>
        Standard work hours are
        <del cite="https://internal.example.com/policy-update-2025-01"
             datetime="2025-01-15T09:00:00">
        9:00 AM to 5:00 PM</del>
        <ins cite="https://internal.example.com/policy-update-2025-01"
             datetime="2025-01-15T09:00:00">
        flexible between 7:00 AM and 7:00 PM</ins>
        to accommodate diverse employee needs.
    </p>

    <h2>Section 5: Remote Work</h2>

    <p>
        Employees may work remotely up to
        <del cite="https://internal.example.com/policy-update-2025-01"
             datetime="2025-01-15T09:00:00">
        two days per week</del>
        <ins cite="https://internal.example.com/policy-update-2025-01"
             datetime="2025-01-15T09:00:00">
        three days per week</ins>
        with manager approval.
    </p>
</article>
```

### Academic Paper Citations

```html
<article>
    <h1>Research Paper: Climate Change Impact</h1>

    <section>
        <h2>Literature Review</h2>

        <p>Previous studies have established a clear correlation:</p>

        <blockquote cite="https://doi.org/10.1234/climate.2024.001">
            <p>
                Global temperature increases of 1.5°C above pre-industrial levels
                will result in significant ecosystem disruption, affecting
                approximately 70-90% of coral reefs worldwide.
            </p>
            <footer>
                <cite>Johnson et al. (2024). Climate Impact Assessment.
                <a href="https://doi.org/10.1234/climate.2024.001">
                DOI: 10.1234/climate.2024.001</a></cite>
            </footer>
        </blockquote>

        <blockquote cite="https://doi.org/10.5678/ocean.2024.042">
            <p>
                Ocean acidification rates have accelerated by 30% over the past
                decade, with pH levels declining at unprecedented rates in
                historical context.
            </p>
            <footer>
                <cite>Smith & Chen (2024). Ocean Chemistry Trends.
                <a href="https://doi.org/10.5678/ocean.2024.042">
                DOI: 10.5678/ocean.2024.042</a></cite>
            </footer>
        </blockquote>
    </section>
</article>
```

### News Article with Multiple Citations

```html
<article>
    <h1>Tech Industry Developments</h1>

    <p>
        Industry analysts predict significant changes ahead. As noted in
        a recent report, <q cite="https://example.com/tech-news/2025-01-15">
        artificial intelligence will transform business operations across
        all sectors by 2030</q>.
    </p>

    <blockquote cite="https://example.com/analyst-reports/ai-impact-2025">
        <p>
            Companies that fail to adopt AI technologies will find themselves
            at a severe competitive disadvantage within the next five years.
            Early adopters are already seeing productivity gains of 40-50%.
        </p>
        <footer>
            — TechAnalyst Group,
            <cite><a href="https://example.com/analyst-reports/ai-impact-2025">
            AI Impact Report 2025</a></cite>
        </footer>
    </blockquote>

    <p>
        However, concerns remain. <q cite="https://example.com/ethics/ai-governance">
        Without proper governance frameworks, AI deployment poses significant
        ethical and societal risks</q>, according to technology ethicists.
    </p>
</article>
```

### Legal Document Changes

```html
<article>
    <h1>Terms of Service</h1>
    <p><em>Last Updated: January 15, 2025</em></p>

    <h2>Section 2: User Obligations</h2>

    <p>
        Users must maintain account security.
        <del cite="https://legal.example.com/tos-amendments/2025-01"
             datetime="2025-01-15">
        Passwords must be changed every 90 days.</del>
        <ins cite="https://legal.example.com/tos-amendments/2025-01"
             datetime="2025-01-15">
        Multi-factor authentication is required for all accounts.</ins>
    </p>

    <h2>Section 4: Data Retention</h2>

    <p>
        <del cite="https://legal.example.com/tos-amendments/2025-01"
             datetime="2025-01-15">
        User data is retained for 12 months after account closure.</del>
        <ins cite="https://legal.example.com/tos-amendments/2025-01"
             datetime="2025-01-15">
        User data is retained for 30 days after account closure, after which
        it is permanently deleted.</ins>
    </p>

    <p style="margin-top: 20pt; font-size: 9pt; color: #666;">
        For details on these changes, see:
        <a href="https://legal.example.com/tos-amendments/2025-01">
        Amendment Notice - January 2025</a>
    </p>
</article>
```

### Data-Bound Citations

```html
<!-- Model: {
    quotes: [
        {
            text: "Excellence is not an act, but a habit.",
            author: "Aristotle",
            source: "https://example.com/philosophy/aristotle",
            title: "Nicomachean Ethics"
        },
        {
            text: "The unexamined life is not worth living.",
            author: "Socrates",
            source: "https://example.com/philosophy/socrates",
            title: "Apology"
        }
    ]
} -->

<article>
    <h1>Philosophical Insights</h1>

    <template data-bind="{{model.quotes}}">
        <blockquote cite="{{.source}}" style="margin: 20pt 0;">
            <p style="font-style: italic;">{{.text}}</p>
            <footer>
                — {{.author}},
                <cite><a href="{{.source}}">{{.title}}</a></cite>
            </footer>
        </blockquote>
    </template>
</article>
```

### Book Review with Citations

```html
<article>
    <h1>Book Review: "Future of Technology"</h1>

    <p>
        The author makes a compelling argument in Chapter 3:
    </p>

    <blockquote cite="urn:isbn:978-0-123456-78-9">
        <p>
            Technological advancement is not merely about creating new tools,
            but about fundamentally rethinking how we approach problems and
            interact with the world around us.
        </p>
        <footer>
            <cite>Future of Technology, Chapter 3, p. 87<br/>
            ISBN: 978-0-123456-78-9</cite>
        </footer>
    </blockquote>

    <p>
        This perspective is particularly relevant when considering
        <q cite="urn:isbn:978-0-123456-78-9">the intersection of human
        creativity and machine learning</q> as discussed in later chapters.
    </p>
</article>
```

### Technical Documentation Updates

```html
<article>
    <h1>API Documentation - Version 2.0</h1>

    <h2>Authentication</h2>

    <p>
        <del cite="https://docs.example.com/changelog/v2.0"
             datetime="2025-01-15">
        API keys are passed in the URL query string as <code>?apikey=XXX</code>
        </del>
        <ins cite="https://docs.example.com/changelog/v2.0"
             datetime="2025-01-15">
        API keys must be passed in the <code>Authorization</code> header as
        <code>Bearer TOKEN</code>
        </ins>
    </p>

    <h2>Rate Limiting</h2>

    <p>
        <del cite="https://docs.example.com/changelog/v2.0"
             datetime="2025-01-15">
        Rate limit: 100 requests per hour
        </del>
        <ins cite="https://docs.example.com/changelog/v2.0"
             datetime="2025-01-15">
        Rate limit: 1000 requests per hour for standard tier,
        10000 requests per hour for premium tier
        </ins>
    </p>

    <p style="margin-top: 20pt; padding: 10pt; background-color: #f0f0f0;">
        <strong>Migration Note:</strong> For details on upgrading from v1.x to v2.0,
        see <a href="https://docs.example.com/changelog/v2.0">
        Version 2.0 Changelog</a>.
    </p>
</article>
```

### Historical Document Analysis

```html
<article>
    <h1>Historical Analysis: Industrial Revolution</h1>

    <section>
        <h2>Primary Sources</h2>

        <blockquote cite="https://archive.org/details/industrial-revolution-1850">
            <p>
                The introduction of steam power fundamentally altered not only
                manufacturing processes but the very fabric of society,
                relocating populations from rural areas to urban centers in
                unprecedented numbers.
            </p>
            <footer>
                <cite>Parliamentary Report on Industrial Conditions, 1850<br/>
                <a href="https://archive.org/details/industrial-revolution-1850">
                Archive.org Reference</a></cite>
            </footer>
        </blockquote>

        <p>
            Contemporary observers noted the rapid pace of change.
            <q cite="https://digitallibrary.example.com/journals/1849">
            Within a single generation, the landscape of Britain has been
            transformed beyond recognition</q>, wrote one journalist in 1849.
        </p>
    </section>
</article>
```

### Product Specifications with Revisions

```html
<article>
    <h1>Product Specifications - Model X1000</h1>

    <h2>Technical Details</h2>

    <table style="width: 100%; border-collapse: collapse;">
        <tr>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Processor</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">
                <del cite="https://specs.example.com/updates/2025-01"
                     datetime="2025-01-15">
                Quad-core 2.4 GHz
                </del>
                <ins cite="https://specs.example.com/updates/2025-01"
                     datetime="2025-01-15">
                Octa-core 3.2 GHz
                </ins>
            </td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Memory</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">
                <del cite="https://specs.example.com/updates/2025-01"
                     datetime="2025-01-15">
                8 GB RAM
                </del>
                <ins cite="https://specs.example.com/updates/2025-01"
                     datetime="2025-01-15">
                16 GB RAM
                </ins>
            </td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Storage</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">
                <del cite="https://specs.example.com/updates/2025-01"
                     datetime="2025-01-15">
                256 GB SSD
                </del>
                <ins cite="https://specs.example.com/updates/2025-01"
                     datetime="2025-01-15">
                512 GB SSD
                </ins>
            </td>
        </tr>
    </table>

    <p style="margin-top: 15pt; font-size: 9pt; color: #666;">
        Specifications updated January 15, 2025. See
        <a href="https://specs.example.com/updates/2025-01">
        detailed change log</a> for more information.
    </p>
</article>
```

### Multiple Source Citations

```html
<article>
    <h1>Climate Change: A Multi-Source Analysis</h1>

    <section>
        <h2>Scientific Consensus</h2>

        <blockquote cite="https://doi.org/10.1234/ipcc.2024.ar6">
            <p>
                Human influence has warmed the climate at a rate that is
                unprecedented in at least the last 2000 years.
            </p>
            <footer>
                <cite>IPCC Sixth Assessment Report, 2024</cite>
            </footer>
        </blockquote>

        <blockquote cite="https://doi.org/10.5678/nature.2024.12345">
            <p>
                Global mean surface temperature has increased by approximately
                1.1°C since the pre-industrial period.
            </p>
            <footer>
                <cite>Nature Climate Study, 2024</cite>
            </footer>
        </blockquote>

        <blockquote cite="https://doi.org/10.9012/science.2024.67890">
            <p>
                Without significant mitigation efforts, temperature increases
                of 2.5-4°C are projected by 2100.
            </p>
            <footer>
                <cite>Science Climate Projections, 2024</cite>
            </footer>
        </blockquote>
    </section>
</article>
```

### Interview Quotes with Citations

```html
<article>
    <h1>Expert Interview: Dr. Sarah Johnson</h1>
    <p><em>Interview conducted January 10, 2025</em></p>

    <p><strong>Q: What are the biggest challenges facing the industry?</strong></p>

    <blockquote cite="https://example.com/interviews/johnson-2025-01">
        <p>
            The primary challenge is balancing rapid innovation with
            regulatory compliance. We're seeing technology evolve faster
            than our ability to establish appropriate governance frameworks.
        </p>
        <footer>— Dr. Sarah Johnson</footer>
    </blockquote>

    <p><strong>Q: How should companies approach this?</strong></p>

    <blockquote cite="https://example.com/interviews/johnson-2025-01">
        <p>
            Companies need to be proactive rather than reactive. Build
            compliance and ethics into your development process from day one,
            rather than treating them as afterthoughts.
        </p>
        <footer>— Dr. Sarah Johnson</footer>
    </blockquote>

    <p style="margin-top: 20pt; font-size: 9pt; color: #666;">
        Full interview available at:
        <a href="https://example.com/interviews/johnson-2025-01">
        example.com/interviews/johnson-2025-01</a>
    </p>
</article>
```

---

## See Also

- [blockquote](/reference/htmltags/blockquote.html) - Block quotation element
- [q](/reference/htmltags/q.html) - Inline quotation element
- [ins](/reference/htmltags/ins.html) - Inserted content element
- [del](/reference/htmltags/del.html) - Deleted content element
- [datetime](/reference/htmlattributes/datetime.html) - Date/time attribute for ins and del
- [a](/reference/htmltags/a.html) - Anchor element for links
- [footer](/reference/htmltags/footer.html) - Footer element for attribution

---
