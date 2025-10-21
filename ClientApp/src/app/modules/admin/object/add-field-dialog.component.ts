import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

export interface Field {
    Id: number;
    ObjectId: number;
    FieldName: string;
    FieldLabel: string;
    FieldType: string;
    FieldDescription: string;
    FieldStatus: string;
    FieldEnum: string;
    CreatedDate?: string;
    ModifiedDate?: string;
}

@Component({
    selector     : 'add-field-dialog',
    standalone   : true,
    imports      : [CommonModule, FormsModule],
    templateUrl  : './add-field-dialog.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class AddFieldDialogComponent implements OnInit
{
    showDialog = false;
    saving = false;
    successMessage: string | null = null;
    errorMessage: string | null = null;

    // Form data
    fieldName = '';
    fieldLabel = '';
    fieldDescription = '';
    fieldType = 'text';

    // Field type options
    fieldTypeOptions = [
        { value: 'text', label: 'Text' },
        { value: 'number', label: 'Number' },
        { value: 'email', label: 'Email' },
        { value: 'date', label: 'Date' },
        { value: 'select', label: 'Select' },
        { value: 'textarea', label: 'Textarea' }
    ];

    // Object ID (will be set when dialog opens)
    objectId: number | null = null;

    constructor(private http: HttpClient) {}

    ngOnInit(): void
    {
        // Listen for open dialog event
        window.addEventListener('openAddFieldDialog', (event: any) => {
            this.objectId = event.detail?.objectId || null;
            this.openDialog();
        });
    }

    openDialog(): void
    {
        this.showDialog = true;
        this.resetForm();
    }

    closeDialog(): void
    {
        this.showDialog = false;
        this.resetForm();
    }

    resetForm(): void
    {
        this.fieldName = '';
        this.fieldLabel = '';
        this.fieldDescription = '';
        this.fieldType = 'text';
        this.successMessage = null;
        this.errorMessage = null;
        this.saving = false;
    }

    onSubmit(): void
    {
        if (!this.objectId) {
            this.errorMessage = 'No object selected';
            return;
        }

        if (!this.fieldName.trim() || !this.fieldLabel.trim()) {
            this.errorMessage = 'Field name and label are required';
            return;
        }

        this.saving = true;
        this.errorMessage = null;

        const fieldData: Field = {
            Id: 0, // Will be set by backend
            ObjectId: this.objectId,
            FieldName: this.fieldName.trim(),
            FieldLabel: this.fieldLabel.trim(),
            FieldType: this.fieldType,
            FieldDescription: this.fieldDescription.trim(),
            FieldStatus: 'Released',
            FieldEnum: 'custom'
        };

        console.log('Creating new field:', fieldData);

        this.http.post('/api/Field', fieldData).subscribe({
            next: (response: any) => {
                console.log('Field created successfully:', response);
                this.saving = false;
                this.successMessage = 'Field created successfully!';
                
                // Dispatch refresh event to reload fields
                window.dispatchEvent(new CustomEvent('refreshFields'));
                
                // Close dialog after 2 seconds
                setTimeout(() => {
                    this.closeDialog();
                }, 2000);
            },
            error: (error) => {
                console.error('Error creating field:', error);
                this.saving = false;
                this.errorMessage = 'Failed to create field: ' + (error.message || 'Unknown error');
            }
        });
    }
}
