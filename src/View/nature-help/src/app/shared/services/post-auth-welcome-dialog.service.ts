import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";

import { IUser } from "@/models/IUser";
import { PostAuthWelcomeDialogComponent } from "@/shared/components/dialogs/post-auth-welcome-dialog/post-auth-welcome-dialog.component";

@Injectable({
  providedIn: "root",
})
export class PostAuthWelcomeDialogService {
  constructor(private dialog: MatDialog) {}

  open(user: IUser): void {
    queueMicrotask(() => {
      this.dialog.open(PostAuthWelcomeDialogComponent, {
        width: "400px",
        maxWidth: "90vw",
        data: { user },
        panelClass: "post-auth-welcome-panel",
      });
    });
  }
}
