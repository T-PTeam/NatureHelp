import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SoilAPIService } from '@/modules/soil-deficiency/services/soil-api.service';
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
        name: [`${deficiency?.creator?.firstName} ${deficiency?.creator?.lastName}` || '', Validators.required],
        email: [deficiency?.creator?.email || '', Validators.email],
        role: [deficiency?.creator?.role || ''],
      }),
      responsibleUser: this.fb.group({
        id: [deficiency?.responsibleUser?.id || ''],
        name: [`${deficiency?.creator?.firstName} ${deficiency?.creator?.lastName}` || '', Validators.required],
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
      ph: [
        deficiency?.ph || 6.5,
        [Validators.required, Validators.min(0)],
      ],
      organicMatter: [
        deficiency?.organicMatter || 0,
        [Validators.required, Validators.min(0)],
      ],
      leadConcentration: [
        deficiency?.leadConcentration || 0,
        [Validators.required, Validators.min(0)],
      ],
      cadmiumConcentration: [
        deficiency?.cadmiumConcentration || 0,
        [Validators.required, Validators.min(0)],
      ],
      mercuryConcentration: [
        deficiency?.mercuryConcentration || 0,
        [Validators.required, Validators.min(0)],
      ],
      pesticidesContent: [
        deficiency?.pesticidesContent || 0,
        [Validators.required, Validators.min(0)],
      ],
      nitratesConcentration: [
        deficiency?.nitratesConcentration || 0,
        [Validators.required, Validators.min(0)],
      ],
      heavyMetalsConcentration: [
        deficiency?.heavyMetalsConcentration || 0,
        [Validators.required, Validators.min(0)],
      ],
      electricalConductivity: [
        deficiency?.electricalConductivity || 0,
        [Validators.required, Validators.min(0)],
      ],
      toxicityLevel: [deficiency?.toxicityLevel || '', Validators.required],
      microbialActivity: [
        deficiency?.microbialActivity || 0,
        [Validators.required, Validators.min(0)],
      ],
      analysisDate: [deficiency?.analysisDate || moment()],
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
        .updateSoilDeficiencyById(formData.id, formData)
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
    this.mapViewService.changeFocus({latitude: 48.65, longitude: 22.26}, 12);
  }
}
