describe("Add Water Deficiency", () => {
  beforeEach(() => {
    cy.visit("/");

    // 2. Відкрити діалог (залежить від вашого компонента, наприклад, натиснення кнопки входу)
    cy.get('[data-cy="auth-menu"]').click();
    cy.get('[data-cy="login-button"]').click();

    // 3. Ввести email і пароль
    cy.get('input[formcontrolname="email"]').type("valentyn@example.com");
    cy.get('input[formcontrolname="password"]').type("12341234");

    // 4. Натиснути кнопку "Login"
    cy.get('button[type="submit"]').contains("Login").click();

    // 5. Перевірити, що діалог закрився і користувач перейшов у систему
    cy.get("mat-dialog-container").should("not.exist"); // перевірка, що діалог закрився

    cy.wait(2000);
    cy.visit("/water/add");
  });
  it("should contain history of changes", () => {
    const deficiencyId = "c3333333-3333-3333-3333-333333333333"; // заміни на існуючий id або динамічно отримай

    // Відкрити сторінку деталей
    cy.visit(`/water/${deficiencyId}`);

    // Переконатися, що історія завантажилась і містить записи
    cy.get('[data-cy="history-expansion-panel"]') // клас або селектор для історії змін
      .should("exist")
      .click();
    cy.get(".log-entry") // дочірні елементи списку
      .should("have.length.greaterThan", 0); // має бути хоча б 1 запис
  });
});
