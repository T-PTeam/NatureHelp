import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataSetService } from '@/modules/water-deficiency/services/data-set.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { EDeficiencyType, EDangerState } from '../../models/enums';
import { IWaterDeficiency } from '../../models/IWaterDeficiency';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import moment from 'moment'

const MOCK_WATER_DEFICIENCY = {
  id: "3",
  createdAt: new Date('2025-01-07'),
  updatedAt: new Date('2025-01-15'),
  title: 'Broken Pipeline',
  description: 'Pipeline failure causing water wastage.',
  type: EDeficiencyType.Water,
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
  waterQualityLevel: 5
}

@Component({
  selector: 'n-water-deficiency-details',
  templateUrl: './water-deficiency-details.component.html',
  styleUrls: ['./water-deficiency-details.component.css'],
  standalone: false,
})
export class WaterDeficiencyDetail implements OnInit {
  public details: IWaterDeficiency = MOCK_WATER_DEFICIENCY;
  // {
  //   populationAffected: 0,
  //   economicImpact: 0,
  //   caused: '',
  //   waterQualityLevel: 0,
  //   title: '',
  //   description: '',
  //   type: EDeficiencyType.Water,
  //   creator: {} as IUser,
  //   location: {} as ILocation,
  //   eDangerState: EDangerState.Moderate,
  //   id: '',
  //   createdAt: new Date(),
  //   updatedAt: new Date(),
  // };

  private isAddingStation: boolean = false;
  detailsForm!: FormGroup;
  deficiencyTypes = Object.values(EDeficiencyType); //.filter(x => Number(x) || Number(x) === 0).map(x => Number(x));
  
  constructor(private stationsDataService: DataSetService,
      private activatedRoute: ActivatedRoute,
      private router: Router,
      private mapViewService: MapViewService,
      private fb: FormBuilder
    ) {

    this.activatedRoute.params.subscribe(params => {
      const id = params['id'];

      if (id == null || id == undefined){
        this.isAddingStation = true;
      }

      if (id){
        this.stationsDataService
          .getWaterDeficiencyById(id)
          .subscribe(station => this.details = station);
      }
    });
  }
  
  ngOnInit(): void {
    this.detailsForm = this.fb.group({
      id: [MOCK_WATER_DEFICIENCY.id],
      createdAt: [MOCK_WATER_DEFICIENCY.createdAt],
      updatedAt: [MOCK_WATER_DEFICIENCY.updatedAt],
      title: [MOCK_WATER_DEFICIENCY.title || '', Validators.required],
      description: [MOCK_WATER_DEFICIENCY.description || '', Validators.required],
      type: [MOCK_WATER_DEFICIENCY.type || EDeficiencyType.Water, Validators.required],
      creator: this.fb.group({
        id: [MOCK_WATER_DEFICIENCY.creator.id],
        name: [MOCK_WATER_DEFICIENCY.creator.name, Validators.required],
        email: [MOCK_WATER_DEFICIENCY.creator.email, Validators.email],
        role: [MOCK_WATER_DEFICIENCY.creator.role],
      }),
      responsibleUser: this.fb.group({
        id: [MOCK_WATER_DEFICIENCY.responsibleUser.id],
        name: [MOCK_WATER_DEFICIENCY.responsibleUser.name, Validators.required],
        email: [MOCK_WATER_DEFICIENCY.responsibleUser.email, Validators.email],
        role: [MOCK_WATER_DEFICIENCY.responsibleUser.role],
      }),
      metrics: [MOCK_WATER_DEFICIENCY.metrics || []], // Optional array
      location: this.fb.group({
        latitude: [MOCK_WATER_DEFICIENCY.location.latitude, [Validators.required, Validators.min(-90), Validators.max(90)]],
        longitude: [MOCK_WATER_DEFICIENCY.location.longitude, [Validators.required, Validators.min(-180), Validators.max(180)]],
        city: [MOCK_WATER_DEFICIENCY.location.city || ''],
        country: [MOCK_WATER_DEFICIENCY.location.country || ''],
      }),
      eDangerState: [MOCK_WATER_DEFICIENCY.eDangerState || '', Validators.required],
      populationAffected: [MOCK_WATER_DEFICIENCY.populationAffected, [Validators.required, Validators.min(0)]],
      economicImpact: [MOCK_WATER_DEFICIENCY.economicImpact, [Validators.required, Validators.min(0)]],
      healthImpact: [MOCK_WATER_DEFICIENCY.healthImpact || '', Validators.required],
      resolvedDate: [MOCK_WATER_DEFICIENCY.resolvedDate || moment()],
      expectedResolutionDate: [MOCK_WATER_DEFICIENCY.expectedResolutionDate || moment()],
      caused: [MOCK_WATER_DEFICIENCY.caused || '', Validators.required],
      waterQualityLevel: [MOCK_WATER_DEFICIENCY.waterQualityLevel, [Validators.required, Validators.min(0), Validators.max(10)]],
    });
  }

  public onSubmit(){
    if (this.isAddingStation){
      this.stationsDataService
        .addNewWaterDeficiency(this.details)
        .subscribe(station => console.log(station));
    }
    else{
      this.changeMapView();

      this.stationsDataService
        .updateWaterDeficiency(this.details.id, this.details)
        .subscribe(station => console.log(station));
    }

    this.stationsDataService.getAllWaterDeficiencies();
    this.router.navigate(["/"]);
  }

  public onCancel(){
    this.changeMapView()

    this.router.navigate(["/"]);
  }
  
  private changeMapView(){
    this.mapViewService.changeStationFocus(48.65, 22.26, 12);
  }

}

