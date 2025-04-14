import { defineConfig } from "cypress";

export default defineConfig({
  e2e: {
    baseUrl: "http://localhost:4200",
    supportFile: false, // Separate support file for e2e
    specPattern: "cypress/e2e/**/*.cy.ts",
  },

  component: {
    devServer: {
      framework: "angular",
      bundler: "webpack",
    },
    supportFile: false, // Separate support file for component tests
    specPattern: ["cypress/components/**/*.cy.ts", "src/app/modules/**/components/**/*.cy.ts"],
  },
});
