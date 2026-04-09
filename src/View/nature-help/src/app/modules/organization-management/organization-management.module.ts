import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { TranslateModule } from "@ngx-translate/core";

import { MatModule } from "@/mat.module";
import { SharedModule } from "../../shared/shared.module";
import { OrganizationListComponent } from "./components/organization-list/organization-list.component";
import { OrganizationDetailComponent } from "./components/organization-detail/organization-detail.component";

@NgModule({
  imports: [CommonModule, SharedModule, MatModule, ReactiveFormsModule, FormsModule, RouterModule, TranslateModule],
  declarations: [OrganizationListComponent, OrganizationDetailComponent],
  exports: [OrganizationListComponent, OrganizationDetailComponent],
})
export class OrganizationManagementModule {}
