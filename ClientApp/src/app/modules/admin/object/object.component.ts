import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AddFieldDialogComponent } from './add-field-dialog.component';
import { AddSelectionValueDialogComponent } from './add-selection-value-dialog.component';

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

export interface SelectValue {
    Id: number;
    FieldId: number;
    Value: string;
    CreatedDate?: string;
    ModifiedDate?: string;
}

export interface Object {
    Id: number;
    ObjectName: string;
    ObjectEnum: string;
    CreatedDate?: string;
    ModifiedDate?: string;
}

@Component({
    selector     : 'object',
    standalone   : true,
    imports      : [CommonModule, FormsModule, AddFieldDialogComponent, AddSelectionValueDialogComponent],
    templateUrl  : './object.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class ObjectComponent implements OnInit
{
    objectId: number | null = null;
    object: Object | null = null;
    fields: Field[] = [];
    selectValues: SelectValue[] = [];
    selectedField: Field | null = null;
    loading = true;
    errorMessage: string | null = null;

    // Editing states
    editingName = false;
    editingDescription = false;

    // Edited values
    editedName = '';
    editedDescription = '';

    // Track if field has been modified
    hasUnsavedChanges = false;
    successMessage: string | null = null;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private http: HttpClient
    ) {}

    ngOnInit(): void
    {
        // Get object ID from route parameters
        this.route.params.subscribe(params => {
            this.objectId = +params['id'];
            if (this.objectId) {
                this.loadObjectData();
            }
        });

        // Listen for field refresh event
        window.addEventListener('refreshFields', () => {
            if (this.objectId) {
                this.loadObjectData();
            }
        });

        // Listen for select values refresh event
        window.addEventListener('refreshSelectValues', () => {
            this.loadSelectValues();
        });
    }

    loadObjectData(): void
    {
        if (!this.objectId) return;

        this.loading = true;
        this.errorMessage = null;
        
        // Clear selected field when switching objects
        this.selectedField = null;
        this.resetEditingStates();

        // Load object details, fields, and select values in parallel
        Promise.all([
            this.http.get<Object>(`/api/Object/${this.objectId}`).toPromise(),
            this.http.get<Field[]>(`/api/Field/object/${this.objectId}`).toPromise(),
            this.http.get<SelectValue[]>('/api/SelectValue').toPromise()
        ]).then(([object, fields, selectValues]) => {
            this.object = object || null;
            this.fields = fields || [];
            this.selectValues = selectValues || [];
            this.loading = false;
        }).catch(error => {
            console.error('Error loading object data:', error);
            this.errorMessage = 'Failed to load object data';
            this.loading = false;
        });
    }

    getFieldTypeIcon(fieldType: string): string
    {
        switch (fieldType.toLowerCase()) {
            case 'text': return 'heroicons_outline:document-text';
            case 'number': return 'heroicons_outline:calculator';
            case 'email': return 'heroicons_outline:envelope';
            case 'date': return 'heroicons_outline:calendar';
            case 'select': return 'heroicons_outline:list-bullet';
            case 'textarea': return 'heroicons_outline:document-duplicate';
            default: return 'heroicons_outline:document';
        }
    }

    getFieldTypeColor(fieldType: string): string
    {
        switch (fieldType.toLowerCase()) {
            case 'text': return 'text-blue-600';
            case 'number': return 'text-green-600';
            case 'email': return 'text-purple-600';
            case 'date': return 'text-orange-600';
            case 'select': return 'text-indigo-600';
            case 'textarea': return 'text-pink-600';
            default: return 'text-gray-600';
        }
    }

    getActiveFieldsCount(): number
    {
        return this.fields.filter(f => f.FieldStatus === 'active').length;
    }

    getTextFieldsCount(): number
    {
        return this.fields.filter(f => f.FieldType === 'text').length;
    }

    getSelectFieldsCount(): number
    {
        return this.fields.filter(f => f.FieldType === 'select').length;
    }

    // Field selection and editing methods
    selectField(field: Field): void
    {
        this.selectedField = field;
        this.resetEditingStates();
    }

    addField(): void
    {
        if (this.objectId) {
            window.dispatchEvent(new CustomEvent('openAddFieldDialog', {
                detail: { objectId: this.objectId }
            }));
        }
    }

    saveChanges(): void
    {
        if (this.selectedField && this.hasUnsavedChanges) {
            // Clear previous messages
            this.successMessage = null;
            this.errorMessage = null;
            
            // Change status back to Released
            this.selectedField.FieldStatus = 'Released';
            this.hasUnsavedChanges = false;
            
            // Update field in database
            this.http.put(`/api/Field/${this.selectedField.Id}`, this.selectedField).subscribe({
                next: (response) => {
                    console.log('Field updated successfully:', response);
                    this.successMessage = 'Field updated successfully!';
                    // Clear success message after 3 seconds
                    setTimeout(() => {
                        this.successMessage = null;
                    }, 3000);
                },
                error: (error) => {
                    console.error('Error updating field:', error);
                    // Revert status change on error
                    this.selectedField!.FieldStatus = 'Draft';
                    this.hasUnsavedChanges = true;
                    this.errorMessage = 'Failed to update field. Please try again.';
                    // Clear error message after 5 seconds
                    setTimeout(() => {
                        this.errorMessage = null;
                    }, 5000);
                }
            });
        }
    }

    // Name editing
    startEditingName(): void
    {
        this.editingName = true;
        this.editedName = this.selectedField?.FieldName || '';
    }

    saveName(): void
    {
        if (this.selectedField) {
            this.selectedField.FieldName = this.editedName;
            this.selectedField.FieldStatus = 'Draft';
            this.hasUnsavedChanges = true;
            this.editingName = false;
            // TODO: Save to backend
        }
    }

    cancelEditingName(): void
    {
        this.editingName = false;
        this.editedName = '';
    }

    // Description editing
    startEditingDescription(): void
    {
        this.editingDescription = true;
        this.editedDescription = this.selectedField?.FieldDescription || '';
    }

    saveDescription(): void
    {
        if (this.selectedField) {
            this.selectedField.FieldDescription = this.editedDescription;
            this.selectedField.FieldStatus = 'Draft';
            this.hasUnsavedChanges = true;
            this.editingDescription = false;
            // TODO: Save to backend
        }
    }

    cancelEditingDescription(): void
    {
        this.editingDescription = false;
        this.editedDescription = '';
    }

    // Helper methods
    resetEditingStates(): void
    {
        this.editingName = false;
        this.editingDescription = false;
        this.editedName = '';
        this.editedDescription = '';
        this.hasUnsavedChanges = false;
        this.successMessage = null;
        this.errorMessage = null;
    }

    getStatusColor(status: string): string
    {
        switch (status.toLowerCase()) {
            case 'active': return 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200';
            case 'inactive': return 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200';
            case 'draft': return 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200';
            case 'released': return 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200';
            default: return 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200';
        }
    }

    getFieldEnumInitial(fieldEnum: string): string
    {
        if (!fieldEnum || fieldEnum.length === 0) return '?';
        return fieldEnum.charAt(0).toUpperCase();
    }

    getFieldEnumColor(fieldEnum: string): string
    {
        switch (fieldEnum.toLowerCase()) {
            case 'standard': return 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200';
            case 'custom': return 'bg-purple-100 text-purple-800 dark:bg-purple-900 dark:text-purple-200';
            default: return 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200';
        }
    }

    getSelectValuesForField(fieldId: number): SelectValue[]
    {
        return this.selectValues.filter(sv => sv.FieldId === fieldId);
    }

    loadSelectValues(): void
    {
        this.http.get<SelectValue[]>('/api/SelectValue').subscribe({
            next: (selectValues) => {
                this.selectValues = selectValues || [];
            },
            error: (error) => {
                console.error('Error loading select values:', error);
            }
        });
    }

    addSelectionValue(): void
    {
        if (this.selectedField) {
            window.dispatchEvent(new CustomEvent('openAddSelectionValueDialog', {
                detail: { 
                    fieldId: this.selectedField.Id,
                    fieldName: this.selectedField.FieldName
                }
            }));
        }
    }

    deleteSelectionValue(selectValue: SelectValue): void
    {
        if (confirm(`Are you sure you want to delete "${selectValue.Value}"?`)) {
            this.http.delete(`/api/SelectValue/${selectValue.Id}`).subscribe({
                next: () => {
                    console.log('Selection value deleted successfully');
                    this.loadSelectValues(); // Refresh the list
                },
                error: (error) => {
                    console.error('Error deleting selection value:', error);
                    alert('Failed to delete selection value');
                }
            });
        }
    }

    isSelectField(field: Field): boolean
    {
        return field.FieldType.toLowerCase() === 'select';
    }
}
