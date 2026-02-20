# Northwind Management Console

Console-based CRUD application built with **C#** and **SQL Server** using a Northwind-style database.

## Features

- **Categories (CRUD)**
  - List, create, update, delete categories
- **Products (CRUD)**
  - List, create, update, delete products
- **Orders**
  - Create orders with multiple products
  - Automatically creates Order + Order Details
  - Shows receipt (check) in console

## Architecture

- Models
- Data (Repositories)
- Services
- UI (Console Menus)
- Client (Program.cs)

## Tech Stack

- C# (.NET)
- SQL Server (LocalDB)
- ADO.NET (`Microsoft.Data.SqlClient`)
- Console Application

## Database

Required tables:
- Categories
- Products
- Orders
- Order Details

Connection string is configured in:

Example:

```csharp
Server=(localdb)\\MSSQLLocalDB;Database=Lesson1Course;Trusted_Connection=True;
