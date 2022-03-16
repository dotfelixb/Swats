const defaultTheme = require("tailwindcss/defaultTheme");

module.exports = {
    content: [
        "./Views/**/*.{html,cshtml}",
        "./Shared/**/*.{html,cshtml}"
    ],
    theme: {
        extend: {
            fontFamily: {
                sans: [
                    'Segoe UI',
                    'Calibri',
                    'Helvetica',
                    ...defaultTheme.fontFamily.sans
                ],
            },
        },
    },
    variants: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms')
    ],
}