import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path';
import tailwindcss from "@tailwindcss/vite";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  cacheDir: '.vite',
  resolve: {
    alias: {
      // "@": path.resolve(__dirname, "./src"),
      "$": path.resolve(__dirname, "../lib/src"),
    },
  },
  server: {
    port: 8383,
  },
})
