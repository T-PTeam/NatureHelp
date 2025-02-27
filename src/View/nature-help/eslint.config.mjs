import pluginJs from "@eslint/js";
import tseslint from "@typescript-eslint/eslint-plugin";
import tsParser from "@typescript-eslint/parser";
import cypress from "eslint-plugin-cypress";
import importPlugin from "eslint-plugin-import";
import simpleImportSort from "eslint-plugin-simple-import-sort";
import unicorn from "eslint-plugin-unicorn";
import * as espree from "espree"; 
import globals from "globals";
import prettier from "eslint-plugin-prettier";
import angular from "eslint-plugin-angular";

/** @type {import('eslint').FlatConfig[]} */
export default [
  {
    ignores: [
      "cypress.config.ts",
      "cypress/**/*.ts",
      "eslint.config.mjs",
      "prettier.config.js",
      "lint-staged.config.js",
      "postcss.config.js",
      ".angular/cache/**"
    ],
  },
  {
    ignores: [
      ".angular/cache/**",
      "vite/deps/**",
      "node_modules/**",
      "dist/**",
      "build/**"
    ],
    files: ["**/I*.ts"], // Match only interface files
    rules: {
      "unicorn/filename-case": "off",
    },
  },
  {
    ignores: [
      ".angular/cache/**",
      "vite/deps/**",
      "node_modules/**",
      "dist/**",
      "build/**"
    ],
    files: ["**/*.ts", "**/*.tsx"],
    languageOptions: {
      parser: tsParser,
      ecmaVersion: "latest",
      sourceType: "module",
    },
    plugins: {
      unicorn,
    },
    rules: {
      "unicorn/filename-case": [
        "error",
        {
          cases: {
            kebabCase: true,
          },
        },
      ],
    },
  },
  {
    ignores: [
      ".angular/cache/**",
      "vite/deps/**",
      "node_modules/**",
      "dist/**",
      "build/**"
    ],
    files: ["**/*.{js,mjs,cjs,ts}"],
    languageOptions: {
      globals: {
        ...globals.browser,  // Spread browser globals from `globals` package
        module: "readonly",  // Allow `module` to be used without errors
      },
      parser: tsParser,  // Use TypeScript parser
      parserOptions: {
        ecmaVersion: "latest",  // Use the latest ECMAScript version
        sourceType: "module",  // Use modules (ESM)
        project: "./tsconfig.json",  // Path to your TypeScript config
      },
    },
    plugins: {
      "@typescript-eslint": tseslint,
      import: importPlugin,
      "simple-import-sort": simpleImportSort,
      unicorn: unicorn,
      cypress: cypress,
      prettier: prettier,
      angular: angular,
    },
    rules: {
      ...pluginJs.configs.recommended.rules,  // ESLint recommended rules
      ...tseslint.configs.recommended.rules, // TypeScript plugin recommended rules

      // Import Sorting
      "simple-import-sort/imports": "error",
      "simple-import-sort/exports": "error",
      "import/first": "error",
      "import/newline-after-import": "error",
      "import/namespace": ["error", { allowComputed: true }],
      "@typescript-eslint/no-explicit-any": "off",
      "@typescript-eslint/no-unused-vars": ["error"],

      "unicorn/no-empty-file": "off", // Disable empty file rule
      "unicorn/filename-case": "off",

      // Cypress Rules
      "cypress/no-unnecessary-waiting": "warn",  // Warn about unnecessary waiting
    },
  },
  {
    files: ["tailwind.config.js"],
    languageOptions: {
      parser: espree, // Use the default JavaScript parser for non-TypeScript files
    },
    rules: {}, // Tailwind config specific rules if any
  },
  {
    files: ["*rc.ts", "*.config.ts"],  // Override for specific TypeScript files
    rules: {
      "unicorn/prefer-module": "off",  // Turn off the prefer-module rule
      "unicorn/filename-case": "off",  // Turn off the filename-case rule
    },
  },
  {
    ignores: ["lint-staged.config.js", "postcss.config.js"]
  }
];
