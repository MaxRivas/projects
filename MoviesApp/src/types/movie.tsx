"use client"
 
import { type ColumnDef } from "@tanstack/react-table"

export type MovieDto = 
{
    /// <summary>
    /// Movie identifier.
    /// </summary>
    id: number;

    /// <summary>
    /// Movie title.
    /// </summary>
    title: string;

    /// <summary>
    /// Movie director.
    /// </summary>
    director: string;

    /// <summary>
    /// Release year.
    /// </summary>
    releaseYear: number;

    /// <summary>
    /// Movie genre.
    /// </summary>
    genre: string;
}
 
export const columns: ColumnDef<MovieDto>[] = [
  {
    accessorKey: "title",
    header: "Title",
  },
  {
    accessorKey: "releaseYear",
    header: "Release Year",
  },
  {
    accessorKey: "genre",
    header: "Genre",
  },
]