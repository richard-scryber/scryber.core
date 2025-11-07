---
layout: default
title: Common Mistakes
nav_order: 9
parent: Data Binding & Expressions
parent_url: /learning/02-data-binding/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Common Mistakes

Learn to identify and fix common data binding errors before they become problems.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Recognize common data binding errors
- Fix syntax and formatting mistakes
- Avoid null reference issues
- Handle context and scope correctly
- Debug template binding problems
- Follow best practices for reliable templates

---

## Mistake 1: Missing or Wrong Context

### ❌ Problem: Forgetting `this` in Loops

{% raw %}
```html
{{#each products}}
    <p>{{name}}</p>  <!-- May not bind correctly -->
{{/each}}
```
{% endraw %}

### ✅ Solution: Always Use `this`

{% raw %}
```html
{{#each products}}
    <p>{{this.name}}</p>  <!-- Explicit context -->
{{/each}}
```
{% endraw %}

### Why It Matters

Without `this`, the binding engine may look for the property in the wrong scope, especially in nested loops or complex templates.

---

## Mistake 2: Incorrect Parent Context Navigation

### ❌ Problem: Wrong Number of `../`

{% raw %}
```html
{{#each departments}}
    {{#each this.employees}}
        <p>{{../companyName}}</p>  <!-- Too few ../ -->
    {{/each}}
{{/each}}
```
{% endraw %}

### ✅ Solution: Count Your Nesting Levels

{% raw %}
```html
{{#each departments}}
    {{#each this.employees}}
        <p>{{../../companyName}}</p>  <!-- Correct: goes up 2 levels -->
    {{/each}}
{{/each}}
```
{% endraw %}

### Debugging Tip

Count the `{{#each}}` blocks you're inside:
- 1 level deep: `../property`
- 2 levels deep: `../../property`
- 3 levels deep: `../../../property`

---

## Mistake 3: Case Sensitivity Issues

### ❌ Problem: Mismatched Case

```csharp
// C# code
doc.Params["model"] = new { Name = "John" };  // Capital N
```

{% raw %}
```html
<!-- Template -->
<p>{{model.name}}</p>  <!-- lowercase n - Won't bind! -->
```
{% endraw %}

### ✅ Solution: Match Case Exactly

```csharp
doc.Params["model"] = new { name = "John" };  // lowercase n
```

{% raw %}
```html
<p>{{model.name}}</p>  <!-- lowercase n - Will bind! -->
```
{% endraw %}

### Best Practice

Use consistent casing convention:
- **camelCase** for properties (recommended): `firstName`, `orderTotal`
- **PascalCase** if matching C# classes: `FirstName`, `OrderTotal`

---

## Mistake 4: Null Reference Errors

### ❌ Problem: Accessing Null Properties

```csharp
doc.Params["model"] = new
{
    customer = null  // Null object
};
```

{% raw %}
```html
<p>{{model.customer.name}}</p>  <!-- Error! customer is null -->
```
{% endraw %}

### ✅ Solution: Check for Null

{% raw %}
```html
{{#if model.customer}}
    <p>{{model.customer.name}}</p>
{{else}}
    <p>No customer information</p>
{{/if}}
```
{% endraw %}

Or provide defaults in C#:

```csharp
doc.Params["model"] = new
{
    customer = customerData ?? new { name = "N/A", email = "N/A" }
};
```

---

## Mistake 5: Wrong Operator in Conditionals

### ❌ Problem: Using `=` Instead of `==`

{% raw %}
```html
{{#if model.status = 'active'}}  <!-- Assignment, not comparison! -->
    <p>Active</p>
{{/if}}
```
{% endraw %}

### ✅ Solution: Use `==` for Comparison

{% raw %}
```html
{{#if model.status == 'active'}}  <!-- Correct comparison -->
    <p>Active</p>
{{/if}}
```
{% endraw %}

### Common Comparison Operators

- `==` Equal to
- `!=` Not equal to
- `>` Greater than
- `>=` Greater than or equal
- `<` Less than
- `<=` Less than or equal

---

## Mistake 6: Incorrect calc() Syntax

### ❌ Problem: Operators Not as Strings

{% raw %}
```html
{{calc(model.price + model.tax)}}  <!-- Won't work -->
```
{% endraw %}

### ✅ Solution: Operators Must Be String Arguments

{% raw %}
```html
{{calc(model.price, '+', model.tax)}}  <!-- Correct -->
```
{% endraw %}

### ❌ Problem: Parentheses as Single Argument

{% raw %}
```html
{{calc((model.a + model.b), '*', model.c)}}  <!-- Won't work -->
```
{% endraw %}

### ✅ Solution: Parentheses as Separate Arguments

{% raw %}
```html
{{calc('(', model.a, '+', model.b, ')', '*', model.c)}}  <!-- Correct -->
```
{% endraw %}

---

## Mistake 7: Forgetting Quotes in Strings

### ❌ Problem: Unquoted String Literals

{% raw %}
```html
{{concat(Hello, model.name)}}  <!-- Error: Hello is undefined -->
```
{% endraw %}

### ✅ Solution: Quote String Literals

{% raw %}
```html
{{concat('Hello, ', model.name)}}  <!-- Correct -->
```
{% endraw %}

---

## Mistake 8: Not Closing Blocks

### ❌ Problem: Missing {{/if}} or {{/each}}

{% raw %}
```html
{{#if model.showSection}}
    <div>Content</div>
<!-- Missing {{/if}} -->

<p>More content</p>
```
{% endraw %}

### ✅ Solution: Always Close Blocks

{% raw %}
```html
{{#if model.showSection}}
    <div>Content</div>
{{/if}}

<p>More content</p>
```
{% endraw %}

### Debugging Tip

Count your opening and closing tags:
- `{{#if}}` must have `{{/if}}`
- `{{#each}}` must have `{{/each}}`
- Keep them properly nested

---

## Mistake 9: Percentage Format Confusion

### ❌ Problem: Double Multiplication

{% raw %}
```html
<!-- If model.rate is already 15 (as in 15%) -->
{{format(model.rate, 'P0')}}  <!-- Shows 1500%! -->
```
{% endraw %}

### ✅ Solution: Understand the Input

{% raw %}
```html
<!-- If model.rate is 0.15 (decimal) -->
{{format(model.rate, 'P0')}}  <!-- Shows 15% -->

<!-- If model.rate is 15 (already percentage) -->
{{model.rate}}%  <!-- Shows 15% -->
```
{% endraw %}

**Remember:** `P` format multiplies by 100!

---

## Mistake 10: Modifying Parameters After Parsing

### ❌ Problem: Setting Params Too Late

```csharp
var doc = Document.ParseDocument("template.html");
doc.SaveAsPDF("output.pdf");
doc.Params["model"] = data;  // Too late!
```

### ✅ Solution: Set Parameters Before Saving

```csharp
var doc = Document.ParseDocument("template.html");
doc.Params["model"] = data;  // Set first
doc.SaveAsPDF("output.pdf");  // Then save
```

---

## Mistake 11: Overwriting Parameters

### ❌ Problem: Same Key Used Twice

```csharp
doc.Params["model"] = customerData;
doc.Params["model"] = invoiceData;  // Overwrites customer data!
```

### ✅ Solution: Use Different Keys

```csharp
doc.Params["customer"] = customerData;
doc.Params["invoice"] = invoiceData;
```

---

## Mistake 12: Complex Inline Calculations

### ❌ Problem: Unreadable Template Logic

{% raw %}
```html
<p>{{format(calc(calc(model.price, '*', model.quantity), '+',
            calc(calc(model.price, '*', model.quantity), '*', 0.08)), 'C2')}}</p>
```
{% endraw %}

### ✅ Solution: Calculate in C#

```csharp
var subtotal = price * quantity;
var tax = subtotal * 0.08;
var total = subtotal + tax;

doc.Params["model"] = new
{
    price = price,
    quantity = quantity,
    subtotal = subtotal,
    tax = tax,
    total = total
};
```

{% raw %}
```html
<p>{{format(model.total, 'C2')}}</p>  <!-- Simple and readable -->
```
{% endraw %}

---

## Mistake 13: Not Handling Empty Collections

### ❌ Problem: No Fallback for Empty Data

{% raw %}
```html
<h2>Products</h2>
{{#each products}}
    <div>{{this.name}}</div>
{{/each}}
<!-- Shows nothing if empty -->
```
{% endraw %}

### ✅ Solution: Add {{else}} Clause

{% raw %}
```html
<h2>Products</h2>
{{#each products}}
    <div>{{this.name}}</div>
{{else}}
    <p>No products available.</p>
{{/each}}
```
{% endraw %}

---

## Mistake 14: Incorrect Date Formatting

### ❌ Problem: Wrong Format Specifiers

{% raw %}
```html
{{format(model.date, 'YYYY-MM-DD')}}  <!-- Wrong: YYYY should be yyyy -->
```
{% endraw %}

### ✅ Solution: Use Correct Specifiers

{% raw %}
```html
{{format(model.date, 'yyyy-MM-dd')}}  <!-- Correct: lowercase yyyy -->
```
{% endraw %}

**Common Mistakes:**
- `YYYY` → Should be `yyyy`
- `DD` → Should be `dd`
- `mm` (minutes) vs `MM` (month)

---

## Mistake 15: Ignoring Data Types

### ❌ Problem: Treating Numbers as Strings

```csharp
doc.Params["model"] = new
{
    price = "123.45"  // String, not number
};
```

{% raw %}
```html
{{calc(model.price, '*', 2)}}  <!-- May not work correctly -->
```
{% endraw %}

### ✅ Solution: Use Correct Data Types

```csharp
doc.Params["model"] = new
{
    price = 123.45  // Numeric type
};
```

{% raw %}
```html
{{calc(model.price, '*', 2)}}  <!-- Works correctly -->
```
{% endraw %}

---

## Debugging Checklist

When data binding doesn't work, check:

- [ ] **Case sensitivity** - Property names match exactly?
- [ ] **Context** - Using `this` in loops?
- [ ] **Null values** - Checking for null before accessing properties?
- [ ] **Closed blocks** - Every `{{#if}}` has `{{/if}}`?
- [ ] **Parameters set** - `doc.Params` set before `SaveAsPDF()`?
- [ ] **Data types** - Numbers as numbers, not strings?
- [ ] **Format strings** - Using correct format specifiers?
- [ ] **Operators** - Using `==` for comparison, not `=`?

---

## Best Practices Summary

### Do's ✅

1. **Always use `this`** in loops
2. **Check for null** before accessing nested properties
3. **Calculate complex values** in C# code
4. **Use consistent naming** conventions
5. **Provide fallbacks** for empty collections
6. **Set parameters early** before saving
7. **Use clear variable names** in templates
8. **Test with empty/null data** to catch edge cases

### Don'ts ❌

1. **Don't** put complex logic in templates
2. **Don't** ignore null values
3. **Don't** forget to close blocks
4. **Don't** use `=` for comparisons (use `==`)
5. **Don't** calculate everything in templates
6. **Don't** rely on default type conversions
7. **Don't** nest loops too deeply (> 3 levels)
8. **Don't** forget to quote string literals

---

## Testing Your Templates

### Create Test Data

```csharp
public object CreateTestData()
{
    return new
    {
        // Normal cases
        customer = new { name = "John Doe", email = "john@example.com" },

        // Edge cases
        emptyList = new object[] { },
        nullValue = (string)null,
        largeNumber = 1234567890,
        smallDecimal = 0.001,
        date = DateTime.Now,

        // Boundary cases
        longText = new string('A', 1000),
        specialChars = "Test & <special> characters"
    };
}
```

### Test Edge Cases

```csharp
[Test]
public void TestTemplateWithEmptyData()
{
    var doc = Document.ParseDocument("template.html");
    doc.Params["model"] = new
    {
        items = new object[] { }  // Empty collection
    };

    byte[] pdf;
    using (var ms = new MemoryStream())
    {
        doc.SaveAsPDF(ms);
        pdf = ms.ToArray();
    }

    Assert.IsNotNull(pdf);
    Assert.Greater(pdf.Length, 0);
}

[Test]
public void TestTemplateWithNullValues()
{
    var doc = Document.ParseDocument("template.html");
    doc.Params["model"] = new
    {
        optionalField = (string)null
    };

    // Should not throw exception
    byte[] pdf;
    using (var ms = new MemoryStream())
    {
        doc.SaveAsPDF(ms);
        pdf = ms.ToArray();
    }

    Assert.IsNotNull(pdf);
}
```

---

## Quick Reference: Common Fixes

| Error | Fix |
|-------|-----|
| Property not binding | Check case sensitivity |
| Null reference | Add `{{#if}}` check |
| Wrong parent context | Count `../` correctly |
| calc() not working | Operators as strings: `'+'` |
| Percentage shows wrong | Check if value needs `P` format |
| Block not rendering | Check for closing `{{/if}}` or `{{/each}}` |
| Data not appearing | Verify `doc.Params` set before save |
| Empty collection shows nothing | Add `{{else}}` clause |

---

## Next Steps

Now that you know how to avoid common mistakes:

1. **[Styling PDFs](/learning/03-styling/)** - Apply advanced styling
2. **[Layout Techniques](/learning/04-layout/)** - Master page layout
3. **[Practical Applications](/learning/08-practical/)** - Build real-world documents

---

**Continue learning →** [Styling PDFs](/learning/03-styling/)**
