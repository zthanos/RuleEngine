/** @type {import('tailwindcss').Config} */
export default {
    content: [
      "./index.html",
      "./src/**/*.{vue,js,ts,jsx,tsx}",
      "./node_modules/flowbite/**/*.js"
    ],
    theme: {
      extend: {
        colors: {
          emSyntax:"#0E1116",
          emDark:{
            dark: "#010101",
            DEFAULT: "#3C3B3B",
            light: "#828282"
          },
          emPurple: {
            dark: "#631A61",
            DEFAULT: "#A834A5",
            light: "#E88FE5"
          },
          emLavender: {
            dark: "#8C52FF",
            DEFAULT: "#9D74EE",
            light: "#BC9AFF"
          }
        }
      },
    },
    plugins: [
      require('flowbite/plugin')
    ],
  }
  
  