import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { IGasStation } from '../../models/IGasStation';
import { DataSetService } from '../../services/data-set.service';
import { Router } from '@angular/router';
import { MapViewService } from '../../services/map-view.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'n-water-deficiencys-list',
  templateUrl: './water-deficiency-list.component.html',
  styleUrls: ['./water-deficiency-list.component.css']
})
export class GasStationsListComponent implements OnInit, OnChanges{
  public gasStationsList: IGasStation[] = [];

  public search: string = "";

  constructor(private stationsDataService: DataSetService,
      private router: Router,
      private mapViewService: MapViewService,) {
   }

  ngOnChanges (changes: SimpleChanges): void {
    if (changes['gasStationsList'].currentValue != changes['gasStationsList'].previousValue) {
      this.gasStationsList = changes['gasStationsList'].currentValue;
    }
  }

  ngOnInit (): void {
    this.stationsDataService.getAllStations()
      .subscribe(stations => this.gasStationsList = stations);
  }

  public navigateToDetail(id?: number){
    if (id){
      this.router.navigate([`/add`]);
    }

    this.changeMapFocus(id!)
    this.router.navigate([`/edit/${id!}`]);
  }

  public onRemove(station: IGasStation){
    this.changeMapFocus(station.id)

    this.stationsDataService.deleteStation(station.id)
      .subscribe((data) => {
        console.log(data);
        this.stationsDataService.getAllStations()
          .subscribe(stations => { this.gasStationsList = stations });

      });

   
  }

  private changeMapFocus(id: number){
    const foundGasStation = this.gasStationsList.find(st => st.id === id);

    if (foundGasStation !== undefined) {
      this.mapViewService.changeStationFocus(foundGasStation.latitude, foundGasStation.longitude, 17);
    }
  }

}
