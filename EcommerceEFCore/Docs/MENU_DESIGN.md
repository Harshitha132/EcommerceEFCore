
# E-Commerce Management System – Menu Design

## 1. CategoryMenu

**Purpose:** Manages product categories through console-based CRUD operations.

### Menu Options

1. Add Category
2. View All Categories
3. View Category By ID
4. Update Category
5. Delete Category
0. Back to Main Menu

### Methods

- `ShowAsync()` – displays the category management menu.
- `AddAsync()` – accepts category details and creates a category.
- `GetAllAsync()` – retrieves and displays all categories.
- `GetByIdAsync()` – retrieves a category using its ID.
- `UpdateAsync()` – updates category name and description.
- `DeleteAsync()` – deletes a category after confirmation.

### Dependencies

- CategoryService
- ConsoleInputHelper

---

## 2. ProductMenu

**Purpose:** Manages products through console-based CRUD operations.

### Menu Options

1. Add Product
2. View All Products
3. View Product By ID
4. Update Product
5. Delete Product
0. Back to Main Menu

### Methods

- `ShowAsync()` – displays the product management menu.
- `AddAsync()` – accepts product details and creates a product.
- `GetAllAsync()` – retrieves and displays all products.
- `GetByIdAsync()` – retrieves a product using its ID.
- `UpdateAsync()` – updates product details.
- `DeleteAsync()` – deletes a product after confirmation.
- `DisplayProduct()` – displays individual product details.

### Validation Rules

- Product name is required.
- Price cannot be negative.
- Stock quantity cannot be negative.
- Category ID must be greater than zero.
- The selected category must exist in the database.

### Dependencies

- ProductService
- ConsoleInputHelper

### Database Relationship

Product belongs to Category through CategoryId (Foreign Key).


---

## 3. CustomerMenu

**Purpose:** Manages customer records through console-based CRUD operations.

### Menu Options

1. Add Customer
2. View All Customers
3. View Customer By ID
4. Update Customer
5. Delete Customer
0. Back to Main Menu

### Methods

- `ShowAsync()` – displays the customer management menu.
- `AddAsync()` – accepts customer details and creates a customer.
- `GetAllAsync()` – retrieves and displays all customers.
- `GetByIdAsync()` – retrieves a customer using their ID.
- `UpdateAsync()` – updates customer details.
- `DeleteAsync()` – deletes a customer after confirmation.
- `DisplayCustomer()` – displays individual customer details.

### Validation Rules

- First name and last name are required.
- Email is required and must pass service-level validation.
- Phone number is required.
- Customer ID must be greater than zero.

### Dependencies

- CustomerService
- ConsoleInputHelper