# Backend Design – Contact Manager

This document describes the **design and architecture** of the backend system for the Contact Manager project.

---

## 1. Architecture Overview
The backend is implemented as a **RESTful Web API** using **ASP.NET Core 8**.  
It provides CRUD operations for managing `Contact` entities stored in a local **SQLite database** through **Entity Framework Core (EF Core)**.

```
[ React (MUI Frontend) ]
        |
        v
[ ASP.NET Core Web API ]
        |
        v
[ Entity Framework Core ]
        |
        v
[ SQLite Database ]
```

---

## 2. Key Components

### 2.1 Models
- **Contact**
  - Represents a contact record in the system
  - Fields: `Id`, `Name`, `Email`, `Phone`, `Note`

### 2.2 Data Layer
- **AppDbContext**
  - EF Core DbContext managing entity sets (`DbSet<Contact>`)
  - Responsible for database connection and queries

- **SeedData**
  - Initializes the database with a few sample records if empty
  - Supports testing and development without manual data entry

### 2.3 Controllers
- **ContactsController**
  - Exposes RESTful endpoints under `/api/contacts`
  - Implements CRUD operations:
    - `GET /api/contacts`
    - `GET /api/contacts/{id}`
    - `POST /api/contacts`
    - `PUT /api/contacts/{id}`
    - `DELETE /api/contacts/{id}`

### 2.4 Configuration
- **Program.cs**
  - Registers services: EF Core, Controllers, Swagger, CORS
  - Defines the HTTP request pipeline
  - Ensures database creation at startup and runs seed logic
  - CORS policy allows requests from `http://localhost:3000` (frontend)

---

## 3. Data Flow

1. **Client request** (e.g., React app calls `GET /api/contacts`)
2. **Controller** receives HTTP request and validates parameters
3. **DbContext** executes EF Core LINQ query against SQLite
4. **Entity** is mapped to JSON and returned to the client
5. **Client UI** displays results in MUI DataGrid

---

## 4. Design Decisions

- **SQLite chosen** for simplicity:
  - Lightweight, file-based, ideal for demo and small-scale apps
  - Can be swapped for SQL Server or PostgreSQL with minimal changes

- **Entity Framework Core** for ORM:
  - Provides strongly-typed queries
  - Simplifies migrations and schema management

- **CORS enabled**:
  - Explicitly allows `http://localhost:3000`
  - Ensures React frontend can consume API in dev environment

- **Swagger integration**:
  - Built-in API testing UI
  - Documents available endpoints

---

## 5. Extensibility

The current design supports extension with minimal disruption:
- **New fields** in `Contact` can be added via EF Core migration
- **Additional entities** (e.g., `Company`, `Tag`) can be introduced by adding models and DbSets
- **AI services** can be layered as separate endpoints (`/api/ai/...`) without touching existing CRUD
- **Authentication/Authorization** can be added with JWT middleware

---

## 6. Limitations

- No authentication: API is open in current design
- No advanced validation beyond data types
- Single-user environment (no multi-tenant support)
- SQLite not suitable for production-scale deployments

---

## 7. Future Roadmap
- Add **search & pagination** in GET endpoints
- Add **JWT authentication** and user roles
- Integrate with **AI micro-features** (note summarization, sentiment analysis)
- Dockerize backend for containerized deployment
