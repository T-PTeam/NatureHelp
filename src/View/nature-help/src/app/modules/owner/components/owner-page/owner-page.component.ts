import { EAuthType } from "@/models/enums";
import { AddOrganizationUsersComponent } from "@/shared/components/dialogs/add-organization-users/add-organization-users.component";
import { UserAPIService } from "@/shared/services/user-api.service";
import { Component } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";

@Component({
  selector: "nat-owner-page",
  templateUrl: "./owner-page.component.html",
  styleUrls: ["./owner-page.component.css"],
  standalone: false,
})
export class OwnerPageComponent {
  constructor(
    private dialog: MatDialog,
    public userService: UserAPIService,
    private notify: MatSnackBar,
  ) {
    this.userService.loadOrganizationUsers(0);
  }

  addUserToOrganization(isMultiple: boolean): void {
    if (isMultiple) {
      const dialogRef = this.dialog.open(AddOrganizationUsersComponent, {
        width: "90vw",
        maxWidth: "90vw",
        height: "80vh",
        data: {
          isAddingOneUser: false,
        },
      });

      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.userService.addOrganizationUsers(EAuthType.AddMultipleToOrganization, result.users).subscribe({
            error: (err) => {
              this.notify.open("Add multiple users to Your organization failed...", "Close", {
                duration: 2000,
              });
              return err;
            },
          });
        }
      });
    } else {
      const dialogRef = this.dialog.open(AddOrganizationUsersComponent, {
        width: "50vw",
        maxWidth: "90vw",
        data: {
          isAddingOneUser: true,
        },
      });

      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.userService.addOrganizationUser(EAuthType.AddOneToOrganization, result.users[0]).subscribe({
            error: (err) => {
              this.notify.open("Add new user to Your organization failed...", "Close", {
                duration: 2000,
              });
              return err;
            },
          });
        }
      });
    }
  }
}
