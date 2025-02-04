import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WaterDeficiencyList } from './modules/water-deficiency/components/water-deficiency-list/water-deficiency-list.component';
import { WaterDeficiencyDetail } from './modules/water-deficiency/components/water-deficiency-details/water-deficiency-details.component';
import { SoilDeficiencyList } from './modules/soil-deficiency/components/soil-deficiency-list/soil-deficiency-list.component';
import { SoilDeficiencyDetail } from './modules/soil-deficiency/components/soil-deficiency-details/soil-deficiency-details.component';
import { AboutComponent } from './modules/main/components/about/about.component';
import { LabsTableComponent } from './modules/laboratories/components/labs-table/labs-table.component';
import { LabDetailsComponent } from './modules/laboratories/components/lab-details/lab-details.component';
import { ReportsTableComponent } from './modules/reports/components/reports-table/reports-table.component';

const routes: Routes = [
  { path: '', component: WaterDeficiencyList },
  { path: 'water', component: WaterDeficiencyList },
  { path: 'water/add', component: WaterDeficiencyDetail },
  { path: 'water/:id', component: WaterDeficiencyDetail },

  { path: 'soil', component: SoilDeficiencyList },
  { path: 'soil/add', component: SoilDeficiencyDetail },
  { path: 'soil/:id', component: SoilDeficiencyDetail },

  { path: 'labs', component: LabsTableComponent },
  { path: 'labs/:id', component: LabDetailsComponent },

  { path: 'reports', component: ReportsTableComponent },
  { path: 'reports/:id', component: ReportsTableComponent },

  { path: 'about', component: AboutComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
