import { useEffect, useState } from 'react';
import './App.css';
import type { MovieDto } from './types/movie';

function App() {
  const [movies, setMovies] = useState<MovieDto[]>([]);

 useEffect(() => {
  const fetchData = async () => {
    try {
      console.log('Fetch attempt started.');

  // Populate movies (prefill). Handle non-OK responses gracefully so we still try to load items.
  // Use the /MoviesApi prefix so Vite dev-server proxy forwards this call to the ASP.NET backend.
  const response1 = await fetch('/MoviesApi/Prefill');
      if (!response1.ok) {
        // Log status and body for debugging (404 or other errors)
        const text = await response1.text();
        console.warn('Prefill endpoint returned non-OK status:', response1.status, text);
        // Do not throw here so we still attempt to fetch the list below.
      } else {
        console.log('Loaded preset movies from Prefill endpoint');
      }

  // Fetch the actual movie list through the proxy so backend receives /Movies
  const response2 = await fetch('/MoviesApi/Movies');
      if (!response2.ok) {
        // Read and log the error body, then throw to be caught below
        const errorText = await response2.text();
        console.error('Server error when fetching movies:', response2.status, errorText);
        throw new Error(`Server responded with an error: ${response2.status}`);
      } else {
        // Read raw text once, then attempt to parse to provide better diagnostics for invalid JSON
        console.log('Fetching movie list from MoviesApi endpoint');
        const raw = await response2.text();
        if (!raw || raw.trim() === '') {
          console.warn('Movies endpoint returned empty body; setting empty list');
          setMovies([]);
        } else {
          try {
            const parsed = JSON.parse(raw) as MovieDto[];
            if (!Array.isArray(parsed)) {
              console.warn('Parsed JSON is not an array. Raw response:', raw);
              // If server returns an object with a wrapper, attempt to find array property common patterns:
              // e.g., { items: [...] } or { value: [...] }
              const fallbackArray = (parsed as any).items ?? (parsed as any).value ?? null;
              if (Array.isArray(fallbackArray)) {
                setMovies(fallbackArray as MovieDto[]);
              } else {
                console.error('Unexpected JSON shape from Movies endpoint:', parsed);
                throw new Error('Unexpected JSON shape; expected an array of movies.');
              }
            } else {
              console.log('Fetched movies:', parsed);
              setMovies(parsed);
            }
          } catch (parseErr) {
            // Log the full raw response to help debugging (HTML error pages, text messages, etc.)
            console.error('Failed to parse Movies endpoint JSON. Raw response was:', raw);
            throw parseErr;
          }
        }
      }

      console.log('Fetch successful');
    } catch (err) {
      console.error('Fetch error data:', err);
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
