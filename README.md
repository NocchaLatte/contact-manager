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

## 📜 License
MIT
