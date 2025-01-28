import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WaterDeficiencyList } from './modules/water-deficiency/components/water-deficiency-list/water-deficiency-list.component';
import { WaterDeficiencyDetail } from './modules/water-deficiency/components/water-deficiency-details/water-deficiency-details.component';
import { SoilDeficiencyList } from './modules/soil-deficiency/components/soil-deficiency-list/soil-deficiency-list.component';
import { SoilDeficiencyDetail } from './modules/soil-deficiency/components/soil-deficiency-details/soil-deficiency-details.component';

const routes: Routes = [
  { path: '', component: WaterDeficiencyList },
  { path: 'water', component: WaterDeficiencyList },
  { path: 'water/add', component: WaterDeficiencyDetail },
  { path: 'water/:id', component: WaterDeficiencyDetail },

  { path: 'soil', component: SoilDeficiencyList },
  { path: 'soil/add', component: SoilDeficiencyDetail },
  { path: 'soil/:id', component: SoilDeficiencyDetail },

  { path: 'labs', component: WaterDeficiencyList },
  { path: 'reports', component: WaterDeficiencyList },
  { path: 'about', component: WaterDeficiencyList },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
