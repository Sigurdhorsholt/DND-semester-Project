/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './src/**/*.{js,jsx,ts,tsx}',  // Tells Tailwind to look for class names in all .js, .jsx, .ts, .tsx files inside /src
    './public/index.html',          // You can include the public folder if necessary
  ],
  theme: {
    extend: {},
  },
  plugins: [],
};