import '@angular/localize/init';
import { mount } from "cypress/angular";
import { AuthDialogComponent } from "./auth-dialog.component";
import { MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

describe("AuthDialogComponent", () => {
  const createMountOptions = (mockDialogRef: any) => ({
    imports: [
      ReactiveFormsModule,
      FormsModule,
      MatDialogModule,
      MatFormFieldModule,
      MatInputModule,
      MatButtonModule,
      BrowserAnimationsModule,
    ],
    providers: [{ provide: MatDialogRef, useValue: mockDialogRef }],
  });

  it("renders correctly", () => {
    mount(AuthDialogComponent, createMountOptions({ close: cy.spy() }));
    cy.get(".dialog-title").should("contain", "Login");
    cy.get("mat-form-field").should("have.length", 2);
  });

  it("validates email field", () => {
    mount(AuthDialogComponent, createMountOptions({ close: cy.spy() }));

    cy.get('input[formControlName="email"]').type("invalid-email").blur();
    cy.get("mat-error").should("contain", "Please enter a valid email");

    cy.get('input[formControlName="email"]').clear().type("user@example.com");
    cy.get("mat-error").should("not.exist");
  });

  it("validates password field", () => {
    mount(AuthDialogComponent, createMountOptions({ close: cy.spy() }));

    cy.get('input[formControlName="password"]').type("12345").blur();
    cy.get("mat-error").should("contain", "Password must be at least 6 characters");

    cy.get('input[formControlName="password"]').clear().type("correctpassword");
    cy.get("mat-error").should("not.exist");
  });

  it("submits the form when valid", () => {
    const mockDialogRef = { close: cy.spy().as("closeSpy") };
    mount(AuthDialogComponent, createMountOptions(mockDialogRef));

    cy.get('input[formControlName="email"]').type("user@example.com");
    cy.get('input[formControlName="password"]').type("password123");

    cy.get('button[type="submit"]').should("not.be.disabled").click();

    cy.get("@closeSpy").should("have.been.calledWith", {
      email: "user@example.com",
      password: "password123",
    });
  });

  it("closes the dialog on cancel button click", () => {
    const mockDialogRef = { close: cy.spy().as("closeSpy") };
    mount(AuthDialogComponent, createMountOptions(mockDialogRef));

    cy.get(".reset-button").click();
    cy.get("@closeSpy").should("have.been.called");
  });
});
