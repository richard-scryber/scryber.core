# Project Status Report Sample

This sample demonstrates Scryber's PDF generation capabilities by creating a comprehensive project status report from JSON data.

## Features Demonstrated

### Scryber Capabilities
- **HTML Templates** with Handlebars syntax
- **CSS Styling** with custom properties (themes)
- **Data Binding** from JSON
- **Expression Functions**:
  - `format()` - Date and number formatting
  - `averageOf()` - Calculate average from collection
  - `count()` - Count collection items
  - `daysBetween()` - Calculate days between dates
  - Mathematical expressions (division, multiplication)
  - `if()` - Conditional expressions
- **Handlebars Helpers**:
  - `{{#each}}` - Iterate over collections
  - `{{#if}}` / `{{else if}}` / `{{else}}` - Conditional rendering
- **Template Variables** with `<var>` for calculated values
- **SVG Graphics** for charts and visualizations
- **Tables** with dynamic data
- **Page Breaks** with `page-break-before`
- **Page Numbers** with `<page-number />` and `<page-count />`

### Report Sections
1. **Header** - Project info, logo, status badge
2. **KPI Cards** - Budget, progress, timeline metrics
3. **Budget Chart** - Visual budget spending bar chart
4. **Phase Progress** - Progress bars for each phase
5. **Milestones** - Table with completion status
6. **Risks & Issues** - Color-coded severity table
7. **Team Allocation** - Team member assignments
8. **Timeline Chart** - SVG Gantt-style visualization

## File Structure

```
project-status-report/
├── templates/
│   └── project-status-report.html    # HTML template with Scryber expressions
├── styles/
│   └── project-status-report.css     # CSS with Corporate (Blue) theme
├── data/
│   └── project-status-sample.json    # Sample JSON data
├── images/
│   └── logo-placeholder.svg          # Placeholder company logo
├── output/
│   └── (generated PDFs go here)
├── Program.cs                         # CLI generator
├── ProjectStatusReport.csproj         # Project file
└── README.md                          # This file
```

## Building

From this directory:

```bash
dotnet build
```

## Running

### Generate report with default sample data:

```bash
dotnet run
```

This will:
- Read data from `data/project-status-sample.json`
- Generate PDF to `output/project-status-report.pdf`

### Generate report with custom data:

```bash
dotnet run path/to/your-data.json path/to/output.pdf
```

### Examples:

```bash
# Use default sample data
dotnet run

# Use custom data file
dotnet run data/my-project.json output/my-report.pdf

# Different project
dotnet run ~/Documents/project-alpha.json ~/Desktop/alpha-status.pdf
```

## JSON Data Structure

The report expects JSON with the following structure:

```json
{
  "reportDate": "2024-03-15T00:00:00",
  "projectName": "Website Redesign Project",
  "projectManager": "Sarah Johnson",
  "clientName": "Acme Corporation",
  "status": "On Track",
  "startDate": "2024-01-15T00:00:00",
  "endDate": "2024-06-30T00:00:00",
  "budget": {
    "total": 150000,
    "spent": 75000,
    "currency": "USD"
  },
  "phases": [
    {
      "name": "Discovery & Planning",
      "status": "Completed",
      "progress": 100,
      "startDate": "2024-01-15T00:00:00",
      "endDate": "2024-02-15T00:00:00"
    }
    // ... more phases
  ],
  "milestones": [
    {
      "name": "Requirements Document Approved",
      "date": "2024-02-01T00:00:00",
      "completed": true
    }
    // ... more milestones
  ],
  "risks": [
    {
      "description": "Third-party API integration delays",
      "severity": "High",
      "mitigation": "Working with vendor to expedite access"
    }
    // ... more risks
  ],
  "team": [
    {
      "name": "Sarah Johnson",
      "role": "Project Manager",
      "allocation": 100
    }
    // ... more team members
  ]
}
```

## Customizing

### Change Color Theme

Edit `styles/project-status-report.css` and modify the CSS variables in `:root`:

```css
:root {
  /* Change primary color for different theme */
  --color-primary: #2563EB;      /* Blue */
  --color-primary-dark: #1E40AF;
  --color-primary-light: #DBEAFE;

  /* Other colors... */
}
```

**Theme Suggestions:**
- **Green/Nature**: `--color-primary: #22C55E;`
- **Purple/Tech**: `--color-primary: #7C3AED;`
- **Orange/Energy**: `--color-primary: #F97316;`
- **Teal/Modern**: `--color-primary: #14B8A6;`

### Modify Template

Edit `templates/project-status-report.html` to:
- Add new sections
- Change layout
- Add more charts
- Customize calculations

### Replace Logo

Replace `images/logo-placeholder.svg` with your company logo (SVG format recommended).

## CSS Considerations for Scryber

When customizing the CSS, keep these important Scryber-specific behaviors in mind:

### Margin Collapsing

**Important**: Unlike web browsers, Scryber does NOT collapse adjacent top/bottom margins between sibling elements.

```css
/* ❌ Problem - margins add up (40pt total space) */
.section {
    margin-bottom: 20pt;
}
.section + .section {
    margin-top: 20pt;  /* Adds to previous margin, not collapsed */
}

/* ✅ Solution - use margin on one side only */
.section {
    margin-bottom: 12pt;  /* Smaller margin, one side only */
}
```

**Recommendation**: Use smaller margins than you would for web browsers, or apply margin on only one side (top OR bottom, not both).

### Float Behavior

Scryber's float implementation has two important requirements:

**1. Element Order**: Elements with `float: right` must appear BEFORE non-floating inline content in HTML source order.

```html
<!-- ❌ Wrong - float: right wraps to next line -->
<div class="header">
    <span class="title">Title</span>
    <span class="value" style="float: right;">Value</span>
</div>

<!-- ✅ Correct - float: right appears first -->
<div class="header">
    <span class="value" style="float: right;">Value</span>
    <span class="title">Title</span>
</div>
```

**2. Element Width**: Floating elements should have explicit width to prevent overflow.

```css
/* ❌ Problem - unconstrained width can overflow */
.report-title {
    float: right;
    /* Inner content uses full width */
}

/* ✅ Solution - explicit width prevents overflow */
.report-title {
    float: right;
    width: 300pt;  /* Constrains content */
}
```

### SVG Font Weights

SVG text elements support both numeric and keyword font-weight values:

```html
<!-- Numeric values (100-900) -->
<text font-weight="700">Bold Text</text>

<!-- Keyword values (also supported) -->
<text font-weight="bold">Bold Text</text>
<text font-weight="normal">Normal Text</text>
<text font-weight="light">Light Text</text>
```

## Scryber Expression Examples

This template showcases various Scryber expression capabilities:

### Date Formatting

**Important**: JSON date properties are strings and must be converted to DateTime objects before formatting using the `date()` function:

```handlebars
<!-- Convert JSON date string to DateTime first, then format -->
{{format(date(model.reportDate), 'MMMM dd, yyyy')}}
{{format(date(model.reportDate), 'yyyy-MM-dd HH:mm')}}
```

❌ **Wrong** (trying to format a string directly):
```handlebars
{{format(model.reportDate, 'MMMM dd, yyyy')}}  <!-- Won't work! -->
```

✅ **Correct** (convert to DateTime first):
```handlebars
{{format(date(model.reportDate), 'MMMM dd, yyyy')}}  <!-- Works! -->
```

### Number Formatting
```handlebars
{{format(model.budget.spent, 'C0')}}          <!-- Currency: $75,000 -->
{{format(percentage, 'P0')}}                   <!-- Percentage: 50% -->
```

### Calculations
```handlebars
{{model.budget.spent / model.budget.total}}   <!-- Division -->
{{daysBetween(date(model.reportDate), date(model.endDate))}} <!-- Date math (convert strings first) -->
{{averageOf(model.phases, .progress)}}        <!-- Collection average -->
{{count(model.team)}}                         <!-- Count items -->
```

**Important Notes**:

1. **Collection functions**: Properties use dot notation (`.property`), not string literals:
   - ✅ Correct: `averageOf(collection, .property)`
   - ❌ Wrong: `averageOf(collection, 'property')`

2. **Date functions**: JSON dates are strings and must be converted with `date()` first:
   - ✅ Correct: `daysBetween(date(model.startDate), date(model.endDate))`
   - ❌ Wrong: `daysBetween(model.startDate, model.endDate)`

### Template Variables
```handlebars
<var data-id="budgetPercentage" data-value="{{model.budget.spent / model.budget.total * 100}}" />
<!-- Use later: -->
{{budgetPercentage}}
```

**Variables in Loops**: Variables inside `{{#each}}` loops update automatically each iteration:
```handlebars
{{#each model.phases}}
    <var data-id="phaseX" data-value="{{@index * 50}}" />
    <rect x="{{phaseX}}" width="40" height="25" />
{{/each}}
```

**Important**: Don't create unique variable names with `@index` - nested binding doesn't work:
- ❌ Wrong: `<var data-id="phase_{{@index}}" ... />` then `{{phase_{{@index}}}}`
- ✅ Correct: `<var data-id="phaseX" ... />` then `{{phaseX}}` (updates each iteration)

### Conditional Rendering
```handlebars
{{#if this.completed}}
    <span>Completed</span>
{{else}}
    <span>Pending</span>
{{/if}}

<!-- Ternary-style with if() function -->
{{if(this.completed, 'Done', 'Not Done')}}
```

### Iteration
```handlebars
{{#each model.phases}}
    <div>{{this.name}} - {{this.progress}}%</div>
{{/each}}
```

### Context Navigation
```handlebars
{{#each model.phases}}
    <!-- Access properties from the loop item -->
    {{this.name}} - {{this.progress}}%

    <!-- Access root parameters directly (no ../ needed) -->
    {{daysBetween(model.startDate, this.startDate)}}

    <!-- Access parent context for nested loops -->
    {{#each this.tasks}}
        {{../this.name}} > {{this.taskName}}
    {{/each}}
{{/each}}
```

**Important**: Root parameters (like `model`) are always accessible directly - you don't need `../` to access them:
- ✅ Correct: `{{model.propertyName}}`
- ❌ Wrong: `{{../model.propertyName}}`

The parent selector `../` is only needed when navigating between nested loops, not for accessing root parameters.

## Debugging

### Debug Trace Logging

Scryber can append a detailed processing trace log to the end of generated PDFs for debugging template issues, performance analysis, and understanding the rendering process.

**Enable via Processing Instruction** (recommended for templates):

```html
<!DOCTYPE html>
<?scryber append-log='true' ?>
<html xmlns="http://www.w3.org/1999/xhtml">
<!-- your template content -->
</html>
```

**Enable Programmatically** (in code):

```csharp
using (var doc = Document.ParseDocument("template.html"))
{
    doc.AppendTraceLog = true;  // Enable debug trace log
    doc.Params["model"] = data;

    using (var stream = new FileStream("output.pdf", FileMode.Create))
    {
        doc.SaveAsPDF(stream);
    }
}
```

The trace log will be appended as additional pages at the end of the PDF, showing:
- Template parsing and binding steps
- Expression evaluation results
- Layout calculations
- Timing information for each phase
- Any warnings or errors encountered

**Note**: This template already has debug logging enabled via the processing instruction at the top of `templates/project-status-report.html`.

## Requirements

- .NET 6.0, 8.0, or 9.0
- Scryber.Core (referenced as project reference)

## Output

The generated PDF includes:
- 2-3 pages (depending on data)
- Professional styling with corporate theme
- Interactive page numbers
- SVG charts and visualizations
- Formatted tables and progress bars
- Status badges with color coding

## Notes

- All dates in JSON should be in ISO 8601 format (`yyyy-MM-ddTHH:mm:ss`)
- Budget amounts are formatted as currency based on system locale
- Progress values should be 0-100 (percentage)
- Status values: "Completed", "In Progress", "Not Started", "On Track", "At Risk", "Delayed"
- Severity values: "High", "Medium", "Low"

## Troubleshooting

**Problem**: "Template file not found"
- **Solution**: Make sure you're running from the project directory, or use absolute paths

**Problem**: "Failed to deserialize JSON"
- **Solution**: Validate your JSON structure matches the expected format

**Problem**: Charts not rendering
- **Solution**: Check that SVG elements have proper viewBox and dimensions

**Problem**: Dates showing as raw strings or not formatting correctly
- **Solution**: JSON date properties are strings and must be converted to DateTime objects before formatting. Use `{{format(date(model.reportDate), 'format')}}` NOT `{{format(model.reportDate, 'format')}}`. The `date()` function converts the ISO 8601 date string into a DateTime object that `format()` can process.

## License

This sample is part of the Scryber.Core project.
