import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataSetWaterService } from '@/modules/water-deficiency/services/data-set-water.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { EDeficiencyType, EDangerState } from '../../models/enums';
import { IWaterDeficiency } from '../../models/IWaterDeficiency';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import moment from 'moment'

const MOCK_WATER_DEFICIENCY = {
  id: "",
  createdAt: new Date(),
  updatedAt: new Date(),
  title: '',
  description: '',
  type: EDeficiencyType.Water,
  creator: {
    id: "", name: '',
    email: '',
    role: ''
  },
  responsibleUser: {
    id: "", name: '',
    email: '',
    role: ''
  },
  metrics: [],
  location: {
    latitude: 1, longitude: 1,
    city: '',
    country: ''
  },
  eDangerState: EDangerState.Moderate,
  populationAffected: 0,
  economicImpact: 0,
  healthImpact: '',
  resolvedDate: new Date('2025-01-10'),
  expectedResolutionDate: new Date('2025-01-20'),
  caused: '',
  waterQualityLevel: 0
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

  private isAddingDeficiency: boolean = false;
  detailsForm!: FormGroup;
  deficiencyTypes = Object.values(EDeficiencyType); //.filter(x => Number(x) || Number(x) === 0).map(x => Number(x));
  
  constructor(private deficiencyDataService: DataSetWaterService,
      private activatedRoute: ActivatedRoute,
      private router: Router,
      private mapViewService: MapViewService,
      private fb: FormBuilder
    ) {

    this.activatedRoute.params.subscribe(params => {
      const id = params['id'];

      if (id == null || id == undefined){
        this.isAddingDeficiency = true;
      }

      if (id){
        this.deficiencyDataService
          .getWaterDeficiencyById(id)
          .subscribe(def => this.details = def);
      }
    });
  }
  
  ngOnInit(): void {
    this.detailsForm = this.fb.group({
      id: [this.details.id],
      createdAt: [this.details.createdAt],
      updatedAt: [this.details.updatedAt],
      title: [this.details.title || '', Validators.required],
      description: [this.details.description || '', Validators.required],
      type: [this.details.type || EDeficiencyType.Water, Validators.required],
      creator: this.fb.group({
        id: [this.details.creator.id],
        name: [this.details.creator.name, Validators.required],
        email: [this.details.creator.email, Validators.email],
        role: [this.details.creator.role],
      }),
      responsibleUser: this.fb.group({
        id: [this.details.responsibleUser?.id],
        name: [this.details.responsibleUser?.name, Validators.required],
        email: [this.details.responsibleUser?.email, Validators.email],
        role: [this.details.responsibleUser?.role],
      }),
      location: this.fb.group({
        latitude: [this.details.location.latitude, [Validators.required, Validators.min(-90), Validators.max(90)]],
        longitude: [this.details.location.longitude, [Validators.required, Validators.min(-180), Validators.max(180)]],
        city: [this.details.location.city || ''],
        country: [this.details.location.country || ''],
      }),
      eDangerState: [this.details.eDangerState || '', Validators.required],
      populationAffected: [this.details.populationAffected, [Validators.required, Validators.min(0)]],
      economicImpact: [this.details.economicImpact, [Validators.required, Validators.min(0)]],
      healthImpact: [this.details.healthImpact || '', Validators.required],
      resolvedDate: [this.details.resolvedDate || moment()],
      expectedResolutionDate: [this.details.expectedResolutionDate || moment()],
      caused: [this.details.caused || '', Validators.required],
      waterQualityLevel: [this.details.waterQualityLevel, [Validators.required, Validators.min(0), Validators.max(10)]],
    });
  }

  public onSubmit(){
    if (this.isAddingDeficiency){
      this.deficiencyDataService
        .addNewWaterDeficiency(this.details)
        .subscribe(def => console.log(def));
    }
    else{
      this.changeMapView();

      this.deficiencyDataService
        .updateWaterDeficiency(this.details.id, this.details)
        .subscribe(def => console.log(def));
    }

    this.deficiencyDataService.getAllWaterDeficiencies();
    this.router.navigate(["/"]);
  }

  public onCancel(){
    this.changeMapView();

    this.router.navigate(["/"]);
  }
  
  private changeMapView(){
    this.mapViewService.changeStationFocus(48.65, 22.26, 12);
  }

}

