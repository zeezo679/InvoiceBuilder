# InvoiceBuilder API

A professional REST API for creating, managing, and sending invoices — built with **ASP.NET Core** following **Clean Architecture** and **Domain-Driven Design** principles.

> 🖥️ **Frontend Repository:** [InvoiceBuilder_Web](https://github.com/zeezo679/InvoiceBuilder_Web)

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 |
| Architecture | Clean Architecture + DDD |
| CQRS | MediatR |
| Database | PostgreSQL + EF Core |
| Auth | ASP.NET Core Identity + JWT |
| Background Jobs | Hangfire |
| Email | MailKit |
| Error Handling | ErrorOr |
| Validation | FluentValidation |

---

## Project Structure
```
InvoiceBuilder/
├── Domain/              # Entities, Value Objects, Domain errors
├── Application/         # Use cases, CQRS handlers, Interfaces, Validators
├── Infrastructure/      # Identity, Email, Background jobs, EF Core
└── Api/                 # Controllers, Requests, Middleware
```

## Modules

- **Auth** — Registration, Email Verification, Login
- **Sender** — Manage sender profiles
- **Customer** — Manage customers
- **Invoice** — Create and manage invoices
- **Payment** — Track payments
- **Reports** — Dashboard and reporting

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/)

### 1. Clone the repository
```bash
git clone https://github.com/zeezo679/InvoiceBuilder.git
cd InvoiceBuilder
```

### 2. Configure `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=invoicebuilder;Username=postgres;Password=yourpassword"
  },
  "App": {
    "BaseUrl": "http://localhost:5043"
  },
  "Jwt": {
    "Secret": "your-secret-key",
    "Issuer": "InvoiceBuilder",
    "Audience": "InvoiceBuilder"
  },
  "Email": {
    "Host": "smtp.yourprovider.com",
    "Port": 587,
    "Username": "your@email.com",
    "Password": "yourpassword",
    "FromEmail": "your@email.com",
    "FromName": "InvoiceBuilder"
  }
}
```

### 3. Apply migrations
```bash
dotnet ef database update --project Infrastructure --startup-project Api
```

### 4. Run the API
```bash
dotnet run --project Api
```

API runs at `http://localhost:5043`. Hangfire dashboard at `http://localhost:5043/hangfire`.

---

## Auth Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/auth/register` | Register a new user |
| GET | `/auth/verify-email` | Verify email address |
| POST | `/auth/login` | Login (requires verified email) |

### Register
```http
POST /auth/register
Content-Type: application/json

{
  "firstName": "Zeyad",
  "lastName": "Abdalla",
  "email": "zeyad@example.com",
  "password": "Passw0rd!"
}
```

### Verify Email
```http
GET /auth/verify-email?userId={userId}&token={token}
```

### Login
```http
POST /auth/login
Content-Type: application/json

{
  "email": "zeyad@example.com",
  "password": "Passw0rd!"
}
```

---

## Architecture Decisions

**Clean Architecture** — Domain and Application layers have zero dependency on infrastructure concerns. All external services are accessed through interfaces defined in the Application layer and implemented in Infrastructure.

**CQRS with MediatR** — Commands and queries are fully separated. Each use case is a single handler with a single responsibility.

**Background Jobs** — Email sending is offloaded to Hangfire to keep registration response times fast. Jobs are persisted in PostgreSQL and retried automatically on failure.

**ErrorOr** — All use cases return `ErrorOr<T>` instead of throwing exceptions, giving controllers full control over HTTP status code mapping.

---

## Frontend

The frontend is built with React + TypeScript + Vite.

👉 [InvoiceBuilder_Web](https://github.com/zeezo679/InvoiceBuilder_Web)
```
