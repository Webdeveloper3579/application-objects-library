import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

export interface SelectValue {
    Id: number;
    FieldId: number;
    Value: string;
    CreatedDate?: string;
    ModifiedDate?: string;
}

@Component({
    selector     : 'add-selection-value-dialog',
    standalone   : true,
    imports      : [CommonModule, FormsModule],
    templateUrl  : './add-selection-value-dialog.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class AddSelectionValueDialogComponent implements OnInit
{
    showDialog = false;
    saving = false;
    successMessage: string | null = null;
    errorMessage: string | null = null;

    // Form data
    value = '';

    // Field ID (will be set when dialog opens)
    fieldId: number | null = null;
    fieldName: string = '';

    constructor(private http: HttpClient) {}

    ngOnInit(): void
    {
        // Listen for open dialog event
        window.addEventListener('openAddSelectionValueDialog', (event: any) => {
            this.fieldId = event.detail?.fieldId || null;
            this.fieldName = event.detail?.fieldName || '';
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
        this.value = '';
        this.successMessage = null;
        this.errorMessage = null;
        this.saving = false;
    }

    onSubmit(): void
    {
        if (!this.fieldId) {
            this.errorMessage = 'No field selected';
            return;
        }

        if (!this.value.trim()) {
            this.errorMessage = 'Value is required';
            return;
        }

        this.saving = true;
        this.errorMessage = null;

        const selectValueData: SelectValue = {
            Id: 0, // Will be set by backend
            FieldId: this.fieldId,
            Value: this.value.trim()
        };

        console.log('Creating new selection value:', selectValueData);

        this.http.post('/api/SelectValue', selectValueData).subscribe({
            next: (response: any) => {
                console.log('Selection value created successfully:', response);
                this.saving = false;
                this.successMessage = 'Selection value added successfully!';
                
                // Dispatch refresh event to reload select values
                window.dispatchEvent(new CustomEvent('refreshSelectValues'));
                
                // Close dialog after 2 seconds
                setTimeout(() => {
                    this.closeDialog();
                }, 2000);
            },
            error: (error) => {
                console.error('Error creating selection value:', error);
                this.saving = false;
                this.errorMessage = 'Failed to add selection value: ' + (error.message || 'Unknown error');
            }
        });
    }
}
