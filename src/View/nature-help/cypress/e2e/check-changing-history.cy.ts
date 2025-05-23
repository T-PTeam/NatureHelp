describe("Add Water Deficiency", () => {
  beforeEach(() => {
    cy.visit("/");

    cy.get('[data-cy="auth-menu"]').click();
    cy.get('[data-cy="login-button"]').click();

    cy.get('input[formcontrolname="email"]').type("valentyn@example.com");
    cy.get('input[formcontrolname="password"]').type("12341234");

    cy.get('button[type="submit"]').contains("Login").click();

    cy.get("mat-dialog-container").should("not.exist");

    cy.wait(2000);
    cy.visit("/water/add");
  });
  it("should contain history of changes", () => {
    const deficiencyId = "c3333333-3333-3333-3333-333333333333";

    cy.visit(`/water/${deficiencyId}`);

    cy.get('[data-cy="history-expansion-panel"]').should("exist").click();
    cy.get(".log-entry").should("have.length.greaterThan", 0);
  });
});
