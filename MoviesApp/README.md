# MoviesApp — minimal movies organizer

This small package provides a JSON-backed organizer for movies (title, genre, release date, director).

Quick start

```bash
python -m venv .venv
source .venv/bin/activate
pip install -r requirements.txt
pytest -q
```

CLI example

```bash
python -m movies_app.cli add --title "Inception" --genre "Sci-Fi" --release-date 2010-07-16 --director Nolan
python -m movies_app.cli list
```
