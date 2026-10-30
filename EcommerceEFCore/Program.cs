using ECommerceEFCore.Menus;
using ECommerceEFCore.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// ========================================
// 1. Register Services
// ========================================

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<CartItemService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderItemService>();
builder.Services.AddScoped<PaymentService>();

// ========================================
// 2. Register Menus
// ========================================

builder.Services.AddScoped<CategoryMenu>();
builder.Services.AddScoped<ProductMenu>();
builder.Services.AddScoped<CustomerMenu>();

builder.Services.AddScoped<CartItemMenu>();
builder.Services.AddScoped<CartMenu>();

builder.Services.AddScoped<OrderItemMenu>();
builder.Services.AddScoped<OrderMenu>();

builder.Services.AddScoped<PaymentMenu>();

builder.Services.AddScoped<MainMenu>();

// ========================================
// 3. Build Host
// ========================================

using var host = builder.Build();

// ========================================
// 4. Create Dependency Injection Scope
// ========================================

using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

// ========================================
// 5. Resolve Main Menu
// ========================================

var mainMenu = services.GetRequiredService<MainMenu>();

// ========================================
// 6. Run Application
// ========================================

await mainMenu.ShowAsync();