# Database Design – Contact Manager

This document describes the **database schema and entity relationships** of the Contact Manager project.

---

## 1. Entities

### Contact
- Core entity representing a person
- Fields: `Id`, `Name`, `Email`, `Phone`, `Note`

### Group
- Represents a logical contact group (e.g., Family, Work)
- Fields: `Id`, `Name`

### ContactGroup
- Join table for Contact ↔ Group (N:M)
- Composite Key: `(ContactId, GroupId)`

### Tag
- Flexible labels applied to contacts (e.g., VIP, Prospect)
- Fields: `Id`, `Name`

### ContactTag
- Join table for Contact ↔ Tag (N:M)
- Composite Key: `(ContactId, TagId)`

### Note
- Notes related to a specific contact
- Fields: `Id`, `ContactId`, `Content`, `CreatedAt`

---

## 2. ER Diagram (Textual)

```
 Contact (1)───(N) Note
    |
    ├───(N:M)─── Group
    |
    └───(N:M)─── Tag
```

---

## 3. EF Core Configuration

- `AppDbContext` defines `DbSet<>` for all entities
- `OnModelCreating` sets composite keys for join tables:
  - `ContactGroup(ContactId, GroupId)`
  - `ContactTag(ContactId, TagId)`
- Cascade delete:
  - Deleting Contact removes related Notes
  - Deleting Group/Tag only removes join records

---

## 4. Performance & Integrity

- Index on `Contact.Email` (unique)
- Composite PKs on join tables → efficient lookups
- Navigation properties + `Include` to avoid N+1 query issues
- Foreign keys with cascade delete for Notes
- SQLite is fine for demo; migration to SQL Server/Postgres recommended for scale

---

## 5. Future Extensions
- Tag categories (e.g., color, type)
- Group hierarchies (nested groups)
- Full-text search on Notes
