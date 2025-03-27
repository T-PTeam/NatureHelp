import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";

import { MatModule } from "@/mat.module";
import { SharedModule } from "@/shared/shared.module";

import { LabDetailsComponent } from "./components/lab-details/lab-details.component";
import { LabsTableComponent } from "./components/labs-table/labs-table.component";
import { ResearchTableComponent } from "./components/research-table/research-table.component";

@NgModule({
    imports: [CommonModule, ReactiveFormsModule, BrowserModule, FormsModule, SharedModule, MatModule],
    declarations: [LabsTableComponent, LabDetailsComponent, ResearchTableComponent],
    exports: [LabsTableComponent, LabDetailsComponent, ResearchTableComponent],
})
export class LabsModule {}
