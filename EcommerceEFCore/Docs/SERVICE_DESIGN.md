# Service Design

## CategoryService

**Purpose:** Implements category-related business rules and coordinates category operations through `CategoryRepository`.

### Methods

- `AddAsync(Category category)` — validates and normalizes category data before insertion.
- `GetAllAsync()` — retrieves all categories.
- `GetByIdAsync(int id)` — validates the ID and retrieves the requested category.
- `UpdateAsync(int id, string categoryName, string? description)` — validates and updates category details.
- `DeleteAsync(int id)` — validates the ID and requests deletion.

### Validation Rules

- Category name is required and cannot exceed 100 characters.
- Description cannot exceed 500 characters.
- Category IDs must be greater than zero.
- Leading and trailing whitespace is removed from text fields.

### Exception Handling

- Validation failures throw `ArgumentException` or `ArgumentOutOfRangeException`.
- Duplicate category names and foreign-key violations are translated into meaningful exceptions by `CategoryRepository`.
- Other database exceptions propagate to the calling layer.

### Dependencies

- `CategoryRepository` handles database access.
- `CategoryService` contains business validation and delegates persistence to the repository.


## ProductService

**Purpose:** Implements product-related business rules and coordinates product operations through `ProductRepository`.

### Methods

- `AddAsync(Product product)` — validates and inserts a product.
- `GetAllAsync()` — retrieves all products with category details.
- `GetByIdAsync(int id)` — validates the ID and retrieves a product.
- `UpdateAsync(...)` — validates and updates product details.
- `DeleteAsync(int id)` — validates the ID and requests product deletion.

### Validation Rules

- Product name is required and limited to 150 characters.
- Description is limited to 1000 characters.
- Price cannot be negative.
- Stock quantity cannot be negative.
- Category ID must be greater than zero.
- Product ID must be greater than zero for retrieval, update, and deletion.
- Leading and trailing whitespace is removed from text fields.

### Exception Handling

- Invalid input throws `ArgumentException` or `ArgumentOutOfRangeException`.
- Database constraint violations are translated into meaningful exceptions by `ProductRepository`.
- Other exceptions propagate to the calling layer.

### Dependencies

- `ProductRepository` handles database access.
- `ProductService` handles product validation and business rules.
