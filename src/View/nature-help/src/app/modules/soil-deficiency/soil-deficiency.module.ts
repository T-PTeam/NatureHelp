import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { MatModule } from '@/mat.module';
import { SharedModule } from '@/shared/shared.module';

import { SoilDeficiencyDetail } from './components/soil-deficiency-details/soil-deficiency-details.component';
import { SoilDeficiencyList } from './components/soil-deficiency-list/soil-deficiency-list.component';

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
