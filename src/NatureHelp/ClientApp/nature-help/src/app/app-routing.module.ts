import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GasStationDetailsComponent } from './components/water-deficiency-details/water-deficiency-details.component';
import { GasStationsListComponent } from './components/water-deficiency-list/water-deficiency-list.component';

const routes: Routes = [
  { path: '', component: GasStationsListComponent },
  { path: 'add', component: GasStationDetailsComponent },
  { path: 'edit/:id', component: GasStationDetailsComponent },
  { path: 'remove/:id', component: GasStationDetailsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
