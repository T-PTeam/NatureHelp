import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataSetWaterService } from '@/modules/water-deficiency/services/data-set-water.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { EDeficiencyType, EDangerState } from '../../../../models/enums';
import { IWaterDeficiency } from '../../models/IWaterDeficiency';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import moment from 'moment'

// const MOCK_WATER_DEFICIENCY = {
//   id: "",
//   createdAt: new Date(),
//   updatedAt: new Date(),
//   title: '',
//   description: '',
//   type: EDeficiencyType.Water,
//   creator: {
//     id: "", name: '',
//     email: '',
//     role: ''
//   },
//   responsibleUser: {
//     id: "", name: '',
//     email: '',
//     role: ''
//   },
//   location: {
//     latitude: 1, longitude: 1,
//     city: '',
//     country: ''
//   },
//   eDangerState: EDangerState.Moderate,
//   populationAffected: 0,
//   economicImpact: 0,
//   healthImpact: '',
//   resolvedDate: new Date('2025-01-10'),
//   expectedResolutionDate: new Date('2025-01-20'),
//   caused: '',
//   waterQualityLevel: 0
// }

@Component({
  selector: 'n-water-deficiency-details',
  templateUrl: './water-deficiency-details.component.html',
  styleUrls: ['./water-deficiency-details.component.css'],
  standalone: false,
})
export class WaterDeficiencyDetail implements OnInit {
  public details: IWaterDeficiency | null = null;
    private isAddingDeficiency: boolean = false;
    detailsForm!: FormGroup;
    deficiencyTypes = Object.values(EDeficiencyType);

  
  constructor(private deficiencyDataService: DataSetWaterService,
      private activatedRoute: ActivatedRoute,
      private router: Router,
      private mapViewService: MapViewService,
      private fb: FormBuilder
    ) {

    this.activatedRoute.params.subscribe(params => {
      const id = params['id'];

      if (!id) {
        this.isAddingDeficiency = true;
      }
      else {
        this.deficiencyDataService
          .getWaterDeficiencyById(id)
          .subscribe((def) => {
            this.details = def;
            this.initializeForm(def);
          });
      }
    });
  }
  
  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(deficiency: IWaterDeficiency | null = null) {
      this.detailsForm = this.fb.group({
        id: [deficiency?.id || ''],
        createdAt: [deficiency?.createdAt || new Date()],
        updatedAt: [deficiency?.updatedAt || new Date()],
        title: [deficiency?.title || '', Validators.required],
        description: [deficiency?.description || '', Validators.required],
        type: [deficiency?.type || EDeficiencyType.Water, Validators.required],
        creator: this.fb.group({
          id: [deficiency?.creator?.id || ''],
          name: [deficiency?.creator?.name || '', Validators.required],
          email: [deficiency?.creator?.email || '', Validators.email],
          role: [deficiency?.creator?.role || ''],
        }),
        responsibleUser: this.fb.group({
          id: [deficiency?.responsibleUser?.id || ''],
          name: [deficiency?.responsibleUser?.name || '', Validators.required],
          email: [deficiency?.responsibleUser?.email || '', Validators.email],
          role: [deficiency?.responsibleUser?.role || ''],
        }),
        location: this.fb.group({
          latitude: [
            deficiency?.location?.latitude || 0,
            [Validators.required, Validators.min(-90), Validators.max(90)],
          ],
          longitude: [
            deficiency?.location?.longitude || 0,
            [Validators.required, Validators.min(-180), Validators.max(180)],
          ],
          city: [deficiency?.location?.city || ''],
          country: [deficiency?.location?.country || ''],
        }),
        eDangerState: [deficiency?.eDangerState || EDangerState.Moderate, Validators.required],
        populationAffected: [
          deficiency?.populationAffected || 0,
          [Validators.required, Validators.min(0)],
        ],
        economicImpact: [
          deficiency?.economicImpact || 0,
          [Validators.required, Validators.min(0)],
        ],
        healthImpact: [deficiency?.healthImpact || '', Validators.required],
        resolvedDate: [deficiency?.resolvedDate || moment()],
        expectedResolutionDate: [deficiency?.expectedResolutionDate || moment()],
        caused: [deficiency?.caused || '', Validators.required],
        waterlQualityLevel: [
          deficiency?.waterQualityLevel || 0,
          [Validators.required, Validators.min(0), Validators.max(10)],
        ],
      });
    }

  public onSubmit(){
    if (this.detailsForm.invalid) {
      return;
    }

    const formData: IWaterDeficiency = this.detailsForm.value;

    if (this.isAddingDeficiency) {
      this.deficiencyDataService
        .addNewWaterDeficiency(formData)
        .subscribe((def) => {
          console.log('Created:', def);
          this.router.navigate(['/water']);
        });
    } else {
      this.deficiencyDataService
        .updateWaterDeficiency(formData.id, formData)
        .subscribe((def) => {
          console.log('Updated:', def);
          this.router.navigate(['/water']);
        });
    }
  }

  public onCancel(){
    this.changeMapView();

    this.router.navigate(["/water"]);
  }
  
  private changeMapView(){
    this.mapViewService.changeDeficiencyFocus(48.65, 22.26, 12);
  }

}

