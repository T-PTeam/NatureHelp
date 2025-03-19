import { Component } from "@angular/core";
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { AuthDialogComponent } from "../login-dialog/auth-dialog.component";

@Component({
    selector: "app-add-organization-users",
    templateUrl: "./add-organization-users.component.html",
    styleUrls: ["./add-organization-users.component.css"],
    standalone: false,
})
export class AddOrganizationUsersComponent {
    formGroup: FormGroup;
    hidePassword = true;

    get formUsers(): FormArray {
        return this.formGroup.get("users") as FormArray;
    }

    get usersCount(): FormControl {
        return this.formGroup.get("usersCount") as FormControl;
    }

    constructor(
        private dialogRef: MatDialogRef<AuthDialogComponent>,
        private fb: FormBuilder,
    ) {
        this.formGroup = this.fb.group({
            usersCount: [1, [Validators.required, Validators.min(1)]],
            users: this.fb.array([]),
        });

        this.formUsers.clear();

        this.updateUsersArray(this.usersCount.value);

        this.usersCount.valueChanges.subscribe((count) => this.updateUsersArray(count));
    }

    onSubmit(): void {
        if (this.formGroup.valid) {
            this.dialogRef.close(this.formGroup.value);
        }
    }

    closeDialog(): void {
        this.dialogRef.close();
    }

    private updateUsersArray(count: number) {
        this.formUsers.clear();

        for (let i = 0; i < count; i++) {
            this.formUsers.push(
                this.fb.group({
                    firstName: [this.generateRandomText(), [Validators.required]],
                    lastName: [this.generateRandomText(), [Validators.required]],
                    email: [this.generateRandomEmail(), [Validators.required, Validators.email]],
                    password: [this.generateRandomPassword(), [Validators.required, Validators.minLength(8)]],
                }),
            );
        }
    }

    private generateRandomText(): string {
        const chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        let username = "";
        for (let i = 0; i < 12; i++) {
            username += chars[Math.floor(Math.random() * chars.length)];
        }
        return `${username}`;
    }

    private generateRandomEmail(): string {
        const chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        let username = "";
        for (let i = 0; i < 12; i++) {
            username += chars[Math.floor(Math.random() * chars.length)];
        }
        return `${username}@example.com`;
    }

    private generateRandomPassword(): string {
        const chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        let password = "";
        for (let i = 0; i < 8; i++) {
            password += chars[Math.floor(Math.random() * chars.length)];
        }
        return password;
    }
}
