# Smart Inventory Management System

A comprehensive, production-ready **Smart Inventory Management System** built with **C#**, **ASP.NET Core 8**, **Clean Architecture**, and **MS SQL Server**.

## 🏗️ Architecture

This solution follows **Clean Architecture** principles with clear separation of concerns:

```
SmartInventory/
├── SmartInventory.Domain/          # Domain Layer (Entities, Interfaces, Enums)
├── SmartInventory.Application/      # Application Layer (CQRS, DTOs, Validators, Mappings)
├── SmartInventory.Infrastructure/  # Infrastructure Layer (DbContext, Repositories, Services)
└── SmartInventory.WebApi/           # API Layer (Controllers, Middleware, Configuration)
```

## ✨ Features

### Core Modules
- ✅ **Product Management** - Full CRUD operations with filtering, sorting, and pagination
- ✅ **Category Management** - CRUD operations for product categories
- ✅ **Supplier Management** - CRUD operations for suppliers
- ✅ **Warehouse Management** - CRUD operations for warehouses
- ✅ **Inventory Transactions** - Stock In/Out operations with automatic validation
- ✅ **Low Stock Alert System** - Background job (Hangfire) that checks and alerts on low stock
- ✅ **Dashboard Analytics** - Comprehensive dashboard with statistics and charts data
- ✅ **Activity Logging** - Tracks important system actions
- ✅ **Authentication & Authorization** - JWT-based authentication with role-based access control
  - **Admin Role**: Full access to all operations
  - **Employee Role**: Limited access (no delete operations)

### Technical Features
- ✅ **CQRS Pattern** - Using MediatR for command/query separation
- ✅ **Repository Pattern** - Generic repository with Unit of Work
- ✅ **FluentValidation** - Centralized validation for all DTOs
- ✅ **AutoMapper** - Automatic object mapping
- ✅ **Global Exception Handling** - Centralized error handling middleware
- ✅ **Serilog Logging** - Console and file logging
- ✅ **Hangfire** - Background job processing
- ✅ **Swagger/OpenAPI** - Full API documentation with JWT support
- ✅ **Entity Framework Core** - Code-first approach with migrations

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB, SQL Server Express, or SQL Server)
- Visual Studio 2022 or VS Code / Rider

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd proje
   ```

2. **Update Connection String**
   
   Open `src/SmartInventory.WebApi/appsettings.json` and update the connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=SmartInventoryDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
   }
   ```

3. **Restore NuGet Packages**
   ```bash
   dotnet restore
   ```

4. **Create Database**
   
   The database will be created automatically on first run using `EnsureCreated()`. For production, use migrations:
   ```bash
   cd src/SmartInventory.Infrastructure
   dotnet ef migrations add InitialCreate --startup-project ../SmartInventory.WebApi
   dotnet ef database update --startup-project ../SmartInventory.WebApi
   ```

5. **Run the Application**
   ```bash
   cd src/SmartInventory.WebApi
   dotnet run
   ```

6. **Access the Application**
   - **Swagger UI**: `https://localhost:5001` or `http://localhost:5000`
   - **Hangfire Dashboard**: `https://localhost:5001/hangfire`

## 📚 API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and get JWT token

### Products
- `GET /api/products` - Get all products (with filtering, sorting, pagination)
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create product (Admin/Employee)
- `PUT /api/products/{id}` - Update product (Admin/Employee)
- `DELETE /api/products/{id}` - Delete product (Admin only)

### Categories
- `GET /api/categories` - Get all categories
- `POST /api/categories` - Create category (Admin/Employee)
- `PUT /api/categories/{id}` - Update category (Admin/Employee)
- `DELETE /api/categories/{id}` - Delete category (Admin only)

### Suppliers
- `GET /api/suppliers` - Get all suppliers
- `POST /api/suppliers` - Create supplier (Admin/Employee)
- `PUT /api/suppliers/{id}` - Update supplier (Admin/Employee)
- `DELETE /api/suppliers/{id}` - Delete supplier (Admin only)

### Warehouses
- `GET /api/warehouses` - Get all warehouses
- `POST /api/warehouses` - Create warehouse (Admin/Employee)
- `PUT /api/warehouses/{id}` - Update warehouse (Admin/Employee)
- `DELETE /api/warehouses/{id}` - Delete warehouse (Admin only)

### Transactions
- `GET /api/transactions` - Get all transactions (with pagination)
- `POST /api/transactions` - Create stock in/out transaction (Admin/Employee)

### Dashboard
- `GET /api/dashboard` - Get dashboard analytics data (Admin/Employee)

## 🔐 Authentication

### Register a User
```json
POST /api/auth/register
{
  "username": "admin",
  "email": "admin@example.com",
  "password": "password123",
  "role": "Admin"
}
```

### Login
```json
POST /api/auth/login
{
  "username": "admin",
  "password": "password123"
}
```

Response includes a JWT token. Use this token in the `Authorization` header:
```
Authorization: Bearer <your-token>
```

## 📊 Dashboard Data Structure

The dashboard endpoint returns:
```json
{
  "totalProducts": 100,
  "totalStockValue": 50000,
  "topLowStockProducts": [...],
  "monthlyTransactions": [...],
  "categoryDistribution": [...]
}
```

## 🔄 Background Jobs

### Low Stock Alert Job

A Hangfire recurring job runs every minute (configurable) to:
1. Check all products for low stock (Quantity < MinStockLevel)
2. Create low stock alerts in the database
3. Send email notifications to suppliers (dummy implementation)

To change the schedule, modify `Program.cs`:
```csharp
RecurringJob.AddOrUpdate<LowStockAlertJob>(
    "low-stock-alert",
    job => job.CheckLowStockProducts(),
    Cron.Hourly); // Change to your preferred schedule
```

## 📝 Logging

Logs are written to:
- **Console**: Real-time logging
- **File**: `logs/log.txt` (daily rolling)

Log levels are configurable in `appsettings.json`.

## 🧪 Testing the API

### Using Swagger UI

1. Navigate to the Swagger UI (root URL)
2. Click "Authorize" button
3. Enter: `Bearer <your-jwt-token>`
4. Test all endpoints

### Using Postman/curl

1. Register/Login to get a token
2. Include the token in requests:
   ```bash
   curl -H "Authorization: Bearer <token>" https://localhost:5001/api/products
   ```

## 🗄️ Database Schema

### Main Tables
- **Products** - Product information with stock levels
- **Categories** - Product categories
- **Suppliers** - Supplier information
- **Warehouses** - Warehouse locations
- **InventoryTransactions** - Stock movement history
- **Users** - System users
- **ActivityLogs** - System activity tracking
- **LowStockAlerts** - Low stock alert records

## 🔧 Configuration

### JWT Settings
Configure in `appsettings.json`:
```json
"Jwt": {
  "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
  "Issuer": "SmartInventory",
  "Audience": "SmartInventory"
}
```

### Hangfire Dashboard
Access at `/hangfire`. In production, implement proper authorization in `HangfireAuthorizationFilter`.

## 📦 Project Structure

```
src/
├── SmartInventory.Domain/
│   ├── Entities/          # Domain entities
│   ├── Enums/            # Domain enums
│   └── Interfaces/       # Repository and service interfaces
│
├── SmartInventory.Application/
│   ├── DTOs/             # Data transfer objects
│   ├── Features/         # CQRS commands and queries
│   ├── Mappings/         # AutoMapper profiles
│   └── Validators/       # FluentValidation validators
│
├── SmartInventory.Infrastructure/
│   ├── Data/             # DbContext and configurations
│   ├── Repositories/     # Repository implementations
│   ├── Services/         # Infrastructure services (Email, etc.)
│   └── Jobs/             # Hangfire background jobs
│
└── SmartInventory.WebApi/
    ├── Controllers/      # API controllers
    ├── Middleware/       # Custom middleware
    └── Program.cs        # Application startup
```

## 🛠️ Development

### Adding a New Feature

1. **Domain Layer**: Add entity in `Domain/Entities/`
2. **Application Layer**: 
   - Create DTOs in `Application/DTOs/`
   - Create Commands/Queries in `Application/Features/`
   - Add validators in `Application/Validators/`
   - Update mapping profile
3. **Infrastructure Layer**: Update DbContext if needed
4. **API Layer**: Add controller in `WebApi/Controllers/`

### Running Migrations

```bash
cd src/SmartInventory.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../SmartInventory.WebApi
dotnet ef database update --startup-project ../SmartInventory.WebApi
```

## ⚠️ Important Notes

1. **Password Hashing**: Currently using SHA256. For production, use bcrypt or Argon2.
2. **JWT Secret**: Change the JWT key in production!
3. **Email Service**: Currently a dummy implementation. Replace with real email service.
4. **Hangfire Authorization**: Implement proper authorization for Hangfire dashboard in production.
5. **Database**: Uses `EnsureCreated()` for development. Use migrations in production.

## 📄 License

This project is provided as-is for educational and commercial use.

## 🤝 Contributing

This is a complete, production-ready solution. Feel free to extend it with additional features as needed.

## 📞 Support

For issues or questions, please refer to the code documentation or create an issue in the repository.

---

**Built with ❤️ using Clean Architecture and ASP.NET Core 8**

