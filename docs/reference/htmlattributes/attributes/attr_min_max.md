---
layout: default
title: min and max
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @min and @max : The Value Range Attributes

The `min` and `max` attributes define the minimum and maximum values for numeric input fields, meter elements, and progress indicators in PDF documents. They establish the valid range for values, enabling proper percentage calculations, threshold evaluations, and visual scaling in rendered output.

## Usage

The `min` and `max` attributes control value ranges and scaling:
- Define the lower and upper bounds for numeric values
- Enable percentage calculations for meters and progress bars
- Set constraints for input field values
- Support decimal and integer values
- Work with data binding for dynamic ranges
- Essential for proper meter threshold coloring
- Default values: min=0, max=1 (for most elements)

```html
<!-- Basic meter with range 0-100 -->
<meter value="75" min="0" max="100">75%</meter>

<!-- Progress bar with custom range -->
<progress value="45" min="0" max="100">45%</progress>

<!-- Input field with numeric constraints -->
<input type="number" value="5" min="1" max="10" />

<!-- Meter with decimal range -->
<meter value="0.75" min="0" max="1">75%</meter>

<!-- Dynamic range with data binding -->
<meter value="{{model.currentValue}}"
       min="{{model.minValue}}"
       max="{{model.maxValue}}">
    {{model.currentValue}}
</meter>
```

---

## Supported Elements

The `min` and `max` attributes are supported by the following elements:

| Element | Description |
|---------|-------------|
| `<meter>` | Gauge/measurement element - defines the full scale range |
| `<progress>` | Progress bar element - defines the completion range |
| `<input>` | Input field (number, range types) - defines valid input range |

---

## How Min and Max Work

### Range Calculation

The min and max values establish the scale for percentage calculations:

```
percentage = (value - min) / (max - min) * 100%
```

**Examples:**
```html
<!-- Value 50 in range 0-100 = 50% -->
<meter value="50" min="0" max="100">50%</meter>

<!-- Value 5 in range 0-10 = 50% -->
<meter value="5" min="0" max="10">50%</meter>

<!-- Value 0.5 in range 0-1 = 50% -->
<meter value="0.5" min="0" max="1">50%</meter>

<!-- Value 150 in range 100-200 = 50% -->
<meter value="150" min="100" max="200">50%</meter>
```

### Value Clamping

Values outside the min/max range are typically clamped:

```html
<!-- Value exceeds max: displayed as 100% -->
<meter value="150" min="0" max="100">Clamped to 100%</meter>

<!-- Value below min: displayed as 0% -->
<meter value="-10" min="0" max="100">Clamped to 0%</meter>

<!-- Valid value: displayed normally -->
<meter value="75" min="0" max="100">75%</meter>
```

### Default Values

If not specified, default values are used:

```html
<!-- No min/max: defaults to 0-1 range -->
<meter value="0.6">60%</meter>

<!-- Same as: -->
<meter value="0.6" min="0" max="1">60%</meter>

<!-- Only max specified: min defaults to 0 -->
<progress value="45" max="100">45%</progress>

<!-- Only min specified: max defaults to 1 -->
<meter value="0.3" min="0">30%</meter>
```

### Meter Threshold Interaction

For meters, min/max interact with low/high/optimum thresholds:

```html
<!-- Full range with thresholds -->
<meter value="75" min="0" max="100" low="30" high="80" optimum="50">
    75%
</meter>

<!-- The thresholds are evaluated within the min/max range:
     - min=0 to low=30: Sub-suboptimal range (red)
     - low=30 to high=80: Optimal range (green if optimum is here)
     - high=80 to max=100: Suboptimal range (orange)
-->
```

---

## Binding Values

The `min` and `max` attributes support data binding for dynamic ranges:

### Simple Range Binding

```html
<!-- Model: { minTemp: 0, maxTemp: 100, currentTemp: 75 } -->
<meter value="{{model.currentTemp}}"
       min="{{model.minTemp}}"
       max="{{model.maxTemp}}">
    {{model.currentTemp}}°C
</meter>
```

### Calculated Ranges

```html
<!-- Model: { baseValue: 100, multiplier: 10 } -->
<meter value="{{model.baseValue * 5}}"
       min="0"
       max="{{model.baseValue * model.multiplier}}">
    50%
</meter>
```

### Conditional Ranges

```html
<!-- Model: { isMetric: true, value: 30 } -->
<meter value="{{model.value}}"
       min="0"
       max="{{model.isMetric ? 100 : 212}}">
    {{model.value}}{{model.isMetric ? '°C' : '°F'}}
</meter>
```

### Dynamic Input Constraints

```html
<!-- Model: { quantity: 5, minOrder: 1, maxStock: 20 } -->
<input type="number"
       value="{{model.quantity}}"
       min="{{model.minOrder}}"
       max="{{model.maxStock}}" />
```

### Repeating Elements with Varying Ranges

```html
<!-- Model: { metrics: [
    {name: "CPU", value: 45, min: 0, max: 100},
    {name: "Memory", value: 3.2, min: 0, max: 8},
    {name: "Disk", value: 450, min: 0, max: 1000}
] } -->
<template data-bind="{{model.metrics}}">
    <div style="margin-bottom: 10pt;">
        <strong>{{.name}}:</strong>
        <meter value="{{.value}}" min="{{.min}}" max="{{.max}}"
               style="width: 200pt; height: 20pt;">
            {{.value}}
        </meter>
    </div>
</template>
```

---

## Notes

### Valid Range Requirements

The max value must be greater than min:

```html
<!-- Valid: max > min -->
<meter value="50" min="0" max="100">50%</meter>

<!-- Invalid: max < min (may cause errors or unexpected behavior) -->
<meter value="50" min="100" max="0">Invalid</meter>

<!-- Invalid: max = min (division by zero) -->
<meter value="50" min="50" max="50">Invalid</meter>
```

### Negative Ranges

Negative values are fully supported:

```html
<!-- Temperature in Celsius -->
<meter value="-5" min="-20" max="40">-5°C</meter>

<!-- Financial loss/profit -->
<meter value="-500" min="-1000" max="1000" optimum="1000">
    -$500
</meter>

<!-- Elevation below sea level -->
<meter value="-85" min="-100" max="100">-85m</meter>
```

### Decimal Precision

Both min and max support decimal values:

```html
<!-- Fractional range -->
<meter value="0.667" min="0" max="1">66.7%</meter>

<!-- Decimal measurement -->
<meter value="3.14159" min="0" max="10">π</meter>

<!-- Currency with cents -->
<meter value="19.99" min="0" max="50">$19.99</meter>

<!-- Scientific notation -->
<meter value="6.022e23" min="0" max="1e24">Avogadro's number</meter>
```

### Large Value Ranges

The attributes handle large numeric ranges:

```html
<!-- Large integers -->
<meter value="1500000" min="0" max="10000000">
    1.5 million
</meter>

<!-- Population data -->
<meter value="8000000000" min="0" max="10000000000">
    8 billion people
</meter>

<!-- Bytes to Gigabytes -->
<meter value="4294967296" min="0" max="10737418240">
    4 GB
</meter>
```

### Offset Ranges

Ranges don't need to start at zero:

```html
<!-- Percentage range 50-100% -->
<meter value="75" min="50" max="100">75%</meter>

<!-- Year range -->
<meter value="2024" min="2000" max="2050">2024</meter>

<!-- pH scale (0-14, showing 7-10) -->
<meter value="8.5" min="7" max="10">pH 8.5</meter>

<!-- Speed range 60-120 mph -->
<meter value="95" min="60" max="120">95 mph</meter>
```

### Progress Bar Behavior

For progress bars, min is typically 0:

```html
<!-- Standard progress: 0-100 -->
<progress value="45" min="0" max="100">45%</progress>

<!-- Task completion: 0-10 steps -->
<progress value="7" min="0" max="10">7/10</progress>

<!-- Download: 0 to file size -->
<progress value="75" min="0" max="100">75 MB</progress>
```

### Meter vs Progress Ranges

Key differences in range usage:

| Feature | `<meter>` | `<progress>` |
|---------|-----------|--------------|
| **Default min** | 0 | 0 |
| **Default max** | 1 | 1 |
| **Negative values** | Supported | Rarely used |
| **Offset ranges** | Common (e.g., 50-100) | Uncommon |
| **Threshold interaction** | Yes (low/high/optimum) | No |
| **Typical range** | Any measurement scale | 0 to completion value |

### Threshold Constraints

When using low/high/optimum with meters:

```html
<!-- Thresholds must be within min/max range -->
<meter value="75" min="0" max="100" low="30" high="80" optimum="50">
    Valid: all thresholds within 0-100
</meter>

<!-- Invalid: high exceeds max -->
<meter value="75" min="0" max="100" low="30" high="120" optimum="50">
    Invalid: high > max
</meter>

<!-- Invalid: low below min -->
<meter value="75" min="10" max="100" low="5" high="80" optimum="50">
    Invalid: low < min
</meter>
```

### Units and Formatting

Min/max are numeric values without units:

```html
<!-- Values are numbers, labels provide context -->
<meter value="75" min="0" max="100">75°C</meter>
<meter value="75" min="0" max="100">$75</meter>
<meter value="75" min="0" max="100">75 kg</meter>
<meter value="75" min="0" max="100">75%</meter>

<!-- Format in label or surrounding text -->
<label>Temperature: </label>
<meter value="75" min="0" max="100">75</meter>
<span> °C</span>
```

### Visual Rendering Impact

The range affects visual fill calculation:

```html
<!-- Same visual appearance (50% fill) -->
<meter value="50" min="0" max="100"
       style="width: 200pt; height: 20pt;">50%</meter>

<meter value="5" min="0" max="10"
       style="width: 200pt; height: 20pt;">50%</meter>

<meter value="0.5" min="0" max="1"
       style="width: 200pt; height: 20pt;">50%</meter>
```

---

## Examples

### Basic Percentage Meter (0-100)

```html
<!-- Standard percentage meter -->
<div style="margin-bottom: 15pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Disk Usage
    </label>
    <meter value="75" min="0" max="100" low="30" high="80" optimum="20"
           style="width: 300pt; height: 24pt;">
        75%
    </meter>
    <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
        75 GB of 100 GB used
    </p>
</div>
```

### Fractional Range (0-1)

```html
<!-- Decimal range for percentages -->
<div style="margin-bottom: 15pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Completion Rate
    </label>
    <meter value="0.67" min="0" max="1" low="0.3" high="0.8" optimum="1"
           style="width: 250pt; height: 20pt;">
        67%
    </meter>
    <span style="margin-left: 10pt;">0.67 (67%)</span>
</div>
```

### Custom Score Range (0-10)

```html
<!-- Rating out of 10 -->
<div style="margin-bottom: 15pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Customer Satisfaction Score
    </label>
    <meter value="8.5" min="0" max="10" low="4" high="7" optimum="10"
           style="width: 300pt; height: 22pt;">
        8.5/10
    </meter>
    <span style="margin-left: 10pt; color: green; font-weight: bold;">
        Excellent (8.5/10)
    </span>
</div>
```

### Temperature Range with Negatives

```html
<!-- Temperature gauge with negative values -->
<div style="padding: 15pt; background-color: #f5f5f5; border-radius: 6pt;">
    <h4 style="margin: 0 0 15pt 0;">Temperature Monitoring</h4>

    <div style="margin-bottom: 12pt;">
        <strong>Freezer:</strong>
        <meter value="-18" min="-30" max="10" low="-20" high="0" optimum="-18"
               style="width: 200pt; height: 20pt; margin-left: 10pt;">
            -18°C
        </meter>
        <span style="margin-left: 10pt;">-18°C (Optimal)</span>
    </div>

    <div style="margin-bottom: 12pt;">
        <strong>Refrigerator:</strong>
        <meter value="4" min="-5" max="15" low="2" high="8" optimum="4"
               style="width: 200pt; height: 20pt; margin-left: 10pt;">
            4°C
        </meter>
        <span style="margin-left: 10pt;">4°C (Optimal)</span>
    </div>

    <div style="margin-bottom: 0;">
        <strong>Room:</strong>
        <meter value="22" min="10" max="35" low="18" high="26" optimum="22"
               style="width: 200pt; height: 20pt; margin-left: 10pt;">
            22°C
        </meter>
        <span style="margin-left: 10pt;">22°C (Optimal)</span>
    </div>
</div>
```

### Progress Bar with Custom Range

```html
<!-- File download with specific byte range -->
<div style="border: 1pt solid #ddd; padding: 15pt; border-radius: 6pt;">
    <h4 style="margin: 0 0 10pt 0;">Downloading large-file.zip</h4>

    <progress value="750" min="0" max="1200"
              style="width: 100%; height: 28pt; border: 1pt solid #ccc; border-radius: 14pt;">
        750 MB
    </progress>

    <div style="margin-top: 8pt; color: #666; font-size: 9pt;">
        <span style="float: left;">750 MB of 1,200 MB</span>
        <span style="float: right;">62.5% complete</span>
        <div style="clear: both;"></div>
    </div>
</div>
```

### Battery Level (0-100 mAh)

```html
<!-- Battery capacity meter -->
<div style="border: 2pt solid #333; border-radius: 8pt; padding: 20pt; width: 350pt;">
    <h3 style="margin: 0 0 15pt 0;">Device Battery Status</h3>

    <div style="margin-bottom: 15pt;">
        <p style="margin: 0 0 8pt 0; font-weight: bold;">
            Laptop Battery
        </p>
        <meter value="4500" min="0" max="5000" low="1000" high="3500" optimum="5000"
               style="width: 100%; height: 24pt; border: 1pt solid #666; border-radius: 4pt;">
            4500 mAh
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
            4,500 mAh of 5,000 mAh (90%)
        </p>
    </div>

    <div style="margin-bottom: 15pt;">
        <p style="margin: 0 0 8pt 0; font-weight: bold;">
            Phone Battery
        </p>
        <meter value="1800" min="0" max="3000" low="600" high="2000" optimum="3000"
               style="width: 100%; height: 24pt; border: 1pt solid #666; border-radius: 4pt;">
            1800 mAh
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
            1,800 mAh of 3,000 mAh (60%)
        </p>
    </div>

    <div style="margin-bottom: 0;">
        <p style="margin: 0 0 8pt 0; font-weight: bold;">
            Tablet Battery
        </p>
        <meter value="650" min="0" max="7000" low="1400" high="5000" optimum="7000"
               style="width: 100%; height: 24pt; border: 1pt solid #666; border-radius: 4pt;">
            650 mAh
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #d32f2f;">
            650 mAh of 7,000 mAh (9.3% - Low Battery!)
        </p>
    </div>
</div>
```

### Financial Range (Profit/Loss)

```html
<!-- Profit and loss meter -->
<div style="padding: 20pt; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white; border-radius: 10pt;">
    <h3 style="margin: 0 0 20pt 0;">Quarterly Financial Performance</h3>

    <div style="background-color: rgba(255,255,255,0.15); padding: 15pt;
                border-radius: 6pt; margin-bottom: 15pt;">
        <p style="margin: 0 0 8pt 0; font-weight: bold;">Q1 2024</p>
        <meter value="250000" min="-500000" max="1000000"
               low="0" high="500000" optimum="1000000"
               style="width: 100%; height: 26pt; border: 2pt solid rgba(255,255,255,0.3);">
            +$250K
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; opacity: 0.9;">
            Profit: $250,000
        </p>
    </div>

    <div style="background-color: rgba(255,255,255,0.15); padding: 15pt;
                border-radius: 6pt; margin-bottom: 15pt;">
        <p style="margin: 0 0 8pt 0; font-weight: bold;">Q2 2024</p>
        <meter value="-150000" min="-500000" max="1000000"
               low="0" high="500000" optimum="1000000"
               style="width: 100%; height: 26pt; border: 2pt solid rgba(255,255,255,0.3);">
            -$150K
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; opacity: 0.9;">
            Loss: $150,000
        </p>
    </div>

    <div style="background-color: rgba(255,255,255,0.15); padding: 15pt;
                border-radius: 6pt;">
        <p style="margin: 0 0 8pt 0; font-weight: bold;">Q3 2024</p>
        <meter value="750000" min="-500000" max="1000000"
               low="0" high="500000" optimum="1000000"
               style="width: 100%; height: 26pt; border: 2pt solid rgba(255,255,255,0.3);">
            +$750K
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; opacity: 0.9;">
            Profit: $750,000 (Record Quarter!)
        </p>
    </div>
</div>
```

### Year Range (2000-2050)

```html
<!-- Timeline progress meter -->
<div style="border: 1pt solid #ddd; padding: 20pt; border-radius: 8pt;">
    <h3 style="margin: 0 0 15pt 0;">Project Timeline: 2020-2030</h3>

    <div style="margin-bottom: 15pt;">
        <p style="margin: 0 0 8pt 0; font-weight: 600;">Current Progress</p>
        <meter value="2024" min="2020" max="2030" low="2022" high="2028" optimum="2030"
               style="width: 100%; height: 28pt; border: 1pt solid #336699;">
            2024
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
            Year 2024 - 40% through project timeline (4 of 10 years)
        </p>
    </div>

    <div style="background-color: #f9f9f9; padding: 12pt; border-radius: 4pt;">
        <table style="width: 100%; font-size: 9pt;">
            <tr>
                <td style="padding: 4pt;"><strong>Start:</strong></td>
                <td style="padding: 4pt;">2020</td>
            </tr>
            <tr>
                <td style="padding: 4pt;"><strong>Current:</strong></td>
                <td style="padding: 4pt;">2024</td>
            </tr>
            <tr>
                <td style="padding: 4pt;"><strong>Target Completion:</strong></td>
                <td style="padding: 4pt;">2030</td>
            </tr>
            <tr>
                <td style="padding: 4pt;"><strong>Years Remaining:</strong></td>
                <td style="padding: 4pt; color: #336699; font-weight: bold;">6 years</td>
            </tr>
        </table>
    </div>
</div>
```

### Speed Gauge (0-200 mph)

```html
<!-- Vehicle speedometer -->
<div style="text-align: center; padding: 25pt; background-color: #1a1a1a;
            color: white; border-radius: 12pt;">
    <h2 style="margin: 0 0 20pt 0; color: #00ff00;">Speed Gauge</h2>

    <div style="margin-bottom: 15pt;">
        <p style="margin: 0 0 10pt 0; font-size: 12pt;">Current Speed</p>
        <meter value="75" min="0" max="200" low="60" high="120" optimum="65"
               style="width: 400pt; height: 40pt; border: 3pt solid #333;
                      border-radius: 20pt;">
            75 mph
        </meter>
        <p style="margin: 15pt 0 0 0; font-size: 32pt; font-weight: bold;
                  color: #00ff00;">
            75 MPH
        </p>
    </div>

    <div style="margin-top: 20pt; padding-top: 15pt;
                border-top: 1pt solid rgba(255,255,255,0.2); font-size: 10pt;">
        <span style="margin: 0 15pt;">Min: 0</span>
        <span style="margin: 0 15pt;">Safe Range: 60-120</span>
        <span style="margin: 0 15pt;">Max: 200</span>
    </div>
</div>
```

### Data-Bound Dynamic Ranges

```html
<!-- Model: {
    metrics: [
        {name: "CPU Usage", value: 45, min: 0, max: 100, unit: "%"},
        {name: "Memory", value: 6.2, min: 0, max: 16, unit: " GB"},
        {name: "Disk Space", value: 450, min: 0, max: 1000, unit: " GB"},
        {name: "Temperature", value: 72, min: 20, max: 100, unit: "°C"}
    ]
} -->

<div style="padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0;">System Metrics</h3>

    <template data-bind="{{model.metrics}}">
        <div style="margin-bottom: 18pt; padding: 12pt; border: 1pt solid #e0e0e0;
                    border-radius: 6pt; background-color: #fafafa;">
            <div style="margin-bottom: 8pt;">
                <strong style="font-size: 11pt;">{{.name}}</strong>
                <span style="float: right; color: #666; font-weight: bold;">
                    {{.value}}{{.unit}}
                </span>
                <div style="clear: both;"></div>
            </div>

            <meter value="{{.value}}" min="{{.min}}" max="{{.max}}"
                   low="{{.max * 0.3}}" high="{{.max * 0.8}}" optimum="{{.min}}"
                   style="width: 100%; height: 22pt; border: 1pt solid #ccc;
                          border-radius: 11pt;">
                {{.value}}{{.unit}}
            </meter>

            <div style="margin-top: 5pt; font-size: 9pt; color: #888;">
                Range: {{.min}} - {{.max}}{{.unit}}
            </div>
        </div>
    </template>
</div>
```

### Storage Capacity (Bytes)

```html
<!-- Disk storage with large byte values -->
<div style="border: 2pt solid #4CAF50; border-radius: 8pt; padding: 20pt;">
    <h3 style="margin: 0 0 15pt 0; color: #4CAF50;">Storage Overview</h3>

    <div style="margin-bottom: 18pt;">
        <div style="margin-bottom: 8pt;">
            <strong>SSD Drive</strong>
            <span style="float: right;">250 GB / 500 GB</span>
            <div style="clear: both;"></div>
        </div>
        <meter value="268435456000" min="0" max="536870912000"
               low="161061273600" high="429496729600" optimum="0"
               style="width: 100%; height: 24pt;">
            250 GB
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
            50% used (268,435,456,000 bytes)
        </p>
    </div>

    <div style="margin-bottom: 18pt;">
        <div style="margin-bottom: 8pt;">
            <strong>HDD Archive</strong>
            <span style="float: right;">1.8 TB / 2 TB</span>
            <div style="clear: both;"></div>
        </div>
        <meter value="1978051092480" min="0" max="2199023255552"
               low="659607418675" high="1759218604441" optimum="0"
               style="width: 100%; height: 24pt;">
            1.8 TB
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #d32f2f;">
            90% used (1,978,051,092,480 bytes) - Consider cleanup
        </p>
    </div>

    <div style="margin-bottom: 0;">
        <div style="margin-bottom: 8pt;">
            <strong>Cloud Storage</strong>
            <span style="float: right;">25 GB / 100 GB</span>
            <div style="clear: both;"></div>
        </div>
        <meter value="26843545600" min="0" max="107374182400"
               low="32212254720" high="85899345920" optimum="0"
               style="width: 100%; height: 24pt;">
            25 GB
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #4CAF50;">
            25% used (26,843,545,600 bytes) - Plenty of space
        </p>
    </div>
</div>
```

### Input Number Constraints

```html
<!-- Numeric input fields with min/max constraints -->
<div style="border: 1pt solid #ddd; padding: 20pt; border-radius: 6pt;">
    <h3 style="margin: 0 0 15pt 0;">Order Form</h3>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Quantity (Min: 1, Max: 100)
        </label>
        <input type="number" value="5" min="1" max="100"
               style="width: 150pt; padding: 8pt; border: 1pt solid #ccc;
                      font-size: 11pt;" />
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
            Minimum order: 1 unit, Maximum order: 100 units
        </p>
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Age (Min: 18, Max: 120)
        </label>
        <input type="number" value="35" min="18" max="120"
               style="width: 150pt; padding: 8pt; border: 1pt solid #ccc;
                      font-size: 11pt;" />
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
            Must be 18 or older
        </p>
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Discount Percentage (Min: 0, Max: 50)
        </label>
        <input type="number" value="15" min="0" max="50"
               style="width: 150pt; padding: 8pt; border: 1pt solid #ccc;
                      font-size: 11pt;" />
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666;">
            Maximum discount allowed: 50%
        </p>
    </div>

    <div style="margin-bottom: 0;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Rating (Min: 1, Max: 5)
        </label>
        <input type="number" value="4" min="1" max="5"
               style="width: 100pt; padding: 8pt; border: 1pt solid #ccc;
                      font-size: 11pt;" />
        <span style="margin-left: 10pt; color: #FF9800; font-size: 14pt;">
            ★★★★☆
        </span>
    </div>
</div>
```

### pH Scale (0-14)

```html
<!-- Scientific measurement with specific range -->
<div style="padding: 20pt; background-color: #eceff1; border-radius: 8pt;">
    <h3 style="margin: 0 0 15pt 0;">Water Quality pH Levels</h3>

    <div style="margin-bottom: 15pt;">
        <p style="margin: 0 0 8pt 0; font-weight: 600;">
            Drinking Water (Tap)
        </p>
        <meter value="7.2" min="0" max="14" low="6.5" high="8.5" optimum="7"
               style="width: 100%; height: 24pt; border: 1pt solid #607d8b;">
            pH 7.2
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #4CAF50;">
            pH 7.2 - Safe for drinking (neutral)
        </p>
    </div>

    <div style="margin-bottom: 15pt;">
        <p style="margin: 0 0 8pt 0; font-weight: 600;">
            Swimming Pool
        </p>
        <meter value="7.4" min="0" max="14" low="7.2" high="7.8" optimum="7.5"
               style="width: 100%; height: 24pt; border: 1pt solid #607d8b;">
            pH 7.4
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #4CAF50;">
            pH 7.4 - Ideal for swimming
        </p>
    </div>

    <div style="margin-bottom: 15pt;">
        <p style="margin: 0 0 8pt 0; font-weight: 600;">
            Aquarium (Freshwater)
        </p>
        <meter value="6.8" min="0" max="14" low="6.5" high="7.5" optimum="7"
               style="width: 100%; height: 24pt; border: 1pt solid #607d8b;">
            pH 6.8
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #4CAF50;">
            pH 6.8 - Good for most freshwater fish
        </p>
    </div>

    <div style="margin-bottom: 0;">
        <p style="margin: 0 0 8pt 0; font-weight: 600;">
            Industrial Wastewater
        </p>
        <meter value="3.5" min="0" max="14" low="6" high="9" optimum="7"
               style="width: 100%; height: 24pt; border: 1pt solid #607d8b;">
            pH 3.5
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #d32f2f;">
            pH 3.5 - Acidic, requires treatment before discharge
        </p>
    </div>
</div>
```

### Pressure Gauge (PSI)

```html
<!-- Tire pressure monitoring -->
<div style="border: 2pt solid #FF5722; border-radius: 10pt; padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0; color: #FF5722;">Tire Pressure Monitor</h3>

    <table style="width: 100%;">
        <tr>
            <td style="padding: 12pt; width: 140pt; font-weight: 600;">
                Front Left
            </td>
            <td style="padding: 12pt;">
                <meter value="32" min="0" max="50" low="28" high="35" optimum="32"
                       style="width: 100%; height: 22pt; border: 1pt solid #ccc;">
                    32 PSI
                </meter>
            </td>
            <td style="padding: 12pt; width: 80pt; text-align: right; color: green;">
                32 PSI ✓
            </td>
        </tr>
        <tr style="background-color: #fff3cd;">
            <td style="padding: 12pt; font-weight: 600;">
                Front Right
            </td>
            <td style="padding: 12pt;">
                <meter value="27" min="0" max="50" low="28" high="35" optimum="32"
                       style="width: 100%; height: 22pt; border: 1pt solid #ccc;">
                    27 PSI
                </meter>
            </td>
            <td style="padding: 12pt; text-align: right; color: orange;">
                27 PSI ⚠
            </td>
        </tr>
        <tr>
            <td style="padding: 12pt; font-weight: 600;">
                Rear Left
            </td>
            <td style="padding: 12pt;">
                <meter value="33" min="0" max="50" low="28" high="35" optimum="32"
                       style="width: 100%; height: 22pt; border: 1pt solid #ccc;">
                    33 PSI
                </meter>
            </td>
            <td style="padding: 12pt; text-align: right; color: green;">
                33 PSI ✓
            </td>
        </tr>
        <tr style="background-color: #f8d7da;">
            <td style="padding: 12pt; font-weight: 600;">
                Rear Right
            </td>
            <td style="padding: 12pt;">
                <meter value="22" min="0" max="50" low="28" high="35" optimum="32"
                       style="width: 100%; height: 22pt; border: 1pt solid #ccc;">
                    22 PSI
                </meter>
            </td>
            <td style="padding: 12pt; text-align: right; color: #d32f2f;">
                22 PSI ✗
            </td>
        </tr>
    </table>

    <p style="margin: 15pt 0 0 0; padding: 10pt; background-color: #fff3cd;
              border-radius: 4pt; font-size: 10pt;">
        <strong>Warning:</strong> Front Right and Rear Right tires are below
        recommended pressure. Inflate to 32 PSI.
    </p>
</div>
```

### Server Response Time (milliseconds)

```html
<!-- API performance metrics -->
<div style="padding: 20pt; background-color: #263238; color: white; border-radius: 8pt;">
    <h3 style="margin: 0 0 20pt 0;">API Response Times</h3>

    <div style="margin-bottom: 15pt;">
        <div style="margin-bottom: 8pt;">
            <strong>/api/users</strong>
            <span style="float: right;">45 ms</span>
            <div style="clear: both;"></div>
        </div>
        <meter value="45" min="0" max="1000" low="200" high="500" optimum="0"
               style="width: 100%; height: 20pt; background-color: #37474f;">
            45ms
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; opacity: 0.8;">
            Excellent response time
        </p>
    </div>

    <div style="margin-bottom: 15pt;">
        <div style="margin-bottom: 8pt;">
            <strong>/api/products</strong>
            <span style="float: right;">285 ms</span>
            <div style="clear: both;"></div>
        </div>
        <meter value="285" min="0" max="1000" low="200" high="500" optimum="0"
               style="width: 100%; height: 20pt; background-color: #37474f;">
            285ms
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; opacity: 0.8;">
            Acceptable response time
        </p>
    </div>

    <div style="margin-bottom: 15pt;">
        <div style="margin-bottom: 8pt;">
            <strong>/api/reports</strong>
            <span style="float: right;">725 ms</span>
            <div style="clear: both;"></div>
        </div>
        <meter value="725" min="0" max="1000" low="200" high="500" optimum="0"
               style="width: 100%; height: 20pt; background-color: #37474f;">
            725ms
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; opacity: 0.8; color: #FF9800;">
            Slow - needs optimization
        </p>
    </div>

    <div style="margin-bottom: 0;">
        <div style="margin-bottom: 8pt;">
            <strong>/api/analytics</strong>
            <span style="float: right;">120 ms</span>
            <div style="clear: both;"></div>
        </div>
        <meter value="120" min="0" max="1000" low="200" high="500" optimum="0"
               style="width: 100%; height: 20pt; background-color: #37474f;">
            120ms
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; opacity: 0.8;">
            Good response time
        </p>
    </div>
</div>
```

### Offset Range Example (Exam Scores 60-100)

```html
<!-- Grade range that doesn't start at zero -->
<div style="border: 1pt solid #336699; border-radius: 8pt; padding: 20pt;">
    <h3 style="margin: 0 0 15pt 0; color: #336699;">
        Student Exam Scores (Passing: 60-100)
    </h3>

    <div style="margin-bottom: 15pt;">
        <div style="margin-bottom: 8pt;">
            <strong>Alice Johnson</strong>
            <span style="float: right; color: green; font-weight: bold;">A (95%)</span>
            <div style="clear: both;"></div>
        </div>
        <meter value="95" min="60" max="100" low="70" high="85" optimum="100"
               style="width: 100%; height: 22pt; border: 1pt solid #ccc;">
            95/100
        </meter>
    </div>

    <div style="margin-bottom: 15pt;">
        <div style="margin-bottom: 8pt;">
            <strong>Bob Smith</strong>
            <span style="float: right; color: green; font-weight: bold;">B (82%)</span>
            <div style="clear: both;"></div>
        </div>
        <meter value="82" min="60" max="100" low="70" high="85" optimum="100"
               style="width: 100%; height: 22pt; border: 1pt solid #ccc;">
            82/100
        </meter>
    </div>

    <div style="margin-bottom: 15pt;">
        <div style="margin-bottom: 8pt;">
            <strong>Carol Williams</strong>
            <span style="float: right; color: orange; font-weight: bold;">C (72%)</span>
            <div style="clear: both;"></div>
        </div>
        <meter value="72" min="60" max="100" low="70" high="85" optimum="100"
               style="width: 100%; height: 22pt; border: 1pt solid #ccc;">
            72/100
        </meter>
    </div>

    <div style="margin-bottom: 0;">
        <div style="margin-bottom: 8pt;">
            <strong>David Brown</strong>
            <span style="float: right; color: #d32f2f; font-weight: bold;">D (65%)</span>
            <div style="clear: both;"></div>
        </div>
        <meter value="65" min="60" max="100" low="70" high="85" optimum="100"
               style="width: 100%; height: 22pt; border: 1pt solid #ccc;">
            65/100
        </meter>
    </div>

    <p style="margin: 15pt 0 0 0; padding: 10pt; background-color: #e3f2fd;
              border-radius: 4pt; font-size: 10pt;">
        Note: Minimum passing score is 60. Scores are shown relative to the
        60-100 passing range.
    </p>
</div>
```

### Network Bandwidth (Mbps)

```html
<!-- Internet speed test results -->
<div style="padding: 25pt; background-color: #f5f5f5; border-radius: 10pt;">
    <h2 style="margin: 0 0 25pt 0; text-align: center; color: #333;">
        Network Speed Test Results
    </h2>

    <div style="background-color: white; padding: 20pt; border-radius: 8pt;
                margin-bottom: 15pt; box-shadow: 0 2pt 4pt rgba(0,0,0,0.1);">
        <h4 style="margin: 0 0 15pt 0; color: #4CAF50;">Download Speed</h4>
        <meter value="450" min="0" max="1000" low="100" high="500" optimum="1000"
               style="width: 100%; height: 32pt; border: 2pt solid #4CAF50;
                      border-radius: 16pt;">
            450 Mbps
        </meter>
        <div style="text-align: center; margin-top: 10pt;">
            <span style="font-size: 28pt; font-weight: bold; color: #4CAF50;">
                450 Mbps
            </span>
        </div>
    </div>

    <div style="background-color: white; padding: 20pt; border-radius: 8pt;
                margin-bottom: 15pt; box-shadow: 0 2pt 4pt rgba(0,0,0,0.1);">
        <h4 style="margin: 0 0 15pt 0; color: #2196F3;">Upload Speed</h4>
        <meter value="125" min="0" max="500" low="50" high="200" optimum="500"
               style="width: 100%; height: 32pt; border: 2pt solid #2196F3;
                      border-radius: 16pt;">
            125 Mbps
        </meter>
        <div style="text-align: center; margin-top: 10pt;">
            <span style="font-size: 28pt; font-weight: bold; color: #2196F3;">
                125 Mbps
            </span>
        </div>
    </div>

    <div style="background-color: white; padding: 20pt; border-radius: 8pt;
                box-shadow: 0 2pt 4pt rgba(0,0,0,0.1);">
        <h4 style="margin: 0 0 15pt 0; color: #FF9800;">Ping</h4>
        <meter value="18" min="0" max="100" low="30" high="50" optimum="0"
               style="width: 100%; height: 32pt; border: 2pt solid #FF9800;
                      border-radius: 16pt;">
            18 ms
        </meter>
        <div style="text-align: center; margin-top: 10pt;">
            <span style="font-size: 28pt; font-weight: bold; color: #FF9800;">
                18 ms
            </span>
        </div>
    </div>
</div>
```

### Conditional Range Based on Data

```html
<!-- Model: {
    useMetric: true,
    temperature: 25,
    fahrenheitMin: 32,
    fahrenheitMax: 212,
    celsiusMin: 0,
    celsiusMax: 100
} -->

<div style="padding: 20pt; border: 1pt solid #ddd; border-radius: 8pt;">
    <h3 style="margin: 0 0 15pt 0;">Temperature Reading</h3>

    <div style="margin-bottom: 10pt;">
        <strong>Unit System:</strong>
        <span style="margin-left: 10pt;">
            {{model.useMetric ? 'Metric (Celsius)' : 'Imperial (Fahrenheit)'}}
        </span>
    </div>

    <meter value="{{model.temperature}}"
           min="{{model.useMetric ? model.celsiusMin : model.fahrenheitMin}}"
           max="{{model.useMetric ? model.celsiusMax : model.fahrenheitMax}}"
           low="{{model.useMetric ? 20 : 68}}"
           high="{{model.useMetric ? 80 : 176}}"
           optimum="{{model.useMetric ? 22 : 72}}"
           style="width: 100%; height: 28pt; border: 1pt solid #336699;">
        {{model.temperature}}{{model.useMetric ? '°C' : '°F'}}
    </meter>

    <div style="margin-top: 10pt; font-size: 9pt; color: #666;">
        <div>Min: {{model.useMetric ? model.celsiusMin : model.fahrenheitMin}}{{model.useMetric ? '°C' : '°F'}}</div>
        <div>Max: {{model.useMetric ? model.celsiusMax : model.fahrenheitMax}}{{model.useMetric ? '°C' : '°F'}}</div>
        <div>Current: {{model.temperature}}{{model.useMetric ? '°C' : '°F'}}</div>
    </div>
</div>
```

---

## See Also

- [meter](/reference/htmltags/meter.html) - Meter/gauge element
- [progress](/reference/htmltags/progress.html) - Progress bar element
- [input](/reference/htmltags/input.html) - Input field element
- [value](/reference/htmlattributes/value.html) - Value attribute
- [high and low](/reference/htmlattributes/high_low.html) - Threshold attributes for meter
- [optimum](/reference/htmlattributes/optimum.html) - Optimal value attribute for meter
- [Data Binding](/reference/binding/) - Dynamic data binding and expressions
- [CSS Styles](/reference/styles/) - Complete CSS styling reference

---
