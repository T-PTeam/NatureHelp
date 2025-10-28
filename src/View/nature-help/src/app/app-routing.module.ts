import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { LabDetailsComponent } from "./modules/laboratories/components/lab-details/lab-details.component";
import { LabsTableComponent } from "./modules/laboratories/components/labs-table/labs-table.component";
import { AboutComponent } from "./modules/main/components/about/about.component";
import { SoilDeficiencyDetail } from "./modules/soil-deficiency/components/soil-deficiency-details/soil-deficiency-details.component";
import { SoilDeficiencyTable } from "./modules/soil-deficiency/components/soil-deficiency-table/soil-deficiency-table.component";
import { WaterDeficiencyDetail } from "./modules/water-deficiency/components/water-deficiency-details/water-deficiency-details.component";
import { WaterDeficiencyTable } from "./modules/water-deficiency/components/water-deficiency-table/water-deficiency-table.component";
import { UnauthorisedComponent } from "./shared/components/unauthorised/unauthorised.component";
import { RoleGuard } from "./shared/guards/role.guard";
import { LaboratoryOwnerGuard } from "./shared/guards/laboratory-owner.guard";
import { LanguageGuard } from "./shared/guards/language.guard";
import { OrganizationUsersTableComponent } from "./modules/owner/components/organization-users-table/organization-users-table.component";
import { ResearchTableComponent } from "./modules/laboratories/components/research-table/research-table.component";
import { EmailConfirmationComponent } from "./shared/components/email-confirmation/email-confirmation.component";
import { PasswordResetDialogComponent } from "./shared/components/dialogs/login-dialog/password-reset-dialog.component";
import { OrganizationListComponent } from "./modules/organization-management/components/organization-list/organization-list.component";
import { OrganizationDetailComponent } from "./modules/organization-management/components/organization-detail/organization-detail.component";
import { PrivacyPolicyComponent } from "./shared/components/privacy-policy/privacy-policy.component";
import { ContactsComponent } from "./shared/components/contacts/contacts.component";

const routes: Routes = [
  { path: "", component: WaterDeficiencyTable },
  { path: "uk", component: WaterDeficiencyTable },
  { path: "en", component: WaterDeficiencyTable },

  { path: "water", component: WaterDeficiencyTable },
  { path: "uk/water", component: WaterDeficiencyTable },
  { path: "en/water", component: WaterDeficiencyTable },
  {
    path: "water/add",
    component: WaterDeficiencyDetail,
    canActivate: [RoleGuard],
    data: { excludeRoles: ["researcher", "guest"] },
  },
  {
    path: "uk/water/add",
    component: WaterDeficiencyDetail,
    canActivate: [RoleGuard],
    data: { excludeRoles: ["researcher", "guest"] },
  },
  {
    path: "en/water/add",
    component: WaterDeficiencyDetail,
    canActivate: [RoleGuard],
    data: { excludeRoles: ["researcher", "guest"] },
  },
  {
    path: "water/:id",
    component: WaterDeficiencyDetail,
    canActivate: [RoleGuard],
    data: { excludeRoles: ["researcher", "guest"] },
  },
  {
    path: "uk/water/:id",
    component: WaterDeficiencyDetail,
    canActivate: [RoleGuard],
    data: { excludeRoles: ["researcher", "guest"] },
  },
  {
    path: "en/water/:id",
    component: WaterDeficiencyDetail,
    canActivate: [RoleGuard],
    data: { excludeRoles: ["researcher", "guest"] },
  },

  { path: "soil", component: SoilDeficiencyTable },
  { path: "uk/soil", component: SoilDeficiencyTable },
  { path: "en/soil", component: SoilDeficiencyTable },
  {
    path: "soil/add",
    component: SoilDeficiencyDetail,
    canActivate: [RoleGuard],
    data: { excludeRoles: ["researcher", "guest"] },
  },
  {
    path: "uk/soil/add",
    component: SoilDeficiencyDetail,
    canActivate: [RoleGuard],
    data: { excludeRoles: ["researcher", "guest"] },
  },
  {
    path: "en/soil/add",
    component: SoilDeficiencyDetail,
    canActivate: [RoleGuard],
    data: { excludeRoles: ["researcher", "guest"] },
  },
  {
    path: "soil/:id",
    component: SoilDeficiencyDetail,
    canActivate: [RoleGuard],
    data: { excludeRoles: ["researcher", "guest"] },
  },
  {
    path: "uk/soil/:id",
    component: SoilDeficiencyDetail,
    canActivate: [RoleGuard],
    data: { excludeRoles: ["researcher", "guest"] },
  },
  {
    path: "en/soil/:id",
    component: SoilDeficiencyDetail,
    canActivate: [RoleGuard],
    data: { excludeRoles: ["researcher", "guest"] },
  },

  {
    path: "labs",
    component: LabsTableComponent,
  },
  {
    path: "uk/labs",
    component: LabsTableComponent,
  },
  {
    path: "en/labs",
    component: LabsTableComponent,
  },
  {
    path: "labs/add",
    component: LabDetailsComponent,
    canActivate: [RoleGuard],
    data: { includeRoles: ["researcher", "owner"] },
  },
  {
    path: "uk/labs/add",
    component: LabDetailsComponent,
    canActivate: [RoleGuard],
    data: { includeRoles: ["researcher", "owner"] },
  },
  {
    path: "en/labs/add",
    component: LabDetailsComponent,
    canActivate: [RoleGuard],
    data: { includeRoles: ["researcher", "owner"] },
  },
  {
    path: "labs/:id",
    component: LabDetailsComponent,
    canActivate: [RoleGuard, LaboratoryOwnerGuard],
    data: { includeRoles: ["researcher", "owner"] },
  },
  {
    path: "uk/labs/:id",
    component: LabDetailsComponent,
    canActivate: [RoleGuard, LaboratoryOwnerGuard],
    data: { includeRoles: ["researcher", "owner"] },
  },
  {
    path: "en/labs/:id",
    component: LabDetailsComponent,
    canActivate: [RoleGuard, LaboratoryOwnerGuard],
    data: { includeRoles: ["researcher", "owner"] },
  },
  // {
  //   path: "researches",
  //   component: ResearchTableComponent,
  // },

  {
    path: "owner",
    component: OrganizationUsersTableComponent,
    canActivate: [RoleGuard],
    data: { includeRoles: ["owner"] },
  },

  {
    path: "organizations",
    component: OrganizationListComponent,
    canActivate: [RoleGuard],
    data: { includeRoles: ["superadmin"] },
  },
  {
    path: "organizations/add",
    component: OrganizationDetailComponent,
    canActivate: [RoleGuard],
    data: { includeRoles: ["superadmin"] },
  },
  {
    path: "organizations/:id",
    component: OrganizationDetailComponent,
    canActivate: [RoleGuard],
    data: { includeRoles: ["superadmin"] },
  },

  { path: "about", component: AboutComponent },
  { path: "uk/about", component: AboutComponent },
  { path: "en/about", component: AboutComponent },

  { path: "privacy", component: PrivacyPolicyComponent },
  { path: "uk/privacy", component: PrivacyPolicyComponent },
  { path: "en/privacy", component: PrivacyPolicyComponent },

  { path: "contacts", component: ContactsComponent },
  { path: "uk/contacts", component: ContactsComponent },
  { path: "en/contacts", component: ContactsComponent },

  { path: "confirm-email", component: EmailConfirmationComponent },
  { path: "uk/confirm-email", component: EmailConfirmationComponent },
  { path: "en/confirm-email", component: EmailConfirmationComponent },
  { path: "password-reset", component: PasswordResetDialogComponent },
  { path: "uk/password-reset", component: PasswordResetDialogComponent },
  { path: "en/password-reset", component: PasswordResetDialogComponent },

  { path: "unauthorized", component: UnauthorisedComponent },
  { path: "uk/unauthorized", component: UnauthorisedComponent },
  { path: "en/unauthorized", component: UnauthorisedComponent },
  { path: "**", redirectTo: "/unauthorized" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
