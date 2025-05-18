import { mount } from "cypress/angular";
import { AuthDialogComponent } from "./auth-dialog.component";
import { MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { ReactiveFormsModule } from "@angular/forms";

describe("AuthDialogComponent", () => {
  let mockDialogRef: any;

  beforeEach(() => {
    mockDialogRef = { close: cy.spy() };

    mount(AuthDialogComponent, {
      imports: [MatDialogModule, ReactiveFormsModule],
      providers: [{ provide: MatDialogRef, useValue: mockDialogRef }],
    });
  });

  it("renders correctly", () => {
    cy.get(".dialog-title").should("contain", "Login");
    cy.get("mat-form-field").should("have.length", 2);
  });

  it("validates email field", () => {
    cy.get('input[formControlName="email"]').type("invalid-email").blur();
    cy.get("mat-error", { timeout: 6000 }).should("exist").should("contain", "Please enter a valid email");

    cy.get('input[formControlName="email"]').clear().type("user@example.com");
    cy.get("mat-error").should("not.exist");
  });

  it("validates password field", () => {
    cy.get('input[formControlName="password"]').type("12345").blur();
    cy.get("mat-error", { timeout: 6000 }).should("exist").should("contain", "Password must be at least 6 characters");

    cy.get('input[formControlName="password"]').clear().type("correctpassword");
    cy.get("mat-error").should("not.exist");
  });

  it("submits the form when valid", () => {
    cy.get('input[formControlName="email"]').type("user@example.com");
    cy.get('input[formControlName="password"]').type("password123");
    cy.get(".reset-button").should("not.be.disabled").click();

    cy.wrap(mockDialogRef).its("close").should("have.been.calledWith", {
      email: "user@example.com",
      password: "password123",
    });
  });

  it("closes the dialog on cancel button click", () => {
    cy.get(".apply-button").click();
    cy.wrap(mockDialogRef).its("close").should("have.been.called");
  });
});
