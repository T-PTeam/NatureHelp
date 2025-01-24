import { DataSetSoilService } from '@/modules/soil-deficiency/services/data-set-soil.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { ISoilDeficiency } from '../../models/ISoilDeficiency';
import { EDangerState, EDeficiencyType } from '../../models/enums';

export const MOCK_SOIL_DEFICIENCIES: ISoilDeficiency[] = [
  {
    id: "1",
    createdAt: new Date('2025-01-01'),
    updatedAt: new Date('2025-01-10'),
    title: 'Soil Shortage in Village',
    description: 'Severe soil shortage affecting daily life.',
    type: EDeficiencyType.Soil,
    creator: {
      id: "1", name: 'Admin',
      email: '',
      role: ''
    },
    responsibleUser: {
      id: "2", name: 'John Doe',
      email: '',
      role: ''
    },
    metrics: [],
    location: {
      latitude: 40.7128, longitude: -74.0060,
      city: '',
      country: ''
    },
    eDangerState: EDangerState.Dangerous,
    populationAffected: 5000,
    economicImpact: 250000,
    healthImpact: 'Moderate',
    resolvedDate: new Date('2025-01-15'),
    expectedResolutionDate: new Date('2025-02-01'),
    caused: 'Drought',
    soilQualityLevel: 3
  },
  {
    id: "2",
    createdAt: new Date('2025-01-05'),
    updatedAt: new Date('2025-01-20'),
    title: 'Polluted River',
    description: 'Pollution causing health issues.',
    type: EDeficiencyType.Soil,
    creator: {
      id: "3", name: 'Admin',
      email: '',
      role: ''
    },
    responsibleUser: {
      id: "3", name: 'Jane Smith',
      email: '',
      role: ''
    },
    metrics: [],
    location: {
      latitude: 34.0522, longitude: -118.2437,
      city: '',
      country: ''
    },
    eDangerState: EDangerState.Critical,
    populationAffected: 12000,
    economicImpact: 500000,
    healthImpact: 'Severe',
    resolvedDate: new Date('2025-01-10'),
    expectedResolutionDate: new Date('2025-03-15'),
    caused: 'Pollution',
    soilQualityLevel: 2
  },
  {
    id: "3",
    createdAt: new Date('2025-01-07'),
    updatedAt: new Date('2025-01-15'),
    title: 'Broken Pipeline',
    description: 'Pipeline failure causing soil wastage.',
    type: EDeficiencyType.Soil,
    creator: {
      id: "1", name: 'Admin',
      email: '',
      role: ''
    },
    responsibleUser: {
      id: "4", name: 'Alice Brown',
      email: '',
      role: ''
    },
    metrics: [],
    location: {
      latitude: 51.5074, longitude: -0.1278,
      city: '',
      country: ''
    },
    eDangerState: EDangerState.Moderate,
    populationAffected: 3000,
    economicImpact: 100000,
    healthImpact: 'Low',
    resolvedDate: new Date('2025-01-10'),
    expectedResolutionDate: new Date('2025-01-20'),
    caused: 'Infrastructure failure',
    soilQualityLevel: 5
  }
];



@Component({
  selector: 'n-soil-deficiencys-deficiencies',
  templateUrl: './soil-deficiency-list.component.html',
  styleUrls: ['./soil-deficiency-list.component.css'],
  standalone: false,
})
export class SoilDeficiencyList implements OnInit, OnChanges{
  public deficiencies: ISoilDeficiency[] = MOCK_SOIL_DEFICIENCIES;

  public search: string = "";

  constructor(private stationsDataService: DataSetSoilService,
      private router: Router,
      private mapViewService: MapViewService) {
   }

  ngOnChanges (changes: SimpleChanges): void {
    if (changes['deficiencies'].currentValue != changes['deficiencies'].previousValue) {
      this.deficiencies = changes['deficiencies'].currentValue;
    }
  }

  ngOnInit (): void {
    // this.stationsDataService.getAllSoilDeficiencies()
    //   .subscribe(stations => this.deficiencies = stations);
  }

  public navigateToDetail(id?: string){
    if (id) {
      this.changeMapFocus(id!)
      this.router.navigate([`/${id!}`]);
    }

    this.router.navigate([`soil/add`]);
  }

  public onRemove(station: ISoilDeficiency){
    this.changeMapFocus(station.id)

    this.stationsDataService.deleteSoilDeficiency(station.id)
      .subscribe((data) => {
        console.log(data);
        this.stationsDataService.getAllSoilDeficiencies()
          .subscribe(stations => { this.deficiencies = stations });

      });
  }

  private changeMapFocus(id: string){
    const foundGasStation = this.deficiencies.find(st => st.id === id);

    if (foundGasStation !== undefined) {
      this.mapViewService.changeStationFocus(foundGasStation.location.latitude, foundGasStation.location.longitude, 17);
    }
  }

}
