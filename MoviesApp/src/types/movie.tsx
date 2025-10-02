"use client"
import { type ColumnDef } from "@tanstack/react-table"
import { MoreHorizontal } from "lucide-react"
import { Button } from "../components/ui/button"

import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "../components/ui/dropdown-menu"

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
    accessorKey: "director",
    header: "Director",
  },
  {
    accessorKey: "genre",
    header: "Genre",
  },
  {
    accessorKey: "releaseYear",
    header: "Release Year",
  },
  {
    id: "actions",
    cell: ({ row }) => {
      const movie = row.original
 
      return (
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" className="h-8 w-8 p-0">
              <span className="sr-only">Open menu</span>
              <MoreHorizontal className="h-4 w-4" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuItem>View details</DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      )
    },
  },
]