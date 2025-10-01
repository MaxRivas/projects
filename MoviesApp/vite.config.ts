// vite.config.js
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // Forward requests starting with /MoviesApi to the ASP.NET backend as-is
      // This ensures fetch('/MoviesApi/...') returns the backend JSON rather than Vite's index.html
      '/MoviesApi': {
        // The backend exposes HTTP on port 5140 (see launchSettings.json). Use http:// so the proxy connects correctly.
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