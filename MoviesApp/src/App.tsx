import { useEffect, useState } from 'react';
import './App.css';
import type { MovieDto } from './types/movie';

function App() {
  const [movie, setMovies] = useState<MovieDto | null>(null);

  useEffect(() => {
    fetch('/Movies/new') // The proxy configured by SpaProxy forwards this to the backend
      .then(response => response.json())
      .then((data: MovieDto) => {
        setMovies(data);
      })
      .catch(error => {
        console.error('Error fetching data:', error);
      });
  }, []);

  return (
    <div className="App">
      <header className="App-header">
        <h1>Prefilled movie Data</h1>
        {movie ? (
          <div>
            <p><strong>Movie Title:</strong> {movie.title}</p>
            <p><strong>Movie Director:</strong> {movie.director}</p>
            <p><strong>Release Year:</strong> {movie.releaseYear}</p>
            <p><strong>Genre:</strong> {movie.genre}</p>
          </div>
        ) : (
          <p>Loading...</p>
        )}
      </header>
    </div>
  );
}

export default App;
