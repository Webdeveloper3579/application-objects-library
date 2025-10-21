import { Component, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, FormControl, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

export interface AdditionalField {
    name: string;
    label: string;
    description: string;
    type: string;
}

@Component({
    selector     : 'add-object-dialog',
    standalone   : true,
    imports      : [CommonModule, ReactiveFormsModule, FormsModule],
    templateUrl  : './add-object-dialog.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class AddObjectDialogComponent implements OnInit, OnDestroy
{
    isOpen = false;
    objectForm: FormGroup;
    additionalFields: AdditionalField[] = [];
    saving = false;
    successMessage: string | null = null;
    errorMessage: string | null = null;

    constructor(
        private formBuilder: FormBuilder,
        private http: HttpClient
    )
    {
        this.objectForm = this.formBuilder.group({
            objectName: ['', Validators.required],
            additionalFields: this.formBuilder.array([])
        });
    }

    ngOnInit(): void
    {
        // Listen for the custom event to open dialog
        window.addEventListener('openAddObjectDialog', this.openDialog.bind(this));
    }

    ngOnDestroy(): void
    {
        // Clean up event listener
        window.removeEventListener('openAddObjectDialog', this.openDialog.bind(this));
    }

    openDialog(): void
    {
        this.isOpen = true;
        this.resetForm();
    }

    closeDialog(): void
    {
        this.isOpen = false;
        this.resetForm();
    }

    resetForm(): void
    {
        this.objectForm.reset();
        this.additionalFields = [];
        this.successMessage = null;
        this.errorMessage = null;
        this.saving = false;
        // Clear the form array
        const formArray = this.objectForm.get('additionalFields') as FormArray;
        formArray.clear();
    }

    get additionalFieldsFormArray(): FormArray {
        return this.objectForm.get('additionalFields') as FormArray;
    }

    addField(): void
    {
        const newField = this.formBuilder.group({
            name: ['', Validators.required],
            label: ['', Validators.required],
            description: [''],
            type: ['text', Validators.required]
        });
        
        this.additionalFieldsFormArray.push(newField);
        this.additionalFields.push({
            name: '',
            label: '',
            description: '',
            type: 'text'
        });
    }

    removeField(index: number): void
    {
        this.additionalFieldsFormArray.removeAt(index);
        this.additionalFields.splice(index, 1);
    }

    onSubmit(): void
    {
        if (this.objectForm.valid) {
            this.saving = true;
            this.errorMessage = null;
            
            const formValue = this.objectForm.value;
            const additionalFields = formValue.additionalFields || [];
            
            console.log('Creating new object with fields:', {
                objectName: formValue.objectName,
                additionalFields: additionalFields
            });

            // First, create the object
            this.http.post('/api/Object', {
                objectName: formValue.objectName,
                objectEnum: 'custom'
            }).subscribe({
                next: (response: any) => {
                    console.log('Object created successfully:', response);
                    console.log('Object ID from response:', response.Id);
                    console.log('Response keys:', Object.keys(response));
                    
                    // If there are additional fields, save them to the Field table
                    if (additionalFields && additionalFields.length > 0) {
                        const fieldsToSave = additionalFields.map((field: any) => ({
                            ObjectId: response.Id,
                            FieldLabel: field.label || '',
                            FieldType: field.type || 'text',
                            FieldDescription: field.description || '',
                            FieldName: field.name || '',
                            FieldStatus: 'Released',
                            FieldEnum: 'Standard'
                        }));

                        console.log('Saving fields:', fieldsToSave);

                        // Try saving the first field individually first to debug
                        const firstField = fieldsToSave[0];
                        console.log('Testing single field creation:', firstField);

                        // Save fields to database
                        this.http.post('/api/Field/multiple', fieldsToSave).subscribe({
                            next: (fieldResponse: any) => {
                                this.saving = false;
                                this.successMessage = `Object "${formValue.objectName}" and ${additionalFields.length} field(s) created successfully!`;
                                
                                // Refresh navigation by dispatching a custom event
                                window.dispatchEvent(new CustomEvent('refreshNavigation'));
                                
                                // Close dialog after 2 seconds
                                setTimeout(() => {
                                    this.closeDialog();
                                }, 2000);
                            },
                            error: (fieldError) => {
                                this.saving = false;
                                console.error('Full field error object:', fieldError);
                                console.error('Field error status:', fieldError.status);
                                console.error('Field error message:', fieldError.message);
                                console.error('Field error error:', fieldError.error);
                                
                                // Log validation errors if available
                                if (fieldError.error && fieldError.error.errors) {
                                    console.error('Validation errors:', fieldError.error.errors);
                                }
                                
                                this.errorMessage = `Object created but failed to save fields: ${fieldError.status} - ${fieldError.message}`;
                            }
                        });
                    } else {
                        // No fields to save, just show success message
                        this.saving = false;
                        this.successMessage = `Object "${formValue.objectName}" created successfully!`;
                        
                        // Refresh navigation by dispatching a custom event
                        window.dispatchEvent(new CustomEvent('refreshNavigation'));
                        
                        // Close dialog after 2 seconds
                        setTimeout(() => {
                            this.closeDialog();
                        }, 2000);
                    }
                },
                error: (error) => {
                    this.saving = false;
                    this.errorMessage = 'Failed to create object: ' + error.message;
                    console.error('Error creating object:', error);
                }
            });
        } else {
            this.errorMessage = 'Please fill in all required fields correctly.';
        }
    }
}
