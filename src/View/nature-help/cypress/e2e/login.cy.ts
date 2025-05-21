describe("Login Dialog", () => {
  it("should log in successfully via the login dialog", () => {
    // 1. Відкрити головну сторінку (або ту, з якої викликається діалог)
    cy.visit("/");

    // 2. Відкрити діалог (залежить від вашого компонента, наприклад, натиснення кнопки входу)
    cy.get('[data-cy="auth-menu"]').click();
    cy.get('[data-cy="login-button"]').click();

    // 3. Ввести email і пароль
    cy.get('input[formcontrolname="email"]').type("test@example.com");
    cy.get('input[formcontrolname="password"]').type("TestPassword123");

    // 4. Натиснути кнопку "Login"
    cy.get('button[type="submit"]').contains("Login").click();

    // 5. Перевірити, що діалог закрився і користувач перейшов у систему
    cy.get("mat-dialog-container").should("not.exist"); // перевірка, що діалог закрився
  });
});
