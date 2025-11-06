# Scryber Data Binding Reference

This reference covers all data binding features available in Scryber templates, including Handlebars helpers, operators, and expression functions.

## Quick Navigation

- [Handlebars Helpers](#handlebars-helpers)
- [Binding Operators](#binding-operators)
  - [Mathematical Operators](#mathematical-operators)
  - [Comparison Operators](#comparison-operators)
  - [Logical Operators](#logical-operators)
  - [Other Operators](#other-operators)
- [Expression Functions](#expression-functions)
  - [Conversion Functions](#conversion-functions)
  - [String Functions](#string-functions)
  - [Mathematical Functions](#mathematical-functions)
  - [Date & Time Functions](#date--time-functions)
  - [Logical Functions](#logical-functions)
  - [Collection Functions](#collection-functions)
  - [Statistical Functions](#statistical-functions)
  - [CSS Functions](#css-functions)

---

## At a Glance

- **6 Handlebars Helpers** - Block-level control structures
- **15+ Operators** - Mathematical, logical, and comparison operators
- **90+ Functions** - Organized into 8 categories
- **Special Variables** - Context-aware variables like @index, this, ..

---

## Handlebars Helpers

Block-level helpers for control flow and iteration in templates.

| Helper | Syntax | Description |
|--------|--------|-------------|
| **each** | `{{#each collection}}...{{/each}}` | Iterate over arrays or collections. Access items with `{{this}}` |
| **with** | `{{#with object}}...{{/with}}` | Change data context to a specific object |
| **if** | `{{#if condition}}...{{/if}}` | Conditional rendering based on expression |
| **else if** | `{{else if condition}}` | Additional condition in an if block |
| **else** | `{{else}}` | Fallback when conditions aren't met |
| **log** | `{{log "message"}}` | Output debug messages to trace log |

[→ Detailed Handlebars Helpers Documentation](./helpers/)

---

## Binding Operators

Operators used within `{{expression}}` bindings.

### Mathematical Operators

| Operator | Example | Description |
|----------|---------|-------------|
| `+` | `{{a + b}}` | Addition |
| `-` | `{{a - b}}` | Subtraction |
| `*` | `{{a * b}}` | Multiplication |
| `/` | `{{a / b}}` | Division |
| `%` | `{{a % b}}` | Modulus (remainder) |
| `^` | `{{a ^ b}}` | Power (exponentiation) |

### Comparison Operators

| Operator | Example | Description |
|----------|---------|-------------|
| `==` | `{{a == b}}` | Equality |
| `!=` | `{{a != b}}` | Inequality |
| `<` | `{{a < b}}` | Less than |
| `<=` | `{{a <= b}}` | Less than or equal |
| `>` | `{{a > b}}` | Greater than |
| `>=` | `{{a >= b}}` | Greater than or equal |

### Logical Operators

| Operator | Example | Description |
|----------|---------|-------------|
| `&&` | `{{a && b}}` | Logical AND |
| `||` | `{{a || b}}` | Logical OR |
| `!` | `{{!a}}` | Logical NOT |

### Other Operators

| Operator | Example | Description |
|----------|---------|-------------|
| `??` | `{{a ?? b}}` | Null coalescing - returns b if a is null |
| `.` | `{{model.property}}` | Property access |
| `[]` | `{{array[0]}}` | Array/indexer access |
| `..` | `{{../parent}}` | Navigate to parent context |
| `this` | `{{this.property}}` | Current context reference |

[→ Detailed Operators Documentation](./operators/)

---

## Expression Functions

Built-in functions available in `{{expression}}` bindings.

### Conversion Functions

| Function | Signature | Description |
|----------|-----------|-------------|
| **string** | `string(value, format?)` | Convert to string with optional formatting |
| **int** | `int(value)` | Convert to 32-bit integer |
| **integer** | `integer(value)` | Alias for int() |
| **long** | `long(value)` | Convert to 64-bit integer |
| **double** | `double(value)` | Convert to double-precision float |
| **decimal** | `decimal(value)` | Convert to decimal number |
| **bool** | `bool(value)` | Convert to boolean |
| **date** | `date(value)` | Convert to DateTime |
| **typeof** | `typeof(value)` | Get type name as string |

[→ Conversion Functions Details](./functions/#conversion-functions)

### String Functions

| Function | Signature | Description |
|----------|-----------|-------------|
| **concat** | `concat(str1, str2, ...)` | Concatenate multiple strings |
| **join** | `join(separator, array)` | Join array elements with separator |
| **substring** | `substring(str, start, length?)` | Extract portion of string |
| **replace** | `replace(str, find, replace)` | Replace text in string |
| **toLower** | `toLower(str)` | Convert to lowercase |
| **toUpper** | `toUpper(str)` | Convert to uppercase |
| **trim** | `trim(str)` | Remove whitespace from both ends |
| **trimEnd** | `trimEnd(str)` | Remove trailing whitespace |
| **length** | `length(str)` | Get string length |
| **contains** | `contains(str, search)` | Check if string contains text |
| **startsWith** | `startsWith(str, prefix)` | Check if starts with prefix |
| **endsWith** | `endsWith(str, suffix)` | Check if ends with suffix |
| **indexOf** | `indexOf(str, search)` | Find position of substring |
| **padLeft** | `padLeft(str, length, char)` | Pad string on left |
| **padRight** | `padRight(str, length, char)` | Pad string on right |
| **split** | `split(str, separator)` | Split string into array |
| **regexIsMatch** | `regexIsMatch(str, pattern)` | Test if matches regex |
| **regexMatches** | `regexMatches(str, pattern)` | Get all regex matches |
| **regexSwap** | `regexSwap(str, pattern, replacement)` | Replace using regex |

[→ String Functions Details](./functions/#string-functions)

### Mathematical Functions

| Function | Signature | Description |
|----------|-----------|-------------|
| **abs** | `abs(value)` | Absolute value |
| **ceiling** | `ceiling(value)` | Round up to nearest integer |
| **floor** | `floor(value)` | Round down to nearest integer |
| **round** | `round(value, decimals?)` | Round to nearest value |
| **truncate** | `truncate(value)` | Remove decimal portion |
| **sqrt** | `sqrt(value)` | Square root |
| **pow** | `pow(base, exponent)` | Raise to power |
| **exp** | `exp(value)` | e raised to power (e^value) |
| **log** | `log(value)` | Natural logarithm (base e) |
| **log10** | `log10(value)` | Base-10 logarithm |
| **sign** | `sign(value)` | Returns -1, 0, or 1 |
| **sin** | `sin(angle)` | Sine (angle in radians) |
| **cos** | `cos(angle)` | Cosine (angle in radians) |
| **tan** | `tan(angle)` | Tangent (angle in radians) |
| **asin** | `asin(value)` | Arcsine |
| **acos** | `acos(value)` | Arccosine |
| **atan** | `atan(value)` | Arctangent |
| **degrees** | `degrees(radians)` | Convert radians to degrees |
| **radians** | `radians(degrees)` | Convert degrees to radians |
| **pi** | `pi()` | Pi constant (3.14159...) |
| **e** | `e()` | Euler's number (2.71828...) |
| **random** | `random()` | Random value between 0 and 1 |

[→ Mathematical Functions Details](./functions/#mathematical-functions)

### Date & Time Functions

| Function | Signature | Description |
|----------|-----------|-------------|
| **addDays** | `addDays(date, days)` | Add days to date |
| **addMonths** | `addMonths(date, months)` | Add months to date |
| **addYears** | `addYears(date, years)` | Add years to date |
| **addHours** | `addHours(date, hours)` | Add hours to date |
| **addMinutes** | `addMinutes(date, minutes)` | Add minutes to date |
| **addSeconds** | `addSeconds(date, seconds)` | Add seconds to date |
| **addMilliseconds** | `addMilliseconds(date, ms)` | Add milliseconds to date |
| **daysBetween** | `daysBetween(date1, date2)` | Calculate days between dates |
| **hoursBetween** | `hoursBetween(date1, date2)` | Calculate hours between dates |
| **minutesBetween** | `minutesBetween(date1, date2)` | Calculate minutes between dates |
| **secondsBetween** | `secondsBetween(date1, date2)` | Calculate seconds between dates |
| **yearOf** | `yearOf(date)` | Extract year |
| **monthOfYear** | `monthOfYear(date)` | Extract month (1-12) |
| **dayOfMonth** | `dayOfMonth(date)` | Extract day of month |
| **dayOfWeek** | `dayOfWeek(date)` | Extract day of week |
| **dayOfYear** | `dayOfYear(date)` | Extract day of year (1-365) |
| **hourOf** | `hourOf(date)` | Extract hour (0-23) |
| **minuteOf** | `minuteOf(date)` | Extract minute (0-59) |
| **secondOf** | `secondOf(date)` | Extract second (0-59) |
| **millisecondOf** | `millisecondOf(date)` | Extract millisecond (0-999) |

[→ Date & Time Functions Details](./functions/#date--time-functions)

### Logical Functions

| Function | Signature | Description |
|----------|-----------|-------------|
| **if** | `if(condition, trueValue, falseValue)` | Ternary conditional operator |
| **ifError** | `ifError(expression, errorValue)` | Return errorValue if expression throws |
| **in** | `in(value, item1, item2, ...)` | Check if value exists in list |

[→ Logical Functions Details](./functions/#logical-functions)

### Collection Functions

| Function | Signature | Description |
|----------|-----------|-------------|
| **count** | `count(array)` | Count elements in array |
| **countOf** | `countOf(array)` | Alias for count() |
| **sum** | `sum(array)` | Sum numeric array |
| **sumOf** | `sumOf(array, 'property')` | Sum property values |
| **min** | `min(array)` | Find minimum value |
| **minOf** | `minOf(array, 'property')` | Find minimum property value |
| **max** | `max(array)` | Find maximum value |
| **maxOf** | `maxOf(array, 'property')` | Find maximum property value |
| **collect** | `collect(expression, array)` | Map expression over array |
| **each** | `each(array)` | Iterate over array (returns array) |
| **eachOf** | `eachOf(array, 'property')` | Extract property from each element |
| **firstWhere** | `firstWhere(array, condition)` | Find first matching element |
| **selectWhere** | `selectWhere(array, condition)` | Filter array by condition |
| **sortBy** | `sortBy(array, 'property')` | Sort array by property |
| **reverse** | `reverse(array)` | Reverse array order |

[→ Collection Functions Details](./functions/#collection-functions)

### Statistical Functions

| Function | Signature | Description |
|----------|-----------|-------------|
| **average** | `average(array)` | Calculate mean of array |
| **averageOf** | `averageOf(array, 'property')` | Calculate mean of property values |
| **mean** | `mean(array)` | Alias for average() |
| **median** | `median(array)` | Find middle value |
| **mode** | `mode(array)` | Find most common value |

[→ Statistical Functions Details](./functions/#statistical-functions)

### CSS Functions

| Function | Signature | Description |
|----------|-----------|-------------|
| **calc** | `calc(expression)` | CSS calculation (legacy - use operators instead) |
| **var** | `var(variableName)` | Get CSS variable value |

[→ CSS Functions Details](./functions/#css-functions)

---

## Special Variables

Variables available in specific contexts:

| Variable | Context | Description |
|----------|---------|-------------|
| `@index` | `{{#each}}` loops | Zero-based index of current item |
| `@first` | `{{#each}}` loops | True if first item |
| `@last` | `{{#each}}` loops | True if last item |
| `this` | Any context | Reference to current data context |
| `.` | Any context | Shorthand for current context |
| `..` | Nested contexts | Reference to parent context |
| `model` | Root | Root data model (from `doc.Params["model"]`) |

---

## Common Patterns

### Formatting Values
```handlebars
<!-- Currency -->
{{format(price, 'C2')}}

<!-- Date -->
{{format(orderDate, 'yyyy-MM-dd')}}

<!-- Number with thousands separator -->
{{format(population, 'N0')}}

<!-- Percentage -->
{{format(ratio, 'P1')}}
```

### Conditional Classes
```handlebars
<div class="{{if(isActive, 'active', 'inactive')}}">
```

### Null-Safe Navigation
```handlebars
{{user.address.city ?? 'No city specified'}}
```

### Complex Expressions
```handlebars
<!-- Calculate percentage -->
{{format((revenue / target) * 100, '0.0')}}%

<!-- Conditional with multiple criteria -->
{{#if score >= 90 && attendance >= 0.8}}
  <span class="excellent">Excellent</span>
{{/if}}
```

### Working with Collections
```handlebars
<!-- Total -->
Total: {{sumOf(items, 'price')}}

<!-- Count -->
Items: {{count(items)}}

<!-- Average -->
Average: {{format(averageOf(items, 'rating'), '0.0')}}
```

---

## Additional Resources

- [Data Binding Basics](../../learning/02-data-binding/01_data_binding_basics.md)
- [Expression Syntax](../../learning/02-data-binding/02_expression_functions.md)
- [Template Iteration](../../learning/02-data-binding/03_template_iteration.md)
- [Conditional Rendering](../../learning/02-data-binding/04_conditional_rendering.md)

---

## Custom Functions and Operators

Scryber allows registering custom functions and operators:

```csharp
// Register custom function
BindingCalcExpressionFactory.RegisterFunction(new MyCustomFunction());

// Register custom operator
BindingCalcExpressionFactory.RegisterOperator(new MyCustomOperator());
```

For more information on extending the binding system, see the [Extending Data Binding](../../learning/02-data-binding/) documentation.
