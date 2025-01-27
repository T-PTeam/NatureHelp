import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SoilAPIService } from '@/modules/soil-deficiency/services/soilAPI.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { EDeficiencyType, EDangerState } from '../../../../models/enums';
import { ISoilDeficiency } from '../../models/ISoilDeficiency';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import moment from 'moment'

@Component({
  selector: 'n-soil-deficiency-details',
  templateUrl: './soil-deficiency-details.component.html',
  styleUrls: ['./soil-deficiency-details.component.css'],
  standalone: false,
})
export class SoilDeficiencyDetail implements OnInit {
  public details: ISoilDeficiency | null = null;
  private isAddingDeficiency: boolean = false;
  detailsForm!: FormGroup;
  deficiencyTypes = Object.values(EDeficiencyType);

  constructor(
    private deficiencyDataService: SoilAPIService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private mapViewService: MapViewService,
    private fb: FormBuilder
  ) {
    this.activatedRoute.params.subscribe((params) => {
      const id = params['id'];

      if (!id) {
        this.isAddingDeficiency = true;
      } else {
        this.deficiencyDataService
          .getSoilDeficiencyById(id)
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

  private initializeForm(deficiency: ISoilDeficiency | null = null) {
    this.detailsForm = this.fb.group({
      id: [deficiency?.id || ''],
      createdAt: [deficiency?.createdAt || new Date()],
      updatedAt: [deficiency?.updatedAt || new Date()],
      title: [deficiency?.title || '', Validators.required],
      description: [deficiency?.description || '', Validators.required],
      type: [deficiency?.type || EDeficiencyType.Soil, Validators.required],
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
      soilQualityLevel: [
        deficiency?.soilQualityLevel || 0,
        [Validators.required, Validators.min(0), Validators.max(10)],
      ],
    });
  }

  public onSubmit() {
    if (this.detailsForm.invalid) {
      return;
    }

    const formData: ISoilDeficiency = this.detailsForm.value;

    if (this.isAddingDeficiency) {
      this.deficiencyDataService
        .addNewSoilDeficiency(formData)
        .subscribe((def) => {
          console.log('Created:', def);
          this.router.navigate(['/soil']);
        });
    } else {
      this.deficiencyDataService
        .updateSoilDeficiency(formData.id, formData)
        .subscribe((def) => {
          console.log('Updated:', def);
          this.router.navigate(['/soil']);
        });
    }
  }

  public onCancel() {
    this.changeMapView();
    this.router.navigate(['/soil']);
  }

  private changeMapView() {
    this.mapViewService.changeDeficiencyFocus(48.65, 22.26, 12);
  }
}
