---
layout: default
title: progress
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;progress&gt; : The Progress Bar Element
{: .no_toc }

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---


## Summary

The `<progress>` element represents the completion progress of a task. In PDF output, it renders as a static visual bar showing the current progress value. Unlike the `<meter>` element which shows measurements against thresholds, `<progress>` specifically represents task completion from 0% to 100%.

## Usage

The `<progress>` element creates a progress indicator that:
- Renders as a horizontal progress bar in static PDF output
- Shows task completion as a percentage (value / max)
- Displays with a green fill for the completed portion by default
- Uses inline-block display by default
- Can be styled extensively with CSS
- Supports data binding for dynamic values
- Automatically calculates percentage widths
- Perfect for showing download progress, upload status, or task completion

```html
<!-- Basic progress: 60% complete -->
<progress value="0.6" max="1">60%</progress>

<!-- File upload progress -->
<progress value="45" max="100">45%</progress>

<!-- Styled progress bar -->
<progress value="7" max="10" style="width: 250pt; height: 25pt;">
    70% complete
</progress>
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

### Progress-Specific Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `value` | double | **Required**. Current progress value. Must be between 0 and max. |
| `max` | double | Maximum value representing 100% completion. Default: 1. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-bind` | expression | Binds the element to a data context for use with templates. |

### CSS Style Support

The `<progress>` element supports extensive CSS styling:

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
- `background-color` (for the bar container background)
- `opacity`
- `transform` (rotation, scaling, translation)
- `overflow`: Controls clipping behavior (default: clip)

**Progress-Specific Styling** (via pseudo-classes):
- `.progress-bar`: Styles the container bar (background)
- `.progress-value`: Styles the filled progress portion (foreground)

---

## Notes

### Progress Calculation

The progress bar automatically calculates the fill percentage:

```
percentage = (value / max) * 100%
```

**Special cases:**
- If `value >= max`: Shows 100% (full bar)
- If `value < 0`: Shows 0% (empty bar)
- If `value` is between 0 and max: Shows proportional fill

**Examples:**
```html
<!-- 50% -->
<progress value="0.5" max="1"></progress>
<progress value="50" max="100"></progress>

<!-- 75% -->
<progress value="75" max="100"></progress>
<progress value="0.75" max="1"></progress>

<!-- 33.3% -->
<progress value="10" max="30"></progress>
```

### Visual Structure

The progress bar is rendered using nested div elements:

```
<div class="progress">                       ← Main container (gray background)
  <div class="progress-bar">                 ← Bar container (100% width)
    <div class="progress-value">             ← Filled portion (green, calculated width)
```

### Default Styling

By default, progress bars have:
- Width: 10rem (160pt approximately)
- Height: 1rem (16pt approximately)
- Container background: Gray (#C0C0C0)
- Progress value color: Green (#008000)
- Display: inline-block
- Overflow: clip (content doesn't overflow)

### Difference from Meter

While `<progress>` and `<meter>` look similar, they serve different purposes:

| Feature | `<progress>` | `<meter>` |
|---------|-------------|-----------|
| **Purpose** | Task completion (0% to 100%) | Measurement within a range |
| **Thresholds** | None | Supports low, high, optimum |
| **Color Coding** | Single color (green) | Multiple colors based on thresholds |
| **Use Case** | Downloads, uploads, tasks | Disk usage, scores, ratings |
| **Semantic** | Indicates ongoing activity | Indicates current state |

### Class Hierarchy

In the Scryber codebase:
- `HTMLProgress` extends `Panel` extends `VisualComponent`
- Uses nested components: `ProgressBar` and `ProgressValue`
- The value bar width is calculated during layout

### Indeterminate State

HTML supports an indeterminate state (progress without a value), but in PDF:
- This would render as an empty bar (0%)
- Always provide a `value` attribute for meaningful PDF output
- Use styling to indicate "in progress" vs "complete"

### Use Cases in PDF

Progress bars are excellent for:
1. **Report Generation Progress**: Show how much of a report is complete
2. **Goal Achievement**: Display progress toward sales targets, KPIs
3. **Task Completion**: Show project milestones and completion status
4. **Data Processing**: Indicate batch processing completion
5. **Survey Results**: Display response rates and participation
6. **Learning Progress**: Show course completion, skill development

---

## Examples

### Basic Progress Bars

```html
<!-- 25% complete -->
<progress value="25" max="100">25%</progress>

<!-- 50% complete -->
<progress value="0.5" max="1">50%</progress>

<!-- 75% complete with styling -->
<progress value="75" max="100" style="width: 200pt; height: 20pt;">
    75%
</progress>

<!-- 100% complete -->
<progress value="100" max="100">100%</progress>
```

### File Upload Progress

```html
<div style="padding: 15pt; border: 1pt solid #ddd; border-radius: 6pt;">
    <h4 style="margin: 0 0 10pt 0;">Uploading document.pdf</h4>

    <progress value="67" max="100"
              style="width: 100%; height: 24pt; border: 1pt solid #ccc; border-radius: 4pt;">
        67%
    </progress>

    <div style="margin-top: 8pt; color: #666; font-size: 9pt;">
        <span style="float: left;">67 MB of 100 MB</span>
        <span style="float: right;">67% complete</span>
        <div style="clear: both;"></div>
    </div>
</div>
```

### Download Progress with Labels

```html
<style>
    .download-container {
        border: 1pt solid #e0e0e0;
        border-radius: 6pt;
        padding: 15pt;
        margin: 10pt 0;
        background-color: #fafafa;
    }

    .download-progress {
        width: 100%;
        height: 22pt;
        border: 1pt solid #ddd;
        border-radius: 11pt;
    }

    .download-info {
        margin-top: 8pt;
        font-size: 9pt;
        color: #555;
    }
</style>

<div class="download-container">
    <div style="margin-bottom: 10pt;">
        <strong>report-2024.pdf</strong>
        <span style="float: right; color: #4CAF50; font-weight: bold;">Downloading...</span>
        <div style="clear: both;"></div>
    </div>

    <progress class="download-progress" value="42" max="100">42%</progress>

    <div class="download-info">
        <span>4.2 MB of 10 MB</span>
        <span style="float: right;">Estimated time: 15 seconds</span>
        <div style="clear: both;"></div>
    </div>
</div>
```

### Task Completion Tracker

```html
<div style="padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0;">Project Tasks</h3>

    <div style="margin-bottom: 20pt;">
        <div style="margin-bottom: 6pt;">
            <strong>Requirements Gathering</strong>
            <span style="float: right; color: green; font-weight: bold;">✓ Complete</span>
            <div style="clear: both;"></div>
        </div>
        <progress value="100" max="100" style="width: 100%; height: 20pt;">100%</progress>
    </div>

    <div style="margin-bottom: 20pt;">
        <div style="margin-bottom: 6pt;">
            <strong>Design Phase</strong>
            <span style="float: right; color: green; font-weight: bold;">✓ Complete</span>
            <div style="clear: both;"></div>
        </div>
        <progress value="100" max="100" style="width: 100%; height: 20pt;">100%</progress>
    </div>

    <div style="margin-bottom: 20pt;">
        <div style="margin-bottom: 6pt;">
            <strong>Development</strong>
            <span style="float: right; color: #2196F3; font-weight: bold;">In Progress (78%)</span>
            <div style="clear: both;"></div>
        </div>
        <progress value="78" max="100" style="width: 100%; height: 20pt;">78%</progress>
    </div>

    <div style="margin-bottom: 20pt;">
        <div style="margin-bottom: 6pt;">
            <strong>Testing</strong>
            <span style="float: right; color: #999;">Not Started</span>
            <div style="clear: both;"></div>
        </div>
        <progress value="0" max="100" style="width: 100%; height: 20pt;">0%</progress>
    </div>
</div>
```

### Styled Progress with Custom Colors

```html
<style>
    .custom-progress {
        width: 280pt;
        height: 28pt;
        background-color: #e0e0e0;
        border: 2pt solid #bdbdbd;
        border-radius: 14pt;
        overflow: hidden;
    }

    .progress-bar {
        height: 100%;
        background-color: transparent;
    }

    .progress-value {
        height: 100%;
        background: linear-gradient(to right, #667eea, #764ba2);
        border-radius: 12pt;
    }
</style>

<div style="padding: 15pt;">
    <h4 style="margin-bottom: 10pt;">Gradient Progress Bar</h4>
    <progress class="custom-progress" value="65" max="100">65%</progress>
    <p style="margin-top: 5pt; font-size: 9pt; color: #666;">65% complete</p>
</div>
```

### Multiple Progress Indicators

```html
<div style="border: 2pt solid #336699; border-radius: 8pt; padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0; color: #336699;">System Setup Progress</h3>

    <table style="width: 100%;">
        <tr>
            <td style="padding: 10pt; width: 140pt; font-weight: 600;">
                Installing Files
            </td>
            <td style="padding: 10pt;">
                <progress value="100" max="100"
                          style="width: 100%; height: 18pt; border: 1pt solid #ccc;">
                    100%
                </progress>
            </td>
            <td style="padding: 10pt; width: 60pt; text-align: right; color: green;">
                100%
            </td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="padding: 10pt; font-weight: 600;">
                Configuring Database
            </td>
            <td style="padding: 10pt;">
                <progress value="85" max="100"
                          style="width: 100%; height: 18pt; border: 1pt solid #ccc;">
                    85%
                </progress>
            </td>
            <td style="padding: 10pt; text-align: right; color: #2196F3;">
                85%
            </td>
        </tr>
        <tr>
            <td style="padding: 10pt; font-weight: 600;">
                Running Tests
            </td>
            <td style="padding: 10pt;">
                <progress value="42" max="100"
                          style="width: 100%; height: 18pt; border: 1pt solid #ccc;">
                    42%
                </progress>
            </td>
            <td style="padding: 10pt; text-align: right; color: #2196F3;">
                42%
            </td>
        </tr>
        <tr style="background-color: #f9f9f9;">
            <td style="padding: 10pt; font-weight: 600;">
                Final Setup
            </td>
            <td style="padding: 10pt;">
                <progress value="0" max="100"
                          style="width: 100%; height: 18pt; border: 1pt solid #ccc;">
                    0%
                </progress>
            </td>
            <td style="padding: 10pt; text-align: right; color: #999;">
                0%
            </td>
        </tr>
    </table>
</div>
```

### Sales Goal Progress

```html
<div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 25pt; border-radius: 10pt; color: white;">
    <h3 style="margin: 0 0 25pt 0; font-size: 16pt;">Quarterly Sales Goal</h3>

    <div style="background-color: rgba(255,255,255,0.15); padding: 20pt; border-radius: 8pt;">
        <div style="margin-bottom: 12pt;">
            <span style="font-size: 32pt; font-weight: bold;">$875,000</span>
            <span style="font-size: 14pt; opacity: 0.9;"> / $1,000,000</span>
        </div>

        <progress value="875" max="1000"
                  style="width: 100%; height: 32pt; border: 3pt solid rgba(255,255,255,0.3);
                         border-radius: 16pt; background-color: rgba(0,0,0,0.2);">
            87.5%
        </progress>

        <div style="margin-top: 15pt;">
            <div style="font-size: 18pt; font-weight: bold;">87.5% Complete</div>
            <div style="font-size: 10pt; opacity: 0.9; margin-top: 5pt;">
                Outstanding performance! Only $125,000 to goal.
            </div>
        </div>
    </div>
</div>
```

### Course Completion Progress

```html
<div style="border: 1pt solid #e0e0e0; border-radius: 8pt; padding: 20pt; margin: 15pt 0;">
    <h3 style="margin: 0 0 15pt 0; color: #333;">Learning Path: Full Stack Development</h3>

    <div style="margin-bottom: 20pt;">
        <div style="margin-bottom: 8pt;">
            <strong style="color: #555;">Overall Progress</strong>
            <span style="float: right; font-size: 18pt; font-weight: bold; color: #4CAF50;">
                68%
            </span>
            <div style="clear: both;"></div>
        </div>
        <progress value="68" max="100"
                  style="width: 100%; height: 28pt; border: 2pt solid #4CAF50; border-radius: 14pt;">
            68%
        </progress>
    </div>

    <div style="background-color: #f9f9f9; padding: 15pt; border-radius: 6pt;">
        <h4 style="margin: 0 0 15pt 0; color: #555;">Module Progress</h4>

        <div style="margin: 10pt 0;">
            <div style="margin-bottom: 4pt; font-size: 10pt;">
                <strong>HTML & CSS Fundamentals</strong>
                <span style="float: right; color: green;">Complete</span>
                <div style="clear: both;"></div>
            </div>
            <progress value="100" max="100" style="width: 100%; height: 16pt;">100%</progress>
        </div>

        <div style="margin: 10pt 0;">
            <div style="margin-bottom: 4pt; font-size: 10pt;">
                <strong>JavaScript Essentials</strong>
                <span style="float: right; color: green;">Complete</span>
                <div style="clear: both;"></div>
            </div>
            <progress value="100" max="100" style="width: 100%; height: 16pt;">100%</progress>
        </div>

        <div style="margin: 10pt 0;">
            <div style="margin-bottom: 4pt; font-size: 10pt;">
                <strong>React Framework</strong>
                <span style="float: right; color: #2196F3;">82% (12/15 lessons)</span>
                <div style="clear: both;"></div>
            </div>
            <progress value="82" max="100" style="width: 100%; height: 16pt;">82%</progress>
        </div>

        <div style="margin: 10pt 0;">
            <div style="margin-bottom: 4pt; font-size: 10pt;">
                <strong>Node.js & Express</strong>
                <span style="float: right; color: #2196F3;">45% (9/20 lessons)</span>
                <div style="clear: both;"></div>
            </div>
            <progress value="45" max="100" style="width: 100%; height: 16pt;">45%</progress>
        </div>

        <div style="margin: 10pt 0;">
            <div style="margin-bottom: 4pt; font-size: 10pt;">
                <strong>Database Design</strong>
                <span style="float: right; color: #999;">Not Started</span>
                <div style="clear: both;"></div>
            </div>
            <progress value="0" max="100" style="width: 100%; height: 16pt;">0%</progress>
        </div>
    </div>
</div>
```

### Data-Bound Progress Bars

```html
<!-- With model = {
    uploadProgress: 73,
    downloadProgress: 92,
    processingProgress: 45
} -->
<div style="padding: 15pt; background-color: #f5f5f5; border-radius: 6pt;">
    <h4 style="margin: 0 0 15pt 0;">File Operations</h4>

    <div style="margin: 12pt 0;">
        <div style="margin-bottom: 5pt;">
            <strong>Upload Progress:</strong>
            <span style="float: right;">{{model.uploadProgress}}%</span>
            <div style="clear: both;"></div>
        </div>
        <progress value="{{model.uploadProgress}}" max="100"
                  style="width: 100%; height: 20pt; border: 1pt solid #ccc;">
            {{model.uploadProgress}}%
        </progress>
    </div>

    <div style="margin: 12pt 0;">
        <div style="margin-bottom: 5pt;">
            <strong>Download Progress:</strong>
            <span style="float: right;">{{model.downloadProgress}}%</span>
            <div style="clear: both;"></div>
        </div>
        <progress value="{{model.downloadProgress}}" max="100"
                  style="width: 100%; height: 20pt; border: 1pt solid #ccc;">
            {{model.downloadProgress}}%
        </progress>
    </div>

    <div style="margin: 12pt 0;">
        <div style="margin-bottom: 5pt;">
            <strong>Processing:</strong>
            <span style="float: right;">{{model.processingProgress}}%</span>
            <div style="clear: both;"></div>
        </div>
        <progress value="{{model.processingProgress}}" max="100"
                  style="width: 100%; height: 20pt; border: 1pt solid #ccc;">
            {{model.processingProgress}}%
        </progress>
    </div>
</div>
```

### Fitness Goals Dashboard

```html
<div style="border: 2pt solid #FF5722; border-radius: 10pt; padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0; color: #FF5722;">Weekly Fitness Goals</h3>

    <div style="margin-bottom: 18pt;">
        <div style="margin-bottom: 8pt;">
            <span style="font-weight: bold; font-size: 11pt;">🏃 Running</span>
            <span style="float: right; color: green; font-weight: bold;">
                10 / 10 miles
            </span>
            <div style="clear: both;"></div>
        </div>
        <progress value="10" max="10"
                  style="width: 100%; height: 24pt; border: 1pt solid #ddd; border-radius: 12pt;">
            100%
        </progress>
        <div style="margin-top: 4pt; font-size: 9pt; color: green;">
            ✓ Goal achieved! Great job!
        </div>
    </div>

    <div style="margin-bottom: 18pt;">
        <div style="margin-bottom: 8pt;">
            <span style="font-weight: bold; font-size: 11pt;">🏋 Strength Training</span>
            <span style="float: right; color: #2196F3; font-weight: bold;">
                3 / 4 sessions
            </span>
            <div style="clear: both;"></div>
        </div>
        <progress value="3" max="4"
                  style="width: 100%; height: 24pt; border: 1pt solid #ddd; border-radius: 12pt;">
            75%
        </progress>
        <div style="margin-top: 4pt; font-size: 9pt; color: #2196F3;">
            Almost there! 1 more session to go.
        </div>
    </div>

    <div style="margin-bottom: 18pt;">
        <div style="margin-bottom: 8pt;">
            <span style="font-weight: bold; font-size: 11pt;">🧘 Yoga</span>
            <span style="float: right; color: #2196F3; font-weight: bold;">
                2 / 3 sessions
            </span>
            <div style="clear: both;"></div>
        </div>
        <progress value="2" max="3"
                  style="width: 100%; height: 24pt; border: 1pt solid #ddd; border-radius: 12pt;">
            67%
        </progress>
        <div style="margin-top: 4pt; font-size: 9pt; color: #2196F3;">
            Keep it up!
        </div>
    </div>

    <div style="margin-bottom: 0;">
        <div style="margin-bottom: 8pt;">
            <span style="font-weight: bold; font-size: 11pt;">💧 Hydration</span>
            <span style="float: right; color: orange; font-weight: bold;">
                42 / 56 glasses
            </span>
            <div style="clear: both;"></div>
        </div>
        <progress value="42" max="56"
                  style="width: 100%; height: 24pt; border: 1pt solid #ddd; border-radius: 12pt;">
            75%
        </progress>
        <div style="margin-top: 4pt; font-size: 9pt; color: orange;">
            Stay hydrated! 14 more glasses this week.
        </div>
    </div>
</div>
```

### Software Update Progress

```html
<div style="background-color: #263238; color: white; padding: 25pt; border-radius: 8pt;">
    <h3 style="margin: 0 0 10pt 0; font-size: 18pt;">System Update in Progress</h3>
    <p style="margin: 0 0 25pt 0; opacity: 0.8; font-size: 10pt;">
        Please do not turn off your computer
    </p>

    <progress value="68" max="100"
              style="width: 100%; height: 36pt; border: 2pt solid #37474F;
                     border-radius: 18pt; background-color: #37474F;">
        68%
    </progress>

    <div style="margin-top: 15pt; text-align: center;">
        <div style="font-size: 28pt; font-weight: bold;">68%</div>
        <div style="font-size: 11pt; opacity: 0.8; margin-top: 5pt;">
            Estimated time remaining: 5 minutes
        </div>
    </div>

    <div style="margin-top: 25pt; padding-top: 20pt; border-top: 1pt solid rgba(255,255,255,0.1);">
        <div style="font-size: 10pt; opacity: 0.7;">Current step:</div>
        <div style="font-size: 11pt; margin-top: 5pt;">Installing security updates...</div>
    </div>
</div>
```

### Project Milestone Progress

```html
<div style="padding: 20pt;">
    <h3 style="margin: 0 0 25pt 0; color: #333;">Development Roadmap</h3>

    <div style="position: relative; padding-left: 40pt;">
        <!-- Milestone 1 -->
        <div style="margin-bottom: 30pt; position: relative;">
            <div style="position: absolute; left: -40pt; top: 0;
                        width: 24pt; height: 24pt; border-radius: 12pt;
                        background-color: #4CAF50; border: 3pt solid white;
                        box-shadow: 0 0 0 2pt #4CAF50;"></div>

            <h4 style="margin: 0 0 8pt 0; color: #4CAF50;">Phase 1: Foundation</h4>
            <progress value="100" max="100"
                      style="width: 100%; height: 20pt; border: 1pt solid #4CAF50;">
                100%
            </progress>
            <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
                Completed: March 2024
            </p>
        </div>

        <!-- Milestone 2 -->
        <div style="margin-bottom: 30pt; position: relative;">
            <div style="position: absolute; left: -40pt; top: 0;
                        width: 24pt; height: 24pt; border-radius: 12pt;
                        background-color: #2196F3; border: 3pt solid white;
                        box-shadow: 0 0 0 2pt #2196F3;"></div>

            <h4 style="margin: 0 0 8pt 0; color: #2196F3;">Phase 2: Core Features</h4>
            <progress value="78" max="100"
                      style="width: 100%; height: 20pt; border: 1pt solid #2196F3;">
                78%
            </progress>
            <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
                In Progress: Expected April 2024
            </p>
        </div>

        <!-- Milestone 3 -->
        <div style="margin-bottom: 0; position: relative;">
            <div style="position: absolute; left: -40pt; top: 0;
                        width: 24pt; height: 24pt; border-radius: 12pt;
                        background-color: #ccc; border: 3pt solid white;
                        box-shadow: 0 0 0 2pt #ccc;"></div>

            <h4 style="margin: 0 0 8pt 0; color: #999;">Phase 3: Polish & Launch</h4>
            <progress value="0" max="100"
                      style="width: 100%; height: 20pt; border: 1pt solid #ccc;">
                0%
            </progress>
            <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
                Scheduled: May 2024
            </p>
        </div>
    </div>
</div>
```

### Budget vs Actual Spending

```html
<div style="border: 1pt solid #e0e0e0; border-radius: 8pt; padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0; color: #333;">Department Budget Utilization</h3>

    <table style="width: 100%;">
        <tr>
            <td style="padding: 12pt; width: 140pt; font-weight: 600; border-bottom: 1pt solid #f0f0f0;">
                Marketing
            </td>
            <td style="padding: 12pt; border-bottom: 1pt solid #f0f0f0;">
                <progress value="85" max="100"
                          style="width: 100%; height: 20pt; border: 1pt solid #ccc;">
                    85%
                </progress>
            </td>
            <td style="padding: 12pt; width: 120pt; text-align: right; border-bottom: 1pt solid #f0f0f0;">
                $85K / $100K
            </td>
        </tr>
        <tr>
            <td style="padding: 12pt; font-weight: 600; border-bottom: 1pt solid #f0f0f0;">
                Engineering
            </td>
            <td style="padding: 12pt; border-bottom: 1pt solid #f0f0f0;">
                <progress value="72" max="100"
                          style="width: 100%; height: 20pt; border: 1pt solid #ccc;">
                    72%
                </progress>
            </td>
            <td style="padding: 12pt; text-align: right; border-bottom: 1pt solid #f0f0f0;">
                $360K / $500K
            </td>
        </tr>
        <tr>
            <td style="padding: 12pt; font-weight: 600; border-bottom: 1pt solid #f0f0f0;">
                Sales
            </td>
            <td style="padding: 12pt; border-bottom: 1pt solid #f0f0f0;">
                <progress value="93" max="100"
                          style="width: 100%; height: 20pt; border: 1pt solid #ccc;">
                    93%
                </progress>
            </td>
            <td style="padding: 12pt; text-align: right; border-bottom: 1pt solid #f0f0f0;">
                $279K / $300K
            </td>
        </tr>
        <tr>
            <td style="padding: 12pt; font-weight: 600;">
                Operations
            </td>
            <td style="padding: 12pt;">
                <progress value="55" max="100"
                          style="width: 100%; height: 20pt; border: 1pt solid #ccc;">
                    55%
                </progress>
            </td>
            <td style="padding: 12pt; text-align: right;">
                $110K / $200K
            </td>
        </tr>
    </table>
</div>
```

### Repeating Progress from Collection

```html
<!-- With model.projects = [
    {name: "Website Redesign", progress: 92, total: 100},
    {name: "Mobile App", progress: 67, total: 100},
    {name: "API Integration", progress: 45, total: 100},
    {name: "Documentation", progress: 23, total: 100}
] -->
<div style="padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0;">Active Projects</h3>

    <template data-bind="{{model.projects}}">
        <div style="border: 1pt solid #e0e0e0; border-radius: 6pt;
                    padding: 15pt; margin-bottom: 15pt; background-color: #fafafa;">
            <div style="margin-bottom: 8pt;">
                <strong style="font-size: 11pt;">{{.name}}</strong>
                <span style="float: right; color: #666; font-weight: bold;">
                    {{.progress}}%
                </span>
                <div style="clear: both;"></div>
            </div>

            <progress value="{{.progress}}" max="{{.total}}"
                      style="width: 100%; height: 22pt; border: 1pt solid #ccc; border-radius: 11pt;">
                {{.progress}}%
            </progress>
        </div>
    </template>
</div>
```

### Circular/Vertical Progress Representation

```html
<!-- Note: Progress bars are horizontal by default, but you can rotate them -->
<div style="text-align: center; padding: 20pt;">
    <h4 style="margin-bottom: 30pt;">Server Status</h4>

    <div style="display: inline-block; margin: 0 20pt;">
        <div style="transform: rotate(-90deg); transform-origin: center;
                    width: 150pt; margin: 75pt 0;">
            <progress value="85" max="100"
                      style="width: 150pt; height: 30pt; border: 2pt solid #333; border-radius: 15pt;">
                85%
            </progress>
        </div>
        <p style="margin-top: 20pt; font-weight: bold;">Server 1<br/>85%</p>
    </div>

    <div style="display: inline-block; margin: 0 20pt;">
        <div style="transform: rotate(-90deg); transform-origin: center;
                    width: 150pt; margin: 75pt 0;">
            <progress value="62" max="100"
                      style="width: 150pt; height: 30pt; border: 2pt solid #333; border-radius: 15pt;">
                62%
            </progress>
        </div>
        <p style="margin-top: 20pt; font-weight: bold;">Server 2<br/>62%</p>
    </div>

    <div style="display: inline-block; margin: 0 20pt;">
        <div style="transform: rotate(-90deg); transform-origin: center;
                    width: 150pt; margin: 75pt 0;">
            <progress value="94" max="100"
                      style="width: 150pt; height: 30pt; border: 2pt solid #333; border-radius: 15pt;">
                94%
            </progress>
        </div>
        <p style="margin-top: 20pt; font-weight: bold;">Server 3<br/>94%</p>
    </div>
</div>
```

### Stacked Progress Bars

```html
<div style="border: 1pt solid #ddd; border-radius: 8pt; padding: 20pt;">
    <h3 style="margin: 0 0 15pt 0;">Storage Breakdown</h3>

    <div style="height: 40pt; position: relative; border: 1pt solid #ccc;
                border-radius: 4pt; overflow: hidden; background-color: #f0f0f0;">
        <!-- Documents: 35% -->
        <div style="position: absolute; left: 0; top: 0; bottom: 0; width: 35%;
                    background-color: #2196F3;"></div>

        <!-- Images: 25% (starts at 35%) -->
        <div style="position: absolute; left: 35%; top: 0; bottom: 0; width: 25%;
                    background-color: #4CAF50;"></div>

        <!-- Videos: 20% (starts at 60%) -->
        <div style="position: absolute; left: 60%; top: 0; bottom: 0; width: 20%;
                    background-color: #FF9800;"></div>

        <!-- Other: 15% (starts at 80%) -->
        <div style="position: absolute; left: 80%; top: 0; bottom: 0; width: 15%;
                    background-color: #9C27B0;"></div>
    </div>

    <div style="margin-top: 15pt;">
        <div style="margin: 8pt 0;">
            <span style="display: inline-block; width: 16pt; height: 16pt;
                         background-color: #2196F3; border-radius: 2pt;
                         vertical-align: middle; margin-right: 8pt;"></span>
            <strong>Documents</strong>
            <span style="float: right;">35% (350 GB)</span>
            <div style="clear: both;"></div>
        </div>
        <div style="margin: 8pt 0;">
            <span style="display: inline-block; width: 16pt; height: 16pt;
                         background-color: #4CAF50; border-radius: 2pt;
                         vertical-align: middle; margin-right: 8pt;"></span>
            <strong>Images</strong>
            <span style="float: right;">25% (250 GB)</span>
            <div style="clear: both;"></div>
        </div>
        <div style="margin: 8pt 0;">
            <span style="display: inline-block; width: 16pt; height: 16pt;
                         background-color: #FF9800; border-radius: 2pt;
                         vertical-align: middle; margin-right: 8pt;"></span>
            <strong>Videos</strong>
            <span style="float: right;">20% (200 GB)</span>
            <div style="clear: both;"></div>
        </div>
        <div style="margin: 8pt 0;">
            <span style="display: inline-block; width: 16pt; height: 16pt;
                         background-color: #9C27B0; border-radius: 2pt;
                         vertical-align: middle; margin-right: 8pt;"></span>
            <strong>Other</strong>
            <span style="float: right;">15% (150 GB)</span>
            <div style="clear: both;"></div>
        </div>
        <div style="margin: 8pt 0; padding-top: 10pt; border-top: 1pt solid #ddd;">
            <strong>Free Space</strong>
            <span style="float: right; color: #4CAF50; font-weight: bold;">
                5% (50 GB available)
            </span>
            <div style="clear: both;"></div>
        </div>
    </div>
</div>
```

---

## See Also

- [meter](/reference/htmltags/meter.html) - Meter/gauge element (for measurements with thresholds)
- [div](/reference/htmltags/div.html) - Generic container (can create custom progress indicators)
- [Data Binding](/reference/binding/) - Data binding and expressions
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Visual Components](/reference/components/visual.html) - Base visual component

---
