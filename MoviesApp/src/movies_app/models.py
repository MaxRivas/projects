from dataclasses import dataclass
from datetime import date
from typing import Optional


@dataclass
class Movie:
    title: str
    genre: str
    release_date: date
    director: Optional[str] = None

    def to_dict(self) -> dict:
        return {
            "title": self.title,
            "genre": self.genre,
            "release_date": self.release_date.isoformat(),
            "director": self.director,
        }

    @staticmethod
    def from_dict(d: dict) -> "Movie":
        rd = d.get("release_date")
        if isinstance(rd, str):
            rd = date.fromisoformat(rd)
        return Movie(
            title=d["title"],
            genre=d["genre"],
            release_date=rd,
            director=d.get("director"),
        )
