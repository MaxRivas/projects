# MoviesApp — minimal movies organizer

This small package provides a JSON-backed organizer for movies (title, genre, release date, director).

Quick start (backend + frontend)

Backend (.NET 8):

```bash
cd server
dotnet build
dotnet run
# API listens on http://localhost:5000 by default
```

Frontend (Vite + React):

```bash
cd app
npm install
npm run dev
```

Seeded DB

The repository includes a seeded `data/movies.db` for local development. To recreate or reseed:

```bash
python3 scripts/seed_sqlite.py
```

Tests

Python unit tests for the small local CLI/library live under `tests/`. To run them:

```bash
python -m venv .venv
source .venv/bin/activate
pip install -r requirements.txt
PYTHONPATH=src .venv/bin/pytest -q
```

CI

There is a GitHub Actions workflow at `.github/workflows/ci.yml` that builds the server (.NET 8) and builds the frontend.

