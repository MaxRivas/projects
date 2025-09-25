import { useEffect, useState } from 'react';
import './App.css';
import type { MovieDto } from './types/movie';

function App() {
  const [movies, setMovies] = useState<MovieDto[]>([]);

 useEffect(() => {
  const fetchData = async () => {
    try {
      console.log('Fetch attempt started.');

      // Populate movies
      const response1 = await fetch('/api/Movies/Prefill');
      if (!response1.ok) {
        throw new Error(`HTTP error! status: ${response1.status}`);
      }
      console.log('Loaded all preset the movies');

      const response2 = await fetch('/api/Movies');
      if (!response2.ok) {
        // Log the error response body
        const errorText = await response2.text();
        console.error('Server error:', response2.status, errorText);
        throw new Error(`Server responded with an error: ${response2.status}`);
      } else {
        // Only parse as JSON if the response is OK
        console.log('Text response2', await response2.text());
        console.log('Response2 status', response2.status);
        const result: MovieDto[] = await response2.json();
        console.log('GET all preset the movies', result);
        setMovies(result);
        // const result: MovieDto[] = JSON.parse(await response2.text());
        // console.log('GET all preset the movies', result);
        // setMovies(result);
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
              {movies.map((item) => (
               <div>
                <p><strong>Movie Title:</strong> {item.title}</p>
                <p><strong>Movie Director:</strong> {item.director}</p>
                <p><strong>Release Year:</strong> {item.releaseYear}</p>
                <p><strong>Genre:</strong> {item.genre}</p>
              </div>
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
