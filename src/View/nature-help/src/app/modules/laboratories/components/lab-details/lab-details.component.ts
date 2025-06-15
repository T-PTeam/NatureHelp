import { Component, OnInit } from "@angular/core";
import { ILaboratory } from "../../models/ILaboratory";
import { MapViewService } from "@/shared/services/map-view.service";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { LabsAPIService } from "../../services/labs-api.service";
import { IUser } from "@/models/IUser";
import { UserAPIService } from "@/shared/services/user-api.service";

@Component({
  selector: "app-lab-details",
  templateUrl: "./lab-details.component.html",
  styleUrls: ["./lab-details.component.css"],
  standalone: false,
})
export class LabDetailsComponent implements OnInit {
  public details: ILaboratory | null = null;
  detailsForm!: FormGroup;

  private isAddingLaboratory: boolean = false;

  constructor(
    private labsAPIService: LabsAPIService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private mapViewService: MapViewService,
    private fb: FormBuilder,
  ) {}

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      const id = params["id"];

      if (!id) {
        this.isAddingLaboratory = true;
        this.initializeForm();
      } else {
        this.labsAPIService.getLabById(id).subscribe((lab) => {
          this.initializeForm(lab);
          this.changeMapView();
        });
      }
    });
  }

  get researchersText(): string {
    return this.details?.researchers?.map((r) => `${r.firstName} ${r.lastName}`).join("\n") || "";
  }

  private initializeForm(laboratory: ILaboratory | null = null): void {
    this.detailsForm = this.fb.group({
      id: [laboratory?.id || crypto.randomUUID()],
      title: [laboratory?.title || "", Validators.required],
      latitude: [laboratory?.latitude || 0, [Validators.required, Validators.min(-90), Validators.max(90)]],
      longitude: [laboratory?.longitude || 0, [Validators.required, Validators.min(-180), Validators.max(180)]],
      researchers: [laboratory?.researchers || []],
      researchersCount: [laboratory?.researchersCount ?? 0, [Validators.required, Validators.min(0)]],
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

  public onSubmit() {
    if (this.detailsForm.invalid) {
      this.detailsForm.markAllAsTouched();
      return;
    }

    const formData: ILaboratory = this.detailsForm.value;
    formData.researchers = [];

    if (this.isAddingLaboratory) {
      this.labsAPIService.addLab(formData).subscribe(() => {
        this.router.navigate(["/labs"]);
      });
    } else {
      this.labsAPIService.updateLabById(formData.id, formData).subscribe(() => {
        this.router.navigate(["/labs"]);
      });
    }
  }

  public onCancel() {
    this.changeMapView();
    this.router.navigate(["/labs"]);
  }

  private changeMapView() {
    this.mapViewService.changeFocus(
      { latitude: this.details?.latitude || 0, longitude: this.details?.longitude || 0 },
      12,
    );
  }
}
