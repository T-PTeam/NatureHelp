import { AuthDialogComponent } from '@/shared/components/dialogs/login-dialog/auth-dialog.component';
import { UserService } from '@/shared/services/user.service';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'n-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.css'],
  standalone: false,
})
export class NavigationBarComponent implements OnInit {

  constructor(
    private dialog: MatDialog,
    public userService: UserService,
    private notify: MatSnackBar,
  ) { }

  ngOnInit() {
  }

  openAuthDialog(isRegister: boolean): void {
    const dialogRef = this.dialog.open(AuthDialogComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.userService.auth(isRegister, result.email, result.password).subscribe({
          error: (err) => {
            this.notify.open("Login failed", 'Close', {duration: 2000});
            return err;
          }
        });
      }
    });
  }

  logout() { this.userService.logout() }
}
