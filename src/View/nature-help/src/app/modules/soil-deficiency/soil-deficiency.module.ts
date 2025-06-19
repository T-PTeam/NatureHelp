import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";

import { MatModule } from "@/mat.module";
import { SharedModule } from "@/shared/shared.module";

import { SoilDeficiencyDetail } from "./components/soil-deficiency-details/soil-deficiency-details.component";
import { SoilDeficiencyTable } from "./components/soil-deficiency-table/soil-deficiency-table.component";
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
    InfiniteScrollDirective,
    PickCoordinatesButtonComponent,
  ],
  declarations: [SoilDeficiencyDetail, SoilDeficiencyTable],
  exports: [SoilDeficiencyTable, SoilDeficiencyDetail],
})
export class SoilDeficiencyModule {}
