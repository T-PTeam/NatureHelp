import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainContainerComponent } from './components/main-container/main-container.component';
import { SharedModule } from "../../shared/shared.module";
import { AppRoutingModule } from '@/app-routing.module';
import { NavigationBarComponent } from './components/navigation-bar/navigation-bar.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MatModule } from '@/mat.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    AppRoutingModule,
    HttpClientModule,
    RouterModule,
    MatModule,
    ReactiveFormsModule,
    FormsModule
],
  declarations: [
    MainContainerComponent,
    NavigationBarComponent
  ],
  exports: [
    MainContainerComponent,
    NavigationBarComponent
  ]
})
export class MainModule { }


