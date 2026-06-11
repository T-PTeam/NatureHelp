import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Router } from "@angular/router";

import { IUser } from "@/models/IUser";

export interface IPostAuthWelcomeDialogData {
  user: IUser;
}

@Component({
  selector: "nat-post-auth-welcome-dialog",
  templateUrl: "./post-auth-welcome-dialog.component.html",
  styleUrls: ["./post-auth-welcome-dialog.component.css"],
  standalone: false,
})
export class PostAuthWelcomeDialogComponent {
  selectedRating = 3;
  readonly stars: readonly number[] = [1, 2, 3, 4, 5];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IPostAuthWelcomeDialogData,
    private dialogRef: MatDialogRef<PostAuthWelcomeDialogComponent>,
    private router: Router,
  ) {}

  get displayName(): string {
    const u = this.data.user;
    const first = u.firstName?.trim();
    if (first) return first;
    const last = u.lastName?.trim();
    if (last) return last;
    if (u.email) return u.email.split("@")[0] ?? "";
    return "";
  }

  get faceIcon(): string {
    const icons: Record<number, string> = {
      1: "sentiment_very_dissatisfied",
      2: "sentiment_dissatisfied",
      3: "sentiment_neutral",
      4: "sentiment_satisfied",
      5: "sentiment_very_satisfied",
    };
    return icons[this.selectedRating] ?? "sentiment_neutral";
  }

  setRating(n: number): void {
    this.selectedRating = n;
  }

  goToProfile(): void {
    const parts = this.router.url.split("/").filter((p) => p);
    const first = parts[0];
    let path = "/profile";
    if (first === "en") path = "/en/profile";
    else if (first === "uk") path = "/uk/profile";
    this.dialogRef.close();
    void this.router.navigateByUrl(path);
  }
}
