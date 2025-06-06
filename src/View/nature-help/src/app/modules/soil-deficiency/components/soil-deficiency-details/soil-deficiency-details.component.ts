import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import moment from "moment";

import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";
import { MapViewService } from "@/shared/services/map-view.service";

import { EDangerState, EDeficiencyType } from "../../../../models/enums";
import { ISoilDeficiency } from "../../models/ISoilDeficiency";
import { IUser } from "@/models/IUser";
import { UserAPIService } from "@/shared/services/user-api.service";
import { enumToSelectOptions } from "@/shared/helpers/enum-helper";

@Component({
  selector: "n-soil-deficiency-details",
  templateUrl: "./soil-deficiency-details.component.html",
  styleUrls: ["./soil-deficiency-details.component.css"],
  standalone: false,
})
export class SoilDeficiencyDetail implements OnInit {
  details: ISoilDeficiency | null = null;
  detailsForm!: FormGroup;
  dangerStates = enumToSelectOptions(EDangerState);

  private isAddingDeficiency: boolean = false;
  private currentUser: IUser | null = null;

  constructor(
    private deficiencyDataService: SoilAPIService,
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
        this.deficiencyDataService.getSoilDeficiencyById(id).subscribe((def) => {
          this.initializeForm(def);
        });
      }
    });
  }

  private initializeForm(deficiency: ISoilDeficiency | null = null): void {
    this.detailsForm = this.fb.group({
      id: [deficiency?.id || crypto.randomUUID()],
      title: [deficiency?.title || "", Validators.required],
      description: [deficiency?.description || "", Validators.required],
      type: [deficiency?.type || EDeficiencyType.Soil, Validators.required],
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

      ph: [deficiency?.ph ?? 6.5, [Validators.required, Validators.min(0)]],
      organicMatter: [deficiency?.organicMatter || 0, [Validators.required, Validators.min(0)]],
      leadConcentration: [deficiency?.leadConcentration || 0, [Validators.required, Validators.min(0)]],
      cadmiumConcentration: [deficiency?.cadmiumConcentration || 0, [Validators.required, Validators.min(0)]],
      mercuryConcentration: [deficiency?.mercuryConcentration || 0, [Validators.required, Validators.min(0)]],
      pesticidesContent: [deficiency?.pesticidesContent || 0, [Validators.required, Validators.min(0)]],
      nitratesConcentration: [deficiency?.nitratesConcentration || 0, [Validators.required, Validators.min(0)]],
      heavyMetalsConcentration: [deficiency?.heavyMetalsConcentration || 0, [Validators.required, Validators.min(0)]],
      electricalConductivity: [deficiency?.electricalConductivity || 0, [Validators.required, Validators.min(0)]],
      microbialActivity: [deficiency?.microbialActivity || 0, [Validators.required, Validators.min(0)]],
      analysisDate: [deficiency?.analysisDate || moment()],

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
        email: deficiency?.creator?.email || this.currentUser?.email,
        role: deficiency?.creator?.role || this.currentUser?.role,
      },
      responsibleUser: {
        firstName: deficiency?.responsibleUser?.firstName || this.currentUser?.firstName,
        lastName: deficiency?.responsibleUser?.lastName || this.currentUser?.lastName,
        email: deficiency?.responsibleUser?.email || this.currentUser?.email,
        role: deficiency?.responsibleUser?.role || this.currentUser?.role,
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

    const formData: ISoilDeficiency = this.detailsForm.value;

    if (this.isAddingDeficiency) {
      this.deficiencyDataService.addNewSoilDeficiency(formData).subscribe(() => {
        this.router.navigate(["/soil"]);
      });
    } else {
      this.deficiencyDataService.updateSoilDeficiencyById(formData.id, formData).subscribe(() => {
        this.router.navigate(["/soil"]);
      });
    }
  }

  public onCancel() {
    this.changeMapView();

    this.router.navigate(["/soil"]);
  }

  private changeMapView() {
    this.mapViewService.changeFocus({ latitude: 48.65, longitude: 22.26 }, 12);
  }
}
