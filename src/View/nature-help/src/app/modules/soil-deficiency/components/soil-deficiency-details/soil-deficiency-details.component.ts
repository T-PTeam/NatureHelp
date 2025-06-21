import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import moment from "moment";
import { takeUntil } from "rxjs/operators";
import { Subject } from "rxjs";

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
  styleUrls: ["../../../../shared/styles/detail-page.component.css", "./soil-deficiency-details.component.css"],
  standalone: false,
})
export class SoilDeficiencyDetail implements OnInit {
  details: ISoilDeficiency | null = null;
  detailsForm!: FormGroup;
  dangerStates = enumToSelectOptions(EDangerState);
  isSelectingCoordinates: boolean = false;
  selectedAddress: any;

  private isAddingDeficiency: boolean = false;
  private currentUser: IUser | null = null;
  private destroy$ = new Subject<void>();

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
          this.changeMapView();
        });
      }
    });

    this.subscribeToCoordinatesPicking();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
    if (this.isSelectingCoordinates) {
      this.mapViewService.disableCoordinateSelection();
    }
  }

  onSubmit() {
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

  onCancel() {
    this.changeMapView();

    this.router.navigate(["/soil"]);
  }

  toggleCoordinateSelection(event?: Event): void {
    if (event) {
      event.preventDefault();
      event.stopPropagation();
    }
    this.isSelectingCoordinates = !this.isSelectingCoordinates;
    if (this.isSelectingCoordinates) {
      this.mapViewService.enableCoordinateSelection();
    } else {
      this.mapViewService.disableCoordinateSelection();
    }
  }

  private initializeForm(deficiency: ISoilDeficiency | null = null): void {
    this.detailsForm = this.fb.group({
      id: [deficiency?.id || crypto.randomUUID()],
      title: [deficiency?.title || "", Validators.required],
      description: [deficiency?.description || "", Validators.required],
      type: [deficiency?.type || EDeficiencyType.Soil, Validators.required],
      latitude: [deficiency?.latitude || 0, [Validators.required, Validators.min(-90), Validators.max(90)]],
      longitude: [deficiency?.longitude || 0, [Validators.required, Validators.min(-180), Validators.max(180)]],
      radiusAffected: [deficiency?.radiusAffected || 0, [Validators.required, Validators.min(0)]],
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
      const userId = sessionStorage.getItem("userId");

      this.currentUser = orgUsers.find((u) => u.id === userId) ?? null;

      if (this.isAddingDeficiency && !this.detailsForm && this.currentUser) {
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

  private changeMapView() {
    this.mapViewService.changeFocus(
      { latitude: this.details?.latitude || 0, longitude: this.details?.longitude || 0 },
      12,
    );
  }

  private subscribeToCoordinatesPicking(): void {
    this.mapViewService.selectedAddress$.pipe(takeUntil(this.destroy$)).subscribe((address) => {
      this.selectedAddress = address;
      if (address) {
        this.detailsForm.patchValue({ address: address.displayName });
      }
    });
  }
}
