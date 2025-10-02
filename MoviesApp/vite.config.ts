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
    // Enable CORS for local development and add permissive headers to help with
    // cross-origin debugging. This is safe for local dev only.
    cors: true,
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods': 'GET,POST,PUT,DELETE,OPTIONS',
      'Access-Control-Allow-Headers': '*',
    },
    proxy: {
      // Forward requests starting with /MoviesApi to the ASP.NET backend as-is
      // This ensures fetch('/MoviesApi/...') returns the backend JSON rather than Vite's index.html
      '/MoviesApi': {
        // Backend uses HTTPS on port 5140. Keep secure false to accept dev certs.
        target: 'https://localhost:5140',
        changeOrigin: true,
        secure: false,
        // Bypass proxy for Vite internals, source maps, and node_modules so assets
        // are served by the dev server instead of being forwarded to the API.
        bypass: (req) => {
          if (!req || !req.url) return;
          const url = req.url;
          // If the request is for a Vite dev asset, return the original url to bypass proxy
          if (
            url.startsWith('/node_modules') ||
            url.startsWith('/@') ||
            url.endsWith('.map') ||
            url.includes('/src/') ||
            url.includes('.vite')
          ) {
            return url;
          }
        },
        // Special-case Prefill (controller is MoviesController -> /Movies/Prefill)
        // and otherwise remove the /MoviesApi prefix so backend sees routes like /Movies
        rewrite: (path) => path.replace(/^\/MoviesApi\/Prefill/, '/Movies/Prefill').replace(/^\/MoviesApi/, ''),
      },
    },
  },
});