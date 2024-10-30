import { HttpErrorResponse } from "@angular/common/http";
import { ToastrService } from "ngx-toastr";

export function showErrors(err: any, errorsContainer: string[], toastr: ToastrService): void {
    errorsContainer.length = 0;

    if (err.status >= 500)
        toastr.error('Something went wrong, please try again later');
    else if (err instanceof HttpErrorResponse) {
        if (Object.hasOwn(err.error, 'errors')) {
            const errors = err.error.errors;

            Object.keys(errors).forEach(prop =>
                errors[prop].forEach((error: string) =>
                    errorsContainer.push(error)));
        }
        else if (Object.hasOwn(err.error, 'detail') || Object.hasOwn(err.error, 'title'))
            toastr.error(err.error.detail, err.error.title);
    }
}