import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import moment from "moment";

import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";
import { MapViewService } from "@/shared/services/map-view.service";

import { EDangerState, EDeficiencyType } from "../../../../models/enums";
import { IWaterDeficiency } from "../../models/IWaterDeficiency";
import { UserAPIService } from "@/shared/services/user-api.service";
import { IUser } from "@/models/IUser";
import { enumToSelectOptions } from "@/shared/helpers/enum-helper";

@Component({
  selector: "n-water-deficiency-details",
  templateUrl: "./water-deficiency-details.component.html",
  styleUrls: ["./water-deficiency-details.component.css"],
  standalone: false,
})
export class WaterDeficiencyDetail implements OnInit {
  details: IWaterDeficiency | null = null;
  detailsForm!: FormGroup;
  dangerStates = enumToSelectOptions(EDangerState);
  changedModelLogsOpened: boolean = false;

  private isAddingDeficiency: boolean = false;
  private currentUser: IUser | null = null;

  constructor(
    private deficiencyDataService: WaterAPIService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private mapViewService: MapViewService,
    private fb: FormBuilder,
    public usersAPIService: UserAPIService,
  ) {}

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      const id = params["id"];

      this.loadOrganizationUsers();
      if (!id) {
        this.isAddingDeficiency = true;
      } else {
        this.deficiencyDataService.getWaterDeficiencyById(id).subscribe((def) => {
          this.initializeForm(def);
        });
      }
    });
  }

  private initializeForm(deficiency: IWaterDeficiency | null = null) {
    this.detailsForm = this.fb.group({
      id: [deficiency?.id || crypto.randomUUID()],
      title: [deficiency?.title || "", Validators.required],
      description: [deficiency?.description || "", Validators.required],
      type: [deficiency?.type || EDeficiencyType.Water, Validators.required],
      location: this.fb.group({
        latitude: [deficiency?.location?.latitude || 0, [Validators.required, Validators.min(-90), Validators.max(90)]],
        longitude: [
          deficiency?.location?.longitude || 0,
          [Validators.required, Validators.min(-180), Validators.max(180)],
        ],
        city: [deficiency?.location?.city || ""],
        country: [deficiency?.location?.country || ""],
        radiusAffected: [deficiency?.location?.radiusAffected || 10],
      }),
      eDangerState: [deficiency?.eDangerState || EDangerState.Moderate, Validators.required],

      ph: [deficiency?.ph || 0, [Validators.required, Validators.min(0), Validators.max(14)]],
      dissolvedOxygen: [deficiency?.dissolvedOxygen || 0, [Validators.required, Validators.min(0), Validators.max(20)]],
      leadConcentration: [
        deficiency?.leadConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(0.01)],
      ],
      mercuryConcentration: [
        deficiency?.mercuryConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(0.001)],
      ],
      nitratesConcentration: [
        deficiency?.nitratesConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(50)],
      ],
      pesticidesContent: [
        deficiency?.pesticidesContent || 0,
        [Validators.required, Validators.min(0), Validators.max(0.005)],
      ],
      microbialActivity: [
        deficiency?.microbialActivity || 0,
        [Validators.required, Validators.min(0), Validators.max(1000)],
      ],
      radiationLevel: [deficiency?.radiationLevel || 0, [Validators.required, Validators.min(0), Validators.max(10)]],
      chemicalOxygenDemand: [
        deficiency?.chemicalOxygenDemand || 0,
        [Validators.required, Validators.min(0), Validators.max(1000)],
      ],
      biologicalOxygenDemand: [
        deficiency?.biologicalOxygenDemand || 0,
        [Validators.required, Validators.min(0), Validators.max(50)],
      ],
      phosphateConcentration: [
        deficiency?.phosphateConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(2)],
      ],
      cadmiumConcentration: [
        deficiency?.cadmiumConcentration || 0,
        [Validators.required, Validators.min(0), Validators.max(0.005)],
      ],
      totalDissolvedSolids: [
        deficiency?.totalDissolvedSolids || 0,
        [Validators.required, Validators.min(0), Validators.max(1500)],
      ],
      electricalConductivity: [
        deficiency?.electricalConductivity || 0,
        [Validators.required, Validators.min(0), Validators.max(2500)],
      ],
      microbialLoad: [deficiency?.microbialLoad || 0, [Validators.required, Validators.min(0), Validators.max(2000)]],

      createdBy: [deficiency?.creator?.id || this.currentUser?.id],
      createdOn: [deficiency?.createdOn || moment()],
      responsibleUserId: [deficiency?.responsibleUser?.id || this.currentUser?.id, [Validators.required]],
      locationId: [deficiency?.location?.id || crypto.randomUUID()],
    });

    this.details = {
      ...this.detailsForm.value,
      creator: {
        firstName: deficiency?.creator?.firstName || this.currentUser?.firstName,
        lastName: deficiency?.creator?.lastName || this.currentUser?.lastName,
      },
      responsibleUser: {
        firstName: deficiency?.responsibleUser?.firstName || this.currentUser?.firstName,
        lastName: deficiency?.responsibleUser?.lastName || this.currentUser?.lastName,
      },
    };
  }

  private loadOrganizationUsers() {
    this.usersAPIService.$organizationUsers.subscribe((orgUsers) => {
      const userId = localStorage.getItem("userId");

      this.currentUser = orgUsers.find((u) => u.id === userId) ?? null;

      if (this.isAddingDeficiency && !this.detailsForm && this.currentUser && this.currentUser.address) {
        this.initializeForm();
      }
    });

    this.usersAPIService.loadOrganizationUsers(-1);
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

  public onSubmit() {
    if (this.detailsForm.invalid) {
      this.detailsForm.markAllAsTouched();
      return;
    }

    const formData: IWaterDeficiency = this.detailsForm.value;

    if (this.isAddingDeficiency) {
      this.deficiencyDataService.addNewWaterDeficiency(formData).subscribe(() => {
        this.router.navigate(["/water"]);
      });
    } else {
      this.deficiencyDataService.updateWaterDeficiencyById(formData.id, formData).subscribe(() => {
        this.router.navigate(["/water"]);
      });
    }
  }

  public onCancel() {
    this.changeMapView();

    this.router.navigate(["/water"]);
  }

  private changeMapView() {
    this.mapViewService.changeFocus({ latitude: 48.65, longitude: 22.26 }, 12);
  }
}
