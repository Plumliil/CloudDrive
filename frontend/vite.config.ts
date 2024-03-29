import { defineConfig } from 'vite'
import path from 'path';
import vue from '@vitejs/plugin-vue'
import tailwindcss from 'tailwindcss'
import autoprefixer from 'autoprefixer'
import AutoImport from 'unplugin-auto-import/vite'
import { viteStaticCopy } from "vite-plugin-static-copy";
// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    AutoImport({
      imports: ['vue', 'vue-router', 'pinia'],
    }),
    viteStaticCopy({
      targets: [
        {
          src: "./node_modules/@idux/components/icon/assets/*.svg",
          dest: "idux-icons",
        },
      ],
    }),
  ],
  css: {
    postcss: {
      plugins: [
        tailwindcss,
        // autoprefixer,
      ]
    }
  },
  resolve: {
    // 配置路径别名
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:3000',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, ''),
      },
      '/CloudDrive': {
        target: 'https://v5698k5455.vicp.fun/CloudDrive',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/CloudDrive/, ''),
      },
    }
  },
})
