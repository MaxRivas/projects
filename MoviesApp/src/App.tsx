import { useEffect, useState } from 'react';
import './App.css';
import type { MovieDto } from './types/movie';

function App() {
  const [movies, setMovies] = useState<MovieDto[]>([]);

 useEffect(() => {
  const fetchData = async () => {
    // Attempt to prefill server-side data; ignore errors
    try { await fetch('/MoviesApi/Prefill'); } catch { /* ignore */ }

    // Fetch movies and set state. On any failure, leave movies empty.
    try {
      const res = await fetch('/MoviesApi/Movies');
      if (!res.ok) { setMovies([]); return; }
      const data = await res.json();
      // Accept either an array or common wrapper shapes { items: [...] } / { value: [...] }
      if (Array.isArray(data)) {
        setMovies(data as MovieDto[]);
        return;
      }

      const d = data as unknown;
      if (d && typeof d === 'object') {
        const obj = d as Record<string, unknown>;
        const maybe = obj['items'] ?? obj['value'] ?? null;
        if (Array.isArray(maybe)) {
          setMovies(maybe as MovieDto[]);
          return;
        }
      }

      setMovies([]);
    } catch {
      setMovies([]);
    }
  };

  fetchData();
}, []); // Add a dependency array

  return (
    <div className="App">
      <header className="App-header">
        <h1>Prefilled movie Data</h1>
        {movies.length > 0 ? (
            <ul>
              {movies.map((item, index) => (
               <li key={index}>
                <p><strong>Movie Title:</strong> {item.title}</p>
                <p><strong>Movie Director:</strong> {item.director}</p>
                <p><strong>Release Year:</strong> {item.releaseYear}</p>
                <p><strong>Genre:</strong> {item.genre}</p>
              </li>
              ))}
            </ul>
          ) : (
            <p>Loading items...</p>
          )}
      </header>
    </div>
  );
}

export default App;
