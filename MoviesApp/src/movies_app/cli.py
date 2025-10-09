import argparse
from datetime import date
from pathlib import Path
import sys

from .models import Movie
from .repository import JsonMovieRepository


def build_parser():
    p = argparse.ArgumentParser(prog="movies-app")
    p.add_argument("--db", default="movies.json", help="path to json db file")
    sub = p.add_subparsers(dest="cmd")

    add = sub.add_parser("add")
    add.add_argument("--title", required=True)
    add.add_argument("--genre", required=True)
    add.add_argument("--release-date", required=True, help="YYYY-MM-DD")
    add.add_argument("--director")

    sub.add_parser("list")

    find = sub.add_parser("find")
    find.add_argument("--title")
    find.add_argument("--genre")
    find.add_argument("--director")

    clear = sub.add_parser("clear")

    return p


def main(argv=None):
    argv = argv or sys.argv[1:]
    parser = build_parser()
    args = parser.parse_args(argv)
    repo = JsonMovieRepository(Path(args.db))

    if args.cmd == "add":
        rd = date.fromisoformat(args.release_date)
        movie = Movie(title=args.title, genre=args.genre, release_date=rd, director=args.director)
        repo.add(movie)
        print("Added")
    elif args.cmd == "list":
        for m in repo.list():
            print(f"{m.title} | {m.genre} | {m.release_date} | {m.director}")
    elif args.cmd == "find":
        results = repo.list()
        if args.title:
            results = repo.find_by_title(args.title)
        if args.genre:
            results = [m for m in results if m.genre.lower() == args.genre.lower()]
        if args.director:
            results = [m for m in results if m.director and args.director.lower() in m.director.lower()]
        for m in results:
            print(f"{m.title} | {m.genre} | {m.release_date} | {m.director}")
    elif args.cmd == "clear":
        repo.clear()
        print("Cleared")
    else:
        parser.print_help()


if __name__ == "__main__":
    main()
