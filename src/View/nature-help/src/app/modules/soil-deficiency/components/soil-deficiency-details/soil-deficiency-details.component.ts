import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";

import { SoilAPIService } from "@/modules/soil-deficiency/services/soil-api.service";
import { DeficiencyDetailsService } from "@/shared/services/deficiency-details.service";
import { IDeficiencyDetailsState } from "@/shared/models/IDeficiencyFormConfig";
import { SoilDeficiencyFormConfig } from "@/shared/services/deficiency-form-configs.service";

import { UserAPIService } from "@/shared/services/user-api.service";

@Component({
  selector: "n-soil-deficiency-details",
  templateUrl: "./soil-deficiency-details.component.html",
  styleUrls: ["../../../../shared/styles/detail-page.component.css", "./soil-deficiency-details.component.css"],
  standalone: false,
})
export class SoilDeficiencyDetail implements OnInit, OnDestroy {
  state: IDeficiencyDetailsState;
  detailsForm!: FormGroup;
  disableResearchFields: boolean = false;
  private formConfig = new SoilDeficiencyFormConfig();

  constructor(
    public deficiencyDataService: SoilAPIService,
    private activatedRoute: ActivatedRoute,
    public usersAPIService: UserAPIService,
    public deficiencyDetailsService: DeficiencyDetailsService,
  ) {
    this.state = this.deficiencyDetailsService.initializeState();
  }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      const id = params["id"];

      this.deficiencyDetailsService.loadOrganizationUsers(this.state).subscribe(() => {
        if (!id) {
          this.state.isAddingDeficiency = true;
          this.detailsForm = this.deficiencyDetailsService.initializeForm(this.state, this.formConfig);
          this.state.detailsForm = this.detailsForm;
        } else {
          this.deficiencyDataService.getSoilDeficiencyById(id).subscribe((def) => {
            this.detailsForm = this.deficiencyDetailsService.initializeForm(this.state, this.formConfig, def);
            this.state.detailsForm = this.detailsForm;
            this.deficiencyDetailsService.changeMapView(this.state);
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
}
