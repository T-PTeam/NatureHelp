import { Component, OnInit, OnDestroy } from "@angular/core";
import { ILaboratory } from "../../models/ILaboratory";
import { MapViewService, IAddress } from "@/shared/services/map-view.service";
import { MobileMapService } from "@/shared/services/mobile-map.service";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { LabsAPIService } from "../../services/labs-api.service";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

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

  private isAddingLaboratory: boolean = false;
  private destroy$ = new Subject<void>();

  get researchersText(): string {
    return this.details?.researchers?.map((r) => `${r.firstName} ${r.lastName}`).join("\n") || "";
  }

  constructor(
    private labsAPIService: LabsAPIService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private mapViewService: MapViewService,
    private mobileMapService: MobileMapService,
    private fb: FormBuilder,
  ) {}

  ngOnInit(): void {
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

    const formData: ILaboratory = this.detailsForm.value;
    formData.researchers = [];

    if (this.mobileMapService.isMobile()) {
      this.mobileMapService.hideMobileMap();
    }

    if (this.isAddingLaboratory) {
      this.labsAPIService.addLab(formData).subscribe({
        next: () => {
          this.router.navigate(["/labs"]);
        },
        error: (error: any) => {
          console.error("Error creating laboratory:", error);
        },
      });
    } else {
      this.labsAPIService.updateLabById(formData.id, formData).subscribe({
        next: () => {
          this.router.navigate(["/labs"]);
        },
        error: (error: any) => {
          console.error("Error updating laboratory:", error);
        },
      });
    }
  }

  onCancel() {
    if (this.mobileMapService.isMobile()) {
      this.mobileMapService.hideMobileMap();
    }

    this.router.navigate(["/laboratories"]);
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
    });

    this.details = {
      ...this.detailsForm.value,
    };
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

  private subscribeToCoordinatesPicking(): void {
    this.mapViewService.selectedCoordinates$.pipe(takeUntil(this.destroy$)).subscribe((coordinates) => {
      if (coordinates) {
        this.detailsForm.patchValue({
          latitude: coordinates.latitude,
          longitude: coordinates.longitude,
        });
        this.isSelectingCoordinates = false;
      }
    });

    this.mapViewService.selectedAddress$.pipe(takeUntil(this.destroy$)).subscribe((address) => {
      this.selectedAddress = address;
      if (address) {
        this.detailsForm.patchValue({ address: address.displayName });
      }
    });
  }
}
