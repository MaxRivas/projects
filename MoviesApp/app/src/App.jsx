import React, { useEffect, useState } from 'react'

export default function App() {
  const [movies, setMovies] = useState([])

  useEffect(() => {
    fetch('/api/movies')
      .then(r => r.json())
      .then(setMovies)
      .catch(console.error)
  }, [])

  return (
    <div style={{ padding: 20 }}>
      <h1>Movies</h1>
      <ul>
        {movies.map(m => (
          <li key={m.id}>{m.title} — {m.genre} — {new Date(m.releaseDate).toLocaleDateString()}</li>
        ))}
      </ul>
    </div>
  )
}
