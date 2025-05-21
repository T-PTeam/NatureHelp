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

  it("should fill the form and submit a new water deficiency", () => {
    cy.get('input[formControlName="title"]').type("Test Deficiency");
    cy.get('textarea[formControlName="description"]').type("Test Description");
    cy.get('mat-select[formControlName="eDangerState"]').click();
    cy.get("mat-option").contains("Dangerous").click(); // або інше доступне значення

    cy.get('input[formControlName="latitude"]').type("49.8419");
    cy.get('input[formControlName="longitude"]').type("24.0315");
    cy.get('input[formControlName="radiusAffected"]').type("100");

    cy.get('input[formControlName="ph"]').type("7");
    cy.get('input[formControlName="dissolvedOxygen"]').type("5");
    cy.get('input[formControlName="leadConcentration"]').type("0.005");
    cy.get('input[formControlName="mercuryConcentration"]').type("0.0005");
    cy.get('input[formControlName="nitratesConcentration"]').type("20");
    cy.get('input[formControlName="pesticidesContent"]').type("0.002");
    cy.get('input[formControlName="microbialActivity"]').type("500");
    cy.get('input[formControlName="radiationLevel"]').type("1");
    cy.get('input[formControlName="chemicalOxygenDemand"]').type("150");
    cy.get('input[formControlName="biologicalOxygenDemand"]').type("20");
    cy.get('input[formControlName="phosphateConcentration"]').type("0.5");
    cy.get('input[formControlName="cadmiumConcentration"]').type("0.003");
    cy.get('input[formControlName="totalDissolvedSolids"]').type("900");
    cy.get('input[formControlName="electricalConductivity"]').type("1200");
    cy.get('input[formControlName="microbialLoad"]').type("800");

    cy.get('mat-select[formControlName="responsibleUserId"]').click();
    cy.get("mat-option").first().click(); // або шукай за іменем

    cy.contains("button", "Submit").click();

    // Очікування результату
    cy.url().should("not.include", "/add"); // перенаправлення
  });
});
