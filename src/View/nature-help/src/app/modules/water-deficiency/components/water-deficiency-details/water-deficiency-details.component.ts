import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import moment from "moment";
import { takeUntil } from "rxjs/operators";
import { Subject } from "rxjs";

import { WaterAPIService } from "@/modules/water-deficiency/services/water-api.service";
import { MapViewService, IAddress } from "@/shared/services/map-view.service";
import { MobileMapService } from "@/shared/services/mobile-map.service";

import { EDangerState, EDeficiencyType } from "../../../../models/enums";
import { IWaterDeficiency } from "../../models/IWaterDeficiency";
import { UserAPIService } from "@/shared/services/user-api.service";
import { IUser } from "@/models/IUser";
import { enumToSelectOptions } from "@/shared/helpers/enum-helper";
import { IDeficiencyAttachment } from "@/models/IAttachment";
import { AttachmentAPIService } from "@/shared/services/attachment-api.service";
import { UploadService } from "@/shared/services/upload.service";
import { MatDialog } from "@angular/material/dialog";
import { AttachmentPreviewDialogComponent } from "@/shared/components/dialogs/attachment-preview-dialog/attachment-preview-dialog.component";

@Component({
  selector: "n-water-deficiency-details",
  templateUrl: "./water-deficiency-details.component.html",
  styleUrls: ["../../../../shared/styles/detail-page.component.css", "./water-deficiency-details.component.css"],
  standalone: false,
})
export class WaterDeficiencyDetail implements OnInit, OnDestroy {
  details: IWaterDeficiency | null = null;
  detailsForm!: FormGroup;
  dangerStates = enumToSelectOptions(EDangerState);
  changedModelLogsOpened: boolean = false;
  isSelectingCoordinates: boolean = false;
  selectedAddress: IAddress | null = null;
  attachments: IDeficiencyAttachment[] = [];

  isUploading: boolean = false;
  uploadProgress: number = 0;
  lastUploadError: string | null = null;
  lastUploadedFiles: File[] = [];

  isAddingDeficiency: boolean = false;
  private currentUser: IUser | null = null;
  private destroy$ = new Subject<void>();

  constructor(
    private deficiencyDataService: WaterAPIService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private mapViewService: MapViewService,
    private mobileMapService: MobileMapService,
    private fb: FormBuilder,
    public usersAPIService: UserAPIService,
    private attachmentService: AttachmentAPIService,
    private snackBar: MatSnackBar,
    private uploadService: UploadService,
    private dialog: MatDialog,
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
          this.loadAttachments(def.id, def.type);
          this.changeMapView();
        });
      }
    });

    this.subscribeToCoordinatesPicking();
  }

  showPreview(attachment: IDeficiencyAttachment) {
    this.dialog.open(AttachmentPreviewDialogComponent, {
      data: {
        url: attachment.previewUrl,
        fileName: attachment.fileName,
      },
      panelClass: "attachment-preview-modal",
      maxWidth: "90vw",
    });
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

    const formData: IWaterDeficiency = this.detailsForm.value;

    if (this.mobileMapService.isMobile()) {
      this.mobileMapService.hideMobileMap();
    }

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

  onCancel() {
    this.changeMapView();

    if (this.mobileMapService.isMobile()) {
      this.mobileMapService.hideMobileMap();
    }

    this.router.navigate(["/water"]);
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

  private initializeForm(deficiency: IWaterDeficiency | null = null) {
    this.detailsForm = this.fb.group({
      id: [deficiency?.id || crypto.randomUUID()],
      title: [deficiency?.title || "", Validators.required],
      description: [deficiency?.description || "", Validators.required],
      type: [deficiency?.type || EDeficiencyType.Water, Validators.required],
      latitude: [deficiency?.latitude || 0, [Validators.required, Validators.min(-90), Validators.max(90)]],
      longitude: [deficiency?.longitude || 0, [Validators.required, Validators.min(-180), Validators.max(180)]],
      address: [deficiency?.address || ""],
      radiusAffected: [deficiency?.radiusAffected || 0, [Validators.required, Validators.min(0)]],
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

  private subscribeToCoordinatesPicking(): void {
    this.mapViewService.selectedCoordinates$.subscribe((coordinates) => {
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

  onUploadStarted(files: File[]) {
    console.log(
      "Upload started for files:",
      files.map((f) => f.name),
    );
    this.isUploading = true;
    this.lastUploadError = null;
    this.uploadProgress = 0;

    this.snackBar.open("Upload started...", "Close", {
      duration: 2000,
      panelClass: ["info-snackbar"],
    });
  }

  onUploadCompleted(newAttachments: IDeficiencyAttachment[]) {
    console.log("Upload completed:", newAttachments);
    this.isUploading = false;
    this.uploadProgress = 100;

    this.attachments = [...this.attachments, ...newAttachments];

    this.snackBar.open(`Successfully uploaded ${newAttachments.length} file(s)`, "Close", {
      duration: 3000,
      panelClass: ["success-snackbar"],
    });

    if (this.details?.id) {
      this.loadAttachments(this.details.id, this.details.type);
    }
  }

  onUploadError(error: string) {
    console.error("Upload error:", error);
    this.isUploading = false;
    this.lastUploadError = error;
    this.uploadProgress = 0;

    this.snackBar.open(`Upload failed: ${error}`, "Close", {
      duration: 5000,
      panelClass: ["error-snackbar"],
    });
  }

  onUploadProgress(progress: number) {
    console.log("Upload progress:", progress);
    this.uploadProgress = progress;
  }

  retryUpload(): void {
    if (this.lastUploadedFiles.length > 0) {
      console.log(
        "Retrying upload for files:",
        this.lastUploadedFiles.map((f) => f.name),
      );
      this.isUploading = true;
      this.lastUploadError = null;

      const uploadObservable = this.uploadService.uploadFilesParallel(this.lastUploadedFiles, this.details!.id, 3);

      uploadObservable.subscribe({
        next: (results) => {
          const successfulUploads = results.filter((result) => result.attachment).map((result) => result.attachment!);

          const failedUploads = results.filter((result) => result.error).map((result) => result.error!);

          if (successfulUploads.length > 0) {
            this.attachments = [...this.attachments, ...successfulUploads];
            this.snackBar.open(`Successfully uploaded ${successfulUploads.length} file(s)`, "Close", {
              duration: 3000,
              panelClass: "success-snackbar",
            });
          }

          if (failedUploads.length > 0) {
            this.lastUploadError = failedUploads.join(", ");
            this.snackBar.open(`Failed to upload ${failedUploads.length} file(s): ${this.lastUploadError}`, "Close", {
              duration: 5000,
              panelClass: "error-snackbar",
            });
          }

          this.isUploading = false;
        },
        error: (error) => {
          console.error("Retry upload error:", error);
          this.isUploading = false;
          this.lastUploadError = error;
          this.snackBar.open(`Upload failed: ${error}`, "Close", { duration: 5000, panelClass: "error-snackbar" });
        },
      });
    }
  }

  testUpload(): void {
    console.log("=== TEST UPLOAD DEBUG ===");
    console.log("Details:", this.details);
    console.log("Details ID:", this.details?.id);
    console.log("Is Adding Deficiency:", this.isAddingDeficiency);
    console.log("Current Attachments:", this.attachments);
    console.log("Upload Service Available:", !!this.uploadService);

    const testFile = new File(["test content"], "test.txt", { type: "text/plain" });
    console.log("Test file created:", testFile);

    if (this.details?.id) {
      console.log("Testing upload service with file:", testFile.name);
      this.uploadService.uploadFile(testFile, this.details.id).subscribe({
        next: (attachment) => {
          console.log("Test upload successful:", attachment);
          this.snackBar.open("Test upload successful!", "Close", { duration: 3000 });
        },
        error: (error) => {
          console.error("Test upload failed:", error);
          this.snackBar.open(`Test upload failed: ${error}`, "Close", { duration: 5000 });
        },
      });
    } else {
      console.error("No deficiency ID available for test upload");
      this.snackBar.open("No deficiency ID available for test upload", "Close", { duration: 3000 });
    }
  }

  onFilesSelected(files: File[]): void {
    console.log("Files selected:", files);
    this.lastUploadedFiles = files;
    this.isUploading = true;
    this.lastUploadError = null;
    this.uploadProgress = 0;

    const progressSubscription = this.uploadService.uploadProgress$.subscribe((progress) => {
      this.uploadProgress = this.uploadService.getOverallProgress();
    });

    this.uploadService.uploadFilesParallel(files, this.details!.id, 3).subscribe({
      next: (results) => {
        const successfulUploads = results.filter((result) => result.attachment).map((result) => result.attachment!);

        const failedUploads = results.filter((result) => result.error).map((result) => result.error!);

        if (successfulUploads.length > 0) {
          this.attachments = [...this.attachments, ...successfulUploads];
          this.snackBar.open(`Successfully uploaded ${successfulUploads.length} file(s)`, "Close", {
            duration: 3000,
            panelClass: "success-snackbar",
          });
        }

        if (failedUploads.length > 0) {
          this.lastUploadError = failedUploads.join(", ");
          this.snackBar.open(`Failed to upload ${failedUploads.length} file(s): ${this.lastUploadError}`, "Close", {
            duration: 5000,
            panelClass: "error-snackbar",
          });
        }

        this.isUploading = false;
        this.uploadProgress = 100;
        progressSubscription.unsubscribe();
      },
      error: (error) => {
        console.error("Upload error:", error);
        this.isUploading = false;
        this.lastUploadError = error;
        this.uploadProgress = 0;
        progressSubscription.unsubscribe();
        this.snackBar.open(`Upload failed: ${error}`, "Close", { duration: 5000, panelClass: "error-snackbar" });
      },
    });
  }

  onFileSelected(file: File): void {
    console.log("Single file selected:", file);
    this.onFilesSelected([file]);
  }

  onFileRemoved(file: File): void {
    console.log("File removed:", file);
    this.lastUploadedFiles = this.lastUploadedFiles.filter((f) => f !== file);
  }

  loadAttachments(deficiencyId: string, deficiencyType: EDeficiencyType) {
    if (!deficiencyId) {
      console.warn("No deficiency ID provided for loading attachments");
      return;
    }

    this.attachmentService.getAttachments(deficiencyId, deficiencyType).subscribe({
      next: (attachments) => {
        this.attachments = attachments || [];
      },
      error: (error) => {
        console.error("Error loading attachments:", error);
        this.attachments = [];
      },
    });
  }
}
