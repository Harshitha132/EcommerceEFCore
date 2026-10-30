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


## CustomerService

**Purpose:** Implements customer-related business rules and coordinates customer operations through `CustomerRepository`.

### Methods

- `AddAsync(Customer customer)` — validates and inserts a customer.
- `GetAllAsync()` — retrieves all customers.
- `GetByIdAsync(int id)` — validates the ID and retrieves a customer.
- `UpdateAsync(...)` — validates and updates customer details.
- `DeleteAsync(int id)` — validates the ID and requests customer deletion.

### Validation Rules

- First name and last name are required and limited to 100 characters each.
- Email is required, limited to 256 characters, and receives basic format validation.
- Phone number is required and limited to 20 characters.
- Email is trimmed and converted to lowercase.
- Customer ID must be greater than zero for retrieval, update, and deletion.

### Exception Handling

- Invalid input throws `ArgumentException` or `ArgumentOutOfRangeException`.
- Database constraint violations are handled by `CustomerRepository`.
- Other exceptions propagate to the calling layer.

### Dependencies

- `CustomerRepository` handles database access.
- `CustomerService` handles customer validation and business rules.


## CartService

**Purpose:** Implements cart-related business rules and coordinates cart operations through `CartRepository`.

### Methods

- `AddAsync(Cart cart)` — validates and creates a cart for a customer.
- `GetAllAsync()` — retrieves all carts with related details.
- `GetByIdAsync(int id)` — validates the ID and retrieves a cart.
- `UpdateAsync(int id, int customerId)` — validates the IDs and updates the cart's customer association.
- `DeleteAsync(int id)` — validates the ID and requests cart deletion.

### Validation Rules

- Cart object cannot be null.
- Customer ID must be greater than zero.
- Cart ID must be greater than zero for retrieval, update, and deletion.

### Exception Handling

- Invalid input throws `ArgumentNullException` or `ArgumentOutOfRangeException`.
- Database constraint violations are handled by `CartRepository`.
- Other exceptions propagate to the calling layer.

### Dependencies

- `CartRepository` handles database access.
- `CartService` handles cart validation and business rules.

## CartItemService

**Purpose:** Implements cart-item business rules and coordinates cart-item operations through `CartItemRepository`.

### Methods

- `AddAsync(CartItem cartItem)` — validates and adds a product to a cart.
- `GetAllAsync()` — retrieves all cart items.
- `GetByIdAsync(int id)` — validates the ID and retrieves a cart item.
- `UpdateAsync(int id, int quantity)` — validates and updates the item quantity.
- `DeleteAsync(int id)` — validates the ID and requests cart-item deletion.

### Validation Rules

- Cart item cannot be null.
- Cart ID must be greater than zero.
- Product ID must be greater than zero.
- Quantity must be greater than zero.
- Cart item ID must be greater than zero for retrieval, update, and deletion.

### Exception Handling

- Invalid input throws `ArgumentNullException` or `ArgumentOutOfRangeException`.
- Database constraint violations are handled by `CartItemRepository`.
- Other exceptions propagate to the calling layer.

### Dependencies

- `CartItemRepository` handles database access.
- `CartItemService` handles cart-item validation and business rules.