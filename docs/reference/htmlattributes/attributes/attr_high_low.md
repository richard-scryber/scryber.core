---
layout: default
title: high and low
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @high and @low : The Threshold Value Attributes

The `high` and `low` attributes define threshold boundaries for the meter element in PDF documents. They establish critical ranges that determine the visual color-coding of meter values, enabling automatic indication of whether measurements are in optimal, suboptimal, or critically suboptimal ranges based on the current value and optimum setting.

## Usage

The `high` and `low` attributes control threshold-based visual feedback:
- Define the boundaries between different value ranges
- Trigger automatic color changes in meter elements (green/orange/red)
- Work with the `optimum` attribute to determine color logic
- Must fall within the min/max range
- Support decimal and integer values
- Enable data binding for dynamic thresholds
- Default: low=min, high=max (when not specified)

```html
<!-- Basic meter with thresholds -->
<meter value="75" min="0" max="100" low="30" high="80" optimum="50">
    75%
</meter>

<!-- Disk usage: high usage is bad -->
<meter value="85" min="0" max="100" low="30" high="70" optimum="20">
    85% used (warning - high threshold exceeded)
</meter>

<!-- Battery level: low battery is bad -->
<meter value="25" min="0" max="100" low="20" high="80" optimum="100">
    25% remaining (warning - low threshold)
</meter>

<!-- Temperature: middle range is optimal -->
<meter value="72" min="50" max="95" low="65" high="80" optimum="72">
    72°F (optimal range)
</meter>

<!-- Dynamic thresholds with data binding -->
<meter value="{{model.value}}"
       min="{{model.min}}"
       max="{{model.max}}"
       low="{{model.lowThreshold}}"
       high="{{model.highThreshold}}"
       optimum="{{model.optimal}}">
    {{model.value}}
</meter>
```

---

## Supported Elements

The `high` and `low` attributes are supported by:

| Element | Description |
|---------|-------------|
| `<meter>` | Gauge/measurement element - uses thresholds for color-coding |

**Note:** The `<progress>` element does not support high/low attributes as it represents simple completion percentage without threshold logic.

---

## How High and Low Thresholds Work

### Threshold Ranges

The low and high attributes divide the meter into three ranges:

```
min ----[low]-------- middle --------[high]---- max
   Range 1        Range 2           Range 3
```

**Three Value Ranges:**
1. **Below Low** (min to low): Critically suboptimal range
2. **Middle** (low to high): Primary measurement range
3. **Above High** (high to max): Extreme range

### Color Logic

The meter element uses these ranges with the `optimum` value to determine color:

**When optimum is in the middle range (low ≤ optimum ≤ high):**
- Value in middle range: **Green** (optimal)
- Value below low or above high: **Orange** (suboptimal)
- Value far from optimum: **Darker orange/red** (critically suboptimal)

**When optimum is low (optimum < low):**
- Value near optimum (below low): **Green** (optimal)
- Value in middle range: **Orange** (suboptimal)
- Value above high: **Darker orange** (critically suboptimal)

**When optimum is high (optimum > high):**
- Value near optimum (above high): **Green** (optimal)
- Value in middle range: **Orange** (suboptimal)
- Value below low: **Darker orange** (critically suboptimal)

### Practical Examples

```html
<!-- Disk usage: Lower is better -->
<!-- optimum=0 means low values are optimal -->
<meter value="25" min="0" max="100" low="30" high="75" optimum="0">
    25% - Green (below low threshold, optimal)
</meter>

<meter value="50" min="0" max="100" low="30" high="75" optimum="0">
    50% - Orange (in middle range, suboptimal)
</meter>

<meter value="85" min="0" max="100" low="30" high="75" optimum="0">
    85% - Darker orange (above high threshold, critical)
</meter>

<!-- Battery level: Higher is better -->
<!-- optimum=100 means high values are optimal -->
<meter value="90" min="0" max="100" low="20" high="50" optimum="100">
    90% - Green (above high threshold, optimal)
</meter>

<meter value="35" min="0" max="100" low="20" high="50" optimum="100">
    35% - Orange (in middle range, suboptimal)
</meter>

<meter value="15" min="0" max="100" low="20" high="50" optimum="100">
    15% - Darker orange (below low threshold, critical)
</meter>

<!-- Temperature: Middle range is best -->
<!-- optimum in middle means moderate values are optimal -->
<meter value="72" min="60" max="90" low="68" high="78" optimum="72">
    72°F - Green (in middle range near optimum, optimal)
</meter>

<meter value="82" min="60" max="90" low="68" high="78" optimum="72">
    82°F - Orange (above high threshold, too hot)
</meter>

<meter value="64" min="60" max="90" low="68" high="78" optimum="72">
    64°F - Orange (below low threshold, too cold)
</meter>
```

### Default Values

If not specified, thresholds default to the range boundaries:

```html
<!-- No thresholds: low defaults to min, high defaults to max -->
<meter value="50" min="0" max="100" optimum="50">
    Entire range is considered optimal
</meter>

<!-- Same as: -->
<meter value="50" min="0" max="100" low="0" high="100" optimum="50">
    Entire range is considered optimal
</meter>

<!-- Only low specified: high defaults to max -->
<meter value="50" min="0" max="100" low="30" optimum="50">
    low=30, high=100 (default)
</meter>

<!-- Only high specified: low defaults to min -->
<meter value="50" min="0" max="100" high="80" optimum="50">
    low=0 (default), high=80
</meter>
```

---

## Binding Values

The `high` and `low` attributes support data binding for dynamic thresholds:

### Simple Threshold Binding

```html
<!-- Model: { currentTemp: 75, lowTemp: 65, highTemp: 80 } -->
<meter value="{{model.currentTemp}}"
       min="60"
       max="90"
       low="{{model.lowTemp}}"
       high="{{model.highTemp}}"
       optimum="72">
    {{model.currentTemp}}°F
</meter>
```

### Calculated Thresholds

```html
<!-- Model: { maxValue: 100 } -->
<!-- Calculate thresholds as percentages of max -->
<meter value="{{model.currentValue}}"
       min="0"
       max="{{model.maxValue}}"
       low="{{model.maxValue * 0.3}}"
       high="{{model.maxValue * 0.8}}"
       optimum="{{model.maxValue * 0.5}}">
    {{model.currentValue}}
</meter>
```

### Conditional Thresholds

```html
<!-- Model: { isStrict: true, value: 65 } -->
<!-- Stricter thresholds in strict mode -->
<meter value="{{model.value}}"
       min="0"
       max="100"
       low="{{model.isStrict ? 40 : 30}}"
       high="{{model.isStrict ? 70 : 80}}"
       optimum="50">
    {{model.value}}% ({{model.isStrict ? 'Strict' : 'Normal'}} mode)
</meter>
```

### Dynamic Thresholds from Configuration

```html
<!-- Model: {
    metrics: [
        {name: "CPU", value: 45, low: 30, high: 75},
        {name: "Memory", value: 68, low: 40, high: 80},
        {name: "Disk", value: 88, low: 50, high: 85}
    ]
} -->
<template data-bind="{{model.metrics}}">
    <div style="margin-bottom: 10pt;">
        <strong>{{.name}}:</strong>
        <meter value="{{.value}}"
               min="0"
               max="100"
               low="{{.low}}"
               high="{{.high}}"
               optimum="0"
               style="width: 200pt; height: 20pt;">
            {{.value}}%
        </meter>
    </div>
</template>
```

### Seasonal or Contextual Thresholds

```html
<!-- Model: { season: "summer", temp: 78 } -->
<!-- Different thresholds for summer vs winter -->
<meter value="{{model.temp}}"
       min="50"
       max="100"
       low="{{model.season === 'summer' ? 72 : 65}}"
       high="{{model.season === 'summer' ? 85 : 75}}"
       optimum="{{model.season === 'summer' ? 75 : 68}}">
    {{model.temp}}°F ({{model.season}})
</meter>
```

---

## Notes

### Threshold Constraints

Thresholds must follow these rules:

```html
<!-- Valid: min ≤ low ≤ high ≤ max -->
<meter value="50" min="0" max="100" low="30" high="80" optimum="50">
    Valid configuration
</meter>

<!-- Invalid: low > high -->
<meter value="50" min="0" max="100" low="80" high="30" optimum="50">
    Invalid: low exceeds high
</meter>

<!-- Invalid: low < min -->
<meter value="50" min="10" max="100" low="5" high="80" optimum="50">
    Invalid: low below min
</meter>

<!-- Invalid: high > max -->
<meter value="50" min="0" max="100" low="30" high="120" optimum="50">
    Invalid: high exceeds max
</meter>

<!-- Invalid: low = high (no middle range) -->
<meter value="50" min="0" max="100" low="50" high="50" optimum="50">
    Invalid: thresholds are equal
</meter>
```

### Threshold Positioning Best Practices

**Symmetric thresholds** (equal distance from center):
```html
<!-- 30-70 range centered at 50 -->
<meter value="60" min="0" max="100" low="30" high="70" optimum="50">
    Symmetric thresholds
</meter>
```

**Asymmetric thresholds** (unequal ranges):
```html
<!-- Larger tolerance above, smaller tolerance below -->
<meter value="50" min="0" max="100" low="40" high="80" optimum="50">
    Asymmetric: more room above
</meter>
```

**Tight thresholds** (narrow optimal range):
```html
<!-- Very narrow optimal range -->
<meter value="50" min="0" max="100" low="48" high="52" optimum="50">
    Precision required: ±2%
</meter>
```

**Wide thresholds** (broad optimal range):
```html
<!-- Very wide optimal range -->
<meter value="50" min="0" max="100" low="20" high="80" optimum="50">
    Lenient: 60% of range is optimal
</meter>
```

### Threshold Spacing

Consider the meaning of each range:

```html
<!-- Disk usage: critical/warning/ok -->
<meter value="85" min="0" max="100" low="30" high="75" optimum="0">
    0-30%: Optimal (green)
    30-75%: Warning (orange)
    75-100%: Critical (dark orange/red)
</meter>

<!-- Battery: critical/low/good/excellent -->
<meter value="25" min="0" max="100" low="20" high="50" optimum="100">
    0-20%: Critical (dark orange/red)
    20-50%: Low (orange)
    50-100%: Good/Excellent (green)
</meter>

<!-- Performance score: fail/pass/excellent -->
<meter value="75" min="0" max="100" low="60" high="85" optimum="100">
    0-60%: Fail (dark orange/red)
    60-85%: Pass (orange)
    85-100%: Excellent (green)
</meter>
```

### Color Transitions

Understanding when colors change:

```html
<!-- CPU Usage: optimum=0 (low is best) -->
<meter value="25" min="0" max="100" low="30" high="75" optimum="0">
    25% - Green (value < low, near optimum)
</meter>

<meter value="30" min="0" max="100" low="30" high="75" optimum="0">
    30% - Orange (value = low, entering warning zone)
</meter>

<meter value="50" min="0" max="100" low="30" high="75" optimum="0">
    50% - Orange (value between low and high)
</meter>

<meter value="75" min="0" max="100" low="30" high="75" optimum="0">
    75% - Orange (value = high, at critical boundary)
</meter>

<meter value="85" min="0" max="100" low="30" high="75" optimum="0">
    85% - Darker orange (value > high, critical)
</meter>
```

### Threshold Interpretation Patterns

**"Lower is better" pattern:**
```html
<!-- CPU, Memory, Disk usage -->
<meter value="45" min="0" max="100" low="30" high="75" optimum="0">
    Low values are optimal (green below low threshold)
</meter>
```

**"Higher is better" pattern:**
```html
<!-- Battery level, Progress, Quality scores -->
<meter value="85" min="0" max="100" low="20" high="50" optimum="100">
    High values are optimal (green above high threshold)
</meter>
```

**"Middle is better" pattern:**
```html
<!-- Temperature, pH levels, Pressure -->
<meter value="72" min="60" max="90" low="68" high="78" optimum="72">
    Middle values are optimal (green in middle range)
</meter>
```

### Decimal Thresholds

Thresholds support decimal precision:

```html
<!-- Fractional thresholds -->
<meter value="0.67" min="0" max="1" low="0.3" high="0.8" optimum="0.5">
    0.67 (67%)
</meter>

<!-- Scientific measurements -->
<meter value="7.2" min="0" max="14" low="6.5" high="8.5" optimum="7">
    pH 7.2
</meter>

<!-- Currency with cents -->
<meter value="19.99" min="0" max="50" low="15" high="40" optimum="25">
    $19.99
</meter>
```

### Thresholds with Negative Ranges

Thresholds work with negative values:

```html
<!-- Temperature in Celsius -->
<meter value="-5" min="-20" max="40" low="-10" high="30" optimum="20">
    -5°C
</meter>

<!-- Financial: profit/loss -->
<meter value="150000" min="-500000" max="1000000"
       low="0" high="500000" optimum="1000000">
    $150K profit
</meter>
```

### Visual Impact

Thresholds directly affect PDF rendering:

```html
<!-- Same value, different thresholds, different colors -->

<!-- Tight thresholds: value exceeds high → orange -->
<meter value="75" min="0" max="100" low="30" high="70" optimum="50"
       style="width: 200pt; height: 24pt;">
    75% (Warning - orange)
</meter>

<!-- Loose thresholds: value within range → green -->
<meter value="75" min="0" max="100" low="30" high="90" optimum="50"
       style="width: 200pt; height: 24pt;">
    75% (Optimal - green)
</meter>
```

### No Thresholds vs. With Thresholds

```html
<!-- Without explicit thresholds: all values treated as optimal -->
<meter value="75" min="0" max="100" optimum="50">
    No thresholds: green fill regardless of value
</meter>

<!-- With thresholds: color changes based on range -->
<meter value="75" min="0" max="100" low="30" high="70" optimum="50">
    With thresholds: color changes at 30 and 70
</meter>
```

### Threshold Testing Strategy

Test boundary values:

```html
<!-- Test all boundary conditions -->

<!-- Value at low threshold -->
<meter value="30" min="0" max="100" low="30" high="70" optimum="50">
    At low boundary (30)
</meter>

<!-- Value just below low threshold -->
<meter value="29" min="0" max="100" low="30" high="70" optimum="50">
    Below low boundary (29)
</meter>

<!-- Value at high threshold -->
<meter value="70" min="0" max="100" low="30" high="70" optimum="50">
    At high boundary (70)
</meter>

<!-- Value just above high threshold -->
<meter value="71" min="0" max="100" low="30" high="70" optimum="50">
    Above high boundary (71)
</meter>
```

---

## Examples

### Disk Space Monitor with Thresholds

```html
<div style="padding: 20pt; background-color: #f5f5f5; border-radius: 8pt;">
    <h3 style="margin: 0 0 20pt 0;">Storage Capacity</h3>

    <div style="margin-bottom: 18pt; padding: 15pt; background-color: white;
                border-radius: 6pt; border: 1pt solid #e0e0e0;">
        <div style="margin-bottom: 8pt;">
            <strong style="font-size: 11pt;">C: System Drive</strong>
            <span style="float: right; color: green; font-weight: bold;">
                125 GB / 500 GB (25%)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="25" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 24pt; border: 1pt solid #ccc;">
            25%
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #4CAF50;">
            Excellent - Plenty of free space
        </p>
    </div>

    <div style="margin-bottom: 18pt; padding: 15pt; background-color: white;
                border-radius: 6pt; border: 1pt solid #e0e0e0;">
        <div style="margin-bottom: 8pt;">
            <strong style="font-size: 11pt;">D: Data Drive</strong>
            <span style="float: right; color: orange; font-weight: bold;">
                560 GB / 1000 GB (56%)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="56" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 24pt; border: 1pt solid #ccc;">
            56%
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #FF9800;">
            Moderate - Monitor usage
        </p>
    </div>

    <div style="margin-bottom: 0; padding: 15pt; background-color: white;
                border-radius: 6pt; border: 1pt solid #e0e0e0;">
        <div style="margin-bottom: 8pt;">
            <strong style="font-size: 11pt;">E: Backup Drive</strong>
            <span style="float: right; color: #d32f2f; font-weight: bold;">
                1850 GB / 2000 GB (92.5%)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="92.5" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 24pt; border: 1pt solid #ccc;">
            92.5%
        </meter>
        <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #d32f2f;">
            Critical - Cleanup required immediately!
        </p>
    </div>
</div>
```

### Battery Level Indicator with Thresholds

```html
<div style="border: 2pt solid #333; border-radius: 10pt; padding: 20pt; width: 400pt;">
    <h3 style="margin: 0 0 20pt 0; text-align: center;">Device Battery Status</h3>

    <div style="margin-bottom: 20pt; padding: 15pt; background-color: #f9f9f9;
                border-radius: 6pt;">
        <div style="margin-bottom: 10pt;">
            <strong>Laptop</strong>
            <span style="float: right; font-size: 18pt; font-weight: bold; color: green;">
                85%
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="85" min="0" max="100" low="20" high="50" optimum="100"
               style="width: 100%; height: 28pt; border: 2pt solid #4CAF50;
                      border-radius: 14pt;">
            85%
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 9pt; color: #4CAF50;">
            Excellent battery level - Above high threshold (50%)
        </p>
    </div>

    <div style="margin-bottom: 20pt; padding: 15pt; background-color: #fff3cd;
                border-radius: 6pt;">
        <div style="margin-bottom: 10pt;">
            <strong>Tablet</strong>
            <span style="float: right; font-size: 18pt; font-weight: bold; color: orange;">
                35%
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="35" min="0" max="100" low="20" high="50" optimum="100"
               style="width: 100%; height: 28pt; border: 2pt solid #FF9800;
                      border-radius: 14pt;">
            35%
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 9pt; color: #FF9800;">
            Moderate - Between low (20%) and high (50%) thresholds
        </p>
    </div>

    <div style="margin-bottom: 0; padding: 15pt; background-color: #f8d7da;
                border-radius: 6pt;">
        <div style="margin-bottom: 10pt;">
            <strong>Phone</strong>
            <span style="float: right; font-size: 18pt; font-weight: bold; color: #d32f2f;">
                15%
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="15" min="0" max="100" low="20" high="50" optimum="100"
               style="width: 100%; height: 28pt; border: 2pt solid #d32f2f;
                      border-radius: 14pt;">
            15%
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 9pt; color: #d32f2f;">
            Critical - Below low threshold (20%) - Charge immediately!
        </p>
    </div>
</div>
```

### Temperature Control with Middle-Range Optimal

```html
<div style="padding: 25pt; background-color: #263238; color: white; border-radius: 10pt;">
    <h3 style="margin: 0 0 25pt 0; text-align: center;">HVAC Temperature Control</h3>

    <div style="margin-bottom: 20pt; padding: 18pt; background-color: #37474f;
                border-radius: 8pt;">
        <h4 style="margin: 0 0 12pt 0; color: #4CAF50;">Living Room</h4>
        <meter value="72" min="60" max="85" low="68" high="76" optimum="72"
               style="width: 100%; height: 32pt; border: 2pt solid rgba(255,255,255,0.3);">
            72°F
        </meter>
        <div style="margin-top: 10pt; text-align: center;">
            <span style="font-size: 28pt; font-weight: bold; color: #4CAF50;">
                72°F
            </span>
            <br/>
            <span style="font-size: 11pt; opacity: 0.9;">
                Optimal (68-76°F range) ✓
            </span>
        </div>
    </div>

    <div style="margin-bottom: 20pt; padding: 18pt; background-color: #37474f;
                border-radius: 8pt;">
        <h4 style="margin: 0 0 12pt 0; color: #FF9800;">Bedroom</h4>
        <meter value="65" min="60" max="85" low="68" high="76" optimum="72"
               style="width: 100%; height: 32pt; border: 2pt solid rgba(255,255,255,0.3);">
            65°F
        </meter>
        <div style="margin-top: 10pt; text-align: center;">
            <span style="font-size: 28pt; font-weight: bold; color: #FF9800;">
                65°F
            </span>
            <br/>
            <span style="font-size: 11pt; opacity: 0.9;">
                Too Cold - Below low threshold (68°F) ⚠
            </span>
        </div>
    </div>

    <div style="margin-bottom: 0; padding: 18pt; background-color: #37474f;
                border-radius: 8pt;">
        <h4 style="margin: 0 0 12pt 0; color: #FF5722;">Kitchen</h4>
        <meter value="80" min="60" max="85" low="68" high="76" optimum="72"
               style="width: 100%; height: 32pt; border: 2pt solid rgba(255,255,255,0.3);">
            80°F
        </meter>
        <div style="margin-top: 10pt; text-align: center;">
            <span style="font-size: 28pt; font-weight: bold; color: #FF5722;">
                80°F
            </span>
            <br/>
            <span style="font-size: 11pt; opacity: 0.9;">
                Too Hot - Above high threshold (76°F) ⚠
            </span>
        </div>
    </div>

    <div style="margin-top: 25pt; padding: 15pt; background-color: rgba(255,255,255,0.1);
                border-radius: 6pt; font-size: 10pt; text-align: center;">
        <strong>Target Range:</strong> 68°F - 76°F  |
        <strong>Optimal:</strong> 72°F
    </div>
</div>
```

### Server Performance Monitoring

```html
<div style="border: 1pt solid #336699; border-radius: 8pt; padding: 20pt;">
    <h3 style="margin: 0 0 20pt 0; color: #336699;">Server Resource Usage</h3>

    <table style="width: 100%; border-collapse: collapse;">
        <tr style="background-color: #f9f9f9;">
            <td style="padding: 15pt; width: 140pt; font-weight: 600;">
                CPU Usage
            </td>
            <td style="padding: 15pt;">
                <meter value="45" min="0" max="100" low="30" high="75" optimum="0"
                       style="width: 100%; height: 22pt; border: 1pt solid #ccc;">
                    45%
                </meter>
            </td>
            <td style="padding: 15pt; width: 120pt; text-align: center;">
                <span style="background-color: #4CAF50; color: white; padding: 6pt 12pt;
                             border-radius: 4pt; font-weight: bold; font-size: 10pt;">
                    Normal
                </span>
                <div style="font-size: 9pt; color: #666; margin-top: 4pt;">
                    Below 75% threshold
                </div>
            </td>
        </tr>
        <tr>
            <td style="padding: 15pt; font-weight: 600;">
                Memory Usage
            </td>
            <td style="padding: 15pt;">
                <meter value="68" min="0" max="100" low="30" high="75" optimum="0"
                       style="width: 100%; height: 22pt; border: 1pt solid #ccc;">
                    68%
                </meter>
            </td>
            <td style="padding: 15pt; text-align: center;">
                <span style="background-color: #4CAF50; color: white; padding: 6pt 12pt;
                             border-radius: 4pt; font-weight: bold; font-size: 10pt;">
                    Normal
                </span>
                <div style="font-size: 9pt; color: #666; margin-top: 4pt;">
                    Below 75% threshold
                </div>
            </td>
        </tr>
        <tr style="background-color: #fff3cd;">
            <td style="padding: 15pt; font-weight: 600;">
                Disk I/O
            </td>
            <td style="padding: 15pt;">
                <meter value="78" min="0" max="100" low="30" high="75" optimum="0"
                       style="width: 100%; height: 22pt; border: 1pt solid #ccc;">
                    78%
                </meter>
            </td>
            <td style="padding: 15pt; text-align: center;">
                <span style="background-color: #FF9800; color: white; padding: 6pt 12pt;
                             border-radius: 4pt; font-weight: bold; font-size: 10pt;">
                    Warning
                </span>
                <div style="font-size: 9pt; color: #666; margin-top: 4pt;">
                    Above 75% threshold
                </div>
            </td>
        </tr>
        <tr style="background-color: #f8d7da;">
            <td style="padding: 15pt; font-weight: 600;">
                Network Load
            </td>
            <td style="padding: 15pt;">
                <meter value="92" min="0" max="100" low="30" high="75" optimum="0"
                       style="width: 100%; height: 22pt; border: 1pt solid #ccc;">
                    92%
                </meter>
            </td>
            <td style="padding: 15pt; text-align: center;">
                <span style="background-color: #d32f2f; color: white; padding: 6pt 12pt;
                             border-radius: 4pt; font-weight: bold; font-size: 10pt;">
                    Critical
                </span>
                <div style="font-size: 9pt; color: #666; margin-top: 4pt;">
                    Well above 75% threshold
                </div>
            </td>
        </tr>
    </table>

    <div style="margin-top: 20pt; padding: 15pt; background-color: #e3f2fd;
                border-radius: 6pt; font-size: 10pt;">
        <strong>Threshold Configuration:</strong><br/>
        Normal: Below 30% (optimal) | Warning: 30-75% | Critical: Above 75%
    </div>
</div>
```

### Student Performance with Grading Thresholds

```html
<div style="border: 2pt solid #673AB7; border-radius: 10pt; padding: 25pt;">
    <h2 style="margin: 0 0 25pt 0; color: #673AB7; text-align: center;">
        Student Performance Report
    </h2>

    <div style="margin-bottom: 22pt; padding: 18pt; background-color: #d4edda;
                border-left: 4pt solid #4CAF50; border-radius: 4pt;">
        <div style="margin-bottom: 10pt;">
            <strong style="font-size: 12pt;">Mathematics</strong>
            <span style="float: right; font-size: 16pt; font-weight: bold; color: #4CAF50;">
                A (92%)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="92" min="0" max="100" low="60" high="80" optimum="100"
               style="width: 100%; height: 26pt; border: 1pt solid #4CAF50;">
            92%
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 10pt; color: #155724;">
            Excellent - Above high threshold (80%)
        </p>
    </div>

    <div style="margin-bottom: 22pt; padding: 18pt; background-color: #d4edda;
                border-left: 4pt solid #8BC34A; border-radius: 4pt;">
        <div style="margin-bottom: 10pt;">
            <strong style="font-size: 12pt;">Science</strong>
            <span style="float: right; font-size: 16pt; font-weight: bold; color: #8BC34A;">
                B (78%)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="78" min="0" max="100" low="60" high="80" optimum="100"
               style="width: 100%; height: 26pt; border: 1pt solid #8BC34A;">
            78%
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 10pt; color: #558b2f;">
            Good - In target range (60-80%)
        </p>
    </div>

    <div style="margin-bottom: 22pt; padding: 18pt; background-color: #fff3cd;
                border-left: 4pt solid #FF9800; border-radius: 4pt;">
        <div style="margin-bottom: 10pt;">
            <strong style="font-size: 12pt;">English</strong>
            <span style="float: right; font-size: 16pt; font-weight: bold; color: #FF9800;">
                C (68%)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="68" min="0" max="100" low="60" high="80" optimum="100"
               style="width: 100%; height: 26pt; border: 1pt solid #FF9800;">
            68%
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 10pt; color: #856404;">
            Satisfactory - In target range, near low threshold (60%)
        </p>
    </div>

    <div style="margin-bottom: 0; padding: 18pt; background-color: #f8d7da;
                border-left: 4pt solid #d32f2f; border-radius: 4pt;">
        <div style="margin-bottom: 10pt;">
            <strong style="font-size: 12pt;">History</strong>
            <span style="float: right; font-size: 16pt; font-weight: bold; color: #d32f2f;">
                D (55%)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="55" min="0" max="100" low="60" high="80" optimum="100"
               style="width: 100%; height: 26pt; border: 1pt solid #d32f2f;">
            55%
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 10pt; color: #721c24;">
            Needs Improvement - Below low threshold (60%)
        </p>
    </div>

    <div style="margin-top: 25pt; padding: 15pt; background-color: #e3f2fd;
                border-radius: 6pt; font-size: 10pt;">
        <strong>Grading Scale:</strong><br/>
        90-100%: A (Excellent) |
        80-89%: B (Good) |
        60-79%: C (Satisfactory) |
        Below 60%: D/F (Needs Improvement)
        <br/><br/>
        <strong>Thresholds:</strong>
        Low = 60% (Minimum passing) | High = 80% (Honor roll threshold)
    </div>
</div>
```

### Water Quality pH Monitoring

```html
<div style="padding: 20pt; background-color: #eceff1; border-radius: 10pt;">
    <h3 style="margin: 0 0 20pt 0; color: #37474f;">Water Quality Monitoring</h3>

    <div style="margin-bottom: 18pt; padding: 15pt; background-color: white;
                border-radius: 6pt; box-shadow: 0 2pt 4pt rgba(0,0,0,0.1);">
        <div style="margin-bottom: 10pt;">
            <strong>Drinking Water Supply</strong>
            <span style="float: right; color: green; font-weight: bold;">
                pH 7.2 (Optimal)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="7.2" min="0" max="14" low="6.5" high="8.5" optimum="7"
               style="width: 100%; height: 24pt; border: 1pt solid #607d8b;">
            pH 7.2
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 9pt; color: #4CAF50;">
            Safe range (6.5-8.5) - Near neutral optimal (pH 7)
        </p>
    </div>

    <div style="margin-bottom: 18pt; padding: 15pt; background-color: white;
                border-radius: 6pt; box-shadow: 0 2pt 4pt rgba(0,0,0,0.1);">
        <div style="margin-bottom: 10pt;">
            <strong>Swimming Pool</strong>
            <span style="float: right; color: green; font-weight: bold;">
                pH 7.6 (Good)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="7.6" min="0" max="14" low="7.2" high="7.8" optimum="7.5"
               style="width: 100%; height: 24pt; border: 1pt solid #607d8b;">
            pH 7.6
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 9pt; color: #4CAF50;">
            Optimal range (7.2-7.8) - Safe for swimming
        </p>
    </div>

    <div style="margin-bottom: 18pt; padding: 15pt; background-color: #fff3cd;
                border-radius: 6pt; box-shadow: 0 2pt 4pt rgba(0,0,0,0.1);">
        <div style="margin-bottom: 10pt;">
            <strong>Industrial Cooling System</strong>
            <span style="float: right; color: orange; font-weight: bold;">
                pH 9.2 (High)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="9.2" min="0" max="14" low="6.5" high="8.5" optimum="7"
               style="width: 100%; height: 24pt; border: 1pt solid #607d8b;">
            pH 9.2
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 9pt; color: #FF9800;">
            Above high threshold (8.5) - Requires treatment
        </p>
    </div>

    <div style="margin-bottom: 0; padding: 15pt; background-color: #f8d7da;
                border-radius: 6pt; box-shadow: 0 2pt 4pt rgba(0,0,0,0.1);">
        <div style="margin-bottom: 10pt;">
            <strong>Wastewater Discharge</strong>
            <span style="float: right; color: #d32f2f; font-weight: bold;">
                pH 4.2 (Critical)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="4.2" min="0" max="14" low="6.5" high="8.5" optimum="7"
               style="width: 100%; height: 24pt; border: 1pt solid #607d8b;">
            pH 4.2
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 9pt; color: #d32f2f;">
            Below low threshold (6.5) - Acidic, must neutralize before discharge
        </p>
    </div>

    <div style="margin-top: 20pt; padding: 15pt; background-color: #bbdefb;
                border-radius: 6pt; font-size: 10pt;">
        <strong>pH Scale Reference:</strong><br/>
        0-6.5: Acidic | 6.5-8.5: Neutral (Safe) | 8.5-14: Alkaline<br/>
        <strong>Thresholds:</strong> Low = 6.5 | High = 8.5 | Optimal = 7.0
    </div>
</div>
```

### Network Speed Test with Bandwidth Thresholds

```html
<div style="padding: 30pt; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white; border-radius: 12pt;">
    <h2 style="margin: 0 0 30pt 0; text-align: center; font-size: 20pt;">
        Internet Speed Test Results
    </h2>

    <div style="background-color: rgba(255,255,255,0.15); padding: 20pt;
                border-radius: 10pt; margin-bottom: 20pt;">
        <h3 style="margin: 0 0 15pt 0;">Download Speed</h3>
        <meter value="850" min="0" max="1000" low="100" high="500" optimum="1000"
               style="width: 100%; height: 40pt; border: 3pt solid rgba(255,255,255,0.3);">
            850 Mbps
        </meter>
        <div style="margin-top: 15pt; text-align: center;">
            <span style="font-size: 36pt; font-weight: bold;">850 Mbps</span>
            <br/>
            <span style="font-size: 12pt; opacity: 0.9;">
                Excellent - Above high threshold (500 Mbps)
            </span>
        </div>
        <div style="margin-top: 12pt; padding: 12pt; background-color: rgba(76,175,80,0.3);
                    border-radius: 6pt; font-size: 10pt;">
            <strong>Status:</strong> Exceeds high-speed threshold<br/>
            Ideal for 4K streaming, gaming, and large file transfers
        </div>
    </div>

    <div style="background-color: rgba(255,255,255,0.15); padding: 20pt;
                border-radius: 10pt; margin-bottom: 20pt;">
        <h3 style="margin: 0 0 15pt 0;">Upload Speed</h3>
        <meter value="250" min="0" max="500" low="50" high="200" optimum="500"
               style="width: 100%; height: 40pt; border: 3pt solid rgba(255,255,255,0.3);">
            250 Mbps
        </meter>
        <div style="margin-top: 15pt; text-align: center;">
            <span style="font-size: 36pt; font-weight: bold;">250 Mbps</span>
            <br/>
            <span style="font-size: 12pt; opacity: 0.9;">
                Good - Above high threshold (200 Mbps)
            </span>
        </div>
        <div style="margin-top: 12pt; padding: 12pt; background-color: rgba(76,175,80,0.3);
                    border-radius: 6pt; font-size: 10pt;">
            <strong>Status:</strong> Excellent upload performance<br/>
            Perfect for video conferencing and cloud backups
        </div>
    </div>

    <div style="background-color: rgba(255,255,255,0.15); padding: 20pt;
                border-radius: 10pt;">
        <h3 style="margin: 0 0 15pt 0;">Ping (Lower is Better)</h3>
        <meter value="22" min="0" max="100" low="30" high="60" optimum="0"
               style="width: 100%; height: 40pt; border: 3pt solid rgba(255,255,255,0.3);">
            22 ms
        </meter>
        <div style="margin-top: 15pt; text-align: center;">
            <span style="font-size: 36pt; font-weight: bold;">22 ms</span>
            <br/>
            <span style="font-size: 12pt; opacity: 0.9;">
                Excellent - Below low threshold (30 ms)
            </span>
        </div>
        <div style="margin-top: 12pt; padding: 12pt; background-color: rgba(76,175,80,0.3);
                    border-radius: 6pt; font-size: 10pt;">
            <strong>Status:</strong> Low latency connection<br/>
            Optimal for online gaming and real-time applications
        </div>
    </div>

    <div style="margin-top: 25pt; padding: 18pt; background-color: rgba(255,255,255,0.1);
                border-radius: 8pt; font-size: 11pt;">
        <strong>Performance Thresholds:</strong><br/>
        <table style="width: 100%; margin-top: 10pt; font-size: 10pt;">
            <tr>
                <td style="padding: 5pt;">Download:</td>
                <td style="padding: 5pt;">0-100 Mbps (Slow) | 100-500 Mbps (Good) | 500+ Mbps (Excellent)</td>
            </tr>
            <tr>
                <td style="padding: 5pt;">Upload:</td>
                <td style="padding: 5pt;">0-50 Mbps (Slow) | 50-200 Mbps (Good) | 200+ Mbps (Excellent)</td>
            </tr>
            <tr>
                <td style="padding: 5pt;">Ping:</td>
                <td style="padding: 5pt;">0-30 ms (Excellent) | 30-60 ms (Good) | 60+ ms (Poor)</td>
            </tr>
        </table>
    </div>
</div>
```

### Air Quality Index (AQI) Monitor

```html
<div style="border: 2pt solid #4CAF50; border-radius: 10pt; padding: 25pt;">
    <h2 style="margin: 0 0 25pt 0; color: #2e7d32; text-align: center;">
        Air Quality Monitoring Dashboard
    </h2>

    <div style="margin-bottom: 20pt; padding: 18pt; background-color: #e8f5e9;
                border-radius: 8pt; border-left: 5pt solid #4CAF50;">
        <div style="margin-bottom: 10pt;">
            <strong style="font-size: 12pt;">Downtown Station</strong>
            <span style="float: right; font-size: 16pt; font-weight: bold; color: #4CAF50;">
                AQI: 42 (Good)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="42" min="0" max="300" low="50" high="100" optimum="0"
               style="width: 100%; height: 28pt; border: 1pt solid #4CAF50;">
            AQI 42
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 10pt; color: #2e7d32;">
            Good - Below low threshold (50). Air quality is satisfactory.
        </p>
    </div>

    <div style="margin-bottom: 20pt; padding: 18pt; background-color: #fff3e0;
                border-radius: 8pt; border-left: 5pt solid #FF9800;">
        <div style="margin-bottom: 10pt;">
            <strong style="font-size: 12pt;">Industrial Zone</strong>
            <span style="float: right; font-size: 16pt; font-weight: bold; color: #FF9800;">
                AQI: 78 (Moderate)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="78" min="0" max="300" low="50" high="100" optimum="0"
               style="width: 100%; height: 28pt; border: 1pt solid #FF9800;">
            AQI 78
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 10pt; color: #e65100;">
            Moderate - Between low (50) and high (100). Sensitive groups may experience effects.
        </p>
    </div>

    <div style="margin-bottom: 20pt; padding: 18pt; background-color: #fce4ec;
                border-radius: 8pt; border-left: 5pt solid #d32f2f;">
        <div style="margin-bottom: 10pt;">
            <strong style="font-size: 12pt;">Highway Corridor</strong>
            <span style="float: right; font-size: 16pt; font-weight: bold; color: #d32f2f;">
                AQI: 165 (Unhealthy)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="165" min="0" max="300" low="50" high="100" optimum="0"
               style="width: 100%; height: 28pt; border: 1pt solid #d32f2f;">
            AQI 165
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 10pt; color: #c62828;">
            Unhealthy - Well above high threshold (100). Everyone may experience health effects.
        </p>
    </div>

    <div style="margin-bottom: 0; padding: 18pt; background-color: #f3e5f5;
                border-radius: 8pt; border-left: 5pt solid #9C27B0;">
        <div style="margin-bottom: 10pt;">
            <strong style="font-size: 12pt;">Residential Area</strong>
            <span style="float: right; font-size: 16pt; font-weight: bold; color: #7B1FA2;">
                AQI: 28 (Good)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="28" min="0" max="300" low="50" high="100" optimum="0"
               style="width: 100%; height: 28pt; border: 1pt solid #9C27B0;">
            AQI 28
        </meter>
        <p style="margin: 8pt 0 0 0; font-size: 10pt; color: #7B1FA2;">
            Good - Below low threshold (50). Excellent air quality.
        </p>
    </div>

    <div style="margin-top: 25pt; padding: 18pt; background-color: #e3f2fd;
                border-radius: 8pt; font-size: 10pt;">
        <strong>AQI Scale & Thresholds:</strong><br/>
        <table style="width: 100%; margin-top: 10pt; font-size: 9pt; border-collapse: collapse;">
            <tr style="background-color: #c8e6c9;">
                <td style="padding: 6pt; border: 1pt solid #ddd;">0-50</td>
                <td style="padding: 6pt; border: 1pt solid #ddd;">Good</td>
                <td style="padding: 6pt; border: 1pt solid #ddd;">Air quality is satisfactory</td>
            </tr>
            <tr style="background-color: #fff9c4;">
                <td style="padding: 6pt; border: 1pt solid #ddd;">51-100</td>
                <td style="padding: 6pt; border: 1pt solid #ddd;">Moderate</td>
                <td style="padding: 6pt; border: 1pt solid #ddd;">Acceptable for most people</td>
            </tr>
            <tr style="background-color: #ffccbc;">
                <td style="padding: 6pt; border: 1pt solid #ddd;">101-150</td>
                <td style="padding: 6pt; border: 1pt solid #ddd;">Unhealthy for Sensitive</td>
                <td style="padding: 6pt; border: 1pt solid #ddd;">May affect sensitive groups</td>
            </tr>
            <tr style="background-color: #ffcdd2;">
                <td style="padding: 6pt; border: 1pt solid #ddd;">151-200</td>
                <td style="padding: 6pt; border: 1pt solid #ddd;">Unhealthy</td>
                <td style="padding: 6pt; border: 1pt solid #ddd;">May affect general public</td>
            </tr>
        </table>
        <div style="margin-top: 10pt;">
            <strong>Current Thresholds:</strong> Low = 50 (Moderate boundary) | High = 100 (Unhealthy boundary)
        </div>
    </div>
</div>
```

### Data-Bound Dynamic Thresholds

```html
<!-- Model: {
    servers: [
        {name: "Web-01", cpu: 42, memLow: 30, memHigh: 75, mem: 58},
        {name: "Web-02", cpu: 78, memLow: 30, memHigh: 75, mem: 82},
        {name: "DB-01", cpu: 65, memLow: 40, memHigh: 80, mem: 72},
        {name: "Cache-01", cpu: 25, memLow: 20, memHigh: 70, mem: 45}
    ]
} -->

<div style="padding: 20pt; background-color: #fafafa; border-radius: 8pt;">
    <h3 style="margin: 0 0 20pt 0;">Server Fleet Monitoring</h3>

    <template data-bind="{{model.servers}}">
        <div style="margin-bottom: 20pt; padding: 15pt; background-color: white;
                    border-radius: 6pt; border: 1pt solid #e0e0e0;
                    box-shadow: 0 2pt 4pt rgba(0,0,0,0.1);">
            <h4 style="margin: 0 0 15pt 0; color: #336699;">{{.name}}</h4>

            <div style="margin-bottom: 12pt;">
                <div style="margin-bottom: 6pt;">
                    <strong>CPU Usage</strong>
                    <span style="float: right; font-weight: bold;">{{.cpu}}%</span>
                    <div style="clear: both;"></div>
                </div>
                <meter value="{{.cpu}}" min="0" max="100" low="30" high="75" optimum="0"
                       style="width: 100%; height: 20pt; border: 1pt solid #ccc;">
                    {{.cpu}}%
                </meter>
                <p style="margin: 4pt 0 0 0; font-size: 9pt; color: #666;">
                    Thresholds: Low=30%, High=75%
                </p>
            </div>

            <div style="margin-bottom: 0;">
                <div style="margin-bottom: 6pt;">
                    <strong>Memory Usage</strong>
                    <span style="float: right; font-weight: bold;">{{.mem}}%</span>
                    <div style="clear: both;"></div>
                </div>
                <meter value="{{.mem}}" min="0" max="100"
                       low="{{.memLow}}" high="{{.memHigh}}" optimum="0"
                       style="width: 100%; height: 20pt; border: 1pt solid #ccc;">
                    {{.mem}}%
                </meter>
                <p style="margin: 4pt 0 0 0; font-size: 9pt; color: #666;">
                    Thresholds: Low={{.memLow}}%, High={{.memHigh}}%
                </p>
            </div>
        </div>
    </template>

    <div style="margin-top: 20pt; padding: 15pt; background-color: #e3f2fd;
                border-radius: 6pt; font-size: 10pt;">
        <strong>Note:</strong> Each server has custom memory thresholds based on
        its role and capacity. CPU thresholds are standardized across the fleet.
    </div>
</div>
```

### Pressure Gauge with Safety Thresholds

```html
<div style="border: 3pt solid #FF5722; border-radius: 12pt; padding: 25pt;
            background-color: #1a1a1a; color: white;">
    <h2 style="margin: 0 0 25pt 0; color: #FF5722; text-align: center;">
        Industrial Pressure Monitoring
    </h2>

    <div style="margin-bottom: 20pt; padding: 20pt; background-color: #2a2a2a;
                border-radius: 8pt; border: 2pt solid #4CAF50;">
        <div style="margin-bottom: 12pt; text-align: center;">
            <strong style="font-size: 14pt; color: #4CAF50;">Boiler #1</strong>
        </div>
        <meter value="85" min="0" max="150" low="50" high="120" optimum="80"
               style="width: 100%; height: 36pt; border: 2pt solid rgba(255,255,255,0.3);">
            85 PSI
        </meter>
        <div style="margin-top: 15pt; text-align: center;">
            <span style="font-size: 32pt; font-weight: bold; color: #4CAF50;">
                85 PSI
            </span>
            <br/>
            <span style="font-size: 11pt; color: #81C784;">
                Operating Range - Between low (50) and high (120) thresholds
            </span>
        </div>
    </div>

    <div style="margin-bottom: 20pt; padding: 20pt; background-color: #2a2a2a;
                border-radius: 8pt; border: 2pt solid #FF9800;">
        <div style="margin-bottom: 12pt; text-align: center;">
            <strong style="font-size: 14pt; color: #FF9800;">Boiler #2</strong>
        </div>
        <meter value="125" min="0" max="150" low="50" high="120" optimum="80"
               style="width: 100%; height: 36pt; border: 2pt solid rgba(255,255,255,0.3);">
            125 PSI
        </meter>
        <div style="margin-top: 15pt; text-align: center;">
            <span style="font-size: 32pt; font-weight: bold; color: #FF9800;">
                125 PSI
            </span>
            <br/>
            <span style="font-size: 11pt; color: #FFB74D;">
                Warning - Above high threshold (120 PSI)
            </span>
        </div>
    </div>

    <div style="margin-bottom: 0; padding: 20pt; background-color: #2a2a2a;
                border-radius: 8pt; border: 2pt solid #d32f2f;">
        <div style="margin-bottom: 12pt; text-align: center;">
            <strong style="font-size: 14pt; color: #d32f2f;">Boiler #3</strong>
        </div>
        <meter value="142" min="0" max="150" low="50" high="120" optimum="80"
               style="width: 100%; height: 36pt; border: 2pt solid rgba(255,255,255,0.3);">
            142 PSI
        </meter>
        <div style="margin-top: 15pt; text-align: center;">
            <span style="font-size: 32pt; font-weight: bold; color: #d32f2f;">
                142 PSI
            </span>
            <br/>
            <span style="font-size: 11pt; color: #EF5350;">
                CRITICAL - Dangerously high! Immediate action required!
            </span>
        </div>
    </div>

    <div style="margin-top: 25pt; padding: 20pt; background-color: rgba(211,47,47,0.2);
                border: 2pt solid #d32f2f; border-radius: 8pt; font-size: 11pt;">
        <strong style="color: #FF5722;">Safety Thresholds:</strong><br/>
        <table style="width: 100%; margin-top: 12pt; font-size: 10pt;">
            <tr>
                <td style="padding: 6pt; color: #4CAF50;">0-50 PSI:</td>
                <td style="padding: 6pt;">Below operating range (startup)</td>
            </tr>
            <tr>
                <td style="padding: 6pt; color: #4CAF50;">50-120 PSI:</td>
                <td style="padding: 6pt;">Normal operating range (safe)</td>
            </tr>
            <tr>
                <td style="padding: 6pt; color: #FF9800;">120-140 PSI:</td>
                <td style="padding: 6pt;">Warning zone (reduce pressure)</td>
            </tr>
            <tr>
                <td style="padding: 6pt; color: #d32f2f;">140-150 PSI:</td>
                <td style="padding: 6pt;">Critical zone (emergency shutdown)</td>
            </tr>
        </table>
        <div style="margin-top: 15pt; padding: 12pt; background-color: rgba(255,255,255,0.1);
                    border-radius: 4pt;">
            <strong>Current Configuration:</strong><br/>
            Low Threshold = 50 PSI | High Threshold = 120 PSI | Optimal = 80 PSI
        </div>
    </div>
</div>
```

---

## See Also

- [meter](/reference/htmltags/meter.html) - Meter/gauge element
- [value](/reference/htmlattributes/value.html) - Value attribute
- [min and max](/reference/htmlattributes/min_max.html) - Range attributes
- [optimum](/reference/htmlattributes/optimum.html) - Optimal value attribute
- [progress](/reference/htmltags/progress.html) - Progress bar element (no thresholds)
- [Data Binding](/reference/binding/) - Dynamic data binding and expressions
- [CSS Styles](/reference/styles/) - Complete CSS styling reference

---
