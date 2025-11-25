/** @type {import('tailwindcss').Config} */

// Helper to allow opacity modifiers: bg-primary/50
function hsl(variableName) {
  return `hsl(var(${variableName}) / <alpha-value>)`;
}

module.exports = {
  presets: [require('@spartan-ng/brain/hlm-tailwind-preset')],
  content: ['./src/**/*.{html,ts}', './libs/ui/**/*.{html,ts}'],
  theme: {
    extend: {
      colors: {
        background: {
          DEFAULT: hsl('--background'),
          hover: hsl('--background-hover'),
          active: hsl('--background-active'),
        },
        foreground: {
          DEFAULT: hsl('--foreground'),
          secondary: hsl('--foreground-secondary'),
          hover: hsl('--foreground-hover'),
          active: hsl('--foreground-active'),
        },
        card: {
          DEFAULT: hsl('--card'),
          foreground: hsl('--card-foreground'),
        },
        popover: {
          DEFAULT: hsl('--popover'),
          foreground: hsl('--popover-foreground'),
        },
        primary: {
          DEFAULT: hsl('--primary'),
          foreground: hsl('--primary-foreground'),
        },
        secondary: {
          DEFAULT: hsl('--secondary'),
          foreground: hsl('--secondary-foreground'),
        },
        muted: {
          DEFAULT: hsl('--muted'),
          foreground: hsl('--muted-foreground'),
        },
        accent: {
          DEFAULT: hsl('--accent'),
          foreground: hsl('--accent-foreground'),
        },
        destructive: {
          DEFAULT: hsl('--destructive'),
          foreground: hsl('--destructive-foreground'),
        },
        border: hsl('--border'),
        input: hsl('--input'),
        ring: hsl('--ring'),

        // Custom Jira Colors
        board: {
          col: hsl('--board-col-bg'),
          card: hsl('--board-card-bg'),
        },
        dialog: {
          DEFAULT: hsl('--dialog-bg'),
        },
      },
    },
  },
  plugins: [],
};
