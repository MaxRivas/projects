import json
from pathlib import Path
from typing import List

from .models import Movie


class JsonMovieRepository:
    def __init__(self, path: Path):
        self.path = Path(path)
        self._data: List[Movie] = []
        self._load()

    def _load(self):
        if not self.path.exists():
            self._data = []
            return
        with open(self.path, "r", encoding="utf-8") as f:
            try:
                raw = json.load(f)
            except json.JSONDecodeError:
                raw = []
        self._data = [Movie.from_dict(d) for d in raw]

    def _save(self):
        with open(self.path, "w", encoding="utf-8") as f:
            json.dump([m.to_dict() for m in self._data], f, ensure_ascii=False, indent=2)

    def add(self, movie: Movie):
        self._data.append(movie)
        self._save()

    def list(self) -> List[Movie]:
        return list(self._data)

    def find_by_title(self, title: str) -> List[Movie]:
        key = title.lower()
        return [m for m in self._data if key in m.title.lower()]

    def find_by_genre(self, genre: str) -> List[Movie]:
        key = genre.lower()
        return [m for m in self._data if key == m.genre.lower()]

    def find_by_director(self, director: str) -> List[Movie]:
        key = director.lower()
        return [m for m in self._data if m.director and key in m.director.lower()]

    def clear(self):
        self._data = []
        self._save()
