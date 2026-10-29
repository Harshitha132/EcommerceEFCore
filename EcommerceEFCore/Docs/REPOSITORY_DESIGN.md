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


## CustomerRepository

**Purpose:** Manages customer records using Entity Framework Core.

### CRUD Methods

- `AddAsync(Customer customer)` — inserts a customer and returns the generated ID.
- `GetAllAsync()` — retrieves all customers.
- `GetByIdAsync(int id)` — retrieves a customer by ID.
- `UpdateAsync(int id, string firstName, string lastName, string email, string phoneNumber)` — updates customer details.
- `DeleteAsync(int id)` — deletes a customer if no related records prevent deletion.

### Exception Handling

- SQL Server errors 2601 and 2627: handles duplicate email or unique-constraint violations.
- SQL Server error 547: handles foreign-key constraint violations when related records exist.
- Other exceptions propagate to the calling layer.

### Database Context

Each repository operation creates and disposes its own `AppDbContext` instance.

### Entity Relationships

Customers can have orders and an associated shopping cart. Database constraints protect related records according to the configured delete behaviors.


## CartRepository

**Purpose:** Manages shopping carts and retrieves their associated customer and cart-item details.

### CRUD Methods

- `AddAsync(Cart cart)` — creates a cart and returns its generated ID.
- `GetAllAsync()` — retrieves carts with their associated customers.
- `GetByIdAsync(int id)` — retrieves a cart, its customer, cart items, and associated products.
- `UpdateAsync(int id, int customerId)` — updates the cart's customer association and modification timestamp.
- `DeleteAsync(int id)` — deletes a cart if database constraints allow it.

### Exception Handling

- SQL Server errors 2601 and 2627: handles duplicate-cart or unique-constraint violations.
- SQL Server error 547: handles foreign-key constraint violations.
- Other exceptions propagate to the calling layer.

### Entity Relationships

- Each cart belongs to one customer.
- A customer can have at most one cart.
- A cart can contain multiple cart items.
- Cart items reference products.

### Database Context

Each repository operation creates and disposes its own `AppDbContext` instance.


## CartItemRepository

**Purpose:** Manages individual products and their quantities within shopping carts.

### CRUD Methods

- `AddAsync(CartItem cartItem)` — adds a product to a cart and returns the generated ID.
- `GetAllAsync()` — retrieves cart items with their associated carts and products.
- `GetByIdAsync(int id)` — retrieves one cart item with its cart and product details.
- `UpdateAsync(int id, int quantity)` — updates the quantity of an existing cart item.
- `DeleteAsync(int id)` — removes a cart item if it exists.

### Exception Handling and Validation

- SQL Server errors 2601 and 2627: handles duplicate cart-product combinations.
- SQL Server error 547: handles foreign-key or check-constraint violations.
- Quantity must be greater than zero.
- Other exceptions propagate to the calling layer.

### Entity Relationships

- Each cart item belongs to one cart.
- Each cart item references one product.
- A cart cannot contain the same product more than once because of the unique index on `(CartId, ProductId)`.

### Database Context

Each repository operation creates and disposes its own `AppDbContext` instance.


## OrderRepository

**Purpose:** Manages customer orders and retrieves related customer, order-item, product, and payment information.

### CRUD Methods

- `AddAsync(Order order)` — inserts an order and returns its generated ID.
- `GetAllAsync()` — retrieves orders with their associated customers.
- `GetByIdAsync(int id)` — retrieves an order with its customer, order items, products, and payment.
- `UpdateAsync(int id, string orderStatus, decimal totalAmount)` — updates an order's status and total amount.
- `DeleteAsync(int id)` — deletes an order if database constraints permit it.

### Exception Handling and Validation

- SQL Server error 547: handles foreign-key and check-constraint violations.
- Negative total amounts are rejected before saving.
- Other exceptions propagate to the calling layer.

### Entity Relationships

- Each order belongs to a customer.
- An order can contain multiple order items.
- An order can have one associated payment.
- Foreign-key restrictions protect related customer and payment records.

### Database Context

Each repository operation creates and disposes its own `AppDbContext` instance.
## OrderRepository

**Purpose:** Manages customer orders and retrieves related customer, order-item, product, and payment information.

### CRUD Methods

- `AddAsync(Order order)` — inserts an order and returns its generated ID.
- `GetAllAsync()` — retrieves orders with their associated customers.
- `GetByIdAsync(int id)` — retrieves an order with its customer, order items, products, and payment.
- `UpdateAsync(int id, string orderStatus, decimal totalAmount)` — updates an order's status and total amount.
- `DeleteAsync(int id)` — deletes an order if database constraints permit it.

### Exception Handling and Validation

- SQL Server error 547: handles foreign-key and check-constraint violations.
- Negative total amounts are rejected before saving.
- Other exceptions propagate to the calling layer.

### Entity Relationships

- Each order belongs to a customer.
- An order can contain multiple order items.
- An order can have one associated payment.
- Foreign-key restrictions protect related customer and payment records.

### Database Context

Each repository operation creates and disposes its own `AppDbContext` instance.
