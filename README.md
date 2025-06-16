# Sneakahs
# 🛒 Sneakahs Ecommerce Backend

This is a fully functional ecommerce backend API built with **C# and ASP.NET Core**, following **Clean Architecture** principles. It supports user authentication, product browsing, cart management, Stripe checkout integration, and order tracking — all backed by a PostgreSQL database and JWT-based authentication.

> ⚠️ This is a **backend-only** project. No frontend is included, but the API is fully ready to integrate with any frontend or mobile application.

---

## 🚀 Features

- ✅ User registration and login (JWT-based auth)
- ✅ Product catalog (retrieve all products)
- ✅ Cart & cart items (add, update, delete)
- ✅ Stripe test mode integration for payments
- ✅ Order management with webhook confirmation
- ✅ Some unit tests for core service logic
- 🧩 Clean and modular folder structure for scalability
- ⏳ Planned: Role-based admin features and frontend integration

---

## 🧱 Tech Stack

| Layer        | Technology            |
|--------------|------------------------|
| Language     | C#                     |
| Framework    | ASP.NET Core Web API   |
| Database     | PostgreSQL             |
| Auth         | JWT Bearer Tokens      |
| Payments     | Stripe (Test Mode)     |
| Architecture | Clean Architecture     |

---

## 📁 Folder Structure

Sneakahs.API/ # Web API entry point (controllers, program config, DI)
├── Controllers/ # API endpoints
├── appsettings.json # Configuration
├── Program.cs # Main entry point

Sneakahs.Application/ # Application layer (business logic)
├── DTO/ # Data Transfer Objects
├── Interfaces/ # Service and repository interfaces
├── Services/ # Core service interfaces

Sneakahs.Infrastructure/ # Infrastructure layer (implementations)
├── Persistence/ # Database context and migrations
├── Services/ # JWT, Stripe, and service implementations
├── DependencyInjection/ # DI registration

Sneakahs.Domain/ # Domain layer (core entities)
├── Entities/ # Domain models like User, Product, CartItem
├── Enums/ # Shared enums
├── Interfaces/ # Domain-level interfaces

---

## 🛠️ Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/BrandonGatewood/sneakahs-backend.git
cd sneakahs-backend
```

### 2. Configure environment
Create a .env file in the root directory with the following:
DB_URL=Host=localhost;Username=postgres;Password=password;Database=sneakahs
JWT_ISSUER=SneakahsApi
JWT_AUDIENCE=SneakahsClient
STRIPE_SECRET_KEY="yourSecretKey"
STRIPE_WEBHOOK_SECRET="yourWebhookSecret"

### 3. Run the application
```bash
cd Sneakahs.API
dotnet run
```

💳 Stripe Test Mode Setup
To test payments and webhook flows:

Install Stripe CLI

Forward webhooks to your local app:
```bash
stripe listen --forward-to localhost:5000/webhook/stripe
```

📬 Sample Endpoints
Method	Endpoint	Description
POST	/api/auth/register	Register new user
POST	/api/auth/login	Login & receive JWT
GET	/api/products	Get all products
POST	/api/cart/add	Add item to cart
POST	/api/orders/checkout	Start Stripe checkout
POST	/webhook/stripe	Handle payment webhook

###Test
dotnet test
