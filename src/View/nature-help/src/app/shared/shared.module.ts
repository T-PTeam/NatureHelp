import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MapComponent } from './components/main-map/main-map.component';
import { FilterPipe } from './pipes/filter.pipe';
import { EnumToStringPipe } from './pipes/enum-to-string.pipe';
import { LoadingService } from './services/loading.service';
import { MatModule } from '@/mat.module';
import { LoginDialogComponent } from './components/dialogs/login-dialog/login-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    MatModule,
    ReactiveFormsModule,
    FormsModule
  ],
  declarations: [
    MapComponent,

    LoginDialogComponent,

    FilterPipe,
    EnumToStringPipe
  ],
  exports:[
    MapComponent,

    LoginDialogComponent,

    FilterPipe,
    EnumToStringPipe
  ],
  providers: [
    LoadingService
  ]
})
export class SharedModule { }
