import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { SharedModule } from '@/shared/shared.module';
import { MatModule } from '@/mat.module';
import { LabsTableComponent } from './components/labs-table/labs-table.component';
import { LabDetailsComponent } from './components/lab-details/lab-details.component';

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
    LabsTableComponent,
    LabDetailsComponent
  ],
  exports: [
    LabsTableComponent,
    LabDetailsComponent
  ],
})
export class LabsModule { }
