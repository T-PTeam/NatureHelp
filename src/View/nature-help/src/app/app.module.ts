import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { WaterAPIService } from './modules/water-deficiency/services/water-api.service';
import { SoilAPIService } from './modules/soil-deficiency/services/soil-api.service';
import { MapViewService } from './shared/services/map-view.service';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { SharedModule } from './shared/shared.module';
import { MatModule } from './mat.module';
import { WaterDeficiencyModule } from './modules/water-deficiency/water-deficiency.module';
import { SoilDeficiencyModule } from './modules/soil-deficiency/soil-deficiency.module';
import { MainModule } from './modules/main/main.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter'; 
import { MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material/core';
import moment from 'moment';
import 'moment/locale/uk';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './shared/services/auth-interceptor.service';
import { LabsModule } from './modules/laboratories/labs.module';
import { LabsAPIService } from './modules/laboratories/services/labs-api.service';

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
    SoilDeficiencyModule,
    MainModule,
    BrowserAnimationsModule,
    LabsModule
  ],
  declarations: [
    AppComponent,
  ],
  bootstrap: [AppComponent],
  providers: [
    WaterAPIService,
    SoilAPIService,
    LabsAPIService,

    MapViewService,
    provideAnimationsAsync(),

    { provide: MAT_DATE_LOCALE, useValue: 'uk-UA' }, 
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } }, 
    { provide: MAT_DATE_FORMATS, useValue: UKRAINIAN_DATE_FORMATS },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
})
export class AppModule {
  constructor() {
    moment.locale('uk');
  }
 }
