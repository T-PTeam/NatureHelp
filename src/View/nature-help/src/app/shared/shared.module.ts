import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { TranslateModule } from "@ngx-translate/core";

import { MatModule } from "@/mat.module";

import { AuthDialogComponent } from "./components/dialogs/login-dialog/auth-dialog.component";
import { PostAuthWelcomeDialogComponent } from "./components/dialogs/post-auth-welcome-dialog/post-auth-welcome-dialog.component";
import { PasswordResetDialogComponent } from "./components/dialogs/login-dialog/password-reset-dialog.component";
import { SendResetLinkDialogComponent } from "./components/dialogs/login-dialog/send-reset-link-dialog.component";
import { MapComponent } from "./components/main-map/main-map.component";
import { EnumToStringPipe } from "./pipes/enum-to-string.pipe";
import { LoadingService } from "./services/loading.service";
import { ReportAPIService } from "./services/report-api.service";
import { RoleStringPipe } from "./pipes/role-string.pipe";
import { LabResearchersPipe } from "./pipes/lab-researchers.pipe";
import { AddOrganizationUsersComponent } from "./components/dialogs/add-organization-users/add-organization-users.component";
import { MapViewService } from "./services/map-view.service";
import { AuditListComponent } from "./components/audit-list/audit-list.component";
import { AttachmentListComponent } from "./components/attachment-list/attachment-list.component";
import { FileUploadComponent } from "./components/file-upload/file-upload.component";
import { AttachmentPreviewDialogComponent } from "./components/dialogs/attachment-preview-dialog/attachment-preview-dialog.component";
import { CommentMessageListComponent } from "./components/comment-message-list/comment-message-list.component";
import { EmailConfirmationComponent } from "./components/email-confirmation/email-confirmation.component";
import { WaterSoilToggleComponent } from "./components/buttons/water-soil-toggle/water-soil-toggle.component";
import { DonationComponent } from "./components/donation/donation.component";
import { LegalInfoComponent } from "./components/legal-info/legal-info.component";
import { FooterComponent } from "./components/footer/footer.component";
import { LanguageSwitcherComponent } from "./components/language-switcher/language-switcher.component";
import { LoadingIndicatorComponent } from "./components/loading-indicator/loading-indicator.component";
import { TruncatedCellComponent } from "./components/truncated-cell/truncated-cell.component";
import { LoginPromptSnackbarComponent } from "./components/login-prompt-snackbar/login-prompt-snackbar.component";

@NgModule({
  imports: [CommonModule, MatModule, ReactiveFormsModule, FormsModule, WaterSoilToggleComponent, TranslateModule],
  declarations: [
    MapComponent,
    AuthDialogComponent,
    PostAuthWelcomeDialogComponent,
    PasswordResetDialogComponent,
    SendResetLinkDialogComponent,
    AddOrganizationUsersComponent,
    AuditListComponent,
    AttachmentListComponent,
    FileUploadComponent,
    AttachmentPreviewDialogComponent,
    CommentMessageListComponent,
    EmailConfirmationComponent,
    DonationComponent,
    LegalInfoComponent,
    FooterComponent,
    LanguageSwitcherComponent,
    LoadingIndicatorComponent,
    TruncatedCellComponent,
    LoginPromptSnackbarComponent,

    EnumToStringPipe,
    RoleStringPipe,
    LabResearchersPipe,
  ],
  exports: [
    MapComponent,
    AuthDialogComponent,
    PostAuthWelcomeDialogComponent,
    PasswordResetDialogComponent,
    SendResetLinkDialogComponent,
    AddOrganizationUsersComponent,
    AuditListComponent,
    AttachmentListComponent,
    FileUploadComponent,
    AttachmentPreviewDialogComponent,
    CommentMessageListComponent,
    EmailConfirmationComponent,
    WaterSoilToggleComponent,
    DonationComponent,
    LegalInfoComponent,
    FooterComponent,
    LanguageSwitcherComponent,
    LoadingIndicatorComponent,
    TruncatedCellComponent,

    EnumToStringPipe,
    RoleStringPipe,
    LabResearchersPipe,
  ],
  providers: [LoadingService, ReportAPIService, MapViewService],
})
export class SharedModule {}
