import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { TranslateModule } from "@ngx-translate/core";

import { AppRoutingModule } from "@/app-routing.module";
import { MatModule } from "@/mat.module";

import { SharedModule } from "../../shared/shared.module";
import { MainContainerComponent } from "./components/main-container/main-container.component";
import { NavigationBarComponent } from "./components/navigation-bar/navigation-bar.component";
import { AboutComponent } from "./components/about/about.component";

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
    TranslateModule,
  ],
  declarations: [MainContainerComponent, NavigationBarComponent, AboutComponent],
  exports: [MainContainerComponent, NavigationBarComponent, AboutComponent],
})
export class MainModule {}
