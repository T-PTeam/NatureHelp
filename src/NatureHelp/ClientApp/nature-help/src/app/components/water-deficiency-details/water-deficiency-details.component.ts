import { Component } from '@angular/core';
import { IGasStation } from '../../models/IGasStation';
import { DataSetService } from '../../services/data-set.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductType } from '../../models/ProductType.enum';
import { MapViewService } from '../../services/map-view.service';

@Component({
  selector: 'n-water-deficiency-details',
  templateUrl: './water-deficiency-details.component.html',
  styleUrls: ['./water-deficiency-details.component.css']
})
export class GasStationDetailsComponent{
  public selectedGasStation: IGasStation = {
    id: 0,
    name: "",
    latitude: 0.0,
    longitude: 0.0,
    productType: ProductType.Diesel,
    workTimeFrom: 0,
    workTimeTo: 0
  };

  private isAddingStation: boolean = false;

  public stationProductType: typeof ProductType = ProductType;

  constructor(private stationsDataService: DataSetService,
      private activatedRoute: ActivatedRoute,
      private router: Router,
      private mapViewService: MapViewService
    ) {

    this.activatedRoute.params.subscribe(params => {
      const id = params['id'];

      if (id == null || id == undefined){
        this.isAddingStation = true;
      }

      const idNumber = parseInt(id, 0);

      if (idNumber != 0){
        this.stationsDataService
          .getStationById(idNumber)
          .subscribe(station => this.selectedGasStation = station);
      }
    });
  }

  public cancelOperation(){
    this.changeMapView()

    this.router.navigate(["/"]);
  }

  public saveStation(){
    if (this.isAddingStation){
      this.stationsDataService
        .addNewStation(this.selectedGasStation)
        .subscribe(station => console.log(station));
    }
    else{
      this.changeMapView();

      this.stationsDataService
        .updateGasStation(this.selectedGasStation.id, this.selectedGasStation)
        .subscribe(station => console.log(station));
    }

    this.stationsDataService.updateStationsList();
    this.router.navigate(["/"]);
  }

  private changeMapView(){
    this.mapViewService.changeStationFocus(48.65, 22.26, 12);
  }

}
