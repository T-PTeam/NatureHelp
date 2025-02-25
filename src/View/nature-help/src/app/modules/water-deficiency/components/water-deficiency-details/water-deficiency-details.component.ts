import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WaterAPIService } from '@/modules/water-deficiency/services/water-api.service';
import { MapViewService } from '@/shared/services/map-view.service';
import { EDeficiencyType, EDangerState } from '../../../../models/enums';
import { IWaterDeficiency } from '../../models/IWaterDeficiency';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import moment from 'moment'

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

  
  constructor(private deficiencyDataService: WaterAPIService,
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

    const id = this.activatedRoute.snapshot.paramMap.get('id');

    if (id) {
      this.deficiencyDataService.getWaterDeficiencyById(id).subscribe(
        (data) => {
          this.details = data;
        },
        (error) => {
          console.error('Error fetching deficiency:', error);
        }
      );
    }
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
        dissolvedOxygen: [
          deficiency?.dissolvedOxygen || 0,
          [Validators.required, Validators.min(0)],
        ],
        leadConcentration: [
          deficiency?.leadConcentration || 0,
          [Validators.required, Validators.min(0)],
        ],
        mercuryConcentration: [
          deficiency?.mercuryConcentration || 0,
          [Validators.required, Validators.min(0)],
        ],
        nitrateConcentration: [
          deficiency?.nitrateConcentration || 0,
          [Validators.required, Validators.min(0)],
        ],
        pesticidesContent: [
          deficiency?.pesticidesContent || 0,
          [Validators.required, Validators.min(0)],
        ],
        microbialActivity: [
          deficiency?.microbialActivity || 0,
          [Validators.required, Validators.min(0)],
        ],
        radiationLevel: [
          deficiency?.radiationLevel || 0,
          [Validators.required, Validators.min(0)],
        ],
        chemicalOxygenDemand: [
          deficiency?.chemicalOxygenDemand || 0,
          [Validators.required, Validators.min(0)],
        ],
        biologicalOxygenDemand: [
          deficiency?.biologicalOxygenDemand || 0,
          [Validators.required, Validators.min(0)],
        ],
        phosphateConcentration: [
          deficiency?.phosphateConcentration || 0,
          [Validators.required, Validators.min(0)],
        ],
        cadmiumConcentration: [
          deficiency?.cadmiumConcentration || 0,
          [Validators.required, Validators.min(0)],
        ],
        totalDissolvedSolids: [
          deficiency?.totalDissolvedSolids || 0,
          [Validators.required, Validators.min(0)],
        ],
        electricalConductivity: [
          deficiency?.electricalConductivity || 0,
          [Validators.required, Validators.min(0)],
        ],
        microbialLoad: [
          deficiency?.microbialLoad || 0,
          [Validators.required, Validators.min(0)],
        ],
        toxicityLevel: [deficiency?.toxicityLevel || '', Validators.required],
      });
    }

  public onSubmit(){
    if (this.detailsForm.invalid) {
      console.log('Form is invalid:', this.getFormErrors(this.detailsForm));
      // return;
    }
  
    console.log('Form data:', this.detailsForm.value);
  
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
        .updateWaterDeficiencyById(formData.id, formData)
        .subscribe((def) => {
          console.log('Updated:', def);
          this.router.navigate(['/water']);
        });
    }
  }

  private getFormErrors(formGroup: FormGroup): any {
    const errors: any = {};
    Object.keys(formGroup.controls).forEach((key) => {
      const control = formGroup.controls[key];
      if (control instanceof FormGroup) {
        errors[key] = this.getFormErrors(control); 
      } else if (control.invalid) {
        errors[key] = control.errors;
      }
    });
    return errors;
  }

  public onCancel(){
    this.changeMapView();

    this.router.navigate(["/water"]);
  }
  
  private changeMapView(){
    this.mapViewService.changeFocus({latitude: 48.65, longitude: 22.26}, 12);
  }

}

