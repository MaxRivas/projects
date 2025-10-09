import React, { useEffect, useState } from 'react'

function MovieItem({ m }) {
  return <li>{m.title} — {m.genre} — {new Date(m.releaseDate).toLocaleDateString()}</li>
}

function AddMovie({ onAdded }) {
  const [title, setTitle] = useState('')
  const [genre, setGenre] = useState('')
  const [releaseDate, setReleaseDate] = useState('')
  const [director, setDirector] = useState('')

  async function submit(e) {
    e.preventDefault()
    const body = { title, genre, releaseDate, director }
    const res = await fetch('/api/movies', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) })
    if (res.ok) {
      setTitle(''); setGenre(''); setReleaseDate(''); setDirector('')
      onAdded()
    }
  }

  return (
    <form onSubmit={submit} style={{ marginBottom: 20 }}>
      <input placeholder="Title" value={title} onChange={e => setTitle(e.target.value)} required />
      <input placeholder="Genre" value={genre} onChange={e => setGenre(e.target.value)} required />
      <input placeholder="YYYY-MM-DD" value={releaseDate} onChange={e => setReleaseDate(e.target.value)} required />
      <input placeholder="Director" value={director} onChange={e => setDirector(e.target.value)} />
      <button type="submit">Add</button>
    </form>
  )
}

export default function App() {
  const [movies, setMovies] = useState([])
  const [q, setQ] = useState('')

  const load = async (query = '') => {
    const url = query ? `/api/movies?q=${encodeURIComponent(query)}` : '/api/movies'
    const res = await fetch(url)
    const data = await res.json()
    setMovies(data)
  }

  useEffect(() => { load() }, [])

  return (
    <div style={{ padding: 20 }}>
      <h1>Movies</h1>

      <div style={{ marginBottom: 10 }}>
        <input placeholder="Search by title" value={q} onChange={e => setQ(e.target.value)} />
        <button onClick={() => load(q)}>Search</button>
        <button onClick={() => { setQ(''); load() }}>Clear</button>
      </div>

      <AddMovie onAdded={() => load(q)} />

      <ul>
        {movies.map(m => <MovieItem key={m.id} m={m} />)}
      </ul>
    </div>
  )
}
