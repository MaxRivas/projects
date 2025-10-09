import sqlite3
from pathlib import Path

DB_PATH = Path(__file__).resolve().parents[1] / 'data' / 'movies.db'
DB_PATH.parent.mkdir(exist_ok=True)

con = sqlite3.connect(DB_PATH)
cur = con.cursor()

cur.execute('''
CREATE TABLE IF NOT EXISTS Movies (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Title TEXT NOT NULL,
  Genre TEXT NOT NULL,
  ReleaseDate TEXT NOT NULL,
  Director TEXT,
  CreatedAt TEXT NOT NULL
);
''')

movies = [
    ("Inception","Sci-Fi","2010-07-16","Christopher Nolan","2025-10-09T00:00:00Z"),
    ("The Matrix","Sci-Fi","1999-03-31","Wachowski","2025-10-09T00:00:00Z"),
    ("Dunkirk","War","2017-07-21","Christopher Nolan","2025-10-09T00:00:00Z"),
]

cur.executemany('INSERT INTO Movies (Title,Genre,ReleaseDate,Director,CreatedAt) VALUES (?,?,?,?,?)', movies)
con.commit()
con.close()

print(f"Seeded {DB_PATH}")
