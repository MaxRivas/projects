// Helper module declarations to make the '@/*' imports resolvable by tsc
// These forward the alias to the real relative modules.

declare module "@/lib/utils" {
  // simple helper exported from src/lib/utils.ts
  export function cn(...inputs: Array<string | false | null | undefined>): string;
}

declare module "@/components/ui/Button" {
  import type { ButtonHTMLAttributes } from 'react';
  type Props = ButtonHTMLAttributes<HTMLButtonElement> & { className?: string };
  export default function Button(props: Props): JSX.Element;
}

declare module "@/components/ui/table" {
  import * as React from 'react';

  export const Table: React.ForwardRefExoticComponent<React.TableHTMLAttributes<HTMLTableElement>>;
  export const TableHeader: React.ForwardRefExoticComponent<React.HTMLAttributes<HTMLTableSectionElement>>;
  export const TableBody: React.ForwardRefExoticComponent<React.HTMLAttributes<HTMLTableSectionElement>>;
  export const TableFooter: React.ForwardRefExoticComponent<React.HTMLAttributes<HTMLTableSectionElement>>;
  export const TableRow: React.ForwardRefExoticComponent<React.HTMLAttributes<HTMLTableRowElement>>;
  export const TableHead: React.ForwardRefExoticComponent<React.ThHTMLAttributes<HTMLTableCellElement>>;
  export const TableCell: React.ForwardRefExoticComponent<React.TdHTMLAttributes<HTMLTableCellElement>>;
  export const TableCaption: React.ForwardRefExoticComponent<React.HTMLAttributes<HTMLTableCaptionElement>>;
}
