# Inventory Management API

A REST API for managing products, suppliers, and stock levels, built with
**ASP.NET Core (.NET)** and **Entity Framework Core**. It follows a clean,
layered architecture (Controller → Service → Repository → EF Core) that keeps
business rules out of the controllers and makes the code easy to test and extend.

The domain models a small warehouse: products belong to suppliers, each product
has a **reorder level**, and the API can report which products have fallen to or
below that level so they can be re-ordered.

---

## Features

- **Product management** – full CRUD for products (name, SKU, quantity, reorder level, supplier).
- **Supplier management** – create, list, look up, and delete suppliers.
- **Low-stock reporting** – `GET /api/products/low-stock` returns products at or below their reorder level.
- **Stock movements** – a `StockMovement` entity linked to each product, ready to record stock-in / stock-out history.
- **Input validation** – DTOs use data annotations (e.g. quantity must be non-negative) so invalid requests are rejected with a clear error.
- **Layered architecture** – controllers stay thin; services return an explicit `(success, error)` result that the controller maps to the right HTTP status.
- **EF Core migrations** – the schema is versioned and reproducible.

## Tech stack

| Concern | Technology |
|---|---|
| Framework | ASP.NET Core / .NET |
| Data access | Entity Framework Core (SQL Server) |
| Architecture | Controller / Service / Repository layers + DTOs |
| API docs | Swagger / OpenAPI |

## Architecture

```
Controllers  ->  Services  ->  Repositories  ->  EF Core DbContext  ->  SQL Server
   (HTTP)        (rules)       (data access)
                 returns (success, error)
```

Separating services from repositories means the validation/business rules
(for example "an SKU must be unique" or "quantity cannot go negative") live in
one place and can be unit-tested without touching the database.

---

## Getting started

Requires the .NET SDK and a reachable SQL Server instance.

```bash
# from the repository root
cd InventoryManagement.API

# set your connection string in appsettings.json (ConnectionStrings:DefaultConnection)
dotnet restore
dotnet ef database update   # apply migrations
dotnet run
```

Swagger UI is then available at the URL printed in the console (e.g. `https://localhost:7xxx/swagger`).

---

## API reference

### Products — `/api/products`

| Method | Route | Description |
|---|---|---|
| `GET` | `/` | List all products. |
| `GET` | `/{id}` | Get one product by id. |
| `GET` | `/low-stock` | List products at or below their reorder level. |
| `POST` | `/` | Create a product. |
| `PUT` | `/{id}` | Update a product. |
| `DELETE` | `/{id}` | Delete a product. |

### Suppliers — `/api/suppliers`

| Method | Route | Description |
|---|---|---|
| `GET` | `/` | List all suppliers. |
| `GET` | `/{id}` | Get one supplier by id. |
| `POST` | `/` | Create a supplier. |
| `DELETE` | `/{id}` | Delete a supplier. |

### Example

```bash
# Create a supplier, then a product that reorders at 10 units
curl -X POST https://localhost:7000/api/suppliers \
  -H "Content-Type: application/json" \
  -d '{"name":"Acme GmbH"}'

curl -X POST https://localhost:7000/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Widget","sku":"WID-001","quantity":5,"reorderLevel":10,"supplierId":1}'

# This product (qty 5 <= reorder 10) now shows up here:
curl https://localhost:7000/api/products/low-stock
```

---

## Roadmap

These are intentionally not yet implemented — the groundwork (a `User` model and the
`JwtBearer` package) is in place for the first item:

- **JWT authentication & authorization** – protect the write endpoints so only signed-in users can modify inventory.
- **Stock-movement endpoints** – record and query stock-in/stock-out history (the `StockMovement` entity already exists).
- **Automated tests** for the service layer.
- **Pagination** on the list endpoints.
