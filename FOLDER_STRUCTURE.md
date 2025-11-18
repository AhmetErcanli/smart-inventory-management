# Smart Inventory Management System - Folder Structure

```
proje/
├── SmartInventory.sln
├── README.md
├── FOLDER_STRUCTURE.md
│
└── src/
    ├── SmartInventory.Domain/
    │   ├── SmartInventory.Domain.csproj
    │   ├── Entities/
    │   │   ├── BaseEntity.cs
    │   │   ├── Product.cs
    │   │   ├── Category.cs
    │   │   ├── Supplier.cs
    │   │   ├── Warehouse.cs
    │   │   ├── InventoryTransaction.cs
    │   │   ├── User.cs
    │   │   ├── ActivityLog.cs
    │   │   └── LowStockAlert.cs
    │   ├── Enums/
    │   │   ├── TransactionType.cs
    │   │   └── Role.cs
    │   └── Interfaces/
    │       ├── IRepository.cs
    │       ├── IUnitOfWork.cs
    │       └── IEmailService.cs
    │
    ├── SmartInventory.Application/
    │   ├── SmartInventory.Application.csproj
    │   ├── DependencyInjection.cs
    │   ├── DTOs/
    │   │   ├── ProductDto.cs
    │   │   ├── CreateProductDto.cs
    │   │   ├── UpdateProductDto.cs
    │   │   ├── CategoryDto.cs
    │   │   ├── CreateCategoryDto.cs
    │   │   ├── UpdateCategoryDto.cs
    │   │   ├── SupplierDto.cs
    │   │   ├── CreateSupplierDto.cs
    │   │   ├── UpdateSupplierDto.cs
    │   │   ├── WarehouseDto.cs
    │   │   ├── CreateWarehouseDto.cs
    │   │   ├── UpdateWarehouseDto.cs
    │   │   ├── InventoryTransactionDto.cs
    │   │   ├── CreateTransactionDto.cs
    │   │   ├── LoginDto.cs
    │   │   ├── RegisterDto.cs
    │   │   ├── AuthResponseDto.cs
    │   │   ├── DashboardDto.cs
    │   │   ├── PagedResultDto.cs
    │   │   └── ApiResponse.cs
    │   ├── Features/
    │   │   ├── Products/
    │   │   │   ├── Commands/
    │   │   │   │   ├── CreateProduct/
    │   │   │   │   │   ├── CreateProductCommand.cs
    │   │   │   │   │   └── CreateProductCommandHandler.cs
    │   │   │   │   ├── UpdateProduct/
    │   │   │   │   │   ├── UpdateProductCommand.cs
    │   │   │   │   │   └── UpdateProductCommandHandler.cs
    │   │   │   │   └── DeleteProduct/
    │   │   │   │       ├── DeleteProductCommand.cs
    │   │   │   │       └── DeleteProductCommandHandler.cs
    │   │   │   └── Queries/
    │   │   │       ├── GetProductById/
    │   │   │       │   ├── GetProductByIdQuery.cs
    │   │   │       │   └── GetProductByIdQueryHandler.cs
    │   │   │       └── GetProducts/
    │   │   │           ├── GetProductsQuery.cs
    │   │   │           └── GetProductsQueryHandler.cs
    │   │   ├── Categories/
    │   │   │   ├── Commands/
    │   │   │   │   ├── CreateCategory/
    │   │   │   │   ├── UpdateCategory/
    │   │   │   │   └── DeleteCategory/
    │   │   │   └── Queries/
    │   │   │       └── GetAllCategories/
    │   │   ├── Suppliers/
    │   │   │   ├── Commands/
    │   │   │   │   ├── CreateSupplier/
    │   │   │   │   ├── UpdateSupplier/
    │   │   │   │   └── DeleteSupplier/
    │   │   │   └── Queries/
    │   │   │       └── GetAllSuppliers/
    │   │   ├── Warehouses/
    │   │   │   ├── Commands/
    │   │   │   │   ├── CreateWarehouse/
    │   │   │   │   ├── UpdateWarehouse/
    │   │   │   │   └── DeleteWarehouse/
    │   │   │   └── Queries/
    │   │   │       └── GetAllWarehouses/
    │   │   ├── Transactions/
    │   │   │   ├── Commands/
    │   │   │   │   └── CreateTransaction/
    │   │   │   └── Queries/
    │   │   │       └── GetAllTransactions/
    │   │   ├── Auth/
    │   │   │   └── Commands/
    │   │   │       ├── Login/
    │   │   │       └── Register/
    │   │   └── Dashboard/
    │   │       └── Queries/
    │   │           └── GetDashboardData/
    │   ├── Mappings/
    │   │   └── MappingProfile.cs
    │   └── Validators/
    │       ├── CreateProductDtoValidator.cs
    │       ├── UpdateProductDtoValidator.cs
    │       ├── CreateCategoryDtoValidator.cs
    │       ├── CreateSupplierDtoValidator.cs
    │       ├── CreateWarehouseDtoValidator.cs
    │       ├── CreateTransactionDtoValidator.cs
    │       ├── LoginDtoValidator.cs
    │       └── RegisterDtoValidator.cs
    │
    ├── SmartInventory.Infrastructure/
    │   ├── SmartInventory.Infrastructure.csproj
    │   ├── DependencyInjection.cs
    │   ├── Data/
    │   │   └── ApplicationDbContext.cs
    │   ├── Repositories/
    │   │   ├── Repository.cs
    │   │   └── UnitOfWork.cs
    │   ├── Services/
    │   │   └── EmailService.cs
    │   └── Jobs/
    │       └── LowStockAlertJob.cs
    │
    └── SmartInventory.WebApi/
        ├── SmartInventory.WebApi.csproj
        ├── Program.cs
        ├── appsettings.json
        ├── appsettings.Development.json
        ├── Properties/
        │   └── launchSettings.json
        ├── Controllers/
        │   ├── ProductsController.cs
        │   ├── CategoriesController.cs
        │   ├── SuppliersController.cs
        │   ├── WarehousesController.cs
        │   ├── TransactionsController.cs
        │   ├── AuthController.cs
        │   └── DashboardController.cs
        └── Middleware/
            └── ExceptionHandlingMiddleware.cs
```

## Layer Responsibilities

### Domain Layer
- **Entities**: Core business entities with no dependencies
- **Enums**: Domain-specific enumerations
- **Interfaces**: Contracts for repositories and services

### Application Layer
- **DTOs**: Data transfer objects for API communication
- **Features**: CQRS commands and queries organized by feature
- **Mappings**: AutoMapper configuration
- **Validators**: FluentValidation rules

### Infrastructure Layer
- **Data**: Entity Framework DbContext and configurations
- **Repositories**: Data access implementations
- **Services**: Infrastructure services (Email, etc.)
- **Jobs**: Background job implementations

### WebApi Layer
- **Controllers**: REST API endpoints
- **Middleware**: Custom middleware (exception handling, etc.)
- **Configuration**: Application startup and configuration

