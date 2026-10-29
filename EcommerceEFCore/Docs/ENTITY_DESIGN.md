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

## Task 2: Customer Model

**File:** `Models/Customer.cs`

| Property | Type | Purpose |
|---|---|---|
| CustomerId | int | Primary key |
| FirstName | string | Customer's first name |
| LastName | string | Customer's last name |
| Email | string | Customer's email address |
| PhoneNumber | string | Customer's contact number |
| CreatedAt | DateTime | Record creation timestamp |
| Orders | ICollection<Order> | Navigation collection for customer orders |
| Cart | Cart? | Optional navigation property for the customer's cart |

### Relationships

- One customer can place multiple orders.
- A customer can have one associated cart.

### Implementation Status

- [x] Customer model created
- [ ] Build verified
- [ ] Documentation updated
- [ ] Git commit



## Task 3: Cart Model

**File:** `Models/Cart.cs`

| Property | Type | Purpose |
|---|---|---|
| CartId | int | Primary key |
| CustomerId | int | Foreign key to Customer |
| CreatedAt | DateTime | Cart creation timestamp |
| UpdatedAt | DateTime | Last update timestamp |
| Customer | Customer? | Navigation property to Customer |
| CartItems | ICollection<CartItem> | Items contained in the cart |

### Relationships

- A cart belongs to a customer.
- A cart contains multiple cart items.
- The intended design allows one cart per customer. We will enforce this relationship in the EF Core configuration.

### Implementation Status

- [x] Cart model created
- [ ] Build verified
- [ ] Documentation updated
- [ ] Git commit



## Task 4: CartItem Model

**File:** `Models/CartItem.cs`

| Property | Type | Purpose |
|---|---|---|
| CartItemId | int | Primary key |
| CartId | int | Foreign key to Cart |
| ProductId | int | Foreign key to Product |
| Quantity | int | Number of units selected |
| AddedAt | DateTime | Item addition timestamp |
| Cart | Cart? | Navigation property to Cart |
| Product | Product? | Navigation property to Product |

### Relationships

- A cart can contain multiple cart items.
- Each cart item references one cart.
- Each cart item references one product.

### Implementation Status

- [x] CartItem model created
- [ ] Build verified
- [ ] Documentation updated
- [ ] Git commit


## Task 5: Order Model

**File:** `Models/Order.cs`

| Property | Type | Purpose |
|---|---|---|
| OrderId | int | Primary key |
| CustomerId | int | Foreign key to Customer |
| OrderDate | DateTime | Order creation timestamp |
| TotalAmount | decimal | Total order amount |
| OrderStatus | string | Current order status |
| Customer | Customer? | Navigation property to Customer |
| OrderItems | ICollection<OrderItem> | Items in the order |
| Payment | Payment? | Associated payment record |

### Relationships

- A customer can place multiple orders.
- An order contains multiple order items.
- An order can have an associated payment record.

### Implementation Status

- [x] Order model created
- [ ] Build verified
- [ ] Documentation updated
- [ ] Git commit


## Task 6: OrderItem Model

**File:** `Models/OrderItem.cs`

| Property | Type | Purpose |
|---|---|---|
| OrderItemId | int | Primary key |
| OrderId | int | Foreign key to Order |
| ProductId | int | Foreign key to Product |
| Quantity | int | Quantity purchased |
| UnitPrice | decimal | Unit price at purchase time |
| Subtotal | decimal | Quantity multiplied by unit price |
| Order | Order? | Navigation property to Order |
| Product | Product? | Navigation property to Product |

### Relationships

- An order contains multiple order items.
- Each order item references one order.
- Each order item references one product.

### Implementation Status

- [x] OrderItem model created
- [ ] Build verified
- [ ] Documentation updated
- [ ] Git commit


## Task 7: Payment Model

**File:** `Models/Payment.cs`

| Property | Type | Purpose |
|---|---|---|
| PaymentId | int | Primary key |
| OrderId | int | Foreign key to Order |
| Amount | decimal | Payment amount |
| PaymentMethod | string | Selected payment method |
| PaymentStatus | string | Current payment status |
| TransactionReference | string? | Optional transaction reference |
| PaymentDate | DateTime | Payment record timestamp |
| Order | Order? | Navigation property to Order |

### Relationship

Each payment is associated with an order. The intended design allows one payment record per order initially; this relationship will be configured in AppDbContext.

### Implementation Status

- [x] Payment model created
- [ ] Build verified
- [ ] Documentation updated
- [ ] Git commit



## Models Layer — Completion Summary

### Implemented Entities

1. Category
2. Product
3. Customer
4. Cart
5. CartItem
6. Order
7. OrderItem
8. Payment

### Key Relationships

- Category has many Products.
- Customer can place many Orders.
- Customer has one associated Cart.
- Cart has many CartItems.
- Each CartItem references one Product.
- Order has many OrderItems.
- Each OrderItem references one Product.
- Order has an associated Payment.

### Verification

- [x] All eight model files created.
- [ ] Project build verified.
- [x] Entity documentation maintained.
- [ ] Final Models layer commit verified.

### Notes

Entity relationships, delete behaviors, decimal precision, indexes, and database constraints will be configured in the Data layer using Entity Framework Core Fluent API.

Business validation will be implemented in the Services layer.


## Initial Database Migration

- **Migration name:** `20261029171722_InitialCreate`
- **Purpose:** Creates the initial database schema for the ECommerce application.
- **Entity tables:** Categories, Products, Customers, Carts, CartItems, Orders, OrderItems, and Payments.
- **Status:** Generated successfully; pending application to SQL Server.

### Migration Commands

Create a migration:

```powershell
dotnet ef migrations add InitialCreate
```

List migrations:

```powershell
dotnet ef migrations list
```

Apply migrations and create or update the database:

```powershell
dotnet ef database update
```

The migration files are generated by EF Core and should be maintained in source control.
