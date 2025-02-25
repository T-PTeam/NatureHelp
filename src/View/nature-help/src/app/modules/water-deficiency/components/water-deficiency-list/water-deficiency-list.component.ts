import { WaterAPIService } from '@/modules/water-deficiency/services/water-api.service';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IWaterDeficiency } from '../../models/IWaterDeficiency';

@Component({
  selector: 'n-water-deficiencys-deficiencies',
  templateUrl: './water-deficiency-list.component.html',
  styleUrls: ['./water-deficiency-list.component.css'],
  standalone: false,
})
export class WaterDeficiencyList {
  public deficiencies: IWaterDeficiency[] = [];
  public search: string = "";

  constructor(
    public waterAPIService: WaterAPIService,
    private router: Router,
  ) {}

  public navigateToDetail(id?: string){
    if (id) {
      this.router.navigate([`/water/${id}`]);
    } else {
      this.router.navigate(['water/add']);
    }
  }

  public goToSoil(){
    this.router.navigateByUrl('soil');
  }
}
