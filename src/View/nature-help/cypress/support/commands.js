/// <reference types="cypress" />

Cypress.Commands.add("loginByApi", () => {
  cy.request({
    method: "POST",
    url: "/api/auth/login",
    body: {
      email: "test@example.com",
      password: "Test1234!",
    },
  }).then((resp) => {
    sessionStorage.setItem("accessToken", resp.body.accessToken);
  });
});
