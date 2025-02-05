import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { WaterDeficiencyDetail } from './components/water-deficiency-details/water-deficiency-details.component';
import { WaterDeficiencyList } from './components/water-deficiency-list/water-deficiency-list.component';
import { SharedModule } from '@/shared/shared.module';
import { MatModule } from '@/mat.module';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    BrowserModule,
    FormsModule,
    MatModule,
    SharedModule
  ],
  declarations: [
    WaterDeficiencyDetail,
    WaterDeficiencyList
  ],
  exports: [WaterDeficiencyList, WaterDeficiencyDetail],
})
export class WaterDeficiencyModule { }
