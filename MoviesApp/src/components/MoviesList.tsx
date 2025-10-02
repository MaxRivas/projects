import type { MovieDto } from '../types/movie';

type Props = {
  movies: MovieDto[];
};

export default function MoviesList({ movies }: Props) {
  if (!movies || movies.length === 0) {
    return <p>No movies available.</p>;
  }

  return (
    <div style={{ padding: '1rem' }}>
      <h2>Movies</h2>
      <ul>
        {movies.map((m, i) => (
          <li key={i} style={{ marginBottom: '0.75rem' }}>
            <div><strong>Title:</strong> {m.title}</div>
            <div><strong>Director:</strong> {m.director}</div>
            <div><strong>Release Year:</strong> {m.releaseYear}</div>
            <div><strong>Genre:</strong> {m.genre}</div>
          </li>
        ))}
      </ul>
    </div>
  );
}
