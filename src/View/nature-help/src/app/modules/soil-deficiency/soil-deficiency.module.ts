import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { SoilDeficiencyDetail } from './components/soil-deficiency-details/soil-deficiency-details.component';
import { SoilDeficiencyList } from './components/soil-deficiency-list/soil-deficiency-list.component';
import { SharedModule } from '@/shared/shared.module';
import { MatModule } from '@/mat.module';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    BrowserModule,
    FormsModule,
    SharedModule,
    MatModule
  ],
  declarations: [
    SoilDeficiencyDetail,
    SoilDeficiencyList,
  ],
  exports: [SoilDeficiencyList, SoilDeficiencyDetail],
})
export class SoilDeficiencyModule { }
