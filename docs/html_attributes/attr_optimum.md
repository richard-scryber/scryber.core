---
layout: default
title: optimum
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @optimum : The Optimal Value Attribute

The `optimum` attribute specifies the ideal target value for the meter element in PDF documents. It works with `low` and `high` threshold attributes to determine which range is considered optimal, directly controlling the color-coding logic that visually indicates whether the current value is in an optimal, suboptimal, or critically suboptimal state.

## Usage

The `optimum` attribute controls semantic coloring logic:
- Defines the ideal target value for measurements
- Determines which range (below low, between low/high, above high) is optimal
- Controls automatic color selection (green for optimal, orange/red for suboptimal)
- Must fall within the min/max range
- Works with low/high thresholds to create three-zone color logic
- Supports decimal and integer values
- Enables data binding for dynamic optimal targets
- Default: NaN (not set - middle range assumed optimal)

```html
<!-- Lower is better: optimum near min -->
<meter value="25" min="0" max="100" low="30" high="75" optimum="0">
    25% - Green (near optimum, below low threshold)
</meter>

<!-- Higher is better: optimum near max -->
<meter value="85" min="0" max="100" low="20" high="50" optimum="100">
    85% - Green (near optimum, above high threshold)
</meter>

<!-- Middle is better: optimum in middle range -->
<meter value="72" min="60" max="90" low="68" high="78" optimum="72">
    72°F - Green (at optimum, in middle range)
</meter>

<!-- Dynamic optimum with data binding -->
<meter value="{{model.currentValue}}"
       min="{{model.min}}"
       max="{{model.max}}"
       low="{{model.low}}"
       high="{{model.high}}"
       optimum="{{model.targetValue}}">
    {{model.currentValue}}
</meter>
```

---

## Supported Elements

The `optimum` attribute is supported by:

| Element | Description |
|---------|-------------|
| `<meter>` | Gauge/measurement element - uses optimum to determine color logic |

**Note:** The `<progress>` element does not support the optimum attribute as it represents simple completion without optimal value concepts.

---

## How Optimum Affects Color Logic

### Three Semantic Patterns

The optimum value position relative to low/high thresholds creates three distinct patterns:

#### Pattern 1: Lower is Better (optimum < low)

When optimum is below the low threshold, lower values are optimal:

```
min [optimum]----[low]--------[high]---- max
    OPTIMAL     SUBOPTIMAL   CRITICAL
     Green       Orange     Dark Orange
```

**Use cases:** CPU usage, memory usage, disk usage, costs, errors

```html
<!-- Disk usage: Lower is better -->
<meter value="25" min="0" max="100" low="30" high="75" optimum="0">
    25% - Green (near optimum, optimal range)
</meter>

<meter value="50" min="0" max="100" low="30" high="75" optimum="0">
    50% - Orange (middle range, suboptimal)
</meter>

<meter value="85" min="0" max="100" low="30" high="75" optimum="0">
    85% - Dark Orange (above high, critically suboptimal)
</meter>
```

#### Pattern 2: Higher is Better (optimum > high)

When optimum is above the high threshold, higher values are optimal:

```
min ----[low]--------[high]---- [optimum] max
      CRITICAL    SUBOPTIMAL    OPTIMAL
    Dark Orange    Orange        Green
```

**Use cases:** Battery level, quality scores, performance ratings, revenue

```html
<!-- Battery level: Higher is better -->
<meter value="85" min="0" max="100" low="20" high="50" optimum="100">
    85% - Green (near optimum, optimal range)
</meter>

<meter value="35" min="0" max="100" low="20" high="50" optimum="100">
    35% - Orange (middle range, suboptimal)
</meter>

<meter value="15" min="0" max="100" low="20" high="50" optimum="100">
    15% - Dark Orange (below low, critically suboptimal)
</meter>
```

#### Pattern 3: Middle is Better (low ≤ optimum ≤ high)

When optimum is in the middle range, moderate values are optimal:

```
min ----[low]-- [optimum] --[high]---- max
     SUBOPTIMAL   OPTIMAL    SUBOPTIMAL
      Orange      Green       Orange
```

**Use cases:** Temperature, pH levels, pressure, humidity

```html
<!-- Temperature: Middle range is optimal -->
<meter value="72" min="60" max="90" low="68" high="78" optimum="72">
    72°F - Green (at optimum, optimal range)
</meter>

<meter value="82" min="60" max="90" low="68" high="78" optimum="72">
    82°F - Orange (above high, too hot)
</meter>

<meter value="64" min="60" max="90" low="68" high="78" optimum="72">
    64°F - Orange (below low, too cold)
</meter>
```

### Color Decision Tree

The meter evaluates color using this logic:

1. **Determine optimal zone** based on optimum position:
   - If optimum < low: Low range is optimal
   - If optimum > high: High range is optimal
   - If low ≤ optimum ≤ high: Middle range is optimal

2. **Evaluate current value** against optimal zone:
   - Value in optimal zone: **Green**
   - Value in adjacent zone: **Orange** (suboptimal)
   - Value in far zone: **Dark Orange/Red** (critically suboptimal)

### Practical Examples by Use Case

**Resource Usage (Lower is Better):**
```html
<!-- CPU: 0% is ideal, higher is worse -->
<meter value="45" min="0" max="100" low="30" high="75" optimum="0">
    45% CPU - Orange (suboptimal)
</meter>

<!-- Memory: 0 GB used is ideal -->
<meter value="2.5" min="0" max="16" low="4" high="12" optimum="0">
    2.5 GB - Green (optimal, below low threshold)
</meter>

<!-- Error rate: 0 errors is ideal -->
<meter value="5" min="0" max="100" low="10" high="50" optimum="0">
    5 errors - Green (optimal, below low threshold)
</meter>
```

**Performance/Quality (Higher is Better):**
```html
<!-- Score: 100% is ideal -->
<meter value="92" min="0" max="100" low="60" high="85" optimum="100">
    92% - Green (optimal, above high threshold)
</meter>

<!-- Battery: 100% charge is ideal -->
<meter value="75" min="0" max="100" low="20" high="50" optimum="100">
    75% - Green (optimal, above high threshold)
</meter>

<!-- Customer satisfaction: 5 stars is ideal -->
<meter value="4.7" min="1" max="5" low="3" high="4" optimum="5">
    4.7 stars - Green (optimal, above high threshold)
</meter>
```

**Balanced Range (Middle is Better):**
```html
<!-- Room temperature: 72°F is ideal -->
<meter value="71" min="60" max="85" low="68" high="76" optimum="72">
    71°F - Green (optimal, near target)
</meter>

<!-- pH level: 7.0 is ideal (neutral) -->
<meter value="7.2" min="0" max="14" low="6.5" high="8.5" optimum="7">
    pH 7.2 - Green (optimal, near neutral)
</meter>

<!-- Blood pressure: 120 mmHg is ideal -->
<meter value="118" min="90" max="180" low="110" high="140" optimum="120">
    118 mmHg - Green (optimal, near ideal)
</meter>
```

---

## Binding Values

The `optimum` attribute supports data binding for dynamic target values:

### Simple Optimum Binding

```html
<!-- Model: { currentValue: 75, targetValue: 80 } -->
<meter value="{{model.currentValue}}"
       min="0"
       max="100"
       low="60"
       high="90"
       optimum="{{model.targetValue}}">
    {{model.currentValue}}% (Target: {{model.targetValue}}%)
</meter>
```

### Calculated Optimum

```html
<!-- Model: { maxCapacity: 100, targetUtilization: 0.8 } -->
<!-- Optimal value is 80% of max capacity -->
<meter value="{{model.currentLoad}}"
       min="0"
       max="{{model.maxCapacity}}"
       low="{{model.maxCapacity * 0.5}}"
       high="{{model.maxCapacity * 0.9}}"
       optimum="{{model.maxCapacity * model.targetUtilization}}">
    {{model.currentLoad}} (Target: {{model.maxCapacity * model.targetUtilization}})
</meter>
```

### Conditional Optimum

```html
<!-- Model: { metric: "cpu", value: 45 } -->
<!-- Different optimum for different metric types -->
<meter value="{{model.value}}"
       min="0"
       max="100"
       low="30"
       high="75"
       optimum="{{model.metric === 'cpu' ? 0 : (model.metric === 'score' ? 100 : 50)}}">
    {{model.value}}% {{model.metric}}
</meter>
```

### Context-Specific Optimum

```html
<!-- Model: { season: "summer", temp: 75, winterTarget: 68, summerTarget: 72 } -->
<!-- Different optimal temperatures for different seasons -->
<meter value="{{model.temp}}"
       min="60"
       max="85"
       low="{{model.season === 'winter' ? 65 : 70}}"
       high="{{model.season === 'winter' ? 72 : 76}}"
       optimum="{{model.season === 'winter' ? model.winterTarget : model.summerTarget}}">
    {{model.temp}}°F ({{model.season}})
</meter>
```

### Dynamic Targets from Collection

```html
<!-- Model: {
    goals: [
        {name: "Sales", current: 875000, target: 1000000},
        {name: "Customers", current: 156, target: 200},
        {name: "Satisfaction", current: 4.6, target: 4.5}
    ]
} -->
<template data-bind="{{model.goals}}">
    <div style="margin-bottom: 15pt;">
        <strong>{{.name}}:</strong>
        <meter value="{{.current}}" min="0" max="{{.target * 1.2}}"
               low="{{.target * 0.5}}" high="{{.target * 0.9}}"
               optimum="{{.target}}"
               style="width: 250pt; height: 20pt;">
            {{.current}}
        </meter>
        <span style="margin-left: 10pt;">
            Target: {{.target}}
        </span>
    </div>
</template>
```

---

## Notes

### Optimum Positioning Requirements

The optimum value must be within the min/max range:

```html
<!-- Valid: optimum within range -->
<meter value="50" min="0" max="100" low="30" high="70" optimum="50">
    Valid configuration
</meter>

<!-- Invalid: optimum below min -->
<meter value="50" min="10" max="100" low="30" high="70" optimum="5">
    Invalid: optimum < min
</meter>

<!-- Invalid: optimum above max -->
<meter value="50" min="0" max="100" low="30" high="70" optimum="120">
    Invalid: optimum > max
</meter>

<!-- Valid: optimum at boundary -->
<meter value="50" min="0" max="100" low="30" high="70" optimum="0">
    Valid: optimum = min (lower is better pattern)
</meter>

<meter value="50" min="0" max="100" low="30" high="70" optimum="100">
    Valid: optimum = max (higher is better pattern)
</meter>
```

### Default Behavior Without Optimum

If optimum is not specified, the middle range is assumed optimal:

```html
<!-- No optimum: middle range defaults as optimal -->
<meter value="50" min="0" max="100" low="30" high="70">
    Assumes middle range (30-70) is optimal
</meter>

<!-- Equivalent to setting optimum in middle -->
<meter value="50" min="0" max="100" low="30" high="70" optimum="50">
    Explicit middle optimum
</meter>
```

### Optimum Precision

The optimum attribute supports decimal values:

```html
<!-- Decimal optimum -->
<meter value="0.67" min="0" max="1" low="0.3" high="0.8" optimum="0.5">
    0.67 (Target: 0.5)
</meter>

<!-- Scientific measurement -->
<meter value="7.2" min="0" max="14" low="6.5" high="8.5" optimum="7.0">
    pH 7.2 (Target: 7.0 neutral)
</meter>

<!-- Precise targeting -->
<meter value="98.6" min="95" max="105" low="97" high="100" optimum="98.6">
    98.6°F (Normal body temperature)
</meter>
```

### Optimum vs. Value

The optimum is the *target*, not the current value:

```html
<!-- Current value: 75, Target (optimum): 50 -->
<meter value="75" min="0" max="100" low="30" high="70" optimum="50">
    Current: 75, Target: 50 (above optimal range)
</meter>

<!-- Don't set optimum to current value -->
<!-- This would make the color always green -->
<meter value="75" min="0" max="100" low="30" high="70" optimum="75">
    Incorrect: optimum should be target, not current
</meter>
```

### Multiple Meters, Same Optimum

Consistent optimum values for related measurements:

```html
<!-- All servers should target 0% (minimal usage) -->
<meter value="45" min="0" max="100" low="30" high="75" optimum="0">
    Server 1: 45%
</meter>

<meter value="62" min="0" max="100" low="30" high="75" optimum="0">
    Server 2: 62%
</meter>

<meter value="88" min="0" max="100" low="30" high="75" optimum="0">
    Server 3: 88% (Critical!)
</meter>
```

### Optimum in Different Scenarios

**Fixed Optimum:**
```html
<!-- Room temperature: always 72°F -->
<meter value="{{model.currentTemp}}" min="60" max="85"
       low="68" high="76" optimum="72">
    {{model.currentTemp}}°F
</meter>
```

**Variable Optimum:**
```html
<!-- Target changes based on workout intensity -->
<meter value="{{model.heartRate}}" min="60" max="200"
       low="{{model.restingHR}}"
       high="{{model.maxHR * 0.85}}"
       optimum="{{model.targetZone}}">
    {{model.heartRate}} bpm
</meter>
```

**Context-Dependent Optimum:**
```html
<!-- Humidity: different optimal based on season -->
<meter value="{{model.humidity}}" min="0" max="100"
       low="30" high="60"
       optimum="{{model.season === 'winter' ? 40 : 45}}">
    {{model.humidity}}% humidity
</meter>
```

### Visual Impact of Optimum Position

Same thresholds, different optimum, different colors:

```html
<!-- Value: 85, optimum at min (0) -->
<meter value="85" min="0" max="100" low="30" high="75" optimum="0"
       style="width: 250pt; height: 24pt;">
    85% - Dark Orange (value far from optimum)
</meter>

<!-- Value: 85, optimum at max (100) -->
<meter value="85" min="0" max="100" low="30" high="75" optimum="100"
       style="width: 250pt; height: 24pt;">
    85% - Green (value near optimum)
</meter>

<!-- Value: 85, optimum in middle (50) -->
<meter value="85" min="0" max="100" low="30" high="75" optimum="50"
       style="width: 250pt; height: 24pt;">
    85% - Orange (value above optimal range)
</meter>
```

### Threshold Symmetry Around Optimum

For middle-is-better patterns, center the optimum:

```html
<!-- Symmetric: optimum centered in middle range -->
<meter value="50" min="0" max="100" low="40" high="60" optimum="50">
    Perfect centering: 40-[50]-60
</meter>

<!-- Asymmetric: optimum offset -->
<meter value="45" min="0" max="100" low="40" high="60" optimum="45">
    Offset optimum: closer to low threshold
</meter>
```

### Common Patterns

**Minimize pattern (optimum = min):**
```html
<!-- Errors, usage, costs - less is better -->
<meter value="25" min="0" max="100" low="30" high="70" optimum="0">
    Minimize pattern
</meter>
```

**Maximize pattern (optimum = max):**
```html
<!-- Scores, battery, quality - more is better -->
<meter value="85" min="0" max="100" low="30" high="70" optimum="100">
    Maximize pattern
</meter>
```

**Target pattern (optimum = specific value):**
```html
<!-- Temperature, levels, balance - specific target -->
<meter value="72" min="60" max="85" low="68" high="76" optimum="72">
    Target pattern
</meter>
```

### Optimum and User Expectations

Align optimum with user intuition:

**Good alignment:**
```html
<!-- Users expect low disk usage to be good -->
<meter value="25" min="0" max="100" low="30" high="75" optimum="0">
    25% disk used - Green (matches expectation)
</meter>

<!-- Users expect high battery to be good -->
<meter value="85" min="0" max="100" low="20" high="50" optimum="100">
    85% battery - Green (matches expectation)
</meter>
```

**Poor alignment (confusing):**
```html
<!-- Confusing: High disk usage shown as good -->
<meter value="85" min="0" max="100" low="30" high="75" optimum="100">
    85% disk used - Green (misleading!)
</meter>
```

### Testing Optimum Configuration

Test each zone to verify color logic:

```html
<!-- Test low zone (should be green if optimum=0) -->
<meter value="15" min="0" max="100" low="30" high="70" optimum="0">
    15% - Expected: Green
</meter>

<!-- Test middle zone (should be orange if optimum=0) -->
<meter value="50" min="0" max="100" low="30" high="70" optimum="0">
    50% - Expected: Orange
</meter>

<!-- Test high zone (should be dark orange if optimum=0) -->
<meter value="85" min="0" max="100" low="30" high="70" optimum="0">
    85% - Expected: Dark Orange
</meter>
```

---

## Examples

### CPU Usage Monitor (Lower is Better)

```html
<div style="padding: 25pt; background-color: #263238; color: white; border-radius: 10pt;">
    <h2 style="margin: 0 0 25pt 0; text-align: center;">CPU Usage Monitor</h2>

    <div style="margin-bottom: 20pt; padding: 18pt; background-color: #37474f;
                border-radius: 8pt;">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 12pt; color: #4CAF50;">Core 1</strong>
            <span style="float: right; font-size: 18pt; font-weight: bold; color: #4CAF50;">
                22%
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="22" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 32pt; border: 2pt solid rgba(255,255,255,0.3);">
            22%
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; opacity: 0.9; color: #4CAF50;">
            Optimal - Low usage (below 30% threshold). Target: 0% (idle)
        </p>
    </div>

    <div style="margin-bottom: 20pt; padding: 18pt; background-color: #37474f;
                border-radius: 8pt;">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 12pt; color: #FF9800;">Core 2</strong>
            <span style="float: right; font-size: 18pt; font-weight: bold; color: #FF9800;">
                52%
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="52" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 32pt; border: 2pt solid rgba(255,255,255,0.3);">
            52%
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; opacity: 0.9; color: #FF9800;">
            Suboptimal - Moderate usage (30-75% range). Target: 0% (idle)
        </p>
    </div>

    <div style="margin-bottom: 0; padding: 18pt; background-color: #37474f;
                border-radius: 8pt;">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 12pt; color: #FF5722;">Core 3</strong>
            <span style="float: right; font-size: 18pt; font-weight: bold; color: #FF5722;">
                88%
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="88" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 32pt; border: 2pt solid rgba(255,255,255,0.3);">
            88%
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; opacity: 0.9; color: #FF5722;">
            Critical - High usage (above 75% threshold). Target: 0% (idle)
        </p>
    </div>

    <div style="margin-top: 25pt; padding: 18pt; background-color: rgba(255,255,255,0.1);
                border-radius: 8pt; font-size: 10pt;">
        <strong>Configuration:</strong><br/>
        Optimum: 0% (idle state is ideal)<br/>
        Low Threshold: 30% | High Threshold: 75%<br/>
        <br/>
        <strong>Logic:</strong> Lower CPU usage is better. Optimum at minimum (0%)
        means green appears below 30%, orange between 30-75%, and red above 75%.
    </div>
</div>
```

### Battery Level Indicator (Higher is Better)

```html
<div style="border: 3pt solid #4CAF50; border-radius: 12pt; padding: 25pt;">
    <h2 style="margin: 0 0 25pt 0; color: #2e7d32; text-align: center;">
        Device Battery Status
    </h2>

    <div style="margin-bottom: 22pt; padding: 18pt; background-color: #e8f5e9;
                border-radius: 8pt; border-left: 5pt solid #4CAF50;">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 13pt;">Laptop</strong>
            <span style="float: right; font-size: 22pt; font-weight: bold; color: #4CAF50;">
                92%
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="92" min="0" max="100" low="20" high="50" optimum="100"
               style="width: 100%; height: 30pt; border: 2pt solid #4CAF50;">
            92%
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #2e7d32;">
            Excellent - High charge (above 50% threshold). Target: 100% (full charge)
        </p>
    </div>

    <div style="margin-bottom: 22pt; padding: 18pt; background-color: #fff3e0;
                border-radius: 8pt; border-left: 5pt solid #FF9800;">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 13pt;">Tablet</strong>
            <span style="float: right; font-size: 22pt; font-weight: bold; color: #FF9800;">
                38%
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="38" min="0" max="100" low="20" high="50" optimum="100"
               style="width: 100%; height: 30pt; border: 2pt solid #FF9800;">
            38%
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #e65100;">
            Moderate - Medium charge (20-50% range). Target: 100% (full charge)
        </p>
    </div>

    <div style="margin-bottom: 0; padding: 18pt; background-color: #ffebee;
                border-radius: 8pt; border-left: 5pt solid #d32f2f;">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 13pt;">Phone</strong>
            <span style="float: right; font-size: 22pt; font-weight: bold; color: #d32f2f;">
                12%
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="12" min="0" max="100" low="20" high="50" optimum="100"
               style="width: 100%; height: 30pt; border: 2pt solid #d32f2f;">
            12%
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #c62828;">
            Critical - Low battery (below 20% threshold). Target: 100% (full charge)
            <br/>
            <strong>⚠ Charge immediately!</strong>
        </p>
    </div>

    <div style="margin-top: 25pt; padding: 18pt; background-color: #e3f2fd;
                border-radius: 8pt; font-size: 10pt;">
        <strong>Configuration:</strong><br/>
        Optimum: 100% (full charge is ideal)<br/>
        Low Threshold: 20% | High Threshold: 50%<br/>
        <br/>
        <strong>Logic:</strong> Higher battery percentage is better. Optimum at
        maximum (100%) means green appears above 50%, orange between 20-50%,
        and red below 20%.
    </div>
</div>
```

### Room Temperature Control (Middle is Better)

```html
<div style="padding: 25pt; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white; border-radius: 12pt;">
    <h2 style="margin: 0 0 30pt 0; text-align: center; font-size: 20pt;">
        Smart Thermostat Control
    </h2>

    <div style="background-color: rgba(255,255,255,0.15); padding: 20pt;
                border-radius: 10pt; margin-bottom: 20pt;">
        <h3 style="margin: 0 0 15pt 0; color: #4CAF50;">Living Room</h3>
        <meter value="72" min="60" max="85" low="68" high="76" optimum="72"
               style="width: 100%; height: 36pt; border: 3pt solid rgba(255,255,255,0.3);">
            72°F
        </meter>
        <div style="margin-top: 15pt; text-align: center;">
            <span style="font-size: 36pt; font-weight: bold; color: #4CAF50;">
                72°F
            </span>
            <br/>
            <span style="font-size: 12pt; opacity: 0.95;">
                Perfect - At optimal temperature (target: 72°F)
            </span>
        </div>
    </div>

    <div style="background-color: rgba(255,255,255,0.15); padding: 20pt;
                border-radius: 10pt; margin-bottom: 20pt;">
        <h3 style="margin: 0 0 15pt 0; color: #FF9800;">Bedroom</h3>
        <meter value="65" min="60" max="85" low="68" high="76" optimum="72"
               style="width: 100%; height: 36pt; border: 3pt solid rgba(255,255,255,0.3);">
            65°F
        </meter>
        <div style="margin-top: 15pt; text-align: center;">
            <span style="font-size: 36pt; font-weight: bold; color: #FF9800;">
                65°F
            </span>
            <br/>
            <span style="font-size: 12pt; opacity: 0.95;">
                Too Cold - Below optimal range (target: 72°F)
            </span>
        </div>
    </div>

    <div style="background-color: rgba(255,255,255,0.15); padding: 20pt;
                border-radius: 10pt;">
        <h3 style="margin: 0 0 15pt 0; color: #FF5722;">Kitchen</h3>
        <meter value="80" min="60" max="85" low="68" high="76" optimum="72"
               style="width: 100%; height: 36pt; border: 3pt solid rgba(255,255,255,0.3);">
            80°F
        </meter>
        <div style="margin-top: 15pt; text-align: center;">
            <span style="font-size: 36pt; font-weight: bold; color: #FF5722;">
                80°F
            </span>
            <br/>
            <span style="font-size: 12pt; opacity: 0.95;">
                Too Hot - Above optimal range (target: 72°F)
            </span>
        </div>
    </div>

    <div style="margin-top: 25pt; padding: 20pt; background-color: rgba(255,255,255,0.1);
                border-radius: 10pt; font-size: 11pt;">
        <strong>Configuration:</strong><br/>
        Optimum: 72°F (comfortable room temperature)<br/>
        Low Threshold: 68°F | High Threshold: 76°F<br/>
        <br/>
        <strong>Logic:</strong> Middle temperature is best. Optimum in the middle
        range (68-76°F) means green appears when temperature is near 72°F, orange
        when too cold (&lt;68°F) or too hot (&gt;76°F).
    </div>
</div>
```

### Student Performance Scoring (Higher is Better)

```html
<div style="border: 2pt solid #673AB7; border-radius: 10pt; padding: 25pt;">
    <h2 style="margin: 0 0 25pt 0; color: #673AB7; text-align: center;">
        Student Performance Dashboard
    </h2>

    <div style="margin-bottom: 20pt; padding: 18pt; background-color: #e8f5e9;
                border-radius: 8pt; border: 1pt solid #4CAF50;">
        <div style="margin-bottom: 10pt;">
            <strong style="font-size: 13pt;">Alice Johnson - Mathematics</strong>
            <span style="float: right; font-size: 20pt; font-weight: bold; color: #4CAF50;">
                A (95%)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="95" min="0" max="100" low="60" high="85" optimum="100"
               style="width: 100%; height: 28pt; border: 2pt solid #4CAF50;">
            95%
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #2e7d32;">
            Excellent - Above high threshold (85%). Target: 100% (perfect score)
        </p>
    </div>

    <div style="margin-bottom: 20pt; padding: 18pt; background-color: #e8f5e9;
                border-radius: 8pt; border: 1pt solid #8BC34A;">
        <div style="margin-bottom: 10pt;">
            <strong style="font-size: 13pt;">Bob Smith - Science</strong>
            <span style="float: right; font-size: 20pt; font-weight: bold; color: #8BC34A;">
                B (82%)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="82" min="0" max="100" low="60" high="85" optimum="100"
               style="width: 100%; height: 28pt; border: 2pt solid #8BC34A;">
            82%
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #558b2f;">
            Good - In target range (60-85%). Target: 100% (perfect score)
        </p>
    </div>

    <div style="margin-bottom: 20pt; padding: 18pt; background-color: #fff3e0;
                border-radius: 8pt; border: 1pt solid #FF9800;">
        <div style="margin-bottom: 10pt;">
            <strong style="font-size: 13pt;">Carol Williams - English</strong>
            <span style="float: right; font-size: 20pt; font-weight: bold; color: #FF9800;">
                C (68%)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="68" min="0" max="100" low="60" high="85" optimum="100"
               style="width: 100%; height: 28pt; border: 2pt solid #FF9800;">
            68%
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #e65100;">
            Satisfactory - In passing range but below good threshold (85%).
            Target: 100% (perfect score)
        </p>
    </div>

    <div style="margin-bottom: 0; padding: 18pt; background-color: #ffebee;
                border-radius: 8pt; border: 1pt solid #d32f2f;">
        <div style="margin-bottom: 10pt;">
            <strong style="font-size: 13pt;">David Brown - History</strong>
            <span style="float: right; font-size: 20pt; font-weight: bold; color: #d32f2f;">
                D (55%)
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="55" min="0" max="100" low="60" high="85" optimum="100"
               style="width: 100%; height: 28pt; border: 2pt solid #d32f2f;">
            55%
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #c62828;">
            Needs Improvement - Below passing threshold (60%).
            Target: 100% (perfect score)
            <br/>
            <strong>⚠ Tutoring recommended</strong>
        </p>
    </div>

    <div style="margin-top: 25pt; padding: 18pt; background-color: #e3f2fd;
                border-radius: 8pt; font-size: 10pt;">
        <strong>Grading Configuration:</strong><br/>
        Optimum: 100% (perfect score is ideal)<br/>
        Low Threshold: 60% (passing grade) | High Threshold: 85% (honor roll)<br/>
        <br/>
        <strong>Logic:</strong> Higher scores are better. Optimum at maximum (100%)
        means green appears above 85%, orange between 60-85%, and red below 60%.
    </div>
</div>
```

### Storage Capacity Management (Lower is Better)

```html
<div style="padding: 25pt; background-color: #f5f5f5; border-radius: 10pt;">
    <h2 style="margin: 0 0 25pt 0; color: #333; text-align: center;">
        Storage Capacity Management
    </h2>

    <div style="margin-bottom: 20pt; padding: 20pt; background-color: white;
                border-radius: 8pt; box-shadow: 0 2pt 8pt rgba(0,0,0,0.1);">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 13pt; color: #4CAF50;">SSD System Drive (C:)</strong>
            <span style="float: right;">
                <span style="font-size: 20pt; font-weight: bold; color: #4CAF50;">120 GB</span>
                <span style="font-size: 11pt; color: #666;"> / 500 GB</span>
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="24" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 30pt; border: 2pt solid #4CAF50;">
            24% used
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #4CAF50;">
            Excellent - Low usage (24%, below 30% threshold). Target: 0% (minimal usage)
            <br/>
            380 GB free space available
        </p>
    </div>

    <div style="margin-bottom: 20pt; padding: 20pt; background-color: white;
                border-radius: 8pt; box-shadow: 0 2pt 8pt rgba(0,0,0,0.1);">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 13pt; color: #FF9800;">HDD Data Drive (D:)</strong>
            <span style="float: right;">
                <span style="font-size: 20pt; font-weight: bold; color: #FF9800;">550 GB</span>
                <span style="font-size: 11pt; color: #666;"> / 1000 GB</span>
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="55" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 30pt; border: 2pt solid #FF9800;">
            55% used
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #FF9800;">
            Moderate - Medium usage (55%, in 30-75% range). Target: 0% (minimal usage)
            <br/>
            450 GB free space remaining - Monitor usage
        </p>
    </div>

    <div style="margin-bottom: 0; padding: 20pt; background-color: white;
                border-radius: 8pt; box-shadow: 0 2pt 8pt rgba(0,0,0,0.1);">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 13pt; color: #d32f2f;">Archive Drive (E:)</strong>
            <span style="float: right;">
                <span style="font-size: 20pt; font-weight: bold; color: #d32f2f;">1850 GB</span>
                <span style="font-size: 11pt; color: #666;"> / 2000 GB</span>
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="92.5" min="0" max="100" low="30" high="75" optimum="0"
               style="width: 100%; height: 30pt; border: 2pt solid #d32f2f;">
            92.5% used
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #d32f2f;">
            Critical - High usage (92.5%, well above 75% threshold). Target: 0% (minimal usage)
            <br/>
            <strong>⚠ Only 150 GB free - Cleanup required immediately!</strong>
        </p>
    </div>

    <div style="margin-top: 25pt; padding: 20pt; background-color: #e3f2fd;
                border-radius: 8pt; font-size: 10pt;">
        <strong>Storage Policy Configuration:</strong><br/>
        Optimum: 0% (minimal storage usage is ideal for maximum available space)<br/>
        Low Threshold: 30% (comfortable usage) | High Threshold: 75% (high usage warning)<br/>
        <br/>
        <strong>Logic:</strong> Lower storage usage is better. Optimum at minimum (0%)
        means green appears below 30% used, orange between 30-75% used, and red
        above 75% used (critical - cleanup needed).
    </div>
</div>
```

### pH Balance Monitoring (Middle is Better)

```html
<div style="padding: 25pt; background-color: #eceff1; border-radius: 10pt;">
    <h2 style="margin: 0 0 25pt 0; color: #37474f; text-align: center;">
        Water Quality pH Monitoring
    </h2>

    <div style="margin-bottom: 20pt; padding: 20pt; background-color: white;
                border-radius: 8pt; border-left: 5pt solid #4CAF50;">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 13pt; color: #2e7d32;">Drinking Water Supply</strong>
            <span style="float: right; font-size: 20pt; font-weight: bold; color: #4CAF50;">
                pH 7.1
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="7.1" min="0" max="14" low="6.5" high="8.5" optimum="7.0"
               style="width: 100%; height: 30pt; border: 2pt solid #607d8b;">
            pH 7.1
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #2e7d32;">
            Optimal - Near neutral pH (7.1, close to target 7.0). Safe for consumption.
        </p>
    </div>

    <div style="margin-bottom: 20pt; padding: 20pt; background-color: white;
                border-radius: 8pt; border-left: 5pt solid #4CAF50;">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 13pt; color: #2e7d32;">Swimming Pool</strong>
            <span style="float: right; font-size: 20pt; font-weight: bold; color: #4CAF50;">
                pH 7.5
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="7.5" min="0" max="14" low="7.2" high="7.8" optimum="7.5"
               style="width: 100%; height: 30pt; border: 2pt solid #607d8b;">
            pH 7.5
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #2e7d32;">
            Perfect - Exactly at optimal pH (7.5). Ideal for swimming.
        </p>
    </div>

    <div style="margin-bottom: 20pt; padding: 20pt; background-color: white;
                border-radius: 8pt; border-left: 5pt solid #FF9800;">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 13pt; color: #e65100;">Aquarium (Freshwater)</strong>
            <span style="float: right; font-size: 20pt; font-weight: bold; color: #FF9800;">
                pH 8.8
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="8.8" min="0" max="14" low="6.5" high="7.5" optimum="7.0"
               style="width: 100%; height: 30pt; border: 2pt solid #607d8b;">
            pH 8.8
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #e65100;">
            Too Alkaline - Above optimal range (8.8, target 7.0). Adjust water chemistry.
        </p>
    </div>

    <div style="margin-bottom: 0; padding: 20pt; background-color: white;
                border-radius: 8pt; border-left: 5pt solid #d32f2f;">
        <div style="margin-bottom: 12pt;">
            <strong style="font-size: 13pt; color: #c62828;">Industrial Wastewater</strong>
            <span style="float: right; font-size: 20pt; font-weight: bold; color: #d32f2f;">
                pH 3.8
            </span>
            <div style="clear: both;"></div>
        </div>
        <meter value="3.8" min="0" max="14" low="6.5" high="8.5" optimum="7.0"
               style="width: 100%; height: 30pt; border: 2pt solid #607d8b;">
            pH 3.8
        </meter>
        <p style="margin: 10pt 0 0 0; font-size: 10pt; color: #c62828;">
            Critically Acidic - Far below optimal range (3.8, target 7.0).
            <br/>
            <strong>⚠ Must neutralize before discharge - Environmental hazard!</strong>
        </p>
    </div>

    <div style="margin-top: 25pt; padding: 20pt; background-color: #bbdefb;
                border-radius: 8pt; font-size: 10pt;">
        <strong>pH Scale Configuration:</strong><br/>
        <table style="width: 100%; margin-top: 12pt; font-size: 9pt;">
            <tr>
                <td style="padding: 6pt;">pH 0-6.5:</td>
                <td style="padding: 6pt;">Acidic (corrosive)</td>
            </tr>
            <tr>
                <td style="padding: 6pt;">pH 6.5-8.5:</td>
                <td style="padding: 6pt;">Neutral Range (safe)</td>
            </tr>
            <tr>
                <td style="padding: 6pt;">pH 8.5-14:</td>
                <td style="padding: 6pt;">Alkaline (caustic)</td>
            </tr>
        </table>
        <div style="margin-top: 12pt;">
            <strong>General Configuration:</strong><br/>
            Optimum: 7.0 (neutral pH is ideal for most applications)<br/>
            Low Threshold: 6.5 | High Threshold: 8.5<br/>
            <br/>
            <strong>Logic:</strong> Middle pH is best. Optimum at neutral (7.0) means
            green appears in neutral range (6.5-8.5), orange when too acidic (&lt;6.5)
            or too alkaline (&gt;8.5).
        </div>
    </div>
</div>
```

### Sales Performance Target (Higher is Better)

```html
<div style="padding: 30pt; background: linear-gradient(135deg, #11998e 0%, #38ef7d 100%);
            color: white; border-radius: 12pt;">
    <h1 style="margin: 0 0 30pt 0; text-align: center; font-size: 24pt;">
        Q4 2024 Sales Performance
    </h1>

    <div style="background-color: rgba(255,255,255,0.2); padding: 25pt;
                border-radius: 10pt; margin-bottom: 25pt;">
        <h3 style="margin: 0 0 15pt 0; font-size: 16pt;">Revenue Goal</h3>
        <meter value="875000" min="0" max="1000000" low="400000" high="800000" optimum="1000000"
               style="width: 100%; height: 45pt; border: 3pt solid rgba(255,255,255,0.4);">
            $875K
        </meter>
        <div style="margin-top: 18pt; text-align: center;">
            <span style="font-size: 42pt; font-weight: bold;">
                $875,000
            </span>
            <span style="font-size: 18pt; opacity: 0.9;"> / $1,000,000</span>
            <br/>
            <span style="font-size: 14pt; opacity: 0.95; margin-top: 8pt; display: block;">
                87.5% of target achieved - Above high threshold (80%)
            </span>
        </div>
        <div style="margin-top: 15pt; padding: 15pt; background-color: rgba(76,175,80,0.3);
                    border-radius: 8pt; font-size: 11pt;">
            <strong>Status:</strong> Excellent performance! On track to exceed goal.<br/>
            Only $125,000 remaining to reach target of $1M.
        </div>
    </div>

    <div style="background-color: rgba(255,255,255,0.2); padding: 25pt;
                border-radius: 10pt; margin-bottom: 25pt;">
        <h3 style="margin: 0 0 15pt 0; font-size: 16pt;">New Customer Acquisition</h3>
        <meter value="156" min="0" max="200" low="80" high="150" optimum="200"
               style="width: 100%; height: 45pt; border: 3pt solid rgba(255,255,255,0.4);">
            156 customers
        </meter>
        <div style="margin-top: 18pt; text-align: center;">
            <span style="font-size: 42pt; font-weight: bold;">
                156
            </span>
            <span style="font-size: 18pt; opacity: 0.9;"> / 200</span>
            <br/>
            <span style="font-size: 14pt; opacity: 0.95; margin-top: 8pt; display: block;">
                78% of target achieved - Above high threshold (75%)
            </span>
        </div>
        <div style="margin-top: 15pt; padding: 15pt; background-color: rgba(76,175,80,0.3);
                    border-radius: 8pt; font-size: 11pt;">
            <strong>Status:</strong> Great progress! 44 more customers needed to reach target.
        </div>
    </div>

    <div style="background-color: rgba(255,255,255,0.2); padding: 25pt;
                border-radius: 10pt;">
        <h3 style="margin: 0 0 15pt 0; font-size: 16pt;">Customer Satisfaction Score</h3>
        <meter value="4.7" min="1" max="5" low="3" high="4.5" optimum="5"
               style="width: 100%; height: 45pt; border: 3pt solid rgba(255,255,255,0.4);">
            4.7/5.0
        </meter>
        <div style="margin-top: 18pt; text-align: center;">
            <span style="font-size: 42pt; font-weight: bold;">
                4.7
            </span>
            <span style="font-size: 18pt; opacity: 0.9;"> / 5.0</span>
            <br/>
            <span style="font-size: 14pt; opacity: 0.95; margin-top: 8pt; display: block;">
                94% satisfaction - Well above high threshold (4.5/5)
            </span>
        </div>
        <div style="margin-top: 15pt; padding: 15pt; background-color: rgba(76,175,80,0.3);
                    border-radius: 8pt; font-size: 11pt;">
            <strong>Status:</strong> Outstanding customer satisfaction!<br/>
            Exceeds excellent threshold (4.5) - maintain quality service.
        </div>
    </div>

    <div style="margin-top: 30pt; padding: 25pt; background-color: rgba(0,0,0,0.2);
                border-radius: 10pt; font-size: 11pt;">
        <strong style="font-size: 13pt;">Performance Target Configuration:</strong><br/>
        <table style="width: 100%; margin-top: 15pt; font-size: 10pt;">
            <tr>
                <td style="padding: 8pt; width: 180pt;"><strong>Revenue:</strong></td>
                <td style="padding: 8pt;">
                    Optimum: $1M (maximum target)<br/>
                    Low: $400K (40%) | High: $800K (80%)
                </td>
            </tr>
            <tr>
                <td style="padding: 8pt;"><strong>New Customers:</strong></td>
                <td style="padding: 8pt;">
                    Optimum: 200 (maximum target)<br/>
                    Low: 80 (40%) | High: 150 (75%)
                </td>
            </tr>
            <tr>
                <td style="padding: 8pt;"><strong>Satisfaction:</strong></td>
                <td style="padding: 8pt;">
                    Optimum: 5.0 (perfect score)<br/>
                    Low: 3.0 (acceptable) | High: 4.5 (excellent)
                </td>
            </tr>
        </table>
        <div style="margin-top: 15pt; padding-top: 15pt;
                    border-top: 1pt solid rgba(255,255,255,0.3);">
            <strong>Logic:</strong> Higher values are better for all metrics. Optimum
            at maximum means green appears above high thresholds, orange in middle
            ranges, and red below low thresholds.
        </div>
    </div>
</div>
```

### Dynamic Optimum Based on Context

```html
<!-- Model: {
    environment: "production",
    cpuUsage: 45,
    devOptimum: 50,
    prodOptimum: 0
} -->

<div style="padding: 20pt; border: 2pt solid #336699; border-radius: 8pt;">
    <h3 style="margin: 0 0 20pt 0; color: #336699;">
        Environment-Specific CPU Monitoring
    </h3>

    <div style="margin-bottom: 15pt; padding: 15pt; background-color: #e3f2fd;
                border-radius: 6pt;">
        <div style="margin-bottom: 8pt;">
            <strong>Environment:</strong>
            <span style="margin-left: 10pt; padding: 4pt 12pt; background-color: #1976d2;
                         color: white; border-radius: 4pt; font-weight: bold;">
                {{model.environment}}
            </span>
        </div>
        <div style="font-size: 10pt; color: #666; margin-top: 8pt;">
            Production: Optimum = 0% (minimal load preferred)<br/>
            Development: Optimum = 50% (moderate load expected during testing)
        </div>
    </div>

    <div style="margin-bottom: 12pt;">
        <strong>Current CPU Usage:</strong>
        <span style="float: right; font-weight: bold; font-size: 14pt;">
            {{model.cpuUsage}}%
        </span>
        <div style="clear: both;"></div>
    </div>

    <meter value="{{model.cpuUsage}}"
           min="0"
           max="100"
           low="30"
           high="75"
           optimum="{{model.environment === 'production' ? model.prodOptimum : model.devOptimum}}"
           style="width: 100%; height: 30pt; border: 2pt solid #336699;">
        {{model.cpuUsage}}%
    </meter>

    <div style="margin-top: 12pt; padding: 12pt; background-color: #fff3e0;
                border-radius: 4pt; font-size: 10pt;">
        <strong>Interpretation:</strong><br/>
        In <strong>{{model.environment}}</strong> environment, optimal CPU is
        <strong>{{model.environment === 'production' ? model.prodOptimum : model.devOptimum}}%</strong>.
        <br/><br/>
        Current usage ({{model.cpuUsage}}%) is
        {{model.cpuUsage > 75 ? 'CRITICAL' : (model.cpuUsage > 30 ? 'MODERATE' : 'OPTIMAL')}}.
        <br/><br/>
        Color logic changes based on environment - same value can be green in
        development but orange in production due to different optimum settings.
    </div>
</div>
```

### Multiple Meters with Different Optimum Values

```html
<!-- Model: {
    metrics: [
        {name: "CPU Usage", value: 45, min: 0, max: 100, low: 30, high: 75, optimum: 0, unit: "%", pattern: "minimize"},
        {name: "Throughput", value: 850, min: 0, max: 1000, low: 400, high: 700, optimum: 1000, unit: " req/s", pattern: "maximize"},
        {name: "Temperature", value: 72, min: 60, max: 90, low: 68, high: 78, optimum: 72, unit: "°F", pattern: "target"}
    ]
} -->

<div style="padding: 25pt; background-color: #fafafa; border-radius: 10pt;">
    <h2 style="margin: 0 0 25pt 0; color: #333; text-align: center;">
        System Metrics Dashboard
    </h2>

    <template data-bind="{{model.metrics}}">
        <div style="margin-bottom: 25pt; padding: 20pt; background-color: white;
                    border-radius: 8pt; box-shadow: 0 2pt 6pt rgba(0,0,0,0.1);">
            <div style="margin-bottom: 12pt;">
                <strong style="font-size: 14pt;">{{.name}}</strong>
                <span style="float: right; font-size: 18pt; font-weight: bold;">
                    {{.value}}{{.unit}}
                </span>
                <div style="clear: both;"></div>
            </div>

            <meter value="{{.value}}"
                   min="{{.min}}"
                   max="{{.max}}"
                   low="{{.low}}"
                   high="{{.high}}"
                   optimum="{{.optimum}}"
                   style="width: 100%; height: 32pt; border: 2pt solid #666;">
                {{.value}}{{.unit}}
            </meter>

            <div style="margin-top: 12pt; padding: 12pt; background-color: #f5f5f5;
                        border-radius: 4pt; font-size: 10pt;">
                <strong>Configuration:</strong><br/>
                Pattern: <strong>{{.pattern}}</strong> | Optimum: {{.optimum}}{{.unit}}<br/>
                Range: {{.min}}-{{.max}}{{.unit}} | Thresholds: Low={{.low}}, High={{.high}}
                <br/><br/>
                <strong>Logic:</strong>
                {{.pattern === "minimize" ? "Lower values are better (optimum at minimum)" : ""}}
                {{.pattern === "maximize" ? "Higher values are better (optimum at maximum)" : ""}}
                {{.pattern === "target" ? "Middle values are better (optimum in range)" : ""}}
            </div>
        </div>
    </template>

    <div style="margin-top: 25pt; padding: 20pt; background-color: #e3f2fd;
                border-radius: 8pt; font-size: 11pt;">
        <strong>Three Optimum Patterns Demonstrated:</strong><br/>
        <table style="width: 100%; margin-top: 12pt; font-size: 10pt;">
            <tr style="background-color: white;">
                <td style="padding: 10pt; border: 1pt solid #ddd;">
                    <strong>Minimize Pattern</strong>
                </td>
                <td style="padding: 10pt; border: 1pt solid #ddd;">
                    Optimum at minimum (0)<br/>
                    Lower is better - CPU, errors, costs
                </td>
            </tr>
            <tr style="background-color: white;">
                <td style="padding: 10pt; border: 1pt solid #ddd;">
                    <strong>Maximize Pattern</strong>
                </td>
                <td style="padding: 10pt; border: 1pt solid #ddd;">
                    Optimum at maximum (1000)<br/>
                    Higher is better - throughput, quality, revenue
                </td>
            </tr>
            <tr style="background-color: white;">
                <td style="padding: 10pt; border: 1pt solid #ddd;">
                    <strong>Target Pattern</strong>
                </td>
                <td style="padding: 10pt; border: 1pt solid #ddd;">
                    Optimum in middle (72)<br/>
                    Middle is better - temperature, pH, pressure
                </td>
            </tr>
        </table>
    </div>
</div>
```

---

## See Also

- [meter](/reference/htmltags/meter.html) - Meter/gauge element
- [value](/reference/htmlattributes/value.html) - Value attribute
- [min and max](/reference/htmlattributes/min_max.html) - Range attributes
- [high and low](/reference/htmlattributes/high_low.html) - Threshold attributes
- [progress](/reference/htmltags/progress.html) - Progress bar element (no optimum)
- [Data Binding](/reference/binding/) - Dynamic data binding and expressions
- [CSS Styles](/reference/styles/) - Complete CSS styling reference

---
