import { FormGroup } from "@angular/forms";
import { HttpErrorResponse } from "@angular/common/http";

export function apiFieldToFormControlName(apiField: string): string {
  if (apiField === "PH") {
    return "ph";
  }

  return apiField.charAt(0).toLowerCase() + apiField.slice(1);
}

export function applyServerValidationErrors(form: FormGroup, error: unknown): boolean {
  const apiErrors = (error as HttpErrorResponse)?.error?.errors as Record<string, string[]> | undefined;
  if (!apiErrors) {
    return false;
  }

  let applied = false;

  Object.entries(apiErrors).forEach(([key, messages]) => {
    const controlName = apiFieldToFormControlName(key);
    const control = form.get(controlName);

    if (!control || !messages?.length) {
      return;
    }

    control.setErrors({ server: messages[0] });
    control.markAsTouched();
    applied = true;
  });

  if (applied) {
    form.updateValueAndValidity({ emitEvent: false });
  }

  return applied;
}
