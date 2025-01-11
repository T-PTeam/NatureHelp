import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppComponent } from './app.component';
import { BrowserModule } from '@angular/platform-browser';

import { MainMapComponent } from './components/main-map/main-map.component';
import { GoogleMapsModule } from '@angular/google-maps';
import { MainContainerComponent } from './containers/main-container/main-container.component';
import { GasStationsListComponent } from './components/water-deficiency-list/water-deficiency-list.component';
import { GasStationDetailsComponent } from './components/water-deficiency-details/water-deficiency-details.component';
import { AppRoutingModule } from './app-routing.module';
import { DataSetService } from './services/data-set.service';
import { FormsModule } from '@angular/forms';
import { MapViewService } from './services/map-view.service';
import { FilterPipe } from './shared/pipes/filter.pipe';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    GoogleMapsModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
  ],
  declarations: [
    AppComponent,

    MainContainerComponent,
    MainMapComponent,
    GasStationsListComponent,
    GasStationDetailsComponent,

    FilterPipe,
  ],
  bootstrap: [AppComponent],
  providers: [
    DataSetService,
    MapViewService
  ]
})
export class AppModule { }
