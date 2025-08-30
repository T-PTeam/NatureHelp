import { defineConfig } from "cypress";

export default defineConfig({
  e2e: {
    viewportWidth: 1920,
    viewportHeight: 1080,

    baseUrl: "http://localhost:4200",
    supportFile: false,
    specPattern: "cypress/e2e/**/*.cy.ts",
  },

  component: {
    viewportWidth: 1920,
    viewportHeight: 1080,

    devServer: {
      framework: "angular",
      bundler: "webpack",
    },
    supportFile: false,
    specPattern: ["cypress/components/**/*.cy.ts", "src/app/**/*.cy.ts"],
  },
});
