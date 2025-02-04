import { SoilAPIService } from '@/modules/soil-deficiency/services/soilAPI.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'n-soil-deficiencys-deficiencies',
  templateUrl: './soil-deficiency-list.component.html',
  styleUrls: ['./soil-deficiency-list.component.css'],
  standalone: false,
})
export class SoilDeficiencyList {
  public search: string = "";

  constructor(
    public soilAPIService: SoilAPIService,
    private router: Router,
    private mapViewService: MapViewService) {
   }

  public navigateToDetail(id?: string){
    if (id) {
      this.router.navigate([`/soil/${id}`]);
    } else {
      this.router.navigate([`soil/add`]);
    }
  }

  public goToWater(){
    this.router.navigateByUrl('/water');
  }
}
