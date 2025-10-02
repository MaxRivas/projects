import { useEffect, useState } from 'react';
import './App.css';
import type { MovieDto } from './types/movie';
import WelcomePage from './pages/WelcomePage';
import MoviesList from './components/MoviesList';

function App() {
  const [movies, setMovies] = useState<MovieDto[]>([]);
  const [route, setRoute] = useState<'welcome' | 'movies'>('welcome');

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
      <nav style={{ padding: '1rem', display: 'flex', gap: '1rem' }}>
      </nav>

      <main>
        {route === 'welcome' && (
          <WelcomePage onShowMovies={() => setRoute('movies')} />
        )}
        {route === 'movies' && <MoviesList movies={movies} />}
      </main>
    </div>
  );
}

export default App;
