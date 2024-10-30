import { InjectionToken } from "@angular/core";

const defaultErrors: {
    [key: string]: any;
  } = {
    required: () => 'This field is required',
    minlength: ({ requiredLength, actualLength }: any) => `This field must be at least ${requiredLength} characters long.`,
    maxlength: ({requiredLength, actualLength}: any) => `This field cannot exceed ${requiredLength} characters`,
    pattern: (pattern: string) => `This field must follow the pattern of ${pattern}`,
    email: () => 'This field has to be an email'
  };
  
  export const FORM_ERRORS = new InjectionToken('FORM_ERRORS', {
    providedIn: 'root',
    factory: () => defaultErrors,
  });