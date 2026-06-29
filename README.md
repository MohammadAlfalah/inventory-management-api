# Inventory Management API

A small REST API for tracking products, suppliers, and stock levels — built with ASP.NET Core (.NET 10) and EF Core.

I wanted a backend project to practice clean layering in ASP.NET Core rather than dumping everything into controllers, so I modeled a small warehouse: products belong to a supplier, each product has a reorder level, and the API can tell you which products have dropped to or below that level so you know what to restock.

The request path is straightforward:

```
Controller  ->  Service  ->  Repository  ->  EF Core (DbContext)  ->  SQL Server
  (HTTP)        (rules)      (data access)
```

The reason I split services from repositories is so the rules (validation, "is this product low on stock", mapping to DTOs) live in one place. Services return an explicit `(success, error)` tuple and the controller just maps that to the right HTTP status — controllers stay thin and there's no business logic hiding in them.

## What it does

- CRUD for products (name, SKU, quantity, reorder level, supplier).
- Create / list / look up / delete suppliers.
- `GET /api/products/low-stock` — products at or below their reorder level.
- DTO validation via data annotations, so e.g. negative quantities get rejected with a clear message instead of hitting the database.
- EF Core migrations, so the schema is versioned and reproducible.

There's also a `StockMovement` entity wired to each product. It's in the model and the migration, but I haven't built endpoints for it yet — see the bottom of this file.

## Stack

- .NET 10 / ASP.NET Core
- Entity Framework Core 10 (SQL Server provider)
- Swagger / OpenAPI (Swashbuckle) for the dev UI

## Running it

You need the .NET 10 SDK and a SQL Server you can reach. By default the connection string points at LocalDB (`(localdb)\MSSQLLocalDB`, database `InventoryDb`), which works out of the box on Windows — change `ConnectionStrings:DefaultConnection` in `appsettings.json` if you're pointing somewhere else.

```bash
cd InventoryManagement.API
dotnet restore
dotnet ef database update   # apply migrations
dotnet run
```

Swagger UI comes up at the HTTPS URL printed in the console (something like `https://localhost:7xxx/swagger`).

> Note: `appsettings.json` currently has a sample `Jwt:Key` committed in it. It's a placeholder, but if you fork this, move it to user secrets or an environment variable rather than a real key in the file.

## Endpoints

**Products — `/api/products`**

| Method | Route | What it does |
|---|---|---|
| GET | `/` | List all products |
| GET | `/{id}` | Get one product |
| GET | `/low-stock` | Products at or below reorder level |
| POST | `/` | Create a product |
| PUT | `/{id}` | Update a product |
| DELETE | `/{id}` | Delete a product |

**Suppliers — `/api/suppliers`**

| Method | Route | What it does |
|---|---|---|
| GET | `/` | List all suppliers |
| GET | `/{id}` | Get one supplier |
| POST | `/` | Create a supplier |
| DELETE | `/{id}` | Delete a supplier |

A quick walk-through — create a supplier, add a product that reorders at 10 units, then watch it show up as low stock:

```bash
curl -X POST https://localhost:7000/api/suppliers \
  -H "Content-Type: application/json" \
  -d '{"name":"Acme GmbH","email":"sales@acme.example","phone":"+49 30 000000"}'

curl -X POST https://localhost:7000/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Widget","sku":"WID-001","quantity":5,"reorderLevel":10,"supplierId":1}'

# quantity 5 <= reorder 10, so this product appears here:
curl https://localhost:7000/api/products/low-stock
```

## What I'd add next

A few things I deliberately left as groundwork rather than half-built features:

- **Auth.** There's a `User` model and the JWT/BCrypt packages are referenced, but authentication isn't wired into the pipeline yet. The plan is to lock down the write endpoints so only signed-in users can change inventory.
- **Stock-movement endpoints** to record and query stock-in/stock-out history (the entity already exists).
- **Tests** for the service layer — that's where most of the logic lives, so it's the obvious place to start.
- **Pagination** on the list endpoints once there's enough data to need it.
