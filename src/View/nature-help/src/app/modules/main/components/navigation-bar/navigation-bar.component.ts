import { LoginDialogComponent } from '@/shared/components/dialogs/login-dialog/login-dialog.component';
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
    private userService: UserService,
    private notify: MatSnackBar
  ) { }

  ngOnInit() {
  }

  openLoginDialog(): void {
    const dialogRef = this.dialog.open(LoginDialogComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log("USER DATA: ", result)
        this.userService.login(result.email, result.password).subscribe({
          error: (err) => {
            this.notify.open("Login failed", 'Close', {duration: 2000});
            return err;
          }
        });
      }
    });
  }

}
