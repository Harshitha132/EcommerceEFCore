# ECommerce — Entity Design

## Task 1: Product and Category Models

### Category

| Property | Type | Purpose |
|---|---|---|
| CategoryId | int | Primary key |
| CategoryName | string | Category name |
| Description | string? | Optional category description |
| Products | ICollection<Product> | Navigation collection |

### Product

| Property | Type | Purpose |
|---|---|---|
| ProductId | int | Primary key |
| ProductName | string | Product name |
| Description | string? | Optional product description |
| Price | decimal | Product price |
| StockQuantity | int | Available stock |
| CategoryId | int | Foreign key |
| Category | Category? | Navigation property |

## Relationship

One Category can contain many Products.

Each Product belongs to one Category.

Relationship: Category (1) → Product (many)

## Implementation Status

- [x] Category model created
- [x] Product model created
- [x] Initial entity design documented
- [ ] Build verification
- [ ] Git commit

## Next Task

Create the Customer model and document its properties and relationships.
