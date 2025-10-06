import type { MovieDto } from '../types/movie';
import { DataTable } from './dataTable';
import {columns } from '../types/movie';

type Props = {
  movies: MovieDto[];
};

export default function MoviesList({ movies }: Props) {
  if (!movies || movies.length === 0) {
    return <p>No movies available.</p>;
  }

  return (
   <div className="container mx-auto py-10">
      <DataTable columns={columns} data={movies} />
    </div>
  );
}
