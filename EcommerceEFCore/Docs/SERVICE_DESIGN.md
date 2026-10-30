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
