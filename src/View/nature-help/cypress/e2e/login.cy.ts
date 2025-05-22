describe("Login Dialog", () => {
  it("should log in successfully via the login dialog", () => {
    cy.visit("/");

    cy.get('[data-cy="auth-menu"]').click();
    cy.get('[data-cy="login-button"]').click();

    cy.get('input[formcontrolname="email"]').type("test@example.com");
    cy.get('input[formcontrolname="password"]').type("TestPassword123");

    cy.get('button[type="submit"]').contains("Login").click();

    cy.get("mat-dialog-container").should("not.exist"); // перевірка, що діалог закрився
  });
});
