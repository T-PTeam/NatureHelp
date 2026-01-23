import { HttpErrorResponse } from "@angular/common/http";

export function getErrorMessage(error: HttpErrorResponse | any): string {
  if (error instanceof HttpErrorResponse) {
    if (error.error?.message) {
      return error.error.message;
    }
    
    if (typeof error.error === "string") {
      return error.error;
    }
    
    if (error.status === 0) {
      return "Network error. Please check your connection.";
    }
    if (error.status === 401) {
      return "Invalid email or password. Please try again.";
    }
    if (error.status === 403) {
      return "Access denied. You don't have permission to perform this action.";
    }
    if (error.status === 404) {
      return "Resource not found.";
    }
    if (error.status === 413) {
      return "File too large.";
    }
    if (error.status >= 500) {
      return "Server error. Please try again later.";
    }
    
    return error.message || `Error ${error.status}: ${error.statusText}`;
  }
  
  if (error?.message) {
    return error.message;
  }
  
  return "An unexpected error occurred. Please try again.";
}

