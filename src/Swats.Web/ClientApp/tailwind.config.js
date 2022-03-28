const defaultTheme = require("tailwindcss/defaultTheme");

module.exports = {
  purge: {
    enabled: false,
    content: [
      "./src/**/*.{js,jsx,ts,tsx}"
    ],
  },
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {
      fontFamily: {
        sans: [
          "Fredoka",
          "Segoe UI",
          "Calibri",
          "Helvetica",
          ...defaultTheme.fontFamily.sans,
        ],
      },
    },
  },
  variants: {
    extend: {},
  },
  plugins: [],
};
