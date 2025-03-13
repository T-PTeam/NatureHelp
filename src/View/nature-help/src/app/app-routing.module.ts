import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { LabDetailsComponent } from "./modules/laboratories/components/lab-details/lab-details.component";
import { LabsTableComponent } from "./modules/laboratories/components/labs-table/labs-table.component";
import { AboutComponent } from "./modules/main/components/about/about.component";
import { ReportsTableComponent } from "./modules/reports/components/reports-table/reports-table.component";
import { SoilDeficiencyDetail } from "./modules/soil-deficiency/components/soil-deficiency-details/soil-deficiency-details.component";
import { SoilDeficiencyList } from "./modules/soil-deficiency/components/soil-deficiency-list/soil-deficiency-list.component";
import { WaterDeficiencyDetail } from "./modules/water-deficiency/components/water-deficiency-details/water-deficiency-details.component";
import { WaterDeficiencyList } from "./modules/water-deficiency/components/water-deficiency-list/water-deficiency-list.component";
import { UnauthorisedComponent } from "./shared/components/unauthorised/unauthorised.component";
import { RoleGuard } from "./shared/guards/role.guard";
import { OwnerPageComponent } from "./modules/owner/components/owner-page/owner-page.component";

const routes: Routes = [
    { path: "", component: WaterDeficiencyList },

    { path: "water", component: WaterDeficiencyList },
    {
        path: "water/add",
        component: WaterDeficiencyDetail,
        canActivate: [RoleGuard],
        data: { excludeRoles: ["researcher", "guest"] },
    },
    {
        path: "water/:id",
        component: WaterDeficiencyDetail,
        canActivate: [RoleGuard],
        data: { excludeRoles: ["researcher", "guest"] },
    },

    { path: "soil", component: SoilDeficiencyList },
    {
        path: "soil/add",
        component: SoilDeficiencyDetail,
        canActivate: [RoleGuard],
        data: { excludeRoles: ["researcher", "guest"] },
    },
    {
        path: "soil/:id",
        component: SoilDeficiencyDetail,
        canActivate: [RoleGuard],
        data: { excludeRoles: ["researcher", "guest"] },
    },

    {
        path: "labs",
        component: LabsTableComponent,
        canActivate: [RoleGuard],
        data: { includeRoles: ["researcher", "owner"] },
    },
    {
        path: "labs/:id",
        component: LabDetailsComponent,
        canActivate: [RoleGuard],
        data: { includeRoles: ["researcher", "owner"] },
    },

    {
        path: "reports",
        component: ReportsTableComponent,
        canActivate: [RoleGuard],
        data: { includeRoles: ["manager", "owner"] },
    },
    {
        path: "reports/:id",
        component: ReportsTableComponent,
        canActivate: [RoleGuard],
        data: { includeRoles: ["manager", "owner"] },
    },

    {
        path: "owner",
        component: OwnerPageComponent,
        canActivate: [RoleGuard],
        data: { includeRoles: ["owner"] },
    },

    { path: "about", component: AboutComponent },

    { path: "unauthorized", component: UnauthorisedComponent },
    { path: "**", redirectTo: "/unauthorized" },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {}
