import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";

import { MatModule } from "@/mat.module";
import { SharedModule } from "@/shared/shared.module";

import { InfiniteScrollDirective } from "ngx-infinite-scroll";
import { OwnerPageComponent } from "./components/owner-page/owner-page.component";
import { OrganizationUsersTableComponent } from "./components/organization-users-table/organization-users-table.component";

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        BrowserModule,
        FormsModule,
        SharedModule,
        MatModule,
        InfiniteScrollDirective,
    ],
    declarations: [OwnerPageComponent, OrganizationUsersTableComponent],
    exports: [OwnerPageComponent, OrganizationUsersTableComponent],
})
export class OwneryModule {}
