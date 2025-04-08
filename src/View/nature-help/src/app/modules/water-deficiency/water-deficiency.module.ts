import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";

import { MatModule } from "@/mat.module";
import { SharedModule } from "@/shared/shared.module";

import { WaterDeficiencyDetail } from "./components/water-deficiency-details/water-deficiency-details.component";
import { WaterDeficiencyTable } from "./components/water-deficiency-table/water-deficiency-table.component";
import { InfiniteScrollDirective } from "ngx-infinite-scroll";

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    BrowserModule,
    FormsModule,
    MatModule,
    SharedModule,
    InfiniteScrollDirective,
  ],
  declarations: [WaterDeficiencyDetail, WaterDeficiencyTable],
  exports: [WaterDeficiencyTable, WaterDeficiencyDetail],
})
export class WaterDeficiencyModule { }
