/** @type {import('tailwindcss').Config} */
module.exports = {
  presets: [require("@spartan-ng/brain/hlm-tailwind-preset")],
  content: ["./src/**/*.{html,ts}", "./libs/ui/**/*.{html,ts}"],

  theme: {
    extend: {
      colors: {
        "board-column-background": "var(--board-column-background)",
        "board-column-card-background": "var(--board-column-card-background)",
        "dialog-background" :"var(--dialog-background)",
        "drop-down-background": "var(--drop-down-background)",
        "dialog-input-background": "var(--dialog-input-background)",
        "drop-down-selected-background": "var(--drop-down-selected-background)"
      },
    },
  },
  plugins: [],
};
