import { mount } from "cypress/angular";
import { AuthDialogComponent } from "./auth-dialog.component";
import { MatDialogModule, MatDialogRef, MatDialog } from "@angular/material/dialog";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { TranslateModule, TranslateService } from "@ngx-translate/core";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";
import { TranslateLoader } from "@ngx-translate/core";

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, "./assets/i18n/", ".json");
}

describe("AuthDialogComponent", () => {
  beforeEach(() => {
    cy.intercept("GET", "**/assets/i18n/*.json", { statusCode: 200, body: {} }).as("i18n");
  });

  const createMountOptions = (mockDialogRef: any) => ({
    imports: [
      ReactiveFormsModule,
      FormsModule,
      MatDialogModule,
      MatFormFieldModule,
      MatInputModule,
      MatButtonModule,
      MatIconModule,
      BrowserAnimationsModule,
      HttpClientModule,
      TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient],
        },
        defaultLanguage: "uk",
      }),
    ],
    providers: [
      { provide: MatDialogRef, useValue: mockDialogRef },
      { provide: MatDialog, useValue: { open: cy.stub() } },
      TranslateService,
    ],
  });

  it("renders correctly", () => {
    mount(AuthDialogComponent, createMountOptions({ close: cy.spy() }));
    cy.wait("@i18n");
    cy.get(".dialog-title", { timeout: 5000 }).should("exist");
    cy.get("mat-form-field", { timeout: 5000 }).should("have.length", 2);
  });

  it("validates email field", () => {
    mount(AuthDialogComponent, createMountOptions({ close: cy.spy() }));
    cy.wait("@i18n");

    cy.get('input[formControlName="email"]', { timeout: 5000 }).type("invalid-email").blur();
    cy.get("mat-error", { timeout: 5000 }).should("exist");

    cy.get('input[formControlName="email"]').clear().type("user@example.com");
    cy.get("mat-error").should("not.exist");
  });

  it("validates password field", () => {
    mount(AuthDialogComponent, createMountOptions({ close: cy.spy() }));
    cy.wait("@i18n");

    cy.get('input[formControlName="password"]', { timeout: 5000 }).type("12345").blur();
    cy.get("mat-error", { timeout: 5000 }).should("exist");

    cy.get('input[formControlName="password"]').clear().type("correctpassword");
    cy.get("mat-error").should("not.exist");
  });

  it("submits the form when valid", () => {
    const mockDialogRef = { close: cy.spy().as("closeSpy") };
    mount(AuthDialogComponent, createMountOptions(mockDialogRef));
    cy.wait("@i18n");

    cy.get('input[formControlName="email"]', { timeout: 5000 }).type("user@example.com");
    cy.get('input[formControlName="password"]').type("password123");

    cy.get('button[type="submit"]', { timeout: 5000 }).should("not.be.disabled").click();

    cy.get("@closeSpy").should("have.been.calledWith", {
      email: "user@example.com",
      password: "password123",
    });
  });

  it("closes the dialog on cancel button click", () => {
    const mockDialogRef = { close: cy.spy().as("closeSpy") };
    mount(AuthDialogComponent, createMountOptions(mockDialogRef));
    cy.wait("@i18n");

    cy.get(".reset-button", { timeout: 5000 }).click();
    cy.get("@closeSpy").should("have.been.called");
  });
});
