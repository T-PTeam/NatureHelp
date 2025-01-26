import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataSetSoilService } from '@/modules/soil-deficiency/services/data-set-soil.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { EDeficiencyType, EDangerState } from '../../../../models/enums';
import { ISoilDeficiency } from '../../models/ISoilDeficiency';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import moment from 'moment'

const MOCK_SOIL_DEFICIENCY = {
    id: "",
   createdAt: new Date(),
  updatedAt: new Date(),
  title: '',
  description: '',
  type: EDeficiencyType.Soil,
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
  soilQualityLevel: 0
}

@Component({
  selector: 'n-soil-deficiency-details',
  templateUrl: './soil-deficiency-details.component.html',
  styleUrls: ['./soil-deficiency-details.component.css'],
  standalone: false,
})
export class SoilDeficiencyDetail implements OnInit {
  public details: ISoilDeficiency = MOCK_SOIL_DEFICIENCY;
  // {
  //   populationAffected: 0,
  //   economicImpact: 0,
  //   caused: '',
  //   soilQualityLevel: 0,
  //   title: '',
  //   description: '',
  //   type: EDeficiencyType.Soil,
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
  
  constructor(private deficiencyDataService: DataSetSoilService,
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
          .getSoilDeficiencyById(id)
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
      type: [this.details.type || EDeficiencyType.Soil, Validators.required],
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
      soilQualityLevel: [this.details.soilQualityLevel, [Validators.required, Validators.min(0), Validators.max(10)]],
    });
  }

  public onSubmit(){
    if (this.isAddingDeficiency){
      this.deficiencyDataService
        .addNewSoilDeficiency(this.details)
        .subscribe(def => console.log(def));
    }
    else{
      this.changeMapView();

      this.deficiencyDataService
        .updateSoilDeficiency(this.details.id, this.details)
        .subscribe(def => console.log(def));
    }

    this.deficiencyDataService.getAllSoilDeficiencies();
    this.router.navigate(["/soil"]);
  }

  public onCancel(){
    this.changeMapView();

    this.router.navigate(["/soil"]);
  }
  
  private changeMapView(){
    this.mapViewService.changeDeficiencyFocus(48.65, 22.26, 12);
  }

}

