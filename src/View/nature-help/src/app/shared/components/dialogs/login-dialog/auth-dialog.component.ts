import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'nat-auth-dialog',
  templateUrl: './auth-dialog.component.html',
  styleUrls: ['./auth-dialog.component.css'],
  standalone: false,
})
export class AuthDialogComponent {
  formGroup: FormGroup;
  hidePassword = true;

  constructor(
    private dialogRef: MatDialogRef<AuthDialogComponent>,
    private fb: FormBuilder
  ) {
    this.formGroup = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit(): void {
    if (this.formGroup.valid) {
      this.dialogRef.close(this.formGroup.value);
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
