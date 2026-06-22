import { Component, OnInit, OnDestroy } from "@angular/core";
import { Location } from "@angular/common";
import { ILaboratory } from "../../models/ILaboratory";
import { MapViewService, IAddress } from "@/shared/services/map-view.service";
import { MobileMapService } from "@/shared/services/mobile-map.service";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { LabsAPIService } from "../../services/labs-api.service";
import { UserAPIService } from "@/shared/services/user-api.service";
import { MatDialog } from "@angular/material/dialog";
import { ERole } from "@/models/enums";
import { IUser } from "@/models/IUser";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { LabResearchersDialogComponent } from "../lab-researchers-dialog/lab-researchers-dialog.component";

@Component({
  selector: "app-lab-details",
  templateUrl: "./lab-details.component.html",
  styleUrls: ["../../../../shared/styles/detail-page.component.css", "./lab-details.component.css"],
  standalone: false,
})
export class LabDetailsComponent implements OnInit, OnDestroy {
  details: ILaboratory | null = null;
  detailsForm!: FormGroup;
  isSelectingCoordinates: boolean = false;
  selectedAddress: IAddress | null = null;
  organizationResearchers: IUser[] = [];
  pendingResearcherIds: string[] = [];

  private isAddingLaboratory: boolean = false;
  private destroy$ = new Subject<void>();
  private coordinatesPickingSubscribed = false;

  get researchersText(): string {
    const sourceResearchers =
      this.organizationResearchers.length > 0
        ? this.organizationResearchers.filter((researcher) => this.pendingResearcherIds.includes(researcher.id))
        : (this.details?.researchers ?? []);

    return sourceResearchers.map((researcher) => `${researcher.firstName} ${researcher.lastName}`).join("\n");
  }

  constructor(
    private labsAPIService: LabsAPIService,
    private userAPIService: UserAPIService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private location: Location,
    private mapViewService: MapViewService,
    private mobileMapService: MobileMapService,
    private fb: FormBuilder,
    private dialog: MatDialog,
  ) {}

  ngOnInit(): void {
    this.loadOrganizationResearchers();

    this.activatedRoute.params.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      const id = params["id"];

      if (!id) {
        this.isAddingLaboratory = true;
        this.initializeForm();
      } else {
        this.labsAPIService.getLabById(id).subscribe((lab: ILaboratory) => {
          this.initializeForm(lab);
          this.changeMapView();
        });
      }
    });

    this.ensureCoordinatesPickingSubscription();
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

    const formData: ILaboratory = {
      ...this.detailsForm.value,
      researcherIds: this.pendingResearcherIds,
    };
    formData.researchers = [];

    if (this.mobileMapService.isMobile()) {
      this.mobileMapService.hideMobileMap();
    }

    if (this.isAddingLaboratory) {
      this.labsAPIService.addLab(formData).subscribe({
        next: (lab) => {
          this.labsAPIService.assignResearchersToLab(lab.id, this.pendingResearcherIds).subscribe({
            next: () => {
              this.router.navigate(["/labs"]);
            },
            error: (error: any) => {
              console.error("Error assigning laboratory researchers:", error);
            },
          });
        },
        error: (error: any) => {
          console.error("Error creating laboratory:", error);
        },
      });
    } else {
      this.labsAPIService.updateLabById(formData.id, formData).subscribe({
        next: (lab) => {
          this.labsAPIService.assignResearchersToLab(lab.id, this.pendingResearcherIds).subscribe({
            next: () => {
              this.router.navigate(["/labs"]);
            },
            error: (error: any) => {
              console.error("Error assigning laboratory researchers:", error);
            },
          });
        },
        error: (error: any) => {
          console.error("Error updating laboratory:", error);
        },
      });
    }
  }

  openResearchersDialog(): void {
    const dialogRef = this.dialog.open(LabResearchersDialogComponent, {
      width: "640px",
      maxWidth: "90vw",
      data: {
        researchers: this.organizationResearchers,
        selectedResearcherIds: this.pendingResearcherIds,
      },
    });

    dialogRef
      .afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe((selectedResearcherIds?: string[]) => {
        if (!selectedResearcherIds) {
          return;
        }

        this.pendingResearcherIds = selectedResearcherIds;
        this.detailsForm.patchValue({
          researchersCount: selectedResearcherIds.length,
        });
      });
  }

  onCancel() {
    if (this.mobileMapService.isMobile()) {
      this.mobileMapService.hideMobileMap();
    }

    this.router.navigate(["/labs"]);
  }

  onBack(): void {
    if (this.mobileMapService.isMobile()) {
      this.mobileMapService.hideMobileMap();
    }

    const navigationId = window.history.state?.navigationId;
    if (typeof navigationId === "number" && navigationId > 1) {
      this.location.back();
      return;
    }

    this.router.navigate(["/labs"]);
  }

  toggleCoordinateSelection(event?: Event): void {
    if (event) {
      event.preventDefault();
      event.stopPropagation();
    }
    this.isSelectingCoordinates = !this.isSelectingCoordinates;
    if (this.isSelectingCoordinates) {
      this.mapViewService.enableCoordinateSelection();
      if (this.mobileMapService.isMobile()) {
        this.mobileMapService.showMobileMap();
      }
    } else {
      this.mapViewService.disableCoordinateSelection();
    }
  }

  changeMapView() {
    this.mapViewService.changeFocus(
      { latitude: this.details?.latitude || 0, longitude: this.details?.longitude || 0 },
      12,
    );

    if (this.mobileMapService.isMobile()) {
      this.mobileMapService.showMobileMap();
    }
  }

  private initializeForm(laboratory?: ILaboratory): void {
    this.detailsForm = this.fb.group({
      id: [laboratory?.id || crypto.randomUUID()],
      title: [laboratory?.title || "", Validators.required],
      latitude: [laboratory?.latitude || 0, [Validators.required, Validators.min(-90), Validators.max(90)]],
      longitude: [laboratory?.longitude || 0, [Validators.required, Validators.min(-180), Validators.max(180)]],
      researchers: [laboratory?.researchers || []],
      researchersCount: [laboratory?.researchersCount ?? 0, [Validators.required, Validators.min(0)]],
      address: [laboratory?.address || ""],
      isPublic: [laboratory?.isPublic || false],
    });

    this.pendingResearcherIds =
      laboratory?.researcherIds ?? laboratory?.researchers?.map((researcher) => researcher.id) ?? [];
    this.detailsForm.patchValue({
      researchersCount: this.pendingResearcherIds.length,
    });
    this.details = {
      ...this.detailsForm.value,
    };

    this.ensureCoordinatesPickingSubscription();
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

  private loadOrganizationResearchers(): void {
    this.userAPIService
      .fetchOrganizationUsers(-1, null, { silent: true })
      .pipe(takeUntil(this.destroy$))
      .subscribe((users) => {
        this.organizationResearchers = users.filter((user) => user.role === ERole.Researcher);
      });
  }

  private ensureCoordinatesPickingSubscription(): void {
    if (this.coordinatesPickingSubscribed) {
      return;
    }

    this.coordinatesPickingSubscribed = true;
    this.subscribeToCoordinatesPicking();
  }

  private subscribeToCoordinatesPicking(): void {
    this.mapViewService.selectedCoordinates$.pipe(takeUntil(this.destroy$)).subscribe((coordinates) => {
      if (!coordinates || !this.detailsForm) {
        return;
      }

      this.detailsForm.patchValue({
        latitude: coordinates.latitude,
        longitude: coordinates.longitude,
      });
      this.isSelectingCoordinates = false;
    });

    this.mapViewService.selectedAddress$.pipe(takeUntil(this.destroy$)).subscribe((address) => {
      this.selectedAddress = address;
      if (address && this.detailsForm) {
        this.detailsForm.patchValue({ address: address.displayName });
      }
    });
  }
}
