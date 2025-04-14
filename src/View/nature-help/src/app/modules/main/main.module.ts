import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";

import { AppRoutingModule } from "@/app-routing.module";
import { MatModule } from "@/mat.module";

import { SharedModule } from "../../shared/shared.module";
import { MainContainerComponent } from "./components/main-container/main-container.component";
import { NavigationBarComponent } from "./components/navigation-bar/navigation-bar.component";

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    AppRoutingModule,
    HttpClientModule,
    RouterModule,
    MatModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  declarations: [MainContainerComponent, NavigationBarComponent],
  exports: [MainContainerComponent, NavigationBarComponent],
})
export class MainModule {}
