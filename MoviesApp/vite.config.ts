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
    },
  },
});