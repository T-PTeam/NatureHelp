import { Component, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";

import { AuthDialogComponent } from "@/shared/components/dialogs/login-dialog/auth-dialog.component";
import { UserAPIService } from "@/shared/services/user-api.service";
import { EAuthType } from "@/models/enums";

@Component({
    selector: "n-navigation-bar",
    templateUrl: "./navigation-bar.component.html",
    styleUrls: ["./navigation-bar.component.css"],
    standalone: false,
})
export class NavigationBarComponent implements OnInit {
    constructor(
        private dialog: MatDialog,
        public userService: UserAPIService,
        private notify: MatSnackBar,
    ) {}

    ngOnInit() {}

    openAuthDialog(isRegister: boolean): void {
        const dialogRef = this.dialog.open(AuthDialogComponent, {
            width: "60vw",
            maxWidth: "60vw",
            height: "300px",
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.userService
                    .auth(isRegister ? EAuthType.Register : EAuthType.Login, result.email, result.password)
                    .subscribe({
                        error: (err) => {
                            this.notify.open("Login failed", "Close", { duration: 2000 });
                            return err;
                        },
                    });
            }
        });
    }

    logout() {
        this.userService.logout();
    }
}
