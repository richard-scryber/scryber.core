# WWF Tigers Alive Annual Report - Scryber Template

This directory contains a complete Scryber XHTML template based on the 2024 WWF Tigers Alive Annual Report. The template demonstrates how to create a professional, multi-section conservation report with dynamic data binding from JSON.

**Key Highlights:**
- **Clean Separation**: Template (HTML), Styles (CSS), and Data (JSON) in separate files
- **Super Simple**: Just 3 lines of code to load JSON and generate PDF
- **Easy Customization**: Edit CSS for styling, JSON for content - no code changes needed
- **Automatic Binding**: Scryber handles all data binding, iteration, and formatting
- **Production Ready**: Complete real-world example with 5,500+ word report

## Files Included

1. **wwf_tigers_report_template.html** - The main XHTML template file
2. **wwf_tigers_report_styles.css** - External CSS stylesheet with all styling
3. **wwf_tigers_report_data.json** - JSON data file with all report content
4. **wwf_tigers_report_model.cs** - C# code to load JSON and generate PDF
5. **wwf_tigers_report_README.md** - This documentation file

## Template Features

### Design Elements

- **Professional Layout**: Clean, modern design with WWF brand colors (orange, black, white)
- **Hero Section**: Eye-catching opening with key statistics
- **Structured Sections**: Four main thematic sections with consistent styling
- **Data Visualization**: Statistics boxes, tables, and trend indicators
- **Typography**: Clear hierarchy with multiple heading levels
- **Page Breaks**: Properly placed for PDF generation

### Content Sections

1. **Hero Section**
   - Report title and year
   - Three key statistics displayed prominently
   - Dramatic black gradient background

2. **Introduction**
   - Opening context
   - Global tiger statistics
   - Major milestones
   - Country-by-country trends table

3. **Section 1: Expand Tiger Range**
   - Success stories from Kazakhstan, China, Thailand, and Laos
   - Tiger reintroduction and natural expansion examples
   - Statistics and key facts for each story

4. **Section 2: Secure Connected Habitat**
   - Wildlife corridor success stories from Nepal and India
   - Transboundary conservation efforts
   - Ranger workforce gap statistics

5. **Section 3: End Exploitation**
   - Anti-poaching initiatives
   - Wildlife trafficking combat efforts
   - Law enforcement strengthening

6. **Section 4: Towards Coexistence**
   - Conflict to Coexistence (C2C) framework
   - Community engagement programs from Nepal, China, Thailand, Kazakhstan
   - Behavior change initiatives

7. **Looking Forward**
   - Future commitments
   - Closing message
   - Key priorities for next decade

8. **Footer**
   - Organization information
   - Publication date
   - Contact details

### Styling Components

The template includes comprehensive CSS styling for:

- **Layout**: Section headers, content blocks, page breaks
- **Color Scheme**: WWF brand colors throughout
- **Typography**: Multiple font sizes and weights for hierarchy
- **Statistics Boxes**: Highlighted data with labels and values
- **Quote Boxes**: Styled pull quotes with attribution
- **Success Story Boxes**: Bordered containers for case studies
- **Tables**: Clean, striped tables for data presentation
- **Highlight Boxes**: Call-out boxes for important information
- **Trend Indicators**: Color-coded up/down trends (green/red)

## How to Use

### Basic Usage with JSON Data

The recommended approach is to use a JSON file for data, which separates content from code:

```csharp
using WwfTigersReport;

// Generate report using the default files
TigersReportGenerator.GenerateReport();

// Or specify custom file paths
TigersReportGenerator.GenerateReport(
    "wwf_tigers_report_data.json",      // JSON data file
    "wwf_tigers_report_template.html",   // XHTML template
    "WWF_Tigers_Report_2024.pdf"         // Output PDF
);
```

### JSON Data Structure

The JSON data file (`wwf_tigers_report_data.json`) contains all report content structured as:

```json
{
  "reportYear": 2024,
  "globalStats": {
    "currentPopulation": 5574,
    "baseline2010": 3200,
    "percentIncrease": 74,
    "percentHistoricRange": 8,
    "countriesIncreasing": 6
  },
  "introduction": {
    "openingText": "...",
    "milestones": [
      {
        "title": "...",
        "description": "..."
      }
    ],
    "countryTrends": [
      {
        "country": "...",
        "increasing": true,
        "notes": "..."
      }
    ]
  },
  "sections": {
    "expandRange": { "..." },
    "secureHabitat": { "..." },
    "endExploitation": { "..." },
    "coexistence": { "..." }
  },
  "conclusion": { "..." },
  "footer": {
    "organizationInfo": "...",
    "publishDate": "2024-12-01T00:00:00",
    "website": "..."
  }
}
```

See `wwf_tigers_report_data.json` for the complete JSON structure with all content from the 2024 report.

### Advanced: Manual Data Loading

If you prefer to construct the data model in C# instead of JSON:

```csharp
using System;
using System.IO;
using Scryber.Components;

using (var doc = Document.ParseDocument("wwf_tigers_report_template.html"))
{
    // Populate with your data object
    doc.Params["model"] = new
    {
        reportYear = 2024,
        globalStats = new { /* ... */ },
        introduction = new { /* ... */ },
        sections = new { /* ... */ },
        conclusion = new { /* ... */ },
        footer = new { /* ... */ }
    };

    // SaveAsPDF handles all processing automatically
    using (var stream = new FileStream("output.pdf", FileMode.Create))
    {
        doc.SaveAsPDF(stream);
    }
}
```

### How the JSON Parsing Works

The `TigersReportGenerator` class is remarkably simple - Scryber handles all the JSON binding automatically:

1. **Read JSON file** using `File.ReadAllText()`
2. **Deserialize JSON** with `JsonSerializer.Deserialize<object>()`
3. **Pass to template** via `doc.Params["model"]`
4. **Generate PDF** with `doc.SaveAsPDF()`

That's it! Scryber automatically handles:
- Object property binding
- Array iteration
- Type conversions (strings, numbers, booleans, dates)
- Nested object access
- Conditional logic based on values

**Complete implementation:**
```csharp
string jsonContent = File.ReadAllText(jsonDataPath);
var model = JsonSerializer.Deserialize<object>(jsonContent);

using (var doc = Document.ParseDocument(templatePath))
{
    doc.Params["model"] = model;

    using (var stream = new FileStream(outputPdfPath, FileMode.Create))
    {
        doc.SaveAsPDF(stream);
    }
}
```

### Benefits of Using JSON for Data

**Separation of Concerns:**
- Content editors can update JSON without touching C# code
- Template designers work on HTML/CSS independently
- Developers focus on application logic

**Easy Maintenance:**
- All report content in one readable file
- Simple to version control and track changes
- Non-developers can edit content directly

**Flexibility:**
- Generate multiple reports from different JSON files
- Translate reports by swapping JSON files
- A/B test different content easily

**Integration:**
- JSON files can be generated from databases
- CMS systems can export directly to JSON
- APIs can provide data in JSON format

## Scryber Features Demonstrated

This template showcases many Scryber capabilities:

### 1. Data Binding
```html
<h1>{{model.reportYear}} Report</h1>
<p>{{model.introduction.openingText}}</p>
```

### 2. Iteration with {{#each}}
```html
{{#each model.sections.expandRange.stories}}
    <div class="success-story">
        <h2>{{this.title}}</h2>
        <p>{{this.description}}</p>
    </div>
{{/each}}
```

### 3. Conditional Rendering with {{#if}}
```html
{{#if this.keyFacts}}
    <ul>
        {{#each this.keyFacts}}
            <li>{{this}}</li>
        {{/each}}
    </ul>
{{/if}}
```

### 4. Inline Conditionals with if()
```html
<span class="{{if(this.increasing, 'trend-up', 'trend-down')}}">
    {{if(this.increasing, '↑ Increasing', '↓ Declining')}}
</span>
```

### 5. Formatting with format()
```html
<span>{{format(model.globalStats.currentPopulation, 'N0')}}</span>
<span>{{format(model.footer.publishDate, 'MMMM yyyy')}}</span>
<span>{{format(model.sections.secureHabitat.rangerGap.salaryShortfall, 'C0')}}</span>
```

### 6. CSS Styling
- External CSS file for clean separation of concerns
- CSS variables for color scheme consistency
- Responsive table and grid layouts
- Page break controls for PDF generation
- Easy to customize without touching the template

### 7. PDF-Specific Features
- Page size control with `@page { size: A4; }`
- Page break management with `page-break-before` and `page-break-after`
- `page-break-inside: avoid` for keeping content together

## Customization

### Styling with CSS

All styles are in the separate `wwf_tigers_report_styles.css` file. This makes customization easy without touching the template or data:

**Color Scheme:** Edit the CSS variables at the top of the file:
```css
:root {
    --wwf-orange: #ff6600;
    --wwf-black: #000000;
    --dark-gray: #2d2d2d;
    --light-gray: #f5f5f5;
    --border-gray: #e0e0e0;
}
```

**Typography:** Modify font sizes, families, and weights:
```css
body {
    font-family: 'Helvetica', 'Arial', sans-serif;
    font-size: 10pt;
}
```

**Layout:** Adjust padding, margins, and spacing for different sections:
```css
.section {
    padding: 40pt;  /* Change to adjust section padding */
}
```

### Template Structure

The HTML template (`wwf_tigers_report_template.html`) is modular:
- Add new sections by duplicating existing section structure
- Remove sections by commenting out or deleting
- Reorder sections as needed
- All styling is controlled via CSS classes

### Data Content

Edit the JSON file (`wwf_tigers_report_data.json`) to update content:
- Optional properties use `{{#if}}` checks in template
- Arrays can have any number of items
- You can add custom properties and reference them in the template

### File Organization

The three-file structure provides clean separation:
- **Template** (HTML): Structure and layout
- **Styles** (CSS): Visual design and branding
- **Data** (JSON): Content and information

This makes it easy for different team members to work on different aspects independently.

## Tips for Using This Template

1. **Start with JSON**: Edit `wwf_tigers_report_data.json` to customize content - it's easier than modifying C# code
2. **Validate JSON**: Use a JSON validator to catch syntax errors before running the generator
3. **Keep Data Consistent**: Ensure all required properties are present in your JSON file
4. **Test with Sample Data**: Use the provided sample data first to verify the template works
5. **Customize Gradually**: Start with JSON content changes, then colors, then layout, then structure
6. **Use Conditionals**: Wrap optional content in `{{#if}}` blocks to handle missing data gracefully
7. **Format Numbers**: Use `format()` function for consistent number and currency formatting
8. **Page Breaks**: Adjust `page-break-before` and `page-break-after` to control pagination
9. **Test PDF Output**: Always generate and review the PDF to ensure proper rendering
10. **Version Control**: Track JSON files separately to manage content versions independently

## Content Source

This template is based on the 2024 WWF Tigers Alive Annual Report available at:
https://tigers.panda.org/news_and_stories/stories/2024_wwf_tigers_alive_annual_report/

The template structure and content demonstrate real-world conservation reporting but can be adapted for any multi-section report with statistics, case studies, and narrative content.

## Technical Requirements

- Scryber.Core library
- .NET Framework or .NET Core/5+ (.NET 5+ recommended for `System.Text.Json`)
- System.Text.Json (built-in for .NET Core 3.1+)
- XHTML-compliant template syntax
- CSS support for PDF rendering

**Note:** For .NET Framework applications, you may need to install the `System.Text.Json` NuGet package.

## License and Usage

This template is provided as a learning example for Scryber.Core. The structure and code can be freely adapted for your own projects. Content from the WWF report is used for demonstration purposes only.

## Support

For questions about Scryber functionality, visit:
- Documentation: https://scrybercore.readthedocs.io/
- GitHub: https://github.com/richard-scryber/scryber.core

---

**Created**: 2025
**Based on**: 2024 WWF Tigers Alive Annual Report
**Template Version**: 3.0 (External CSS + Simplified JSON binding)
