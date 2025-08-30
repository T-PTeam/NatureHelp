import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import { EmailVerificationService } from "@/shared/services/email-verification.service";

@Component({
  selector: "nat-email-confirmation",
  templateUrl: "./email-confirmation.component.html",
  styleUrls: ["./email-confirmation.component.css"],
  standalone: false,
})
export class EmailConfirmationComponent implements OnInit {
  isLoading = true;
  isSuccess = false;
  isError = false;
  errorMessage = "";

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private emailVerificationService: EmailVerificationService,
    private notify: MatSnackBar,
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const token = params["token"];

      if (!token) {
        this.isError = true;
        this.errorMessage = "Invalid confirmation link. No token provided.";
        this.isLoading = false;
        return;
      }

      this.confirmEmail(token);
    });
  }

  private confirmEmail(token: string): void {
    this.emailVerificationService.confirmEmail(token).subscribe({
      next: (response) => {
        this.isSuccess = true;
        this.isLoading = false;
        this.notify.open("Email confirmed successfully!", "Close", {
          duration: 3000,
        });
      },
      error: (error) => {
        console.error("Error confirming email:", error);
        this.isError = true;

        this.notify.open("Failed to confirm email. Please try again.", "Close", { duration: 2000 });

        this.isLoading = false;
      },
    });
  }

  goToHome(): void {
    this.router.navigate(["/"]);
  }

  resendVerification(): void {
    this.router.navigate(["/"]);
  }
}
