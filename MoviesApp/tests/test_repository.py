import tempfile
from pathlib import Path
from datetime import date

from movies_app.models import Movie
from movies_app.repository import JsonMovieRepository


def test_add_and_list(tmp_path: Path):
    db = tmp_path / "movies.json"
    repo = JsonMovieRepository(db)
    m = Movie(title="Inception", genre="Sci-Fi", release_date=date(2010, 7, 16), director="Nolan")
    repo.add(m)
    items = repo.list()
    assert len(items) == 1
    assert items[0].title == "Inception"


def test_find_by_title_and_genre(tmp_path: Path):
    db = tmp_path / "movies.json"
    repo = JsonMovieRepository(db)
    repo.add(Movie(title="The Matrix", genre="Sci-Fi", release_date=date(1999, 3, 31), director="Wachowski"))
    repo.add(Movie(title="Matrix Reloaded", genre="Sci-Fi", release_date=date(2003, 5, 15), director="Wachowski"))
    results = repo.find_by_title("matrix")
    assert len(results) == 2
    sci = repo.find_by_genre("sci-fi")
    assert len(sci) == 2


def test_find_by_director(tmp_path: Path):
    db = tmp_path / "movies.json"
    repo = JsonMovieRepository(db)
    repo.add(Movie(title="Dunkirk", genre="War", release_date=date(2017, 7, 21), director="Christopher Nolan"))
    res = repo.find_by_director("nolan")
    assert len(res) == 1
