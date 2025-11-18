# Database Setup Guide

## Finding Your SQL Server Instance

Open SSMS and note the server name you see when connecting.

### Option 1: SQL Server Express (Most Common)

If the server name in SSMS looks like this:

  - `localhost\SQLEXPRESS`
  - `.\SQLEXPRESS`
  - `YOUR_COMPUTER_NAME\SQLEXPRESS`

Then use the following setting in the `appsettings.json` file:

```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=SmartInventoryDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
```

### Option 2: Default SQL Server Instance

If the server name in SSMS looks just like this:

  - `localhost`
  - `YOUR_COMPUTER_NAME`
  - `.`

Then use the following setting in the `appsettings.json` file:

```json
"DefaultConnection": "Server=localhost;Database=SmartInventoryDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
```

### Option 3: Named Instance (Custom Name)

If you have a custom instance name (e.g., `localhost\MYINSTANCE`):

```json
"DefaultConnection": "Server=localhost\\MYINSTANCE;Database=SmartInventoryDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
```

## Checking if SQL Server Service is Running

1.  **Via Windows Services:**

      - Press Windows + R
      - Type `services.msc` and press Enter
      - Check that the "SQL Server (SQLEXPRESS)" or "SQL Server (MSSQLSERVER)" service is in the "Running" state
      - If it is stopped, right-click and select "Start"

2.  **Via SSMS:**

      - Open SSMS
      - Enter your server name and click the "Connect" button
      - If you can connect, the service is running

## Database Will Be Created Automatically

When you run the application:

  - The `SmartInventoryDb` database is created automatically
  - All tables are created automatically
  - You don't need to do anything manually\!

## Testing

Run the application:

```powershell
cd src/SmartInventory.WebApi
dotnet run
```

If you receive an error, check the error message. Usually:

  - "Cannot open database" → Server name is incorrect
  - "Login failed" → Authentication issue
  - "Server not found" → SQL Server is not running
