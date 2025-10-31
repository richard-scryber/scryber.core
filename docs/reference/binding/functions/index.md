---
layout: default
title: Expression Functions
parent: Data Binding Reference
parent_url: /reference/binding/
grand_parent: Reference
grand_parent_url: /reference/
has_children: true
has_toc: false
---

# Expression Functions Reference
{: .no_toc }

Built-in functions for data manipulation and formatting in Scryber binding expressions.

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{:toc}
</details>

---

## Overview

Expression functions transform and manipulate data within binding expressions. Over 90 built-in functions are available across multiple categories.

**Usage:**
```handlebars
{{functionName(param1, param2, ...)}}
```

---

## Conversion Functions

Convert values between different types.

| Function | Description | Example |
|----------|-------------|---------|
| [int](./int.md) | Convert to integer | `{{int(model.value)}}` |
| [long](./long.md) | Convert to long integer | `{{long(model.bigNumber)}}` |
| [double](./double.md) | Convert to double precision float | `{{double(model.value)}}` |
| [decimal](./decimal.md) | Convert to decimal | `{{decimal(model.price)}}` |
| [bool](./bool.md) | Convert to boolean | `{{bool(model.flag)}}` |
| [date](./date.md) | Convert to DateTime | `{{date(model.dateString)}}` |
| [typeof](./typeof.md) | Get type name | `{{typeof(model.value)}}` |

---

## String Functions

Manipulate and format text.

| Function | Description | Example |
|----------|-------------|---------|
| [format](./format.md) / [string](./string.md) | Format values as strings | `{{format(model.price, 'C2')}}` |
| [concat](./concat.md) | Concatenate strings | `{{concat(model.first, ' ', model.last)}}` |
| [join](./join.md) | Join array with separator | `{{join(model.items, ', ')}}` |
| [substring](./substring.md) | Extract substring | `{{substring(model.text, 0, 10)}}` |
| [replace](./replace.md) | Replace text | `{{replace(model.text, 'old', 'new')}}` |
| [toLower](./toLower.md) | Convert to lowercase | `{{toLower(model.text)}}` |
| [toUpper](./toUpper.md) | Convert to UPPERCASE | `{{toUpper(model.code)}}` |
| [trim](./trim.md) | Remove leading/trailing whitespace | `{{trim(model.text)}}` |
| [trimEnd](./trimEnd.md) | Remove trailing whitespace | `{{trimEnd(model.text)}}` |
| [length](./length.md) | Get string length | `{{length(model.text)}}` |
| [contains](./contains.md) | Check if contains substring | `{{contains(model.text, 'search')}}` |
| [startsWith](./startsWith.md) | Check if starts with substring | `{{startsWith(model.text, 'prefix')}}` |
| [endsWith](./endsWith.md) | Check if ends with substring | `{{endsWith(model.text, 'suffix')}}` |
| [indexOf](./indexOf.md) | Find substring position | `{{indexOf(model.text, 'search')}}` |
| [padLeft](./padLeft.md) | Pad left with characters | `{{padLeft(model.num, 5, '0')}}` |
| [padRight](./padRight.md) | Pad right with characters | `{{padRight(model.text, 10, ' ')}}` |
| [split](./split.md) | Split string into array | `{{split(model.text, ',')}}` |
| [regexIsMatch](./regexIsMatch.md) | Test regex pattern | `{{regexIsMatch(model.email, '^.+@.+$')}}` |
| [regexMatches](./regexMatches.md) | Find all regex matches | `{{regexMatches(model.text, '\\d+')}}` |
| [regexSwap](./regexSwap.md) | Replace using regex | `{{regexSwap(model.text, '\\d+', 'X')}}` |

---

## Mathematical Functions

Perform calculations and mathematical operations.

| Function | Description | Example |
|----------|-------------|---------|
| [abs](./abs.md) | Absolute value | `{{abs(model.value)}}` |
| [ceiling](./ceiling.md) | Round up to integer | `{{ceiling(model.value)}}` |
| [floor](./floor.md) | Round down to integer | `{{floor(model.value)}}` |
| [round](./round.md) | Round to nearest | `{{round(model.value, 2)}}` |
| [truncate](./truncate.md) | Truncate decimal | `{{truncate(model.value)}}` |
| [sqrt](./sqrt.md) | Square root | `{{sqrt(model.value)}}` |
| [pow](./pow.md) | Raise to power | `{{pow(model.base, model.exp)}}` |
| [exp](./exp.md) | Exponential (e^x) | `{{exp(model.value)}}` |
| [log](./log.md) | Natural logarithm | `{{log(model.value)}}` |
| [log10](./log10.md) | Base-10 logarithm | `{{log10(model.value)}}` |
| [sign](./sign.md) | Sign of number (-1, 0, 1) | `{{sign(model.value)}}` |
| [sin](./sin.md) | Sine | `{{sin(model.radians)}}` |
| [cos](./cos.md) | Cosine | `{{cos(model.radians)}}` |
| [tan](./tan.md) | Tangent | `{{tan(model.radians)}}` |
| [asin](./asin.md) | Arcsine | `{{asin(model.value)}}` |
| [acos](./acos.md) | Arccosine | `{{acos(model.value)}}` |
| [atan](./atan.md) | Arctangent | `{{atan(model.value)}}` |
| [degrees](./degrees.md) | Convert radians to degrees | `{{degrees(model.radians)}}` |
| [radians](./radians.md) | Convert degrees to radians | `{{radians(model.degrees)}}` |
| [pi](./pi.md) | Pi constant (3.14159...) | `{{pi()}}` |
| [e](./e.md) | Euler's number (2.71828...) | `{{e()}}` |
| [random](./random.md) | Random number | `{{random()}}` |

---

## Date & Time Functions

Work with dates and timestamps.

| Function | Description | Example |
|----------|-------------|---------|
| [addDays](./addDays.md) | Add days to date | `{{addDays(model.date, 7)}}` |
| [addMonths](./addMonths.md) | Add months to date | `{{addMonths(model.date, 1)}}` |
| [addYears](./addYears.md) | Add years to date | `{{addYears(model.date, 1)}}` |
| [addHours](./addHours.md) | Add hours to date | `{{addHours(model.date, 2)}}` |
| [addMinutes](./addMinutes.md) | Add minutes to date | `{{addMinutes(model.date, 30)}}` |
| [addSeconds](./addSeconds.md) | Add seconds to date | `{{addSeconds(model.date, 45)}}` |
| [addMilliseconds](./addMilliseconds.md) | Add milliseconds to date | `{{addMilliseconds(model.date, 500)}}` |
| [daysBetween](./daysBetween.md) | Days between two dates | `{{daysBetween(model.start, model.end)}}` |
| [hoursBetween](./hoursBetween.md) | Hours between two dates | `{{hoursBetween(model.start, model.end)}}` |
| [minutesBetween](./minutesBetween.md) | Minutes between two dates | `{{minutesBetween(model.start, model.end)}}` |
| [secondsBetween](./secondsBetween.md) | Seconds between two dates | `{{secondsBetween(model.start, model.end)}}` |
| [yearOf](./yearOf.md) | Extract year | `{{yearOf(model.date)}}` |
| [monthOfYear](./monthOfYear.md) | Extract month (1-12) | `{{monthOfYear(model.date)}}` |
| [dayOfMonth](./dayOfMonth.md) | Extract day (1-31) | `{{dayOfMonth(model.date)}}` |
| [dayOfWeek](./dayOfWeek.md) | Extract day of week (0-6) | `{{dayOfWeek(model.date)}}` |
| [dayOfYear](./dayOfYear.md) | Extract day of year (1-366) | `{{dayOfYear(model.date)}}` |
| [hourOf](./hourOf.md) | Extract hour (0-23) | `{{hourOf(model.date)}}` |
| [minuteOf](./minuteOf.md) | Extract minute (0-59) | `{{minuteOf(model.date)}}` |
| [secondOf](./secondOf.md) | Extract second (0-59) | `{{secondOf(model.date)}}` |
| [millisecondOf](./millisecondOf.md) | Extract millisecond (0-999) | `{{millisecondOf(model.date)}}` |

---

## Logical Functions

Control flow and conditional logic within expressions.

| Function | Description | Example |
|----------|-------------|---------|
| [if](./if.md) | Ternary conditional | `{{if(model.active, 'Yes', 'No')}}` |
| [ifError](./ifError.md) | Fallback on error | `{{ifError(model.value, 'default')}}` |
| [in](./in.md) | Check if value in list | `{{in(model.status, 'active', 'pending')}}` |

---

## Collection Functions

Aggregate and manipulate collections.

| Function | Description | Example |
|----------|-------------|---------|
| [count](./count.md) | Count items in collection | `{{count(model.items)}}` |
| [countOf](./countOf.md) | Count with condition | `{{countOf(model.items, 'isActive')}}` |
| [sum](./sum.md) | Sum numeric values | `{{sum(model.numbers)}}` |
| [sumOf](./sumOf.md) | Sum property values | `{{sumOf(model.items, 'price')}}` |
| [min](./min.md) | Minimum value | `{{min(model.numbers)}}` |
| [minOf](./minOf.md) | Minimum property value | `{{minOf(model.items, 'price')}}` |
| [max](./max.md) | Maximum value | `{{max(model.numbers)}}` |
| [maxOf](./maxOf.md) | Maximum property value | `{{maxOf(model.items, 'price')}}` |
| [collect](./collect.md) | Extract property values | `{{collect(model.items, 'name')}}` |
| [each](./each.md) | Iterate with function | `{{each(model.items, 'transform')}}` |
| [eachOf](./eachOf.md) | Iterate property with function | `{{eachOf(model.items, 'prop', 'fn')}}` |
| [firstWhere](./firstWhere.md) | Find first matching item | `{{firstWhere(model.items, 'isActive')}}` |
| [selectWhere](./selectWhere.md) | Filter collection | `{{selectWhere(model.items, 'isActive')}}` |
| [sortBy](./sortBy.md) | Sort by property | `{{sortBy(model.items, 'name')}}` |
| [reverse](./reverse.md) | Reverse order | `{{reverse(model.items)}}` |

---

## Statistical Functions

Calculate statistics on collections.

| Function | Description | Example |
|----------|-------------|---------|
| [average](./average.md) | Average of values | `{{average(model.numbers)}}` |
| [averageOf](./averageOf.md) | Average of property | `{{averageOf(model.items, 'price')}}` |
| [mean](./mean.md) | Mean value | `{{mean(model.numbers)}}` |
| [median](./median.md) | Median value | `{{median(model.numbers)}}` |
| [mode](./mode.md) | Mode (most frequent) | `{{mode(model.numbers)}}` |

---

## CSS Functions

Dynamic CSS calculations.

| Function | Description | Example |
|----------|-------------|---------|
| [calc](./calc.md) | CSS calc expression | `{{calc('100% - 20pt')}}` |
| [var](./var.md) | CSS variable reference | `{{var('primary-color')}}` |

---

## Common Patterns

### Formatting Currency

```handlebars
<p>Total: {{format(model.total, 'C2')}}</p>
<!-- Output: Total: $1,234.56 -->
```

### Date Calculations

```handlebars
<p>Due: {{format(addDays(model.orderDate, 30), 'MMMM dd, yyyy')}}</p>
<!-- Output: Due: April 14, 2024 -->
```

### Conditional Values

```handlebars
<span class="{{if(model.score >= 70, 'pass', 'fail')}}">
  Score: {{model.score}}
</span>
```

### Collection Aggregation

```handlebars
<p>Total Items: {{count(model.items)}}</p>
<p>Total Price: {{format(sumOf(model.items, 'price'), 'C2')}}</p>
<p>Average: {{format(averageOf(model.items, 'price'), 'C2')}}</p>
```

### String Manipulation

```handlebars
<h2>{{toUpper(substring(model.title, 0, 1))}}{{substring(model.title, 1)}}</h2>
<!-- Output: Capitalize first letter -->
```

---

## See Also

- [Handlebars Helpers](../helpers/) - Control flow and context management
- [Binding Operators](../operators/) - Mathematical, comparison, and logical operators
- [Expressions Guide](../../learning/02-data-binding/02_expressions.md) - Complete guide to expressions

---
