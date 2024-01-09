/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    './src/**/*.{vue,js,ts,jsx,tsx}'
  ],
  theme: {
    extend: {
      width: {
        '1%': '1%',
        '98%': '98%',
      },
    },
  },
  plugins: [],
}

