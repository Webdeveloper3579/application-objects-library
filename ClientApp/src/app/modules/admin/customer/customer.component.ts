import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

export interface CustomField {
  CustomerFieldId: number;
  CustomerCustomFieldName: string;
  CustomerCustomType: string;
  CustomerCustomFieldLabel: string;
  CustomerCustomFieldDescription: string;
  CustomerCustomFieldType: string;
  CustomerCustomFieldStatus: string;
  CreatedDate: string;
  ModifiedDate?: string;
}

@Component({
    selector     : 'customer',
    standalone   : true,
    imports      : [CommonModule, FormsModule],
    templateUrl  : './customer.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class CustomerComponent implements OnInit
{
    selectedField: CustomField | null = null;
    customFields: CustomField[] = [];
    loading = true;
    error: string | null = null;
    
    // Editing states
    editingLabel = false;
    editingDescription = false;
    editedLabel = '';
    editedDescription = '';
    originalStatus = '';

    // Add field dialog states
    showAddFieldDialog = false;
    newField = {
        fieldName: '',
        fieldLabel: '',
        fieldDescription: '',
        fieldType: 'text'
    };

    // Field type options
    fieldTypeOptions = [
        { value: 'text', label: 'Text' },
        { value: 'textarea', label: 'Textarea' },
        { value: 'email', label: 'Email' },
        { value: 'date', label: 'Date' },
        { value: 'datetime', label: 'DateTime' },
        { value: 'url', label: 'URL' },
        { value: 'select', label: 'Select' },
        { value: 'multi-select', label: 'Multi-select' },
        { value: 'integer', label: 'Integer' }
    ];

    /**
     * Constructor
     */
    constructor(private http: HttpClient)
    {
    }

    ngOnInit(): void {
        this.loadCustomerCustomFields();
    }

    loadCustomerCustomFields(): void {
        this.http.get<CustomField[]>('/api/CustomerCustomFields').subscribe({
            next: (data) => {
                this.customFields = data;
                this.loading = false;
            },
            error: (error) => {
                console.error('Error loading customer custom fields:', error);
                this.error = 'Failed to load customer custom fields';
                this.loading = false;
            }
        });
    }

    selectField(field: CustomField): void {
        this.selectedField = field;
        // Reset editing states when selecting a new field
        this.editingLabel = false;
        this.editingDescription = false;
        this.editedLabel = field.CustomerCustomFieldLabel;
        this.editedDescription = field.CustomerCustomFieldDescription;
        // Store original status
        this.originalStatus = field.CustomerCustomFieldStatus;
    }

    startEditingLabel(): void {
        this.editingLabel = true;
        this.editedLabel = this.selectedField?.CustomerCustomFieldLabel || '';
        // Change status to draft when editing starts
        if (this.selectedField) {
            this.selectedField.CustomerCustomFieldStatus = 'draft';
        }
    }

    startEditingDescription(): void {
        this.editingDescription = true;
        this.editedDescription = this.selectedField?.CustomerCustomFieldDescription || '';
        // Change status to draft when editing starts
        if (this.selectedField) {
            this.selectedField.CustomerCustomFieldStatus = 'draft';
        }
    }

    saveLabel(): void {
        if (this.selectedField) {
            this.selectedField.CustomerCustomFieldLabel = this.editedLabel;
            this.editingLabel = false;
        }
    }

    saveDescription(): void {
        if (this.selectedField) {
            this.selectedField.CustomerCustomFieldDescription = this.editedDescription;
            this.editingDescription = false;
        }
    }

    cancelEditingLabel(): void {
        this.editedLabel = this.selectedField?.CustomerCustomFieldLabel || '';
        this.editingLabel = false;
        // Restore original status if no other fields are being edited
        if (this.selectedField && !this.editingDescription) {
            this.selectedField.CustomerCustomFieldStatus = this.originalStatus;
        }
    }

    cancelEditingDescription(): void {
        this.editedDescription = this.selectedField?.CustomerCustomFieldDescription || '';
        this.editingDescription = false;
        // Restore original status if no other fields are being edited
        if (this.selectedField && !this.editingLabel) {
            this.selectedField.CustomerCustomFieldStatus = this.originalStatus;
        }
    }

    saveChanges(): void {
        if (!this.selectedField) {
            console.error('No field selected');
            return;
        }

        // Change status to released when saving
        this.selectedField.CustomerCustomFieldStatus = 'released';

        // Create the update payload with only the fields that can be modified
        const updatePayload = {
            CustomerFieldId: this.selectedField.CustomerFieldId,
            CustomerCustomFieldName: this.selectedField.CustomerCustomFieldName,
            CustomerCustomType: this.selectedField.CustomerCustomType,
            CustomerCustomFieldLabel: this.selectedField.CustomerCustomFieldLabel,
            CustomerCustomFieldDescription: this.selectedField.CustomerCustomFieldDescription,
            CustomerCustomFieldType: this.selectedField.CustomerCustomFieldType,
            CustomerCustomFieldStatus: this.selectedField.CustomerCustomFieldStatus,
            CreatedDate: this.selectedField.CreatedDate,
            ModifiedDate: this.selectedField.ModifiedDate
        };

        console.log('Sending PUT request to:', `/api/CustomerCustomFields/${this.selectedField.CustomerFieldId}`);
        console.log('Update payload:', updatePayload);
        
        this.http.put<CustomField>(`/api/CustomerCustomFields/${this.selectedField.CustomerFieldId}`, updatePayload)
            .subscribe({
                next: (updatedField) => {
                    console.log('Field updated successfully:', updatedField);
                    
                    // Update the field in the local array
                    const index = this.customFields.findIndex(f => f.CustomerFieldId === updatedField.CustomerFieldId);
                    if (index !== -1) {
                        this.customFields[index] = updatedField;
                    }
                    
                    // Update the selected field
                    this.selectedField = updatedField;
                    
                    // Reset editing states
                    this.editingLabel = false;
                    this.editingDescription = false;
                    
                    // Show success message (you could add a toast notification here)
                    alert('Field updated successfully!');
                },
                error: (error) => {
                    console.error('Error updating field:', error);
                    alert('Failed to update field. Please try again.');
                }
            });
    }

    addCustomField(): void {
        // Reset form
        this.newField = {
            fieldName: '',
            fieldLabel: '',
            fieldDescription: '',
            fieldType: 'text'
        };
        this.showAddFieldDialog = true;
    }

    closeAddFieldDialog(): void {
        this.showAddFieldDialog = false;
        this.newField = {
            fieldName: '',
            fieldLabel: '',
            fieldDescription: '',
            fieldType: 'text'
        };
    }

    saveNewField(): void {
        if (!this.newField.fieldName || !this.newField.fieldLabel || !this.newField.fieldDescription) {
            alert('Please fill in all required fields');
            return;
        }

        // Create the new field payload
        const newFieldPayload = {
            CustomerCustomFieldName: this.newField.fieldName,
            CustomerCustomType: 'Custom',
            CustomerCustomFieldLabel: this.newField.fieldLabel,
            CustomerCustomFieldDescription: this.newField.fieldDescription,
            CustomerCustomFieldType: this.newField.fieldType,
            CustomerCustomFieldStatus: 'released',
            CreatedDate: new Date().toISOString(),
            ModifiedDate: null
        };

        console.log('Creating new field:', newFieldPayload);
        
        this.http.post<CustomField>('/api/CustomerCustomFields', newFieldPayload)
            .subscribe({
                next: (createdField) => {
                    console.log('Field created successfully:', createdField);
                    
                    // Add the new field to the local array
                    this.customFields.push(createdField);
                    
                    // Close dialog and show success message
                    this.closeAddFieldDialog();
                    alert('Field added successfully!');
                },
                error: (error) => {
                    console.error('Error creating field:', error);
                    alert('Failed to add field. Please try again.');
                }
            });
    }   

    getCustomTypeLabel(customType: string): string {
        if (!customType) {
            return '?';
        }
        
        const type = customType.toLowerCase();
        if (type === 'standard') {
            return 'S';
        } else if (type === 'custom') {
            return 'C';
        }
        
        return customType.charAt(0).toUpperCase();
    }

    getCustomTypeColor(customType: string): string {
        if (!customType) {
            return 'text-gray-600 bg-gray-100';
        }
        
        const type = customType.toLowerCase();
        if (type === 'standard') {
            return 'text-blue-600 bg-blue-100';
        } else if (type === 'custom') {
            return 'text-purple-600 bg-purple-100';
        }
        
        return 'text-gray-600 bg-gray-100';
    }

    getStatusColor(status: string): string {
        return status === 'released' ? 'text-green-600 bg-green-100' : 'text-red-600 bg-red-100';
    }

    getFieldTypeColor(type: string): string {
        if (!type) {
            return 'text-gray-600 bg-gray-100';
        }
        
        const colors: { [key: string]: string } = {
            'text': 'text-blue-600 bg-blue-100',
            'email': 'text-purple-600 bg-purple-100',
            'date': 'text-green-600 bg-green-100',
            'datetime': 'text-green-600 bg-green-100',
            'url': 'text-orange-600 bg-orange-100',
            'textarea': 'text-indigo-600 bg-indigo-100',
            'select': 'text-pink-600 bg-pink-100',
            'multi-select': 'text-teal-600 bg-teal-100',
            'integer': 'text-cyan-600 bg-cyan-100'
        };
        return colors[type.toLowerCase()] || 'text-gray-600 bg-gray-100';
    }
}
