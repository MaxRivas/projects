# Implementation Plan — MoviesApp

This document captures the chosen stacks and the minimal implementation plan for the MoviesApp: frontend (Vite + React), backend (ASP.NET Core Web API), and local SQLite storage for movie metadata.

## Big picture

- Frontend: Vite + React (minimal libraries). Responsible for UI: list, search, add, edit movies. Development server proxies API calls to backend.
- Backend: ASP.NET Core Web API (.NET 7+ recommended). Exposes RESTful endpoints for basic CRUD and search. Uses EF Core with SQLite provider for persistence.
- DB: Local SQLite file (development: `data/movies.db`). Schema stored via EF Core migrations.

## Data model (single table)

- Movies
  - Id: INTEGER (PK, autoincrement)
  - Title: TEXT (indexed)
  - Genre: TEXT (indexed)
  - ReleaseDate: DATE
  - Director: TEXT
  - CreatedAt: DATETIME

## API surface (minimal)

- GET /api/movies
  - Query: page, pageSize, q (title partial), genre, director
  - Returns: paginated list of movies

- GET /api/movies/{id}
  - Returns movie by id or 404

- POST /api/movies
  - Body: { title, genre, releaseDate, director }
  - Returns: 201 + created resource

- PUT /api/movies/{id}
  - Body: same as POST
  - Returns: 200 + updated resource

- DELETE /api/movies/{id}
  - Returns: 204 on success

## Dev / build / test commands

- Backend (from `backend/`):
  - dotnet build
  - dotnet run --project backend/MoviesApi
  - dotnet test

- Database migrations (backend):
  - dotnet ef migrations add Initial -p backend/MoviesApi -s backend/MoviesApi
  - dotnet ef database update -p backend/MoviesApi -s backend/MoviesApi

- Frontend (from `frontend/`):
  - npm install
  - npm run dev
  - npm run build
  - npm test (if tests added)

- Full-local (root):
  - Start backend (dotnet run) and then frontend dev server which proxies to the backend.

## Conventions & patterns

- Project layout (follow `.specify/templates/plan-template.md`):
  - backend/MoviesApi/ — ASP.NET Core Web API
  - backend/MoviesApi/Models, Controllers, Data
  - frontend/ — Vite + React app
  - data/ — SQLite file stored in `data/movies.db` (gitignored)

- Keep frontend dependencies minimal: React, React Router (if navigation required), fetch or axios (fetch preferred). Reuse UI components.
- API responses follow the project's error shape: { code, message, details }.

## Integration & testing

- Unit tests: backend unit tests for controllers/services and frontend component/unit tests.
- Integration tests: run a backend test that uses an in-memory SQLite or test database file; CI should run migrations then tests.

## CI notes

- CI job for backend: restore → build → migrations (in memory or test db) → test
- CI job for frontend: npm ci → build → unit tests

## Agent integration notes

- Update `.specify/templates/agent-file-template.md` and `.specify/scripts/bash/update-agent-context.sh` to include the commands above for detected technologies.
- Ensure `.specify/scripts/bash/update-agent-context.sh` detects .NET and Node stacks in `Plan.md` so agent files include the right commands.
