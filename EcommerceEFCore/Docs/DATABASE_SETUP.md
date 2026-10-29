# ECommerceEFCore — Database Setup

## Task: AppDbContext and Entity Configuration

### 1. AppDbContext

**File:** `Data/AppDbContext.cs`

The AppDbContext class inherits from Entity Framework Core's DbContext and manages communication between the application and SQL Server.

It contains DbSet properties for eight entities:

- Category
- Product
- Customer
- Cart
- CartItem
- Order
- OrderItem
- Payment

### 2. SQL Server Configuration

The application currently uses the SQL Server instance `.\SQLEXPRESS` and database name `ECommerceDB`.

The connection uses Windows Integrated Authentication.

The database will be created through EF Core migrations after the configuration is verified.

### 3. Entity Relationships

- Category → Products: one-to-many.
- Customer → Orders: one-to-many.
- Customer → Cart: one-to-one.
- Cart → CartItems: one-to-many.
- Product → CartItems: one-to-many.
- Order → OrderItems: one-to-many.
- Product → OrderItems: one-to-many.
- Order → Payment: one-to-one.

### 4. Database Constraints

- Category names must be unique.
- Customer email addresses must be unique.
- Prices and monetary amounts cannot be negative.
- Stock quantities cannot be negative.
- Cart item and order item quantities must be greater than zero.
- A cart cannot contain duplicate entries for the same product.
- An order can have at most one payment record in the initial design.

### 5. Delete Behaviors

- Products cannot be deleted while referenced by categories or order/cart records where restrictive relationships apply.
- Orders cannot be deleted while their associated customer relationship restricts deletion.
- Cart items are deleted when their parent cart is deleted.
- Order items are deleted when their parent order is deleted.
- Cart deletion follows the configured customer-to-cart cascade behavior.

### 6. Implementation Status

- [x] AppDbContext created
- [x] Eight DbSet properties configured
- [x] SQL Server connection configured
- [x] Entity relationships configured
- [x] Monetary precision configured
- [x] Database constraints configured
- [ ] Final build verification
- [ ] Git commit
- [ ] EF Core migration generated
- [ ] SQL Server database verified

## Next Task

Configure and verify EF Core packages and database connectivity before generating the initial migration.
