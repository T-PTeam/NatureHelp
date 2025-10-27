import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment.dev";

interface PaymentRequest {
  amount: number;
  currency: string;
  description: string;
  resultUrl?: string;
  userEmail?: string;
}

interface PaymentResponse {
  data: string;
  signature: string;
  paymentUrl: string;
}

@Component({
  selector: "app-donation",
  templateUrl: "./donation.component.html",
  styleUrls: ["./donation.component.css"],
  standalone: false,
})
export class DonationComponent implements OnInit {
  donationForm!: FormGroup;
  isLoading = false;
  paymentResponse: PaymentResponse | null = null;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
  ) {}

  ngOnInit(): void {
    this.donationForm = this.fb.group({
      amount: [100, [Validators.required, Validators.min(1)]],
      currency: ["UAH", Validators.required],
      description: ["NatureHelp donation", Validators.required],
      userEmail: ["", [Validators.email]],
    });
  }

  onSubmit(): void {
    if (this.donationForm.valid) {
      this.isLoading = true;
      const paymentRequest: PaymentRequest = this.donationForm.value;

      this.http.post<PaymentResponse>(`${environment.apiUrl}/payment/create-donation`, paymentRequest).subscribe({
        next: (response) => {
          this.paymentResponse = response;
          this.redirectToPayment(response);
        },
        error: (error) => {
          console.error("Payment creation failed:", error);
          this.isLoading = false;
        },
      });
    }
  }

  private redirectToPayment(paymentResponse: PaymentResponse): void {
    // Create a form to submit to the payment provider
    const form = document.createElement("form");
    form.method = "POST";
    form.action = paymentResponse.paymentUrl;
    form.target = "_blank";

    const dataInput = document.createElement("input");
    dataInput.type = "hidden";
    dataInput.name = "data";
    dataInput.value = paymentResponse.data;
    form.appendChild(dataInput);

    const signatureInput = document.createElement("input");
    signatureInput.type = "hidden";
    signatureInput.name = "signature";
    signatureInput.value = paymentResponse.signature;
    form.appendChild(signatureInput);

    document.body.appendChild(form);
    form.submit();
    document.body.removeChild(form);
  }
}
