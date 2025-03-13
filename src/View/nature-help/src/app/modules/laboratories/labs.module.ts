import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";

import { MatModule } from "@/mat.module";
import { SharedModule } from "@/shared/shared.module";

import { LabDetailsComponent } from "./components/lab-details/lab-details.component";
import { LabsTableComponent } from "./components/labs-table/labs-table.component";

@NgModule({
    imports: [CommonModule, ReactiveFormsModule, BrowserModule, FormsModule, SharedModule, MatModule],
    declarations: [LabsTableComponent, LabDetailsComponent],
    exports: [LabsTableComponent, LabDetailsComponent],
})
export class LabsModule {}
