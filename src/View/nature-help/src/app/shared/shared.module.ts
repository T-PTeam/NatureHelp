import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { MatModule } from "@/mat.module";

import { AuthDialogComponent } from "./components/dialogs/login-dialog/auth-dialog.component";
import { MapComponent } from "./components/main-map/main-map.component";
import { EnumToStringPipe } from "./pipes/enum-to-string.pipe";
import { FilterPipe } from "./pipes/filter.pipe";
import { LoadingService } from "./services/loading.service";
import { ReportAPIService } from "./services/report-api.service";
import { RoleStringPipe } from "./pipes/role-string.pipe";
import { LabResearchersPipe } from "./pipes/lab-researchers.pipe";

@NgModule({
    imports: [CommonModule, MatModule, ReactiveFormsModule, FormsModule],
    declarations: [MapComponent, AuthDialogComponent, FilterPipe, EnumToStringPipe, RoleStringPipe, LabResearchersPipe],
    exports: [MapComponent, AuthDialogComponent, FilterPipe, EnumToStringPipe, RoleStringPipe, LabResearchersPipe],
    providers: [LoadingService, ReportAPIService],
})
export class SharedModule {}
