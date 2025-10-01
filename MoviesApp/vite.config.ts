// vite.config.ts
import path from 'path';
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import svgr from 'vite-plugin-svgr';

// See https://ui.shadcn.com/docs/installation/vite for guidance — this config
// adds a convenient alias, SVGR support (import SVGs as React components), and
// a few optimizeDeps entries that shadcn components rely on.
export default defineConfig({
  plugins: [react(), svgr()],
  resolve: {
    alias: {
      // Allows imports like: import X from '@/components/X'
      '@': path.resolve(__dirname, "./src"),
    },
  },
  optimizeDeps: {
    include: [
      // Common shadcn/ui runtime deps
      'class-variance-authority',
      'tailwind-merge',
      // A few Radix packages often used by shadcn components (add more as needed)
      '@radix-ui/react-popover',
      '@radix-ui/react-dialog',
      '@radix-ui/react-tooltip',
    ],
  },
  // Prevent Vite SSR from externalizing some shadcn runtime dependencies
  ssr: {
    noExternal: [
      'class-variance-authority',
      'tailwind-merge',
      '@radix-ui/react-popover',
      '@radix-ui/react-dialog',
      '@radix-ui/react-tooltip',
    ],
  },
  server: {
    proxy: {
      // Forward requests starting with /MoviesApi to the ASP.NET backend as-is
      // This ensures fetch('/MoviesApi/...') returns the backend JSON rather than Vite's index.html
      '/MoviesApi': {
        // Backend uses HTTPS on port 5140. Keep secure false to accept dev certs.
        target: 'https://localhost:5140',
        changeOrigin: true,
        secure: false,
        // Special-case Prefill (controller is MoviesController -> /Movies/Prefill)
        // and otherwise remove the /MoviesApi prefix so backend sees routes like /Movies
        rewrite: (path) => path.replace(/^\/MoviesApi\/Prefill/, '/Movies/Prefill').replace(/^\/MoviesApi/, ''),
      },
    },
  },
});