---
layout: default
title: data-test
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-test : The Conditional Test Expression Attribute

The `data-test` attribute evaluates boolean expressions to conditionally render content within `<if>` elements. When the expression evaluates to `true`, the content is rendered; when `false`, the content is completely omitted from the PDF output. This enables sophisticated conditional logic for dynamic document generation.

---

## Summary

The `data-test` attribute provides conditional rendering capabilities, enabling:
- **Boolean expression evaluation** for dynamic content control
- **Complete content removal** when condition is false (not just hidden)
- **Complex logical operations** with AND, OR, NOT operators
- **Data-driven document structure** based on runtime values
- **Flexible conditional logic** for any document scenario

This attribute is essential for:
- Personalized documents with user-specific content
- Role-based content inclusion (admin vs. user views)
- Status-dependent sections (draft, published, archived)
- Configuration-driven document generation
- Multi-language document variants
- Conditional formatting and styling
- Business logic implementation in templates

**Behavior**: When `data-test` evaluates to `false`, the `<if>` element and its content are not rendered at all, reducing document size and processing time.

---

## Usage

The `data-test` attribute is applied to `<if>` elements:

```html
<if data-test="{{model.showSection}}">
    <!-- Content rendered only when showSection is true -->
    <div>Conditional content</div>
</if>
```

### Basic Syntax

```html
<!-- Simple boolean property -->
<if data-test="{{model.isActive}}">
    <div>Active content</div>
</if>

<!-- Comparison expression -->
<if data-test="{{model.age >= 18}}">
    <div>Adult content</div>
</if>

<!-- Logical operators -->
<if data-test="{{model.isAdmin && model.hasPermission}}">
    <div>Admin content</div>
</if>

<!-- Negation -->
<if data-test="{{!model.isHidden}}">
    <div>Visible content</div>
</if>
```

---

## Supported Elements

The `data-test` attribute is **only** supported on the following element:

- `<if>` - The conditional rendering element

**Note**: This attribute is required on `<if>` elements. Without `data-test`, the `<if>` element will always render its content.

---

## Binding Values

### Expression Types

The `data-test` attribute accepts any expression that evaluates to a boolean value:

| Expression Type | Example | Description |
|----------------|---------|-------------|
| **Boolean Property** | `{{model.isActive}}` | Direct boolean value from model |
| **Comparison** | `{{model.age >= 18}}` | Numeric or string comparison |
| **Equality** | `{{model.status == 'approved'}}` | Equality check |
| **Inequality** | `{{model.status != 'rejected'}}` | Not equal check |
| **Logical AND** | `{{model.a && model.b}}` | Both conditions must be true |
| **Logical OR** | `{{model.a \|\| model.b}}` | At least one condition true |
| **Negation** | `{{!model.isDisabled}}` | Logical NOT |
| **Null Check** | `{{model.value != null}}` | Check for null values |
| **Length Check** | `{{model.items.length > 0}}` | Array/collection size |
| **Complex** | `{{(model.a && model.b) \|\| model.c}}` | Combined logic |

### Type

**Type**: `boolean` (or expression that evaluates to boolean)
**Default**: N/A (attribute is required on `<if>` elements)

### Comparison Operators

Supported comparison operators:

| Operator | Description | Example |
|----------|-------------|---------|
| `==` | Equal to | `{{model.status == 'active'}}` |
| `!=` | Not equal to | `{{model.type != 'draft'}}` |
| `>` | Greater than | `{{model.quantity > 10}}` |
| `>=` | Greater than or equal | `{{model.age >= 18}}` |
| `<` | Less than | `{{model.price < 100}}` |
| `<=` | Less than or equal | `{{model.score <= 50}}` |

### Logical Operators

Supported logical operators:

| Operator | Description | Example |
|----------|-------------|---------|
| `&&` | Logical AND | `{{model.a && model.b}}` |
| `\|\|` | Logical OR | `{{model.a \|\| model.b}}` |
| `!` | Logical NOT | `{{!model.isDisabled}}` |

### Operator Precedence

From highest to lowest precedence:
1. Parentheses `()`
2. Negation `!`
3. Comparison `==`, `!=`, `>`, `<`, `>=`, `<=`
4. Logical AND `&&`
5. Logical OR `||`

---

## Notes

### Complete Content Removal

Unlike CSS-based hiding, `data-test="false"` completely removes content:
- Content is not included in the PDF structure
- No space is allocated for the content
- Processing time is reduced (content not evaluated)
- PDF file size is smaller (content not included)

### Evaluation Timing

Expressions are evaluated during the data binding phase:
- Occurs before layout and rendering
- Model values must be available at binding time
- Changes to model after binding don't affect rendered output

### Truthy and Falsy Values

JavaScript-style truthy/falsy evaluation:

**Falsy values** (evaluate to `false`):
- `false` (boolean)
- `0` (number)
- `""` (empty string)
- `null`
- `undefined`

**Truthy values** (evaluate to `true`):
- `true` (boolean)
- Non-zero numbers
- Non-empty strings
- Objects and arrays (even if empty)

### Common Patterns

**Null/Undefined Checks**:
```html
<if data-test="{{model.value != null}}">
    <div>{{model.value}}</div>
</if>
```

**Empty Collection Checks**:
```html
<if data-test="{{model.items.length > 0}}">
    <template data-bind="{{model.items}}">
        <div>{{.name}}</div>
    </template>
</if>
```

**Multiple Conditions**:
```html
<if data-test="{{model.isActive && model.isPaid && !model.isExpired}}">
    <div>Valid subscription</div>
</if>
```

### Nesting Conditionals

Conditionals can be nested for complex logic:

```html
<if data-test="{{model.hasPermission}}">
    <if data-test="{{model.isActive}}">
        <!-- Shown only if both conditions true -->
        <div>Active with permission</div>
    </if>
</if>
```

### Alternative Approaches

For simple visibility control, consider alternatives:

**CSS-based hiding** (content still in PDF):
```html
<div hidden="{{!model.show ? 'hidden' : ''}}">Content</div>
```

**Conditional styling**:
```html
<div style="display: {{model.show ? 'block' : 'none'}};">Content</div>
```

Use `<if>` when content should be completely excluded.

### Performance Implications

Conditional rendering improves performance:
- Reduces PDF processing time
- Decreases memory usage
- Smaller PDF file size
- Faster document generation

For large conditional sections with many items, the performance benefit can be significant.

---

## Examples

### 1. Simple Boolean Test

Show content based on boolean flag:

```html
<if data-test="{{model.showDisclaimer}}">
    <div style="padding: 10pt; background-color: #fff3cd; border: 1pt solid #ffc107;">
        <strong>Disclaimer:</strong> This information is provided for reference only.
    </div>
</if>
```

### 2. Numeric Comparison

Show content based on age verification:

```html
<if data-test="{{model.age >= 21}}">
    <div style="padding: 15pt;">
        <h3>Age-Restricted Content</h3>
        <p>This section is only visible to users 21 and older.</p>
    </div>
</if>
```

### 3. String Equality

Display content based on user role:

```html
<if data-test="{{model.userRole == 'Admin'}}">
    <div style="background-color: #f8d7da; padding: 10pt; margin: 10pt 0;">
        <strong>Administrator View</strong><br/>
        Internal ID: {{model.internalId}}<br/>
        Created: {{model.createdDate}}
    </div>
</if>
```

### 4. Logical AND

Require multiple conditions:

```html
<if data-test="{{model.isPremiumMember && model.orderTotal > 100}}">
    <div style="background-color: #d4edda; padding: 15pt; border: 2pt solid #28a745;">
        <strong>Premium Discount Applied!</strong><br/>
        You saved ${{model.premiumDiscount}} on this order.
    </div>
</if>
```

### 5. Logical OR

Show content if any condition is true:

```html
<if data-test="{{model.isUrgent || model.isHighPriority}}">
    <div style="background-color: #fff3cd; padding: 10pt; border-left: 5pt solid #ffc107;">
        <strong>⚠ Attention Required</strong><br/>
        This item requires immediate attention.
    </div>
</if>
```

### 6. Negation

Show content when condition is false:

```html
<if data-test="{{!model.isDisabled}}">
    <div>
        <h3>{{model.featureName}}</h3>
        <p>{{model.featureDescription}}</p>
    </div>
</if>
```

### 7. Null Check

Verify value exists before displaying:

```html
<if data-test="{{model.specialInstructions != null}}">
    <div style="margin: 15pt 0; padding: 10pt; border: 1pt dashed #666;">
        <strong>Special Instructions:</strong><br/>
        {{model.specialInstructions}}
    </div>
</if>
```

### 8. Collection Length Check

Show section only if items exist:

```html
<if data-test="{{model.orderItems.length > 0}}">
    <h2>Order Items</h2>
    <table style="width: 100%;">
        <template data-bind="{{model.orderItems}}">
            <tr>
                <td>{{.productName}}</td>
                <td>${{.price}}</td>
            </tr>
        </template>
    </table>
</if>
```

### 9. Status-Based Display

Different sections based on order status:

```html
<if data-test="{{model.orderStatus == 'pending'}}">
    <div style="color: orange; font-weight: bold; padding: 10pt; background-color: #fff3cd;">
        ⧗ Order Pending - Awaiting processing
    </div>
</if>

<if data-test="{{model.orderStatus == 'shipped'}}">
    <div style="color: green; font-weight: bold; padding: 10pt; background-color: #d4edda;">
        ✓ Order Shipped - Tracking: {{model.trackingNumber}}
    </div>
</if>

<if data-test="{{model.orderStatus == 'delivered'}}">
    <div style="color: blue; font-weight: bold; padding: 10pt; background-color: #d1ecf1;">
        ✓ Order Delivered - {{model.deliveryDate}}
    </div>
</if>
```

### 10. Complex Conditional Logic

Multiple conditions with parentheses:

```html
<if data-test="{{(model.isVIP || model.orderTotal > 500) && model.isActive}}">
    <div style="background-color: #e7f3ff; padding: 15pt; border: 2pt solid #0066cc;">
        <h3 style="margin: 0 0 10pt 0;">VIP Benefits Applied</h3>
        <ul style="margin: 0;">
            <li>Free expedited shipping</li>
            <li>Priority customer support</li>
            <li>Extended return period (60 days)</li>
        </ul>
    </div>
</if>
```

### 11. Date-Based Conditional

Show content based on date comparison:

```html
<if data-test="{{model.expirationDate != null && model.daysUntilExpiration < 30}}">
    <div style="background-color: #fff3cd; padding: 12pt; margin: 10pt 0; border-left: 4pt solid #ffc107;">
        <strong>⚠ Expiration Warning</strong><br/>
        This item expires in {{model.daysUntilExpiration}} days on {{model.expirationDate}}.
    </div>
</if>
```

### 12. Permission-Based Sections

Multiple permission levels:

```html
<!-- Public content (always shown) -->
<div>
    <h2>Public Information</h2>
    <p>{{model.publicDescription}}</p>
</div>

<!-- Member-only content -->
<if data-test="{{model.accessLevel >= 1}}">
    <div style="page-break-before: always;">
        <h2>Member Content</h2>
        <p>{{model.memberContent}}</p>
    </div>
</if>

<!-- Premium member content -->
<if data-test="{{model.accessLevel >= 2}}">
    <div style="page-break-before: always;">
        <h2>Premium Content</h2>
        <p>{{model.premiumContent}}</p>
    </div>
</if>

<!-- Admin content -->
<if data-test="{{model.accessLevel >= 99}}">
    <div style="page-break-before: always; background-color: #f0f0f0;">
        <h2>Administrative Data</h2>
        <p>{{model.adminContent}}</p>
    </div>
</if>
```

### 13. Invoice Payment Status

Conditional payment messages:

```html
<div style="margin-top: 30pt;">
    <if data-test="{{model.isPaid}}">
        <div style="background-color: #d4edda; padding: 15pt; border: 2pt solid #28a745;">
            <h3 style="color: #28a745; margin: 0;">✓ PAID</h3>
            <p style="margin: 5pt 0 0 0;">
                Payment received on {{model.paymentDate}}<br/>
                Payment method: {{model.paymentMethod}}
            </p>
        </div>
    </if>

    <if data-test="{{!model.isPaid && !model.isOverdue}}">
        <div style="background-color: #fff3cd; padding: 15pt; border: 2pt solid #ffc107;">
            <h3 style="color: #856404; margin: 0;">⧗ PAYMENT DUE</h3>
            <p style="margin: 5pt 0 0 0;">
                Due date: {{model.dueDate}}<br/>
                Amount due: ${{model.amountDue}}
            </p>
        </div>
    </if>

    <if data-test="{{!model.isPaid && model.isOverdue}}">
        <div style="background-color: #f8d7da; padding: 15pt; border: 2pt solid #dc3545;">
            <h3 style="color: #dc3545; margin: 0;">⚠ OVERDUE</h3>
            <p style="margin: 5pt 0 0 0;">
                This invoice is {{model.daysOverdue}} days overdue.<br/>
                Amount due: ${{model.amountDue}}<br/>
                Late fees may apply.
            </p>
        </div>
    </if>
</div>
```

### 14. Nested Conditionals

Complex nested logic:

```html
<if data-test="{{model.hasInsurance}}">
    <div style="margin: 15pt 0;">
        <h3>Insurance Coverage</h3>
        <p>Policy Number: {{model.insurancePolicyNumber}}</p>

        <if data-test="{{model.insuranceType == 'comprehensive'}}">
            <div style="padding: 10pt; background-color: #d4edda;">
                <strong>Comprehensive Coverage</strong><br/>
                Full coverage including collision and liability.
            </div>
        </if>

        <if data-test="{{model.insuranceType == 'basic'}}">
            <div style="padding: 10pt; background-color: #fff3cd;">
                <strong>Basic Coverage</strong><br/>
                Standard liability coverage only.
            </div>
        </if>
    </div>
</if>
```

### 15. Conditional Tables

Show entire table based on data presence:

```html
<if data-test="{{model.lineItems != null && model.lineItems.length > 0}}">
    <h2>Line Items</h2>
    <table style="width: 100%; border-collapse: collapse;">
        <thead>
            <tr style="background-color: #f0f0f0;">
                <th style="padding: 8pt; border: 1pt solid black;">Description</th>
                <th style="padding: 8pt; border: 1pt solid black; text-align: right;">Qty</th>
                <th style="padding: 8pt; border: 1pt solid black; text-align: right;">Price</th>
                <th style="padding: 8pt; border: 1pt solid black; text-align: right;">Total</th>
            </tr>
        </thead>
        <tbody>
            <template data-bind="{{model.lineItems}}">
                <tr>
                    <td style="padding: 6pt; border: 1pt solid #ddd;">{{.description}}</td>
                    <td style="padding: 6pt; border: 1pt solid #ddd; text-align: right;">{{.quantity}}</td>
                    <td style="padding: 6pt; border: 1pt solid #ddd; text-align: right;">${{.unitPrice}}</td>
                    <td style="padding: 6pt; border: 1pt solid #ddd; text-align: right; font-weight: bold;">
                        ${{.lineTotal}}
                    </td>
                </tr>
            </template>
        </tbody>
    </table>
</if>
```

### 16. Multi-Language Support

Language-specific content:

```html
<if data-test="{{model.language == 'en'}}">
    <div>
        <h1>Welcome</h1>
        <p>Thank you for choosing our services.</p>
    </div>
</if>

<if data-test="{{model.language == 'es'}}">
    <div>
        <h1>Bienvenido</h1>
        <p>Gracias por elegir nuestros servicios.</p>
    </div>
</if>

<if data-test="{{model.language == 'fr'}}">
    <div>
        <h1>Bienvenue</h1>
        <p>Merci d'avoir choisi nos services.</p>
    </div>
</if>
```

### 17. Conditional Styling and Formatting

Show warnings based on threshold:

```html
<div>
    <h3>Account Balance: ${{model.balance}}</h3>

    <if data-test="{{model.balance < 0}}">
        <div style="color: red; font-weight: bold; padding: 10pt; background-color: #f8d7da;">
            ⚠ OVERDRAWN - Immediate action required
        </div>
    </if>

    <if data-test="{{model.balance >= 0 && model.balance < 100}}">
        <div style="color: orange; font-weight: bold; padding: 10pt; background-color: #fff3cd;">
            ⚠ Low balance warning
        </div>
    </if>

    <if data-test="{{model.balance >= 100}}">
        <div style="color: green; padding: 10pt; background-color: #d4edda;">
            ✓ Account in good standing
        </div>
    </if>
</div>
```

### 18. Configuration-Driven Sections

Show sections based on configuration:

```html
<if data-test="{{model.config.showExecutiveSummary}}">
    <div style="page-break-after: always;">
        <h1>Executive Summary</h1>
        <p>{{model.executiveSummary}}</p>
    </div>
</if>

<if data-test="{{model.config.showDetailedAnalysis}}">
    <div style="page-break-after: always;">
        <h1>Detailed Analysis</h1>
        <p>{{model.detailedAnalysis}}</p>
    </div>
</if>

<if data-test="{{model.config.showCharts}}">
    <div style="page-break-after: always;">
        <h1>Charts and Graphs</h1>
        <!-- Chart content -->
    </div>
</if>

<if data-test="{{model.config.showAppendix}}">
    <div style="page-break-after: always;">
        <h1>Appendix</h1>
        <p>{{model.appendixContent}}</p>
    </div>
</if>
```

### 19. Dynamic Document Sections

Build document structure from data:

```html
<template data-bind="{{model.sections}}">
    <if data-test="{{.enabled}}">
        <div style="margin-bottom: 30pt;">
            <h2>{{.title}}</h2>

            <if data-test="{{.hasIntroduction}}">
                <p style="font-style: italic;">{{.introduction}}</p>
            </if>

            <div>{{.content}}</div>

            <if data-test="{{.hasFootnote}}">
                <div style="font-size: 9pt; color: #666; margin-top: 10pt;">
                    {{.footnote}}
                </div>
            </if>
        </div>
    </if>
</template>
```

### 20. Comprehensive Business Logic

Complex real-world scenario:

```html
<div class="document">
    <!-- Header always shown -->
    <div style="background-color: #336699; color: white; padding: 20pt;">
        <h1 style="margin: 0;">{{model.documentTitle}}</h1>
        <div>Date: {{model.generatedDate}}</div>
    </div>

    <!-- Main content -->
    <div style="padding: 20pt;">
        <!-- Standard content -->
        <p>{{model.standardContent}}</p>

        <!-- VIP customer benefits -->
        <if data-test="{{model.customerType == 'VIP' && model.accountStatus == 'active'}}">
            <div style="background-color: #fff9e6; padding: 15pt; margin: 15pt 0; border: 2pt solid #ffd700;">
                <h3 style="color: #b8860b; margin: 0 0 10pt 0;">VIP Customer Benefits</h3>
                <ul style="margin: 0;">
                    <li>24/7 Priority Support</li>
                    <li>Free shipping on all orders</li>
                    <li>Exclusive early access to new products</li>
                </ul>
            </div>
        </if>

        <!-- Account warnings -->
        <if data-test="{{model.accountBalance < 0 || model.daysOverdue > 30}}">
            <div style="background-color: #f8d7da; padding: 15pt; margin: 15pt 0; border: 2pt solid #dc3545;">
                <h3 style="color: #dc3545; margin: 0 0 10pt 0;">Account Action Required</h3>

                <if data-test="{{model.accountBalance < 0}}">
                    <p>Your account balance is negative: ${{model.accountBalance}}</p>
                </if>

                <if data-test="{{model.daysOverdue > 30}}">
                    <p>You have invoices that are {{model.daysOverdue}} days overdue.</p>
                </if>

                <p style="font-weight: bold;">Please contact accounting immediately.</p>
            </div>
        </if>

        <!-- Regional-specific content -->
        <if data-test="{{model.region == 'EU'}}">
            <div style="margin: 15pt 0; padding: 10pt; border: 1pt solid #0066cc; background-color: #e7f3ff;">
                <strong>EU Customers:</strong> All prices include VAT as required by EU regulations.
            </div>
        </if>

        <!-- Promotional content -->
        <if data-test="{{model.hasActivePromotion && model.promotionEligible}}">
            <div style="background-color: #d4edda; padding: 15pt; margin: 15pt 0;">
                <h3 style="color: #28a745; margin: 0 0 10pt 0;">Special Offer Available!</h3>
                <p>{{model.promotionDescription}}</p>
                <p style="font-weight: bold;">Offer expires: {{model.promotionExpiry}}</p>
            </div>
        </if>
    </div>

    <!-- Footer -->
    <div style="margin-top: 30pt; padding: 15pt; border-top: 2pt solid #336699;">
        <if data-test="{{model.isConfidential}}">
            <div style="color: red; font-weight: bold; text-align: center; margin-bottom: 10pt;">
                CONFIDENTIAL - DO NOT DISTRIBUTE
            </div>
        </if>

        <if data-test="{{model.isDraft}}">
            <div style="color: #666; font-style: italic; text-align: center; margin-bottom: 10pt;">
                DRAFT - Not for Distribution
            </div>
        </if>

        <div style="text-align: center; font-size: 9pt; color: #666;">
            Generated: {{model.generatedTimestamp}} | Document ID: {{model.documentId}}
        </div>
    </div>
</div>
```

---

## See Also

- [if element](/reference/htmltags/if.html) - Conditional rendering element
- [data-template attribute](/reference/htmlattributes/data-template.html) - Inline template content
- [template element](/reference/htmltags/template.html) - Template for repeating content
- [Data Binding](/reference/binding/) - Complete data binding guide
- [Expressions](/reference/expressions/) - Expression syntax reference
- [hidden attribute](/reference/attributes/hidden.html) - Alternative visibility control

---
