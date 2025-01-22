import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { DataSetService } from './modules/water-deficiency/services/data-set.service';
import { MapViewService } from './shared/services/map-view.service';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { SharedModule } from './shared/shared.module';
import { MatModule } from './mat.module';
import { WaterDeficiencyModule } from './modules/water-deficiency/water-deficiency.module';
import { MainModule } from './modules/main/main.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter'; 
import { MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material/core';
import moment from 'moment';
import 'moment/locale/uk';

const UKRAINIAN_DATE_FORMATS = {
  parse: {
    dateInput: 'L',
  },
  display: {
    dateInput: 'L',
    monthYearLabel: 'MMMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@NgModule({
  imports: [
    SharedModule,
    MatModule,
    WaterDeficiencyModule,
    MainModule,
    BrowserAnimationsModule
  ],
  declarations: [
    AppComponent,
  ],
  bootstrap: [AppComponent],
  providers: [
    DataSetService,
    MapViewService,
    provideAnimationsAsync(),

    { provide: MAT_DATE_LOCALE, useValue: 'uk-UA' }, 
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } }, 
    { provide: MAT_DATE_FORMATS, useValue: UKRAINIAN_DATE_FORMATS },
  ]
})
export class AppModule {
  constructor() {
    moment.locale('uk');
  }
 }
