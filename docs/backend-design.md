# Backend Design – Contact Manager

This document describes the **design and architecture** of the backend system for the Contact Manager project.

---

## 1. Architecture Overview
The backend is implemented as a **RESTful Web API** using **ASP.NET Core 8**.  
It provides CRUD operations for managing `Contact` entities stored in a local **SQLite database** through **Entity Framework Core (EF Core)**.  
The design also supports **Groups**, **Tags**, and **Notes**, reflecting a richer contact management system.

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
- **Group**
  - Represents logical contact groups (Family, Work, etc.)
  - N:M relationship with Contact through `ContactGroup`
- **Tag**
  - Flexible labels attached to contacts (e.g., VIP, Prospect)
  - N:M relationship with Contact through `ContactTag`
- **Note**
  - Notes linked to a Contact (1:N relationship)
  - Supports audit trail or AI-generated entries

### 2.2 Data Layer
- **AppDbContext**
  - EF Core DbContext managing entity sets (`DbSet<Contact>`, `DbSet<Group>`, `DbSet<Tag>`, etc.)
  - Responsible for database connection and queries

- **SeedData**
  - Initializes the database with sample data for testing and development

### 2.3 Controllers
- **ContactsController**
  - Exposes CRUD endpoints for `Contact`
- **GroupsController**, **TagsController**, **NotesController** (future)
  - Provide CRUD for related entities

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

Additional flows:
- Groups/Tags fetched via join tables
- Notes appended automatically or manually linked to a Contact

---

## 4. Design Decisions

- **SQLite chosen** for simplicity:
  - Lightweight, file-based, ideal for demo and small-scale apps
  - Can be swapped for SQL Server or PostgreSQL with minimal changes

- **Entity Framework Core** for ORM:
  - Provides strongly-typed queries
  - Simplifies migrations and schema management

- **N:M relationships** explicitly modeled with join tables:
  - Ensures database normalization and efficient lookups

- **CORS enabled**:
  - Explicitly allows `http://localhost:3000`
  - Ensures React frontend can consume API in dev environment

- **Swagger integration**:
  - Built-in API testing UI
  - Documents available endpoints

---

## 5. Extensibility

The current design supports extension with minimal disruption:
- Adding **new fields** or **entities** via EF Core migration
- Expanding Group/Tag logic without breaking existing CRUD
- Adding **AI services** (summarization, sentiment analysis) as new controllers
- Introducing **Authentication/Authorization** with JWT middleware

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
