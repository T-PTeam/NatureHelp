import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { TranslateModule } from "@ngx-translate/core";

import { MatModule } from "@/mat.module";
import { SharedModule } from "@/shared/shared.module";

import { LabDetailsComponent } from "./components/lab-details/lab-details.component";
import { LabResearchersDialogComponent } from "./components/lab-researchers-dialog/lab-researchers-dialog.component";
import { LabsTableComponent } from "./components/labs-table/labs-table.component";
import { ResearchTableComponent } from "./components/research-table/research-table.component";
import { InfiniteScrollDirective } from "ngx-infinite-scroll";
import { PickCoordinatesButtonComponent } from "@/shared/components/custom-elements/pick-coordinates-button/pick-coordinates-button.component";

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    BrowserModule,
    FormsModule,
    SharedModule,
    MatModule,
    TranslateModule,
    InfiniteScrollDirective,
    PickCoordinatesButtonComponent,
  ],
  declarations: [LabsTableComponent, LabDetailsComponent, LabResearchersDialogComponent, ResearchTableComponent],
  exports: [LabsTableComponent, LabDetailsComponent, ResearchTableComponent],
})
export class LabsModule {}
