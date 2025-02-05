import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MapComponent } from './components/main-map/main-map.component';
import { FilterPipe } from './pipes/filter.pipe';
import { EnumToStringPipe } from './pipes/enum-to-string.pipe';
import { LoadingService } from './services/loading.service';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    MapComponent,

    FilterPipe,
    EnumToStringPipe
  ],
  exports:[
    MapComponent,

    FilterPipe,
    EnumToStringPipe
  ],
  providers: [
    LoadingService
  ]
})
export class SharedModule { }
