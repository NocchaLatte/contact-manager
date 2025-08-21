# Contact Manager

A simple full-stack demo application built with **ASP.NET Core Web API (C#)**, **React**, and **Material-UI (MUI)**.  
The project demonstrates a minimal **CRUD (Create, Read, Update, Delete)** workflow for managing contacts, designed to showcase integration between .NET backend and React frontend.

---

## 🚀 Features
- ASP.NET Core Web API with Entity Framework Core and SQLite
- RESTful CRUD endpoints for contact management
- React frontend with Material-UI DataGrid
- Axios-based API integration
- CORS-enabled backend for cross-origin requests
- Environment variable support for API base URL

---

## 📂 Project Structure
```
contact-manager/
├── backend/     # ASP.NET Core Web API (C#)
└── frontend/    # React + MUI (TypeScript)
```

---

## ⚙️ Setup & Run

### Backend (ASP.NET Core API)
```bash
cd backend
dotnet restore
dotnet run
```
API will be available at: `http://localhost:5217/swagger` 
### Frontend (React + MUI)
```bash
cd frontend
npm install
npm start
```
App will be running at: `http://localhost:3000` in default setup 

---

## 🛠 Tech Stack
- **Backend:** ASP.NET Core 8, C#, Entity Framework Core, SQLite
- **Frontend:** React (TypeScript), Material-UI (MUI), Axios
- **Dev Tools:** VS Code, GitHub, Docker (optional)

---

## 📌 Roadmap
- [x] Setup ASP.NET Core API with CORS
- [x] Initialize React + MUI frontend
- [ ] Implement Contact model & CRUD endpoints
- [ ] Connect React UI with API
- [ ] Add pagination and search
- [ ] Dockerize backend and frontend

---

## 📖 Purpose
This project was created as a short-term demo (1–2 days) to gain hands-on experience with **ASP.NET Core + React + MUI stack** and to demonstrate full-stack development workflow for internship/job applications.

---

## 🔐 Environment variables
Add a `.env` file in the project root (and `frontend/.env` for the React app if needed). Do NOT commit real secrets. Use `.env.example` as a template.

Example variables (see `.env.example`):
- REACT_APP_API_BASE_URL: base URL the frontend uses to call the API (dev default: http://localhost:5137)
- ConnectionStrings__DefaultConnection: EF Core connection string (dev default: Data Source=contacts.db)
- ASPNETCORE_ENVIRONMENT: Development | Production
- ASPNETCORE_URLS: URLs the backend listens on (dev default: http://localhost:5137)
- DEMO_API_KEY: optional demo key to protect write endpoints in public demos

To run locally:
- Backend: copy `.env.example` -> `.env` and run `dotnet run` in `backend/`
- Frontend: copy `.env.example` -> `frontend/.env` and run `npm start` in `frontend/`

---

## 📜 License
MIT
