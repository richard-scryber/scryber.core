---
layout: default
title: meter
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;meter&gt; : The Gauge/Measurement Element

The `<meter>` element represents a scalar measurement within a known range, or a fractional value. In PDF output, it renders as a static visual bar that displays the current value relative to minimum, maximum, and optimal ranges. It's ideal for showing disk usage, relevance scores, voting results, or any measurement that has low/high thresholds.

## Usage

The `<meter>` element creates a visual gauge that:
- Renders as a horizontal bar chart in static PDF output
- Shows a value within a defined min/max range
- Supports low, high, and optimum thresholds for color-coding
- Displays different colors based on whether the value is optimal, suboptimal, or critically suboptimal
- Uses inline-block display by default
- Can be styled extensively with CSS
- Supports data binding for dynamic values
- Automatically calculates percentage widths

```html
<!-- Basic meter: 60% full -->
<meter value="0.6">60%</meter>

<!-- Disk usage meter with thresholds -->
<meter value="85" min="0" max="100" low="30" high="80" optimum="20">
    85% used
</meter>

<!-- Styled meter with custom colors -->
<meter value="7" min="0" max="10" low="3" high="7" optimum="5"
       style="width: 200pt; height: 20pt;">
    7 out of 10
</meter>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Tooltip text or outline title. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Meter-Specific Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `value` | double | **Required**. Current value of the meter. Must be between min and max. |
| `min` | double | Minimum value of the range. Default: 0. |
| `max` | double | Maximum value of the range. Default: 1. |
| `low` | double | Upper bound of the "low" range. Values below this are suboptimal. Default: min. |
| `high` | double | Lower bound of the "high" range. Values above this are suboptimal. Default: max. |
| `optimum` | double | Optimal value. Determines which range is considered optimal. Default: NaN (not set). |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-bind` | expression | Binds the element to a data context for use with templates. |

### CSS Style Support

The `<meter>` element supports extensive CSS styling:

**Sizing**:
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`

**Positioning**:
- `display`: `inline-block` (default), `block`, `inline`, `none`
- `position`: `static`, `relative`, `absolute`
- `float`: `left`, `right`, `none`
- `vertical-align`: `top`, `middle`, `bottom`, `baseline`

**Spacing**:
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding` (all variants)

**Visual Effects**:
- `border`, `border-width`, `border-color`, `border-style`, `border-radius`
- `background-color` (for the bar background)
- `opacity`
- `transform` (rotation, scaling, translation)
- `overflow`: Controls clipping behavior

**Meter-Specific Styling** (via pseudo-classes):
- `.meter-bar`: Styles the container bar
- `.meter-optimum-value`: Styles the value bar when in optimal range (green)
- `.meter-suboptimal-value`: Styles when in suboptimal range (orange)
- `.meter-sub-suboptimal-value`: Styles when critically suboptimal (darker orange/red)

---

## Notes

### Value Ranges and Color Coding

The `<meter>` element uses a sophisticated color-coding system based on thresholds:

1. **Optimal Range** (Green): Value is in the best range
   - Default color: Green (`#008000`)
   - Determined by the `optimum` value

2. **Suboptimal Range** (Orange): Value is acceptable but not optimal
   - Default color: Orange (`#FFAA00`)
   - Value is either above `high` or below `low` threshold

3. **Critically Suboptimal** (Darker Orange): Value is in the worst range
   - Displayed when value is below `low` threshold
   - Uses a different shade to indicate severity

### How Thresholds Work

The meter evaluates thresholds in this order:

```
min ----[low]-------- [optimal] --------[high]---- max
```

**Value Position:**
- `value < low`: Critically suboptimal (sub-suboptimal)
- `low <= value < high`: Optimal range (if optimum is in this range)
- `value > high`: Suboptimal

**Default Values:**
- If `min` not set: defaults to 0
- If `max` not set: defaults to 1
- If `low` not set: defaults to `min`
- If `high` not set: defaults to `max`
- If `optimum` not set: the meter assumes mid-range is optimal

### Visual Structure

The meter is rendered using nested div elements:

```
<div class="meter">                          ← Main container (gray background)
  <div class="meter-bar">                     ← Bar container
    <div class="meter-optimum-value">         ← Value bar (green when optimal)
    <div class="meter-suboptimal-value">      ← Value bar (orange when suboptimal, hidden when optimal)
```

### Default Styling

By default, meters have:
- Width: 10rem (160pt approximately)
- Height: 1rem (16pt approximately)
- Background: Gray (#C0C0C0)
- Optimal value color: Green
- Suboptimal color: Orange (#FFAA00)
- Display: inline-block
- Overflow: clip (content doesn't overflow)

### Class Hierarchy

In the Scryber codebase:
- `HTMLMeter` extends `HTMLProgress` extends `Panel`
- Inherits progress bar functionality but adds threshold logic
- Uses special CSS classes: `meter-bar`, `meter-optimum-value`, `meter-suboptimal-value`

### Use Cases in PDF

While not interactive in PDF, meters are excellent for:
1. **Status Reports**: Show completion percentages, capacity usage
2. **Health Metrics**: Display vital signs, system health indicators
3. **Performance Ratings**: Show scores with good/bad thresholds
4. **Inventory Levels**: Display stock levels with reorder thresholds
5. **Quality Scores**: Show measurements against acceptable ranges

---

## Examples

### Basic Meter - Simple Value

```html
<!-- 60% meter with default range (0 to 1) -->
<meter value="0.6">60%</meter>

<!-- 75% meter -->
<meter value="0.75" style="width: 150pt; height: 20pt;">75%</meter>
```

### Disk Usage Meter

```html
<div style="padding: 15pt;">
    <h4 style="margin: 0 0 5pt 0;">Disk Usage</h4>
    <meter value="85" min="0" max="100" low="30" high="80" optimum="20"
           style="width: 300pt; height: 25pt;">
        85% used
    </meter>
    <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
        85 GB of 100 GB used
    </p>
</div>
```

### Battery Level Indicator

```html
<style>
    .battery-meter {
        width: 120pt;
        height: 24pt;
        border: 2pt solid #333;
        border-radius: 4pt;
    }
</style>

<div>
    <p style="font-weight: bold; margin-bottom: 8pt;">Battery Status</p>

    <!-- High battery: 85% - shows green -->
    <div style="margin: 8pt 0;">
        <span style="display: inline-block; width: 80pt;">Laptop:</span>
        <meter class="battery-meter" value="85" min="0" max="100"
               low="20" high="50" optimum="100">85%</meter>
        <span style="margin-left: 10pt; color: green; font-weight: bold;">85%</span>
    </div>

    <!-- Medium battery: 35% - shows orange -->
    <div style="margin: 8pt 0;">
        <span style="display: inline-block; width: 80pt;">Phone:</span>
        <meter class="battery-meter" value="35" min="0" max="100"
               low="20" high="50" optimum="100">35%</meter>
        <span style="margin-left: 10pt; color: orange; font-weight: bold;">35%</span>
    </div>

    <!-- Low battery: 15% - shows red/dark orange -->
    <div style="margin: 8pt 0;">
        <span style="display: inline-block; width: 80pt;">Tablet:</span>
        <meter class="battery-meter" value="15" min="0" max="100"
               low="20" high="50" optimum="100">15%</meter>
        <span style="margin-left: 10pt; color: red; font-weight: bold;">15%</span>
    </div>
</div>
```

### Temperature Gauge

```html
<style>
    .temp-meter {
        width: 200pt;
        height: 30pt;
        border: 1pt solid #666;
        border-radius: 15pt;
    }

    .temp-label {
        display: inline-block;
        width: 100pt;
        font-weight: 600;
    }
</style>

<div style="padding: 15pt; background-color: #f9f9f9; border-radius: 8pt;">
    <h4 style="margin: 0 0 15pt 0;">System Temperatures</h4>

    <div style="margin: 10pt 0;">
        <span class="temp-label">CPU:</span>
        <meter class="temp-meter" value="45" min="0" max="100"
               low="30" high="70" optimum="40">45°C</meter>
        <span style="margin-left: 10pt;">45°C</span>
    </div>

    <div style="margin: 10pt 0;">
        <span class="temp-label">GPU:</span>
        <meter class="temp-meter" value="75" min="0" max="100"
               low="30" high="70" optimum="40">75°C</meter>
        <span style="margin-left: 10pt; color: orange;">75°C</span>
    </div>

    <div style="margin: 10pt 0;">
        <span class="temp-label">Storage:</span>
        <meter class="temp-meter" value="35" min="0" max="100"
               low="30" high="70" optimum="40">35°C</meter>
        <span style="margin-left: 10pt; color: green;">35°C</span>
    </div>
</div>
```

### Performance Score Meter

```html
<div style="border: 1pt solid #ddd; padding: 20pt; margin: 15pt 0; border-radius: 8pt;">
    <h3 style="margin: 0 0 15pt 0; color: #336699;">Performance Evaluation</h3>

    <table style="width: 100%;">
        <tr>
            <td style="padding: 8pt; width: 150pt; font-weight: 600;">Code Quality:</td>
            <td style="padding: 8pt;">
                <meter value="9" min="0" max="10" low="4" high="7" optimum="10"
                       style="width: 200pt; height: 18pt;">9/10</meter>
            </td>
            <td style="padding: 8pt; text-align: right; color: green; font-weight: bold;">9/10</td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="padding: 8pt; font-weight: 600;">Performance:</td>
            <td style="padding: 8pt;">
                <meter value="6" min="0" max="10" low="4" high="7" optimum="10"
                       style="width: 200pt; height: 18pt;">6/10</meter>
            </td>
            <td style="padding: 8pt; text-align: right; color: orange; font-weight: bold;">6/10</td>
        </tr>
        <tr>
            <td style="padding: 8pt; font-weight: 600;">Documentation:</td>
            <td style="padding: 8pt;">
                <meter value="3" min="0" max="10" low="4" high="7" optimum="10"
                       style="width: 200pt; height: 18pt;">3/10</meter>
            </td>
            <td style="padding: 8pt; text-align: right; color: #cc6600; font-weight: bold;">3/10</td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="padding: 8pt; font-weight: 600;">Test Coverage:</td>
            <td style="padding: 8pt;">
                <meter value="8.5" min="0" max="10" low="4" high="7" optimum="10"
                       style="width: 200pt; height: 18pt;">8.5/10</meter>
            </td>
            <td style="padding: 8pt; text-align: right; color: green; font-weight: bold;">8.5/10</td>
        </tr>
    </table>
</div>
```

### Student Grade Meter

```html
<style>
    .grade-card {
        border: 1pt solid #e0e0e0;
        border-radius: 8pt;
        padding: 15pt;
        margin: 10pt 0;
        background-color: #fff;
    }

    .grade-meter {
        width: 100%;
        height: 25pt;
        border: 1pt solid #ccc;
        border-radius: 4pt;
    }

    .grade-label {
        font-weight: 600;
        color: #333;
        display: block;
        margin-bottom: 5pt;
    }
</style>

<div class="grade-card">
    <h4 style="margin: 0 0 15pt 0; color: #336699;">Student Performance Report</h4>

    <div style="margin-bottom: 15pt;">
        <span class="grade-label">Mathematics: A (92%)</span>
        <meter class="grade-meter" value="92" min="0" max="100"
               low="60" high="80" optimum="100">92%</meter>
    </div>

    <div style="margin-bottom: 15pt;">
        <span class="grade-label">Science: B (78%)</span>
        <meter class="grade-meter" value="78" min="0" max="100"
               low="60" high="80" optimum="100">78%</meter>
    </div>

    <div style="margin-bottom: 15pt;">
        <span class="grade-label">English: C (65%)</span>
        <meter class="grade-meter" value="65" min="0" max="100"
               low="60" high="80" optimum="100">65%</meter>
    </div>

    <div style="margin-bottom: 0;">
        <span class="grade-label">History: D (55%)</span>
        <meter class="grade-meter" value="55" min="0" max="100"
               low="60" high="80" optimum="100">55%</meter>
    </div>
</div>
```

### Resource Usage Dashboard

```html
<div style="background-color: #2c3e50; color: white; padding: 20pt; border-radius: 8pt;">
    <h3 style="margin: 0 0 20pt 0;">System Resources</h3>

    <div style="margin-bottom: 15pt;">
        <div style="margin-bottom: 5pt;">
            <span style="font-weight: 600;">CPU Usage</span>
            <span style="float: right;">45%</span>
        </div>
        <meter value="45" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 20pt; background-color: #34495e;">
            45%
        </meter>
    </div>

    <div style="margin-bottom: 15pt;">
        <div style="margin-bottom: 5pt;">
            <span style="font-weight: 600;">Memory Usage</span>
            <span style="float: right;">68%</span>
        </div>
        <meter value="68" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 20pt; background-color: #34495e;">
            68%
        </meter>
    </div>

    <div style="margin-bottom: 15pt;">
        <div style="margin-bottom: 5pt;">
            <span style="font-weight: 600;">Disk I/O</span>
            <span style="float: right;">22%</span>
        </div>
        <meter value="22" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 20pt; background-color: #34495e;">
            22%
        </meter>
    </div>

    <div>
        <div style="margin-bottom: 5pt;">
            <span style="font-weight: 600;">Network Usage</span>
            <span style="float: right;">89%</span>
        </div>
        <meter value="89" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 20pt; background-color: #34495e;">
            89%
        </meter>
    </div>
</div>
```

### Voting/Poll Results

```html
<div style="border: 2pt solid #336699; border-radius: 8pt; padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0; color: #336699;">Survey Results</h3>

    <div style="margin-bottom: 20pt;">
        <h4 style="margin: 0 0 8pt 0; color: #555;">
            How satisfied are you with our service?
        </h4>

        <div style="margin: 10pt 0;">
            <div style="margin-bottom: 4pt;">
                <strong>Very Satisfied</strong>
                <span style="float: right; color: #666;">45 votes (45%)</span>
            </div>
            <meter value="45" max="100" low="20" high="60" optimum="100"
                   style="width: 100%; height: 22pt; border: 1pt solid #ddd;">
                45%
            </meter>
        </div>

        <div style="margin: 10pt 0;">
            <div style="margin-bottom: 4pt;">
                <strong>Satisfied</strong>
                <span style="float: right; color: #666;">32 votes (32%)</span>
            </div>
            <meter value="32" max="100" low="20" high="60" optimum="100"
                   style="width: 100%; height: 22pt; border: 1pt solid #ddd;">
                32%
            </meter>
        </div>

        <div style="margin: 10pt 0;">
            <div style="margin-bottom: 4pt;">
                <strong>Neutral</strong>
                <span style="float: right; color: #666;">15 votes (15%)</span>
            </div>
            <meter value="15" max="100" low="20" high="60" optimum="100"
                   style="width: 100%; height: 22pt; border: 1pt solid #ddd;">
                15%
            </meter>
        </div>

        <div style="margin: 10pt 0;">
            <div style="margin-bottom: 4pt;">
                <strong>Dissatisfied</strong>
                <span style="float: right; color: #666;">8 votes (8%)</span>
            </div>
            <meter value="8" max="100" low="20" high="60" optimum="100"
                   style="width: 100%; height: 22pt; border: 1pt solid #ddd;">
                8%
            </meter>
        </div>
    </div>
</div>
```

### Project Progress with Meters

```html
<style>
    .project-meter {
        width: 250pt;
        height: 24pt;
        border: 1pt solid #999;
        border-radius: 12pt;
    }
</style>

<div style="padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0;">Project Milestones</h3>

    <table style="width: 100%; border-collapse: collapse;">
        <tr>
            <td style="padding: 12pt; width: 180pt; font-weight: 600; border-bottom: 1pt solid #eee;">
                Planning Phase
            </td>
            <td style="padding: 12pt; border-bottom: 1pt solid #eee;">
                <meter class="project-meter" value="100" max="100"
                       low="30" high="70" optimum="100">100%</meter>
            </td>
            <td style="padding: 12pt; text-align: right; border-bottom: 1pt solid #eee;">
                <span style="color: green; font-weight: bold;">✓ Complete</span>
            </td>
        </tr>
        <tr>
            <td style="padding: 12pt; font-weight: 600; border-bottom: 1pt solid #eee;">
                Design Phase
            </td>
            <td style="padding: 12pt; border-bottom: 1pt solid #eee;">
                <meter class="project-meter" value="100" max="100"
                       low="30" high="70" optimum="100">100%</meter>
            </td>
            <td style="padding: 12pt; text-align: right; border-bottom: 1pt solid #eee;">
                <span style="color: green; font-weight: bold;">✓ Complete</span>
            </td>
        </tr>
        <tr>
            <td style="padding: 12pt; font-weight: 600; border-bottom: 1pt solid #eee;">
                Development Phase
            </td>
            <td style="padding: 12pt; border-bottom: 1pt solid #eee;">
                <meter class="project-meter" value="75" max="100"
                       low="30" high="70" optimum="100">75%</meter>
            </td>
            <td style="padding: 12pt; text-align: right; border-bottom: 1pt solid #eee;">
                <span style="color: orange; font-weight: bold;">In Progress</span>
            </td>
        </tr>
        <tr>
            <td style="padding: 12pt; font-weight: 600; border-bottom: 1pt solid #eee;">
                Testing Phase
            </td>
            <td style="padding: 12pt; border-bottom: 1pt solid #eee;">
                <meter class="project-meter" value="25" max="100"
                       low="30" high="70" optimum="100">25%</meter>
            </td>
            <td style="padding: 12pt; text-align: right; border-bottom: 1pt solid #eee;">
                <span style="color: #cc6600; font-weight: bold;">Starting</span>
            </td>
        </tr>
        <tr>
            <td style="padding: 12pt; font-weight: 600;">
                Deployment Phase
            </td>
            <td style="padding: 12pt;">
                <meter class="project-meter" value="0" max="100"
                       low="30" high="70" optimum="100">0%</meter>
            </td>
            <td style="padding: 12pt; text-align: right;">
                <span style="color: #999; font-weight: bold;">Not Started</span>
            </td>
        </tr>
    </table>
</div>
```

### Data-Bound Meters

```html
<!-- With model = {
    cpuUsage: 45,
    memoryUsage: 72,
    diskUsage: 88,
    networkUsage: 23
} -->
<div style="padding: 15pt; background-color: #f5f5f5; border-radius: 6pt;">
    <h4 style="margin: 0 0 15pt 0;">Real-Time System Metrics</h4>

    <div style="margin: 10pt 0;">
        <span style="display: inline-block; width: 120pt; font-weight: 600;">CPU:</span>
        <meter value="{{model.cpuUsage}}" min="0" max="100"
               low="30" high="75" optimum="0"
               style="width: 180pt; height: 20pt;">
            {{model.cpuUsage}}%
        </meter>
        <span style="margin-left: 10pt;">{{model.cpuUsage}}%</span>
    </div>

    <div style="margin: 10pt 0;">
        <span style="display: inline-block; width: 120pt; font-weight: 600;">Memory:</span>
        <meter value="{{model.memoryUsage}}" min="0" max="100"
               low="30" high="75" optimum="0"
               style="width: 180pt; height: 20pt;">
            {{model.memoryUsage}}%
        </meter>
        <span style="margin-left: 10pt;">{{model.memoryUsage}}%</span>
    </div>

    <div style="margin: 10pt 0;">
        <span style="display: inline-block; width: 120pt; font-weight: 600;">Disk:</span>
        <meter value="{{model.diskUsage}}" min="0" max="100"
               low="30" high="75" optimum="0"
               style="width: 180pt; height: 20pt;">
            {{model.diskUsage}}%
        </meter>
        <span style="margin-left: 10pt;">{{model.diskUsage}}%</span>
    </div>

    <div style="margin: 10pt 0;">
        <span style="display: inline-block; width: 120pt; font-weight: 600;">Network:</span>
        <meter value="{{model.networkUsage}}" min="0" max="100"
               low="30" high="75" optimum="0"
               style="width: 180pt; height: 20pt;">
            {{model.networkUsage}}%
        </meter>
        <span style="margin-left: 10pt;">{{model.networkUsage}}%</span>
    </div>
</div>
```

### Health Score Card

```html
<div style="border: 2pt solid #4CAF50; border-radius: 8pt; padding: 20pt; margin: 15pt 0;">
    <h3 style="margin: 0 0 10pt 0; color: #4CAF50;">Health Score: 82/100</h3>
    <p style="margin: 0 0 20pt 0; color: #666;">Overall Status: Good</p>

    <div style="background-color: #f9f9f9; padding: 15pt; border-radius: 4pt; margin-bottom: 15pt;">
        <h4 style="margin: 0 0 10pt 0; color: #555;">Physical Health</h4>

        <div style="margin: 8pt 0;">
            <span style="display: block; margin-bottom: 4pt;">Blood Pressure: 120/80</span>
            <meter value="120" min="90" max="140" low="100" high="130" optimum="120"
                   style="width: 100%; height: 18pt;">Normal</meter>
        </div>

        <div style="margin: 8pt 0;">
            <span style="display: block; margin-bottom: 4pt;">Heart Rate: 72 bpm</span>
            <meter value="72" min="60" max="100" low="65" high="85" optimum="70"
                   style="width: 100%; height: 18pt;">Normal</meter>
        </div>

        <div style="margin: 8pt 0;">
            <span style="display: block; margin-bottom: 4pt;">Blood Sugar: 95 mg/dL</span>
            <meter value="95" min="70" max="140" low="80" high="120" optimum="90"
                   style="width: 100%; height: 18pt;">Normal</meter>
        </div>
    </div>

    <div style="background-color: #f9f9f9; padding: 15pt; border-radius: 4pt;">
        <h4 style="margin: 0 0 10pt 0; color: #555;">Activity Metrics</h4>

        <div style="margin: 8pt 0;">
            <span style="display: block; margin-bottom: 4pt;">Daily Steps: 8,500 / 10,000</span>
            <meter value="8500" min="0" max="10000" low="3000" high="7000" optimum="10000"
                   style="width: 100%; height: 18pt;">85%</meter>
        </div>

        <div style="margin: 8pt 0;">
            <span style="display: block; margin-bottom: 4pt;">Sleep: 7.5 hours</span>
            <meter value="7.5" min="0" max="10" low="6" high="9" optimum="8"
                   style="width: 100%; height: 18pt;">7.5 hrs</meter>
        </div>

        <div style="margin: 8pt 0;">
            <span style="display: block; margin-bottom: 4pt;">Water Intake: 6 / 8 glasses</span>
            <meter value="6" min="0" max="8" low="4" high="7" optimum="8"
                   style="width: 100%; height: 18pt;">75%</meter>
        </div>
    </div>
</div>
```

### Sales Target Meter

```html
<div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white; padding: 25pt; border-radius: 10pt;">
    <h3 style="margin: 0 0 20pt 0; font-size: 16pt;">Q4 Sales Performance</h3>

    <div style="background-color: rgba(255,255,255,0.1); padding: 15pt;
                border-radius: 6pt; margin-bottom: 15pt;">
        <div style="margin-bottom: 8pt;">
            <span style="font-weight: 600; font-size: 11pt;">Revenue Target</span>
            <span style="float: right; font-size: 13pt; font-weight: bold;">$875K / $1M</span>
        </div>
        <meter value="875" min="0" max="1000" low="400" high="800" optimum="1000"
               style="width: 100%; height: 28pt; border: 2pt solid rgba(255,255,255,0.3);">
            87.5%
        </meter>
        <div style="margin-top: 5pt; font-size: 9pt; opacity: 0.9;">
            87.5% of target achieved - On track to meet goal
        </div>
    </div>

    <div style="background-color: rgba(255,255,255,0.1); padding: 15pt;
                border-radius: 6pt; margin-bottom: 15pt;">
        <div style="margin-bottom: 8pt;">
            <span style="font-weight: 600; font-size: 11pt;">New Customers</span>
            <span style="float: right; font-size: 13pt; font-weight: bold;">156 / 200</span>
        </div>
        <meter value="156" min="0" max="200" low="80" high="150" optimum="200"
               style="width: 100%; height: 28pt; border: 2pt solid rgba(255,255,255,0.3);">
            78%
        </meter>
        <div style="margin-top: 5pt; font-size: 9pt; opacity: 0.9;">
            78% of customer acquisition goal
        </div>
    </div>

    <div style="background-color: rgba(255,255,255,0.1); padding: 15pt; border-radius: 6pt;">
        <div style="margin-bottom: 8pt;">
            <span style="font-weight: 600; font-size: 11pt;">Customer Satisfaction</span>
            <span style="float: right; font-size: 13pt; font-weight: bold;">4.6 / 5.0</span>
        </div>
        <meter value="4.6" min="0" max="5" low="3" high="4" optimum="5"
               style="width: 100%; height: 28pt; border: 2pt solid rgba(255,255,255,0.3);">
            92%
        </meter>
        <div style="margin-top: 5pt; font-size: 9pt; opacity: 0.9;">
            Excellent customer satisfaction rating
        </div>
    </div>
</div>
```

### Skill Proficiency Chart

```html
<div style="border: 1pt solid #e0e0e0; border-radius: 8pt; padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0; color: #333;">Technical Skills Assessment</h3>

    <table style="width: 100%;">
        <tr>
            <td style="padding: 10pt 10pt 10pt 0; width: 140pt;">
                <strong>C# / .NET</strong>
            </td>
            <td style="padding: 10pt;">
                <meter value="9.5" min="0" max="10" low="4" high="7" optimum="10"
                       style="width: 100%; height: 20pt;">Expert</meter>
            </td>
            <td style="padding: 10pt; width: 80pt; text-align: center;">
                <span style="background-color: #4CAF50; color: white; padding: 4pt 8pt;
                             border-radius: 3pt; font-size: 9pt; font-weight: bold;">
                    Expert
                </span>
            </td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="padding: 10pt 10pt 10pt 0;">
                <strong>JavaScript</strong>
            </td>
            <td style="padding: 10pt;">
                <meter value="8" min="0" max="10" low="4" high="7" optimum="10"
                       style="width: 100%; height: 20pt;">Advanced</meter>
            </td>
            <td style="padding: 10pt; text-align: center;">
                <span style="background-color: #8BC34A; color: white; padding: 4pt 8pt;
                             border-radius: 3pt; font-size: 9pt; font-weight: bold;">
                    Advanced
                </span>
            </td>
        </tr>
        <tr>
            <td style="padding: 10pt 10pt 10pt 0;">
                <strong>Python</strong>
            </td>
            <td style="padding: 10pt;">
                <meter value="6.5" min="0" max="10" low="4" high="7" optimum="10"
                       style="width: 100%; height: 20pt;">Intermediate</meter>
            </td>
            <td style="padding: 10pt; text-align: center;">
                <span style="background-color: #FF9800; color: white; padding: 4pt 8pt;
                             border-radius: 3pt; font-size: 9pt; font-weight: bold;">
                    Intermediate
                </span>
            </td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="padding: 10pt 10pt 10pt 0;">
                <strong>SQL</strong>
            </td>
            <td style="padding: 10pt;">
                <meter value="8.5" min="0" max="10" low="4" high="7" optimum="10"
                       style="width: 100%; height: 20pt;">Advanced</meter>
            </td>
            <td style="padding: 10pt; text-align: center;">
                <span style="background-color: #8BC34A; color: white; padding: 4pt 8pt;
                             border-radius: 3pt; font-size: 9pt; font-weight: bold;">
                    Advanced
                </span>
            </td>
        </tr>
        <tr>
            <td style="padding: 10pt 10pt 10pt 0;">
                <strong>Docker</strong>
            </td>
            <td style="padding: 10pt;">
                <meter value="7" min="0" max="10" low="4" high="7" optimum="10"
                       style="width: 100%; height: 20pt;">Proficient</meter>
            </td>
            <td style="padding: 10pt; text-align: center;">
                <span style="background-color: #FF9800; color: white; padding: 4pt 8pt;
                             border-radius: 3pt; font-size: 9pt; font-weight: bold;">
                    Proficient
                </span>
            </td>
        </tr>
    </table>
</div>
```

### Vertical Meter Representation

```html
<!-- Note: Meters are horizontal by default, but you can create vertical effect with rotation -->
<div style="text-align: center; padding: 20pt;">
    <h4 style="margin-bottom: 20pt;">Vertical Gauges</h4>

    <div style="display: inline-block; margin: 0 15pt;">
        <div style="transform: rotate(-90deg); transform-origin: center;
                    width: 120pt; margin: 60pt 0;">
            <meter value="85" max="100" low="30" high="70" optimum="100"
                   style="width: 120pt; height: 25pt;">85%</meter>
        </div>
        <p style="margin-top: 10pt; font-weight: bold;">CPU</p>
    </div>

    <div style="display: inline-block; margin: 0 15pt;">
        <div style="transform: rotate(-90deg); transform-origin: center;
                    width: 120pt; margin: 60pt 0;">
            <meter value="45" max="100" low="30" high="70" optimum="100"
                   style="width: 120pt; height: 25pt;">45%</meter>
        </div>
        <p style="margin-top: 10pt; font-weight: bold;">Memory</p>
    </div>

    <div style="display: inline-block; margin: 0 15pt;">
        <div style="transform: rotate(-90deg); transform-origin: center;
                    width: 120pt; margin: 60pt 0;">
            <meter value="22" max="100" low="30" high="70" optimum="100"
                   style="width: 120pt; height: 25pt;">22%</meter>
        </div>
        <p style="margin-top: 10pt; font-weight: bold;">Disk</p>
    </div>
</div>
```

### Repeating Meters from Collection

```html
<!-- With model.servers = [
    {name: "Web-01", cpu: 45, memory: 68, disk: 72},
    {name: "Web-02", cpu: 52, memory: 71, disk: 65},
    {name: "DB-01", cpu: 78, memory: 85, disk: 88}
] -->
<div style="padding: 15pt;">
    <h3 style="margin: 0 0 15pt 0;">Server Status Dashboard</h3>

    <template data-bind="{{model.servers}}">
        <div style="border: 1pt solid #ddd; border-radius: 6pt; padding: 15pt;
                    margin-bottom: 15pt; background-color: #fafafa;">
            <h4 style="margin: 0 0 12pt 0; color: #336699;">{{.name}}</h4>

            <div style="margin: 8pt 0;">
                <span style="display: inline-block; width: 80pt; font-weight: 600;">CPU:</span>
                <meter value="{{.cpu}}" max="100" low="30" high="75" optimum="0"
                       style="width: 200pt; height: 18pt;">{{.cpu}}%</meter>
                <span style="margin-left: 10pt;">{{.cpu}}%</span>
            </div>

            <div style="margin: 8pt 0;">
                <span style="display: inline-block; width: 80pt; font-weight: 600;">Memory:</span>
                <meter value="{{.memory}}" max="100" low="30" high="75" optimum="0"
                       style="width: 200pt; height: 18pt;">{{.memory}}%</meter>
                <span style="margin-left: 10pt;">{{.memory}}%</span>
            </div>

            <div style="margin: 8pt 0;">
                <span style="display: inline-block; width: 80pt; font-weight: 600;">Disk:</span>
                <meter value="{{.disk}}" max="100" low="30" high="75" optimum="0"
                       style="width: 200pt; height: 18pt;">{{.disk}}%</meter>
                <span style="margin-left: 10pt;">{{.disk}}%</span>
            </div>
        </div>
    </template>
</div>
```

### Custom Styled Meters with CSS Classes

```html
<style>
    .custom-meter {
        width: 220pt;
        height: 26pt;
        background-color: #eceff1;
        border: 2pt solid #b0bec5;
        border-radius: 13pt;
    }

    .meter-optimum-value {
        background: linear-gradient(to right, #4CAF50, #8BC34A);
    }

    .meter-suboptimal-value {
        background: linear-gradient(to right, #FF9800, #FFC107);
    }

    .meter-sub-suboptimal-value {
        background: linear-gradient(to right, #F44336, #FF5722);
    }
</style>

<div style="padding: 20pt;">
    <h4 style="margin-bottom: 15pt;">Custom Gradient Meters</h4>

    <div style="margin: 12pt 0;">
        <p style="margin-bottom: 5pt; font-weight: 600;">Excellent (95%)</p>
        <meter class="custom-meter" value="95" max="100" low="40" high="70" optimum="100">
            95%
        </meter>
    </div>

    <div style="margin: 12pt 0;">
        <p style="margin-bottom: 5pt; font-weight: 600;">Good (75%)</p>
        <meter class="custom-meter" value="75" max="100" low="40" high="70" optimum="100">
            75%
        </meter>
    </div>

    <div style="margin: 12pt 0;">
        <p style="margin-bottom: 5pt; font-weight: 600;">Fair (55%)</p>
        <meter class="custom-meter" value="55" max="100" low="40" high="70" optimum="100">
            55%
        </meter>
    </div>

    <div style="margin: 12pt 0;">
        <p style="margin-bottom: 5pt; font-weight: 600;">Poor (25%)</p>
        <meter class="custom-meter" value="25" max="100" low="40" high="70" optimum="100">
            25%
        </meter>
    </div>
</div>
```

---

## See Also

- [progress](/reference/htmltags/progress.html) - Progress bar element (for task completion)
- [div](/reference/htmltags/div.html) - Generic container (can create custom gauges)
- [Data Binding](/reference/binding/) - Data binding and expressions
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Visual Components](/reference/components/visual.html) - Base visual component

---
