import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { MatModule } from "@/mat.module";

import { AuthDialogComponent } from "./components/dialogs/login-dialog/auth-dialog.component";
import { MapComponent } from "./components/main-map/main-map.component";
import { EnumToStringPipe } from "./pipes/enum-to-string.pipe";
import { FilterPipe } from "./pipes/filter.pipe";
import { LoadingService } from "./services/loading.service";

@NgModule({
    imports: [CommonModule, MatModule, ReactiveFormsModule, FormsModule],
    declarations: [MapComponent, AuthDialogComponent, FilterPipe, EnumToStringPipe],
    exports: [MapComponent, AuthDialogComponent, FilterPipe, EnumToStringPipe],
    providers: [LoadingService],
})
export class SharedModule {}
