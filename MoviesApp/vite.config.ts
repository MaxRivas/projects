// vite.config.js
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // All requests starting with /api will be sent to the ASP.NET server
      '/api': {
        target: 'https://localhost:5140', // Your ASP.NET backend address
        changeOrigin: true, // Changes the origin of the host header to the target URL
        secure: false, // Set to true if your backend uses HTTPS with a valid certificate
      },

      // Forward requests starting with /MoviesApi to the ASP.NET backend as-is
      // This ensures fetch('/MoviesApi/...') returns the backend JSON rather than Vite's index.html
      '/MoviesApi': {
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