import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";

import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";
import { DeficiencyDetailsService } from "@/shared/services/deficiency-details.service";
import { IDeficiencyDetailsState } from "@/shared/models/IDeficiencyFormConfig";
import { WaterDeficiencyFormConfig } from "@/shared/services/deficiency-form-configs.service";

import { UserAPIService } from "@/shared/services/user-api.service";
import { DeficiencyConfirmationService } from "@/shared/services/deficiency-confirmation.service";
import { EDeficiencyType } from "@/models/enums";
import { MatSnackBar } from "@angular/material/snack-bar";
import { TranslateService } from "@ngx-translate/core";

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

        if (!id) {
          this.state.isAddingDeficiency = true;
          this.detailsForm = this.deficiencyDetailsService.initializeForm(this.state, this.formConfig);
          this.state.detailsForm = this.detailsForm;
          this.state.details = this.detailsForm.value as any;
          this.isLoading = false;
        } else {
          this.isLoading = true;
          this.deficiencyDataService.getWaterDeficiencyById(id).subscribe({
            next: (def) => {
              this.detailsForm = this.deficiencyDetailsService.initializeForm(this.state, this.formConfig, def);
              this.state.detailsForm = this.detailsForm;
              this.deficiencyDetailsService.loadAttachments(this.state, def.id, def.type);
              this.deficiencyDetailsService.changeMapView(this.state);
              this.isLoading = false;
            },
            error: (err) => {
              console.error("Error loading deficiency", err);
              this.isLoading = false;
            },
          });
        }
      });
    });

    this.deficiencyDetailsService.subscribeToCoordinatesPicking(this.state);
  }

  ngOnDestroy() {
    this.deficiencyDetailsService.ngOnDestroy();
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

  onSubmit(): void {
    if (this.detailsForm.invalid || this.isSaving) return;

    this.isSaving = true;
    this.deficiencyDetailsService.onSubmit(this.state, this.deficiencyDataService, 0).subscribe({
      next: () => {
        this.isSaving = false;
      },
      error: (err) => {
        console.error("Error saving deficiency", err);
        this.isSaving = false;
      },
    });
  }
}
