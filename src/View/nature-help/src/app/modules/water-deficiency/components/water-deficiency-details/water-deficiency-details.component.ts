import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { Subject } from "rxjs";

import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";
import { DeficiencyDetailsService } from "@/shared/services/deficiency-details.service";
import { IDeficiencyDetailsState } from "@/shared/models/IDeficiencyFormConfig";
import { WaterDeficiencyFormConfig } from "@/shared/services/deficiency-form-configs.service";

import { UserAPIService } from "@/shared/services/user-api.service";
import { DeficiencyConfirmationService } from "@/shared/services/deficiency-confirmation.service";
import { EDeficiencyType } from "@/models/enums";
import { MatSnackBar } from "@angular/material/snack-bar";
import { TranslateService } from "@ngx-translate/core";
import { applyServerValidationErrors } from "@/shared/helpers/form-validation.helper";
import { getErrorMessage } from "@/shared/utils/error.utils";

@Component({
  selector: "n-water-deficiency-details",
  templateUrl: "./water-deficiency-details.component.html",
  styleUrls: ["../../../../shared/styles/detail-page.component.css", "./water-deficiency-details.component.css"],
  standalone: false,
})
export class WaterDeficiencyDetail implements OnInit, OnDestroy {
  state: IDeficiencyDetailsState;
  detailsForm!: FormGroup;
  changedModelLogsOpened: boolean = false;
  disableResearchFields: boolean = false;
  isLoading: boolean = true;
  isLoadingUsers: boolean = true;
  isSaving: boolean = false;
  confirmDeficiencyPending = false;
  confirmDeficiencyDone = false;
  private formConfig = new WaterDeficiencyFormConfig();
  private destroy$ = new Subject<void>();

  constructor(
    public deficiencyDataService: WaterAPIService,
    private activatedRoute: ActivatedRoute,
    public usersAPIService: UserAPIService,
    public deficiencyDetailsService: DeficiencyDetailsService,
    private deficiencyConfirmation: DeficiencyConfirmationService,
    private snackBar: MatSnackBar,
    private translate: TranslateService,
  ) {
    this.state = this.deficiencyDetailsService.initializeState();
  }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      const id = params["id"];

      this.deficiencyDetailsService.loadOrganizationUsers(this.state).subscribe(() => {
        this.isLoadingUsers = false;
      });

      if (!id) {
        this.state.isAddingDeficiency = true;
        this.detailsForm = this.deficiencyDetailsService.initializeForm(this.state, this.formConfig);
        this.state.detailsForm = this.detailsForm;
        this.state.details = this.detailsForm.value as any;
        this.deficiencyDetailsService.subscribeToCoordinatesPicking(this.state, this.destroy$);
        this.deficiencyDetailsService.subscribeToCoordinateFormChanges(this.state, this.destroy$);
        this.isLoading = false;
        return;
      }

      this.isLoading = true;
      this.deficiencyDataService.getWaterDeficiencyById(id).subscribe({
        next: (def) => {
          this.detailsForm = this.deficiencyDetailsService.initializeForm(this.state, this.formConfig, def);
          this.state.detailsForm = this.detailsForm;
          this.deficiencyDetailsService.loadAttachments(this.state, def.id, def.type);
          this.deficiencyDetailsService.subscribeToCoordinatesPicking(this.state, this.destroy$);
          this.deficiencyDetailsService.subscribeToCoordinateFormChanges(this.state, this.destroy$);
          this.deficiencyDetailsService.changeMapView(this.state);
          this.isLoading = false;
        },
        error: (err) => {
          console.error("Error loading deficiency", err);
          this.isLoading = false;
          this.snackBar.open(this.translate.instant("deficiency.loadError"), undefined, { duration: 4000 });
        },
      });
    });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  get isMonitoringActive(): boolean {
    return this.state.details?.deficiencyMonitoring?.isMonitoring ?? false;
  }

  showConfirmAction(): boolean {
    if (this.state.isAddingDeficiency || !this.state.details?.id) return false;
    const uid = sessionStorage.getItem("userId");
    const creatorId = this.state.details.createdBy?.id;
    if (!uid || !creatorId || !sessionStorage.getItem("accessToken")) return false;
    return uid !== creatorId;
  }

  onConfirmDeficiency(): void {
    const id = this.state.details?.id;
    if (!id || this.confirmDeficiencyPending) return;
    this.confirmDeficiencyPending = true;
    this.deficiencyConfirmation.confirm(id, EDeficiencyType.Water).subscribe({
      next: (r) => {
        this.confirmDeficiencyPending = false;
        this.confirmDeficiencyDone = true;
        const key = r.alreadyConfirmed ? "deficiency.alreadyConfirmed" : "deficiency.confirmThanks";
        this.snackBar.open(this.translate.instant(key), undefined, { duration: 4000 });
      },
      error: () => {
        this.confirmDeficiencyPending = false;
        this.snackBar.open(this.translate.instant("deficiency.confirmError"), undefined, { duration: 4000 });
      },
    });
  }

  onResearchFieldsToggle(disabled: boolean): void {
    this.deficiencyDetailsService.setResearchFieldsDisabled(
      this.detailsForm,
      this.formConfig.researchFieldNames,
      disabled,
    );
  }

  fieldError(controlName: string, rangeKey: string, requiredKey?: string): string {
    const control = this.detailsForm.get(controlName);
    if (!control?.invalid || !control.touched) {
      return "";
    }

    if (control.getError("server")) {
      return control.getError("server");
    }

    if (requiredKey && control.hasError("required")) {
      return this.translate.instant(requiredKey);
    }

    return this.translate.instant(rangeKey);
  }

  onSubmit(): void {
    if (this.detailsForm.invalid || this.isSaving) {
      if (this.detailsForm.invalid) {
        this.detailsForm.markAllAsTouched();
        this.snackBar.open(this.translate.instant("forms.formInvalid"), undefined, { duration: 4000 });
      }
      return;
    }

    this.isSaving = true;
    this.deficiencyDetailsService.onSubmit(this.state, this.deficiencyDataService, 0).subscribe({
      next: () => {
        this.isSaving = false;
      },
      error: (err) => {
        console.error("Error saving deficiency", err);
        this.isSaving = false;
        if (applyServerValidationErrors(this.detailsForm, err)) {
          const hasResearchFieldErrors = this.formConfig.researchFieldNames.some((name) =>
            this.detailsForm.get(name)?.hasError("server"),
          );
          if (hasResearchFieldErrors && this.disableResearchFields) {
            this.disableResearchFields = false;
            this.onResearchFieldsToggle(false);
          }
          this.snackBar.open(this.translate.instant("forms.formInvalid"), undefined, { duration: 4000 });
          return;
        }
        this.snackBar.open(getErrorMessage(err), undefined, { duration: 4000 });
      },
    });
  }
}
