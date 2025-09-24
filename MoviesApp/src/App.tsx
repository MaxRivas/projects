import { useEffect, useState } from 'react';
import './App.css';
import type { MovieDto } from './types/movie';

function App() {
  const [movie, setMovies] = useState<MovieDto | null>(null);

  useEffect(() => {
    fetch('/Movies/new') // The proxy configured by SpaProxy forwards this to the backend
      .then(response => {
        // Log the entire response object to see its status, headers, etc.
        console.log('Response object:', response);

        // Check if the response is successful (status code 200-299)
        if (!response.ok) {
          // If not successful, try to get the error message as text
          return response.text().then(errorText => {
            throw new Error(`HTTP error! Status: ${response.status}, Message: ${errorText}`);
          });
        }
      })
      .catch(error => {
        console.error('Fetch error data:', error);
      });
    },
   []);

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
