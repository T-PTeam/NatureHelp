import { Component, OnInit } from "@angular/core";
import { ILaboratory } from "../../models/ILaboratory";
import { MapViewService } from "@/shared/services/map-view.service";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import moment from "moment";
import { LabsAPIService } from "../../services/labs-api.service";

@Component({
  selector: "app-lab-details",
  templateUrl: "./lab-details.component.html",
  styleUrls: ["./lab-details.component.css"],
  standalone: false,
})
export class LabDetailsComponent implements OnInit {
  public details: ILaboratory | null = null;
  detailsForm!: FormGroup;

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
        this.initializeForm();
        this.details = this.detailsForm.value;
      } else {
        this.labsAPIService.getLabById(id).subscribe((def) => {
          this.initializeForm(def);
          this.details = def;
        });
      }
    });
  }

  get researchersText(): string {
    return this.details?.researchers?.map((r) => `${r.firstName} ${r.lastName}`).join("\n") || "";
  }

  private initializeForm(laboratory: ILaboratory | null = null) {
    this.detailsForm = this.fb.group({
      id: [laboratory?.id || ""],
      title: [laboratory?.title || moment()],
      researches: [laboratory?.researchers || []],
      location: [laboratory?.location || { city: "", country: "" }, Validators.required],
      researchersCount: [laboratory?.researchersCount || 0, Validators.required],
    });
  }

  public onSubmit() {
    if (this.detailsForm.invalid) {
      return;
    }

    const formData: ILaboratory = this.detailsForm.value;

    this.labsAPIService.updateLabById(formData.id, formData).subscribe((lab) => {
      console.log("Updated:", lab);
      this.router.navigate(["/labs"]);
    });
  }

  public onCancel() {
    this.changeMapView();
    this.router.navigate(["/labs"]);
  }

  private changeMapView() {
    this.mapViewService.changeFocus({ latitude: 48.65, longitude: 22.26 }, 12);
  }
}
