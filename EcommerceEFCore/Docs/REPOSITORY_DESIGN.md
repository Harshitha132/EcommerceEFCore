# Repository Design

## CategoryRepository

**Purpose:** Handles database operations for the `Category` entity using Entity Framework Core.

### CRUD Methods

- `AddAsync(Category category)` — inserts a category and returns its generated ID.
- `GetAllAsync()` — retrieves all categories.
- `GetByIdAsync(int id)` — retrieves a category by ID.
- `UpdateAsync(int id, string categoryName, string? description)` — updates an existing category.
- `DeleteAsync(int id)` — deletes a category if it exists.

### Exception Handling

- SQL Server error 2601 or 2627: reports a duplicate category name or unique-constraint violation.
- SQL Server error 547: reports that the category cannot be deleted because related products exist.
- Other exceptions propagate to the calling layer for handling.

### Database Context

Each repository operation creates and disposes its own `AppDbContext` instance.


## ProductRepository

**Purpose:** Handles CRUD operations for the `Product` entity using Entity Framework Core and SQL Server.

### CRUD Methods

- `AddAsync(Product product)` — inserts a new product and returns its generated `ProductId`.
- `GetAllAsync()` — retrieves all products, including their associated category details.
- `GetByIdAsync(int id)` — retrieves a product by ID, including its category.
- `UpdateAsync(int id, string productName, string? description, decimal price, int stockQuantity, int categoryId)` — updates the product's details and category.
- `DeleteAsync(int id)` — deletes a product if it exists.

### Entity Relationship

- Each product belongs to a category through `CategoryId`.
- The repository uses `Include(p => p.Category)` when retrieving products to load their category details.
- Products referenced by cart items or order items cannot be deleted because of the configured foreign-key restrictions.

### Exception Handling

- **SQL Server errors 2601 and 2627:** Handles unique-index or constraint violations during insertion.
- **SQL Server error 547:** Handles foreign-key or check-constraint violations, such as an invalid category or a product referenced by cart/order items.
- Other exceptions propagate to the calling layer for appropriate handling.

### Database Context

Each repository operation creates and disposes its own `AppDbContext` instance.

### Validation

The database enforces constraints that prevent negative prices and stock quantities. Friendly input validation will be implemented in the service layer.
