import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import {
    FormsModule,
    NgForm,
    ReactiveFormsModule,
    UntypedFormBuilder,
    UntypedFormGroup,
    Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router, RouterLink } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertComponent, FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';

@Component({
    selector: 'auth-sign-up',
    templateUrl: './sign-up.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
    imports: [
        RouterLink,
        FuseAlertComponent,
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatIconModule,
        MatCheckboxModule,
        MatProgressSpinnerModule,
    ],
})
export class AuthSignUpComponent implements OnInit {
    @ViewChild('signUpNgForm') signUpNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: '',
    };
    signUpForm: UntypedFormGroup;
    showAlert: boolean = false;

    /**
     * Constructor
     */
    constructor(
        private _authService: AuthService,
        private _formBuilder: UntypedFormBuilder,
        private _router: Router
    ) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        // Create the form
        this.signUpForm = this._formBuilder.group({
            name: ['', Validators.required],
            company: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            password: ['', Validators.required],
            agreements: ['', Validators.requiredTrue],
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Sign up
     */
    signUp(): void {
        // Do nothing if the form is invalid
        if (this.signUpForm.invalid) {
            return;
        }

        // Disable the form
        this.signUpForm.disable();

        // Hide the alert
        this.showAlert = false;

        // Sign up
        console.log('Signing up with data:', this.signUpForm.value);
        this._authService.signUp(this.signUpForm.value).subscribe(
            (response) => {
                console.log('Sign-up successful:', response);
                // Navigate to the sign-in page
                this._router.navigateByUrl('/sign-in');
            },
            (error) => {
                console.error('Sign-up error:', error);
                console.error('Error details:', error.error);
                console.error('Validation errors:', error.error?.errors);
                // Re-enable the form
                this.signUpForm.enable();

                // Reset the form
                this.signUpNgForm.resetForm();

                // Get error message from validation errors or exception
                let errorMessage = 'Something went wrong, please try again.';
                if (error.error?.errors) {
                    const errors = Object.values(error.error.errors).flat();
                    errorMessage = errors.join(', ');
                } else if (typeof error.error === 'string') {
                    // If error is a string (like exception message)
                    errorMessage = error.error;
                } else if (error.error?.title) {
                    // If error has a title (like validation problem details)
                    errorMessage = error.error.title;
                } else if (error.error?.message || error.error?.Message) {
                    errorMessage = error.error.message || error.error.Message;
                }

                // Set the alert
                this.alert = {
                    type: 'error',
                    message: errorMessage,
                };

                // Show the alert
                this.showAlert = true;
            }
        );
    }
}
