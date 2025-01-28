import { WaterAPIService } from '@/modules/water-deficiency/services/waterAPI.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { IWaterDeficiency } from '../../models/IWaterDeficiency';
import { EDangerState, EDeficiencyType } from '../../../../models/enums';

@Component({
  selector: 'n-water-deficiencys-deficiencies',
  templateUrl: './water-deficiency-list.component.html',
  styleUrls: ['./water-deficiency-list.component.css'],
  standalone: false,
})
export class WaterDeficiencyList implements OnInit, OnChanges{
  public deficiencies: IWaterDeficiency[] = [];
  public search: string = "";

  constructor(
    private waterDataService: WaterAPIService,
    private router: Router,
    private mapViewService: MapViewService
  ) {}

  ngOnChanges (changes: SimpleChanges): void {
    if (changes['deficiencies'] && changes['deficiencies'].currentValue !== changes['deficiencies'].previousValue) {
      this.deficiencies = changes['deficiencies'].currentValue;
    }
  }

  ngOnInit (): void {
    this.loadWaterDeficiencies();
  }

  private loadWaterDeficiencies(): void {
    this.waterDataService.getAllWaterDeficiencies()
      .subscribe(
        (data: IWaterDeficiency[]) => {
          this.deficiencies = data;
        },
        (error) => {
          console.error('Error fetching water deficiencies:', error);
        }
      );
  }

  public navigateToDetail(id?: string){
    console.log(id)
    if (id) {
      // this.changeMapFocus(id);
      // this.router.navigate([`/${id}`]);
      this.router.navigate([`/water/${id}`]);
    } else {
      this.router.navigate(['water/add']);
    }
  }

  public onRemove(def: IWaterDeficiency){
    // this.changeMapFocus(def.id);

    this.waterDataService.deleteWaterDeficiency(def.id)
      .subscribe(
        () => {
          this.loadWaterDeficiencies();
        },
        (error) => {
          console.error('Error: ', error);
        }
      );
  }

  // private changeMapFocus(id: string){
  //   const foundDeficiency = this.deficiencies.find(st => st.id === id);

  //   if (foundDeficiency !== undefined) {
  //     this.mapViewService.changeDeficiencyFocus(foundDeficiency.location.latitude, foundDeficiency.location.longitude, 17);
  //   }
  // }

}
