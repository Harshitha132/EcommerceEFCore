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
