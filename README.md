# PaymentTracker

## Setup and Run

### 1. Configure the Database
```bash
docker run -e "ACCEPT_EULA=1" -e "MSSQL_SA_PASSWORD=pwd" ` -p 1433:1433 --name sqlserver ` -d mcr.microsoft.com/azure-sql-edge
```
### 2. Run the Application
Open the project in Visual Studio and press F5.
## Application Overview
- Basic CRUD for managing payments.
- Unique constraint on Title + Category.
### Technologies Used
- C# .NET 8 (MVC)
- Entity Framework Core
- SQL Server
- Bootstrap
