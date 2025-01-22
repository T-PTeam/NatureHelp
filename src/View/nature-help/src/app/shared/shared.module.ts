import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MapComponent } from './components/main-map/main-map.component';
import { FilterPipe } from './pipes/filter.pipe';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    MapComponent,
    

    FilterPipe,
  ],
  exports:[
    MapComponent,
    
    FilterPipe
  ]
})
export class SharedModule { }
