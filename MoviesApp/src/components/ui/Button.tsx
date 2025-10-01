import { cva, type VariantProps } from 'class-variance-authority';
import React from 'react';

const button = cva(
  'inline-flex items-center justify-center rounded-md text-sm font-medium transition-colors focus:outline-none',
  {
    variants: {
      variant: {
        default: 'bg-slate-900 text-white hover:bg-slate-700',
        outline: 'border border-slate-200 bg-white text-slate-900',
      },
      size: {
        sm: 'px-2 py-1 text-sm',
        md: 'px-4 py-2',
      },
    },
    defaultVariants: {
      variant: 'default',
      size: 'md',
    },
  },
);

type ButtonProps = React.ButtonHTMLAttributes<HTMLButtonElement> &
  VariantProps<typeof button>;

export function Button({ className, variant, size, ...props }: ButtonProps) {
  return <button className={button({ variant, size }) + (className ? ' ' + className : '')} {...props} />;
}

export default Button;
