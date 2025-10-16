import { Injectable, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatDialog } from "@angular/material/dialog";
import moment from "moment";
import { takeUntil } from "rxjs/operators";
import { Subject, Observable } from "rxjs";

import { MapViewService, IAddress } from "./map-view.service";
import { MobileMapService } from "./mobile-map.service";
import { AuditService } from "./audit.service";
import { UserAPIService } from "./user-api.service";
import { AttachmentAPIService } from "./attachment-api.service";
import { UploadService } from "./upload.service";

import { EDangerState, EDeficiencyType } from "@/models/enums";
import { IDeficiency } from "@/models/IDeficiency";
import { IUser } from "@/models/IUser";
import { IDeficiencyAttachment } from "@/models/IAttachment";
import { enumToSelectOptions } from "@/shared/helpers/enum-helper";
import { AttachmentPreviewDialogComponent } from "@/shared/components/dialogs/attachment-preview-dialog/attachment-preview-dialog.component";
import { IDeficiencyDetailsState, IDeficiencyFormConfig } from "@/shared/models/IDeficiencyFormConfig";

const defaultUser: IUser = {
  id: "",
  firstName: "",
  lastName: "",
  email: "",
  password: "",
  passwordHash: "",
  role: 0,
  organizationId: null,
};

@Injectable({
  providedIn: "root",
})
export class DeficiencyDetailsService implements OnDestroy {
  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private mapViewService: MapViewService,
    private mobileMapService: MobileMapService,
    private usersAPIService: UserAPIService,
    private attachmentService: AttachmentAPIService,
    private snackBar: MatSnackBar,
    private uploadService: UploadService,
    private dialog: MatDialog,
    private auditService: AuditService,
  ) {}

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  initializeState(): IDeficiencyDetailsState {
    return {
      details: null,
      detailsForm: this.fb.group({}),
      dangerStates: enumToSelectOptions(EDangerState),
      isSelectingCoordinates: false,
      selectedAddress: null,
      attachments: [],
      isUploading: false,
      uploadProgress: 0,
      lastUploadError: null,
      lastUploadedFiles: [],
      isAddingDeficiency: false,
      currentUser: null,
    };
  }

  initializeForm(
    state: IDeficiencyDetailsState,
    config: IDeficiencyFormConfig,
    deficiency: IDeficiency | null = null,
  ): FormGroup {
    const commonFields = {
      id: [deficiency?.id || crypto.randomUUID()],
      title: [deficiency?.title || "", Validators.required],
      description: [deficiency?.description || "", Validators.required],
      type: [deficiency?.type || config.deficiencyType, Validators.required],
      latitude: [deficiency?.latitude || 0, [Validators.required, Validators.min(-90), Validators.max(90)]],
      longitude: [deficiency?.longitude || 0, [Validators.required, Validators.min(-180), Validators.max(180)]],
      address: [deficiency?.address || ""],
      radiusAffected: [deficiency?.radiusAffected || 0, [Validators.required, Validators.min(0)]],
      eDangerState: [deficiency?.eDangerState || EDangerState.Moderate, Validators.required],
      createdBy: [deficiency?.creator?.id || state.currentUser?.id],
      createdOn: [deficiency?.createdOn || moment()],
      responsibleUserId: [deficiency?.responsibleUser?.id || state.currentUser?.id, [Validators.required]],
      isMonitoring: [deficiency?.deficiencyMonitoring?.isMonitoring || false],
    };

    const specificFields = config.getSpecificFormFields(deficiency, state.currentUser);
    const allFields = { ...commonFields, ...specificFields };

    const form = this.fb.group(allFields);

    const formValue = form.value;
    state.details = {
      ...formValue,
      creator: deficiency?.creator || state.currentUser || defaultUser,
      responsibleUser: deficiency?.responsibleUser || state.currentUser || defaultUser,
      deficiencyMonitoring: deficiency?.deficiencyMonitoring,
    } as IDeficiency;

    return form;
  }

  loadOrganizationUsers(state: IDeficiencyDetailsState): Observable<void> {
    return new Observable((observer) => {
      this.usersAPIService.$organizationUsers.pipe(takeUntil(this.destroy$)).subscribe((orgUsers) => {
        const userId = sessionStorage.getItem("userId");
        state.currentUser = orgUsers.find((u) => u.id === userId) ?? null;

        if (state.detailsForm && state.currentUser) {
          state.detailsForm.patchValue({
            createdBy: state.currentUser.id,
            responsibleUserId: state.currentUser.id,
          });

          if (state.details) {
            state.details.creator = state.currentUser;
            state.details.responsibleUser = state.currentUser;
          }
        }

        if (orgUsers.length > 0 && !observer.closed) {
          observer.next();
          observer.complete();
        }
      });

      this.usersAPIService.loadOrganizationUsers(-1);
    });
  }

  subscribeToCoordinatesPicking(state: IDeficiencyDetailsState): void {
    this.mapViewService.selectedCoordinates$.subscribe((coordinates) => {
      if (coordinates) {
        state.detailsForm.patchValue({
          latitude: coordinates.latitude,
          longitude: coordinates.longitude,
        });
        state.isSelectingCoordinates = false;
      }
    });

    this.mapViewService.selectedAddress$.pipe(takeUntil(this.destroy$)).subscribe((address) => {
      state.selectedAddress = address;
      if (address) {
        state.detailsForm.patchValue({ address: address.displayName });
      }
    });
  }

  toggleCoordinateSelection(state: IDeficiencyDetailsState, event?: Event): void {
    if (event) {
      event.preventDefault();
      event.stopPropagation();
    }
    state.isSelectingCoordinates = !state.isSelectingCoordinates;
    if (state.isSelectingCoordinates) {
      this.mapViewService.enableCoordinateSelection();
      if (this.mobileMapService.isMobile()) {
        this.mobileMapService.showMobileMap();
      }
    } else {
      this.mapViewService.disableCoordinateSelection();
    }
  }

  changeMapView(state: IDeficiencyDetailsState): void {
    this.mapViewService.changeFocus(
      { latitude: state.details?.latitude || 0, longitude: state.details?.longitude || 0 },
      12,
    );
    if (this.mobileMapService.isMobile()) {
      this.mobileMapService.showMobileMap();
    }
  }

  toggleMonitoring(state: IDeficiencyDetailsState, deficiencyType: EDeficiencyType): Observable<boolean> {
    return new Observable((observer) => {
      this.auditService.toggleMonitoring(state.details?.id || null, deficiencyType).subscribe({
        next: (success) => {
          if (success && state.details) {
            if (!state.details.deficiencyMonitoring) {
              state.details.deficiencyMonitoring = {
                isMonitoring: true,
              };
            } else {
              state.details.deficiencyMonitoring.isMonitoring = !state.details.deficiencyMonitoring.isMonitoring;
            }
          }
          observer.next(success);
          observer.complete();
        },
        error: (error) => {
          console.error("Error toggling monitoring:", error);
          observer.error(error);
        },
      });
    });
  }

  onSubmit(state: IDeficiencyDetailsState, deficiencyDataService: any, deficiencyType: EDeficiencyType): void {
    if (state.detailsForm.invalid) {
      state.detailsForm.markAllAsTouched();
      return;
    }

    const formData: IDeficiency = state.detailsForm.value;

    if (this.mobileMapService.isMobile()) {
      this.mobileMapService.hideMobileMap();
    }

    if (state.isAddingDeficiency) {
      const methodName = deficiencyType === EDeficiencyType.Water ? "addNewWaterDeficiency" : "addNewSoilDeficiency";
      deficiencyDataService[methodName](formData).subscribe(() => {
        this.router.navigate([`/${deficiencyType === EDeficiencyType.Water ? "water" : "soil"}`]);
      });
    } else {
      const methodName =
        deficiencyType === EDeficiencyType.Water ? "updateWaterDeficiencyById" : "updateSoilDeficiencyById";
      deficiencyDataService[methodName](formData.id, formData).subscribe(() => {
        this.router.navigate([`/${deficiencyType === EDeficiencyType.Water ? "water" : "soil"}`]);
      });
    }
  }

  onCancel(state: IDeficiencyDetailsState): void {
    this.changeMapView(state);

    if (this.mobileMapService.isMobile()) {
      this.mobileMapService.hideMobileMap();
    }

    this.router.navigate(["/"]);
  }

  showPreview(attachment: IDeficiencyAttachment): void {
    this.dialog.open(AttachmentPreviewDialogComponent, {
      width: "fit-content",
      height: "fit-content",
      maxHeight: "80vh",
      maxWidth: "80vw",
      minHeight: "300px",
      minWidth: "400px",
      data: {
        url: attachment.previewUrl,
        fileName: attachment.fileName,
      },
      panelClass: "attachment-preview-modal",
    });
  }

  onUploadStarted(state: IDeficiencyDetailsState, files: File[]): void {
    state.isUploading = true;
    state.lastUploadError = null;
    state.uploadProgress = 0;

    this.snackBar.open("Upload started...", "Close", {
      duration: 2000,
      panelClass: ["info-snackbar"],
    });
  }

  onUploadCompleted(state: IDeficiencyDetailsState, newAttachments: IDeficiencyAttachment[]): void {
    state.isUploading = false;
    state.uploadProgress = 100;

    state.attachments = [...state.attachments, ...newAttachments];

    this.snackBar.open(`Successfully uploaded ${newAttachments.length} file(s)`, "Close", {
      duration: 3000,
      panelClass: ["success-snackbar"],
    });

    if (state.details?.id) {
      this.loadAttachments(state, state.details.id, state.details.type);
    }
  }

  onUploadError(state: IDeficiencyDetailsState, error: string): void {
    console.error("Upload error:", error);
    state.isUploading = false;
    state.lastUploadError = error;
    state.uploadProgress = 0;

    this.snackBar.open(`Upload failed: ${error}`, "Close", {
      duration: 5000,
      panelClass: ["error-snackbar"],
    });
  }

  onUploadProgress(state: IDeficiencyDetailsState, progress: number): void {
    state.uploadProgress = progress;
  }

  onFilesSelected(state: IDeficiencyDetailsState, files: File[]): void {
    state.lastUploadedFiles = files;
    state.isUploading = true;
    state.lastUploadError = null;
    state.uploadProgress = 0;

    const progressSubscription = this.uploadService.uploadProgress$.subscribe((progress) => {
      state.uploadProgress = this.uploadService.getOverallProgress();
    });

    this.uploadService.uploadFilesParallel(files, state.details!.id, 3).subscribe({
      next: (results) => {
        const successfulUploads = results.filter((result) => result.attachment).map((result) => result.attachment!);
        const failedUploads = results.filter((result) => result.error).map((result) => result.error!);

        if (successfulUploads.length > 0) {
          state.attachments = [...state.attachments, ...successfulUploads];
          this.snackBar.open(`Successfully uploaded ${successfulUploads.length} file(s)`, "Close", {
            duration: 3000,
            panelClass: "success-snackbar",
          });
        }

        if (failedUploads.length > 0) {
          state.lastUploadError = failedUploads.join(", ");
          this.snackBar.open(`Failed to upload ${failedUploads.length} file(s): ${state.lastUploadError}`, "Close", {
            duration: 5000,
            panelClass: "error-snackbar",
          });
        }

        state.isUploading = false;
        state.uploadProgress = 100;
        progressSubscription.unsubscribe();
      },
      error: (error) => {
        console.error("Upload error:", error);
        state.isUploading = false;
        state.lastUploadError = error;
        state.uploadProgress = 0;
        progressSubscription.unsubscribe();
        this.snackBar.open(`Upload failed: ${error}`, "Close", { duration: 5000, panelClass: "error-snackbar" });
      },
    });
  }

  onFileSelected(state: IDeficiencyDetailsState, file: File): void {
    console.log("Single file selected:", file);
    this.onFilesSelected(state, [file]);
  }

  onFileRemoved(state: IDeficiencyDetailsState, file: File): void {
    console.log("File removed:", file);
    state.lastUploadedFiles = state.lastUploadedFiles.filter((f) => f !== file);
  }

  loadAttachments(state: IDeficiencyDetailsState, deficiencyId: string, deficiencyType: EDeficiencyType): void {
    if (!deficiencyId) {
      console.warn("No deficiency ID provided for loading attachments");
      return;
    }

    this.attachmentService.getAttachments(deficiencyId, deficiencyType).subscribe({
      next: (attachments) => {
        state.attachments = attachments || [];
      },
      error: (error) => {
        console.error("Error loading attachments:", error);
        state.attachments = [];
      },
    });
  }

  retryUpload(state: IDeficiencyDetailsState): void {
    if (state.lastUploadedFiles.length > 0) {
      state.isUploading = true;
      state.lastUploadError = null;

      const uploadObservable = this.uploadService.uploadFilesParallel(state.lastUploadedFiles, state.details!.id, 3);

      uploadObservable.subscribe({
        next: (results) => {
          const successfulUploads = results.filter((result) => result.attachment).map((result) => result.attachment!);
          const failedUploads = results.filter((result) => result.error).map((result) => result.error!);

          if (successfulUploads.length > 0) {
            state.attachments = [...state.attachments, ...successfulUploads];
            this.snackBar.open(`Successfully uploaded ${successfulUploads.length} file(s)`, "Close", {
              duration: 3000,
              panelClass: "success-snackbar",
            });
          }

          if (failedUploads.length > 0) {
            state.lastUploadError = failedUploads.join(", ");
            this.snackBar.open(`Failed to upload ${failedUploads.length} file(s): ${state.lastUploadError}`, "Close", {
              duration: 5000,
              panelClass: "error-snackbar",
            });
          }

          state.isUploading = false;
        },
        error: (error) => {
          console.error("Retry upload error:", error);
          state.isUploading = false;
          state.lastUploadError = error;
          this.snackBar.open(`Upload failed: ${error}`, "Close", { duration: 5000, panelClass: "error-snackbar" });
        },
      });
    }
  }
}
