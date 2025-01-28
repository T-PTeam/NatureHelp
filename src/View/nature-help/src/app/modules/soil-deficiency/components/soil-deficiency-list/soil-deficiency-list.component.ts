import { SoilAPIService } from '@/modules/soil-deficiency/services/soilAPI.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { ISoilDeficiency } from '../../models/ISoilDeficiency';
import { EDangerState, EDeficiencyType } from '../../../../models/enums';

@Component({
  selector: 'n-soil-deficiencys-deficiencies',
  templateUrl: './soil-deficiency-list.component.html',
  styleUrls: ['./soil-deficiency-list.component.css'],
  standalone: false,
})
export class SoilDeficiencyList implements OnInit, OnChanges{
  public deficiencies: ISoilDeficiency[] = [];
  public search: string = "";

  constructor(
    private soilDataService: SoilAPIService,
    private router: Router,
    private mapViewService: MapViewService) {
   }

  ngOnChanges (changes: SimpleChanges): void {
    if (changes['deficiencies'] && changes['deficiencies'].currentValue !== changes['deficiencies'].previousValue) {
      this.deficiencies = changes['deficiencies'].currentValue;
    }
  }

  ngOnInit (): void {
    this.loadSoilDeficiencies();
  }

  private loadSoilDeficiencies(): void {
    this.soilDataService.getAllSoilDeficiencies()
    .subscribe(
      (data: ISoilDeficiency[]) => {
        this.deficiencies = data;
      },
      (error) => {
        console.error('Error fetching soil deficiencies:', error);
      }
    );
  }

  public navigateToDetail(id?: string){
    if (id) {
      // this.changeMapFocus(id);
      // this.router.navigate([`/${id}`]);
      this.router.navigate([`/soil/${id}`]);
    } else {
      this.router.navigate([`soil/add`]);
    }
  }

  public onRemove(def: ISoilDeficiency){
    // this.changeMapFocus(def.id)

    this.soilDataService.deleteSoilDeficiency(def.id)
    .subscribe(
      () => {
        this.loadSoilDeficiencies();
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
